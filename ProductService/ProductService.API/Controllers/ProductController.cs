using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Models;
using ProductService.Application.Results;
using ProductService.Domain.Entities;
using ProductService.Domain.Utils;

namespace ProductService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    // [HttpGet]
    // [Route("search")]
    // public ActionResult<ServiceResult<PaginatedResult<Product>> Search(
    //     [FromQuery] ProductSearchRequest request, 
    //     CancellationToken cancellationToken = default)
    // {

    // }
}
