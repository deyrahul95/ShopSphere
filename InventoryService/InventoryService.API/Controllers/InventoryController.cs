using System.Net;
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
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

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

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateInventoryRequest request,
        CancellationToken token = default)
    {
        logger.LogInformation(
            "Start processing update inventory request. Product Id: {ProductId}, Request: {@Request}",
            id,
            request);

        var response = await inventoryService.UpdateInventory(
            productId: id,
            request: request,
            token: token);

        logger.LogInformation(
            "Completed processing update inventory request.Product Id: {ProductId}, Status Code: {StatusCode}",
            id,
            response.StatusCode);

        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            return NoContent();
        }

        return StatusCode((int)response.StatusCode, response);
    }
}
