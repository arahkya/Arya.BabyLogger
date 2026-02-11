using System.Text;
using System.Text.Json;

namespace Arya.BabyLogger.Mobile.Services.Jwt;

public static class JwtTokenService
{
    /// <summary>
    /// Basic JWT validator that checks structure and expiry claims (nbf/exp).
    /// </summary>
    /// <param name="token">JWT bearer token</param>
    /// <returns>True when the token is well-formed and not expired.</returns>
    public static bool IsTokenValid(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        var parts = token.Split('.');
        if (parts.Length != 3)
        {
            return false;
        }

        try
        {
            var payloadJson = DecodeBase64Url(parts[1]);
            using var payload = JsonDocument.Parse(payloadJson);

            var now = DateTimeOffset.UtcNow;

            if (payload.RootElement.TryGetProperty("nbf", out var nbfElement) &&
                TryReadEpochSeconds(nbfElement, out var notBefore) &&
                now < notBefore)
            {
                return false;
            }

            if (!payload.RootElement.TryGetProperty("exp", out var expElement) ||
                !TryReadEpochSeconds(expElement, out var expiresAt))
            {
                return false; // missing or invalid expiry
            }

            return now < expiresAt;
        }
        catch
        {
            return false; // malformed payload
        }
    }

    private static bool TryReadEpochSeconds(JsonElement element, out DateTimeOffset value)
    {
        value = default;

        if (element.ValueKind == JsonValueKind.Number && element.TryGetInt64(out var seconds))
        {
            value = DateTimeOffset.FromUnixTimeSeconds(seconds);
            return true;
        }

        if (element.ValueKind == JsonValueKind.String &&
            long.TryParse(element.GetString(), out seconds))
        {
            value = DateTimeOffset.FromUnixTimeSeconds(seconds);
            return true;
        }

        return false;
    }

    private static string DecodeBase64Url(string input)
    {
        var padded = input.Replace('-', '+').Replace('_', '/');
        padded = padded.PadRight(padded.Length + ((4 - padded.Length % 4) % 4), '=');

        var bytes = Convert.FromBase64String(padded);
        return Encoding.UTF8.GetString(bytes);
    }
    
    public static string GetClaim(string claimKey, string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return string.Empty;
        }

        var parts = token.Split('.');
        if (parts.Length != 3)
        {
            return string.Empty;
        }

        try
        {
            var payloadJson = DecodeBase64Url(parts[1]);
            using var payload = JsonDocument.Parse(payloadJson);

            var now = DateTimeOffset.UtcNow;

            if (!payload.RootElement.TryGetProperty(claimKey, out var nbfElement))
            {
                return string.Empty;
            }

            return nbfElement.GetString() ?? string.Empty;
        }
        catch
        {
            return string.Empty; // malformed payload
        }
    }
}
