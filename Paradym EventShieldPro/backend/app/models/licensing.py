from datetime import datetime, timedelta
from sqlalchemy import Column, Integer, String, Boolean, DateTime, Text, ForeignKey, JSON, Enum, Index, Numeric
from sqlalchemy.orm import relationship, validates
from sqlalchemy.ext.hybrid import hybrid_property
from app import db
import enum
import uuid

class LicenseTier(enum.Enum):
    TRIAL = 'trial'
    STANDARD = 'standard'
    PREMIUM = 'premium'
    ENTERPRISE = 'enterprise'

class LicenseStatus(enum.Enum):
    ACTIVE = 'active'
    EXPIRED = 'expired'
    SUSPENDED = 'suspended'
    CANCELLED = 'cancelled'
    PENDING = 'pending'
    TRIAL = 'trial'

class FeatureType(enum.Enum):
    EVENT_CREATION = 'event_creation'
    TICKET_SALES = 'ticket_sales'
    ACCESS_CONTROL = 'access_control'
    FACIAL_RECOGNITION = 'facial_recognition'
    TURNSILE_INTEGRATION = 'turnstile_integration'
    ANALYTICS = 'analytics'
    REPORTING = 'reporting'
    API_ACCESS = 'api_access'
    WHITE_LABEL = 'white_label'
    CUSTOM_INTEGRATIONS = 'custom_integrations'
    PRIORITY_SUPPORT = 'priority_support'
    DEDICATED_SERVER = 'dedicated_server'

class License(db.Model):
    __tablename__ = 'licenses'
    
    id = Column(Integer, primary_key=True)
    license_key = Column(String(100), unique=True, nullable=False, index=True)
    tenant_id = Column(Integer, ForeignKey('tenants.id'), nullable=False, index=True)
    
    # License details
    tier = Column(Enum(LicenseTier), nullable=False, index=True)
    status = Column(Enum(LicenseStatus), default=LicenseStatus.PENDING, nullable=False, index=True)
    
    # Subscription details
    stripe_subscription_id = Column(String(255), nullable=True, index=True)
    stripe_customer_id = Column(String(255), nullable=True, index=True)
    
    # Timing
    issued_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    activated_at = Column(DateTime, nullable=True)
    expires_at = Column(DateTime, nullable=True)
    trial_ends_at = Column(DateTime, nullable=True)
    
    # Usage limits
    max_events = Column(Integer, nullable=True)
    max_attendees = Column(Integer, nullable=True)
    max_users = Column(Integer, nullable=True)
    max_api_calls = Column(Integer, nullable=True)
    
    # Current usage
    current_events = Column(Integer, default=0, nullable=False)
    current_attendees = Column(Integer, default=0, nullable=False)
    current_users = Column(Integer, default=0, nullable=False)
    current_api_calls = Column(Integer, default=0, nullable=False)
    
    # Features
    enabled_features = Column(JSON, nullable=True)
    feature_flags = Column(JSON, nullable=True)
    
    # White-label settings
    white_label_enabled = Column(Boolean, default=False)
    custom_domain = Column(String(255), nullable=True)
    custom_branding = Column(JSON, nullable=True)
    
    # Metadata
    notes = Column(Text, nullable=True)
    custom_fields = Column(JSON, nullable=True)
    
    # Relationships
    tenant = relationship('Tenant', back_populates='licenses')
    license_features = relationship('LicenseFeature', back_populates='license')
    license_usage = relationship('LicenseUsage', back_populates='license')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_licenses_tenant_status', 'tenant_id', 'status'),
        Index('idx_licenses_tier_status', 'tier', 'status'),
        Index('idx_licenses_expiry', 'expires_at'),
        Index('idx_licenses_trial', 'trial_ends_at'),
    )
    
    def __init__(self, **kwargs):
        super(License, self).__init__(**kwargs)
        if not self.license_key:
            self.license_key = self.generate_license_key()
    
    def generate_license_key(self):
        """Generate unique license key"""
        return f"LIC-{uuid.uuid4().hex[:16].upper()}"
    
    @hybrid_property
    def is_active(self):
        """Check if license is active"""
        return self.status == LicenseStatus.ACTIVE
    
    @hybrid_property
    def is_trial(self):
        """Check if license is in trial period"""
        return self.status == LicenseStatus.TRIAL
    
    @hybrid_property
    def is_expired(self):
        """Check if license has expired"""
        if self.expires_at:
            return datetime.utcnow() > self.expires_at
        return False
    
    @hybrid_property
    def is_trial_expired(self):
        """Check if trial period has expired"""
        if self.trial_ends_at:
            return datetime.utcnow() > self.trial_ends_at
        return False
    
    @hybrid_property
    def days_until_expiry(self):
        """Get days until license expires"""
        if self.expires_at:
            delta = self.expires_at - datetime.utcnow()
            return max(0, delta.days)
        return None
    
    @hybrid_property
    def days_until_trial_expiry(self):
        """Get days until trial expires"""
        if self.trial_ends_at:
            delta = self.trial_ends_at - datetime.utcnow()
            return max(0, delta.days)
        return None
    
    @hybrid_property
    def usage_percentage(self):
        """Get overall usage percentage"""
        total_usage = 0
        total_limit = 0
        
        if self.max_events:
            total_usage += self.current_events
            total_limit += self.max_events
        
        if self.max_attendees:
            total_usage += self.current_attendees
            total_limit += self.max_attendees
        
        if self.max_users:
            total_usage += self.current_users
            total_limit += self.max_users
        
        if total_limit > 0:
            return (total_usage / total_limit) * 100
        return 0
    
    def activate(self):
        """Activate the license"""
        self.status = LicenseStatus.ACTIVE
        self.activated_at = datetime.utcnow()
        
        # Set trial period if applicable
        if self.tier == LicenseTier.TRIAL:
            self.status = LicenseStatus.TRIAL
            self.trial_ends_at = datetime.utcnow() + timedelta(days=14)
    
    def suspend(self, reason=None):
        """Suspend the license"""
        self.status = LicenseStatus.SUSPENDED
        if reason:
            self.notes = f"Suspended: {reason}"
    
    def cancel(self, reason=None):
        """Cancel the license"""
        self.status = LicenseStatus.CANCELLED
        if reason:
            self.notes = f"Cancelled: {reason}"
    
    def check_feature_access(self, feature_name):
        """Check if a feature is enabled"""
        if not self.enabled_features:
            return False
        return feature_name in self.enabled_features
    
    def check_usage_limit(self, resource_type, amount=1):
        """Check if usage is within limits"""
        if resource_type == 'events':
            if self.max_events and self.current_events + amount > self.max_events:
                return False
        elif resource_type == 'attendees':
            if self.max_attendees and self.current_attendees + amount > self.max_attendees:
                return False
        elif resource_type == 'users':
            if self.max_users and self.current_users + amount > self.max_users:
                return False
        elif resource_type == 'api_calls':
            if self.max_api_calls and self.current_api_calls + amount > self.max_api_calls:
                return False
        return True
    
    def increment_usage(self, resource_type, amount=1):
        """Increment usage for a resource type"""
        if resource_type == 'events':
            self.current_events += amount
        elif resource_type == 'attendees':
            self.current_attendees += amount
        elif resource_type == 'users':
            self.current_users += amount
        elif resource_type == 'api_calls':
            self.current_api_calls += amount
    
    def __repr__(self):
        return f'<License {self.license_key} - {self.tier.value} {self.status.value}>'

class LicenseFeature(db.Model):
    __tablename__ = 'license_features'
    
    id = Column(Integer, primary_key=True)
    license_id = Column(Integer, ForeignKey('licenses.id'), nullable=False, index=True)
    feature_name = Column(Enum(FeatureType), nullable=False, index=True)
    
    # Feature configuration
    is_enabled = Column(Boolean, default=True, nullable=False)
    max_usage = Column(Integer, nullable=True)
    current_usage = Column(Integer, default=0, nullable=False)
    
    # Feature settings
    settings = Column(JSON, nullable=True)
    restrictions = Column(JSON, nullable=True)
    
    # Status
    status = Column(String(50), default='active', nullable=False)
    
    # Relationships
    license = relationship('License', back_populates='license_features')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_license_features_license', 'license_id', 'feature_name'),
        Index('idx_license_features_enabled', 'feature_name', 'is_enabled'),
    )
    
    @hybrid_property
    def is_available(self):
        """Check if feature is available"""
        if not self.is_enabled:
            return False
        if self.max_usage and self.current_usage >= self.max_usage:
            return False
        return True
    
    @hybrid_property
    def usage_percentage(self):
        """Get usage percentage for this feature"""
        if self.max_usage:
            return (self.current_usage / self.max_usage) * 100
        return 0
    
    @hybrid_property
    def remaining_usage(self):
        """Get remaining usage for this feature"""
        if self.max_usage:
            return max(0, self.max_usage - self.current_usage)
        return None
    
    def increment_usage(self, amount=1):
        """Increment usage for this feature"""
        if self.max_usage and self.current_usage + amount > self.max_usage:
            raise ValueError("Feature usage limit exceeded")
        self.current_usage += amount
    
    def __repr__(self):
        return f'<LicenseFeature {self.feature_name.value} - {self.is_enabled}>'

class LicenseUsage(db.Model):
    __tablename__ = 'license_usage'
    
    id = Column(Integer, primary_key=True)
    license_id = Column(Integer, ForeignKey('licenses.id'), nullable=False, index=True)
    feature_name = Column(Enum(FeatureType), nullable=False, index=True)
    
    # Usage details
    usage_type = Column(String(50), nullable=False)  # increment, reset, check
    usage_amount = Column(Integer, default=1, nullable=False)
    usage_description = Column(Text, nullable=True)
    
    # Context
    user_id = Column(Integer, ForeignKey('users.id'), nullable=True, index=True)
    event_id = Column(Integer, ForeignKey('events.id'), nullable=True, index=True)
    ip_address = Column(String(45), nullable=True)
    
    # Metadata
    metadata = Column(JSON, nullable=True)
    
    # Relationships
    license = relationship('License', back_populates='license_usage')
    user = relationship('User')
    event = relationship('Event')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_license_usage_license_feature', 'license_id', 'feature_name'),
        Index('idx_license_usage_user_time', 'user_id', 'created_at'),
        Index('idx_license_usage_event_time', 'event_id', 'created_at'),
    )
    
    def __repr__(self):
        return f'<LicenseUsage {self.feature_name.value} - {self.usage_amount}>'

class LicenseAudit(db.Model):
    __tablename__ = 'license_audit'
    
    id = Column(Integer, primary_key=True)
    license_id = Column(Integer, ForeignKey('licenses.id'), nullable=False, index=True)
    
    # Audit details
    action = Column(String(100), nullable=False)  # created, activated, suspended, etc.
    action_by_user_id = Column(Integer, ForeignKey('users.id'), nullable=True, index=True)
    
    # Changes
    old_values = Column(JSON, nullable=True)
    new_values = Column(JSON, nullable=True)
    
    # Context
    ip_address = Column(String(45), nullable=True)
    user_agent = Column(String(500), nullable=True)
    reason = Column(Text, nullable=True)
    
    # Relationships
    license = relationship('License')
    action_by_user = relationship('User')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_license_audit_license', 'license_id', 'created_at'),
        Index('idx_license_audit_action', 'action', 'created_at'),
        Index('idx_license_audit_user', 'action_by_user_id', 'created_at'),
    )
    
    def __repr__(self):
        return f'<LicenseAudit {self.action} - {self.license_id}>'

class LicenseTemplate(db.Model):
    __tablename__ = 'license_templates'
    
    id = Column(Integer, primary_key=True)
    name = Column(String(100), nullable=False, unique=True)
    description = Column(Text, nullable=True)
    
    # Template configuration
    tier = Column(Enum(LicenseTier), nullable=False)
    duration_days = Column(Integer, nullable=True)
    trial_days = Column(Integer, default=0, nullable=False)
    
    # Default limits
    max_events = Column(Integer, nullable=True)
    max_attendees = Column(Integer, nullable=True)
    max_users = Column(Integer, nullable=True)
    max_api_calls = Column(Integer, nullable=True)
    
    # Default features
    default_features = Column(JSON, nullable=True)
    feature_limits = Column(JSON, nullable=True)
    
    # Pricing
    monthly_price = Column(Numeric(10, 2), nullable=True)
    yearly_price = Column(Numeric(10, 2), nullable=True)
    currency = Column(String(3), default='USD', nullable=False)
    
    # Status
    is_active = Column(Boolean, default=True, nullable=False)
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_license_templates_tier', 'tier', 'is_active'),
        Index('idx_license_templates_active', 'is_active'),
    )
    
    def create_license(self, tenant_id, **kwargs):
        """Create a new license from this template"""
        license_data = {
            'tenant_id': tenant_id,
            'tier': self.tier,
            'max_events': self.max_events,
            'max_attendees': self.max_attendees,
            'max_users': self.max_users,
            'max_api_calls': self.max_api_calls,
            'enabled_features': self.default_features,
            'feature_flags': self.feature_limits
        }
        
        # Override with custom values
        license_data.update(kwargs)
        
        # Set expiration if duration is specified
        if self.duration_days:
            license_data['expires_at'] = datetime.utcnow() + timedelta(days=self.duration_days)
        
        # Set trial period if applicable
        if self.trial_days > 0:
            license_data['trial_ends_at'] = datetime.utcnow() + timedelta(days=self.trial_days)
            license_data['status'] = LicenseStatus.TRIAL
        
        return License(**license_data)
    
    def __repr__(self):
        return f'<LicenseTemplate {self.name} - {self.tier.value}>'

