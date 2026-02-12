using Arya.BabyLogger.Shared.BreastPump;

namespace Arya.BabyLogger.WebApi.Services;

public interface IBreastPumpService
{
    Task<Guid> CreateAsync(BreastPumpCreateRequest request, Guid userId);
    Task<BreastPumpListItemsResponse> ListAsync(DateTime startDateUtc, DateTime endDateUtc, Guid userId);
    Task<BreastPumpDetailResponse?> GetAsync(Guid id);
    Task UpdateAsync(Guid id, BreastPumpUpdateRequest request);
    Task DeleteAsync(Guid id);
}
