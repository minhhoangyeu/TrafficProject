using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Traffic.Application.Dtos;
using Traffic.Application.Models;
using Traffic.Application.Models.Common;
using Traffic.Application.Models.User;
using Traffic.Data.Entities;

namespace Traffic.Application.Interfaces
{
    public interface IUserService
    {
        Task<ApiResult<UserDto>> Authencate(LoginRequest request);
        Task<ApiResult<bool>> Register(RegisterRequest request);
        Task<ApiResult<bool>> Update(UserUpdateRequest request);
        Task<ApiResult<PagedResult<UserDto>>> GetUsersPaging(GetUserPagingRequest request);
        Task<ApiResult<UserDto>> GetById(int id);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<UserDto>> SearchUser(SearchUserRequest request);
        Task<ApiResult<bool>> ForgotPassword(ForgotPasswordRequest request);
        Task<ApiResult<bool>> ChangePassword(UserPasswordChangeRequest request);
    }
}
