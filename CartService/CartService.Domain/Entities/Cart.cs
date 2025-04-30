using CartService.Domain.Constants;
using CartService.Domain.Exceptions;

namespace CartService.Domain.Entities;

public sealed class Cart
{
    private readonly List<CartItem> _items = [];

    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();
    public DateTime CreatedAt { get; init; }
    public DateTime UpdateAt { get; private set; }

    private Cart(Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        UpdateAt = DateTime.UtcNow;
    }

    public static Cart Create(Guid userId)
    {
        if (userId.Equals(Guid.Empty))
        {
            throw new CartValidationException(
                field: nameof(UserId),
                message: ExceptionMessages.InvalidUserIdFormat
            );
        }

        return new Cart(userId: userId);
    }

    public void AddItem(Guid productId, string productName, int quantity, decimal price)
    {
        if (_items.Count >= DomainConstants.MaxQuantityPerItemInCart)
        {
            throw new CartValidationException(
                field: nameof(Cart),
                message: ExceptionMessages.MaxDistinctItemsExceeded
            );
        }

        var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.IncreasedQuantity(quantity: quantity);
            return;
        }

        var newItem = CartItem.Create(
            productId: productId,
            productName: productName,
            quantity: quantity,
            price: price);

        _items.Add(newItem);
    }

    public void RemoveItem(Guid productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);

        if (item != null)
        {
            _items.Remove(item);
        }

        throw new CartValidationException(
            nameof(CartItem.ProductId),
            ExceptionMessages.ProductNotFoundInCart(id: productId)
        );
    }

    public void Clear()
    {
        _items.Clear();
    }

    public decimal GetTotalPrice()
    {
        return _items.Sum(i => i.GetTotalPrice());
    }
}
