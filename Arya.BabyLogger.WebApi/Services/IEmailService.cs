namespace Arya.BabyLogger.WebApi.Services;

public interface IEmailService
{
    Task SendAsync(string email, string subject, string body);
}