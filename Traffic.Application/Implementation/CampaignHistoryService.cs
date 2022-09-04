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

        public async Task<ApiResult<PagedResult<CampaignHistoryDto>>> GetListPaging(GetListPagingRequest request)
        {
            var query = _campaignHistoryRepository.FindAll();
            if (request.UserId > 0)
            {
                query = query.Where(x => x.ImplementBy.Equals(request.UserId));
            }
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
    }
}
