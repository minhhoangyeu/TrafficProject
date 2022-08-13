using AutoMapper;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Traffic.Application.Interfaces;
using Traffic.Application.Models.User;
using Traffic.Data.Entities;
using Traffic.Data.Interfaces;
using Traffic.Utilities.Helpers;

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

        public User Authenticate(LoginRequest model)
        {
            var user = _userRepository.FindAll(x => x.Username == model.UserName).SingleOrDefault();
            // validate
            if (user == null)
                throw new AppException("Username is incorrect");
            var passwordDecrypt = Cryptography.DecryptString(user.PasswordHash);

            if (passwordDecrypt != model.Password)
                throw new AppException("password is incorrect");
            return user;
        }
        public void Delete(int id)
        {
            var user = _userRepository.FindAll().Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
                throw new AppException("Username not found");
            _userRepository.Remove(user);
            _unitOfWork.Commit();
        }
        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }
        public void Register(UserCreateRequest model)
        {
            // validate
            var userEntity = _userRepository.FindAll().Where(u => u.Username == model.UserName).FirstOrDefault();
            if (userEntity != null)
                throw new AppException("Username '" + model.UserName + "' is already taken");
            string password = Cryptography.EncryptString(model.PasswordHash);
            var user = new User()
            {
                Email = model.Email,
                Username = model.UserName,
                Name = model.Name,
                Phone = model.Phone,
                PasswordHash = password,
                Address = model.Address,
                Role = model.Role,
                LevelId = model.LevelId,
                Gender = model.Gender,
                IpAddress = model.IpAddress,
                Balance = 0,
                Avatar = model.Avatar,
                IsDeleted = false,
                CreatedDate = System.DateTime.Now

            };
            // save user
            _userRepository.Add(user);
            _unitOfWork.Commit();

        }
        public void Update( UserUpdateRequest model)
        {
            var user = _userRepository.FindAll().Where(u => u.Id == model.Id).FirstOrDefault();
            if (user == null)
                throw new KeyNotFoundException("User not found");
            user.Email = model.Email;
            user.Phone = model.Phone;
            user.Name = model.Name;
            user.Address = model.Address;
            user.Role = model.Role;
            user.LevelId = model.LevelId;
            user.Gender = model.Gender;
            user.IpAddress = model.IpAddress;
            user.Balance = model.Balance;
            user.Status = model.Status;
            user.Avatar = model.Avatar;
            //_mapper.Map(model, user);
            _userRepository.Update(user);
             _unitOfWork.Commit();
        }

        public void ChangePassword(UserPasswordChangeRequest model)
        {
            var user = _userRepository.FindAll().Where(u => u.Id == model.Id).FirstOrDefault();
            if (user == null)
                throw new KeyNotFoundException("User not found");
            var passwordDecrypt = Cryptography.DecryptString(user.PasswordHash);

            if (passwordDecrypt != model.CurrentPassword)
                throw new AppException("CurrentPassword is incorrect");
            var passwordEncrypt = Cryptography.EncryptString(model.NewPassword);
            user.PasswordHash = passwordEncrypt;
            _userRepository.Update(user);
            _unitOfWork.Commit();
        }

        public User GetById(int id)
        {
            var user = _userRepository.FindAll().Where(u => u.Id == id).FirstOrDefault();
            return user;
        }
        
    }
}
