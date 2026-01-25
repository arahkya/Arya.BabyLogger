namespace Arya.BabyLogger.Mobile.Services;

public interface ILocalNotificationService
{
    Task ShowNotificationAsync(string title, string message, DateTime? notifyTime);
    Task CancelAllNotificationsAsync();

    Task<bool> IsPendingNotificationAsync();
}