using APIProject.Common.DTOs.Staff;
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
    [SwaggerTag("Nhân viên")]
    public class StaffController : ControllerBase
    {

        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }
        /// <summary>
        /// Danh sách nhân viên
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="status"></param>
        /// <param name="searchKey"></param>
        /// <param name="searchProvince"></param>
        /// <returns></returns>
        [HttpGet("GetListStaff")]
        [Authorize]
        public async Task<JsonResultModel> GetListStaff(int page=SystemParam.PAGE_DEFAULT, int limit=SystemParam.LIMIT_DEFAULT, int? status= null, string searchKey=null, int? searchProvince = null)
        {
            return await _staffService.GetListStaff(page, limit, status, searchKey,searchProvince);
        }
        /// <summary>
        /// Thêm nhân viên
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("CreateStaff")]
        [Authorize]
        public async Task<JsonResultModel> CreateStaff([FromBody]AddStaffModel input)
        {
            return await _staffService.CreateStaff(input);
        }
        /// <summary>
        /// Cập nhật thông tin nhân viên
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("UpdateStaff")]
        [Authorize]
        public async Task<JsonResultModel> UpdateStaff([FromBody]UpdateStaffModel input)
        {
            return await _staffService.UpdateStaff(input);
        }
        /// <summary>
        /// Xoá nhân viên
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("DeleteStaff/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> DeleteStaff(int ID)
        {
            return await _staffService.DeleteStaff(ID);
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
            return await _staffService.ChangeStatus(ID);
        }
    }
}
