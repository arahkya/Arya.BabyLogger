using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace Arya.BabyLogger.WebApi.Db;

public class BabyLoggerDbContext(DbContextOptions<BabyLoggerDbContext> options) : DbContext(options)
{
    public DbSet<FeedEntity> Feeds { get; set; } = null!;
    public DbSet<ExcretionEntity> Excretions { get; set; } = null!;
    public DbSet<SleepEntity> Sleeps { get; set; } = null!;
    public DbSet<BreastPumpEntity> BreastPumps { get; set; } = null!;
    public DbSet<UserEntity> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Excretion data - 20 records covering 7 days back from January 12, 2026
        var excretionSeedData = GenerateExcretionSeedData();
        modelBuilder.Entity<ExcretionEntity>().HasData(excretionSeedData);

        modelBuilder.Entity<SleepEntity>().HasIndex(s => s.SleepStartTime);
        modelBuilder.Entity<SleepEntity>().HasIndex(s => s.SleepEndTime);

        // Seed Sleep data - 20 records covering 7 days back from January 12, 2026
        var sleepSeedData = GenerateSleepSeedData();
        modelBuilder.Entity<SleepEntity>().HasData(sleepSeedData);

        modelBuilder.Entity<BreastPumpEntity>().HasIndex(p => p.PumpTime);

        // Seed Breast Pump data - 20 records covering 7 days back from January 12, 2026
        var breastPumpSeedData = GenerateBreastPumpSeedData();
        modelBuilder.Entity<BreastPumpEntity>().HasData(breastPumpSeedData);
        
        var userSeedData = GenerateUserSeedData();
        modelBuilder.Entity<UserEntity>().HasData(userSeedData);
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

    private static List<SleepEntity> GenerateSleepSeedData()
    {
        var baseDate = new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc);

        var sleepIds = new[]
        {
            Guid.Parse("22222222-2222-2222-2222-222222222201"),
            Guid.Parse("22222222-2222-2222-2222-222222222202"),
            Guid.Parse("22222222-2222-2222-2222-222222222203"),
            Guid.Parse("22222222-2222-2222-2222-222222222204"),
            Guid.Parse("22222222-2222-2222-2222-222222222205"),
            Guid.Parse("22222222-2222-2222-2222-222222222206"),
            Guid.Parse("22222222-2222-2222-2222-222222222207"),
            Guid.Parse("22222222-2222-2222-2222-222222222208"),
            Guid.Parse("22222222-2222-2222-2222-222222222209"),
            Guid.Parse("22222222-2222-2222-2222-222222222210"),
            Guid.Parse("22222222-2222-2222-2222-222222222211"),
            Guid.Parse("22222222-2222-2222-2222-222222222212"),
            Guid.Parse("22222222-2222-2222-2222-222222222213"),
            Guid.Parse("22222222-2222-2222-2222-222222222214"),
            Guid.Parse("22222222-2222-2222-2222-222222222215"),
            Guid.Parse("22222222-2222-2222-2222-222222222216"),
            Guid.Parse("22222222-2222-2222-2222-222222222217"),
            Guid.Parse("22222222-2222-2222-2222-222222222218"),
            Guid.Parse("22222222-2222-2222-2222-222222222219"),
            Guid.Parse("22222222-2222-2222-2222-222222222220")
        };

        return new List<SleepEntity>
        {
            new() { Id = sleepIds[0], SleepStartTime = baseDate.AddHours(0), SleepEndTime = baseDate.AddHours(2), Note = "Overnight sleep" },
            new() { Id = sleepIds[1], SleepStartTime = baseDate.AddHours(3), SleepEndTime = baseDate.AddHours(4).AddMinutes(30), Note = "Night wake" },
            new() { Id = sleepIds[2], SleepStartTime = baseDate.AddHours(6), SleepEndTime = baseDate.AddHours(7), Note = "Morning nap" },
            new() { Id = sleepIds[3], SleepStartTime = baseDate.AddDays(-1).AddHours(0), SleepEndTime = baseDate.AddDays(-1).AddHours(2).AddMinutes(15), Note = "Overnight sleep" },
            new() { Id = sleepIds[4], SleepStartTime = baseDate.AddDays(-1).AddHours(4), SleepEndTime = baseDate.AddDays(-1).AddHours(5), Note = "Early morning nap" },
            new() { Id = sleepIds[5], SleepStartTime = baseDate.AddDays(-1).AddHours(7), SleepEndTime = baseDate.AddDays(-1).AddHours(8).AddMinutes(20), Note = "Morning nap" },
            new() { Id = sleepIds[6], SleepStartTime = baseDate.AddDays(-2).AddHours(0), SleepEndTime = baseDate.AddDays(-2).AddHours(2).AddMinutes(10), Note = "Overnight sleep" },
            new() { Id = sleepIds[7], SleepStartTime = baseDate.AddDays(-2).AddHours(3), SleepEndTime = baseDate.AddDays(-2).AddHours(4), Note = "Night wake" },
            new() { Id = sleepIds[8], SleepStartTime = baseDate.AddDays(-2).AddHours(6), SleepEndTime = baseDate.AddDays(-2).AddHours(7).AddMinutes(15), Note = "Morning nap" },
            new() { Id = sleepIds[9], SleepStartTime = baseDate.AddDays(-3).AddHours(0), SleepEndTime = baseDate.AddDays(-3).AddHours(1).AddMinutes(50), Note = "Overnight sleep" },
            new() { Id = sleepIds[10], SleepStartTime = baseDate.AddDays(-3).AddHours(3), SleepEndTime = baseDate.AddDays(-3).AddHours(4).AddMinutes(10), Note = "Night wake" },
            new() { Id = sleepIds[11], SleepStartTime = baseDate.AddDays(-3).AddHours(6), SleepEndTime = baseDate.AddDays(-3).AddHours(7), Note = "Morning nap" },
            new() { Id = sleepIds[12], SleepStartTime = baseDate.AddDays(-4).AddHours(0), SleepEndTime = baseDate.AddDays(-4).AddHours(2).AddMinutes(25), Note = "Overnight sleep" },
            new() { Id = sleepIds[13], SleepStartTime = baseDate.AddDays(-4).AddHours(3), SleepEndTime = baseDate.AddDays(-4).AddHours(4).AddMinutes(5), Note = "Night wake" },
            new() { Id = sleepIds[14], SleepStartTime = baseDate.AddDays(-4).AddHours(6), SleepEndTime = baseDate.AddDays(-4).AddHours(7).AddMinutes(35), Note = "Morning nap" },
            new() { Id = sleepIds[15], SleepStartTime = baseDate.AddDays(-5).AddHours(0), SleepEndTime = baseDate.AddDays(-5).AddHours(2), Note = "Overnight sleep" },
            new() { Id = sleepIds[16], SleepStartTime = baseDate.AddDays(-5).AddHours(3), SleepEndTime = baseDate.AddDays(-5).AddHours(4).AddMinutes(20), Note = "Night wake" },
            new() { Id = sleepIds[17], SleepStartTime = baseDate.AddDays(-5).AddHours(6), SleepEndTime = baseDate.AddDays(-5).AddHours(7).AddMinutes(10), Note = "Morning nap" },
            new() { Id = sleepIds[18], SleepStartTime = baseDate.AddDays(-6).AddHours(0), SleepEndTime = baseDate.AddDays(-6).AddHours(2).AddMinutes(30), Note = "Overnight sleep" },
            new() { Id = sleepIds[19], SleepStartTime = baseDate.AddDays(-6).AddHours(3), SleepEndTime = baseDate.AddDays(-6).AddHours(4), Note = "Night wake" }
        };
    }

    private static List<BreastPumpEntity> GenerateBreastPumpSeedData()
    {
        var baseDate = new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc);

        var pumpIds = new[]
        {
            Guid.Parse("33333333-3333-3333-3333-333333333301"),
            Guid.Parse("33333333-3333-3333-3333-333333333302"),
            Guid.Parse("33333333-3333-3333-3333-333333333303"),
            Guid.Parse("33333333-3333-3333-3333-333333333304"),
            Guid.Parse("33333333-3333-3333-3333-333333333305"),
            Guid.Parse("33333333-3333-3333-3333-333333333306"),
            Guid.Parse("33333333-3333-3333-3333-333333333307"),
            Guid.Parse("33333333-3333-3333-3333-333333333308"),
            Guid.Parse("33333333-3333-3333-3333-333333333309"),
            Guid.Parse("33333333-3333-3333-3333-333333333310"),
            Guid.Parse("33333333-3333-3333-3333-333333333311"),
            Guid.Parse("33333333-3333-3333-3333-333333333312"),
            Guid.Parse("33333333-3333-3333-3333-333333333313"),
            Guid.Parse("33333333-3333-3333-3333-333333333314"),
            Guid.Parse("33333333-3333-3333-3333-333333333315"),
            Guid.Parse("33333333-3333-3333-3333-333333333316"),
            Guid.Parse("33333333-3333-3333-3333-333333333317"),
            Guid.Parse("33333333-3333-3333-3333-333333333318"),
            Guid.Parse("33333333-3333-3333-3333-333333333319"),
            Guid.Parse("33333333-3333-3333-3333-333333333320")
        };

        return new List<BreastPumpEntity>
        {
            new() { Id = pumpIds[0], PumpTime = baseDate.AddHours(5).AddMinutes(15), AmountML = 120, Note = "Morning pump" },
            new() { Id = pumpIds[1], PumpTime = baseDate.AddHours(8).AddMinutes(45), AmountML = 90, Note = "Mid-morning" },
            new() { Id = pumpIds[2], PumpTime = baseDate.AddHours(12).AddMinutes(10), AmountML = 110, Note = "Noon session" },
            new() { Id = pumpIds[3], PumpTime = baseDate.AddHours(15).AddMinutes(30), AmountML = 100, Note = "Afternoon pump" },
            new() { Id = pumpIds[4], PumpTime = baseDate.AddHours(19).AddMinutes(5), AmountML = 130, Note = "Evening pump" },
            new() { Id = pumpIds[5], PumpTime = baseDate.AddDays(-1).AddHours(6), AmountML = 115, Note = "Morning pump" },
            new() { Id = pumpIds[6], PumpTime = baseDate.AddDays(-1).AddHours(9).AddMinutes(20), AmountML = 95, Note = "Mid-morning" },
            new() { Id = pumpIds[7], PumpTime = baseDate.AddDays(-1).AddHours(13), AmountML = 105, Note = "Noon session" },
            new() { Id = pumpIds[8], PumpTime = baseDate.AddDays(-1).AddHours(16).AddMinutes(10), AmountML = 100, Note = "Afternoon pump" },
            new() { Id = pumpIds[9], PumpTime = baseDate.AddDays(-1).AddHours(20).AddMinutes(5), AmountML = 125, Note = "Evening pump" },
            new() { Id = pumpIds[10], PumpTime = baseDate.AddDays(-2).AddHours(5).AddMinutes(30), AmountML = 118, Note = "Morning pump" },
            new() { Id = pumpIds[11], PumpTime = baseDate.AddDays(-2).AddHours(9), AmountML = 92, Note = "Mid-morning" },
            new() { Id = pumpIds[12], PumpTime = baseDate.AddDays(-2).AddHours(12).AddMinutes(40), AmountML = 108, Note = "Noon session" },
            new() { Id = pumpIds[13], PumpTime = baseDate.AddDays(-2).AddHours(15).AddMinutes(50), AmountML = 98, Note = "Afternoon pump" },
            new() { Id = pumpIds[14], PumpTime = baseDate.AddDays(-2).AddHours(19), AmountML = 132, Note = "Evening pump" },
            new() { Id = pumpIds[15], PumpTime = baseDate.AddDays(-3).AddHours(6).AddMinutes(10), AmountML = 112, Note = "Morning pump" },
            new() { Id = pumpIds[16], PumpTime = baseDate.AddDays(-3).AddHours(9).AddMinutes(35), AmountML = 88, Note = "Mid-morning" },
            new() { Id = pumpIds[17], PumpTime = baseDate.AddDays(-3).AddHours(13).AddMinutes(5), AmountML = 107, Note = "Noon session" },
            new() { Id = pumpIds[18], PumpTime = baseDate.AddDays(-3).AddHours(16).AddMinutes(25), AmountML = 97, Note = "Afternoon pump" },
            new() { Id = pumpIds[19], PumpTime = baseDate.AddDays(-3).AddHours(20).AddMinutes(15), AmountML = 128, Note = "Evening pump" }
        };
    }

    private static List<UserEntity> GenerateUserSeedData()
    {
        return [

            new() { Id = Guid.Parse("44444444-4444-4444-4444-444444444401"), Email = "arahk@outlook.com", Username = "Arahk8986", PasswordHash = "a2697f4143cbb043c514129a7bb96a53f48ac829a1413ad6aa09beb10b54f622" },
            new() { Id = Guid.Parse("44444444-4444-4444-4444-444444444402"), Email = "wiparat500267@gmail.com", Username = "wiparat500267", PasswordHash = "85dfffbb42725a20b1c6cc3c78073f39a19281230bc558e6e252f1f6a67973c8" }
        ];
    }
}
