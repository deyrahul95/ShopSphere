using System.ComponentModel.DataAnnotations;

namespace InventoryService.Application.Models;

public record UpdateInventoryRequest(
    [Required] int Quantity,
    bool IsOrdered
);
