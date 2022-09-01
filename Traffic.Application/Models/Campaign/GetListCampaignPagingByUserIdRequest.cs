using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Application.Models.Common;

namespace Traffic.Application.Models.Campaign
{
    public class GetListCampaignPagingByUserIdRequest : PagingRequestBase
    {
        public int UserId { get; set; }
    }
}
