using System.Net;
using CartService.Application.Services.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Events.Order;

namespace CartService.Application.Consumers;

public class OrderCreatedConsumer(
    ICartsService cartsService,
    ILogger<OrderCreatedConsumer> logger) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        logger.LogInformation("Start clearing cart details.");
        var serviceResult = await cartsService.ClearCart(context.Message.UserId);

        if (serviceResult.StatusCode != HttpStatusCode.OK)
        {
            logger.LogWarning("Failed to cleared cart details");
            return;
        }

        logger.LogInformation("Cart details cleared successfully");
        return;
    }
}
