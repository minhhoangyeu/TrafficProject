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
using Traffic.Application.Models.UserCampaign;
using Traffic.Data.Entities;
using Traffic.Data.Interfaces;
using Traffic.Utilities.Helpers;
using static Traffic.Utilities.Enums;

namespace Traffic.Application.Interfaces
{
    public class UserCampaignService : IUserCampaignService
    {
        private readonly IRepository<UserCampaign, int> _userCampaignRepository;
        private readonly IRepository<CampaignHistory, int> _campaignHistoryRepository;
        private readonly IRepository<Campaign, int> _campaignRepository;
        private readonly IRepository<User, int> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IFileStorageService _fileStorageService;
        public UserCampaignService(IConfiguration configuration, IRepository<Campaign, int> campaignRepository, IRepository<User, int> userRepository, IRepository<UserCampaign, int> userCampaignRepository, IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService, IRepository<CampaignHistory, int> campaignHistoryRepository)
        {
            _userRepository = userRepository;
            _userCampaignRepository = userCampaignRepository;
            _campaignRepository = campaignRepository;
            _fileStorageService = fileStorageService;
            _campaignHistoryRepository = campaignHistoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ApiResult<bool>> DoTask(UserCampaignCreateRequest request)
        {
            var campaign = await _campaignRepository.FindAll().FirstOrDefaultAsync(u => u.Id == request.CampaignId && u.Status == CampaignStatus.Approved.ToString());
            if (campaign == null)
            {
                return new ApiErrorResult<bool>("Chiến dịch không tồn tại");
            }
            UserCampaign newCampaign = new UserCampaign()
            {
                CampaignId = request.CampaignId,
                ImplementBy = request.ImplementBy,
                Token = Cryptography.EncryptString(request.Token),
                IsExpiredToken = false,
                IsDoneTask = false,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
                Status = DoTaskStatus.Processing.ToString()

            };
            _userCampaignRepository.Add(newCampaign);
            await _unitOfWork.Commit();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> FinishTask(UserCampaignUpdateRequest request)
        {
            var campaign = _campaignRepository.FindAll().FirstOrDefault(u => u.Id == request.CampaignId);
            var userCampaign = _userCampaignRepository.FindAll().FirstOrDefault(u => u.Id == request.Id && u.ImplementBy == request.ImplementBy);
            var userCredit = _userRepository.FindAll().Where(x => x.Id == request.ImplementBy).FirstOrDefault();
            if (userCredit == null)
            {
                return new ApiErrorResult<bool>("User thực hiện nhiệm vụ không hợp lệ");
            }
            CampaignHistory campaignHistory = new CampaignHistory()
            {
                CampaignId = request.CampaignId,
                ImplementBy = request.ImplementBy,
                Status = DoTaskStatus.Completed.ToString(),
                IsDeleted = false,
                CreatedDate = DateTime.Now
            };
            var tokenVerify = Cryptography.EncryptString(request.Token);
            if (tokenVerify != userCampaign.Token)
            {
                campaignHistory.Status = DoTaskStatus.Failed.ToString();
                return new ApiErrorResult<bool>("Token không đúng");
            }

            userCredit.Balance = userCredit.Balance + campaign.BidPerTaskCompletion;
            await UpdateUserCredit(userCredit);
            campaign.RemainingBudget = campaign.RemainingBudget - campaign.BidPerTaskCompletion;
            campaign.TotalFinishedTask = campaign.TotalFinishedTask + 1;
            await UpdateRemainingCampaign(campaign);
            userCampaign.IsDoneTask = true;
            userCampaign.IsExpiredToken = true;
            userCampaign.Status = DoTaskStatus.Completed.ToString();
            userCampaign.UpdatedDate = DateTime.Now;

            await InsertCampaignHistory(campaignHistory);
            _userCampaignRepository.Update(userCampaign);
            await _unitOfWork.Commit();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<PagedResult<CampaignDto>>> GetTaskListPaging(GetListCampaignPagingByUserIdRequest request)
        {

            // Phải lọc ra những Task nào User đã DoTask rồi bỏ qua.

            var query = _campaignRepository.FindAll().Where(x => x.Status == CampaignStatus.Approved.ToString() && x.RemainingBudget > 0).OrderBy(r => r.CreatedDate);
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CampaignDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    AverageCompletionTime = x.AverageCompletionTime,
                    BidPerTaskCompletion = x.BidPerTaskCompletion,
                    Budget = x.Budget,
                    LinkYoutube = x.LinkYoutube,
                    Guideline = x.Guideline,
                    LinkPage = x.LinkPage,
                    DurationOnPage = x.DurationOnPage,
                    Status = x.Status,
                    OwnerBy = x.OwnerBy,
                    Document = _fileStorageService.GetFileUrl(x.Document)

                }).ToListAsync();
            var pagedResult = new PagedResult<CampaignDto>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<CampaignDto>>(pagedResult);
        }

        public async Task<ApiResult<UserEarningDto>> ViewEarning(UserEarningRequest request)
        {
            var query = from ucam in _userCampaignRepository.FindAll()
                       join cam in _campaignRepository.FindAll()
                       on ucam.CampaignId equals cam.Id into MatchedCampaigns
                       from match in MatchedCampaigns
                       select new 
                       {
                           match.Id,
                           match.BidPerTaskCompletion,
                           ucam.Status,
                           ucam.CreatedDate,
                           ucam.ImplementBy
                       };
            query = query.Where(x => x.ImplementBy == request.UserId);
            if (request.FromDate != null)
            {
                query = query.Where(x => x.CreatedDate >= request.FromDate);
            }
            if (request.ToDate != null)
            {
                query = query.Where(x => x.CreatedDate <= request.ToDate);
            }
            var data =await query.ToListAsync();
            UserEarningDto dto = new UserEarningDto();
            dto.TotalEaning = data.Where(s => s.Status == DoTaskStatus.Completed.ToString()).Sum(x => x.BidPerTaskCompletion);
            dto.TotalTaskCompleted = data.Where(s => s.Status == DoTaskStatus.Completed.ToString()).Count();
            dto.TotalTaskFailed = data.Where(s => s.Status != DoTaskStatus.Completed.ToString()).Count();
            return new ApiSuccessResult<UserEarningDto>(dto);

        }

        public async Task<ApiResult<CampaignDto>> ViewTaskDetail(int campaignId)
        {
            var campaign = await _campaignRepository.FindAll().FirstOrDefaultAsync(u => u.Id == campaignId);
            if (campaign == null)
            {
                return new ApiErrorResult<CampaignDto>("Chiến dịch không tồn tại");
            }
            // will using auto mapper
            var dto = new CampaignDto();
            dto.Id = campaign.Id;
            dto.Name = campaign.Name;
            dto.AverageCompletionTime = campaign.AverageCompletionTime;
            dto.BidPerTaskCompletion = campaign.BidPerTaskCompletion;
            dto.Budget = campaign.Budget;
            dto.LinkYoutube = campaign.LinkYoutube;
            dto.Guideline = campaign.Guideline;
            dto.LinkPage = campaign.LinkPage;
            dto.DurationOnPage = campaign.DurationOnPage;
            dto.OwnerBy = campaign.OwnerBy;
            dto.Status = campaign.Status;
            dto.Document = _fileStorageService.GetFileUrl(campaign.Document);
            dto.CreatedDate = campaign.CreatedDate;
            return new ApiSuccessResult<CampaignDto>(dto);
        }
        private async Task UpdateUserCredit(User user)
        {
            _userRepository.Update(user);
            await _unitOfWork.Commit();
        }
        private async Task UpdateRemainingCampaign(Campaign campaign)
        {
            _campaignRepository.Update(campaign);
            await _unitOfWork.Commit();
        }
        private async Task InsertCampaignHistory(CampaignHistory entity)
        {
            _campaignHistoryRepository.Update(entity);
            await _unitOfWork.Commit();
        }
    }
}
