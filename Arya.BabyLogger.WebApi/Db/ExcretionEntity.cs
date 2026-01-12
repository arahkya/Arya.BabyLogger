namespace Arya.BabyLogger.WebApi.Db;

public class ExcretionEntity
{
    public Guid Id { get; set; }
    public DateTime ExcretionDateTime { get; set; }
    public int ExcretionLevel { get; set; } = 1;
    public string ExcretionColor { get; set; } = "#FFFF00";
    public required string Consistency { get; set; }
    public string? Note { get; set; }
}
