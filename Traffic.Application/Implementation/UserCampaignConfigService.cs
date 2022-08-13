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
    public class UserCampaignConfigService : IUserCampaignConfigService
    {
        private readonly IRepository<UserCampaignConfig, int> _userCampaignConfigRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public UserCampaignConfigService(IConfiguration configuration, IRepository<UserCampaignConfig, int> userCampaignConfigRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userCampaignConfigRepository = userCampaignConfigRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public void Create(UserCampaignConfigCreateRequest model)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(UserCampaignConfigUpdateRequest model)
        {
            throw new NotImplementedException();
        }
    }
}
