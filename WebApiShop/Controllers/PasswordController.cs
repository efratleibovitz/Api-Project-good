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
        private readonly IPasswordService _pass;
        public PasswordController(IPasswordService pass)
        {
            _pass = pass;
        }

      


        // POST api/<PasswordController>
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] string pass)
        {

            PasswordEntity _passWord = await _pass.CheckPasswordStrength(pass);
            if (_passWord == null)
                return NoContent();

            return Ok(_passWord.Strength);
        }

    }
}
