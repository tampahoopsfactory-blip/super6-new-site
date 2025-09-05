from datetime import datetime, timedelta
from sqlalchemy import Column, Integer, String, Boolean, DateTime, Text, ForeignKey, JSON, Enum, Index, Float, Numeric
from sqlalchemy.orm import relationship, validates
from sqlalchemy.ext.hybrid import hybrid_property
from app import db
import enum

class AnalyticsEventType(enum.Enum):
    USER_LOGIN = 'user_login'
    USER_LOGOUT = 'user_logout'
    EVENT_CREATED = 'event_created'
    EVENT_UPDATED = 'event_updated'
    EVENT_DELETED = 'event_deleted'
    TICKET_PURCHASED = 'ticket_purchased'
    TICKET_VALIDATED = 'ticket_validated'
    ACCESS_GRANTED = 'access_granted'
    ACCESS_DENIED = 'access_denied'
    PAYMENT_SUCCESS = 'payment_success'
    PAYMENT_FAILED = 'payment_failed'
    SUBSCRIPTION_CREATED = 'subscription_created'
    SUBSCRIPTION_CANCELLED = 'subscription_cancelled'
    FEATURE_USED = 'feature_used'
    API_CALL = 'api_call'
    ERROR_OCCURRED = 'error_occurred'
    SYSTEM_MAINTENANCE = 'system_maintenance'

class AnalyticsEvent(db.Model):
    __tablename__ = 'analytics_events'
    
    id = Column(Integer, primary_key=True)
    tenant_id = Column(Integer, ForeignKey('tenants.id'), nullable=False, index=True)
    event_type = Column(Enum(AnalyticsEventType), nullable=False, index=True)
    
    # Event details
    event_name = Column(String(100), nullable=False)
    event_description = Column(Text, nullable=True)
    event_category = Column(String(50), nullable=True, index=True)
    
    # User context
    user_id = Column(Integer, ForeignKey('users.id'), nullable=True, index=True)
    user_role = Column(String(50), nullable=True)
    user_ip_address = Column(String(45), nullable=True)
    user_agent = Column(String(500), nullable=True)
    
    # Event context
    related_event_id = Column(Integer, ForeignKey('events.id'), nullable=True, index=True)
    related_ticket_id = Column(Integer, ForeignKey('tickets.id'), nullable=True, index=True)
    related_payment_id = Column(Integer, ForeignKey('payments.id'), nullable=True, index=True)
    
    # Event data
    event_data = Column(JSON, nullable=True)  # Additional event-specific data
    metadata = Column(JSON, nullable=True)  # General metadata
    
    # Performance metrics
    processing_time_ms = Column(Integer, nullable=True)
    response_size_bytes = Column(Integer, nullable=True)
    
    # Status
    is_successful = Column(Boolean, default=True, nullable=False)
    error_message = Column(Text, nullable=True)
    error_code = Column(String(100), nullable=True)
    
    # Relationships
    tenant = relationship('Tenant')
    user = relationship('User')
    related_event = relationship('Event')
    related_ticket = relationship('Ticket')
    related_payment = relationship('Payment')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_analytics_events_tenant_time', 'tenant_id', 'created_at'),
        Index('idx_analytics_events_type_time', 'event_type', 'created_at'),
        Index('idx_analytics_events_user_time', 'user_id', 'created_at'),
        Index('idx_analytics_events_category', 'event_category'),
        Index('idx_analytics_events_success', 'is_successful'),
    )
    
    @hybrid_property
    def is_error(self):
        """Check if event represents an error"""
        return not self.is_successful
    
    @hybrid_property
    def processing_time_seconds(self):
        """Get processing time in seconds"""
        if self.processing_time_ms:
            return self.processing_time_ms / 1000.0
        return None
    
    @hybrid_property
    def response_size_kb(self):
        """Get response size in kilobytes"""
        if self.response_size_bytes:
            return self.response_size_bytes / 1024.0
        return None
    
    def __repr__(self):
        return f'<AnalyticsEvent {self.event_type.value} - {self.event_name}>'

class UserActivity(db.Model):
    __tablename__ = 'user_activities'
    
    id = Column(Integer, primary_key=True)
    user_id = Column(Integer, ForeignKey('users.id'), nullable=False, index=True)
    tenant_id = Column(Integer, ForeignKey('tenants.id'), nullable=False, index=True)
    
    # Session information
    session_id = Column(String(100), nullable=False, index=True)
    login_time = Column(DateTime, nullable=False)
    logout_time = Column(DateTime, nullable=True)
    last_activity = Column(DateTime, nullable=False)
    
    # Access details
    ip_address = Column(String(45), nullable=True)
    user_agent = Column(String(500), nullable=True)
    location = Column(String(255), nullable=True)
    device_type = Column(String(50), nullable=True)  # desktop, mobile, tablet
    
    # Activity metrics
    page_views = Column(Integer, default=0, nullable=False)
    api_calls = Column(Integer, default=0, nullable=False)
    actions_performed = Column(Integer, default=0, nullable=False)
    
    # Performance metrics
    average_response_time_ms = Column(Float, nullable=True)
    total_processing_time_ms = Column(Integer, default=0, nullable=False)
    
    # Status
    is_active = Column(Boolean, default=True, nullable=False, index=True)
    logout_reason = Column(String(100), nullable=True)  # timeout, logout, error
    
    # Relationships
    user = relationship('User')
    tenant = relationship('Tenant')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_user_activities_user_time', 'user_id', 'created_at'),
        Index('idx_user_activities_session', 'session_id'),
        Index('idx_user_activities_active', 'is_active'),
        Index('idx_user_activities_tenant_time', 'tenant_id', 'created_at'),
    )
    
    @hybrid_property
    def session_duration_seconds(self):
        """Get session duration in seconds"""
        if self.logout_time:
            return (self.logout_time - self.login_time).total_seconds()
        else:
            return (datetime.utcnow() - self.login_time).total_seconds()
    
    @hybrid_property
    def session_duration_minutes(self):
        """Get session duration in minutes"""
        return self.session_duration_seconds / 60
    
    @hybrid_property
    def session_duration_hours(self):
        """Get session duration in hours"""
        return self.session_duration_seconds / 3600
    
    @hybrid_property
    def inactivity_duration_seconds(self):
        """Get duration since last activity"""
        return (datetime.utcnow() - self.last_activity).total_seconds()
    
    @hybrid_property
    def average_response_time_seconds(self):
        """Get average response time in seconds"""
        if self.average_response_time_ms:
            return self.average_response_time_ms / 1000.0
        return None
    
    @hybrid_property
    def total_processing_time_seconds(self):
        """Get total processing time in seconds"""
        return self.total_processing_time_ms / 1000.0
    
    def update_activity(self):
        """Update last activity timestamp"""
        self.last_activity = datetime.utcnow()
        self.updated_at = datetime.utcnow()
    
    def increment_page_views(self, count=1):
        """Increment page view count"""
        self.page_views += count
        self.update_activity()
    
    def increment_api_calls(self, count=1):
        """Increment API call count"""
        self.api_calls += count
        self.update_activity()
    
    def increment_actions(self, count=1):
        """Increment actions performed count"""
        self.actions_performed += count
        self.update_activity()
    
    def add_processing_time(self, processing_time_ms):
        """Add processing time to total"""
        self.total_processing_time_ms += processing_time_ms
        # Update average
        total_calls = self.api_calls + self.actions_performed
        if total_calls > 0:
            self.average_response_time_ms = self.total_processing_time_ms / total_calls
    
    def logout(self, reason='logout'):
        """Logout the user session"""
        self.logout_time = datetime.utcnow()
        self.is_active = False
        self.logout_reason = reason
        self.update_activity()
    
    def __repr__(self):
        return f'<UserActivity {self.user_id} - {self.session_id}>'

class SystemMetrics(db.Model):
    __tablename__ = 'system_metrics'
    
    id = Column(Integer, primary_key=True)
    tenant_id = Column(Integer, ForeignKey('tenants.id'), nullable=True, index=True)  # NULL for system-wide metrics
    
    # Metric identification
    metric_name = Column(String(100), nullable=False, index=True)
    metric_category = Column(String(50), nullable=False, index=True)  # performance, business, security, hardware
    
    # Metric values
    metric_value = Column(Float, nullable=False)
    metric_unit = Column(String(20), nullable=True)  # ms, %, count, bytes, etc.
    metric_type = Column(String(20), nullable=False)  # gauge, counter, histogram
    
    # Context
    hostname = Column(String(100), nullable=True)
    service_name = Column(String(100), nullable=True)
    instance_id = Column(String(100), nullable=True)
    
    # Additional data
    labels = Column(JSON, nullable=True)  # Key-value pairs for filtering
    metadata = Column(JSON, nullable=True)
    
    # Relationships
    tenant = relationship('Tenant')
    
    # Timestamps
    recorded_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_system_metrics_name_time', 'metric_name', 'recorded_at'),
        Index('idx_system_metrics_category_time', 'metric_category', 'recorded_at'),
        Index('idx_system_metrics_tenant_time', 'tenant_id', 'recorded_at'),
        Index('idx_system_metrics_service', 'service_name'),
    )
    
    def __repr__(self):
        return f'<SystemMetrics {self.metric_name} - {self.metric_value} {self.metric_unit}>'

class BusinessMetrics(db.Model):
    __tablename__ = 'business_metrics'
    
    id = Column(Integer, primary_key=True)
    tenant_id = Column(Integer, ForeignKey('tenants.id'), nullable=False, index=True)
    
    # Metric identification
    metric_name = Column(String(100), nullable=False, index=True)
    metric_period = Column(String(20), nullable=False, index=True)  # daily, weekly, monthly, yearly
    
    # Metric values
    metric_value = Column(Numeric(15, 4), nullable=False)
    previous_value = Column(Numeric(15, 4), nullable=True)
    change_percentage = Column(Float, nullable=True)
    
    # Time period
    period_start = Column(DateTime, nullable=False, index=True)
    period_end = Column(DateTime, nullable=False, index=True)
    
    # Context
    event_id = Column(Integer, ForeignKey('events.id'), nullable=True, index=True)
    user_id = Column(Integer, ForeignKey('users.id'), nullable=True, index=True)
    
    # Additional data
    breakdown = Column(JSON, nullable=True)  # Detailed breakdown of the metric
    metadata = Column(JSON, nullable=True)
    
    # Relationships
    tenant = relationship('Tenant')
    event = relationship('Event')
    user = relationship('User')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_business_metrics_tenant_period', 'tenant_id', 'metric_period', 'period_start'),
        Index('idx_business_metrics_name_time', 'metric_name', 'period_start'),
        Index('idx_business_metrics_event', 'event_id'),
    )
    
    @hybrid_property
    def period_duration_days(self):
        """Get period duration in days"""
        return (self.period_end - self.period_start).days
    
    @hybrid_property
    def has_previous_value(self):
        """Check if previous value exists for comparison"""
        return self.previous_value is not None
    
    @hybrid_property
    def is_positive_change(self):
        """Check if change is positive"""
        if self.change_percentage is not None:
            return self.change_percentage > 0
        return None
    
    def calculate_change_percentage(self):
        """Calculate change percentage from previous value"""
        if self.previous_value and self.previous_value != 0:
            self.change_percentage = ((self.metric_value - self.previous_value) / self.previous_value) * 100
        else:
            self.change_percentage = None
    
    def __repr__(self):
        return f'<BusinessMetrics {self.metric_name} - {self.metric_value} ({self.metric_period})>'

class DashboardWidget(db.Model):
    __tablename__ = 'dashboard_widgets'
    
    id = Column(Integer, primary_key=True)
    tenant_id = Column(Integer, ForeignKey('tenants.id'), nullable=False, index=True)
    user_id = Column(Integer, ForeignKey('users.id'), nullable=True, index=True)  # NULL for tenant-wide widgets
    
    # Widget configuration
    name = Column(String(100), nullable=False)
    description = Column(Text, nullable=True)
    widget_type = Column(String(50), nullable=False)  # chart, metric, table, list
    
    # Data source
    data_source = Column(String(100), nullable=False)  # SQL query, API endpoint, etc.
    data_config = Column(JSON, nullable=True)  # Configuration for data source
    
    # Display configuration
    position_x = Column(Integer, default=0, nullable=False)
    position_y = Column(Integer, default=0, nullable=False)
    width = Column(Integer, default=300, nullable=False)
    height = Column(Integer, default=200, nullable=False)
    
    # Chart configuration (for chart widgets)
    chart_type = Column(String(50), nullable=True)  # line, bar, pie, etc.
    chart_config = Column(JSON, nullable=True)  # Chart-specific configuration
    
    # Refresh settings
    auto_refresh = Column(Boolean, default=False, nullable=False)
    refresh_interval_seconds = Column(Integer, default=300, nullable=False)  # 5 minutes default
    last_refresh = Column(DateTime, nullable=True)
    
    # Status
    is_active = Column(Boolean, default=True, nullable=False)
    is_public = Column(Boolean, default=False, nullable=False)  # Visible to all users
    
    # Relationships
    tenant = relationship('Tenant')
    user = relationship('User')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_dashboard_widgets_tenant_user', 'tenant_id', 'user_id'),
        Index('idx_dashboard_widgets_type', 'widget_type'),
        Index('idx_dashboard_widgets_active', 'is_active'),
    )
    
    @hybrid_property
    def needs_refresh(self):
        """Check if widget needs refresh"""
        if not self.auto_refresh or not self.last_refresh:
            return True
        
        refresh_due = self.last_refresh + timedelta(seconds=self.refresh_interval_seconds)
        return datetime.utcnow() > refresh_due
    
    @hybrid_property
    def seconds_until_refresh(self):
        """Get seconds until next refresh"""
        if not self.auto_refresh or not self.last_refresh:
            return 0
        
        refresh_due = self.last_refresh + timedelta(seconds=self.refresh_interval_seconds)
        delta = refresh_due - datetime.utcnow()
        return max(0, int(delta.total_seconds()))
    
    def refresh(self):
        """Mark widget as refreshed"""
        self.last_refresh = datetime.utcnow()
        self.updated_at = datetime.utcnow()
    
    def __repr__(self):
        return f'<DashboardWidget {self.name} - {self.widget_type}>'

class ReportTemplate(db.Model):
    __tablename__ = 'report_templates'
    
    id = Column(Integer, primary_key=True)
    tenant_id = Column(Integer, ForeignKey('tenants.id'), nullable=False, index=True)
    
    # Template configuration
    name = Column(String(100), nullable=False)
    description = Column(Text, nullable=True)
    report_type = Column(String(50), nullable=False)  # event_summary, user_activity, financial, etc.
    
    # Data configuration
    data_source = Column(String(100), nullable=False)
    data_filters = Column(JSON, nullable=True)  # Default filters
    data_sorting = Column(JSON, nullable=True)  # Default sorting
    
    # Output configuration
    output_format = Column(String(50), default='pdf', nullable=False)  # pdf, excel, csv, html
    template_file = Column(String(255), nullable=True)  # Path to template file
    styling_config = Column(JSON, nullable=True)  # CSS/styling configuration
    
    # Schedule configuration
    is_scheduled = Column(Boolean, default=False, nullable=False)
    schedule_cron = Column(String(100), nullable=True)  # Cron expression for scheduling
    schedule_timezone = Column(String(50), default='UTC', nullable=False)
    
    # Delivery configuration
    delivery_method = Column(String(50), default='email', nullable=False)  # email, webhook, storage
    delivery_config = Column(JSON, nullable=True)  # Delivery-specific configuration
    
    # Status
    is_active = Column(Boolean, default=True, nullable=False)
    is_public = Column(Boolean, default=False, nullable=False)
    
    # Relationships
    tenant = relationship('Tenant')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_report_templates_tenant_type', 'tenant_id', 'report_type'),
        Index('idx_report_templates_active', 'is_active'),
        Index('idx_report_templates_scheduled', 'is_scheduled'),
    )
    
    @hybrid_property
    def is_scheduled_report(self):
        """Check if this is a scheduled report"""
        return self.is_scheduled and self.schedule_cron is not None
    
    def get_next_schedule(self):
        """Get next scheduled run time"""
        if not self.is_scheduled_report:
            return None
        
        try:
            from croniter import croniter
            from datetime import datetime
            cron = croniter(self.schedule_cron, datetime.utcnow())
            return cron.get_next(datetime)
        except ImportError:
            return None
    
    def __repr__(self):
        return f'<ReportTemplate {self.name} - {self.report_type}>'

class DataExport(db.Model):
    __tablename__ = 'data_exports'
    
    id = Column(Integer, primary_key=True)
    tenant_id = Column(Integer, ForeignKey('tenants.id'), nullable=False, index=True)
    user_id = Column(Integer, ForeignKey('users.id'), nullable=False, index=True)
    
    # Export configuration
    export_name = Column(String(100), nullable=False)
    export_type = Column(String(50), nullable=False)  # events, users, tickets, analytics, etc.
    export_format = Column(String(20), nullable=False)  # csv, excel, json, xml
    
    # Data configuration
    data_filters = Column(JSON, nullable=True)
    data_columns = Column(JSON, nullable=True)  # Selected columns
    date_range_start = Column(DateTime, nullable=True)
    date_range_end = Column(DateTime, nullable=True)
    
    # Export status
    status = Column(String(50), default='pending', nullable=False, index=True)  # pending, processing, completed, failed
    progress_percentage = Column(Float, default=0.0, nullable=False)
    
    # File information
    file_path = Column(String(500), nullable=True)
    file_size_bytes = Column(Integer, nullable=True)
    download_url = Column(String(500), nullable=True)
    
    # Processing information
    started_at = Column(DateTime, nullable=True)
    completed_at = Column(DateTime, nullable=True)
    error_message = Column(Text, nullable=True)
    
    # Metadata
    metadata = Column(JSON, nullable=True)
    
    # Relationships
    tenant = relationship('Tenant')
    user = relationship('User')
    
    # Timestamps
    created_at = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow, nullable=False)
    
    __table_args__ = (
        Index('idx_data_exports_tenant_status', 'tenant_id', 'status'),
        Index('idx_data_exports_user_status', 'user_id', 'status'),
        Index('idx_data_exports_type_time', 'export_type', 'created_at'),
    )
    
    @hybrid_property
    def is_pending(self):
        """Check if export is pending"""
        return self.status == 'pending'
    
    @hybrid_property
    def is_processing(self):
        """Check if export is processing"""
        return self.status == 'processing'
    
    @hybrid_property
    def is_completed(self):
        """Check if export is completed"""
        return self.status == 'completed'
    
    @hybrid_property
    def is_failed(self):
        """Check if export failed"""
        return self.status == 'failed'
    
    @hybrid_property
    def processing_duration_seconds(self):
        """Get processing duration in seconds"""
        if self.started_at and self.completed_at:
            return (self.completed_at - self.started_at).total_seconds()
        return None
    
    @hybrid_property
    def file_size_mb(self):
        """Get file size in megabytes"""
        if self.file_size_bytes:
            return self.file_size_bytes / (1024 * 1024)
        return None
    
    def start_processing(self):
        """Start processing the export"""
        self.status = 'processing'
        self.started_at = datetime.utcnow()
        self.progress_percentage = 0.0
        self.updated_at = datetime.utcnow()
    
    def update_progress(self, percentage):
        """Update export progress"""
        self.progress_percentage = min(100.0, max(0.0, percentage))
        self.updated_at = datetime.utcnow()
    
    def complete(self, file_path, file_size_bytes, download_url=None):
        """Mark export as completed"""
        self.status = 'completed'
        self.completed_at = datetime.utcnow()
        self.progress_percentage = 100.0
        self.file_path = file_path
        self.file_size_bytes = file_size_bytes
        self.download_url = download_url
        self.updated_at = datetime.utcnow()
    
    def fail(self, error_message):
        """Mark export as failed"""
        self.status = 'failed'
        self.completed_at = datetime.utcnow()
        self.error_message = error_message
        self.updated_at = datetime.utcnow()
    
    def __repr__(self):
        return f'<DataExport {self.export_name} - {self.status}>'

