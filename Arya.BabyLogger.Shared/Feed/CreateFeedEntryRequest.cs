namespace Arya.BabyLogger.Shared.Feed;

public class CreateFeedEntryRequest
{
    public enum FeedTypes
    {
        BreastMilk,
        FormulaMilk
    }

    public enum Units
    {
        Ounces,
        Milliliters
    }

    public DateTimeOffset Time { get; set; }
    public string? Notes { get; set; }
    public double Amount { get; set; }
    public required string Unit { get; set; }
    public required string Type { get; set; }
}
