namespace ProductService.Domain.Utils;

public class PaginatedResult<T>(int pageNumber, int pageSize, int totalItems, List<T> items)
{
    public int PageNumber { get; set; } = pageNumber;
    public int PageSize { get; set; } = pageSize;
    public int TotalPages { get; set; } = (int)Math.Ceiling(totalItems / (double)pageSize);
    public int TotalItems { get; set; } = totalItems;
    public List<T> Items { get; set; } = items;
}
