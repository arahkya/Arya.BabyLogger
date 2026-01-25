using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http.Json;
using Arya.BabyLogger.Mobile.Views.BreastPump;
using Arya.BabyLogger.Shared.BreastPump;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Arya.BabyLogger.Mobile.Services;
using CommunityToolkit.Maui.ApplicationModel;

namespace Arya.BabyLogger.Mobile.ViewModels.BreastPump;

public partial class BreastPumpItemViewModel : ObservableObject
{
    public Guid Id { get; set; }
    public DateTimeOffset PumpTime { get; set; }
    public string PumpTimeString => PumpTime.ToString("HH:mm");
    public double AmountInMl { get; set; }

    [RelayCommand]
    public async Task ItemSelectedAsync()
    {
        var viewModel = App.Services.GetRequiredService<BreastPumpEntryViewModel>();
        viewModel.ListItemId = Id;
        await viewModel.LoadDataAsync();

        var page = new BreastPumpEntryPage(viewModel);

        await Shell.Current.Navigation.PushModalAsync(page);
    }
}

public class BreastPumpGroupViewModel : ObservableCollection<BreastPumpItemViewModel>
{
    private string _groupTitle = string.Empty;
    public string GroupTitle
    {
        get => _groupTitle;
        set
        {
            _groupTitle = value;
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(GroupTitle)));
        }
    }

    public double TotalAmountInMl => this.Sum(i => i.AmountInMl);

    public BreastPumpGroupViewModel(string groupTitle, BreastPumpItemViewModel[] items) : base(items)
    {
        GroupTitle = groupTitle;
    }

    protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        base.OnCollectionChanged(e);
        OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(TotalAmountInMl)));
    }
}

public partial class BreastPumpListViewModel : ObservableObject
{
    private int _timeIntervalInHours = 4;
    public int TimeIntervalInHours => _timeIntervalInHours;

    private DateTime _nextPumpTime;
    public DateTime NextPumpTime
    {
        get => _nextPumpTime;
        set => SetProperty(ref _nextPumpTime, value);
    }

    public string RemainingMinutesTilNextPump
    {
        get
        {
            var timeDiff = NextPumpTime - DateTime.Now;
            var hours = (int)timeDiff.TotalHours;
            var minutes = timeDiff.Minutes;

            if (minutes < 0)
            {
                if (hours == 0)
                {
                    return $"เลยเวลามาแล้ว {Math.Abs(minutes)} นาที";
                }

                return $"เลยเวลามาแล้ว {Math.Abs(hours)} ชั่วโมง {Math.Abs(minutes)} นาที";
            }

            if (minutes == 0)
            {
                return "ถึงเวลาปั๊มนมแล้ว";
            }

            if (hours == 0)
            {
                return $"{minutes} นาที";
            }

            return $"{hours} ชั่วโมง {minutes} นาที";
        }
    }

    private DateTimeOffset _startDate = DateTimeOffset.Now.AddDays(-7);
    private DateTimeOffset _endDate = DateTimeOffset.Now;
    public DateTimeOffset StartDate
    {
        get => _startDate;
        set => SetProperty(ref _startDate, value);
    }
    public DateTimeOffset EndDate
    {
        get => _endDate;
        set => SetProperty(ref _endDate, value);
    }

    private readonly HttpClient httpClient;
    private readonly ILocalNotificationService localNotificationService;
    private readonly IBadge badge;

    public BreastPumpListViewModel(HttpClient httpClient, IBadge badge, ILocalNotificationService localNotificationService)
    {
        this.httpClient = httpClient;
        this.localNotificationService = localNotificationService;
        this.badge = badge;

        PropertyChanged += async (s, e) =>
        {
            if (e.PropertyName == nameof(EndDate) || e.PropertyName == nameof(StartDate))
            {
                await LoadDataAsync();
            }
        };
    }

    public ObservableCollection<BreastPumpGroupViewModel> BreastPumpItemsGroup { get; } = [];

    [RelayCommand]
    public async Task AddNewBreastPumpAsync()
    {
        var viewModel = App.Services.GetRequiredService<BreastPumpEntryViewModel>();
        var page = new BreastPumpEntryPage(viewModel);

        await Shell.Current.Navigation.PushModalAsync(page);
    }

    public async Task LoadDataAsync()
    {
        badge.SetCount(0);

        var startDate = new DateTimeOffset(StartDate.Year, StartDate.Month, StartDate.Day, 0, 0, 0, TimeSpan.Zero).ToString("yyyy-MM-ddTHH:mm:ssZ", new CultureInfo("en-US"));
        var endDate = new DateTimeOffset(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 59, TimeSpan.Zero).ToString("yyyy-MM-ddTHH:mm:ssZ", new CultureInfo("en-US"));
        var url = $"breastpump?startDate={startDate}&endDate={endDate}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var items = await response.Content.ReadFromJsonAsync<BreastPumpListItemsResponse>();
        var groupedItems = items!.Items
            .Select(i => new BreastPumpItemViewModel
            {
                Id = i.Id,
                PumpTime = i.PumpTime,
                AmountInMl = i.AmountML
            })
            .GroupBy(i => i.PumpTime.ToString("yyyy-MM-dd"));

        MainThread.BeginInvokeOnMainThread(() =>
        {
            BreastPumpItemsGroup.Clear();

            foreach (var group in groupedItems)
            {
                BreastPumpItemsGroup.Add(new BreastPumpGroupViewModel(group.Key, [.. group.OrderBy(i => i.PumpTime)]));
            }
        });

        NextPumpTime = items.Items.FirstOrDefault()?.PumpTime.AddHours(TimeIntervalInHours).DateTime ?? DateTime.Now.AddHours(TimeIntervalInHours);

        OnPropertyChanged(nameof(RemainingMinutesTilNextPump));

        await localNotificationService.CancelAllNotificationsAsync();

        if (await localNotificationService.IsPendingNotificationAsync())
            return;

        var timeInterval = 15;

        var remainingTime = NextPumpTime.Subtract(DateTime.Now);

        if (remainingTime.TotalMinutes < timeInterval)
        {
            timeInterval = 2;
        }

        var notifyTime = DateTime.Now.AddMinutes(remainingTime.TotalMinutes - timeInterval);

        if (remainingTime.TotalMinutes < 0)
        {
            notifyTime = DateTime.Now.AddMinutes(timeInterval);
        }

        await localNotificationService.ShowNotificationAsync(
            "เวลาปั๊มนมแล้ว",
            "ถึงเวลาปั๊มนมอีกครั้งแล้ว อย่าลืมปั๊มนมให้น้องนะครับ",
            notifyTime);
    }
}
