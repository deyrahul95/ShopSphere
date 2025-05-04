using OrderService.Application.Results;
using OrderService.Domain.Exceptions;

namespace OrderService.Application.Extensions;

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
