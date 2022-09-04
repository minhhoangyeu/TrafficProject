using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Application.Models.Campaign
{
    public class CampaignHistoryCreateRequest
    {
        public int CampaignId { get; set; }
        public int? ImplementBy { get; set; }
        public string Status { get; set; }

    }
}
