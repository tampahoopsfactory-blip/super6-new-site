using AccessControl.Backend.Data;
using AccessControl.Backend.Data.Entities;
using AccessControl.Backend.Models;
using AccessControl.Domain.Models;
using AccessControl.Domain.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccessControl.Backend.Repositories;

public sealed class EfTicketRepository : ITicketRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<EfTicketRepository> _logger;

    public EfTicketRepository(ApplicationDbContext dbContext, ILogger<EfTicketRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task AddAsync(GuestProfile guest, Ticket ticket, CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;

        var guestEntity = await _dbContext.Guests
            .FirstOrDefaultAsync(g => g.Id == guest.Id, cancellationToken)
            .ConfigureAwait(false);

        if (guestEntity is null)
        {
            guestEntity = new GuestEntity
            {
                Id = guest.Id,
                Name = guest.Name,
                Phone = guest.Phone,
                Email = guest.Email,
                FacePhotoUrl = guest.FacePhotoUrl,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            };

            await _dbContext.Guests.AddAsync(guestEntity, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            guestEntity.Name = guest.Name;
            guestEntity.Phone = guest.Phone;
            guestEntity.Email = guest.Email;
            guestEntity.FacePhotoUrl = guest.FacePhotoUrl;
            guestEntity.UpdatedAtUtc = now;
        }

        var ticketEntity = new TicketEntity
        {
            Id = ticket.Id,
            Number = ticket.Number,
            Guest = guestEntity,
            GuestId = guestEntity.Id,
            Type = ticket.Type,
            Status = ticket.Status,
            PurchasedAt = ticket.PurchasedAt,
            ExpiresAt = ticket.ExpiresAt,
            VenueCode = ticket.VenueCode,
            GroupId = ticket.GroupId,
            FamilyMemberIndex = ticket.FamilyMemberIndex,
            PhoneExtension = ticket.PhoneExtension
        };

        await _dbContext.Tickets.AddAsync(ticketEntity, cancellationToken).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<(Ticket Ticket, GuestProfile Guest)>> GetActiveTicketsAsync(DateTimeOffset currentTime, CancellationToken cancellationToken = default)
    {
        var results = await _dbContext.Tickets
            .AsNoTracking()
            .Include(t => t.Guest)
            .Where(t => t.Status == TicketStatus.Active && t.ExpiresAt >= currentTime)
            .OrderBy(t => t.ExpiresAt)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return results
            .Select(MapToDomainTuple)
            .ToList();
    }

    public async Task<(Ticket Ticket, GuestProfile Guest)?> GetTicketWithGuestAsync(Guid ticketId, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Tickets
            .Include(t => t.Guest)
            .FirstOrDefaultAsync(t => t.Id == ticketId, cancellationToken)
            .ConfigureAwait(false);

        return entity is null ? null : MapToDomainTuple(entity);
    }

    public async Task UpdateAsync(Ticket ticket, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Tickets
            .FirstOrDefaultAsync(t => t.Id == ticket.Id, cancellationToken)
            .ConfigureAwait(false);

        if (entity is null)
        {
            _logger.LogWarning("Attempted to update ticket {TicketId} but it was not found", ticket.Id);
            return;
        }

        entity.Status = ticket.Status;
        entity.ExpiresAt = ticket.ExpiresAt;
        entity.PurchasedAt = ticket.PurchasedAt;
        entity.Type = ticket.Type;
        entity.VenueCode = ticket.VenueCode;
        entity.GroupId = ticket.GroupId;
        entity.FamilyMemberIndex = ticket.FamilyMemberIndex;
        entity.PhoneExtension = ticket.PhoneExtension;

        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private static (Ticket Ticket, GuestProfile Guest) MapToDomainTuple(TicketEntity entity)
    {
        var ticket = new Ticket
        {
            Id = entity.Id,
            Number = entity.Number,
            GuestId = entity.GuestId,
            Type = entity.Type,
            Status = entity.Status,
            PurchasedAt = entity.PurchasedAt,
            ExpiresAt = entity.ExpiresAt,
            VenueCode = entity.VenueCode,
            GroupId = entity.GroupId,
            FamilyMemberIndex = entity.FamilyMemberIndex,
            PhoneExtension = entity.PhoneExtension
        };

        var guest = new GuestProfile
        {
            Id = entity.Guest.Id,
            Name = entity.Guest.Name,
            Phone = entity.Guest.Phone,
            Email = entity.Guest.Email,
            FacePhotoUrl = entity.Guest.FacePhotoUrl
        };

        return (ticket, guest);
    }
}
