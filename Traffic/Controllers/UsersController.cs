using Microsoft.AspNetCore.Mvc;
using System.Net;
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
        public IActionResult CreateUser(UserCreateRequest request)
        {
            _userService.Register(request);
            return Ok(new { message = "Registration successful" });
        }

        [HttpGet("{id}")]
        public  IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            if(user ==null)
                return NotFound(new { StatusCode = (int)HttpStatusCode.NotFound, message = "User not found" });
            UserDto dto = new UserDto()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.Username,
                Phone = user.Phone,
                Address = user.Address
            };
            return Ok(dto);
        }

        [HttpPut]
        public IActionResult Update(UserUpdateRequest model)
        {
            _userService.Update(model);
            return Ok(new { message = "User updated successfully" });
        }
        [HttpPut("change-password")]
        public IActionResult ChangePassword( [FromBody] UserPasswordChangeRequest request)
        {
            _userService.ChangePassword(request);
            return Ok(new { message = "Password updated successfully" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok(new { message = "User deleted successfully" });
        }
    }
}