using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class HealthController : ControllerBase
{
    public IActionResult Health()
    {
        var response = new
        {
            StatusCode = HttpStatusCode.OK,
            Message = "PaymentService is healthy"
        };
        return Ok(response);
    }
}
