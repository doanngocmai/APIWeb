using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Models.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using APIProject.Common.Models.Users;
using APIProject.Middleware;
using APIProject.Domain.Models;

namespace APIProject.Controllers.Web
{
    [Route("api/web/[controller]")]
    [ApiExplorerSettings(GroupName = "Web")]
    [ApiController]
    [SwaggerTag("Authen")]
    public class AuthenticationController : ControllerBase
    {
        public IConfiguration _Configuration;
        private readonly IUserService _userService;
        private readonly string secretKey;
        private readonly int timeout;
        public AuthenticationController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _Configuration = configuration;
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
        }

        /// <summary>
        /// Đăng nhập cho Admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///       "phone"    : "0987654321",
        ///       "password" : "12345678"
        ///     }
        ///
        /// </remarks>
        [HttpPost("Login")]
        public async Task<JsonResultModel> Login(LoginModel model)
        {
            return await _userService.Authenticate(model, secretKey, timeout);
        }
        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<JsonResultModel> ChangePassword(ChangePasswordWebModel model)
        {
            var user = (User)HttpContext.Items["Payload"];
            return await _userService.ChangePassword(user.ID, model.OldPassword, model.NewPassword);
        }
        /// <summary>
        /// Thông tin tài khoản
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserInfo")]
        [Authorize]
        public async Task<JsonResultModel> GetUserInfo()
        {
            var user = (User)HttpContext.Items["Payload"];
            return await _userService.GetUserInfo(user.ID);
        }
    }
}
