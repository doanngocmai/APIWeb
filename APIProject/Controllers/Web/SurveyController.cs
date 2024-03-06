using APIProject.Common.DTOs.News;
using APIProject.Common.DTOs.Stall;
using APIProject.Common.DTOs.Survey;
using APIProject.Common.Utils;
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
    [SwaggerTag("Câu hỏi khảo sát")]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _SurveryService;
        public SurveyController(ISurveyService surveryService)
        {
            _SurveryService = surveryService;
        }
        /// <summary>
        /// Danh sách câu hỏi khảo sát trên Web
        /// </summary>
        /// <param name="SearchKey"></param>
        /// <returns></returns>
        [HttpGet("GetListSurveryQuestions")]
        //[Authorize]
        public async Task<JsonResultModel> GetListSurveryQuestions(string SearchKey)
        {
            return await _SurveryService.GetListSurveyQuestions(SearchKey);
        }
        /// <summary>
        /// Thống kê tỉ lệ câu trả lời
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetSurveyAnswerStatistic")]
        public async Task<JsonResultModel> GetSurveyAnswerStatistic(int ID)
        {
            return await _SurveryService.GetSurveyAnswerStatistic(ID);
        }
        /// <summary>
        /// Thống kê ý kiến khách hàng
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetSurveyContentStatistic")]
        public async Task<JsonResultModel> GetSurveyContentStatistic(int ID, int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT)
        {
            return await _SurveryService.GetSurveyContentStatistic(ID, page, limit);
        }
        /// <summary>
        /// Xoá câu hỏi khảo sát
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("DeleteSurvery/{ID}")]
        //[Authorize]
        public async Task<JsonResultModel> DeleteSurvey(int ID)
        {
            return await _SurveryService.DeleteSurvey(ID);
        }
        /// <summary>
        /// Thay đổi trạng thái
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("ChangeStatus/{ID}")]
        //[Authorize]
        public async Task<JsonResultModel> ChangeStatus(int ID)
        {
            return await _SurveryService.ChangeStatus(ID);
        }
        /// <summary>
        /// Thêm khảo sát
        /// </summary>
        /// <param name="surveyQuestion"></param>
        /// <returns></returns>
        [HttpPost("CreateSurvery")]
        //[Authorize]
        public async Task<JsonResultModel> CreateSurvery(CreateSurveryModel surveyQuestion)
        {
            return await _SurveryService.CreateSurvey(surveyQuestion);
        }
        /// <summary>
        /// Sửa câu hỏi khảo sát
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("UpdateSurvery")]
        //[Authorize]
        public async Task<JsonResultModel> UpdateSurvery(UpdateSurveryModel input)
        {
            return await _SurveryService.UpdateSurvey(input);
        }
    }
}
