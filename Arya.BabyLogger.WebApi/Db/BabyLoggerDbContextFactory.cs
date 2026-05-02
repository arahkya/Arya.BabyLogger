using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Arya.BabyLogger.WebApi.Db;

public class BabyLoggerDbContextFactory : IDesignTimeDbContextFactory<BabyLoggerDbContext>
{
    public BabyLoggerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BabyLoggerDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=BabyLoggerDb;User Id=SA;Password=Design_Time_Only;TrustServerCertificate=True;");
        return new BabyLoggerDbContext(optionsBuilder.Options);
    }
}
