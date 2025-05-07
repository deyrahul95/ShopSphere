using System.Net;
using MassTransit;
using Microsoft.Extensions.Logging;
using OrderService.Application.Services.Interfaces;
using Shared.Contracts.Events.Order;

namespace OrderService.Application.Consumers;

public class OrderCreatedConsumer(
    ICartHttpClient cartHttpClient,
    ILogger<OrderCreatedConsumer> logger) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        await ClearCart();
    }

    private async Task ClearCart()
    {
        logger.LogInformation("Start clearing cart details.");
        var statusCode = await cartHttpClient.ClearCart();

        if (statusCode != HttpStatusCode.NoContent)
        {
            logger.LogWarning("Failed to cleared cart details");
            return;
        }

        logger.LogInformation("Cart details cleared successfully");
        return;
    }
}
