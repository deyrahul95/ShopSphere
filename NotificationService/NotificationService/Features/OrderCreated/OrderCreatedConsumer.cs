using MassTransit;
using NotificationService.Infrastructures.Notifications;
using Shared.Contracts.Events.Order;

namespace NotificationService.Features.OrderCreated;

public class OrderCreatedConsumer(INotificationSender sender) : IConsumer<OrderCreatedEvent>
{
    public Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        return sender.SendAsync(
            "OrderCreated",
            $"Order {context.Message.OrderId} created for {context.Message.UserId}.");
    }
}