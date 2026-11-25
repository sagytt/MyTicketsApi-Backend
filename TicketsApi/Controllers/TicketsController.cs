
using Microsoft.AspNetCore.Mvc;
using TicketsApi.DTOs;
using TicketsApi.Models;
using TicketsApi.Query;
using TicketsApi.Services;

namespace TicketsApi.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketsController : ControllerBase
{
    private readonly TicketService _ticketService;

    public TicketsController(TicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpPost]
    public async Task<ActionResult<TicketResponse>> Create(
        CreateTicketRequest request,
        CancellationToken cancellationToken)
    {
        var ticket = await _ticketService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = ticket.Id },
            ticket);
    }

    [HttpGet]
    public async Task<ActionResult<List<TicketResponse>>> GetAll(
        [FromQuery] string? status,
        [FromQuery] string? priority,
        CancellationToken cancellationToken)
    {
        TicketStatus? parsedStatus = null;
        TicketPriority? parsedPriority = null;

        if (Request.Query.ContainsKey("status"))
        {
            if (!EnumQueryParser.TryParse<TicketStatus>(
                status,
                out var result))
            {
                return BadRequest("Invalid status value.");
            }

            parsedStatus = result;
        }

        if (Request.Query.ContainsKey("priority"))
        {
            if (!EnumQueryParser.TryParse<TicketPriority>(
                priority,
                out var result))
            {
                return BadRequest("Invalid priority value.");
            }

            parsedPriority = result;
        }

        var tickets = await _ticketService.GetAllAsync(
            parsedStatus,
            parsedPriority,
            cancellationToken);

        return Ok(tickets);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TicketResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var ticket = await _ticketService.GetByIdAsync(
            id,
            cancellationToken);

        if (ticket is null)
        {
            return NotFound();
        }

        return Ok(ticket);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        UpdateTicketStatusRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await _ticketService.UpdateStatusAsync(
            id,
            request.Status!.Value,
            cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var deleted = await _ticketService.DeleteAsync(
            id,
            cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
