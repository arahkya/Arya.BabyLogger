using Arya.BabyLogger.WebApi.Db;

namespace Arya.BabyLogger.WebApi.Services;

public interface IUserService
{
    UserEntity? LookupUserNameAsync(string username);

    string GenerateJwtToken(UserEntity user);
}