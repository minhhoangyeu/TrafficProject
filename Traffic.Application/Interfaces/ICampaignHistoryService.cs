using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Traffic.Application.Dtos;
using Traffic.Application.Models.Campaign;
using Traffic.Application.Models.Common;

namespace Traffic.Application.Interfaces
{
    public interface ICampaignHistoryService
    {

        Task<ApiResult<bool>> Create(CampaignHistoryCreateRequest request);
        Task<ApiResult<PagedResult<CampaignHistoryDto>>> GetListPagingByUser(GetListPagingRequest request,int userId);
        Task<ApiResult<PagedResult<CampaignHistoryClientDto>>> GetListPagingByClient(GetListPagingRequest request,int userId);
    }
}
