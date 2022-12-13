
using AuthenticationProject.Services.UserService;
using Microsoft.AspNetCore.Mvc;


namespace AuthenticationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // GET: HomeController
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]       
        public async Task<ActionResult<User>> Register(UserRequest request)
        {
            var value = await _userService.Register(request);
            if (value.Success)
            {
                return Ok(value.Value);
            }
            return BadRequest(value.Error);
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserLogin request)
        {
            var value = await _userService.Login(request);
            if (value.Success)
            {
                return Ok(value.Value);
            }
            return BadRequest(value.Error);

        }

    }
}
