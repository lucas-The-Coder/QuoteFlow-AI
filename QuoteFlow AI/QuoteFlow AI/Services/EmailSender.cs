using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using QuoteFlowAI.Configurations;

namespace QuoteFlowAI.Services
{
    public class EmailSender : IChannelSender
    {
        private readonly EmailSettings _settings;

        public EmailSender(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<DeliveryResult> SendAsync(MessageRequest request)
        {
            try
            {
                using var client = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort);
                client.EnableSsl = _settings.EnableSsl;
                client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
                var mail = new MailMessage
                {
                    From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                    Subject = "QuoteFlow AI - Follow Up",
                    Body = request.Body,
                    IsBodyHtml = false
                };
                mail.To.Add(request.To);
                await client.SendMailAsync(mail);
                return new DeliveryResult { Success = true, Status = "Sent" };
            }
            catch (Exception ex)
            {
                return new DeliveryResult { Success = false, Status = "Failed", ErrorMessage = ex.Message };
            }
        }
    }
}