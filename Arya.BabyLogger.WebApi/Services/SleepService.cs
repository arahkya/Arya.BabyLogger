using Arya.BabyLogger.Shared.Sleep;
using Arya.BabyLogger.WebApi.Db;
using Microsoft.EntityFrameworkCore;

namespace Arya.BabyLogger.WebApi.Services;

public class SleepService(BabyLoggerDbContext dbContext) : ISleepService
{
    public async Task<Guid> CreateAsync(SleepCreateRequest request)
    {
        var sleepEntity = new SleepEntity
        {
            Id = Guid.NewGuid(),
            SleepStartTime = request.StartTime.UtcDateTime,
            SleepEndTime = request.EndTime.UtcDateTime,
            Note = request.Note
        };

        await dbContext.Sleeps.AddAsync(sleepEntity);
        await dbContext.SaveChangesAsync();

        return sleepEntity.Id;
    }

    public async Task<SleepListItemsResponse> ListAsync(DateTime startDateUtc, DateTime endDateUtc)
    {
        var sleeps = await dbContext.Sleeps
            .Where(s => s.SleepStartTime >= startDateUtc && s.SleepEndTime <= endDateUtc)
            .OrderByDescending(s => s.SleepStartTime)
            .Select(s => new SleepListItemsResponse.SleepListItem
            {
                Id = s.Id,
                StartTime = new DateTimeOffset(s.SleepStartTime, TimeSpan.Zero),
                EndTime = new DateTimeOffset(s.SleepEndTime, TimeSpan.Zero),
                DurationMinutes = (int)Math.Round((s.SleepEndTime - s.SleepStartTime).TotalMinutes)
            })
            .ToListAsync();

        return new SleepListItemsResponse
        {
            Items = sleeps
        };
    }

    public async Task<SleepDetailResponse?> GetAsync(Guid id)
    {
        var sleep = await dbContext.Sleeps.FindAsync(id);

        if (sleep is null)
        {
            return null;
        }

        return new SleepDetailResponse
        {
            Id = sleep.Id,
            StartTime = new DateTimeOffset(sleep.SleepStartTime, TimeSpan.Zero),
            EndTime = new DateTimeOffset(sleep.SleepEndTime, TimeSpan.Zero),
            DurationMinutes = (int)Math.Round((sleep.SleepEndTime - sleep.SleepStartTime).TotalMinutes),
            Note = sleep.Note
        };
    }

    public async Task UpdateAsync(Guid id, SleepUpdateRequest request)
    {
        var sleep = await dbContext.Sleeps.FindAsync(id);

        if (sleep is null)
        {
            return;
        }

        sleep.SleepStartTime = request.StartTime.UtcDateTime;
        sleep.SleepEndTime = request.EndTime.UtcDateTime;
        sleep.Note = request.Note;

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var sleep = await dbContext.Sleeps.FindAsync(id);

        if (sleep is null)
        {
            return;
        }

        dbContext.Sleeps.Remove(sleep);
        await dbContext.SaveChangesAsync();
    }
}
