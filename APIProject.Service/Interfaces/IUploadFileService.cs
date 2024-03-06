using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Interfaces
{
    public interface IUploadFileService
    {
        List<string> UploadImages(string FileName, HttpContext context, IWebHostEnvironment webHostEnvironment);
    }
}
