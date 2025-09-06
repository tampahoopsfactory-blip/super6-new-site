"""
EventShield Pro - Configuration Management
Environment-based configuration for cloud and standalone deployments
"""

import os
from typing import Dict, Any, Optional, List
from pathlib import Path
from dataclasses import dataclass, field
from enum import Enum
import yaml
import json


class DeploymentMode(Enum):
    """Deployment mode enumeration"""
    CLOUD = "cloud"
    STANDALONE = "standalone"
    HYBRID = "hybrid"


class Environment(Enum):
    """Environment enumeration"""
    DEVELOPMENT = "development"
    STAGING = "staging"
    PRODUCTION = "production"


@dataclass
class DatabaseConfig:
    """Database configuration"""
    url: str
    pool_size: int = 10
    max_overflow: int = 20
    pool_timeout: int = 30
    pool_recycle: int = 3600
    echo: bool = False


@dataclass
class RedisConfig:
    """Redis configuration"""
    url: str
    host: str = "localhost"
    port: int = 6379
    db: int = 0
    password: Optional[str] = None
    max_connections: int = 20


@dataclass
class DeviceConfig:
    """Device communication configuration"""
    default_tcp_port: int = 8000
    default_udp_port: int = 8101
    connection_timeout: int = 5000
    retry_count: int = 3
    keep_alive_interval: int = 30
    max_concurrent_connections: int = 100


@dataclass
class BiometricConfig:
    """Biometric processing configuration"""
    face_recognition_threshold: float = 0.6
    palm_recognition_threshold: float = 0.6
    max_image_size: int = 5 * 1024 * 1024  # 5MB
    supported_formats: List[str] = field(default_factory=lambda: ["jpg", "jpeg", "png", "bmp"])
    quality_threshold: float = 0.4
    max_biometric_data_size: int = 10 * 1024 * 1024  # 10MB


@dataclass
class SecurityConfig:
    """Security configuration"""
    secret_key: str
    jwt_expiration: int = 3600  # 1 hour
    password_min_length: int = 8
    max_login_attempts: int = 5
    lockout_duration: int = 900  # 15 minutes
    encryption_key: Optional[str] = None
    enable_2fa: bool = False
    session_timeout: int = 1800  # 30 minutes


@dataclass
class LoggingConfig:
    """Logging configuration"""
    level: str = "INFO"
    enable_console: bool = True
    enable_file: bool = True
    enable_database: bool = False
    enable_remote: bool = False
    log_directory: str = "logs"
    max_file_size: int = 10 * 1024 * 1024  # 10MB
    backup_count: int = 5
    remote_endpoint: Optional[str] = None


@dataclass
class MonitoringConfig:
    """Monitoring and metrics configuration"""
    enable_metrics: bool = True
    metrics_port: int = 9090
    health_check_interval: int = 30
    performance_monitoring: bool = True
    error_tracking: bool = True
    alert_webhook: Optional[str] = None


@dataclass
class LicenseConfig:
    """License management configuration"""
    license_server_url: Optional[str] = None
    offline_mode: bool = False
    license_check_interval: int = 3600  # 1 hour
    grace_period: int = 7 * 24 * 3600  # 7 days


@dataclass
class UpdateConfig:
    """Update management configuration"""
    update_server_url: Optional[str] = None
    auto_update: bool = False
    update_check_interval: int = 24 * 3600  # 24 hours
    rollback_enabled: bool = True
    backup_before_update: bool = True


@dataclass
class SMTPConfig:
    """SMTP configuration for email notifications"""
    host: str = "smtp.gmail.com"
    port: int = 587
    username: Optional[str] = None
    password: Optional[str] = None
    use_tls: bool = True
    use_ssl: bool = False


@dataclass
class EventShieldConfig:
    """Main EventShield Pro configuration"""
    
    # Core settings
    deployment_mode: DeploymentMode = DeploymentMode.STANDALONE
    environment: Environment = Environment.DEVELOPMENT
    debug: bool = False
    host: str = "0.0.0.0"
    port: int = 5000
    
    # Multi-tenancy
    multi_tenant: bool = False
    default_tenant_id: Optional[str] = None
    
    # Component configurations
    database: DatabaseConfig = field(default_factory=DatabaseConfig)
    redis: RedisConfig = field(default_factory=RedisConfig)
    device: DeviceConfig = field(default_factory=DeviceConfig)
    biometric: BiometricConfig = field(default_factory=BiometricConfig)
    security: SecurityConfig = field(default_factory=SecurityConfig)
    logging: LoggingConfig = field(default_factory=LoggingConfig)
    monitoring: MonitoringConfig = field(default_factory=MonitoringConfig)
    license: LicenseConfig = field(default_factory=LicenseConfig)
    update: UpdateConfig = field(default_factory=UpdateConfig)
    smtp: SMTPConfig = field(default_factory=SMTPConfig)
    
    # Feature flags
    features: Dict[str, bool] = field(default_factory=lambda: {
        "device_management": True,
        "biometric_processing": True,
        "sms_alerts": True,
        "email_notifications": True,
        "real_time_monitoring": True,
        "audit_logging": True,
        "multi_tenant": False,
        "cloud_deployment": False,
        "offline_mode": False
    })
    
    # API settings
    api_version: str = "v1"
    cors_origins: List[str] = field(default_factory=lambda: ["*"])
    rate_limit: int = 1000  # requests per hour
    
    # File upload settings
    max_file_size: int = 10 * 1024 * 1024  # 10MB
    allowed_extensions: List[str] = field(default_factory=lambda: [
        "jpg", "jpeg", "png", "bmp", "gif", "pdf", "doc", "docx"
    ])
    
    # Backup settings
    backup_enabled: bool = True
    backup_interval: int = 24 * 3600  # 24 hours
    backup_retention: int = 30  # days
    backup_location: str = "backups"


class ConfigManager:
    """Configuration manager for EventShield Pro"""
    
    def __init__(self, config_file: Optional[str] = None):
        self.config_file = config_file
        self.config = self._load_configuration()
    
    def _load_configuration(self) -> EventShieldConfig:
        """Load configuration from environment variables and config files"""
        
        # Start with default configuration
        config = EventShieldConfig()
        
        # Load from config file if provided
        if self.config_file and Path(self.config_file).exists():
            config = self._load_from_file(self.config_file)
        
        # Override with environment variables
        config = self._load_from_environment(config)
        
        # Validate configuration
        self._validate_configuration(config)
        
        return config
    
    def _load_from_file(self, config_file: str) -> EventShieldConfig:
        """Load configuration from YAML or JSON file"""
        config_path = Path(config_file)
        
        if config_path.suffix.lower() in ['.yaml', '.yml']:
            with open(config_path, 'r') as f:
                data = yaml.safe_load(f)
        elif config_path.suffix.lower() == '.json':
            with open(config_path, 'r') as f:
                data = json.load(f)
        else:
            raise ValueError(f"Unsupported config file format: {config_path.suffix}")
        
        # Convert to EventShieldConfig
        return self._dict_to_config(data)
    
    def _load_from_environment(self, config: EventShieldConfig) -> EventShieldConfig:
        """Load configuration from environment variables"""
        
        # Core settings
        config.deployment_mode = DeploymentMode(
            os.getenv('DEPLOYMENT_MODE', config.deployment_mode.value)
        )
        config.environment = Environment(
            os.getenv('ENVIRONMENT', config.environment.value)
        )
        config.debug = os.getenv('DEBUG', 'false').lower() == 'true'
        config.host = os.getenv('HOST', config.host)
        config.port = int(os.getenv('PORT', config.port))
        
        # Multi-tenancy
        config.multi_tenant = os.getenv('MULTI_TENANT', 'false').lower() == 'true'
        config.default_tenant_id = os.getenv('DEFAULT_TENANT_ID')
        
        # Database
        config.database.url = os.getenv('DATABASE_URL', config.database.url)
        config.database.pool_size = int(os.getenv('DB_POOL_SIZE', config.database.pool_size))
        config.database.echo = os.getenv('DB_ECHO', 'false').lower() == 'true'
        
        # Redis
        config.redis.url = os.getenv('REDIS_URL', config.redis.url)
        config.redis.host = os.getenv('REDIS_HOST', config.redis.host)
        config.redis.port = int(os.getenv('REDIS_PORT', config.redis.port))
        config.redis.password = os.getenv('REDIS_PASSWORD')
        
        # Security
        config.security.secret_key = os.getenv('SECRET_KEY', config.security.secret_key)
        config.security.encryption_key = os.getenv('ENCRYPTION_KEY')
        
        # Device communication
        config.device.default_tcp_port = int(os.getenv('DEVICE_TCP_PORT', config.device.default_tcp_port))
        config.device.default_udp_port = int(os.getenv('DEVICE_UDP_PORT', config.device.default_udp_port))
        config.device.connection_timeout = int(os.getenv('DEVICE_TIMEOUT', config.device.connection_timeout))
        
        # Biometric processing
        config.biometric.face_recognition_threshold = float(
            os.getenv('FACE_THRESHOLD', config.biometric.face_recognition_threshold)
        )
        config.biometric.palm_recognition_threshold = float(
            os.getenv('PALM_THRESHOLD', config.biometric.palm_recognition_threshold)
        )
        
        # Logging
        config.logging.level = os.getenv('LOG_LEVEL', config.logging.level)
        config.logging.enable_console = os.getenv('LOG_CONSOLE', 'true').lower() == 'true'
        config.logging.enable_file = os.getenv('LOG_FILE', 'true').lower() == 'true'
        config.logging.enable_remote = os.getenv('LOG_REMOTE', 'false').lower() == 'true'
        
        # Monitoring
        config.monitoring.enable_metrics = os.getenv('ENABLE_METRICS', 'true').lower() == 'true'
        config.monitoring.metrics_port = int(os.getenv('METRICS_PORT', config.monitoring.metrics_port))
        
        # License management
        config.license.license_server_url = os.getenv('LICENSE_SERVER_URL')
        config.license.offline_mode = os.getenv('LICENSE_OFFLINE', 'false').lower() == 'true'
        
        # Update management
        config.update.update_server_url = os.getenv('UPDATE_SERVER_URL')
        config.update.auto_update = os.getenv('AUTO_UPDATE', 'false').lower() == 'true'
        
        # SMTP
        config.smtp.host = os.getenv('SMTP_HOST', config.smtp.host)
        config.smtp.port = int(os.getenv('SMTP_PORT', config.smtp.port))
        config.smtp.username = os.getenv('SMTP_USERNAME')
        config.smtp.password = os.getenv('SMTP_PASSWORD')
        
        # Feature flags
        for feature in config.features:
            env_key = f"FEATURE_{feature.upper()}"
            if os.getenv(env_key):
                config.features[feature] = os.getenv(env_key).lower() == 'true'
        
        return config
    
    def _dict_to_config(self, data: Dict[str, Any]) -> EventShieldConfig:
        """Convert dictionary to EventShieldConfig"""
        # This is a simplified implementation
        # In a real implementation, you'd use a more sophisticated mapping
        config = EventShieldConfig()
        
        # Map dictionary keys to config attributes
        for key, value in data.items():
            if hasattr(config, key):
                setattr(config, key, value)
        
        return config
    
    def _validate_configuration(self, config: EventShieldConfig):
        """Validate configuration settings"""
        
        # Validate required settings
        if not config.security.secret_key:
            raise ValueError("SECRET_KEY is required")
        
        if config.multi_tenant and not config.default_tenant_id:
            raise ValueError("DEFAULT_TENANT_ID is required for multi-tenant mode")
        
        if config.database.url and not config.database.url.startswith(('sqlite://', 'postgresql://', 'mysql://')):
            raise ValueError("Invalid database URL format")
        
        # Validate port numbers
        if not (1 <= config.port <= 65535):
            raise ValueError("Port must be between 1 and 65535")
        
        if not (1 <= config.device.default_tcp_port <= 65535):
            raise ValueError("Device TCP port must be between 1 and 65535")
        
        if not (1 <= config.device.default_udp_port <= 65535):
            raise ValueError("Device UDP port must be between 1 and 65535")
        
        # Validate thresholds
        if not (0.0 <= config.biometric.face_recognition_threshold <= 1.0):
            raise ValueError("Face recognition threshold must be between 0.0 and 1.0")
        
        if not (0.0 <= config.biometric.palm_recognition_threshold <= 1.0):
            raise ValueError("Palm recognition threshold must be between 0.0 and 1.0")
    
    def get_config(self) -> EventShieldConfig:
        """Get current configuration"""
        return self.config
    
    def update_config(self, updates: Dict[str, Any]):
        """Update configuration with new values"""
        for key, value in updates.items():
            if hasattr(self.config, key):
                setattr(self.config, key, value)
    
    def save_config(self, file_path: str):
        """Save current configuration to file"""
        config_dict = self._config_to_dict(self.config)
        
        config_path = Path(file_path)
        if config_path.suffix.lower() in ['.yaml', '.yml']:
            with open(config_path, 'w') as f:
                yaml.dump(config_dict, f, default_flow_style=False)
        elif config_path.suffix.lower() == '.json':
            with open(config_path, 'w') as f:
                json.dump(config_dict, f, indent=2)
        else:
            raise ValueError(f"Unsupported config file format: {config_path.suffix}")
    
    def _config_to_dict(self, config: EventShieldConfig) -> Dict[str, Any]:
        """Convert EventShieldConfig to dictionary"""
        # This is a simplified implementation
        # In a real implementation, you'd use a more sophisticated conversion
        result = {}
        for key, value in config.__dict__.items():
            if isinstance(value, (DatabaseConfig, RedisConfig, DeviceConfig, 
                                BiometricConfig, SecurityConfig, LoggingConfig,
                                MonitoringConfig, LicenseConfig, UpdateConfig, SMTPConfig)):
                result[key] = value.__dict__
            else:
                result[key] = value
        return result
    
    def is_feature_enabled(self, feature: str) -> bool:
        """Check if a feature is enabled"""
        return self.config.features.get(feature, False)
    
    def get_database_url(self) -> str:
        """Get database URL"""
        return self.config.database.url
    
    def get_redis_url(self) -> str:
        """Get Redis URL"""
        return self.config.redis.url
    
    def is_cloud_deployment(self) -> bool:
        """Check if this is a cloud deployment"""
        return self.config.deployment_mode == DeploymentMode.CLOUD
    
    def is_standalone_deployment(self) -> bool:
        """Check if this is a standalone deployment"""
        return self.config.deployment_mode == DeploymentMode.STANDALONE
    
    def is_multi_tenant(self) -> bool:
        """Check if multi-tenancy is enabled"""
        return self.config.multi_tenant


# Global configuration manager
config_manager = ConfigManager()

# Convenience function to get config
def get_config() -> EventShieldConfig:
    """Get current configuration"""
    return config_manager.get_config()
