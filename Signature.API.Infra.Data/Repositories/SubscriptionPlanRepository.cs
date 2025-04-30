using Signature.API.Domain.Entities;
using Signature.API.Domain.Interfaces;
using Signature.API.Infra.Data.Context;

namespace Signature.API.Infra.Data.Repositories
{
    public class SubscriptionPlanRepository : BaseRepository<SubscriptionPlan>, ISubscriptionPlanRepository
    {
        public SubscriptionPlanRepository(AppDbContext ctx)
            : base(ctx)
        {
        }
    }
}
