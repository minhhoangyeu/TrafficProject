using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Traffic.Application.Dtos;
using Traffic.Application.Models.Campaign;
using Traffic.Application.Models.Common;
using Traffic.Data.Entities;
using Traffic.Data.Interfaces;
using static Traffic.Utilities.Enums;

namespace Traffic.Application.Interfaces
{
    public class CampaignService : ICampaignService
    {
        private readonly IRepository<Campaign, int> _campaignRepository;
        private readonly IRepository<User, int> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        public CampaignService(IRepository<User, int> userRepository, IRepository<Campaign, int> campaignRepository, IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
        {
            _campaignRepository = campaignRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userRepository = userRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<ApiResult<bool>> Create(CampaignCreateRequest request)
        {
            var userCredit = _userRepository.FindAll().Where(x => x.Id == request.OwnerBy).FirstOrDefault();
            if (userCredit == null)
            {
                return new ApiErrorResult<bool>("Owner Chiến dịch không hợp lệ");
            }
            var clientBalance = userCredit.Balance;
            if (clientBalance < request.Budget)
            {
                return new ApiErrorResult<bool>("Số dư không đủ để tạo chiến dịch");
            }

            Campaign newCampaign = new Campaign()
            {
                Name = request.Name,
                AverageCompletionTime = request.AverageCompletionTime,
                BidPerTaskCompletion = request.BidPerTaskCompletion,
                Budget = request.Budget,
                LinkYoutube = request.LinkYoutube,
                Guideline = request.Guideline,
                LinkPage = request.LinkPage,
                DurationOnPage = request.DurationOnPage,
                Status = CampaignStatus.New.ToString(),
                OwnerBy = request.OwnerBy,
            };
            if (request.Document != null)
            {
                newCampaign.Document = await this.SaveFile(request.Document);
            }
            _campaignRepository.Add(newCampaign);
            await _unitOfWork.Commit();
            var totalCredit = clientBalance - request.Budget;
            await UpdateCredit(request.OwnerBy, totalCredit);

            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var campaign = _campaignRepository.FindAll().Where(u => u.Id == id).FirstOrDefault();
            if (campaign == null)
            {
                return new ApiErrorResult<bool>("Chiến dịch không tồn tại");
            }
            _campaignRepository.Remove(campaign);
            await _unitOfWork.Commit();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<CampaignDto>> GetById(int id)
        {
            var campaign = await _campaignRepository.FindAll().FirstOrDefaultAsync(u => u.Id == id);
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
            dto.Status = campaign.Status;;
            dto.Document = _fileStorageService.GetFileUrl(campaign.Document);
            return new ApiSuccessResult<CampaignDto>(dto);
        }

        public async Task<ApiResult<PagedResult<CampaignDto>>> GetListCampaignPagingByUserId(int userId, SearchCampaignRequest request)
        {
            var campaign =  _campaignRepository.FindAll().Where(u => u.Id == userId);
            int totalRow = await campaign.CountAsync();
            var data = await campaign.Skip((request.PageIndex - 1) * request.PageSize)
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

        public async Task<ApiResult<PagedResult<CampaignDto>>> SearchCampaignPaging(SearchCampaignRequest request)
        {
            var query = _campaignRepository.FindAll();
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Name.Contains(request.Keyword)
                 || x.LinkPage.Contains(request.Keyword)
                 || x.LinkYoutube.Contains(request.Keyword));
            }
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

        public async Task<ApiResult<bool>> Update(CampaignUpdateRequest request)
        {
            var query = _campaignRepository.FindAll();
            var campaign = query.FirstOrDefault(u => u.Id == request.Id);
            if (campaign == null)
            {
                return new ApiErrorResult<bool>("Chiến dịch không tồn tại");
            }
            campaign.Name = request.Name;
            campaign.AverageCompletionTime = request.AverageCompletionTime;
            campaign.BidPerTaskCompletion = request.BidPerTaskCompletion;
            campaign.LinkYoutube = request.LinkYoutube;
            campaign.Guideline = request.Guideline;
            campaign.LinkPage = request.LinkPage;
            campaign.DurationOnPage = request.DurationOnPage;
            campaign.UpdatedBy = request.UpdatedBy;
            campaign.UpdatedDate = DateTime.Now;
            campaign.Status = CampaignStatus.New.ToString();
            if (request.Document != null)
            {
                campaign.Document = await this.SaveFile(request.Document);
            }
            if (campaign.Budget != request.Budget)
            {
                var userCredit = _userRepository.FindAll().Where(x => x.Id == campaign.OwnerBy).FirstOrDefault();
                if (userCredit == null)
                {
                    return new ApiErrorResult<bool>("Owner Chiến dịch không hợp lệ");
                }
                var clientBalance = userCredit.Balance;
                var totalCredit = clientBalance - request.Budget;
                await UpdateCredit(campaign.OwnerBy, totalCredit);
            }
            else
            {
                campaign.Budget = request.Budget;

            }
            _campaignRepository.Update(campaign);
            await _unitOfWork.Commit();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> UpdateStatus(int campaignId, string status)
        {
            var query = _campaignRepository.FindAll();
            var campaign = query.FirstOrDefault(u => u.Id == campaignId);
            if (campaign == null)
            {
                return new ApiErrorResult<bool>("Chiến dịch không tồn tại");
            }
            if (status == CampaignStatus.Rejected.ToString())
            {
                var userCredit = _userRepository.FindAll().Where(x => x.Id == campaign.OwnerBy).FirstOrDefault();
                if (userCredit == null)
                {
                    return new ApiErrorResult<bool>("Owner Chiến dịch không hợp lệ");
                }
                var clientBalance = userCredit.Balance;
                var totalCredit = clientBalance + campaign.Budget;
                await UpdateCredit(campaign.OwnerBy, totalCredit);
            }
            campaign.Status = CampaignStatus.New.ToString();
            _campaignRepository.Update(campaign);
            await _unitOfWork.Commit();
            return new ApiSuccessResult<bool>();
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _fileStorageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;

        }
        private async Task UpdateCredit(int userId, decimal totalCredit)
        {
            var user = await _userRepository.FindAll().Where(u => u.Id == userId).FirstOrDefaultAsync();
            user.Balance = totalCredit;
            _userRepository.Update(user);
            await _unitOfWork.Commit();

        }

    }
}
