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

    public async Task<Tuple<Guid,string>> RequestResetPasswordAsync(string requestEmail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(requestEmail);

        var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Email == requestEmail);
        if (user is null)
        {
            throw new Exception("Email not found");
        }

        var requestResetPasswordAttempt = await dbContext.ResetPasswordRequests.CountAsync(r => r.UserId == user.Id);
        if (requestResetPasswordAttempt > 3)
        {
            throw new Exception("You account locked please contact arahk@arahk.com");
        }
        
        var secretCode = GenerateSixDigitCode();

        var resetRequest = new ResetPasswordRequestEntity
        {
            UserId = user.Id,
            SecretCode = secretCode
        };

        await dbContext.ResetPasswordRequests.AddAsync(resetRequest);
        await dbContext.SaveChangesAsync();

        return new Tuple<Guid, string>(resetRequest.Id ,secretCode);
    }

    public async Task<bool> ResetPasswordAsync(string secretKey, string secretCode, string requestNewPassword)
    {
        var resetPassword = await dbContext.ResetPasswordRequests.SingleOrDefaultAsync(p => p.Id.ToString() == secretKey);
        
        if (resetPassword is null)
        {
            throw new Exception("Invalid request");
        }

        var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Id == resetPassword.UserId);
        
        if (user is null)
        {
            throw new Exception("Invalid request");
        }
        
        if (resetPassword.SecretCode != secretCode)
        {
            throw new Exception("Invalid secret code");
        }
        
        user.PasswordHash = HashedPassword(requestNewPassword);
        
        dbContext.ResetPasswordRequests.Remove(resetPassword);
        
        var effectedRows = await dbContext.SaveChangesAsync();
        
        return effectedRows > 0;
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
        if (user is null) throw new Exception("User not found");

        if (user.PasswordHash != HashedPassword(currentPassword)) throw new Exception("Current password is incorrect");
        
        user.PasswordHash = HashedPassword(newPassword);
        await dbContext.SaveChangesAsync();
        
        return true;
    }
    
    private static string GenerateSixDigitCode()
    {
        // Generates a zero-padded 6-digit number using a cryptographically secure RNG.
        Span<byte> buffer = stackalloc byte[4];
        RandomNumberGenerator.Fill(buffer);
        var value = BitConverter.ToUInt32(buffer) % 1_000_000;
        return value.ToString("D6");
    }

    public async Task<Guid> CreateUserAsync(UserEntity user)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        
        return user.Id;
    }
}
