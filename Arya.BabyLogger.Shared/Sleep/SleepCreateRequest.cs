namespace Arya.BabyLogger.Shared.Sleep;

public class SleepCreateRequest
{
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public string? Note { get; set; }
}
