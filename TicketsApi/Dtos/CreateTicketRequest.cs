
using System.ComponentModel.DataAnnotations;
using TicketsApi.Models;
using TicketsApi.Validation;

namespace TicketsApi.DTOs;

public class CreateTicketRequest
{
    [Required]
    [NotBlank]
    public string Title { get; set; } = "";

    [Required]
    [NotBlank]
    public string Description { get; set; } = "";

    [Required]
    public TicketPriority? Priority { get; set; }
}
