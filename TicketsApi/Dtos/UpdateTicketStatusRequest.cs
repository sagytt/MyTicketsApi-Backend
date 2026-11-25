
using System.ComponentModel.DataAnnotations;
using TicketsApi.Models;

namespace TicketsApi.DTOs;

public class UpdateTicketStatusRequest
{
    [Required]
    public TicketStatus? Status { get; set; }
}
