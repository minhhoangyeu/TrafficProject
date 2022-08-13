using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Traffic.Data.Interfaces;

namespace Traffic.Data.Entities
{
    public class UserCampaignConfig : DomainEntity<int>, ITracking
    {
       
        public int LevelId { get; set; }   //Unique
        public int CampaignAmount { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
