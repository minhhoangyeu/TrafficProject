using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Traffic.Application.Models.Campaign
{
    public class UserCampaignCreateRequest
    {
        [Required]
        public int CampaignId { get; set; }
        [Required]
        public int ImplementBy { get; set; } 
        public string Token { get; set; } 
       
    }
}
