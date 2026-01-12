using Arya.BabyLogger.Shared.Excretion;
using Arya.BabyLogger.WebApi.Db;
using Microsoft.EntityFrameworkCore;

namespace Arya.BabyLogger.WebApi.Services;

public class ExcretionService(BabyLoggerDbContext dbContext) : IExcretionService
{
    public async Task<Guid> CreateAsync(ExcretionCreateRequest request)
    {
        var excretionEntity = new ExcretionEntity
        {
            Id = Guid.NewGuid(),
            ExcretionDateTime = request.ExcretionDateTime,
            ExcretionLevel = request.ExcretionLevel,
            ExcretionColor = request.ExcretionColor,
            Consistency = request.Consistency,
            Note = request.Note
        };

        await dbContext.Excretions.AddAsync(excretionEntity);
        await dbContext.SaveChangesAsync();

        return excretionEntity.Id;
    }

    public async Task<ExcretionListItemsResponse> ListAsync(DateTime startDate, DateTime endDate)
    {
        var excretions = await dbContext.Excretions
            .Where(e => e.ExcretionDateTime >= startDate && e.ExcretionDateTime <= endDate)
            .OrderByDescending(e => e.ExcretionDateTime)
            .Select(e => new ExcretionListItemsResponse.ExcretionListItem
            {
                Id = e.Id,
                ExcretionDateTime = e.ExcretionDateTime,
                ExcretionLevel = e.ExcretionLevel,
                ExcretionColor = e.ExcretionColor,
                Consistency = e.Consistency
            })
            .ToListAsync();

        return new ExcretionListItemsResponse
        {
            Items = excretions
        };
    }

    public async Task<ExcretionDetailResponse?> GetAsync(Guid id)
    {
        var excretion = await dbContext.Excretions.FindAsync(id);

        if (excretion is null)
        {
            return null;
        }

        return new ExcretionDetailResponse
        {
            Id = excretion.Id,
            ExcretionDateTime = excretion.ExcretionDateTime,
            ExcretionLevel = excretion.ExcretionLevel,
            ExcretionColor = excretion.ExcretionColor,
            Consistency = excretion.Consistency,
            Note = excretion.Note
        };
    }

    public async Task UpdateAsync(Guid id, ExcretionUpdateRequest request)
    {
        var excretion = await dbContext.Excretions.FindAsync(id);

        if (excretion is null)
        {
            return;
        }

        excretion.ExcretionDateTime = request.ExcretionDateTime;
        excretion.ExcretionLevel = request.ExcretionLevel;
        excretion.ExcretionColor = request.ExcretionColor;
        excretion.Consistency = request.Consistency;
        excretion.Note = request.Note;

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var excretion = await dbContext.Excretions.FindAsync(id);

        if (excretion is null)
        {
            return;
        }

        dbContext.Excretions.Remove(excretion);
        await dbContext.SaveChangesAsync();
    }
}
