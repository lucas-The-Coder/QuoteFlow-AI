using Microsoft.EntityFrameworkCore;
using QuoteFlowAI.Data;
using QuoteFlowAI.Helpers;
using QuoteFlowAI.Models;

namespace QuoteFlowAI.Services
{
    public class FollowUpService : IFollowUpService
    {
        private readonly ApplicationDbContext _context;
        private readonly IChannelSender _whatsAppSender;
        private readonly EmailSender _emailSender;
        private readonly TemplateHelper _templateHelper;

        public FollowUpService(ApplicationDbContext context, IChannelSender whatsAppSender, EmailSender emailSender, TemplateHelper templateHelper)
        {
            _context = context;
            _whatsAppSender = whatsAppSender;
            _emailSender = emailSender;
            _templateHelper = templateHelper;
        }

        public async Task GenerateScheduleAsync(Guid quotationId)
        {
            var quote = await _context.Quotations.FindAsync(quotationId);
            if (quote == null) return;

            var followUps = new List<FollowUp>();
            var scheduledDays = new[] { 3, 7, 14, 21 };
            for (int i = 0; i < scheduledDays.Length; i++)
            {
                followUps.Add(new FollowUp
                {
                    QuotationId = quotationId,
                    FollowUpNumber = i + 1,
                    ScheduledDate = quote.QuoteDate.AddDays(scheduledDays[i]),
                    Status = "Pending",
                    Channel = "WhatsApp"
                });
            }
            await _context.FollowUps.AddRangeAsync(followUps);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FollowUp>> GetDueFollowUpsAsync(DateTime asOf)
        {
            return await _context.FollowUps
                .Include(f => f.Quotation)
                    .ThenInclude(q => q!.Customer)
                .Where(f => f.ScheduledDate <= asOf && f.Status == "Pending")
                .ToListAsync();
        }

        public async Task SendFollowUpAsync(Guid followUpId)
        {
            var followUp = await _context.FollowUps
                .Include(f => f.Quotation)
                    .ThenInclude(q => q!.Customer)
                .FirstOrDefaultAsync(f => f.Id == followUpId);

            if (followUp == null || followUp.Status != "Pending")
                return;

            var quotation = followUp.Quotation;
            if (quotation == null) return;

            var customer = quotation.Customer;
            if (customer == null) return;

            var message = _templateHelper.BuildFollowUpMessage(customer.CustomerName, followUp.FollowUpNumber);
            DeliveryResult result;

            if (followUp.Channel == "WhatsApp")
                result = await _whatsAppSender.SendAsync(new MessageRequest { To = customer.PhoneNumber, Body = message, Channel = "WhatsApp" });
            else
                result = await _emailSender.SendAsync(new MessageRequest { To = customer.Email, Body = message, Channel = "Email" });

            followUp.SentDate = DateTime.UtcNow;
            followUp.Status = result.Success ? "Sent" : "Failed";
            followUp.DeliveryStatus = result.Status;
            followUp.ExternalMessageId = result.ExternalMessageId;
            await _context.SaveChangesAsync();

            var msg = new Message
            {
                FollowUpId = followUp.Id,
                MessageContent = message,
                DeliveryStatus = result.Status,
                CreatedDate = DateTime.UtcNow
            };
            await _context.Messages.AddAsync(msg);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ProcessResponseAsync(Guid followUpId, string responseText)
        {
            var followUp = await _context.FollowUps.FindAsync(followUpId);
            if (followUp == null) return false;

            followUp.ResponseReceived = true;
            await _context.SaveChangesAsync();

            var msg = await _context.Messages.FirstOrDefaultAsync(m => m.FollowUpId == followUpId);
            if (msg != null)
            {
                msg.ReplyStatus = true;
                await _context.SaveChangesAsync();
            }

            var lowerResponse = responseText.ToLower();
            if (lowerResponse.Contains("accept") || lowerResponse.Contains("yes") || lowerResponse.Contains("proceed"))
            {
                var quote = await _context.Quotations.FindAsync(followUp.QuotationId);
                if (quote != null)
                {
                    quote.Status = "Accepted";
                    await _context.SaveChangesAsync();
                }
            }
            else if (lowerResponse.Contains("reject") || lowerResponse.Contains("no") || lowerResponse.Contains("cancel"))
            {
                var quote = await _context.Quotations.FindAsync(followUp.QuotationId);
                if (quote != null)
                {
                    quote.Status = "Rejected";
                    await _context.SaveChangesAsync();
                }
            }

            return true;
        }

        public async Task UpdateDeliveryStatusAsync(Guid followUpId, string deliveryStatus)
        {
            var followUp = await _context.FollowUps.FindAsync(followUpId);
            if (followUp != null)
            {
                followUp.DeliveryStatus = deliveryStatus;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateReadStatusAsync(Guid followUpId, bool isRead)
        {
            var msg = await _context.Messages.FirstOrDefaultAsync(m => m.FollowUpId == followUpId);
            if (msg != null)
            {
                msg.ReadStatus = isRead;
                await _context.SaveChangesAsync();
            }
        }
    }
}