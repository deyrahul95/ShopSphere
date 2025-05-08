using FluentValidation;

namespace InventoryService.Application.Models;

public record UpdateInventoryRequest(int Quantity, bool IsOrdered);

public class UpdateInventoryValidator : AbstractValidator<UpdateInventoryRequest>
{
    public UpdateInventoryValidator()
    {
        RuleFor(x => x.Quantity)
            .NotEmpty()
            .WithMessage("Quantity is required");
    }
}