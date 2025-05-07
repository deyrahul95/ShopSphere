using MassTransit;
using NotificationService.Infrastructures.Notifications;
using Shared.Contracts.Events.Order;

namespace NotificationService.Features.OrderCancelled;

public class OrderCancelledConsumer(INotificationSender sender) : IConsumer<OrderCancelledEvent>
{
    public Task Consume(ConsumeContext<OrderCancelledEvent> context)
    {
        return sender.SendAsync(
            "OrderCancelled",
            $"Order {context.Message.OrderId} has been cancelled for user {context.Message.UserId}. Error: {context.Message.Error}");
    }
}
