using APIProject.Common.DTOs.Config;
using APIProject.Common.Utils;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace APIProject.Controllers.Web
{
    [Route("api/web/[controller]")]
    [ApiExplorerSettings(GroupName = "Web")]
    [ApiController]
    [SwaggerTag("Cấu hình")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigService _ConfigService;
        public ConfigController(IConfigService ConfigService)
        {
                _ConfigService=ConfigService;
        }
        /// <summary>
        /// Phiếu khảo sát
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSurvery")]
        //[Authorize]
        public async  Task<JsonResultModel> GetSurvery()
        {
            return await _ConfigService.GetLinkSurvery();
        }
        /// <summary>
        /// Sửa link phiếu khảo sát
        /// </summary>
        /// <param name="pointAdd"></param>
        /// <param name="orderValue"></param>
        /// <returns></returns>
        [HttpPost("UpdateLinkSurvery")]
        //[Authorize]
        public async Task<JsonResultModel> UpdateLinkSurvery(string linkSurvery)
        {
            return await _ConfigService.UpdateLinkSurvery(linkSurvery);
        }


        /// <summary>
        /// Contact
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetContact")]
        //[Authorize]
        public async Task<JsonResultModel> GetContact()
        {
            return await _ConfigService.GetContact();
        }
        /// <summary>
        /// Sửa Contact
        /// </summary>
        /// <param name="pointAdd"></param>
        /// <param name="orderValue"></param>
        /// <returns></returns>
        [HttpPost("UpdateContact")]
        //[Authorize]
        public async Task<JsonResultModel> UpdateContact(string linkHotLine, string linkWebsite, string linkFacebook)
        {
            return await _ConfigService.UpdateContact(linkHotLine,linkWebsite, linkFacebook);
        }


        /// <summary>
        /// Event Info
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEventInfo")]
        //[Authorize]
        public async Task<JsonResultModel> GetEventInfo()
        {
            return await _ConfigService.GetEventInfo();
        }
        /// <summary>
        /// Sửa Event Info
        /// </summary>
        /// <param name="pointAdd"></param>
        /// <param name="orderValue"></param>
        /// <returns></returns>
        [HttpPost("UpdateEventInfo")]
        //[Authorize]
        public async Task<JsonResultModel> UpdateEventInfo(long pointAdd, long orderValue)
        {
            return await _ConfigService.UpdateEventInfo(pointAdd,orderValue);
        }
    }
}
