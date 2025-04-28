using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Models;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Results;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<ServiceResult<LoginResponseModel>>> Login(
        [FromBody] LoginRequestModel request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Start processing login request. {@request}", request);

        if (ModelState.IsValid is false)
        {
            logger.LogInformation("Invalid model state. {@modelState}", ModelState);
            return BadRequest(ModelState);
        }

        var response = await authService.Login(
            request: request,
            cancellationToken: cancellationToken);

        logger.LogInformation("Stop processing login request. {@response}", response);

        return StatusCode((int)response.StatusCode, response);
    }
}
