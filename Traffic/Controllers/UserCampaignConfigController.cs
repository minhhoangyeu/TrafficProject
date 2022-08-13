using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Traffic.Application.Interfaces;

namespace Traffic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCampaignConfigController : ControllerBase
    {
        private readonly IUserCampaignConfigService _userCampaignConfigService;
        public UserCampaignConfigController(IUserCampaignConfigService userCampaignConfigService)
        {
            _userCampaignConfigService = userCampaignConfigService;
        }
    }
}
