namespace CartService.Domain.Exceptions;

public sealed class CartItemValidationException(
    string field,
    string message) : ValidationException(field: field, message: message)
{

}
