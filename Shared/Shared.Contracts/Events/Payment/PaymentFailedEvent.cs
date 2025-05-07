namespace Shared.Contracts.Events.Payment;

public record PaymentFailedEvent(Guid OrderId, Guid UserId, string Error);
