using APIProject.Common.Utils;
using APIProject.Domain.Models;
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
    [SwaggerTag("Thông báo")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        /// <summary>
        /// Danh sách thông báo
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetListNotification")]
        [Authorize]
        public async Task<JsonResultModel> GetListNotification(int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _notificationService.GetListNotification(page, limit, cus.ID);
        }
        /// <summary>
        /// Đọc thông báo
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("ReadNotification/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> ReadNotification(int ID)
        {
            return await _notificationService.ReadNotification(ID);
        }
        /// <summary>
        /// Đọc tất cả thông báo
        /// </summary>
        /// <returns></returns>
        [HttpPost("ReadAllNotification")]
        [Authorize]
        public async Task<JsonResultModel> ReadAllNotification()
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _notificationService.ReadAllNotification(cus.ID);
        }
    }
}
