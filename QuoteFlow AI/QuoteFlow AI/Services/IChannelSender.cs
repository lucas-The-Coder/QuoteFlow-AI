namespace QuoteFlowAI.Services
{
    public interface IChannelSender
    {
        Task<DeliveryResult> SendAsync(MessageRequest request);
    }

    public class MessageRequest
    {
        public string To { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
    }

    public class DeliveryResult
    {
        public bool Success { get; set; }
        public string? ExternalMessageId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }
    }
}