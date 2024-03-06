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
    [SwaggerTag("Sơ đồ mặt bằng")]
    public class MapController : ControllerBase
    {
        private readonly IMapService _mapService;
        public MapController(IMapService mapService)
        {
            _mapService = mapService;
        }
        /// <summary>
        /// Danh sách sơ đồ mặt bằng
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListMap")]
        public async Task<JsonResultModel> GetListMap()
        {
            return await _mapService.GetListMap();
        }
    }
}
