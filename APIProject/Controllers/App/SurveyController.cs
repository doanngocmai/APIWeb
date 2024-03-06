using APIProject.Common.DTOs.Survey;
using APIProject.Domain.Models;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace APIProject.Controllers.App
{
    [Route("api/app/[controller]")]
    [ApiExplorerSettings(GroupName = "App")]
    [ApiController]
    [SwaggerTag("Phiếu khảo sát")]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _SurveyService;
        public SurveyController(ISurveyService surveyService)
        {
            _SurveyService = surveyService;
        }
        /// <summary>
        /// Danh sách phiếu khảo sát 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListSurveryQuestionsApp")]
        //[Authorize]
        public async Task<JsonResultModel> GetListSurveryQuestionsApp()
        {
            return await _SurveyService.GetListSurveyQuestions();
        }
        /// <summary>
        /// Trả lời phiếu khảo sát
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("AnswerSurveyQuestion")]
        public async Task<JsonResultModel> AnswerSurveyQuestion(AnswerSurveyModel input)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _SurveyService.AnswerSurveyQuestion(input, cus.ID);
        }
    }
}
