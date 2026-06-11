using QuoteFlowAI.Models;

namespace QuoteFlowAI.Services
{
    public interface IFollowUpService
    {
        Task GenerateScheduleAsync(Guid quotationId);
        Task<IEnumerable<FollowUp>> GetDueFollowUpsAsync(DateTime asOf);
        Task SendFollowUpAsync(Guid followUpId);
        Task<bool> ProcessResponseAsync(Guid followUpId, string responseText);
        Task UpdateDeliveryStatusAsync(Guid followUpId, string deliveryStatus);
        Task UpdateReadStatusAsync(Guid followUpId, bool isRead);
    }
}