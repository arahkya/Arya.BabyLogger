using System.Diagnostics;
using System.Net.Http.Headers;
using Arya.BabyLogger.Mobile.Services;
using Arya.BabyLogger.Mobile.Services.Jwt;
using Arya.BabyLogger.Mobile.Services.Net;
using Arya.BabyLogger.Mobile.Services.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arya.BabyLogger.Mobile.ViewModels.Profile;

public partial class InviteCareHolderViewModel : ObservableObject
{
    [ObservableProperty] private string _inviteCode = string.Empty;
    
    [RelayCommand]
    private async Task RequestInviteCodeAsync()
    {
        try
        {
            var httpClient = new HttpClient(HttpClientHandlerProvider.CreateHandler())
            {
                BaseAddress = new Uri(BuildConstraints.WebApiUrl)
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "user/invite-care-holder");
            var authToken = MobileStorageProvider.GetSecureStorage("AUTH_TOKEN");
            var userId = JwtTokenService.GetClaim("sub", authToken!);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            request.Headers.Add("User-Id", userId);

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            InviteCode = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}