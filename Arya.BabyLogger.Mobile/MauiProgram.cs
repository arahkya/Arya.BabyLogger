using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Channels;
using Arya.BabyLogger.Mobile.Services;
using Arya.BabyLogger.Mobile.ViewModels.BreastPump;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.ApplicationModel;
using Microsoft.Extensions.Logging;

namespace Arya.BabyLogger.Mobile;

public static class MauiProgram
{
	/// <summary>
	/// https://medium.com/@rahmitugrulaltin/net-maui-httpclient-request-boost-up-ios-d00d39c3e2fc
	/// </summary>
	/// <returns></returns>
	/// <exception cref="SocketException"></exception>
	private static HttpMessageHandler CreateHandler()
	{
		var isWifi = Connectivity.Current.ConnectionProfiles.Contains(ConnectionProfile.WiFi);

		var socketHandlerItem = new SocketsHttpHandler
		{
			AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Brotli,
			EnableMultipleHttp2Connections = true,
			PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
			ConnectTimeout = TimeSpan.FromSeconds(15),

			// Wi-Fi detect proxy/PAC
			UseProxy = !isWifi,

			// HTTP/2 keep-alive ping 
			KeepAlivePingDelay = TimeSpan.FromSeconds(20),
			KeepAlivePingTimeout = TimeSpan.FromSeconds(10),
			KeepAlivePingPolicy = HttpKeepAlivePingPolicy.Always
		};

		if (isWifi)
		{
			// Wi-Fi’de IPv6/ route to force IPv4 
			socketHandlerItem.ConnectCallback = async (ctx, ct) =>
			{
				var host = ctx.DnsEndPoint.Host;
				var port = ctx.DnsEndPoint.Port;
				var addresses = await Dns.GetHostAddressesAsync(host, CancellationToken.None);
				var v4 = Array.Find(addresses, a => a.AddressFamily == AddressFamily.InterNetwork) ?? (addresses.Length > 0 ? addresses[0] : throw new SocketException((int)SocketError.HostNotFound));
				var sock = new Socket(v4.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				
				await using var reg = ct.Register(() => { try { sock.Dispose(); }
					catch
					{
						// ignored
					}
				});
				await sock.ConnectAsync(new IPEndPoint(v4, port), ct);
				return new NetworkStream(sock, ownsSocket: true);
			};
		}

		return socketHandlerItem;
	}

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
			
			var httpClient = new HttpClient(CreateHandler())
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
