using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebAPI.Controllers.v1._0.Users
{
    [ApiController]
    [Route("api/v1.0/users")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public UsersController(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("create")]
        public IActionResult CreateUser([FromBody]UserViewModel user)
        {
            _userService.CreateUser(user.Email, user.Password, user.Role);
            
            return Ok();
        }

        [HttpGet("login")]
        public IActionResult LoginUser(string email, string password)
        {
            var user = _userService.LoginUser(email, password);

            var jwt = GetJWT(user);

            return Ok(jwt);
        }

        private string GetJWT(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, ((short)user.Role).ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("JWT:SymmetricSecurityKey").Value ?? ""));

            var token = new JwtSecurityToken(claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
