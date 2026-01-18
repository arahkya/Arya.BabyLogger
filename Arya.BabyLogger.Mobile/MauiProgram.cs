using System.Net;
using System.Net.Sockets;
using Arya.BabyLogger.Mobile.ViewModels;
using Arya.BabyLogger.Mobile.ViewModels.BreastPump;
using Arya.BabyLogger.Mobile.ViewModels.Feed;
using CommunityToolkit.Maui;
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

		var ScoketHandlerItem = new SocketsHttpHandler
		{
			AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Brotli,
			EnableMultipleHttp2Connections = true,
			PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
			ConnectTimeout = TimeSpan.FromSeconds(15),

			// Wi-Fi detech proxy/PAC
			UseProxy = !isWifi ? true : false,

			// HTTP/2 keep-alive ping 
			KeepAlivePingDelay = TimeSpan.FromSeconds(20),
			KeepAlivePingTimeout = TimeSpan.FromSeconds(10),
			KeepAlivePingPolicy = HttpKeepAlivePingPolicy.Always
		};

		if (isWifi)
		{
			// Wi-Fi’de IPv6/ route to force IPv4 
			ScoketHandlerItem.ConnectCallback = async (ctx, ct) =>
			{
				var host = ctx.DnsEndPoint.Host;
				var port = ctx.DnsEndPoint.Port;

				var addrs = await Dns.GetHostAddressesAsync(host);
				var v4 = Array.Find(addrs, a => a.AddressFamily == AddressFamily.InterNetwork);
				if (v4 == default)
					v4 = addrs.Length > 0 ? addrs[0] : throw new SocketException((int)SocketError.HostNotFound);

				var sock = new Socket(v4.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				using var reg = ct.Register(() => { try { sock.Dispose(); } catch { } });
				await sock.ConnectAsync(new IPEndPoint(v4, port), ct);
				return new NetworkStream(sock, ownsSocket: true);
			};
		}

		return ScoketHandlerItem;
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

		builder.Services.AddSingleton<HttpClient>(p =>
		{
			var httpClient = new HttpClient(CreateHandler())
			{
				BaseAddress = new Uri("https://baby-logger.arahk.com/api/"),
				//BaseAddress = new Uri("http://localhost:5001/api/"),

				Timeout = TimeSpan.FromSeconds(30),
				DefaultRequestVersion = HttpVersion.Version11,
				DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower
			};

			return httpClient;
		});
		builder.Services.AddTransient<MainPageViewModel>();
		builder.Services.AddTransient<FeedListViewModel>();
		builder.Services.AddTransient<FeedEntryViewModel>();
		builder.Services.AddTransient<BreastPumpListViewModel>();
		builder.Services.AddTransient<BreastPumpEntryViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
