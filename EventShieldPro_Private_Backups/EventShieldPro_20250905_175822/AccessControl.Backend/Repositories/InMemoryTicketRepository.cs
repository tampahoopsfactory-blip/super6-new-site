using System.Collections.Concurrent;
using AccessControl.Backend.Models;
using AccessControl.Domain.Models;
using AccessControl.Domain.Tickets;

namespace AccessControl.Backend.Repositories;

public sealed class InMemoryTicketRepository : ITicketRepository
{
    private readonly ConcurrentDictionary<Guid, GuestProfile> _guests = new();
    private readonly ConcurrentDictionary<Guid, Ticket> _tickets = new();

    public Task AddAsync(GuestProfile guest, Ticket ticket, CancellationToken cancellationToken = default)
    {
        _guests.AddOrUpdate(guest.Id, guest, (_, _) => guest);
        _tickets[ticket.Id] = ticket;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<(Ticket Ticket, GuestProfile Guest)>> GetActiveTicketsAsync(DateTimeOffset currentTime, CancellationToken cancellationToken = default)
    {
        var activeTickets = _tickets.Values
            .Where(t => t.Status == TicketStatus.Active && t.ExpiresAt >= currentTime)
            .Select(ticket =>
            {
                _guests.TryGetValue(ticket.GuestId, out var guestProfile);
                guestProfile ??= new GuestProfile { Id = ticket.GuestId, Name = "Unknown", Phone = "" };
                return (ticket, guestProfile);
            })
            .OrderBy(tuple => tuple.ticket.PurchasedAt)
            .ToList();

        return Task.FromResult((IReadOnlyList<(Ticket, GuestProfile)>)activeTickets);
    }

    public Task<(Ticket Ticket, GuestProfile Guest)?> GetTicketWithGuestAsync(Guid ticketId, CancellationToken cancellationToken = default)
    {
        if (_tickets.TryGetValue(ticketId, out var ticket))
        {
            _guests.TryGetValue(ticket.GuestId, out var guestProfile);
            guestProfile ??= new GuestProfile { Id = ticket.GuestId, Name = "Unknown", Phone = string.Empty };
            return Task.FromResult<(Ticket, GuestProfile)?>( (ticket, guestProfile) );
        }

        return Task.FromResult<(Ticket, GuestProfile)?>(null);
    }

    public Task UpdateAsync(Ticket ticket, CancellationToken cancellationToken = default)
    {
        _tickets[ticket.Id] = ticket;
        return Task.CompletedTask;
    }
}
