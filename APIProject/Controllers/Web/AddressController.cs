using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace APIProject.Controllers.Web
{
    [Route("api/web/[controller]")]
    [ApiExplorerSettings(GroupName = "Web")]
    [ApiController]
    [SwaggerTag("Địa chỉ")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        /// <summary>
        /// Tỉnh/thành phố
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListProvince")]
        public async Task<JsonResultModel> GetListProvince()
        {
            return await _addressService.GetListProvice();
        }
        /// <summary>
        /// Quận/huyện
        /// </summary>
        /// <param name="provinceID"></param>
        /// <returns></returns>
        [HttpGet("GetListDistrict")]
        public async Task<JsonResultModel> GetListDistrict(int provinceID)
        {
            return await _addressService.GetListDistrict(provinceID);
        }
        /// <summary>
        /// Xã/phường
        /// </summary>
        /// <param name="districtID"></param>
        /// <returns></returns>
        [HttpGet("GetListWard")]
        public async Task<JsonResultModel> GetListWard(int districtID)
        {
            return await _addressService.GetListWard(districtID);
        }
    }
}
