using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Traffic.Api.Authorization;
using Traffic.Application.Interfaces;
using Traffic.Application.Models.Campaign;
using Traffic.Application.Models.UserCampaign;
using Traffic.Utilities.Constants;

namespace Traffic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCampaignController : ControllerBase
    {
        private readonly IUserCampaignService _userCampaignService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserCampaignController(IUserCampaignService userCampaignService, IHttpContextAccessor httpContextAccessor)
        {
            _userCampaignService = userCampaignService;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost]
        //[Authorize(Roles = RoleConstants.UserRoleName)]
        public async Task<IActionResult> DoTask([FromBody] UserCampaignCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            string userId = TrafficAuthenticationHandler.GetCurrentUser(this._httpContextAccessor, ClaimConstants.UserId);
            request.ImplementBy = int.Parse(userId);
            request.Token = UserCampaignConstants.Token;
            var result = await _userCampaignService.DoTask(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpGet("{campaignId}")]
        public async Task<IActionResult> ViewTaskDetail(int campaignId)
        {
            var result = await _userCampaignService.ViewTaskDetail(campaignId);
            return Ok(result);
        }
        [HttpPut("finishtask")]
        public async Task<IActionResult> FinishTask(UserCampaignUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            string userId = TrafficAuthenticationHandler.GetCurrentUser(this._httpContextAccessor, ClaimConstants.UserId);
            request.ImplementBy = int.Parse(userId);
            var result = await _userCampaignService.FinishTask(request);
            return Ok(result);
        }
        [HttpGet("gettask")]
        public async Task<IActionResult> GetListCampaignPaging([FromQuery] GetListCampaignPagingByUserIdRequest request)
        {
            var campaign = await _userCampaignService.GetTaskListPaging(request);
            return Ok(campaign);
        }
        [HttpGet("viewearning")]
        public async Task<IActionResult> ViewEarning([FromQuery] UserEarningRequest request)
        {
            var campaign = await _userCampaignService.ViewEarning(request);
            return Ok(campaign);
        }
    }
}
