namespace WorkerService.Services
{
    public interface IMessageSender
    {
        Task<DeliveryResult> SendWhatsAppAsync(string toPhoneNumber, string message);
        Task<DeliveryResult> SendEmailAsync(string toEmail, string subject, string body);
    }

    public class DeliveryResult
    {
        public bool Success { get; set; }
        public string? ExternalMessageId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }
    }
}