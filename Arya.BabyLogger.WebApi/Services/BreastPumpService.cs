using Arya.BabyLogger.Shared.BreastPump;
using Arya.BabyLogger.WebApi.Db;
using Microsoft.EntityFrameworkCore;

namespace Arya.BabyLogger.WebApi.Services;

public class BreastPumpService(BabyLoggerDbContext dbContext) : IBreastPumpService
{
    public async Task<Guid> CreateAsync(BreastPumpCreateRequest request)
    {
        var breastPumpEntity = new BreastPumpEntity
        {
            Id = Guid.NewGuid(),
            PumpTime = request.PumpTime.UtcDateTime,
            AmountML = request.AmountML,
            Note = request.Note
        };

        await dbContext.BreastPumps.AddAsync(breastPumpEntity);
        await dbContext.SaveChangesAsync();

        return breastPumpEntity.Id;
    }

    public async Task<BreastPumpListItemsResponse> ListAsync(DateTime startDateUtc, DateTime endDateUtc, Guid userId = default)
    {
        var userEntity = await dbContext.Users.SingleAsync(p => p.Id == userId);
        
        var items = await dbContext.BreastPumps
            .Where(p => 
                p.PumpTime >= startDateUtc && p.PumpTime <= endDateUtc &&
                p.CareHouseholdId == userEntity.CareHouseholdId)
            .OrderByDescending(p => p.PumpTime)
            .Select(p => new BreastPumpListItemsResponse.BreastPumpListItem
            {
                Id = p.Id,
                PumpTime = new DateTimeOffset(p.PumpTime, TimeSpan.Zero),
                AmountML = p.AmountML,
            })
            .ToListAsync();

        return new BreastPumpListItemsResponse
        {
            Items = items
        };
    }

    public async Task<BreastPumpDetailResponse?> GetAsync(Guid id)
    {
        var breastPump = await dbContext.BreastPumps.FindAsync(id);

        if (breastPump is null)
        {
            return null;
        }

        return new BreastPumpDetailResponse
        {
            Id = breastPump.Id,
            PumpTime = new DateTimeOffset(breastPump.PumpTime, TimeSpan.Zero),
            AmountML = breastPump.AmountML,
            Note = breastPump.Note
        };
    }

    public async Task UpdateAsync(Guid id, BreastPumpUpdateRequest request)
    {
        var breastPump = await dbContext.BreastPumps.FindAsync(id);

        if (breastPump is null)
        {
            return;
        }

        breastPump.PumpTime = request.PumpTime.UtcDateTime;
        breastPump.AmountML = request.AmountML;
        breastPump.Note = request.Note;

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var breastPump = await dbContext.BreastPumps.FindAsync(id);

        if (breastPump is null)
        {
            return;
        }

        dbContext.BreastPumps.Remove(breastPump);
        await dbContext.SaveChangesAsync();
    }
}
