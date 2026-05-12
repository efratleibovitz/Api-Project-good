using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using DTOs;

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UsersController(IUserService userService, ILogger<UsersController> logger, IConfiguration configuration)
        {
            _userService = userService;
            _logger = logger;
            _configuration = configuration;

        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<GetUserDTO>> Get(int id)
        {
            GetUserDTO user = await _userService.GetUserById(id);
            if (user == null) return NoContent();
            return Ok(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] UserDto user)
        {
            string? token = await _userService.addUser(user);
            if (token == null)
                return BadRequest("סיסמא חלשה - נסה סיסמא שונה");

            SetTokenCookie(token);
            return Ok(new { message = "User created successfully" });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginDTO user)
        {
            string? token = await _userService.login(user);
            if (token == null)
            {
                _logger.LogInformation("Login failed: UserName={UserEmail}", user.UserEmail);
                return Unauthorized("פרטי התחברות שגויים");
            }

            _logger.LogInformation("Login success: UserName={UserEmail}", user.UserEmail);
            SetTokenCookie(token);
            return Ok(new { message = "Login successful" });
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, [FromBody] UserDto user)
        {
            _userService.updateUser(id, user);
            return Ok(user);
        }


        private void SetTokenCookie(string token)
        {
            var expiresMinutes = double.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60");
            var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = !isDev, // Only set Secure in non-development
                SameSite = SameSiteMode.Strict,
                //Expires = DateTimeOffset.UtcNow.AddMinutes(expiresMinutes)
            });
        }

        [HttpPost("Logout")]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new { message = "Logged out" });
        }
    }
}
