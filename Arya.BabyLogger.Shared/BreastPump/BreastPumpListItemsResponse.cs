namespace Arya.BabyLogger.Shared.BreastPump;

public class BreastPumpListItemsResponse
{
    public List<BreastPumpListItem> Items { get; set; } = new();

    public class BreastPumpListItem
    {
        public Guid Id { get; set; }
        public DateTimeOffset PumpTime { get; set; }
        public int AmountML { get; set; }
    }
}
