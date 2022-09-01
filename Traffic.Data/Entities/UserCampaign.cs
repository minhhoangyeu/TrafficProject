using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Traffic.Data.Interfaces;

namespace Traffic.Data.Entities
{
    public class UserCampaign : DomainEntity<int>, ITracking
    {
        public int CampaignId { get; set; } //int (FK)
        public int? ImplementBy { get; set; } // FK
        [StringLength(512)]
        public string Token { get; set; }   //Unique, Hash
        public bool IsExpiredToken { get; set; }
        public bool IsDoneTask { get; set; }
        public string  Status { get; set; }
        [ForeignKey("CampaignId")]
        public virtual Campaign Campaign { set; get; }
        [ForeignKey("ImplementBy")]
        public virtual User User { set; get; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
