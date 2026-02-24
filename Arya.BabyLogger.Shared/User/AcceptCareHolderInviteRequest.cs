using System.ComponentModel.DataAnnotations;

namespace Arya.BabyLogger.Shared.User;

public class AcceptCareHolderInviteRequest
{
    [Required]
    [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "Invite code must be 6 digits.")]
    public string InviteSecret { get; init; } = string.Empty;
}
