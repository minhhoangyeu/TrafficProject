using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Traffic.Application.Dtos;
using Traffic.Application.Models.Campaign;
using Traffic.Application.Models.Common;
using Traffic.Application.Models.UserCampaign;

namespace Traffic.Application.Interfaces
{
    public interface IUserCampaignService
    {
        Task<ApiResult<bool>> DoTask(UserCampaignCreateRequest request);
        Task<ApiResult<CampaignDto>> ViewTaskDetail(int campaignId);
        Task<ApiResult<bool>> FinishTask(UserCampaignUpdateRequest request);
        Task<ApiResult<PagedResult<CampaignDto>>> GetTaskListPaging(GetListCampaignPagingByUserIdRequest request);
        Task<ApiResult<bool>> ViewEarning(UserEarningRequest request);
    }
}
