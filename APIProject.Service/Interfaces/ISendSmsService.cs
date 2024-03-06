using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface ISendSmsService
    {
        Task<string> SendSms(string phone ,string otp, IWebHostEnvironment webHostEnvironment); 
    }
}
