namespace OrderService.Domain.Exceptions;

public class OrderItemValidationException(
    string field,
    string message) : ValidationException(field: field, message: message)
{
    
}
