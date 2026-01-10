
using System.Diagnostics;
using Arya.BabyLogger.Shared.Feed;
using Arya.BabyLogger.WebApi.Db;

namespace Arya.BabyLogger.WebApi.Services;

public class FeedService(BabyLoggerDbContext dbContext) : IFeedService
{
    private string GetListFeedItemTitle(Db.FeedEntity feedEntity)
    {
        return $"{feedEntity.Type switch { "BreastMilk" => "นมแม่", "FormulaMilk" => "นมผง", _ => "อื่นๆ" }} - {feedEntity.Amount} {feedEntity.Unit switch { "Ounces" => "ออนซ์", "Milliliters" => "มิลลิลิตร", _ => "" }}";
    }


    public async Task<Guid> CreateFeedEntryAsync(CreateFeedEntryRequest entry)
    {
        var feedEntity = new Db.FeedEntity
        {
            Id = Guid.NewGuid(),
            Time = new DateTime(entry.Time.Year, entry.Time.Month, entry.Time.Day, entry.Time.Hour, entry.Time.Minute, entry.Time.Second),
            Note = entry.Note,
            Amount = entry.Amount,
            Unit = entry.Unit.ToString(),
            Type = entry.Type.ToString()
        };

        await dbContext.Feeds.AddAsync(feedEntity);
        await dbContext.SaveChangesAsync();

        return feedEntity.Id;
    }

    public Task<ListFeedResponse> GetAllFeedEntriesAsync(DateTime startDate, DateTime endDate)
    {
        var feedEntities = dbContext.Feeds
            .Where(f => f.Time >= startDate && f.Time <= endDate)
            .OrderByDescending(f => f.Time)
            .ToList();

        var feedEntries = feedEntities.Select(f => new ListFeedResponse.FeedItem
        {
            Id = f.Id,
            Time = f.Time,
            Title = GetListFeedItemTitle(f),
            Type = f.Type
        }).ToList();

        var response = new ListFeedResponse
        {
            Items = feedEntries
        };

        return Task.FromResult(response);
    }

    public async Task<Db.FeedEntity?> GetFeedEntryByIdAsync(Guid id)
    {
        return await dbContext.Feeds.FindAsync(id);
    }

    public async Task DeleteFeedEntryAsync(Guid id)
    {
        var feedEntity = await dbContext.Feeds.FindAsync(id);
        if (feedEntity is null)
        {
            return;
        }

        dbContext.Feeds.Remove(feedEntity);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateFeedEntryAsync(Guid id, UpdateFeedEntryRequest request)
    {
        var feedEntity = await dbContext.Feeds.FindAsync(id);
        if (feedEntity is null)
        {
            return;
        }

        feedEntity.Time = new DateTime(request.Time.Year, request.Time.Month, request.Time.Day, request.Time.Hour, request.Time.Minute, request.Time.Second);
        feedEntity.Note = request.Note;
        feedEntity.Amount = request.Amount;
        feedEntity.Unit = request.Unit.ToString();
        feedEntity.Type = request.Type.ToString();

        await dbContext.SaveChangesAsync();
    }
}
