using WorkerService.Models;

namespace WorkerService.Data.Repositories
{
    public interface IFollowUpRepository
    {
        Task<FollowUp?> GetByIdAsync(Guid id);
        Task<IEnumerable<FollowUp>> GetDueFollowUpsAsync(DateTime asOf);
        Task<FollowUp> CreateAsync(FollowUp followUp);
        Task<FollowUp> UpdateAsync(FollowUp followUp);
        Task UpdateStatusAsync(Guid id, string status);
        Task UpdateDeliveryStatusAsync(Guid id, string deliveryStatus);
        Task MarkAsSentAsync(Guid id, DateTime sentDate, string externalMessageId);
    }
}