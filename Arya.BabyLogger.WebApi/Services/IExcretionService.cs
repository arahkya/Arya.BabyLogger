using Arya.BabyLogger.Shared.Excretion;

namespace Arya.BabyLogger.WebApi.Services;

public interface IExcretionService
{
    Task<Guid> CreateAsync(ExcretionCreateRequest request);
    Task<ExcretionListItemsResponse> ListAsync(DateTime startDate, DateTime endDate);
    Task<ExcretionDetailResponse?> GetAsync(Guid id);
    Task UpdateAsync(Guid id, ExcretionUpdateRequest request);
    Task DeleteAsync(Guid id);
}
