namespace Shared.Contracts.Events.Payment;

public record PaymentInitiatedEvent(Guid OrderId, Guid UserId);
