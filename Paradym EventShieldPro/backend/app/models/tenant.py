"""
EventShield Pro - Tenant Management Models
Multi-tenant architecture for customer isolation
"""

from datetime import datetime, timedelta
from sqlalchemy import Column, Integer, String, Boolean, DateTime, Text, ForeignKey, JSON, Numeric
from sqlalchemy.orm import relationship, validates
from sqlalchemy.ext.hybrid import hybrid_property
from app import db

class Tenant(db.Model):
    """Tenant model for multi-tenant architecture"""
    
    __tablename__ = 'tenants'
    
    id = Column(Integer, primary_key=True)
    name = Column(String(255), nullable=False)
    slug = Column(String(100), unique=True, nullable=False, index=True)
    domain = Column(String(255), unique=True, index=True)
    
    # Company information
    company_name = Column(String(255))
    company_address = Column(Text)
    company_city = Column(String(100))
    company_state = Column(String(100))
    company_country = Column(String(100))
    company_postal_code = Column(String(20))
    company_phone = Column(String(20))
    company_email = Column(String(255))
    company_website = Column(String(500))
    
    # Contact person
    contact_first_name = Column(String(100))
    contact_last_name = Column(String(100))
    contact_email = Column(String(255))
    contact_phone = Column(String(20))
    
    # Subscription and billing
    subscription_tier = Column(String(50), default='trial', nullable=False)  # trial, standard, premium, enterprise
    subscription_status = Column(String(50), default='active', nullable=False)  # active, suspended, cancelled, expired
    trial_ends_at = Column(DateTime)
    subscription_ends_at = Column(DateTime)
    
    # Limits based on subscription tier
    max_events = Column(Integer, default=5)
    max_attendees = Column(Integer, default=100)
    max_users = Column(Integer, default=10)
    max_storage_gb = Column(Integer, default=1)
    
    # Feature flags
    features_enabled = Column(JSON)  # JSON object of enabled features
    
    # White-label settings
    logo_url = Column(String(500))
    primary_color = Column(String(7))  # Hex color code
    secondary_color = Column(String(7))
    custom_css = Column(Text)
    custom_js = Column(Text)
    
    # Status and metadata
    is_active = Column(Boolean, default=True, nullable=False)
    is_verified = Column(Boolean, default=False, nullable=False)
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)
    
    # Relationships
    users = relationship('User', back_populates='tenant')
    events = relationship('Event', back_populates='tenant')
    subscription = relationship('TenantSubscription', back_populates='tenant', uselist=False)
    settings = relationship('TenantSettings', back_populates='tenant', uselist=False)
    
    @validates('slug')
    def validate_slug(self, key, slug):
        """Validate tenant slug format"""
        if not slug or len(slug) < 3:
            raise ValueError('Slug must be at least 3 characters long')
        if not slug.replace('-', '').replace('_', '').isalnum():
            raise ValueError('Slug can only contain letters, numbers, hyphens, and underscores')
        return slug.lower()
    
    @validates('subscription_tier')
    def validate_subscription_tier(self, key, tier):
        """Validate subscription tier"""
        valid_tiers = ['trial', 'standard', 'premium', 'enterprise']
        if tier not in valid_tiers:
            raise ValueError(f'Invalid subscription tier. Must be one of: {", ".join(valid_tiers)}')
        return tier
    
    @hybrid_property
    def is_trial(self):
        """Check if tenant is in trial period"""
        if self.subscription_tier != 'trial':
            return False
        if self.trial_ends_at is None:
            return False
        return datetime.utcnow() < self.trial_ends_at
    
    @hybrid_property
    def is_subscription_active(self):
        """Check if subscription is active"""
        if self.subscription_status != 'active':
            return False
        if self.subscription_ends_at and datetime.utcnow() > self.subscription_ends_at:
            return False
        return True
    
    @hybrid_property
    def days_until_trial_expires(self):
        """Get days until trial expires"""
        if not self.is_trial or self.trial_ends_at is None:
            return 0
        delta = self.trial_ends_at - datetime.utcnow()
        return max(0, delta.days)
    
    def upgrade_subscription(self, new_tier, duration_months=1):
        """Upgrade tenant subscription"""
        self.subscription_tier = new_tier
        self.subscription_status = 'active'
        
        # Set new limits based on tier
        if new_tier == 'standard':
            self.max_events = 50
            self.max_attendees = 1000
            self.max_users = 25
            self.max_storage_gb = 5
        elif new_tier == 'premium':
            self.max_events = 200
            self.max_attendees = 10000
            self.max_users = 100
            self.max_storage_gb = 25
        elif new_tier == 'enterprise':
            self.max_events = -1  # Unlimited
            self.max_attendees = -1  # Unlimited
            self.max_users = -1  # Unlimited
            self.max_storage_gb = 100
        
        # Set subscription end date
        if duration_months > 0:
            self.subscription_ends_at = datetime.utcnow() + timedelta(days=30 * duration_months)
        
        # End trial if upgrading
        if self.subscription_tier != 'trial':
            self.trial_ends_at = None
    
    def suspend_subscription(self, reason='Payment required'):
        """Suspend tenant subscription"""
        self.subscription_status = 'suspended'
        # Could add suspension_reason field if needed
    
    def cancel_subscription(self):
        """Cancel tenant subscription"""
        self.subscription_status = 'cancelled'
        self.subscription_ends_at = datetime.utcnow()
    
    def get_feature_status(self, feature_name):
        """Check if a specific feature is enabled for this tenant"""
        if not self.features_enabled:
            return False
        return self.features_enabled.get(feature_name, False)
    
    def to_dict(self):
        """Convert tenant to dictionary"""
        return {
            'id': self.id,
            'name': self.name,
            'slug': self.slug,
            'domain': self.domain,
            'company_name': self.company_name,
            'company_email': self.company_email,
            'company_website': self.company_website,
            'contact_first_name': self.contact_first_name,
            'contact_last_name': self.contact_last_name,
            'contact_email': self.contact_email,
            'subscription_tier': self.subscription_tier,
            'subscription_status': self.subscription_status,
            'is_trial': self.is_trial,
            'is_subscription_active': self.is_subscription_active,
            'days_until_trial_expires': self.days_until_trial_expires,
            'max_events': self.max_events,
            'max_attendees': self.max_attendees,
            'max_users': self.max_users,
            'max_storage_gb': self.max_storage_gb,
            'features_enabled': self.features_enabled,
            'logo_url': self.logo_url,
            'primary_color': self.primary_color,
            'secondary_color': self.secondary_color,
            'is_active': self.is_active,
            'is_verified': self.is_verified,
            'created_at': self.created_at.isoformat() if self.created_at else None,
            'trial_ends_at': self.trial_ends_at.isoformat() if self.trial_ends_at else None,
            'subscription_ends_at': self.subscription_ends_at.isoformat() if self.subscription_ends_at else None
        }
    
    def __repr__(self):
        return f'<Tenant {self.name}>'

class TenantSubscription(db.Model):
    """Tenant subscription details and billing information"""
    
    __tablename__ = 'tenant_subscriptions'
    
    id = Column(Integer, primary_key=True)
    tenant_id = Column(Integer, ForeignKey('tenants.id'), nullable=False, unique=True)
    
    # Stripe integration
    stripe_customer_id = Column(String(255), unique=True, index=True)
    stripe_subscription_id = Column(String(255), unique=True, index=True)
    stripe_price_id = Column(String(255))
    
    # Subscription details
    current_period_start = Column(DateTime)
    current_period_end = Column(DateTime)
    cancel_at_period_end = Column(Boolean, default=False)
    cancelled_at = Column(DateTime)
    
    # Billing information
    billing_cycle = Column(String(20), default='monthly')  # monthly, yearly
    amount = Column(Numeric(10, 2))  # Amount in cents
    currency = Column(String(3), default='USD')
    
    # Payment method
    payment_method_id = Column(String(255))
    payment_method_type = Column(String(50))  # card, bank_account, etc.
    
    # Metadata
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)
    
    # Relationships
    tenant = relationship('Tenant', back_populates='subscription')
    
    @hybrid_property
    def is_active(self):
        """Check if subscription is active"""
        if self.cancel_at_period_end:
            return False
        if self.cancelled_at:
            return False
        if self.current_period_end and datetime.utcnow() > self.current_period_end:
            return False
        return True
    
    @hybrid_property
    def days_until_renewal(self):
        """Get days until subscription renews"""
        if not self.current_period_end:
            return 0
        delta = self.current_period_end - datetime.utcnow()
        return max(0, delta.days)
    
    def cancel_subscription(self, at_period_end=True):
        """Cancel subscription"""
        if at_period_end:
            self.cancel_at_period_end = True
        else:
            self.cancelled_at = datetime.utcnow()
    
    def to_dict(self):
        """Convert subscription to dictionary"""
        return {
            'id': self.id,
            'tenant_id': self.tenant_id,
            'stripe_customer_id': self.stripe_customer_id,
            'stripe_subscription_id': self.stripe_subscription_id,
            'stripe_price_id': self.stripe_price_id,
            'current_period_start': self.current_period_start.isoformat() if self.current_period_start else None,
            'current_period_end': self.current_period_end.isoformat() if self.current_period_end else None,
            'cancel_at_period_end': self.cancel_at_period_end,
            'cancelled_at': self.cancelled_at.isoformat() if self.cancelled_at else None,
            'billing_cycle': self.billing_cycle,
            'amount': float(self.amount) if self.amount else None,
            'currency': self.currency,
            'payment_method_type': self.payment_method_type,
            'is_active': self.is_active,
            'days_until_renewal': self.days_until_renewal
        }
    
    def __repr__(self):
        return f'<TenantSubscription {self.tenant_id}>'

class TenantSettings(db.Model):
    """Tenant-specific configuration settings"""
    
    __tablename__ = 'tenant_settings'
    
    id = Column(Integer, primary_key=True)
    tenant_id = Column(Integer, ForeignKey('tenants.id'), nullable=False, unique=True)
    
    # General settings
    timezone = Column(String(50), default='UTC')
    language = Column(String(10), default='en')
    date_format = Column(String(20), default='MM/DD/YYYY')
    time_format = Column(String(20), default='12h')  # 12h or 24h
    
    # Notification settings
    email_notifications = Column(Boolean, default=True)
    sms_notifications = Column(Boolean, default=False)
    push_notifications = Column(Boolean, default=True)
    
    # Security settings
    require_mfa = Column(Boolean, default=False)
    session_timeout_minutes = Column(Integer, default=480)  # 8 hours
    max_failed_logins = Column(Integer, default=5)
    
    # Event settings
    auto_approve_events = Column(Boolean, default=False)
    require_event_approval = Column(Boolean, default=True)
    allow_public_events = Column(Boolean, default=True)
    max_event_duration_hours = Column(Integer, default=72)
    
    # Ticket settings
    allow_ticket_transfers = Column(Boolean, default=True)
    require_id_verification = Column(Boolean, default=False)
    auto_generate_qr_codes = Column(Boolean, default=True)
    
    # Access control settings
    facial_recognition_enabled = Column(Boolean, default=True)
    turnstile_integration_enabled = Column(Boolean, default=True)
    emergency_override_enabled = Column(Boolean, default=True)
    
    # Analytics settings
    track_user_behavior = Column(Boolean, default=True)
    share_analytics = Column(Boolean, default=False)
    data_retention_days = Column(Integer, default=365)
    
    # Custom settings (JSON)
    custom_settings = Column(JSON)
    
    # Metadata
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)
    
    # Relationships
    tenant = relationship('Tenant', back_populates='settings')
    
    def get_setting(self, key, default=None):
        """Get a custom setting value"""
        if not self.custom_settings:
            return default
        return self.custom_settings.get(key, default)
    
    def set_setting(self, key, value):
        """Set a custom setting value"""
        if not self.custom_settings:
            self.custom_settings = {}
        self.custom_settings[key] = value
    
    def to_dict(self):
        """Convert settings to dictionary"""
        return {
            'id': self.id,
            'tenant_id': self.tenant_id,
            'timezone': self.timezone,
            'language': self.language,
            'date_format': self.date_format,
            'time_format': self.time_format,
            'email_notifications': self.email_notifications,
            'sms_notifications': self.sms_notifications,
            'push_notifications': self.push_notifications,
            'require_mfa': self.require_mfa,
            'session_timeout_minutes': self.session_timeout_minutes,
            'max_failed_logins': self.max_failed_logins,
            'auto_approve_events': self.auto_approve_events,
            'require_event_approval': self.require_event_approval,
            'allow_public_events': self.allow_public_events,
            'max_event_duration_hours': self.max_event_duration_hours,
            'allow_ticket_transfers': self.allow_ticket_transfers,
            'require_id_verification': self.require_id_verification,
            'auto_generate_qr_codes': self.auto_generate_qr_codes,
            'facial_recognition_enabled': self.facial_recognition_enabled,
            'turnstile_integration_enabled': self.turnstile_integration_enabled,
            'emergency_override_enabled': self.emergency_override_enabled,
            'track_user_behavior': self.track_user_behavior,
            'share_analytics': self.share_analytics,
            'data_retention_days': self.data_retention_days,
            'custom_settings': self.custom_settings
        }
    
    def __repr__(self):
        return f'<TenantSettings {self.tenant_id}>'
