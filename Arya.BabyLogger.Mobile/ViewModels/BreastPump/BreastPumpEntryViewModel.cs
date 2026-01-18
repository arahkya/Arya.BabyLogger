using System.Net.Http.Json;
using Arya.BabyLogger.Shared.BreastPump;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arya.BabyLogger.Mobile.ViewModels.BreastPump;

public partial class BreastPumpEntryViewModel(HttpClient client) : ObservableObject
{
    public bool IsEditMode => _listItemId != Guid.Empty;
    public bool IsAddMode => _listItemId == Guid.Empty;

    private Guid _listItemId = Guid.Empty;
    private BreastPumpDetailResponse? _breastDetailResponse;

    public Guid ListItemId
    {
        get => _listItemId;
        set => SetProperty(ref _listItemId, value);
    }

    public Guid Id
    {
        get => _breastDetailResponse?.Id ?? Guid.Empty;
        set
        {
            if (_breastDetailResponse != null)
            {
                _breastDetailResponse.Id = value;
                OnPropertyChanged();
            }
        }
    }

    public TimeSpan PumpTime
    {
        get => new TimeSpan(_breastDetailResponse?.PumpTime.TimeOfDay.Ticks ?? default);
        set
        {
            if (_breastDetailResponse != null)
            {
                var date = _breastDetailResponse.PumpTime.Date;
                _breastDetailResponse.PumpTime = new DateTimeOffset(date.Add(value), _breastDetailResponse.PumpTime.Offset);
                OnPropertyChanged();
            }
        }
    }

    public DateTime PumpDate
    {
        get
        {
            var date = new DateTime(_breastDetailResponse?.PumpTime.Year ?? 1,
                                    _breastDetailResponse?.PumpTime.Month ?? 1,
                                    _breastDetailResponse?.PumpTime.Day ?? 1);
            return date;
        }
        set
        {
            if (_breastDetailResponse != null)
            {
                _breastDetailResponse.PumpTime = new DateTimeOffset(value.Add(_breastDetailResponse.PumpTime.TimeOfDay), _breastDetailResponse.PumpTime.Offset);
                OnPropertyChanged();
            }
        }
    }
    public int AmountML
    {
        get => _breastDetailResponse?.AmountML ?? 0;
        set
        {
            if (_breastDetailResponse != null)
            {
                _breastDetailResponse.AmountML = value;
                OnPropertyChanged();
            }
        }
    }
    public string? Note
    {
        get => _breastDetailResponse?.Note;
        set
        {
            if (_breastDetailResponse != null)
            {
                _breastDetailResponse.Note = value;
                OnPropertyChanged();
            }
        }
    }

    public async Task LoadDataAsync()
    {
        if (_listItemId == Guid.Empty)
        {
            _breastDetailResponse = new BreastPumpDetailResponse
            {
                Id = Guid.NewGuid(),
                PumpTime = DateTimeOffset.Now,
                AmountML = 0,
                Note = string.Empty
            };

            OnPropertyChanged(nameof(Id));
            OnPropertyChanged(nameof(PumpDate));
            OnPropertyChanged(nameof(PumpTime));
            OnPropertyChanged(nameof(AmountML));
            OnPropertyChanged(nameof(Note));

            return;
        }

        // Implementation for loading data related to breast pump entries
        var url = $"breastpump/{_listItemId}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var breastDetailResponse = await response.Content.ReadFromJsonAsync<BreastPumpDetailResponse>();

        _breastDetailResponse = breastDetailResponse;

        OnPropertyChanged(nameof(Id));
        OnPropertyChanged(nameof(PumpDate));
        OnPropertyChanged(nameof(PumpTime));
        OnPropertyChanged(nameof(AmountML));
        OnPropertyChanged(nameof(Note));
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        if ((_breastDetailResponse?.AmountML ?? 0) <= 0)
        {
            return;
        }

        var breastPumpCreateRequest = new BreastPumpCreateRequest
        {
            PumpTime = _breastDetailResponse!.PumpTime.AddHours(7),
            AmountML = _breastDetailResponse.AmountML,
            Note = _breastDetailResponse.Note
        };

        var url = "breastpump";
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(breastPumpCreateRequest)
        };
        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        await Shell.Current.Navigation.PopModalAsync();
    }

    [RelayCommand]
    public async Task UpdateAsync()
    {
        var url = $"breastpump/{_listItemId}";
        var request = new HttpRequestMessage(HttpMethod.Put, url)
        {
            Content = JsonContent.Create(_breastDetailResponse)
        };
        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        await Shell.Current.Navigation.PopModalAsync();
    }

    [RelayCommand]
    public async Task DeleteAsync()
    {
        var url = $"breastpump/{_listItemId}";
        var request = new HttpRequestMessage(HttpMethod.Delete, url);
        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        await Shell.Current.Navigation.PopModalAsync();
    }

    [RelayCommand]
    public async Task CancelAsync()
    {
        await Shell.Current.Navigation.PopModalAsync();
    }
}
