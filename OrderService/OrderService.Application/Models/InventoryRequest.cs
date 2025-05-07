using System.ComponentModel.DataAnnotations;

namespace OrderService.Application.Models;

public record InventoryRequest(
    [Required] Guid ProductId,
    [Required] int Quantity
);
