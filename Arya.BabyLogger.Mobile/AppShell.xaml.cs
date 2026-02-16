using Arya.BabyLogger.Mobile.Services.Jwt;
using Arya.BabyLogger.Mobile.Services.Storage;
using Arya.BabyLogger.Mobile.ViewModels;
using Arya.BabyLogger.Mobile.Views.BreastPump;
using Arya.BabyLogger.Mobile.Views.Landing;
using Arya.BabyLogger.Mobile.Views.Profile;

namespace Arya.BabyLogger.Mobile;

public partial class AppShell
{
	public AppShell()
	{
		InitializeComponent();

		BindingContext = new ShellViewModel();
		
		Routing.RegisterRoute(nameof(BreastPumpListPage), typeof(BreastPumpListPage));
		Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
		Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(Views.Profile.ChangePasswordPage));
		Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
	}

	protected override void OnNavigating(ShellNavigatingEventArgs args)
	{
		base.OnNavigating(args);
		
		var authToken = MobileStorageProvider.GetSecureStorage("AUTH_TOKEN") ?? string.Empty;
		var isAuthenticated = JwtTokenService.IsTokenValid(authToken);

		if (isAuthenticated) return;

		if (Current == null) return;
		
		MobileStorageProvider.ClearSecureStorage("AUTH_TOKEN");
		
		App.SwapGreetingPage().GetAwaiter().GetResult();
	}

	private async void OnSignOutClicked(object? sender, EventArgs e)
	{
		MobileStorageProvider.ClearSecureStorage("AUTH_TOKEN");
		await App.SwapGreetingPage();
	}
}
