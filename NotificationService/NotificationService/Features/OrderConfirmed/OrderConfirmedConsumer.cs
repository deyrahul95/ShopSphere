using MassTransit;
using NotificationService.Infrastructures.Notifications;

namespace NotificationService.Features.OrderConfirmed;

public class OrderConfirmedConsumer(INotificationSender sender) : IConsumer<OrderConfirmed>
{
    public Task Consume(ConsumeContext<OrderConfirmed> context)
    {
        return sender.SendAsync(
            "OrderConfirmed",
            $"Order {context.Message.OrderId} has been confirmed for user {context.Message.OrderId}.");
    }
}