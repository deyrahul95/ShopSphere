using MassTransit;
using NotificationService.Infrastructures.Notifications;
using Shared.Contracts.Events.Payment;

namespace NotificationService.Features.PaymentInitiated;

public class PaymentInitiatedConsumer(INotificationSender sender) : IConsumer<PaymentInitiatedEvent>
{
    public Task Consume(ConsumeContext<PaymentInitiatedEvent> context)
    {
        return sender.SendAsync(
             "PaymentInitiated",
             $"Payment initiated for order {context.Message.OrderId}. Amount: {context.Message.Amount}");
    }
}
