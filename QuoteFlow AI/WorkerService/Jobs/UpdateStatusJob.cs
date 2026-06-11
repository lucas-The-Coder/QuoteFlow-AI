using Microsoft.Extensions.Logging;
using WorkerService.Data.Repositories;

namespace WorkerService.Jobs
{
    public class UpdateStatusJob
    {
        private readonly IFollowUpRepository _repository;
        private readonly ILogger<UpdateStatusJob> _logger;

        public UpdateStatusJob(IFollowUpRepository repository, ILogger<UpdateStatusJob> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            // For demo, we could mark follow-ups that are older than 30 days without response as "Expired"
            // But implement as needed
            _logger.LogInformation("UpdateStatusJob ran at {Time}", DateTime.UtcNow);
        }
    }
}