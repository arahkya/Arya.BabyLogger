using System.ComponentModel.DataAnnotations;

namespace Arya.BabyLogger.WebApi.Db;

public class UserEntity
{
    public Guid Id { get; set; }
    
    [MinLength(4)]
    [MaxLength(12)]
    public required string Username { get; set; } = string.Empty;
    
    [MinLength(4)]
    [MaxLength(30)]
    public required string Email { get; set; } = string.Empty;
    
    [MinLength(10)]
    [MaxLength(100)]
    public required string PasswordHash { get; set; } = string.Empty;
}