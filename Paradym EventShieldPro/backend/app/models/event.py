"""
EventShield Pro - Event Management Models
Event creation, management, and scheduling
"""

from datetime import datetime, timedelta
from sqlalchemy import Column, Integer, String, Boolean, DateTime, Text, ForeignKey, JSON, Numeric, Enum
from sqlalchemy.orm import relationship, validates
from sqlalchemy.ext.hybrid import hybrid_property
from app import db
import enum

class EventStatus(enum.Enum):
    """Event status enumeration"""
    DRAFT = 'draft'
    PUBLISHED = 'published'
    ACTIVE = 'active'
    CANCELLED = 'cancelled'
    COMPLETED = 'completed'
    ARCHIVED = 'archived'

class EventVisibility(enum.Enum):
    """Event visibility enumeration"""
    PUBLIC = 'public'
    PRIVATE = 'private'
    INVITE_ONLY = 'invite_only'
    TENANT_ONLY = 'tenant_only'

class Event(db.Model):
    """Event model for event management"""
    
    __tablename__ = 'events'
    
    id = Column(Integer, primary_key=True)
    title = Column(String(255), nullable=False)
    slug = Column(String(255), unique=True, nullable=False, index=True)
    description = Column(Text)
    short_description = Column(String(500))
    
    # Event details
    event_type = Column(String(100))  # conference, concert, workshop, etc.
    category_id = Column(Integer, ForeignKey('event_categories.id'))
    location_id = Column(Integer, ForeignKey('event_locations.id'))
    
    # Status and visibility
    status = Column(Enum(EventStatus), default=EventStatus.DRAFT, nullable=False)
    visibility = Column(Enum(EventVisibility), default=EventVisibility.PRIVATE, nullable=False)
    is_featured = Column(Boolean, default=False)
    is_approved = Column(Boolean, default=False)
    
    # Capacity and pricing
    max_capacity = Column(Integer)
    current_registrations = Column(Integer, default=0)
    is_free = Column(Boolean, default=True)
    price = Column(Numeric(10, 2))  # Price in cents
    currency = Column(String(3), default='USD')
    
    # Registration settings
    registration_opens_at = Column(DateTime)
    registration_closes_at = Column(DateTime)
    allow_waitlist = Column(Boolean, default=True)
    max_waitlist = Column(Integer, default=50)
    
    # Event settings
    allow_cancellations = Column(Boolean, default=True)
    cancellation_deadline_hours = Column(Integer, default=24)
    allow_transfers = Column(Boolean, default=True)
    require_approval = Column(Boolean, default=False)
    
    # Media and branding
    banner_image_url = Column(String(500))
    logo_url = Column(String(500))
    custom_css = Column(Text)
    custom_js = Column(Text)
    
    # Metadata
    tags = Column(JSON)  # Array of tag strings
    custom_fields = Column(JSON)  # Custom event fields
    
    # Foreign keys
    tenant_id = Column(Integer, ForeignKey('tenants.id'), nullable=False)
    organizer_id = Column(Integer, ForeignKey('users.id'), nullable=False)
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)
    
    # Relationships
    tenant = relationship('Tenant', back_populates='events')
    organizer = relationship('User', back_populates='events')
    category = relationship('EventCategory', backref='events')
    location = relationship('EventLocation', backref='events')
    schedules = relationship('EventSchedule', back_populates='event', cascade='all, delete-orphan')
    tickets = relationship('Ticket', back_populates='event')
    
    @validates('slug')
    def validate_slug(self, key, slug):
        """Validate event slug format"""
        if not slug or len(slug) < 3:
            raise ValueError('Slug must be at least 3 characters long')
        if not slug.replace('-', '').replace('_', '').replace(' ', '').isalnum():
            raise ValueError('Slug can only contain letters, numbers, hyphens, underscores, and spaces')
        return slug.lower().replace(' ', '-')
    
    @validates('max_capacity')
    def validate_max_capacity(self, key, capacity):
        """Validate maximum capacity"""
        if capacity is not None and capacity <= 0:
            raise ValueError('Maximum capacity must be greater than 0')
        return capacity
    
    @validates('price')
    def validate_price(self, key, price):
        """Validate event price"""
        if price is not None and price < 0:
            raise ValueError('Price cannot be negative')
        return price
    
    @hybrid_property
    def is_registration_open(self):
        """Check if event registration is currently open"""
        now = datetime.utcnow()
        
        if self.registration_opens_at and now < self.registration_opens_at:
            return False
        
        if self.registration_closes_at and now > self.registration_closes_at:
            return False
        
        return self.status == EventStatus.PUBLISHED and self.is_approved
    
    @hybrid_property
    def is_full(self):
        """Check if event is at full capacity"""
        if self.max_capacity is None:
            return False
        return self.current_registrations >= self.max_capacity
    
    @hybrid_property
    def available_spots(self):
        """Get number of available spots"""
        if self.max_capacity is None:
            return None
        return max(0, self.max_capacity - self.current_registrations)
    
    @hybrid_property
    def registration_percentage(self):
        """Get registration percentage"""
        if self.max_capacity is None or self.max_capacity == 0:
            return 0
        return (self.current_registrations / self.max_capacity) * 100
    
    def can_register(self, user):
        """Check if a user can register for this event"""
        if not self.is_registration_open:
            return False, "Registration is not open"
        
        if self.is_full:
            return False, "Event is full"
        
        # Check if user is already registered
        existing_ticket = Ticket.query.filter_by(
            event_id=self.id,
            attendee_id=user.id
        ).first()
        
        if existing_ticket:
            return False, "Already registered for this event"
        
        return True, "Can register"
    
    def increment_registrations(self):
        """Increment current registration count"""
        self.current_registrations += 1
    
    def decrement_registrations(self):
        """Decrement current registration count"""
        if self.current_registrations > 0:
            self.current_registrations -= 1
    
    def publish(self):
        """Publish the event"""
        self.status = EventStatus.PUBLISHED
        self.updated_at = datetime.utcnow()
    
    def cancel(self, reason="Event cancelled"):
        """Cancel the event"""
        self.status = EventStatus.CANCELLED
        self.updated_at = datetime.utcnow()
        # Could add cancellation_reason field
    
    def to_dict(self):
        """Convert event to dictionary"""
        return {
            'id': self.id,
            'title': self.title,
            'slug': self.slug,
            'description': self.description,
            'short_description': self.short_description,
            'event_type': self.event_type,
            'category_id': self.category_id,
            'location_id': self.location_id,
            'status': self.status.value if self.status else None,
            'visibility': self.visibility.value if self.visibility else None,
            'is_featured': self.is_featured,
            'is_approved': self.is_approved,
            'max_capacity': self.max_capacity,
            'current_registrations': self.current_registrations,
            'is_free': self.is_free,
            'price': float(self.price) if self.price else None,
            'currency': self.currency,
            'registration_opens_at': self.registration_opens_at.isoformat() if self.registration_opens_at else None,
            'registration_closes_at': self.registration_closes_at.isoformat() if self.registration_closes_at else None,
            'allow_waitlist': self.allow_waitlist,
            'max_waitlist': self.max_waitlist,
            'allow_cancellations': self.allow_cancellations,
            'cancellation_deadline_hours': self.cancellation_deadline_hours,
            'allow_transfers': self.allow_transfers,
            'require_approval': self.require_approval,
            'banner_image_url': self.banner_image_url,
            'logo_url': self.logo_url,
            'tags': self.tags,
            'custom_fields': self.custom_fields,
            'tenant_id': self.tenant_id,
            'organizer_id': self.organizer_id,
            'is_registration_open': self.is_registration_open,
            'is_full': self.is_full,
            'available_spots': self.available_spots,
            'registration_percentage': self.registration_percentage,
            'created_at': self.created_at.isoformat() if self.created_at else None,
            'updated_at': self.updated_at.isoformat() if self.updated_at else None
        }
    
    def __repr__(self):
        return f'<Event {self.title}>'

class EventCategory(db.Model):
    """Event category classification"""
    
    __tablename__ = 'event_categories'
    
    id = Column(Integer, primary_key=True)
    name = Column(String(100), nullable=False, unique=True)
    slug = Column(String(100), unique=True, nullable=False, index=True)
    description = Column(Text)
    icon = Column(String(100))  # Icon class or identifier
    color = Column(String(7))   # Hex color code
    is_active = Column(Boolean, default=True, nullable=False)
    sort_order = Column(Integer, default=0)
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)
    
    @validates('slug')
    def validate_slug(self, key, slug):
        """Validate category slug format"""
        if not slug or len(slug) < 2:
            raise ValueError('Slug must be at least 2 characters long')
        if not slug.replace('-', '').replace('_', '').isalnum():
            raise ValueError('Slug can only contain letters, numbers, hyphens, and underscores')
        return slug.lower()
    
    def to_dict(self):
        """Convert category to dictionary"""
        return {
            'id': self.id,
            'name': self.name,
            'slug': self.slug,
            'description': self.description,
            'icon': self.icon,
            'color': self.color,
            'is_active': self.is_active,
            'sort_order': self.sort_order,
            'created_at': self.created_at.isoformat() if self.created_at else None
        }
    
    def __repr__(self):
        return f'<EventCategory {self.name}>'

class EventLocation(db.Model):
    """Event location information"""
    
    __tablename__ = 'event_locations'
    
    id = Column(Integer, primary_key=True)
    name = Column(String(255), nullable=False)
    address = Column(Text, nullable=False)
    city = Column(String(100), nullable=False)
    state = Column(String(100))
    country = Column(String(100), nullable=False)
    postal_code = Column(String(20))
    
    # Coordinates
    latitude = Column(Numeric(10, 8))
    longitude = Column(Numeric(11, 8))
    
    # Contact information
    phone = Column(String(20))
    email = Column(String(255))
    website = Column(String(500))
    
    # Venue details
    venue_type = Column(String(100))  # conference_center, hotel, outdoor, etc.
    capacity = Column(Integer)
    amenities = Column(JSON)  # Array of amenity strings
    
    # Virtual event support
    is_virtual = Column(Boolean, default=False)
    virtual_platform = Column(String(100))  # zoom, teams, custom, etc.
    virtual_url = Column(String(500))
    virtual_instructions = Column(Text)
    
    # Metadata
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)
    
    @validates('latitude')
    def validate_latitude(self, key, lat):
        """Validate latitude value"""
        if lat is not None and (lat < -90 or lat > 90):
            raise ValueError('Latitude must be between -90 and 90')
        return lat
    
    @validates('longitude')
    def validate_longitude(self, key, lon):
        """Validate longitude value"""
        if lon is not None and (lon < -180 or lon > 180):
            raise ValueError('Longitude must be between -180 and 180')
        return lon
    
    def to_dict(self):
        """Convert location to dictionary"""
        return {
            'id': self.id,
            'name': self.name,
            'address': self.address,
            'city': self.city,
            'state': self.state,
            'country': self.country,
            'postal_code': self.postal_code,
            'latitude': float(self.latitude) if self.latitude else None,
            'longitude': float(self.longitude) if self.longitude else None,
            'phone': self.phone,
            'email': self.email,
            'website': self.website,
            'venue_type': self.venue_type,
            'capacity': self.capacity,
            'amenities': self.amenities,
            'is_virtual': self.is_virtual,
            'virtual_platform': self.virtual_platform,
            'virtual_url': self.virtual_url,
            'virtual_instructions': self.virtual_instructions
        }
    
    def __repr__(self):
        return f'<EventLocation {self.name}>'

class EventSchedule(db.Model):
    """Event schedule and timing information"""
    
    __tablename__ = 'event_schedules'
    
    id = Column(Integer, primary_key=True)
    event_id = Column(Integer, ForeignKey('events.id'), nullable=False)
    
    # Schedule details
    start_datetime = Column(DateTime, nullable=False)
    end_datetime = Column(DateTime, nullable=False)
    timezone = Column(String(50), default='UTC', nullable=False)
    
    # Recurring events
    is_recurring = Column(Boolean, default=False)
    recurrence_pattern = Column(String(100))  # daily, weekly, monthly, yearly
    recurrence_interval = Column(Integer, default=1)  # every X days/weeks/months
    recurrence_end_date = Column(DateTime)
    recurrence_exceptions = Column(JSON)  # Array of exception dates
    
    # Session information
    session_name = Column(String(255))
    session_description = Column(Text)
    session_type = Column(String(100))  # keynote, workshop, break, etc.
    
    # Metadata
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)
    
    # Relationships
    event = relationship('Event', back_populates='schedules')
    
    @validates('end_datetime')
    def validate_end_datetime(self, key, end_dt):
        """Validate end datetime is after start datetime"""
        if self.start_datetime and end_dt <= self.start_datetime:
            raise ValueError('End datetime must be after start datetime')
        return end_dt
    
    @validates('recurrence_interval')
    def validate_recurrence_interval(self, key, interval):
        """Validate recurrence interval"""
        if interval < 1:
            raise ValueError('Recurrence interval must be at least 1')
        return interval
    
    @hybrid_property
    def duration_minutes(self):
        """Get event duration in minutes"""
        if self.start_datetime and self.end_datetime:
            delta = self.end_datetime - self.start_datetime
            return int(delta.total_seconds() / 60)
        return 0
    
    @hybrid_property
    def is_past(self):
        """Check if event is in the past"""
        if self.end_datetime:
            return datetime.utcnow() > self.end_datetime
        return False
    
    @hybrid_property
    def is_upcoming(self):
        """Check if event is upcoming"""
        if self.start_datetime:
            return datetime.utcnow() < self.start_datetime
        return False
    
    @hybrid_property
    def is_ongoing(self):
        """Check if event is currently ongoing"""
        now = datetime.utcnow()
        if self.start_datetime and self.end_datetime:
            return self.start_datetime <= now <= self.end_datetime
        return False
    
    def to_dict(self):
        """Convert schedule to dictionary"""
        return {
            'id': self.id,
            'event_id': self.event_id,
            'start_datetime': self.start_datetime.isoformat() if self.start_datetime else None,
            'end_datetime': self.end_datetime.isoformat() if self.end_datetime else None,
            'timezone': self.timezone,
            'is_recurring': self.is_recurring,
            'recurrence_pattern': self.recurrence_pattern,
            'recurrence_interval': self.recurrence_interval,
            'recurrence_end_date': self.recurrence_end_date.isoformat() if self.recurrence_end_date else None,
            'recurrence_exceptions': self.recurrence_exceptions,
            'session_name': self.session_name,
            'session_description': self.session_description,
            'session_type': self.session_type,
            'duration_minutes': self.duration_minutes,
            'is_past': self.is_past,
            'is_upcoming': self.is_upcoming,
            'is_ongoing': self.is_ongoing
        }
    
    def __repr__(self):
        return f'<EventSchedule {self.event_id} {self.start_datetime}>'
