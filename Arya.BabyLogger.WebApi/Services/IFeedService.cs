using Arya.BabyLogger.Shared.Feed;

namespace Arya.BabyLogger.WebApi.Services;

public interface IFeedService
{
    Task<Guid> CreateFeedEntryAsync(CreateFeedEntryRequest entry);
    Task<ListFeedResponse> GetAllFeedEntriesAsync();
}