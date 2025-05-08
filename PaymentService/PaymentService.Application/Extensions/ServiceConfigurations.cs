using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using PaymentService.Application.Constants;
using PaymentService.Application.Handlers;
using PaymentService.Application.Models;
using PaymentService.Application.Services;
using PaymentService.Application.Services.Interfaces;
using Polly;

namespace PaymentService.Application.Extensions;

public static class ServiceConfigurations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<ProcessPaymentValidator>();

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

        services.AddHttpClient<IOrderHttpClient, OrderHttpClient>(client =>
        {
            client.BaseAddress = new Uri(configuration[HttpClientConstants.BaseAddress] ?? "");
        })
        .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()
        .AddResilienceHandler(
            HttpClientConstants.OrderPipelineName,
            ConfigureDefaultResiliencePipeline);

        services.AddSingleton<RetryHandler>();
        services.AddScoped<IPaymentService, PaymentServiceImpl>();

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
