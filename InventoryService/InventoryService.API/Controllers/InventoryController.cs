using InventoryService.Application.Models;
using InventoryService.Application.Results;
using InventoryService.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryController(
    IInventoryService inventoryService,
    ILogger<InventoryController> logger) : ControllerBase
{
    [HttpPost]
    [Route("check")]
    public async Task<ActionResult<ServiceResult<CheckInventoryResponse>>> CheckInventory(
        [FromBody] CheckInventoryRequest request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Start processing check inventory request. Request: {@Request}",
            request);
            
        var response = await inventoryService.CheckInventory(
            request: request,
            token: cancellationToken);

        logger.LogInformation(
            "Completed processing check inventory request. Status Code: {StatusCode}",
            response.StatusCode);

        return StatusCode((int)response.StatusCode, response);
    }
}
