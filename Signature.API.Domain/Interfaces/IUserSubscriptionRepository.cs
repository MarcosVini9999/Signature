using Signature.API.Domain.Entities;

namespace Signature.API.Domain.Interfaces
{
    public interface IUserSubscriptionRepository : IBaseRepository<UserSubscription>
    {
        Task<IEnumerable<UserSubscription>> GetByUserIdAsync(Guid userId);
    }
}
