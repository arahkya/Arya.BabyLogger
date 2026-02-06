using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Arya.BabyLogger.Mobile.Views.BreastPump;
using Arya.BabyLogger.Shared.BreastPump;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Arya.BabyLogger.Mobile.Services;
using Arya.BabyLogger.Mobile.Services.Storage;
using CommunityToolkit.Maui.ApplicationModel;

namespace Arya.BabyLogger.Mobile.ViewModels.BreastPump;

public partial class BreastPumpListViewModel : ObservableObject
{
    public static int TimeIntervalInHours => 4;
    private readonly HttpClient _httpClient;
    private readonly ILocalNotificationService _localNotificationService;
    private readonly IBadge _badgeService;
    
    public DateTime NextPumpTime
    {
        get;
        set => SetProperty(ref field, value);
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

    

    public BreastPumpListViewModel(HttpClient httpClient, IBadge badgeService, ILocalNotificationService localNotificationService)
    {
        _httpClient = httpClient;
        _localNotificationService = localNotificationService;
        _badgeService = badgeService;

        PropertyChanged += async (_, e) =>
        {
            if (e.PropertyName is nameof(EndDate) or nameof(StartDate))
            {
                await LoadDataAsync();
            }
        };
    }

    public ObservableCollection<BreastPumpGroupViewModel> BreastPumpItemsGroup { get; } = [];

    [RelayCommand]
    private static async Task AddNewBreastPumpAsync()
    {
        var viewModel = App.Services.GetRequiredService<BreastPumpEntryViewModel>();
        var page = new BreastPumpEntryPage(viewModel);

        await Shell.Current.Navigation.PushModalAsync(page);
    }

    [RelayCommand]
    private static async Task SignOutAsync()
    {
        MobileStorageProvider.ClearSecureStorage("AUTH_TOKEN");
        
        await App.SwapGreetingPage();
    }
    
    public async Task LoadDataAsync()
    {
        _badgeService.SetCount(0);
        var authToken = MobileStorageProvider.GetSecureStorage("AUTH_TOKEN");
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        
        var startDate = new DateTimeOffset(StartDate.Year, StartDate.Month, StartDate.Day, 0, 0, 0, TimeSpan.Zero).ToString("yyyy-MM-ddTHH:mm:ssZ", new CultureInfo("en-US"));
        var endDate = new DateTimeOffset(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 59, TimeSpan.Zero).ToString("yyyy-MM-ddTHH:mm:ssZ", new CultureInfo("en-US"));
        var url = $"breastpump?startDate={startDate}&endDate={endDate}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await _httpClient.SendAsync(request);

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

        await _localNotificationService.CancelAllNotificationsAsync();

        if (await _localNotificationService.IsPendingNotificationAsync())
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

        await _localNotificationService.ShowNotificationAsync(
            "เวลาปั๊มนมแล้ว",
            "ถึงเวลาปั๊มนมอีกครั้งแล้ว อย่าลืมปั๊มนมให้น้องนะครับ",
            notifyTime);
    }
}
