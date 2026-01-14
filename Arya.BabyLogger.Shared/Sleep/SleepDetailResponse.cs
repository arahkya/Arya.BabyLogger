namespace Arya.BabyLogger.Shared.Sleep;

public class SleepDetailResponse
{
    public Guid Id { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public int DurationMinutes { get; set; }
    public string? Note { get; set; }
}
