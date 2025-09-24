using System.ComponentModel.DataAnnotations;

namespace AccessControl.Backend.Models;

public sealed record GuestCreateRequest(
    [property: Required, MaxLength(160)] string Name,
    [property: Required, Phone, MaxLength(32)] string Phone,
    [property: EmailAddress, MaxLength(160)] string? Email,
    [property: Url, MaxLength(512)] string? FacePhotoUrl
);

