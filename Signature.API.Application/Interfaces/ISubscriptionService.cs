using Signature.API.Application.DTOs;

namespace Signature.API.Application.Interfaces
{
    public interface ISubscriptionService
    {
        Task SubscribeAsync(Guid userId, Guid planId);
        Task<IEnumerable<UserSubscriptionDto>> GetByUserAsync(Guid userId);
    }
}
