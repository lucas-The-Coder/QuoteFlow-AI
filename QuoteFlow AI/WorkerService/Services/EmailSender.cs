using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using WorkerService.Configurations;

namespace WorkerService.Services
{
    public class EmailSender : IMessageSender
    {
        private readonly EmailCreds _creds;

        public EmailSender(IOptions<ChannelCredentials> creds)
        {
            _creds = creds.Value.Email;
        }

        public async Task<DeliveryResult> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using var client = new SmtpClient(_creds.SmtpServer, _creds.SmtpPort);
                client.EnableSsl = _creds.EnableSsl;
                client.Credentials = new NetworkCredential(_creds.Username, _creds.Password);
                var mail = new MailMessage
                {
                    From = new MailAddress(_creds.SenderEmail, _creds.SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };
                mail.To.Add(toEmail);
                await client.SendMailAsync(mail);
                return new DeliveryResult { Success = true, Status = "Sent" };
            }
            catch (Exception ex)
            {
                return new DeliveryResult { Success = false, Status = "Failed", ErrorMessage = ex.Message };
            }
        }

        public Task<DeliveryResult> SendWhatsAppAsync(string toPhoneNumber, string message)
        {
            throw new NotImplementedException();
        }
    }
}