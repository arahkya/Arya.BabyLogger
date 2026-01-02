namespace Arya.BabyLogger.Shared.Models;

public class FeedEntryModel
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
    public Units Unit { get; set; }
    public FeedTypes Type { get; set; }
}
