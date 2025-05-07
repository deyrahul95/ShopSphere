using MassTransit;
using NotificationService.Infrastructures.Notifications;
using Shared.Contracts.Events.Payment;

namespace NotificationService.Features.PaymentFailed;

public class PaymentFailedConsumer(INotificationSender sender) : IConsumer<PaymentFailedEvent>
{
    public Task Consume(ConsumeContext<PaymentFailedEvent> context)
    {
        return sender.SendAsync(
            "PaymentFailed",
            $"Payment failed for order {context.Message.OrderId}. UserId: {context.Message.UserId}, Error: {context.Message.Error}");
    }
}