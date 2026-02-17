using System.Net.Http.Headers;
using System.Text.Json;
using Arya.BabyLogger.Mobile.Services.Jwt;
using Arya.BabyLogger.Mobile.Services.Storage;
using Arya.BabyLogger.Mobile.Views.BreastPump;
using Arya.BabyLogger.Shared.User;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arya.BabyLogger.Mobile.ViewModels.Profile;

public partial class ChangePasswordViewModel(HttpClient client) : ObservableObject
{
	[ObservableProperty]
	private string _currentPassword = string.Empty;

	[ObservableProperty]
	private string _newPassword = string.Empty;
	
	[ObservableProperty]
	private string _confirmPassword = string.Empty;

	[ObservableProperty]
	private string _confirmPasswordError = string.Empty;

	[ObservableProperty]
	private string _newPasswordError = string.Empty;
	
	[ObservableProperty]
	private string _currentPasswordError = string.Empty;

	[RelayCommand(CanExecute = nameof(CanUpdate))]
	private async Task Update()
	{
		if (!CanUpdate()) return;
		HttpResponseMessage? response = null;
		try
		{
			var json = JsonSerializer.Serialize(new ChangePasswordRequest { CurrentPassword = CurrentPassword, NewPassword = NewPassword });
			var request = new HttpRequestMessage(HttpMethod.Patch, "user/change-password");
			var content = new StringContent(json);
			
			var authToken = MobileStorageProvider.GetSecureStorage("AUTH_TOKEN");
			var userId = JwtTokenService.GetClaim("sub", authToken!);
			
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			
			request.Content = content;
			
			response = await client.SendAsync(request);
			
			response.EnsureSuccessStatusCode();
			
			await Application.Current!.Windows[0].Page!.DisplayAlertAsync("สำเร็จ", "รหัสผ่านถูกเปลี่ยนแล้ว", "OK");
			CurrentPassword = string.Empty;
			ConfirmPassword = string.Empty;
			
			await Shell.Current.GoToAsync($"//{nameof(BreastPumpListPage)}");
		}
		catch (HttpRequestException ex)
		{
			var errorMessage = ex.Message;
			
			if (response is not null)
			{
				errorMessage = response.Headers.GetValues("Error").FirstOrDefault() ?? errorMessage;
			}
			
			await Application.Current!.Windows[0].Page!.DisplayAlertAsync("เกิดข้อผิดพลาด", errorMessage, "OK");
		}
	}

	private bool CanUpdate() =>
		string.IsNullOrEmpty(CurrentPasswordError) &&
		string.IsNullOrEmpty(NewPasswordError) &&
		string.IsNullOrEmpty(ConfirmPasswordError) &&
		!string.IsNullOrWhiteSpace(CurrentPassword) &&
		!string.IsNullOrWhiteSpace(NewPassword) &&
		!string.IsNullOrWhiteSpace(ConfirmPassword);

	partial void OnCurrentPasswordChanged(string value)
	{
		ValidateCurrentPassword(value);
		
		UpdateCommand.NotifyCanExecuteChanged();
	}

	partial void OnNewPasswordChanged(string value)
	{
		ValidateConfirmPassword(ConfirmPassword, value);
		
		UpdateCommand.NotifyCanExecuteChanged();
	}
	
	partial void OnConfirmPasswordChanged(string value)
	{
		ValidateConfirmPassword(value, NewPassword);
		UpdateCommand.NotifyCanExecuteChanged();
	}

	private void ValidateCurrentPassword(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			CurrentPasswordError = "Password is required.";
		}
		else if (value.Length is < 4 or > 15)
		{
			CurrentPasswordError = "Password must be 4-15 characters.";
		}
		else
		{
			CurrentPasswordError = string.Empty;
		}
	}

	private void ValidateConfirmPassword(string confirm, string current)
	{
		if (string.IsNullOrWhiteSpace(confirm))
		{
			ConfirmPasswordError = "Please confirm your password.";
		}
		else if (confirm != current)
		{
			ConfirmPasswordError = "Passwords must match.";
		}
		else
		{
			ConfirmPasswordError = string.Empty;
		}
	}
}
