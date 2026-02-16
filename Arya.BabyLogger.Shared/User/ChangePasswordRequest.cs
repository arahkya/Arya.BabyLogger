namespace Arya.BabyLogger.Shared.User;

public class ChangePasswordRequest
{
    public string SecretKey { get; set; } = string.Empty;
    public string SecretCode { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}