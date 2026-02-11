namespace Arya.BabyLogger.WebApi.Db;

public class BreastPumpEntity
{
    public Guid Id { get; set; }
    public DateTime PumpTime { get; set; }
    public int AmountML { get; set; }
    public string? Note { get; set; }
    
    public Guid CareHouseholdId { get; init; }
    public CareHouseholdEntity? CareHousehold { get; init; }
}
