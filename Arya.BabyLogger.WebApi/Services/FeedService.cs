
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
            Note = entry.Notes,
            Amount = entry.Amount,
            Unit = entry.Unit.ToString(),
            Type = entry.Type.ToString()
        };

        await dbContext.Feeds.AddAsync(feedEntity);
        await dbContext.SaveChangesAsync();

        return feedEntity.Id;
    }

    public Task<ListFeedResponse> GetAllFeedEntriesAsync()
    {
        var feedEntities = dbContext.Feeds
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
}