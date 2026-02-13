using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Arya.BabyLogger.Mobile.Services.Storage;
using Arya.BabyLogger.Mobile.Views.Landing;
using Arya.BabyLogger.Shared.User;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arya.BabyLogger.Mobile.ViewModels.Landing;

public partial class GreetingViewModel(HttpClient httpClient) : ObservableObject
{
    public string Email { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public string? ErrorMessage { get; set; }
    
    [RelayCommand]
    private async Task LoginAsync()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "user/login");
            var content = new StringContent(JsonSerializer.Serialize(new LoginRequest
            {
                Email = Email,
                Password = Password
            }));

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Content = content;

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

            MobileStorageProvider.SetSecureStorage("AUTH_TOKEN", loginResponse!.Token);

            await App.SwapMainPage();
        } 
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Login failed: {ex.StatusCode} ({ex.Message})");

            ErrorMessage = string.IsNullOrWhiteSpace(ex.StatusCode.ToString()) ? ex.Message : ex.StatusCode.ToString();
            OnPropertyChanged(nameof(ErrorMessage));
        }
        catch
        {
            ErrorMessage = "An unknown error occurred";
        }
    }
    
    [RelayCommand]
    private static async Task GotoRegisterPageAsync()
    {
        var registerPage = App.Services.GetRequiredService<RegisterPage>();
        
        await Application.Current.MainPage.Navigation.PushAsync(registerPage);
    }
}