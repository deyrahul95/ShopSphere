using PaymentService.Domain.Enums;

namespace PaymentService.Domain.Providers;

public interface IPaymentProviderFactory
{
    IPaymentProvider GetPaymentProvider(PaymentMode paymentMode);
}
