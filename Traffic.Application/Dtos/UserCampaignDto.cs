using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Application.Dtos
{
    public class UserCampaignDto
    {
        public int Id { get; set; }
        public int CampaignId { get; set; } 
        public int ImplementBy { get; set; } 
        public string Token { get; set; }  
        public bool IsExpiredToken { get; set; }
        public bool IsDoneTask { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
