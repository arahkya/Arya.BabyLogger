using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using Arya.BabyLogger.Mobile.Views.Landing;
using Arya.BabyLogger.Shared.User;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arya.BabyLogger.Mobile.ViewModels.Landing;

public partial class RegisterViewModel(HttpClient httpClient) : ObservableObject
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    private Guid? CareHolderId { get; set; }

    private string _careHolderIdText = string.Empty;
    public string CareHolderIdText
    {
        get => _careHolderIdText;
        set
        {
            if (SetProperty(ref _careHolderIdText, value))
            {
                CareHolderId = Guid.TryParse(value, out var parsed) ? parsed : null;
            }
        }
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "user/register");
        var registerJson = JsonSerializer.Serialize(new RegisterRequest
        {
            Username = Username,
            Email = Email,
            Password = Password,
            CareHolderId = CareHolderId?.ToString()
        });
        var requestContent = new StringContent(registerJson);
        requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        request.Content = requestContent;
        
        var response = await httpClient.SendAsync(request);
        
        response.EnsureSuccessStatusCode();
        
        Debug.WriteLine("Successfully registered");

        await Application.Current.MainPage.Navigation.PushModalAsync(new RegisterCompletePage());
    }
}
