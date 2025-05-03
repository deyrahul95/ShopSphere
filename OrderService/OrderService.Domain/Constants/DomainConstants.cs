namespace OrderService.Domain.Constants;

public class DomainConstants
{
    /// <summary>
    /// Maximum quantity of a single item allowed in a shopping order
    /// </summary>
    public const int MaxQuantityPerItemInOrder = 5;

    /// <summary>
    /// Maximum total number of distinct items allowed in a shopping order
    /// </summary>
    public const int MaxDistinctItemsInOrder = 10;

    public const int DefaultAccessTokenExpiryInDays = 1;
    public const string JwtConfigName = "JwtConfigs";
}
