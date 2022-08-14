


using Traffic.Application.Models.Common;

namespace Traffic.Application.Models.User
{
    public class GetUserPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}