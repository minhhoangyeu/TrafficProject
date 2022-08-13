using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Application.Dtos
{
    public class CampaignHistoryDto
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public int ImplementBy { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        
    }
}
