using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace QuoteFlowAI.Services
{
    public class WebhookProcessor
    {
        private readonly IFollowUpService _followUpService;
        private readonly ILogger<WebhookProcessor> _logger;

        public WebhookProcessor(IFollowUpService followUpService, ILogger<WebhookProcessor> logger)
        {
            _followUpService = followUpService;
            _logger = logger;
        }

        public async Task ProcessWhatsAppWebhookAsync(JsonElement payload)
        {
            try
            {
                // Simplified parsing: expect "entry[0].changes[0].value.messages[0]"
                if (payload.TryGetProperty("entry", out var entry) && entry.GetArrayLength() > 0)
                {
                    var changes = entry[0].GetProperty("changes");
                    if (changes.GetArrayLength() > 0)
                    {
                        var value = changes[0].GetProperty("value");
                        if (value.TryGetProperty("messages", out var messages) && messages.GetArrayLength() > 0)
                        {
                            var msg = messages[0];
                            var from = msg.GetProperty("from").GetString();
                            var text = msg.GetProperty("text").GetProperty("body").GetString();
                            var msgId = msg.GetProperty("id").GetString();

                            // Find follow-up by external message ID (need to store mapping)
                            // For demo, we assume we can look up by from phone number and pending status
                            // Real implementation would store ExternalMessageId in FollowUp table
                            _logger.LogInformation($"WhatsApp reply from {from}: {text}");

                            // Process response – find latest pending follow-up for customer
                            // For brevity, we skip lookup; implement as needed.
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing WhatsApp webhook");
            }
        }
    }
}