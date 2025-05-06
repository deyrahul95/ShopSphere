using Microsoft.Extensions.Logging;
using PaymentService.Domain.Enums;
using PaymentService.Domain.Providers;
using PaymentService.Infrastructure.Helpers;

namespace PaymentService.Infrastructure.Providers;

public class UPIPaymentProvider(ILogger<UPIPaymentProvider> logger) : IPaymentProvider
{
    private readonly PaymentMode _paymentMode = PaymentMode.UPI;

    public async Task<PaymentStatus> ProcessTransaction(decimal amount, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting {Mode} payment for amount: {Amount}", _paymentMode.ToString(), amount);

        try
        {
            MockPaymentHelper.EnforceTransactionLimit(_paymentMode, amount);

            await MockPaymentHelper.SimulateNetworkDelay();

            cancellationToken.ThrowIfCancellationRequested();
            MockPaymentHelper.MaybeThrowRandomError(_paymentMode);

            var status = PaymentStatus.Completed;
            logger.LogInformation("{Mode} Payment result: {Status}", _paymentMode.ToString(), status);
            return status;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Mode} payment failed due to error.", _paymentMode.ToString());
            return PaymentStatus.Failed;
        }
    }
}

