using System.ComponentModel.DataAnnotations;

namespace Arya.BabyLogger.WebApi.Db;

public class CareHolderEntity
{
    public Guid Id { get; init; }
    
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(10)]
    public string? InviteCode { get; set;}
    
    [MaxLength(70)]
    public string? InviteUserEmail { get; set; }
}