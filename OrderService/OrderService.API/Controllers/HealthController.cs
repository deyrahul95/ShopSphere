using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Results;

namespace OrderService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult<ServiceResult> Health()
    {
        var response = new ServiceResult(HttpStatusCode.OK, "OrderService is healthy");

        return Ok(response);
    }
}
