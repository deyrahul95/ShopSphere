using System.ComponentModel.DataAnnotations;

namespace OrderService.Application.Models;

public record CheckInventoryRequest(
    [Required] Guid ProductId,
    [Required] int Quantity
);
