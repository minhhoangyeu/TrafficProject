using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Traffic.Data.Interfaces;

namespace Traffic.Data.Entities
{
    public class User : DomainEntity<int>, ITracking
    {
        
        [StringLength(250)]
        public string UserName { get; set; }
        [MaxLength(255)]
        [Required]
        public string Email { get; set; }
        [MaxLength(255)]
        [Required]
        public string Name { get; set; }
        [MaxLength(32)]
        [Required]
        public string Phone { get; set; }

        [MaxLength(512)]
        [Required]
        public string Address { get; set; }
        [MaxLength(16)]
        [Required]
        public string Role { get; set; }

        public int LevelId { get; set; }
        [MaxLength(8)]
        public string Gender { get; set; }
        [MaxLength(32)]
        public string IpAddress { get; set; } // Unique
        public decimal Balance { get; set; }
        [MaxLength(32)]
        public string Status { get; set; }
        [MaxLength(512)]
        public string Avatar { get; set; }
        public string PasswordHash { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public virtual ICollection<UserCampaign> UserCampaigns { get; set; }
        public virtual ICollection<Campaign> Campaign { get; set; }
        public virtual ICollection<CampaignHistory> CampaignHistory { get; set; }
    }
}
