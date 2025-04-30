using Signature.API.Domain.Common;

namespace Signature.API.Domain.Entities
{
    public class SubscriptionPlan : BaseEntity
    {
        public string Title { get; private set; }
        public decimal Price { get; private set; }

        private SubscriptionPlan() { }

        public SubscriptionPlan(string title, decimal price)
        {
            Title = title;
            Price = price;
        }
    }
}
