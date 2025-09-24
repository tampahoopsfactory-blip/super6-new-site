using System.ComponentModel.DataAnnotations;
using AccessControl.Domain.Tickets;

namespace AccessControl.Backend.Models;

public sealed record TicketCreateRequest(
    [property: Required] Guid GuestId,
    [property: Required] string GuestName,
    [property: Required] string Phone,
    [property: Required] TicketType Type,
    [property: Required] string VenueCode,
    DateTimeOffset? PurchaseTimeUtc
);
