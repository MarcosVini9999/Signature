using Microsoft.AspNetCore.Mvc;
using Signature.API.Application.DTOs;
using Signature.API.Application.Interfaces;

namespace Signature.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _svc;

        public UsersController(IUserService svc)
        {
            _svc = svc;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
        {
            var user = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(Guid id)
        {
            var user = await _svc.GetByIdAsync(id);
            if (user is null) return NotFound();
            return Ok(user);
        }
    }
}
