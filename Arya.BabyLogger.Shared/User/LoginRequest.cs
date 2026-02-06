namespace Arya.BabyLogger.Shared.User;

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
}