namespace Shared.Contracts.Events.Payment;

public record PaymentCompletedEvent(Guid OrderId, Guid UserId);