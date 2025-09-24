using AccessControl.Backend.Models;
using AccessControl.Backend.Repositories;
using AccessControl.Domain.Models;
using AccessControl.Domain.Tickets;
using AccessControl.Domain.Time;

namespace AccessControl.Backend.Services;

public sealed class TicketService
{
    private readonly TicketFactory _ticketFactory;
    private readonly ITicketRepository _ticketRepository;
    private readonly IDateTimeProvider _clock;

    public TicketService(TicketFactory ticketFactory, ITicketRepository ticketRepository, IDateTimeProvider clock)
    {
        _ticketFactory = ticketFactory;
        _ticketRepository = ticketRepository;
        _clock = clock;
    }

    public async Task<TicketResponse> CreateTicketAsync(TicketCreateRequest request, CancellationToken cancellationToken = default)
    {
        var guestProfile = new GuestProfile
        {
            Id = request.GuestId,
            Name = request.GuestName,
            Phone = request.Phone
        };

        var ticket = _ticketFactory.CreateTicket(request.GuestId, request.Type, request.VenueCode, request.PurchaseTimeUtc);
        await _ticketRepository.AddAsync(guestProfile, ticket, cancellationToken).ConfigureAwait(false);
        return TicketResponse.FromTicket(ticket, guestProfile.Name, guestProfile.Phone);
    }

    public async Task<IReadOnlyList<TicketResponse>> GetActiveTicketsAsync(CancellationToken cancellationToken = default)
    {
        var now = _clock.UtcNow;
        var active = await _ticketRepository.GetActiveTicketsAsync(now, cancellationToken).ConfigureAwait(false);

        return active
            .Select(tuple => TicketResponse.FromTicket(tuple.Ticket, tuple.Guest.Name, tuple.Guest.Phone))
            .ToList();
    }

    public async Task<TicketResponse?> CancelTicketAsync(Guid ticketId, CancellationToken cancellationToken = default)
    {
        var existing = await _ticketRepository.GetTicketWithGuestAsync(ticketId, cancellationToken).ConfigureAwait(false);

        if (existing is null)
        {
            return null;
        }

        var (ticket, guest) = existing.Value;
        ticket.Revoke();
        await _ticketRepository.UpdateAsync(ticket, cancellationToken).ConfigureAwait(false);

        return TicketResponse.FromTicket(ticket, guest.Name, guest.Phone);
    }

    public async Task<TicketResponse?> ExtendTicketAsync(Guid ticketId, TimeSpan extension, CancellationToken cancellationToken = default)
    {
        if (extension <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(extension), "Extension duration must be positive.");
        }

        var existing = await _ticketRepository.GetTicketWithGuestAsync(ticketId, cancellationToken).ConfigureAwait(false);

        if (existing is null)
        {
            return null;
        }

        var (ticket, guest) = existing.Value;
        ticket.Extend(extension);
        await _ticketRepository.UpdateAsync(ticket, cancellationToken).ConfigureAwait(false);

        return TicketResponse.FromTicket(ticket, guest.Name, guest.Phone);
    }
}
