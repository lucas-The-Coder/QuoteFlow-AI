using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuoteFlowAI.Data;
using QuoteFlowAI.DTOs;

namespace QuoteFlowAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("metrics")]
        public async Task<IActionResult> GetMetrics()
        {
            var quotesSent = await _context.Quotations.CountAsync();
            var accepted = await _context.Quotations.CountAsync(q => q.Status == "Accepted");
            var rejected = await _context.Quotations.CountAsync(q => q.Status == "Rejected");
            var pending = await _context.Quotations.CountAsync(q => q.Status == "Sent");
            var totalValue = await _context.Quotations.SumAsync(q => q.Amount);
            var wonRevenue = await _context.Quotations.Where(q => q.Status == "Accepted").SumAsync(q => q.Amount);
            var lostRevenue = await _context.Quotations.Where(q => q.Status == "Rejected").SumAsync(q => q.Amount);
            var conversionRate = quotesSent == 0 ? 0 : (double)accepted / quotesSent * 100;

            return Ok(new DashboardDto
            {
                QuotesSent = quotesSent,
                QuotesAccepted = accepted,
                QuotesRejected = rejected,
                PendingQuotes = pending,
                ConversionRate = conversionRate,
                TotalQuoteValue = totalValue,
                WonRevenue = wonRevenue,
                LostRevenue = lostRevenue
            });
        }
    }
}