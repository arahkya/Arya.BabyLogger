namespace Arya.BabyLogger.Mobile.ViewModels;

using Arya.BabyLogger.Shared.Feed;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http.Json;

public partial class FeedEntryViewModel(HttpClient httpClient) : ObservableObject
{
    private readonly CreateFeedEntryRequest feedEntry = new()
    {
        Time = DateTime.Now,
        Type = FeedTypes.BreastMilk.ToString(),
        Amount = 0,
        Unit = FeedUnits.Milliliters.ToString(),
        Note = string.Empty
    };
    private Guid? feedEntryId;

    public string Note
    {
        get => feedEntry.Note ?? string.Empty;
        set
        {
            feedEntry.Note = value;
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

    private string feedType = "นมแม่";

    public string FeedType
    {
        get => feedType;
        set => SetProperty(ref feedType, value);
    }

    public bool ShowSaveButton { get; set; } = true;

    public bool ShowDeleteButton { get; set; } = false;

    [RelayCommand]
    public async Task SaveFeedingEntryAsync()
    {
        feedEntry.Unit = FeedUnits.Milliliters.ToString();

        feedEntry.Type = FeedType switch
        {
            "นมแม่" => FeedTypes.BreastMilk.ToString(),
            "นมผง" => FeedTypes.FormulaMilk.ToString(),
            _ => FeedTypes.BreastMilk.ToString(),
        };

        var response = await httpClient.PostAsJsonAsync("feed", feedEntry);

        response.EnsureSuccessStatusCode();

        await Shell.Current.Navigation.PopModalAsync();
    }

    [RelayCommand]
    public async Task UpdateFeedingEntryAsync()
    {
        if (!feedEntryId.HasValue)
        {
            return;
        }

        feedEntry.Unit = FeedUnits.Milliliters.ToString();

        feedEntry.Type = FeedType switch
        {
            "นมแม่" => FeedTypes.BreastMilk.ToString(),
            "นมผง" => FeedTypes.FormulaMilk.ToString(),
            _ => FeedTypes.BreastMilk.ToString(),
        };

        var response = await httpClient.PutAsJsonAsync($"feed/{feedEntryId.Value}", feedEntry);

        response.EnsureSuccessStatusCode();

        await Shell.Current.Navigation.PopModalAsync();
    }

    [RelayCommand]
    public static async Task CancelFeedingEntryAsync()
    {
        await Shell.Current.Navigation.PopModalAsync();
    }

    [RelayCommand]
    public async Task DeleteFeedingEntryAsync()
    {
        if (!feedEntryId.HasValue)
        {
            return;
        }

        var response = await httpClient.DeleteAsync($"feed/{feedEntryId.Value}");
        response.EnsureSuccessStatusCode();

        await Shell.Current.Navigation.PopModalAsync();
    }

    public async Task LoadFeedEventByIdAsync(Guid value)
    {
        var response = await httpClient.GetFromJsonAsync<CreateFeedEntryRequest>($"feed/{value}");
        if (response == null)
        {
            return;
        }
        feedEntryId = value;

        feedEntry.Time = response.Time;
        feedEntry.Note = response.Note ?? string.Empty;
        feedEntry.Amount = response.Amount;
        feedEntry.Unit = response.Unit;
        feedEntry.Type = response.Type;

        FeedType = response.Type switch
        {
            "BreastMilk" => "นมแม่",
            "FormulaMilk" => "นมผง",
            _ => "นมแม่"
        };

        ShowDeleteButton = true;
        ShowSaveButton = false;

        OnPropertyChanged(nameof(Date));
        OnPropertyChanged(nameof(Time));
        OnPropertyChanged(nameof(Note));
        OnPropertyChanged(nameof(Amount));
        OnPropertyChanged(nameof(ShowDeleteButton));
        OnPropertyChanged(nameof(ShowSaveButton));
    }
}
