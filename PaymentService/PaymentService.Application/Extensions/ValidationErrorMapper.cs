using PaymentService.Application.Results;
using PaymentService.Domain.Exceptions;

namespace PaymentService.Application.Extensions;

public static class ValidationErrorMapper
{
    public static ValidationError ToError(this PaymentValidationException exception)
    {
        return new ValidationError(
            Field: exception.Field,
            ErrorMessage: exception.Message
        );
    }
}
