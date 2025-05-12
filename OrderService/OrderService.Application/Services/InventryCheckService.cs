using System.Collections.Concurrent;
using System.Net;
using System.Threading.Channels;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderService.Application.Models;
using OrderService.Application.Services.Interfaces;
using Shared.Contracts.Events.Order;

namespace OrderService.Application.Services;

public class InventoryCheckService(
    ILogger<InventoryCheckService> logger,
    Channel<InventoryCheckJob> channel,
    IServiceScopeFactory scopeFactory,
    ConcurrentDictionary<Guid, InventoryCheckStatus> inventoryStatusDictionary) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var job in channel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                inventoryStatusDictionary[job.OrderId] = InventoryCheckStatus.Processing;

                using var scope = scopeFactory.CreateScope();
                var inventoryHttpClient = scope.ServiceProvider.GetRequiredService<IInventoryHttpClient>();
                var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

                await ProcessJobAsync(
                    job: job,
                    inventoryHttpClient: inventoryHttpClient,
                    publishEndpoint: publishEndpoint);
            }
            catch (Exception ex)
            {
                inventoryStatusDictionary[job.OrderId] = InventoryCheckStatus.Failed;
                logger.LogError(
                    ex,
                    "Error occurred during inventory check job. Order Id: {OrderId}",
                    job.OrderId);
            }
        }
    }

    private async Task ProcessJobAsync(
        InventoryCheckJob job,
        IInventoryHttpClient inventoryHttpClient,
        IPublishEndpoint publishEndpoint)
    {
        foreach (var request in job.InventoryRequests)
        {
            logger.LogInformation(
                "Start fetching inventory details.Product Id: {ProductId}",
                request.ProductId);

            var result = await inventoryHttpClient.CheckInventory(request: request, token: job.Token);

            if (result == null || result.StatusCode != HttpStatusCode.OK)
            {
                inventoryStatusDictionary[job.OrderId] = InventoryCheckStatus.Failed;
                logger.LogWarning(
                    "Failed to fetched inventory details. Result: {@Result}",
                    result);
                return;
            }

            logger.LogInformation(
                "Inventory details fetched successfully. Product Id: {ProductId}",
                request.ProductId);

            if (result.Data?.Available is false)
            {
                inventoryStatusDictionary[job.OrderId] = InventoryCheckStatus.Completed;
                logger.LogWarning("Product is out of stock.Product Id: {ProductId}", request.ProductId);

                await publishEndpoint.Publish(message: new OrderCancelledEvent(
                    OrderId: job.OrderId,
                    UserId: job.UserId,
                    Error: $"Order item with id: {request.ProductId} is out of stock"));
                return;
            }
        }

        inventoryStatusDictionary[job.OrderId] = InventoryCheckStatus.Completed;
        await publishEndpoint.Publish(message: new OrderConfirmedEvent(
            OrderId: job.OrderId,
            UserId: job.UserId));
        return;
    }
}
