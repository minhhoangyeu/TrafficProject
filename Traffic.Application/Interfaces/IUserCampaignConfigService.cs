using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Application.Models.Campaign;

namespace Traffic.Application.Interfaces
{
    public interface IUserCampaignConfigService
    {
        void Create(UserCampaignConfigCreateRequest model);
        void Update(UserCampaignConfigUpdateRequest model);
        void Delete(int id);
    }
}
