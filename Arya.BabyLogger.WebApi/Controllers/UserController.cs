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
        try
        {
            var (id, secretKey) = await userService.RequestResetPasswordAsync(request.Email);

            await emailService.SendAsync(request.Email, "BabyLogger: Reset Password", $"Please enter Secret Key : {secretKey} to reset your password in BabyLogger Mobile Application.");

            return id.ToString();
        }
        catch (Exception ex)
        {
            Response.StatusCode = Convert.ToInt16(HttpStatusCode.BadRequest);
            Response.Headers.Append("Error", ex.Message);
            
            return string.Empty;
        }
    }

    [AllowAnonymous]
    [HttpPatch("reset-password")]
    public async Task<bool> ResetPasswordAsync([FromBody] ResetPasswordRequest request, [FromServices] IUserService userService)
    {
        try
        {
            var success = await userService.ResetPasswordAsync(request.SecretKey, request.SecretCode, request.NewPassword);
            
            return success;
            
        }
        catch(Exception ex)
        {
            Response.StatusCode = Convert.ToInt16(HttpStatusCode.BadRequest);
            Response.Headers.Append("Error", ex.Message);
            
            return false;
        }
    }

    [Authorize]
    [HttpPatch("change-password")]
    public async Task<bool> ChangePasswordAsync([FromBody] ChangePasswordRequest request, [FromServices] IUserService userService)
    {
        try
        {
            var userId = Request.Headers["User-Id"];
            if (!Guid.TryParse(userId, out var userIdGuid))
            {
                Response.StatusCode = Convert.ToInt16(HttpStatusCode.BadRequest);
                Response.Headers.Append("Error", "User-Id header is required.");
                
                return false;
            }
            
            var success = await userService.ChangePasswordAsync(userIdGuid, request.CurrentPassword, request.NewPassword);

            return success;
        }
        catch(Exception ex)
        {
            Response.StatusCode = Convert.ToInt16(HttpStatusCode.BadRequest);
            Response.Headers.Append("Error", ex.Message);
            
            return false;
        }
    }

    [Authorize]
    [HttpPatch("username")]
    public async Task<bool> UpdateUsernameAsync([FromBody] UpdateUsernameRequest request, [FromServices] IUserService userService)
    {
        try
        {
            var userId = Request.Headers["User-Id"];
            if (!Guid.TryParse(userId, out var userIdGuid))
            {
                Response.StatusCode = Convert.ToInt16(HttpStatusCode.BadRequest);
                Response.Headers.Append("Error", "User-Id header is required.");
                
                return false;
            }

            var success = await userService.UpdateUsernameAsync(userIdGuid, request.Username);

            return success;
        }
        catch (Exception ex)
        {
            Response.StatusCode = Convert.ToInt16(HttpStatusCode.BadRequest);
            Response.Headers.Append("Error", ex.Message);

            return false;
        }
    }

    [Authorize]
    [HttpPut("settings-breastpump")]
    public async Task<bool> SetBreastpumpSettingsAsync([FromBody] BreastPumpSettingsRquestResponse request, [FromServices] IUserService userService)
    {
        var userId = Request.Headers["User-Id"];
        if (!Guid.TryParse(userId, out var userIdGuid))
        {
            Response.StatusCode = Convert.ToInt16(HttpStatusCode.BadRequest);
            Response.Headers.Append("Error", "User-Id header is required.");
                
            return false;
        }

        var success = await userService.SaveBreastPumpSettingsAsync(userIdGuid, new BreastPumpSettingEntity
        {
            PumpIntervalHours = request.PumpIntervalHours
        });
        
        return success;
    }

    [Authorize]
    [HttpGet("settings-breastpump")]
    public async Task<BreastPumpSettingsRquestResponse> GetBreastpumpSettingsAsync([FromServices] IUserService userService)
    {
        var userId = Request.Headers["User-Id"];
        if (!Guid.TryParse(userId, out var userIdGuid))
        {
            Response.StatusCode = Convert.ToInt16(HttpStatusCode.BadRequest);
            Response.Headers.Append("Error", "User-Id header is required.");
                
            return new BreastPumpSettingsRquestResponse();
        }
        
        var settings = await userService.GetBreastPumpSettingsAsync(userIdGuid);

        return new BreastPumpSettingsRquestResponse
        {
            PumpIntervalHours = settings.PumpIntervalHours
        };
    }

    [Authorize]
    [HttpPost("invite-care-holder")]
    public async Task<string> InviteCareHolderAsync([FromServices] IUserService userService)
    {
        var userId = Request.Headers["User-Id"];
        if (!Guid.TryParse(userId, out var userIdGuid))
        {
            Response.StatusCode = Convert.ToInt16(HttpStatusCode.BadRequest);
            Response.Headers.Append("Error", "User-Id header is required.");
                
            return string.Empty;;
        }
        
        var inviteSecretCode = await userService.InviteCareHolderAsync(userIdGuid); 
        
        return inviteSecretCode;
    }
}
