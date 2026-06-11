using QuoteFlowAI.DTOs;
using QuoteFlowAI.Models;
using QuoteFlowAI.Repositories;

namespace QuoteFlowAI.Services
{
    public class QuotationService : IQuotationService
    {
        private readonly IQuotationRepository _quotationRepository;
        private readonly IFollowUpService _followUpService;

        public QuotationService(IQuotationRepository quotationRepository, IFollowUpService followUpService)
        {
            _quotationRepository = quotationRepository;
            _followUpService = followUpService;
        }

        public async Task<Quotation> CreateQuotationAsync(CreateQuotationDto dto, Guid salesRepId)
        {
            if (await _quotationRepository.ExistsAsync(dto.QuoteNumber))
                throw new InvalidOperationException("Quote number already exists");

            var quotation = new Quotation
            {
                QuoteNumber = dto.QuoteNumber,
                CustomerId = dto.CustomerId,
                Amount = dto.Amount,
                Description = dto.Description,
                QuoteDate = dto.QuoteDate,
                SalesRepresentativeId = salesRepId,
                Status = "Sent"
            };

            var created = await _quotationRepository.CreateAsync(quotation);

            // Generate follow-up schedule
            await _followUpService.GenerateScheduleAsync(created.Id);

            return created;
        }

        public async Task<Quotation?> GetQuotationByIdAsync(Guid id)
        {
            return await _quotationRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Quotation>> GetAllQuotationsAsync()
        {
            return await _quotationRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Quotation>> GetQuotationsByStatusAsync(string status)
        {
            return await _quotationRepository.GetByStatusAsync(status);
        }

        public async Task<Quotation?> UpdateQuotationAsync(Guid id, UpdateQuotationDto dto)
        {
            var quote = await _quotationRepository.GetByIdAsync(id);
            if (quote == null) return null;

            quote.Amount = dto.Amount;
            quote.Description = dto.Description;
            quote.Status = dto.Status;

            return await _quotationRepository.UpdateAsync(quote);
        }

        public async Task<bool> DeleteQuotationAsync(Guid id)
        {
            return await _quotationRepository.DeleteAsync(id);
        }

        public async Task<bool> UpdateQuotationStatusAsync(Guid id, string newStatus)
        {
            var quote = await _quotationRepository.GetByIdAsync(id);
            if (quote == null) return false;

            quote.Status = newStatus;
            await _quotationRepository.UpdateAsync(quote);
            return true;
        }
    }
}