using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace APIProject.Controllers.App
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly IGiftService _GifService;
        public GiftController(IGiftService GifService)
        {
            _GifService= GifService;
        }
        /// <summary>
        /// Danh sachs Voucher
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListVoucher")]
        public async Task<JsonResultModel> GetListVoucher()
        {
            return await _GifService.GetListGift();
        }
    }
}
