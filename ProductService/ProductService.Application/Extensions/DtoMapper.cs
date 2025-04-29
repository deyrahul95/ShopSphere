using ProductService.Application.DTOs;
using ProductService.Domain.Entities;

namespace ProductService.Application.Extensions;

public static class DtoMapper
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(
            Id: product.Id,
            Name: product.Name,
            Description: product.Description,
            Price: product.Price,
            Category: product.Category
        );
    }

    public static List<ProductDto> ToDtoList(this IEnumerable<Product> products)
    {
        if (products.Any() is false)
        {
            return [];
        }

        return products.Select(ToDto).ToList();
    }
}
