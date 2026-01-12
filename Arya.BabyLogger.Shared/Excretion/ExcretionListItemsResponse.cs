namespace Arya.BabyLogger.Shared.Excretion;

public class ExcretionListItemsResponse
{
    public List<ExcretionListItem> Items { get; set; } = new();

    public class ExcretionListItem
    {
        public Guid Id { get; set; }
        public DateTime ExcretionDateTime { get; set; }
        public int ExcretionLevel { get; set; }
        public string ExcretionColor { get; set; } = string.Empty;
        public string Consistency { get; set; } = string.Empty;
    }
}
