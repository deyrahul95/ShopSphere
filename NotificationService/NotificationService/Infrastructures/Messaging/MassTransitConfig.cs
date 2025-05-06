using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Constants;
using NotificationService.Features.OrderCancelled;
using NotificationService.Features.OrderConfirmed;
using NotificationService.Features.OrderCreated;
using NotificationService.Features.PaymentCompleted;
using NotificationService.Features.PaymentFailed;

namespace NotificationService.Infrastructures.Messaging;

public static class MassTransitConfig
{
    public static void AddMassTransitConfiguration(this IServiceCollection services)
    {
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
            x.AddConsumer<PaymentFailedConsumer>(
                cfg => cfg.UseMessageRetry(r => r.Interval(
                    MassTransitConstants.MaxRetryCount,
                    TimeSpan.FromSeconds(MassTransitConstants.RetryTimeSpanInSecond))));
            x.AddConsumer<PaymentCompletedConsumer>(
                cfg => cfg.UseMessageRetry(r => r.Interval(
                    MassTransitConstants.MaxRetryCount,
                    TimeSpan.FromSeconds(MassTransitConstants.RetryTimeSpanInSecond))));

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(MassTransitConstants.RabbitMqHost, "/", h =>
                {
                    h.Username(MassTransitConstants.RabbitMqDefaultUser);
                    h.Password(MassTransitConstants.RabbitMqDefaultPassword);
                });

                cfg.ReceiveEndpoint(MassTransitConstants.NotificationQueueName, ep =>
                {
                    ep.ConfigureConsumer<OrderCreatedConsumer>(ctx);
                    ep.ConfigureConsumer<OrderConfirmedConsumer>(ctx);
                    ep.ConfigureConsumer<OrderCancelledConsumer>(ctx);
                    ep.ConfigureConsumer<PaymentFailedConsumer>(ctx);
                    ep.ConfigureConsumer<PaymentCompletedConsumer>(ctx);

                    ep.UseMessageRetry(
                        r => r.Interval(
                            MassTransitConstants.MaxRetryCount,
                            TimeSpan.FromSeconds(MassTransitConstants.RetryTimeSpanInSecond)));
                    ep.UseInMemoryOutbox(ctx);

                    ep.DeadLetterExchange = MassTransitConstants.DeadLetterExchangeName;
                });
            });
        });
    }
}
