using APIProject.Domain.Models;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Models.Customer;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace APIProject.Controllers.App
{
    [Route("api/app/[controller]")]
    [ApiExplorerSettings(GroupName = "App")]
    [ApiController]
    [SwaggerTag("Khách hàng")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        /// <summary>
        /// Cập nhật thông tin cá nhân
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("UpdateCustomerInfo")]
        [Authorize]
        public async Task<JsonResultModel> UpdateCustomerInfo([FromBody] UpdateCustomerInfoModel input)
        {
            var cus= (Customer)HttpContext.Items["Payload"];
            return await _customerService.UpdateCustomerInfo(cus,input);
        }
        /// <summary>
        /// Xóa tài khoản
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeleteCustomer")]
        [Authorize]
        public async Task<JsonResultModel> DeleteCustomer()
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _customerService.DeleteCustomer(cus.ID);
        }
    }
}
