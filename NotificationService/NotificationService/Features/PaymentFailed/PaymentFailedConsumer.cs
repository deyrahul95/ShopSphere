using MassTransit;
using NotificationService.Infrastructures.Notifications;

namespace NotificationService.Features.PaymentFailed;

public class PaymentFailedConsumer(INotificationSender sender) : IConsumer<PaymentFailed>
{
    public Task Consume(ConsumeContext<PaymentFailed> context)
    {
        return sender.SendAsync(
            "PaymentFailed",
            $"Payment failed for order {context.Message.OrderId}. UserId: {context.Message.UserId}, Error: {context.Message.Error}");
    }
}