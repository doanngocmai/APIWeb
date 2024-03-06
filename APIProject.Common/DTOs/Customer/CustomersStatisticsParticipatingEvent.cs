using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Customer
{
    //thống kê khách hàng theo tuần 
    public class CustomersStatisticsParticipatingEvent
    {
        public DateTime Date { get; set; }
        public string Week { get; set; }
        public int Amount { get; set; }
        public List<CustomersSamePeriodModel> ListCustomerWeek { get; set; }
        public List<CustomersSamePeriodModelCompare> ListCustomerWeekCompare { get; set; }
    }
    public class CustomerStatisticsParticipatingSameQuarter
    {
        public List<CustomersSamePeriodModel> ListSameQuarter { get; set; }
        public List<CustomersSamePeriodModelCompare> ListSameQuarterCompare { get; set; }
    }
    public class CustomersSamePeriodModel//fgnm,.
    {
        public string Time { get; set; }
        public int Amount { get; set; }
    }
    public class CustomersSamePeriodModelCompare
    {
        public string Time { get; set; }
        public int AmountCompare { get; set; }
    }
    public class ExchangeGiftProgram
    {
        public string Time { get; set; }
        public long Values { get; set; }
    }
    public class NumberOfGiftExchange
    {
        public int ID { get; set; }
        public string NameCampaign { get; set; }
        public int NumberCustomer { get; set; }
        public int GiftExchange { get; set; }
        public int RemainingGift { get; set; }
        public int NumberBill { get; set; }
    }
    public class NumberOfGiftExchangeDetail
    {
        public int ID { get; set; }
        public string NameCus { get; set; }
        public string Phone { get; set; }
        public int NumberGift { get; set; }
        public List<GiftVoucher> GiftVouchers { get; set; }
        public long TotalAmount { get; set; }
        public string Date { get; set; }
    }

    public class GiftVoucher
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int QuantityExchanged { get; set; } // số lượng quà tặng và voucher đã đổi
        public int Quantity { get; set; }//tổng số lượng quà tặng
    }
}
