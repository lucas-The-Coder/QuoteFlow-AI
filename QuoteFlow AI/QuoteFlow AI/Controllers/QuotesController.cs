using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuoteFlowAI.DTOs;
using QuoteFlowAI.Services;
using System.Security.Claims;

namespace QuoteFlowAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuotesController : ControllerBase
    {
        private readonly IQuotationService _quotationService;

        public QuotesController(IQuotationService quotationService)
        {
            _quotationService = quotationService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateQuotationDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var quote = await _quotationService.CreateQuotationAsync(dto, Guid.Parse(userId));
            return CreatedAtAction(nameof(GetById), new { id = quote.Id }, quote);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? status)
        {
            if (!string.IsNullOrEmpty(status))
                return Ok(await _quotationService.GetQuotationsByStatusAsync(status));
            return Ok(await _quotationService.GetAllQuotationsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var quote = await _quotationService.GetQuotationByIdAsync(id);
            if (quote == null) return NotFound();
            return Ok(quote);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateQuotationDto dto)
        {
            var updated = await _quotationService.UpdateQuotationAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _quotationService.DeleteQuotationAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}