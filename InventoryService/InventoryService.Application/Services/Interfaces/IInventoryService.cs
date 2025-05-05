using InventoryService.Application.Models;
using InventoryService.Application.Results;

namespace InventoryService.Application.Services.Interfaces;

public interface IInventoryService
{
    Task<ServiceResult<CheckInventoryResponse>> CheckInventory(
        CheckInventoryRequest request,
        CancellationToken token = default);
}
