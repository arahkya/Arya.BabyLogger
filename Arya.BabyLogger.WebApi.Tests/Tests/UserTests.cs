using System.Net;
using System.Net.Http.Json;
using Arya.BabyLogger.Shared.User;
using Arya.BabyLogger.WebApi.Tests.Helpers;

namespace Arya.BabyLogger.WebApi.Tests.Tests;

[Collection("WebApi")]
public class UserTests(BabyLoggerWebApiFactory factory)
{
    private HttpClient Client => factory.CreateClient();

    [Fact]
    public async Task Register_returns_guid_user_id()
    {
        var response = await Client.PostAsJsonAsync("api/user/register", new RegisterRequest
        {
            Username = "testuser",
            Email = $"t{Guid.NewGuid():N}"[..16] + "@t.com",
            Password = "Test@123"
        });

        response.EnsureSuccessStatusCode();
        var userId = await response.Content.ReadFromJsonAsync<Guid>();
        Assert.NotEqual(Guid.Empty, userId);
    }

    [Fact]
    public async Task Register_duplicate_email_returns_conflict()
    {
        var email = $"t{Guid.NewGuid():N}"[..16] + "@t.com";
        var request = new RegisterRequest { Username = "testuser", Email = email, Password = "Test@123" };

        await Client.PostAsJsonAsync("api/user/register", request);
        var response = await Client.PostAsJsonAsync("api/user/register", request);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Login_with_valid_credentials_returns_jwt_token()
    {
        var (token, _) = await AuthHelper.RegisterAndLoginAsync(Client);
        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Fact]
    public async Task Login_with_wrong_password_returns_unauthorized()
    {
        var email = $"t{Guid.NewGuid():N}"[..16] + "@t.com";
        await Client.PostAsJsonAsync("api/user/register", new RegisterRequest
        {
            Username = "testuser", Email = email, Password = "Test@123"
        });

        var response = await Client.PostAsJsonAsync("api/user/login", new LoginRequest
        {
            Email = email, Password = "WrongPass"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Change_password_succeeds()
    {
        var client = Client;
        var (token, userId) = await AuthHelper.RegisterAndLoginAsync(client);
        client.WithAuth(token);
        client.DefaultRequestHeaders.Add("User-Id", userId.ToString());

        var response = await client.PatchAsJsonAsync("api/user/change-password", new ChangePasswordRequest
        {
            CurrentPassword = "Test@123",
            NewPassword = "NewPass@456"
        });

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<bool>();
        Assert.True(result);
    }

    [Fact]
    public async Task Update_username_succeeds()
    {
        var client = Client;
        var (token, userId) = await AuthHelper.RegisterAndLoginAsync(client);
        client.WithAuth(token);
        client.DefaultRequestHeaders.Add("User-Id", userId.ToString());

        var response = await client.PatchAsJsonAsync("api/user/username", new UpdateUsernameRequest
        {
            Username = "newname"
        });

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Get_breastpump_settings_returns_default()
    {
        var client = Client;
        var (token, userId) = await AuthHelper.RegisterAndLoginAsync(client);
        client.WithAuth(token);
        client.DefaultRequestHeaders.Add("User-Id", userId.ToString());

        var response = await client.GetAsync("api/user/settings-breastpump");

        response.EnsureSuccessStatusCode();
        var settings = await response.Content.ReadFromJsonAsync<BreastPumpSettingsRquestResponse>();
        Assert.NotNull(settings);
        Assert.True(settings.PumpIntervalHours > 0);
    }

    [Fact]
    public async Task Save_breastpump_settings_persists()
    {
        var client = Client;
        var (token, userId) = await AuthHelper.RegisterAndLoginAsync(client);
        client.WithAuth(token);
        client.DefaultRequestHeaders.Add("User-Id", userId.ToString());

        var response = await client.PutAsJsonAsync("api/user/settings-breastpump",
            new BreastPumpSettingsRquestResponse { PumpIntervalHours = 6 });

        response.EnsureSuccessStatusCode();
        var saved = await response.Content.ReadFromJsonAsync<bool>();
        Assert.True(saved);
    }
}
