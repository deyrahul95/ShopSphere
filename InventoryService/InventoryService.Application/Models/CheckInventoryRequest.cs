using System.ComponentModel.DataAnnotations;

namespace InventoryService.Application.Models;

public record CheckInventoryRequest(
    [Required] Guid ProductId,
    [Required] int Quantity
);
