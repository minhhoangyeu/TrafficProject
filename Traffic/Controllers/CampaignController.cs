using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Traffic.Application.Interfaces;

namespace Traffic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;
        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }
    }
}
