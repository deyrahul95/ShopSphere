namespace CartService.Application.Results;

public record ValidationError(
    string Field,
    string ErrorMessage
);
