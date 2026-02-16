using Mailtrap;
using Mailtrap.Emails.Requests;

namespace Arya.BabyLogger.WebApi.Services;

public class EmailService(IConfiguration configuration, ILogger<EmailService> logger) : IEmailService
{   
    public async Task SendAsync(string email, string subject, string body)
    {
        try
        {
            var apiToken = configuration.GetValue<string>("MailSettings:ApiToken") ?? throw new Exception("Mailtrap ApiKey not found");
            using var mailtrapClientFactory = new MailtrapClientFactory(apiToken);
            var mailtrapClient = mailtrapClientFactory.CreateClient();
            var request = SendEmailRequest
                .Create()
                .From("no-reply@arahk.com", "BabyLogger")
                .To(email)
                .Subject(subject)
                .Category("Reset Password")
                .Text(body);
            var response = await mailtrapClient
                .Email()
                .Send(request);
        }
        catch (Exception ex)
        {
            logger.LogError("An error occurred while sending email: {Exception}", ex);
        }
    }
}
