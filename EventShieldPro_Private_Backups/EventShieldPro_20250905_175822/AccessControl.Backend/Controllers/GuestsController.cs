using AccessControl.Backend.Data;
using AccessControl.Backend.Data.Entities;
using AccessControl.Backend.Models;
using AccessControl.Domain.Tickets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class GuestsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public GuestsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> SearchAsync([FromQuery] string? q, CancellationToken cancellationToken)
    {
        var query = _dbContext.Guests.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(q))
        {
            var pattern = q.Trim();
            query = query.Where(g => EF.Functions.Like(g.Name, $"%{pattern}%") || EF.Functions.Like(g.Phone, $"%{pattern}%"));
        }

        var now = DateTimeOffset.UtcNow;
        var guests = await query
            .OrderByDescending(g => g.UpdatedAtUtc)
            .Take(50)
            .Select(g => new GuestResponse(
                g.Id,
                g.Name,
                g.Phone,
                g.Email,
                g.FacePhotoUrl,
                g.CreatedAtUtc,
                g.UpdatedAtUtc,
                g.Tickets.Count(t => t.Status == TicketStatus.Active && t.ExpiresAt >= now)))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return Ok(new { success = true, total = guests.Count, guests });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var guest = await _dbContext.Guests
            .AsNoTracking()
            .Where(g => g.Id == id)
            .Select(g => new GuestResponse(
                g.Id,
                g.Name,
                g.Phone,
                g.Email,
                g.FacePhotoUrl,
                g.CreatedAtUtc,
                g.UpdatedAtUtc,
                g.Tickets.Count(t => t.Status == TicketStatus.Active && t.ExpiresAt >= now)))
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        if (guest is null)
        {
            return NotFound(new { success = false, error = "Guest not found" });
        }

        return Ok(new { success = true, guest });
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] GuestCreateRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var existing = await _dbContext.Guests
            .FirstOrDefaultAsync(g => g.Phone == request.Phone, cancellationToken)
            .ConfigureAwait(false);

        var now = DateTimeOffset.UtcNow;

        if (existing is not null)
        {
            existing.Name = request.Name;
            existing.Email = request.Email;
            existing.FacePhotoUrl = request.FacePhotoUrl;
            existing.UpdatedAtUtc = now;

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            var response = new GuestResponse(
                existing.Id,
                existing.Name,
                existing.Phone,
                existing.Email,
                existing.FacePhotoUrl,
                existing.CreatedAtUtc,
                existing.UpdatedAtUtc,
                await _dbContext.Tickets.CountAsync(t => t.GuestId == existing.Id && t.Status == TicketStatus.Active && t.ExpiresAt >= now, cancellationToken).ConfigureAwait(false));

            return Ok(new { success = true, guest = response, updated = true });
        }

        var guestId = Guid.NewGuid();
        var entity = new GuestEntity
        {
            Id = guestId,
            Name = request.Name,
            Phone = request.Phone,
            Email = request.Email,
            FacePhotoUrl = request.FacePhotoUrl,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        await _dbContext.Guests.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        var guestResponse = new GuestResponse(
            entity.Id,
            entity.Name,
            entity.Phone,
            entity.Email,
            entity.FacePhotoUrl,
            entity.CreatedAtUtc,
            entity.UpdatedAtUtc,
            0);

        return CreatedAtAction(nameof(GetAsync), new { id = entity.Id }, new { success = true, guest = guestResponse });
    }
}
