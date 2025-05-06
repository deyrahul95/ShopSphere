namespace PaymentService.Domain.Exceptions;

public sealed class ExceptionMessages
{
    public const string InvalidOrderIdFormat = "Order ID must be a valid GUID";
    public const string AmountMustBePositive = "Amount must be a positive number";
}