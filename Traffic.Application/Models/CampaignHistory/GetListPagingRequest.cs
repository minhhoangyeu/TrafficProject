using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Application.Models.Common;

namespace Traffic.Application.Models.Campaign
{
    public class GetListPagingRequest : PagingRequestBase
    {
        public int UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
