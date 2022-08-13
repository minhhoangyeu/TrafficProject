using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Application.Models.Campaign;

namespace Traffic.Application.Interfaces
{
    public interface ICampaignService
    {
        void Create(CampaignCreateRequest model);
        void Update(CampaignUpdateRequest model);
        void Delete(int id);
    }
}
