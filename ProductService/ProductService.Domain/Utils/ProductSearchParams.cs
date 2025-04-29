using System.ComponentModel.DataAnnotations;
using ProductService.Domain.Constants;

namespace ProductService.Domain.Utils;

public record ProductSearchParams(
    int PageNumber = DomainConstants.DefaultPageNumber,
    int PageSize = DomainConstants.DefaultPageSize,
    string? Name = null,
    string? Category = null
);
