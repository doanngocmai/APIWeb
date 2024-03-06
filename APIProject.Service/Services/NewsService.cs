using APIProject.Common.DTOs.News;
using APIProject.Common.Utils;
using APIProject.Domain.Models;
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
using System.Transactions;

namespace APIProject.Service.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IRelatedStallRepository _relatedStallReposirory;
        private readonly IGiftNewsRepository _GiftNewsRepository;
        private readonly IGiftRepository _GiftRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        private readonly IStallRepository _stallRepository;
        private readonly IPushNotificationService _pushNotificationService;
        private readonly ICustomerService _customerService;
        public NewsService(INewsRepository newsRepository, IGiftRepository giftRepository, IRelatedStallRepository relatedStallReposirory, IGiftNewsRepository giftNewsRepository, IMapper mapper, IHub sentryHub, IStallRepository stallRepository, IPushNotificationService pushNotificationService, ICustomerService customerService)
        {
            _newsRepository = newsRepository;
            _relatedStallReposirory = relatedStallReposirory;
            _GiftRepository = giftRepository;
            _GiftNewsRepository = giftNewsRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
            _stallRepository = stallRepository;
            _pushNotificationService = pushNotificationService;
            _customerService = customerService;
        }

        public async Task<JsonResultModel> GetListNews(int page, int limit, string searchKey, int? type, int? status, string fromDate, string toDate)
        {
            try
            {
                var model = await _newsRepository.GetListNews(page, limit, searchKey, type, status, fromDate, toDate);
                var data = new DataPagedListModel
                {
                    Data = model,
                    Limit = limit,
                    Page = page,
                    TotalItemCount = model.TotalItemCount
                };
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetListNews(int page, int limit, string searchKey, int? type, string fromDate, string toDate)
        {
            try
            {
                var model = await _newsRepository.GetListNews(page, limit, searchKey, type, fromDate, toDate);

                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetNewsRelated(int ID)
        {
            try
            {
                var model = await _newsRepository.GetNewsRelated(ID);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetNewsDetail(int ID)
        {
            try
            {
                var model = await _newsRepository.GetNewsDetail(ID);
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_NEWS_NOT_FOUND, SystemParam.MESSAGE_NEWS_NOT_FOUND);
                }
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> GetNewsDetailApp(int ID)
        {
            try
            {
                var model = await _newsRepository.GetNewsDetailApp(ID);
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_NEWS_NOT_FOUND, SystemParam.MESSAGE_NEWS_NOT_FOUND);
                }
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> CreateNews(CreateNewsModel input) 
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (input.TypePost != SystemParam.NEWS_TYPEPOST_POSTED)
                    {
                        if (String.IsNullOrEmpty(input.Title) || input.Type == 0 || String.IsNullOrEmpty(input.UrlImage) || string.IsNullOrEmpty(input.Content))
                        {
                            return JsonResponse.Error(SystemParam.ERROR_NEWS_FIELDS_INVALID, SystemParam.MESSAGE_NEWS_FIELDS_INVALID);
                        }
                         else if (input.Type.Equals(SystemParam.TYPE_NEWS_EVENT))
                        {
                            return JsonResponse.Error(SystemParam.ERROR_CODE_NOT_FOUND_NEWS, SystemParam.MESSAGE_NEWS_TYPE_NOT_AVAILABLE);
                        }
                    }
                    if (input.StartDate.HasValue)
                    {
                        input.StartDate = Util.ConvertFromDate(input.StartDate.Value.ToString(SystemParam.CONVERT_DATETIME)).GetValueOrDefault();
                    }
                    if (input.EndDate.HasValue)
                    {
                        input.EndDate = Util.ConvertToDate(input.EndDate.Value.ToString(SystemParam.CONVERT_DATETIME)).GetValueOrDefault();
                    }
                    var model = _mapper.Map<News>(input);
                    if (input.IsPopup.Equals(SystemParam.ACTIVE))
                    {
                        var newsPopupOld = await _newsRepository.GetAllAsync(x => x.IsPopup.Equals(SystemParam.ACTIVE));
                        foreach (var item in newsPopupOld)
                        {
                            item.IsPopup = SystemParam.ACTIVE_FALSE;
                            await _newsRepository.UpdateAsync(item);
                        }
                    }
                    model.Status = SystemParam.ACTIVE;
                    await _newsRepository.AddAsync(model);

                    if (input.ListRelatedStall != null || input.ListRelatedStall.Count > 0)
                    {
                        foreach (var item in input.ListRelatedStall)
                        {
                            var stall = await _stallRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(item) && x.IsActive.Equals(SystemParam.ACTIVE));
                            if (stall == null)
                                return JsonResponse.Error(SystemParam.ERROR_STALL_NOT_FOUND, SystemParam.MESSAGE_STALL_NOT_FOUND);
                            var relatedStall = new RelatedStall
                            {
                                NewsID = model.ID,
                                StallID = item,
                            };
                            await _relatedStallReposirory.AddAsync(relatedStall);
                        }
                    }
                    if (input.ListGiftNews != null || input.ListGiftNews.Count > 0)
                    {
                        foreach (var item in input.ListGiftNews)
                        {
                            var gift = await _GiftRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(item) && x.IsActive.Equals(SystemParam.ACTIVE));
                            if (gift == null)
                                return JsonResponse.Error(SystemParam.ERROR_GIFT_NOT_FOUND, SystemParam.MESSAGE_GIFT_NOT_FOUND);
                            var GiftNews = new GiftNews
                            {
                                NewsID = model.ID,
                                GiftID = item,
                            };
                            await _GiftNewsRepository.AddAsync(GiftNews);
                        }
                    }
                    if (input.IsNotify == SystemParam.ACTIVE)
                    {
                        var listCus = await _customerService.GetAllAsync(x => x.IsActive.Equals(SystemParam.ACTIVE));
                        await _pushNotificationService.PushNotification(listCus, SystemParam.TYPE_NOTIFICATION_NEWS, input.Title, model.ID);
                    }
                    scope.Complete();
                    return JsonResponse.Success();
                }
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> UpdateNews(UpdateNewsModel input)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (input.TypePost != SystemParam.NEWS_TYPEPOST_POSTED)
                    {
                        if (String.IsNullOrEmpty(input.Title) || input.Type == 0 || String.IsNullOrEmpty(input.UrlImage) || string.IsNullOrEmpty(input.Content))
                        {
                            return JsonResponse.Error(SystemParam.ERROR_NEWS_FIELDS_INVALID, SystemParam.MESSAGE_NEWS_FIELDS_INVALID);
                        }
                    }
                    var model = await _newsRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                    if (model == null)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_NEWS_NOT_FOUND, SystemParam.MESSAGE_NEWS_NOT_FOUND);
                    }
                    if (input.IsPopup.Equals(SystemParam.ACTIVE))
                    {
                        var newsPopupOld = await _newsRepository.GetAllAsync(x => x.IsPopup.Equals(SystemParam.ACTIVE) && !x.ID.Equals(model.ID));
                        foreach (var item in newsPopupOld)
                        {
                            item.IsPopup = SystemParam.ACTIVE_FALSE;
                            await _newsRepository.UpdateAsync(item);
                        }
                    }
                    if (input.StartDate.HasValue)
                    {
                        input.StartDate = Util.ConvertFromDate(input.StartDate.Value.ToString(SystemParam.CONVERT_DATETIME)).GetValueOrDefault();
                    }
                    if (input.EndDate.HasValue)
                    {
                        input.EndDate = Util.ConvertToDate(input.EndDate.Value.ToString(SystemParam.CONVERT_DATETIME)).GetValueOrDefault();
                    }
                    _mapper.Map(input, model);
                    await _newsRepository.UpdateAsync(model);
                    var rs = await _relatedStallReposirory.GetAllAsync(x => x.NewsID.Equals(input.ID));
                    await _relatedStallReposirory.DeleteListAsync(rs);
                    foreach (var item in input.ListRelatedStall)
                    {
                        var stall = await _stallRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(item) && x.IsActive.Equals(SystemParam.ACTIVE));
                        if (stall == null)
                            return JsonResponse.Error(SystemParam.ERROR_STALL_NOT_FOUND, SystemParam.MESSAGE_STALL_NOT_FOUND);
                        var relatedStall = new RelatedStall
                        {
                            NewsID = model.ID,
                            StallID = item,
                        };
                        await _relatedStallReposirory.AddAsync(relatedStall);
                    }
                    if (input.IsNotify == SystemParam.ACTIVE)
                    {
                        var listCus = await _customerService.GetAllAsync(x => x.IsActive.Equals(SystemParam.ACTIVE));
                        await _pushNotificationService.PushNotification(listCus, SystemParam.TYPE_NOTIFICATION_NEWS, input.Title, model.ID);
                    }
                    scope.Complete();
                    return JsonResponse.Success();
                }
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> DeleteNews(int ID)
        {
            try
            {
                var model = await _newsRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_NEWS_NOT_FOUND, SystemParam.MESSAGE_NEWS_NOT_FOUND);
                }
                model.IsActive = SystemParam.ACTIVE_FALSE;

                await _newsRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> ChangeStatus(int ID)
        {
            try
            {
                var model = await _newsRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_NEWS_NOT_FOUND, SystemParam.MESSAGE_NEWS_NOT_FOUND);
                }
                if (model.Status == SystemParam.ACTIVE)
                    model.Status = SystemParam.ACTIVE_FALSE;
                else
                    model.Status = SystemParam.ACTIVE;
                await _newsRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetListNewsBanner()
        {
            try
            {
                var model = await _newsRepository.GetListNewsBanner();
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
