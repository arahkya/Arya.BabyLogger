using System.ComponentModel.DataAnnotations;

namespace Arya.BabyLogger.Shared.User;

public class RegisterRequest
{
    [Required]
    [MinLength(4)]
    [MaxLength(12)]
    public string Username { get; init; } = string.Empty;
    [Required]
    [MinLength(8)]
    [MaxLength(50)]
    public string Email { get; init; } = string.Empty;
    [Required]
    [MinLength(4)]
    [MaxLength(15)]
    public string Password { get; init; } = string.Empty;
    public string? CareHolderId { get; init; }
}