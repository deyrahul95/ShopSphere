using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Domain.Results;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class HealthController : ControllerBase
{
    [HttpGet]
    [Route("health")]
    public ActionResult<ServiceResult> Health()
    {
        var response = new ServiceResult(HttpStatusCode.OK, "Api is healthy");

        return Ok(response);
    }
}
