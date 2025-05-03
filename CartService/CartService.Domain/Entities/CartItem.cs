using CartService.Domain.Constants;
using CartService.Domain.Exceptions;

namespace CartService.Domain.Entities;

public sealed class CartItem
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdateAt { get; private set; }

    private CartItem(Guid productId, string productName, int quantity, decimal price)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        Price = price;
        CreatedAt = DateTime.UtcNow;
        UpdateAt = DateTime.UtcNow;
    }

    public static CartItem Create(Guid productId, string productName, int quantity, decimal price)
    {
        if (productId.Equals(Guid.Empty))
        {
            throw new CartItemValidationException(
                field: nameof(ProductId),
                message: ExceptionMessages.InvalidProductIdFormat
            );
        }

        if (string.IsNullOrEmpty(productName))
        {
            throw new CartItemValidationException(
                field: nameof(ProductName),
                message: ExceptionMessages.ProductNameCannotEmpty
            );
        }

        if (quantity <= 0)
        {
            throw new CartItemValidationException(
                field: nameof(Quantity),
                message: ExceptionMessages.ItemQuantityMustBePositive
            );
        }

        if (quantity > DomainConstants.MaxQuantityPerItemInCart)
        {
            throw new CartItemValidationException(
                field: nameof(Quantity),
                message: ExceptionMessages.ItemQuantityExceedsMaxLimit
            );
        }

        if (price <= 0)
        {
            throw new CartItemValidationException(
                field: nameof(Price),
                message: ExceptionMessages.ItemPriceMustBePositive
            );
        }

        return new CartItem(
            productId: productId,
            productName: productName,
            quantity: quantity,
            price: price);
    }

    public decimal GetTotalPrice()
    {
        return Price * Quantity;
    }

    public void IncreasedQuantity(int quantity = DomainConstants.DefaultIncreasedQuantity)
    {
        if (quantity <= 0)
        {
            throw new CartItemValidationException(
                field: nameof(Quantity),
                message: ExceptionMessages.ItemQuantityMustBePositive
            );
        }

        int finalQuantity = Quantity + quantity;

        if (quantity > DomainConstants.MaxQuantityPerItemInCart
            || finalQuantity > DomainConstants.MaxQuantityPerItemInCart)
        {
            throw new CartItemValidationException(
                field: nameof(Quantity),
                message: ExceptionMessages.ItemQuantityExceedsMaxLimit
            );
        }

        Quantity = finalQuantity;
        UpdateAt = DateTime.UtcNow;
    }

    public void DecreasedQuantity(int quantity = DomainConstants.DefaultDecreasedQuantity)
    {
        if (quantity <= 0)
        {
            throw new CartItemValidationException(
                field: nameof(Quantity),
                message: ExceptionMessages.ItemQuantityMustBePositive
            );
        }

        int finalQuantity = Quantity - quantity;

        if (finalQuantity < 0)
        {
            throw new CartItemValidationException(
                field: nameof(Quantity),
                message: ExceptionMessages.ItemQuantityCannotBeNegative
            );
        }

        Quantity = finalQuantity;
        UpdateAt = DateTime.UtcNow;
    }

    public void UpdatePrice(decimal price)
    {
        if (price <= 0)
        {
            throw new CartItemValidationException(
                field: nameof(Price),
                message: ExceptionMessages.ItemPriceMustBePositive
            );
        }

        Price = price;
        UpdateAt = DateTime.UtcNow;
    }
}
