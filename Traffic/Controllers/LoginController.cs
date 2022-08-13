using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using Traffic.Api.Middlewares;
using Traffic.Application.Interfaces;
using Traffic.Application.Models.User;


namespace Traffic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IJwtUtils _JwtUtils;
        public LoginController(IConfiguration configuration,IUserService userService, IJwtUtils JwtUtils)
        {
            _configuration = configuration;
            _userService = userService;
            _JwtUtils = JwtUtils;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            var user =  _userService.Authenticate(login);
            var token =_JwtUtils.GenerateToken(user);
            //var claims = new[]
            //{
            //    new Claim(ClaimTypes.Name, login.UserName),
            //    new Claim("UserId", user.Id.ToString()),
            //    new Claim("Roles", user.Role.ToString())
            //};
            return Ok(new LoginResponse { Successful = true, Token = token });
        }
       
    }
}
