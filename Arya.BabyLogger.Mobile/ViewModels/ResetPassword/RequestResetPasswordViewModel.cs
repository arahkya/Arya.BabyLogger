using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Text.Json;
using Arya.BabyLogger.Mobile.Views.ResetPassword;
using Arya.BabyLogger.Shared.User;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arya.BabyLogger.Mobile.ViewModels.ResetPassword;

public partial class RequestResetPasswordViewModel(HttpClient httpClient) : ObservableObject
{
    [ObservableProperty]
    private string _email = string.Empty;
    
    public bool IsEmailValid => Regex.IsMatch(Email ?? string.Empty,
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);
    public bool ShowEmailError => !string.IsNullOrWhiteSpace(Email) && !IsEmailValid;

    partial void OnEmailChanged(string value)
    {
        OnPropertyChanged(nameof(IsEmailValid));
        OnPropertyChanged(nameof(ShowEmailError));
    }
    
    [RelayCommand]
    private async Task RequestSecretCodeAsync()
    {
        if (!IsEmailValid) return;
        
        var json = JsonSerializer.Serialize(new RequestResetPasswordRequest { Email = Email });
        var content = new StringContent(json);
        var request = new HttpRequestMessage(HttpMethod.Post, "user/reset-password");
        
        request.Content = content;
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpResponseMessage? response = null;
        
        try
        {
            response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var resetPasswordId = await response.Content.ReadAsStringAsync();

            await Application.Current!.Windows[0].Page!.DisplayAlertAsync("สำเร็จ", "ส่งอีเมล์สำเร็จโปรดตรวจสอบอินบ๊อกซ์ของท่าน", "OK");
            await Application.Current.Windows[0].Navigation.PushModalAsync(new ResetPasswordPage(new ResetPasswordViewModel(httpClient, resetPasswordId)));
        }
        catch (HttpRequestException ex)
        {   
            var errorMessage = response!.Headers.TryGetValues("Error", out var values) ? values.First() : ex.Message;
            await Application.Current!.Windows[0].Page!.DisplayAlertAsync("ตรวจสอบ", errorMessage, "OK");
        }
    }
}
