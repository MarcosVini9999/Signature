using Microsoft.AspNetCore.Mvc;
using Signature.API.Application.Interfaces;

namespace Signature.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _svc;

        public SubscriptionsController(ISubscriptionService svc)
        {
            _svc = svc;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromQuery] Guid userId, [FromQuery] Guid planId)
        {
            await _svc.SubscribeAsync(userId, planId);
            return Ok(new { Message = "Subscription successful." });
        }
    }
}
