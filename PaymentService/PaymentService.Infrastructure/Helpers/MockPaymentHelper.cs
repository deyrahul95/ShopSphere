using PaymentService.Domain.Enums;
using PaymentService.Domain.Exceptions;

namespace PaymentService.Infrastructure.Helpers;

public static class MockPaymentHelper
{
    private static readonly Random _random = new();

    public static async Task SimulateNetworkDelay()
    {
        await Task.Delay(_random.Next(300, 1500));
    }

    public static void MaybeThrowRandomError(PaymentMode mode)
    {
        var nextValue = _random.NextDouble();

        if (nextValue < 0.1)
        {
            throw new PaymentException("Bank server busy. Please try again later.");
        }

        if (nextValue < 0.25)
        {
            throw new PaymentException("Insufficient balance.");
        }

        if ((mode == PaymentMode.CreditCard || mode == PaymentMode.DebitCard) && nextValue < 0.4)
        {
            throw new PaymentException("Invalid card details.");
        }

        if (nextValue < 0.5)
        {
            throw new TimeoutException("Payment gateway timed out.");
        }
    }

    public static decimal GetTransactionLimit(PaymentMode mode)
    {
        return mode switch
        {
            PaymentMode.UPI => 100_000m,
            PaymentMode.DebitCard => 75_000m,
            PaymentMode.CreditCard => 200_000m,
            _ => throw new ArgumentOutOfRangeException(nameof(mode))
        };
    }

    public static void EnforceTransactionLimit(PaymentMode mode, decimal amount)
    {
        var limit = GetTransactionLimit(mode);
        if (amount > limit)
            throw new PaymentException($"Transaction exceeds limit for {mode}. Limit: ₹{limit:N0}, Attempted: ₹{amount:N0}");
    }
}
