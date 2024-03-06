using APIProject.Common.DTOs.Customer;
using APIProject.Common.DTOs.EventParticipant;
using APIProject.Common.Utils;
using APIProject.Domain.Models;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace APIProject.Controllers.Web
{

    [Route("api/web/[controller]")]
    [ApiExplorerSettings(GroupName = "Web")]
    [ApiController]
    [SwaggerTag("Khách hàng")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IEventParticipantService _eventParticipantService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomerController(ICustomerService customerService, IEventParticipantService eventParticipantService, IWebHostEnvironment webHostEnvironment)
        {
            _customerService = customerService;
            _eventParticipantService = eventParticipantService;
            _webHostEnvironment = webHostEnvironment;
        }
        /// <summary>
        /// Danh sách khách hàng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="status"></param>
        /// <param name="searchKey"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="originCustomer">1:Sử dụng App , 2: PG tạo</param>
        /// <returns></returns>
        [HttpGet("GetListCustomer")]
        [Authorize]
        public async Task<JsonResultModel> GetListCustomer(int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT, int? status = null, string searchKey = null, string fromDate = null, string toDate = null, int? originCustomer = null)
        {
            return await _customerService.GetCustomers(page, limit, status, searchKey, fromDate, toDate, originCustomer);
        }
        /// <summary>
        /// Chi tiết khách hàng
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetCustomerDetail/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> GetCustomerDetail(int ID)
        {
            return await _customerService.GetCustomerDetail(ID);
        }
        /// <summary>
        /// Thay đổi trạng thái
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("ChangeStatus/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> ChangeStatus(int ID)
        {
            return await _customerService.ChangeStatus(ID);
        }
        /// <summary>
        /// Thêm khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("CreateCustomer")]
        [Authorize]
        public async Task<JsonResultModel> CreateCustomer(CreateCustomer input)
        {
            return await _customerService.CreateCustomer(input);
        }
        /// <summary>
        /// Export excel khách hàng zalo OA
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExportExcelZaloOA")]
        public async Task<JsonResultModel> ExportExcelZaloOA()
        {
            return await _customerService.ExportExcelZaloOA(_webHostEnvironment, HttpContext);
        }
        /// <summary>
        /// Mẫu import khách hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet("ImportSampleCustomer")]
        public async Task<JsonResultModel> ImportSampleCustomer()
        {
            return await _customerService.ImportSampleCustomer(HttpContext);
        }
        /// <summary>
        /// Import khách hàng
        /// </summary>
        /// <returns></returns>
        [HttpPost("ImportCustomer")]
        public async Task<JsonResultModel> ImportCustomer()
        {
            return await _customerService.ImportCustomer(HttpContext, _webHostEnvironment);
        }
    }
}
