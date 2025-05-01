using System.Net;
using CartService.Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult<ServiceResult> Health()
    {
        var response = new ServiceResult(HttpStatusCode.OK, "Api is healthy");

        return Ok(response);
    }
}
