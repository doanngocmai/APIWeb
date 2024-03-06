using APIProject.Common.DTOs.News;
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
    [SwaggerTag("Tin tức")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }
        /// <summary>
        /// Danh sách tin tức 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchKey"></param>
        /// <param name="type">1: Khuyến mại , 3: Tuyển dụng ,4: Sự kiện</param>
        /// <param name="status"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [HttpGet("GetListNews")]
        [Authorize]
        public async Task<JsonResultModel> GetListNews(int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT, string searchKey = "", int? type = null, int? status = null, string fromDate = null, string toDate = null)
        {
            return await _newsService.GetListNews(page, limit, searchKey, type, status, fromDate, toDate);
        }
        /// <summary>
        /// chi tiết tin tức
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetNewsDetail/{ID}")]
        //[Authorize]
        public async Task<JsonResultModel> GetNewsDetail(int ID)
        {
            return await _newsService.GetNewsDetail(ID);
        }
        /// <summary>
        /// Thêm mới tin tức
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("CreateNews")]
        [Authorize]
        public async Task<JsonResultModel> CreateNews(CreateNewsModel input)
        {
            var user = (User)HttpContext.Items["Payload"];
            input.UserID = user.ID;
            return await _newsService.CreateNews(input);
        }
        /// <summary>
        /// Cập nhật tin tức
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("UpdateNews")]
        [Authorize]
        public async Task<JsonResultModel> UpdateNews(UpdateNewsModel input)
        {
            var user = (User)HttpContext.Items["Payload"];
            input.UserID = user.ID;
            return await _newsService.UpdateNews(input);
        }
        /// <summary>
        /// Xoá tin tức
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("DeleteNews/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> DeleteNews(int ID)
        {
            return await _newsService.DeleteNews(ID);
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
            return await _newsService.ChangeStatus(ID);
        }
    }
}
