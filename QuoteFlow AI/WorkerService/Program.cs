using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WorkerService.Configurations;
using WorkerService.Data;
using WorkerService.Data.Repositories;
using WorkerService.Jobs;
using WorkerService.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;
        services.Configure<WorkerSettings>(config.GetSection("WorkerSettings"));
        services.Configure<ChannelCredentials>(config.GetSection("ChannelCredentials"));

        services.AddDbContext<WorkerDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("PostgreSql")));

        services.AddScoped<IFollowUpRepository, FollowUpRepository>();
        services.AddScoped<IFollowUpScheduler, FollowUpScheduler>();
        services.AddScoped<IMessageSender, WhatsAppSender>();
        services.AddScoped<EmailSender>();
        services.AddScoped<WebhookNotifier>();

        services.AddTransient<CheckDueFollowUpsJob>();
        services.AddTransient<SendFollowUpJob>();
        services.AddTransient<UpdateStatusJob>();

        services.AddHttpClient();

        services.AddHostedService<WorkerBackgroundService>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

await host.RunAsync();

public class WorkerBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly WorkerSettings _settings;
    private readonly ILogger<WorkerBackgroundService> _logger;

    public WorkerBackgroundService(IServiceScopeFactory scopeFactory, IOptions<WorkerSettings> settings, ILogger<WorkerBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _settings = settings.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var checkJob = scope.ServiceProvider.GetRequiredService<CheckDueFollowUpsJob>();
                await checkJob.RunAsync(stoppingToken);

                var updateJob = scope.ServiceProvider.GetRequiredService<UpdateStatusJob>();
                await updateJob.RunAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in worker background loop");
            }

            await Task.Delay(TimeSpan.FromSeconds(_settings.CheckIntervalSeconds), stoppingToken);
        }
    }
}