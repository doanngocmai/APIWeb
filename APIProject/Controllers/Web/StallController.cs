using APIProject.Common.DTOs.Stall;
using APIProject.Common.Utils;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace APIProject.Controllers.Web
{
    [Route("api/web/[controller]")]
    [ApiExplorerSettings(GroupName = "Web")]
    [ApiController]
    [SwaggerTag("Gian hàng")]
    public class StallController : ControllerBase
    {
        private readonly IStallService _stallService;
        public StallController(IStallService stallService)
        {
            _stallService = stallService;
        }

        /// <summary>
        /// Danh sách gian hàng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="floor"></param>
        /// <param name="status"></param>
        /// <param name="searchKey"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("GetListStall")]
        [Authorize]
        public async Task<JsonResultModel> GetListStall(int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT, int? floor = null, int? status = null, string searchKey = null, int? type = null)
        {
            return await _stallService.GetListStall(page, limit, floor, status, searchKey, type);
        }
        /// <summary>
        /// Chi tiết gian hàng
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetStallDetail/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> GetStallDetail(int ID)
        {
            return await _stallService.GetStallDetail(ID);
        }
        /// <summary>
        /// Thay đổi trạng thái gian hàng
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("ChangeStatus/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> ChangeStatus(int ID)
        {
            return await _stallService.ChangeStatus(ID);
        }
        /// <summary>
        /// Thêm mới gian hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("CreateStall")]
        [Authorize]
        public async Task<JsonResultModel> CreateStall(CreateStallModel input)
        {
            return await _stallService.CreateStall(input);
        }

        /// <summary>
        /// Cập nhật gian hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("UpdateStall")]
        [Authorize]
        public async Task<JsonResultModel> UpdateStall(UpdateStallModel input)
        {
            return await _stallService.UpdateStall(input);
        }
        /// <summary>
        /// Xoá gian hàng
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("DeleteStall/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> DeleteStall(int ID)
        {
            return await _stallService.DeleteStall(ID);
        }
    }
}
