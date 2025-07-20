using System.Net;
using MassTransit;
using Microsoft.Extensions.Logging;
using OrderService.Application.Models;
using OrderService.Application.Services.Interfaces;
using OrderService.Domain.Enums;
using OrderService.Domain.Repositories;
using Shared.Contracts.Events.Payment;

namespace OrderService.Application.Consumers;

public class PaymentInitiatedConsumer(
    IOrderRepository orderRepository,
    IPaymentHttpClient paymentHttpClient,
    ILogger<PaymentInitiatedConsumer> logger) : IConsumer<PaymentInitiatedEvent>
{
    public async Task Consume(ConsumeContext<PaymentInitiatedEvent> context)
    {
        try
        {
            var orderId = context.Message.OrderId;
            logger.LogInformation("Fetching order details. Order Id: {OrderId}", orderId);
            var order = await orderRepository.GetOrder(
                userId: context.Message.UserId,
                orderId: orderId);

            if (order is null)
            {
                logger.LogWarning("Order not found. Order Id: {OrderId}", orderId);
                return;
            }

            Enum.TryParse(
                value: order.PaymentMode,
                ignoreCase: true,
                result: out PaymentMode paymentMode);

            var paymentStatus = await MakePayment(
                orderId: orderId,
                amount: order.TotalAmount,
                paymentMode: paymentMode);

            order.UpdatePaymentState(paymentStatus);
            await orderRepository.UpdateOrder(order);
        }
        catch (Exception ex)
        {
            logger.LogError(
                exception: ex,
                message: "Failed to process the payment. Order Id: {OrderId}",
                args: context.Message.OrderId);
        }
    }

    private async Task<OrderPaymentStatus> MakePayment(
        Guid orderId,
        decimal amount,
        PaymentMode paymentMode)
    {
        logger.LogInformation("Start make payment.");
        var request = new ProcessPaymentRequest
        {
            OrderId = orderId,
            Amount = amount,
            PaymentMode = paymentMode
        };

        var result = await paymentHttpClient.ProcessPayment(request: request);

        if (result == null || result.StatusCode != HttpStatusCode.OK)
        {
            logger.LogWarning(
                "Failed to make payment. Result: {@Result}",
                result);
            return OrderPaymentStatus.Failed;
        }

        logger.LogInformation(
            "Payment completed. Result: {@Result}",
            result);

        var paymentDto = result.Data;

        if (paymentDto is null)
        {
            return OrderPaymentStatus.Failed;
        }

        if (paymentDto.PaymentStatus.Equals(
            "Completed",
            StringComparison.OrdinalIgnoreCase))
        {
            return OrderPaymentStatus.Paid;
        }

        return OrderPaymentStatus.Failed;
    }
}
