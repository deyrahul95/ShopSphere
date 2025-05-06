using MassTransit;
using NotificationService.Infrastructures.Notifications;

namespace NotificationService.Features.PaymentCompleted;

public class PaymentCompletedConsumer(INotificationSender sender) : IConsumer<PaymentCompleted>
{
    public Task Consume(ConsumeContext<PaymentCompleted> context)
    {
        return sender.SendAsync(
            "PaymentCompleted",
            $"Payment completed for order {context.Message.OrderId}. UserId: {context.Message.UserId}");
    }
}