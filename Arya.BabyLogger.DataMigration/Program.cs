// Migrates all data from a SQLite BabyLoggerDb to a SQL Server BabyLoggerDb.
// The SQL Server schema must already exist (run EF Core migrations first).
//
// Usage:
//   dotnet run -- \
//     --sqlite "/path/to/BabyLoggerDb.sqlite.bak" \
//     --sqlserver "Server=localhost,1433;Database=BabyLoggerDb;User Id=SA;Password=...;TrustServerCertificate=True;"

using System.Globalization;
using Microsoft.Data.Sqlite;
using Microsoft.Data.SqlClient;

var sqlitePath = GetArg(args, "--sqlite");
var sqlServerCs = GetArg(args, "--sqlserver");

if (string.IsNullOrWhiteSpace(sqlitePath) || string.IsNullOrWhiteSpace(sqlServerCs))
{
    Console.Error.WriteLine("Usage: --sqlite <path> --sqlserver <connection-string>");
    return 1;
}

if (!File.Exists(sqlitePath))
{
    Console.Error.WriteLine($"SQLite file not found: {sqlitePath}");
    return 1;
}

Console.WriteLine($"Source SQLite : {sqlitePath}");
Console.WriteLine("Target        : SQL Server (connection string provided)");
Console.WriteLine();

using var sqlite = new SqliteConnection($"Data Source={sqlitePath};Mode=ReadOnly;");
using var sqlServer = new SqlConnection(sqlServerCs);

sqlite.Open();
sqlServer.Open();

// FK-safe insertion order
MigrateCareHolders(sqlite, sqlServer);
MigrateUsers(sqlite, sqlServer);
MigrateFeeds(sqlite, sqlServer);
MigrateExcretions(sqlite, sqlServer);
MigrateSleeps(sqlite, sqlServer);
MigrateBreastPumps(sqlite, sqlServer);
MigrateResetPasswordRequests(sqlite, sqlServer);
MigrateBreastPumpSettings(sqlite, sqlServer);

Console.WriteLine();
Console.WriteLine("Data migration complete.");
return 0;

// ── helpers ──────────────────────────────────────────────────────────────────

static string? GetArg(string[] args, string name)
{
    var idx = Array.IndexOf(args, name);
    return idx >= 0 && idx + 1 < args.Length ? args[idx + 1] : null;
}

static Guid ParseGuid(SqliteDataReader r, int ordinal) =>
    Guid.Parse(r.GetString(ordinal));

static object NullOr(SqliteDataReader r, int ordinal) =>
    r.IsDBNull(ordinal) ? DBNull.Value : (object)r.GetString(ordinal);

// AddWithValue infers 'datetime' (min 1753). Use this to force 'datetime2' (min 0001).
// InvariantCulture is required — th-TH culture interprets year digits as Thai Buddhist Era,
// turning Gregorian 2026 into Gregorian 1483 (off by 543 years).
static SqlParameter Dt2(string name, string rawValue)
{
    var p = new SqlParameter(name, System.Data.SqlDbType.DateTime2);
    p.Value = DateTime.Parse(rawValue, CultureInfo.InvariantCulture);
    return p;
}

static void MigrateCareHolders(SqliteConnection src, SqlConnection dst)
{
    Console.Write("Migrating CareHolders ... ");
    using var cmd = src.CreateCommand();
    cmd.CommandText = "SELECT Id, Name, InviteCode, InviteUserEmail FROM CareHolders";
    using var r = cmd.ExecuteReader();
    int n = 0;
    while (r.Read())
    {
        using var ins = dst.CreateCommand();
        ins.CommandText = """
            IF NOT EXISTS (SELECT 1 FROM CareHolders WHERE Id = @Id)
                INSERT INTO CareHolders (Id, Name, InviteCode, InviteUserEmail)
                VALUES (@Id, @Name, @InviteCode, @InviteUserEmail)
            """;
        ins.Parameters.AddWithValue("@Id", ParseGuid(r, 0));
        ins.Parameters.AddWithValue("@Name", r.GetString(1));
        ins.Parameters.AddWithValue("@InviteCode", NullOr(r, 2));
        ins.Parameters.AddWithValue("@InviteUserEmail", NullOr(r, 3));
        ins.ExecuteNonQuery();
        n++;
    }
    Console.WriteLine($"{n} rows.");
}

static void MigrateUsers(SqliteConnection src, SqlConnection dst)
{
    Console.Write("Migrating Users ... ");
    using var cmd = src.CreateCommand();
    cmd.CommandText = "SELECT Id, Username, Email, PasswordHash, CareHolderId FROM Users";
    using var r = cmd.ExecuteReader();
    int n = 0;
    while (r.Read())
    {
        using var ins = dst.CreateCommand();
        ins.CommandText = """
            IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @Id)
                INSERT INTO Users (Id, Username, Email, PasswordHash, CareHolderId)
                VALUES (@Id, @Username, @Email, @PasswordHash, @CareHolderId)
            """;
        ins.Parameters.AddWithValue("@Id", ParseGuid(r, 0));
        ins.Parameters.AddWithValue("@Username", r.GetString(1));
        ins.Parameters.AddWithValue("@Email", r.GetString(2));
        ins.Parameters.AddWithValue("@PasswordHash", r.GetString(3));
        ins.Parameters.AddWithValue("@CareHolderId", ParseGuid(r, 4));
        ins.ExecuteNonQuery();
        n++;
    }
    Console.WriteLine($"{n} rows.");
}

static void MigrateFeeds(SqliteConnection src, SqlConnection dst)
{
    Console.Write("Migrating Feeds ... ");
    using var cmd = src.CreateCommand();
    cmd.CommandText = "SELECT Id, Time, Note, Amount, Unit, Type FROM Feeds";
    using var r = cmd.ExecuteReader();
    int n = 0;
    while (r.Read())
    {
        using var ins = dst.CreateCommand();
        ins.CommandText = """
            IF NOT EXISTS (SELECT 1 FROM Feeds WHERE Id = @Id)
                INSERT INTO Feeds (Id, Time, Note, Amount, Unit, Type)
                VALUES (@Id, @Time, @Note, @Amount, @Unit, @Type)
            """;
        ins.Parameters.AddWithValue("@Id", ParseGuid(r, 0));
        ins.Parameters.Add(Dt2("@Time", r.GetString(1)));
        ins.Parameters.AddWithValue("@Note", NullOr(r, 2));
        ins.Parameters.AddWithValue("@Amount", r.GetDouble(3));
        ins.Parameters.AddWithValue("@Unit", r.GetString(4));
        ins.Parameters.AddWithValue("@Type", r.GetString(5));
        ins.ExecuteNonQuery();
        n++;
    }
    Console.WriteLine($"{n} rows.");
}

static void MigrateExcretions(SqliteConnection src, SqlConnection dst)
{
    Console.Write("Migrating Excretions ... ");
    using var cmd = src.CreateCommand();
    cmd.CommandText = "SELECT Id, ExcretionDateTime, ExcretionLevel, ExcretionColor, Consistency, Note FROM Excretions";
    using var r = cmd.ExecuteReader();
    int n = 0;
    while (r.Read())
    {
        using var ins = dst.CreateCommand();
        ins.CommandText = """
            IF NOT EXISTS (SELECT 1 FROM Excretions WHERE Id = @Id)
                INSERT INTO Excretions (Id, ExcretionDateTime, ExcretionLevel, ExcretionColor, Consistency, Note)
                VALUES (@Id, @ExcretionDateTime, @ExcretionLevel, @ExcretionColor, @Consistency, @Note)
            """;
        ins.Parameters.AddWithValue("@Id", ParseGuid(r, 0));
        ins.Parameters.Add(Dt2("@ExcretionDateTime", r.GetString(1)));
        ins.Parameters.AddWithValue("@ExcretionLevel", r.GetInt32(2));
        ins.Parameters.AddWithValue("@ExcretionColor", r.GetString(3));
        ins.Parameters.AddWithValue("@Consistency", r.GetString(4));
        ins.Parameters.AddWithValue("@Note", NullOr(r, 5));
        ins.ExecuteNonQuery();
        n++;
    }
    Console.WriteLine($"{n} rows.");
}

static void MigrateSleeps(SqliteConnection src, SqlConnection dst)
{
    Console.Write("Migrating Sleeps ... ");
    using var cmd = src.CreateCommand();
    cmd.CommandText = "SELECT Id, SleepStartTime, SleepEndTime, Note FROM Sleeps";
    using var r = cmd.ExecuteReader();
    int n = 0;
    while (r.Read())
    {
        using var ins = dst.CreateCommand();
        ins.CommandText = """
            IF NOT EXISTS (SELECT 1 FROM Sleeps WHERE Id = @Id)
                INSERT INTO Sleeps (Id, SleepStartTime, SleepEndTime, Note)
                VALUES (@Id, @SleepStartTime, @SleepEndTime, @Note)
            """;
        ins.Parameters.AddWithValue("@Id", ParseGuid(r, 0));
        ins.Parameters.Add(Dt2("@SleepStartTime", r.GetString(1)));
        ins.Parameters.Add(Dt2("@SleepEndTime", r.GetString(2)));
        ins.Parameters.AddWithValue("@Note", NullOr(r, 3));
        ins.ExecuteNonQuery();
        n++;
    }
    Console.WriteLine($"{n} rows.");
}

static void MigrateBreastPumps(SqliteConnection src, SqlConnection dst)
{
    Console.Write("Migrating BreastPumps ... ");
    using var cmd = src.CreateCommand();
    cmd.CommandText = "SELECT Id, PumpTime, AmountML, Note, CareHolderId FROM BreastPumps";
    using var r = cmd.ExecuteReader();
    int n = 0, skipped = 0;
    while (r.Read())
    {
        var careHolderId = ParseGuid(r, 4);
        using var ins = dst.CreateCommand();
        ins.CommandText = """
            IF (NOT EXISTS (SELECT 1 FROM BreastPumps WHERE Id = @Id)
                AND EXISTS (SELECT 1 FROM CareHolders WHERE Id = @CareHolderId))
            BEGIN
                INSERT INTO BreastPumps (Id, PumpTime, AmountML, Note, CareHolderId)
                VALUES (@Id, @PumpTime, @AmountML, @Note, @CareHolderId)
            END
            """;
        ins.Parameters.AddWithValue("@Id", ParseGuid(r, 0));
        ins.Parameters.Add(Dt2("@PumpTime", r.GetString(1)));
        ins.Parameters.AddWithValue("@AmountML", r.GetInt32(2));
        ins.Parameters.AddWithValue("@Note", NullOr(r, 3));
        ins.Parameters.AddWithValue("@CareHolderId", careHolderId);
        var affected = ins.ExecuteNonQuery();
        if (affected > 0) n++; else skipped++;
    }
    Console.WriteLine($"{n} rows. {(skipped > 0 ? $"({skipped} orphaned CareHolderId — skipped)" : "")}");
}

static void MigrateResetPasswordRequests(SqliteConnection src, SqlConnection dst)
{
    Console.Write("Migrating ResetPasswordRequests ... ");
    using var cmd = src.CreateCommand();
    cmd.CommandText = "SELECT Id, UserId, SecretCode, CreatedAtUtc FROM ResetPasswordRequests";
    using var r = cmd.ExecuteReader();
    int n = 0, skipped = 0;
    while (r.Read())
    {
        using var ins = dst.CreateCommand();
        ins.CommandText = """
            IF (NOT EXISTS (SELECT 1 FROM ResetPasswordRequests WHERE Id = @Id)
                AND EXISTS (SELECT 1 FROM Users WHERE Id = @UserId))
            BEGIN
                INSERT INTO ResetPasswordRequests (Id, UserId, SecretCode, CreatedAtUtc)
                VALUES (@Id, @UserId, @SecretCode, @CreatedAtUtc)
            END
            """;
        ins.Parameters.AddWithValue("@Id", ParseGuid(r, 0));
        ins.Parameters.AddWithValue("@UserId", ParseGuid(r, 1));
        ins.Parameters.AddWithValue("@SecretCode", r.GetString(2));
        ins.Parameters.Add(Dt2("@CreatedAtUtc", r.GetString(3)));
        var affected = ins.ExecuteNonQuery();
        if (affected > 0) n++; else skipped++;
    }
    Console.WriteLine($"{n} rows. {(skipped > 0 ? $"({skipped} orphaned UserId — skipped)" : "")}");
}

static void MigrateBreastPumpSettings(SqliteConnection src, SqlConnection dst)
{
    Console.Write("Migrating BreastPumpSettings ... ");
    using var cmd = src.CreateCommand();
    cmd.CommandText = "SELECT CareHolderId, PumpIntervalHours FROM BreastPumpSettings";
    using var r = cmd.ExecuteReader();
    int n = 0;
    while (r.Read())
    {
        using var ins = dst.CreateCommand();
        ins.CommandText = """
            IF NOT EXISTS (SELECT 1 FROM BreastPumpSettings WHERE CareHolderId = @CareHolderId)
                INSERT INTO BreastPumpSettings (CareHolderId, PumpIntervalHours)
                VALUES (@CareHolderId, @PumpIntervalHours)
            """;
        ins.Parameters.AddWithValue("@CareHolderId", ParseGuid(r, 0));
        ins.Parameters.AddWithValue("@PumpIntervalHours", r.GetInt32(1));
        ins.ExecuteNonQuery();
        n++;
    }
    Console.WriteLine($"{n} rows.");
}
