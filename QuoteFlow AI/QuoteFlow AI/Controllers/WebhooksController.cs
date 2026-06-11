using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuoteFlowAI.Configurations;
using QuoteFlowAI.Services;

namespace QuoteFlowAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhooksController : ControllerBase
    {
        private readonly WhatsAppSettings _whatsAppSettings;
        private readonly WebhookProcessor _processor;

        public WebhooksController(IOptions<WhatsAppSettings> whatsAppSettings, WebhookProcessor processor)
        {
            _whatsAppSettings = whatsAppSettings.Value;
            _processor = processor;
        }

        [HttpGet("whatsapp")]
        public IActionResult VerifyWhatsApp([FromQuery] string hub_mode, [FromQuery] string hub_verify_token, [FromQuery] string hub_challenge)
        {
            if (hub_mode == "subscribe" && hub_verify_token == _whatsAppSettings.WebhookVerifyToken)
                return Ok(hub_challenge);
            return BadRequest();
        }

        [HttpPost("whatsapp")]
        public async Task<IActionResult> HandleWhatsApp([FromBody] JsonElement payload)
        {
            await _processor.ProcessWhatsAppWebhookAsync(payload);
            return Ok();
        }
    }
}