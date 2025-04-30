using Signature.API.Domain.Common;
using Signature.API.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Signature.API.Infra.Data.Context;

namespace Signature.API.Infra.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : class, IBaseEntity
    {
        protected readonly AppDbContext _ctx;
        protected readonly DbSet<T> _db;

        public BaseRepository(AppDbContext ctx)
        {
            _ctx = ctx;
            _db = ctx.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _db.AddAsync(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _db.ToListAsync();

        public async Task<T?> GetByIdAsync(Guid id) =>
            await _db.FindAsync(id);

        public void Remove(T entity)
        {
            _db.Remove(entity);
            _ctx.SaveChanges();
        }

        public void Update(T entity)
        {
            _db.Update(entity);
            _ctx.SaveChanges();
        }
    }
}
