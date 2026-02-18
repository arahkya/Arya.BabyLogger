using System.Diagnostics;
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
		Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
		Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
		Routing.RegisterRoute(nameof(BreastPumpSettingsPage), typeof(BreastPumpSettingsPage));
	}

	protected override async void OnNavigating(ShellNavigatingEventArgs args)
	{
		try
		{
			base.OnNavigating(args);

			var authToken = MobileStorageProvider.GetSecureStorage("AUTH_TOKEN") ?? string.Empty;
			var isAuthenticated = JwtTokenService.IsTokenValid(authToken);

			if (isAuthenticated) return;

			if (Current == null) return;

			MobileStorageProvider.ClearSecureStorage("AUTH_TOKEN");

			await App.SwapGreetingPage();
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Failed to navigate : {ex.Message}");
		}
	}

	private async void OnSignOutClicked(object? sender, EventArgs e)
	{
		try
		{
			MobileStorageProvider.ClearSecureStorage("AUTH_TOKEN");
			await App.SwapGreetingPage();
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Failed to sign out : {ex.Message}");
		}
	}
}
