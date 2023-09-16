using DotNetEnv;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NetCorePress.Services.Messages
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService()
        {
            Env.Load();

            _smtpClient = new SmtpClient(Env.GetString("SMTP_HOST"), Env.GetInt("SMTP_PORT"))
            {
                Credentials = new NetworkCredential(Env.GetString("SMTP_USERNAME"), Env.GetString("SMTP_PASSWORD")),
                EnableSsl = true, // Set to 'true' if you are using SSL/TLS
            };
        }

        public async Task<bool> SendEmailAsync(string from, string to, string subject, string body, bool isHtml = false)
        {
            try
            {
                // Creates the message
                var message = new MailMessage
                {
                    From = new MailAddress(from),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml,
                };

                // Add the receiver
                message.To.Add(to);

                // Send the email
                await _smtpClient.SendMailAsync(message);

                return true; // Email successfully sent
            }
            catch (Exception)
            {
                return false; // Error sending email
            }
        }
    }
}