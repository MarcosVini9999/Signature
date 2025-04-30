using Signature.API.Application.DTOs;
using Signature.API.Application.Interfaces;
using Signature.API.Domain.Entities;
using Signature.API.Domain.Interfaces;

namespace Signature.API.Application.Services
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        private readonly ISubscriptionPlanRepository _repo;

        public SubscriptionPlanService(ISubscriptionPlanRepository repo)
        {
            _repo = repo;
        }

        public async Task<SubscriptionPlanDto> CreateAsync(CreateSubscriptionPlanDto dto)
        {
            var plan = new SubscriptionPlan(dto.Title, dto.Price);
            await _repo.AddAsync(plan);

            return new SubscriptionPlanDto(plan.Id, plan.Title, plan.Price);
        }

        public async Task<IEnumerable<SubscriptionPlanDto>> GetAllAsync()
        {
            var plans = await _repo.GetAllAsync();
            return plans.Select(p => new SubscriptionPlanDto(p.Id, p.Title, p.Price));
        }

        public async Task<SubscriptionPlanDto?> GetByIdAsync(Guid id)
        {
            var plan = await _repo.GetByIdAsync(id);
            return plan is null ? null : new SubscriptionPlanDto(plan.Id, plan.Title, plan.Price);
        }
    }
}
