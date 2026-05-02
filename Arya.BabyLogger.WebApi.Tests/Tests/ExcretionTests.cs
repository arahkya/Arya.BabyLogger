using System.Net;
using System.Net.Http.Json;
using Arya.BabyLogger.Shared.Excretion;
using Arya.BabyLogger.WebApi.Tests.Helpers;

namespace Arya.BabyLogger.WebApi.Tests.Tests;

[Collection("WebApi")]
public class ExcretionTests(BabyLoggerWebApiFactory factory)
{
    private async Task<HttpClient> AuthenticatedClient()
    {
        var client = factory.CreateClient();
        var (token, _) = await AuthHelper.RegisterAndLoginAsync(client);
        return client.WithAuth(token);
    }

    private static ExcretionCreateRequest SampleExcretion() => new()
    {
        ExcretionDateTime = DateTime.UtcNow,
        ExcretionLevel = 3,
        ExcretionColor = "#FFFF00",
        Consistency = "Soft",
        Note = "Test excretion"
    };

    [Fact]
    public async Task Create_excretion_returns_201()
    {
        var client = await AuthenticatedClient();
        var response = await client.PostAsJsonAsync("api/excretion", SampleExcretion());
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task List_excretions_returns_created_entry()
    {
        var client = await AuthenticatedClient();
        await client.PostAsJsonAsync("api/excretion", SampleExcretion());

        var response = await client.GetAsync("api/excretion");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ExcretionListItemsResponse>();
        Assert.NotNull(result);
        Assert.NotEmpty(result.Items);
    }

    [Fact]
    public async Task Get_excretion_by_id_returns_correct_data()
    {
        var client = await AuthenticatedClient();
        var create = await client.PostAsJsonAsync("api/excretion", SampleExcretion());
        var id = ExtractIdFromLocation(create);

        var response = await client.GetAsync($"api/excretion/{id}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ExcretionDetailResponse>();
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task Update_excretion_returns_200()
    {
        var client = await AuthenticatedClient();
        var create = await client.PostAsJsonAsync("api/excretion", SampleExcretion());
        var id = ExtractIdFromLocation(create);

        var response = await client.PutAsJsonAsync($"api/excretion/{id}", new ExcretionUpdateRequest
        {
            ExcretionDateTime = DateTime.UtcNow,
            ExcretionLevel = 5,
            ExcretionColor = "#808080",
            Consistency = "Hard",
            Note = "Updated"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Delete_excretion_returns_200()
    {
        var client = await AuthenticatedClient();
        var create = await client.PostAsJsonAsync("api/excretion", SampleExcretion());
        var id = ExtractIdFromLocation(create);

        var response = await client.DeleteAsync($"api/excretion/{id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private static Guid ExtractIdFromLocation(HttpResponseMessage response)
    {
        var location = response.Headers.Location!.ToString();
        return Guid.Parse(location.Split('/').Last());
    }
}
