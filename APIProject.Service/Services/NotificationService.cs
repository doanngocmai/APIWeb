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

namespace APIProject.Service.Services
{
    public class NotificationService: INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;

        public NotificationService(INotificationRepository notificationRepository, IMapper mapper, IHub sentryHub)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
        }

        public async Task<JsonResultModel> GetListNotification(int page, int limit, int cusID)
        {
            try
            {
                var model = await _notificationRepository.GetListNotification(page, limit, cusID);
                return JsonResponse.Success(model);
            }
            catch(Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> ReadAllNotification(int cusID)
        {
            try
            {
                var model = await _notificationRepository.GetAllAsync(x => x.CustomerID.Equals(cusID) && x.Viewed.Equals(SystemParam.NOTI_NOT_READ));
                foreach (var item in model)
                {
                    item.Viewed = SystemParam.NOTI_READ;
                    await _notificationRepository.UpdateAsync(item);
                }
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> ReadNotification(int ID)
        {
            try
            {
                var model = await _notificationRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID));
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_NOTIFICATION_NOT_FOUND, SystemParam.MESSAGE_NOTIFICATION_NOT_FOUND);
                }
                model.Viewed = SystemParam.NOTI_READ;
                await _notificationRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task CreateNotification(Customer cus, string content, int type, int? newsID)
        {
            try
            {
                var model = new Notification
                {
                    CustomerID = cus.ID,
                    Title = content,
                    Type = type,
                };
                await _notificationRepository.AddAsync(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);

            }
        }

        public async Task CreateNotification(IList<Customer> listCus, string content, int type, int? newsID)
        {
            try
            {
                List<Notification> model = new List<Notification>();
                foreach (var item in listCus)
                {
                    var noti = new Notification
                    {
                        CustomerID = item.ID,
                        Type = type,
                        Title = content,
                        NewsID = newsID,
                        IsAdmin = SystemParam.ACTIVE_FALSE,
                    };
                    model.Add(noti);
                }
                await _notificationRepository.AddManyAsync(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);

            }
        }
    }
}
