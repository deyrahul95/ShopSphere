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

        var cartItem = _items.FirstOrDefault(item => item.ProductId == productId);

        if (cartItem is not null)
        {
            cartItem.IncreasedQuantity(quantity: quantity);
            UpdateAt = DateTime.UtcNow;
            return;
        }

        var newItem = CartItem.Create(
            productId: productId,
            productName: productName,
            quantity: quantity,
            price: price);

        _items.Add(newItem);
        UpdateAt = DateTime.UtcNow;
    }

    public void RemoveItem(Guid productId)
    {
        var item = _items.FirstOrDefault(item => item.ProductId == productId);

        if (item is not null)
        {
            _items.Remove(item);
            UpdateAt = DateTime.UtcNow;
            return;
        }

        throw new CartValidationException(
            field: nameof(CartItem.ProductId),
            message: ExceptionMessages.ProductNotFoundInCart(id: productId));
    }

    public void Clear()
    {
        _items.Clear();
        UpdateAt = DateTime.UtcNow;
    }

    public decimal GetTotalPrice()
    {
        return _items.Sum(item => item.GetTotalPrice());
    }
}
