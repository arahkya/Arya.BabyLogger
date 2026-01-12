using Microsoft.EntityFrameworkCore;

namespace Arya.BabyLogger.WebApi.Db;

public class BabyLoggerDbContext(DbContextOptions<BabyLoggerDbContext> options) : DbContext(options)
{
    public DbSet<FeedEntity> Feeds { get; set; } = null!;
    public DbSet<ExcretionEntity> Excretions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Excretion data - 20 records covering 7 days back from January 12, 2026
        var excretionSeedData = GenerateExcretionSeedData();
        modelBuilder.Entity<ExcretionEntity>().HasData(excretionSeedData);
    }

    private static List<ExcretionEntity> GenerateExcretionSeedData()
    {
        var baseDate = new DateTime(2026, 1, 12); // January 12, 2026

        var excretionIds = new[]
        {
            Guid.Parse("11111111-1111-1111-1111-111111111101"),
            Guid.Parse("11111111-1111-1111-1111-111111111102"),
            Guid.Parse("11111111-1111-1111-1111-111111111103"),
            Guid.Parse("11111111-1111-1111-1111-111111111104"),
            Guid.Parse("11111111-1111-1111-1111-111111111105"),
            Guid.Parse("11111111-1111-1111-1111-111111111106"),
            Guid.Parse("11111111-1111-1111-1111-111111111107"),
            Guid.Parse("11111111-1111-1111-1111-111111111108"),
            Guid.Parse("11111111-1111-1111-1111-111111111109"),
            Guid.Parse("11111111-1111-1111-1111-111111111110"),
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Guid.Parse("11111111-1111-1111-1111-111111111112"),
            Guid.Parse("11111111-1111-1111-1111-111111111113"),
            Guid.Parse("11111111-1111-1111-1111-111111111114"),
            Guid.Parse("11111111-1111-1111-1111-111111111115"),
            Guid.Parse("11111111-1111-1111-1111-111111111116"),
            Guid.Parse("11111111-1111-1111-1111-111111111117"),
            Guid.Parse("11111111-1111-1111-1111-111111111118"),
            Guid.Parse("11111111-1111-1111-1111-111111111119"),
            Guid.Parse("11111111-1111-1111-1111-111111111120")
        };

        return new List<ExcretionEntity>
        {
            new() { Id = excretionIds[0], ExcretionDateTime = baseDate.AddDays(0).AddHours(6), ExcretionLevel = 1, ExcretionColor = "#FFFF00", Consistency = "Watery", Note = "Normal" },
            new() { Id = excretionIds[1], ExcretionDateTime = baseDate.AddDays(-1).AddHours(6).AddMinutes(20), ExcretionLevel = 2, ExcretionColor = "#8B7355", Consistency = "Soft", Note = "Soft stool" },
            new() { Id = excretionIds[2], ExcretionDateTime = baseDate.AddDays(-2).AddHours(6).AddMinutes(40), ExcretionLevel = 3, ExcretionColor = "#A9A9A9", Consistency = "Hard", Note = "Slightly hard" },
            new() { Id = excretionIds[3], ExcretionDateTime = baseDate.AddDays(-3).AddHours(6), ExcretionLevel = 4, ExcretionColor = "#DAA520", Consistency = "Paste", Note = "Watery" },
            new() { Id = excretionIds[4], ExcretionDateTime = baseDate.AddDays(-4).AddHours(6).AddMinutes(20), ExcretionLevel = 5, ExcretionColor = "#808080", Consistency = "Liquid", Note = "Normal consistency" },
            new() { Id = excretionIds[5], ExcretionDateTime = baseDate.AddDays(-5).AddHours(6).AddMinutes(40), ExcretionLevel = 6, ExcretionColor = "#FFFF00", Consistency = "Watery", Note = null },
            new() { Id = excretionIds[6], ExcretionDateTime = baseDate.AddDays(-6).AddHours(6), ExcretionLevel = 7, ExcretionColor = "#8B7355", Consistency = "Soft", Note = null },
            new() { Id = excretionIds[7], ExcretionDateTime = baseDate.AddDays(-0).AddHours(9).AddMinutes(20), ExcretionLevel = 8, ExcretionColor = "#A9A9A9", Consistency = "Hard", Note = "Changed diet" },
            new() { Id = excretionIds[8], ExcretionDateTime = baseDate.AddDays(-1).AddHours(9).AddMinutes(40), ExcretionLevel = 9, ExcretionColor = "#DAA520", Consistency = "Paste", Note = "Morning" },
            new() { Id = excretionIds[9], ExcretionDateTime = baseDate.AddDays(-2).AddHours(9), ExcretionLevel = 10, ExcretionColor = "#808080", Consistency = "Liquid", Note = "After feeding" },
            new() { Id = excretionIds[10], ExcretionDateTime = baseDate.AddDays(-3).AddHours(9).AddMinutes(20), ExcretionLevel = 1, ExcretionColor = "#FFFF00", Consistency = "Watery", Note = null },
            new() { Id = excretionIds[11], ExcretionDateTime = baseDate.AddDays(-4).AddHours(9).AddMinutes(40), ExcretionLevel = 2, ExcretionColor = "#8B7355", Consistency = "Soft", Note = null },
            new() { Id = excretionIds[12], ExcretionDateTime = baseDate.AddDays(-5).AddHours(9), ExcretionLevel = 3, ExcretionColor = "#A9A9A9", Consistency = "Hard", Note = "Healthy" },
            new() { Id = excretionIds[13], ExcretionDateTime = baseDate.AddDays(-6).AddHours(9).AddMinutes(20), ExcretionLevel = 4, ExcretionColor = "#DAA520", Consistency = "Paste", Note = "Monitor" },
            new() { Id = excretionIds[14], ExcretionDateTime = baseDate.AddDays(-0).AddHours(12).AddMinutes(40), ExcretionLevel = 5, ExcretionColor = "#808080", Consistency = "Liquid", Note = null },
            new() { Id = excretionIds[15], ExcretionDateTime = baseDate.AddDays(-1).AddHours(12), ExcretionLevel = 6, ExcretionColor = "#FFFF00", Consistency = "Watery", Note = "Normal" },
            new() { Id = excretionIds[16], ExcretionDateTime = baseDate.AddDays(-2).AddHours(12).AddMinutes(20), ExcretionLevel = 7, ExcretionColor = "#8B7355", Consistency = "Soft", Note = "Soft stool" },
            new() { Id = excretionIds[17], ExcretionDateTime = baseDate.AddDays(-3).AddHours(12).AddMinutes(40), ExcretionLevel = 8, ExcretionColor = "#A9A9A9", Consistency = "Hard", Note = "Slightly hard" },
            new() { Id = excretionIds[18], ExcretionDateTime = baseDate.AddDays(-4).AddHours(12), ExcretionLevel = 9, ExcretionColor = "#DAA520", Consistency = "Paste", Note = "Watery" },
            new() { Id = excretionIds[19], ExcretionDateTime = baseDate.AddDays(-5).AddHours(12).AddMinutes(20), ExcretionLevel = 10, ExcretionColor = "#808080", Consistency = "Liquid", Note = "Normal consistency" }
        };
    }
}