using Arya.BabyLogger.Shared.User;
using Arya.BabyLogger.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arya.BabyLogger.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<LoginResponse> LoginAsync([FromBody] LoginRequest request, [FromServices] IUserService userService)
    {   
        var user = userService.LookupEmailAsync(request.Email);

        if (user is null)
        {
            return Unauthorized("Invalid username");
        }

        if (user.PasswordHash != userService.HashedPassword(request.Password))
        {
            return Unauthorized("Invalid password");
        }

        var loginResponse = new LoginResponse
        {
            Token = userService.GenerateJwtToken(user)
        };

        return loginResponse;
    }
}