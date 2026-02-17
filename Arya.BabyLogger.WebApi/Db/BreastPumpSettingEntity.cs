namespace Arya.BabyLogger.WebApi.Db;

public class BreastPumpSettingEntity
{
    public Guid CareHolderId { get; set; }
    public CareHolderEntity? CareHolder { get; set; }
    
    public int PumpIntervalHours { get; set; }
}