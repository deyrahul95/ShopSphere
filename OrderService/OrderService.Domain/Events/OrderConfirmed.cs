namespace OrderService.Domain.Events;

public record OrderConfirmed(Guid OrderId, Guid UserId);