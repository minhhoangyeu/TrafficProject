using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Traffic.Application.Dtos;
using Traffic.Application.Models.Campaign;
using Traffic.Application.Models.Common;
using Traffic.Application.Models.User;

namespace Traffic.Application.Interfaces
{
    public interface ICampaignService
    {
        Task<ApiResult<bool>> Create(CampaignCreateRequest request);
        Task<ApiResult<bool>> Update(CampaignUpdateRequest request);
        Task<ApiResult<PagedResult<CampaignDto>>> SearchCampaignPaging(SearchCampaignRequest request);
        Task<ApiResult<PagedResult<CampaignDto>>> GetListCampaignPagingByUserId(GetListCampaignPagingByUserIdRequest request);
        Task<ApiResult<CampaignDto>> GetById(int id);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> UpdateStatus(int campaignId, string status);
    }
}
