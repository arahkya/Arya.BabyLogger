using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http.Json;
using Arya.BabyLogger.Mobile.Views.BreastPump;
using Arya.BabyLogger.Shared.BreastPump;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

public class BreastPumpGroupViewModel(string groupTitle, BreastPumpItemViewModel[] items) : ObservableCollection<BreastPumpItemViewModel>(items)
{
    public string GroupTitle { get; set; } = groupTitle;
    public double TotalAmountInMl => this.Sum(i => i.AmountInMl);
}

public partial class BreastPumpListViewModel : ObservableObject
{
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

    public BreastPumpListViewModel(HttpClient httpClient)
    {
        this.httpClient = httpClient;

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

        BreastPumpItemsGroup.Clear();

        foreach (var group in groupedItems)
        {
            BreastPumpItemsGroup.Add(new BreastPumpGroupViewModel(group.Key, [.. group.OrderBy(i => i.PumpTime)]));
        }
    }
}