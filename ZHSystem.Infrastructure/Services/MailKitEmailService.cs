using MailKit.Net.Smtp; 
using MailKit.Security;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using MimeKit;
using ZHSystem.Application.Common.Exceptions;
using ZHSystem.Application.Common.Interfaces;
using ZHSystem.Infrastructure.Persistence.Models;


namespace ZHSystem.Infrastructure.Services
{
    
    public class MailKitEmailService : IEmailService
    {
        private readonly SmtpSettings _smtp;
        public MailKitEmailService(IOptions<SmtpSettings> smtp)
        {
            _smtp = smtp.Value;
        }





        public async Task SendAsync(string destination, string subject, string htmlBody)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(
                    _smtp.DisplayName ?? "ZHSystem System",
                    _smtp.From));

                message.To.Add(MailboxAddress.Parse(destination));
                message.Subject = subject;
                message.Body = new TextPart("html") { Text = htmlBody };
                using var client = new SmtpClient();

                await client.ConnectAsync(
                    _smtp.Host,
                    _smtp.Port,
                    SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(
                    _smtp.User,
                    _smtp.Password);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch(Exception ex)
            {
                throw new EmailSendException($"Failed to send email to {destination}",ex);
            }
           
        }
    }
}
