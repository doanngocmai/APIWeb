using APIProject.Common.DTOs.Event;
using APIProject.Common.Utils;
using APIProject.Domain.Models;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIProject.Controllers.Web
{
    [Route("api/web/[controller]")]
    [ApiExplorerSettings(GroupName = "Web")]
    [ApiController]
    [SwaggerTag("Chiến dịch")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _EventService;

        public EventController(IEventService festivalService)
        {
            _EventService = festivalService;
        }
        /// <summary>
        /// Danh sách chiến dịch
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchKey"></param>
        /// <param name="status"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [HttpGet("GetListEvent")]
        [Authorize]
        public async Task<JsonResultModel> GetListEvent(int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT, string searchKey = "", int? status = null, string fromDate = null, string toDate = null)
        {
            return await _EventService.GetListEvent(page, limit, searchKey, status, fromDate, toDate);
        }
        /// <summary>
        /// Danh sách chiến dịch thống kê biểu đồ
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListEventStatistic")]
        public async Task<JsonResultModel> GetListEventStatistic()
        {
            return await _EventService.GetListEventStatistic();
        }
        /// <summary>
        /// Chi tiết chiến dịch
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetEventDetail/{ID}")]
        //[Authorize]
        public async Task<JsonResultModel> GetEventDetail(int ID)
        {
            return await _EventService.GetEventDetail(ID);
        }
        /// <summary>
        /// Thêm mới tin tức
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("CreateEvent")]
        [Authorize]
        public async Task<JsonResultModel> CreateEvent(CreateEventModel input)
        {
            var user = (User)HttpContext.Items["Payload"];
            input.UserID = user.ID;
            return await _EventService.CreateEvent(input);
        }
        /// <summary>
        /// Cập nhật chiến dịch
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("UpdateEvent")]
        [Authorize]
        public async Task<JsonResultModel> UpdateEvent(UpdateEventModel input)
        {
            var user = (User)HttpContext.Items["Payload"];
            input.UserID = user.ID;
            return await _EventService.UpdateEvent(input);
        }
        /// <summary>
        /// Xoá chiến dịch
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("DeleteEvent/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> DeleteEvent(int ID)
        {
            return await _EventService.DeleteEvent(ID);
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
            return await _EventService.ChangeStatus(ID);
        }
    }
}
