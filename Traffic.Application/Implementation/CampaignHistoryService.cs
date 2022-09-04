using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traffic.Application.Dtos;
using Traffic.Application.Models.Campaign;
using Traffic.Application.Models.Common;
using Traffic.Data.Entities;
using Traffic.Data.Interfaces;

namespace Traffic.Application.Interfaces
{
    public class CampaignHistoryService : ICampaignHistoryService
    {
        private readonly IRepository<CampaignHistory, int> _campaignHistoryRepository;
        private readonly IRepository<UserCampaign, int> _userCampaignRepository;
        private readonly IRepository<Campaign, int> _campaignRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public CampaignHistoryService(IConfiguration configuration, IRepository<CampaignHistory, int> campaignHistoryRepository, IUnitOfWork unitOfWork, IMapper mapper, IRepository<UserCampaign, int> userCampaignRepository, IRepository<Campaign, int> campaignRepository)
        {
            _campaignHistoryRepository = campaignHistoryRepository;
            _userCampaignRepository = userCampaignRepository;
            _campaignRepository = campaignRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ApiResult<bool>> Create(CampaignHistoryCreateRequest request)
        {
            CampaignHistory campaignHistory = new CampaignHistory()
            {
                CampaignId = request.CampaignId,
                ImplementBy = request.ImplementBy,
                Status = request.Status,
            };
            _campaignHistoryRepository.Add(campaignHistory);
            await _unitOfWork.Commit();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<PagedResult<CampaignHistoryDto>>> GetListPagingByUser(GetListPagingRequest request, int userId)
        {
            var query = _campaignHistoryRepository.FindAll();
            query = query.Where(x => x.ImplementBy.Equals(userId));
            if (request.FromDate != null)
            {
                query = query.Where(x => x.CreatedDate >= request.FromDate);
            }
            if (request.ToDate != null)
            {
                query = query.Where(x => x.CreatedDate <= request.ToDate);
            }
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CampaignHistoryDto()
                {
                    Id = x.Id,
                    CampaignId = x.CampaignId,
                    ImplementBy = (int)x.ImplementBy,
                    Status = x.Status,
                    CreatedDate = x.CreatedDate

                }).ToListAsync();
            var pagedResult = new PagedResult<CampaignHistoryDto>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<CampaignHistoryDto>>(pagedResult);
        }
        public async Task<ApiResult<PagedResult<CampaignHistoryClientDto>>> GetListPagingByClient(GetListPagingRequest request,int userId)
        {
            var query = from ucam in _userCampaignRepository.FindAll()
                        join cam in _campaignRepository.FindAll()
                        on ucam.CampaignId equals cam.Id into MatchedCampaigns
                        from match in MatchedCampaigns
                        select new
                        {
                            match.Id,
                            match.Name,
                            match.BidPerTaskCompletion,
                            match.Budget,
                            match.RemainingBudget,
                            match.OwnerBy,
                            match.TotalFinishedTask,
                            ucam.Status,
                            ucam.CreatedDate,
                            ucam.ImplementBy
                        };
            query = query.Where(x => x.OwnerBy == userId);
            if (request.FromDate != null)
            {
                query = query.Where(x => x.CreatedDate >= request.FromDate);
            }
            if (request.ToDate != null)
            {
                query = query.Where(x => x.CreatedDate <= request.ToDate);
            }
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
               .Take(request.PageSize)
               .Select(x => new CampaignHistoryClientDto()
               {
                   Id = x.Id,
                   Name = x.Name,
                   BidPerTaskCompletion = x.BidPerTaskCompletion,
                   Budget = x.Budget,
                   TotalFinishedTask = x.TotalFinishedTask,
                   RemainingBudget=x.RemainingBudget,
                   OwnerBy = x.OwnerBy,
                   ImplementedBy = (int)x.ImplementBy,
                   ImplementedDate = x.CreatedDate,
                   TaskStatus = x.Status
               }).ToListAsync();
            var pagedResult = new PagedResult<CampaignHistoryClientDto>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<CampaignHistoryClientDto>>(pagedResult);
        }

    }
}
