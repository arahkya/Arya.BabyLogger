using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Arya.BabyLogger.Mobile.Services;
using Arya.BabyLogger.Mobile.Services.Jwt;
using Arya.BabyLogger.Mobile.Services.Net;
using Arya.BabyLogger.Mobile.Services.Storage;
using Arya.BabyLogger.Mobile.Views.BreastPump;
using Arya.BabyLogger.Shared.User;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arya.BabyLogger.Mobile.ViewModels.Profile;

public partial class AcceptInviteCareHolderViewModel : ObservableObject
{
    [ObservableProperty] private string _inviteCode = string.Empty;
    [ObservableProperty] private string _errors = string.Empty;
    [ObservableProperty] private bool _isErrors;
    
    [RelayCommand]
    private async Task AcceptInvite()
    { 
        IsErrors = false;
        Errors = string.Empty;

        if (string.IsNullOrWhiteSpace(InviteCode) || !Regex.IsMatch(InviteCode, @"^[0-9]{6}$"))
        {
            Errors = "รูปแบบ Invite Code ไม่ถูกต้อง";
            IsErrors = true;
            return;
        }

        HttpResponseMessage? response = null;

        try
        {
            var authToken = MobileStorageProvider.GetSecureStorage("AUTH_TOKEN") ?? string.Empty;
            var userId = JwtTokenService.GetClaim("sub", authToken);

            var requestBody = new AcceptCareHolderInviteRequest { InviteSecret = InviteCode };
            var request = new HttpRequestMessage(HttpMethod.Post, "user/accept-invite-care-holder")
            {
                Content = JsonContent.Create(requestBody)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            request.Headers.Add("User-Id", userId);

            var httpClient = new HttpClient(HttpClientHandlerProvider.CreateHandler())
            {
                BaseAddress = new Uri(BuildConstraints.WebApiUrl)
            };

            response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            await Shell.Current.GoToAsync($"//{nameof(BreastPumpListPage)}");
        }
        catch (HttpRequestException ex)
        {
            var errorMessage = ex.Message;
            if (response != null && response.Headers.TryGetValues("Error", out var headerErrors))
            {
                errorMessage = headerErrors.FirstOrDefault() ?? errorMessage;
            }

            Errors = errorMessage;
            IsErrors = true;
        }
    }
}
