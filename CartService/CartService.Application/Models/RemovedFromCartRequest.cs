using System.ComponentModel.DataAnnotations;

namespace CartService.Application.Models;

public class RemovedFromCartRequest
{
    [Required]
    public Guid ProductId { get; set; }

    // [Range(ValidationConstants.MinQuantityPerItemInCart, DomainConstants.MaxQuantityPerItemInCart)]
    // public int Quantity { get; set; } = ValidationConstants.MinQuantityPerItemInCart;

}
