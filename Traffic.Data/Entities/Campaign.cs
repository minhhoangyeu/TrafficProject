using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Traffic.Data.Interfaces;

namespace Traffic.Data.Entities
{
    public class Campaign : DomainEntity<int>, ITracking
    {
        
        [StringLength(512)]
        public string Name { get; set; }
        public int AverageCompletionTime { get; set; }
        public decimal BidPerTaskCompletion { get; set; }
        public decimal Budget { get; set; }
        [StringLength(512)]
        public string Document { get; set; }
        [StringLength(512)]
        public string LinkYoutube { get; set; }
        public string Guideline { get; set; }
        [Required]
        public string LinkPage { get; set; }
        public int DurationOnPage { get; set; }
        [StringLength(16)]
        public string Status { get; set; }
        public int OwnerBy { get; set; } //varchar(255) : FK
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [ForeignKey("OwnerBy")]
        public virtual User User { set; get; }
        public virtual ICollection<UserCampaign> UserCampaigns { get; set; }
        public virtual ICollection<CampaignHistory> CampaignHistorys { get; set; }
    }
}
