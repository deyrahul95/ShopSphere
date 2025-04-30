namespace CartService.Domain.Exceptions;

public class CartValidationException(
    string field,
    string message) : ValidationException(field: field, message: message)
{
    
}
