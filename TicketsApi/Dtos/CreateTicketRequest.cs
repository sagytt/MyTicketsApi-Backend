using System.ComponentModel.DataAnnotations;
using TicketsApi.Models;

namespace TicketsApi.DTOs;

public class CreateTicketRequest
{
    [Required]
    public string Title { get; set; } = "";

    [Required]
    public string Description { get; set; } = "";

    [Required]
    public TicketPriority? Priority { get; set; }
}