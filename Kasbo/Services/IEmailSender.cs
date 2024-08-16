namespace Kasbo.Services;

public interface IEmailSender
{
    public Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = false);
}