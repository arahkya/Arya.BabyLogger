using Arya.BabyLogger.Shared.Feed;
using Arya.BabyLogger.WebApi.Db;

namespace Arya.BabyLogger.WebApi.Services;

public interface IFeedService
{
    Task<Guid> CreateFeedEntryAsync(CreateFeedEntryRequest entry);
    Task<ListFeedResponse> GetAllFeedEntriesAsync();
    Task<FeedEntity?> GetFeedEntryByIdAsync(Guid id);
    Task UpdateFeedEntryAsync(Guid id, UpdateFeedEntryRequest request);
    Task DeleteFeedEntryAsync(Guid id);
}
