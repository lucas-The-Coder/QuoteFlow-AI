using WorkerService.Data.Repositories;
using WorkerService.Models;

namespace WorkerService.Services
{
    public class FollowUpScheduler : IFollowUpScheduler
    {
        private readonly IFollowUpRepository _repository;

        public FollowUpScheduler(IFollowUpRepository repository)
        {
            _repository = repository;
        }

        public async Task GenerateScheduleAsync(Guid quotationId, DateTime quoteDate)
        {
            var days = new[] { 3, 7, 14, 21 };
            for (int i = 0; i < days.Length; i++)
            {
                var followUp = new FollowUp
                {
                    QuotationId = quotationId,
                    FollowUpNumber = i + 1,
                    ScheduledDate = quoteDate.AddDays(days[i]),
                    Status = "Pending",
                    Channel = "WhatsApp" // Could be configurable
                };
                await _repository.CreateAsync(followUp);
            }
        }

        public async Task<IEnumerable<FollowUp>> GetDueFollowUpsAsync(DateTime asOf)
        {
            return await _repository.GetDueFollowUpsAsync(asOf);
        }
    }
}