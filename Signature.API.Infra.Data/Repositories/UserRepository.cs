using Microsoft.EntityFrameworkCore;
using Signature.API.Domain.Entities;
using Signature.API.Domain.Interfaces;
using Signature.API.Infra.Data.Context;

namespace Signature.API.Infra.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext ctx)
            : base(ctx)
        {
        }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _ctx.Users
                .FirstOrDefaultAsync(u => u.Email == email);
    }
}
