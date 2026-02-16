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
    Task<Tuple<Guid,string>> ResetPasswordAsync(string requestEmail);
    Task<bool> ChangePasswordAsync(string secretKey, string secretCode, string requestNewPassword);
}