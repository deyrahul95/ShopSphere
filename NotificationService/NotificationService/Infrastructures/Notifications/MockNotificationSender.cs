using Microsoft.Extensions.Logging;

namespace NotificationService.Infrastructures.Notifications;

public class MockNotificationSender(ILogger<MockNotificationSender> logger) : INotificationSender
{
    public Task SendAsync(string eventType, string message)
    {
        logger.LogInformation("[Notification] Type: {EventType}, Message: {Message}", eventType, message);
        return Task.CompletedTask;
    }
}
