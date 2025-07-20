using FluentValidation;
using ProductService.Application.Constants;

namespace ProductService.Application.Models;

public record ProductSearchRequest(
    int? PageNumber = null,
    int? PageSize = null,
    string? Name = null,
    string? Category = null,
    string? Sort = null
);

public class ProductSearchValidator : AbstractValidator<ProductSearchRequest>
{
    public ProductSearchValidator()
    {
        RuleFor(x => x.PageNumber)
            .InclusiveBetween(ValidationConstants.MinPageNumber, ValidationConstants.MaxPageNumber)
            .WithMessage($"Page number must be in between {ValidationConstants.MinPageNumber} and {ValidationConstants.MaxPageNumber}");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(ValidationConstants.MinPageSize, ValidationConstants.MaxPageSize)
            .WithMessage($"Page size must be in between {ValidationConstants.MinPageSize} and {ValidationConstants.MaxPageSize}");

        RuleFor(x => x.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .WithMessage($"Name can't be more than {ValidationConstants.MaxNameLength} characters long");

        RuleFor(x => x.Category)
            .MaximumLength(ValidationConstants.MaxCategoryLength)
            .WithMessage($"Category can't be more than {ValidationConstants.MaxCategoryLength} characters long");
    }
}