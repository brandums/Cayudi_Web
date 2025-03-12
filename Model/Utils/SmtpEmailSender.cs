using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace EPlatformWebApp.Model.Utils
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public SmtpEmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailSettings.UserName);
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(_emailSettings.Host, _emailSettings.Port))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
    }
}
