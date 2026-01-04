namespace Arya.BabyLogger.WebApi.Db;

public class FeedEntity
{
    public Guid Id { get; set; }
    public DateTime Time { get; set; }
    public string? Note { get; set; }
    public double Amount { get; set; }
    public required string Unit { get; set; }
    public required string Type { get; set; }
}