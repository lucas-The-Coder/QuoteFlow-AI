namespace QuoteFlowAI.DTOs
{
    public class CreateQuotationDto
    {
        public string QuoteNumber { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime QuoteDate { get; set; }
    }
}