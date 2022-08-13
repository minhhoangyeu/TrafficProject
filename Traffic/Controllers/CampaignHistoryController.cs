using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Traffic.Application.Interfaces;

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
    }
}
