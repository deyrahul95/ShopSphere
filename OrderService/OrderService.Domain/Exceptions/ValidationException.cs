namespace OrderService.Domain.Exceptions;

public abstract class ValidationException(
    string field,
    string message) : Exception(message)
{
    public string Field { get; set; } = field;
    public string Error { get; set; } = message;
}
