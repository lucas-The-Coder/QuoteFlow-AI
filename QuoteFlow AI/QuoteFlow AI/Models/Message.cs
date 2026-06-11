namespace QuoteFlowAI.Models
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FollowUpId { get; set; }
        public string MessageContent { get; set; } = string.Empty;
        public string? DeliveryStatus { get; set; }
        public bool ReadStatus { get; set; }
        public bool ReplyStatus { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public FollowUp? FollowUp { get; set; }
    }
}