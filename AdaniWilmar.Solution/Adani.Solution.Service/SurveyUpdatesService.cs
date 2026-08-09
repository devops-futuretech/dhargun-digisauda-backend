using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using GMCore.Logger;
using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using GMCore.Helper;
using Adani.Solution.DTO.Enums;
using System.Data.Entity;
using System.Collections.Generic;

namespace Adani.Solution.Service
{
    public interface ISurveyUpdatesService
    {
        //Question
        ResultDto SaveQuestion(QuestionDto inputDto);
        ResultDto GetQuestionList(LoginUserIdDto inputDto);
        ResultDto GetQuestionDetailsById(long questionId);
        ResultDto GetQuestionSurveyDetailsById(long questionId);
        ResultDto UpdateQuestion(QuestionDto inputDto);
        //Bulletin
        ResultDto GetBulletinList(BulletinInputDto bulletinInputDto);
        ResultDto GetBulletinDetailsById(BulletinInputDto bulletinInputDto);
        ResultDto SaveBulletin(BulletinDto inputDto);
        ResultDto DeleteBulletinMedia(BulletinInputDto inputDto);
        ResultDto UpdateBulletin(BulletinDto inputDto);
        //Feedback
        ResultDto GetFeedbackTypeList(LoginUserIdDto loginUserIdDto);
        ResultDto GetFeedbackList(FeedbackInputDto feedbackInputDto);
    }

    public class SurveyUpdatesService : ISurveyUpdatesService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("SurveyUpdates Service");
        private const string ServiceName = "SurveyUpdates Service";
        private string _methodName;
        private readonly IResultService _resultService;

        public SurveyUpdatesService(IAdaniContext emamiContext, IResultService resultService)
        {
            try
            {
                _emamiContext = emamiContext;
                _resultService = resultService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for Lookup Service", exception);
            }
        }

        #region Question

        /// <summary>
        /// Method to Save Question
        /// </summary>
        /// <param name="employeeDto"></param>
        /// <returns></returns>
        public ResultDto SaveQuestion(QuestionDto inputDto)
        {
            _methodName = "SaveQuestion";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                var input = new Questions
                {
                    Question = inputDto.Question,
                    ValidFrom = inputDto.ValidFrom,
                    ValidTo = inputDto.ValidTo,
                    Isactive = inputDto.IsActive,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                };
                _emamiContext.Questions.Add(input);
                _emamiContext.SaveChanges();

                return _resultService.SuccessMessageWitObject(input.Id, Constants.PriceDetailsSavedSuccessfully);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.CostAlreadyExistiInThisDate);

            }
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
                IQueryable<Questions> resultContext;
                if (inputDto.IsToReturnInactiveData)
                {
                    resultContext = _emamiContext.Questions.AsNoTracking();
                }
                else
                {
                    resultContext = _emamiContext.Questions.AsNoTracking().Where(_ => _.Isactive);
                }

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

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.Id).ToList() : outputDto;
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

        /// <summary>
        /// Method to get Get Question Details By Id
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public ResultDto GetQuestionDetailsById(long questionId)
        {
            _methodName = "GetQuestionDetailsById";
            var resultDto = new ResultDto();
            var outputDto = new QuestionDto();
            try
            {
                var resultContext = _emamiContext.Questions.AsNoTracking().FirstOrDefault(_ => _.Id == questionId);
                if (resultContext != null)
                {
                    outputDto.Id = resultContext.Id;
                    outputDto.Question = resultContext.Question;
                    outputDto.IsActive = resultContext.Isactive;
                    outputDto.ValidFrom = resultContext.ValidFrom;
                    outputDto.ValidTo = resultContext.ValidTo;
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

        /// <summary>
        /// Method to get Get Question Details By Id
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public ResultDto GetQuestionSurveyDetailsById(long questionId)
        {
            _methodName = "GetQuestionSurveyDetailsById";
            var resultDto = new ResultDto();
            var outputDto = new QuestionDto();
            try
            {
                var resultContext = _emamiContext.Questions.AsNoTracking().FirstOrDefault(_ => _.Id == questionId);
                if (resultContext != null)
                {
                    outputDto.Id = resultContext.Id;
                    outputDto.Question = resultContext.Question;
                    outputDto.IsActive = resultContext.Isactive;
                    outputDto.ValidFrom = resultContext.ValidFrom;
                    outputDto.ValidTo = resultContext.ValidTo;

                    //QuestionSurvey if any

                    var questionSurveyContext = _emamiContext.Answers.AsNoTracking().Where(_ => _.QuestionId == resultContext.Id).ToList();
                    if(questionSurveyContext.Any())
                    {
                        foreach(var survey in questionSurveyContext)
                        {
                            var questionSurveyViewDto = new QuestionSurveyViewDto()
                            {
                                QuestionSurveyId = survey.Id,
                                Comments = survey.Answer,
                                CreatedDate = survey.CreatedDate.ToString("dd-MMM-yyyy hh:mm tt"),
                                CreatedUserId = survey.CreatedBy,
                                CreatedUserName = survey.CreatedBy > 0 ? _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == survey.CreatedBy).Name : string.Empty
                            };
                            outputDto.Comments.Add(questionSurveyViewDto);
                        }
                    }
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

        /// <summary>
        /// Method to Update Question
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto UpdateQuestion(QuestionDto inputDto)
        {
            _methodName = "UpdateQuestion";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }

                var result = _emamiContext.Questions.FirstOrDefault(_ => _.Id == inputDto.Id);
                result.Question = inputDto.Question;
                result.Isactive = inputDto.IsActive;
                result.ValidFrom = inputDto.ValidFrom;
                result.ValidTo = inputDto.ValidTo;
                result.ModifiedBy = inputDto.LoginUserId;
                result.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();


                resultDto.IsSuccess = true;
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

        #endregion

        #region Bulletin
        /// <summary>
        /// Method to Get Bulletin List
        /// </summary>
        /// <returns></returns>        
        public ResultDto GetBulletinList(BulletinInputDto inputDto)
        {
            _methodName = "GetBulletinList";
            var resultDto = new ResultDto();
            var outputDto = new List<BulletinDto>();
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
                if (inputDto.ContentTypeId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.ContentTypeIdMissing);
                }

                IQueryable<Bulletin> resultContext;
               
                resultContext = _emamiContext.Bulletin.AsNoTracking().Where(_ => _.ContentTypeId == inputDto.ContentTypeId);
                

                if (resultContext != null && resultContext.Any())
                {
                    outputDto = resultContext.Select(c => new BulletinDto
                    {
                        BulletinId = c.Id,
                        Title = c.Title,
                        Content = c.Content,
                        IsActive = c.IsActive,
                        IsApproved = c.IsApproved,
                        ReviewedBy = c.ReviewedBy,
                        MediaList = c.BulletinMedia.Select(bulletin => new BulletinMediaDto { MediaPath = bulletin.MediaPath, MediaTypeId = bulletin.MediaTypeId, MediaTypeName = bulletin.MediaType.Name }).ToList()
                    }).ToList();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.BulletinId).ToList() : outputDto;
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
                outputDto.MediaList = bulletinContext.BulletinMedia.Select(bulletin => new BulletinMediaDto { BulletinMediaId = bulletin.Id, MediaPath = bulletin.MediaPath, MediaTypeId = bulletin.MediaTypeId, MediaTypeName = bulletin.MediaType.Name }).ToList();
               
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

        /// <summary>
        /// Method to Save  Bulletin
        /// </summary>
        /// <param name="BulletinDto"></param>
        /// <returns></returns>
        public ResultDto SaveBulletin(BulletinDto inputDto)
        {
            _methodName = "SaveBulletin";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if(inputDto.ContentTypeId == (int)DTO.Enums.ContentType.LatestUpdate && inputDto.IsActive)
                {
                    var bulletinCount = _emamiContext.Bulletin.AsNoTracking().Where(_ => _.ContentTypeId == inputDto.ContentTypeId && _.IsActive).Count();
                    if(bulletinCount > 0)
                    {
                        return _resultService.ErrorMessage(Constants.BulletinCreationError);
                    }
                }
                var bulletinContext = new Bulletin
                {
                    Title = inputDto.Title,
                    Content = inputDto.Content,
                    ContentTypeId = inputDto.ContentTypeId,
                    IsActive = inputDto.IsActive,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                };
                _emamiContext.Bulletin.Add(bulletinContext);
                _emamiContext.SaveChanges();

                if (inputDto.MediaList.Any())
                {
                    foreach (var media in inputDto.MediaList)
                    {
                        var bulletinMeida = new BulletinMedia
                        {
                            BulletinId = bulletinContext.Id,
                            MediaPath = media.MediaPath,
                            MediaTypeId = media.MediaTypeId,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.BulletinMedia.Add(bulletinMeida);
                    }
                    _emamiContext.SaveChanges();
                }


                return _resultService.SuccessMessageWitObject(inputDto.BulletinId, Constants.BulletinSavedSuccessfully);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);

            }
        }

        /// <summary>
        /// Method to Update Bulletin
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto UpdateBulletin(BulletinDto inputDto)
        {
            _methodName = "UpdateBulletin";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }

                var bulletinContext = _emamiContext.Bulletin.FirstOrDefault(_ => _.Id == inputDto.BulletinId);
                if (bulletinContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                bulletinContext.Title = inputDto.Title;
                bulletinContext.Content = inputDto.Content;
                bulletinContext.ContentTypeId = inputDto.ContentTypeId;
                bulletinContext.IsActive = inputDto.IsActive;
                bulletinContext.ModifiedBy = inputDto.LoginUserId;
                bulletinContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();

                if (inputDto.MediaList.Any())
                {
                    foreach (var media in inputDto.MediaList)
                    {
                        var bulletinMeida = new BulletinMedia
                        {
                            BulletinId = bulletinContext.Id,
                            MediaPath = media.MediaPath,
                            MediaTypeId = media.MediaTypeId,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.BulletinMedia.Add(bulletinMeida);
                    }
                    _emamiContext.SaveChanges();
                }


                resultDto.IsSuccess = true;
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

        public ResultDto DeleteBulletinMedia(BulletinInputDto inputDto)
        {
            _methodName = "DeleteBulletinMedia";
            var resultDto = new ResultDto();

            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (inputDto.LoginUserId <= 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }


                var deleteBulletinMedia = _emamiContext.BulletinMedia.FirstOrDefault(_ => _.Id == inputDto.BulletinMediaId);

                if (deleteBulletinMedia == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                //var bulletinMediaCount = _salesContext.BulletinMedia.AsNoTracking().Count(_ => _.BulletinId == deleteBulletinMedia.BulletinId);
                //if (bulletinMediaCount == 1)
                //{
                //    resultDto.IsSuccess = false;
                //    resultDto.ErrorDto.ErrorCode = Constants.AtleastOneMediaNeedForBulletin;
                //    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.AtleastOneMediaNeedForBulletin, Constants.EnglishLanguage);
                //    return resultDto;
                //}

                _emamiContext.BulletinMedia.Remove(deleteBulletinMedia);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.GetMessage(Constants.RecordDeleted, Utility.MessageLanguage);
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
        #endregion

        #region Feedback
        public ResultDto GetFeedbackTypeList(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetFeedbackTypeList";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<FeedbackType> feedbackType;
                if (loginUserIdDto.IsToReturnInactiveData)
                {
                    feedbackType = _emamiContext.FeedbackTypes.AsNoTracking()
                  .Where(w => w.IsActive);
                }
                else
                {
                    feedbackType = _emamiContext.FeedbackTypes.AsNoTracking();
                }

                var feebackList = feedbackType
                    .Select(s => new FeedbackTypeDto()
                    {
                        FeedbackTypeId = s.Id,
                        FeedbackTypeName = s.Name
                    }).ToList();

                resultDto.SuccessDto.Response = feebackList;
                resultDto.IsSuccess = true;
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
        /// Method to Get Feedback List
        /// </summary>
        /// <returns></returns>        
        public ResultDto GetFeedbackList(FeedbackInputDto inputDto)
        {
            _methodName = "GetFeedbackList";
            var resultDto = new ResultDto();
            var outputDto = new List<FeedbackRequestDto>();
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
                if (inputDto.CreatedDate == null || inputDto.CreatedDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }

                IQueryable<FeedbackRequest> resultContext;

                resultContext = _emamiContext.FeedbackRequests.AsNoTracking().Where(_ => _.FeedbackTypeId == inputDto.FeedbackTypeId && DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(inputDto.CreatedDate));


                if (resultContext != null && resultContext.Any())
                {
                    outputDto = resultContext.ToList().Select(c => new FeedbackRequestDto
                    {
                        FeedbackId = c.Id,
                        FeedbackTypeId = c.FeedbackTypeId,
                        CreatedBy = c.CreatedBy > 0 ? _emamiContext.Users.FirstOrDefault(_ => _.Id == c.CreatedBy).Name : string.Empty,
                        Details = c.Details,
                        CreatedDate = c.CreatedDate.ToString("dd-MMM-yyyy hh:mm tt")
                    }).ToList();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.FeedbackId).ToList() : outputDto;
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
        #endregion
    }
}
