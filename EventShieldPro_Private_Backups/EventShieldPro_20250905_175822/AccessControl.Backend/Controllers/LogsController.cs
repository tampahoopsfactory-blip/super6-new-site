using AccessControl.Backend.Data;
using AccessControl.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class LogsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public LogsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] int take = 100, CancellationToken cancellationToken = default)
    {
        take = Math.Clamp(take, 1, 500);

        var logs = await _dbContext.AccessLogs
            .AsNoTracking()
            .OrderByDescending(l => l.Timestamp)
            .Take(take)
            .Select(l => new AccessLogResponse(
                l.Id,
                l.DeviceId,
                l.Guest != null ? l.Guest.Name : null,
                l.Guest != null ? l.Guest.Phone : null,
                l.Result,
                l.Details,
                l.Timestamp))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return Ok(new { success = true, total = logs.Count, logs });
    }
}

