using System.Net;
using System.Net.Sockets;

namespace Arya.BabyLogger.Mobile.Services.Net;

public static class HttpClientHandlerProvider
{
    /// <summary>
    /// https://medium.com/@rahmitugrulaltin/net-maui-httpclient-request-boost-up-ios-d00d39c3e2fc
    /// </summary>
    /// <returns></returns>
    /// <exception cref="SocketException"></exception>
    public static HttpMessageHandler CreateHandler()
    {
        var isWifi = Connectivity.Current.ConnectionProfiles.Contains(ConnectionProfile.WiFi);

        var httpMessageHandler = new SocketsHttpHandler
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
            httpMessageHandler.ConnectCallback = async (ctx, ct) =>
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

        return httpMessageHandler;
    }
}