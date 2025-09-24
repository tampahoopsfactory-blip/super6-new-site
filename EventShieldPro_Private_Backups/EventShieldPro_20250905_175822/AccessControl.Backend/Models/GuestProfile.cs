namespace AccessControl.Backend.Models;

public sealed class GuestProfile
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string? FacePhotoUrl { get; init; }
}
