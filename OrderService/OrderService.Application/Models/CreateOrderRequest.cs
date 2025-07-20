using System.Text.Json.Serialization;
using FluentValidation;
using OrderService.Domain.Enums;

namespace OrderService.Application.Models;

public class CreateOrderRequest
{
    public Guid CartId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PaymentMode PaymentMode { get; set; }
}

public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.CartId)
            .NotEmpty()
            .WithMessage("Cart id is required")
            .NotEqual(Guid.Empty)
            .WithMessage("Cart id must be a valid guid");

        RuleFor(x => x.PaymentMode)
            .NotEmpty()
            .WithMessage("Payment mode is required")
            .IsInEnum()
            .WithMessage($"Payment mode is invalid. Please use any of this options: [{string.Join(
                ", ",
                Enum.GetNames(typeof(PaymentMode)))}]");
    }
}