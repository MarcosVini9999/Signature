using Signature.API.Application.DTOs;
using Signature.API.Application.Interfaces;
using Signature.API.Domain.Entities;
using Signature.API.Domain.Interfaces;


namespace Signature.API.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            if (await _repo.GetByEmailAsync(dto.Email) != null)
            {
                throw new InvalidOperationException("Email already in use.");
            }

            var user = new User(dto.Name, dto.Email);
            await _repo.AddAsync(user);

            return new UserDto(user.Id, user.Name, user.Email);
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);
            return user is null ? null : new UserDto(user.Id, user.Name, user.Email);
        }
    }
}
