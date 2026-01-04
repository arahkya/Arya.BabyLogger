namespace Arya.BabyLogger.Shared.Feed;

public class ListFeedResponse
{
    public class FeedItem
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }

    public List<FeedItem> Items { get; set; } = [];
}