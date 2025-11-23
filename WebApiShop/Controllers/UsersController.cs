using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static WebApiShop.Controllers.UsersController;
using Entities;
using Repository;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        IUserService _userService ;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
           
            User user= _userService.GetUserById(id);
            if (user == null)
                   return NoContent();
            return Ok(user);
        }
        // POST api/<UsersController>
        [HttpPost]
        public ActionResult<User> Post([FromBody] User user)
        {
            User _user = _userService.addUser(user);
            if (_user == null)
            {
                return BadRequest("סיסמא חלשה - נסה סיסמא שונה");
            }
            return CreatedAtAction(nameof(Get), new { id = user.id }, user);

        }

        [HttpPost("Login")]
        public ActionResult<User> Login([FromBody] User user)
        {
            User _user = _userService.login(user);
            if (_user == null)
                return NoContent() ;
            return Ok(_user);

        }
        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User user)
        {
            _userService.updateUser(id, user);
            return Ok(user);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
