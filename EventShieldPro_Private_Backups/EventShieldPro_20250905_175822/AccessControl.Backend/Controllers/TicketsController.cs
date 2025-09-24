using AccessControl.Backend.Models;
using AccessControl.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TicketsController : ControllerBase
{
    private readonly TicketService _ticketService;

    public TicketsController(TicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<TicketResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveTicketsAsync(CancellationToken cancellationToken)
    {
        var tickets = await _ticketService.GetActiveTicketsAsync(cancellationToken).ConfigureAwait(false);
        return Ok(tickets);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TicketResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTicketAsync([FromBody] TicketCreateRequest request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketService.CreateTicketAsync(request, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(GetActiveTicketsAsync), new { id = ticket.Id }, ticket);
    }

    [HttpPost("{ticketId:guid}/cancel")]
    [ProducesResponseType(typeof(TicketResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelTicketAsync(Guid ticketId, CancellationToken cancellationToken)
    {
        var ticket = await _ticketService.CancelTicketAsync(ticketId, cancellationToken).ConfigureAwait(false);
        if (ticket is null)
        {
            return NotFound();
        }

        return Ok(ticket);
    }

    [HttpPost("{ticketId:guid}/extend")]
    [ProducesResponseType(typeof(TicketResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExtendTicketAsync(Guid ticketId, [FromBody] TicketExtendRequest request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketService.ExtendTicketAsync(ticketId, TimeSpan.FromHours(request.Hours), cancellationToken).ConfigureAwait(false);
        if (ticket is null)
        {
            return NotFound();
        }

        return Ok(ticket);
    }
}
