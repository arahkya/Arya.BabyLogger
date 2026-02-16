using Arya.BabyLogger.Mobile.Services.Jwt;
using Arya.BabyLogger.Mobile.Services.Storage;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Arya.BabyLogger.Mobile.ViewModels;

public partial class ShellViewModel : ObservableObject
{
	[ObservableProperty]
	private string _username = "Guest";

	public ShellViewModel()
	{
		var token = MobileStorageProvider.GetSecureStorage("AUTH_TOKEN") ?? string.Empty;
		var nameClaim = JwtTokenService.GetClaim("unique_name", token);

		if (!string.IsNullOrWhiteSpace(nameClaim))
		{
			Username = nameClaim;
		}
	}
}
