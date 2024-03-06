using APIProject.Common.Utils;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace APIProject.Controllers.App
{
    [Route("api/app/[controller]")]
    [ApiExplorerSettings(GroupName = "App")]
    [ApiController]
    [SwaggerTag("Gian hàng")]
    public class StallController : ControllerBase
    {
        private readonly IStallService _stallService;
        private readonly ICategoryService _categoryService;
        public StallController(IStallService stallService, ICategoryService categoryService)
        {
            _stallService = stallService;
            _categoryService = categoryService;
        }
        /// <summary>
        /// Danh sách gian hàng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="categoryID">Ngành hàng</param>
        /// <param name="floor">Tầng : 1,2,3</param>
        /// <param name="searchKey"></param>
        /// <param name="orderBy">1: Sắp xếp theo: A-Z , 2: Sắp xếp theo: Z-A, 3: Sắp xếp theo: Gian hàng mới</param>
        /// <returns></returns>
        [HttpGet("GetListStall")]
        public async Task<JsonResultModel> GetListStall(int? page = null, int? limit = null, int? categoryID = null, int? floor = null, string searchKey = null, int? orderBy = null)
        {
            return await _stallService.GetListStall(page, limit, floor, searchKey, categoryID, orderBy);
        }
        /// <summary>
        /// Chi tiết gian hàng
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetStallDetail/{ID}")]
        public async Task<JsonResultModel> GetStallDetail(int ID)
        {
            return await _stallService.GetStallDetail(ID);
        }
        /// <summary>
        /// Danh sách gian hàng liên quan
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetStallRelated/{ID}")]
        public async Task<JsonResultModel> GetStallRelated(int ID)
        {
            return await _stallService.GetStallRelated(ID);
        }
        /// <summary>
        /// Danh sách ngành hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListCategory")]
        public async Task<JsonResultModel> GetListCategory()
        {
            return await _categoryService.GetListCategory();
        }
    }
}
