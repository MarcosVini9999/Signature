using Signature.API.Domain.Common;

namespace Signature.API.Domain.Entities
{
    public class UserSubscription : BaseEntity
    {
        public Guid UserId { get; private set; }
        public Guid SubscriptionPlanId { get; private set; }
        public DateTime SubscribedAt { get; private set; }

        private UserSubscription() { }

        public UserSubscription(Guid userId, Guid planId)
        {
            UserId = userId;
            SubscriptionPlanId = planId;
            SubscribedAt = DateTime.UtcNow;
        }
    }
}
