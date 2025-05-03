namespace OrderService.Domain.Exceptions;

public class OrderValidationException(
    string field,
    string message) : ValidationException(field: field, message: message)
{

}
