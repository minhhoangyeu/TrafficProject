using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Traffic.Application.Interfaces;
using Traffic.Application.Models.Campaign;

namespace Traffic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignHistoryController : ControllerBase
    {
        private readonly ICampaignHistoryService _campaignHistoryService;
        public CampaignHistoryController(ICampaignHistoryService campaignHistoryService)
        {
            _campaignHistoryService = campaignHistoryService;
        }
        [HttpGet("getlist")]
        public async Task<IActionResult> GetListPaging([FromQuery] GetListPagingRequest request)
        {
            var campaign = await _campaignHistoryService.GetListPaging(request);
            return Ok(campaign);
        }
    }
}
