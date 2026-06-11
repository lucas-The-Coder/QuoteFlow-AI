using Microsoft.EntityFrameworkCore;
using QuoteFlowAI.Data;
using QuoteFlowAI.Models;

namespace QuoteFlowAI.Repositories
{
    public class QuotationRepository : IQuotationRepository
    {
        private readonly ApplicationDbContext _context;

        public QuotationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Quotation?> GetByIdAsync(Guid id)
        {
            return await _context.Quotations
                .Include(q => q.Customer)
                .Include(q => q.FollowUps)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<IEnumerable<Quotation>> GetAllAsync()
        {
            return await _context.Quotations
                .Include(q => q.Customer)
                .OrderByDescending(q => q.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Quotation>> GetByStatusAsync(string status)
        {
            return await _context.Quotations
                .Where(q => q.Status == status)
                .Include(q => q.Customer)
                .ToListAsync();
        }

        public async Task<IEnumerable<Quotation>> GetBySalesRepresentativeAsync(Guid salesRepId)
        {
            return await _context.Quotations
                .Where(q => q.SalesRepresentativeId == salesRepId)
                .Include(q => q.Customer)
                .ToListAsync();
        }

        public async Task<Quotation> CreateAsync(Quotation quotation)
        {
            _context.Quotations.Add(quotation);
            await _context.SaveChangesAsync();
            return quotation;
        }

        public async Task<Quotation> UpdateAsync(Quotation quotation)
        {
            _context.Quotations.Update(quotation);
            await _context.SaveChangesAsync();
            return quotation;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var quote = await _context.Quotations.FindAsync(id);
            if (quote == null) return false;
            _context.Quotations.Remove(quote);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(string quoteNumber)
        {
            return await _context.Quotations.AnyAsync(q => q.QuoteNumber == quoteNumber);
        }
    }
}