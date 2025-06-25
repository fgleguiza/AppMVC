using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace app.Service
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly bool _smtpEnableSsl;
        private readonly string _senderEmail;
        private readonly string _senderName;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;

            _smtpServer = _configuration["EmailSettings:SmtpServer"];
            _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            _smtpEnableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"] ?? "true");
            _senderEmail = _configuration["EmailSettings:SenderEmail"];
            _senderName = _configuration["EmailSettings:SenderName"];
            _smtpUsername = _configuration["EmailSettings:SmtpUsername"];
            _smtpPassword = _configuration["EmailSettings:SmtpPassword"];
        }

        public async Task SendEmailAsync(string toEmail, string subject, string bodyHtml)
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(_senderEmail, _senderName);
                message.To.Add(toEmail);
                message.Subject = subject;
                message.Body = bodyHtml;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient(_smtpServer, _smtpPort))
                {
                    smtp.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                    smtp.EnableSsl = _smtpEnableSsl;

                    await smtp.SendMailAsync(message);
                }
            }
        }
    }
}
