using APIProject.Common.DTOs.Customer;
using APIProject.Common.DTOs.UsageFrequency;
using APIProject.Domain.Models;
using APIProject.Service.Models;
using APIProject.Service.Models.Customer;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<int> CountEmailOfCustomer(string Email);
        Task<int> CountPhoneOfCustomer(string Phone);
        Task<int> CountCustomer();
        Task<CustomerAppModel> GetCustomerInfo(Customer cus);
        Task<CustomerDetailModel> GetCustomerDetail(int ID);
        Task<IPagedList<CustomerWebModel>> GetCustomers(int page, int limit, int? status, string searchKey, string fromDate, string toDate,int? originCustomer);
        //Số khách hàng tham gia sự kiện
        Task<IPagedList<CustomerNumberEvent>> GetNumberCustomerEvent(int page, int limit, string searchKey,int? IDNews, string fromDate, string toDate);
        //Chi tiết khách hàng tham gia sự kiện
        Task<IPagedList<CustomerEventDetail>> CustomerEventDetail(int page, int limit, string searchKey, int ID, string fromDate, string toDate);
        // tỷ lệ phần trăm các phường trong quận Long Biên 
        Task<List<PercentageCustomers>> GetListPercentageCustomer(int? EventID);
        // tỷ lệ phần trăm giới tính khách hàng
        Task<List<PercentageGenderCustomer>> GetListPercentageGenderCustomer(int? EventID);
        // tỷ lệ phần trăm kênh khách hàng tham gia sự kiện
        Task<List<CustomerChannelPercentage>> GetListCustomerChannelPercentage(int? EventID);
        //tỷ lệ phần trăm khách hàng các tỉnh lân cận 
        Task<List<CustomerPercentageProvinces>> GetListCustomerPercentageProvinces(int? EventID);
        //tỷ lệ phần trăm độ tuổi khách hàng 
        Task<List<CustomerPercentageAge>> GetListCustomerPercentageAge(int? EventID);

        //Bill
        Task<long> SumBillAmount(DateTime FromDate, DateTime Todate);              // Tổng giá trị hoá đơn quà tặng 
        Task<int> CountEventParticipant(DateTime FromDate, DateTime Todate);        // Đếm số khách hàng tham gia sự kiện
        Task<int> CountCustomerCampaign(int ID, DateTime Date); // Đếm số lượng khách hàng trong chiến dịch
        Task<int> CountSumBillCampaign(int EventID, DateTime Date); // Đếm số lượng hoá đơn trong chiến dịch
        Task<long> CountTotalPriceCampaign(int EventID, DateTime Date); // Tổng tiền hoá đơn trong chiến dịch
        Task<int> CountGiftBills(DateTime FromDate, DateTime Todate);               // Số lượng hoá đơn quà tặng 
        Task<int> CountEventParticipantTime(DateTime Date);                           // Thống kê khách hàng theo ngày

        // campaign 

        Task<IPagedList<NumberOfGiftExchange>> StatisticsGiftExchange(int page, int limit, string SeachKey, string fromDate, string toDate);
        Task<IPagedList<NumberOfGiftExchangeDetail>> GetNumberOfGiftExchangeDetail(int page, int limit, int ID, string SeachKey, string fromDate, string toDate);
        Task<List<NumberOfGiftExchangeDetail>> GetALLNumberOfGiftExchangeDetail(int ID, string SeachKey, string fromDate, string toDate);
        Task<IPagedList<GiftVoucher>> GetGiftVoucherDetail(int page, int limit, int ID, string SeachKey);

    }
}
