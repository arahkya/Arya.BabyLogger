using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Arya.BabyLogger.WebApi.Db;
using Microsoft.IdentityModel.Tokens;

namespace Arya.BabyLogger.WebApi.Services;

public class UserService(BabyLoggerDbContext dbContext) : IUserService
{
    public UserEntity? LookupUserNameAsync(string username)
    {
        var user = dbContext.Users.FirstOrDefault(u => u.Username == username);

        return user;
    }

    public string GenerateJwtToken(UserEntity user)
    {
        var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new Exception("JWT_ISSUER not set");
        var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new Exception("JWT_AUDIENCE not set");
        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? throw new Exception("JWT_SECRET_KEY not set");
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
            ],
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}