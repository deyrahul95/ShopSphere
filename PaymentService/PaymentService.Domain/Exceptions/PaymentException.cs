namespace PaymentService.Domain.Exceptions;

public class PaymentException(string message) : Exception(message)
{
}
