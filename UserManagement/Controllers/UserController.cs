using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Infrastructure.Interfaces;

namespace UserManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet()]
        public async Task<IActionResult> Users(int id)
        {
            var user = await _userRepository.GetAllUsersAsync();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
