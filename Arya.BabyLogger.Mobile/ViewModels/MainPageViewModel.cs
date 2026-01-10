using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;
using Arya.BabyLogger.Shared.Feed;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arya.BabyLogger.Mobile.ViewModels;

public partial class MainPageViewModel(HttpClient httpClient) : ObservableObject
{
    public partial class BabyEvent(HttpClient httpClient) : ObservableObject
    {
        public required Guid Id { get; set; }
        public required string EventType { get; set; }
        public required string SubTitle { get; set; }
        public required string Title { get; set; }

        [RelayCommand]
        public async Task SelectEventAsync(Guid Id)
        {
            await Shell.Current.Navigation.PushModalAsync(new Views.FeedEntryView(new FeedEntryViewModel(httpClient)) { EventId = Id });
        }
    }

    private DateTime filterDate = DateTime.Now;
    public DateTime FilterDate
    {
        get => filterDate;
        set
        {
            SetProperty(ref filterDate, value);
            LoadEvents();
        }
    }

    public ObservableCollection<BabyEvent> BabyEvents { get; set; } = [];

    public async void LoadEvents()
    {
        var events = await ListFeedResponseAsync();

        BabyEvents.Clear();
        foreach (var babyEvent in events)
        {
            BabyEvents.Add(babyEvent);
        }
    }

    private async Task<List<BabyEvent>> ListFeedResponseAsync()
    {
        try
        {
            var selectFilterDate = FilterDate;
            var request = new HttpRequestMessage(HttpMethod.Get, "feed");
            var startDate = new DateTime(selectFilterDate.Year, selectFilterDate.Month, selectFilterDate.Day, 0, 0, 0);

            var endDate = new DateTime(selectFilterDate.Year, selectFilterDate.Month, selectFilterDate.Day, 23, 59, 59);
            request.Headers.Add("Start-Date", startDate.ToString("o"));
            request.Headers.Add("End-Date", endDate.ToString("o"));

            var listFeedResponse = await httpClient.SendAsync(request).Result.Content.ReadFromJsonAsync<ListFeedResponse>();
            var events = new List<BabyEvent>(listFeedResponse?.Items.Count ?? 0);

            foreach (var item in listFeedResponse?.Items ?? [])
            {
                events.Add(new BabyEvent(httpClient) { Id = item.Id, Title = item.Title, SubTitle = item.Time.ToString("d MMM yyyy HH:mm", new CultureInfo("th-TH")), EventType = item.Type });
            }

            return events;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching feed data: {ex.Message}");
            return [];
        }
    }
}
