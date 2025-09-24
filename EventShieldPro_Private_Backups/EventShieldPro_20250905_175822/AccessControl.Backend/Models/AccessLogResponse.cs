namespace AccessControl.Backend.Models;

public sealed record AccessLogResponse(
    Guid Id,
    Guid DeviceId,
    string? GuestName,
    string? GuestPhone,
    string Result,
    string? Details,
    DateTimeOffset Timestamp
);
