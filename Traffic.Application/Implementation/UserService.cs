﻿using Traffic.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Traffic.Application.Interfaces;
using Traffic.Data.Interfaces;
using AutoMapper;
using Traffic.Application.Models.Common;
using Traffic.Application.Models.User;
using Traffic.Utilities.Helpers;
using Traffic.Application.Dtos;
using static Traffic.Utilities.Enums;

namespace Traffic.Application.Implementation
{
    public class UserService : IUserService
    {
        private readonly IRepository<User, int> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public UserService(IConfiguration configuration, IRepository<User, int> userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ApiResult<UserDto>> Authencate(LoginRequest request)
        {
            var user = await _userRepository.FindAll(x => x.UserName == request.UserName).SingleOrDefaultAsync();
            if (user == null) return new ApiErrorResult<UserDto>("Tài khoản không tồn tại");
            // validate
            var passwordDecrypt = Cryptography.DecryptString(user.PasswordHash);

            if (passwordDecrypt != request.Password)
                return new ApiErrorResult<UserDto>("Mật khẩu không đúng");
            if (user.Status == UserStatus.Locked.ToString())
                return new ApiErrorResult<UserDto>("Tài khoản đã bị khóa");
            if (user.Status == UserStatus.Pending.ToString())
                return new ApiErrorResult<UserDto>("Tài khoản chưa được kích hoạt");

            var claims = new[]
           {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, request.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
                _configuration["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);
            string writeToken = tokenHandler.WriteToken(token);
            //using mapper repplace for this manual
            UserDto dto = new UserDto();
            dto.Id = user.Id;
            dto.UserName = user.UserName;
            dto.Email = user.Email;
            dto.Phone = user.Phone;
            dto.Name = user.Name;
            dto.Address = user.Address;
            dto.Role = user.Role;
            dto.LevelId = user.LevelId;
            dto.Gender = user.Gender;
            dto.IpAddress = user.IpAddress;
            dto.Balance = user.Balance;
            dto.Status = user.Status;
            dto.Avatar = user.Avatar;
            dto.Token = writeToken;

            return new ApiSuccessResult<UserDto>(dto);
        }

        public async Task<ApiResult<bool>> ChangePassword(UserPasswordChangeRequest request)
        {
            var user = _userRepository.FindAll().Where(u => u.Id == request.Id).FirstOrDefault();
            if (user == null)
                return new ApiErrorResult<bool>("User không tồn tại");
            var passwordDecrypt = Cryptography.DecryptString(user.PasswordHash);

            if (passwordDecrypt != request.CurrentPassword)
                throw new AppException("Mật khẩu hiện tại không đúng");
            var passwordEncrypt = Cryptography.EncryptString(request.NewPassword);
            user.PasswordHash = passwordEncrypt;
            _userRepository.Update(user);
            await _unitOfWork.Commit();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Delete(int id)
        {
            var user = _userRepository.FindAll().Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
            {
                return new ApiErrorResult<bool>("User không tồn tại");
            }
            _userRepository.Remove(user);
            await _unitOfWork.Commit();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> ForgotPassword(ForgotPasswordRequest request)
        {
            var query = _userRepository.FindAll();
            if (request.Account != null)
            {
                query = query.Where(x => x.UserName.Equals(request.Account) || x.Email.Equals(request.Account));

            }
            var user = await query.FirstOrDefaultAsync();
            if (user == null)
                return new ApiErrorResult<bool>("Tài khoản hoặc email không tồn tại");
            // Sending email for user

            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<UserDto>> GetById(int id)
        {
            var user = await _userRepository.FindAll().Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return new ApiErrorResult<UserDto>("User không tồn tại");
            }
            // will using auto mapper
            var dto = new UserDto();
            dto.Id = user.Id;
            dto.UserName = user.UserName;
            dto.Email = user.Email;
            dto.Phone = user.Phone;
            dto.Name = user.Name;
            dto.Address = user.Address;
            dto.Role = user.Role;
            dto.LevelId = user.LevelId;
            dto.Gender = user.Gender;
            dto.IpAddress = user.IpAddress;
            dto.Balance = user.Balance;
            dto.Status = user.Status;
            dto.Avatar = user.Avatar;
            return new ApiSuccessResult<UserDto>(dto);
        }

        public async Task<ApiResult<PagedResult<UserDto>>> GetUsersPaging(GetUserPagingRequest request)
        {
            var query = _userRepository.FindAll();
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword)
                 || x.Phone.Contains(request.Keyword));
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new UserDto()
                {
                    Email = x.Email,
                    Id = x.Id,
                    UserName = x.UserName,
                    Phone = x.Phone,
                    Address = x.Address,
                    Role = x.Role,
                    LevelId = x.LevelId,
                    Gender = x.Gender,
                    IpAddress = x.IpAddress,
                    Balance = x.Balance,
                    Status = x.Status,
                    Avatar = x.Avatar

                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<UserDto>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<UserDto>>(pagedResult);
        }

        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {
            var user = _userRepository.FindAll(x => x.UserName == request.UserName || x.Email == request.Email || x.IpAddress == request.IpAddress)
                .Select(u => new User()
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    IpAddress = u.IpAddress
                }).SingleOrDefault();
            if (user != null && user.UserName == request.UserName)
            {
                return new ApiErrorResult<bool>("Tài khoản đã tồn tại");
            }
            if (user != null && user.Email == request.Email)
            {
                return new ApiErrorResult<bool>("Emai đã tồn tại");
            }
            if (user != null && user.IpAddress == request.IpAddress)
            {
                return new ApiErrorResult<bool>("Ipaddress đã được đăng ký tài khoản trước đó");
            }
            string password = Cryptography.EncryptString(request.Password);
            // will using automap for this manual
            var userEntity = new User()
            {
                Email = request.Email,
                UserName = request.UserName,
                Name = request.Name,
                Phone = request.Phone,
                PasswordHash = password,
                Address = request.Address,
                Role = request.Role,
                Status = UserStatus.Pending.ToString(),
                LevelId = 1,
                Gender = request.Gender,
                IpAddress = request.IpAddress,
                Balance = 0,
                Avatar = request.Avatar,
                IsDeleted = false,
                CreatedDate = System.DateTime.Now

            };
            _userRepository.Add(userEntity);
            await _unitOfWork.Commit();

            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<UserDto>> SearchUser(SearchUserRequest request)
        {
            var query = _userRepository.FindAll();
            if (request.UserName != null)
            {
                query = query.Where(x => x.UserName.Equals(request.UserName));

            }
            if (request.Email != null)
            {
                query = query.Where(x => x.Email.Equals(request.Email));
            }
            if (request.Phone != null)
            {
                query = query.Where(x => x.Phone.Equals(request.Phone));
            }
            var user = await query.FirstOrDefaultAsync();
            if (user == null)
                return new ApiErrorResult<UserDto>("Không có data");
            // will using auto mapper
            var dto = new UserDto();
            dto.Id = user.Id;
            dto.UserName = user.UserName;
            dto.Email = user.Email;
            dto.Phone = user.Phone;
            dto.Name = user.Name;
            dto.Address = user.Address;
            dto.Role = user.Role;
            dto.LevelId = user.LevelId;
            dto.Gender = user.Gender;
            dto.IpAddress = user.IpAddress;
            dto.Balance = user.Balance;
            dto.Status = user.Status;
            dto.Avatar = user.Avatar;
            return new ApiSuccessResult<UserDto>(dto);

        }

        public async Task<ApiResult<bool>> Update(UserUpdateRequest request)
        {
            var query = _userRepository.FindAll();
            if (await query.AnyAsync(x => x.Email == request.Email && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Emai đã tồn tại");
            }
            var user = _userRepository.FindAll().Where(u => u.Id == request.Id).FirstOrDefault();

            user.Name = request.Name;
            user.Email = request.Email;
            user.Phone = request.Phone;
            user.Gender = request.Gender;
            user.Avatar = request.Avatar;
            user.Address = request.Address;
            _userRepository.Update(user);
            await _unitOfWork.Commit();

            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> UpdateStatus(int userId, string status)
        {
            var user = await _userRepository.FindAll().Where(u => u.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return new ApiErrorResult<bool>("User không tồn tại");
            }
            user.Status = status;
            _userRepository.Update(user);
            await _unitOfWork.Commit();
            return new ApiSuccessResult<bool>();
        }
    }
}
