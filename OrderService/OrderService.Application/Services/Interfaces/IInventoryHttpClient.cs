using OrderService.Application.Models;
using OrderService.Application.Results;

namespace OrderService.Application.Services.Interfaces;

public interface IInventoryHttpClient
{
    Task<ServiceResult<InventoryResponse>?> CheckInventory(
        InventoryRequest request,
        string? token = null,
        CancellationToken cancellationToken = default);    
}
