using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using Arya.BabyLogger.Shared.User;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Arya.BabyLogger.WebApi.Tests.Tests;

/// <summary>
/// Verifies that real production users can log in after the SQLite→SQL Server migration.
///
/// Required environment variables before running:
///   PROD_LOGIN_EMAIL     — email of a real production account
///   PROD_LOGIN_PASSWORD  — plain-text password for that account
///   PROD_HASHING_KEY     — value of UserHashingPasswordKey used in production
///                          (the secret used to HMAC-SHA256 the passwords)
///
/// Run with:
///   PROD_LOGIN_EMAIL=you@example.com \
///   PROD_LOGIN_PASSWORD=yourpassword \
///   PROD_HASHING_KEY=yourkey \
///   dotnet test --filter "FullyQualifiedName~ProductionLogin"
/// </summary>
public class ProductionLoginTests
{
    private const string ProdConnectionString =
        "Server=localhost,1433;Database=BabyLoggerDb;User Id=SA;Password=vkiydKN8986;TrustServerCertificate=True;";

    // ── Test 1: hash-level check (no HTTP, no factory) ───────────────────────
    // This is the most direct proof that the password was migrated correctly
    // and that PROD_HASHING_KEY is the right key.

    [Fact]
    public async Task Production_user_password_hash_is_valid_in_sqlserver()
    {
        var email      = Environment.GetEnvironmentVariable("PROD_LOGIN_EMAIL");
        var password   = Environment.GetEnvironmentVariable("PROD_LOGIN_PASSWORD");
        var hashingKey = Environment.GetEnvironmentVariable("PROD_HASHING_KEY");

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(hashingKey))
            return; // opt-in: set PROD_LOGIN_EMAIL, PROD_LOGIN_PASSWORD, PROD_HASHING_KEY to run

        // Compute expected hash using the same algorithm as UserService.HashedPassword
        var expectedHash = ComputeHmacSha256Hex(password, hashingKey);

        await using var conn = new SqlConnection(ProdConnectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT PasswordHash FROM Users WHERE Email = @email";
        cmd.Parameters.AddWithValue("@email", email);
        var storedHash = (string?)await cmd.ExecuteScalarAsync();

        Assert.NotNull(storedHash);                 // user exists in SQL Server
        Assert.Equal(expectedHash, storedHash);     // hash matches → login will succeed
    }

    // ── Test 2: full API login (end-to-end through WebApi) ───────────────────
    // Proves the complete stack works: routing → UserService → JWT generation.

    [Fact]
    public async Task Production_user_can_login_via_api_and_receives_jwt()
    {
        var email      = Environment.GetEnvironmentVariable("PROD_LOGIN_EMAIL");
        var password   = Environment.GetEnvironmentVariable("PROD_LOGIN_PASSWORD");
        var hashingKey = Environment.GetEnvironmentVariable("PROD_HASHING_KEY");

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(hashingKey))
            return; // opt-in: set PROD_LOGIN_EMAIL, PROD_LOGIN_PASSWORD, PROD_HASHING_KEY to run

        // Temporarily set the real hashing key so UserService.HashedPassword
        // computes the correct hash against production data.
        // (BabyLoggerWebApiFactory sets it to a test value — we override it here.)
        var previous = Environment.GetEnvironmentVariable("UserHashingPasswordKey");
        Environment.SetEnvironmentVariable("UserHashingPasswordKey", hashingKey);
        try
        {
            using var factory = new ProductionWebApiFactory();
            var client = factory.CreateClient();

            var response = await client.PostAsJsonAsync("api/user/login", new LoginRequest
            {
                Email = email,
                Password = password
            });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result.Token),
                "Login succeeded but JWT token is empty.");
        }
        finally
        {
            // Restore the previous value so other tests are not affected.
            Environment.SetEnvironmentVariable("UserHashingPasswordKey", previous);
        }
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static string ComputeHmacSha256Hex(string password, string key)
    {
        var keyBytes      = Encoding.UTF8.GetBytes(key);
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        using var hmac    = new HMACSHA256(keyBytes);
        return Convert.ToHexString(hmac.ComputeHash(passwordBytes)).ToLowerInvariant();
    }
}

/// <summary>
/// Factory that points to the real production database (BabyLoggerDb).
/// Does NOT drop or recreate the database.
/// Uses a fixed JWT key only to satisfy startup — production token validation
/// is not tested here (we only test that login returns a token).
/// </summary>
file class ProductionWebApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:BabyLoggerDb"] = "Server=localhost,1433;Database=BabyLoggerDb;User Id=SA;Password=vkiydKN8986;TrustServerCertificate=True;",
                ["Jwt:SigningKey"] = "prod-login-test-key-must-be-at-least-32-chars!",
                ["Jwt:Issuer"]    = "BabyLoggerTest",
                ["Jwt:Audience"]  = "BabyLoggerTest",
            });
        });
    }
}
