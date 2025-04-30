using CartService.Application.Models;
using CartService.Domain.Exceptions;

namespace CartService.Application.Extensions;

public static class ValidationErrorMapper
{
    public static ValidationError ToError(this ValidationException exception)
    {
        return new ValidationError(
            Field: exception.Field,
            ErrorMessage: exception.Message
        );
    }
}
