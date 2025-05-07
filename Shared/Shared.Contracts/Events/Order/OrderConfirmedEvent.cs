namespace Shared.Contracts.Events.Order;

public record OrderConfirmedEvent(Guid OrderId, Guid UserId);
