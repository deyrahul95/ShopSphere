using System.Net;
using InventoryService.Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class HealthController : ControllerBase
{
    public ActionResult<ServiceResult> Health()
    {
        var response = new ServiceResult(
            statusCode: HttpStatusCode.OK,
            message: "InventoryService is healthy");

        return Ok(response);
    }
}
