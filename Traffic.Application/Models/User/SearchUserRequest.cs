using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Application.Models.User
{
    public class SearchUserRequest
    {
        public string UserName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

    }
}
