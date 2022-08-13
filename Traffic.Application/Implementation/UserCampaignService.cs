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
    public class UserCampaignService : IUserCampaignService
    {
        private readonly IRepository<UserCampaign, int> _userCampaignRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public UserCampaignService(IConfiguration configuration, IRepository<UserCampaign, int> userCampaignRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userCampaignRepository = userCampaignRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public void Create(UserCampaignCreateRequest model)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(UserCampaignUpdateRequest model)
        {
            throw new NotImplementedException();
        }
    }
}
