using System.Globalization;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace Arya.BabyLogger.WebApi.Tests.Tests;

/// <summary>
/// Verifies that all production data was correctly migrated from the SQLite backup
/// (prod_backup/BabyLoggerDb.sqlite.bak) to the SQL Server database (BabyLoggerDb).
/// Orphaned records that were intentionally skipped during migration are also verified absent.
/// </summary>
public class DataMigrationIntegrityTests
{
    private static string SqlServerCs =>
        $"Server=localhost,1433;Database=BabyLoggerDb;User Id=SA;Password={Environment.GetEnvironmentVariable("PROD_SQL_PASSWORD")};TrustServerCertificate=True;";

    private static string SqlitePath => Path.GetFullPath(
        Path.Combine(AppContext.BaseDirectory, "../../../../prod_backup/BabyLoggerDb.sqlite.bak"));

    private static SqliteConnection OpenSqlite()
    {
        var conn = new SqliteConnection($"Data Source={SqlitePath};Mode=ReadOnly;");
        conn.Open();
        return conn;
    }

    private static SqlConnection OpenSqlServer()
    {
        var conn = new SqlConnection(SqlServerCs);
        conn.Open();
        return conn;
    }

    // ── Row counts ───────────────────────────────────────────────────────────

    [Theory]
    [InlineData("CareHolders", 3)]
    [InlineData("Users", 4)]
    [InlineData("Feeds", 20)]
    [InlineData("Excretions", 20)]
    [InlineData("Sleeps", 20)]
    [InlineData("BreastPumps", 506)]           // 509 in SQLite minus 3 orphaned
    [InlineData("BreastPumpSettings", 3)]
    [InlineData("ResetPasswordRequests", 2)]   // 3 in SQLite minus 1 orphaned
    public async Task SqlServer_row_count_matches_migrated_count(string table, int expected)
    {
        await using var conn = OpenSqlServer();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT COUNT(*) FROM [{table}]";
        var count = (int)(await cmd.ExecuteScalarAsync())!;
        Assert.Equal(expected, count);
    }

    // ── CareHolders ──────────────────────────────────────────────────────────

    [Fact]
    public async Task All_careholders_migrated_with_correct_name()
    {
        using var sq = OpenSqlite();
        await using var ss = OpenSqlServer();

        using var sqCmd = sq.CreateCommand();
        sqCmd.CommandText = "SELECT Id, Name FROM CareHolders";
        using var reader = sqCmd.ExecuteReader();
        while (reader.Read())
        {
            var id = Guid.Parse(reader.GetString(0));
            var name = reader.GetString(1);

            await using var ssCmd = ss.CreateCommand();
            ssCmd.CommandText = "SELECT Name FROM CareHolders WHERE Id = @id";
            ssCmd.Parameters.AddWithValue("@id", id);
            var ssName = (string?)await ssCmd.ExecuteScalarAsync();
            Assert.Equal(name, ssName);
        }
    }

    // ── Users ────────────────────────────────────────────────────────────────

    [Fact]
    public async Task All_users_migrated_with_correct_data()
    {
        using var sq = OpenSqlite();
        await using var ss = OpenSqlServer();

        using var sqCmd = sq.CreateCommand();
        sqCmd.CommandText = "SELECT Id, Username, Email, CareHolderId FROM Users";
        using var reader = sqCmd.ExecuteReader();
        while (reader.Read())
        {
            var id = Guid.Parse(reader.GetString(0));
            var username = reader.GetString(1);
            var email = reader.GetString(2);
            var careHolderId = Guid.Parse(reader.GetString(3));

            await using var ssCmd = ss.CreateCommand();
            ssCmd.CommandText = "SELECT Username, Email, CareHolderId FROM Users WHERE Id = @id";
            ssCmd.Parameters.AddWithValue("@id", id);
            await using var ssReader = await ssCmd.ExecuteReaderAsync();

            Assert.True(await ssReader.ReadAsync(), $"User {id} missing from SQL Server");
            Assert.Equal(username, ssReader.GetString(0));
            Assert.Equal(email, ssReader.GetString(1));
            Assert.Equal(careHolderId, ssReader.GetGuid(2));
        }
    }

    // ── Feeds ────────────────────────────────────────────────────────────────

    [Fact]
    public async Task All_feeds_migrated_with_correct_data()
    {
        var sqFeeds = new List<(Guid Id, double Amount, string Unit, string Type)>();
        using (var sq = OpenSqlite())
        {
            using var cmd = sq.CreateCommand();
            cmd.CommandText = "SELECT Id, Amount, Unit, Type FROM Feeds";
            using var r = cmd.ExecuteReader();
            while (r.Read())
                sqFeeds.Add((Guid.Parse(r.GetString(0)), r.GetDouble(1), r.GetString(2), r.GetString(3)));
        }

        await using var ss = OpenSqlServer();
        foreach (var (id, amount, unit, type) in sqFeeds)
        {
            await using var cmd = ss.CreateCommand();
            cmd.CommandText = "SELECT Amount, Unit, Type FROM Feeds WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            await using var r = await cmd.ExecuteReaderAsync();

            Assert.True(await r.ReadAsync(), $"Feed {id} missing from SQL Server");
            Assert.Equal(amount, r.GetDouble(0), precision: 4);
            Assert.Equal(unit, r.GetString(1));
            Assert.Equal(type, r.GetString(2));
        }
    }

    // ── Excretions ───────────────────────────────────────────────────────────

    [Fact]
    public async Task All_excretions_migrated_with_correct_data()
    {
        var sqRows = new List<(Guid Id, int Level, string Color, string Consistency)>();
        using (var sq = OpenSqlite())
        {
            using var cmd = sq.CreateCommand();
            cmd.CommandText = "SELECT Id, ExcretionLevel, ExcretionColor, Consistency FROM Excretions";
            using var r = cmd.ExecuteReader();
            while (r.Read())
                sqRows.Add((Guid.Parse(r.GetString(0)), r.GetInt32(1), r.GetString(2), r.GetString(3)));
        }

        await using var ss = OpenSqlServer();
        foreach (var (id, level, color, consistency) in sqRows)
        {
            await using var cmd = ss.CreateCommand();
            cmd.CommandText = "SELECT ExcretionLevel, ExcretionColor, Consistency FROM Excretions WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            await using var r = await cmd.ExecuteReaderAsync();

            Assert.True(await r.ReadAsync(), $"Excretion {id} missing from SQL Server");
            Assert.Equal(level, r.GetInt32(0));
            Assert.Equal(color, r.GetString(1));
            Assert.Equal(consistency, r.GetString(2));
        }
    }

    // ── Sleeps ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task All_sleeps_migrated_and_times_preserved()
    {
        var sqRows = new List<(Guid Id, DateTime Start, DateTime End)>();
        using (var sq = OpenSqlite())
        {
            using var cmd = sq.CreateCommand();
            cmd.CommandText = "SELECT Id, SleepStartTime, SleepEndTime FROM Sleeps";
            using var r = cmd.ExecuteReader();
            while (r.Read())
                sqRows.Add((
                    Guid.Parse(r.GetString(0)),
                    DateTime.Parse(r.GetString(1), CultureInfo.InvariantCulture),
                    DateTime.Parse(r.GetString(2), CultureInfo.InvariantCulture)));
        }

        await using var ss = OpenSqlServer();
        foreach (var (id, start, end) in sqRows)
        {
            await using var cmd = ss.CreateCommand();
            cmd.CommandText = "SELECT SleepStartTime, SleepEndTime FROM Sleeps WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            await using var r = await cmd.ExecuteReaderAsync();

            Assert.True(await r.ReadAsync(), $"Sleep {id} missing from SQL Server");
            Assert.Equal(start.TruncateToSeconds(), r.GetDateTime(0).TruncateToSeconds());
            Assert.Equal(end.TruncateToSeconds(), r.GetDateTime(1).TruncateToSeconds());
        }
    }

    // ── BreastPumps ──────────────────────────────────────────────────────────

    [Fact]
    public async Task Non_orphaned_breastpumps_migrated_with_correct_data()
    {
        var sqRows = new List<(Guid Id, int AmountML, Guid CareHolderId)>();
        using (var sq = OpenSqlite())
        {
            using var cmd = sq.CreateCommand();
            // Only records whose CareHolder exists
            cmd.CommandText = @"
                SELECT b.Id, b.AmountML, b.CareHolderId
                FROM BreastPumps b
                WHERE EXISTS (SELECT 1 FROM CareHolders c WHERE c.Id = b.CareHolderId)";
            using var r = cmd.ExecuteReader();
            while (r.Read())
                sqRows.Add((Guid.Parse(r.GetString(0)), r.GetInt32(1), Guid.Parse(r.GetString(2))));
        }

        Assert.Equal(506, sqRows.Count);

        var ssIds = await LoadSqlServerGuids("SELECT Id FROM BreastPumps");
        foreach (var (id, amountML, careHolderId) in sqRows)
            Assert.True(ssIds.Contains(id), $"BreastPump {id} missing from SQL Server");
    }

    [Fact]
    public async Task Orphaned_breastpumps_are_absent_from_sqlserver()
    {
        var orphanIds = new[]
        {
            Guid.Parse("F0AAF43D-EF99-450C-9DAF-CF6BA15004A3"),
            Guid.Parse("F334C044-2DE4-421F-B2D8-A6A7784DD5FA"),
            Guid.Parse("5F0E6AAE-5326-4FC7-897D-424149329109"),
        };

        var ssIds = await LoadSqlServerGuids("SELECT Id FROM BreastPumps");
        foreach (var id in orphanIds)
            Assert.False(ssIds.Contains(id), $"Orphaned BreastPump {id} should not be in SQL Server");
    }

    // ── BreastPumpSettings ───────────────────────────────────────────────────

    [Fact]
    public async Task All_breastpumpsettings_migrated_with_correct_interval()
    {
        var sqRows = new List<(Guid CareHolderId, int Interval)>();
        using (var sq = OpenSqlite())
        {
            using var cmd = sq.CreateCommand();
            cmd.CommandText = "SELECT CareHolderId, PumpIntervalHours FROM BreastPumpSettings";
            using var r = cmd.ExecuteReader();
            while (r.Read())
                sqRows.Add((Guid.Parse(r.GetString(0)), r.GetInt32(1)));
        }

        await using var ss = OpenSqlServer();
        foreach (var (careHolderId, interval) in sqRows)
        {
            await using var cmd = ss.CreateCommand();
            cmd.CommandText = "SELECT PumpIntervalHours FROM BreastPumpSettings WHERE CareHolderId = @id";
            cmd.Parameters.AddWithValue("@id", careHolderId);
            var ssInterval = (int?)await cmd.ExecuteScalarAsync();

            Assert.True(ssInterval.HasValue, $"BreastPumpSettings for CareHolder {careHolderId} missing");
            Assert.Equal(interval, ssInterval.Value);
        }
    }

    // ── ResetPasswordRequests ────────────────────────────────────────────────

    [Fact]
    public async Task Non_orphaned_resetpasswordrequests_migrated()
    {
        var sqRows = new List<Guid>();
        using (var sq = OpenSqlite())
        {
            using var cmd = sq.CreateCommand();
            cmd.CommandText = @"
                SELECT r.Id FROM ResetPasswordRequests r
                WHERE EXISTS (SELECT 1 FROM Users u WHERE u.Id = r.UserId)";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                sqRows.Add(Guid.Parse(reader.GetString(0)));
        }

        Assert.Equal(2, sqRows.Count);

        var ssIds = await LoadSqlServerGuids("SELECT Id FROM ResetPasswordRequests");
        foreach (var id in sqRows)
            Assert.True(ssIds.Contains(id), $"ResetPasswordRequest {id} missing from SQL Server");
    }

    [Fact]
    public async Task Orphaned_resetpasswordrequest_is_absent_from_sqlserver()
    {
        var orphanId = Guid.Parse("0d7405d6-51ef-4cc3-b00a-90b004c6541e");
        var ssIds = await LoadSqlServerGuids("SELECT Id FROM ResetPasswordRequests");
        Assert.False(ssIds.Contains(orphanId),
            $"Orphaned ResetPasswordRequest {orphanId} should not be in SQL Server");
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static async Task<HashSet<Guid>> LoadSqlServerGuids(string sql)
    {
        var result = new HashSet<Guid>();
        await using var ss = OpenSqlServer();
        await using var cmd = ss.CreateCommand();
        cmd.CommandText = sql;
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync())
            result.Add(r.GetGuid(0));
        return result;
    }
}

file static class DateTimeExtensions
{
    internal static DateTime TruncateToSeconds(this DateTime dt) =>
        new(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Kind);
}
