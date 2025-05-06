using Microsoft.Extensions.Logging;
using PaymentService.Application.Constants;
using PaymentService.Domain.Exceptions;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace PaymentService.Application.Handlers;

public class RetryHandler(ILogger<RetryHandler> logger)
{
    public async Task<T> ExecuteWithRetryAsync<T>(
        Func<CancellationToken, Task<T>> action,
        string operationName,
        CancellationToken cancellationToken,
        int retryCount = RetryConstants.MaxRetryCount,
        TimeSpan? medianFirstRetryDelay = null)
    {
        medianFirstRetryDelay ??= TimeSpan.FromMilliseconds(RetryConstants.FirstRetryDelayInMilliseconds);

        var delay = Backoff.DecorrelatedJitterBackoffV2(
            medianFirstRetryDelay: medianFirstRetryDelay.Value,
            retryCount: retryCount
        );

        var retryPolicy = Policy
            .Handle<PaymentException>()
            .Or<TimeoutException>()
            .WaitAndRetryAsync(
                delay,
                onRetry: (exception, timeSpan, retryAttempt, context) =>
                {
                    logger.LogWarning(exception,
                        "Retry {RetryAttempt} for {Operation} after {Delay}s due to: {Message}",
                        retryAttempt, operationName, timeSpan.TotalSeconds, exception.Message);
                });

        return await retryPolicy.ExecuteAsync(async (ctx, token) =>
        {
            return await action(token);
        }, new Context(operationName), cancellationToken);
    }
}
