namespace NotificationService.Features.PaymentCompleted;

public record PaymentCompleted(Guid OrderId, Guid UserId);
