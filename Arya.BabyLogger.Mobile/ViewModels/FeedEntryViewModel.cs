namespace Arya.BabyLogger.Mobile.ViewModels;

using Arya.BabyLogger.Shared.Models;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http.Json;

public partial class FeedEntryViewModel : ObservableObject
{
    private readonly FeedEntryModel feedEntry = new()
    {
        Time = DateTime.Now,
        Type = FeedEntryModel.FeedTypes.BreastMilk,
        Amount = 0,
        Unit = FeedEntryModel.Units.Milliliters,
        Notes = string.Empty
    };

    public string Notes
    {
        get => feedEntry.Notes ?? string.Empty;
        set
        {
            feedEntry.Notes = value;
            OnPropertyChanged();
        }
    }

    public DateTime Date
    {
        get => feedEntry.Time.Date == DateTime.MinValue
            ? DateTime.Now.Date
            : feedEntry.Time.Date;
        set
        {
            feedEntry.Time = new DateTimeOffset(value.Year, value.Month, value.Day, feedEntry.Time.Hour, feedEntry.Time.Minute, 0, feedEntry.Time.Offset);
            OnPropertyChanged();
        }
    }

    public TimeSpan Time
    {
        get => feedEntry.Time.Date == DateTime.MinValue ? DateTime.Now.TimeOfDay : feedEntry.Time.TimeOfDay;
        set
        {
            feedEntry.Time = new DateTimeOffset(feedEntry.Time.Year, feedEntry.Time.Month, feedEntry.Time.Day, value.Hours, value.Minutes, 0, feedEntry.Time.Offset);
            OnPropertyChanged();
        }
    }

    public double Amount
    {
        get => feedEntry.Amount;
        set
        {
            feedEntry.Amount = value;
            OnPropertyChanged();
        }
    }

    private string feedType;

    public string FeedType
    {
        get => feedType;
        set => SetProperty(ref feedType, value);
    }

    public FeedEntryViewModel()
    {
        feedType = "นมแม่";
    }

    [RelayCommand]
    public async Task SaveFeedingEntryAsync()
    {
        feedEntry.Unit = FeedEntryModel.Units.Milliliters;

        feedEntry.Type = FeedType switch
        {
            "นมแม่" => FeedEntryModel.FeedTypes.BreastMilk,
            "นมผง" => FeedEntryModel.FeedTypes.FormulaMilk,
            _ => FeedEntryModel.FeedTypes.BreastMilk,
        };

        const string UrlEndpoint = "http://localhost:5001/api/feed";

        var httpClient = new HttpClient();
        var response = await httpClient.PostAsJsonAsync(UrlEndpoint, feedEntry);

        response.EnsureSuccessStatusCode();

        await Shell.Current.Navigation.PopModalAsync();
    }

    [RelayCommand]
    public static async Task CancelFeedingEntryAsync()
    {
        await Shell.Current.Navigation.PopModalAsync();
    }
}
