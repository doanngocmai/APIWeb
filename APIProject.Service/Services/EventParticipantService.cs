using APIProject.Common.DTOs.EventParticipant;
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
    public class EventParticipantService : IEventParticipantService
    {
        private readonly IEventParticipantRepository _eventParticipantRepository;
        private readonly IQRCodeRepository _qrCodeRepository;
        private readonly IQRCodeBillRepository _qrCodeBillRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly INewsRepository _newsRepository;
        private readonly IBillRepository _billRepository;
        private readonly IMemberPointHistoryRepository _memberPointHistoryRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IPushNotificationService _pushNotificationService;
        private readonly IEventChannelRepository _eventChannelRepository;
        private readonly IGiftEventRepository _giftEventRepository;
        private readonly IGiftEventParticipantRepository _giftEventParticipantRepository;
        private readonly IGiftRepository _giftRepository;
        private readonly IStallRepository _stallRepository;
        private readonly IConfigRepository _configRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;

        public EventParticipantService(IEventParticipantRepository eventParticipantRepository, IMapper mapper, IHub sentryHub, IQRCodeRepository qrCodeRepository, IQRCodeBillRepository qrCodeBillRepository, ICustomerRepository customerRepository, IBillRepository billRepository, IMemberPointHistoryRepository memberPointHistoryRepository, INewsRepository newsRepository, INotificationRepository notificationRepository, IStallRepository stallRepository, IEventChannelRepository eventChannelRepository, IGiftEventRepository giftEventRepository, IGiftRepository giftRepository, IPushNotificationService pushNotificationService, IGiftEventParticipantRepository giftEventParticipantRepository, IConfigRepository configRepository)
        {
            _eventParticipantRepository = eventParticipantRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
            _qrCodeRepository = qrCodeRepository;
            _qrCodeBillRepository = qrCodeBillRepository;
            _customerRepository = customerRepository;
            _billRepository = billRepository;
            _memberPointHistoryRepository = memberPointHistoryRepository;
            _newsRepository = newsRepository;
            _notificationRepository = notificationRepository;
            _stallRepository = stallRepository;
            _eventChannelRepository = eventChannelRepository;
            _giftEventRepository = giftEventRepository;
            _giftRepository = giftRepository;
            _pushNotificationService = pushNotificationService;
            _giftEventParticipantRepository = giftEventParticipantRepository;
            _configRepository = configRepository;
        }

        public async Task<JsonResultModel> CreateQRCode(CreateQRCodeModel input)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (String.IsNullOrEmpty(input.Name) || String.IsNullOrEmpty(input.Phone) || input.DistrictID == 0 || input.WardID == 0
                        || input.ProvinceID == 0 || input.EventID == 0 || input.EventChannelID == 0)
                        return JsonResponse.Error(SystemParam.ERROR_CREATE_QRCODE_INVALID, SystemParam.MESSAGE_CREATE_QRCODE_INVALID);
                    if (!Util.validPhone(input.Phone))
                    {
                        return JsonResponse.Error(SystemParam.ERROR_PHONE_NOT_VALID, SystemParam.MESSAGE_PHONE_NOT_VALID);
                    }
                    var qr = _mapper.Map<QRCode>(input);
                    qr.Code = Util.GenerateCodeProject();
                    await _qrCodeRepository.AddAsync(qr);
                    if (input.ListBill != null)
                    {
                        foreach (var item in input.ListBill)
                        {
                            if (String.IsNullOrEmpty(item.Code))
                            {
                                return JsonResponse.Error(SystemParam.ERROR_EVENT_CODE_EMPTY, SystemParam.MESSAGE_EVENT_CODE_EMPTY);
                            }
                            if (item.Price == 0)
                            {
                                return JsonResponse.Error(SystemParam.ERROR_EVENT_PRICE_EMPTY, SystemParam.MESSAGE_EVENT_PRICE_EMPTY);
                            }
                            //if (String.IsNullOrEmpty(item.ImageUrl))
                            //{
                            //    return JsonResponse.Error(SystemParam.ERROR_EVENT_IMAGE_EMPTY, SystemParam.MESSAGE_EVENT_IMAGE_EMPTY);
                            //}
                            if (item.StallID == 0)
                            {
                                return JsonResponse.Error(SystemParam.ERROR_EVENT_STALL_EMPTY, SystemParam.MESSAGE_EVENT_STALL_EMPTY);
                            }
                            var stall = await _stallRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(item.StallID) && x.Status.Equals(SystemParam.ACTIVE) && x.IsActive.Equals(SystemParam.ACTIVE));
                            if (stall == null)
                            {
                                return JsonResponse.Error(SystemParam.ERROR_STALL_NOT_FOUND, SystemParam.MESSAGE_STALL_NOT_FOUND);
                            }
                            var bill = _mapper.Map<QRCodeBill>(item);
                            bill.QRCodeID = qr.ID;
                            await _qrCodeBillRepository.AddAsync(bill);
                            qr.TotalPrice += item.Price;
                        }
                    }
                    await _qrCodeRepository.UpdateAsync(qr);
                    var cus = await _customerRepository.GetFirstOrDefaultAsync(x => x.Phone.Equals(input.Phone) && x.IsActive.Equals(SystemParam.ACTIVE));
                    if (cus != null)
                    {
                        _mapper.Map(input, cus);
                        await _customerRepository.UpdateAsync(cus);
                    }
                    else
                    {
                        var newCus = _mapper.Map<Customer>(input);
                        newCus.Role = SystemParam.ROLE_CUSTOMER;
                        await _customerRepository.AddAsync(newCus);
                    }
                    scope.Complete();
                    return JsonResponse.Success(qr.Code);
                }
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetEventParticipantDetail(int page, int limit, int ID)
        {
            try
            {
                var model = await _eventParticipantRepository.GetEventParticipantDetail(page, limit, ID);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetListEvent()
        {
            try
            {
                var model = await _newsRepository.GetListEvent();
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetListEventChannel()
        {
            try
            {
                var model = await _eventChannelRepository.GetListEventChannel();
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetListEventGift(int eventID)
        {
            try
            {
                var model = await _giftEventRepository.GetListEventGiftModel(eventID);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetListStall()
        {
            try
            {
                var model = await _stallRepository.GetListStall();
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> InputCustomerInfo(EventParticipantModel input, int StaffID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (String.IsNullOrEmpty(input.Name) || String.IsNullOrEmpty(input.Phone) || input.DistrictID == 0 || input.WardID == 0
                        || input.ProvinceID == 0 || input.EventID == 0 || input.ListBill == null || input.ListBill.Count == 0 || input.EventChannelID == 0)
                        return JsonResponse.Error(SystemParam.ERROR_CREATE_QRCODE_INVALID, SystemParam.MESSAGE_CREATE_QRCODE_INVALID);
                    if (!Util.validPhone(input.Phone))
                    {
                        return JsonResponse.Error(SystemParam.ERROR_PHONE_NOT_VALID, SystemParam.MESSAGE_PHONE_NOT_VALID);
                    }
                    var news = await _newsRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.EventID) && x.IsActive.Equals(SystemParam.ACTIVE) && x.StartDate < DateTime.Now && x.EndDate > DateTime.Now);
                    if (news == null)
                        return JsonResponse.Error(SystemParam.ERROR_EVENT_INVALID, SystemParam.MESSAGE_EVENT_INVALID);
                    var eventParticipantOld = await _eventParticipantRepository.GetFirstOrDefaultAsync(x => x.Phone.Equals(input.Phone) && x.NewsID.Equals(input.EventID)
                        && x.CreatedDate.Day == DateTime.Now.Day && x.CreatedDate.Month == DateTime.Now.Month && x.CreatedDate.Year == DateTime.Now.Year);
                    if (eventParticipantOld != null)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_EVENT_ALREADY_PARTICIPATE_TODAY, SystemParam.MESSAGE_EVENT_ALREADY_PARTICIPATE_TODAY);
                    }

                    long totalMoney = 0, point = 0;
                    foreach (var item in input.ListBill)
                    {
                        totalMoney += item.Price;
                    }
                    var pointAddConfig = await _configRepository.GetFirstOrDefaultAsync(x => x.Key.Equals(SystemParam.POINT_ADD));
                    var orderValueConfig = await _configRepository.GetFirstOrDefaultAsync(x => x.Key.Equals(SystemParam.ORDER_VALUE));

                    if (pointAddConfig != null && orderValueConfig != null && pointAddConfig.ValueLong > 0 && orderValueConfig.ValueLong > 0)
                    {
                        point = totalMoney * pointAddConfig.ValueLong / orderValueConfig.ValueLong;
                    }
                    var eventParticipant = _mapper.Map<EventParticipant>(input);
                    eventParticipant.TotalMoney = totalMoney;
                    eventParticipant.StaffID = StaffID;
                    var cus = await _customerRepository.GetFirstOrDefaultAsync(x => x.Phone.Equals(input.Phone) && x.IsActive.Equals(SystemParam.ACTIVE));
                    if (cus != null)
                    {
                        _mapper.Map(input, cus);
                        cus.Point += point;
                        await _customerRepository.UpdateAsync(cus);
                        eventParticipant.CustomerID = cus.ID;
                        await _eventParticipantRepository.AddAsync(eventParticipant);
                        var title = "Bạn được cộng " + point + " điểm";
                        MemberPointHistory pointHistory = new MemberPointHistory()
                        {
                            Title = title,
                            Point = point,
                            Type = SystemParam.TYPE_MEMBER_HISTORY_EARN_POINT,
                            Balance = cus.Point,
                            CustomerID = cus.ID,
                            EventParticipantID = eventParticipant.ID,
                        };
                        await _memberPointHistoryRepository.AddAsync(pointHistory);
                        await _pushNotificationService.PushNotification(cus, SystemParam.TYPE_NOTIFICATION_EARN_POINT, title, null);
                        //Notification notify = new Notification()
                        //{
                        //    CustomerID = cus.ID,
                        //    Title = title,
                        //    Type = SystemParam.TYPE_NOTIFICATION_EARN_POINT,
                        //    Viewed = SystemParam.ACTIVE_FALSE,
                        //};
                        //await _notificationRepository.AddAsync(notify);
                    }
                    else
                    {
                        var newCus = _mapper.Map<Customer>(input);
                        newCus.Role = SystemParam.ROLE_CUSTOMER;
                        newCus.IsActive = SystemParam.ACTIVE;
                        newCus.Point = point;
                        newCus.OriginCustomer = SystemParam.CUSTOMER_ORIGIN_PG;
                        await _customerRepository.AddAsync(newCus);
                        eventParticipant.CustomerID = newCus.ID;
                        await _eventParticipantRepository.AddAsync(eventParticipant);
                        var title = "Bạn được cộng " + point + " điểm";
                        MemberPointHistory pointHistory = new MemberPointHistory()
                        {
                            Title = title,
                            Point = point,
                            Type = SystemParam.TYPE_MEMBER_HISTORY_EARN_POINT,
                            Balance = point,
                            CustomerID = newCus.ID,
                            EventParticipantID = eventParticipant.ID,
                        };
                        await _memberPointHistoryRepository.AddAsync(pointHistory);
                        Notification notify = new Notification()
                        {
                            CustomerID = newCus.ID,
                            Title = title,
                            Type = SystemParam.TYPE_MEMBER_HISTORY_EARN_POINT,
                            Viewed = SystemParam.ACTIVE_FALSE,
                        };
                        await _notificationRepository.AddAsync(notify);
                    }
                    foreach (var item in input.ListBill)
                    {
                        if (String.IsNullOrEmpty(item.Code))
                        {
                            return JsonResponse.Error(SystemParam.ERROR_EVENT_CODE_EMPTY, SystemParam.MESSAGE_EVENT_CODE_EMPTY);
                        }
                        if (item.Price == 0)
                        {
                            return JsonResponse.Error(SystemParam.ERROR_EVENT_PRICE_EMPTY, SystemParam.MESSAGE_EVENT_PRICE_EMPTY);
                        }
                        if (String.IsNullOrEmpty(item.ImageUrl))
                        {
                            return JsonResponse.Error(SystemParam.ERROR_EVENT_IMAGE_EMPTY, SystemParam.MESSAGE_EVENT_IMAGE_EMPTY);
                        }
                        if (item.StallID == 0)
                        {
                            return JsonResponse.Error(SystemParam.ERROR_EVENT_STALL_EMPTY, SystemParam.MESSAGE_EVENT_STALL_EMPTY);
                        }
                        var stall = await _stallRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(item.StallID) && x.Status.Equals(SystemParam.ACTIVE) && x.IsActive.Equals(SystemParam.ACTIVE));
                        if (stall == null)
                        {
                            return JsonResponse.Error(SystemParam.ERROR_STALL_NOT_FOUND, SystemParam.MESSAGE_STALL_NOT_FOUND);
                        }
                        var bill = _mapper.Map<Bill>(item);
                        bill.EventParticipantID = eventParticipant.ID;
                        await _billRepository.AddAsync(bill);
                    }
                    if (input.ListGift != null)
                    {
                        foreach (var item in input.ListGift)
                        {
                            var giftEvent = await _giftEventRepository.GetFirstOrDefaultAsync(x => x.GiftID.Equals(item) && x.NewsID.Equals(input.EventID) && x.IsActive.Equals(SystemParam.ACTIVE));
                            if (giftEvent != null)
                            {
                                if (giftEvent.Quantity > 0)
                                {
                                    giftEvent.Quantity--;
                                    giftEvent.QuantityExchanged++;
                                    await _giftEventRepository.UpdateAsync(giftEvent);
                                }
                                else
                                {
                                    return JsonResponse.Error(SystemParam.ERROR_EVENT_GIFT_EMPTY, SystemParam.MESSAGE_EVENT_GIFT_EMPTY);
                                }
                                var gift = await _giftRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(item) && x.IsActive.Equals(SystemParam.ACTIVE));
                                gift.Number--;
                                await _giftRepository.UpdateAsync(gift);
                                var giftEventParticipant = new GiftEventParticipant
                                {
                                    EventParticipantID = eventParticipant.ID,
                                    GiftID = gift.ID,
                                };
                                await _giftEventParticipantRepository.AddAsync(giftEventParticipant);
                            }
                        }
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

        public async Task<JsonResultModel> ScanQRCode(ScanQRCodeModel input)
        {
            try
            {
                var qr = await _qrCodeRepository.ScanQRCode(input.Code);
                if (qr == null)
                    return JsonResponse.Error(SystemParam.ERROR_SCAN_QRCODE_INVALID, SystemParam.MESSAGE_SCAN_QRCODE_INVALID);
                return JsonResponse.Success(qr);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}
