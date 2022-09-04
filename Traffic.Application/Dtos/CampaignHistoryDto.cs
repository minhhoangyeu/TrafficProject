using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Application.Dtos
{
    public class CampaignHistoryDto
    {
        public int CampaignId { get; set; }
        public string Name { get; set; }
        public decimal BidPerTaskCompletion { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        
    }
}
