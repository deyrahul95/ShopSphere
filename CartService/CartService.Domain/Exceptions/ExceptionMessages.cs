using CartService.Domain.Constants;

namespace CartService.Domain.Exceptions;

public sealed class ExceptionMessages
{
    public const string ItemQuantityMustBePositive = "Quantity of items in the cart must be a positive number";
    public readonly static string ItemQuantityExceedsMaxLimit = $"Quantity of a single item in the cart cannot exceed {DomainConstants.MaxQuantityPerItemInCart}";
    public const string ItemQuantityCannotBeNegative = "Quantity of items in the cart cannot be less than zero";
    public const string ItemPriceMustBePositive = "Price of items in the cart must be a positive number";
    public const string InvalidUserIdFormat = "User ID must be a valid GUID";
    public static string ProductNotFoundInCart(Guid id) => $"Product with ID: {id} was not found in the cart";
    public static readonly string MaxDistinctItemsExceeded = $"Cart cannot contain more than {DomainConstants.MaxDistinctItemsInCart} distinct items.";
}
