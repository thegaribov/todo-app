using MimeKit;
using System.Net.Mail;
using TodoListApp.Dtos;

namespace TodoListApp.Services;

public interface IEmailService
{
    void SendEmail(EmailMessageDto message);
    void SendEmail(string to, string subject, string content);
}
