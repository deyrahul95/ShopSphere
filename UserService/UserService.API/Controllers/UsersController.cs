using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Services.Interfaces;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(
    IUserService userService,
    ILogger<UsersController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Start processing get user request. User Id: {id}", id);
        var response = await userService.GetUserDetails(
            userId: id,
            cancellationToken: cancellationToken);

        logger.LogInformation("Stop processing get user request. {@response}", response);

        return StatusCode((int)response.StatusCode, response);
    }
}
