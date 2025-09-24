using AccessControl.Backend.Models;
using AccessControl.Domain.Models;

namespace AccessControl.Backend.Repositories;

public interface ITicketRepository
{
    Task AddAsync(GuestProfile guest, Ticket ticket, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<(Ticket Ticket, GuestProfile Guest)>> GetActiveTicketsAsync(DateTimeOffset currentTime, CancellationToken cancellationToken = default);
    Task<(Ticket Ticket, GuestProfile Guest)?> GetTicketWithGuestAsync(Guid ticketId, CancellationToken cancellationToken = default);
    Task UpdateAsync(Ticket ticket, CancellationToken cancellationToken = default);
}
