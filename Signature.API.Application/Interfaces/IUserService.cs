using Signature.API.Application.DTOs;

namespace Signature.API.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task<UserDto?> GetByIdAsync(Guid id);
    }
}
