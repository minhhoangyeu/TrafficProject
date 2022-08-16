using Traffic.Data.Entities;
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
using System.Net.Http.Headers;
using System.IO;

namespace Traffic.Application.Implementation
{
    public class UserService : IUserService
    {
        private readonly IRepository<User, int> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IFileStorageService _fileStorageService;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IConfiguration configuration, IRepository<User, int> userRepository, IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _fileStorageService = fileStorageService;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
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
            dto.Avatar = _fileStorageService.GetFileUrl(user.Avatar);
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
            dto.Avatar = _fileStorageService.GetFileUrl(user.Avatar);
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
                    Avatar = _fileStorageService.GetFileUrl(x.Avatar)

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
                IsDeleted = false,
                CreatedDate = System.DateTime.Now

            };
            if (request.Avatar != null)
            {
                userEntity.Avatar = await this.SaveFile(request.Avatar);
            }
            _userRepository.Add(userEntity);
            await _unitOfWork.Commit();
            await SendMailActivate(userEntity.Email,userEntity.Name);
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
            dto.Avatar = _fileStorageService.GetFileUrl(user.Avatar);
            return new ApiSuccessResult<UserDto>(dto);

        }

        public async Task<ApiResult<bool>> UpdateInfo(UserUpdateRequest request)
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
            user.UpdatedDate = DateTime.Now;
            if (request.Avatar != null)
            {
                user.Avatar = await this.SaveFile(request.Avatar);
            }
            user.Address = request.Address;
            _userRepository.Update(user);
            await _unitOfWork.Commit();

            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> UpdateAvatar(UserAvatarRequest request)
        {
            var user = _userRepository.FindAll().Where(u => u.Id == request.Id).FirstOrDefault();
            if (user == null)
            {
                return new ApiErrorResult<bool>("User không tồn tại");
            }
            if (request.Avatar != null)
            {
                user.Avatar = await this.SaveFile(request.Avatar);
                user.UpdatedDate = DateTime.Now;
            }
            _userRepository.Update(user);
            await _unitOfWork.Commit();

            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> DeleteAvatar(int id)
        {
            var user = _userRepository.FindAll().Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
            {
                return new ApiErrorResult<bool>("User không tồn tại");
            }
            user.Avatar = null;
            user.UpdatedDate = DateTime.Now;
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
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _fileStorageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<ApiResult<string>> Activate(string code)
        {
            string emailDecode = Cryptography.DecryptString(code);
            var user = _userRepository.FindAll().Where(u => u.Email == emailDecode).FirstOrDefault();
            if (user == null)
            {
                return new ApiErrorResult<string>("Kích hoạt tài khoản không thành công");
            }
            user.UpdatedDate = DateTime.Now;
            user.Status = UserStatus.Activated.ToString();
            _userRepository.Update(user);
            await _unitOfWork.Commit();
            return new ApiSuccessResult<string>("Kích hoạt tài khoản thành công");
        }

        private async Task SendMailActivate(string email, string name)
        {
            string emailDecode = Cryptography.EncryptString(email);
            var controller = "/api/Users/active-user?code=" + emailDecode;
            var absUrl = string.Format("{0}://{1}{2}", _httpContextAccessor.HttpContext.Request.Scheme, _httpContextAccessor.HttpContext.Request.Host, controller);
            var message = BuildActivateUserTemplate(name, absUrl);
            await _emailService.SendEmail(email, "Kích hoạt tài khoản", message);
        }
        private string BuildActivateUserTemplate(string name, string url)
        {
            var html = new StringBuilder();
            html.Append("<h1>Welcome!</h1>" +
               "< p > Hello < strong style = 'font-family: Avenir,Helvetica,sans-serif; box-sizing: border-box;' >" + 
               name +
               "</ strong > ! < br />< br /> Thank you for registering on our platform.You're almost ready to start.<br /><br />Simply click the button below to confirm your email address and active your account.</p>" +
               "< table style = 'font-family: Avenir,Helvetica,sans-serif; box-sizing: border-box; margin: 30px auto; padding: 0; text-align: center; width: 100%;' width = '100%' cellspacing = '0' cellpadding = '0' align = 'center' >" +
               "< tbody > < tr >< td style = 'font-family: Avenir,Helvetica,sans-serif; box-sizing: border-box;' align = 'center' >" +
               "< table style = 'font-family: Avenir,Helvetica,sans-serif; box-sizing: border-box;' border = '0' width = '100%' cellspacing = '0' cellpadding = '0' >" +
               "< tbody > < tr > < td style = 'font-family: Avenir,Helvetica,sans-serif; box-sizing: border-box;' align = 'center' >" +
               "< table style = 'font-family: Avenir,Helvetica,sans-serif; box-sizing: border-box;' border = '0' cellspacing = '0' cellpadding = '0' >" +
               "< tbody > < tr > < td style = 'font-family: Avenir,Helvetica,sans-serif; box-sizing: border-box;' >< a href='" +
               url + 
               "' style = 'font-family: Avenir,Helvetica,sans-serif; box-sizing: border-box; border-radius: 3px; color: #fff; display: inline-block; text-decoration: none; background-color: #16a1fd; border-top: 10px solid #16a1fd; border-right: 18px solid #16a1fd; border-bottom: 10px solid #16a1fd; border-left: 18px solid #16a1fd;' target = '_blank' > Confirm Email Address </ a ></ td >" +
               "</ tr > </ tbody > </ table > </ td > </ tr > </ tbody > </ table > </ td > </ tr > </ tbody > </ table >" +
               "< hr style = 'font-family: Avenir,Helvetica,sans-serif; box-sizing: border-box;' />" +
               "< p style = 'font-family: Avenir,Helvetica,sans-serif; box-sizing: border-box; color: #74787e; font-size: 16px; line-height: 1.5em; margin-top: 0; text-align: left; margin-bottom: 0; padding-bottom: 0;' > Best Regards, < br /> Traffic Teams </ p >"
               );
            return html.ToString();
        }
    }
}
