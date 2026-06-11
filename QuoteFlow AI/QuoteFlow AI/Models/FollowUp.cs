namespace QuoteFlowAI.Models
{
    public class FollowUp
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid QuotationId { get; set; }
        public int FollowUpNumber { get; set; } // 1,2,3,4
        public DateTime ScheduledDate { get; set; }
        public DateTime? SentDate { get; set; }
        public string Channel { get; set; } = "WhatsApp";
        public string Status { get; set; } = "Pending"; // Pending, Sent, Failed
        public string? DeliveryStatus { get; set; }
        public bool ResponseReceived { get; set; }
        public string? ExternalMessageId { get; set; }

        public Quotation? Quotation { get; set; }
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}