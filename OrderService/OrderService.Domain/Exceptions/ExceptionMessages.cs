using OrderService.Domain.Constants;

namespace OrderService.Domain.Exceptions;

public sealed class ExceptionMessages
{
    public const string InvalidUserIdFormat = "User ID must be a valid GUID";
    public const string InvalidProductIdFormat = "Product ID must be a valid GUID";
    public const string ItemQuantityMustBePositive = "Quantity of items in the order must be a positive number";
    public readonly static string ItemQuantityExceedsMaxLimit = $"Quantity of a single item cannot exceed {DomainConstants.MaxQuantityPerItemInOrder}";
    public const string ItemPriceMustBePositive = "Price of items in the order must be a positive number";
    public const string OrderItemsCannotBeEmpty = "Order must contain at least one item. Please add items to your cart before placing an order.";
    public static readonly string MaxDistinctItemsExceeded = $"Order cannot contain more than {DomainConstants.MaxDistinctItemsInOrder} distinct items.";
}
