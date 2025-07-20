using CartService.Application.Constants;
using CartService.Domain.Constants;
using FluentValidation;

namespace CartService.Application.Models;

public record AddToCartRequest(Guid ProductId, int Quantity);

public class AddToCartValidator : AbstractValidator<AddToCartRequest>
{
    public AddToCartValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product id is required")
            .NotEqual(Guid.Empty)
            .WithMessage("Product id must be a valid guid");

        RuleFor(x => x.Quantity)
            .NotEmpty()
            .WithMessage("Quantity is required")
            .InclusiveBetween(ValidationConstants.MinQuantityPerItemInCart, DomainConstants.MaxQuantityPerItemInCart)
            .WithMessage($"Quantity must be in between {ValidationConstants.MinQuantityPerItemInCart} and {DomainConstants.MaxQuantityPerItemInCart}");
    }
}