namespace OrderService.Application.Models;

public enum InventoryCheckStatus
{
    Queued,
    Processing,
    Completed,
    Failed
}
