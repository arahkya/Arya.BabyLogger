using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text.Json;
using Arya.BabyLogger.Mobile.Views.Landing;
using Arya.BabyLogger.Shared.User;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arya.BabyLogger.Mobile.ViewModels.Landing;

public partial class RegisterViewModel(HttpClient httpClient) : ObservableObject
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

    private string _username = string.Empty;
    public string Username
    {
        get => _username;
        set
        {
            if (SetProperty(ref _username, value) && !string.IsNullOrWhiteSpace(UsernameError))
            {
                ValidateUsername();
            }
        }
    }

    private Guid? CareHolderId { get; set; }

    private string _errorMessage = string.Empty;
    public string ErrorMessage
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

    private string _usernameError = string.Empty;
    public string UsernameError
    {
        get => _usernameError;
        private set => SetProperty(ref _usernameError, value);
    }

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
        ErrorMessage = string.Empty;

        if (!ValidateInput())
        {
            return;
        }

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "user/register");
            var registerJson = JsonSerializer.Serialize(new RegisterRequest
            {
                Username = Username.Trim(),
                Email = Email.Trim(),
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
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Register failed: {ex.StatusCode} ({ex.Message})");
            ErrorMessage = "ไม่สามารถลงทะเบียนได้ กรุณาลองใหม่อีกครั้ง";
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Register failed unexpectedly: {ex.Message}");
            ErrorMessage = "เกิดข้อผิดพลาดที่ไม่คาดคิด";
        }
    }

    private bool ValidateInput()
    {
        var isUsernameValid = ValidateUsername();
        var isEmailValid = ValidateEmail();
        var isPasswordValid = ValidatePassword();

        return isUsernameValid && isEmailValid && isPasswordValid;
    }

    private bool ValidateUsername()
    {
        var value = Username?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(value))
        {
            UsernameError = "กรุณากรอก Username";
            return false;
        }

        if (value.Length is < 4 or > 12)
        {
            UsernameError = "Username ต้องยาว 4-12 ตัวอักษร";
            return false;
        }

        UsernameError = string.Empty;
        return true;
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
}
