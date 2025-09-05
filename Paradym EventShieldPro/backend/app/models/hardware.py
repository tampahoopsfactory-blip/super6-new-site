from datetime import datetime, timedelta
from sqlalchemy import Column, Integer, String, Boolean, DateTime, Text, ForeignKey, JSON, Enum, Index, Float
from sqlalchemy.orm import relationship, validates
from sqlalchemy.ext.hybrid import hybrid_property
from app import db
import enum

class DeviceType(enum.Enum):
    FACIAL_RECOGNITION_CAMERA = 'facial_recognition_camera'
    TURNSTILE = 'turnstile'
    RFID_READER = 'rfid_reader'
    BIOMETRIC_READER = 'biometric_reader'
    QR_SCANNER = 'qr_scanner'
    GATE = 'gate'
    DOOR_LOCK = 'door_lock'
    ACCESS_CONTROL_PANEL = 'access_control_panel'

class DeviceStatus(enum.Enum):
    ONLINE = 'online'
    OFFLINE = 'offline'
    MAINTENANCE = 'maintenance'
    ERROR = 'error'
    INITIALIZING = 'initializing'
    UPDATING = 'updating'

class DeviceModel(enum.Enum):
    DS_F881 = 'DS-F881'  # Facial recognition camera
    DSN_50P = 'DSN-50P'  # Turnstile
    DS_K1T671M = 'DS-K1T671M'  # Face recognition terminal
    DS_K1T671M_W = 'DS-K1T671M-W'  # Face recognition terminal with WiFi

class CommunicationProtocol(enum.Enum):
    TCP_IP = 'tcp_ip'
    UDP = 'udp'
    SERIAL_RS485 = 'serial_rs485'
    SERIAL_RS232 = 'serial_rs232'
    USB = 'usb'
    BLUETOOTH = 'bluetooth'
    WIFI = 'wifi'
    ETHERNET = 'ethernet'

class HardwareDevice(db.Model):
    __tablename__ = 'hardware_devices'
    
    id = Column(Integer, primary_key=True)
    device_id = Column(String(100), unique=True, nullable=False, index=True)
    access_point_id = Column(Integer, ForeignKey('access_points.id'), nullable=False, index=True)
    
    # Device identification
    name = Column(String(100), nullable=False)
    description = Column(Text, nullable=True)
    device_type = Column(Enum(DeviceType), nullable=False, index=True)
    model = Column(Enum(DeviceModel), nullable=True)
    
    # Hardware specifications
    serial_number = Column(String(100), unique=True, nullable=True, index=True)
    firmware_version = Column(String(50), nullable=True)
    hardware_version = Column(String(50), nullable=True)
    manufacturer = Column(String(100), nullable=True)
    
    # Communication configuration
    communication_protocol = Column(Enum(CommunicationProtocol), nullable=False)
    ip_address = Column(String(45), nullable=True)
    port = Column(Integer, nullable=True)
    mac_address = Column(String(17), nullable=True)
    serial_port = Column(String(50), nullable=True)
    baud_rate = Column(Integer, nullable=True)
    
    # Network configuration
    subnet_mask = Column(String(45), nullable=True)
    gateway = Column(String(45), nullable=True)
    dns_servers = Column(JSON, nullable=True)
    wifi_ssid = Column(String(100), nullable=True)
    wifi_password = Column(String(255), nullable=True)
    
    # Device capabilities
    capabilities = Column(JSON, nullable=True)  # Array of supported features
    supported_formats = Column(JSON, nullable=True)  # Supported data formats
    max_connections = Column(Integer, nullable=True)
    
    # Configuration
    device_config = Column(JSON, nullable=True)  # Device-specific configuration
    access_control_config = Column(JSON, nullable=True)  # Access control settings
    security_config = Column(JSON, nullable=True)  # Security settings
    
    # Status and health
    status = Column(Enum(DeviceStatus), default=DeviceStatus.OFFLINE, nullable=False, index=True)
    last_heartbeat = Column(DateTime, nullable=True)
    last_maintenance = Column(DateTime, nullable=True)
    uptime_seconds = Column(Integer, default=0, nullable=False)
    
    # Performance metrics
    response_time_ms = Column(Integer, nullable=True)
    throughput = Column(Float, nullable=True)
    error_count = Column(Integer, default=0, nullable=False)
    success_count = Column(Integer, default=0, nullable=False)
    
    # Maintenance
    maintenance_required = Column(Boolean, default=False)
    maintenance_schedule = Column(JSON, nullable=True)
    last_calibration = Column(DateTime, nullable=True)
    calibration_due = Column(DateTime, nullable=True)
    
    # Relationships
    access_point = relationship('AccessPoint', back_populates='hardware_devices')
    device_logs = relationship('DeviceLog', back_populates='device')
    device_status = relationship('DeviceStatus', back_populates='device')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_hardware_devices_type_status', 'device_type', 'status'),
        Index('idx_hardware_devices_protocol', 'communication_protocol'),
        Index('idx_hardware_devices_ip', 'ip_address'),
        Index('idx_hardware_devices_serial', 'serial_number'),
    )
    
    @hybrid_property
    def is_online(self):
        """Check if device is online"""
        return self.status == DeviceStatus.ONLINE
    
    @hybrid_property
    def is_offline(self):
        """Check if device is offline"""
        return self.status == DeviceStatus.OFFLINE
    
    @hybrid_property
    def is_maintenance_mode(self):
        """Check if device is in maintenance mode"""
        return self.status == DeviceStatus.MAINTAINANCE
    
    @hybrid_property
    def is_error_state(self):
        """Check if device is in error state"""
        return self.status == DeviceStatus.ERROR
    
    @hybrid_property
    def uptime_hours(self):
        """Get uptime in hours"""
        return self.uptime_seconds / 3600
    
    @hybrid_property
    def uptime_days(self):
        """Get uptime in days"""
        return self.uptime_seconds / 86400
    
    @hybrid_property
    def success_rate(self):
        """Calculate success rate percentage"""
        total_operations = self.success_count + self.error_count
        if total_operations > 0:
            return (self.success_count / total_operations) * 100
        return 0
    
    @hybrid_property
    def needs_maintenance(self):
        """Check if device needs maintenance"""
        if self.maintenance_required:
            return True
        if self.last_maintenance and self.maintenance_schedule:
            # Check if maintenance is due based on schedule
            maintenance_interval = self.maintenance_schedule.get('interval_days', 30)
            last_maintenance = self.last_maintenance
            maintenance_due = last_maintenance + timedelta(days=maintenance_interval)
            return datetime.utcnow() > maintenance_due
        return False
    
    @hybrid_property
    def needs_calibration(self):
        """Check if device needs calibration"""
        if self.calibration_due:
            return datetime.utcnow() > self.calibration_due
        return False
    
    def update_status(self, status, heartbeat=True):
        """Update device status"""
        self.status = status
        if heartbeat:
            self.last_heartbeat = datetime.utcnow()
            if status == DeviceStatus.ONLINE:
                self.uptime_seconds += 1
    
    def increment_success(self):
        """Increment success count"""
        self.success_count += 1
    
    def increment_error(self):
        """Increment error count"""
        self.error_count += 1
    
    def update_performance_metrics(self, response_time=None, throughput=None):
        """Update performance metrics"""
        if response_time is not None:
            self.response_time_ms = response_time
        if throughput is not None:
            self.throughput = throughput
    
    def mark_maintenance_complete(self):
        """Mark maintenance as complete"""
        self.maintenance_required = False
        self.last_maintenance = datetime.utcnow()
        self.status = DeviceStatus.ONLINE
    
    def mark_calibration_complete(self):
        """Mark calibration as complete"""
        self.last_calibration = datetime.utcnow()
        if self.calibration_due:
            # Set next calibration due date
            calibration_interval = self.device_config.get('calibration_interval_days', 90)
            self.calibration_due = datetime.utcnow() + timedelta(days=calibration_interval)
    
    def get_connection_string(self):
        """Get connection string for the device"""
        if self.communication_protocol == CommunicationProtocol.TCP_IP:
            return f"tcp://{self.ip_address}:{self.port}"
        elif self.communication_protocol == CommunicationProtocol.SERIAL_RS485:
            return f"serial://{self.serial_port}:{self.baud_rate}"
        elif self.communication_protocol == CommunicationProtocol.USB:
            return f"usb://{self.serial_number}"
        else:
            return f"{self.communication_protocol.value}://{self.device_id}"
    
    def __repr__(self):
        return f'<HardwareDevice {self.device_id} - {self.device_type.value}>'

class DeviceLog(db.Model):
    __tablename__ = 'device_logs'
    
    id = Column(Integer, primary_key=True)
    device_id = Column(Integer, ForeignKey('hardware_devices.id'), nullable=False, index=True)
    
    # Log details
    log_level = Column(String(20), nullable=False, index=True)  # info, warning, error, debug
    log_type = Column(String(50), nullable=False, index=True)  # system, access, maintenance, error
    message = Column(Text, nullable=False)
    
    # Context
    user_id = Column(Integer, ForeignKey('users.id'), nullable=True, index=True)
    event_id = Column(Integer, ForeignKey('events.id'), nullable=True, index=True)
    access_log_id = Column(Integer, ForeignKey('access_logs.id'), nullable=True, index=True)
    
    # Device state
    device_status = Column(String(50), nullable=True)
    device_temperature = Column(Float, nullable=True)
    device_memory_usage = Column(Float, nullable=True)
    device_cpu_usage = Column(Float, nullable=True)
    
    # Network information
    ip_address = Column(String(45), nullable=True)
    response_time_ms = Column(Integer, nullable=True)
    
    # Error details
    error_code = Column(String(100), nullable=True)
    error_details = Column(JSON, nullable=True)
    stack_trace = Column(Text, nullable=True)
    
    # Metadata
    metadata = Column(JSON, nullable=True)
    
    # Relationships
    device = relationship('HardwareDevice', back_populates='device_logs')
    user = relationship('User')
    event = relationship('Event')
    access_log = relationship('AccessLog')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_device_logs_device_time', 'device_id', 'created_at'),
        Index('idx_device_logs_level_type', 'log_level', 'log_type'),
        Index('idx_device_logs_user_time', 'user_id', 'created_at'),
        Index('idx_device_logs_event_time', 'event_id', 'created_at'),
    )
    
    @hybrid_property
    def is_error(self):
        """Check if log is an error"""
        return self.log_level == 'error'
    
    @hybrid_property
    def is_warning(self):
        """Check if log is a warning"""
        return self.log_level == 'warning'
    
    @hybrid_property
    def is_info(self):
        """Check if log is info"""
        return self.log_level == 'info'
    
    @hybrid_property
    def response_time_seconds(self):
        """Get response time in seconds"""
        if self.response_time_ms:
            return self.response_time_ms / 1000.0
        return None
    
    def __repr__(self):
        return f'<DeviceLog {self.device_id} - {self.log_level} {self.log_type}>'

class DeviceStatus(db.Model):
    __tablename__ = 'device_status'
    
    id = Column(Integer, primary_key=True)
    device_id = Column(Integer, ForeignKey('hardware_devices.id'), nullable=False, index=True)
    access_point_id = Column(Integer, ForeignKey('access_points.id'), nullable=False, index=True)
    
    # Status information
    status = Column(Enum(DeviceStatus), nullable=False, index=True)
    status_message = Column(Text, nullable=True)
    
    # Health metrics
    temperature = Column(Float, nullable=True)
    humidity = Column(Float, nullable=True)
    memory_usage_percent = Column(Float, nullable=True)
    cpu_usage_percent = Column(Float, nullable=True)
    disk_usage_percent = Column(Float, nullable=True)
    
    # Network metrics
    network_latency_ms = Column(Integer, nullable=True)
    network_throughput_mbps = Column(Float, nullable=True)
    connection_count = Column(Integer, default=0, nullable=False)
    
    # Performance metrics
    response_time_ms = Column(Integer, nullable=True)
    operations_per_second = Column(Float, nullable=True)
    error_rate_percent = Column(Float, nullable=True)
    
    # Maintenance information
    last_maintenance = Column(DateTime, nullable=True)
    maintenance_due = Column(DateTime, nullable=True)
    maintenance_notes = Column(Text, nullable=True)
    
    # Alerts
    active_alerts = Column(JSON, nullable=True)  # Array of active alerts
    alert_count = Column(Integer, default=0, nullable=False)
    
    # Relationships
    device = relationship('HardwareDevice', back_populates='device_status')
    access_point = relationship('AccessPoint', back_populates='device_status')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_device_status_device', 'device_id'),
        Index('idx_device_status_access_point', 'access_point_id'),
        Index('idx_device_status_status', 'status'),
        Index('idx_device_status_updated', 'updated_at'),
    )
    
    @hybrid_property
    def is_healthy(self):
        """Check if device is healthy"""
        if self.temperature and self.temperature > 80:  # Over 80°C
            return False
        if self.memory_usage_percent and self.memory_usage_percent > 90:
            return False
        if self.cpu_usage_percent and self.cpu_usage_percent > 95:
            return False
        if self.disk_usage_percent and self.disk_usage_percent > 95:
            return False
        if self.error_rate_percent and self.error_rate_percent > 5:
            return False
        return True
    
    @hybrid_property
    def needs_attention(self):
        """Check if device needs attention"""
        if not self.is_healthy:
            return True
        if self.maintenance_due and datetime.utcnow() > self.maintenance_due:
            return True
        if self.alert_count > 0:
            return True
        return False
    
    @hybrid_property
    def health_score(self):
        """Calculate health score (0-100)"""
        score = 100
        
        # Temperature penalty
        if self.temperature and self.temperature > 60:
            temp_penalty = min(30, (self.temperature - 60) * 2)
            score -= temp_penalty
        
        # Memory usage penalty
        if self.memory_usage_percent and self.memory_usage_percent > 80:
            mem_penalty = min(20, (self.memory_usage_percent - 80) * 2)
            score -= mem_penalty
        
        # CPU usage penalty
        if self.cpu_usage_percent and self.cpu_usage_percent > 90:
            cpu_penalty = min(20, (self.cpu_usage_percent - 90) * 2)
            score -= cpu_penalty
        
        # Error rate penalty
        if self.error_rate_percent and self.error_rate_percent > 1:
            error_penalty = min(30, self.error_rate_percent * 10)
            score -= error_penalty
        
        return max(0, score)
    
    def update_health_metrics(self, **kwargs):
        """Update health metrics"""
        for key, value in kwargs.items():
            if hasattr(self, key):
                setattr(self, key, value)
        self.updated_at = datetime.utcnow()
    
    def add_alert(self, alert_type, message, severity='warning'):
        """Add an alert"""
        if not self.active_alerts:
            self.active_alerts = []
        
        alert = {
            'type': alert_type,
            'message': message,
            'severity': severity,
            'created_at': datetime.utcnow().isoformat()
        }
        
        self.active_alerts.append(alert)
        self.alert_count = len(self.active_alerts)
    
    def clear_alerts(self, alert_type=None):
        """Clear alerts"""
        if alert_type:
            self.active_alerts = [a for a in self.active_alerts if a['type'] != alert_type]
        else:
            self.active_alerts = []
        self.alert_count = len(self.active_alerts)
    
    def __repr__(self):
        return f'<DeviceStatus {self.device_id} - {self.status.value}>'

class FacialRecognitionDevice(db.Model):
    __tablename__ = 'facial_recognition_devices'
    
    id = Column(Integer, primary_key=True)
    device_id = Column(Integer, ForeignKey('hardware_devices.id'), nullable=False, unique=True, index=True)
    
    # Facial recognition specific configuration
    recognition_threshold = Column(Float, default=0.8, nullable=False)  # Confidence threshold
    max_faces_per_image = Column(Integer, default=10, nullable=False)
    face_detection_mode = Column(String(50), default='fast', nullable=False)  # fast, accurate, balanced
    
    # Camera settings
    resolution_width = Column(Integer, default=1920, nullable=False)
    resolution_height = Column(Integer, default=1080, nullable=False)
    frame_rate = Column(Integer, default=30, nullable=False)
    exposure_mode = Column(String(50), default='auto', nullable=False)
    white_balance = Column(String(50), default='auto', nullable=False)
    
    # Recognition settings
    liveness_detection = Column(Boolean, default=True, nullable=False)
    anti_spoofing = Column(Boolean, default=True, nullable=False)
    face_quality_threshold = Column(Float, default=0.6, nullable=False)
    
    # Database settings
    max_face_templates = Column(Integer, default=10000, nullable=False)
    current_face_templates = Column(Integer, default=0, nullable=False)
    template_quality_threshold = Column(Float, default=0.7, nullable=False)
    
    # Performance settings
    recognition_timeout_ms = Column(Integer, default=5000, nullable=False)
    batch_processing_enabled = Column(Boolean, default=False, nullable=False)
    batch_size = Column(Integer, default=10, nullable=False)
    
    # Security settings
    encryption_enabled = Column(Boolean, default=True, nullable=False)
    secure_communication = Column(Boolean, default=True, nullable=False)
    audit_logging_enabled = Column(Boolean, default=True, nullable=False)
    
    # Relationships
    device = relationship('HardwareDevice', back_populates='facial_recognition_config')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_facial_recognition_devices_threshold', 'recognition_threshold'),
        Index('idx_facial_recognition_devices_templates', 'current_face_templates'),
    )
    
    @hybrid_property
    def is_at_capacity(self):
        """Check if device is at face template capacity"""
        return self.current_face_templates >= self.max_face_templates
    
    @hybrid_property
    def available_capacity(self):
        """Get available face template capacity"""
        return max(0, self.max_face_templates - self.current_face_templates)
    
    @hybrid_property
    def capacity_percentage(self):
        """Get capacity usage percentage"""
        return (self.current_face_templates / self.max_face_templates) * 100
    
    def add_face_template(self, count=1):
        """Add face templates"""
        if self.current_face_templates + count > self.max_face_templates:
            raise ValueError("Face template capacity exceeded")
        self.current_face_templates += count
    
    def remove_face_template(self, count=1):
        """Remove face templates"""
        self.current_face_templates = max(0, self.current_face_templates - count)
    
    def __repr__(self):
        return f'<FacialRecognitionDevice {self.device_id} - {self.recognition_threshold}>'

class TurnstileDevice(db.Model):
    __tablename__ = 'turnstile_devices'
    
    id = Column(Integer, primary_key=True)
    device_id = Column(Integer, ForeignKey('hardware_devices.id'), nullable=False, unique=True, index=True)
    
    # Turnstile specific configuration
    turnstile_type = Column(String(50), default='tripod', nullable=False)  # tripod, full_height, optical
    direction_control = Column(String(50), default='bidirectional', nullable=False)  # entry_only, exit_only, bidirectional
    access_mode = Column(String(50), default='controlled', nullable=False)  # free, controlled, emergency
    
    # Mechanical settings
    rotation_speed = Column(Float, default=1.0, nullable=False)  # rotations per second
    rotation_angle = Column(Integer, default=90, nullable=False)  # degrees
    motor_power = Column(Float, default=100.0, nullable=False)  # percentage
    sensor_sensitivity = Column(Float, default=0.8, nullable=False)  # 0.0 to 1.0
    
    # Safety settings
    emergency_override_enabled = Column(Boolean, default=True, nullable=False)
    obstruction_detection = Column(Boolean, default=True, nullable=False)
    anti_tailgating = Column(Boolean, default=True, nullable=False)
    anti_panic = Column(Boolean, default=True, nullable=False)
    
    # Access control settings
    max_occupancy = Column(Integer, default=1, nullable=False)
    current_occupancy = Column(Integer, default=0, nullable=False)
    access_timeout_seconds = Column(Integer, default=10, nullable=False)
    reentry_delay_seconds = Column(Integer, default=5, nullable=False)
    
    # Performance settings
    cycle_time_ms = Column(Integer, default=1000, nullable=False)
    max_throughput_per_hour = Column(Integer, default=3600, nullable=False)
    current_throughput_per_hour = Column(Integer, default=0, nullable=False)
    
    # Maintenance settings
    maintenance_interval_hours = Column(Integer, default=720, nullable=False)  # 30 days
    last_maintenance = Column(DateTime, nullable=True)
    maintenance_due = Column(DateTime, nullable=True)
    
    # Relationships
    device = relationship('HardwareDevice', back_populates='turnstile_config')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_turnstile_devices_type', 'turnstile_type'),
        Index('idx_turnstile_devices_direction', 'direction_control'),
        Index('idx_turnstile_devices_occupancy', 'current_occupancy'),
    )
    
    @hybrid_property
    def is_occupied(self):
        """Check if turnstile is occupied"""
        return self.current_occupancy > 0
    
    @hybrid_property
    def is_available(self):
        """Check if turnstile is available for use"""
        return self.current_occupancy < self.max_occupancy
    
    @hybrid_property
    def throughput_percentage(self):
        """Get current throughput as percentage of maximum"""
        if self.max_throughput_per_hour > 0:
            return (self.current_throughput_per_hour / self.max_throughput_per_hour) * 100
        return 0
    
    @hybrid_property
    def needs_maintenance(self):
        """Check if turnstile needs maintenance"""
        if self.maintenance_due:
            return datetime.utcnow() > self.maintenance_due
        return False
    
    def increment_occupancy(self, count=1):
        """Increment current occupancy"""
        if self.current_occupancy + count > self.max_occupancy:
            raise ValueError("Turnstile occupancy limit exceeded")
        self.current_occupancy += count
    
    def decrement_occupancy(self, count=1):
        """Decrement current occupancy"""
        self.current_occupancy = max(0, self.current_occupancy - count)
    
    def increment_throughput(self, count=1):
        """Increment hourly throughput"""
        self.current_throughput_per_hour += count
    
    def reset_hourly_throughput(self):
        """Reset hourly throughput counter"""
        self.current_throughput_per_hour = 0
    
    def mark_maintenance_complete(self):
        """Mark maintenance as complete"""
        self.last_maintenance = datetime.utcnow()
        self.maintenance_due = datetime.utcnow() + timedelta(hours=self.maintenance_interval_hours)
    
    def __repr__(self):
        return f'<TurnstileDevice {self.device_id} - {self.turnstile_type}>'

