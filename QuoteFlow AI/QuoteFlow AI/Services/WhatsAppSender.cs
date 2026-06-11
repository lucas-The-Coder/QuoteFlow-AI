using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using QuoteFlowAI.Configurations;

namespace QuoteFlowAI.Services
{
    public class WhatsAppSender : IChannelSender
    {
        private readonly HttpClient _httpClient;
        private readonly WhatsAppSettings _settings;

        public WhatsAppSender(HttpClient httpClient, IOptions<WhatsAppSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<DeliveryResult> SendAsync(MessageRequest request)
        {
            try
            {
                var url = $"{_settings.ApiBaseUrl}/{_settings.PhoneNumberId}/messages";
                var payload = new
                {
                    messaging_product = "whatsapp",
                    to = request.To,
                    type = "text",
                    text = new { body = request.Body }
                };
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _settings.AccessToken);
                var response = await _httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var doc = JsonDocument.Parse(responseString);
                    var msgId = doc.RootElement.GetProperty("messages")[0].GetProperty("id").GetString();
                    return new DeliveryResult
                    {
                        Success = true,
                        ExternalMessageId = msgId,
                        Status = "Sent"
                    };
                }
                return new DeliveryResult { Success = false, Status = "Failed", ErrorMessage = responseString };
            }
            catch (Exception ex)
            {
                return new DeliveryResult { Success = false, Status = "Error", ErrorMessage = ex.Message };
            }
        }
    }
}