using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Traffic.Api.Authorization;
using Traffic.Application.Interfaces;
using Traffic.Application.Models.Campaign;
using Traffic.Utilities.Constants;

namespace Traffic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignHistoryController : ControllerBase
    {
        private readonly ICampaignHistoryService _campaignHistoryService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CampaignHistoryController(ICampaignHistoryService campaignHistoryService,IHttpContextAccessor httpContextAccessor)
        {
            _campaignHistoryService = campaignHistoryService;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("getlist")]
        public async Task<IActionResult> GetListPaging([FromQuery] GetListPagingRequest request)
        {
            OkObjectResult campaign = null ;
            var userId = int.Parse(TrafficAuthenticationHandler.GetCurrentUser(this._httpContextAccessor, ClaimConstants.UserId));
            string Role = TrafficAuthenticationHandler.GetCurrentUser(this._httpContextAccessor, ClaimTypes.Role);
            if (Role== RoleConstants.UserRoleName)
            {
                 var data = await _campaignHistoryService.GetListPagingByUser(request,userId);
                campaign = Ok(data);
            }
            if(Role == RoleConstants.ClientRoleName)
            {
                 var data = await _campaignHistoryService.GetListPagingByClient(request,userId);
                campaign = Ok(data);
            }
            return campaign;

        }
    }
}