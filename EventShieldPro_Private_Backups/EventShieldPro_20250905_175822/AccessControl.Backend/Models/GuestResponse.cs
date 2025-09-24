namespace AccessControl.Backend.Models;

public sealed record GuestResponse(
    Guid Id,
    string Name,
    string Phone,
    string? Email,
    string? FacePhotoUrl,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset UpdatedAtUtc,
    int ActiveTickets
);

