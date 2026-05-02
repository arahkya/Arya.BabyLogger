using System.Net;
using System.Net.Http.Json;
using Arya.BabyLogger.Shared.Feed;
using Arya.BabyLogger.WebApi.Tests.Helpers;

namespace Arya.BabyLogger.WebApi.Tests.Tests;

[Collection("WebApi")]
public class FeedTests(BabyLoggerWebApiFactory factory)
{
    private async Task<HttpClient> AuthenticatedClient()
    {
        var client = factory.CreateClient();
        var (token, _) = await AuthHelper.RegisterAndLoginAsync(client);
        return client.WithAuth(token);
    }

    private static CreateFeedEntryRequest SampleFeed() => new()
    {
        Time = DateTimeOffset.UtcNow,
        Amount = 120,
        Unit = "ml",
        Type = "Breast Milk",
        Note = "Test feed"
    };

    [Fact]
    public async Task Create_feed_returns_201()
    {
        var client = await AuthenticatedClient();
        var response = await client.PostAsJsonAsync("api/feed", SampleFeed());
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Get_feeds_returns_created_entry()
    {
        var client = await AuthenticatedClient();
        await client.PostAsJsonAsync("api/feed", SampleFeed());

        var response = await client.GetAsync("api/feed");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ListFeedResponse>();
        Assert.NotNull(result);
        Assert.NotEmpty(result.Items);
    }

    [Fact]
    public async Task Get_feed_by_id_returns_correct_data()
    {
        var client = await AuthenticatedClient();
        var create = await client.PostAsJsonAsync("api/feed", SampleFeed());
        var id = ExtractIdFromLocation(create);

        var response = await client.GetAsync($"api/feed/{id}");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Update_feed_returns_200()
    {
        var client = await AuthenticatedClient();
        var create = await client.PostAsJsonAsync("api/feed", SampleFeed());
        var id = ExtractIdFromLocation(create);

        var response = await client.PutAsJsonAsync($"api/feed/{id}", new UpdateFeedEntryRequest
        {
            Time = DateTimeOffset.UtcNow,
            Amount = 150,
            Unit = "ml",
            Type = "Formula",
            Note = "Updated"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Delete_feed_returns_200_and_get_returns_404()
    {
        var client = await AuthenticatedClient();
        var create = await client.PostAsJsonAsync("api/feed", SampleFeed());
        var id = ExtractIdFromLocation(create);

        var delete = await client.DeleteAsync($"api/feed/{id}");
        Assert.Equal(HttpStatusCode.OK, delete.StatusCode);

        var get = await client.GetAsync($"api/feed/{id}");
        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
    }

    private static Guid ExtractIdFromLocation(HttpResponseMessage response)
    {
        var location = response.Headers.Location!.ToString();
        return Guid.Parse(location.Split('/').Last());
    }
}
