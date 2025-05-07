using MassTransit;
using NotificationService.Infrastructures.Notifications;
using Shared.Contracts.Events.Payment;

namespace NotificationService.Features.PaymentCompleted;

public class PaymentCompletedConsumer(INotificationSender sender) : IConsumer<PaymentCompletedEvent>
{
    public Task Consume(ConsumeContext<PaymentCompletedEvent> context)
    {
        return sender.SendAsync(
            "PaymentCompleted",
            $"Payment completed for order {context.Message.OrderId}. UserId: {context.Message.UserId}");
    }
}