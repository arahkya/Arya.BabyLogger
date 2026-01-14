using Arya.BabyLogger.Shared.BreastPump;

namespace Arya.BabyLogger.WebApi.Services;

public interface IBreastPumpService
{
    Task<Guid> CreateAsync(BreastPumpCreateRequest request);
    Task<BreastPumpListItemsResponse> ListAsync(DateTime startDateUtc, DateTime endDateUtc);
    Task<BreastPumpDetailResponse?> GetAsync(Guid id);
    Task UpdateAsync(Guid id, BreastPumpUpdateRequest request);
    Task DeleteAsync(Guid id);
}
