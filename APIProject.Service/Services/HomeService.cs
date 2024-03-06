using APIProject.Common.DTOs.Customer;
using APIProject.Common.DTOs.Home;
using APIProject.Common.Utils;
using APIProject.Repository;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
using AutoMapper;
using Sentry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Services
{
    public class HomeService : IHomeService
    {
        private readonly IStallRepository _stallRepository;
        private readonly ICustomerRepository _CustomerRepository;
        private readonly INewsRepository _newsRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        public HomeService(IStallRepository stallRepository, INewsRepository newsRepository, ICustomerRepository customer, IMapper mapper, IHub sentryHub, ICategoryRepository categoryRepository)
        {
            _stallRepository = stallRepository;
            _CustomerRepository = customer;
            _newsRepository = newsRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
            _categoryRepository = categoryRepository;
        }

        public async Task<JsonResultModel> CheckTimezone()
        {
            try
            {
                return JsonResponse.Success(DateTime.Now);
            }catch(Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetHome()
        {
            try
            {
                var listEvent = await _newsRepository.GetListNews(SystemParam.PAGE_DEFAULT, SystemParam.LIMIT_DEFAULT, "", SystemParam.TYPE_NEWS_NEWS_EVENT, "", "");
                var ListPromotion = await _newsRepository.GetListNews(SystemParam.PAGE_DEFAULT, SystemParam.LIMIT_DEFAULT, "", SystemParam.TYPE_NEWS_PROMOTION, "", "");
                var ListCampaign = await _newsRepository.GetListNews(SystemParam.PAGE_DEFAULT, SystemParam.LIMIT_DEFAULT, "", SystemParam.TYPE_NEWS_EVENT, "", "");
                var listBanner = await _newsRepository.GetListNewsBanner();
                var listCategory = await _categoryRepository.GetListCategory(SystemParam.PAGE_DEFAULT, SystemParam.LIMIT_DEFAULT);
                var newsPopup = await _newsRepository.GetNewsPopup();
                var model = new HomeModel
                {
                    ListBanner = listBanner,
                    ListEvent = listEvent,
                    ListPromotion = ListPromotion,
                    ListCampaign = ListCampaign,
                    ListCategory = listCategory,
                    NewsPopup = newsPopup
                };
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> GetHomeStaff()
        {
            try
            {
                var listEvent = await _newsRepository.GetListNews(SystemParam.PAGE_DEFAULT, SystemParam.LIMIT_DEFAULT, "", SystemParam.TYPE_NEWS_EVENT, "", "");
                var listBanner = await _newsRepository.GetListNewsBanner();
                var model = new HomeStaffModel
                {
                    ListBanner = listBanner,
                    ListEvent = listEvent,
                };
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> OverView()
        {
            try
            {
                var Cus = await _CustomerRepository.GetAllAsync(c => c.IsActive.Equals(SystemParam.ACTIVE));
                int countCus = Cus.Count;
                var Stall = await _stallRepository.GetAllAsync(c => c.IsActive.Equals(SystemParam.ACTIVE));
                int countStall = Stall.Count;
                var Event = await _newsRepository.GetAllAsync(c => c.IsActive.Equals(SystemParam.ACTIVE) && c.StartDate <= DateTime.Now && c.EndDate <= DateTime.Now);
                int countEvent = Event.Count;
                var model = new OverViewModel
                {
                    Customer = countCus,
                    Stall = countStall,
                    EventActive = countEvent,
                };
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}
