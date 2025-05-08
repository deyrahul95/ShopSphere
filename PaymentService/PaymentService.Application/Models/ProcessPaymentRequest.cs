using System.Text.Json.Serialization;
using FluentValidation;
using PaymentService.Domain.Enums;

namespace PaymentService.Application.Models;

public class ProcessPaymentRequest
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PaymentMode PaymentMode { get; set; }
}

public class ProcessPaymentValidator : AbstractValidator<ProcessPaymentRequest>
{
    public ProcessPaymentValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("Order id is required")
            .NotEqual(Guid.Empty)
            .WithMessage("Order id must be a valid Guid");

        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("Amount is required");

        RuleFor(x => x.PaymentMode)
            .NotEmpty()
            .WithMessage("Payment mode is required")
            .IsInEnum()
            .WithMessage($"Payment mode is invalid. Please use any of this options: [{string.Join(
                ", ",
                Enum.GetNames(typeof(PaymentMode)))}]");
    }
}