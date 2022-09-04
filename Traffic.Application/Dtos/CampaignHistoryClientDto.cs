using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Application.Dtos
{
    public class CampaignHistoryClientDto
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public string Name { get; set; }
        public decimal BidPerTaskCompletion { get; set; }
        public decimal Budget { get; set; }
        public decimal RemainingBudget { get; set; }
        public int TotalFinishedTask { get; set; }
        public int OwnerBy { get; set; }
        public int ImplementedBy { get; set; }
        public DateTime ImplementedDate { get; set; }
        public string TaskStatus { get; set; }

    }
}
