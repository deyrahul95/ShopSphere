namespace NotificationService.Features.PaymentFailed;

public record PaymentFailed(Guid OrderId, Guid UserId, string Error);
