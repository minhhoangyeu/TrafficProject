using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Traffic.Application.Dtos;
using Traffic.Application.Interfaces;
using Traffic.Application.Models.User;

namespace Traffic.Api.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
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
        [HttpGet("active-user")]
        [AllowAnonymous]
        public async Task<IActionResult> ActiveUser([FromQuery] string code)
        {
            var user = await _userService.Activate(code);
            return Ok(user);
        }
    }
}