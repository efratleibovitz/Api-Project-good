using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static WebApiShop.Controllers.UsersController;
using Entities;
using Repository;
using Services;
using DTOs;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService ;
        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;

        }

        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDTO>> Get(int id)
        {

            GetUserDTO user = await _userService.GetUserById(id);
            if (user == null)
                   return NoContent();
            return Ok(user);
        }
        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult<GetUserDTO>> Post([FromBody] UserDto user)
        {
            GetUserDTO _user =await _userService.addUser(user);
            if (_user == null)
            {
                return BadRequest("סיסמא חלשה - נסה סיסמא שונה");
            }
            return CreatedAtAction(nameof(Get), new { id = _user.Id }, _user);

        }

        [HttpPost("Login")]
        public async Task<ActionResult<GetUserDTO>> Login([FromBody] LoginDTO user)
        {
            GetUserDTO _user = await _userService.login(user);
            if (_user == null)
            {
                _logger.LogInformation("Login failed: UserName={UserEmail},Password={Password}", user.UserEmail, user.Password);
                return NoContent();

            }

            _logger.LogInformation("Login success: UserName={UserEmail},Password={Password}",
            user.UserEmail,  user.Password);
            return Ok(_user);

        }

      
        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserDto user)
        {
            _userService.updateUser(id,user);
            return Ok(user);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
