using OrderService.Domain.Constants;
using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;

namespace OrderService.Domain.Entities;

public class Order
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public List<OrderItem> Items { get; init; }
    public decimal TotalAmount { get; init; }
    public string OrderState { get; private set; }
    public string PaymentState { get; private set; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastUpdated { get; private set; }

    private Order(Guid userId, List<OrderItem> items)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Items = items;
        TotalAmount = CalculateTotalAmount();
        OrderState = OrderStatus.Pending.ToString();
        PaymentState = OrderPaymentStatus.Unpaid.ToString();
        CreatedAt = DateTime.UtcNow;
        LastUpdated = DateTime.UtcNow;
    }

    public static Order Create(Guid userId, List<OrderItem> items)
    {
        if (userId.Equals(Guid.Empty))
        {
            throw new OrderValidationException(
                field: nameof(UserId),
                message: ExceptionMessages.InvalidUserIdFormat
            );
        }

        if (items.Count == 0)
        {
            throw new OrderValidationException(
                field: nameof(Items),
                message: ExceptionMessages.OrderItemsCannotBeEmpty
            );
        }

        if (items.Count > DomainConstants.MaxDistinctItemsInOrder)
        {
            throw new OrderValidationException(
                field: nameof(Items),
                message: ExceptionMessages.MaxDistinctItemsExceeded
            );
        }

        return new Order(userId: userId, items: items);
    }

    public void UpdateOrderState(OrderStatus status)
    {
        OrderState = status.ToString();
        LastUpdated = DateTime.UtcNow;
    }

    public void UpdatePaymentState(OrderPaymentStatus status)
    {
        PaymentState = status.ToString();
        LastUpdated = DateTime.UtcNow;
    }

    private decimal CalculateTotalAmount()
    {
        return Items.Select(x => x.Price * x.Quantity).Sum();
    }
}
