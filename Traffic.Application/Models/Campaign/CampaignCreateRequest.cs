﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Application.Models.Campaign
{
    public class CampaignCreateRequest
    {
        public string Name { get; set; }
        public int AverageCompletionTime { get; set; }
        public decimal BidPerTaskCompletion { get; set; }
        public decimal Budget { get; set; }
        public IFormFile Document { get; set; }
        public string LinkYoutube { get; set; }
        public string Guideline { get; set; }
        public string LinkPage { get; set; }
        public int DurationOnPage { get; set; }
        public string Status { get; set; }
        public int OwnerBy { get; set; } 
       
    }
}
