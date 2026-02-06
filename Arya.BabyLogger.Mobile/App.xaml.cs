using System.Diagnostics;
using System.Threading.Channels;
using Arya.BabyLogger.Mobile.Services.Jwt;
using Arya.BabyLogger.Mobile.Services.Storage;
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
		var authToken = MobileStorageProvider.GetSecureStorage("AUTH_TOKEN") ?? throw new InvalidOperationException("AUTH_TOKEN is not set.");
		var isAuthenticated = JwtTokenService.IsTokenValid(authToken);
		var window = new Window(isAuthenticated ? appShell : _greetingPage!);

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
}
