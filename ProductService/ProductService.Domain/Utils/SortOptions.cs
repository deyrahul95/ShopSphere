namespace ProductService.Domain.Utils;

public class SortOptions
{
    public const string PriceAsc = "price_asc";
    public const string PriceDesc = "price_desc";

    public static readonly HashSet<string> ValidOptions =
    [
        PriceAsc,
        PriceDesc
    ];
}
