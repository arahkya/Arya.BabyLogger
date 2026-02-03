using System.Diagnostics;
using System.Threading.Channels;

namespace Arya.BabyLogger.Mobile;

public partial class App
{
	public static IServiceProvider Services => Current?.Handler?.MauiContext?.Services ?? throw new InvalidOperationException("Service provider is not available.");

	private readonly Channel<bool> _recalculateNextPumpTimeChannel;
	
	public App(
		Channel<bool> recalculateNextPumpTimeChannel)
	{
		InitializeComponent();
		
		this._recalculateNextPumpTimeChannel = recalculateNextPumpTimeChannel;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var windows = new Window(new AppShell());

		windows.Activated += async (_, _) =>
		{
			Debug.WriteLine("Window Activated");
			
			await _recalculateNextPumpTimeChannel.Writer.WriteAsync(true);
		};
		
		return windows;
	}
}