using System.ComponentModel.DataAnnotations;
using CartService.Application.Constants;
using CartService.Domain.Constants;

namespace CartService.Application.Models;

public class AddToCartRequest
{
    [Required]
    public Guid ProductId { get; set; }

    [Range(ValidationConstants.MinQuantityPerItemInCart, DomainConstants.MaxQuantityPerItemInCart)]
    public int Quantity { get; set; } = ValidationConstants.MinQuantityPerItemInCart;
}
