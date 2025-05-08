using CartService.Application.Constants;
using CartService.Application.Handlers;
using CartService.Application.Models;
using CartService.Application.Services;
using CartService.Application.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace CartService.Application.Extensions;

public static class ServiceConfigurations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblyContaining<AddToCartValidator>()
            .AddValidatorsFromAssemblyContaining<RemovedFromCartValidator>();

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
