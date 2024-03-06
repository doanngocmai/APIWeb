using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Common.Utils;
using APIProject.Common.DTOs.Survey;
using Sentry.PlatformAbstractions;
using PagedList.Core;

namespace APIProject.Repository
{
    public class SurveryQuestionRepository : BaseRepository<SurveyQuestion>, ISurveyQuestionRepository
    {
        public SurveryQuestionRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<List<AnswerStatisticModel>> GetListAnswerStatistic(int ID)
        {
            return await Task.Run(() =>
            {
                var model = (from a in DbContext.SurveyAnswers
                             where a.IsActive.Equals(SystemParam.ACTIVE) && a.SurveyQuestionID.Equals(ID)
                             select new AnswerStatisticModel
                             {
                                 ID = a.ID,
                                 Answer = a.Answer,
                                 Count = DbContext.SurveySheets.Count(x => x.SurveyAnswerID == a.ID && a.IsActive.Equals(SystemParam.ACTIVE))
                             }).ToList();
                return model;
            });
        }

        public async Task<IPagedList<ContentStatisticModel>> GetListContentStatistic(int ID, int page, int limit)
        {
            return await Task.Run(() =>
            {
                var model = (from a in DbContext.SurveySheets
                             where a.IsActive.Equals(SystemParam.ACTIVE) && a.SurveyQuestionID.Equals(ID) && a.SurveyAnswerID == null
                             select new ContentStatisticModel
                             {
                                 ID = a.ID,
                                 CustomerName = a.Customer.Name,
                                 Content = a.Content
                             }).ToPagedList(page, limit);
                return model;
            });
        }

        public async Task<List<SurveryModel>> GetListSurveyQuestion(string SearchKey)
        {
            return await Task.Run(() =>
            {
                var model = (from a in DbContext.SurveyQuestions
                             where a.IsActive.Equals(SystemParam.ACTIVE)
                             && (!string.IsNullOrEmpty(SearchKey) ? a.Question.Contains(SearchKey) : true)
                             select new SurveryModel
                             {
                                 ID = a.ID,
                                 Status = a.Status,
                                 Type = a.Type,
                                 Question = a.Question,
                                 ListAnswer = (from b in DbContext.SurveyAnswers
                                               where b.SurveyQuestionID == a.ID
                                               select new SurveryAnswerModel
                                               {
                                                   ID = b.ID,
                                                   Answer = b.Answer,
                                               }).ToList()
                             }).ToList();
                return model;
            });
        }

        public async Task<List<SurveryModel>> GetListSurveyQuestion()
        {
            try
            {
                var model = (from a in DbContext.SurveyQuestions
                             where a.IsActive.Equals(SystemParam.ACTIVE)
                             select new SurveryModel
                             {
                                 ID = a.ID,
                                 Type = a.Type,
                                 Question = a.Question,
                                 ListAnswer = (from b in DbContext.SurveyAnswers
                                               where b.SurveyQuestionID.Equals(a.ID)
                                               select new SurveryAnswerModel
                                               {
                                                   ID = b.ID,
                                                   Answer = b.Answer,
                                                   
                                               }).ToList()
                             }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
