using MassTransit;
using NotificationService.Infrastructures.Notifications;
using Shared.Contracts.Events.Order;

namespace NotificationService.Features.OrderConfirmed;

public class OrderConfirmedConsumer(INotificationSender sender) : IConsumer<OrderConfirmedEvent>
{
    public Task Consume(ConsumeContext<OrderConfirmedEvent> context)
    {
        return sender.SendAsync(
            "OrderConfirmed",
            $"Order {context.Message.OrderId} has been confirmed for user {context.Message.OrderId}.");
    }
}