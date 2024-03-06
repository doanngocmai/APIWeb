using API.Models;
using APIProject.Common.DTOs.Authentication;
using APIProject.Common.Models.Users;
using APIProject.Common.Utils;
using APIProject.Domain.Models;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Models.Authentication;
using APIProject.Service.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sentry;
using SmsSami;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Controllers.App
{
    [Route("api/app/[controller]")]
    [ApiExplorerSettings(GroupName = "App")]
    [ApiController]
    [SwaggerTag("Authen")]
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration _Configuration;
        private readonly ICustomerService _customerService;
        private readonly ISendSmsService _sendSmsService;
        private readonly string secretKey;
        private readonly int timeout;
        private readonly IHub _sentryHub;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public AuthenticationController(IConfiguration configuration, ICustomerService customerService, IHub hub, ISendSmsService sendSmsService, IWebHostEnvironment webHostEnvironment)
        {
            _Configuration = configuration;
            _customerService = customerService;
            try
            {
                secretKey = _Configuration["AppSettings:Secret"];
                timeout = int.Parse(_Configuration["Time:timeout"]);
            }
            catch
            {
                secretKey = String.Empty;
                timeout = 5;
            }
            _sentryHub = hub;
            _sendSmsService = sendSmsService;
            _webHostEnvironment = webHostEnvironment;
        }
        /// <summary>
        /// Đăng nhập 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// </remarks>
        [HttpPost("Login")]
        public async Task<JsonResultModel> Login(LoginAppModel model)
        {
            return await _customerService.Authenticate(model, secretKey, timeout);
        }
        /// <summary>
        /// Kiểm tra số điện thoại đăng nhập
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("CheckPhoneLogin")]
        public async Task<JsonResultModel> CheckPhoneLogin(CheckPhoneModel model)
        {
            return await _customerService.CheckPhoneLogin(model.Phone, _webHostEnvironment);
        }
        /// <summary>
        /// Đăng xuất
        /// </summary>
        /// <returns></returns>
        [HttpPost("Logout")]
        [Authorize]
        public async Task<JsonResultModel> Logout()
        {
            try
            {
                var cus = (Customer)HttpContext.Items["Payload"];
                cus.Token = String.Empty;
                cus.DeviceID = String.Empty;
                await _customerService.UpdateAsync(cus);
                return JsonResponse.Success();
            }
            catch (Exception)
            {
                return JsonResponse.ServerError();
            }

        }
        /// <summary>
        /// Kiểm tra số điện thoại đăng ký
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("CheckPhoneRegister")]
        public async Task<JsonResultModel> CheckPhoneRegister(CheckPhoneModel model)
        {
            return await _customerService.CheckPhoneRegister(model.Phone, _webHostEnvironment);
        }
        /// <summary>
        /// Đăng ký tài khoản
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<JsonResultModel> Register([FromBody] RegisterModel input)
        {
            return await _customerService.Register(input, secretKey, timeout);
        }
        /// <summary>
        /// Thông tin tài khoản khách hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserInfo")]
        [Authorize]
        public async Task<JsonResultModel> GetUserInfo()
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _customerService.GetCustomerInfo(cus);
        }
        /// <summary>
        /// Test gửi tin nhắn
        /// </summary>
        /// <returns></returns>
        [HttpGet("SendSms")]
        public async Task<JsonResultModel> SendSms(string phone, string otp)
        {
            try
            {
                var result = await _sendSmsService.SendSms(phone, otp, _webHostEnvironment);
                return JsonResponse.Success(result);
            }
            catch (Exception ex)
            {
                return JsonResponse.Error(1, ex.ToString());
            }
        }
    }
}
