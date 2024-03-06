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
    [SwaggerTag("Tổng quan")]
    public class OverViewController : ControllerBase
    {
        private readonly IHomeService _Home;
        public OverViewController(IHomeService home)
        {
            _Home=home;
        }
        [HttpGet("OverView")]
        //[Authorize]
        public async Task<JsonResultModel> OverView()
        {
            return await _Home.OverView();
        }
    }
}
