using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Application.Models.Campaign
{
    public class CampaignUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AverageCompletionTime { get; set; }
        public decimal BidPerTaskCompletion { get; set; }
        public decimal Budget { get; set; }
        public IFormFile Document { get; set; }
        public string LinkYoutube { get; set; }
        public string Guideline { get; set; }
        public string LinkPage { get; set; }
        public int DurationOnPage { get; set; }
        public string UpdatedBy { get; set; }
        public string OwnerBy { get; set; }
    }
}
