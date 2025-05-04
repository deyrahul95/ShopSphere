using System.ComponentModel.DataAnnotations;

namespace OrderService.Application.Models;

public class CreateOrderRequest
{
    [Required]
    public Guid CartId { get; set; }
}
