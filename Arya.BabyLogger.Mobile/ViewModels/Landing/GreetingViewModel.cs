using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Text.Json;
using Arya.BabyLogger.Mobile.Services.Storage;
using Arya.BabyLogger.Mobile.ViewModels.ResetPassword;
using Arya.BabyLogger.Mobile.Views.Landing;
using Arya.BabyLogger.Mobile.Views.ResetPassword;
using Arya.BabyLogger.Shared.User;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arya.BabyLogger.Mobile.ViewModels.Landing;

public partial class GreetingViewModel(HttpClient httpClient) : ObservableObject
{
    private string _email = string.Empty;
    public string Email
    {
        get => _email;
        set
        {
            if (SetProperty(ref _email, value) && !string.IsNullOrWhiteSpace(EmailError))
            {
                ValidateEmail();
            }
        }
    }

    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set
        {
            if (SetProperty(ref _password, value) && !string.IsNullOrWhiteSpace(PasswordError))
            {
                ValidatePassword();
            }
        }
    }

    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        private set => SetProperty(ref _errorMessage, value);
    }

    private string _emailError = string.Empty;
    public string EmailError
    {
        get => _emailError;
        private set => SetProperty(ref _emailError, value);
    }

    private string _passwordError = string.Empty;
    public string PasswordError
    {
        get => _passwordError;
        private set => SetProperty(ref _passwordError, value);
    }
    
    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;

        if (!ValidateInput())
        {
            return;
        }

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "user/login");
            var content = new StringContent(JsonSerializer.Serialize(new LoginRequest
            {
                Email = Email.Trim(),
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
        
        await Application.Current!.Windows[0].Page!.Navigation.PushAsync(registerPage);
    }

    private bool ValidateInput()
    {
        var isEmailValid = ValidateEmail();
        var isPasswordValid = ValidatePassword();

        return isEmailValid && isPasswordValid;
    }

    private bool ValidateEmail()
    {
        var value = Email?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(value))
        {
            EmailError = "กรุณากรอก Email";
            return false;
        }

        try
        {
            _ = new MailAddress(value);
            EmailError = string.Empty;
            return true;
        }
        catch
        {
            EmailError = "รูปแบบ Email ไม่ถูกต้อง";
            return false;
        }
    }

    private bool ValidatePassword()
    {
        if (string.IsNullOrWhiteSpace(Password))
        {
            PasswordError = "กรุณากรอก Password";
            return false;
        }

        if (Password.Length is < 4 or > 12)
        {
            PasswordError = "Password ต้องยาว 4-12 ตัวอักษร";
            return false;
        }

        PasswordError = string.Empty;
        return true;
    }

    [RelayCommand]
    private async Task GotoForgotPasswordPageAsync()
    {
        await Application.Current!.Windows[0].Page!.Navigation.PushAsync(new RequestResetPasswordPage(new RequestResetPasswordViewModel(httpClient)));
    }
}
