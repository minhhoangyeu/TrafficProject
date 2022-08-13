using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Application.Dtos
{
    public class UserCampaignConfigsDto
    {
        public int Id { get; set; }
        public int LevelId { get; set; } 
        public int CampaignAmount { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
