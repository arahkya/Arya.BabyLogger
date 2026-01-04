using Microsoft.EntityFrameworkCore;

namespace Arya.BabyLogger.WebApi.Db;

public class BabyLoggerDbContext(DbContextOptions<BabyLoggerDbContext> options) : DbContext(options)
{
    public DbSet<FeedEntity> Feeds { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}