using Arya.BabyLogger.WebApi.Db;

namespace Arya.BabyLogger.WebApi.Services;

public interface IUserService
{
    UserEntity? LookupEmailAsync(string email);

    string GenerateJwtToken(UserEntity user);
    
    string HashedPassword(string password);
    
    Task<Guid> CreateUserAsync(UserEntity user);

    Task<Guid> CreateNewCareHolderAsync();
    Task<UserEntity?> GetUserByEmailAsync(string requestEmail);
    Task<Tuple<Guid,string>> RequestResetPasswordAsync(string requestEmail);
    Task<bool> ResetPasswordAsync(string secretKey, string secretCode, string requestNewPassword);
    Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
}