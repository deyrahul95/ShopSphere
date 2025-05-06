using Microsoft.Extensions.DependencyInjection;
using PaymentService.Domain.Enums;
using PaymentService.Domain.Providers;

namespace PaymentService.Infrastructure.Providers;

public class PaymentProviderFactory(IServiceProvider serviceProvider) : IPaymentProviderFactory
{
    public IPaymentProvider GetPaymentProvider(PaymentMode paymentMode)
    {
        IPaymentProvider paymentProvider = paymentMode switch
        {
            PaymentMode.UPI => serviceProvider.GetRequiredService<UPIPaymentProvider>(),
            PaymentMode.DebitCard => serviceProvider.GetRequiredService<DebitCardPaymentProvider>(),
            PaymentMode.CreditCard => serviceProvider.GetRequiredService<CreditCardPaymentProvider>(),
            _ => throw new ArgumentOutOfRangeException(nameof(paymentMode), "Unsupported payment mode")
        };
        return paymentProvider;
    }
}
