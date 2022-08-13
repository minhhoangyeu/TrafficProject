using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Application.Models.Campaign;

namespace Traffic.Application.Interfaces
{
    public interface ICampaignHistoryService
    {
        void Create(CampaignHistoryCreateRequest model);
        void Update(CampaignHistoryUpdateRequest model);
        void Delete(int id);
    }
}
