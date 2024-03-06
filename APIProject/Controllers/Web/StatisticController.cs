using APIProject.Common.Utils;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System;
using APIProject.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace APIProject.Controllers.Web
{
    [Route("api/web/[controller]")]
    [ApiExplorerSettings(GroupName = "Web")]
    [ApiController]
    [SwaggerTag("Thống kê")]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statisticService;
        private readonly ICustomerService _customerService;
        private readonly IStallService _stallService;
        private readonly ICategoryService _categoryService;
        private readonly IEventParticipantService _eventParticipantService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public StatisticController(ICustomerService customerService, IStallService stallService, ICategoryService categoryService, IEventParticipantService eventParticipantService, IStatisticService statisticService, IWebHostEnvironment webHostEnvironment)
        {
            _customerService = customerService;
            _stallService = stallService;
            _categoryService = categoryService;
            _eventParticipantService = eventParticipantService;
            _statisticService = statisticService;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Đếm số khách hàng tham gia sự kiện 
        /// </summary>
        /// <param name="EventID"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchKey"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [HttpGet("GetListNewCustomer")]
        [Authorize]//Đã thêm bộ lọc
        public async Task<JsonResultModel> GetListNewCustomer(int? EventID, int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT, string searchKey = null, string fromDate = null, string toDate = null)
        {
            return await _customerService.GetNumberCustomerEvent(page, limit, searchKey, EventID, fromDate, toDate);
        }
        /// <summary>
        /// chi tiết khách hàng tham gia sự kiện
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="SearchKey"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("CustomerEventDetail")]
        [Authorize]
        public async Task<JsonResultModel> CustomerEventDetail(int ID, string SearchKey, string fromDate, string toDate, int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT)
        {
            return await _customerService.CustomerEventDetail(page, limit, SearchKey, ID, fromDate, toDate);
        }
        /// <summary>
        /// Tỷ lệ dân cư khách hàng các xã phường trong quận Long Biên
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListPercentageCustomer")]
        [Authorize]//DONE
        public async Task<JsonResultModel> GetListPercentageCustomer(int? EventID)
        {
            return await _customerService.GetListPercentageCustomer(EventID);
        }
        /// <summary>
        /// Tỷ lệ giới tính khách hàng 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListPercentageGenderCustomer")]
        [Authorize]//dONE
        public async Task<JsonResultModel> GetListPercentageGenderCustomer(int? EventID)
        {
            return await _customerService.GetListPercentageGenderCustomer(EventID);
        }
        /// <summary>
        /// Tỷ lệ phần trăm kênh khách hàng tham gia sự kiện
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListCustomerChannelPercentage")]
        [Authorize]//DONE
        public async Task<JsonResultModel> GetListCustomerChannelPercentage(int? EventID)
        {
            return await _customerService.GetListCustomerChannelPercentage(EventID);
        }
        /// <summary>
        /// Tỷ lệ khách hàng các tỉnh lân cận 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListCustomerPercentageProvinces")]
        [Authorize]//done
        public async Task<JsonResultModel> GetListCustomerPercentageProvinces(int? EventID)
        {
            return await _customerService.GetListCustomerPercentageProvinces(EventID);
        }
        /// <summary>
        /// Tỷ lệ độ tuổi khách hàng 
        /// </summary>
        /// <param name="EventID"></param>
        /// <returns></returns>
        [HttpGet("GetListCustomerPercentageAge")]
        [Authorize]//done
        public async Task<JsonResultModel> GetListCustomerPercentageAge(int? EventID)
        {
            return await _customerService.GetListCustomerPercentageAge(EventID);
        }
        /// <summary>
        /// Thống kế số hoá đơn của các gian hàng( từ bé đến lớn)
        /// </summary>
        /// <param name="TypeOrder"></param>
        /// <param name="EventID"></param>
        /// <returns></returns>
        [HttpGet("GetSumBillStall")]
        public async Task<JsonResultModel> GetSumBillStall(int TypeOrder, int? EventID)
        {
            return await _stallService.GetSumBillStall(TypeOrder, EventID);
        }
        /// <summary>
        /// Xuất excel số hóa đơn của các giang hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExportExcelSumBillStall")]
        public async Task<JsonResultModel> ExportExcelSumBillStall()
        {
            return await _stallService.ExportExcelSumBillStall(_webHostEnvironment, HttpContext);
        }
        /// <summary>
        /// Thống kế giá trị hóa đơn trung bình của các nghành hàng( từ bé đến lớn)
        /// </summary>
        /// <param name="TypeOrder"></param>
        /// <param name="EventID"></param>
        /// <returns></returns>
        [HttpGet("GetSumBillCategory")]
        //[Authorize]
        public async Task<JsonResultModel> GetSumBillCategory(int TypeOrder, int? EventID)
        {
            return await _categoryService.GetSumBillCategory(TypeOrder, EventID);
        }

        /// <summary>
        /// Xuất excel thống kê giá trị đơn trung bình của các ngành hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExportExcelSumBillCategory")]
        public async Task<JsonResultModel> ExportExcelSumBillCategory()
        {
            return await _categoryService.ExportExcelSumBillCategory(_webHostEnvironment, HttpContext);
        }

        /// <summary>
        /// Tổng giá trị hoá đơn đổi quà trong chương trình
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="EventID"></param>
        /// <returns></returns>
        [HttpGet("GetTotalGiftBills")]
        //[Authorize]
        public async Task<JsonResultModel> GetTotalGiftBills(string fromDate, string toDate, int? EventID)
        {
            return await _statisticService.GetTotalGiftBills(fromDate, toDate, EventID);
        }

        /// <summary>
        /// Xuất excel tổng giá trị đơn đổi quà
        /// </summary>
        /// <param name="Fromdate"></param>
        /// <param name="Todate"></param>
        /// <returns></returns>
        [HttpGet("ExportExcelTotalGiftBills")]
        public async Task<JsonResultModel> ExportExcelTotalGiftBills(string Fromdate, string Todate)
        {
            return await _statisticService.ExportExcelTotalGiftBills(Fromdate, Todate, _webHostEnvironment, HttpContext);
        }

        /// <summary>
        /// Tổng số hoá đơn đổi quà trong chương trình
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="EventID"></param>
        /// <returns></returns>
        [HttpGet("GetCountGiftBills")]
        //[Authorize]
        public async Task<JsonResultModel> GetCountGiftBills(string fromDate, string toDate, int? EventID)
        {
            return await _statisticService.GetCountGiftBills(fromDate, toDate, EventID);
        }
        /// <summary>
        /// Xuất excel tổng số hóa đơn đổi quà trong chương trình 
        /// </summary>
        /// <param name="Fromdate"></param>
        /// <param name="Todate"></param>
        /// <returns></returns>
        [HttpGet("ExportExcelCountGiftBills")]
        public async Task<JsonResultModel> ExportExcelCountGiftBills(string Fromdate, string Todate)
        {
            return await _statisticService.ExportExcelCountGiftBills(Fromdate, Todate, _webHostEnvironment, HttpContext);
        }
        /// <summary>
        /// Thống kê số lượng khách hàng cùng kỳ
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("GetListSameFilter")]
        public async Task<JsonResultModel> GetListSameFilter(int? type)
        {
            return await _statisticService.GetListSameFilter(type);
        }
        /// <summary>
        /// Thống kê khách hàng 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [HttpGet("FilterEventParticipant")]
        public async Task<JsonResultModel> FilterEventParticipant(string fromDate, string toDate)
        {
            return await _statisticService.FilterEventParticipant(fromDate, toDate);
        }
        /// <summary>
        /// Thống kê số lượng khách hàng trong chiến dịch
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCustomerCampaign")]
        public async Task<JsonResultModel> GetCustomerCampaign(int ID)
        {
            return await _statisticService.GetCustomerCampaign(ID);
        }
        /// <summary>
        /// Thống kê số lần đổi quà tronng sự kiện
        /// </summary>
        /// <param name="SearchKey"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("StatisticsGiftExchange")]
        [Authorize]
        public async Task<JsonResultModel> StatisticsGiftExchange(string SearchKey, string fromDate, string toDate, int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT)

        {
            return await _customerService.StatisticsGiftExchange(page, limit, SearchKey, fromDate, toDate);
        }
        /// <summary>
        /// Chi tiết số lượt đổi quà
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="SearchKey"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetNumberOfGiftExchangeDetail")]
        public async Task<JsonResultModel> GetNumberOfGiftExchangeDetail(int ID, string SearchKey, string fromDate, string toDate, int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT)
        {
            return await _customerService.GetNumberOfGiftExchangeDetail(page, limit, ID, SearchKey, fromDate, toDate);
        }
        /// <summary>
        /// Chi tiết quà tặng trong chiến dịch
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="SearchKey"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetGiftVoucherDetail")]
        [Authorize]
        public async Task<JsonResultModel> GetGiftVoucherDetail(int ID, string SearchKey, int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT)
        {
            return await _customerService.GetGiftVoucherDetail(page, limit, ID, SearchKey);
        }
        /// <summary>
        /// Chi tiết hóa đơn đổi quà
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetEventParticipantDetail")]
        public async Task<JsonResultModel> GetEventParticipantDetail(int ID, int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT)
        {
            return await _eventParticipantService.GetEventParticipantDetail(page, limit, ID);
        }

        /// <summary>
        /// Xuất excel danh sách khách hàng đổi quà
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="SeachKey"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [HttpGet("ExportExcelGiftExchange")]
        public async Task<JsonResultModel> ExportExcelGiftExchange(int ID, string SeachKey, string fromDate, string toDate)
        {
            return await _customerService.ExportExcelGiftExchange(ID, SeachKey, fromDate, toDate, _webHostEnvironment, HttpContext);
        }
    }
}

