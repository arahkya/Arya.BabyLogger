using Arya.BabyLogger.Shared.Sleep;

namespace Arya.BabyLogger.WebApi.Services;

public interface ISleepService
{
    Task<Guid> CreateAsync(SleepCreateRequest request);
    Task<SleepListItemsResponse> ListAsync(DateTime startDateUtc, DateTime endDateUtc);
    Task<SleepDetailResponse?> GetAsync(Guid id);
    Task UpdateAsync(Guid id, SleepUpdateRequest request);
    Task DeleteAsync(Guid id);
}
