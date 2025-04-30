namespace CartService.Application.Models;

public record ValidationError(
    string Field,
    string ErrorMessage
);
