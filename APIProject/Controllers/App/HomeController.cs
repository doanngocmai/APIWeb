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
    [SwaggerTag("Trang chủ")]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;
        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }
        /// <summary>
        /// Màn trang chủ
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetHome")]
        public async Task<JsonResultModel> GetHome()
        {
            return await _homeService.GetHome();
        }
        /// <summary>
        /// Màn trang chủ nhân viên
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetHomeStaff")]
        public async Task<JsonResultModel> GetHomeStaff()
        {
            return await _homeService.GetHomeStaff();
        }
        /// <summary>
        /// Lấy Timezone
        /// </summary>
        /// <returns></returns>
        [HttpGet("CheckTimezone")]
        public async Task<JsonResultModel> CheckTimezone()
        {
            return await _homeService.CheckTimezone();
        }
    }
}
