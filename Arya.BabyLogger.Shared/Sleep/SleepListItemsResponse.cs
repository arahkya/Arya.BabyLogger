namespace Arya.BabyLogger.Shared.Sleep;

public class SleepListItemsResponse
{
    public List<SleepListItem> Items { get; set; } = new();

    public class SleepListItem
    {
        public Guid Id { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public int DurationMinutes { get; set; }
    }
}
