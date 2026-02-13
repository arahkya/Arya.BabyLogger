using System.Diagnostics;
using System.Net;
using System.Threading.Channels;
using Arya.BabyLogger.Mobile.Services;
using Arya.BabyLogger.Mobile.Services.Jwt;
using Arya.BabyLogger.Mobile.Services.Net;
using Arya.BabyLogger.Mobile.Services.Storage;
using Arya.BabyLogger.Mobile.ViewModels.Landing;
using Arya.BabyLogger.Mobile.Views.Landing;

namespace Arya.BabyLogger.Mobile;

public partial class App
{
	public static IServiceProvider Services => Current?.Handler?.MauiContext?.Services ?? throw new InvalidOperationException("Service provider is not available.");

	private readonly Channel<bool> _recalculateNextPumpTimeChannel;
	private readonly GreetingPage? _greetingPage;
	
	public App(
		Channel<bool> recalculateNextPumpTimeChannel,
		GreetingPage greetingPage)
	{
		InitializeComponent();
		this._greetingPage = greetingPage;
		this._recalculateNextPumpTimeChannel = recalculateNextPumpTimeChannel;
	}
	
	protected override Window CreateWindow(IActivationState? activationState)
	{
		var appShell = new AppShell();
		var authToken = MobileStorageProvider.GetSecureStorage("AUTH_TOKEN") ?? string.Empty;
		var isAuthenticated = JwtTokenService.IsTokenValid(authToken);
		var window = new Window(isAuthenticated ? appShell : new NavigationPage(_greetingPage!));

		window.Activated += async (_, _) =>
		{
			Debug.WriteLine("Window Activated");
			
			await _recalculateNextPumpTimeChannel.Writer.WriteAsync(true);
		};
		
		return window;
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
