using APIProject.Common.DTOs.Gift;
using APIProject.Common.Utils;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace APIProject.Controllers.Web
{
    [Route("api/web/[controller]")]
    [ApiExplorerSettings(GroupName = "Web")]
    [ApiController]
    [SwaggerTag("Quà tặng")]
    public class GiftController : ControllerBase
    {
        private readonly IGiftService _giftService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public GiftController(IGiftService giftService, IWebHostEnvironment webHostEnvironment)
        {
            _giftService = giftService;
            _webHostEnvironment = webHostEnvironment;
        }
        /// <summary>
        /// Danh sách quà tặng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchKey"></param>
        /// <param name="type"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [HttpGet("GetListGift")]
        [Authorize]
        public async Task<JsonResultModel> GetListGift(int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT, string searchKey = null, int? type = null, string fromDate = null, string toDate = null)
        {
            return await _giftService.GetListGift(page, limit, searchKey, type, fromDate, toDate);
        }
        /// <summary>
        /// Chi tiết quà tặng
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetGiftDetail/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> GetGiftDetail(int ID)
        {
            return await _giftService.GetGiftDetail(ID);
        }
        /// <summary>
        /// Danh sách đổi quà
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchKey"></param>
        /// <param name="type"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [HttpGet("GetListExchangeGift")]
        [Authorize]
        public async Task<JsonResultModel> GetListExchangeGift(int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT, string searchKey = null, int? type = null, string fromDate = null, string toDate = null)
        {
            return await _giftService.GetListExchangeGift(page, limit, searchKey, type, fromDate, toDate);
        }
        /// <summary>
        /// Danh sách mã quà tặng/voucher
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchKey"></param>
        /// <param name="status"></param>
        /// <param name="GiftID"></param>
        /// <returns></returns>
        [HttpGet("GetListGiftCode")]
        [Authorize]
        public async Task<JsonResultModel> GetListGiftCode(int GiftID, int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT, string searchKey = null, int? status = null)
        {
            return await _giftService.GetListGiftCode(page, limit, searchKey, status, GiftID);
        }
        /// <summary>
        /// Thêm quà tặng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("CreateGift")]
        [Authorize]
        public async Task<JsonResultModel> CreateGift(CreateGiftModel input)
        {
            return await _giftService.CreateGift(input);
        }
        /// <summary>
        /// Cập nhật quà tặng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("UpdateGift")]
        [Authorize]
        public async Task<JsonResultModel> UpdateGift(UpdateGiftModel input)
        {
            return await _giftService.UpdateGift(input);
        }
        /// <summary>
        /// Xoá quà tặng
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("DeleteGift/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> DeleteGift(int ID)
        {
            return await _giftService.DeleteGift(ID);
        }
        /// <summary>
        /// Cập nhật trạng thái
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("ChangeStatus/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> ChangeStatus(int ID)
        {
            return await _giftService.ChangeStatus(ID);
        }
        /// <summary>
        /// Thêm mới mã voucher/quà tặng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("CreateGiftCode")]
        [Authorize]
        public async Task<JsonResultModel> CreateGiftCode([FromBody] CreateGiftCodeModel input)
        {
            return await _giftService.CreateGiftCode(input);
        }
        /// <summary>
        /// Cập nhật mã quà tặng/voucher
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("UpdateGiftCode")]
        [Authorize]
        public async Task<JsonResultModel> UpdateGiftCode([FromBody] UpdateGiftCodeModel input)
        {
            return await _giftService.UpdateGiftCode(input);
        }
        /// <summary>
        /// Xoá mã quà tặng/voucher
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("DeleteGiftCode/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> DeleteGiftCode(int ID)
        {
            return await _giftService.DeleteGiftCode(ID);
        }
        /// <summary>
        /// Mẫu Excel
        /// </summary>
        /// <returns></returns>
        [HttpGet("ImportVoucherCode")]
        //[Authorize]
        public async Task<JsonResultModel> ImportVoucherCode()
        {
            return await _giftService.ImportVoucherCode(HttpContext);
        }
        /// <summary>
        /// Thêm voucher bằng file Excel
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("ImportVoucher")]
        public async Task<JsonResultModel> ImportVoucher([FromForm] InportVoucherModel input)
        {
            return await _giftService.ImportVoucherCode(input.GiftID, HttpContext, _webHostEnvironment);
        }


    }
}
