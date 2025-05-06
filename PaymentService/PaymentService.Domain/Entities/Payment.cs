using PaymentService.Domain.Enums;
using PaymentService.Domain.Exceptions;

namespace PaymentService.Domain.Entities;

public class Payment
{
    public Guid Id { get; init; }
    public Guid OrderId { get; init; }
    public Guid UserId { get; init; }
    public decimal Amount { get; init; }
    public string Mode { get; init; }
    public string Status { get; private set; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastUpdated { get; private set; }

    private Payment(Guid orderId, Guid userId, decimal amount, PaymentMode paymentMode)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        UserId = userId;
        Amount = amount;
        Mode = paymentMode.ToString();
        Status = nameof(PaymentStatus.Processing);
        CreatedAt = DateTime.UtcNow;
        LastUpdated = DateTime.UtcNow;
    }

    public static Payment Create(Guid orderId, Guid userId, decimal amount, PaymentMode paymentMode)
    {
        if (orderId.Equals(Guid.Empty))
        {
            throw new PaymentValidationException(
                field: nameof(OrderId),
                message: ExceptionMessages.InvalidOrderIdFormat
            );
        }

        if (userId.Equals(Guid.Empty))
        {
            throw new PaymentValidationException(
                field: nameof(UserId),
                message: ExceptionMessages.InvalidUserIdFormat
            );
        }

        if (amount <= 0)
        {
            throw new PaymentValidationException(
                field: nameof(Amount),
                message: ExceptionMessages.AmountMustBePositive
            );
        }

        return new Payment(
            orderId: orderId,
            userId: userId,
            amount: amount,
            paymentMode: paymentMode);
    }

    public void UpdateStatus(PaymentStatus status)
    {
        Status = status.ToString();
        LastUpdated = DateTime.UtcNow;
    }
}
