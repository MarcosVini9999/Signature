using Microsoft.AspNetCore.Mvc;
using Signature.API.Application.DTOs;
using Signature.API.Application.Interfaces;

namespace Signature.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionPlansController : ControllerBase
    {
        private readonly ISubscriptionPlanService _svc;

        public SubscriptionPlansController(ISubscriptionPlanService svc)
        {
            _svc = svc;
        }

        [HttpPost]
        public async Task<ActionResult<SubscriptionPlanDto>> Create([FromBody] CreateSubscriptionPlanDto dto)
        {
            var plan = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = plan.Id }, plan);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionPlanDto>>> GetAll()
        {
            var plans = await _svc.GetAllAsync();
            return Ok(plans);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionPlanDto>> Get(Guid id)
        {
            var plan = await _svc.GetByIdAsync(id);
            if (plan is null) return NotFound();
            return Ok(plan);
        }
    }
}
