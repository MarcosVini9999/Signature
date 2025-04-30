using Microsoft.EntityFrameworkCore;
using Signature.API.Domain.Entities;
using Signature.API.Domain.Interfaces;
using Signature.API.Infra.Data.Context;

namespace Signature.API.Infra.Data.Repositories
{
    public class UserSubscriptionRepository : BaseRepository<UserSubscription>, IUserSubscriptionRepository
    {
        public UserSubscriptionRepository(AppDbContext ctx)
            : base(ctx)
        {
        }

        public async Task<IEnumerable<UserSubscription>> GetByUserIdAsync(Guid userId) =>
            await _ctx.UserSubscriptions
                .Where(us => us.UserId == userId)
                .ToListAsync();
    }
}
