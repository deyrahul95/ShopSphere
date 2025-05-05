namespace InventoryService.Domain.Entities;

public class ProductStock
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int AvailableQuantity  { get; set; }
}
