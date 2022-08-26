using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Application.Models.Common;

namespace Traffic.Application.Models.Campaign
{
    public class SearchCampaignRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }

}
