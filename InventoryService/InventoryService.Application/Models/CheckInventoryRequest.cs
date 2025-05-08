using FluentValidation;

namespace InventoryService.Application.Models;

public record CheckInventoryRequest(Guid ProductId, int Quantity);

public class CheckInventoryValidator : AbstractValidator<CheckInventoryRequest>
{
    public CheckInventoryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product id is required")
            .NotEqual(Guid.Empty)
            .WithMessage("Product id must be a valid guid");

        RuleFor(x => x.Quantity)
            .NotEmpty()
            .WithMessage("Quantity is required");
    }
}