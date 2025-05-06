using MassTransit;
using NotificationService.Infrastructures.Notifications;

namespace NotificationService.Features.OrderCreated;

public class OrderCreatedConsumer(INotificationSender sender) : IConsumer<OrderCreated>
{
    public Task Consume(ConsumeContext<OrderCreated> context)
    {
        return sender.SendAsync(
            "OrderCreated",
            $"Order {context.Message.OrderId} created for {context.Message.UserId}.");
    }
}