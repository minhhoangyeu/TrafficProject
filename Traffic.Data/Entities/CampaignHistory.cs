using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Traffic.Data.Interfaces;

namespace Traffic.Data.Entities
{
    public class CampaignHistory : DomainEntity<int>, ITracking
    {
        public int CampaignId { get; set; }// Unique FK
        public int ImplementBy { get; set; }
        [StringLength(16)]
        public string Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [ForeignKey("CampaignId")]
        public virtual Campaign Campaign { set; get; }
   
        [ForeignKey("ImplementBy")]
        public virtual User User { set; get; }
    }
}
