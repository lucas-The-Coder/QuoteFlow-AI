using QuoteFlowAI.DTOs;
using QuoteFlowAI.Models;

namespace QuoteFlowAI.Services
{
    public interface IQuotationService
    {
        Task<Quotation> CreateQuotationAsync(CreateQuotationDto dto, Guid salesRepId);
        Task<Quotation?> GetQuotationByIdAsync(Guid id);
        Task<IEnumerable<Quotation>> GetAllQuotationsAsync();
        Task<IEnumerable<Quotation>> GetQuotationsByStatusAsync(string status);
        Task<Quotation?> UpdateQuotationAsync(Guid id, UpdateQuotationDto dto);
        Task<bool> DeleteQuotationAsync(Guid id);
        Task<bool> UpdateQuotationStatusAsync(Guid id, string newStatus);
    }
}