using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using WorkerService.Configurations;

namespace WorkerService.Services
{
    public class WhatsAppSender : IMessageSender
    {
        private readonly HttpClient _httpClient;
        private readonly WhatsAppCreds _creds;

        public WhatsAppSender(HttpClient httpClient, IOptions<ChannelCredentials> creds)
        {
            _httpClient = httpClient;
            _creds = creds.Value.WhatsApp;
        }

        public async Task<DeliveryResult> SendWhatsAppAsync(string toPhoneNumber, string message)
        {
            try
            {
                var url = $"{_creds.ApiBaseUrl}/{_creds.PhoneNumberId}/messages";
                var payload = new
                {
                    messaging_product = "whatsapp",
                    to = toPhoneNumber,
                    type = "text",
                    text = new { body = message }
                };
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _creds.AccessToken);
                var response = await _httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var doc = JsonDocument.Parse(responseString);
                    var msgId = doc.RootElement.GetProperty("messages")[0].GetProperty("id").GetString();
                    return new DeliveryResult { Success = true, ExternalMessageId = msgId, Status = "Sent" };
                }
                return new DeliveryResult { Success = false, Status = "Failed", ErrorMessage = responseString };
            }
            catch (Exception ex)
            {
                return new DeliveryResult { Success = false, Status = "Error", ErrorMessage = ex.Message };
            }
        }

        public Task<DeliveryResult> SendEmailAsync(string toEmail, string subject, string body)
        {
            throw new NotImplementedException();
        }
    }
}