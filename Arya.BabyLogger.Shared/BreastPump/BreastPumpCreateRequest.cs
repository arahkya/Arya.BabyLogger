namespace Arya.BabyLogger.Shared.BreastPump;

public class BreastPumpCreateRequest
{
    public DateTimeOffset PumpTime { get; set; }
    public int AmountML { get; set; }
    public string? Note { get; set; }
}
