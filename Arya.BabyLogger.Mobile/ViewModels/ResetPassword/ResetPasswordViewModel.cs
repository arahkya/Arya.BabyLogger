using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using Arya.BabyLogger.Shared.User;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arya.BabyLogger.Mobile.ViewModels.ResetPassword;

public partial class ResetPasswordViewModel(HttpClient httpClient, string resetPasswordKey) : ObservableObject
{
    [ObservableProperty]
    private string _secretCode = string.Empty;

    [ObservableProperty]
    private string _newPassword = string.Empty;

    [ObservableProperty]
    private string _confirmPassword = string.Empty;

    public bool IsCodeValid => Regex.IsMatch(SecretCode ?? string.Empty, @"^\d{6}$");
    public bool IsPasswordValid => (NewPassword ?? string.Empty).Length >= 4;
    public bool DoPasswordsMatch => !string.IsNullOrEmpty(NewPassword) && NewPassword == ConfirmPassword;
    public bool ShowCodeError => !string.IsNullOrWhiteSpace(SecretCode) && !IsCodeValid;
    public bool ShowPasswordError => !string.IsNullOrWhiteSpace(NewPassword) && !IsPasswordValid;
    public bool ShowConfirmError => !string.IsNullOrWhiteSpace(ConfirmPassword) && !DoPasswordsMatch;
    public bool CanSubmit => IsCodeValid && IsPasswordValid && DoPasswordsMatch;

    partial void OnSecretCodeChanged(string value)
    {
        RaiseValidity();
    }

    partial void OnNewPasswordChanged(string value)
    {
        RaiseValidity();
    }

    partial void OnConfirmPasswordChanged(string value)
    {
        RaiseValidity();
    }

    private void RaiseValidity()
    {
        OnPropertyChanged(nameof(IsCodeValid));
        OnPropertyChanged(nameof(IsPasswordValid));
        OnPropertyChanged(nameof(DoPasswordsMatch));
        OnPropertyChanged(nameof(ShowCodeError));
        OnPropertyChanged(nameof(ShowPasswordError));
        OnPropertyChanged(nameof(ShowConfirmError));
        OnPropertyChanged(nameof(CanSubmit));
    }

    [RelayCommand]
    private async Task ResetPasswordAsync()
    {
        if (!CanSubmit) return;

        var json = JsonSerializer.Serialize(new ChangePasswordRequest { SecretKey = resetPasswordKey, NewPassword = NewPassword, SecretCode = SecretCode});
        var content = new StringContent(json);
        var request = new HttpRequestMessage(HttpMethod.Patch, "user/reset-password");
        
        request.Content = content;
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpResponseMessage? response = null;

        try
        {
            response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var success = await response.Content.ReadAsStringAsync();

            if (!success.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                await Application.Current!.Windows[0].Page!.DisplayAlertAsync("ผิดพลาด", "เปลี่ยนรหัสผ่านไม่สำเร็จ กรุณาติดต่อ arahk@arahk.com", "OK");
            }
            
            await Application.Current!.Windows[0].Page!.DisplayAlertAsync("สำเร็จ", "ระบบจะพากลับไปหน้า Login.", "OK");
            await App.SwapGreetingPage();
        }
        catch (HttpRequestException ex)
        {
            var errorMessage = response!.Headers.TryGetValues("Error", out var values) ? values.First() : ex.Message;
            await Application.Current!.Windows[0].Page!.DisplayAlertAsync("ตรวจสอบ", errorMessage, "OK");
        }
    }

    [RelayCommand]
    private static async Task CloseAsync()
    {
        await Application.Current!.Windows[0].Page!.Navigation.PopModalAsync();
    }
}
