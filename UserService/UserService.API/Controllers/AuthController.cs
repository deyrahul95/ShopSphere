using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Models;
using UserService.Application.Results;
using UserService.Application.Services.Interfaces;

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
        logger.LogInformation(
            "Start processing login request. User Name: {username}",
            request.Username);

        if (ModelState.IsValid is false)
        {
            logger.LogInformation("Invalid model state. {@modelState}", ModelState);
            return BadRequest(ModelState);
        }

        var response = await authService.Login(
            request: request,
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "Stop processing login request. User Name: {username}, Status Code: {code}",
            request.Username,
            response.StatusCode);

        return StatusCode((int)response.StatusCode, response);
    }
}
