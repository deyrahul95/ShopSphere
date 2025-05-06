namespace PaymentService.Domain.Exceptions;

public class PaymentValidationException(
    string field,
    string message) : Exception(message)
{
    public string Field { get; set; } = field;
    public string Error { get; set; } = message;
}
