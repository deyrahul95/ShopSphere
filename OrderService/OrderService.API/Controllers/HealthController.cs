using Microsoft.AspNetCore.Mvc;

namespace OrderService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Health()
    {
        return Ok("Api is healthy!");
    }
}
