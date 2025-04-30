using Signature.API.Application.DTOs;

namespace Signature.API.Application.Interfaces
{
    public interface ISubscriptionPlanService
    {
        Task<SubscriptionPlanDto> CreateAsync(CreateSubscriptionPlanDto dto);
        Task<IEnumerable<SubscriptionPlanDto>> GetAllAsync();
        Task<SubscriptionPlanDto?> GetByIdAsync(Guid id);
    }
}
