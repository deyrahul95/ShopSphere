using System.Collections.Concurrent;
using System.Threading.Channels;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using OrderService.Application.Constants;
using OrderService.Application.Consumers;
using OrderService.Application.Handlers;
using OrderService.Application.Models;
using OrderService.Application.Services;
using OrderService.Application.Services.Interfaces;
using Polly;

namespace OrderService.Application.Extensions;

public static class ServiceConfigurations
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<CreateOrderValidator>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderCreatedConsumer>(
                cfg => cfg.UseMessageRetry(r => r.Interval(
                    MassTransitConstants.MaxRetryCount,
                    TimeSpan.FromSeconds(MassTransitConstants.RetryTimeSpanInSecond))));

            x.AddConsumer<OrderConfirmedConsumer>(
                cfg => cfg.UseMessageRetry(r => r.Interval(
                    MassTransitConstants.MaxRetryCount,
                    TimeSpan.FromSeconds(MassTransitConstants.RetryTimeSpanInSecond))));

            x.AddConsumer<OrderCancelledConsumer>(
                cfg => cfg.UseMessageRetry(r => r.Interval(
                    MassTransitConstants.MaxRetryCount,
                    TimeSpan.FromSeconds(MassTransitConstants.RetryTimeSpanInSecond))));

            x.AddConsumer<PaymentInitiatedConsumer>(
                cfg => cfg.UseMessageRetry(r => r.Interval(
                    MassTransitConstants.MaxRetryCount,
                    TimeSpan.FromSeconds(MassTransitConstants.RetryTimeSpanInSecond))));

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(MassTransitConstants.RabbitMqHost, "/", h =>
                {
                    h.Username(MassTransitConstants.RabbitMqDefaultUser);
                    h.Password(MassTransitConstants.RabbitMqDefaultPassword);
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
            client.BaseAddress = new Uri(configuration[HttpClientConstants.BaseAddress] ?? "");
        })
        .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()
        .AddResilienceHandler(
            HttpClientConstants.InventoryPipelineName,
            ConfigureDefaultResiliencePipeline);

        services.AddHttpClient<IPaymentHttpClient, PaymentHttpClient>(client =>
        {
            client.BaseAddress = new Uri(configuration[HttpClientConstants.BaseAddress] ?? "");
        })
        .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()
        .AddResilienceHandler(
            HttpClientConstants.PaymentPipelineName,
            ConfigureDefaultResiliencePipeline);

        services.AddScoped<IOrdersService, OrdersService>();

        services.AddSingleton(_ =>
        {
            var channel = Channel.CreateBounded<InventoryCheckJob>(new BoundedChannelOptions(100)
            {
                FullMode = BoundedChannelFullMode.Wait
            });

            return channel;
        });
        services.AddSingleton<ConcurrentDictionary<Guid, InventoryCheckStatus>>();
        services.AddHostedService<InventoryCheckService>();

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
