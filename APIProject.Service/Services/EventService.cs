using APIProject.Common.DTOs.Event;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace APIProject.Service.Services
{
    public class EventService : IEventService
    {
        private readonly INewsRepository _newsRepository;
        private readonly ICustomerService _customerService;
        private readonly IPushNotificationService _pushNotificationService;
        private readonly IStallRepository _stallRepository;
        private readonly IRelatedStallRepository _relatedStallReposirory;
        private readonly IGiftRepository _giftRepository;
        private readonly IGiftEventRepository _giftEventRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;

        public EventService(INewsRepository newsRepository, IMapper mapper, IHub sentryHub, IRelatedStallRepository relatedStallReposirory, IStallRepository stallRepository, IGiftRepository giftRepository, IGiftEventRepository giftEventRepository, IPushNotificationService pushNotificationService, ICustomerService customerService)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
            _relatedStallReposirory = relatedStallReposirory;
            _stallRepository = stallRepository;
            _giftRepository = giftRepository;
            _giftEventRepository = giftEventRepository;
            _pushNotificationService = pushNotificationService;
            _customerService = customerService;
        }

        public async Task<JsonResultModel> GetListEvent(int page, int limit, string searchKey, int? status, string fromDate, string toDate)
        {
            try
            {
                var model = await _newsRepository.GetListEvent(page, limit, searchKey, status, fromDate, toDate);
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
        public async Task<JsonResultModel> GetListEventStatistic()
        {
            try
            {
                var model = await _newsRepository.GetListEventStatistic();
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }


        public async Task<JsonResultModel> GetEventDetail(int ID)
        {
            try
            {
                var model = await _newsRepository.GetEventDetail(ID);
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

        public async Task<JsonResultModel> CreateEvent(CreateEventModel input)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (String.IsNullOrEmpty(input.Title) || !input.StartDate.HasValue || !input.EndDate.HasValue || String.IsNullOrEmpty(input.UrlImage) || string.IsNullOrEmpty(input.Content))
                    {
                        return JsonResponse.Error(SystemParam.ERROR_NEWS_FIELDS_INVALID, SystemParam.MESSAGE_NEWS_FIELDS_INVALID);
                    }
                    if (input.IsPopup.Equals(SystemParam.ACTIVE))
                    {
                        var newsPopupOld = await _newsRepository.GetAllAsync(x => x.IsPopup.Equals(SystemParam.ACTIVE));
                        foreach (var item in newsPopupOld)
                        {
                            item.IsPopup = SystemParam.ACTIVE_FALSE;
                            await _newsRepository.UpdateAsync(item);
                        }
                    }
                    input.StartDate = Util.StartOfDay(input.StartDate.Value);
                    input.EndDate = Util.EndOfDay(input.EndDate.Value);
                    var model = _mapper.Map<News>(input);
                    model.Type = SystemParam.TYPE_NEWS_EVENT;
                    model.TypePost = SystemParam.NEWS_TYPEPOST_POSTED;
                    model.Status = SystemParam.ACTIVE;
                    await _newsRepository.AddAsync(model);
                    if (input.ListEventGift != null)
                    {
                        foreach (var item in input.ListEventGift)
                        {
                            if (item.GiftID == 0)
                            {
                                return JsonResponse.Error(SystemParam.ERROR_CREATE_QRCODE_INVALID, SystemParam.MESSAGE_CREATE_QRCODE_INVALID);
                            }
                            var gift = await _giftRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(item.GiftID) && x.IsActive.Equals(SystemParam.ACTIVE));
                            if (gift == null)
                            {
                                return JsonResponse.Error(SystemParam.ERROR_VOUCHER_NOT_FOUND, SystemParam.MESSAGE_GIFT_NOT_FOUND);
                            }
                            else if (item.Quantity == 0)
                            {
                                return JsonResponse.Error(SystemParam.ERROR_QUANTY_EVENT_GIFT_INVALID, SystemParam.MESSAGE_QUANTY_EVENT_GIFT_INVALID);
                            }
                            else if (item.Quantity > gift.Number)
                            {
                                return JsonResponse.Error(SystemParam.ERROR_QUANTY_GIFT_MAX, SystemParam.MESSAGE_QUANTY_GIFT_MAX);
                            }
                            GiftEvent giftEvent = new GiftEvent()
                            {
                                GiftID = gift.ID,
                                NewsID = model.ID,
                                QuantityExchanged = 0,
                                Quantity = item.Quantity,
                                CreatedDate = DateTime.Now,
                            };
                            await _giftEventRepository.AddAsync(giftEvent);
                        }
                    }
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
        public async Task<JsonResultModel> UpdateEvent(UpdateEventModel input)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (String.IsNullOrEmpty(input.Title) || !input.StartDate.HasValue || !input.EndDate.HasValue || String.IsNullOrEmpty(input.UrlImage) || string.IsNullOrEmpty(input.Content))
                    {
                        return JsonResponse.Error(SystemParam.ERROR_NEWS_FIELDS_INVALID, SystemParam.MESSAGE_NEWS_FIELDS_INVALID);
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
                    input.StartDate = Util.StartOfDay(input.StartDate.Value);
                    input.EndDate = Util.EndOfDay(input.EndDate.Value);
                    _mapper.Map(input, model);
                    await _newsRepository.UpdateAsync(model);
                    var listGiftEvent = await _giftEventRepository.GetAllAsync(x => x.NewsID.Equals(input.ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                    foreach (var itemA in listGiftEvent)
                    {
                        if (!input.ListEventGift.Select(x => x.GiftID).Contains(itemA.GiftID))
                        {
                            itemA.IsActive = SystemParam.ACTIVE_FALSE;
                            await _giftEventRepository.UpdateAsync(itemA);
                        }
                    }
                    foreach (var a in input.ListEventGift)
                    {
                        var gift = await _giftRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(a.GiftID));
                        var GiftEvent = listGiftEvent.FirstOrDefault(x => x.GiftID == a.GiftID);
                        if (GiftEvent != null)
                        {
                            if (gift == null)
                            {
                                GiftEvent.IsActive = SystemParam.ACTIVE;
                                await _giftEventRepository.UpdateAsync(GiftEvent);
                                continue;
                            }
                            else if (a.Quantity > gift.Number)
                            {
                                return JsonResponse.Error(SystemParam.ERROR_QUANTY_GIFT_MAX, SystemParam.MESSAGE_QUANTY_GIFT_MAX);
                            }
                            GiftEvent.Quantity = a.Quantity;
                            await _giftEventRepository.UpdateAsync(GiftEvent);
                        }
                        else
                        {
                            if (gift == null)
                            {
                                continue;
                            }
                            else if (a.Quantity == 0)
                            {
                                return JsonResponse.Error(SystemParam.ERROR_QUANTY_EVENT_GIFT_INVALID, SystemParam.MESSAGE_QUANTY_EVENT_GIFT_INVALID);
                            }
                            else if (a.Quantity > gift.Number)
                            {
                                return JsonResponse.Error(SystemParam.ERROR_QUANTY_GIFT_MAX, SystemParam.MESSAGE_QUANTY_GIFT_MAX);
                            }
                            GiftEvent giftEvent = new GiftEvent()
                            {
                                GiftID = gift.ID,
                                NewsID = model.ID,
                                QuantityExchanged = 0,
                                Quantity = a.Quantity,
                                CreatedDate = DateTime.Now,
                            };
                            await _giftEventRepository.AddAsync(giftEvent);
                        }
                    }
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

        public async Task<JsonResultModel> DeleteEvent(int ID)
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
    }
}
