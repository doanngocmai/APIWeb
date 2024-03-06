using APIProject.Service.Models;
using APIProject.Domain.Models;
using APIProject.Service.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Service.Models.Authentication;
using APIProject.Service.Models.Customer;
using APIProject.Common.Models.Users;
using APIProject.Common.DTOs.Authentication;
using APIProject.Common.DTOs.Customer;
using PagedList.Core;
using APIProject.Common.DTOs.UsageFrequency;
using Microsoft.Extensions.Logging;
using static Microsoft.IO.RecyclableMemoryStreamManager;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace APIProject.Service.Interfaces
{
    public interface ICustomerService : IServices<Customer>
    {
        Task<JsonResultModel> Authenticate(LoginAppModel model, string secretKey, int timeout);
        Task<JsonResultModel> CheckPhoneLogin(string phone, IWebHostEnvironment webHostEnvironment);
        Task<JsonResultModel> CheckPhoneRegister(string phone, IWebHostEnvironment webHostEnvironment);
        Task<JsonResultModel> GetCustomers(int page, int limit, int? status, string searchKey, string startDate, string endDate,int? originCustomer);
        Task<JsonResultModel> GetCustomerDetail(int ID);
        Task<JsonResultModel> GetCustomerInfo(Customer cus);
        Task<JsonResultModel> GetUserInfo(int ID);
        Task<JsonResultModel> ChangeStatus(int customerID);
        Task<JsonResultModel> Register(RegisterModel model, string secretKey, int timeout);
        Task<JsonResultModel> ChangeAvatar(Customer customer, string ImageUrl);
        Task<JsonResultModel> UpdateCustomerInfo(Customer customer, UpdateCustomerInfoModel input);
        Task<JsonResultModel> UpdateUserWebInfo(ChangeCustomerInfoWebModel input);
        Task<JsonResultModel> DeleteCustomer(int ID);
        Task<JsonResultModel> CreateCustomer(CreateCustomer customer);
        Task<JsonResultModel> ExportExcelZaloOA(IWebHostEnvironment webHostEnvironment, HttpContext context);
        //Thêm thông tin khách hàng
        Task<JsonResultModel> CustomerInfo(CustomerInfo cus);
        //số lượng khách hàng tham gia sự kiện
        Task<JsonResultModel> GetNumberCustomerEvent(int page, int limit, string searchKey,int? EventID, string fromDate, string toDate);
        // Chi tiết khách hàng tham gia sự kiện
        Task<JsonResultModel> CustomerEventDetail(int page, int limit, string searchKey, int ID, string fromDate, string toDate);
        Task<JsonResultModel> GetListPercentageCustomer(int? EventID);
        Task<JsonResultModel> GetListPercentageGenderCustomer(int? EventID);
        Task<JsonResultModel> GetListCustomerChannelPercentage(int? EventID);
        Task<JsonResultModel> GetListCustomerPercentageProvinces(int? EventID);
        Task<JsonResultModel> GetListCustomerPercentageAge(int? EventID);
        Task<JsonResultModel> StatisticsGiftExchange(int page, int limit, string SeachKey, string fromDate, string toDate);
        Task<JsonResultModel> GetNumberOfGiftExchangeDetail(int page, int limit, int ID, string SeachKey, string fromDate, string toDate);
        Task<JsonResultModel> GetGiftVoucherDetail(int page, int limit, int ID, string SeachKey);
        Task<JsonResultModel> ImportCustomer(HttpContext context, IWebHostEnvironment webHostEnvironment);
        Task<JsonResultModel> ImportSampleCustomer(HttpContext context);
        Task<JsonResultModel> ExportExcelGiftExchange(int ID, string SeachKey, string fromDate, string toDate, IWebHostEnvironment webHostEnvironment, HttpContext context);
    }
}
