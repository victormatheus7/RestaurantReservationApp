using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1._0.Users
{
    [ApiController]
    [Route("api/v1.0/users")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UserService _userService;

        public UsersController(ILogger<UsersController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [Route("create")]
        [HttpPost]
        public IActionResult CreateUser([FromBody]UserViewModel user)
        {
            try
            {
                _userService.CreateUser(user.Email, user.Password, user.Role);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [Route("login")]
        [HttpGet]
        public IActionResult LoginUser(string email, string password)
        {
            try
            {
                _userService.LoginUser(email, password);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
