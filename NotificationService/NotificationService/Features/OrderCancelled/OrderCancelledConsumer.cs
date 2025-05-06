using MassTransit;
using NotificationService.Infrastructures.Notifications;

namespace NotificationService.Features.OrderCancelled;

public class OrderCancelledConsumer(INotificationSender sender) : IConsumer<OrderCancelled>
{
    public Task Consume(ConsumeContext<OrderCancelled> context)
    {
        return sender.SendAsync(
            "OrderCancelled",
            $"Order {context.Message.OrderId} has been cancelled for user {context.Message.UserId}. Error: {context.Message.Error}");
    }
}
