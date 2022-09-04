using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Application.Models.Common;

namespace Traffic.Application.Models.Campaign
{
    public class GetListPagingByClientIdRequest : PagingRequestBase
    {
        public int ClientId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
