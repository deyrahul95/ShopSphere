namespace Shared.Contracts.Events.Order;

public record OrderCancelledEvent(Guid OrderId, Guid UserId, string Error);