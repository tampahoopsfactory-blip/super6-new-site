-- EventShield Pro - Database Initialization Script
-- Azure SQL Database Setup

-- Create database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'eventshield_pro')
BEGIN
    CREATE DATABASE eventshield_pro;
END
GO

USE eventshield_pro;
GO

-- Enable JSON support
IF NOT EXISTS (SELECT * FROM sys.extended_properties WHERE name = 'JSON_SUPPORT')
BEGIN
    EXEC sp_addextendedproperty 'JSON_SUPPORT', 'true';
END
GO

-- Create schemas
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'auth')
BEGIN
    CREATE SCHEMA auth;
END
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'events')
BEGIN
    CREATE SCHEMA events;
END
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'licensing')
BEGIN
    CREATE SCHEMA licensing;
END
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'hardware')
BEGIN
    CREATE SCHEMA hardware;
END
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'analytics')
BEGIN
    CREATE SCHEMA analytics;
END
GO

-- Create custom types
IF NOT EXISTS (SELECT * FROM sys.types WHERE name = 'EventStatus')
BEGIN
    CREATE TYPE EventStatus AS TABLE (
        value NVARCHAR(50) NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.types WHERE name = 'EventVisibility')
BEGIN
    CREATE TYPE EventVisibility AS TABLE (
        value NVARCHAR(50) NOT NULL
    );
END
GO

-- Create indexes for better performance
CREATE NONCLUSTERED INDEX IX_Users_Email ON Users(Email);
CREATE NONCLUSTERED INDEX IX_Users_Username ON Users(Username);
CREATE NONCLUSTERED INDEX IX_Users_TenantId ON Users(TenantId);

CREATE NONCLUSTERED INDEX IX_Events_Slug ON Events(Slug);
CREATE NONCLUSTERED INDEX IX_Events_TenantId ON Events(TenantId);
CREATE NONCLUSTERED INDEX IX_Events_Status ON Events(Status);
CREATE NONCLUSTERED INDEX IX_Events_OrganizerId ON Events(OrganizerId);

CREATE NONCLUSTERED INDEX IX_Tickets_EventId ON Tickets(EventId);
CREATE NONCLUSTERED INDEX IX_Tickets_AttendeeId ON Tickets(AttendeeId);
CREATE NONCLUSTERED INDEX IX_Tickets_Status ON Tickets(Status);

CREATE NONCLUSTERED INDEX IX_AccessLogs_EventId ON AccessLogs(EventId);
CREATE NONCLUSTERED INDEX IX_AccessLogs_UserId ON AccessLogs(UserId);
CREATE NONCLUSTERED INDEX IX_AccessLogs_Timestamp ON AccessLogs(Timestamp);

-- Create views for common queries
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_EventSummary')
    DROP VIEW vw_EventSummary;
GO

CREATE VIEW vw_EventSummary AS
SELECT 
    e.Id,
    e.Title,
    e.Slug,
    e.Status,
    e.Visibility,
    e.MaxCapacity,
    e.CurrentRegistrations,
    e.IsFree,
    e.Price,
    e.Currency,
    e.TenantId,
    t.Name AS TenantName,
    u.FirstName + ' ' + u.LastName AS OrganizerName,
    e.CreatedAt,
    e.UpdatedAt
FROM Events e
INNER JOIN Tenants t ON e.TenantId = t.Id
INNER JOIN Users u ON e.OrganizerId = u.Id;
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_UserSummary')
    DROP VIEW vw_UserSummary;
GO

CREATE VIEW vw_UserSummary AS
SELECT 
    u.Id,
    u.Email,
    u.Username,
    u.FirstName,
    u.LastName,
    u.IsActive,
    u.IsVerified,
    u.TenantId,
    t.Name AS TenantName,
    t.SubscriptionTier,
    u.CreatedAt,
    u.LastLoginAt
FROM Users u
INNER JOIN Tenants t ON u.TenantId = t.Id;
GO

-- Create stored procedures for common operations
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetEventStats')
    DROP PROCEDURE sp_GetEventStats;
GO

CREATE PROCEDURE sp_GetEventStats
    @TenantId INT = NULL,
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        e.Id,
        e.Title,
        e.Status,
        e.MaxCapacity,
        e.CurrentRegistrations,
        ISNULL(e.MaxCapacity, 0) - e.CurrentRegistrations AS AvailableSpots,
        CASE 
            WHEN e.MaxCapacity IS NULL OR e.MaxCapacity = 0 THEN 0
            ELSE (e.CurrentRegistrations * 100.0) / e.MaxCapacity
        END AS RegistrationPercentage,
        COUNT(t.Id) AS TotalTickets,
        COUNT(CASE WHEN t.Status = 'valid' THEN 1 END) AS ValidTickets,
        COUNT(CASE WHEN t.Status = 'used' THEN 1 END) AS UsedTickets,
        COUNT(CASE WHEN t.Status = 'cancelled' THEN 1 END) AS CancelledTickets
    FROM Events e
    LEFT JOIN Tickets t ON e.Id = t.EventId
    WHERE (@TenantId IS NULL OR e.TenantId = @TenantId)
        AND (@StartDate IS NULL OR e.CreatedAt >= @StartDate)
        AND (@EndDate IS NULL OR e.CreatedAt <= @EndDate)
    GROUP BY e.Id, e.Title, e.Status, e.MaxCapacity, e.CurrentRegistrations;
END
GO

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetUserActivity')
    DROP PROCEDURE sp_GetUserActivity;
GO

CREATE PROCEDURE sp_GetUserActivity
    @UserId INT,
    @Days INT = 30
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @StartDate DATETIME = DATEADD(DAY, -@Days, GETUTCDATE());
    
    SELECT 
        'Events Created' AS ActivityType,
        e.Title AS Description,
        e.CreatedAt AS Timestamp,
        e.Id AS RelatedId
    FROM Events e
    WHERE e.OrganizerId = @UserId AND e.CreatedAt >= @StartDate
    
    UNION ALL
    
    SELECT 
        'Tickets Purchased' AS ActivityType,
        CONCAT('Ticket for ', e.Title) AS Description,
        t.CreatedAt AS Timestamp,
        t.Id AS RelatedId
    FROM Tickets t
    INNER JOIN Events e ON t.EventId = e.Id
    WHERE t.AttendeeId = @UserId AND t.CreatedAt >= @StartDate
    
    UNION ALL
    
    SELECT 
        'Access Logs' AS ActivityType,
        CONCAT('Access to ', e.Title) AS Description,
        al.Timestamp,
        al.Id AS RelatedId
    FROM AccessLogs al
    INNER JOIN Events e ON al.EventId = e.Id
    WHERE al.UserId = @UserId AND al.Timestamp >= @StartDate
    
    ORDER BY Timestamp DESC;
END
GO

-- Create functions for data validation
IF EXISTS (SELECT * FROM sys.objects WHERE name = 'fn_ValidateEmail')
    DROP FUNCTION fn_ValidateEmail;
GO

CREATE FUNCTION fn_ValidateEmail(@Email NVARCHAR(255))
RETURNS BIT
AS
BEGIN
    DECLARE @IsValid BIT = 0;
    
    IF @Email LIKE '%_@__%.__%' 
        AND @Email NOT LIKE '@%' 
        AND @Email NOT LIKE '%@' 
        AND @Email NOT LIKE '%..%'
        AND @Email NOT LIKE '%@%@%'
    BEGIN
        SET @IsValid = 1;
    END
    
    RETURN @IsValid;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE name = 'fn_CalculateEventRevenue')
    DROP FUNCTION fn_CalculateEventRevenue;
GO

CREATE FUNCTION fn_CalculateEventRevenue(@EventId INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @TotalRevenue DECIMAL(10,2) = 0;
    
    SELECT @TotalRevenue = ISNULL(SUM(t.Price), 0)
    FROM Tickets t
    WHERE t.EventId = @EventId 
        AND t.Status IN ('valid', 'used')
        AND t.Price IS NOT NULL;
    
    RETURN @TotalRevenue;
END
GO

-- Create triggers for data integrity
IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'tr_UpdateEventRegistrationCount')
    DROP TRIGGER tr_UpdateEventRegistrationCount;
GO

CREATE TRIGGER tr_UpdateEventRegistrationCount
ON Tickets
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @EventId INT;
    
    -- Handle INSERT
    IF EXISTS (SELECT * FROM INSERTED)
    BEGIN
        SELECT @EventId = EventId FROM INSERTED;
        UPDATE Events 
        SET CurrentRegistrations = (
            SELECT COUNT(*) 
            FROM Tickets 
            WHERE EventId = @EventId AND Status IN ('valid', 'used')
        )
        WHERE Id = @EventId;
    END
    
    -- Handle DELETE
    IF EXISTS (SELECT * FROM DELETED)
    BEGIN
        SELECT @EventId = EventId FROM DELETED;
        UPDATE Events 
        SET CurrentRegistrations = (
            SELECT COUNT(*) 
            FROM Tickets 
            WHERE EventId = @EventId AND Status IN ('valid', 'used')
        )
        WHERE Id = @EventId;
    END
END
GO

-- Create indexes for better performance
CREATE NONCLUSTERED INDEX IX_Tenants_Slug ON Tenants(Slug);
CREATE NONCLUSTERED INDEX IX_Tenants_Domain ON Tenants(Domain);
CREATE NONCLUSTERED INDEX IX_Tenants_SubscriptionTier ON Tenants(SubscriptionTier);

CREATE NONCLUSTERED INDEX IX_EventCategories_Slug ON EventCategories(Slug);
CREATE NONCLUSTERED INDEX IX_EventCategories_IsActive ON EventCategories(IsActive);

CREATE NONCLUSTERED INDEX IX_EventLocations_City ON EventLocations(City);
CREATE NONCLUSTERED INDEX IX_EventLocations_Country ON EventLocations(Country);
CREATE NONCLUSTERED INDEX IX_EventLocations_IsVirtual ON EventLocations(IsVirtual);

CREATE NONCLUSTERED INDEX IX_EventSchedules_EventId ON EventSchedules(EventId);
CREATE NONCLUSTERED INDEX IX_EventSchedules_StartDatetime ON EventSchedules(StartDatetime);
CREATE NONCLUSTERED INDEX IX_EventSchedules_EndDatetime ON EventSchedules(EndDatetime);

CREATE NONCLUSTERED INDEX IX_TicketTypes_EventId ON TicketTypes(EventId);
CREATE NONCLUSTERED INDEX IX_TicketTypes_IsActive ON TicketTypes(IsActive);

CREATE NONCLUSTERED INDEX IX_TicketPurchases_TicketId ON TicketPurchases(TicketId);
CREATE NONCLUSTERED INDEX IX_TicketPurchases_PurchaseDate ON TicketPurchases(PurchaseDate);

CREATE NONCLUSTERED INDEX IX_TicketValidations_TicketId ON TicketValidations(TicketId);
CREATE NONCLUSTERED INDEX IX_TicketValidations_ValidationTime ON TicketValidations(ValidationTime);

CREATE NONCLUSTERED INDEX IX_AccessPoints_EventId ON AccessPoints(EventId);
CREATE NONCLUSTERED INDEX IX_AccessPoints_IsActive ON AccessPoints(IsActive);

CREATE NONCLUSTERED INDEX IX_AccessPermissions_UserId ON AccessPermissions(UserId);
CREATE NONCLUSTERED INDEX IX_AccessPermissions_EventId ON AccessPermissions(EventId);

CREATE NONCLUSTERED INDEX IX_Licenses_TenantId ON Licenses(TenantId);
CREATE NONCLUSTERED INDEX IX_Licenses_Status ON Licenses(Status);
CREATE NONCLUSTERED INDEX IX_Licenses_ExpiresAt ON Licenses(ExpiresAt);

CREATE NONCLUSTERED INDEX IX_LicenseFeatures_LicenseId ON LicenseFeatures(LicenseId);
CREATE NONCLUSTERED INDEX IX_LicenseFeatures_FeatureName ON LicenseFeatures(FeatureName);

CREATE NONCLUSTERED INDEX IX_LicenseUsage_LicenseId ON LicenseUsage(LicenseId);
CREATE NONCLUSTERED INDEX IX_LicenseUsage_UsageDate ON LicenseUsage(UsageDate);

CREATE NONCLUSTERED INDEX IX_BillingAccounts_TenantId ON BillingAccounts(TenantId);
CREATE NONCLUSTERED INDEX IX_BillingAccounts_Status ON BillingAccounts(Status);

CREATE NONCLUSTERED INDEX IX_Invoices_BillingAccountId ON Invoices(BillingAccountId);
CREATE NONCLUSTERED INDEX IX_Invoices_InvoiceDate ON Invoices(InvoiceDate);
CREATE NONCLUSTERED INDEX IX_Invoices_Status ON Invoices(Status);

CREATE NONCLUSTERED INDEX IX_Payments_InvoiceId ON Payments(InvoiceId);
CREATE NONCLUSTERED INDEX IX_Payments_PaymentDate ON Payments(PaymentDate);
CREATE NONCLUSTERED INDEX IX_Payments_Status ON Payments(Status);

CREATE NONCLUSTERED INDEX IX_Subscriptions_TenantId ON Subscriptions(TenantId);
CREATE NONCLUSTERED INDEX IX_Subscriptions_Status ON Subscriptions(Status);
CREATE NONCLUSTERED INDEX IX_Subscriptions_CurrentPeriodEnd ON Subscriptions(CurrentPeriodEnd);

CREATE NONCLUSTERED INDEX IX_HardwareDevices_TenantId ON HardwareDevices(TenantId);
CREATE NONCLUSTERED INDEX IX_HardwareDevices_DeviceType ON HardwareDevices(DeviceType);
CREATE NONCLUSTERED INDEX IX_HardwareDevices_Status ON HardwareDevices(Status);

CREATE NONCLUSTERED INDEX IX_DeviceLogs_DeviceId ON DeviceLogs(DeviceId);
CREATE NONCLUSTERED INDEX IX_DeviceLogs_Timestamp ON DeviceLogs(Timestamp);
CREATE NONCLUSTERED INDEX IX_DeviceLogs_LogLevel ON DeviceLogs(LogLevel);

CREATE NONCLUSTERED INDEX IX_DeviceStatus_DeviceId ON DeviceStatus(DeviceId);
CREATE NONCLUSTERED INDEX IX_DeviceStatus_Timestamp ON DeviceStatus(Timestamp);

CREATE NONCLUSTERED INDEX IX_AnalyticsEvents_TenantId ON AnalyticsEvents(TenantId);
CREATE NONCLUSTERED INDEX IX_AnalyticsEvents_EventType ON AnalyticsEvents(EventType);
CREATE NONCLUSTERED INDEX IX_AnalyticsEvents_Timestamp ON AnalyticsEvents(Timestamp);

CREATE NONCLUSTERED INDEX IX_UserActivity_UserId ON UserActivity(UserId);
CREATE NONCLUSTERED INDEX IX_UserActivity_ActivityType ON UserActivity(ActivityType);
CREATE NONCLUSTERED INDEX IX_UserActivity_Timestamp ON UserActivity(Timestamp);

CREATE NONCLUSTERED INDEX IX_SystemMetrics_Timestamp ON SystemMetrics(Timestamp);
CREATE NONCLUSTERED INDEX IX_SystemMetrics_MetricName ON SystemMetrics(MetricName);

-- Grant permissions
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::auth TO [eventshield_app];
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::events TO [eventshield_app];
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::licensing TO [eventshield_app];
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::hardware TO [eventshield_app];
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::analytics TO [eventshield_app];

-- Create database user if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'eventshield_app')
BEGIN
    CREATE USER [eventshield_app] FOR LOGIN [eventshield_app];
END
GO

-- Grant permissions to the application user
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO [eventshield_app];
GRANT EXECUTE ON SCHEMA::dbo TO [eventshield_app];

PRINT 'EventShield Pro database initialization completed successfully!';
GO
