using QuoteFlowAI.Models;

namespace QuoteFlowAI.Repositories
{
    public interface IQuotationRepository
    {
        Task<Quotation?> GetByIdAsync(Guid id);
        Task<IEnumerable<Quotation>> GetAllAsync();
        Task<IEnumerable<Quotation>> GetByStatusAsync(string status);
        Task<IEnumerable<Quotation>> GetBySalesRepresentativeAsync(Guid salesRepId);
        Task<Quotation> CreateAsync(Quotation quotation);
        Task<Quotation> UpdateAsync(Quotation quotation);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(string quoteNumber);
    }
}