using OrderService.Domain.Constants;
using OrderService.Domain.Exceptions;

namespace OrderService.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal Price { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastUpdated { get; init; }

    private OrderItem(Guid productId, int quantity, decimal price)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        Quantity = quantity;
        Price = price;
        CreatedAt = DateTime.UtcNow;
        LastUpdated = DateTime.UtcNow;
    }

    public static OrderItem Create(Guid productId, int quantity, decimal price)
    {
        if (productId.Equals(Guid.Empty))
        {
            throw new OrderItemValidationException(
                field: nameof(ProductId),
                message: ExceptionMessages.InvalidProductIdFormat
            );
        }

        if (quantity <= 0)
        {
            throw new OrderItemValidationException(
                field: nameof(Quantity),
                message: ExceptionMessages.ItemQuantityMustBePositive
            );
        }

        if (quantity > DomainConstants.MaxQuantityPerItemInOrder)
        {
            throw new OrderItemValidationException(
                field: nameof(Quantity),
                message: ExceptionMessages.ItemQuantityExceedsMaxLimit
            );
        }

        if (price <= 0)
        {
            throw new OrderItemValidationException(
                field: nameof(Price),
                message: ExceptionMessages.ItemPriceMustBePositive
            );
        }

        return new OrderItem(
            productId: productId,
            quantity: quantity,
            price: price);
    }
}
