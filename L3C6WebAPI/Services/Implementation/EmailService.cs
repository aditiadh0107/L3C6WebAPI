using L3C6WebAPI.Services.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace L3C6WebAPI.Services.Implementation;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpConfig = configuration.GetSection("Smtp");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(
            smtpConfig["FromName"], smtpConfig["FromEmail"]));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(
            smtpConfig["Host"],
            int.Parse(smtpConfig["Port"]!),
            MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(smtpConfig["Username"], smtpConfig["Password"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
