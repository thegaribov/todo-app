using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using TodoListApp.Configs;
using TodoListApp.Dtos;

namespace TodoListApp.Services;

public class MailKitEmailSender : IEmailService
{
    private readonly EmailConfiguration _emailConfig;

    public MailKitEmailSender(IOptions<EmailConfiguration> emailConfigOptions)
    {
        _emailConfig = emailConfigOptions.Value;
    }

    public void SendEmail(string to, string subject, string content)
    {
        SendEmail(new EmailMessageDto(new List<string> { to }, subject, content));
    }

    public void SendEmail(EmailMessageDto message)
    {
        var emailMessage = CreateEmailMessage(message);
        Send(emailMessage);
    }

    private MimeMessage CreateEmailMessage(EmailMessageDto message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailConfig.Username, _emailConfig.From));
        emailMessage.To.AddRange(message.To.Select(t => new MailboxAddress(t, t)));
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
        return emailMessage;
    }

    private void Send(MimeMessage mailMessage)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.Authenticate(_emailConfig.Username, _emailConfig.Password, default);

                client.Send(mailMessage);

            }
            catch
            {
                throw;
            }
            finally
            {
                client.Disconnect(true);
            }

        }
    }
}
