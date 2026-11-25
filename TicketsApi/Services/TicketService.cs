
using Microsoft.EntityFrameworkCore;
using TicketsApi.Data;
using TicketsApi.DTOs;
using TicketsApi.Models;

namespace TicketsApi.Services;

public class TicketService
{
    private readonly AppDbContext _db;

    public TicketService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<TicketResponse> CreateAsync(
        CreateTicketRequest request,
        CancellationToken cancellationToken = default)
    {
        var ticket = new Ticket
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Priority = request.Priority!.Value,
            Status = TicketStatus.Open,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _db.Tickets.Add(ticket);

        await _db.SaveChangesAsync(cancellationToken);

        return TicketResponse.FromEntity(ticket);
    }

    public async Task<List<TicketResponse>> GetAllAsync(
        TicketStatus? status = null,
        TicketPriority? priority = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Ticket> query = _db.Tickets.AsNoTracking();

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        if (priority.HasValue)
        {
            query = query.Where(t => t.Priority == priority.Value);
        }

        var tickets = await query
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);

        return tickets
            .Select(TicketResponse.FromEntity)
            .ToList();
    }

    public async Task<TicketResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var ticket = await _db.Tickets
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        return ticket is null
            ? null
            : TicketResponse.FromEntity(ticket);
    }

    public async Task<bool> UpdateStatusAsync(
        int id,
        TicketStatus status,
        CancellationToken cancellationToken = default)
    {
        var ticket = await _db.Tickets
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (ticket is null)
        {
            return false;
        }

        ticket.Status = status;

        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var ticket = await _db.Tickets
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (ticket is null)
        {
            return false;
        }

        _db.Tickets.Remove(ticket);

        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }
}
