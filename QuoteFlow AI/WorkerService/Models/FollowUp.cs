namespace WorkerService.Models
{
    public class FollowUp
    {
        public Guid Id { get; set; }
        public Guid QuotationId { get; set; }
        public int FollowUpNumber { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime? SentDate { get; set; }
        public string Channel { get; set; } = "WhatsApp";
        public string Status { get; set; } = "Pending";
        public string? DeliveryStatus { get; set; }
        public bool ResponseReceived { get; set; }
        public string? ExternalMessageId { get; set; }
    }
}