using System.ComponentModel.DataAnnotations;
using ProductService.Application.Constants;

namespace ProductService.Application.Models;

public record ProductSearchRequest(
    [Range(ValidationConstants.MinPageNumber, ValidationConstants.MaxPageNumber)] int? PageNumber = null,
    [Range(ValidationConstants.MinPageSize, ValidationConstants.MaxPageSize)] int? PageSize = null,
    [MaxLength(ValidationConstants.MaxNameLength)] string? Name = null,
    [MaxLength(ValidationConstants.MaxCategoryLength)] string? Category = null,
    string? Sort = null
);
