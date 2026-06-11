namespace QuoteFlowAI.DTOs
{
    public class UpdateQuotationDto
    {
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}