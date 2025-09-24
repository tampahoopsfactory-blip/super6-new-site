namespace AccessControl.Backend.Data.Entities;

public class GuestEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? FacePhotoUrl { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }

    public ICollection<TicketEntity> Tickets { get; set; } = new List<TicketEntity>();
}

