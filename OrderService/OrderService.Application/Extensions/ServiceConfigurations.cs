using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using OrderService.Application.Constants;
using OrderService.Application.Handlers;
using OrderService.Application.Services;
using OrderService.Application.Services.Interfaces;
using Polly;

namespace OrderService.Application.Extensions;

public static class ServiceConfigurations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });
        });

        services.AddTransient<AuthenticatedHttpClientHandler>();

        services.AddHttpClient<ICartHttpClient, CartHttpClient>(client =>
        {
            client.BaseAddress = new Uri(configuration[HttpClientConstants.BaseAddress] ?? "");
        })
        .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()
        .AddResilienceHandler(
            HttpClientConstants.OrderPipelineName,
            ConfigureDefaultResiliencePipeline);

        services.AddHttpClient<IInventoryHttpClient, InventoryHttpClient>(client =>
        {
            client.BaseAddress = new Uri(configuration[HttpClientConstants.InventoryBaseAddress] ?? "");
        })
        .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()
        .AddResilienceHandler(
            HttpClientConstants.InventoryPipelineName,
            ConfigureDefaultResiliencePipeline);

        services.AddHttpClient<IPaymentHttpClient, PaymentHttpClient>(client =>
        {
            client.BaseAddress = new Uri(configuration[HttpClientConstants.PaymentBaseAddress] ?? "");
        })
        .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()
        .AddResilienceHandler(
            HttpClientConstants.PaymentPipelineName,
            ConfigureDefaultResiliencePipeline);

        services.AddScoped<IOrdersService, OrdersService>();

        return services;
    }

    private static void ConfigureDefaultResiliencePipeline(ResiliencePipelineBuilder<HttpResponseMessage> pipelineBuilder)
    {
        pipelineBuilder.AddTimeout(TimeSpan.FromSeconds(HttpClientConstants.RequestTimeoutInSeconds));

        pipelineBuilder.AddRetry(new HttpRetryStrategyOptions
        {
            MaxRetryAttempts = HttpClientConstants.MaxRetryCount,
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            Delay = TimeSpan.FromMilliseconds(HttpClientConstants.RetryDelayInMilliseconds)
        });

        pipelineBuilder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
        {
            SamplingDuration = TimeSpan.FromSeconds(HttpClientConstants.SamplingDurationInSeconds),
            FailureRatio = HttpClientConstants.FailureRatio,
            MinimumThroughput = HttpClientConstants.MinimumThroughput,
            BreakDuration = TimeSpan.FromSeconds(HttpClientConstants.BreakDurationInSeconds)
        });
    }
}
