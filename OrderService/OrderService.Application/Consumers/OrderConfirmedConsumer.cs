using MassTransit;
using Microsoft.Extensions.Logging;
using OrderService.Application.Models;
using OrderService.Application.Services.Interfaces;
using OrderService.Domain.Enums;
using Shared.Contracts.Events.Order;
using Shared.Contracts.Events.Payment;

namespace OrderService.Application.Consumers;

public class OrderConfirmedConsumer(
    IOrdersService ordersService,
    IPublishEndpoint publishEndpoint,
    ILogger<OrderConfirmedConsumer> logger) : IConsumer<OrderConfirmedEvent>
{
    public async Task Consume(ConsumeContext<OrderConfirmedEvent> context)
    {
        try
        {
            var request = new UpdateStatusRequest(
                OrderId: context.Message.OrderId,
                OrderState: OrderStatus.Confirmed,
                PaymentState: OrderPaymentStatus.Unpaid);

            await ordersService.UpdateOrderStatus(userId: context.Message.UserId, request: request);

            await publishEndpoint.Publish(new PaymentInitiatedEvent(
                OrderId: context.Message.OrderId,
                UserId: context.Message.UserId));
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to update order status in order confirmed consumer");
        }
    }
}
