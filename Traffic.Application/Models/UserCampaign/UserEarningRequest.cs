using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Application.Models.UserCampaign
{
    public class UserEarningRequest
    {
        public int UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
