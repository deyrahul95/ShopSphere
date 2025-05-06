using OrderService.Application.Models;
using OrderService.Application.Results;

namespace OrderService.Application.Services.Interfaces;

public interface IInventoryHttpClient
{
    Task<ServiceResult<CheckInventoryResponse>?> CheckInventory(
        CheckInventoryRequest request,
        CancellationToken cancellationToken = default);    
}
