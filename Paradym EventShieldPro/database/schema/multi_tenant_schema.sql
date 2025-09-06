-- EventShield Pro - Multi-Tenant Database Schema
-- Commercial-grade database schema for white-label licensing

-- Enable UUID extension
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Create schemas for multi-tenancy
CREATE SCHEMA IF NOT EXISTS tenants;
CREATE SCHEMA IF NOT EXISTS devices;
CREATE SCHEMA IF NOT EXISTS biometrics;
CREATE SCHEMA IF NOT EXISTS events;
CREATE SCHEMA IF NOT EXISTS analytics;
CREATE SCHEMA IF NOT EXISTS audit;

-- =============================================
-- TENANT MANAGEMENT
-- =============================================

-- Tenants table
CREATE TABLE tenants.tenants (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id VARCHAR(255) UNIQUE NOT NULL,
    name VARCHAR(255) NOT NULL,
    domain VARCHAR(255) UNIQUE NOT NULL,
    license_key VARCHAR(255) UNIQUE NOT NULL,
    features JSONB DEFAULT '[]'::jsonb,
    branding JSONB DEFAULT '{}'::jsonb,
    settings JSONB DEFAULT '{}'::jsonb,
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    expires_at TIMESTAMP WITH TIME ZONE,
    last_activity TIMESTAMP WITH TIME ZONE
);

-- Tenant users (for multi-user access)
CREATE TABLE tenants.tenant_users (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id UUID REFERENCES tenants.tenants(id) ON DELETE CASCADE,
    user_id VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    name VARCHAR(255) NOT NULL,
    role VARCHAR(50) DEFAULT 'user',
    permissions JSONB DEFAULT '{}'::jsonb,
    is_active BOOLEAN DEFAULT true,
    last_login TIMESTAMP WITH TIME ZONE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    UNIQUE(tenant_id, user_id)
);

-- =============================================
-- DEVICE MANAGEMENT
-- =============================================

-- Device types
CREATE TABLE devices.device_types (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    name VARCHAR(100) NOT NULL,
    model VARCHAR(100) NOT NULL,
    manufacturer VARCHAR(100) NOT NULL,
    communication_protocols JSONB DEFAULT '[]'::jsonb,
    capabilities JSONB DEFAULT '{}'::jsonb,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Devices table
CREATE TABLE devices.devices (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id UUID REFERENCES tenants.tenants(id) ON DELETE CASCADE,
    device_id VARCHAR(255) NOT NULL,
    device_sn VARCHAR(16) NOT NULL,
    device_type_id UUID REFERENCES devices.device_types(id),
    ip_address INET NOT NULL,
    port INTEGER DEFAULT 8000,
    communication_mode INTEGER DEFAULT 2, -- TCP_CLIENT
    password VARCHAR(255),
    status VARCHAR(50) DEFAULT 'offline',
    last_seen TIMESTAMP WITH TIME ZONE,
    configuration JSONB DEFAULT '{}'::jsonb,
    location VARCHAR(255),
    notes TEXT,
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    UNIQUE(tenant_id, device_id)
);

-- Device communication logs
CREATE TABLE devices.device_logs (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    device_id UUID REFERENCES devices.devices(id) ON DELETE CASCADE,
    operation VARCHAR(100) NOT NULL,
    command_type VARCHAR(100),
    success BOOLEAN NOT NULL,
    response_time INTEGER, -- milliseconds
    error_message TEXT,
    request_data JSONB,
    response_data JSONB,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Device status history
CREATE TABLE devices.device_status_history (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    device_id UUID REFERENCES devices.devices(id) ON DELETE CASCADE,
    status VARCHAR(50) NOT NULL,
    previous_status VARCHAR(50),
    reason TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- =============================================
-- BIOMETRIC DATA
-- =============================================

-- Persons table
CREATE TABLE biometrics.persons (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id UUID REFERENCES tenants.tenants(id) ON DELETE CASCADE,
    person_id VARCHAR(255) NOT NULL,
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255),
    phone VARCHAR(20),
    company VARCHAR(255),
    role VARCHAR(100),
    notes TEXT,
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    UNIQUE(tenant_id, person_id)
);

-- Biometric data types
CREATE TABLE biometrics.biometric_types (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    type_name VARCHAR(50) NOT NULL,
    description TEXT,
    quality_threshold FLOAT DEFAULT 0.6,
    max_data_size INTEGER DEFAULT 10485760, -- 10MB
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Biometric data
CREATE TABLE biometrics.biometric_data (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    person_id UUID REFERENCES biometrics.persons(id) ON DELETE CASCADE,
    biometric_type_id UUID REFERENCES biometrics.biometric_types(id),
    data BYTEA NOT NULL,
    quality_score FLOAT,
    metadata JSONB DEFAULT '{}'::jsonb,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Biometric templates (for recognition)
CREATE TABLE biometrics.biometric_templates (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    person_id UUID REFERENCES biometrics.persons(id) ON DELETE CASCADE,
    biometric_type_id UUID REFERENCES biometrics.biometric_types(id),
    template_data BYTEA NOT NULL,
    quality_score FLOAT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Recognition logs
CREATE TABLE biometrics.recognition_logs (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    person_id UUID REFERENCES biometrics.persons(id) ON DELETE SET NULL,
    biometric_type_id UUID REFERENCES biometrics.biometric_types(id),
    device_id UUID REFERENCES devices.devices(id) ON DELETE SET NULL,
    confidence_score FLOAT,
    success BOOLEAN NOT NULL,
    processing_time INTEGER, -- milliseconds
    image_data BYTEA,
    metadata JSONB DEFAULT '{}'::jsonb,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- =============================================
-- EVENT MANAGEMENT
-- =============================================

-- Events table
CREATE TABLE events.events (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id UUID REFERENCES tenants.tenants(id) ON DELETE CASCADE,
    event_id VARCHAR(255) NOT NULL,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    start_date TIMESTAMP WITH TIME ZONE NOT NULL,
    end_date TIMESTAMP WITH TIME ZONE NOT NULL,
    location VARCHAR(255),
    settings JSONB DEFAULT '{}'::jsonb,
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    UNIQUE(tenant_id, event_id)
);

-- Event attendees
CREATE TABLE events.event_attendees (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    event_id UUID REFERENCES events.events(id) ON DELETE CASCADE,
    person_id UUID REFERENCES biometrics.persons(id) ON DELETE CASCADE,
    ticket_type VARCHAR(100),
    ticket_number VARCHAR(255),
    check_in_time TIMESTAMP WITH TIME ZONE,
    check_out_time TIMESTAMP WITH TIME ZONE,
    status VARCHAR(50) DEFAULT 'registered',
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    UNIQUE(event_id, person_id)
);

-- Access control events
CREATE TABLE events.access_events (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    event_id UUID REFERENCES events.events(id) ON DELETE CASCADE,
    person_id UUID REFERENCES biometrics.persons(id) ON DELETE SET NULL,
    device_id UUID REFERENCES devices.devices(id) ON DELETE SET NULL,
    access_type VARCHAR(50) NOT NULL, -- entry, exit, denied
    success BOOLEAN NOT NULL,
    reason TEXT,
    confidence_score FLOAT,
    image_data BYTEA,
    metadata JSONB DEFAULT '{}'::jsonb,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- =============================================
-- ANALYTICS AND REPORTING
-- =============================================

-- Daily analytics
CREATE TABLE analytics.daily_metrics (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id UUID REFERENCES tenants.tenants(id) ON DELETE CASCADE,
    event_id UUID REFERENCES events.events(id) ON DELETE CASCADE,
    date DATE NOT NULL,
    total_attendees INTEGER DEFAULT 0,
    check_ins INTEGER DEFAULT 0,
    check_outs INTEGER DEFAULT 0,
    denied_access INTEGER DEFAULT 0,
    device_uptime FLOAT DEFAULT 0.0,
    avg_recognition_time FLOAT DEFAULT 0.0,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    UNIQUE(tenant_id, event_id, date)
);

-- Device performance metrics
CREATE TABLE analytics.device_metrics (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    device_id UUID REFERENCES devices.devices(id) ON DELETE CASCADE,
    metric_name VARCHAR(100) NOT NULL,
    metric_value FLOAT NOT NULL,
    unit VARCHAR(50),
    timestamp TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    metadata JSONB DEFAULT '{}'::jsonb
);

-- System performance metrics
CREATE TABLE analytics.system_metrics (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id UUID REFERENCES tenants.tenants(id) ON DELETE CASCADE,
    metric_name VARCHAR(100) NOT NULL,
    metric_value FLOAT NOT NULL,
    unit VARCHAR(50),
    timestamp TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    metadata JSONB DEFAULT '{}'::jsonb
);

-- =============================================
-- AUDIT TRAIL
-- =============================================

-- Audit logs
CREATE TABLE audit.audit_logs (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id UUID REFERENCES tenants.tenants(id) ON DELETE CASCADE,
    user_id VARCHAR(255),
    action VARCHAR(100) NOT NULL,
    resource_type VARCHAR(100) NOT NULL,
    resource_id VARCHAR(255),
    old_values JSONB,
    new_values JSONB,
    ip_address INET,
    user_agent TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Security events
CREATE TABLE audit.security_events (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id UUID REFERENCES tenants.tenants(id) ON DELETE CASCADE,
    event_type VARCHAR(100) NOT NULL,
    severity VARCHAR(20) DEFAULT 'medium',
    user_id VARCHAR(255),
    ip_address INET,
    description TEXT,
    metadata JSONB DEFAULT '{}'::jsonb,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- =============================================
-- INDEXES FOR PERFORMANCE
-- =============================================

-- Tenant indexes
CREATE INDEX idx_tenants_tenant_id ON tenants.tenants(tenant_id);
CREATE INDEX idx_tenants_domain ON tenants.tenants(domain);
CREATE INDEX idx_tenants_license_key ON tenants.tenants(license_key);
CREATE INDEX idx_tenants_is_active ON tenants.tenants(is_active);

-- Device indexes
CREATE INDEX idx_devices_tenant_id ON devices.devices(tenant_id);
CREATE INDEX idx_devices_device_id ON devices.devices(device_id);
CREATE INDEX idx_devices_device_sn ON devices.devices(device_sn);
CREATE INDEX idx_devices_status ON devices.devices(status);
CREATE INDEX idx_devices_last_seen ON devices.devices(last_seen);

-- Device logs indexes
CREATE INDEX idx_device_logs_device_id ON devices.device_logs(device_id);
CREATE INDEX idx_device_logs_created_at ON devices.device_logs(created_at);
CREATE INDEX idx_device_logs_operation ON devices.device_logs(operation);

-- Biometric indexes
CREATE INDEX idx_persons_tenant_id ON biometrics.persons(tenant_id);
CREATE INDEX idx_persons_person_id ON biometrics.persons(person_id);
CREATE INDEX idx_persons_email ON biometrics.persons(email);
CREATE INDEX idx_biometric_data_person_id ON biometrics.biometric_data(person_id);
CREATE INDEX idx_biometric_data_type ON biometrics.biometric_data(biometric_type_id);
CREATE INDEX idx_recognition_logs_person_id ON biometrics.recognition_logs(person_id);
CREATE INDEX idx_recognition_logs_device_id ON biometrics.recognition_logs(device_id);
CREATE INDEX idx_recognition_logs_created_at ON biometrics.recognition_logs(created_at);

-- Event indexes
CREATE INDEX idx_events_tenant_id ON events.events(tenant_id);
CREATE INDEX idx_events_event_id ON events.events(event_id);
CREATE INDEX idx_events_dates ON events.events(start_date, end_date);
CREATE INDEX idx_access_events_event_id ON events.access_events(event_id);
CREATE INDEX idx_access_events_person_id ON events.access_events(person_id);
CREATE INDEX idx_access_events_device_id ON events.access_events(device_id);
CREATE INDEX idx_access_events_created_at ON events.access_events(created_at);

-- Analytics indexes
CREATE INDEX idx_daily_metrics_tenant_event ON analytics.daily_metrics(tenant_id, event_id);
CREATE INDEX idx_daily_metrics_date ON analytics.daily_metrics(date);
CREATE INDEX idx_device_metrics_device_id ON analytics.device_metrics(device_id);
CREATE INDEX idx_device_metrics_timestamp ON analytics.device_metrics(timestamp);
CREATE INDEX idx_system_metrics_tenant_id ON analytics.system_metrics(tenant_id);
CREATE INDEX idx_system_metrics_timestamp ON analytics.system_metrics(timestamp);

-- Audit indexes
CREATE INDEX idx_audit_logs_tenant_id ON audit.audit_logs(tenant_id);
CREATE INDEX idx_audit_logs_user_id ON audit.audit_logs(user_id);
CREATE INDEX idx_audit_logs_created_at ON audit.audit_logs(created_at);
CREATE INDEX idx_security_events_tenant_id ON audit.security_events(tenant_id);
CREATE INDEX idx_security_events_type ON audit.security_events(event_type);
CREATE INDEX idx_security_events_created_at ON audit.security_events(created_at);

-- =============================================
-- TRIGGERS FOR AUTOMATIC UPDATES
-- =============================================

-- Function to update updated_at timestamp
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = NOW();
    RETURN NEW;
END;
$$ language 'plpgsql';

-- Apply updated_at triggers to relevant tables
CREATE TRIGGER update_tenants_updated_at BEFORE UPDATE ON tenants.tenants
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_devices_updated_at BEFORE UPDATE ON devices.devices
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_persons_updated_at BEFORE UPDATE ON biometrics.persons
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_biometric_data_updated_at BEFORE UPDATE ON biometrics.biometric_data
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_events_updated_at BEFORE UPDATE ON events.events
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

-- =============================================
-- INITIAL DATA
-- =============================================

-- Insert default biometric types
INSERT INTO biometrics.biometric_types (type_name, description, quality_threshold) VALUES
('face', 'Facial recognition', 0.6),
('palm', 'Palm recognition', 0.6),
('fingerprint', 'Fingerprint recognition', 0.7);

-- Insert default device types
INSERT INTO devices.device_types (name, model, manufacturer, communication_protocols, capabilities) VALUES
('DS-F8881', 'DS-F8881', 'Dahua', '["TCP", "UDP", "MQTT"]', '{"face_recognition": true, "palm_recognition": true, "access_control": true}');

-- =============================================
-- ROW LEVEL SECURITY (RLS)
-- =============================================

-- Enable RLS on tenant-specific tables
ALTER TABLE devices.devices ENABLE ROW LEVEL SECURITY;
ALTER TABLE biometrics.persons ENABLE ROW LEVEL SECURITY;
ALTER TABLE biometrics.biometric_data ENABLE ROW LEVEL SECURITY;
ALTER TABLE biometrics.biometric_templates ENABLE ROW LEVEL SECURITY;
ALTER TABLE biometrics.recognition_logs ENABLE ROW LEVEL SECURITY;
ALTER TABLE events.events ENABLE ROW LEVEL SECURITY;
ALTER TABLE events.event_attendees ENABLE ROW LEVEL SECURITY;
ALTER TABLE events.access_events ENABLE ROW LEVEL SECURITY;
ALTER TABLE analytics.daily_metrics ENABLE ROW LEVEL SECURITY;
ALTER TABLE analytics.device_metrics ENABLE ROW LEVEL SECURITY;
ALTER TABLE analytics.system_metrics ENABLE ROW LEVEL SECURITY;
ALTER TABLE audit.audit_logs ENABLE ROW LEVEL SECURITY;
ALTER TABLE audit.security_events ENABLE ROW LEVEL SECURITY;

-- Create RLS policies (these would be created dynamically based on tenant context)
-- Example policy for devices table:
-- CREATE POLICY tenant_devices_policy ON devices.devices
--     FOR ALL TO application_role
--     USING (tenant_id = current_setting('app.current_tenant_id')::uuid);

-- =============================================
-- VIEWS FOR COMMON QUERIES
-- =============================================

-- Device status view
CREATE VIEW devices.device_status_view AS
SELECT 
    d.id,
    d.device_id,
    d.device_sn,
    d.ip_address,
    d.port,
    d.status,
    d.last_seen,
    dt.name as device_type,
    t.name as tenant_name,
    CASE 
        WHEN d.last_seen > NOW() - INTERVAL '5 minutes' THEN 'online'
        WHEN d.last_seen > NOW() - INTERVAL '1 hour' THEN 'recent'
        ELSE 'offline'
    END as connection_status
FROM devices.devices d
JOIN devices.device_types dt ON d.device_type_id = dt.id
JOIN tenants.tenants t ON d.tenant_id = t.id;

-- Recognition statistics view
CREATE VIEW biometrics.recognition_stats_view AS
SELECT 
    p.tenant_id,
    p.person_id,
    p.name,
    COUNT(rl.id) as total_recognitions,
    COUNT(CASE WHEN rl.success = true THEN 1 END) as successful_recognitions,
    AVG(rl.confidence_score) as avg_confidence,
    AVG(rl.processing_time) as avg_processing_time,
    MAX(rl.created_at) as last_recognition
FROM biometrics.persons p
LEFT JOIN biometrics.recognition_logs rl ON p.id = rl.person_id
GROUP BY p.tenant_id, p.person_id, p.name;

-- Event attendance view
CREATE VIEW events.event_attendance_view AS
SELECT 
    e.id as event_id,
    e.name as event_name,
    e.start_date,
    e.end_date,
    COUNT(ea.id) as total_registrations,
    COUNT(CASE WHEN ea.check_in_time IS NOT NULL THEN 1 END) as check_ins,
    COUNT(CASE WHEN ea.check_out_time IS NOT NULL THEN 1 END) as check_outs,
    COUNT(CASE WHEN ea.status = 'active' THEN 1 END) as currently_present
FROM events.events e
LEFT JOIN events.event_attendees ea ON e.id = ea.event_id
GROUP BY e.id, e.name, e.start_date, e.end_date;

-- =============================================
-- FUNCTIONS FOR COMMON OPERATIONS
-- =============================================

-- Function to get tenant by domain
CREATE OR REPLACE FUNCTION get_tenant_by_domain(domain_name TEXT)
RETURNS TABLE(
    tenant_id UUID,
    name TEXT,
    domain TEXT,
    license_key TEXT,
    features JSONB,
    branding JSONB,
    is_active BOOLEAN
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        t.id,
        t.name,
        t.domain,
        t.license_key,
        t.features,
        t.branding,
        t.is_active
    FROM tenants.tenants t
    WHERE t.domain = domain_name;
END;
$$ LANGUAGE plpgsql;

-- Function to log device activity
CREATE OR REPLACE FUNCTION log_device_activity(
    p_device_id UUID,
    p_operation TEXT,
    p_success BOOLEAN,
    p_response_time INTEGER DEFAULT NULL,
    p_error_message TEXT DEFAULT NULL,
    p_request_data JSONB DEFAULT NULL,
    p_response_data JSONB DEFAULT NULL
)
RETURNS UUID AS $$
DECLARE
    log_id UUID;
BEGIN
    INSERT INTO devices.device_logs (
        device_id,
        operation,
        success,
        response_time,
        error_message,
        request_data,
        response_data
    ) VALUES (
        p_device_id,
        p_operation,
        p_success,
        p_response_time,
        p_error_message,
        p_request_data,
        p_response_data
    ) RETURNING id INTO log_id;
    
    RETURN log_id;
END;
$$ LANGUAGE plpgsql;

-- Function to update device status
CREATE OR REPLACE FUNCTION update_device_status(
    p_device_id UUID,
    p_status TEXT,
    p_reason TEXT DEFAULT NULL
)
RETURNS BOOLEAN AS $$
DECLARE
    old_status TEXT;
BEGIN
    -- Get current status
    SELECT status INTO old_status FROM devices.devices WHERE id = p_device_id;
    
    -- Update device status
    UPDATE devices.devices 
    SET status = p_status, last_seen = NOW()
    WHERE id = p_device_id;
    
    -- Log status change
    INSERT INTO devices.device_status_history (
        device_id,
        status,
        previous_status,
        reason
    ) VALUES (
        p_device_id,
        p_status,
        old_status,
        p_reason
    );
    
    RETURN TRUE;
END;
$$ LANGUAGE plpgsql;

-- =============================================
-- CLEANUP AND MAINTENANCE
-- =============================================

-- Function to clean up old logs
CREATE OR REPLACE FUNCTION cleanup_old_logs(retention_days INTEGER DEFAULT 90)
RETURNS INTEGER AS $$
DECLARE
    deleted_count INTEGER := 0;
BEGIN
    -- Clean up device logs older than retention period
    DELETE FROM devices.device_logs 
    WHERE created_at < NOW() - INTERVAL '1 day' * retention_days;
    
    GET DIAGNOSTICS deleted_count = ROW_COUNT;
    
    -- Clean up recognition logs older than retention period
    DELETE FROM biometrics.recognition_logs 
    WHERE created_at < NOW() - INTERVAL '1 day' * retention_days;
    
    -- Clean up access events older than retention period
    DELETE FROM events.access_events 
    WHERE created_at < NOW() - INTERVAL '1 day' * retention_days;
    
    -- Clean up old device status history
    DELETE FROM devices.device_status_history 
    WHERE created_at < NOW() - INTERVAL '1 day' * retention_days;
    
    RETURN deleted_count;
END;
$$ LANGUAGE plpgsql;

-- Create a scheduled job to run cleanup (requires pg_cron extension)
-- SELECT cron.schedule('cleanup-logs', '0 2 * * *', 'SELECT cleanup_old_logs(90);');

-- =============================================
-- GRANTS AND PERMISSIONS
-- =============================================

-- Create application role
-- CREATE ROLE eventshield_app;
-- GRANT USAGE ON SCHEMA tenants, devices, biometrics, events, analytics, audit TO eventshield_app;
-- GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA tenants, devices, biometrics, events, analytics, audit TO eventshield_app;
-- GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA tenants, devices, biometrics, events, analytics, audit TO eventshield_app;
-- GRANT EXECUTE ON ALL FUNCTIONS IN SCHEMA public TO eventshield_app;

-- Create read-only role for reporting
-- CREATE ROLE eventshield_readonly;
-- GRANT USAGE ON SCHEMA tenants, devices, biometrics, events, analytics, audit TO eventshield_readonly;
-- GRANT SELECT ON ALL TABLES IN SCHEMA tenants, devices, biometrics, events, analytics, audit TO eventshield_readonly;
-- GRANT SELECT ON ALL VIEWS IN SCHEMA tenants, devices, biometrics, events, analytics, audit TO eventshield_readonly;

COMMIT;
