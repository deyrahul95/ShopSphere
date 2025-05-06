namespace OrderService.Domain.Events;

public record OrderCancelled(Guid OrderId, Guid UserId, String Error);