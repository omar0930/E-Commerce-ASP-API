using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Services.Handle_Responses;
using Store.Services.Services.UserServices;
using Store.Services.Services.UserServices.Dtos;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Store.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }
        // GET: api/<AccountController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AccountController>
        [HttpPost]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto input)
        {
            var user = await this.userService.Login(input);
            if (user is not null)
            {
                return user;
            }
            else
            {
                return BadRequest(new Custom_Exception(400, "Login Failed"));
            }
        }
        [HttpPost]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto input)
        {
            var user = await this.userService.Register(input);
            if (user is not null)
            {
                return user;
            }
            else
            {
                return BadRequest(new Custom_Exception(400, "Registration Failed"));
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var UserId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await this.userService.GetCurrentUserDetails((Guid.Parse(UserId)));
        }
        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
