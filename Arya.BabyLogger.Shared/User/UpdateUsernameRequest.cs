using System.ComponentModel.DataAnnotations;

namespace Arya.BabyLogger.Shared.User;

public class UpdateUsernameRequest
{
    [Required]
    [MinLength(4)]
    [MaxLength(12)]
    public string Username { get; init; } = string.Empty;
}
