using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Application.Models.User
{
    public class UserAvatarRequest
    {
        public int Id { get; set; }
        public IFormFile Avatar { get; set; }
    }
}
