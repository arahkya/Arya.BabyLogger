using System.Net.Http.Headers;
using System.Text.Json;
using System.Linq;
using Arya.BabyLogger.Mobile.Services.Jwt;
using Arya.BabyLogger.Mobile.Services.Storage;
using Arya.BabyLogger.Shared.User;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arya.BabyLogger.Mobile.ViewModels.Profile;

public partial class ProfileViewModel(HttpClient httpClient) : ObservableObject
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
			
			var authToken = MobileStorageProvider.GetSecureStorage("AUTH_TOKEN");
			if (string.IsNullOrWhiteSpace(authToken))
			{
				await Application.Current!.Windows[0].Page!.DisplayAlertAsync("ข้อผิดพลาด", "กรุณาเข้าสู่ระบบใหม่", "OK");
				return;
			}
			var userId = JwtTokenService.GetClaim("sub", authToken!);
			
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

			var request = new HttpRequestMessage(HttpMethod.Patch, "user/username");
			var payload = JsonSerializer.Serialize(new UpdateUsernameRequest { Username = Username.Trim() });
			var content = new StringContent(payload);
			content.Headers.Add("User-Id",userId);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;

			var response = await httpClient.SendAsync(request);

			if (!response.IsSuccessStatusCode)
			{
				var errorMessage = response.Headers.TryGetValues("Error", out var values)
					? values.FirstOrDefault()
					: response.ReasonPhrase ?? "ไม่สามารถอัปเดต Username";
				
				await Application.Current!.Windows[0].Page!.DisplayAlertAsync("เกิดข้อผิดพลาด", errorMessage, "OK");
				return;
			}

			await Application.Current!.Windows[0].Page!.DisplayAlertAsync("สำเร็จ", "Username ถูกอัปเดตแล้ว กรุณาออกจากระบบ และเข้าใหม่เพื่อการแสดงผลที่ถูกต้อง", "OK");

			if (Shell.Current?.BindingContext is ViewModels.ShellViewModel shellVm)
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
		var trimmed = value?.Trim() ?? string.Empty;

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

		Username = nameClaim ?? string.Empty;
	}
}
