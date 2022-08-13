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
    public class CampaignService : ICampaignService
    {
        private readonly IRepository<Campaign, int> _campaignRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public CampaignService(IConfiguration configuration, IRepository<Campaign, int> campaignRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _campaignRepository = campaignRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public void Create(CampaignCreateRequest model)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(CampaignUpdateRequest model)
        {
            throw new NotImplementedException();
        }
    }
}
