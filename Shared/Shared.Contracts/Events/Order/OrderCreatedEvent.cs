namespace Shared.Contracts.Events.Order;

public record OrderCreatedEvent(Guid OrderId, Guid UserId);