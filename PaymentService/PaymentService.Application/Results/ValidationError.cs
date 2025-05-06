namespace PaymentService.Application.Results;

public record ValidationError(
    string Field,
    string ErrorMessage
);
