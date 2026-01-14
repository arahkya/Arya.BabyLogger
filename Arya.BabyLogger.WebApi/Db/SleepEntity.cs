namespace Arya.BabyLogger.WebApi.Db;

public class SleepEntity
{
    public Guid Id { get; set; }
    public DateTime SleepStartTime { get; set; }
    public DateTime SleepEndTime { get; set; }
    public string? Note { get; set; }
}
