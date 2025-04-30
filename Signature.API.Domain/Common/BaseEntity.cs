namespace Signature.API.Domain.Common
{
    public abstract class BaseEntity : IBaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
    }
}
