using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Results;

namespace ProductService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult<ServiceResult> Health()
    {
        var response = new ServiceResult(HttpStatusCode.OK, "ProductService is healthy");

        return Ok(response);
    }
}
