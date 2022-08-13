using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Traffic.Application.Dtos;
using Traffic.Application.Models;
using Traffic.Application.Models.User;
using Traffic.Data.Entities;

namespace Traffic.Application.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        void Register(UserCreateRequest model);
        void Update(UserUpdateRequest model);
        void Delete(int id);
        void ChangePassword(UserPasswordChangeRequest model);
        User Authenticate(LoginRequest model);
     

    }
}
