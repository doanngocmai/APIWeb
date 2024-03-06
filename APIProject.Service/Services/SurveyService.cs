using APIProject.Common.DTOs.Survey;
using APIProject.Common.Utils;
using APIProject.Domain.Models;
using APIProject.Repository;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
using AutoMapper;
using Sentry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace APIProject.Service.Services
{

    public class SurveyService : ISurveyService
    {
        private readonly ISurveyQuestionRepository _SurveryQuestionRepository;
        private readonly ISurveyAnswerRepository _SurveryAnswerRepository;
        private readonly ISurveySheetRepository _SurveySheetRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        public SurveyService(ISurveyQuestionRepository surveryQuestionRepository, ISurveyAnswerRepository surveryAnswerRepository, IMapper mapper, IHub hub, ISurveySheetRepository surveySheetRepository)
        {
            _SurveryQuestionRepository = surveryQuestionRepository;
            _SurveryAnswerRepository = surveryAnswerRepository;
            _mapper = mapper;
            _sentryHub = hub;
            _SurveySheetRepository = surveySheetRepository;
        }

        public async Task<JsonResultModel> GetListSurveyQuestions(string SearchKey)
        {
            try
            {
                var model = await _SurveryQuestionRepository.GetListSurveyQuestion(SearchKey);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> ChangeStatus(int ID)
        {
            try
            {
                var model = await _SurveryQuestionRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_SURVERY_NOT_FOUND, SystemParam.MESSAGE_SURVERY_NOT_FOUND);
                }
                if (model.Status == SystemParam.ACTIVE)
                    model.Status = SystemParam.ACTIVE_FALSE;
                else
                    model.Status = SystemParam.ACTIVE;
                await _SurveryQuestionRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> DeleteSurvey(int ID)
        {
            try
            {
                var model = await _SurveryQuestionRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_SURVERY_NOT_FOUND, SystemParam.MESSAGE_SURVERY_NOT_FOUND);
                }
                model.IsActive = SystemParam.ACTIVE_FALSE;
                await _SurveryQuestionRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> CreateSurvey(CreateSurveryModel Input)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (string.IsNullOrEmpty(Input.Question))
                    {
                        return JsonResponse.Error(SystemParam.ERROR_STALL_FIELDS_INVALID, SystemParam.MESSAGE_STALL_FIELDS_INVALID);
                    }
                    var modelQ = new SurveyQuestion
                    {
                        Question = Input.Question,
                        Type = Input.Type,
                    };
                    modelQ.Status = SystemParam.ACTIVE;
                    await _SurveryQuestionRepository.AddAsync(modelQ);
                    if (Input.ListAnswer != null)
                    {
                        foreach (var item in Input.ListAnswer)
                        {
                            var Answers = new SurveyAnswer
                            {
                                Answer = item,
                                SurveyQuestionID = modelQ.ID
                            };
                            await _SurveryAnswerRepository.AddAsync(Answers);
                        }
                    }
                    scope.Complete();
                    return JsonResponse.Success();
                }
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> UpdateSurvey(UpdateSurveryModel Input)
        {
            try
            {
                if (string.IsNullOrEmpty(Input.Question))
                {
                    return JsonResponse.Error(SystemParam.ERROR_STALL_FIELDS_INVALID, SystemParam.MESSAGE_STALL_FIELDS_INVALID);
                }

                var model = await _SurveryQuestionRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(Input.ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_STALL_NOT_FOUND, SystemParam.MESSAGE_STALL_NOT_FOUND);
                }

                model.Question = Input.Question;
                model.Type = Input.Type;
                await _SurveryQuestionRepository.UpdateAsync(model);

                var listAnswers = await _SurveryAnswerRepository.GetAllAsync(x => x.SurveyQuestionID.Equals(model.ID));
                await _SurveryAnswerRepository.DeleteListAsync(listAnswers);
                if (Input.ListAnswer != null)
                {
                    foreach (var item in Input.ListAnswer)
                    {
                        var Answers = new SurveyAnswer
                        {
                            Answer = item,
                            SurveyQuestionID = model.ID
                        };
                        await _SurveryAnswerRepository.AddAsync(Answers);
                    }
                }
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetListSurveyQuestions()
        {
            try
            {
                var model = await _SurveryQuestionRepository.GetListSurveyQuestion();
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> AnswerSurveyQuestion(AnswerSurveyModel input, int CustomerID)
        {
            try
            {
                var question = await _SurveryQuestionRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.QuestionID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (question == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_SURVEY_QUESTION_NOT_FOUND, SystemParam.MESSAGE_SURVEY_QUESTION_NOT_FOUND);
                }
                if (input.AnswerID != null)
                {
                    var answer = await _SurveryAnswerRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.AnswerID) && x.IsActive.Equals(SystemParam.ACTIVE));
                    if (answer == null)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_SURVEY_ANSWER_NOT_FOUND, SystemParam.MESSAGE_SURVEY_ANSWER_NOT_FOUND);
                    }
                }
                var model = new SurveySheet
                {
                    CustomerID = CustomerID,
                    SurveyAnswerID = input.AnswerID,
                    SurveyQuestionID = input.QuestionID,
                    Content = input.Content
                };
                await _SurveySheetRepository.AddAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetSurveyAnswerStatistic(int ID)
        {
            try
            {
                var model = await _SurveryQuestionRepository.GetListAnswerStatistic(ID);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetSurveyContentStatistic(int ID, int page, int limit)
        {
            try
            {
                var model = await _SurveryQuestionRepository.GetListContentStatistic(ID, page, limit);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}
