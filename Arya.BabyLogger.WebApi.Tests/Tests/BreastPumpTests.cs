using System.Net;
using System.Net.Http.Json;
using Arya.BabyLogger.Shared.BreastPump;
using Arya.BabyLogger.WebApi.Tests.Helpers;

namespace Arya.BabyLogger.WebApi.Tests.Tests;

[Collection("WebApi")]
public class BreastPumpTests(BabyLoggerWebApiFactory factory)
{
    private async Task<(HttpClient Client, Guid UserId)> AuthenticatedClient()
    {
        var client = factory.CreateClient();
        var (token, userId) = await AuthHelper.RegisterAndLoginAsync(client);
        client.WithAuth(token);
        client.DefaultRequestHeaders.Add("User-Id", userId.ToString());
        return (client, userId);
    }

    private static BreastPumpCreateRequest SamplePump() => new()
    {
        PumpTime = DateTimeOffset.UtcNow,
        AmountML = 80,
        Note = "Test pump"
    };

    [Fact]
    public async Task Create_breastpump_returns_201()
    {
        var (client, _) = await AuthenticatedClient();
        var response = await client.PostAsJsonAsync("api/breastpump", SamplePump());
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Create_breastpump_with_zero_amount_returns_400()
    {
        var (client, _) = await AuthenticatedClient();
        var response = await client.PostAsJsonAsync("api/breastpump", new BreastPumpCreateRequest
        {
            PumpTime = DateTimeOffset.UtcNow,
            AmountML = 0
        });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task List_breastpumps_returns_created_entry()
    {
        var (client, _) = await AuthenticatedClient();
        await client.PostAsJsonAsync("api/breastpump", SamplePump());

        var response = await client.GetAsync("api/breastpump");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<BreastPumpListItemsResponse>();
        Assert.NotNull(result);
        Assert.NotEmpty(result.Items);
    }

    [Fact]
    public async Task Get_breastpump_by_id_returns_correct_data()
    {
        var (client, _) = await AuthenticatedClient();
        var create = await client.PostAsJsonAsync("api/breastpump", SamplePump());
        var id = ExtractIdFromLocation(create);

        var response = await client.GetAsync($"api/breastpump/{id}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<BreastPumpDetailResponse>();
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task Update_breastpump_returns_204()
    {
        var (client, _) = await AuthenticatedClient();
        var create = await client.PostAsJsonAsync("api/breastpump", SamplePump());
        var id = ExtractIdFromLocation(create);

        var response = await client.PutAsJsonAsync($"api/breastpump/{id}", new BreastPumpUpdateRequest
        {
            PumpTime = DateTimeOffset.UtcNow,
            AmountML = 100,
            Note = "Updated"
        });

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_breastpump_returns_204()
    {
        var (client, _) = await AuthenticatedClient();
        var create = await client.PostAsJsonAsync("api/breastpump", SamplePump());
        var id = ExtractIdFromLocation(create);

        var response = await client.DeleteAsync($"api/breastpump/{id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Missing_user_id_header_returns_400()
    {
        var client = factory.CreateClient();
        var (token, _) = await AuthHelper.RegisterAndLoginAsync(client);
        client.WithAuth(token);
        // intentionally NOT setting User-Id header

        var response = await client.PostAsJsonAsync("api/breastpump", SamplePump());
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private static Guid ExtractIdFromLocation(HttpResponseMessage response)
    {
        var location = response.Headers.Location!.ToString();
        return Guid.Parse(location.Split('/').Last());
    }
}
