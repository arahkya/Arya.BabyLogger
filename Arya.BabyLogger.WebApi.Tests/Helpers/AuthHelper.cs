using System.Net.Http.Json;
using Arya.BabyLogger.Shared.User;

namespace Arya.BabyLogger.WebApi.Tests.Helpers;

public static class AuthHelper
{
    public static async Task<(string Token, Guid UserId)> RegisterAndLoginAsync(HttpClient client)
    {
        var email = $"t{Guid.NewGuid():N}"[..16] + "@t.com";
        var registerRequest = new RegisterRequest
        {
            Username = "testuser",
            Email = email,
            Password = "Test@123"
        };

        var registerResponse = await client.PostAsJsonAsync("api/user/register", registerRequest);
        var userId = await registerResponse.Content.ReadFromJsonAsync<Guid>();

        var loginResponse = await client.PostAsJsonAsync("api/user/login", new LoginRequest
        {
            Email = email,
            Password = "Test@123"
        });

        var login = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        return (login!.Token, userId);
    }

    public static HttpClient WithAuth(this HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return client;
    }
}
