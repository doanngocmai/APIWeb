using APIProject.Common.DTOs.Survey;
using APIProject.Domain.Models;
using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface ISurveyService
    {
        Task<JsonResultModel> GetListSurveyQuestions(string SearchKey);
        Task<JsonResultModel> GetListSurveyQuestions();
        Task<JsonResultModel> GetSurveyAnswerStatistic(int ID);
        Task<JsonResultModel> GetSurveyContentStatistic(int ID,int page,int limit);
        Task<JsonResultModel> AnswerSurveyQuestion(AnswerSurveyModel input, int CustomerID);
        Task<JsonResultModel> ChangeStatus(int ID);
        Task<JsonResultModel> DeleteSurvey(int ID);
        Task<JsonResultModel> CreateSurvey(CreateSurveryModel Input);
        Task<JsonResultModel> UpdateSurvey(UpdateSurveryModel Input);
    }
}
