using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Kasbo.Models;
using Kasbo.Services;

namespace Kasbo.Services;

public class EmailSender : IEmailSender
{

    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = false)
    {


        var emailConfig = new EmailConfig();
        _configuration.GetSection("Email").Bind(emailConfig);

        using (var message = new MailMessage())
        {
            message.To.Add(new MailAddress(toEmail));
            message.From = new MailAddress(emailConfig.Email);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = isHtml;

            using (var client = new SmtpClient(emailConfig.SmtpServer))
            {
                client.Port = emailConfig.SmtpPort;
                client.Credentials = new NetworkCredential(emailConfig.Email, emailConfig.Password);

                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                await client.SendMailAsync(message);
            }
        }



    }
}