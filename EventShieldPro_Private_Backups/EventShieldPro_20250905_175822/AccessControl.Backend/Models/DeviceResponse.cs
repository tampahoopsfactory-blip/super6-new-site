namespace AccessControl.Backend.Models;

public sealed record DeviceResponse(
    Guid Id,
    string Name,
    string IpAddress,
    int Port,
    string DeviceType,
    string Location,
    bool IsOnline,
    string Status,
    DateTimeOffset LastSyncUtc
);
