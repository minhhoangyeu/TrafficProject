using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Traffic.Application.Models.Campaign
{
    public class UserCampaignUpdateRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int CampaignId { get; set; }
        [Required]
        public int ImplementBy { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
