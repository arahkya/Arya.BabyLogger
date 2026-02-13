using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Arya.BabyLogger.WebApi.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Arya.BabyLogger.WebApi.Services;

public class UserService(BabyLoggerDbContext dbContext, IConfiguration configuration) : IUserService
{
    public UserEntity? LookupEmailAsync(string email)
    {
        var user = dbContext.Users.FirstOrDefault(u => u.Email == email);

        return user;
    }

    public string GenerateJwtToken(UserEntity user)
    {
        var jwtConfig = configuration.GetSection("Jwt");
        var issuer = jwtConfig["Issuer"] ?? throw new Exception("Jwt:Issuer not set");
        var audience = jwtConfig["Audience"] ?? throw new Exception("Jwt:Audience not set");
        var secretKey = jwtConfig["SigningKey"] ?? throw new Exception("Jwt:SingingKey not set");
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
            ],
            expires: DateTime.UtcNow.AddMonths(1),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    public string HashedPassword(string password)
    {
        var secretKey = Environment.GetEnvironmentVariable("UserHashingPasswordKey") ?? throw new Exception("UserHashingPasswordKey not set");
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        using var hmac = new HMACSHA256(keyBytes);
        var hashBytes = hmac.ComputeHash(passwordBytes);

        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }

    public async Task<Guid> CreateNewCareHolderAsync()
    {
        var careHousehold = new CareHolderEntity();
        
        await dbContext.CareHolders.AddAsync(careHousehold);
        await dbContext.SaveChangesAsync();
        
        return careHousehold.Id;
    }

    public async Task<UserEntity?> GetUserByEmailAsync(string requestEmail)
    {
        var userEntity = await dbContext.Users.SingleOrDefaultAsync(p => p.Email == requestEmail);
        
        return userEntity;
    }

    public async Task<Guid> CreateUserAsync(UserEntity user)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        
        return user.Id;
    }
}
