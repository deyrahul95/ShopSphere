using InventoryService.Application.Models;
using InventoryService.Application.Results;
using InventoryService.Application.Services.Interfaces;
using InventoryService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace InventoryService.Application.Services;

public class InventoryServiceImpl(
    IInventoryRepository inventoryRepository,
    ILogger<InventoryServiceImpl> logger) : IInventoryService
{
    public async Task<ServiceResult<CheckInventoryResponse>> CheckInventory(
        CheckInventoryRequest request,
        CancellationToken token = default)
    {
        try
        {
            logger.LogInformation(
                "Fetching product stock details. Product Id: {ProductId}",
                request.ProductId);

            var productStock = await inventoryRepository.GetProductStock(
                productId: request.ProductId,
                cancellationToken: token);

            if (productStock is null)
            {
                logger.LogWarning(
                    "Product stock details not found. Product Id: {ProductId}",
                    request.ProductId);
                return InventoryResults<CheckInventoryResponse>.NotFound(request.ProductId);
            }

            logger.LogInformation(
                "Product stock details fetched successfully. Product Stock: {@ProductStock}",
                productStock);

            var isAvailable = productStock.AvailableQuantity >= request.Quantity;

            var result = new CheckInventoryResponse(Available: isAvailable);
            return InventoryResults<CheckInventoryResponse>.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to fetch product data. Id: {ProductID}, Error: {Error}",
                request.ProductId,
                ex.Message);
            return InventoryResults<CheckInventoryResponse>.InternalServerError;
        }
    }
}
