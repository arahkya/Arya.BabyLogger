namespace Arya.BabyLogger.Shared.Feed;

public class UpdateFeedEntryRequest
{
    public DateTimeOffset Time { get; set; }
    public string? Note { get; set; }
    public double Amount { get; set; }
    public required string Unit { get; set; }
    public required string Type { get; set; }
}