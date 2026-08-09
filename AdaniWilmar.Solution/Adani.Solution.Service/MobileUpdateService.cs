using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using GMCore.Logger;
using GMCore.Helper;
using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;

namespace Adani.Solution.Service
{
    public interface IMobileUpdateService
    {
        ResultDto GetFeedbackTypeList();
        ResultDto SaveFeedback(FeedbackRequestInputDto inputDto);
        ResultDto GetQuestionList(LoginUserIdDto inputDto);
        ResultDto AddAnswer(QuestionSurveyDto inputDto);
        //Bulletin
        ResultDto GetBulletinList(IdInputDto inputDto);
        ResultDto GetBulletinDetailsById(BulletinInputDto bulletinInputDto);
        ResultDto GetLatestUpdateBulletin();

    }

    public class MobileUpdateService : IMobileUpdateService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Mobile update Service");
        private const string ServiceName = "Mobile update Service";
        private string _methodName;
        private readonly IResultService _resultService;

        public MobileUpdateService(IAdaniContext emamiContext, IResultService resultService)
        {
            try
            {
                _emamiContext = emamiContext;
                _resultService = resultService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for update Service", exception);
            }
        }

        public ResultDto GetFeedbackTypeList()
        {
            _methodName = "GetFeedbackTypeList";
            var resultDto = new ResultDto();
            try
            {
                var feedbackTypeList = _emamiContext.FeedbackTypes.AsNoTracking().Where(_ => _.IsActive)
                    .Select(s => new ZoneDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        isActive = s.IsActive
                    }).ToList();


                if (feedbackTypeList == null || !feedbackTypeList.Any())
                    return _resultService.ErrorMessage(Constants.RecordNotFound);

                return _resultService.SuccessObject(feedbackTypeList);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto SaveFeedback(FeedbackRequestInputDto inputDto)
        {
            _methodName = "SaveFeedback";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                    return _resultService.ErrorMessage(Constants.InvalidRequest);

                var feedback = new FeedbackRequest
                {
                    FeedbackTypeId = inputDto.FeedbackTypeId,
                    UserId = inputDto.LoginUserId,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    Details = inputDto.Details
                };
                _emamiContext.FeedbackRequests.Add(feedback);
                _emamiContext.SaveChanges();

                return _resultService.SuccessMessage(Constants.FeedbackSuccess);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        /// <summary>
        /// Method to Get Question List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto GetQuestionList(LoginUserIdDto inputDto)
        {
            _methodName = "GetQuestionList";
            var resultDto = new ResultDto();
            var outputDto = new List<QuestionDto>();
            try
            {
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var resultContext = _emamiContext.Questions.AsNoTracking().Where(_ => _.Isactive &&
                DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) &&
                DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).ToList();


                if (resultContext != null && resultContext.Any())
                {
                    outputDto = resultContext.Select(c => new QuestionDto
                    {
                        Id = c.Id,
                        Question = c.Question,
                        IsActive = c.Isactive,
                        ValidFrom = c.ValidFrom,
                        ValidTo = c.ValidTo,
                    }).ToList();
                }

                return _resultService.SuccessObject(outputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto AddAnswer(QuestionSurveyDto inputDto)
        {
            _methodName = "SaveFeedback";
            var resultDto = new ResultDto();
            try
            {
                string errorMessageList = string.Empty;
                var errorFlag = false;
                if (inputDto == null || inputDto.QuestionSurveys == null || !inputDto.QuestionSurveys.Any())
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                
                List<QuestionSurveyViewDto> QuestionSurveyViewList = inputDto.QuestionSurveys.ToList();
                foreach (var survey in QuestionSurveyViewList)
                {
                    var errorMessage = string.Empty;
                    if (survey.QuestionSurveyId==0)
                    {
                        errorMessage = Constants.QuestionMissing;
                        errorFlag = true;
                    }
                    else
                    {
                        var questionContext = _emamiContext.Questions.AsNoTracking().FirstOrDefault(_ => _.Id == survey.QuestionSurveyId);
                        if (questionContext == null)
                        {
                            errorMessage = Constants.BindErrorMessage(Constants.QuestionNotFound, errorMessage);
                            errorFlag = true;
                        }
                        else
                        {
                            errorMessage = questionContext.Question;
                            if (string.IsNullOrEmpty(survey.Comments))
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.AnswerMissing, errorMessage);
                                errorFlag = true;
                            }
                        }
                    }
                    if(errorFlag)
                    {
                        if (!string.IsNullOrEmpty(errorMessageList))
                        {
                            errorMessageList = Constants.BindErrorMessage(System.Environment.NewLine + errorMessage, errorMessageList);
                        }
                        else
                        {
                            errorMessageList = Constants.BindErrorMessage(errorMessage, errorMessageList);
                        }
                    }
                }

                if (!errorFlag)
                {
                    foreach (var survey in QuestionSurveyViewList)
                    {
                        var questionSurveyViewDto = new Answers()
                        {
                            QuestionId = survey.QuestionSurveyId,
                            Answer = survey.Comments,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            CreatedBy = survey.CreatedUserId,
                        };
                        _emamiContext.Answers.Add(questionSurveyViewDto);
                    }
                    _emamiContext.SaveChanges();
                }
                if (errorFlag)
                {
                    return _resultService.ErrorMessage(errorMessageList);
                }
                else
                {
                    return _resultService.SuccessMessage(Constants.AnswerSuccess);
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }


        #region Bulletin
        public ResultDto GetBulletinDetailsById(BulletinInputDto inputDto)
        {
            _methodName = "GetBulletinDetailsById";
            var resultDto = new ResultDto();
            var outputDto = new BulletinDto();

            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.LoginUserId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                if (inputDto.BulletinId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.BulletinIdMissing);
                }
                var bulletinContext = _emamiContext.Bulletin.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.BulletinId);
                if (bulletinContext == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                outputDto.BulletinId = bulletinContext.Id;
                outputDto.Title = bulletinContext.Title;
                outputDto.Content = bulletinContext.Content;
                outputDto.IsActive = bulletinContext.IsActive;
                outputDto.ContentTypeId = bulletinContext.ContentTypeId;
                outputDto.IsApproved = bulletinContext.IsApproved;
                outputDto.ReviewedBy = bulletinContext.ReviewedBy;
                outputDto.MediaList = bulletinContext.BulletinMedia.Select(bulletin => new BulletinMediaDto {
                    BulletinMediaId = bulletin.Id,
                    MediaPath = _resultService.GetBulletinMediaPath(bulletinContext.ContentTypeId, bulletin.MediaPath),
                    MediaTypeId = bulletin.MediaTypeId, MediaTypeName = bulletin.MediaType.Name }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetBulletinList(IdInputDto inputDto)
        {
            _methodName = "GetBulletinList";
            var resultDto = new ResultDto();
            var outputDto = new List<BulletinDto>();
            try
            {
                if(inputDto == null)
                {
                    _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                List<Bulletin> bulletinsContext = _emamiContext.Bulletin.AsNoTracking().Where(_ => _.ContentTypeId == inputDto.Id && _.IsActive).ToList();
                if (bulletinsContext != null && bulletinsContext.Any())
                {
                    foreach (var c in bulletinsContext)
                    {
                        var dto = new BulletinDto()
                        {
                            BulletinId = c.Id,
                            Title = c.Title,
                            Content = c.Content,
                            ContentTypeId = c.ContentTypeId,
                            IsActive = c.IsActive,
                            IsApproved = c.IsApproved,
                            ReviewedBy = c.ReviewedBy,
                        };
                        foreach (var item in c.BulletinMedia)
                        {
                            var mediadto = new BulletinMediaDto()
                            {
                                MediaPath = item.MediaPath != null ? _resultService.GetBulletinMediaPath((int)inputDto.Id, item.MediaPath) : string.Empty,
                                MediaTypeId = item.MediaTypeId,
                                MediaTypeName = item.MediaType.Name
                            };
                            dto.MediaList.Add(mediadto);
                        }
                        outputDto.Add(dto);
                    }
                }


                if (outputDto  == null || !outputDto.Any())
                {
                    _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetLatestUpdateBulletin()
        {
            _methodName = "GetLatestUpdateBulletin";
            var resultDto = new ResultDto();
            var outputDto = new BulletinDto();
            try
            {


                var bulletinContext = _emamiContext.Bulletin.FirstOrDefault(_ => _.ContentTypeId == (int)DTO.Enums.ContentType.LatestUpdate && _.IsActive);
                if (bulletinContext != null)
                {
                    outputDto = new BulletinDto()
                    {
                        BulletinId = bulletinContext.Id,
                        Title = bulletinContext.Title,
                        ContentTypeId = bulletinContext.ContentTypeId,
                        Content = bulletinContext.Content,
                        IsActive = bulletinContext.IsActive,
                        IsApproved = bulletinContext.IsApproved,
                        ReviewedBy = bulletinContext.ReviewedBy,
                        MediaList = bulletinContext.BulletinMedia.Select(bulletinmedia => new BulletinMediaDto {
                            MediaPath = _resultService.GetBulletinMediaPath(bulletinContext.ContentTypeId, bulletinmedia.MediaPath),
                            MediaTypeId = bulletinmedia.MediaTypeId,
                            MediaTypeName = bulletinmedia.MediaType.Name }).ToList()
                    };

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = outputDto;
                    return resultDto;
                }
               

                return _resultService.ErrorMessage(Constants.RecordNotFound);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }
        #endregion

    }
}
