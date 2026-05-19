"""
EventShield Pro - Configuration Settings
Environment-specific configuration classes
"""

import os
from datetime import timedelta

class Config:
    """Base configuration class"""
    
    # Application
    APP_NAME = os.getenv('APP_NAME', 'EventShield Pro')
    APP_ENV = os.getenv('APP_ENV', 'development')
    DEBUG = os.getenv('APP_DEBUG', 'false').lower() == 'true'
    SECRET_KEY = os.getenv('APP_SECRET_KEY', 'dev-secret-key-change-in-production')
    
    # Database
    SQLALCHEMY_DATABASE_URI = os.getenv('DATABASE_URL') or \
        f"mssql+pyodbc://{os.getenv('DB_USER', 'sa')}:{os.getenv('DB_PASSWORD', 'EventShieldPro2024!')}@{os.getenv('DB_HOST', 'localhost')}:{os.getenv('DB_PORT', '1433')}/{os.getenv('DB_NAME', 'eventshield_pro')}?driver={os.getenv('DB_DRIVER', 'ODBC+Driver+17+for+SQL+Server')}"
    
    SQLALCHEMY_TRACK_MODIFICATIONS = False
    SQLALCHEMY_ENGINE_OPTIONS = {
        'pool_size': int(os.getenv('DB_POOL_SIZE', 10)),
        'max_overflow': int(os.getenv('DB_MAX_OVERFLOW', 20)),
        'pool_timeout': int(os.getenv('DB_CONNECTION_TIMEOUT', 30)),
        'pool_pre_ping': True
    }
    
    # Redis
    REDIS_URL = f"redis://{os.getenv('REDIS_HOST', 'localhost')}:{os.getenv('REDIS_PORT', '6379')}/{os.getenv('REDIS_DB', '0')}"
    
    # JWT
    JWT_SECRET_KEY = os.getenv('JWT_SECRET_KEY', 'jwt-secret-key-change-in-production')
    JWT_ACCESS_TOKEN_EXPIRES = int(os.getenv('JWT_ACCESS_TOKEN_EXPIRES', 3600))
    JWT_REFRESH_TOKEN_EXPIRES = int(os.getenv('JWT_REFRESH_TOKEN_EXPIRES', 604800))
    JWT_ALGORITHM = os.getenv('JWT_ALGORITHM', 'HS256')
    
    # CORS
    CORS_ORIGINS = os.getenv('CORS_ORIGINS', 'http://localhost:3000').split(',')
    CORS_METHODS = os.getenv('CORS_METHODS', 'GET,POST,PUT,DELETE,OPTIONS').split(',')
    CORS_ALLOW_HEADERS = os.getenv('CORS_ALLOW_HEADERS', 'Content-Type,Authorization,X-Requested-With').split(',')
    
    # Rate Limiting
    RATELIMIT_DEFAULT = os.getenv('RATE_LIMIT_DEFAULT', '100 per minute')
    RATELIMIT_STORAGE_URL = REDIS_URL
    
    # Caching
    CACHE_TYPE = 'redis'
    CACHE_REDIS_URL = REDIS_URL
    CACHE_DEFAULT_TIMEOUT = 300
    
    # File Upload
    MAX_CONTENT_LENGTH = 16 * 1024 * 1024  # 16MB
    UPLOAD_FOLDER = 'uploads'
    
    # Azure Storage
    AZURE_STORAGE_CONNECTION_STRING = os.getenv('AZURE_STORAGE_CONNECTION_STRING')
    AZURE_STORAGE_ACCOUNT = os.getenv('AZURE_STORAGE_ACCOUNT')
    AZURE_STORAGE_KEY = os.getenv('AZURE_STORAGE_KEY')
    AZURE_STORAGE_CONTAINER = os.getenv('AZURE_STORAGE_CONTAINER', 'eventshield-pro')
    
    # Stripe
    STRIPE_PUBLISHABLE_KEY = os.getenv('STRIPE_PUBLISHABLE_KEY', '')
    STRIPE_SECRET_KEY = os.getenv('STRIPE_SECRET_KEY', '')
    STRIPE_WEBHOOK_SECRET = os.getenv('STRIPE_WEBHOOK_SECRET', '')
    STRIPE_PRICE_STANDARD = os.getenv('STRIPE_PRICE_STANDARD', '')
    STRIPE_PRICE_PREMIUM = os.getenv('STRIPE_PRICE_PREMIUM', '')
    STRIPE_PRICE_ENTERPRISE = os.getenv('STRIPE_PRICE_ENTERPRISE', '')
    
    # Email
    SENDGRID_API_KEY = os.getenv('SENDGRID_API_KEY')
    SENDGRID_FROM_EMAIL = os.getenv('SENDGRID_FROM_EMAIL', 'noreply@eventshieldpro.com')
    SENDGRID_FROM_NAME = os.getenv('SENDGRID_FROM_NAME', 'EventShield Pro')
    
    # Hardware Integration
    FACIAL_RECOGNITION_ENABLED = os.getenv('FACIAL_RECOGNITION_ENABLED', 'false').lower() == 'true'
    FACIAL_RECOGNITION_IP = os.getenv('FACIAL_RECOGNITION_IP', '192.168.1.100')
    FACIAL_RECOGNITION_PORT = int(os.getenv('FACIAL_RECOGNITION_PORT', '8000'))
    
    TURNSTILE_ENABLED = os.getenv('TURNSTILE_ENABLED', 'false').lower() == 'true'
    TURNSTILE_SERIAL_PORT = os.getenv('TURNSTILE_SERIAL_PORT', '/dev/ttyUSB0')
    
    # Logging
    LOG_LEVEL = os.getenv('LOG_LEVEL', 'INFO')
    LOG_DIR = 'logs'
    LOG_FILE = 'app.log'
    LOG_TO_FILE = True
    
    # Security
    BCRYPT_LOG_ROUNDS = int(os.getenv('BCRYPT_LOG_ROUNDS', 12))
    PASSWORD_MIN_LENGTH = int(os.getenv('PASSWORD_MIN_LENGTH', 8))
    
    # Business Configuration
    TRIAL_DURATION_DAYS = int(os.getenv('TRIAL_DURATION_DAYS', 14))
    TRIAL_EVENT_LIMIT = int(os.getenv('TRIAL_EVENT_LIMIT', 5))
    TRIAL_ATTENDEE_LIMIT = int(os.getenv('TRIAL_ATTENDEE_LIMIT', 100))
    
    STANDARD_EVENT_LIMIT = int(os.getenv('STANDARD_EVENT_LIMIT', 50))
    STANDARD_ATTENDEE_LIMIT = int(os.getenv('STANDARD_ATTENDEE_LIMIT', 1000))
    
    PREMIUM_EVENT_LIMIT = int(os.getenv('PREMIUM_EVENT_LIMIT', 200))
    PREMIUM_ATTENDEE_LIMIT = int(os.getenv('PREMIUM_ATTENDEE_LIMIT', 10000))
    
    # Feature Flags
    FEATURE_FACIAL_RECOGNITION = os.getenv('FEATURE_FACIAL_RECOGNITION', 'true').lower() == 'true'
    FEATURE_TURNSTILE_INTEGRATION = os.getenv('FEATURE_TURNSTILE_INTEGRATION', 'true').lower() == 'true'
    FEATURE_OFFLINE_MODE = os.getenv('FEATURE_OFFLINE_MODE', 'true').lower() == 'true'
    FEATURE_WHITE_LABEL = os.getenv('FEATURE_WHITE_LABEL', 'true').lower() == 'true'
    FEATURE_ADVANCED_ANALYTICS = os.getenv('FEATURE_ADVANCED_ANALYTICS', 'true').lower() == 'true'
    FEATURE_MULTI_TENANT = os.getenv('FEATURE_MULTI_TENANT', 'true').lower() == 'true'

class DevelopmentConfig(Config):
    """Development environment configuration"""

    DEBUG = True
    SQLALCHEMY_ECHO = True

    # Development-specific settings
    CORS_ORIGINS = ['http://localhost:3000', 'http://localhost:3001', 'http://127.0.0.1:3000']

    # Use local database, fall back to SQLite when DATABASE_URL is not set
    SQLALCHEMY_DATABASE_URI = os.getenv('DATABASE_URL', 'sqlite:///eventshield.db')

    # Development logging
    LOG_LEVEL = 'DEBUG'

    # Disable SSL for local development
    SQLALCHEMY_ENGINE_OPTIONS = {
        'pool_size': 5,
        'max_overflow': 10,
        'pool_timeout': 30,
        'pool_pre_ping': True
    }


class StandaloneConfig(Config):
    """Standalone / quick-start configuration — SQLite, no external services required"""

    DEBUG = True
    SQLALCHEMY_ECHO = False

    SQLALCHEMY_DATABASE_URI = os.getenv('DATABASE_URL', 'sqlite:///eventshield.db')

    SQLALCHEMY_ENGINE_OPTIONS = {
        'pool_pre_ping': True
    }

    CORS_ORIGINS = ['http://localhost:3000', 'http://localhost:3001', 'http://127.0.0.1:3000']
    LOG_LEVEL = 'DEBUG'

class TestingConfig(Config):
    """Testing environment configuration"""
    
    TESTING = True
    DEBUG = True
    
    # Use test database
    SQLALCHEMY_DATABASE_URI = os.getenv('TEST_DATABASE_URL', 'sqlite:///test.db')
    
    # Disable CSRF protection for testing
    WTF_CSRF_ENABLED = False
    
    # Use in-memory cache for testing
    CACHE_TYPE = 'simple'
    
    # Disable logging during tests
    LOG_LEVEL = 'ERROR'

class StagingConfig(Config):
    """Staging environment configuration"""
    
    DEBUG = False
    
    # Staging-specific settings
    CORS_ORIGINS = [
        'https://staging.eventshieldpro.com',
        'https://staging-app.eventshieldpro.com'
    ]
    
    # Staging database
    SQLALCHEMY_DATABASE_URI = os.getenv('STAGING_DATABASE_URL')
    
    # Staging logging
    LOG_LEVEL = 'INFO'
    
    # Enable SSL
    SQLALCHEMY_ENGINE_OPTIONS = {
        'pool_size': 20,
        'max_overflow': 30,
        'pool_timeout': 30,
        'pool_pre_ping': True,
        'connect_args': {'sslmode': 'require'}
    }

class ProductionConfig(Config):
    """Production environment configuration"""
    
    DEBUG = False
    
    # Production-specific settings
    CORS_ORIGINS = [
        'https://app.eventshieldpro.com',
        'https://eventshieldpro.com'
    ]
    
    # Production database
    SQLALCHEMY_DATABASE_URI = os.getenv('PRODUCTION_DATABASE_URL')
    
    # Production logging
    LOG_LEVEL = 'WARNING'
    
    # Production security
    SESSION_COOKIE_SECURE = True
    SESSION_COOKIE_HTTPONLY = True
    SESSION_COOKIE_SAMESITE = 'Lax'
    
    # Production database settings
    SQLALCHEMY_ENGINE_OPTIONS = {
        'pool_size': 50,
        'max_overflow': 100,
        'pool_timeout': 30,
        'pool_pre_ping': True,
        'connect_args': {'sslmode': 'require'}
    }
    
    # Enable all security features
    FEATURE_FACIAL_RECOGNITION = True
    FEATURE_TURNSTILE_INTEGRATION = True
    FEATURE_OFFLINE_MODE = True
    FEATURE_WHITE_LABEL = True
    FEATURE_ADVANCED_ANALYTICS = True
    FEATURE_MULTI_TENANT = True

# Configuration mapping
config = {
    'development': DevelopmentConfig,
    'standalone': StandaloneConfig,
    'testing': TestingConfig,
    'staging': StagingConfig,
    'production': ProductionConfig,
    'default': StandaloneConfig
}

def get_config():
    """Get configuration based on environment"""
    env = os.getenv('FLASK_ENV', 'development')
    return config.get(env, config['default'])
