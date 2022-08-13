using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Application.Models.Campaign;

namespace Traffic.Application.Interfaces
{
    public interface IUserCampaignService
    {
        void Create(UserCampaignCreateRequest model);
        void Update(UserCampaignUpdateRequest model);
        void Delete(int id);
    }
}
