using FluentValidation;

namespace CartService.Application.Models;

public record RemovedFromCartRequest(Guid ProductId);

public class RemovedFromCartValidator : AbstractValidator<RemovedFromCartRequest>
{
    public RemovedFromCartValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product id is required")
            .NotEqual(Guid.Empty)
            .WithMessage("Product id must be a valid guid");
    }
}