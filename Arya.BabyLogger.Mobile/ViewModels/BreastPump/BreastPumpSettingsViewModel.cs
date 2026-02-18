using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using Arya.BabyLogger.Mobile.Services.Jwt;
using Arya.BabyLogger.Mobile.Services.Storage;
using Arya.BabyLogger.Shared.User;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arya.BabyLogger.Mobile.ViewModels.BreastPump;

public partial class BreastPumpSettingsViewModel : ObservableObject
{
    private const string PreferenceKey = "BreastPumpTimeIntervalHours";
    private const int DefaultIntervalHours = 3;
    private const int MinIntervalHours = 1;
    private const int MaxIntervalHours = 12;

    private readonly HttpClient _httpClient;

    public int BreastPumpTimeIntervalHours
    {
        get;
        set
        {
            var clampedValue = Math.Clamp(value, MinIntervalHours, MaxIntervalHours);
            SetProperty(ref field, clampedValue);
        }
    } = DefaultIntervalHours;

    public BreastPumpSettingsViewModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
        LoadSettings();
    }

    public static int GetSavedTimeIntervalHours()
    {
        return Preferences.Get(PreferenceKey, DefaultIntervalHours);
    }

    private void LoadSettings()
    {   
        BreastPumpTimeIntervalHours = GetSavedTimeIntervalHours();
    }

    [RelayCommand]
    private async Task SaveSettings()
    {
        try
        {
            var json = JsonSerializer.Serialize(new BreastPumpSettingsRquestResponse() { PumpIntervalHours = BreastPumpTimeIntervalHours });
            var request = new HttpRequestMessage(HttpMethod.Put, "user/settings-breastpump");
            request.Content = new StringContent(json);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            var authToken = MobileStorageProvider.GetSecureStorage("AUTH_TOKEN") ?? string.Empty;

            if (authToken == string.Empty) return;

            var userId = JwtTokenService.GetClaim("sub", authToken!);
            request.Headers.Add("User-Id", userId);
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
            
            Preferences.Set(PreferenceKey, BreastPumpTimeIntervalHours);
        }
        catch(Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}
