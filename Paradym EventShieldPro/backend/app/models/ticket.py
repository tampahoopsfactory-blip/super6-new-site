from datetime import datetime, timedelta
from sqlalchemy import Column, Integer, String, Boolean, DateTime, Text, ForeignKey, JSON, Numeric, Enum, Index
from sqlalchemy.orm import relationship, validates
from sqlalchemy.ext.hybrid import hybrid_property
from app import db
import enum
import uuid

class TicketStatus(enum.Enum):
    RESERVED = 'reserved'
    PAID = 'paid'
    VALID = 'valid'
    USED = 'used'
    EXPIRED = 'expired'
    CANCELLED = 'cancelled'
    TRANSFERRED = 'transferred'
    REFUNDED = 'refunded'

class TicketType(enum.Enum):
    GENERAL = 'general'
    VIP = 'vip'
    EARLY_BIRD = 'early_bird'
    STUDENT = 'student'
    SENIOR = 'senior'
    CHILD = 'child'
    COMPLIMENTARY = 'complimentary'
    CORPORATE = 'corporate'

class Ticket(db.Model):
    __tablename__ = 'tickets'
    
    id = Column(Integer, primary_key=True)
    ticket_number = Column(String(50), unique=True, nullable=False, index=True)
    event_id = Column(Integer, ForeignKey('events.id'), nullable=False, index=True)
    ticket_type_id = Column(Integer, ForeignKey('ticket_types.id'), nullable=False)
    purchaser_id = Column(Integer, ForeignKey('users.id'), nullable=False, index=True)
    attendee_id = Column(Integer, ForeignKey('users.id'), nullable=True, index=True)
    status = Column(Enum(TicketStatus), default=TicketStatus.RESERVED, nullable=False, index=True)
    
    # Pricing
    original_price = Column(Numeric(10, 2), nullable=False)
    final_price = Column(Numeric(10, 2), nullable=False)
    discount_amount = Column(Numeric(10, 2), default=0.00)
    tax_amount = Column(Numeric(10, 2), default=0.00)
    currency = Column(String(3), default='USD', nullable=False)
    
    # Timing
    reserved_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    paid_at = Column(DateTime, nullable=True)
    expires_at = Column(DateTime, nullable=True)
    used_at = Column(DateTime, nullable=True)
    cancelled_at = Column(DateTime, nullable=True)
    
    # Access control
    qr_code = Column(String(255), unique=True, nullable=False, index=True)
    access_code = Column(String(20), unique=True, nullable=True)
    check_in_location = Column(String(255), nullable=True)
    check_in_time = Column(DateTime, nullable=True)
    check_out_time = Column(DateTime, nullable=True)
    
    # Metadata
    notes = Column(Text, nullable=True)
    custom_fields = Column(JSON, nullable=True)
    metadata = Column(JSON, nullable=True)
    
    # Relationships
    event = relationship('Event', back_populates='tickets')
    ticket_type = relationship('TicketType', back_populates='tickets')
    purchaser = relationship('User', foreign_keys=[purchaser_id], back_populates='purchased_tickets')
    attendee = relationship('User', foreign_keys=[attendee_id], back_populates='attending_tickets')
    access_logs = relationship('AccessLog', back_populates='ticket')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_tickets_event_status', 'event_id', 'status'),
        Index('idx_tickets_purchaser_status', 'purchaser_id', 'status'),
        Index('idx_tickets_qr_code', 'qr_code'),
        Index('idx_tickets_access_code', 'access_code'),
    )
    
    def __init__(self, **kwargs):
        super(Ticket, self).__init__(**kwargs)
        if not self.ticket_number:
            self.ticket_number = self.generate_ticket_number()
        if not self.qr_code:
            self.qr_code = self.generate_qr_code()
        if not self.access_code:
            self.access_code = self.generate_access_code()
    
    def generate_ticket_number(self):
        """Generate unique ticket number"""
        return f"TKT-{uuid.uuid4().hex[:8].upper()}"
    
    def generate_qr_code(self):
        """Generate unique QR code identifier"""
        return f"QR-{uuid.uuid4().hex[:16].upper()}"
    
    def generate_access_code(self):
        """Generate unique access code"""
        import random
        import string
        return ''.join(random.choices(string.ascii_uppercase + string.digits, k=8))
    
    @hybrid_property
    def is_valid(self):
        """Check if ticket is valid for entry"""
        if self.status != TicketStatus.VALID:
            return False
        if self.expires_at and datetime.utcnow() > self.expires_at:
            return False
        return True
    
    @hybrid_property
    def is_used(self):
        """Check if ticket has been used"""
        return self.status == TicketStatus.USED
    
    @hybrid_property
    def is_expired(self):
        """Check if ticket has expired"""
        return self.expires_at and datetime.utcnow() > self.expires_at
    
    @hybrid_property
    def can_transfer(self):
        """Check if ticket can be transferred"""
        return self.status in [TicketStatus.PAID, TicketStatus.VALID] and not self.is_used
    
    @hybrid_property
    def can_refund(self):
        """Check if ticket can be refunded"""
        return self.status in [TicketStatus.PAID, TicketStatus.VALID] and not self.is_used
    
    def mark_as_paid(self):
        """Mark ticket as paid"""
        self.status = TicketStatus.PAID
        self.paid_at = datetime.utcnow()
        if self.expires_at is None:
            # Set expiration to 24 hours after payment
            self.expires_at = datetime.utcnow() + timedelta(hours=24)
    
    def mark_as_valid(self):
        """Mark ticket as valid for entry"""
        self.status = TicketStatus.VALID
    
    def mark_as_used(self, location=None):
        """Mark ticket as used"""
        self.status = TicketStatus.USED
        self.used_at = datetime.utcnow()
        self.check_in_location = location
        self.check_in_time = datetime.utcnow()
    
    def mark_as_cancelled(self):
        """Mark ticket as cancelled"""
        self.status = TicketStatus.CANCELLED
        self.cancelled_at = datetime.utcnow()
    
    def transfer_to_user(self, new_user_id):
        """Transfer ticket to another user"""
        if not self.can_transfer:
            raise ValueError("Ticket cannot be transferred")
        self.attendee_id = new_user_id
        self.status = TicketStatus.TRANSFERRED
    
    def calculate_final_price(self):
        """Calculate final price with discounts and taxes"""
        self.final_price = self.original_price - self.discount_amount + self.tax_amount
        return self.final_price
    
    def __repr__(self):
        return f'<Ticket {self.ticket_number} - {self.status.value}>'

class TicketType(db.Model):
    __tablename__ = 'ticket_types'
    
    id = Column(Integer, primary_key=True)
    event_id = Column(Integer, ForeignKey('events.id'), nullable=False, index=True)
    name = Column(String(100), nullable=False)
    description = Column(Text, nullable=True)
    type = Column(Enum(TicketType), nullable=False)
    
    # Pricing
    price = Column(Numeric(10, 2), nullable=False)
    currency = Column(String(3), default='USD', nullable=False)
    early_bird_price = Column(Numeric(10, 2), nullable=True)
    early_bird_end_date = Column(DateTime, nullable=True)
    
    # Availability
    max_quantity = Column(Integer, nullable=True)
    current_quantity = Column(Integer, default=0, nullable=False)
    min_per_order = Column(Integer, default=1, nullable=False)
    max_per_order = Column(Integer, nullable=True)
    
    # Features
    includes_meal = Column(Boolean, default=False)
    includes_drinks = Column(Boolean, default=False)
    includes_swag = Column(Boolean, default=False)
    vip_access = Column(Boolean, default=False)
    priority_seating = Column(Boolean, default=False)
    
    # Timing
    sale_start_date = Column(DateTime, nullable=True)
    sale_end_date = Column(DateTime, nullable=True)
    
    # Status
    is_active = Column(Boolean, default=True, nullable=False)
    
    # Relationships
    event = relationship('Event', back_populates='ticket_types')
    tickets = relationship('Ticket', back_populates='ticket_type')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_ticket_types_event_active', 'event_id', 'is_active'),
        Index('idx_ticket_types_sale_dates', 'sale_start_date', 'sale_end_date'),
    )
    
    @hybrid_property
    def is_available(self):
        """Check if ticket type is available for purchase"""
        if not self.is_active:
            return False
        if self.max_quantity and self.current_quantity >= self.max_quantity:
            return False
        now = datetime.utcnow()
        if self.sale_start_date and now < self.sale_start_date:
            return False
        if self.sale_end_date and now > self.sale_end_date:
            return False
        return True
    
    @hybrid_property
    def is_early_bird(self):
        """Check if early bird pricing is active"""
        if not self.early_bird_price or not self.early_bird_end_date:
            return False
        return datetime.utcnow() <= self.early_bird_end_date
    
    @hybrid_property
    def current_price(self):
        """Get current price (early bird or regular)"""
        if self.is_early_bird:
            return self.early_bird_price
        return self.price
    
    @hybrid_property
    def available_quantity(self):
        """Get available quantity for purchase"""
        if self.max_quantity is None:
            return None
        return max(0, self.max_quantity - self.current_quantity)
    
    def increment_quantity(self, amount=1):
        """Increment current quantity"""
        self.current_quantity += amount
    
    def decrement_quantity(self, amount=1):
        """Decrement current quantity"""
        self.current_quantity = max(0, self.current_quantity - amount)
    
    def __repr__(self):
        return f'<TicketType {self.name} - {self.type.value}>'

class TicketPurchase(db.Model):
    __tablename__ = 'ticket_purchases'
    
    id = Column(Integer, primary_key=True)
    purchase_number = Column(String(50), unique=True, nullable=False, index=True)
    purchaser_id = Column(Integer, ForeignKey('users.id'), nullable=False, index=True)
    event_id = Column(Integer, ForeignKey('events.id'), nullable=False, index=True)
    
    # Payment
    stripe_payment_intent_id = Column(String(255), nullable=True, index=True)
    stripe_charge_id = Column(String(255), nullable=True)
    payment_status = Column(String(50), default='pending', nullable=False, index=True)
    payment_method = Column(String(50), nullable=True)
    
    # Pricing
    subtotal = Column(Numeric(10, 2), nullable=False)
    tax_amount = Column(Numeric(10, 2), default=0.00)
    discount_amount = Column(Numeric(10, 2), default=0.00)
    total_amount = Column(Numeric(10, 2), nullable=False)
    currency = Column(String(3), default='USD', nullable=False)
    
    # Purchase details
    quantity = Column(Integer, nullable=False)
    purchase_date = Column(DateTime, default=datetime.utcnow, nullable=False)
    
    # Billing
    billing_address = Column(JSON, nullable=True)
    billing_email = Column(String(255), nullable=False)
    
    # Status
    status = Column(String(50), default='pending', nullable=False, index=True)
    
    # Relationships
    purchaser = relationship('User', back_populates='ticket_purchases')
    event = relationship('Event', back_populates='ticket_purchases')
    tickets = relationship('Ticket', backref='purchase')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_ticket_purchases_purchaser_status', 'purchaser_id', 'status'),
        Index('idx_ticket_purchases_event_status', 'event_id', 'status'),
        Index('idx_ticket_purchases_payment_status', 'payment_status'),
    )
    
    def __init__(self, **kwargs):
        super(TicketPurchase, self).__init__(**kwargs)
        if not self.purchase_number:
            self.purchase_number = self.generate_purchase_number()
    
    def generate_purchase_number(self):
        """Generate unique purchase number"""
        return f"PUR-{uuid.uuid4().hex[:8].upper()}"
    
    @hybrid_property
    def is_paid(self):
        """Check if purchase is fully paid"""
        return self.payment_status == 'succeeded'
    
    @hybrid_property
    def is_pending(self):
        """Check if purchase is pending payment"""
        return self.payment_status == 'pending'
    
    @hybrid_property
    def is_failed(self):
        """Check if purchase payment failed"""
        return self.payment_status == 'failed'
    
    def mark_as_paid(self, stripe_charge_id=None):
        """Mark purchase as paid"""
        self.payment_status = 'succeeded'
        self.status = 'completed'
        if stripe_charge_id:
            self.stripe_charge_id = stripe_charge_id
    
    def mark_as_failed(self):
        """Mark purchase as failed"""
        self.payment_status = 'failed'
        self.status = 'cancelled'
    
    def calculate_total(self):
        """Calculate total amount"""
        self.total_amount = self.subtotal + self.tax_amount - self.discount_amount
        return self.total_amount
    
    def __repr__(self):
        return f'<TicketPurchase {self.purchase_number} - {self.status}>'

class TicketValidation(db.Model):
    __tablename__ = 'ticket_validations'
    
    id = Column(Integer, primary_key=True)
    ticket_id = Column(Integer, ForeignKey('tickets.id'), nullable=False, index=True)
    validator_id = Column(Integer, ForeignKey('users.id'), nullable=False, index=True)
    
    # Validation details
    validation_method = Column(String(50), nullable=False)  # qr_scan, facial_recognition, manual
    validation_location = Column(String(255), nullable=True)
    validation_device = Column(String(100), nullable=True)
    
    # Result
    is_valid = Column(Boolean, nullable=False)
    validation_notes = Column(Text, nullable=True)
    
    # Timing
    validated_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    
    # Relationships
    ticket = relationship('Ticket', back_populates='validations')
    validator = relationship('User', back_populates='ticket_validations')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_ticket_validations_ticket', 'ticket_id'),
        Index('idx_ticket_validations_validator', 'validator_id'),
        Index('idx_ticket_validations_method', 'validation_method'),
    )
    
    def __repr__(self):
        return f'<TicketValidation {self.ticket_id} - {self.is_valid}>'

