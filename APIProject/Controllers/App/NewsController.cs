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
    [SwaggerTag("Tin tức")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }
        /// <summary>
        /// Lấy danh sách tin tức
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchKey"></param>
        /// <param name="type">1:Khuyến mại ,3:Tuyển dụng , 4: Sự kiện, 5: Tiện ích</param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [HttpGet("GetListNews")]
        public async Task<JsonResultModel> GetListNews(int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT, string searchKey = "", int? type = null, string fromDate = null, string toDate = null)
        {
            return await _newsService.GetListNews(page, limit, searchKey, type, fromDate, toDate);
        }
        /// <summary>
        /// Chi tiết tin tức
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetNewsDetail/{ID}")]
        public async Task<JsonResultModel> GetNewsDetail(int ID)
        {
            return await _newsService.GetNewsDetailApp(ID);
        }
        /// <summary>
        /// Tin tức liên quan
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetNewsRelated/{ID}")]
        public async Task<JsonResultModel> GetNewsRelated(int ID)
        {
            return await _newsService.GetNewsRelated(ID);
        }
    }
}
