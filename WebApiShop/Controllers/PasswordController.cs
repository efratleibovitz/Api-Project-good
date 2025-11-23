using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        IPasswordService _pass;
        public PasswordController(IPasswordService pass)
        {
            _pass = pass;
        }

        // GET: api/<PasswordController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PasswordController>/5
        [HttpGet("{pass}")]
        public void Get(string pass)
        {
            
        }

        // POST api/<PasswordController>
        [HttpPost]
        public ActionResult<string> Post([FromBody] string pass)
        {

            PasswordEntity _passWord = _pass.Level(pass);
            if (_passWord == null)
                return NoContent();

            return Ok(_passWord.Strength);
        }

        // PUT api/<PasswordController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PasswordController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
