using APIProject.Common.Utils;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
using Microsoft.AspNetCore.Hosting;
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
    [SwaggerTag("Upload file")]
    public class UploadController : ControllerBase
    {
        private readonly IUploadFileService _uploadFileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadController(IUploadFileService uploadFileService, IWebHostEnvironment webHostEnvironment)
        {
            _uploadFileService = uploadFileService;
            _webHostEnvironment = webHostEnvironment;
        }
        /// <summary>
        /// Upload nhiều ảnh
        /// </summary>
        /// <returns></returns>
        [HttpPost("UploadImage")]
        public JsonResultModel UploadImage()
        {
            try
            {
                var image = _uploadFileService.UploadImages(SystemParam.FILE_NAME, HttpContext, _webHostEnvironment);
                return JsonResponse.Success(image);
            }
            catch (Exception ex)
            {
                return JsonResponse.ServerError();
            }
        }
    }
}
