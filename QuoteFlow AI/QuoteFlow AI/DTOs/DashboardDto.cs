namespace QuoteFlowAI.DTOs
{
    public class DashboardDto
    {
        public int QuotesSent { get; set; }
        public int QuotesAccepted { get; set; }
        public int QuotesRejected { get; set; }
        public int PendingQuotes { get; set; }
        public double ConversionRate { get; set; }
        public decimal TotalQuoteValue { get; set; }
        public decimal WonRevenue { get; set; }
        public decimal LostRevenue { get; set; }
    }
}