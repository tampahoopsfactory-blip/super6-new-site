"""
EventShield Pro - Flask Backend Application
Main application factory and configuration
"""

import os
import logging
from datetime import timedelta
from flask import Flask
from flask_cors import CORS
from flask_sqlalchemy import SQLAlchemy
from flask_migrate import Migrate
from flask_jwt_extended import JWTManager
from flask_limiter import Limiter
from flask_limiter.util import get_remote_address
from flask_compress import Compress
from flask_caching import Cache
from flask_socketio import SocketIO
from prometheus_client import Counter, Histogram
import structlog

# Initialize extensions
db = SQLAlchemy()
migrate = Migrate()
jwt = JWTManager()
limiter = Limiter(key_func=get_remote_address)
compress = Compress()
cache = Cache()
socketio = SocketIO()

# Prometheus metrics
REQUEST_COUNT = Counter('http_requests_total', 'Total HTTP requests', ['method', 'endpoint', 'status'])
REQUEST_LATENCY = Histogram('http_request_duration_seconds', 'HTTP request latency')

def create_app(config_name=None):
    """Application factory pattern"""
    
    # Create Flask app instance
    app = Flask(__name__)
    
    # Load configuration
    if config_name is None:
        config_name = os.getenv('FLASK_ENV', 'development')
    
    app.config.from_object(f'app.config.{config_name.capitalize()}Config')
    
    # Initialize extensions
    initialize_extensions(app)
    
    # Configure logging
    configure_logging(app)
    
    # Register blueprints
    register_blueprints(app)
    
    # Register error handlers
    register_error_handlers(app)
    
    # Register CLI commands
    register_commands(app)
    
    # Initialize hardware integration
    initialize_hardware(app)
    
    # Initialize licensing system
    initialize_licensing(app)
    
    # Health check endpoint
    @app.route('/health')
    def health_check():
        return {'status': 'healthy', 'service': 'EventShield Pro API'}
    
    # Metrics endpoint for Prometheus
    @app.route('/metrics')
    def metrics():
        from prometheus_client import generate_latest
        return generate_latest()
    
    return app

def initialize_extensions(app):
    """Initialize Flask extensions"""
    
    # Database
    db.init_app(app)
    migrate.init_app(app, db)
    
    # JWT Authentication
    jwt.init_app(app)
    
    # CORS
    CORS(app, resources={
        r"/api/*": {
            "origins": app.config.get('CORS_ORIGINS', ['http://localhost:3000']),
            "methods": app.config.get('CORS_METHODS', ['GET', 'POST', 'PUT', 'DELETE', 'OPTIONS']),
            "allow_headers": app.config.get('CORS_ALLOW_HEADERS', ['Content-Type', 'Authorization'])
        }
    })
    
    # Rate Limiting
    limiter.init_app(app)
    
    # Compression
    compress.init_app(app)
    
    # Caching
    cache.init_app(app)
    
    # WebSocket
    socketio.init_app(app, cors_allowed_origins="*")
    
    # JWT Configuration
    app.config['JWT_ACCESS_TOKEN_EXPIRES'] = timedelta(
        seconds=int(app.config.get('JWT_ACCESS_TOKEN_EXPIRES', 3600))
    )
    app.config['JWT_REFRESH_TOKEN_EXPIRES'] = timedelta(
        seconds=int(app.config.get('JWT_REFRESH_TOKEN_EXPIRES', 604800))
    )

def configure_logging(app):
    """Configure application logging"""
    
    # Create logs directory if it doesn't exist
    log_dir = app.config.get('LOG_DIR', 'logs')
    os.makedirs(log_dir, exist_ok=True)
    
    # Configure structlog
    structlog.configure(
        processors=[
            structlog.stdlib.filter_by_level,
            structlog.stdlib.add_logger_name,
            structlog.stdlib.add_log_level,
            structlog.stdlib.PositionalArgumentsFormatter(),
            structlog.processors.TimeStamper(fmt="iso"),
            structlog.processors.StackInfoRenderer(),
            structlog.processors.format_exc_info,
            structlog.processors.UnicodeDecoder(),
            structlog.processors.JSONRenderer()
        ],
        context_class=dict,
        logger_factory=structlog.stdlib.LoggerFactory(),
        wrapper_class=structlog.stdlib.BoundLogger,
        cache_logger_on_first_use=True,
    )
    
    # Configure Flask logging
    if app.config.get('LOG_TO_FILE', True):
        file_handler = logging.FileHandler(
            os.path.join(log_dir, app.config.get('LOG_FILE', 'app.log'))
        )
        file_handler.setLevel(logging.INFO)
        app.logger.addHandler(file_handler)
    
    # Set log level
    app.logger.setLevel(
        getattr(logging, app.config.get('LOG_LEVEL', 'INFO').upper())
    )

def register_blueprints(app):
    """Register Flask blueprints"""
    
    from app.routes.auth import auth_bp
    from app.routes.users import users_bp
    from app.routes.events import events_bp
    from app.routes.tickets import tickets_bp
    from app.routes.access_control import access_control_bp
    from app.routes.licensing import licensing_bp
    from app.routes.billing import billing_bp
    from app.routes.analytics import analytics_bp
    from app.routes.hardware import hardware_bp
    from app.routes.webhooks import webhooks_bp
    from app.routes.admin import admin_bp
    
    # API v1 blueprints
    app.register_blueprint(auth_bp, url_prefix='/api/v1/auth')
    app.register_blueprint(users_bp, url_prefix='/api/v1/users')
    app.register_blueprint(events_bp, url_prefix='/api/v1/events')
    app.register_blueprint(tickets_bp, url_prefix='/api/v1/tickets')
    app.register_blueprint(access_control_bp, url_prefix='/api/v1/access-control')
    app.register_blueprint(licensing_bp, url_prefix='/api/v1/licensing')
    app.register_blueprint(billing_bp, url_prefix='/api/v1/billing')
    app.register_blueprint(analytics_bp, url_prefix='/api/v1/analytics')
    app.register_blueprint(hardware_bp, url_prefix='/api/v1/hardware')
    app.register_blueprint(webhooks_bp, url_prefix='/api/v1/webhooks')
    app.register_blueprint(admin_bp, url_prefix='/api/v1/admin')

def register_error_handlers(app):
    """Register error handlers"""
    
    @app.errorhandler(404)
    def not_found(error):
        return {'error': 'Resource not found'}, 404
    
    @app.errorhandler(500)
    def internal_error(error):
        app.logger.error(f'Internal server error: {error}')
        return {'error': 'Internal server error'}, 500
    
    @app.errorhandler(429)
    def rate_limit_exceeded(error):
        return {'error': 'Rate limit exceeded'}, 429

def register_commands(app):
    """Register CLI commands"""
    
    from app.commands import db_commands, user_commands, test_commands
    
    app.cli.add_command(db_commands.db_group)
    app.cli.add_command(user_commands.user_group)
    app.cli.add_command(test_commands.test_group)

def initialize_hardware(app):
    """Initialize hardware integration modules"""
    
    if app.config.get('FACIAL_RECOGNITION_ENABLED', False):
        try:
            from app.hardware.facial_recognition import initialize_facial_recognition
            initialize_facial_recognition(app)
            app.logger.info("Facial recognition system initialized")
        except Exception as e:
            app.logger.warning(f"Failed to initialize facial recognition: {e}")
    
    if app.config.get('TURNSTILE_ENABLED', False):
        try:
            from app.hardware.turnstiles import initialize_turnstiles
            initialize_turnstiles(app)
            app.logger.info("Turnstile system initialized")
        except Exception as e:
            app.logger.warning(f"Failed to initialize turnstiles: {e}")

def initialize_licensing(app):
    """Initialize licensing system"""
    
    try:
        from app.licensing.services import initialize_licensing_system
        initialize_licensing_system(app)
        app.logger.info("Licensing system initialized")
    except Exception as e:
        app.logger.warning(f"Failed to initialize licensing system: {e}")

# Create app instance
app = create_app()

# Import models to ensure they are registered with SQLAlchemy
from app.models import *

# Import routes to ensure they are registered
from app.routes import *

if __name__ == '__main__':
    app.run(debug=app.config.get('DEBUG', False))
