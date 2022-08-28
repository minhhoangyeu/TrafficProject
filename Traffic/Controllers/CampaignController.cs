using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Traffic.Api.Authorization;
using Traffic.Application.Interfaces;
using Traffic.Application.Models.Campaign;
using Traffic.Utilities.Constants;
using static Traffic.Utilities.Enums;

namespace Traffic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CampaignController(ICampaignService campaignService, IHttpContextAccessor httpContextAccessor)
        {
            _campaignService = campaignService;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost]
        [Authorize(Roles = RoleConstants.ClientRoleName)]
        public async Task<IActionResult> Create([FromForm] CampaignCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            string userId = TrafficAuthenticationHandler.GetCurrentUser(this._httpContextAccessor, ClaimConstants.UserId);
            request.OwnerBy = int.Parse(userId);
            var result = await _campaignService.Create(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _campaignService.GetById(id);
            return Ok(user);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _campaignService.Delete(id);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCampaign([FromForm] CampaignUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
             string userName = TrafficAuthenticationHandler.GetCurrentUser(this._httpContextAccessor, ClaimTypes.Name);
            if (!string.IsNullOrEmpty(userName))
            {
                request.UpdatedBy = userName;
            }
            else
            {
                return BadRequest();
            }
            var result = await _campaignService.Update(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPut("Status")]
        public async Task<IActionResult> UpdateStatus(int campaignId, string status)
        {
            string role = TrafficAuthenticationHandler.GetCurrentUser(this._httpContextAccessor, ClaimTypes.Role);
            if((status == CampaignStatus.Approved.ToString() || status == CampaignStatus.Rejected.ToString()) && RoleConstants.AdminRoleName != role)
            {
                return BadRequest();
            }
            var result = await _campaignService.UpdateStatus(campaignId, status);
            return Ok(result);
        }

    }
}
