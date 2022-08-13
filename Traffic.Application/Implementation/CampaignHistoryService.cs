using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Application.Models.Campaign;
using Traffic.Data.Entities;
using Traffic.Data.Interfaces;

namespace Traffic.Application.Interfaces
{
    public class CampaignHistoryService : ICampaignHistoryService
    {
        private readonly IRepository<CampaignHistory, int> _campaignHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public CampaignHistoryService(IConfiguration configuration, IRepository<CampaignHistory, int> campaignHistoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _campaignHistoryRepository = campaignHistoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public void Create(CampaignHistoryCreateRequest model)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(CampaignHistoryUpdateRequest model)
        {
            throw new NotImplementedException();
        }
    }
}
