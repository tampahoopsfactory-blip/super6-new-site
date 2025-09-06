"""
EventShield Pro - Custom Exception Classes
Robust error handling for commercial-grade application
"""

from typing import Optional, Dict, Any
import traceback
from datetime import datetime


class EventShieldError(Exception):
    """Base exception class for EventShield Pro"""
    
    def __init__(self, message: str, error_code: str = None, details: Dict[str, Any] = None):
        super().__init__(message)
        self.message = message
        self.error_code = error_code or "EVENTSHIELD_ERROR"
        self.details = details or {}
        self.timestamp = datetime.utcnow()
        self.traceback = traceback.format_exc()
    
    def to_dict(self) -> Dict[str, Any]:
        """Convert exception to dictionary for API responses"""
        return {
            "error": self.error_code,
            "message": self.message,
            "details": self.details,
            "timestamp": self.timestamp.isoformat(),
            "traceback": self.traceback if self.details.get("include_traceback", False) else None
        }


class DeviceConnectionError(EventShieldError):
    """Device communication errors"""
    
    def __init__(self, message: str, device_id: str = None, device_sn: str = None, 
                 connection_type: str = None, details: Dict[str, Any] = None):
        super().__init__(message, "DEVICE_CONNECTION_ERROR", details)
        self.device_id = device_id
        self.device_sn = device_sn
        self.connection_type = connection_type or "TCP"
    
    def to_dict(self) -> Dict[str, Any]:
        result = super().to_dict()
        result.update({
            "device_id": self.device_id,
            "device_sn": self.device_sn,
            "connection_type": self.connection_type
        })
        return result


class BiometricProcessingError(EventShieldError):
    """Biometric operation errors"""
    
    def __init__(self, message: str, biometric_type: str = None, 
                 operation: str = None, quality_score: float = None, 
                 details: Dict[str, Any] = None):
        super().__init__(message, "BIOMETRIC_PROCESSING_ERROR", details)
        self.biometric_type = biometric_type
        self.operation = operation
        self.quality_score = quality_score
    
    def to_dict(self) -> Dict[str, Any]:
        result = super().to_dict()
        result.update({
            "biometric_type": self.biometric_type,
            "operation": self.operation,
            "quality_score": self.quality_score
        })
        return result


class LicenseValidationError(EventShieldError):
    """License validation errors"""
    
    def __init__(self, message: str, tenant_id: str = None, license_key: str = None,
                 feature: str = None, details: Dict[str, Any] = None):
        super().__init__(message, "LICENSE_VALIDATION_ERROR", details)
        self.tenant_id = tenant_id
        self.license_key = license_key
        self.feature = feature
    
    def to_dict(self) -> Dict[str, Any]:
        result = super().to_dict()
        result.update({
            "tenant_id": self.tenant_id,
            "license_key": self.license_key,
            "feature": self.feature
        })
        return result


class DatabaseError(EventShieldError):
    """Database operation errors"""
    
    def __init__(self, message: str, operation: str = None, table: str = None,
                 query: str = None, details: Dict[str, Any] = None):
        super().__init__(message, "DATABASE_ERROR", details)
        self.operation = operation
        self.table = table
        self.query = query
    
    def to_dict(self) -> Dict[str, Any]:
        result = super().to_dict()
        result.update({
            "operation": self.operation,
            "table": self.table,
            "query": self.query
        })
        return result


class AuthenticationError(EventShieldError):
    """Authentication and authorization errors"""
    
    def __init__(self, message: str, user_id: str = None, tenant_id: str = None,
                 resource: str = None, action: str = None, details: Dict[str, Any] = None):
        super().__init__(message, "AUTHENTICATION_ERROR", details)
        self.user_id = user_id
        self.tenant_id = tenant_id
        self.resource = resource
        self.action = action
    
    def to_dict(self) -> Dict[str, Any]:
        result = super().to_dict()
        result.update({
            "user_id": self.user_id,
            "tenant_id": self.tenant_id,
            "resource": self.resource,
            "action": self.action
        })
        return result


class ConfigurationError(EventShieldError):
    """Configuration and setup errors"""
    
    def __init__(self, message: str, config_key: str = None, config_value: str = None,
                 environment: str = None, details: Dict[str, Any] = None):
        super().__init__(message, "CONFIGURATION_ERROR", details)
        self.config_key = config_key
        self.config_value = config_value
        self.environment = environment
    
    def to_dict(self) -> Dict[str, Any]:
        result = super().to_dict()
        result.update({
            "config_key": self.config_key,
            "config_value": self.config_value,
            "environment": self.environment
        })
        return result


class NetworkError(EventShieldError):
    """Network communication errors"""
    
    def __init__(self, message: str, url: str = None, method: str = None,
                 status_code: int = None, response_time: float = None, 
                 details: Dict[str, Any] = None):
        super().__init__(message, "NETWORK_ERROR", details)
        self.url = url
        self.method = method
        self.status_code = status_code
        self.response_time = response_time
    
    def to_dict(self) -> Dict[str, Any]:
        result = super().to_dict()
        result.update({
            "url": self.url,
            "method": self.method,
            "status_code": self.status_code,
            "response_time": self.response_time
        })
        return result


class ValidationError(EventShieldError):
    """Data validation errors"""
    
    def __init__(self, message: str, field: str = None, value: Any = None,
                 validation_rule: str = None, details: Dict[str, Any] = None):
        super().__init__(message, "VALIDATION_ERROR", details)
        self.field = field
        self.value = value
        self.validation_rule = validation_rule
    
    def to_dict(self) -> Dict[str, Any]:
        result = super().to_dict()
        result.update({
            "field": self.field,
            "value": str(self.value) if self.value is not None else None,
            "validation_rule": self.validation_rule
        })
        return result


class UpdateError(EventShieldError):
    """Application update errors"""
    
    def __init__(self, message: str, current_version: str = None, target_version: str = None,
                 update_type: str = None, details: Dict[str, Any] = None):
        super().__init__(message, "UPDATE_ERROR", details)
        self.current_version = current_version
        self.target_version = target_version
        self.update_type = update_type
    
    def to_dict(self) -> Dict[str, Any]:
        result = super().to_dict()
        result.update({
            "current_version": self.current_version,
            "target_version": self.target_version,
            "update_type": self.update_type
        })
        return result


class MonitoringError(EventShieldError):
    """Monitoring and metrics collection errors"""
    
    def __init__(self, message: str, metric_name: str = None, metric_value: Any = None,
                 collection_method: str = None, details: Dict[str, Any] = None):
        super().__init__(message, "MONITORING_ERROR", details)
        self.metric_name = metric_name
        self.metric_value = metric_value
        self.collection_method = collection_method
    
    def to_dict(self) -> Dict[str, Any]:
        result = super().to_dict()
        result.update({
            "metric_name": self.metric_name,
            "metric_value": str(self.metric_value) if self.metric_value is not None else None,
            "collection_method": self.collection_method
        })
        return result


# Error recovery strategies
class ErrorRecoveryStrategy:
    """Base class for error recovery strategies"""
    
    def can_recover(self, error: EventShieldError) -> bool:
        """Check if this strategy can recover from the error"""
        return False
    
    def recover(self, error: EventShieldError) -> bool:
        """Attempt to recover from the error"""
        return False


class DeviceConnectionRecovery(ErrorRecoveryStrategy):
    """Recovery strategy for device connection errors"""
    
    def can_recover(self, error: EventShieldError) -> bool:
        return isinstance(error, DeviceConnectionError)
    
    def recover(self, error: DeviceConnectionError) -> bool:
        """Attempt to reconnect to device"""
        try:
            # Implementation would go here
            # This is a placeholder for the actual recovery logic
            return True
        except Exception:
            return False


class DatabaseConnectionRecovery(ErrorRecoveryStrategy):
    """Recovery strategy for database connection errors"""
    
    def can_recover(self, error: EventShieldError) -> bool:
        return isinstance(error, DatabaseError)
    
    def recover(self, error: DatabaseError) -> bool:
        """Attempt to reconnect to database"""
        try:
            # Implementation would go here
            # This is a placeholder for the actual recovery logic
            return True
        except Exception:
            return False


# Global error recovery manager
class ErrorRecoveryManager:
    """Manages error recovery strategies"""
    
    def __init__(self):
        self.strategies = [
            DeviceConnectionRecovery(),
            DatabaseConnectionRecovery()
        ]
    
    def attempt_recovery(self, error: EventShieldError) -> bool:
        """Attempt to recover from an error using available strategies"""
        for strategy in self.strategies:
            if strategy.can_recover(error):
                try:
                    if strategy.recover(error):
                        return True
                except Exception as recovery_error:
                    # Log recovery failure but continue trying other strategies
                    print(f"Recovery strategy failed: {recovery_error}")
        return False


# Global recovery manager instance
recovery_manager = ErrorRecoveryManager()
