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
        
        var userEntity = new UserEntity
        {
            Username = request.Username,
            PasswordHash = userService.HashedPassword(request.Password),
            Email = request.Email,
            CareHouseholdId = careHolderId
        };
        
        var id= await userService.CreateUserAsync(userEntity);

        return id;
    }
}