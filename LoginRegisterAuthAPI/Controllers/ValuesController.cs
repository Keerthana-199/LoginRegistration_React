using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using LoginRegisterAuthAPI.Models;
using LoginRegisterAuthAPI.Repositories;


namespace LoginRegisterAuthAPI.Controllers
{
   
    [Route("api/users")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        //[HttpGet("GetUser")]
        //public IActionResult GetUser(int Id)
        //{

        //}

        private readonly IUserRepository _userRepository;


        public ValuesController(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Users user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = await _userRepository.CreateUserAsync(user);
            return Ok(new { message = "User registered successfully!", userId });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var tokenInfo = await _userRepository.AuthenticateUserAsync(model.Email, model.Password);

            if (tokenInfo == null)
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(new { tokenInfo });
        }
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _userRepository.GetUserByIdAsync(userId);

            return user == null ? NotFound() : Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }
    }
}

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}

