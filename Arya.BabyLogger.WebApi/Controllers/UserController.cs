using System.Net;
using Arya.BabyLogger.WebApi.Db;
using Arya.BabyLogger.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arya.BabyLogger.WebApi.Controllers;

using Shared.User;

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
        
        var passwordHash = userService.HashedPassword(request.Password);
        
        if (user.PasswordHash != passwordHash)
        {
            return Unauthorized("Invalid password");
        }

        var loginResponse = new LoginResponse
        {
            Token = userService.GenerateJwtToken(user)
        };

        return loginResponse;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<Guid> RegisterAsync([FromBody] RegisterRequest request, [FromServices] IUserService userService)
    {
        Guid careHolderId;
        
        if (string.IsNullOrEmpty(request.CareHolderId))
        {
            careHolderId = await userService.CreateNewCareHolderAsync();
        }
        else
        {
            careHolderId = Guid.Parse(request.CareHolderId);
        }
        
        var existedUser = await userService.GetUserByEmailAsync(request.Email);
        if (existedUser is not null)
        {
            Response.StatusCode = Convert.ToInt16(HttpStatusCode.Conflict);
            return Guid.Empty;
        }
        
        var userEntity = new UserEntity
        {
            Username = request.Username,
            PasswordHash = userService.HashedPassword(request.Password),
            Email = request.Email,
            CareHolderId = careHolderId
        };
        
        var id= await userService.CreateUserAsync(userEntity);

        return id;
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<string> RequestResetPasswordAsync([FromBody] RequestResetPasswordRequest request, [FromServices] IUserService userService, [FromServices] IEmailService emailService)
    {
        var (id, secretKey) = await userService.ResetPasswordAsync(request.Email);

        await emailService.SendAsync(request.Email, "BabyLogger: Reset Password", $"Please enter Secret Key : {secretKey} to reset your password in BabyLogger Mobile Application.");
        
        return id.ToString();
    }
}