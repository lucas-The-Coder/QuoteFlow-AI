using Microsoft.Extensions.Logging;
using WorkerService.Data.Repositories;
using WorkerService.Services;

namespace WorkerService.Jobs
{
    public class CheckDueFollowUpsJob
    {
        private readonly IFollowUpRepository _repository;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CheckDueFollowUpsJob> _logger;

        public CheckDueFollowUpsJob(IFollowUpRepository repository, IServiceScopeFactory scopeFactory, ILogger<CheckDueFollowUpsJob> logger)
        {
            _repository = repository;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var due = await _repository.GetDueFollowUpsAsync(DateTime.UtcNow);
            _logger.LogInformation("Found {Count} due follow-ups", due.Count());

            foreach (var followUp in due)
            {
                // Create a new scope for each job to avoid DbContext threading issues
                using var scope = _scopeFactory.CreateScope();
                var sendJob = scope.ServiceProvider.GetRequiredService<SendFollowUpJob>();
                await sendJob.RunAsync(followUp.Id, cancellationToken);
            }
        }
    }
}