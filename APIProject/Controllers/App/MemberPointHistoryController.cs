using APIProject.Common.Utils;
using APIProject.Domain.Models;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace APIProject.Controllers.App
{
    [Route("api/App/[controller]")]
    [ApiExplorerSettings(GroupName = "App")]
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
        /// <param name="type">1: Tích điểm , 2:Đổi điểm </param>
        /// <returns></returns>
        [HttpGet("GetPointHistory")]
        [Authorize]
        public async Task<JsonResultModel> GetPointHistory(int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT, int? type = null)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _memberPointHistoryService.GetPointHistoryApp(page, limit, type, cus.ID);
        }
    }
}
