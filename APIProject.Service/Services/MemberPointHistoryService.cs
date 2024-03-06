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
    public class MemberPointHistoryService : IMemberPointHistoryService
    {
        private readonly IMemberPointHistoryRepository _memberPointHistoryRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        public MemberPointHistoryService(IMemberPointHistoryRepository memberPointHistoryRepository, IMapper mapper, IHub hub, ICustomerRepository customerRepository)
        {
            _memberPointHistoryRepository = memberPointHistoryRepository;
            _mapper = mapper;
            _sentryHub = hub;
            _customerRepository = customerRepository;
        }

        public async Task<JsonResultModel> GetListPointHistory(int page, int limit, int? type, int? customerID, string eventName, string fromDate, string toDate)
        {
            try
            {
                var model = await _memberPointHistoryRepository.GetListPointHistory(page, limit, type, customerID, eventName, fromDate, toDate);
                var data = new DataPagedListModel
                {
                    Data = model,
                    Limit = limit,
                    Page = page,
                    TotalItemCount = model.TotalItemCount,
                };
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetPointHistoryDetail(int page, int limit, int ID)
        {
            try
            {
                var model = await _memberPointHistoryRepository.GetPointHistoryDetail(page, limit, ID);

                var data = new DataPagedListModel
                {
                    Data = model,
                    Page = page,
                    Limit = limit,
                    TotalItemCount = model.TotalItemCount,
                };
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetListChangeHistory(int page, int limit, int? type, int? customerID, string searchKey, string fromDate, string toDate)
        {
            try
            {
                var model = await _memberPointHistoryRepository.GetListChangeHistory(page, limit, type, customerID, searchKey, fromDate, toDate);
                var data = new DataPagedListModel
                {
                    Data = model,
                    Limit = limit,
                    Page = page,
                    TotalItemCount = model.TotalItemCount,
                };
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetPointHistoryApp(int page, int limit, int? type,int customerID)
        {
            try
            {
                var cus = await _customerRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(customerID));
                var model = await _memberPointHistoryRepository.GetListPointHistoryModel(page, limit, type, customerID);
                var data = new 
                {
                    TotalPoint = cus.Point,
                    Data = model,
                };
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}
