using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arya.BabyLogger.Mobile.ViewModels.ResetPassword;

public partial class ResetPasswordViewModel(string resetPasswordKey) : ObservableObject
{
    [ObservableProperty]
    private string _secretCode = string.Empty;

    [ObservableProperty]
    private string _newPassword = string.Empty;

    [ObservableProperty]
    private string _confirmPassword = string.Empty;

    public bool IsCodeValid => Regex.IsMatch(SecretCode ?? string.Empty, @"^\d{6}$");
    public bool IsPasswordValid => (NewPassword ?? string.Empty).Length >= 8;
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
    private static async Task ResetPasswordAsync()
    {
        await Application.Current!.Windows[0].Page!.DisplayAlertAsync("สำเร็จ", "ระบบจะพากลับไปหน้า Login.", "OK");
        await App.SwapGreetingPage();
    }

    [RelayCommand]
    private static async Task CloseAsync()
    {
        await Application.Current!.Windows[0].Page!.Navigation.PopModalAsync();
    }
}
