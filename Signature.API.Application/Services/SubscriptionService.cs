using Signature.API.Application.Interfaces;
using Signature.API.Domain.Entities;
using Signature.API.Domain.Interfaces;
using MassTransit;
using Signature.API.Domain.Events;
using Signature.API.Application.DTOs;

namespace Signature.API.Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUserSubscriptionRepository _repo;
        private readonly IPublishEndpoint _publishEndpoint;

        public SubscriptionService(
            IUserSubscriptionRepository repo,
            IPublishEndpoint publishEndpoint)
        {
            _repo = repo;
            _publishEndpoint = publishEndpoint;
        }

        public async Task SubscribeAsync(Guid userId, Guid planId)
        {
            var subscription = new UserSubscription(userId, planId);
            await _repo.AddAsync(subscription);

            var @event = new UserSubscribed(
                userId,
                planId,
                subscription.SubscribedAt);

            await _publishEndpoint.Publish(@event);
        }

        public async Task<IEnumerable<UserSubscriptionDto>> GetByUserAsync(Guid userId)
        {
            var subs = await _repo.GetByUserIdAsync(userId);
            return subs.Select(s =>
                new UserSubscriptionDto(
                    s.UserId,
                    s.SubscriptionPlanId,
                    s.SubscribedAt));
        }
    }
}
