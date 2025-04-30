namespace Signature.API.Application.DTOs
{
    public record UserSubscriptionDto(Guid UserId, Guid PlanId, DateTime SubscribedAt);
}
