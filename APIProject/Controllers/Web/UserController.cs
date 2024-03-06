using APIProject.Common.Models.Users;
using APIProject.Common.Utils;
using APIProject.Domain.Models;
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
    [SwaggerTag("User")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Danh sách user
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="SearchKey"></param>
        /// <param name="role"></param>
        /// <param name="status"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [HttpGet("GetListUser")]
        [Authorize]
        public async Task<JsonResultModel> GetListUser(int page=SystemParam.PAGE_DEFAULT, int limit=SystemParam.LIMIT_DEFAULT, string SearchKey=null, int? role = null, int? status = null, string fromDate = null, string toDate = null)
        {
            return await _userService.GetListUser(page, limit, SearchKey, role, status, fromDate, toDate);
        }
        /// <summary>
        /// Chi tiết tài khoản
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetUserDetail/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> GetUserDetail(int ID)
        {
            //var user = (User)HttpContext.Items["Payload"];
            return await _userService.GetUserDetail(ID);
        }
        /// <summary>
        /// Thay đổi trạng thái tài khoản
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("ChangeStatus/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> ChangeStatus(int ID)
        {
            return await _userService.ChangeStatus(ID);
        }
        /// <summary>
        /// Thêm tài khoản cho admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("CreateUser")]
        public async Task<JsonResultModel> CreateUser(CreateUserModel model)
        {
            return await _userService.CreateUser(model);
        }
        /// <summary>
        /// Cập nhật user
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("UpdateUser")]
        [Authorize]
        public async Task<JsonResultModel> UpdateUser(UpdateUserModel input)
        {
            return await _userService.UpdateUser(input);
        }
        /// <summary>
        /// Xoá user
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("DeleteUser/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> DeleteUser(int ID)
        {
            return await _userService.DeleteUser(ID);
        }
    }
}
