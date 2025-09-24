using AccessControl.Domain.Models;
using AccessControl.Domain.Tickets;

namespace AccessControl.Backend.Models;

public sealed record TicketResponse(
    Guid Id,
    string Number,
    Guid GuestId,
    string GuestName,
    string Phone,
    TicketType Type,
    TicketStatus Status,
    DateTimeOffset PurchasedAt,
    DateTimeOffset ExpiresAt,
    string VenueCode
)
{
    public static TicketResponse FromTicket(Ticket ticket, string guestName, string phone)
    {
        return new TicketResponse(
            ticket.Id,
            ticket.Number,
            ticket.GuestId,
            guestName,
            phone,
            ticket.Type,
            ticket.Status,
            ticket.PurchasedAt,
            ticket.ExpiresAt,
            ticket.VenueCode
        );
    }
}
