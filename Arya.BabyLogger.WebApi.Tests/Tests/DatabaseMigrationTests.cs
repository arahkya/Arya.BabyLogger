using Arya.BabyLogger.WebApi.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Arya.BabyLogger.WebApi.Tests.Tests;

[Collection("WebApi")]
public class DatabaseMigrationTests(BabyLoggerWebApiFactory factory)
{
    [Fact]
    public void Can_connect_to_sql_server()
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BabyLoggerDbContext>();
        Assert.True(db.Database.CanConnect());
    }

    [Fact]
    public async Task InitialSqlServer_migration_is_applied()
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BabyLoggerDbContext>();
        var applied = await db.Database.GetAppliedMigrationsAsync();
        Assert.Contains(applied, m => m.Contains("InitialSqlServer"));
    }

    [Fact]
    public async Task All_tables_are_queryable()
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BabyLoggerDbContext>();

        // Each call verifies the table exists and EF Core can query it (no exception = pass)
        await db.Feeds.AnyAsync();
        await db.Excretions.AnyAsync();
        await db.Sleeps.AnyAsync();
        await db.BreastPumps.AnyAsync();
        await db.Users.AnyAsync();
        await db.CareHolders.AnyAsync();
        await db.ResetPasswordRequests.AnyAsync();
        await db.BreastPumpSettings.AnyAsync();
    }
}
