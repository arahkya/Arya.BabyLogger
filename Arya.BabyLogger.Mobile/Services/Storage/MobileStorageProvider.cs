using System.Diagnostics;

namespace Arya.BabyLogger.Mobile.Services.Storage;

public static class MobileStorageProvider
{
    public static string? GetSecureStorage(string key)
    {
        try
        {
            return SecureStorage.Default.GetAsync(key).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SecureStorage: unable to read {key} - {ex.Message}");
            return null;
        }
    }

    public static void SetSecureStorage(string key, string context)
    {
        SecureStorage.Default.SetAsync(key, context).GetAwaiter().GetResult();
    }

    public static void ClearSecureStorage(string key)
    {
        SecureStorage.Default.Remove(key);
    }
}