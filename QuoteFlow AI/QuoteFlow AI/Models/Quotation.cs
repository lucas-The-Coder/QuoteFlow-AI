namespace QuoteFlowAI.Models
{
    public class Quotation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string QuoteNumber { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime QuoteDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Draft"; // Draft, Sent, Accepted, Rejected, Expired
        public Guid SalesRepresentativeId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Customer? Customer { get; set; }
        public User? SalesRepresentative { get; set; }
        public ICollection<FollowUp> FollowUps { get; set; } = new List<FollowUp>();
    }
}