using Arya.BabyLogger.Mobile.Services.Jwt;
using Arya.BabyLogger.Mobile.Services.Storage;
using Arya.BabyLogger.Mobile.Views.BreastPump;
using Arya.BabyLogger.Mobile.Views.Landing;

namespace Arya.BabyLogger.Mobile;

public partial class AppShell
{
	public AppShell()
	{
		InitializeComponent();
		
		Routing.RegisterRoute(nameof(BreastPumpListPage), typeof(BreastPumpListPage));
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
}
