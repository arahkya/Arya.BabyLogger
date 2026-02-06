using Arya.BabyLogger.Mobile.Services.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arya.BabyLogger.Mobile.ViewModels.Landing;

public partial class GreetingViewModel(HttpClient httpClient) : ObservableObject
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    
    [RelayCommand]
    private async Task LoginAsync()
    {
        #if DEBUG
        var authToken = Environment.GetEnvironmentVariable("AUTH_TOKEN");
        MobileStorageProvider.SetSecureStorage("AUTH_TOKEN", authToken ?? throw new InvalidOperationException("AUTH_TOKEN is cannot not set."));
        #else
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "user/login"));
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        
        response.EnsureSuccessStatusCode();
        
        MobileStorageProvider.SetSecureStorage("AUTH_KEY", loginResponse!.Token);
        
        await App.SwapMainPage();
        #endif
    }
}