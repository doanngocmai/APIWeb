using APIProject.Common.Utils;
using APIProject.Domain.Models;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace APIProject.Controllers.Web
{
    [Route("api/Web/[controller]")]
    [ApiExplorerSettings(GroupName = "Web")]
    [ApiController]
    [SwaggerTag("Lịch sử điểm")]
    public class MemberPointHistoryController : ControllerBase
    {
        private readonly IMemberPointHistoryService _memberPointHistoryService;

        public MemberPointHistoryController(IMemberPointHistoryService memberPointHistoryService)
        {
            _memberPointHistoryService = memberPointHistoryService;
        }
        /// <summary>
        /// Danh sách lịch sử tích điểm
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="type"></param>
        /// <param name="customerID"></param>
        /// <param name="eventName"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [HttpGet("GetListPointHistory")]
        [Authorize]
        public async Task<JsonResultModel> GetListPointHistory(int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT, int? type = null, int? customerID = null, string eventName = null, string fromDate = null, string toDate = null)
        {
            return await _memberPointHistoryService.GetListPointHistory(page, limit, type, customerID, eventName, fromDate, toDate);
        }
        /// <summary>
        /// Chi tiết lịch sử tích điểm
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetPointHistoryDetail/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> GetPointHistoryDetail(int ID, int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT)
        {
            return await _memberPointHistoryService.GetPointHistoryDetail(page, limit, ID);
        }
        /// <summary>
        /// Danh sách lịch sử đổi điểm
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="type"></param>
        /// <param name="customerID"></param>
        /// <param name="searchKey"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [HttpGet("GetListChangeHistory")]
        [Authorize]
        public async Task<JsonResultModel> GetListChangeHistory(int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT, int? type = null, int? customerID = null, string searchKey = null, string fromDate = null, string toDate = null)
        {
            return await _memberPointHistoryService.GetListChangeHistory(page, limit, type, customerID, searchKey, fromDate, toDate);
        }
    }
}
