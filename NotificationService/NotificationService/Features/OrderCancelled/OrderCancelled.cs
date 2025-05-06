namespace NotificationService.Features.OrderCancelled;

public record OrderCancelled(Guid OrderId, Guid UserId, String Error);