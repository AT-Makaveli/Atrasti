using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Atrasti.API.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<EmailConfiguration> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public EmailConfiguration Options { get; } //set only via Secret Manager

        public Task<bool> SendEmailAsync(string email, string subject, string message, bool isHtml)
        {
            return Execute(Options.Email, Options.Password, email, subject, message, isHtml);
        }

        private async Task<bool> Execute(string fromEmail, string fromPassword, string toEmail, string subject,
            string message, bool isHtml)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(fromEmail, "Atrasti");
            mailMessage.To.Add(new MailAddress(toEmail));
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = isHtml;

            using SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true
            };

            try
            {
                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;
        }
    }
}