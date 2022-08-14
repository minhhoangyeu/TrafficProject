using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Application.Models.User
{
    public class ForgotPasswordRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
