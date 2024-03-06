using APIProject.Common.Utils;
using APIProject.Domain.Models;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace APIProject.Controllers.App
{
    [Route("api/app/[controller]")]
    [ApiExplorerSettings(GroupName = "App")]
    [ApiController]
    [SwaggerTag("Voucher")]
    public class VoucherController : ControllerBase
    {
        private readonly IGiftService _GiftService;
        public VoucherController(IGiftService giftService)
        {
            _GiftService = giftService;
        }
        /// <summary>
        /// Danh sách voucher 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetListVoucher")]
        public async Task<JsonResultModel> GetListVoucher( int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT)
        {
            return await _GiftService.GetListVoucher(page, limit);
        }
        /// <summary>
        /// Danh sách voucher hết hạn sử dụng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetMyListVoucherExpired")]
        [Authorize]
        public async Task<JsonResultModel> GetMyListVoucherExpired(int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _GiftService.GetMyListVoucherExpired(page, limit,cus.ID);
        }
        /// <summary>
        /// Danh sách voucher của tôi và đã sử dụng
        /// </summary>
        /// <param name="status"> 1: Voucher của tôi, 2: Voucher đã sử dụng</param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetMyListVoucher")]
        [Authorize]
        public async Task<JsonResultModel> GetMyListVoucher(int status , int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _GiftService.GetMyListVoucher(page, limit, status, cus.ID);
        }
        /// <summary>
        /// Chi tiết voucher của tôi
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetMyVoucherDetail/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> GetMyVoucherDetail(int ID)
        {
            return await _GiftService.GetMyVoucherDetail(ID);
        }
        /// <summary>
        /// Chi tiết voucher
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetVoucherDetail/{ID}")]
        public async Task<JsonResultModel> GetVoucherDetail(int ID)
        {
            return await _GiftService.GetVoucherDetail(ID);
        }
        /// <summary>
        /// Đổi voucher
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("ExchangeVoucher/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> ExchangeVoucher(int ID)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _GiftService.ExchangeVoucher(ID, cus.ID);
        }
        /// <summary>
        /// Sử dụng voucher
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("UseVoucher/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> UseVoucher(int ID)
        {
            return await _GiftService.UseVoucher(ID);
        }
    }
}
