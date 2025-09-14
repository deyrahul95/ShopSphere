using CartService.Application.Constants;
using CartService.Application.Consumers;
using CartService.Application.Handlers;
using CartService.Application.Models;
using CartService.Application.Services;
using CartService.Application.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Shared.Contracts.Constants;

namespace CartService.Application.Extensions;

public static class ServiceConfigurations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblyContaining<AddToCartValidator>()
            .AddValidatorsFromAssemblyContaining<RemovedFromCartValidator>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderCreatedConsumer>(
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

                cfg.ReceiveEndpoint(MassTransitConstants.NotificationQueueName, ep =>
               {
                   ep.ConfigureConsumer<OrderCreatedConsumer>(context);

                   ep.UseMessageRetry(
                       r => r.Interval(
                           MassTransitConstants.MaxRetryCount,
                           TimeSpan.FromSeconds(MassTransitConstants.RetryTimeSpanInSecond)));
                   ep.UseInMemoryOutbox(context);

                   ep.DeadLetterExchange = MassTransitConstants.DeadLetterExchangeName;
               });
            });
        });

        services.AddTransient<AuthenticatedHttpClientHandler>();

        services.AddHttpClient<IProductHttpClient, ProductHttpClient>(client =>
        {
            client.BaseAddress = new Uri(configuration[HttpClientConstants.BaseAddress] ?? "");
        })
        .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()
        .AddResilienceHandler(
            HttpClientConstants.ProductPipelineName,
            ConfigureDefaultResiliencePipeline);

        services.AddScoped<ICartsService, CartsService>();

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
