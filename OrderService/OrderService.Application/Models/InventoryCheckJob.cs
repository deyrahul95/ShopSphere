namespace OrderService.Application.Models;

public record InventoryCheckJob(
    Guid OrderId,
    Guid UserId,
    List<InventoryRequest> InventoryRequests,
    string? Token = null);