using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Traffic.Application.Interfaces;

namespace Traffic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCampaignController : ControllerBase
    {
        private readonly IUserCampaignService _userCampaignService;
        public UserCampaignController(IUserCampaignService userCampaignService)
        {
            _userCampaignService = userCampaignService;
        }
    }
}
