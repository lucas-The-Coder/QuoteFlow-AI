namespace WorkerService.Models
{
    public class Quotation
    {
        public Guid Id { get; set; }
        public string QuoteNumber { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime QuoteDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid SalesRepresentativeId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}