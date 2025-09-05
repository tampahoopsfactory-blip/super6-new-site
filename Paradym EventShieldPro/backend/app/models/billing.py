from datetime import datetime, timedelta
from sqlalchemy import Column, Integer, String, Boolean, DateTime, Text, ForeignKey, JSON, Enum, Index, Numeric
from sqlalchemy.orm import relationship, validates
from sqlalchemy.ext.hybrid import hybrid_property
from app import db
import enum
import uuid

class BillingCycle(enum.Enum):
    MONTHLY = 'monthly'
    QUARTERLY = 'quarterly'
    YEARLY = 'yearly'
    CUSTOM = 'custom'

class PaymentStatus(enum.Enum):
    PENDING = 'pending'
    PROCESSING = 'processing'
    SUCCEEDED = 'succeeded'
    FAILED = 'failed'
    CANCELLED = 'cancelled'
    REFUNDED = 'refunded'
    PARTIALLY_REFUNDED = 'partially_refunded'

class InvoiceStatus(enum.Enum):
    DRAFT = 'draft'
    OPEN = 'open'
    PAID = 'paid'
    VOID = 'void'
    UNCOLLECTIBLE = 'uncollectible'

class SubscriptionStatus(enum.Enum):
    ACTIVE = 'active'
    PAST_DUE = 'past_due'
    UNPAID = 'unpaid'
    CANCELLED = 'cancelled'
    TRIAL = 'trial'
    PAUSED = 'paused'

class BillingAccount(db.Model):
    __tablename__ = 'billing_accounts'
    
    id = Column(Integer, primary_key=True)
    tenant_id = Column(Integer, ForeignKey('tenants.id'), nullable=False, unique=True, index=True)
    
    # Stripe integration
    stripe_customer_id = Column(String(255), unique=True, nullable=True, index=True)
    stripe_default_payment_method = Column(String(255), nullable=True)
    
    # Billing information
    company_name = Column(String(255), nullable=True)
    billing_email = Column(String(255), nullable=False)
    billing_phone = Column(String(50), nullable=True)
    
    # Address
    billing_address = Column(JSON, nullable=True)  # street, city, state, postal_code, country
    
    # Tax information
    tax_id = Column(String(100), nullable=True)
    tax_exempt = Column(Boolean, default=False)
    tax_exemption_code = Column(String(100), nullable=True)
    
    # Payment preferences
    auto_pay_enabled = Column(Boolean, default=True)
    payment_terms_days = Column(Integer, default=30)
    currency = Column(String(3), default='USD', nullable=False)
    
    # Status
    is_active = Column(Boolean, default=True, nullable=False)
    credit_limit = Column(Numeric(10, 2), nullable=True)
    current_balance = Column(Numeric(10, 2), default=0.00, nullable=False)
    
    # Relationships
    tenant = relationship('Tenant', back_populates='billing_account')
    subscriptions = relationship('Subscription', back_populates='billing_account')
    invoices = relationship('Invoice', back_populates='billing_account')
    payments = relationship('Payment', back_populates='billing_account')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_billing_accounts_stripe', 'stripe_customer_id'),
        Index('idx_billing_accounts_active', 'is_active'),
    )
    
    @hybrid_property
    def has_outstanding_balance(self):
        """Check if account has outstanding balance"""
        return self.current_balance > 0
    
    @hybrid_property
    def is_over_credit_limit(self):
        """Check if account is over credit limit"""
        if self.credit_limit:
            return self.current_balance > self.credit_limit
        return False
    
    def add_charge(self, amount):
        """Add a charge to the current balance"""
        self.current_balance += amount
    
    def add_payment(self, amount):
        """Add a payment to reduce the current balance"""
        self.current_balance = max(0, self.current_balance - amount)
    
    def __repr__(self):
        return f'<BillingAccount {self.tenant_id} - {self.billing_email}>'

class Subscription(db.Model):
    __tablename__ = 'subscriptions'
    
    id = Column(Integer, primary_key=True)
    subscription_number = Column(String(50), unique=True, nullable=False, index=True)
    billing_account_id = Column(Integer, ForeignKey('billing_accounts.id'), nullable=False, index=True)
    license_id = Column(Integer, ForeignKey('licenses.id'), nullable=False, index=True)
    
    # Stripe integration
    stripe_subscription_id = Column(String(255), unique=True, nullable=True, index=True)
    stripe_price_id = Column(String(255), nullable=True)
    
    # Plan details
    plan_name = Column(String(100), nullable=False)
    plan_tier = Column(String(50), nullable=False)
    billing_cycle = Column(Enum(BillingCycle), nullable=False)
    
    # Pricing
    unit_price = Column(Numeric(10, 2), nullable=False)
    quantity = Column(Integer, default=1, nullable=False)
    discount_percent = Column(Numeric(5, 2), default=0.00)
    discount_amount = Column(Numeric(10, 2), default=0.00)
    tax_rate = Column(Numeric(5, 4), default=0.00)
    
    # Calculated amounts
    subtotal = Column(Numeric(10, 2), nullable=False)
    tax_amount = Column(Numeric(10, 2), default=0.00)
    total_amount = Column(Numeric(10, 2), nullable=False)
    
    # Billing cycle
    current_period_start = Column(DateTime, nullable=False)
    current_period_end = Column(DateTime, nullable=False)
    next_billing_date = Column(DateTime, nullable=False)
    
    # Status
    status = Column(Enum(SubscriptionStatus), default=SubscriptionStatus.ACTIVE, nullable=False, index=True)
    
    # Trial information
    trial_start = Column(DateTime, nullable=True)
    trial_end = Column(DateTime, nullable=True)
    
    # Cancellation
    cancelled_at = Column(DateTime, nullable=True)
    cancellation_reason = Column(Text, nullable=True)
    end_date = Column(DateTime, nullable=True)
    
    # Metadata
    metadata = Column(JSON, nullable=True)
    
    # Relationships
    billing_account = relationship('BillingAccount', back_populates='subscriptions')
    license = relationship('License', back_populates='subscription')
    invoices = relationship('Invoice', back_populates='subscription')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_subscriptions_status', 'status'),
        Index('idx_subscriptions_billing_date', 'next_billing_date'),
        Index('idx_subscriptions_period', 'current_period_start', 'current_period_end'),
    )
    
    def __init__(self, **kwargs):
        super(Subscription, self).__init__(**kwargs)
        if not self.subscription_number:
            self.subscription_number = self.generate_subscription_number()
    
    def generate_subscription_number(self):
        """Generate unique subscription number"""
        return f"SUB-{uuid.uuid4().hex[:8].upper()}"
    
    @hybrid_property
    def is_active(self):
        """Check if subscription is active"""
        return self.status == SubscriptionStatus.ACTIVE
    
    @hybrid_property
    def is_trial(self):
        """Check if subscription is in trial period"""
        return self.status == SubscriptionStatus.TRIAL
    
    @hybrid_property
    def is_cancelled(self):
        """Check if subscription is cancelled"""
        return self.status == SubscriptionStatus.CANCELLED
    
    @hybrid_property
    def is_past_due(self):
        """Check if subscription is past due"""
        return self.status == SubscriptionStatus.PAST_DUE
    
    @hybrid_property
    def days_until_next_billing(self):
        """Get days until next billing"""
        delta = self.next_billing_date - datetime.utcnow()
        return max(0, delta.days)
    
    @hybrid_property
    def days_until_trial_end(self):
        """Get days until trial ends"""
        if self.trial_end:
            delta = self.trial_end - datetime.utcnow()
            return max(0, delta.days)
        return None
    
    def calculate_amounts(self):
        """Calculate subtotal, tax, and total amounts"""
        self.subtotal = (self.unit_price * self.quantity) - self.discount_amount
        self.tax_amount = self.subtotal * self.tax_rate
        self.total_amount = self.subtotal + self.tax_amount
        return self.total_amount
    
    def start_trial(self, trial_days=14):
        """Start trial period"""
        self.status = SubscriptionStatus.TRIAL
        self.trial_start = datetime.utcnow()
        self.trial_end = datetime.utcnow() + timedelta(days=trial_days)
        self.next_billing_date = self.trial_end
    
    def activate(self):
        """Activate subscription after trial"""
        self.status = SubscriptionStatus.ACTIVE
        self.trial_end = None
        self.next_billing_date = datetime.utcnow() + self.get_billing_interval()
    
    def cancel(self, reason=None, end_date=None):
        """Cancel subscription"""
        self.status = SubscriptionStatus.CANCELLED
        self.cancelled_at = datetime.utcnow()
        self.cancellation_reason = reason
        if end_date:
            self.end_date = end_date
        else:
            self.end_date = self.current_period_end
    
    def get_billing_interval(self):
        """Get billing interval as timedelta"""
        if self.billing_cycle == BillingCycle.MONTHLY:
            return timedelta(days=30)
        elif self.billing_cycle == BillingCycle.QUARTERLY:
            return timedelta(days=90)
        elif self.billing_cycle == BillingCycle.YEARLY:
            return timedelta(days=365)
        else:
            return timedelta(days=30)  # Default to monthly
    
    def advance_billing_period(self):
        """Advance to next billing period"""
        self.current_period_start = self.current_period_end
        self.current_period_end = self.current_period_start + self.get_billing_interval()
        self.next_billing_date = self.current_period_end
    
    def __repr__(self):
        return f'<Subscription {self.subscription_number} - {self.plan_name}>'

class Invoice(db.Model):
    __tablename__ = 'invoices'
    
    id = Column(Integer, primary_key=True)
    invoice_number = Column(String(50), unique=True, nullable=False, index=True)
    billing_account_id = Column(Integer, ForeignKey('billing_accounts.id'), nullable=False, index=True)
    subscription_id = Column(Integer, ForeignKey('subscriptions.id'), nullable=True, index=True)
    
    # Stripe integration
    stripe_invoice_id = Column(String(255), unique=True, nullable=True, index=True)
    
    # Invoice details
    status = Column(Enum(InvoiceStatus), default=InvoiceStatus.DRAFT, nullable=False, index=True)
    invoice_type = Column(String(50), nullable=False)  # subscription, one_time, credit, refund
    
    # Billing information
    billing_date = Column(DateTime, default=datetime.utcnow, nullable=False)
    due_date = Column(DateTime, nullable=False)
    paid_date = Column(DateTime, nullable=True)
    
    # Amounts
    subtotal = Column(Numeric(10, 2), nullable=False)
    tax_amount = Column(Numeric(10, 2), default=0.00)
    discount_amount = Column(Numeric(10, 2), default=0.00)
    total_amount = Column(Numeric(10, 2), nullable=False)
    amount_paid = Column(Numeric(10, 2), default=0.00, nullable=False)
    amount_due = Column(Numeric(10, 2), nullable=False)
    
    # Currency
    currency = Column(String(3), default='USD', nullable=False)
    
    # Line items
    line_items = Column(JSON, nullable=True)  # Array of line items with description, quantity, unit_price, amount
    
    # Notes
    notes = Column(Text, nullable=True)
    terms = Column(Text, nullable=True)
    
    # Relationships
    billing_account = relationship('BillingAccount', back_populates='invoices')
    subscription = relationship('Subscription', back_populates='invoices')
    payments = relationship('Payment', back_populates='invoice')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_invoices_status', 'status'),
        Index('idx_invoices_due_date', 'due_date'),
        Index('idx_invoices_billing_date', 'billing_date'),
        Index('idx_invoices_subscription', 'subscription_id'),
    )
    
    def __init__(self, **kwargs):
        super(Invoice, self).__init__(**kwargs)
        if not self.invoice_number:
            self.invoice_number = self.generate_invoice_number()
        if not self.due_date:
            self.due_date = self.billing_date + timedelta(days=30)
    
    def generate_invoice_number(self):
        """Generate unique invoice number"""
        return f"INV-{uuid.uuid4().hex[:8].upper()}"
    
    @hybrid_property
    def is_paid(self):
        """Check if invoice is fully paid"""
        return self.status == InvoiceStatus.PAID
    
    @hybrid_property
    def is_overdue(self):
        """Check if invoice is overdue"""
        if self.status == InvoiceStatus.OPEN:
            return datetime.utcnow() > self.due_date
        return False
    
    @hybrid_property
    def days_overdue(self):
        """Get days overdue"""
        if self.is_overdue:
            delta = datetime.utcnow() - self.due_date
            return delta.days
        return 0
    
    @hybrid_property
    def days_until_due(self):
        """Get days until due"""
        if self.status == InvoiceStatus.OPEN:
            delta = self.due_date - datetime.utcnow()
            return max(0, delta.days)
        return None
    
    def calculate_amounts(self):
        """Calculate invoice amounts"""
        self.amount_due = self.total_amount - self.amount_paid
        return self.amount_due
    
    def add_payment(self, amount):
        """Add payment to invoice"""
        self.amount_paid += amount
        if self.amount_paid >= self.total_amount:
            self.status = InvoiceStatus.PAID
            self.paid_date = datetime.utcnow()
        self.calculate_amounts()
    
    def mark_as_paid(self, paid_date=None):
        """Mark invoice as paid"""
        self.status = InvoiceStatus.PAID
        self.amount_paid = self.total_amount
        self.amount_due = 0
        self.paid_date = paid_date or datetime.utcnow()
    
    def void(self, reason=None):
        """Void the invoice"""
        self.status = InvoiceStatus.VOID
        if reason:
            self.notes = f"Voided: {reason}"
    
    def __repr__(self):
        return f'<Invoice {self.invoice_number} - {self.status.value}>'

class Payment(db.Model):
    __tablename__ = 'payments'
    
    id = Column(Integer, primary_key=True)
    payment_number = Column(String(50), unique=True, nullable=False, index=True)
    billing_account_id = Column(Integer, ForeignKey('billing_accounts.id'), nullable=False, index=True)
    invoice_id = Column(Integer, ForeignKey('invoices.id'), nullable=True, index=True)
    
    # Stripe integration
    stripe_payment_intent_id = Column(String(255), unique=True, nullable=True, index=True)
    stripe_charge_id = Column(String(255), nullable=True)
    
    # Payment details
    amount = Column(Numeric(10, 2), nullable=False)
    currency = Column(String(3), default='USD', nullable=False)
    payment_method = Column(String(50), nullable=False)  # credit_card, bank_transfer, etc.
    
    # Status
    status = Column(Enum(PaymentStatus), default=PaymentStatus.PENDING, nullable=False, index=True)
    
    # Payment method details
    payment_method_details = Column(JSON, nullable=True)  # card last4, bank account, etc.
    
    # Processing
    processing_fee = Column(Numeric(10, 2), default=0.00)
    net_amount = Column(Numeric(10, 2), nullable=False)
    
    # Timing
    processed_at = Column(DateTime, nullable=True)
    failed_at = Column(DateTime, nullable=True)
    
    # Error information
    error_code = Column(String(100), nullable=True)
    error_message = Column(Text, nullable=True)
    
    # Metadata
    metadata = Column(JSON, nullable=True)
    
    # Relationships
    billing_account = relationship('BillingAccount', back_populates='payments')
    invoice = relationship('Invoice', back_populates='payments')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_payments_status', 'status'),
        Index('idx_payments_method', 'payment_method'),
        Index('idx_payments_processed', 'processed_at'),
    )
    
    def __init__(self, **kwargs):
        super(Payment, self).__init__(**kwargs)
        if not self.payment_number:
            self.payment_number = self.generate_payment_number()
        if not self.net_amount:
            self.net_amount = self.amount - self.processing_fee
    
    def generate_payment_number(self):
        """Generate unique payment number"""
        return f"PAY-{uuid.uuid4().hex[:8].upper()}"
    
    @hybrid_property
    def is_successful(self):
        """Check if payment was successful"""
        return self.status == PaymentStatus.SUCCEEDED
    
    @hybrid_property
    def is_failed(self):
        """Check if payment failed"""
        return self.status == PaymentStatus.FAILED
    
    @hybrid_property
    def is_pending(self):
        """Check if payment is pending"""
        return self.status == PaymentStatus.PENDING
    
    @hybrid_property
    def is_refunded(self):
        """Check if payment was refunded"""
        return self.status in [PaymentStatus.REFUNDED, PaymentStatus.PARTIALLY_REFUNDED]
    
    def mark_as_succeeded(self, processed_at=None):
        """Mark payment as succeeded"""
        self.status = PaymentStatus.SUCCEEDED
        self.processed_at = processed_at or datetime.utcnow()
        
        # Update billing account balance
        if self.billing_account:
            self.billing_account.add_payment(self.amount)
        
        # Update invoice if applicable
        if self.invoice:
            self.invoice.add_payment(self.amount)
    
    def mark_as_failed(self, error_code=None, error_message=None):
        """Mark payment as failed"""
        self.status = PaymentStatus.FAILED
        self.failed_at = datetime.utcnow()
        self.error_code = error_code
        self.error_message = error_message
    
    def refund(self, amount=None, reason=None):
        """Refund the payment"""
        refund_amount = amount or self.amount
        if refund_amount >= self.amount:
            self.status = PaymentStatus.REFUNDED
        else:
            self.status = PaymentStatus.PARTIALLY_REFUNDED
        
        if reason:
            self.metadata = self.metadata or {}
            self.metadata['refund_reason'] = reason
    
    def __repr__(self):
        return f'<Payment {self.payment_number} - {self.amount} {self.currency}>'

