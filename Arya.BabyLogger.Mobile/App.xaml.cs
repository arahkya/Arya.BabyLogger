using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json; 
using Arya.BabyLogger.Mobile.Services;
using Arya.BabyLogger.Mobile.Services.Jwt;
using Arya.BabyLogger.Mobile.Services.Net;
using Arya.BabyLogger.Mobile.Services.Storage;
using Arya.BabyLogger.Mobile.ViewModels.Landing;
using Arya.BabyLogger.Mobile.Views.Landing;
using Arya.BabyLogger.Shared.User;

namespace Arya.BabyLogger.Mobile;

public partial class App
{
	public static IServiceProvider Services => Current?.Handler?.MauiContext?.Services ?? throw new InvalidOperationException("Service provider is not available.");
	
	private readonly GreetingPage? _greetingPage;
	
	public App(GreetingPage greetingPage)
	{
		InitializeComponent();
		_greetingPage = greetingPage;
	}
	
	protected override Window CreateWindow(IActivationState? activationState)
	{
		var appShell = new AppShell();
		var authToken = MobileStorageProvider.GetSecureStorage("AUTH_TOKEN") ?? string.Empty;
		var isAuthenticated = JwtTokenService.IsTokenValid(authToken);
		var window = new Window(isAuthenticated ? appShell : new NavigationPage(_greetingPage!));
		
		return window;
	}

	protected override async void OnStart()
	{
		try
		{
			await LoadBreastpumpSettings();
		}
		catch (Exception e)
		{
			Debug.WriteLine(e);
		}
	}

	private static async Task LoadBreastpumpSettings()
	{
		var authToken = MobileStorageProvider.GetSecureStorage("AUTH_TOKEN") ?? string.Empty;
		
		if(authToken == string.Empty) return;
		
		var httpClient = Services.GetRequiredService<HttpClient>();
		var userId = JwtTokenService.GetClaim("sub", authToken);
		
		httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
		httpClient.DefaultRequestHeaders.Add("User-Id", userId);
		
		var breastPumpSettings = await httpClient.GetFromJsonAsync<BreastPumpSettingsRquestResponse>("user/settings-breastpump");
		
		Preferences.Set("BreastPumpTimeIntervalHours", breastPumpSettings!.PumpIntervalHours);
	}
	
	public static async Task SwapMainPage()
	{
		await MainThread.InvokeOnMainThreadAsync(() =>
		{
			var shell = new AppShell();
			Current!.Windows[0].Page = shell;
		});
	}

	public static async Task SwapGreetingPage()
	{
		await MainThread.InvokeOnMainThreadAsync(() =>
		{
			const string webApiUrl = BuildConstraints.WebApiUrl;
			var greetingPage = new GreetingPage(new GreetingViewModel(new HttpClient(HttpClientHandlerProvider.CreateHandler())
			{
				BaseAddress = new Uri(webApiUrl),
				Timeout = TimeSpan.FromSeconds(30),
				DefaultRequestVersion = HttpVersion.Version11,
				DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower
			}));
			
			Current!.Windows[0].Page = new NavigationPage(greetingPage);
		});
	}
}
