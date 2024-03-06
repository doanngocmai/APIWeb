using APIProject.Common.DTOs.Survey;
using APIProject.Domain.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface ISurveyQuestionRepository : IRepository<SurveyQuestion>
    {
        Task<List<SurveryModel>> GetListSurveyQuestion(string SearchKey);
        Task<List<SurveryModel>> GetListSurveyQuestion();//lấy danh sách câu hỏi khảo sát trên App
        Task<List<AnswerStatisticModel>> GetListAnswerStatistic(int ID);
        Task<IPagedList<ContentStatisticModel>> GetListContentStatistic(int ID, int page, int limit);
    }
}
