namespace NotificationService.Infrastructures.Notifications;

public interface INotificationSender
{
    Task SendAsync(string eventType, string message);
}
