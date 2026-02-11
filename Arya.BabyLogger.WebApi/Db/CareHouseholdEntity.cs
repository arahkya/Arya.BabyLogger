namespace Arya.BabyLogger.WebApi.Db;

public class CareHouseholdEntity
{
    public Guid Id { get; init; }

    public List<UserEntity> Users { get; init; } = [];
}