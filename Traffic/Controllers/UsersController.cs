using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Traffic.Api.Authorization;
using Traffic.Application.Interfaces;
using Traffic.Application.Models.User;
using Traffic.Utilities.Helpers;

namespace Traffic.Api.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsersController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Register(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }
        [Route("profile")]
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            string userId = TrafficAuthenticationHandler.GetCurrentUser(this._httpContextAccessor);
            if (!string.IsNullOrEmpty(userId))
            {
                userId = "0";
            }
            var user = await _userService.GetById(int.Parse(userId));
            return Ok(user);
        }
        [HttpGet("searchuser")]
        public async Task<IActionResult> SearchUser([FromQuery] GetUserPagingRequest request)
        {
            var users = await _userService.GetUsersPaging(request);
            return Ok(users);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserInfo([FromForm] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            string userId = TrafficAuthenticationHandler.GetCurrentUser(this._httpContextAccessor);
            if (!string.IsNullOrEmpty(userId))
            {
                request.Id = int.Parse(userId);
            }
            var result = await _userService.UpdateInfo(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] UserPasswordChangeRequest request)
        {
            var result = await _userService.ChangePassword(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }
        [HttpPut("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _userService.ForgotPassword(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.Delete(id);
            return Ok(result);
        }

        [HttpPut("Status")]
        public async Task<IActionResult> UpdateStatus(int userId, string status)
        {
            var result = await _userService.UpdateStatus(userId, status);
            return Ok(result);
        }
        [HttpPut("UpdateAvatar")]
        public async Task<IActionResult> UpdateAvatar([FromForm] UserAvatarRequest request)
        {
            var result = await _userService.UpdateAvatar(request);
            return Ok(result);
        }
        [HttpDelete("DeleteAvatar")]
        public async Task<IActionResult> DeleteAvatar(int id)
        {
            var result = await _userService.DeleteAvatar(id);
            return Ok(result);
        }
        [HttpGet("verify")]
        public ContentResult Verify()
        {
            var html = "<div id='actionElement'><div class='mdl - card mdl - shadow--2dp firebaseui-container firebaseui - id - page - email - verification - failure'><div class='firebaseui - card - header'><h1 class='firebaseui - title'>Your account has been Activated</h1></div><div class='firebaseui - card - content'><p class='firebaseui - text'>Your account has already been used</p></div><div class='firebaseui - card - actions'></div></div></div>";
            return base.Content(html, "text/html");
        }
        [HttpGet("active-user")]
        [AllowAnonymous]
        public ContentResult ActiveUser([FromQuery] string code)
        {
            string html = "";
            var result =  _userService.Activate(code);
            if (result.Result.IsSuccessed)
            {
                html = "<div id='actionElement'><div class='mdl - card mdl - shadow--2dp firebaseui-container firebaseui - id - page - email - verification - failure'><div class='firebaseui - card - header'><h1 class='firebaseui - title'>Your account has been Activated</h1></div><div class='firebaseui - card - content'><p class='firebaseui - text'>Your account has already been used</p></div><div class='firebaseui - card - actions'></div></div></div>";
            }
            else
            {
                 html = "<div>Failed ! Try verifying your email again.</div>";
            }
            return base.Content(html, "text/html");
        }
        [HttpGet("test-active-user")]
        [AllowAnonymous]
        public async Task<IActionResult> geturl(int id)
        {
            var user = await _userService.GetById(id);
            string emailDecode = Cryptography.EncryptString(user.ResultObj.Email);
            var controller = "/api/Users/active-user?code=" + emailDecode;
            var absUrl = string.Format("{0}://{1}{2}", _httpContextAccessor.HttpContext.Request.Scheme, _httpContextAccessor.HttpContext.Request.Host, controller);
            return Ok(absUrl);
        }
    }
}