using ProductService.Domain.Constants;

namespace ProductService.Domain.Utils;

public record ProductSearchFilters(
    int PageNumber = DomainConstants.DefaultPageNumber,
    int PageSize = DomainConstants.DefaultPageSize,
    string? Name = null,
    string? Category = null
);
