using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Api.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet("all")]// <- Added explicit HTTP method so Swagger can generate operations
        public IActionResult Index()
        {
            var users = _userRepository.GetAll(); // returns IEnumerable<User>

            // ?? Domain ? DTO mapping
            var result = users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email.Value,
                FullName = u.FullName,
                Role = u.Role,
                IsActive = u.IsActive
            });

            return Ok(result);
        }

        // (other actions unchanged)
    }
}