using System.Diagnostics;
using System.Net;
using System.Threading.Channels;
using Arya.BabyLogger.Mobile.Services;
using Arya.BabyLogger.Mobile.Services.Net;
using Arya.BabyLogger.Mobile.ViewModels.BreastPump;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.ApplicationModel;
using Microsoft.Extensions.Logging;

namespace Arya.BabyLogger.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<Channel<bool>>(_ => Channel.CreateUnbounded<bool>());
		builder.Services.AddSingleton<HttpClient>(_ =>
		{
			const string webApiUrl = BuildConstraints.WebApiUrl;
			Debug.WriteLine($"Running with WebApi at {webApiUrl}");
			
			var httpClient = new HttpClient(HttpClientHandlerProvider.CreateHandler())
			{
				BaseAddress = new Uri(webApiUrl),
				Timeout = TimeSpan.FromSeconds(30),
				DefaultRequestVersion = HttpVersion.Version11,
				DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower
			};

			return httpClient;
		});
		
		builder.Services.AddTransient<BreastPumpListViewModel>();
		builder.Services.AddTransient<BreastPumpEntryViewModel>();

		builder.Services.AddSingleton(Badge.Default);

#if IOS
		builder.Services.AddSingleton<ILocalNotificationService, Platforms.iOS.LocalNotificationService>();
#endif

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
