namespace OrderService.Domain.Events;

public record OrderCreated(Guid OrderId, Guid UserId);
