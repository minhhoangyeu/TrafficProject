
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Traffic.Data.Entities;
using Traffic.Utilities.Helpers;

namespace Traffic.Data
{
    public class DbInitializer
    {
        private readonly TrafficContext _context;
        private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        public DbInitializer(TrafficContext context)
        {
            _context = context;
        }

        public async Task Seed()
        {

            if (!_context.Users.Any())
            {
                string PasswordHash = Cryptography.EncryptString("default@123");
                _context.Users.AddRange(new List<User>()
                {
                    new User {Email = "admin@gmail.com",UserName = "admin",PasswordHash =PasswordHash,Name = "Nguyen Admin",Phone = "123456789", Address = "TP.HCM",Role = "Admin",LevelId = 1,Gender = "Male",IpAddress = "172.24.208.1",Balance = 0, Avatar = "", IsDeleted = false, CreatedDate = System.DateTime.Now },
                    new User {Email = "user@gmail.com",UserName = "user",PasswordHash =PasswordHash,Name = "Nguyen Admin",Phone = "023456789", Address = "TP.HCM",Role = "Admin",LevelId = 1,Gender = "Male",IpAddress = "172.24.208.2",Balance = 0, Avatar = "", IsDeleted = false, CreatedDate = System.DateTime.Now },
                    new User {Email = "client@gmail.com",UserName = "client",PasswordHash =PasswordHash,Name = "Nguyen Admin",Phone = "223456789", Address = "TP.HCM",Role = "Admin",LevelId = 1,Gender = "Male",IpAddress = "172.24.208.3",Balance = 0, Avatar = "", IsDeleted = false, CreatedDate = System.DateTime.Now }
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}