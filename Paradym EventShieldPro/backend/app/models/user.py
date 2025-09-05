"""
EventShield Pro - User Management Models
User accounts, profiles, roles, and permissions
"""

from datetime import datetime, timedelta
from sqlalchemy import Column, Integer, String, Boolean, DateTime, Text, ForeignKey, Table
from sqlalchemy.orm import relationship, validates
from sqlalchemy.ext.hybrid import hybrid_property
from werkzeug.security import generate_password_hash, check_password_hash
from app import db

# Association tables for many-to-many relationships
user_roles = Table('user_roles', db.Model.metadata, extend_existing=True,
    Column('user_id', Integer, ForeignKey('users.id'), primary_key=True),
    Column('role_id', Integer, ForeignKey('roles.id'), primary_key=True)
)

user_permissions = Table('user_permissions', db.Model.metadata,
    Column('user_id', Integer, ForeignKey('users.id'), primary_key=True),
    Column('permission_id', Integer, ForeignKey('user_permissions.id'), primary_key=True)
)

class User(db.Model):
    """User account model"""
    
    __tablename__ = 'users'
    
    id = Column(Integer, primary_key=True)
    email = Column(String(255), unique=True, nullable=False, index=True)
    username = Column(String(100), unique=True, nullable=False, index=True)
    password_hash = Column(String(255), nullable=False)
    first_name = Column(String(100), nullable=False)
    last_name = Column(String(100), nullable=False)
    phone = Column(String(20))
    is_active = Column(Boolean, default=True, nullable=False)
    is_verified = Column(Boolean, default=False, nullable=False)
    email_verified_at = Column(DateTime)
    phone_verified_at = Column(DateTime)
    last_login_at = Column(DateTime)
    failed_login_attempts = Column(Integer, default=0)
    locked_until = Column(DateTime)
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)
    
    # Foreign keys
    tenant_id = Column(Integer, ForeignKey('tenants.id'), nullable=False)
    
    # Relationships
    tenant = relationship('Tenant', back_populates='users')
    profile = relationship('UserProfile', back_populates='user', uselist=False)
    roles = relationship('UserRole', secondary=user_roles, back_populates='users')
    permissions = relationship('UserPermission', secondary=user_permissions, back_populates='users')
    
    # Events and tickets
    events = relationship('Event', back_populates='organizer')
    tickets = relationship('Ticket', back_populates='attendee')
    access_logs = relationship('AccessLog', back_populates='user')
    
    @validates('email')
    def validate_email(self, key, email):
        """Validate email format"""
        if not email or '@' not in email:
            raise ValueError('Invalid email address')
        return email.lower()
    
    @validates('username')
    def validate_username(self, key, username):
        """Validate username format"""
        if not username or len(username) < 3:
            raise ValueError('Username must be at least 3 characters long')
        if not username.replace('_', '').replace('-', '').isalnum():
            raise ValueError('Username can only contain letters, numbers, underscores, and hyphens')
        return username.lower()
    
    @property
    def password(self):
        """Password property - cannot be read"""
        raise AttributeError('Password is not a readable attribute')
    
    @password.setter
    def password(self, password):
        """Set password hash"""
        if len(password) < 8:
            raise ValueError('Password must be at least 8 characters long')
        self.password_hash = generate_password_hash(password)
    
    def verify_password(self, password):
        """Verify password"""
        return check_password_hash(self.password_hash, password)
    
    @hybrid_property
    def full_name(self):
        """Get user's full name"""
        return f"{self.first_name} {self.last_name}"
    
    @hybrid_property
    def is_locked(self):
        """Check if user account is locked"""
        if self.locked_until is None:
            return False
        return datetime.utcnow() < self.locked_until
    
    def lock_account(self, duration_minutes=30):
        """Lock user account for specified duration"""
        self.locked_until = datetime.utcnow() + timedelta(minutes=duration_minutes)
        self.failed_login_attempts = 0
    
    def unlock_account(self):
        """Unlock user account"""
        self.locked_until = None
        self.failed_login_attempts = 0
    
    def record_failed_login(self):
        """Record a failed login attempt"""
        self.failed_login_attempts += 1
        if self.failed_login_attempts >= 5:
            self.lock_account()
    
    def record_successful_login(self):
        """Record a successful login"""
        self.last_login_at = datetime.utcnow()
        self.failed_login_attempts = 0
        if self.is_locked:
            self.unlock_account()
    
    def has_role(self, role_name):
        """Check if user has specific role"""
        return any(role.name == role_name for role in self.roles)
    
    def has_permission(self, permission_name):
        """Check if user has specific permission"""
        return any(perm.name == permission_name for perm in self.permissions)
    
    def to_dict(self):
        """Convert user to dictionary"""
        return {
            'id': self.id,
            'email': self.email,
            'username': self.username,
            'first_name': self.first_name,
            'last_name': self.last_name,
            'phone': self.phone,
            'is_active': self.is_active,
            'is_verified': self.is_verified,
            'full_name': self.full_name,
            'tenant_id': self.tenant_id,
            'created_at': self.created_at.isoformat() if self.created_at else None,
            'last_login_at': self.last_login_at.isoformat() if self.last_login_at else None
        }
    
    def __repr__(self):
        return f'<User {self.username}>'

class UserProfile(db.Model):
    """User profile information"""
    
    __tablename__ = 'user_profiles'
    
    id = Column(Integer, primary_key=True)
    user_id = Column(Integer, ForeignKey('users.id'), nullable=False, unique=True)
    
    # Profile information
    avatar_url = Column(String(500))
    bio = Column(Text)
    date_of_birth = Column(DateTime)
    gender = Column(String(20))
    address = Column(Text)
    city = Column(String(100))
    state = Column(String(100))
    country = Column(String(100))
    postal_code = Column(String(20))
    
    # Preferences
    language = Column(String(10), default='en')
    timezone = Column(String(50), default='UTC')
    notification_preferences = Column(Text)  # JSON string
    
    # Social media
    website = Column(String(500))
    linkedin = Column(String(500))
    twitter = Column(String(500))
    facebook = Column(String(500))
    
    # Metadata
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)
    
    # Relationships
    user = relationship('User', back_populates='profile')
    
    def to_dict(self):
        """Convert profile to dictionary"""
        return {
            'id': self.id,
            'user_id': self.user_id,
            'avatar_url': self.avatar_url,
            'bio': self.bio,
            'date_of_birth': self.date_of_birth.isoformat() if self.date_of_birth else None,
            'gender': self.gender,
            'address': self.address,
            'city': self.city,
            'state': self.state,
            'country': self.country,
            'postal_code': self.postal_code,
            'language': self.language,
            'timezone': self.timezone,
            'notification_preferences': self.notification_preferences,
            'website': self.website,
            'linkedin': self.linkedin,
            'twitter': self.twitter,
            'facebook': self.facebook
        }
    
    def __repr__(self):
        return f'<UserProfile {self.user_id}>'

class UserRole(db.Model):
    """User roles for access control"""
    
    __tablename__ = 'user_roles'
    
    id = Column(Integer, primary_key=True)
    name = Column(String(100), unique=True, nullable=False)
    description = Column(Text)
    is_system_role = Column(Boolean, default=False, nullable=False)
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)
    
    # Relationships
    users = relationship('User', secondary=user_roles, back_populates='roles')
    permissions = relationship('UserPermission', back_populates='roles')
    
    # Predefined system roles
    SYSTEM_ROLES = {
        'super_admin': 'Super Administrator with full system access',
        'tenant_admin': 'Tenant Administrator with full tenant access',
        'event_manager': 'Event Manager with event management access',
        'access_control': 'Access Control Operator',
        'attendee': 'Event Attendee with basic access',
        'viewer': 'Read-only access to assigned resources'
    }
    
    @classmethod
    def create_system_roles(cls):
        """Create predefined system roles"""
        for role_name, description in cls.SYSTEM_ROLES.items():
            if not cls.query.filter_by(name=role_name).first():
                role = cls(name=role_name, description=description, is_system_role=True)
                db.session.add(role)
        db.session.commit()
    
    def to_dict(self):
        """Convert role to dictionary"""
        return {
            'id': self.id,
            'name': self.name,
            'description': self.description,
            'is_system_role': self.is_system_role,
            'created_at': self.created_at.isoformat() if self.created_at else None
        }
    
    def __repr__(self):
        return f'<UserRole {self.name}>'

class UserPermission(db.Model):
    """User permissions for fine-grained access control"""
    
    __tablename__ = 'user_permissions'
    
    id = Column(Integer, primary_key=True)
    name = Column(String(100), unique=True, nullable=False)
    description = Column(Text)
    resource = Column(String(100), nullable=False)  # e.g., 'event', 'user', 'ticket'
    action = Column(String(100), nullable=False)    # e.g., 'create', 'read', 'update', 'delete'
    is_system_permission = Column(Boolean, default=False, nullable=False)
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)
    
    # Relationships
    users = relationship('User', secondary=user_permissions, back_populates='permissions')
    roles = relationship('UserRole', back_populates='permissions')
    
    # Predefined system permissions
    SYSTEM_PERMISSIONS = [
        # User management
        ('user', 'create', 'Create new users'),
        ('user', 'read', 'View user information'),
        ('user', 'update', 'Modify user information'),
        ('user', 'delete', 'Delete users'),
        
        # Event management
        ('event', 'create', 'Create new events'),
        ('event', 'read', 'View event information'),
        ('event', 'update', 'Modify event information'),
        ('event', 'delete', 'Delete events'),
        
        # Ticket management
        ('ticket', 'create', 'Create new tickets'),
        ('ticket', 'read', 'View ticket information'),
        ('ticket', 'update', 'Modify ticket information'),
        ('ticket', 'delete', 'Delete tickets'),
        
        # Access control
        ('access_control', 'read', 'View access logs'),
        ('access_control', 'update', 'Modify access permissions'),
        
        # System administration
        ('system', 'admin', 'Full system administration'),
        ('tenant', 'admin', 'Tenant administration'),
    ]
    
    @classmethod
    def create_system_permissions(cls):
        """Create predefined system permissions"""
        for resource, action, description in cls.SYSTEM_PERMISSIONS:
            permission_name = f"{resource}:{action}"
            if not cls.query.filter_by(name=permission_name).first():
                permission = cls(
                    name=permission_name,
                    description=description,
                    resource=resource,
                    action=action,
                    is_system_permission=True
                )
                db.session.add(permission)
        db.session.commit()
    
    def to_dict(self):
        """Convert permission to dictionary"""
        return {
            'id': self.id,
            'name': self.name,
            'description': self.description,
            'resource': self.resource,
            'action': self.action,
            'is_system_permission': self.is_system_permission,
            'created_at': self.created_at.isoformat() if self.created_at else None
        }
    
    def __repr__(self):
        return f'<UserPermission {self.name}>'
