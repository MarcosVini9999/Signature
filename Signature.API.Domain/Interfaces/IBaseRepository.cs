using Signature.API.Domain.Common;

namespace Signature.API.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : IBaseEntity
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
