using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WorkerService.Configurations;
using WorkerService.Data;
using WorkerService.Data.Repositories;
using WorkerService.Services;

namespace WorkerService.Jobs
{
    public class SendFollowUpJob
    {
        private readonly IFollowUpRepository _followUpRepository;
        private readonly WorkerDbContext _context;
        private readonly IMessageSender _whatsAppSender;
        private readonly EmailSender _emailSender;
        private readonly WorkerSettings _settings;
        private readonly ILogger<SendFollowUpJob> _logger;

        public SendFollowUpJob(IFollowUpRepository followUpRepository, WorkerDbContext context, IMessageSender whatsAppSender, EmailSender emailSender, IOptions<WorkerSettings> settings, ILogger<SendFollowUpJob> logger)
        {
            _followUpRepository = followUpRepository;
            _context = context;
            _whatsAppSender = whatsAppSender;
            _emailSender = emailSender;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task RunAsync(Guid followUpId, CancellationToken cancellationToken)
        {
            var followUp = await _followUpRepository.GetByIdAsync(followUpId);
            if (followUp == null || followUp.Status != "Pending") return;

            // Load quotation and customer
            var quotation = await _context.Quotations.FindAsync(followUp.QuotationId);
            if (quotation == null) return;
            var customer = await _context.Customers.FindAsync(quotation.CustomerId);
            if (customer == null) return;

            var message = BuildMessage(customer.CustomerName, followUp.FollowUpNumber);
            DeliveryResult result;
            var channel = _settings.DefaultChannel;

            if (channel == "WhatsApp" && _settings.EnableWhatsApp)
                result = await _whatsAppSender.SendWhatsAppAsync(customer.PhoneNumber, message);
            else if (channel == "Email" && _settings.EnableEmail)
                result = await _emailSender.SendEmailAsync(customer.Email, "QuoteFlow AI Follow-up", message);
            else
                return;

            if (result.Success)
            {
                await _followUpRepository.MarkAsSentAsync(followUpId, DateTime.UtcNow, result.ExternalMessageId ?? string.Empty);
                _logger.LogInformation("Sent follow-up {FollowUpId} to {CustomerName}", followUpId, customer.CustomerName);
            }
            else
            {
                await _followUpRepository.UpdateStatusAsync(followUpId, "Failed");
                _logger.LogWarning("Failed to send follow-up {FollowUpId}: {Error}", followUpId, result.ErrorMessage);
            }
        }

        private string BuildMessage(string customerName, int followUpNumber)
        {
            return followUpNumber switch
            {
                1 => $"Hi {customerName}, just checking whether you had a chance to review the quotation we sent. Please let us know if you have any questions.",
                2 => $"Hi {customerName}, we wanted to follow up regarding your quotation. We'd be happy to assist with any questions or adjustments.",
                3 => $"Hi {customerName}, just following up on the quotation we provided. Please let us know if you would like to proceed.",
                4 => $"Hi {customerName}, this is our final follow-up regarding your quotation. Please contact us if you would like to discuss the project further.",
                _ => $"Hi {customerName}, please review your quotation."
            };
        }
    }
}