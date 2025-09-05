"""
EventShield Pro - Database Models
Import all models to ensure they are registered with SQLAlchemy
"""

from .user import User, UserProfile, UserRole, UserPermission
from .tenant import Tenant, TenantSubscription, TenantSettings
from .event import Event, EventCategory, EventLocation, EventSchedule
from .ticket import Ticket, TicketType, TicketPurchase, TicketValidation
from .access_control import AccessLog, AccessPoint, AccessPermission
from .licensing import License, LicenseFeature, LicenseUsage
from .billing import BillingAccount, Invoice, Payment, Subscription
from .hardware import HardwareDevice, DeviceLog, DeviceStatus
from .analytics import AnalyticsEvent, UserActivity, SystemMetrics

# Export all models
__all__ = [
    # User Management
    'User', 'UserProfile', 'UserRole', 'UserPermission',
    
    # Multi-tenancy
    'Tenant', 'TenantSubscription', 'TenantSettings',
    
    # Event Management
    'Event', 'EventCategory', 'EventLocation', 'EventSchedule',
    
    # Ticket System
    'Ticket', 'TicketType', 'TicketPurchase', 'TicketValidation',
    
    # Access Control
    'AccessLog', 'AccessPoint', 'AccessPermission',
    
    # Licensing
    'License', 'LicenseFeature', 'LicenseUsage',
    
    # Billing
    'BillingAccount', 'Invoice', 'Payment', 'Subscription',
    
    # Hardware Integration
    'HardwareDevice', 'DeviceLog', 'DeviceStatus',
    
    # Analytics
    'AnalyticsEvent', 'UserActivity', 'SystemMetrics'
]
