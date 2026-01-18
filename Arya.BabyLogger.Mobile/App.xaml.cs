using Microsoft.Extensions.DependencyInjection;

namespace Arya.BabyLogger.Mobile;

public partial class App : Application
{
	public static IServiceProvider Services => Current?.Handler?.MauiContext?.Services ?? throw new InvalidOperationException("Service provider is not available.");

	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}