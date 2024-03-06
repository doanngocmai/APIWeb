using APIProject.Common.DTOs.Staff;
using APIProject.Common.Utils;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models.Authentication;
using APIProject.Service.Models.Customer;
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
    public class StaffService:IStaffService
    {
        private readonly IStaffRepository _staffRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEventParticipantRepository _eventParticipantRepository;
        private readonly IMemberPointHistoryRepository _memberPointHistory;
        private readonly IBillRepository _billRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        public StaffService(IStaffRepository staffRepository, INotificationRepository notificationRepository, ICustomerRepository customerRepository, IEventParticipantRepository eventParticipantRepository, IMemberPointHistoryRepository memberPointHistory, IBillRepository billRepository, IMapper mapper, IHub sentryHub)
        {
            _staffRepository = staffRepository;
            _customerRepository = customerRepository;
            _eventParticipantRepository = eventParticipantRepository;
            _memberPointHistory = memberPointHistory;
            _billRepository = billRepository;
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
        }

        public async Task<JsonResultModel> GetListStaff(int page, int limit, int? status, string searchKey, int? searchProvince)
        {
            try
            {
                var model = await _staffRepository.GetListStaff(page, limit, status, searchKey, searchProvince);
                var data = new DataPagedListModel()
                {
                    Data = model,
                    Page = page,
                    Limit = limit,
                    TotalItemCount = model.TotalItemCount,
                };
                return JsonResponse.Success(data);
            }
            catch(Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> CreateStaff(AddStaffModel input)
        {
            try
            {
                if (!Util.validPhone(input.Phone))
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_PHONE_INVALID, SystemParam.MESSAGE_REGISTER_PHONE_INVALID);
                var checkPhone = await _staffRepository.GetFirstOrDefaultAsync(x => x.Phone.Equals(input.Phone) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (checkPhone != null)
                    return JsonResponse.Error(SystemParam.ERROR_REGISTER_PHONE_EXIST, SystemParam.MESSAGE_REGISTER_PHONE_EXIST);
                var model = _mapper.Map<Customer>(input);
                model.Role = SystemParam.ROLE_STAFF;
                await _staffRepository.AddAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> UpdateStaff(UpdateStaffModel input)
        {
            try
            {
                var model = await _staffRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if(model == null) { return JsonResponse.Error(SystemParam.ERROR_NOT_FOUND_STAFF, SystemParam.MESSAGE_NOT_FOUND_STAFF); }
                _mapper.Map(input, model);
                await _staffRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> DeleteStaff(int ID)
        {
            try
            {
                var cus = await _staffRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (cus == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_CUSOTMER_NOT_EXSIST, SystemParam.MESSAGE_CUSOTMER_NOT_EXSIST);
                }
                cus.IsActive = SystemParam.ACTIVE_FALSE;
                await _staffRepository.UpdateAsync(cus);
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
                Customer customer = await _staffRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (customer == null) JsonResponse.Error(SystemParam.ERROR_NOT_FOUND_CUSTOMER, SystemParam.MESSAGE_NOT_FOUND_CUSTOMER);
                if (customer.Status == SystemParam.ACTIVE_FALSE)
                {
                    customer.Status = SystemParam.ACTIVE;
                }
                else
                {
                    customer.Status = SystemParam.ACTIVE_FALSE;
                }

                await _staffRepository.UpdateAsync(customer);
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
