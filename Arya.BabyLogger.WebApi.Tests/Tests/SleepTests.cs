using System.Net;
using System.Net.Http.Json;
using Arya.BabyLogger.Shared.Sleep;
using Arya.BabyLogger.WebApi.Tests.Helpers;

namespace Arya.BabyLogger.WebApi.Tests.Tests;

[Collection("WebApi")]
public class SleepTests(BabyLoggerWebApiFactory factory)
{
    private async Task<HttpClient> AuthenticatedClient()
    {
        var client = factory.CreateClient();
        var (token, _) = await AuthHelper.RegisterAndLoginAsync(client);
        return client.WithAuth(token);
    }

    private static SleepCreateRequest SampleSleep() => new()
    {
        StartTime = DateTimeOffset.UtcNow.AddHours(-2),
        EndTime = DateTimeOffset.UtcNow,
        Note = "Test sleep"
    };

    [Fact]
    public async Task Create_sleep_returns_201()
    {
        var client = await AuthenticatedClient();
        var response = await client.PostAsJsonAsync("api/sleep", SampleSleep());
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Create_sleep_with_end_before_start_returns_400()
    {
        var client = await AuthenticatedClient();
        var response = await client.PostAsJsonAsync("api/sleep", new SleepCreateRequest
        {
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow.AddHours(-1)
        });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task List_sleeps_returns_created_entry()
    {
        var client = await AuthenticatedClient();
        await client.PostAsJsonAsync("api/sleep", SampleSleep());

        var response = await client.GetAsync("api/sleep");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<SleepListItemsResponse>();
        Assert.NotNull(result);
        Assert.NotEmpty(result.Items);
    }

    [Fact]
    public async Task Get_sleep_by_id_returns_correct_data()
    {
        var client = await AuthenticatedClient();
        var create = await client.PostAsJsonAsync("api/sleep", SampleSleep());
        var id = ExtractIdFromLocation(create);

        var response = await client.GetAsync($"api/sleep/{id}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<SleepDetailResponse>();
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task Update_sleep_returns_204()
    {
        var client = await AuthenticatedClient();
        var create = await client.PostAsJsonAsync("api/sleep", SampleSleep());
        var id = ExtractIdFromLocation(create);

        var response = await client.PutAsJsonAsync($"api/sleep/{id}", new SleepUpdateRequest
        {
            StartTime = DateTimeOffset.UtcNow.AddHours(-3),
            EndTime = DateTimeOffset.UtcNow,
            Note = "Updated sleep"
        });

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_sleep_returns_204()
    {
        var client = await AuthenticatedClient();
        var create = await client.PostAsJsonAsync("api/sleep", SampleSleep());
        var id = ExtractIdFromLocation(create);

        var response = await client.DeleteAsync($"api/sleep/{id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    private static Guid ExtractIdFromLocation(HttpResponseMessage response)
    {
        var location = response.Headers.Location!.ToString();
        return Guid.Parse(location.Split('/').Last());
    }
}
