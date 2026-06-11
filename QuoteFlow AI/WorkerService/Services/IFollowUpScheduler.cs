using WorkerService.Models;

namespace WorkerService.Services
{
    public interface IFollowUpScheduler
    {
        Task GenerateScheduleAsync(Guid quotationId, DateTime quoteDate);
        Task<IEnumerable<FollowUp>> GetDueFollowUpsAsync(DateTime asOf);
    }
}