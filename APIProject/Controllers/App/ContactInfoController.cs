using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace APIProject.Controllers.App
{
    [Route("api/app/[controller]")]
    [ApiExplorerSettings(GroupName = "App")]
    [ApiController]
    [SwaggerTag("Thông tin liên hệ")]
    public class ContactInfoController : ControllerBase
    {
        private readonly IConfigService _ConfigService;
        public ContactInfoController(IConfigService configService)
        {
            _ConfigService = configService;
        }
        /// <summary>
        /// Thông tin liên hệ
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetContactInfo")]
        //[Authorize]
        public async Task<JsonResultModel> GetContactInfo()
        {
            return await _ConfigService.GetContactInfo();
        }
    }
}
