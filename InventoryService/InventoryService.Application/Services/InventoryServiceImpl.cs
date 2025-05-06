using InventoryService.Application.Models;
using InventoryService.Application.Results;
using InventoryService.Application.Services.Interfaces;
using InventoryService.Domain.Entities;
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
            var productStock = await FetchProductStock(productId: request.ProductId, token: token);

            if (productStock is null)
            {
                return InventoryResults<CheckInventoryResponse>.NotFound(request.ProductId);
            }

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

    public async Task<ServiceResult> UpdateInventory(
        Guid productId,
        UpdateInventoryRequest request,
        CancellationToken token = default)
    {
        try
        {
            var productStock = await FetchProductStock(productId: productId, token: token);

            if (productStock is null)
            {
                return InventoryResults.NotFound(productId);
            }

            if (request.IsOrdered)
            {
                productStock.AvailableQuantity -= request.Quantity;
            }
            else
            {
                productStock.AvailableQuantity += request.Quantity;
            }

            var isSuccess = await inventoryRepository.UpdateProductStock(
                productStock: productStock,
                cancellationToken: token);

            if (isSuccess is false)
            {
                logger.LogWarning(
                    "Failed to update product stocks. An error occurred in repository. Product Id: {ProductId}",
                    productId);
                return InventoryResults.InternalServerError;
            }

            logger.LogInformation(
                "Successfully updated product stocks. Product Id: {ProductId}",
                productId);

            return InventoryResults.NoContent;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to fetch product data. Id: {ProductID}, Error: {Error}",
                productId,
                ex.Message);
            return InventoryResults.InternalServerError;
        }
    }

    private async Task<ProductStock?> FetchProductStock(
        Guid productId,
        CancellationToken token)
    {
        logger.LogInformation(
            "Fetching product stock details. Product Id: {ProductId}",
            productId);

        var productStock = await inventoryRepository.GetProductStock(
            productId: productId,
            cancellationToken: token);

        if (productStock is null)
        {
            logger.LogWarning(
                "Product stock details not found. Product Id: {ProductId}",
                productId);
            return null;
        }

        logger.LogInformation(
            "Product stock details fetched successfully. Product Stock: {@ProductStock}",
            productStock);
        return productStock;
    }
}
