using System.Diagnostics;
using System.Net;
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
    public required string Email
    {
        get;
        set
        {
            if (SetProperty(ref field, value) && !string.IsNullOrWhiteSpace(EmailError))
            {
                ValidateEmail();
            }
        }
    }
    
    public required string Password
    {
        get;
        set
        {
            if (SetProperty(ref field, value) && !string.IsNullOrWhiteSpace(PasswordError))
            {
                ValidatePassword();
            }
        }
    }
    
    public required string ConfirmPassword
    {
        get;
        set
        {
            if (SetProperty(ref field, value) && !string.IsNullOrWhiteSpace(PasswordError))
            {
                ValidatePassword();
            }
        }
    }
    
    public required string Username
    {
        get;
        set
        {
            if (SetProperty(ref field, value) && !string.IsNullOrWhiteSpace(UsernameError))
            {
                ValidateUsername();
            }
        }
    }

    public string? ErrorMessage
    {
        get;
        private set => SetProperty(ref field, value);
    }
    
    public string? EmailError
    {
        get;
        private set => SetProperty(ref field, value);
    }
    
    public string? PasswordError
    {
        get;
        private set => SetProperty(ref field, value);
    }
    
    public string? ConfirmPasswordError
    {
        get;
        private set => SetProperty(ref field, value);
    }
    
    public string? UsernameError
    {
        get;
        private set => SetProperty(ref field, value);
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
                Password = Password
            });
            var requestContent = new StringContent(registerJson);
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Content = requestContent;

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            Debug.WriteLine("Successfully registered");

            await Application.Current!.Windows[0].Page!.Navigation.PushModalAsync(new RegisterCompletePage());
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Conflict)
            {
                ErrorMessage = "Email นี้ถูกลงทะเบียนไว้แล้ว กรุณาใช้อีเมล์อื่นและลองใหม่อีกครั้ง";
                Debug.WriteLine($"Register failed: {ex.StatusCode} ({ex.Message})");
                
                return;
            }
            
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
        var isConfirmPasswordValid = ValidateConfirmPassword();

        return isUsernameValid && isEmailValid && isPasswordValid && isConfirmPasswordValid;
    }

    private bool ValidateUsername()
    {
        var value = Username.Trim();

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
        var value = Email.Trim();

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

    private bool ValidateConfirmPassword()
    {
        if (Password == ConfirmPassword) return true;

        ConfirmPasswordError = "Password ไม่ตรงกัน";
        
        return false;
    }
}
