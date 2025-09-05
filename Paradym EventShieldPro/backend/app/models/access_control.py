from datetime import datetime, timedelta
from sqlalchemy import Column, Integer, String, Boolean, DateTime, Text, ForeignKey, JSON, Enum, Index, Float
from sqlalchemy.orm import relationship, validates
from sqlalchemy.ext.hybrid import hybrid_property
from app import db
import enum

class AccessType(enum.Enum):
    ENTRY = 'entry'
    EXIT = 'exit'
    REENTRY = 'reentry'
    EMERGENCY = 'emergency'
    MAINTENANCE = 'maintenance'

class AccessStatus(enum.Enum):
    GRANTED = 'granted'
    DENIED = 'denied'
    PENDING = 'pending'
    OVERRIDE = 'override'
    ERROR = 'error'

class ValidationMethod(enum.Enum):
    QR_SCAN = 'qr_scan'
    FACIAL_RECOGNITION = 'facial_recognition'
    RFID = 'rfid'
    MANUAL = 'manual'
    BIOMETRIC = 'biometric'
    PIN_CODE = 'pin_code'

class AccessPointType(enum.Enum):
    MAIN_ENTRANCE = 'main_entrance'
    VIP_ENTRANCE = 'vip_entrance'
    STAFF_ENTRANCE = 'staff_entrance'
    EMERGENCY_EXIT = 'emergency_exit'
    PARKING_ENTRANCE = 'parking_entrance'
    TURNSILE = 'turnstile'
    GATE = 'gate'
    DOOR = 'door'

class AccessLog(db.Model):
    __tablename__ = 'access_logs'
    
    id = Column(Integer, primary_key=True)
    event_id = Column(Integer, ForeignKey('events.id'), nullable=False, index=True)
    access_point_id = Column(Integer, ForeignKey('access_points.id'), nullable=False, index=True)
    user_id = Column(Integer, ForeignKey('users.id'), nullable=True, index=True)
    ticket_id = Column(Integer, ForeignKey('tickets.id'), nullable=True, index=True)
    
    # Access details
    access_type = Column(Enum(AccessType), nullable=False, index=True)
    access_status = Column(Enum(AccessStatus), nullable=False, index=True)
    validation_method = Column(Enum(ValidationMethod), nullable=False)
    
    # Timing
    access_time = Column(DateTime, default=datetime.utcnow, nullable=False, index=True)
    processing_time_ms = Column(Integer, nullable=True)
    
    # Location and device
    location = Column(String(255), nullable=True)
    device_id = Column(String(100), nullable=True)
    device_type = Column(String(50), nullable=True)
    
    # Validation results
    confidence_score = Column(Float, nullable=True)  # For facial recognition
    validation_notes = Column(Text, nullable=True)
    error_message = Column(Text, nullable=True)
    
    # Override information
    override_reason = Column(Text, nullable=True)
    override_by_user_id = Column(Integer, ForeignKey('users.id'), nullable=True)
    
    # Metadata
    ip_address = Column(String(45), nullable=True)
    user_agent = Column(String(500), nullable=True)
    custom_fields = Column(JSON, nullable=True)
    
    # Relationships
    event = relationship('Event', back_populates='access_logs')
    access_point = relationship('AccessPoint', back_populates='access_logs')
    user = relationship('User', foreign_keys=[user_id], back_populates='access_logs')
    ticket = relationship('Ticket', back_populates='access_logs')
    override_by_user = relationship('User', foreign_keys=[override_by_user_id])
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_access_logs_event_time', 'event_id', 'access_time'),
        Index('idx_access_logs_user_time', 'user_id', 'access_time'),
        Index('idx_access_logs_status_time', 'access_status', 'access_time'),
        Index('idx_access_logs_method_time', 'validation_method', 'access_time'),
    )
    
    @hybrid_property
    def is_successful(self):
        """Check if access was successful"""
        return self.access_status == AccessStatus.GRANTED
    
    @hybrid_property
    def is_denied(self):
        """Check if access was denied"""
        return self.access_status == AccessStatus.DENIED
    
    @hybrid_property
    def is_override(self):
        """Check if access was granted via override"""
        return self.access_status == AccessStatus.OVERRIDE
    
    @hybrid_property
    def processing_time_seconds(self):
        """Get processing time in seconds"""
        if self.processing_time_ms:
            return self.processing_time_ms / 1000.0
        return None
    
    def __repr__(self):
        return f'<AccessLog {self.id} - {self.access_type.value} {self.access_status.value}>'

class AccessPoint(db.Model):
    __tablename__ = 'access_points'
    
    id = Column(Integer, primary_key=True)
    event_id = Column(Integer, ForeignKey('events.id'), nullable=False, index=True)
    name = Column(String(100), nullable=False)
    description = Column(Text, nullable=True)
    type = Column(Enum(AccessPointType), nullable=False)
    
    # Location
    location = Column(String(255), nullable=True)
    coordinates = Column(JSON, nullable=True)  # lat, lng
    floor_level = Column(String(20), nullable=True)
    building = Column(String(100), nullable=True)
    
    # Hardware configuration
    device_id = Column(String(100), unique=True, nullable=False, index=True)
    device_type = Column(String(50), nullable=False)  # turnstile, camera, gate, etc.
    hardware_model = Column(String(100), nullable=True)  # DS-F881, DSN-50P, etc.
    serial_number = Column(String(100), nullable=True)
    
    # Capabilities
    supports_facial_recognition = Column(Boolean, default=False)
    supports_qr_scanning = Column(Boolean, default=False)
    supports_rfid = Column(Boolean, default=False)
    supports_biometric = Column(Boolean, default=False)
    supports_manual_override = Column(Boolean, default=True)
    
    # Configuration
    is_active = Column(Boolean, default=True, nullable=False)
    is_emergency_exit = Column(Boolean, default=False)
    requires_authentication = Column(Boolean, default=True)
    max_capacity = Column(Integer, nullable=True)
    current_capacity = Column(Integer, default=0)
    
    # Access control
    allowed_user_roles = Column(JSON, nullable=True)  # List of role IDs
    restricted_hours = Column(JSON, nullable=True)  # Operating hours
    maintenance_schedule = Column(JSON, nullable=True)
    
    # Status
    status = Column(String(50), default='online', nullable=False, index=True)
    last_heartbeat = Column(DateTime, nullable=True)
    last_maintenance = Column(DateTime, nullable=True)
    
    # Relationships
    event = relationship('Event', back_populates='access_points')
    access_logs = relationship('AccessLog', back_populates='access_point')
    device_status = relationship('DeviceStatus', back_populates='access_point')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_access_points_event_active', 'event_id', 'is_active'),
        Index('idx_access_points_device_id', 'device_id'),
        Index('idx_access_points_type', 'type'),
        Index('idx_access_points_status', 'status'),
    )
    
    @hybrid_property
    def is_online(self):
        """Check if access point is online"""
        return self.status == 'online'
    
    @hybrid_property
    def is_offline(self):
        """Check if access point is offline"""
        return self.status == 'offline'
    
    @hybrid_property
    def is_maintenance_mode(self):
        """Check if access point is in maintenance mode"""
        return self.status == 'maintenance'
    
    @hybrid_property
    def is_at_capacity(self):
        """Check if access point is at capacity"""
        if self.max_capacity is None:
            return False
        return self.current_capacity >= self.max_capacity
    
    @hybrid_property
    def available_capacity(self):
        """Get available capacity"""
        if self.max_capacity is None:
            return None
        return max(0, self.max_capacity - self.current_capacity)
    
    def increment_capacity(self, amount=1):
        """Increment current capacity"""
        if self.max_capacity and self.current_capacity + amount > self.max_capacity:
            raise ValueError("Capacity limit exceeded")
        self.current_capacity += amount
    
    def decrement_capacity(self, amount=1):
        """Decrement current capacity"""
        self.current_capacity = max(0, self.current_capacity - amount)
    
    def update_status(self, status, heartbeat=True):
        """Update access point status"""
        self.status = status
        if heartbeat:
            self.last_heartbeat = datetime.utcnow()
    
    def __repr__(self):
        return f'<AccessPoint {self.name} - {self.type.value}>'

class AccessPermission(db.Model):
    __tablename__ = 'access_permissions'
    
    id = Column(Integer, primary_key=True)
    user_id = Column(Integer, ForeignKey('users.id'), nullable=False, index=True)
    access_point_id = Column(Integer, ForeignKey('access_points.id'), nullable=False, index=True)
    event_id = Column(Integer, ForeignKey('events.id'), nullable=False, index=True)
    
    # Permission details
    permission_type = Column(String(50), nullable=False)  # entry, exit, both
    is_active = Column(Boolean, default=True, nullable=False)
    
    # Time restrictions
    valid_from = Column(DateTime, nullable=True)
    valid_until = Column(DateTime, nullable=True)
    allowed_days = Column(JSON, nullable=True)  # List of day numbers (0-6)
    allowed_hours = Column(JSON, nullable=True)  # List of hour ranges
    
    # Usage limits
    max_uses = Column(Integer, nullable=True)
    current_uses = Column(Integer, default=0)
    
    # Override permissions
    can_override = Column(Boolean, default=False)
    override_reason_required = Column(Boolean, default=True)
    
    # Relationships
    user = relationship('User', back_populates='access_permissions')
    access_point = relationship('AccessPoint')
    event = relationship('Event')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_access_permissions_user_active', 'user_id', 'is_active'),
        Index('idx_access_permissions_access_point', 'access_point_id', 'is_active'),
        Index('idx_access_permissions_event', 'event_id', 'is_active'),
        Index('idx_access_permissions_validity', 'valid_from', 'valid_until'),
    )
    
    @hybrid_property
    def is_valid(self):
        """Check if permission is currently valid"""
        if not self.is_active:
            return False
        
        now = datetime.utcnow()
        
        if self.valid_from and now < self.valid_from:
            return False
        
        if self.valid_until and now > self.valid_until:
            return False
        
        if self.max_uses and self.current_uses >= self.max_uses:
            return False
        
        return True
    
    @hybrid_property
    def is_expired(self):
        """Check if permission has expired"""
        if self.valid_until:
            return datetime.utcnow() > self.valid_until
        return False
    
    @hybrid_property
    def is_at_usage_limit(self):
        """Check if permission has reached usage limit"""
        if self.max_uses:
            return self.current_uses >= self.max_uses
        return False
    
    def increment_usage(self, amount=1):
        """Increment usage count"""
        if self.max_uses and self.current_uses + amount > self.max_uses:
            raise ValueError("Usage limit exceeded")
        self.current_uses += amount
    
    def check_time_restrictions(self, access_time=None):
        """Check if access is allowed at the given time"""
        if access_time is None:
            access_time = datetime.utcnow()
        
        # Check day restrictions
        if self.allowed_days:
            day_of_week = access_time.weekday()
            if day_of_week not in self.allowed_days:
                return False
        
        # Check hour restrictions
        if self.allowed_hours:
            current_hour = access_time.hour
            allowed = False
            for hour_range in self.allowed_hours:
                start_hour, end_hour = hour_range
                if start_hour <= current_hour <= end_hour:
                    allowed = True
                    break
            if not allowed:
                return False
        
        return True
    
    def __repr__(self):
        return f'<AccessPermission {self.user_id} -> {self.access_point_id}>'

class FacialRecognitionLog(db.Model):
    __tablename__ = 'facial_recognition_logs'
    
    id = Column(Integer, primary_key=True)
    access_log_id = Column(Integer, ForeignKey('access_logs.id'), nullable=False, index=True)
    device_id = Column(String(100), nullable=False, index=True)
    
    # Recognition details
    face_id = Column(String(100), nullable=True, index=True)
    confidence_score = Column(Float, nullable=False)
    recognition_time_ms = Column(Integer, nullable=True)
    
    # Image data
    image_hash = Column(String(64), nullable=True)
    image_metadata = Column(JSON, nullable=True)
    
    # Quality metrics
    image_quality_score = Column(Float, nullable=True)
    face_detection_confidence = Column(Float, nullable=True)
    lighting_conditions = Column(String(50), nullable=True)
    
    # Processing results
    processing_status = Column(String(50), default='processing', nullable=False)
    error_message = Column(Text, nullable=True)
    
    # Relationships
    access_log = relationship('AccessLog')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_facial_recognition_logs_device', 'device_id'),
        Index('idx_facial_recognition_logs_face_id', 'face_id'),
        Index('idx_facial_recognition_logs_confidence', 'confidence_score'),
    )
    
    @hybrid_property
    def recognition_time_seconds(self):
        """Get recognition time in seconds"""
        if self.recognition_time_ms:
            return self.recognition_time_ms / 1000.0
        return None
    
    @hybrid_property
    def is_high_confidence(self):
        """Check if recognition has high confidence"""
        return self.confidence_score >= 0.8
    
    @hybrid_property
    def is_medium_confidence(self):
        """Check if recognition has medium confidence"""
        return 0.6 <= self.confidence_score < 0.8
    
    @hybrid_property
    def is_low_confidence(self):
        """Check if recognition has low confidence"""
        return self.confidence_score < 0.6
    
    def __repr__(self):
        return f'<FacialRecognitionLog {self.id} - {self.confidence_score:.2f}>'

class TurnstileLog(db.Model):
    __tablename__ = 'turnstile_logs'
    
    id = Column(Integer, primary_key=True)
    access_log_id = Column(Integer, ForeignKey('access_logs.id'), nullable=False, index=True)
    device_id = Column(String(100), nullable=False, index=True)
    
    # Turnstile details
    turnstile_id = Column(String(100), nullable=False, index=True)
    direction = Column(String(20), nullable=False)  # entry, exit
    rotation_count = Column(Integer, default=0)
    
    # Mechanical status
    motor_status = Column(String(50), nullable=True)
    sensor_status = Column(JSON, nullable=True)
    emergency_status = Column(String(50), default='normal')
    
    # Safety and maintenance
    obstruction_detected = Column(Boolean, default=False)
    maintenance_required = Column(Boolean, default=False)
    last_maintenance = Column(DateTime, nullable=True)
    
    # Performance metrics
    cycle_time_ms = Column(Integer, nullable=True)
    power_consumption = Column(Float, nullable=True)
    temperature = Column(Float, nullable=True)
    
    # Relationships
    access_log = relationship('AccessLog')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_turnstile_logs_device', 'device_id'),
        Index('idx_turnstile_logs_turnstile', 'turnstile_id'),
        Index('idx_turnstile_logs_direction', 'direction'),
    )
    
    @hybrid_property
    def cycle_time_seconds(self):
        """Get cycle time in seconds"""
        if self.cycle_time_ms:
            return self.cycle_time_ms / 1000.0
        return None
    
    @hybrid_property
    def is_emergency_mode(self):
        """Check if turnstile is in emergency mode"""
        return self.emergency_status != 'normal'
    
    @hybrid_property
    def needs_maintenance(self):
        """Check if turnstile needs maintenance"""
        return self.maintenance_required
    
    def __repr__(self):
        return f'<TurnstileLog {self.id} - {self.direction}>'

