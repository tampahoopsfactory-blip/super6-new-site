using AccessControl.Domain.Tickets;

namespace AccessControl.Backend.Data.Entities;

public class TicketEntity
{
    public Guid Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public Guid GuestId { get; set; }
    public TicketType Type { get; set; }
    public TicketStatus Status { get; set; }
    public DateTimeOffset PurchasedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public string VenueCode { get; set; } = string.Empty;
    public Guid? GroupId { get; set; }
    public int? FamilyMemberIndex { get; set; }
    public string? PhoneExtension { get; set; }

    public GuestEntity Guest { get; set; } = null!;
}

