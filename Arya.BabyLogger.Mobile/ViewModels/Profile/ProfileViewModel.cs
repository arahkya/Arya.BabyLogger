using System.Net.Http.Headers;
using System.Text.Json;
using Arya.BabyLogger.Mobile.Services;
using Arya.BabyLogger.Mobile.Services.Jwt;
using Arya.BabyLogger.Mobile.Services.Net;
using Arya.BabyLogger.Mobile.Services.Storage;
using Arya.BabyLogger.Shared.User;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arya.BabyLogger.Mobile.ViewModels.Profile;

public partial class ProfileViewModel : ObservableObject
{
	[ObservableProperty]
	private string _username = string.Empty;
	
	[ObservableProperty]
	private string _usernameError = string.Empty;

	[ObservableProperty]
	private bool _isBusy;

	partial void OnUsernameChanged(string value)
	{
		ValidateUsername(value);
		UpdateUsernameCommand.NotifyCanExecuteChanged();
	}

	public bool CanUpdate() => !IsBusy && string.IsNullOrWhiteSpace(UsernameError) && !string.IsNullOrWhiteSpace(Username);

	[RelayCommand(CanExecute = nameof(CanUpdate))]
	private async Task UpdateUsernameAsync()
	{
		if (!CanUpdate()) return;

		try
		{
			IsBusy = true;
			UpdateUsernameCommand.NotifyCanExecuteChanged();
			
			var jsonPayload = JsonSerializer.Serialize(new UpdateUsernameRequest { Username = Username.Trim() });
			var request = new HttpRequestMessage(HttpMethod.Patch, "user/username");
			var content = new StringContent(jsonPayload);
			var authToken = MobileStorageProvider.GetSecureStorage("AUTH_TOKEN");
			var userId = JwtTokenService.GetClaim("sub", authToken!);
			
			request.Content = content;
			request.Content.Headers.Add("User-Id", userId);
			request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

			var httpClient = new HttpClient(HttpClientHandlerProvider.CreateHandler())
			{
				BaseAddress	= new Uri(BuildConstraints.WebApiUrl)
			};
			
			var response = await httpClient.SendAsync(request);

			if (!response.IsSuccessStatusCode)
			{
				var errorMessage = response.Headers.TryGetValues("Error", out var values)
					? values.FirstOrDefault()
					: response.ReasonPhrase ?? "ไม่สามารถอัปเดต Username";
				
				await Application.Current!.Windows[0].Page!.DisplayAlertAsync("เกิดข้อผิดพลาด", errorMessage, "OK");
				return;
			}

			await Application.Current!.Windows[0].Page!.DisplayAlertAsync("สำเร็จ", "Username ถูกอัปเดตแล้ว", "OK");

			if (Shell.Current?.BindingContext is ShellViewModel shellVm)
			{
				shellVm.Username = Username.Trim();
			}
		}
		catch (HttpRequestException ex)
		{
			await Application.Current!.Windows[0].Page!.DisplayAlertAsync("เกิดข้อผิดพลาด", ex.Message, "OK");
		}
		finally
		{
			IsBusy = false;
			UpdateUsernameCommand.NotifyCanExecuteChanged();
		}
	}

	private void ValidateUsername(string value)
	{
		var trimmed = value.Trim();

		if (string.IsNullOrWhiteSpace(trimmed))
		{
			UsernameError = "กรุณากรอก Username";
			return;
		}

		if (trimmed.Length is < 4 or > 12)
		{
			UsernameError = "Username ต้องยาว 5-14 ตัวอักษร";
			return;
		}

		UsernameError = string.Empty;
	}

	public void LoadCurrentUsername()
	{
		var token = MobileStorageProvider.GetSecureStorage("AUTH_TOKEN") ?? string.Empty;
		var nameClaim = JwtTokenService.GetClaim("unique_name", token);

		Username = nameClaim;
		
		if (Shell.Current?.BindingContext is ShellViewModel shellVm)
		{
			Username = shellVm.Username;
		}
	}
}
