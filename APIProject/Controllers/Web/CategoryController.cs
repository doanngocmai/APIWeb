using APIProject.Common.DTOs.Category;
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
    [SwaggerTag("Ngành hàng")]
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        /// <summary>
        /// danh sách Ngành hàng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetListCategory")]
        [Authorize]
        public async Task<JsonResultModel> GetListCategory(int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT)
        {
            return await _categoryService.GetListCategory(page, limit);
        }

        /// <summary>
        /// Thêm mới Ngành hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("CreateCategory")]
        [Authorize]
        public async Task<JsonResultModel> CreateCategory(CreateCategoryModel input)
        {
            return await _categoryService.CreateCategory(input);
        }
        /// <summary>
        /// Cập nhật Ngành hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("UpdateCategory")]
        [Authorize]
        public async Task<JsonResultModel> UpdateCategory(CategoryModel input)
        {
            return await _categoryService.UpdateCategory(input);
        }
        /// <summary>
        /// Xoá Ngành hàng
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("DeleteCategory/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> DeleteCategory(int ID)
        {
            return await _categoryService.DeleteCategory(ID);
        }
    }
}
