using Arya.BabyLogger.WebApi.Db;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arya.BabyLogger.WebApi.Tests;

public class BabyLoggerWebApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string TestConnectionString =
        "Server=localhost,1433;Database=BabyLoggerDb_Test;User Id=SA;Password=vkiydKN8986;TrustServerCertificate=True;";

    public BabyLoggerWebApiFactory()
    {
        Environment.SetEnvironmentVariable("UserHashingPasswordKey", "test-hashing-key-for-integration-tests");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:BabyLoggerDb"] = TestConnectionString,
                ["Jwt:SigningKey"] = "test-signing-key-must-be-at-least-32-characters!!",
                ["Jwt:Issuer"]    = "BabyLoggerTest",
                ["Jwt:Audience"]  = "BabyLoggerTest",
            });
        });
    }

    public async Task InitializeAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BabyLoggerDbContext>();
        await db.Database.EnsureDeletedAsync();
        await db.Database.MigrateAsync();
    }

    public new async Task DisposeAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BabyLoggerDbContext>();
        await db.Database.EnsureDeletedAsync();
        await base.DisposeAsync();
    }
}

[CollectionDefinition("WebApi")]
public class WebApiCollection : ICollectionFixture<BabyLoggerWebApiFactory> { }
