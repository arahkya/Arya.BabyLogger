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
    public DbSet<CareHouseholdEntity> CareHouseholdEntities { get; set; } = null!;

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
        modelBuilder.Entity<BreastPumpEntity>().HasKey(p => p.Id);
        modelBuilder.Entity<BreastPumpEntity>().Property(p => p.CareHouseholdId).HasConversion<string>();
        modelBuilder.Entity<BreastPumpEntity>().HasOne(p => p.CareHousehold).WithMany().HasForeignKey(p => p.CareHouseholdId).IsRequired();
        
        modelBuilder.Entity<UserEntity>().HasKey(p => p.Id);
        modelBuilder.Entity<UserEntity>().HasOne(p => p.CareHousehold).WithMany(p => p.Users).HasForeignKey(p => p.CareHouseholdId).IsRequired();

        modelBuilder.Entity<CareHouseholdEntity>().ToTable("CareHouseholds");
        modelBuilder.Entity<CareHouseholdEntity>().HasKey(p => p.Id);
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
    
}
