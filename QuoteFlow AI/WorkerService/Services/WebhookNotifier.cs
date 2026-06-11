using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WorkerService.Configurations;

namespace WorkerService.Services
{
    public class WebhookNotifier
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly ILogger<WebhookNotifier> _logger;

        public WebhookNotifier(HttpClient httpClient, IOptions<WorkerSettings> settings, ILogger<WebhookNotifier> logger)
        {
            _httpClient = httpClient;
            _apiBaseUrl = settings.Value.ApiBaseUrl ?? "http://localhost:5000";
            _logger = logger;
        }

        public async Task NotifyDeliveryUpdateAsync(Guid followUpId, string deliveryStatus, string externalMessageId)
        {
            var payload = new { followUpId, deliveryStatus, externalMessageId };
            await SendAsync("/api/webhooks/delivery", payload);
        }

        private async Task SendAsync(string endpoint, object payload)
        {
            try
            {
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync($"{_apiBaseUrl}{endpoint}", content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send webhook to {Endpoint}", endpoint);
            }
        }
    }
}