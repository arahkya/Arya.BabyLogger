namespace Arya.BabyLogger.Shared.BreastPump;

public class BreastPumpDetailResponse
{
    public Guid Id { get; set; }
    public DateTimeOffset PumpTime { get; set; }
    public int AmountML { get; set; }
    public string? Note { get; set; }
}
