"""
EventShield Pro - Comprehensive Logging System
Structured logging with multiple outputs for commercial-grade application
"""

import os
import sys
import json
import logging
import logging.handlers
from datetime import datetime
from typing import Dict, Any, Optional
from pathlib import Path
import structlog
from colorlog import ColoredFormatter


class EventShieldLogger:
    """Enhanced logging system for EventShield Pro"""
    
    def __init__(self, name: str = "eventshield", log_level: str = "INFO"):
        self.name = name
        self.log_level = getattr(logging, log_level.upper())
        self.log_dir = Path("logs")
        self.log_dir.mkdir(exist_ok=True)
        
        # Initialize structured logging
        self._setup_structured_logging()
        
        # Create logger instances
        self.logger = structlog.get_logger(name)
        self.api_logger = structlog.get_logger(f"{name}.api")
        self.device_logger = structlog.get_logger(f"{name}.device")
        self.biometric_logger = structlog.get_logger(f"{name}.biometric")
        self.security_logger = structlog.get_logger(f"{name}.security")
        self.performance_logger = structlog.get_logger(f"{name}.performance")
        self.audit_logger = structlog.get_logger(f"{name}.audit")
    
    def _setup_structured_logging(self):
        """Configure structured logging with multiple processors"""
        
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
        
        # Setup console logging with colors
        self._setup_console_logging()
        
        # Setup file logging
        self._setup_file_logging()
        
        # Setup database logging
        self._setup_database_logging()
        
        # Setup remote logging for cloud deployments
        if os.getenv("REMOTE_LOGGING_ENABLED", "false").lower() == "true":
            self._setup_remote_logging()
    
    def _setup_console_logging(self):
        """Setup colored console logging"""
        console_handler = logging.StreamHandler(sys.stdout)
        console_handler.setLevel(self.log_level)
        
        # Colored formatter for console
        color_formatter = ColoredFormatter(
            "%(log_color)s%(asctime)s %(name)s %(levelname)s%(reset)s %(message)s",
            datefmt="%Y-%m-%d %H:%M:%S",
            log_colors={
                'DEBUG': 'cyan',
                'INFO': 'green',
                'WARNING': 'yellow',
                'ERROR': 'red',
                'CRITICAL': 'red,bg_white',
            }
        )
        
        console_handler.setFormatter(color_formatter)
        
        # Add to root logger
        root_logger = logging.getLogger()
        root_logger.addHandler(console_handler)
        root_logger.setLevel(self.log_level)
    
    def _setup_file_logging(self):
        """Setup file-based logging with rotation"""
        
        # Main application log
        main_handler = logging.handlers.RotatingFileHandler(
            self.log_dir / "eventshield.log",
            maxBytes=10 * 1024 * 1024,  # 10MB
            backupCount=5
        )
        main_handler.setLevel(self.log_level)
        main_handler.setFormatter(logging.Formatter(
            '%(asctime)s %(name)s %(levelname)s %(message)s'
        ))
        
        # API access log
        api_handler = logging.handlers.RotatingFileHandler(
            self.log_dir / "api_access.log",
            maxBytes=50 * 1024 * 1024,  # 50MB
            backupCount=10
        )
        api_handler.setLevel(logging.INFO)
        api_handler.setFormatter(logging.Formatter(
            '%(asctime)s %(message)s'
        ))
        
        # Device communication log
        device_handler = logging.handlers.RotatingFileHandler(
            self.log_dir / "device_communication.log",
            maxBytes=20 * 1024 * 1024,  # 20MB
            backupCount=5
        )
        device_handler.setLevel(logging.DEBUG)
        device_handler.setFormatter(logging.Formatter(
            '%(asctime)s %(name)s %(levelname)s %(message)s'
        ))
        
        # Security events log
        security_handler = logging.handlers.RotatingFileHandler(
            self.log_dir / "security_events.log",
            maxBytes=10 * 1024 * 1024,  # 10MB
            backupCount=10
        )
        security_handler.setLevel(logging.INFO)
        security_handler.setFormatter(logging.Formatter(
            '%(asctime)s %(name)s %(levelname)s %(message)s'
        )
        
        # Performance metrics log
        performance_handler = logging.handlers.RotatingFileHandler(
            self.log_dir / "performance_metrics.log",
            maxBytes=20 * 1024 * 1024,  # 20MB
            backupCount=5
        )
        performance_handler.setLevel(logging.INFO)
        performance_handler.setFormatter(logging.Formatter(
            '%(asctime)s %(message)s'
        )
        
        # Audit trail log
        audit_handler = logging.handlers.RotatingFileHandler(
            self.log_dir / "audit_trail.log",
            maxBytes=10 * 1024 * 1024,  # 10MB
            backupCount=20
        )
        audit_handler.setLevel(logging.INFO)
        audit_handler.setFormatter(logging.Formatter(
            '%(asctime)s %(message)s'
        )
        
        # Add handlers to specific loggers
        logging.getLogger("eventshield").addHandler(main_handler)
        logging.getLogger("eventshield.api").addHandler(api_handler)
        logging.getLogger("eventshield.device").addHandler(device_handler)
        logging.getLogger("eventshield.security").addHandler(security_handler)
        logging.getLogger("eventshield.performance").addHandler(performance_handler)
        logging.getLogger("eventshield.audit").addHandler(audit_handler)
    
    def _setup_database_logging(self):
        """Setup database logging for audit trails"""
        # This would integrate with the database to store logs
        # Implementation depends on the database choice
        pass
    
    def _setup_remote_logging(self):
        """Setup remote logging for cloud deployments"""
        # This would send logs to remote logging services
        # like ELK stack, Splunk, or cloud logging services
        pass
    
    def log_api_request(self, method: str, endpoint: str, user_id: str = None, 
                       tenant_id: str = None, status_code: int = None, 
                       response_time: float = None, **kwargs):
        """Log API request with structured data"""
        self.api_logger.info(
            "API Request",
            method=method,
            endpoint=endpoint,
            user_id=user_id,
            tenant_id=tenant_id,
            status_code=status_code,
            response_time=response_time,
            **kwargs
        )
    
    def log_device_communication(self, device_id: str, device_sn: str, 
                               operation: str, success: bool, 
                               response_time: float = None, **kwargs):
        """Log device communication events"""
        level = "info" if success else "error"
        getattr(self.device_logger, level)(
            "Device Communication",
            device_id=device_id,
            device_sn=device_sn,
            operation=operation,
            success=success,
            response_time=response_time,
            **kwargs
        )
    
    def log_biometric_operation(self, operation: str, biometric_type: str,
                              person_id: str = None, quality_score: float = None,
                              success: bool = True, **kwargs):
        """Log biometric operations"""
        level = "info" if success else "error"
        getattr(self.biometric_logger, level)(
            "Biometric Operation",
            operation=operation,
            biometric_type=biometric_type,
            person_id=person_id,
            quality_score=quality_score,
            success=success,
            **kwargs
        )
    
    def log_security_event(self, event_type: str, user_id: str = None,
                          tenant_id: str = None, ip_address: str = None,
                          severity: str = "medium", **kwargs):
        """Log security-related events"""
        self.security_logger.warning(
            "Security Event",
            event_type=event_type,
            user_id=user_id,
            tenant_id=tenant_id,
            ip_address=ip_address,
            severity=severity,
            **kwargs
        )
    
    def log_performance_metric(self, metric_name: str, value: float,
                             unit: str = None, tags: Dict[str, str] = None):
        """Log performance metrics"""
        self.performance_logger.info(
            "Performance Metric",
            metric_name=metric_name,
            value=value,
            unit=unit,
            tags=tags or {}
        )
    
    def log_audit_event(self, action: str, resource: str, user_id: str = None,
                       tenant_id: str = None, details: Dict[str, Any] = None):
        """Log audit trail events"""
        self.audit_logger.info(
            "Audit Event",
            action=action,
            resource=resource,
            user_id=user_id,
            tenant_id=tenant_id,
            details=details or {}
        )
    
    def log_error(self, error: Exception, context: Dict[str, Any] = None):
        """Log errors with context"""
        self.logger.error(
            "Application Error",
            error_type=type(error).__name__,
            error_message=str(error),
            context=context or {}
        )
    
    def log_system_event(self, event: str, level: str = "info", **kwargs):
        """Log general system events"""
        getattr(self.logger, level.lower())(
            event,
            **kwargs
        )


# Global logger instance
logger = EventShieldLogger()


# Logging decorators
def log_function_call(logger_instance=None):
    """Decorator to log function calls"""
    def decorator(func):
        def wrapper(*args, **kwargs):
            logger_to_use = logger_instance or logger.logger
            logger_to_use.info(
                "Function Call",
                function=func.__name__,
                args=args,
                kwargs=kwargs
            )
            try:
                result = func(*args, **kwargs)
                logger_to_use.info(
                    "Function Success",
                    function=func.__name__,
                    result=result
                )
                return result
            except Exception as e:
                logger_to_use.error(
                    "Function Error",
                    function=func.__name__,
                    error=str(e)
                )
                raise
        return wrapper
    return decorator


def log_api_endpoint(logger_instance=None):
    """Decorator to log API endpoint calls"""
    def decorator(func):
        def wrapper(*args, **kwargs):
            logger_to_use = logger_instance or logger.api_logger
            start_time = datetime.utcnow()
            
            logger_to_use.info(
                "API Endpoint Called",
                endpoint=func.__name__,
                start_time=start_time.isoformat()
            )
            
            try:
                result = func(*args, **kwargs)
                end_time = datetime.utcnow()
                response_time = (end_time - start_time).total_seconds()
                
                logger_to_use.info(
                    "API Endpoint Success",
                    endpoint=func.__name__,
                    end_time=end_time.isoformat(),
                    response_time=response_time
                )
                return result
            except Exception as e:
                end_time = datetime.utcnow()
                response_time = (end_time - start_time).total_seconds()
                
                logger_to_use.error(
                    "API Endpoint Error",
                    endpoint=func.__name__,
                    error=str(e),
                    end_time=end_time.isoformat(),
                    response_time=response_time
                )
                raise
        return wrapper
    return decorator


# Performance monitoring decorator
def monitor_performance(metric_name: str, logger_instance=None):
    """Decorator to monitor function performance"""
    def decorator(func):
        def wrapper(*args, **kwargs):
            logger_to_use = logger_instance or logger.performance_logger
            start_time = datetime.utcnow()
            
            try:
                result = func(*args, **kwargs)
                end_time = datetime.utcnow()
                execution_time = (end_time - start_time).total_seconds()
                
                logger_to_use.info(
                    "Performance Metric",
                    metric_name=metric_name,
                    execution_time=execution_time,
                    function=func.__name__
                )
                return result
            except Exception as e:
                end_time = datetime.utcnow()
                execution_time = (end_time - start_time).total_seconds()
                
                logger_to_use.error(
                    "Performance Error",
                    metric_name=metric_name,
                    execution_time=execution_time,
                    function=func.__name__,
                    error=str(e)
                )
                raise
        return wrapper
    return decorator


# Logging configuration for different environments
def configure_logging_for_environment(environment: str = "development"):
    """Configure logging based on environment"""
    
    if environment == "production":
        # Production logging - more conservative, focus on errors and warnings
        log_level = "WARNING"
        enable_console_colors = False
        enable_debug_logging = False
    elif environment == "staging":
        # Staging logging - balanced approach
        log_level = "INFO"
        enable_console_colors = True
        enable_debug_logging = False
    else:  # development
        # Development logging - verbose for debugging
        log_level = "DEBUG"
        enable_console_colors = True
        enable_debug_logging = True
    
    # Update global logger configuration
    global logger
    logger.log_level = getattr(logging, log_level.upper())
    
    return logger
