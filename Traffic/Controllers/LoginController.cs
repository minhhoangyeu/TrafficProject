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
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.Authencate(request);
            return Ok(result);
        }

    }
}
