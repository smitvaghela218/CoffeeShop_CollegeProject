using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using CoffeeShop.Models;

namespace CoffeeShop.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(ContactModel contact)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.Email));
            emailMessage.To.Add(new MailboxAddress(contact.Name, contact.Email));
            emailMessage.Subject = contact.Subject;
            emailMessage.Body = new TextPart("plain")
            {
                Text = contact.Message
            };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                smtpClient.Authenticate(_emailSettings.Email, _emailSettings.Password);
                await smtpClient.SendAsync(emailMessage);
                smtpClient.Disconnect(true);
            }
        }
    }
}
