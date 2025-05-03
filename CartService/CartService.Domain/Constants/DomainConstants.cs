namespace CartService.Domain.Constants;

public sealed class DomainConstants
{
    public const int DefaultIncreasedQuantity = 1;
    public const int DefaultDecreasedQuantity = 1;
    /// <summary>
    /// Maximum quantity of a single item allowed in a shopping cart
    /// </summary>
    public const int MaxQuantityPerItemInCart = 5;

    /// <summary>
    /// Maximum total number of distinct items allowed in a shopping cart
    /// </summary>
    public const int MaxDistinctItemsInCart = 10;
}
