using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.DTOs;
using ProductService.Application.Models;
using ProductService.Application.Results;
using ProductService.Application.Services.Interfaces;
using ProductService.Domain.Utils;

namespace ProductService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController(
    IProductsService productService,
    ILogger<ProductsController> logger) : ControllerBase
{
    [HttpGet]
    [Route("search")]
    public async Task<ActionResult<ServiceResult<PaginatedResult<ProductDto>>>> Search(
        [FromQuery] ProductSearchRequest request,
        CancellationToken cancellationToken = default)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        logger.LogInformation("Start processing search product request. Request: {@Request}", request);

        var response = await productService.SearchProducts(
            request: request,
            cancellationToken: cancellationToken);

        logger.LogInformation("Completed processing search products request. Status Code: {StatusCode}", response.StatusCode);

        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Start processing get product request. User Id: {id}",
            id);

        var response = await productService.GetProduct(
            id: id,
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "Completed processing get product request. Product Id: {ProductId}, Status Code: {StatusCode}",
            id,
            response.StatusCode);

        return StatusCode((int)response.StatusCode, response);
    }
}
