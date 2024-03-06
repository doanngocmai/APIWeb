using APIProject.Common.DTOs.EventParticipant;
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

namespace APIProject.Controllers.App
{
    [Route("api/app/[controller]")]
    [ApiExplorerSettings(GroupName = "App")]
    [ApiController]
    [SwaggerTag("Tham gia sự kiện")]
    public class EventParticipantController : ControllerBase
    {
        private readonly IEventParticipantService _eventParticipantService;
        public EventParticipantController(IEventParticipantService eventParticipantService)
        {
            _eventParticipantService = eventParticipantService;
        }
        /// <summary>
        /// Tạo mã QR
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("CreateQRCode")]
        public async Task<JsonResultModel> CreateQRCode(CreateQRCodeModel input)
        {
            return await _eventParticipantService.CreateQRCode(input);
        }
        /// <summary>
        /// Scan QR
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("ScanQRCode")]
        [Authorize]
        public async Task<JsonResultModel> ScanQRCode(ScanQRCodeModel input)
        {
            return await _eventParticipantService.ScanQRCode(input);
        }
        /// <summary>
        /// Nhập thông tin khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("InputCustomerInfo")]
        [Authorize]
        public async Task<JsonResultModel> InputCustomerInfo(EventParticipantModel input)
        {
            var staff = (Customer)HttpContext.Items["Payload"];
            return await _eventParticipantService.InputCustomerInfo(input,staff.ID);
        }
        /// <summary>
        /// Lấy danh sách sự kiện
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListEvent")]
        public async Task<JsonResultModel> GetListEvent()
        {
            return await _eventParticipantService.GetListEvent();
        }
        /// <summary>
        /// Lấy danh sách kênh tham gia sự kiện
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListEventChannel")]
        public async Task<JsonResultModel> GetListEventChannel()
        {
            return await _eventParticipantService.GetListEventChannel();
        }
        /// <summary>
        /// Lấy danh sách gian hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListStall")]
        public async Task<JsonResultModel> GetListStall()
        {
            return await _eventParticipantService.GetListStall();
        }
        /// <summary>
        /// Lấy danh sách quà tặng
        /// </summary>
        /// <param name="eventID"></param>
        /// <returns></returns>
        [HttpGet("GetListEventGift")]
        public async Task<JsonResultModel> GetListEventGift(int eventID)
        {
            return await _eventParticipantService.GetListEventGift(eventID);
        }
    }
}
