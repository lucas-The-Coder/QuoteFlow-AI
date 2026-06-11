using System.Text;

namespace QuoteFlowAI.Helpers
{
    public class TemplateHelper
    {
        public string BuildFollowUpMessage(string customerName, int followUpNumber)
        {
            return followUpNumber switch
            {
                1 => $"Hi {customerName}, just checking whether you had a chance to review the quotation we sent. Please let us know if you have any questions.",
                2 => $"Hi {customerName}, we wanted to follow up regarding your quotation. We'd be happy to assist with any questions or adjustments.",
                3 => $"Hi {customerName}, just following up on the quotation we provided. Please let us know if you would like to proceed.",
                4 => $"Hi {customerName}, this is our final follow-up regarding your quotation. Please contact us if you would like to discuss the project further.",
                _ => $"Hi {customerName}, please review your quotation at your earliest convenience."
            };
        }
    }
}