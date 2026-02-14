using Arya.BabyLogger.Mobile.Services;
using Foundation;
using UserNotifications;

namespace Arya.BabyLogger.Mobile.Platforms.iOS;

public class LocalNotificationService : ILocalNotificationService
{
    int messageId = 0;
    bool hasNotificationsPermission;

    public LocalNotificationService()
    {
        UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Badge, (approved, err) =>
        {
            hasNotificationsPermission = approved;
        });
    }

    public Task<bool> IsPendingNotificationAsync()
    {
        var tcs = new TaskCompletionSource<bool>();

        UNUserNotificationCenter.Current.GetPendingNotificationRequests((requests) =>
        {
            tcs.SetResult(requests.Length > 0);
        });

        return tcs.Task;
    }

    public async Task ShowNotificationAsync(string title, string message, DateTime? notifyTime)
    {
        if (!hasNotificationsPermission)
            return;

        messageId++;
        var content = new UNMutableNotificationContent()
        {
            Title = title,
            Subtitle = "",
            Body = message,
            Badge = 1,
            Sound = UNNotificationSound.Default,
            CategoryIdentifier = "BabyLogger",
            ThreadIdentifier = "BabyLogger",
            UserInfo = new NSDictionary(),
            LaunchImageName = "AppIcon"
            
        };

        UNNotificationTrigger trigger;
        if (notifyTime != null)
            // Create a calendar-based trigger.
            trigger = UNCalendarNotificationTrigger.CreateTrigger(GetNSDateComponentsBuddish(notifyTime.Value), true);
        else
            // Create a time-based trigger, interval is in seconds and must be greater than 0.
            trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(0.25, true);

        var request = UNNotificationRequest.FromIdentifier(messageId.ToString(), content, trigger);

        UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
        {
            if (err != null)
                throw new Exception($"Failed to schedule notification: {err}");
        });
    }

    private async Task CancelAllNotificationsAsync()
    {
        UNUserNotificationCenter.Current.RemoveAllPendingNotificationRequests();
        UNUserNotificationCenter.Current.RemoveAllDeliveredNotifications();
    }

    Task ILocalNotificationService.CancelAllNotificationsAsync()
    {
        return CancelAllNotificationsAsync();
    }

    NSDateComponents GetNSDateComponentsBuddish(DateTime dateTime)
    {
        var nsDate = (NSDate)dateTime;

        var calendar = NSCalendar.CurrentCalendar;
        var budishDateComponents = calendar.Components(
            NSCalendarUnit.Year |
            NSCalendarUnit.Month |
            NSCalendarUnit.Day |
            NSCalendarUnit.Hour |
            NSCalendarUnit.Minute |
            NSCalendarUnit.Second,
            nsDate);

        return budishDateComponents;
    }
}
