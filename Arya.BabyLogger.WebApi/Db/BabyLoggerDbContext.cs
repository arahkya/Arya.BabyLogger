using Microsoft.EntityFrameworkCore;

namespace Arya.BabyLogger.WebApi.Db;

public class BabyLoggerDbContext(DbContextOptions<BabyLoggerDbContext> options) : DbContext(options)
{
    public DbSet<FeedEntity> Feeds { get; set; } = null!;
    public DbSet<ExcretionEntity> Excretions { get; set; } = null!;
    public DbSet<SleepEntity> Sleeps { get; set; } = null!;
    public DbSet<BreastPumpEntity> BreastPumps { get; set; } = null!;
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<CareHolderEntity> CareHolders { get; set; } = null!;
    public DbSet<ResetPasswordRequestEntity> ResetPasswordRequests { get; set; } = null!;
    public DbSet<BreastPumpSettingEntity> BreastPumpSettings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SleepEntity>().HasIndex(s => s.SleepStartTime);
        modelBuilder.Entity<SleepEntity>().HasIndex(s => s.SleepEndTime);

        modelBuilder.Entity<BreastPumpEntity>().HasIndex(p => p.PumpTime);
        modelBuilder.Entity<BreastPumpEntity>().HasKey(p => p.Id);
        modelBuilder.Entity<BreastPumpEntity>().Property(p => p.CareHolderId).HasConversion<string>();
        modelBuilder.Entity<BreastPumpEntity>().HasOne(p => p.CareHolder).WithMany().HasForeignKey(p => p.CareHolderId).IsRequired();
        
        modelBuilder.Entity<UserEntity>().HasKey(p => p.Id);
        modelBuilder.Entity<UserEntity>().Property(p => p.CareHolderId).HasConversion<string>();
        modelBuilder.Entity<UserEntity>().HasOne(p => p.CareHolder).WithMany().HasForeignKey(p => p.CareHolderId).IsRequired();

        modelBuilder.Entity<ResetPasswordRequestEntity>().HasKey(p => p.Id);
        modelBuilder.Entity<ResetPasswordRequestEntity>().Property(p => p.Id).HasConversion<string>();
        modelBuilder.Entity<ResetPasswordRequestEntity>().Property(p => p.SecretCode).HasMaxLength(6).IsRequired();
        modelBuilder.Entity<ResetPasswordRequestEntity>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<BreastPumpSettingEntity>().ToTable("BreastPumpSettings");
        modelBuilder.Entity<BreastPumpSettingEntity>().HasKey(p => new { p.CareHolderId });
        modelBuilder.Entity<BreastPumpSettingEntity>().HasOne(p => p.CareHolder).WithMany().HasForeignKey(p => p.CareHolderId).IsRequired();
        
        modelBuilder.Entity<CareHolderEntity>().ToTable("CareHolders");
        modelBuilder.Entity<CareHolderEntity>().HasKey(p => p.Id);
        modelBuilder.Entity<CareHolderEntity>().Property(p => p.Id).HasConversion<string>();
        modelBuilder.Entity<CareHolderEntity>().Property(p => p.Name).HasMaxLength(50);
        modelBuilder.Entity<CareHolderEntity>().Property(p => p.InviteCode).HasMaxLength(10);
        modelBuilder.Entity<CareHolderEntity>().Property(p => p.InviteUserEmail).HasMaxLength(70);
    }

}
