using System.Collections.Generic;

namespace Traffic.Application.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public int LevelId { get; set; }
        public string Gender { get; set; }
        public string IpAddress { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; }
        public string Avatar { get; set; }
        public string Token { get; set; }
    }
}
