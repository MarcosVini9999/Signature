namespace Signature.API.Domain.Events
{
    public record UserSubscribed(Guid UserId, Guid PlanId, DateTime SubscribedAt);
}
