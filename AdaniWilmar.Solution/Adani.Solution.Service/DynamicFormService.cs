using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.DTO.Enums;
using Adani.Solution.Service.Common;
using Aspose.Cells.Charts;
using GMCore.Helper;
using GMCore.Logger;
using Kendo.Mvc.Extensions;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Security;

namespace Adani.Solution.Service
{
    public interface IDynamicFormService
    {
        //#region Section
        //ResultDto GetSectionsList();
        //ResultDto GetActiveSectionsList();
        //ResultDto GetSectionDetailsById(SectionIdDto inputDto);
        //ResultDto SaveSectionDetails(SectionDto inputDto);
        //ResultDto UpdateSectionDetails(SectionDto inputDto);
        //#endregion

        //#region Question Type
        ResultDto GetActiveQuestionTypes();
        ResultDto GetSubmittedDetailsList(DynamicFormReportFilterInputDto inputDto);
        ResultDto SubmittedFormDetailsbyIdList(FormIdDto formId);
        //#endregion

        //#region Question Master
        ResultDto AddQuestionAndAnswers(QuestionAddDto questionAddDto);
        ResultDto ExportSubmittedForm(DynamicFormReportFilterInputDto inputDto);
        ResultDto UpdateQuestionAndAnswers(QuestionAddDto questionAddDto);
        //ResultDto DeleteSectionQuestions(QuestionRemoveDto questionRemoveDto);
        ResultDto GetSectionQuestions(FormIdDto formId);
        ResultDto GetSectionQuestionsList();
        ResultDto GetSectionQuestionsFormList();
        ResultDto ViewQuestionDetails(QuestionIdDto questionIdDto);
        //ResultDto SaveQuestionOrder(List<QuestionrOrderDto> inputDto);
        //#endregion

        //#region Form CRUD operations
        ResultDto AddFormAndQuestions(FormAddDto formAddDto);
        ResultDto GetFormQuestions(FormIdDto formIdDto);
        ResultDto UpdateFormAndQuestions(FormUpdateDto questionIdDto);
        ResultDto GetFormsList(UserIdDto inputDto);
        ResultDto GetFormsView();
        //ResultDto GetComplaintFormsList(LoginUserIdDto inputDto);
        //#endregion

        //#region Submit Form CRUD operations
        ResultDto SubmitForm(FormInputDto formAddDto);
        //ResultDto InsertFormInput(FormInputDto input);
        //ResultDto UpdateForm(SubmitFormAddDto formAddDto);
        ResultDto ViewSubmittedFormDetails(SubmittedFormIdDto formIdDto);
        ResultDto ViewSubmittedFormsListByDateRange(SubmittedFormsInputDto submittedFormsInputDto);
        //#endregion

        //#region Demo Scheduling
        //ResultDto GetOpenComplaintFormsList();
        //ResultDto GetUnderstandingFormsBasedOnComplaintForm(IdInputDto inputDto);
        //ResultDto GetAvailableUserListForDemo(ScheduleDemoInputDto scheduleDemoInputDto);
        //ResultDto ScheduleDemo(ScheduleDemoInputDto scheduleDemoInputDto);
        //ResultDto UpdateDemoSchedule(ScheduleDemoInputDto scheduleDemoInputDto);
        //ResultDto GetAllScheduledDemos(DateFilterDto inputDto);
        //ResultDto GetScheduledDemoDetails(ScheduleDemoInputDto scheduleDemoInputDto);
        //#endregion

        //#region Complaint Approval / status
        //ResultDto GetComplaintApprovalList(DateFilterDto inputDto);
        //ResultDto UpdateComplaintApproval(ComplaintApprovalListInputDto inputDto);
        //ResultDto GetComplaintFormStatusList(DateFilterDto inputDto);
        //ResultDto UpdateComplaintFormStatus(ComplaintApprovalListInputDto inputDto);
        //#endregion

        //#region Mobile APIs
        //ResultDto GetUserAssignedFormListAndDetails(LoginUserIdDto loginUserIdDto);
        //ResultDto GetSubmittedFormListAndDetailsByUserId(SubmittedFormsInputDto submittedFormsInputDto);
        //ResultDto GetScheduledDemoDetailsByUserId(SubmittedFormsInputDto inputDto);
        //ResultDto GetActiveCustomerList();
        //ResultDto GetSubmittedFormsPlantList(LoginUserIdDto loginUserIdDto);
        //ResultDto GetSubmittedFormsStateCityList(LoginUserIdDto loginUserIdDto);
        //ResultDto GetCMSSEUsersList();
        //ResultDto GetSubmittedFormListAndDemoDetailsByUserIdForManager(SubmittedFormsInputDto submittedFormsInputDto);
        //#endregion

        //#region Web reports
        //ResultDto SubmittedFormReports(SubmittedFormsInputDto submittedFormsInputDto);
        //#endregion
        //ResultDto GetComplaintFormRemarks(FormRemarkInputDto inputDto);
    }
    public class DynamicFormService : IDynamicFormService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Dynamic Form Service");
        private const string ServiceName = "Dynamic Form Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;

        public DynamicFormService(IAdaniContext salesContext, IResultService resultService, INotificationService notificationService)
        {
            try
            {
                _emamiContext = salesContext;
                _resultService = resultService;
                _notificationService = notificationService;
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for dynamic form Service", exception);
            }
        }

        //    #region Section
        //public ResultDto GetSectionsList()
        //{
        //    _methodName = "GetSectionsList";
        //    var sectionListDto = new List<SectionDto>();
        //    try
        //    {
        //        sectionListDto = _emamiContext.QuestionSections.AsNoTracking().Select(s => new SectionDto
        //        {
        //            SectionId = s.Id,
        //            SectionName = s.SectionName,
        //            IsActive = s.IsActive,
        //        }).ToList();
        //        return SucessResult(sectionListDto);
        //    }
        //    catch (Exception exception)
        //    {
        //        return ExceptionResult(exception);
        //    }
        //}
        //    public ResultDto GetActiveSectionsList()
        //    {
        //        _methodName = "GetActiveSectionsList";
        //        var sectionListDto = new List<SectionDto>();
        //        try
        //        {
        //            sectionListDto = _emamiContext.QuestionSections.AsNoTracking().Where(_ => _.IsActive).Select(s => new SectionDto
        //            {
        //                SectionId = s.Id,
        //                SectionName = s.SectionName,
        //                IsActive = s.IsActive,
        //            }).ToList();
        //            return SucessResult(sectionListDto);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }

        //    #endregion

        #region Question Type
        public ResultDto GetActiveQuestionTypes()
        {
            _methodName = "GetActiveQuestionTypes";
            var questionTypeListDto = new List<QuestionTypeDto>();
            try
            {
                questionTypeListDto = _emamiContext.QuestionTypes.AsNoTracking().Where(_ => _.IsActive).OrderBy(_ => _.Name).Select(c => new QuestionTypeDto
                {
                    QuestionTypeId = c.Id,
                    QuestionTypeName = c.Name,
                    IsActive = c.IsActive,
                }).ToList();
                return _resultService.SuccessObject(questionTypeListDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto GetSubmittedDetailsList(DynamicFormReportFilterInputDto inputDto)
        {
            _methodName = "GetSubmittedDetailsList";
            var submittedFormListDto = new List<FormInputDto>();
            try
            {
                var query = _emamiContext.SubmittedForms.AsNoTracking().AsQueryable();

                if (inputDto.FromDate != DateTime.MinValue)
                {
                    query = query.Where(c => DbFunctions.TruncateTime(c.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                                          && DbFunctions.TruncateTime(c.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate));
                }

                if (inputDto.roleIds != null && inputDto.roleIds.Any())
                {
                    query = query.Where(c => _emamiContext.UserRoles
                        .Where(ur => ur.UserId == c.UserId && inputDto.roleIds.Contains(ur.RoleId))
                        .Any());
                }

                submittedFormListDto = query
                    .Select(c => new
                    {
                        c.Id,
                        c.FormName,
                        UserId = (long)c.UserId,
                        c.CustomerName,
                        c.CreatedDate,
                        UserRoleName = _emamiContext.UserRoles
                            .Where(ur => ur.UserId == c.UserId)
                            .Join(_emamiContext.Roles,
                                  ur => ur.RoleId,
                                  r => r.Id,
                                  (ur, r) => r.Name)
                            .FirstOrDefault()
                    })
                    .ToList()
                    .Select(dto => new FormInputDto
                    {
                        Id = dto.Id,
                        FormName = dto.FormName,
                        UserId = dto.UserId,
                        CustomerName = dto.CustomerName,
                        CreatedDate = dto.CreatedDate,
                        UserRoleType = dto.UserRoleName
                    })
                    .ToList();

                return _resultService.SuccessObject(submittedFormListDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        //public ResultDto GetSubmittedDetailsList()
        //{
        //    _methodName = "GetSubmittedDetailsList";
        //    var submittedFormListDto = new List<FormInputDto>();
        //    try
        //    {
        //        submittedFormListDto = _emamiContext.SubmittedForms
        //            .AsNoTracking()
        //            //.Where(_ => !_.IsDeleted)
        //            .Select(c => new
        //            {
        //                c.Id,
        //                c.FormName,
        //                UserId = (long)c.UserId,
        //                c.CustomerName,
        //                c.CreatedDate,
        //                UserRoleName = _emamiContext.UserRoles.Where(ur => ur.UserId == c.UserId).Join(_emamiContext.Roles,ur => ur.RoleId,r => r.Id,(ur, r) => r.Name).FirstOrDefault()
        //            })
        //            .ToList()
        //            .Select(dto => new FormInputDto
        //            {
        //                Id = dto.Id,
        //                FormName = dto.FormName,
        //                UserId = dto.UserId,
        //                CustomerName = dto.CustomerName,
        //                CreatedDate = dto.CreatedDate,
        //                UserRoleType = dto.UserRoleName
        //            })
        //            .ToList();

        //        return _resultService.SuccessObject(submittedFormListDto);
        //    }
        //    catch (Exception exception)
        //    {
        //        return ExceptionResult(exception);
        //    }
        //}

        public ResultDto SubmittedFormDetailsbyIdList(FormIdDto formId)
        {
            _methodName = "SubmittedFormDetailsbyIdList";
            try
            {
                var QuestionAnswer = _emamiContext.SubmittedFormQuestions
                                           .Where(q => q.SubmittedFormId == formId.FormId)
                                           .Select(q => new QuestionAnswerInput
                                           {
                                               QuestionId = q.QuestionId,
                                               Query = q.Query,
                                               QuestionTypeName = q.QuestionTypeName,
                                               Answer = q.Answer
                                           }).ToList();

                return _resultService.SuccessObject(QuestionAnswer);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }



        #endregion

        //    #region Question Master
        public ResultDto AddQuestionAndAnswers(QuestionAddDto questionAddDto)
        {
            _methodName = "AddQuestionAndAnswers";
            try
            {
                if (questionAddDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (string.IsNullOrEmpty(questionAddDto.Query))
                {
                    return _resultService.ErrorMessage(Constants.QuestionEmpty);
                }
                //if (questionAddDto.SectionId == 0)
                //{
                //    return _resultService.ErrorMessage(Constants.SectionIdMissing);
                //}
                if (questionAddDto.QuestionTypeId == 0)
                {
                    return _resultService.ErrorMessage(Constants.QuestionTypeIdMissing);
                }
                if (questionAddDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                if (!_resultService.UserIsAcive(questionAddDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                if (questionAddDto.QuestionTypeId == (int)DTO.Enums.QuestionType.MultipleChoice || questionAddDto.QuestionTypeId == (int)DTO.Enums.QuestionType.SingleChoice)
                {
                    if (!questionAddDto.AnswerOptions.Any())
                    {
                        return _resultService.ErrorMessage(Constants.QuestionAnswerOptionMissing);
                    }
                }
                if (questionAddDto.QuestionTypeId != (int)DTO.Enums.QuestionType.MultipleChoice && questionAddDto.QuestionTypeId != (int)DTO.Enums.QuestionType.SingleChoice)
                {
                    if (questionAddDto.AnswerOptions.Any())
                    {
                        return _resultService.ErrorMessage(Constants.AnswerOptionWrong);
                    }
                }
                if (questionAddDto.AnswerOptions.Any())
                {
                    var isAnswerOptionDuplicate = questionAddDto.AnswerOptions.GroupBy(_ => _.Option).Any(_ => _.Count() > 1);
                    if (isAnswerOptionDuplicate)
                    {
                        return _resultService.ErrorMessage(Constants.DuplicateAnswerOptions);
                    }
                }
                var questionQueryContext = _emamiContext.QuestionMasters.AsNoTracking().Count(_ => _.Query == questionAddDto.Query && !_.IsDeleted);
                if (questionQueryContext > 0)
                {
                    return _resultService.ErrorMessage(Constants.QuestionExist);
                }
                var QuestionOrderId = _emamiContext.QuestionMasters.AsNoTracking().Where(_ => !_.IsDeleted).Select(_ => _.OrderId).DefaultIfEmpty(0).Count();
                var questionContext = new QuestionMaster
                {
                    //QuestionSectionId = questionAddDto.SectionId,
                    QuestionTypeId = questionAddDto.QuestionTypeId,
                    Query = questionAddDto.Query.Length > 4000 ? questionAddDto.Query.Substring(0, 3999) : questionAddDto.Query,
                    //Description = questionAddDto.Description.Length > 4000 ? questionAddDto.Description.Substring(0, 3999) : questionAddDto.Description,
                    Textlength = questionAddDto.Textlength,
                    IsMandatory = questionAddDto.IsMandatory,
                    IsDeleted = !questionAddDto.IsActive,
                    OrderId = QuestionOrderId + 1,
                    CreatedBy = questionAddDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                };
                _emamiContext.QuestionMasters.Add(questionContext);
                _emamiContext.SaveChanges();
                if (questionAddDto.AnswerOptions.Any())
                {
                    foreach (var answerOption in questionAddDto.AnswerOptions)
                    {
                        var answerOptionContext = new AnswerOption
                        {
                            QuestionId = questionContext.Id,
                            Option = answerOption.Option.Length > 1000 ? answerOption.Option.Substring(0, 999) : answerOption.Option,
                            CreatedBy = questionAddDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.AnswerOptions.Add(answerOptionContext);
                    }
                    _emamiContext.SaveChanges();
                }
                return _resultService.SuccessMessage(Constants.QuestionSavedSuccessfully);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }
        public ResultDto UpdateQuestionAndAnswers(QuestionAddDto questionAddDto)
        {
            _methodName = "UpdateQuestionAndAnswers";
            try
            {
                if (questionAddDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                //if (questionAddDto.QuestionId == 0)
                //{
                //    return _resultService.ErrorMessage(Constants.QuestionIdMissing);
                //}
                if (string.IsNullOrEmpty(questionAddDto.Query))
                {
                    return _resultService.ErrorMessage(Constants.QuestionEmpty);
                }
                //if (questionAddDto.SectionId == 0)
                //{
                //    return _resultService.ErrorMessage(Constants.SectionIdMissing);
                //}
                //if (questionAddDto.QuestionTypeId == 0)
                //{
                //    return _resultService.ErrorMessage(Constants.QuestionTypeIdMissing);
                //}
                if (questionAddDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                if (!_resultService.UserIsAcive(questionAddDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                //if (questionAddDto.QuestionTypeId == (int)DTO.Enums.QuestionType.MultipleChoice || questionAddDto.QuestionTypeId == (int)DTO.Enums.QuestionType.SingleChoice)
                //{
                //    if (!questionAddDto.AnswerOptions.Any())
                //    {
                //        return _resultService.ErrorMessage(Constants.QuestionAnswerOptionMissing);
                //    }
                //}
                //if (questionAddDto.QuestionTypeId != (int)DTO.Enums.QuestionType.MultipleChoice && questionAddDto.QuestionTypeId != (int)DTO.Enums.QuestionType.SingleChoice)
                //{
                //    if (questionAddDto.AnswerOptions.Any())
                //    {
                //        return _resultService.ErrorMessage(Constants.AnswerOptionWrong);
                //    }
                //}
                if (questionAddDto.AnswerOptions.Any())
                {
                    var isAnswerOptionDuplicate = questionAddDto.AnswerOptions.GroupBy(_ => _.Option).Any(_ => _.Count() > 1);
                    if (isAnswerOptionDuplicate)
                    {
                        return _resultService.ErrorMessage(Constants.DuplicateAnswerOptions);
                    }
                }
                var questionQueryContext = _emamiContext.QuestionMasters.AsNoTracking().Count(_ => _.Query == questionAddDto.Query && _.Id != questionAddDto.QuestionId && !_.IsDeleted);
                if (questionQueryContext > 0)
                {
                    return _resultService.ErrorMessage(Constants.QuestionExist);
                }
                if (questionAddDto.AnswerOptions.Any())
                {
                    foreach (var answerOption in questionAddDto.AnswerOptions)
                    {
                        var answerOptinContext = _emamiContext.AnswerOptions.AsNoTracking().Count(_ => _.QuestionId == questionAddDto.QuestionId && _.Id != answerOption.AnswerOptionId && _.Option == answerOption.Option && !_.IsDeleted);
                        if (answerOptinContext > 0)
                        {
                            return _resultService.ErrorMessage(Constants.AnswerOptionExist);
                        }
                    }
                }
                var questionContext = _emamiContext.QuestionMasters.FirstOrDefault(_ => _.Id == questionAddDto.QuestionId);
                if (questionContext == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
                //Question Type Change validation
                if (questionContext.QuestionTypeId != questionAddDto.QuestionTypeId)
                {
                    var questionInCheckList = _emamiContext.FormQuestions.AsNoTracking().Count(_ => _.Form.IsActive && _.QuestionId == questionAddDto.QuestionId);
                    if (questionInCheckList > 0)
                    {
                        return _resultService.ErrorMessage(Constants.QuestionUsedInForm);
                    }
                    //Delet all Answer Option,if old Question type in Choice and new type is NOT choice type
                    if ((questionContext.QuestionTypeId == (int)DTO.Enums.QuestionType.MultipleChoice || questionContext.QuestionTypeId == (int)DTO.Enums.QuestionType.SingleChoice) && (questionAddDto.QuestionTypeId == (int)DTO.Enums.QuestionType.YesOrNo || questionAddDto.QuestionTypeId == (int)DTO.Enums.QuestionType.TextEntry))
                    {
                        if (questionContext.AnswerOptions.Any())
                        {
                            foreach (var optionContext in questionContext.AnswerOptions.ToList())
                            {
                                _emamiContext.AnswerOptions.Remove(optionContext);
                            }
                            _emamiContext.SaveChanges();
                        }
                    }
                }
                //questionContext.QuestionSectionId = questionAddDto.SectionId;
                questionContext.QuestionTypeId = questionAddDto.QuestionTypeId;
                questionContext.Query = questionAddDto.Query.Length > 4000 ? questionAddDto.Query.Substring(0, 3999) : questionAddDto.Query;
                //questionContext.Description = questionAddDto.Description.Length > 4000 ? questionAddDto.Description.Substring(0, 3999) : questionAddDto.Description;
                questionContext.Textlength = questionAddDto.Textlength;
                questionContext.IsMandatory = questionAddDto.IsMandatory;
                questionContext.IsDeleted = !questionAddDto.IsActive;
                questionContext.ModifiedBy = questionAddDto.LoginUserId;
                questionContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();

                if (questionAddDto.AnswerOptions.Any())
                {
                    foreach (var answerOption in questionAddDto.AnswerOptions)
                    {
                        if (answerOption.AnswerOptionId > 0)
                        {
                            //Update Answer option
                            var answerOptionContext = _emamiContext.AnswerOptions.FirstOrDefault(_ => _.Id == answerOption.AnswerOptionId);
                            if (answerOptionContext != null)
                            {
                                answerOptionContext.Option = answerOption.Option;
                                answerOptionContext.ModifiedBy = questionAddDto.LoginUserId;
                                answerOptionContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            }
                        }
                        else
                        {
                            //Insert Answer Option
                            var answerOptionContext = new AnswerOption
                            {
                                QuestionId = questionContext.Id,
                                Option = answerOption.Option,
                                CreatedBy = questionAddDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            };
                            _emamiContext.AnswerOptions.Add(answerOptionContext);
                        }
                    };
                }
                //Remove Answer options
                if (questionAddDto.RemovedAnswerIds.Any())
                {
                    foreach (var removedAnswerId in questionAddDto.RemovedAnswerIds)
                    {
                        var answerOptionContext = _emamiContext.AnswerOptions.FirstOrDefault(_ => _.Id == removedAnswerId);
                        if (answerOptionContext != null)
                        {
                            //_airportContext.AnswerOption.Remove(answerOptionContext);
                            answerOptionContext.IsDeleted = true;
                        }
                    }
                }
                _emamiContext.SaveChanges();

                if (!questionAddDto.IsActive)
                {
                    var QuestionMaster = _emamiContext.QuestionMasters.Where(_ => !_.IsDeleted && _.OrderId > questionContext.OrderId);
                    foreach (var item in QuestionMaster)
                    {
                        --item.OrderId;
                    }
                    questionContext.OrderId = 0;
                    _emamiContext.SaveChanges();
                }

                return _resultService.SuccessMessage(Constants.QuestionSavedSuccessfully);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }
        //    public ResultDto DeleteSectionQuestions(QuestionRemoveDto questionRemoveDto)
        //    {
        //        _methodName = "DeleteSectionQuestions";
        //        try
        //        {
        //            if (questionRemoveDto == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            if (!questionRemoveDto.QuestionIds.Any())
        //            {
        //                return _resultService.ErrorMessage(Constants.QuestionIdMissing);
        //            }
        //            foreach (var questionId in questionRemoveDto.QuestionIds)
        //            {
        //                var questionContext = _emamiContext.QuestionMasters.FirstOrDefault(_ => _.Id == questionId);
        //                if (questionContext != null)
        //                {
        //                    questionContext.IsDeleted = true;
        //                    if (questionContext.AnswerOptions.Where(_ => !_.IsDeleted).Any())
        //                    {
        //                        foreach (var option in questionContext.AnswerOptions.Where(_ => !_.IsDeleted).ToList())
        //                        {
        //                            var answerOptionContext = questionContext.AnswerOptions.First(_ => _.Id == option.Id);
        //                            if (answerOptionContext != null)
        //                            {
        //                                answerOptionContext.IsDeleted = true;
        //                            }
        //                        }
        //                        //Check other conditions
        //                        var checklistquestionContext = questionContext.FormQuestions.Where(_ => _.QuestionId == questionContext.Id).ToList();
        //                        if (checklistquestionContext != null)
        //                        {
        //                            foreach (var checklistquestion in checklistquestionContext)
        //                            {
        //                                checklistquestion.IsDeleted = true;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            _emamiContext.SaveChanges();
        //            return _resultService.SuccessMessage(Constants.QuestionAnswerOptionDeleted);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }
        //private Dictionary<long, List<string>> GetSubmittedAnswers(long submittedFormId)
        //{
        //    var submittedAnswers = (from sfq in _emamiContext.SubmittedFormQuestions
        //                            join sf in _emamiContext.SubmittedForms on sfq.SubmittedFormId equals sf.Id
        //                            where sf.Id == submittedFormId
        //                            select new
        //                            {
        //                                sfq.QuestionId,
        //                                sfq.Answer
        //                            }).ToList();

        //    var answerDictionary = new Dictionary<long, List<string>>();

        //    foreach (var answer in submittedAnswers)
        //    {
        //        if (!answerDictionary.ContainsKey(answer.QuestionId))
        //        {
        //            answerDictionary[answer.QuestionId] = new List<string>();
        //        }
        //        answerDictionary[answer.QuestionId].Add(answer.Answer);
        //    }

        //    return answerDictionary;
        //}


        //public ResultDto GetSectionQuestions(FormIdDto formId)
        //{
        //    var sectionQuestionsViewDto = new SectionQuestionsViewDto
        //    {
        //        Questions = new List<QuestionsViewDto>()
        //    };

        //    try
        //    {
        //        // Fetch submitted answers for the specific form ID
        //        var submittedAnswers = GetSubmittedAnswers(formId.FormId);

        //        var sectionQuestionListContext = (from f in _emamiContext.Forms
        //                                          join fq in _emamiContext.FormQuestions on f.Id equals fq.FormId
        //                                          join qm in _emamiContext.QuestionMasters on fq.QuestionId equals qm.Id
        //                                          where f.Id == formId.FormId && !qm.IsDeleted
        //                                          select qm).ToList();

        //        foreach (var sectionQuestion in sectionQuestionListContext)
        //        {
        //            var questionsViewDto = new QuestionsViewDto
        //            {
        //                QuestionTypeId = sectionQuestion.QuestionTypeId,
        //                QuestionTypeName = sectionQuestion.QuestionType.Name,
        //                QuestionId = sectionQuestion.Id,
        //                Query = sectionQuestion.Query,
        //                Description = sectionQuestion.Description,
        //                IsDeleted = sectionQuestion.IsDeleted,
        //                IsActive = !sectionQuestion.IsDeleted,
        //                IsMandatory = sectionQuestion.IsMandatory,
        //                OrderId = sectionQuestion.OrderId ?? 0,
        //                // Assign submitted answers directly
        //                SubmittedAnswer = submittedAnswers.ContainsKey(sectionQuestion.Id)
        //                                 ? string.Join(", ", submittedAnswers[sectionQuestion.Id])
        //                                 : string.Empty,
        //                AnswerOptions = new List<AnswerOptionDto>()
        //            };

        //            sectionQuestionsViewDto.Questions.Add(questionsViewDto);
        //        }

        //        sectionQuestionsViewDto.Questions = sectionQuestionsViewDto.Questions
        //            .OrderBy(q => q.OrderId).ToList();

        //        return _resultService.SuccessObject(sectionQuestionsViewDto);
        //    }
        //    catch (Exception exception)
        //    {
        //        return ExceptionResult(exception);
        //    }
        //}
        private Dictionary<long, List<string>> GetSubmittedAnswers(long submittedFormId, long userId)
        {

            var submittedAnswers = (from sfq in _emamiContext.SubmittedFormQuestions
                                    join sf in _emamiContext.SubmittedForms on sfq.SubmittedFormId equals sf.Id
                                    where sf.FormId == submittedFormId && sf.UserId == userId
                                    select new
                                    {
                                        sfq.QuestionId,
                                        sfq.Answer
                                    }).ToList();

            var answerDictionary = new Dictionary<long, List<string>>();

            foreach (var answer in submittedAnswers)
            {
                if (!answerDictionary.ContainsKey(answer.QuestionId))
                {
                    answerDictionary[answer.QuestionId] = new List<string>();
                }
                answerDictionary[answer.QuestionId].Add(answer.Answer);
            }

            return answerDictionary;
        }
        public ResultDto GetSectionQuestions(FormIdDto formId)
        {
            var sectionQuestionsViewDto = new SectionQuestionsViewDto
            {
                Questions = new List<QuestionsViewDto>()
            };

            try
            {
                // Check if the form is submitted
                var isFormSubmitted = _emamiContext.SubmittedForms.Any(sf => sf.FormId == formId.FormId);

                // Initialize submitted answers to an empty dictionary
                var submittedAnswers = new Dictionary<long, List<string>>();

                // If the form is submitted, get the submitted answers based on FormId and UserId
                if (isFormSubmitted)
                {
                    submittedAnswers = GetSubmittedAnswers(formId.FormId, formId.UserId);
                }

                // Get the user's roles
                var userRoles = (from ur in _emamiContext.UserRoles
                                 where ur.UserId == formId.UserId
                                 select ur.RoleId).ToList();

                // Fetch questions, question types, and answer options in one query
                var sectionQuestionListContext = (from f in _emamiContext.Forms
                                                  join fq in _emamiContext.FormQuestions on f.Id equals fq.FormId
                                                  join qm in _emamiContext.QuestionMasters on fq.QuestionId equals qm.Id
                                                  join qo in _emamiContext.AnswerOptions on qm.Id equals qo.QuestionId into answerOptionsGroup
                                                  from answerOption in answerOptionsGroup.DefaultIfEmpty()
                                                  where f.Id == formId.FormId &&
                                                        userRoles.Any(roleId => f.RoleIds.Contains(roleId.ToString())) &&
                                                        !qm.IsDeleted && !fq.IsDeleted
                                                  select new
                                                  {
                                                      QuestionMaster = qm,
                                                      AnswerOption = answerOption
                                                  }).ToList();

                // Group by question to avoid duplication in case of multiple answer options
                var groupedQuestions = sectionQuestionListContext
                    .GroupBy(x => x.QuestionMaster.Id)
                    .ToList();

                foreach (var sectionQuestionGroup in groupedQuestions)
                {
                    var question = sectionQuestionGroup.First().QuestionMaster;

                    var questionsViewDto = new QuestionsViewDto
                    {
                        QuestionTypeId = question.QuestionTypeId,
                        QuestionTypeName = question.QuestionType.Name,
                        QuestionId = question.Id,
                        Query = question.Query,
                        Textlength = question.Textlength,
                        Description = question.Description,
                        IsDeleted = question.IsDeleted,
                        IsActive = !question.IsDeleted,
                        IsMandatory = question.IsMandatory,
                        OrderId = question.OrderId ?? 0,
                        // Add submitted answers if the form is submitted
                        SubmittedAnswer = isFormSubmitted && submittedAnswers.ContainsKey(question.Id)
                                         ? string.Join(", ", submittedAnswers[question.Id])
                                         : string.Empty,
                        // Add answer options
                        AnswerOptions = sectionQuestionGroup
                                        .Where(x => x.AnswerOption != null)  // Filter out questions without answer options
                                        .Select(x => new AnswerOptionDto
                                        {
                                            QuestionId = x.QuestionMaster.Id,
                                            AnswerOptionId = x.AnswerOption.Id,  // Assuming there's an Id in AnswerOptions
                                            Option = x.AnswerOption.Option
                                        }).ToList()
                    };

                    sectionQuestionsViewDto.Questions.Add(questionsViewDto);
                }

                // Order questions by OrderId
                sectionQuestionsViewDto.Questions = sectionQuestionsViewDto.Questions
                    .OrderBy(q => q.OrderId).ToList();

                return _resultService.SuccessObject(sectionQuestionsViewDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        //Work
        //public ResultDto GetSectionQuestions(FormIdDto formId)
        //{
        //    var sectionQuestionsViewDto = new SectionQuestionsViewDto
        //    {
        //        Questions = new List<QuestionsViewDto>()
        //    };

        //    try
        //    {
        //        // Fetch submitted answers for the specific form and user
        //        var submittedAnswers = GetSubmittedAnswers(formId.FormId, formId.UserId);

        //        // Get the user's roles
        //        var userRoles = (from ur in _emamiContext.UserRoles
        //                         where ur.UserId == formId.UserId
        //                         select ur.RoleId).ToList();

        //        // Fetch questions, question types, and answer options in one query
        //        var sectionQuestionListContext = (from f in _emamiContext.Forms
        //                                          join fq in _emamiContext.FormQuestions on f.Id equals fq.FormId
        //                                          join qm in _emamiContext.QuestionMasters on fq.QuestionId equals qm.Id
        //                                          join qo in _emamiContext.AnswerOptions on qm.Id equals qo.QuestionId into answerOptionsGroup
        //                                          from answerOption in answerOptionsGroup.DefaultIfEmpty()
        //                                          where f.Id == formId.FormId &&
        //                                                userRoles.Any(roleId => f.RoleIds.Contains(roleId.ToString())) &&
        //                                                !qm.IsDeleted
        //                                          select new
        //                                          {
        //                                              QuestionMaster = qm,
        //                                              AnswerOption = answerOption
        //                                          }).ToList();

        //        // Group by question to avoid duplication in case of multiple answer options
        //        var groupedQuestions = sectionQuestionListContext
        //            .GroupBy(x => x.QuestionMaster.Id)
        //            .ToList();

        //        foreach (var sectionQuestionGroup in groupedQuestions)
        //        {
        //            var question = sectionQuestionGroup.First().QuestionMaster;

        //            var questionsViewDto = new QuestionsViewDto
        //            {
        //                QuestionTypeId = question.QuestionTypeId,
        //                QuestionTypeName = question.QuestionType.Name,
        //                QuestionId = question.Id,
        //                Query = question.Query,
        //                Description = question.Description,
        //                IsDeleted = question.IsDeleted,
        //                IsActive = !question.IsDeleted,
        //                IsMandatory = question.IsMandatory,
        //                OrderId = question.OrderId ?? 0,
        //                // Add submitted answers
        //                SubmittedAnswer = submittedAnswers.ContainsKey(question.Id)
        //                                 ? string.Join(", ", submittedAnswers[question.Id])
        //                                 : string.Empty,
        //                // Add answer options
        //                AnswerOptions = sectionQuestionGroup
        //                                .Where(x => x.AnswerOption != null)  // Filter out questions without answer options
        //                                .Select(x => new AnswerOptionDto
        //                                {
        //                                    QuestionId = x.QuestionMaster.Id,
        //                                    AnswerOptionId = x.AnswerOption.Id,  // Assuming there's an Id in AnswerOptions
        //                                    Option = x.AnswerOption.Option
        //                                }).ToList()
        //            };

        //            sectionQuestionsViewDto.Questions.Add(questionsViewDto);
        //        }

        //        // Order questions by OrderId
        //        sectionQuestionsViewDto.Questions = sectionQuestionsViewDto.Questions
        //            .OrderBy(q => q.OrderId).ToList();

        //        return _resultService.SuccessObject(sectionQuestionsViewDto);
        //    }
        //    catch (Exception exception)
        //    {
        //        return ExceptionResult(exception);
        //    }
        //}



        public ResultDto GetSectionQuestionsList()
        {
            _methodName = "GetSectionQuestions";
            var sectionQuestionsViewDto = new SectionQuestionsViewDto();
            try
            {
                //if (sectionIdDto == null)
                //{
                //    return _resultService.ErrorMessage(Constants.InvalidRequest);
                //}

                var sectionQuestionListContext = _emamiContext.QuestionMasters.AsNoTracking().ToList();

                if (sectionQuestionListContext.Any())
                {
                    foreach (var sectionQuestion in sectionQuestionListContext)
                    {
                        var questionsViewDto = new QuestionsViewDto
                        {
                            QuestionTypeId = sectionQuestion.QuestionTypeId,
                            QuestionTypeName = sectionQuestion.QuestionType.Name,
                            QuestionId = sectionQuestion.Id,
                            Query = sectionQuestion.Query,
                            Textlength = sectionQuestion.Textlength,
                            Description = sectionQuestion.Description,
                            IsDeleted = sectionQuestion.IsDeleted,
                            IsActive = !sectionQuestion.IsDeleted,
                            IsMandatory = sectionQuestion.IsMandatory,
                            OrderId = sectionQuestion.OrderId ?? 0
                        };
                        if (sectionQuestion.QuestionTypeId == (int)DTO.Enums.QuestionType.MultipleChoice || sectionQuestion.QuestionTypeId == (int)DTO.Enums.QuestionType.SingleChoice)
                        {
                            if (sectionQuestion.AnswerOptions.Where(_ => !_.IsDeleted).Any())
                            {
                                foreach (var answerOption in sectionQuestion.AnswerOptions.Where(_ => !_.IsDeleted).ToList())
                                {
                                    var answerOptionDto = new AnswerOptionDto
                                    {
                                        AnswerOptionId = answerOption.Id,
                                        Option = answerOption.Option
                                    };
                                    questionsViewDto.AnswerOptions.Add(answerOptionDto);
                                }
                            }
                        }
                        sectionQuestionsViewDto.Questions.Add(questionsViewDto);
                    }
                    sectionQuestionsViewDto.Questions = sectionQuestionsViewDto.Questions.OrderBy(_ => _.CreatedDate).ToList();
                }
                return _resultService.SuccessObject(sectionQuestionsViewDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto GetSectionQuestionsFormList()
        {
            _methodName = "GetSectionQuestionsFormList";
            var sectionQuestionsViewDto = new SectionQuestionsViewDto();
            try
            {
                //if (sectionIdDto == null)
                //{
                //    return _resultService.ErrorMessage(Constants.InvalidRequest);
                //}

                var sectionQuestionListContext = _emamiContext.QuestionMasters.AsNoTracking().Where(_ => !_.IsDeleted).ToList();

                if (sectionQuestionListContext.Any())
                {
                    foreach (var sectionQuestion in sectionQuestionListContext)
                    {
                        var questionsViewDto = new QuestionsViewDto
                        {
                            QuestionTypeId = sectionQuestion.QuestionTypeId,
                            QuestionTypeName = sectionQuestion.QuestionType.Name,
                            QuestionId = sectionQuestion.Id,
                            Query = sectionQuestion.Query,
                            Textlength = sectionQuestion.Textlength,
                            Description = sectionQuestion.Description,
                            IsDeleted = sectionQuestion.IsDeleted,
                            IsActive = !sectionQuestion.IsDeleted,
                            IsMandatory = sectionQuestion.IsMandatory,
                            OrderId = sectionQuestion.OrderId ?? 0
                        };
                        if (sectionQuestion.QuestionTypeId == (int)DTO.Enums.QuestionType.MultipleChoice || sectionQuestion.QuestionTypeId == (int)DTO.Enums.QuestionType.SingleChoice)
                        {
                            if (sectionQuestion.AnswerOptions.Where(_ => !_.IsDeleted).Any())
                            {
                                foreach (var answerOption in sectionQuestion.AnswerOptions.Where(_ => !_.IsDeleted).ToList())
                                {
                                    var answerOptionDto = new AnswerOptionDto
                                    {
                                        AnswerOptionId = answerOption.Id,
                                        Option = answerOption.Option
                                    };
                                    questionsViewDto.AnswerOptions.Add(answerOptionDto);
                                }
                            }
                        }
                        sectionQuestionsViewDto.Questions.Add(questionsViewDto);
                    }
                    sectionQuestionsViewDto.Questions = sectionQuestionsViewDto.Questions.OrderBy(_ => _.OrderId).ToList();
                }
                return _resultService.SuccessObject(sectionQuestionsViewDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto ViewQuestionDetails(QuestionIdDto questionIdDto)
        {
            _methodName = "ViewQuestionDetails";
            var questionDto = new QuestionMasterDto();
            try
            {
                if (questionIdDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (questionIdDto.QuestionId == 0)
                {
                    return _resultService.ErrorMessage(Constants.QuestionIdMissing);
                }
                var questionContext = _emamiContext.QuestionMasters.AsNoTracking().FirstOrDefault(_ => _.Id == questionIdDto.QuestionId);
                if (questionContext == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
                questionDto.QuestionTypeId = questionContext.QuestionTypeId;
                questionDto.QuestionTypeName = questionContext.QuestionType.Name;
                questionDto.QuestionId = questionContext.Id;
                questionDto.Query = questionContext.Query;
                questionDto.Textlength = questionContext.Textlength;
                questionDto.Description = questionContext.Description;
                questionDto.IsActive = !questionContext.IsDeleted;
                questionDto.IsMandatory = questionContext.IsMandatory;

                if (questionContext.AnswerOptions.Where(_ => !_.IsDeleted).Any())
                {
                    foreach (var answerOption in questionContext.AnswerOptions.Where(_ => !_.IsDeleted).ToList())
                    {
                        var answerOptionDto = new AnswerOptionDto
                        {
                            QuestionId = answerOption.QuestionId,
                            AnswerOptionId = answerOption.Id,
                            Option = answerOption.Option,
                        };
                        questionDto.AnswerOptions.Add(answerOptionDto);
                    }
                }
                return _resultService.SuccessObject(questionDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        //    public ResultDto SaveQuestionOrder(List<QuestionrOrderDto> inputDto)
        //    {
        //        _methodName = "ViewQuestionDetails";
        //        var questionDto = new QuestionMasterDto();
        //        try
        //        {
        //            if (inputDto == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            if (!inputDto.Any())
        //            {
        //                return _resultService.ErrorMessage(Constants.QuestionEmpty);
        //            }
        //            var sectionId = inputDto.FirstOrDefault().SectionId;
        //            var QuestionList = _emamiContext.QuestionMasters.Where(_ => _.QuestionSectionId == sectionId);
        //            if (QuestionList.Any())
        //            {
        //                foreach (var item in QuestionList)
        //                {
        //                    item.OrderId = 0;
        //                }
        //                foreach (var item in inputDto)
        //                {
        //                    var question = QuestionList.FirstOrDefault(_ => _.QuestionSectionId == item.SectionId && _.Id == item.QuestionId);
        //                    if (question != null)
        //                    {
        //                        question.OrderId = item.OrderId;
        //                    }
        //                }
        //                _emamiContext.SaveChanges();
        //            }


        //            return _resultService.SuccessMessage(Constants.QuestionSavedSuccessfully);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }
        //    #endregion

        //    #region Form CRUD operations
        //Form Add
        public ResultDto AddFormAndQuestions(FormAddDto formAddDto)
        {
            _methodName = "AddFormAndQuestions";
            try
            {
                if (formAddDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (string.IsNullOrEmpty(formAddDto.FormName))
                {
                    return _resultService.ErrorMessage(Constants.FormNameEmpty);
                }
                if (!formAddDto.SectionQuestions.Any())
                {
                    return _resultService.ErrorMessage(Constants.FormQuestionsEmpty);
                }
                if (formAddDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                if (!_resultService.UserIsAcive(formAddDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }

                List<PushNotificationInputDto> notificationlist = new List<PushNotificationInputDto>();

                var checklistNameContext = _emamiContext.Forms.AsNoTracking().Count(_ => _.Name == formAddDto.FormName);
                if (checklistNameContext > 0)
                {
                    return _resultService.ErrorMessage(Constants.FormNameExist);
                }
                var formContext = new Form
                {
                    Name = formAddDto.FormName.Length > 2000 ? formAddDto.FormName.Substring(0, 1999) : formAddDto.FormName,
                    ParentFormId = formAddDto.ParentFormId,
                    IsActive = formAddDto.IsActive,
                    IsFormStatus = formAddDto.IsFormStatus,
                    RoleIds = string.Join(",", formAddDto.RoleIds),
                    CreatedBy = formAddDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                };
                _emamiContext.Forms.Add(formContext);
                _emamiContext.SaveChanges();

                if (formAddDto.FormUsers.Any())
                {
                    var UserContext = _emamiContext.Users.AsNoTracking().ToList();
                    var Formdetails = this.GetFormQuestions(new FormIdDto() { FormId = formContext.Id });
                    var NotificationContent = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(_ => _.Name.Equals(Constants.NewFormAssigned));

                    foreach (var user in formAddDto.FormUsers)
                    {
                        var formUser = new FormUser
                        {
                            FormId = formContext.Id,
                            UserId = user,
                            IsActive = true,
                            CreatedBy = formAddDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        var userdetail = UserContext.FirstOrDefault(_ => _.Id == user && _.IsActive && !string.IsNullOrEmpty(_.PushTokenKey));
                        if (userdetail != null && NotificationContent != null)
                        {
                            notificationlist.Add(new PushNotificationInputDto
                            {
                                PushTokenKey = userdetail.PushTokenKey,
                                RegistrationTypeId = userdetail.RegistrationTypeId != null ? (int)userdetail.RegistrationTypeId : 0,
                                Title = Constants.NewFormAssigned,
                                NotificationTypeId = (int)DTO.Enums.NotificationTypeForms.FormAssigned,
                                Message = NotificationContent.PlainTemplate.Replace(Constants.FormName, formAddDto.FormName),
                                IsCMSNotification = true,
                                SubmittedFormId = formContext.Id
                                //NotificationObject = Formdetails != null && Formdetails.IsSuccess ? Formdetails.SuccessDto : null
                            });
                        }
                        _emamiContext.FormUsers.Add(formUser);
                    }
                    _emamiContext.SaveChanges();
                }

                if (formAddDto.SectionQuestions.Any())
                {
                    foreach (var sectionquestion in formAddDto.SectionQuestions)
                    {
                        var formQuestionContext = new FormQuestion
                        {
                            FormId = formContext.Id,
                            QuestionId = sectionquestion.QuestionId,
                            QuestionSectionId = sectionquestion.SectionId,
                            //OrderNo = sectionquestion.Questions.OrderNo,
                            CreatedBy = formAddDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.FormQuestions.Add(formQuestionContext);
                    }
                    _emamiContext.SaveChanges();
                }

                foreach (var notification in notificationlist)
                {
                    _notificationService.SendPushNotificationThroughFirebase(notification);
                }
                return _resultService.SuccessMessage(Constants.FormSavedSuccessfully);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto GetFormQuestions(FormIdDto formIdDto)
        {
            _methodName = "GetFormQuestions";
            var formQuestionsViewDto = new FormQuestionsViewDto();
            try
            {
                if (formIdDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (formIdDto.FormId == 0)
                {
                    return _resultService.ErrorMessage(Constants.FormIdMissing);
                }
                var FormQuestionListContext = _emamiContext.FormQuestions.AsNoTracking().Where(_ => _.FormId == formIdDto.FormId && !_.IsDeleted).ToList();
                if (FormQuestionListContext.Any())
                {
                    var formContext = FormQuestionListContext.FirstOrDefault(_ => _.FormId == formIdDto.FormId).Form;
                    if (formContext != null)
                    {
                        formQuestionsViewDto.FormId = formContext.Id;
                        formQuestionsViewDto.FormName = formContext.Name;
                        formQuestionsViewDto.IsFormStatus = formContext.IsFormStatus;
                        formQuestionsViewDto.RoleIds = formContext.RoleIds.Split(',').Select(long.Parse).ToList();
                        formQuestionsViewDto.IsActive = formContext.IsActive;
                        formQuestionsViewDto.DependentFormId = formContext.ParentFormId;
                    }

                    //var groupedSectionContext = FormQuestionListContext.GroupBy(_ => _.QuestionSectionId)
                    //                                                    .Select(group => new
                    //                                                    {
                    //                                                        group.Key,
                    //                                                        sectionItems = group.ToList()
                    //                                                    }).ToList();
                    //foreach (var sectionQuestion in groupedSectionContext)
                    //{
                    //var sectionsDto = new SectionQuestionsViewDto
                    //{
                    //    SectionId = sectionQuestion.Key,
                    //};
                    foreach (var question in FormQuestionListContext)
                    {
                        var questionsViewDto = new QuestionsViewDto
                        {
                            QuestionTypeId = question.Question.QuestionTypeId,
                            QuestionTypeName = question.Question.QuestionType.Name,
                            Query = question.Question.Query,
                            QuestionId = question.Question.Id,
                            //OrderNo = question.OrderNo,
                            IsDeleted = question.IsDeleted,
                            Description = question.Question.Description,
                            IsMandatory = question.Question.IsMandatory
                        };
                        formQuestionsViewDto.Query = question.Question.Query;
                        formQuestionsViewDto.QuestionTypeName = question.Question.QuestionType.Name;
                        if (question.Question.QuestionTypeId == (int)DTO.Enums.QuestionType.MultipleChoice || question.Question.QuestionTypeId == (int)DTO.Enums.QuestionType.SingleChoice)
                        {
                            if (question.Question.AnswerOptions.Where(_ => !_.IsDeleted).Any())
                            {
                                foreach (var answerOption in question.Question.AnswerOptions.Where(_ => !_.IsDeleted).ToList())
                                {
                                    var answerOptionDto = new AnswerOptionDto
                                    {
                                        AnswerOptionId = answerOption.Id,
                                        Option = answerOption.Option
                                    };
                                    questionsViewDto.AnswerOptions.Add(answerOptionDto);
                                }
                            }
                        }
                        formQuestionsViewDto.Questions.Add(questionsViewDto);
                    }
                    //formQuestionsViewDto.SectionQuestions.Add(sectionsDto);

                    //}
                }

                //Get Form assigned to users list
                //var FormUserList = _emamiContext.FormUsers.AsNoTracking().Where(_ => _.FormId == formIdDto.FormId && _.IsActive)
                //                                                         .Select(_ => _.UserId).ToList();
                //if (FormUserList != null && FormUserList.Count > 0)
                //{
                //    formQuestionsViewDto.FormUsers.AddRange(FormUserList);
                //}
                return _resultService.SuccessObject(formQuestionsViewDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }
        public ResultDto UpdateFormAndQuestions(FormUpdateDto formUpdateDto)
        {
            _methodName = "UpdateFormAndQuestions";
            try
            {
                if (formUpdateDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (string.IsNullOrEmpty(formUpdateDto.FormName))
                {
                    return _resultService.ErrorMessage(Constants.FormNameEmpty);
                }
                if (formUpdateDto.FormId == 0)
                {
                    return _resultService.ErrorMessage(Constants.FormIdMissing);
                }
                if (formUpdateDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                if (!_resultService.UserIsAcive(formUpdateDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                var formNameContext = _emamiContext.Forms.AsNoTracking().Count(_ => _.Name == formUpdateDto.FormName && _.Id != formUpdateDto.FormId);
                if (formNameContext > 0)
                {
                    return _resultService.ErrorMessage(Constants.FormNameExist);
                }
                var formContext = _emamiContext.Forms.FirstOrDefault(_ => _.Id == formUpdateDto.FormId);
                if (formContext == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                formContext.Name = formUpdateDto.FormName.Length > 2000 ? formUpdateDto.FormName.Substring(0, 1999) : formUpdateDto.FormName;
                formContext.ParentFormId = formUpdateDto.ParentFormId;
                formContext.IsActive = formUpdateDto.IsActive;
                formContext.IsFormStatus = formUpdateDto.IsFormStatus;
                formContext.RoleIds = string.Join(",", formUpdateDto.RoleIds);
                formContext.ModifiedBy = formUpdateDto.LoginUserId;
                formContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();

                List<PushNotificationInputDto> notificationlist = new List<PushNotificationInputDto>();

                //Remove all users and update the modified
                var savedFormUsersContext = _emamiContext.FormUsers.Where(_ => _.FormId == formContext.Id && _.IsActive);
                foreach (var form in savedFormUsersContext)
                {
                    form.IsActive = false;
                }
                _emamiContext.SaveChanges();
                if (formUpdateDto.NewUsers.Any())
                {
                    var UserContext = _emamiContext.Users.AsNoTracking().ToList();
                    var Formdetails = this.GetFormQuestions(new FormIdDto() { FormId = formContext.Id });
                    var NotificationContent = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(_ => _.Name.Equals(Constants.NewFormAssigned));

                    foreach (var user in formUpdateDto.NewUsers)
                    {
                        var formUsersContext = _emamiContext.FormUsers.FirstOrDefault(_ => _.FormId == formContext.Id && _.UserId == user);
                        if (formUsersContext != null && !formUsersContext.IsActive)
                        {
                            formUsersContext.IsActive = true;
                            var userdetail = UserContext.FirstOrDefault(_ => _.Id == user && _.IsActive && !string.IsNullOrEmpty(_.PushTokenKey));
                            if (userdetail != null && NotificationContent != null)
                            {
                                notificationlist.Add(new PushNotificationInputDto
                                {
                                    PushTokenKey = userdetail.PushTokenKey,
                                    RegistrationTypeId = userdetail.RegistrationTypeId != null ? (int)userdetail.RegistrationTypeId : 0,
                                    Title = Constants.NewFormAssigned,
                                    NotificationTypeId = (int)DTO.Enums.NotificationTypeForms.FormAssigned,
                                    Message = NotificationContent.PlainTemplate.Replace(Constants.FormName, formUpdateDto.FormName),
                                    IsCMSNotification = true,
                                    SubmittedFormId = formContext.Id
                                    //NotificationObject = Formdetails != null && Formdetails.IsSuccess ? Formdetails.SuccessDto : null
                                });
                            }
                        }
                        else
                        {
                            var formNewUserContext = new FormUser
                            {
                                FormId = formContext.Id,
                                UserId = user,
                                IsActive = true,
                                CreatedBy = formUpdateDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                            };
                            var userdetail = UserContext.FirstOrDefault(_ => _.Id == user && _.IsActive && !string.IsNullOrEmpty(_.PushTokenKey));
                            if (userdetail != null && NotificationContent != null)
                            {
                                notificationlist.Add(new PushNotificationInputDto
                                {
                                    PushTokenKey = userdetail.PushTokenKey,
                                    RegistrationTypeId = userdetail.RegistrationTypeId != null ? (int)userdetail.RegistrationTypeId : 0,
                                    Title = Constants.NewFormAssigned,
                                    NotificationTypeId = (int)DTO.Enums.NotificationTypeForms.FormAssigned,
                                    Message = NotificationContent.PlainTemplate.Replace(Constants.FormName, formUpdateDto.FormName),
                                    IsCMSNotification = true,
                                    SubmittedFormId = formContext.Id
                                    //NotificationObject = Formdetails != null && Formdetails.IsSuccess ? Formdetails.SuccessDto : null
                                });
                            }
                            _emamiContext.FormUsers.Add(formNewUserContext);
                        }
                    }
                    _emamiContext.SaveChanges();
                }

                //if (formUpdateDto.NewQuestions.Any()/* || formUpdateDto.RemovedQuestions.Any()*/)
                //{
                //    //Remove all questions in Form
                var formSavedQuestionContext = _emamiContext.FormQuestions.Where(_ => _.FormId == formUpdateDto.FormId && !_.IsDeleted);
                foreach (var form in formSavedQuestionContext)
                {
                    form.IsDeleted = true;
                }
                _emamiContext.SaveChanges();

                //Add new questions
                if (formUpdateDto.SectionQuestions.Any())
                {
                    foreach (var question in formUpdateDto.SectionQuestions)
                    {
                        var formQuestionExistsContext = _emamiContext.FormQuestions.FirstOrDefault(_ => _.QuestionId == question.QuestionId && _.FormId == formUpdateDto.FormId);
                        if (formQuestionExistsContext != null && formQuestionExistsContext.IsDeleted)
                        {
                            formQuestionExistsContext.IsDeleted = false;
                        }
                        else
                        {
                            var formNewQuestionContext = new FormQuestion
                            {
                                FormId = formContext.Id,
                                QuestionId = question.QuestionId,
                                QuestionSectionId = question.SectionId,
                                CreatedBy = formUpdateDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                                //OrderNo = question.OrderNo
                            };
                            _emamiContext.FormQuestions.Add(formNewQuestionContext);
                        }
                    }
                }
                _emamiContext.SaveChanges();
                ////Remove Questions
                //if (formUpdateDto.RemovedQuestions.Any())
                //{
                //    foreach (var question in formUpdateDto.RemovedQuestions)
                //    {
                //        var formQuestionContext = _emamiContext.FormQuestions.FirstOrDefault(_ => _.QuestionId == question.QuestionId && _.QuestionSectionId == question.SectionId && _.FormId == formUpdateDto.FormId);
                //        if (formQuestionContext != null)
                //        {
                //            formQuestionContext.IsDeleted = true;
                //        }
                //    }
                //}
                //_emamiContext.SaveChanges();
                //}
                foreach (var notification in notificationlist)
                {
                    _notificationService.SendPushNotificationThroughFirebase(notification);
                }
                return _resultService.SuccessMessage(Constants.FormUpdatedSuccessfully);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto GetFormsView()
        {
            _methodName = "GetFormsView";
            var formDto = new List<FormDto>();
            try
            {
                formDto = _emamiContext.Forms.AsNoTracking().OrderByDescending(_ => _.Id).ToList().Select(c => new FormDto
                {
                    FormId = c.Id,
                    FormName = c.Name,
                    IsFormStatus = c.IsFormStatus,
                    RoleIds = c.RoleIds,
                    IsActive = c.IsActive,
                    ParentFormId = c.ParentFormId ?? 0,
                    ParentFormName = GetFormName(c.ParentFormId ?? 0)
                }).ToList();

                return _resultService.SuccessObject(formDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }
        //public ResultDto GetFormsList(UserIdDto inputDto)
        //{
        //    _methodName = "GetFormsList";
        //    List<FormDto> formDto;

        //    try
        //    {
        //        bool isUserDealer = _emamiContext.UserRoles.Any(ur => ur.UserId == inputDto.UserId && ur.RoleId == (int)DTO.Enums.Role.Dealer);

        //        var forms = _emamiContext.Forms.AsNoTracking().Where(c => (isUserDealer && c.IsFormStatus == false) || (!isUserDealer && c.IsFormStatus == true) && c.IsActive)
        //            .Select(c => new
        //            {
        //                c.Id,
        //                c.Name,
        //                c.IsFormStatus,
        //                c.IsActive,
        //                c.ParentFormId,
        //                IsSubmittedForms = _emamiContext.SubmittedForms.Any(sf => sf.FormId == c.Id && sf.UserId == inputDto.UserId)
        //            }).OrderBy(c => c.Name).ToList();

        //        formDto = forms.Select(c => new FormDto
        //        {
        //            FormId = c.Id,
        //            FormName = c.Name,
        //            IsFormStatus = c.IsFormStatus,
        //            IsActive = c.IsActive,
        //            ParentFormId = c.ParentFormId ?? 0,
        //            ParentFormName = GetFormName(c.ParentFormId ?? 0),
        //            IsSubmittedForms = c.IsSubmittedForms
        //        }).ToList();

        //        return _resultService.SuccessObject(formDto);
        //    }
        //    catch (Exception exception)
        //    {
        //        return ExceptionResult(exception);
        //    }
        //}

        public ResultDto GetFormsList(UserIdDto inputDto)
        {
            _methodName = "GetFormsList";
            List<FormDto> formDto;

            try
            {
                var validRoleIds = new List<int>
                {
                    (int)DTO.Enums.Role.Dealer, // 5
                    (int)DTO.Enums.Role.ZonalTrader, // 9
                    (int)DTO.Enums.Role.NationalTrader, // 12
                    (int)DTO.Enums.Role.AreaSalesManager // 14
                };

                var userRoleIds = _emamiContext.UserRoles
                    .Where(ur => ur.UserId == inputDto.UserId)
                    .Select(ur => (int)ur.RoleId)
                    .ToList();

                //bool hasValidRole = userRoleIds.Any(roleId => validRoleIds.Contains(roleId));

                var forms = _emamiContext.Forms.AsNoTracking()
                    .Where(c => c.IsActive)
                    .Select(c => new
                    {
                        c.Id,
                        c.Name,
                        c.IsActive,
                        c.ParentFormId,
                        c.RoleIds,
                        IsSubmittedForms = _emamiContext.SubmittedForms.Any(sf => sf.FormId == c.Id && sf.UserId == inputDto.UserId)
                    })
                    .OrderBy(c => c.Name)
                    .ToList();

                var filteredForms = forms
            .Where(c => !string.IsNullOrEmpty(c.RoleIds) && c.RoleIds.Split(',')
            .Select(roleId =>
            {
                int id;
                return int.TryParse(roleId, out id) ? (int?)id : null;
            })
            .Any(roleId => roleId.HasValue && userRoleIds.Contains(roleId.Value)))
            .ToList();


                formDto = filteredForms.Select(c => new FormDto
                {
                    FormId = c.Id,
                    FormName = c.Name,
                    IsActive = c.IsActive,
                    ParentFormId = c.ParentFormId ?? 0,
                    ParentFormName = GetFormName(c.ParentFormId ?? 0),
                    IsSubmittedForms = c.IsSubmittedForms
                }).ToList();

                return _resultService.SuccessObject(formDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        //    public ResultDto GetComplaintFormsList(LoginUserIdDto inputDto)
        //    {
        //        _methodName = "GetComplaintFormsList";
        //        var formDto = new List<FormDto>();
        //        try
        //        {
        //            IQueryable<Form> formEntity;
        //            if (inputDto.IsToReturnInactiveData)
        //            {
        //                formEntity = _emamiContext.Forms.AsNoTracking().Where(_ => _.ParentFormId == 0 || _.ParentFormId == null).OrderBy(_ => _.Name);
        //            }
        //            else
        //            {
        //                formEntity = _emamiContext.Forms.AsNoTracking().Where(_ => (_.ParentFormId == 0 || _.ParentFormId == null) && _.IsActive).OrderBy(_ => _.Name);
        //            }
        //            formDto = formEntity.Select(c => new FormDto
        //            {
        //                FormId = c.Id,
        //                FormName = c.Name,
        //                IsActive = c.IsActive
        //            }).ToList();
        //            return _resultService.SuccessObject(formDto);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }

        //    #endregion

        //    #region Submit Form CRUD operations
        //public ResultDto SubmitForm(FormInputDto formAddDto)
        //{
        //    _methodName = "SubmitForm";
        //    try
        //    {
        //        if (formAddDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        if (formAddDto.FormId == 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.FormIdMissing);
        //        }
        //        if (formAddDto.LoginUserId == 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserIdMissing);
        //        }
        //        if (!_resultService.UserIsAcive(formAddDto.LoginUserId))
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidUser);
        //        }
        //        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(u => u.Id == formAddDto.CustomerId);

        //        var formContext = _emamiContext.Forms.AsNoTracking().FirstOrDefault(_ => _.Id == formAddDto.FormId);
        //        var submittedFormContext = new SubmittedForm
        //        {
        //            UserId = formAddDto.CustomerId, //Save customer user id
        //            CustomerName = userContext.Name, //Save customer user id
        //            FormId = formAddDto.FormId,
        //            FormName = formContext.Name,
        //            CreatedBy = formAddDto.LoginUserId,
        //            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
        //        };

        //        _emamiContext.SubmittedForms.Add(submittedFormContext);
        //        _emamiContext.SaveChanges();
        //        if (formAddDto.Questions.Any())
        //        {
        //            foreach (var question in formAddDto.Questions)
        //            {
        //                var questionContext = _emamiContext.QuestionMasters.AsNoTracking().FirstOrDefault(_ => _.Id == question.QuestionId);
        //                var submittedQuestionContext = new SubmittedFormQuestion
        //                {
        //                    SubmittedFormId = submittedFormContext.Id,
        //                    QuestionId = question.QuestionId,
        //                    Query = questionContext != null ? questionContext.Query : string.Empty,
        //                    QuestionTypeId = questionContext != null ? questionContext.QuestionTypeId : 0,
        //                    QuestionTypeName = questionContext != null ? questionContext.QuestionType.Name : string.Empty,
        //                    Answer = question.Answer,
        //                    CreatedBy = formAddDto.LoginUserId,
        //                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
        //                };
        //                _emamiContext.SubmittedFormQuestions.Add(submittedQuestionContext);
        //                _emamiContext.SaveChanges();
        //            }
        //        }

        //        return _resultService.SuccessMessage(Constants.FormSavedSuccessfully);
        //    }
        //    catch (Exception exception)
        //    {
        //        return ExceptionResult(exception);
        //    }
        //}
        public ResultDto SubmitForm(FormInputDto formAddDto)
        {
            _methodName = "SubmitForm";
            try
            {
                if (formAddDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (formAddDto.FormId == 0)
                {
                    return _resultService.ErrorMessage(Constants.FormIdMissing);
                }
                if (formAddDto.UserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                if (!_resultService.UserIsAcive(formAddDto.UserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(u => u.Id == formAddDto.UserId);

                var formContext = _emamiContext.Forms.AsNoTracking().FirstOrDefault(_ => _.Id == formAddDto.FormId);
                var submittedFormContext = new SubmittedForm
                {
                    UserId = formAddDto.UserId,
                    CustomerName = userContext?.Name,
                    FormId = formAddDto.FormId,
                    FormName = formContext?.Name,
                    CreatedBy = formAddDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                };

                _emamiContext.SubmittedForms.Add(submittedFormContext);
                _emamiContext.SaveChanges();

                if (formAddDto.QuestionAnswer.Any())
                {
                    foreach (var question in formAddDto.QuestionAnswer)
                    {
                        var questionContext = _emamiContext.QuestionMasters.AsNoTracking().FirstOrDefault(_ => _.Id == question.QuestionId);
                        var submittedQuestionContext = new SubmittedFormQuestion
                        {
                            SubmittedFormId = submittedFormContext.Id,
                            QuestionId = question.QuestionId,
                            Query = questionContext?.Query ?? string.Empty,
                            QuestionTypeId = questionContext?.QuestionTypeId ?? 0,
                            QuestionTypeName = questionContext?.QuestionType.Name ?? string.Empty,
                            Answer = question.Answer,
                            CreatedBy = formAddDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };

                        _emamiContext.SubmittedFormQuestions.Add(submittedQuestionContext);
                        _emamiContext.SaveChanges();
                    }
                }

                return _resultService.SuccessMessage(Constants.FormSubmittedSavedSuccessfully);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }


        public ResultDto ViewSubmittedFormDetails(SubmittedFormIdDto formIdDto)
        {
            _methodName = "ViewSubmittedFormDetails";

            try
            {
                if (formIdDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (formIdDto.SubmittedFormId == 0)
                {
                    return _resultService.ErrorMessage(Constants.FormIdMissing);
                }
                var submittedFormContext = _emamiContext.SubmittedForms.AsNoTracking().FirstOrDefault(_ => _.Id == formIdDto.SubmittedFormId);
                if (submittedFormContext == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
                var submittedFormViewDto = new SubmittedFormViewDto
                {
                    RaisedFor = submittedFormContext.Retailer != null ? submittedFormContext.Retailer.AccountName : submittedFormContext.CustomerName,
                    DealerName = submittedFormContext.DealerName,
                    SubmittedFormId = submittedFormContext.Id,
                    CreatedDate = submittedFormContext.CreatedDate,
                    RaisedBy = GetUserName(submittedFormContext.CreatedBy),
                    FormName = submittedFormContext.FormName,
                    ParentFormId = submittedFormContext.ParentFormId,
                    ParentFormName = submittedFormContext.ParentFormId != null && submittedFormContext.ParentFormId != 0 ? GetSubmittedFormName(submittedFormContext.ParentFormId ?? 0) : string.Empty,
                    DemonstratedBy = GetDemonstratorName(submittedFormContext.DemoId ?? 0),
                    DemoIncharge = GetDemoInchargeName(submittedFormContext.DemoId ?? 0),
                    FormStatusName = submittedFormContext.FormStatus != null ? submittedFormContext.FormStatus.Name : string.Empty,
                    FormApprovalStatusId = submittedFormContext.FormApprovalStatusId ?? 0,
                    FormStatusId = submittedFormContext.FormStatusId ?? 0,
                    Remarks = submittedFormContext.Remarks ?? submittedFormContext.Remarks,
                    EALUserName = GetEALUserName(submittedFormContext.DemoId ?? 0),

                };
                //Add Submitted form details
                var submittedformdetails = _emamiContext.SubmittedFormDetails.AsNoTracking().FirstOrDefault(_ => _.SubmittedFormId == submittedFormContext.Id);
                if (submittedformdetails != null)
                {
                    submittedFormViewDto.SkuId = submittedformdetails.SkuId;
                    submittedFormViewDto.SkuName = submittedformdetails.Sku != null ? submittedformdetails.Sku.SkuName : string.Empty;
                    submittedFormViewDto.PlantId = submittedformdetails.PlantId;
                    submittedFormViewDto.PlantName = GetPlantName(submittedformdetails.PlantId);
                    submittedFormViewDto.StateId = submittedformdetails.StateId;
                    submittedFormViewDto.StateName = submittedformdetails.State != null ? submittedformdetails.State.StateName : string.Empty;
                    submittedFormViewDto.CityId = submittedformdetails.CityId;
                    submittedFormViewDto.CityName = submittedformdetails.City != null ? submittedformdetails.City.CityName : string.Empty;
                }

                //var RemarksContext = _emamiContext.SubmittedFormRemarks.AsNoTracking().Where(_ => _.SubmittedFormId == submittedFormContext.Id).ToList();
                //submittedFormViewDto.Comments = RemarksContext.Select(_ => new FormRemarksDto()
                //{
                //    Description = _.Description,
                //    CreatedOn = _.CreatedDate,
                //    CreatedBy = GetUserName(_.CreatedBy)
                //}).OrderByDescending(_ => _.CreatedOn).ToList();
                if (submittedFormContext.ParentFormId == null)
                {
                    submittedFormViewDto.DependentFormDetails = _emamiContext.SubmittedForms.AsNoTracking().Where(_ => _.ParentFormId == submittedFormContext.Id)
                        .Select(_ => new DropDownDto()
                        {
                            Id = _.Id,
                            Name = _.FormName
                        }).ToList();
                }
                //Add Dependent form details (DependentFormMaster)
                var dependentFormsContext = _emamiContext.Forms.AsNoTracking().Where(_ => _.ParentFormId == submittedFormContext.FormId && _.IsActive).ToList();
                if (dependentFormsContext.Any())
                {
                    foreach (var form in dependentFormsContext)
                    {
                        var dependentForm = new FormQuestionsViewDto()
                        {
                            FormId = form.Id,
                            FormName = form.Name
                        };
                        if (form.FormQuestions.Any())
                        {
                            var dependentSectionContext = form.FormQuestions.Where(_ => !_.IsDeleted).GroupBy(_ => _.QuestionSectionId)
                                                                               .Select(group => new
                                                                               {
                                                                                   group.Key,
                                                                                   sectionItems = group.ToList()
                                                                               }).ToList();
                            foreach (var section in dependentSectionContext)
                            {
                                var sectionDto = new SectionQuestionsViewDto
                                {
                                    SectionId = section.Key,
                                };
                                foreach (var question in section.sectionItems)
                                {
                                    var questionsViewDto = new QuestionsViewDto
                                    {
                                        QuestionTypeId = question.Question.QuestionTypeId,
                                        QuestionTypeName = question.Question.QuestionType.Name,
                                        Query = question.Question.Query,
                                        QuestionId = question.Question.Id,
                                        OrderNo = question.OrderNo,
                                        IsDeleted = question.IsDeleted,
                                        Description = question.Question.Description,
                                        IsMandatory = question.Question.IsMandatory
                                    };
                                    if (question.Question.QuestionTypeId == (int)DTO.Enums.QuestionType.MultipleChoice || question.Question.QuestionTypeId == (int)DTO.Enums.QuestionType.SingleChoice)
                                    {
                                        if (question.Question.AnswerOptions.Where(_ => !_.IsDeleted).Any())
                                        {
                                            foreach (var answerOption in question.Question.AnswerOptions.Where(_ => !_.IsDeleted).ToList())
                                            {
                                                var answerOptionDto = new AnswerOptionDto
                                                {
                                                    AnswerOptionId = answerOption.Id,
                                                    Option = answerOption.Option
                                                };
                                                questionsViewDto.AnswerOptions.Add(answerOptionDto);
                                            }
                                        }
                                    }
                                    sectionDto.Questions.Add(questionsViewDto);
                                }
                                dependentForm.SectionQuestions.Add(sectionDto);
                            }
                            submittedFormViewDto.DependentFormsMaster.Add(dependentForm);
                        }
                    }
                }


                //Add Submitted Dependent form details
                var submittedDependentFormContext = _emamiContext.SubmittedForms.AsNoTracking().Where(_ => _.ParentFormId == submittedFormContext.Id).ToList();
                if (submittedDependentFormContext.Any())
                {
                    foreach (var form in submittedDependentFormContext)
                    {
                        var dependentForm = new SubmittedDependentFormDto
                        {
                            SubmittedFormId = form.Id,
                            FormId = form.FormId,
                            FormName = form.FormName,
                            CreatedDate = form.CreatedDate,
                            DemonstratedBy = GetUserName(form.DemoUserId ?? 0),
                            DemoId = form.DemoId ?? 0
                        };
                        if (form.SubmittedFormQuestions.Any())
                        {
                            var dependentSectionContext = form.SubmittedFormQuestions.GroupBy(_ => _.SectionId)
                                                                               .Select(group => new
                                                                               {
                                                                                   group.Key,
                                                                                   group.FirstOrDefault().SectionName,
                                                                                   sectionItems = group.ToList()
                                                                               }).ToList();
                            foreach (var section in dependentSectionContext)
                            {
                                var sectionDto = new SectionDto
                                {
                                    SectionId = section.Key,
                                    SectionName = section.SectionName
                                };
                                foreach (var question in section.sectionItems)
                                {
                                    var submittedQuestionViewDto = new SubmittedFormQuestionViewDto
                                    {
                                        QuestionId = question.QuestionId,
                                        QuestionTypeId = question.QuestionTypeId,
                                        Question = question.Query,
                                        QuestionTypeName = question.QuestionTypeName
                                    };
                                    if (question.Answers.Any())
                                    {
                                        foreach (var answer in question.Answers.ToList())
                                        {
                                            if (answer.IsYes != null)
                                            {
                                                submittedQuestionViewDto.YesNo = new SubmittedYesNoAnswerViewDto
                                                {
                                                    IsYes = Convert.ToBoolean(answer.IsYes)
                                                };
                                                break;
                                            }
                                            else if (!string.IsNullOrEmpty(answer.TextAnswer))
                                            {
                                                submittedQuestionViewDto.TextAnswer = new SubmittedTextAnswerViewDto
                                                {
                                                    TextAnswer = answer.TextAnswer
                                                };
                                                break;
                                            }
                                            else if (!string.IsNullOrEmpty(answer.AttachmentFileName))
                                            {
                                                var submittedAttachmentDto = new SubmittedAttachmentViewDto
                                                {
                                                    FileName = answer.AttachmentFileName,
                                                    MediaTypeId = answer.MediaTypeId ?? 0
                                                };
                                                submittedQuestionViewDto.Attachments.Add(submittedAttachmentDto);
                                            }
                                            else
                                            {
                                                var inspectionMultiAnswerViewDto = new SubmittedFormMultiAnswerViewDto
                                                {
                                                    AnswerOptionId = answer.AnswerOptionId ?? 0,
                                                    Option = answer.Option,
                                                    IsSelected = answer.IsSelected,
                                                };
                                                submittedQuestionViewDto.AnswerOptions.Add(inspectionMultiAnswerViewDto);
                                            }
                                        }
                                    }
                                    sectionDto.Questions.Add(submittedQuestionViewDto);
                                }
                                dependentForm.Sections.Add(sectionDto);
                            }
                            submittedFormViewDto.DependentForms.Add(dependentForm);
                        }
                    }
                }
                //demodetail
                var scheduleDemoContext = _emamiContext.ScheduleDemoUsers.AsNoTracking().Where(_ => _.SubmittedFormId == formIdDto.SubmittedFormId &&
                                                                                                     _.IsActive).OrderByDescending(_ => _.CreatedDate)
                                                                                                     .GroupBy(_ => _.SubmittedFormId)
                                                                                                     .Select(group => new
                                                                                                     {
                                                                                                         subFormId = group.Key,
                                                                                                         demodetails = group.ToList()
                                                                                                     }).ToList();

                foreach (var demo in scheduleDemoContext)
                {
                    foreach (var demoDetail in demo.demodetails)
                    {
                        var submittedUnderstandingForms = _emamiContext.SubmittedForms.AsNoTracking().Where(_ => _.DemoId == demoDetail.Id)
                                                                                                     .Select(_ => _.Id).ToList();
                        var scheduleDemoEalUserIds = _emamiContext.ScheduleDemoUserMappings.AsNoTracking().Where(_ => _.DemoId == demoDetail.Id).Select(_ => _.EALUserId).ToList();
                        var demoDto = new ScheduleDemoOutputDto
                        {
                            DemoId = demoDetail.Id,
                            DemoCreatedBy = GetUserName(demoDetail.CreatedBy),
                            DemoDateTime = demoDetail.DemoDate,
                            DemonstratorName = demoDetail.DemoUser != null ? demoDetail.DemoUser.Name : string.Empty,
                            DemoInchargeName = GetUserName(demoDetail.DemoInchargeId),
                            SalesExecutiveName = GetUserName(submittedFormContext.CreatedBy),
                            IsActive = demoDetail.IsActive,
                            SubmittedUnderstandingForms = submittedUnderstandingForms,
                            ComplaintFormId = demoDetail.SubmittedFormId,
                            ComplaintFormName = demoDetail.SubmittedForm.FormName,
                            EALUserId = scheduleDemoEalUserIds,
                            UnderstandingFormId = demoDetail.DependentMasterFormId,
                        };
                        submittedFormViewDto.DemoDetails.Add(demoDto);
                    }
                }

                if (submittedFormContext.SubmittedFormQuestions.Any())
                {
                    var groupedSectionContext = submittedFormContext.SubmittedFormQuestions.GroupBy(_ => _.SectionId)
                                                                                           .Select(group => new
                                                                                           {
                                                                                               group.Key,
                                                                                               group.FirstOrDefault().SectionName,
                                                                                               sectionItems = group.ToList()
                                                                                           }).ToList();
                    foreach (var section in groupedSectionContext)
                    {
                        var sectionDto = new SectionDto
                        {
                            SectionId = section.Key,
                            SectionName = section.SectionName
                        };
                        foreach (var question in section.sectionItems)
                        {
                            var submittedQuestionViewDto = new SubmittedFormQuestionViewDto
                            {
                                Question = question.Query,
                                QuestionTypeName = question.QuestionTypeName,
                                QuestionTypeId = question.QuestionTypeId,
                                QuestionId = question.QuestionId,
                                SubmittedFormQuestionId = question.Id
                            };
                            if (question.Answers.Any())
                            {
                                foreach (var answer in question.Answers.ToList())
                                {
                                    if (answer.IsYes != null)
                                    {
                                        submittedQuestionViewDto.YesNo = new SubmittedYesNoAnswerViewDto
                                        {
                                            IsYes = Convert.ToBoolean(answer.IsYes)
                                        };
                                        submittedQuestionViewDto.Answer = answer.IsYes ?? false ? Constants.Yes : Constants.No;
                                        break;
                                    }
                                    else if (!string.IsNullOrEmpty(answer.TextAnswer))
                                    {
                                        submittedQuestionViewDto.TextAnswer = new SubmittedTextAnswerViewDto
                                        {
                                            TextAnswer = answer.TextAnswer
                                        };
                                        submittedQuestionViewDto.Answer = answer.TextAnswer;
                                        break;
                                    }
                                    else if (!string.IsNullOrEmpty(answer.AttachmentFileName))
                                    {
                                        var submittedAttachmentDto = new SubmittedAttachmentViewDto
                                        {
                                            FileName = answer.AttachmentFileName,
                                            MediaTypeId = answer.MediaTypeId ?? 0
                                        };
                                        if (string.IsNullOrEmpty(submittedQuestionViewDto.Answer))
                                            submittedQuestionViewDto.Answer = answer.AttachmentFileName + " - " + answer.MediaTypeId;
                                        else
                                            submittedQuestionViewDto.Answer += "," + answer.AttachmentFileName + " - " + answer.MediaTypeId;
                                        submittedQuestionViewDto.Attachments.Add(submittedAttachmentDto);
                                    }
                                    else
                                    {
                                        var submittedMultiAnswerViewDto = new SubmittedFormMultiAnswerViewDto
                                        {
                                            AnswerOptionId = answer.AnswerOptionId ?? 0,
                                            Option = answer.Option,
                                            IsSelected = answer.IsSelected,
                                        };
                                        if (answer.IsSelected ?? false)
                                        {
                                            if (string.IsNullOrEmpty(submittedQuestionViewDto.Answer))
                                                submittedQuestionViewDto.Answer = answer.Option;
                                            else
                                                submittedQuestionViewDto.Answer += " - " + answer.Option;
                                        }
                                        submittedQuestionViewDto.AnswerOptions.Add(submittedMultiAnswerViewDto);
                                    }
                                }
                            }
                            sectionDto.Questions.Add(submittedQuestionViewDto);
                        }
                        submittedFormViewDto.Sections.Add(sectionDto);
                    }
                }
                return _resultService.SuccessObject(submittedFormViewDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }
        public ResultDto ViewSubmittedFormsListByDateRange(SubmittedFormsInputDto submittedFormsInputDto)
        {
            _methodName = "ViewSubmittedFormsListByDateRange";
            var submittedFormListDto = new List<SubmittedFormShortViewDto>();
            try
            {
                if (submittedFormsInputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (submittedFormsInputDto.FromDate == null || submittedFormsInputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (submittedFormsInputDto.ToDate == null || submittedFormsInputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }
                if (submittedFormsInputDto.FromDate > submittedFormsInputDto.ToDate)
                {
                    return _resultService.ErrorMessage(Constants.FromDateInvalid);
                }
                if (submittedFormsInputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                if (!_resultService.UserIsAcive(submittedFormsInputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                var fromDate = submittedFormsInputDto.FromDate.Date.AddMilliseconds(1);
                var toDate = submittedFormsInputDto.ToDate.Date.AddDays(1).AddSeconds(-1);
                var submittedFormContext = _emamiContext.SubmittedForms.AsNoTracking().Where(_ => _.CreatedDate >= fromDate && _.CreatedDate <= toDate).OrderByDescending(_ => _.CreatedDate).ToList();
                if (submittedFormContext.Any())
                {
                    foreach (var submittedForm in submittedFormContext)
                    {
                        var submittedViewDto = new SubmittedFormShortViewDto
                        {
                            SubmittedFormId = submittedForm.Id,
                            CreatedDate = submittedForm.CreatedDate,
                            FormId = submittedForm.FormId,
                            FormName = submittedForm.FormName,
                            //DemonstratedBy = GetDemonstratorName(submittedForm.DemoId ?? 0),
                            //DemoInchargeName = GetDemoInchargeName(submittedForm.DemoId ?? 0),
                            ////RaisedFor = submittedForm.Retailer != null ? submittedForm.Retailer.AccountName : submittedForm.CustomerName,
                            //DealerName = submittedForm.DealerName,
                            //RaisedBy = GetUserName(submittedForm.CreatedBy),
                            //ParentFormName = submittedForm.ParentFormId != null && submittedForm.ParentFormId != 0 ? GetSubmittedFormName(submittedForm.ParentFormId ?? 0) : string.Empty,
                            //FormApprovalStatusName = (submittedForm.FormApprovalStatusId ?? 0) > 0 ? Utility.GetEnumDescription((DTO.Enums.Status)submittedForm.FormApprovalStatusId) : string.Empty,
                            //FormStatus = submittedForm.FormStatus != null ? submittedForm.FormStatus.Name : string.Empty,
                            //Remarks = submittedForm.Remarks ?? submittedForm.Remarks,
                            //EALUserName = GetEALUserName(submittedForm.DemoId ?? 0)
                        };
                        //Add Submitted form details
                        var submittedformdetails = _emamiContext.SubmittedFormDetails.AsNoTracking().FirstOrDefault(_ => _.SubmittedFormId == submittedForm.Id);
                        if (submittedformdetails != null)
                        {
                            submittedViewDto.SkuName = submittedformdetails.Sku != null ? submittedformdetails.Sku.SkuName : string.Empty;
                            submittedViewDto.PlantName = GetPlantName(submittedformdetails.PlantId);
                            submittedViewDto.StateName = submittedformdetails.State != null ? submittedformdetails.State.StateName : string.Empty;
                            submittedViewDto.CityName = submittedformdetails.City != null ? submittedformdetails.City.CityName : string.Empty;
                        }
                        submittedFormListDto.Add(submittedViewDto);
                    }
                }
                return _resultService.SuccessObject(submittedFormListDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto ExportSubmittedForm(DynamicFormReportFilterInputDto inputDto)
        {
            _methodName = "ExportSubmittedForm";
            var resultDto = new ResultDto();

            try
            {
                var submittedFormListDto = new List<FormInputDto>();


                var roleIds = inputDto.roleIds?.ToList() ?? new List<long>();
                if (roleIds.Any() && roleIds.Contains(0))
                {
                    roleIds.Remove(0);
                }
                var query = _emamiContext.SubmittedForms.AsNoTracking().AsQueryable();

                // Apply date filter
                if (inputDto.FromDate != DateTime.MinValue && inputDto.ToDate != DateTime.MinValue)
                {
                    query = query.Where(c => DbFunctions.TruncateTime(c.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                                            && DbFunctions.TruncateTime(c.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate));
                }

                // Apply role filter
                if (roleIds.Any())
                {
                    query = query.Where(c => _emamiContext.UserRoles
                        .Where(ur => ur.UserId == c.UserId && roleIds.Contains(ur.RoleId))
                        .Any());
                }

                // Fetch the submitted forms data
                var submittedForms = query
                    .Select(c => new
                    {
                        c.Id,
                        c.FormName,
                        UserId = (long)c.UserId,
                        c.CustomerName,
                        c.CreatedDate,
                        UserRoleName = _emamiContext.UserRoles
                            .Where(ur => ur.UserId == c.UserId || roleIds.Contains(ur.RoleId))
                            .Join(_emamiContext.Roles,
                                  ur => ur.RoleId,
                                  r => r.Id,
                                  (ur, r) => r.Name)
                            .FirstOrDefault()
                    })
                    .ToList();

                if (submittedForms.Any())
                {
                    // Fetch questions and answers
                    var questionAnswers = _emamiContext.SubmittedFormQuestions
                        .GroupBy(q => q.SubmittedFormId)
                        .ToDictionary(g => g.Key, g => g.Select(q => new QuestionAnswerInput
                        {
                            QuestionId = q.QuestionId,
                            Query = q.Query,
                            QuestionTypeName = q.QuestionTypeName,
                            Answer = q.Answer
                        }).ToList());

                    // Combine parent and child data
                    submittedFormListDto = submittedForms
                        .Select(dto => new FormInputDto
                        {
                            Id = dto.Id,
                            FormName = dto.FormName,
                            UserId = dto.UserId,
                            CustomerName = dto.CustomerName,
                            CreatedDate = dto.CreatedDate,
                            UserRoleType = dto.UserRoleName,
                            QuestionAnswer = questionAnswers.ContainsKey(dto.Id) ? questionAnswers[dto.Id] : new List<QuestionAnswerInput>()
                        })
                        .ToList();

                    // Return data in a simple format
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = submittedFormListDto;
                }
                else
                {
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = new { ErrorMessage = "No records found" };
                }
            }
            catch (Exception ex)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage("Data retrieval failed");
            }

            return resultDto;
        }



        //    #endregion        

        //    #region Demo Scheduling

        //    /// <summary>
        //    /// Get open status complaints list  
        //    /// </summary>
        //    /// <returns></returns>        
        //    public ResultDto GetOpenComplaintFormsList()
        //    {
        //        _methodName = "GetOpenComplaintFormsList";
        //        var formDto = new List<FormDto>();
        //        try
        //        {
        //            formDto = _emamiContext.SubmittedForms.AsNoTracking().Where(_ => (_.ParentFormId == 0 || _.ParentFormId == null) && _.IsFormStatus &&
        //                                                                (_.FormStatusId != (int)DTO.Enums.DynamicFormStatus.Resolved) && _.FormApprovalStatusId == (int)DTO.Enums.Status.Approved)
        //                                                                .OrderByDescending(_ => _.CreatedDate)
        //                                                                .ToList()
        //                                                                .Select(c => new FormDto
        //                                                                {
        //                                                                    FormId = c.Id,
        //                                                                    FormName = c.Id + " - " + c.FormName + " - " + GetUserName(c.CreatedBy),
        //                                                                }).ToList();
        //            return _resultService.SuccessObject(formDto);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }

        //    /// <summary>
        //    /// Get  understandingFormList based on complaint  
        //    /// </summary>
        //    /// <returns></returns>
        //    public ResultDto GetUnderstandingFormsBasedOnComplaintForm(IdInputDto inputDto)
        //    {
        //        _methodName = "GetUnderstandingFormsBasedOnComplaintForm";
        //        var resultDto = new ResultDto();
        //        var understandingFormList = new List<DropDownDto>();
        //        try
        //        {
        //            var masterFormId = _emamiContext.SubmittedForms.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.Id).FormId;
        //            understandingFormList = _emamiContext.Forms.AsNoTracking().Where(_ => (_.ParentFormId == masterFormId) && _.IsActive)
        //                                                                .Select(c => new DropDownDto
        //                                                                {
        //                                                                    Id = c.Id,
        //                                                                    Name = c.Id + " - " + c.Name,
        //                                                                }).ToList();

        //            resultDto.SuccessDto.Response = understandingFormList;
        //            resultDto.IsSuccess = true;
        //        }
        //        catch (Exception exception)
        //        {
        //            var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.Message = Constants.Exception;
        //            _logger.Error(message);
        //        }
        //        return resultDto;
        //    }
        //    /// <summary>
        //    /// To get available users list based on role id - dynamic user list
        //    /// </summary>
        //    /// <param name="scheduleDemoInputDto"></param>
        //    /// <returns></returns>
        //    public ResultDto GetAvailableUserListForDemo(ScheduleDemoInputDto scheduleDemoInputDto)
        //{
        //        _methodName = "GetAvailableUserListForDemo";
        //        var demoUserListDto = new List<DemoUserDto>();
        //        try
        //        {
        //            if (scheduleDemoInputDto == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            if (scheduleDemoInputDto.DemoDateTime == null || scheduleDemoInputDto.DemoDateTime == DateTime.MinValue)
        //            {
        //                return _resultService.ErrorMessage(Constants.DemoDateEmpty);
        //            }
        //            var demoUserId = new List<long>();
        //            var salesExecutiveUsersContext = new List<long>();

        //            if (!scheduleDemoInputDto.IsEALUser)
        //            {
        //                salesExecutiveUsersContext = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == scheduleDemoInputDto.DemoUserRoleId && _.User.IsActive).Select(_ => _.UserId).ToList();
        //                if (salesExecutiveUsersContext != null && salesExecutiveUsersContext.Count > 0)
        //                {
        //                    var scheduleDemoContextList = _emamiContext.ScheduleDemoUsers.AsNoTracking().Where(_ =>
        //                                                    ((scheduleDemoInputDto.DemoUserRoleId == (int)DTO.Enums.Role.Demonstrator && salesExecutiveUsersContext.Contains(_.DemoUserId)) ||
        //                                                    (scheduleDemoInputDto.DemoUserRoleId == (int)DTO.Enums.Role.DemoInCharge && salesExecutiveUsersContext.Contains(_.DemoInchargeId))) &&
        //                                                    (DbFunctions.TruncateTime(_.DemoDate) == DbFunctions.TruncateTime(scheduleDemoInputDto.DemoDateTime)) &&
        //                                                    _.IsActive);
        //                    if (scheduleDemoInputDto.DemoUserRoleId == (int)DTO.Enums.Role.Demonstrator)
        //                    {
        //                        demoUserListDto = scheduleDemoContextList.Where(_ => _.Id == scheduleDemoInputDto.DemoId)
        //                                          .Select(_ => new DemoUserDto
        //                                          {
        //                                              UserId = _.DemoUserId,
        //                                              UserName = _.DemoUser != null ? _.DemoUser.Name : string.Empty
        //                                          }).Distinct().ToList();
        //                        demoUserId = scheduleDemoContextList.Select(s => s.DemoUserId).ToList();
        //                    }
        //                    else if (scheduleDemoInputDto.DemoUserRoleId == (int)DTO.Enums.Role.DemoInCharge)
        //                    {
        //                        demoUserListDto = scheduleDemoContextList
        //                                          .Join(_emamiContext.Users.AsNoTracking(), d => d.DemoInchargeId, u => u.Id, (d, u) => new { demo = d, user = u })
        //                                          .Where(_ => _.demo.Id == scheduleDemoInputDto.DemoId)
        //                                          .Select(_ => new DemoUserDto
        //                                          {
        //                                              UserId = _.user.Id,
        //                                              UserName = _.user.Name
        //                                          }).Distinct().ToList();
        //                        demoUserId = scheduleDemoContextList.Select(s => s.DemoInchargeId).ToList();
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                var rolesForEalUsers = _emamiContext.Roles.AsNoTracking().Where(_ => _.IsActive && _.Id != (int)DTO.Enums.Role.Admin && _.Id != (int)DTO.Enums.Role.Dealer && _.Id != (int)DTO.Enums.Role.Broker && _.Id != (int)DTO.Enums.Role.ShipToParty && _.Id != (int)DTO.Enums.Role.Demonstrator && _.Id != (int)DTO.Enums.Role.DemoInCharge).Select(a => a.Id).ToList();
        //                salesExecutiveUsersContext = _emamiContext.Users.AsNoTracking()
        //                                    .Join(_emamiContext.UserRoles.AsNoTracking(), a => a.Id, b => b.UserId, (a, b) => new { a, b })
        //                                    .Where(_ => _.a.IsActive && rolesForEalUsers.Contains(_.b.RoleId))
        //                                    .Select(_ => _.a.Id).Distinct().ToList();
        //                if (salesExecutiveUsersContext != null && salesExecutiveUsersContext.Any())
        //                {
        //                    var scheduleDemoContextList = new List<long>();

        //                    var scheduleDemoMappingContext = _emamiContext.ScheduleDemoUserMappings.AsNoTracking();
        //                    var scheduleDemoContextIdsList = _emamiContext.ScheduleDemoUsers.AsNoTracking().Where(_ => (DbFunctions.TruncateTime(_.DemoDate) == DbFunctions.TruncateTime(scheduleDemoInputDto.DemoDateTime)) && _.IsActive).Select(a => a.Id).ToList();
        //                    var scheduleDemoEalUserId = scheduleDemoMappingContext.AsNoTracking().Where(_ => scheduleDemoContextIdsList.Contains(_.DemoId)).Select(a => a.EALUserId).ToList();
        //                    if (scheduleDemoEalUserId.Count > 0)
        //                    {
        //                        foreach (var i in scheduleDemoEalUserId)
        //                        {
        //                            if (salesExecutiveUsersContext.Contains(i))
        //                                scheduleDemoContextList.Add(i);
        //                        }
        //                    }
        //                    var demoEALuser = scheduleDemoMappingContext.Where(_ => _.DemoId == scheduleDemoInputDto.DemoId).Select(a => a.EALUserId).ToList();

        //                    if (demoEALuser.Count > 0)
        //                    {
        //                        demoUserListDto = _emamiContext.Users.AsNoTracking().Where(_ => demoEALuser.Contains(_.Id))
        //                                 .Select(_ => new DemoUserDto
        //                                 {
        //                                     UserId = _.Id,
        //                                     UserName = _.Name
        //                                 }).Distinct().ToList();

        //                    }
        //                    demoUserId.AddRange(scheduleDemoContextList);
        //                }
        //            }

        //            var availableUser = _emamiContext.Users.AsNoTracking().Where(_ => salesExecutiveUsersContext.Contains(_.Id) && !demoUserId.Contains(_.Id) && _.IsActive)
        //                                    .Select(_ => new DemoUserDto
        //                                    {
        //                                        UserId = _.Id,
        //                                        UserName = _.Name
        //                                    }).ToList();
        //            demoUserListDto.AddRange(availableUser);
        //            var distinctDemoUserList = demoUserListDto.Select(user => new { user.UserId , user.UserName}).Distinct().ToList();

        //            return _resultService.SuccessObject(distinctDemoUserList);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }
        //    /// <summary>
        //    /// Schedule demo based on user's availability
        //    /// </summary>
        //    /// <param name="complaintFormAddDto"></param>
        //    /// <returns></returns>
        //    public ResultDto ScheduleDemo(ScheduleDemoInputDto scheduleDemoInputDto)
        //    {
        //        _methodName = "ScheduleDemo";
        //        try
        //        {
        //            var submittedunderstandingForms = new List<long>();
        //            if (scheduleDemoInputDto == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            if (scheduleDemoInputDto.LoginUserId == 0)
        //            {
        //                return _resultService.ErrorMessage(Constants.UserIdMissing);
        //            }
        //            if (!_resultService.UserIsAcive(scheduleDemoInputDto.LoginUserId))
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidUser);
        //            }
        //            if (scheduleDemoInputDto.DemoDateTime == null || scheduleDemoInputDto.DemoDateTime == DateTime.MinValue)
        //            {
        //                return _resultService.ErrorMessage(Constants.DemoDateEmpty);
        //            }
        //            if (scheduleDemoInputDto.DemoDateTime.Date <= DateTime.Today.Date.AddDays(-1))
        //            {
        //                return _resultService.ErrorMessage(Constants.DemoDateCannotBePast);
        //            }
        //            var scheduleDemoContext = _emamiContext.ScheduleDemoUsers.AsNoTracking().Where(_ => _.SubmittedFormId == scheduleDemoInputDto.ComplaintFormId &&
        //                                                                                                 (_.SubmittedForm.ParentFormId == 0 || _.SubmittedForm.ParentFormId == null) &&

        //                                                                                                 DbFunctions.TruncateTime(_.DemoDate) >= DbFunctions.TruncateTime(DateTime.Today) && _.IsActive).Select(a => a.Id).ToList();
        //            if (scheduleDemoContext != null && scheduleDemoContext.Count > 0)
        //            {
        //                submittedunderstandingForms = _emamiContext.SubmittedForms.AsNoTracking().Where(_ => scheduleDemoContext.Contains(_.DemoId.Value)).Select(a => a.Id).ToList();

        //            }

        //            if (scheduleDemoContext.Count != submittedunderstandingForms.Count)
        //            {
        //                return _resultService.ErrorMessage(Constants.AlreadyActiveDemoPresent);
        //            }
        //            else
        //            {
        //                //Add sales executive id, demonstrator id
        //                var scheduleDemo = new ScheduleDemoUser
        //                {
        //                    DemoUserId = scheduleDemoInputDto.DemonstratorId,
        //                    DemoInchargeId = scheduleDemoInputDto.DemoInchargeId,
        //                    SubmittedFormId = scheduleDemoInputDto.ComplaintFormId,
        //                    DependentMasterFormId = scheduleDemoInputDto.UnderstandingFormId,
        //                    DemoDate = scheduleDemoInputDto.DemoDateTime,
        //                    IsActive = true,
        //                    CreatedBy = scheduleDemoInputDto.LoginUserId,
        //                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        //                    //EALUserId = string.Join(",", scheduleDemoInputDto.EALUserId)
        //                };
        //                _emamiContext.ScheduleDemoUsers.Add(scheduleDemo);

        //                ////Change form status to inprogress
        //                //var complaintFormContext = _emamiContext.SubmittedForms.FirstOrDefault(_ => _.Id == scheduleDemoInputDto.ComplaintFormId);
        //                //if (complaintFormContext != null && (complaintFormContext.FormStatusId == (int)DTO.Enums.DynamicFormStatus.Open))
        //                //{
        //                //    complaintFormContext.FormStatusId = (int)DTO.Enums.DynamicFormStatus.InProgress;
        //                //}
        //                _emamiContext.SaveChanges();

        //                if (scheduleDemoInputDto.EALUserId.Count > 0)
        //                {
        //                    //Mapping EalUserId against Demo 
        //                    foreach (var id in scheduleDemoInputDto.EALUserId)
        //                    {
        //                        var ScheduleDemoMapping = new ScheduleDemoUserMapping()
        //                        {
        //                            DemoId = scheduleDemo.Id,
        //                            EALUserId = id,
        //                            CreatedBy = scheduleDemoInputDto.LoginUserId,
        //                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
        //                        };
        //                        _emamiContext.ScheduleDemoUserMappings.Add(ScheduleDemoMapping);
        //                    }
        //                    _emamiContext.SaveChanges();
        //                }


        //                var complaintFormContext = _emamiContext.SubmittedForms.FirstOrDefault(_ => _.Id == scheduleDemoInputDto.ComplaintFormId);
        //                var UserContext = _emamiContext.Users.AsNoTracking().ToList();

        //                var Demonstrator = UserContext.FirstOrDefault(_ => _.Id == scheduleDemoInputDto.DemonstratorId && _.IsActive && !string.IsNullOrEmpty(_.PushTokenKey));
        //                var Demoincharge = UserContext.FirstOrDefault(_ => _.Id == scheduleDemoInputDto.DemoInchargeId && _.IsActive && !string.IsNullOrEmpty(_.PushTokenKey));
        //                var SalesExecutive = UserContext.FirstOrDefault(_ => _.Id == complaintFormContext.CreatedBy && _.IsActive && !string.IsNullOrEmpty(_.PushTokenKey));
        //                var EALUser = UserContext.Where(_ => scheduleDemoInputDto.EALUserId.Contains(_.Id) && _.IsActive && !string.IsNullOrEmpty(_.PushTokenKey));
        //                var NotificationContent = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(_ => _.Name.Equals(Constants.Demoscheduled));
        //                if (NotificationContent != null)
        //                {
        //                    var Formdetails = this.ViewSubmittedFormDetails(new SubmittedFormIdDto() { SubmittedFormId = scheduleDemoInputDto.ComplaintFormId });

        //                    if (Demonstrator != null)
        //                    {
        //                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                        {
        //                            PushTokenKey = Demonstrator.PushTokenKey,
        //                            RegistrationTypeId = Demonstrator.RegistrationTypeId != null ? (int)Demonstrator.RegistrationTypeId : 0,
        //                            Title = Constants.Demoscheduled,
        //                            Message = NotificationContent.PlainTemplate.Replace(Constants.Date, scheduleDemoInputDto.DemoDateTime.ToString("dd'/'MMM'/'yyyy"))
        //                                                                       .Replace(Constants.FormId, scheduleDemoInputDto.ComplaintFormId.ToString()),
        //                            IsCMSNotification = true,
        //                            SubmittedFormId = scheduleDemoInputDto.ComplaintFormId,
        //                            NotificationTypeId = (int)DTO.Enums.NotificationTypeForms.FormSubmitted
        //                            //NotificationObject = Formdetails != null && Formdetails.IsSuccess ? Formdetails.SuccessDto : null
        //                        };
        //                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                    }
        //                    if (Demoincharge != null)
        //                    {
        //                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                        {
        //                            PushTokenKey = Demoincharge.PushTokenKey,
        //                            RegistrationTypeId = Demoincharge.RegistrationTypeId != null ? (int)Demoincharge.RegistrationTypeId : 0,
        //                            Title = Constants.Demoscheduled,
        //                            Message = NotificationContent.PlainTemplate.Replace(Constants.Date, scheduleDemoInputDto.DemoDateTime.ToString("dd'/'MMM'/'yyyy"))
        //                                                                       .Replace(Constants.FormId, scheduleDemoInputDto.ComplaintFormId.ToString()),
        //                            IsCMSNotification = true,
        //                            SubmittedFormId = scheduleDemoInputDto.ComplaintFormId,
        //                            NotificationTypeId = (int)DTO.Enums.NotificationTypeForms.FormSubmitted,
        //                            //NotificationObject = Formdetails != null && Formdetails.IsSuccess ? Formdetails.SuccessDto : null
        //                        };
        //                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                    }
        //                    if (SalesExecutive != null)
        //                    {
        //                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                        {
        //                            PushTokenKey = SalesExecutive.PushTokenKey,
        //                            RegistrationTypeId = SalesExecutive.RegistrationTypeId != null ? (int)SalesExecutive.RegistrationTypeId : 0,
        //                            Title = Constants.Demoscheduled,
        //                            Message = NotificationContent.PlainTemplate.Replace(Constants.Date, scheduleDemoInputDto.DemoDateTime.ToString("dd'/'MMM'/'yyyy"))
        //                                                                       .Replace(Constants.FormId, scheduleDemoInputDto.ComplaintFormId.ToString()),
        //                            IsCMSNotification = true,
        //                            SubmittedFormId = scheduleDemoInputDto.ComplaintFormId,
        //                            NotificationTypeId = (int)DTO.Enums.NotificationTypeForms.FormSubmitted,
        //                            //NotificationObject = Formdetails != null && Formdetails.IsSuccess ? Formdetails.SuccessDto : null
        //                        };
        //                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                    }
        //                    if (EALUser != null)
        //                    {
        //                        foreach (var user in EALUser)
        //                        {
        //                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                            {
        //                                PushTokenKey = user.PushTokenKey,
        //                                RegistrationTypeId = user.RegistrationTypeId != null ? (int)user.RegistrationTypeId : 0,
        //                                Title = Constants.Demoscheduled,
        //                                Message = NotificationContent.PlainTemplate.Replace(Constants.Date, scheduleDemoInputDto.DemoDateTime.ToString("dd'/'MMM'/'yyyy"))
        //                                                                       .Replace(Constants.FormId, scheduleDemoInputDto.ComplaintFormId.ToString()),
        //                                IsCMSNotification = true,
        //                                SubmittedFormId = scheduleDemoInputDto.ComplaintFormId,
        //                                NotificationTypeId = (int)DTO.Enums.NotificationTypeForms.FormSubmitted,
        //                                //NotificationObject = Formdetails != null && Formdetails.IsSuccess ? Formdetails.SuccessDto : null
        //                            };
        //                            _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                        }
        //                    }
        //                }
        //            }
        //            return _resultService.SuccessMessage(Constants.DemoScheduledSuccessfully);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }
        //    /// <summary>
        //    /// Provision to cancel demo - inactive, update demo date
        //    /// </summary>
        //    /// <param name="scheduleDemoInputDto"></param>
        //    /// <returns></returns>
        //    public ResultDto UpdateDemoSchedule(ScheduleDemoInputDto scheduleDemoInputDto)
        //    {
        //        _methodName = "UpdateDemoSchedule";
        //        try
        //        {
        //            if (scheduleDemoInputDto == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            if (scheduleDemoInputDto.LoginUserId == 0)
        //            {
        //                return _resultService.ErrorMessage(Constants.UserIdMissing);
        //            }
        //            if (!_resultService.UserIsAcive(scheduleDemoInputDto.LoginUserId))
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidUser);
        //            }
        //            if (scheduleDemoInputDto.DemoDateTime == null || scheduleDemoInputDto.DemoDateTime == DateTime.MinValue)
        //            {
        //                return _resultService.ErrorMessage(Constants.DemoDateEmpty);
        //            }
        //            if (scheduleDemoInputDto.IsActive)
        //            {
        //                if (scheduleDemoInputDto.DemoDateTime.Date <= DateTime.Today.Date.AddDays(-1))
        //                {
        //                    return _resultService.ErrorMessage(Constants.DemoDateCannotBePast);
        //                }
        //            }

        //            var scheduleDemoContext = _emamiContext.ScheduleDemoUsers.FirstOrDefault(_ => _.Id == scheduleDemoInputDto.DemoId);
        //            var submittedForm = _emamiContext.SubmittedForms.AsNoTracking().FirstOrDefault(_ => _.DemoId == scheduleDemoInputDto.DemoId /*&& DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(DateTime.Now)*/);
        //            if (scheduleDemoContext == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.DemoDataEmpty);
        //            }
        //            else if (scheduleDemoContext.DemoDate < DateTime.Today.Date)
        //            {
        //                return _resultService.ErrorMessage(Constants.PastDemoCannotBeEdited);
        //            }
        //            else if (submittedForm != null)
        //            {
        //                return _resultService.ErrorMessage(Constants.DemoCompleted + submittedForm.CreatedDate.Date.ToShortDateString());
        //            }
        //            else
        //            {
        //                scheduleDemoContext.IsActive = scheduleDemoInputDto.IsActive;
        //                scheduleDemoContext.DemoUserId = scheduleDemoInputDto.DemonstratorId;
        //                scheduleDemoContext.DemoInchargeId = scheduleDemoInputDto.DemoInchargeId;
        //                //scheduleDemoContext.EALUserId = string.Join(",",scheduleDemoInputDto.EALUserId);
        //                scheduleDemoContext.DemoDate = scheduleDemoInputDto.DemoDateTime;
        //                scheduleDemoContext.SubmittedFormId = scheduleDemoInputDto.ComplaintFormId;
        //                scheduleDemoContext.DependentMasterFormId = scheduleDemoInputDto.UnderstandingFormId;
        //                scheduleDemoContext.ModifiedBy = scheduleDemoInputDto.LoginUserId;
        //                scheduleDemoContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

        //                ////Change form status to inprogress
        //                //var complaintFormContext = _emamiContext.SubmittedForms.FirstOrDefault(_ => _.Id == scheduleDemoInputDto.ComplaintFormId);
        //                //if (complaintFormContext != null && (complaintFormContext.FormStatusId == (int)DTO.Enums.DynamicFormStatus.Open ))
        //                //{
        //                //    complaintFormContext.FormStatusId = (int)DTO.Enums.DynamicFormStatus.InProgress;
        //                //}
        //                _emamiContext.SaveChanges();

        //                //Deleting  EalUserId is mapped against DemoId
        //                var ScheduledDemoUserMappingContext = _emamiContext.ScheduleDemoUserMappings.Where(_ => _.DemoId == scheduleDemoInputDto.DemoId).ToList();
        //                if (ScheduledDemoUserMappingContext.Count > 0)
        //                {
        //                    foreach (var data in ScheduledDemoUserMappingContext)
        //                    {
        //                        _emamiContext.ScheduleDemoUserMappings.Remove(data);
        //                    }
        //                    _emamiContext.SaveChanges();
        //                }

        //                if (scheduleDemoInputDto.EALUserId.Count > 0)
        //                {
        //                    //Mapping EalUserId against Demo 
        //                    foreach (var id in scheduleDemoInputDto.EALUserId)
        //                    {
        //                        var ScheduleDemoMapping = new ScheduleDemoUserMapping()
        //                        {
        //                            DemoId = scheduleDemoContext.Id,
        //                            EALUserId = id,
        //                            CreatedBy = scheduleDemoInputDto.LoginUserId,
        //                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
        //                        };
        //                        _emamiContext.ScheduleDemoUserMappings.Add(ScheduleDemoMapping);
        //                    }
        //                    _emamiContext.SaveChanges();
        //                }
        //            }
        //            var UserContext = _emamiContext.Users.AsNoTracking().ToList();

        //            var Demonstrator = UserContext.FirstOrDefault(_ => _.Id == scheduleDemoInputDto.DemonstratorId && _.IsActive && !string.IsNullOrEmpty(_.PushTokenKey));
        //            var Demoincharge = UserContext.FirstOrDefault(_ => _.Id == scheduleDemoInputDto.DemoInchargeId && _.IsActive && !string.IsNullOrEmpty(_.PushTokenKey));
        //            var SalesExecutive = UserContext.FirstOrDefault(_ => _.Id == scheduleDemoContext.SubmittedForm.CreatedBy && _.IsActive && !string.IsNullOrEmpty(_.PushTokenKey));
        //            var EALUser = UserContext.FirstOrDefault(_ => scheduleDemoInputDto.EALUserId.Contains(_.Id) && _.IsActive && !string.IsNullOrEmpty(_.PushTokenKey));

        //            var NotificationContent = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(_ => _.Name.Equals(Constants.DemoRescheduled));
        //            if (NotificationContent != null)
        //            {
        //                var Formdetails = this.ViewSubmittedFormDetails(new SubmittedFormIdDto() { SubmittedFormId = scheduleDemoInputDto.ComplaintFormId });

        //                if (Demonstrator != null)
        //                {
        //                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                    {
        //                        PushTokenKey = Demonstrator.PushTokenKey,
        //                        RegistrationTypeId = Demonstrator.RegistrationTypeId != null ? (int)Demonstrator.RegistrationTypeId : 0,
        //                        Title = Constants.DemoRescheduled,
        //                        Message = NotificationContent.PlainTemplate.Replace(Constants.Date, scheduleDemoInputDto.DemoDateTime.ToString("dd'/'MMM'/'yyyy"))
        //                                                                   .Replace(Constants.FormId, scheduleDemoInputDto.ComplaintFormId.ToString()),
        //                        IsCMSNotification = true,
        //                        SubmittedFormId = scheduleDemoInputDto.ComplaintFormId,
        //                        NotificationTypeId = (int)DTO.Enums.NotificationTypeForms.FormSubmitted,
        //                        //NotificationObject = Formdetails != null && Formdetails.IsSuccess ? Formdetails.SuccessDto : null
        //                    };
        //                    _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                }
        //                if (Demoincharge != null)
        //                {
        //                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                    {
        //                        PushTokenKey = Demoincharge.PushTokenKey,
        //                        RegistrationTypeId = Demoincharge.RegistrationTypeId != null ? (int)Demoincharge.RegistrationTypeId : 0,
        //                        Title = Constants.DemoRescheduled,
        //                        Message = NotificationContent.PlainTemplate.Replace(Constants.Date, scheduleDemoInputDto.DemoDateTime.ToString("dd'/'MMM'/'yyyy"))
        //                                                                   .Replace(Constants.FormId, scheduleDemoInputDto.ComplaintFormId.ToString()),
        //                        IsCMSNotification = true,
        //                        SubmittedFormId = scheduleDemoInputDto.ComplaintFormId,
        //                        NotificationTypeId = (int)DTO.Enums.NotificationTypeForms.FormSubmitted,
        //                        //NotificationObject = Formdetails != null && Formdetails.IsSuccess ? Formdetails.SuccessDto : null
        //                    };
        //                    _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                }
        //                if (SalesExecutive != null)
        //                {
        //                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                    {
        //                        PushTokenKey = SalesExecutive.PushTokenKey,
        //                        RegistrationTypeId = SalesExecutive.RegistrationTypeId != null ? (int)SalesExecutive.RegistrationTypeId : 0,
        //                        Title = Constants.DemoRescheduled,
        //                        Message = NotificationContent.PlainTemplate.Replace(Constants.Date, scheduleDemoInputDto.DemoDateTime.ToString("dd'/'MMM'/'yyyy"))
        //                                                                   .Replace(Constants.FormId, scheduleDemoInputDto.ComplaintFormId.ToString()),
        //                        IsCMSNotification = true,
        //                        SubmittedFormId = scheduleDemoInputDto.ComplaintFormId,
        //                        NotificationTypeId = (int)DTO.Enums.NotificationTypeForms.FormSubmitted,
        //                        //NotificationObject = Formdetails != null && Formdetails.IsSuccess ? Formdetails.SuccessDto : null
        //                    };
        //                    _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                }
        //                if (EALUser != null)
        //                {
        //                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                    {
        //                        PushTokenKey = EALUser.PushTokenKey,
        //                        RegistrationTypeId = EALUser.RegistrationTypeId != null ? (int)EALUser.RegistrationTypeId : 0,
        //                        Title = Constants.Demoscheduled,
        //                        Message = NotificationContent.PlainTemplate.Replace(Constants.Date, scheduleDemoInputDto.DemoDateTime.ToString("dd'/'MMM'/'yyyy"))
        //                                                                   .Replace(Constants.FormId, scheduleDemoInputDto.ComplaintFormId.ToString()),
        //                        IsCMSNotification = true,
        //                        SubmittedFormId = scheduleDemoInputDto.ComplaintFormId,
        //                        NotificationTypeId = (int)DTO.Enums.NotificationTypeForms.FormSubmitted,
        //                        //NotificationObject = Formdetails != null && Formdetails.IsSuccess ? Formdetails.SuccessDto : null
        //                    };
        //                    _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                }
        //            }
        //            return _resultService.SuccessMessage(Constants.DemoScheduleUpdatedSuccessfully);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }
        //    /// <summary>
        //    /// Get all scheduled demos list
        //    /// </summary>
        //    /// <returns></returns>
        //    public ResultDto GetAllScheduledDemos(DateFilterDto inputDto)
        //    {
        //        _methodName = "GetAllScheduledDemos";
        //        var scheduleDemoListDto = new List<ScheduleDemoOutputDto>();
        //        try
        //        {
        //            if (inputDto == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            var ScheduleDemoUserMappingContext = _emamiContext.ScheduleDemoUserMappings.AsNoTracking();
        //            if (inputDto.LoginUserId == 0)
        //            {
        //                scheduleDemoListDto = _emamiContext.ScheduleDemoUsers.AsNoTracking().OrderByDescending(_ => _.CreatedDate)
        //                                                                .ToList()
        //                                                                .Select(c => new ScheduleDemoOutputDto
        //                                                                {
        //                                                                    DemoId = c.Id,
        //                                                                    ComplaintFormName = c.SubmittedForm != null ? c.SubmittedFormId + " - " + c.SubmittedForm.FormName : string.Empty,
        //                                                                    UnderstandingFormName = GetFormName(c.DependentMasterFormId ?? 0),
        //                                                                    DemoDateTime = c.DemoDate,
        //                                                                    DemoCreatedBy = GetUserName(c.CreatedBy),
        //                                                                    DemonstratorName = c.DemoUser != null ? c.DemoUser.Name : string.Empty,
        //                                                                    SalesExecutiveName = c.SubmittedForm != null ? GetUserName(c.SubmittedForm.CreatedBy) : string.Empty,
        //                                                                    FormStatus = (c.SubmittedForm != null && c.SubmittedForm.FormStatus != null) ? c.SubmittedForm.FormStatus.Name : string.Empty,
        //                                                                    IsActive = c.IsActive,
        //                                                                    EALUserId = ScheduleDemoUserMappingContext.Where(_ => _.DemoId == c.Id) != null ? ScheduleDemoUserMappingContext.Where(_ => _.DemoId == c.Id).Select(a => a.EALUserId).ToList() : new List<long>(),
        //                                                                    EALUserName = ScheduleDemoUserMappingContext.Where(_ => _.DemoId == c.Id) != null ? GetEALUserName(c.Id) : string.Empty,
        //                                                                }).ToList();


        //            }
        //            else
        //            {
        //                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
        //                {
        //                    return _resultService.ErrorMessage(Constants.FromDateInvalid);
        //                }
        //                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
        //                {
        //                    return _resultService.ErrorMessage(Constants.ToDateInvalid);
        //                }
        //                scheduleDemoListDto = _emamiContext.ScheduleDemoUsers.AsNoTracking().Where(_ => _.CreatedBy == inputDto.LoginUserId && (DbFunctions.TruncateTime(_.DemoDate) >= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(_.DemoDate) <= DbFunctions.TruncateTime(inputDto.ToDate))).OrderByDescending(_ => _.CreatedDate)
        //                                                                .ToList()
        //                                                                .Select(c => new ScheduleDemoOutputDto
        //                                                                {
        //                                                                    DemoId = c.Id,
        //                                                                    ComplaintFormName = c.SubmittedForm != null ? c.SubmittedFormId + " - " + c.SubmittedForm.FormName : string.Empty,
        //                                                                    UnderstandingFormName = GetFormName(c.DependentMasterFormId ?? 0),
        //                                                                    DemoDateTime = c.DemoDate,
        //                                                                    DemoCreatedBy = GetUserName(c.CreatedBy),
        //                                                                    DemonstratorName = c.DemoUser != null ? c.DemoUser.Name : string.Empty,
        //                                                                    SalesExecutiveName = c.SubmittedForm != null ? GetUserName(c.SubmittedForm.CreatedBy) : string.Empty,
        //                                                                    FormStatus = (c.SubmittedForm != null && c.SubmittedForm.FormStatus != null) ? c.SubmittedForm.FormStatus.Name : string.Empty,
        //                                                                    IsActive = c.IsActive,
        //                                                                    EALUserId = ScheduleDemoUserMappingContext.Where(_ => _.DemoId == c.Id) != null ? ScheduleDemoUserMappingContext.Where(_ => _.DemoId == c.Id).Select(a => a.EALUserId).ToList() : new List<long>(),
        //                                                                    EALUserName = ScheduleDemoUserMappingContext.Where(_ => _.DemoId == c.Id) != null ? GetEALUserName(c.Id) : string.Empty,
        //                                                                    ComplaintFormId = c.SubmittedForm != null ? c.SubmittedFormId : 0,
        //                                                                    ComplaintRemarks = c.SubmittedForm != null ? c.SubmittedForm.Remarks : string.Empty,
        //                                                                    DemoInchargeName = GetDemoInchargeName(c.Id),
        //                                                                    DemoInchargeId = c.DemoInchargeId,
        //                                                                    DemonstratorId = c.DemoUserId,
        //                                                                    UnderstandingFormId = c.DependentMasterFormId ?? 0
        //                                                                }).ToList();
        //            }

        //            return _resultService.SuccessObject(scheduleDemoListDto);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }
        //    /// <summary>
        //    /// Get Scheduled Demo Details by demo id
        //    /// </summary>
        //    /// <param name="scheduleDemoInputDto"></param>
        //    /// <returns></returns>
        //    public ResultDto GetScheduledDemoDetails(ScheduleDemoInputDto scheduleDemoInputDto)
        //    {
        //        _methodName = "GetScheduledDemoDetails";
        //        var scheduleDetails = new ScheduleDemoOutputDto();
        //        try
        //        {
        //            if (scheduleDemoInputDto == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            if (scheduleDemoInputDto.DemoId == 0)
        //            {
        //                return _resultService.ErrorMessage(Constants.DemoIdMissing);
        //            }
        //            var scheduleDemoContext = _emamiContext.ScheduleDemoUsers.FirstOrDefault(_ => _.Id == scheduleDemoInputDto.DemoId);
        //            var scheduleDemoMapping = _emamiContext.ScheduleDemoUserMappings.Where(_ => _.DemoId == scheduleDemoInputDto.DemoId);
        //            if (scheduleDemoContext == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.DemoDataEmpty);
        //            }
        //            else
        //            {
        //                scheduleDetails.DemoId = scheduleDemoContext.Id;
        //                scheduleDetails.ComplaintFormId = scheduleDemoContext.SubmittedFormId;
        //                scheduleDetails.ComplaintFormName = scheduleDemoContext.SubmittedForm != null ? scheduleDemoContext.SubmittedForm.FormName : string.Empty;
        //                scheduleDetails.UnderstandingFormId = scheduleDemoContext.DependentMasterFormId;
        //                scheduleDetails.UnderstandingFormName = GetFormName(scheduleDemoContext.DependentMasterFormId ?? 0);
        //                scheduleDetails.DemoCreatedBy = GetUserName(scheduleDemoContext.CreatedBy);
        //                scheduleDetails.DemoDateTime = scheduleDemoContext.DemoDate;
        //                scheduleDetails.DemonstratorId = scheduleDemoContext.DemoUserId;
        //                scheduleDetails.DemoInchargeId = scheduleDemoContext.DemoInchargeId;
        //                scheduleDetails.DemonstratorName = scheduleDemoContext.DemoUser != null ? scheduleDemoContext.DemoUser.Name : string.Empty;
        //                scheduleDetails.FormStatus = (scheduleDemoContext.SubmittedForm != null && scheduleDemoContext.SubmittedForm.FormStatus != null) ? scheduleDemoContext.SubmittedForm.FormStatus.Name : string.Empty;
        //                scheduleDetails.SalesExecutiveName = scheduleDemoContext.SubmittedForm != null ? GetUserName(scheduleDemoContext.SubmittedForm.CreatedBy) : string.Empty;
        //                scheduleDetails.IsActive = scheduleDemoContext.IsActive;
        //                scheduleDetails.EALUserId = scheduleDemoMapping != null ? scheduleDemoMapping.Select(a => a.EALUserId).ToList() : new List<long>();
        //                scheduleDetails.EALUserName = scheduleDemoMapping != null ? GetEALUserName(scheduleDemoContext.Id) : string.Empty;

        //            }
        //            return _resultService.SuccessObject(scheduleDetails);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }

        //    #endregion

        //    #region Complaint Approval

        //    public ResultDto GetComplaintApprovalList(DateFilterDto inputDto)
        //    {
        //        _methodName = "GetComplaintApprovalList";
        //        try
        //        {
        //            if (inputDto == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            if (inputDto.LoginUserId == 0)
        //            {
        //                return _resultService.ErrorMessage(Constants.UserIdMissing);
        //            }
        //            if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
        //            {
        //                return _resultService.ErrorMessage(Constants.FromDateInvalid);
        //            }
        //            if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
        //            {
        //                return _resultService.ErrorMessage(Constants.ToDateInvalid);
        //            }

        //            var complaints = _emamiContext.SubmittedForms.AsNoTracking().Join
        //                            (_emamiContext.Users.AsNoTracking(),
        //                                sf => sf.CreatedBy,
        //                                u => u.Id, (sf, u) => new { forms = sf, user = u })
        //                            .Where(_ => DbFunctions.TruncateTime(_.forms.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
        //                                        DbFunctions.TruncateTime(_.forms.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate) &&
        //                                        (_.forms.ParentFormId == 0 || _.forms.ParentFormId == null) &&
        //                                        _.forms.FormApprovalStatusId == (int)DTO.Enums.Status.Pending)
        //                            .ToList();

        //            var result = complaints.Select(_ => new ComplaintApprovalDto()
        //            {
        //                ApprovalStatus = (_.forms.FormApprovalStatusId ?? 0) > 0 ? Utility.GetEnumDescription((DTO.Enums.Status)_.forms.FormApprovalStatusId) : string.Empty,
        //                ApprovalStatusId = Convert.ToInt32(_.forms.FormApprovalStatusId ?? 0),
        //                ComplaintId = _.forms.Id,
        //                CreatedOn = _.forms.CreatedDate,
        //                CustomerName = _.forms.Retailer != null ? _.forms.Retailer.AccountName : _.forms.CustomerName,
        //                FormName = _.forms.Form.Name,
        //                //SalesExecutiveName = _emamiContext.Users.AsNoTracking().FirstOrDefault(d => d.Id == _.CreatedBy)?.Name
        //                SalesExecutiveName = _.user.Name,
        //                DealerName = _.forms.DealerName
        //            }).ToList();

        //            return _resultService.SuccessObject(result);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }
        //    public ResultDto UpdateComplaintApproval(ComplaintApprovalListInputDto inputDto)
        //    {
        //        _methodName = "UpdateComplaintApproval";
        //        try
        //        {
        //            if (inputDto == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            if (inputDto.LoginUserId == 0)
        //            {
        //                return _resultService.ErrorMessage(Constants.UserIdMissing);
        //            }
        //            if (inputDto.approvallist == null || !inputDto.approvallist.Any())
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }

        //            var UserContext = _emamiContext.Users.AsNoTracking().ToList();

        //            var LoginUser = UserContext.FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
        //            if (LoginUser == null || !LoginUser.IsActive)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidUser);
        //            }

        //            List<PushNotificationInputDto> notificationList = new List<PushNotificationInputDto>();
        //            var NotificationContent = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(_ => _.Name.Equals(Constants.ComplaintApprovalStatus));
        //            foreach (var item in inputDto.approvallist)
        //            {
        //                var Complaint = _emamiContext.SubmittedForms.FirstOrDefault(_ => _.Id == item.ComplaintId);
        //                if (Complaint != null && !Complaint.FormApprovalStatusId.Equals(item.StatusId))
        //                {
        //                    Complaint.FormApprovalStatusId = item.StatusId;
        //                    Complaint.ModifiedBy = inputDto.LoginUserId;
        //                    Complaint.ModifiedDate = DateTime.Now;

        //                    var SalesExective = UserContext.FirstOrDefault(_ => _.Id == Complaint.CreatedBy);
        //                    if (SalesExective != null && !string.IsNullOrEmpty(SalesExective.PushTokenKey) && NotificationContent != null && item.StatusId != (int)DTO.Enums.Status.Pending)
        //                    {
        //                        var Formdetails = this.ViewSubmittedFormDetails(new SubmittedFormIdDto() { SubmittedFormId = Complaint.Id });
        //                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                        {
        //                            PushTokenKey = SalesExective.PushTokenKey,
        //                            RegistrationTypeId = SalesExective.RegistrationTypeId != null ? (int)SalesExective.RegistrationTypeId : 0,
        //                            Title = Constants.ComplaintApprovalStatus,
        //                            Message = NotificationContent.PlainTemplate.Replace(Constants.FormId, Complaint.Id.ToString())
        //                                                                       .Replace(Constants.Status, UtilityHelper.GetEnumDescription((DTO.Enums.Status)item.StatusId))
        //                                                                       .Replace(Constants.UserName, LoginUser.Name),
        //                            IsCMSNotification = true,
        //                            SubmittedFormId = Complaint.Id,
        //                            NotificationTypeId = (int)DTO.Enums.NotificationTypeForms.FormSubmitted
        //                            //NotificationObject = Formdetails != null && Formdetails.IsSuccess ? Formdetails.SuccessDto : null
        //                        };
        //                        notificationList.Add(pushNotificationInputDto);
        //                    }
        //                }
        //                if (!string.IsNullOrEmpty(item.Remarks))
        //                {
        //                    var Remarks = new SubmittedFormRemarks()
        //                    {
        //                        CreatedBy = inputDto.LoginUserId,
        //                        CreatedDate = DateTime.Now,
        //                        Description = item.Remarks,
        //                        IsActive = true,
        //                        RemarkType = (int)DTO.Enums.FormRemarksType.FormApprovalStatusRemark,
        //                        SubmittedFormId = Complaint.Id
        //                    };
        //                    _emamiContext.SubmittedFormRemarks.Add(Remarks);
        //                }

        //            }
        //            _emamiContext.SaveChanges();

        //            foreach (var singleNotification in notificationList)
        //            {
        //                _notificationService.SendPushNotificationThroughFirebase(singleNotification);
        //            }

        //            return _resultService.SuccessObject("Success");
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }
        //    public ResultDto GetComplaintFormStatusList(DateFilterDto inputDto)
        //    {
        //        _methodName = "GetComplaintFormStatusList";
        //        try
        //        {
        //            if (inputDto == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            if (inputDto.LoginUserId == 0)
        //            {
        //                return _resultService.ErrorMessage(Constants.UserIdMissing);
        //            }
        //            if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
        //            {
        //                return _resultService.ErrorMessage(Constants.FromDateInvalid);
        //            }
        //            if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
        //            {
        //                return _resultService.ErrorMessage(Constants.ToDateInvalid);
        //            }

        //            var complaints = _emamiContext.SubmittedForms.AsNoTracking()
        //                .Join(_emamiContext.Users.AsNoTracking(), sf => sf.CreatedBy, u => u.Id, (sf, u) => new { forms = sf, user = u })
        //                .Where(_ => DbFunctions.TruncateTime(_.forms.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(_.forms.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate) &&
        //                (_.forms.ParentFormId == 0 || _.forms.ParentFormId == null) && _.forms.FormApprovalStatusId == (int)DTO.Enums.Status.Approved && _.forms.IsFormStatus && _.forms.FormStatusId != null).ToList();

        //            var result = complaints.Select(_ => new ComplaintStatusDto()
        //            {
        //                ComplaintId = _.forms.Id,
        //                CreatedOn = _.forms.CreatedDate,
        //                CustomerName = _.forms.Retailer != null ? _.forms.Retailer.AccountName : _.forms.CustomerName,
        //                FormName = _.forms.Form.Name,
        //                //SubmitedBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(d => d.Id == _.forms.CreatedBy)?.Name,
        //                SubmitedBy = _.user.Name,
        //                Status = (_.forms.FormStatusId ?? 0) > 0 ? Utility.GetEnumDescription((DTO.Enums.DynamicFormStatus)_.forms.FormStatusId) : string.Empty,
        //                StatusId = Convert.ToInt32(_.forms.FormStatusId ?? 0),
        //                DealerName = _.forms.DealerName
        //            }).ToList();

        //            return _resultService.SuccessObject(result);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }
        //    public ResultDto UpdateComplaintFormStatus(ComplaintApprovalListInputDto inputDto)
        //    {
        //        _methodName = "UpdateComplaintFormStatus";
        //        try
        //        {
        //            if (inputDto == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            if (inputDto.LoginUserId == 0)
        //            {
        //                return _resultService.ErrorMessage(Constants.UserIdMissing);
        //            }
        //            if (inputDto.approvallist == null || !inputDto.approvallist.Any())
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            var UserContext = _emamiContext.Users.AsNoTracking().ToList();

        //            var LoginUser = UserContext.FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
        //            if (LoginUser == null || !LoginUser.IsActive)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidUser);
        //            }

        //            List<ComplaintApprovalInputDto> NotifyUser = new List<ComplaintApprovalInputDto>();

        //            foreach (var item in inputDto.approvallist)
        //            {
        //                var Complaint = _emamiContext.SubmittedForms.FirstOrDefault(_ => _.Id == item.ComplaintId);
        //                if (Complaint != null && !Complaint.FormStatusId.Equals(item.StatusId))
        //                {
        //                    Complaint.FormStatusId = item.StatusId;
        //                    Complaint.ModifiedBy = inputDto.LoginUserId;
        //                    Complaint.ModifiedDate = DateTime.Now;
        //                    NotifyUser.Add(new ComplaintApprovalInputDto() { ComplaintId = Complaint.Id, LoginUserId = Complaint.CreatedBy, StatusId = item.StatusId });//adding SalesExecutive Id as Login user Id                   
        //                }
        //                if (!string.IsNullOrEmpty(item.Remarks))
        //                {
        //                    var Remarks = new SubmittedFormRemarks()
        //                    {
        //                        CreatedBy = inputDto.LoginUserId,
        //                        CreatedDate = DateTime.Now,
        //                        Description = item.Remarks,
        //                        IsActive = true,
        //                        RemarkType = (int)DTO.Enums.FormRemarksType.FormStatusRemark,
        //                        SubmittedFormId = Complaint.Id
        //                    };
        //                    _emamiContext.SubmittedFormRemarks.Add(Remarks);
        //                }
        //            }
        //            _emamiContext.SaveChanges();

        //            var DemoList = _emamiContext.ScheduleDemoUsers.AsNoTracking().ToList();
        //            var TempNotifyUserList = new List<ComplaintApprovalInputDto>();
        //            foreach (var item in NotifyUser)
        //            {
        //                var DemoDetail = DemoList.FirstOrDefault(_ => _.SubmittedFormId == item.ComplaintId);
        //                if (DemoDetail != null)
        //                {
        //                    TempNotifyUserList.Add(new ComplaintApprovalInputDto()
        //                    {
        //                        ComplaintId = item.ComplaintId,
        //                        LoginUserId = DemoDetail.DemoUserId,
        //                        StatusId = item.StatusId
        //                    });
        //                    TempNotifyUserList.Add(new ComplaintApprovalInputDto()
        //                    {
        //                        ComplaintId = item.ComplaintId,
        //                        LoginUserId = DemoDetail.DemoInchargeId,
        //                        StatusId = item.StatusId
        //                    });
        //                }
        //            }

        //            NotifyUser.AddRange(TempNotifyUserList);
        //            var NotificationContent = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(_ => _.Name.Equals(Constants.ComplaintFormStatus));
        //            var NotifyUserDetails = UserContext.Join(NotifyUser, u => u.Id, n => n.LoginUserId, (u, n) => new { u, n })
        //                                    .Where(_ => _.u.IsActive && !string.IsNullOrEmpty(_.u.PushTokenKey)).ToList();
        //            if (NotificationContent != null)
        //                foreach (var user in NotifyUserDetails)
        //                {
        //                    var Formdetails = this.ViewSubmittedFormDetails(new SubmittedFormIdDto() { SubmittedFormId = user.n.ComplaintId });
        //                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                    {
        //                        PushTokenKey = user.u.PushTokenKey,
        //                        RegistrationTypeId = user.u.RegistrationTypeId != null ? (int)user.u.RegistrationTypeId : 0,
        //                        Title = Constants.ComplaintFormStatus,
        //                        Message = NotificationContent.PlainTemplate.Replace(Constants.FormId, user.n.ComplaintId.ToString())
        //                                                                       .Replace(Constants.Status, UtilityHelper.GetEnumDescription((DTO.Enums.DynamicFormStatus)user.n.StatusId))
        //                                                                       .Replace(Constants.UserName, LoginUser.Name),
        //                        IsCMSNotification = true,
        //                        SubmittedFormId = user.n.ComplaintId
        //                        //NotificationObject = Formdetails != null && Formdetails.IsSuccess ? Formdetails.SuccessDto : null,
        //                    };
        //                    _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                }

        //            return _resultService.SuccessObject("Success");
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }
        //    public ResultDto GetComplaintFormRemarks(FormRemarkInputDto inputDto)
        //    {
        //        _methodName = "GetComplaintFormRemarks";
        //        try
        //        {
        //            if (inputDto == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            if (inputDto.FormId == 0 || inputDto.RemarkTypeId == 0)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            var result = new List<FormRemarksDto>();
        //            var formContext = _emamiContext.SubmittedFormRemarks.AsNoTracking().ToList();
        //            result = formContext.Where(_ => _.SubmittedFormId == inputDto.FormId && _.RemarkType == inputDto.RemarkTypeId)
        //                .Select(_ => new FormRemarksDto()
        //                {
        //                    CreatedOn = _.CreatedDate,
        //                    Description = _.Description,
        //                    CreatedBy = GetUserName(_.CreatedBy)
        //                }).OrderByDescending(_ => _.CreatedOn).ToList();

        //            return _resultService.SuccessObject(result);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }

        //    #endregion Complaint Approval

        //    #region Mobile APIs
        //    /// <summary>
        //    /// Mobile APIs - Get logged in users assigned form list and its details
        //    /// </summary>
        //    /// <param name="loginUserIdDto"></param>
        //    /// <returns></returns>
        //    public ResultDto GetUserAssignedFormListAndDetails(LoginUserIdDto loginUserIdDto)
        //    {
        //        _methodName = "GetUserAssignedFormListAndDetails";
        //        var formDetailsListDto = new List<FormQuestionsViewDto>();
        //        try
        //        {
        //            if (loginUserIdDto == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            if (loginUserIdDto.LoginUserId == 0)
        //            {
        //                return _resultService.ErrorMessage(Constants.UserIdMissing);
        //            }
        //            if (!_resultService.UserIsAcive(loginUserIdDto.LoginUserId))
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidUser);
        //            }
        //            var userFormContext = _emamiContext.FormUsers.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.LoginUserId && _.IsActive)
        //                                                                                        .Select(_ => _.FormId).ToList();
        //            if (userFormContext != null)
        //            {
        //                foreach (var formId in userFormContext)
        //                {
        //                    var FormQuestionListContext = _emamiContext.FormQuestions.AsNoTracking().Where(_ => _.FormId == formId && !_.IsDeleted).OrderBy(_ => _.OrderNo).ToList();
        //                    if (FormQuestionListContext.Any())
        //                    {
        //                        var formQuestionsViewDto = new FormQuestionsViewDto();
        //                        var formContext = FormQuestionListContext.FirstOrDefault(_ => _.FormId == formId).Form;
        //                        if (formContext != null && formContext.IsActive)
        //                        {
        //                            formQuestionsViewDto.FormId = formContext.Id;
        //                            formQuestionsViewDto.FormName = formContext.Name;
        //                            formQuestionsViewDto.DependentFormId = formContext.ParentFormId;
        //                            formQuestionsViewDto.IsActive = formContext.IsActive;
        //                            formQuestionsViewDto.CreatedDate = formContext.CreatedDate;
        //                            formQuestionsViewDto.ModifiedDate = formContext.ModifiedDate;

        //                            var groupedSectionContext = FormQuestionListContext.GroupBy(_ => _.QuestionSectionId)
        //                                                                       .Select(group => new
        //                                                                       {
        //                                                                           group.Key,
        //                                                                           group.FirstOrDefault().Question.QuestionSection.SectionName,
        //                                                                           sectionItems = group.OrderBy(_ => _.Question.OrderId).ToList()
        //                                                                       }).OrderBy(_ => _.Key).ToList();
        //                            foreach (var sectionQuestion in groupedSectionContext)
        //                            {
        //                                var sectionsDto = new SectionQuestionsViewDto
        //                                {
        //                                    SectionId = sectionQuestion.Key,
        //                                    SectionName = sectionQuestion.SectionName
        //                                };
        //                                foreach (var question in sectionQuestion.sectionItems)
        //                                {
        //                                    var questionsViewDto = new QuestionsViewDto
        //                                    {
        //                                        QuestionTypeId = question.Question.QuestionTypeId,
        //                                        QuestionTypeName = question.Question.QuestionType.Name,
        //                                        Query = question.Question.Query,
        //                                        QuestionId = question.Question.Id,
        //                                        OrderNo = question.OrderNo,
        //                                        IsDeleted = question.IsDeleted,
        //                                        Description = question.Question.Description,
        //                                        IsMandatory = question.Question.IsMandatory,
        //                                        CreatedDate = question.CreatedDate,
        //                                        ModifiedDate = question.ModifiedDate
        //                                    };
        //                                    if (question.Question.QuestionTypeId == (int)DTO.Enums.QuestionType.MultipleChoice || question.Question.QuestionTypeId == (int)DTO.Enums.QuestionType.SingleChoice)
        //                                    {
        //                                        if (question.Question.AnswerOptions.Where(_ => !_.IsDeleted).Any())
        //                                        {
        //                                            foreach (var answerOption in question.Question.AnswerOptions.Where(_ => !_.IsDeleted).ToList())
        //                                            {
        //                                                var answerOptionDto = new AnswerOptionDto
        //                                                {
        //                                                    AnswerOptionId = answerOption.Id,
        //                                                    Option = answerOption.Option
        //                                                };
        //                                                questionsViewDto.AnswerOptions.Add(answerOptionDto);
        //                                            }
        //                                        }
        //                                    }
        //                                    sectionsDto.Questions.Add(questionsViewDto);
        //                                }
        //                                formQuestionsViewDto.SectionQuestions.Add(sectionsDto);
        //                            }
        //                            formDetailsListDto.Add(formQuestionsViewDto);
        //                        }
        //                    }
        //                }
        //                return _resultService.SuccessObject(formDetailsListDto);
        //            }
        //            else
        //            {
        //                return _resultService.ErrorMessage(Constants.FormsNotAssignedToUser);
        //            }
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }

        //    public ResultDto GetSubmittedFormListAndDetailsByUserId(SubmittedFormsInputDto submittedFormsInputDto)
        //    {
        //        _methodName = "GetSubmittedFormListAndDetailsByUserId";
        //        var submittedFormListDto = new SubmittedFormsPaginationOutputDto();
        //        try
        //        {
        //            if (submittedFormsInputDto == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            if (submittedFormsInputDto.LoginUserId == 0)
        //            {
        //                return _resultService.ErrorMessage(Constants.UserIdMissing);
        //            }
        //            if (!_resultService.UserIsAcive(submittedFormsInputDto.LoginUserId))
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidUser);
        //            }
        //            var userRole = GetUserRoleId(submittedFormsInputDto.LoginUserId);
        //            if (userRole == 0)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidUser);
        //            }
        //            var submittedFormContext = new List<SubmittedForm>();

        //            var UserRoleClaims = _emamiContext.RoleClaims.AsNoTracking().Where(_ => _.RoleId == userRole).Select(_ => _.ClaimId);
        //            if (UserRoleClaims == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.ClaimEmpty);
        //            }
        //            if (UserRoleClaims.Contains((int)Claims.CreateComplaintForms) || UserRoleClaims.Contains((int)Claims.CreateUnderstandingForms))
        //            {
        //                submittedFormContext = _emamiContext.SubmittedForms.AsNoTracking()
        //                                                                    .Where(_ => _.CreatedBy == submittedFormsInputDto.LoginUserId || _.DemoUserId == submittedFormsInputDto.LoginUserId)
        //                                                                    .OrderByDescending(_ => _.CreatedDate)
        //                                                                    .ToList();
        //            }
        //            else if (UserRoleClaims.Contains((int)Claims.ManageComplaints))
        //            {
        //                submittedFormContext = _emamiContext.SubmittedForms.AsNoTracking()
        //                                                                   .Where(_ => _.ParentFormId == null || _.ParentFormId == 0)
        //                                                                   .OrderByDescending(_ => _.CreatedDate).ToList();
        //            }
        //            if (submittedFormContext.Any())
        //            {
        //                //Applying Sorting filters
        //                if (submittedFormsInputDto.CustomerId != 0)
        //                {
        //                    var customerFilteredContext = submittedFormContext.Where(_ => _.UserId == submittedFormsInputDto.CustomerId)
        //                                                                      .OrderByDescending(_ => _.CreatedDate).ToList();
        //                    submittedFormContext.Clear();
        //                    submittedFormContext.AddRange(customerFilteredContext);
        //                }
        //                if (submittedFormsInputDto.DealerId != 0)
        //                {
        //                    var DealerFilteredContext = submittedFormContext.Where(_ => _.DealerId == submittedFormsInputDto.DealerId)
        //                                                                      .OrderByDescending(_ => _.CreatedDate).ToList();
        //                    submittedFormContext.Clear();
        //                    submittedFormContext.AddRange(DealerFilteredContext);
        //                }
        //                if (submittedFormsInputDto.SubmittedDate != null && submittedFormsInputDto.SubmittedDate != DateTime.MinValue)
        //                {
        //                    var dateFilteredContext = submittedFormContext.Where(_ => _.CreatedDate.Date == submittedFormsInputDto.SubmittedDate.Value.Date)
        //                                                                  .OrderByDescending(_ => _.CreatedDate).ToList();
        //                    submittedFormContext.Clear();
        //                    submittedFormContext.AddRange(dateFilteredContext);
        //                }
        //                if (submittedFormsInputDto.EmployeeId != 0)
        //                {
        //                    var employeeFilteredContext = submittedFormContext.Where(_ => _.CreatedBy == submittedFormsInputDto.EmployeeId)
        //                                                                      .OrderByDescending(_ => _.CreatedDate).ToList();
        //                    submittedFormContext.Clear();
        //                    submittedFormContext.AddRange(employeeFilteredContext);
        //                }
        //                if (submittedFormsInputDto.StatusId != 0)
        //                {
        //                    var statusFilteredContext = submittedFormContext.Where(_ => _.FormStatusId == submittedFormsInputDto.StatusId)
        //                                                                    .OrderByDescending(_ => _.CreatedDate).ToList();
        //                    submittedFormContext.Clear();
        //                    submittedFormContext.AddRange(statusFilteredContext);
        //                }
        //                if (submittedFormsInputDto.SkuId != 0)
        //                {
        //                    var submittedFormIds = submittedFormContext.Select(sf => sf.Id).ToList();
        //                    var skuFilteredContext = _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                                    .Where(_ => _.SkuId == submittedFormsInputDto.SkuId &&
        //                                                                    submittedFormIds.Contains(_.SubmittedFormId))
        //                                                                    .Select(_ => _.SubmittedForm)
        //                                                                    .OrderByDescending(_ => _.CreatedDate)
        //                                                                    .ToList();
        //                    submittedFormContext.Clear();
        //                    submittedFormContext.AddRange(skuFilteredContext);

        //                }
        //                if (submittedFormsInputDto.PlantId != 0)
        //                {
        //                    var submittedFormIds = submittedFormContext.Select(sf => sf.Id).ToList();
        //                    var plantFilteredContext = _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                            .Where(_ => _.PlantId == submittedFormsInputDto.PlantId &&
        //                                                            submittedFormIds.Contains(_.SubmittedFormId))
        //                                                            .Select(_ => _.SubmittedForm)
        //                                                            .OrderByDescending(_ => _.CreatedDate)
        //                                                            .ToList();
        //                    submittedFormContext.Clear();
        //                    submittedFormContext.AddRange(plantFilteredContext);
        //                }
        //                if (submittedFormsInputDto.StateId != 0)
        //                {
        //                    var submittedFormIds = submittedFormContext.Select(sf => sf.Id).ToList();
        //                    var stateFilteredContext = _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                            .Where(_ => _.StateId == submittedFormsInputDto.StateId &&
        //                                                            submittedFormIds.Contains(_.SubmittedFormId))
        //                                                            .Select(_ => _.SubmittedForm)
        //                                                            .OrderByDescending(_ => _.CreatedDate)
        //                                                            .ToList();
        //                    submittedFormContext.Clear();
        //                    submittedFormContext.AddRange(stateFilteredContext);
        //                }
        //                if (submittedFormsInputDto.CityId != 0)
        //                {
        //                    var submittedFormIds = submittedFormContext.Select(sf => sf.Id).ToList();
        //                    var cityFilteredContext = _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                            .Where(_ => _.CityId == submittedFormsInputDto.CityId &&
        //                                                            submittedFormIds.Contains(_.SubmittedFormId))
        //                                                            .Select(_ => _.SubmittedForm)
        //                                                            .OrderByDescending(_ => _.CreatedDate)
        //                                                            .ToList();
        //                    submittedFormContext.Clear();
        //                    submittedFormContext.AddRange(cityFilteredContext);
        //                }

        //                submittedFormListDto.TotalRecords = submittedFormContext.Count;
        //                //Pagination for output list
        //                if (submittedFormsInputDto.PageSize > 0 && submittedFormsInputDto.StartIndex >= 0 && submittedFormContext.Count > 0)
        //                {
        //                    int CurrentPage = submittedFormsInputDto.StartIndex;
        //                    int PageSize = submittedFormsInputDto.PageSize;
        //                    CurrentPage = Convert.ToInt32(Math.Floor(Convert.ToDecimal(CurrentPage / PageSize)));
        //                    CurrentPage = CurrentPage + 1;
        //                    if (submittedFormsInputDto.StartIndex >= submittedFormContext.Count)
        //                    {
        //                        return _resultService.ErrorMessage(Constants.StartIndexExceedTotalRecords);
        //                    }
        //                    //Apply pagination
        //                    submittedFormContext = submittedFormContext.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        //                }
        //                foreach (var submittedForm in submittedFormContext)
        //                {
        //                    var submittedViewDto = new SubmittedFormViewDto
        //                    {
        //                        SubmittedFormId = submittedForm.Id,
        //                        CreatedDate = submittedForm.CreatedDate,
        //                        FormId = submittedForm.FormId,
        //                        FormName = submittedForm.FormName,
        //                        ParentFormId = submittedForm.ParentFormId,
        //                        ParentFormName = submittedForm.ParentFormId != null && submittedForm.ParentFormId != 0 ? GetSubmittedFormName(submittedForm.ParentFormId ?? 0) : string.Empty,
        //                        FormStatusName = submittedForm.FormStatus != null ? submittedForm.FormStatus.Name : string.Empty,
        //                        DemonstratedBy = GetUserName(submittedForm.DemoUserId ?? 0),
        //                        RaisedFor = submittedForm.Retailer != null ? submittedForm.Retailer.AccountName : submittedForm.CustomerName,
        //                        FormApprovalStatusId = submittedForm.FormApprovalStatusId ?? 0,
        //                        FormStatusId = submittedForm.FormStatusId ?? 0,
        //                        Remarks = submittedForm.Remarks,
        //                        RaisedBy = GetUserName(submittedForm.CreatedBy),
        //                        CustomerId = submittedForm.UserId ?? 0,
        //                        DealerId = submittedForm.DealerId ?? 0,
        //                        DealerName = submittedForm.DealerName
        //                    };
        //                    //Add Submitted form details
        //                    var submittedformdetails = _emamiContext.SubmittedFormDetails.AsNoTracking().FirstOrDefault(_ => _.SubmittedFormId == submittedForm.Id);
        //                    if (submittedformdetails != null)
        //                    {
        //                        submittedViewDto.SkuId = submittedformdetails.SkuId;
        //                        submittedViewDto.SkuName = submittedformdetails.Sku != null ? submittedformdetails.Sku.SkuName : string.Empty;
        //                        submittedViewDto.PlantId = submittedformdetails.PlantId;
        //                        submittedViewDto.PlantName = GetPlantName(submittedformdetails.PlantId);
        //                        submittedViewDto.StateId = submittedformdetails.StateId;
        //                        submittedViewDto.StateName = submittedformdetails.State != null ? submittedformdetails.State.StateName : string.Empty;
        //                        submittedViewDto.CityId = submittedformdetails.CityId;
        //                        submittedViewDto.CityName = submittedformdetails.City != null ? submittedformdetails.City.CityName : string.Empty;
        //                    }

        //                    if (submittedForm.SubmittedFormQuestions.Any())
        //                    {
        //                        var groupedSectionContext = submittedForm.SubmittedFormQuestions.GroupBy(_ => _.SectionId)
        //                                                                                       .Select(group => new
        //                                                                                       {
        //                                                                                           group.Key,
        //                                                                                           group.FirstOrDefault().SectionName,
        //                                                                                           sectionItems = group.ToList()
        //                                                                                       }).ToList();
        //                        foreach (var section in groupedSectionContext)
        //                        {
        //                            var sectionDto = new SectionDto
        //                            {
        //                                SectionId = section.Key,
        //                                SectionName = section.SectionName
        //                            };
        //                            foreach (var question in section.sectionItems)
        //                            {
        //                                var submittedQuestionViewDto = new SubmittedFormQuestionViewDto
        //                                {
        //                                    QuestionId = question.QuestionId,
        //                                    QuestionTypeId = question.QuestionTypeId,
        //                                    Question = question.Query,
        //                                    QuestionTypeName = question.QuestionTypeName,
        //                                    SubmittedFormQuestionId = question.Id,
        //                                    Description = GetQuestionDescription(question.QuestionId),
        //                                    IsMandatory = GetQuestionMandatoryValue(question.QuestionId)
        //                                };
        //                                if (question.Answers.Any())
        //                                {
        //                                    foreach (var answer in question.Answers.ToList())
        //                                    {
        //                                        if (answer.IsYes != null)
        //                                        {
        //                                            submittedQuestionViewDto.YesNo = new SubmittedYesNoAnswerViewDto
        //                                            {
        //                                                IsYes = Convert.ToBoolean(answer.IsYes)
        //                                            };
        //                                            break;
        //                                        }
        //                                        else if (!string.IsNullOrEmpty(answer.TextAnswer))
        //                                        {
        //                                            submittedQuestionViewDto.TextAnswer = new SubmittedTextAnswerViewDto
        //                                            {
        //                                                TextAnswer = answer.TextAnswer
        //                                            };
        //                                            break;
        //                                        }
        //                                        else if (!string.IsNullOrEmpty(answer.AttachmentFileName))
        //                                        {
        //                                            var submittedAttachmentDto = new SubmittedAttachmentViewDto
        //                                            {
        //                                                FileName = answer.AttachmentFileName,
        //                                                MediaTypeId = answer.MediaTypeId ?? 0
        //                                            };
        //                                            submittedQuestionViewDto.Attachments.Add(submittedAttachmentDto);
        //                                        }
        //                                        else
        //                                        {
        //                                            if (answer.AnswerOptionId != null && !string.IsNullOrEmpty(answer.Option) && answer.IsSelected != null)
        //                                            {
        //                                                var questionMultiAnswerViewDto = new SubmittedFormMultiAnswerViewDto
        //                                                {
        //                                                    AnswerOptionId = answer.AnswerOptionId ?? 0,
        //                                                    Option = answer.Option,
        //                                                    IsSelected = answer.IsSelected,
        //                                                };
        //                                                submittedQuestionViewDto.AnswerOptions.Add(questionMultiAnswerViewDto);
        //                                            }
        //                                            else
        //                                            {
        //                                                //If question's answer is not saved because of Not mandatory fields, Added the options from masters
        //                                                var questionMasterAnswerOptionContext = _emamiContext.AnswerOptions.AsNoTracking().Where(_ => !_.IsDeleted && _.QuestionId == question.QuestionId).ToList();
        //                                                foreach (var masteranswer in questionMasterAnswerOptionContext)
        //                                                {
        //                                                    var questionMultiAnswerMasterViewDto = new SubmittedFormMultiAnswerViewDto
        //                                                    {
        //                                                        AnswerOptionId = masteranswer.Id,
        //                                                        Option = masteranswer.Option,
        //                                                        IsSelected = false,
        //                                                    };
        //                                                    submittedQuestionViewDto.AnswerOptions.Add(questionMultiAnswerMasterViewDto);
        //                                                }
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                sectionDto.Questions.Add(submittedQuestionViewDto);
        //                            }
        //                            submittedViewDto.Sections.Add(sectionDto);
        //                        }

        //                        //Add Dependent form details
        //                        var submittedDependentFormContext = _emamiContext.SubmittedForms.AsNoTracking().Where(_ => _.ParentFormId == submittedForm.Id).ToList();
        //                        if (submittedDependentFormContext.Any())
        //                        {
        //                            foreach (var form in submittedDependentFormContext)
        //                            {
        //                                var dependentForm = new SubmittedDependentFormDto
        //                                {
        //                                    SubmittedFormId = form.Id,
        //                                    FormId = form.FormId,
        //                                    FormName = form.FormName,
        //                                    CreatedDate = form.CreatedDate,
        //                                    DemonstratedBy = GetUserName(form.DemoUserId ?? 0)
        //                                };
        //                                if (form.SubmittedFormQuestions.Any())
        //                                {
        //                                    var dependentSectionContext = form.SubmittedFormQuestions.GroupBy(_ => _.SectionId)
        //                                                                                       .Select(group => new
        //                                                                                       {
        //                                                                                           group.Key,
        //                                                                                           group.FirstOrDefault().SectionName,
        //                                                                                           sectionItems = group.ToList()
        //                                                                                       }).ToList();
        //                                    foreach (var section in dependentSectionContext)
        //                                    {
        //                                        var sectionDto = new SectionDto
        //                                        {
        //                                            SectionId = section.Key,
        //                                            SectionName = section.SectionName
        //                                        };
        //                                        foreach (var question in section.sectionItems)
        //                                        {
        //                                            var submittedQuestionViewDto = new SubmittedFormQuestionViewDto
        //                                            {
        //                                                QuestionId = question.QuestionId,
        //                                                QuestionTypeId = question.QuestionTypeId,
        //                                                Question = question.Query,
        //                                                QuestionTypeName = question.QuestionTypeName
        //                                            };
        //                                            if (question.Answers.Any())
        //                                            {
        //                                                foreach (var answer in question.Answers.ToList())
        //                                                {
        //                                                    if (answer.IsYes != null)
        //                                                    {
        //                                                        submittedQuestionViewDto.YesNo = new SubmittedYesNoAnswerViewDto
        //                                                        {
        //                                                            IsYes = Convert.ToBoolean(answer.IsYes)
        //                                                        };
        //                                                        break;
        //                                                    }
        //                                                    else if (!string.IsNullOrEmpty(answer.TextAnswer))
        //                                                    {
        //                                                        submittedQuestionViewDto.TextAnswer = new SubmittedTextAnswerViewDto
        //                                                        {
        //                                                            TextAnswer = answer.TextAnswer
        //                                                        };
        //                                                        break;
        //                                                    }
        //                                                    else if (!string.IsNullOrEmpty(answer.AttachmentFileName))
        //                                                    {
        //                                                        var submittedAttachmentDto = new SubmittedAttachmentViewDto
        //                                                        {
        //                                                            FileName = answer.AttachmentFileName,
        //                                                            MediaTypeId = answer.MediaTypeId ?? 0
        //                                                        };
        //                                                        submittedQuestionViewDto.Attachments.Add(submittedAttachmentDto);
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        var inspectionMultiAnswerViewDto = new SubmittedFormMultiAnswerViewDto
        //                                                        {
        //                                                            AnswerOptionId = answer.AnswerOptionId ?? 0,
        //                                                            Option = answer.Option,
        //                                                            IsSelected = answer.IsSelected,
        //                                                        };
        //                                                        submittedQuestionViewDto.AnswerOptions.Add(inspectionMultiAnswerViewDto);
        //                                                    }
        //                                                }
        //                                            }
        //                                            sectionDto.Questions.Add(submittedQuestionViewDto);
        //                                        }
        //                                        dependentForm.Sections.Add(sectionDto);
        //                                    }
        //                                    submittedViewDto.DependentForms.Add(dependentForm);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    submittedFormListDto.SubmittedFormViewList.Add(submittedViewDto);
        //                }
        //            }
        //            return _resultService.SuccessObject(submittedFormListDto);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }

        //    public ResultDto GetScheduledDemoDetailsByUserId(SubmittedFormsInputDto inputDto)
        //    {
        //        _methodName = "GetScheduledDemoDetailsByUserId";
        //        var submittedFormListDto = new SubmittedFormsPaginationOutputDto();
        //        try
        //        {
        //            if (inputDto == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            if (inputDto.LoginUserId == 0)
        //            {
        //                return _resultService.ErrorMessage(Constants.UserIdMissing);
        //            }
        //            if (!_resultService.UserIsAcive(inputDto.LoginUserId))
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidUser);
        //            }
        //            var userRole = GetUserRoleId(inputDto.LoginUserId);
        //            if (userRole == 0)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidUser);
        //            }
        //            var UserRoleClaims = _emamiContext.RoleClaims.AsNoTracking().Where(_ => _.RoleId == userRole).Select(_ => _.ClaimId);
        //            if (UserRoleClaims == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.ClaimEmpty);
        //            }

        //            //complaints created by login userid
        //            var SubmittedComplaintFormIds = _emamiContext.SubmittedForms.AsNoTracking().Where(_ => _.CreatedBy == inputDto.LoginUserId && (_.DemoId == 0 || _.DemoId == null)).Select(a => a.Id).ToList();
        //            //the login user id - whether he is an Eal UserId of any demo
        //            var DemoIds = _emamiContext.ScheduleDemoUserMappings.AsNoTracking().Where(_ => _.EALUserId == inputDto.LoginUserId).Select(a => a.DemoId).Distinct().ToList();
        //            var scheduleDemoContext = _emamiContext.ScheduleDemoUsers.AsNoTracking().Where(_ => (_.DemoInchargeId == inputDto.LoginUserId || _.DemoUserId == inputDto.LoginUserId) ||
        //                                                                                                     //DbFunctions.TruncateTime(_.DemoDate) == DbFunctions.TruncateTime(DateTime.Today) &&
        //                                                                                                     DemoIds.Contains(_.Id) || SubmittedComplaintFormIds.Contains(_.SubmittedFormId) &&
        //                                                                                                 _.IsActive).OrderByDescending(_ => _.CreatedDate)
        //                                                                                                 .GroupBy(_ => _.SubmittedFormId)
        //                                                                                                 .Select(group => new
        //                                                                                                 {
        //                                                                                                     subFormId = group.Key,
        //                                                                                                     demodetails = group.ToList()
        //                                                                                                 }).ToList();
        //            var SubmittedIdsWithDemo = scheduleDemoContext.Select(a => a.subFormId).ToList();
        //            var SubmittedFormIdsWithoutDemo = SubmittedComplaintFormIds.Where(complaint => !SubmittedIdsWithDemo.Any(a => a == complaint)).ToList();



        //            if (inputDto.SubmittedDate != null && inputDto.SubmittedDate != DateTime.MinValue)
        //            {
        //                SubmittedComplaintFormIds = _emamiContext.SubmittedForms.AsNoTracking().Where(_ => _.CreatedBy == inputDto.LoginUserId &&
        //                                                                                                DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(inputDto.SubmittedDate) && (_.DemoId == 0 || _.DemoId == null)).Select(a => a.Id).ToList();

        //                var ScheduleDemoUserContext = _emamiContext.ScheduleDemoUsers.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.DemoDate) == DbFunctions.TruncateTime(inputDto.SubmittedDate)
        //                                                                                                    && _.IsActive).OrderByDescending(_ => _.CreatedDate).ToList();

        //                scheduleDemoContext = ScheduleDemoUserContext.Where(_ => (_.DemoInchargeId == inputDto.LoginUserId || _.DemoUserId == inputDto.LoginUserId) ||
        //                                                                                                    DemoIds.Contains(_.Id) || SubmittedComplaintFormIds.Contains(_.SubmittedFormId))
        //                                                                                                 .GroupBy(_ => _.SubmittedFormId)
        //                                                                                                 .Select(group => new
        //                                                                                                 {
        //                                                                                                     subFormId = group.Key,
        //                                                                                                     demodetails = group.ToList()
        //                                                                                                 }).ToList();
        //                SubmittedIdsWithDemo = scheduleDemoContext.Select(a => a.subFormId).ToList();
        //                SubmittedFormIdsWithoutDemo = SubmittedComplaintFormIds.Where(complaint => !SubmittedIdsWithDemo.Any(a => a == complaint)).ToList();

        //            }
        //         if (scheduleDemoContext.Any() || SubmittedComplaintFormIds.IsAny())
        //           {
        //                if (UserRoleClaims.Contains((int)Claims.ViewComplaints))
        //                {
        //                    var submittedFormFilteredIDContext = new List<long>();

        //                    var submittedFormFilteredContext = (from submittedforms in _emamiContext.SubmittedForms.AsNoTracking()
        //                                                        join subformDetails in _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                        on submittedforms.Id equals subformDetails.SubmittedFormId
        //                                                        join demoforms in _emamiContext.ScheduleDemoUsers.AsNoTracking()
        //                                                        on submittedforms.Id equals demoforms.SubmittedFormId
        //                                                        select submittedforms.Id).Distinct().ToList();

        //                    //Applying Sorting filters
        //                    if (inputDto.StateId != 0)
        //                    {
        //                        var stateFilterContext = _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                             .Where(_ => (_.StateId == inputDto.StateId) &&
        //                                                                         submittedFormFilteredContext.Contains(_.SubmittedFormId))
        //                                                             .Select(_ => _.SubmittedFormId).Distinct().ToList();
        //                        submittedFormFilteredIDContext.Clear();
        //                        submittedFormFilteredIDContext.AddRange(stateFilterContext);
        //                    }
        //                    else if (inputDto.CityId != 0)
        //                    {
        //                        var cityFilterContext = _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                             .Where(_ => (_.CityId == inputDto.CityId) &&
        //                                                                         submittedFormFilteredContext.Contains(_.SubmittedFormId))
        //                                                             .Select(_ => _.SubmittedFormId).Distinct().ToList();
        //                        submittedFormFilteredIDContext.Clear();
        //                        submittedFormFilteredIDContext.AddRange(cityFilterContext);
        //                    }
        //                    else
        //                    {
        //                        submittedFormFilteredIDContext.Clear();
        //                        submittedFormFilteredIDContext.AddRange(submittedFormFilteredContext);
        //                    }

        //                    if (inputDto.PlantId != 0)
        //                    {
        //                        var plantFilteredContext = _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                             .Where(_ => (_.PlantId == inputDto.PlantId) &&
        //                                                                         submittedFormFilteredIDContext.Contains(_.SubmittedFormId))
        //                                                             .Select(_ => _.SubmittedFormId).Distinct().ToList();
        //                        submittedFormFilteredIDContext.Clear();
        //                        submittedFormFilteredIDContext.AddRange(plantFilteredContext);
        //                    }
        //                    if (inputDto.SkuId != 0)
        //                    {
        //                        var skuFilteredContext = _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                             .Where(_ => (_.SkuId == inputDto.SkuId) &&
        //                                                                         submittedFormFilteredIDContext.Contains(_.SubmittedFormId))
        //                                                             .Select(_ => _.SubmittedFormId).Distinct().ToList();
        //                        submittedFormFilteredIDContext.Clear();
        //                        submittedFormFilteredIDContext.AddRange(skuFilteredContext);
        //                    }
        //                    if (inputDto.CustomerId != 0)
        //                    {
        //                        var customerFilteredContext = _emamiContext.SubmittedForms.AsNoTracking()
        //                                                             .Where(_ => (_.UserId == inputDto.CustomerId) &&
        //                                                                         submittedFormFilteredIDContext.Contains(_.Id))
        //                                                             .Select(_ => _.Id).Distinct().ToList();
        //                        submittedFormFilteredIDContext.Clear();
        //                        submittedFormFilteredIDContext.AddRange(customerFilteredContext);
        //                    }
        //                    if (inputDto.DealerId != 0)
        //                    {
        //                        var DealerFilteredContext = _emamiContext.SubmittedForms.AsNoTracking()
        //                                                             .Where(_ => (_.DealerId == inputDto.DealerId) &&
        //                                                                         submittedFormFilteredIDContext.Contains(_.Id))
        //                                                             .Select(_ => _.Id).Distinct().ToList();
        //                        submittedFormFilteredIDContext.Clear();
        //                        submittedFormFilteredIDContext.AddRange(DealerFilteredContext);
        //                    }
        //                    if (inputDto.EmployeeId != 0)
        //                    {
        //                        var employeeFilteredContext = _emamiContext.SubmittedForms.AsNoTracking()
        //                                                             .Where(_ => (_.CreatedBy == inputDto.EmployeeId) &&
        //                                                                         submittedFormFilteredIDContext.Contains(_.Id))
        //                                                             .Select(_ => _.Id).Distinct().ToList();
        //                        submittedFormFilteredIDContext.Clear();
        //                        submittedFormFilteredIDContext.AddRange(employeeFilteredContext);
        //                    }
        //                    if (inputDto.StatusId != 0)
        //                    {
        //                        var StatusFilteredContext = _emamiContext.SubmittedForms.AsNoTracking()
        //                                                             .Where(_ => (_.FormStatusId == inputDto.StatusId) &&
        //                                                                         submittedFormFilteredIDContext.Contains(_.Id))
        //                                                             .Select(_ => _.Id).Distinct().ToList();
        //                        submittedFormFilteredIDContext.Clear();
        //                        submittedFormFilteredIDContext.AddRange(StatusFilteredContext);
        //                    }



        //                    foreach (var demo in scheduleDemoContext)
        //                    {
        //                        if (submittedFormFilteredIDContext.Any())
        //                        {
        //                            var submittedFormContext = _emamiContext.SubmittedForms.AsNoTracking().FirstOrDefault(_ => _.Id == demo.subFormId && submittedFormFilteredIDContext.Contains(demo.subFormId));
        //                            if (submittedFormContext != null)
        //                            {
        //                                var demoViewDto = new SubmittedFormViewDto()
        //                                {
        //                                    SubmittedFormId = demo.subFormId,
        //                                    FormId = submittedFormContext.FormId,
        //                                    FormName = submittedFormContext.FormName,
        //                                    FormStatusName = submittedFormContext.FormStatus != null ? submittedFormContext.FormStatus.Name : string.Empty,
        //                                    FormApprovalStatusId = submittedFormContext.FormApprovalStatusId ?? 0,
        //                                    FormStatusId = submittedFormContext.FormStatusId ?? 0,
        //                                    CreatedDate = (submittedFormContext.ModifiedDate == null || submittedFormContext.ModifiedDate == DateTime.MinValue) ? submittedFormContext.CreatedDate : submittedFormContext.ModifiedDate.Value,
        //                                    RaisedBy = GetUserName(submittedFormContext.CreatedBy),
        //                                    RaisedFor = submittedFormContext.Retailer != null ? submittedFormContext.Retailer.AccountName : submittedFormContext.CustomerName,
        //                                    IsLatLonUpdated = submittedFormContext.Retailer != null ? ((string.IsNullOrEmpty(submittedFormContext.Retailer.Longitude) && string.IsNullOrEmpty(submittedFormContext.Retailer.Latitude)) ? false : true) : false,
        //                                    CustomerId = submittedFormContext.UserId ?? 0,
        //                                    Remarks = submittedFormContext.Remarks,
        //                                    DealerId = submittedFormContext.DealerId ?? 0,
        //                                    DealerName = submittedFormContext.DealerName,
        //                                };

        //                                //Add Submitted form details
        //                                var submittedformdetails = _emamiContext.SubmittedFormDetails.AsNoTracking().FirstOrDefault(_ => _.SubmittedFormId == demo.subFormId);
        //                                if (submittedformdetails != null)
        //                                {
        //                                    demoViewDto.SkuId = submittedformdetails.SkuId;
        //                                    demoViewDto.SkuName = submittedformdetails.Sku != null ? submittedformdetails.Sku.SkuName : string.Empty;
        //                                    demoViewDto.PlantId = submittedformdetails.PlantId;
        //                                    demoViewDto.PlantName = GetPlantName(submittedformdetails.PlantId);
        //                                    demoViewDto.StateId = submittedformdetails.StateId;
        //                                    demoViewDto.StateName = submittedformdetails.State != null ? submittedformdetails.State.StateName : string.Empty;
        //                                    demoViewDto.CityId = submittedformdetails.CityId;
        //                                    demoViewDto.CityName = submittedformdetails.City != null ? submittedformdetails.City.CityName : string.Empty;
        //                                }

        //                                foreach (var demoDetail in demo.demodetails)
        //                                {
        //                                    var submittedUnderstandingForms = _emamiContext.SubmittedForms.AsNoTracking().Where(_ => _.DemoId == demoDetail.Id)
        //                                                                                                                 .Select(_ => _.Id).ToList();
        //                                    var scheduleDemoEalUserIds = _emamiContext.ScheduleDemoUserMappings.AsNoTracking().Where(_ => _.DemoId == demoDetail.Id).Select(_ => _.EALUserId).ToList();
        //                                    var demoDto = new ScheduleDemoOutputDto
        //                                    {
        //                                        DemoId = demoDetail.Id,
        //                                        DemoCreatedBy = GetUserName(demoDetail.CreatedBy),
        //                                        DemoDateTime = demoDetail.DemoDate,
        //                                        DemonstratorName = demoDetail.DemoUser != null ? demoDetail.DemoUser.Name : string.Empty,
        //                                        DemoInchargeName = GetUserName(demoDetail.DemoInchargeId),
        //                                        SalesExecutiveName = GetUserName(submittedFormContext.CreatedBy),
        //                                        IsActive = demoDetail.IsActive,
        //                                        SubmittedUnderstandingForms = submittedUnderstandingForms,
        //                                        UnderstandingFormId = demoDetail.DependentMasterFormId,
        //                                        EALUserId = scheduleDemoEalUserIds
        //                                    };
        //                                    demoViewDto.DemoDetails.Add(demoDto);
        //                                }

        //                                GetSubmittedAndDependentMasterFormDetails(submittedFormContext, demoViewDto);
        //                                submittedFormListDto.SubmittedFormViewList.Add(demoViewDto);
        //                            }
        //                        }
        //                    }
        //                }
        //                    GetSubmittedForms(inputDto, SubmittedFormIdsWithoutDemo, submittedFormListDto);
        //                    submittedFormListDto.TotalRecords = submittedFormListDto.SubmittedFormViewList.Count;
        //                    //Pagination for output list
        //                    if (inputDto.PageSize > 0 && inputDto.StartIndex >= 0 && submittedFormListDto.SubmittedFormViewList.Count > 0)
        //                    {
        //                        int CurrentPage = inputDto.StartIndex;
        //                        int PageSize = inputDto.PageSize;
        //                        CurrentPage = Convert.ToInt32(Math.Floor(Convert.ToDecimal(CurrentPage / PageSize)));
        //                        CurrentPage = CurrentPage + 1;
        //                        if (inputDto.StartIndex >= submittedFormListDto.SubmittedFormViewList.Count)
        //                        {
        //                            return _resultService.ErrorMessage(Constants.StartIndexExceedTotalRecords);
        //                        }
        //                        //Apply pagination
        //                        submittedFormListDto.SubmittedFormViewList = submittedFormListDto.SubmittedFormViewList.OrderByDescending(a => a.SubmittedFormId).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        //                    }

        //           }
        //            return _resultService.SuccessObject(submittedFormListDto);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }

        //    private void GetSubmittedForms(SubmittedFormsInputDto submittedFormsInputDto, List<long> SubmittedComplaintFormIds, SubmittedFormsPaginationOutputDto submittedFormListDto)
        //    {

        //        var submittedFormContextList = _emamiContext.SubmittedForms.AsNoTracking().Where(_ => SubmittedComplaintFormIds.Contains(_.Id)).ToList();

        //        if (submittedFormContextList.IsAny())
        //        {
        //            //Applying Sorting filters
        //            if (submittedFormsInputDto.CustomerId != 0)
        //            {
        //                var customerFilteredContext = submittedFormContextList.Where(_ => _.UserId == submittedFormsInputDto.CustomerId)
        //                                                                  .OrderByDescending(_ => _.CreatedDate).ToList();
        //                submittedFormContextList.Clear();
        //                submittedFormContextList.AddRange(customerFilteredContext);
        //            }
        //            if (submittedFormsInputDto.DealerId != 0)
        //            {
        //                var DealerFilteredContext = submittedFormContextList.Where(_ => _.DealerId == submittedFormsInputDto.DealerId)
        //                                                                  .OrderByDescending(_ => _.CreatedDate).ToList();
        //                submittedFormContextList.Clear();
        //                submittedFormContextList.AddRange(DealerFilteredContext);
        //            }
        //            if (submittedFormsInputDto.SubmittedDate != null && submittedFormsInputDto.SubmittedDate != DateTime.MinValue)
        //            {
        //                var dateFilteredContext = submittedFormContextList.Where(_ => _.CreatedDate.Date == submittedFormsInputDto.SubmittedDate.Value.Date)
        //                                                              .OrderByDescending(_ => _.CreatedDate).ToList();
        //                submittedFormContextList.Clear();
        //                submittedFormContextList.AddRange(dateFilteredContext);
        //            }
        //            if (submittedFormsInputDto.EmployeeId != 0)
        //            {
        //                var employeeFilteredContext = submittedFormContextList.Where(_ => _.CreatedBy == submittedFormsInputDto.EmployeeId)
        //                                                                  .OrderByDescending(_ => _.CreatedDate).ToList();
        //                submittedFormContextList.Clear();
        //                submittedFormContextList.AddRange(employeeFilteredContext);
        //            }
        //            if (submittedFormsInputDto.StatusId != 0)
        //            {
        //                var statusFilteredContext = submittedFormContextList.Where(_ => _.FormStatusId == submittedFormsInputDto.StatusId)
        //                                                                .OrderByDescending(_ => _.CreatedDate).ToList();
        //                submittedFormContextList.Clear();
        //                submittedFormContextList.AddRange(statusFilteredContext);
        //            }
        //            if (submittedFormsInputDto.SkuId != 0)
        //            {
        //                var submittedFormIds = submittedFormContextList.Select(sf => sf.Id).ToList();
        //                var skuFilteredContext = _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                                .Where(_ => _.SkuId == submittedFormsInputDto.SkuId &&
        //                                                                submittedFormIds.Contains(_.SubmittedFormId))
        //                                                                .Select(_ => _.SubmittedForm)
        //                                                                .OrderByDescending(_ => _.CreatedDate)
        //                                                                .ToList();
        //                submittedFormContextList.Clear();
        //                submittedFormContextList.AddRange(skuFilteredContext);

        //            }
        //            if (submittedFormsInputDto.PlantId != 0)
        //            {
        //                var submittedFormIds = submittedFormContextList.Select(sf => sf.Id).ToList();
        //                var plantFilteredContext = _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                        .Where(_ => _.PlantId == submittedFormsInputDto.PlantId &&
        //                                                        submittedFormIds.Contains(_.SubmittedFormId))
        //                                                        .Select(_ => _.SubmittedForm)
        //                                                        .OrderByDescending(_ => _.CreatedDate)
        //                                                        .ToList();
        //                submittedFormContextList.Clear();
        //                submittedFormContextList.AddRange(plantFilteredContext);
        //            }
        //            if (submittedFormsInputDto.StateId != 0)
        //            {
        //                var submittedFormIds = submittedFormContextList.Select(sf => sf.Id).ToList();
        //                var stateFilteredContext = _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                        .Where(_ => _.StateId == submittedFormsInputDto.StateId &&
        //                                                        submittedFormIds.Contains(_.SubmittedFormId))
        //                                                        .Select(_ => _.SubmittedForm)
        //                                                        .OrderByDescending(_ => _.CreatedDate)
        //                                                        .ToList();
        //                submittedFormContextList.Clear();
        //                submittedFormContextList.AddRange(stateFilteredContext);
        //            }
        //            if (submittedFormsInputDto.CityId != 0)
        //            {
        //                var submittedFormIds = submittedFormContextList.Select(sf => sf.Id).ToList();
        //                var cityFilteredContext = _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                        .Where(_ => _.CityId == submittedFormsInputDto.CityId &&
        //                                                        submittedFormIds.Contains(_.SubmittedFormId))
        //                                                        .Select(_ => _.SubmittedForm)
        //                                                        .OrderByDescending(_ => _.CreatedDate)
        //                                                        .ToList();
        //                submittedFormContextList.Clear();
        //                submittedFormContextList.AddRange(cityFilteredContext);
        //            }
        //            foreach (var submittedFormContext in submittedFormContextList)
        //            {
        //                if (submittedFormContext != null)
        //                {
        //                    var demoViewDto = new SubmittedFormViewDto()
        //                    {
        //                        SubmittedFormId = submittedFormContext.Id,
        //                        FormId = submittedFormContext.FormId,
        //                        FormName = submittedFormContext.FormName,
        //                        FormStatusName = submittedFormContext.FormStatus != null ? submittedFormContext.FormStatus.Name : string.Empty,
        //                        FormApprovalStatusId = submittedFormContext.FormApprovalStatusId ?? 0,
        //                        FormStatusId = submittedFormContext.FormStatusId ?? 0,
        //                        CreatedDate = (submittedFormContext.ModifiedDate == null || submittedFormContext.ModifiedDate == DateTime.MinValue) ? submittedFormContext.CreatedDate : submittedFormContext.ModifiedDate.Value,
        //                        RaisedBy = GetUserName(submittedFormContext.CreatedBy),
        //                        RaisedFor = submittedFormContext.Retailer != null ? submittedFormContext.Retailer.AccountName : submittedFormContext.CustomerName,
        //                        IsLatLonUpdated = submittedFormContext.Retailer != null ? ((string.IsNullOrEmpty(submittedFormContext.Retailer.Longitude) && string.IsNullOrEmpty(submittedFormContext.Retailer.Latitude)) ? false : true) : false,
        //                        CustomerId = submittedFormContext.UserId ?? 0,
        //                        Remarks = submittedFormContext.Remarks,
        //                        DealerId = submittedFormContext.DealerId ?? 0,
        //                        DealerName = submittedFormContext.DealerName
        //                    };

        //                    //Add Submitted form details
        //                    var submittedformdetails = _emamiContext.SubmittedFormDetails.AsNoTracking().FirstOrDefault(_ => _.SubmittedFormId == submittedFormContext.Id);
        //                    if (submittedformdetails != null)
        //                    {
        //                        demoViewDto.SkuId = submittedformdetails.SkuId;
        //                        demoViewDto.SkuName = submittedformdetails.Sku != null ? submittedformdetails.Sku.SkuName : string.Empty;
        //                        demoViewDto.PlantId = submittedformdetails.PlantId;
        //                        demoViewDto.PlantName = GetPlantName(submittedformdetails.PlantId);
        //                        demoViewDto.StateId = submittedformdetails.StateId;
        //                        demoViewDto.StateName = submittedformdetails.State != null ? submittedformdetails.State.StateName : string.Empty;
        //                        demoViewDto.CityId = submittedformdetails.CityId;
        //                        demoViewDto.CityName = submittedformdetails.City != null ? submittedformdetails.City.CityName : string.Empty;
        //                    }

        //                    if (submittedFormContext.SubmittedFormQuestions.Any())
        //                    {
        //                        var groupedSectionContext = submittedFormContext.SubmittedFormQuestions.GroupBy(_ => _.SectionId)
        //                                                                                       .Select(group => new
        //                                                                                       {
        //                                                                                           group.Key,
        //                                                                                           group.FirstOrDefault().SectionName,
        //                                                                                           sectionItems = group.ToList()
        //                                                                                       }).ToList();
        //                        foreach (var section in groupedSectionContext)
        //                        {
        //                            var sectionDto = new SectionDto
        //                            {
        //                                SectionId = section.Key,
        //                                SectionName = section.SectionName
        //                            };
        //                            foreach (var question in section.sectionItems)
        //                            {
        //                                var submittedQuestionViewDto = new SubmittedFormQuestionViewDto
        //                                {
        //                                    QuestionId = question.QuestionId,
        //                                    QuestionTypeId = question.QuestionTypeId,
        //                                    Question = question.Query,
        //                                    QuestionTypeName = question.QuestionTypeName,
        //                                    SubmittedFormQuestionId = question.Id
        //                                };
        //                                if (question.Answers.Any())
        //                                {
        //                                    foreach (var answer in question.Answers.ToList())
        //                                    {
        //                                        if (answer.IsYes != null)
        //                                        {
        //                                            submittedQuestionViewDto.YesNo = new SubmittedYesNoAnswerViewDto
        //                                            {
        //                                                IsYes = Convert.ToBoolean(answer.IsYes)
        //                                            };
        //                                            break;
        //                                        }
        //                                        else if (!string.IsNullOrEmpty(answer.TextAnswer))
        //                                        {
        //                                            submittedQuestionViewDto.TextAnswer = new SubmittedTextAnswerViewDto
        //                                            {
        //                                                TextAnswer = answer.TextAnswer
        //                                            };
        //                                            break;
        //                                        }
        //                                        else if (!string.IsNullOrEmpty(answer.AttachmentFileName))
        //                                        {
        //                                            var submittedAttachmentDto = new SubmittedAttachmentViewDto
        //                                            {
        //                                                FileName = answer.AttachmentFileName,
        //                                                MediaTypeId = answer.MediaTypeId ?? 0
        //                                            };
        //                                            submittedQuestionViewDto.Attachments.Add(submittedAttachmentDto);
        //                                        }
        //                                        else
        //                                        {
        //                                            var inspectionMultiAnswerViewDto = new SubmittedFormMultiAnswerViewDto
        //                                            {
        //                                                AnswerOptionId = answer.AnswerOptionId ?? 0,
        //                                                Option = answer.Option,
        //                                                IsSelected = answer.IsSelected,
        //                                            };
        //                                            submittedQuestionViewDto.AnswerOptions.Add(inspectionMultiAnswerViewDto);
        //                                        }
        //                                    }
        //                                }
        //                                sectionDto.Questions.Add(submittedQuestionViewDto);
        //                            }
        //                            demoViewDto.Sections.Add(sectionDto);
        //                        }

        //                    }

        //                    submittedFormListDto.SubmittedFormViewList.Add(demoViewDto);
        //                }

        //            }
        //        }

        //    }

        //    //private void GetSubmittedAndDependentMasterFormDetails(SubmittedForm submittedFormContext, SubmittedFormViewDto demoViewDto)
        //    //{
        //    //    if (submittedFormContext.SubmittedFormQuestions.Any())
        //    //    {
        //    //        var groupedSectionContext = submittedFormContext.SubmittedFormQuestions.GroupBy(_ => _.SectionId)
        //    //                                                                       .Select(group => new
        //    //                                                                       {
        //    //                                                                           group.Key,
        //    //                                                                           group.FirstOrDefault().SectionName,
        //    //                                                                           sectionItems = group.ToList()
        //    //                                                                       }).ToList();
        //    //        foreach (var section in groupedSectionContext)
        //    //        {
        //    //            var sectionDto = new SectionDto
        //    //            {
        //    //                SectionId = section.Key,
        //    //                SectionName = section.SectionName
        //    //            };
        //    //            foreach (var question in section.sectionItems)
        //    //            {
        //    //                var submittedQuestionViewDto = new SubmittedFormQuestionViewDto
        //    //                {
        //    //                    QuestionId = question.QuestionId,
        //    //                    QuestionTypeId = question.QuestionTypeId,
        //    //                    Question = question.Query,
        //    //                    QuestionTypeName = question.QuestionTypeName,
        //    //                    SubmittedFormQuestionId = question.Id
        //    //                };
        //    //                if (question.Answers.Any())
        //    //                {
        //    //                    foreach (var answer in question.Answers.ToList())
        //    //                    {
        //    //                        if (answer.IsYes != null)
        //    //                        {
        //    //                            submittedQuestionViewDto.YesNo = new SubmittedYesNoAnswerViewDto
        //    //                            {
        //    //                                IsYes = Convert.ToBoolean(answer.IsYes)
        //    //                            };
        //    //                            break;
        //    //                        }
        //    //                        else if (!string.IsNullOrEmpty(answer.TextAnswer))
        //    //                        {
        //    //                            submittedQuestionViewDto.TextAnswer = new SubmittedTextAnswerViewDto
        //    //                            {
        //    //                                TextAnswer = answer.TextAnswer
        //    //                            };
        //    //                            break;
        //    //                        }
        //    //                        else if (!string.IsNullOrEmpty(answer.AttachmentFileName))
        //    //                        {
        //    //                            var submittedAttachmentDto = new SubmittedAttachmentViewDto
        //    //                            {
        //    //                                FileName = answer.AttachmentFileName,
        //    //                                MediaTypeId = answer.MediaTypeId ?? 0
        //    //                            };
        //    //                            submittedQuestionViewDto.Attachments.Add(submittedAttachmentDto);
        //    //                        }
        //    //                        else
        //    //                        {
        //    //                            var inspectionMultiAnswerViewDto = new SubmittedFormMultiAnswerViewDto
        //    //                            {
        //    //                                AnswerOptionId = answer.AnswerOptionId ?? 0,
        //    //                                Option = answer.Option,
        //    //                                IsSelected = answer.IsSelected,
        //    //                            };
        //    //                            submittedQuestionViewDto.AnswerOptions.Add(inspectionMultiAnswerViewDto);
        //    //                        }
        //    //                    }
        //    //                }
        //    //                sectionDto.Questions.Add(submittedQuestionViewDto);
        //    //            }
        //    //            demoViewDto.Sections.Add(sectionDto);
        //    //        }

        //    //        //Add Dependent form details
        //    //        var dependentFormsContext = _emamiContext.Forms.AsNoTracking().Where(_ => _.ParentFormId == submittedFormContext.FormId && _.IsActive).ToList();
        //    //        if (dependentFormsContext.Any())
        //    //        {
        //    //            foreach (var form in dependentFormsContext)
        //    //            {
        //    //                var dependentForm = new FormQuestionsViewDto()
        //    //                {
        //    //                    FormId = form.Id,
        //    //                    FormName = form.Name,
        //    //                    ModifiedDate = form.ModifiedDate
        //    //                };
        //    //                if (form.FormQuestions.Any())
        //    //                {
        //    //                    var dependentSectionContext = form.FormQuestions.Where(_ => !_.IsDeleted).GroupBy(_ => _.QuestionSectionId)
        //    //                                                                       .Select(group => new
        //    //                                                                       {
        //    //                                                                           group.Key,
        //    //                                                                           group.FirstOrDefault().Question.QuestionSection.SectionName,
        //    //                                                                           sectionItems = group.ToList()
        //    //                                                                       }).ToList();
        //    //                    foreach (var section in dependentSectionContext)
        //    //                    {
        //    //                        var sectionDto = new SectionQuestionsViewDto
        //    //                        {
        //    //                            SectionId = section.Key,
        //    //                            SectionName = section.SectionName
        //    //                        };
        //    //                        foreach (var question in section.sectionItems)
        //    //                        {
        //    //                            var questionsViewDto = new QuestionsViewDto
        //    //                            {
        //    //                                QuestionTypeId = question.Question.QuestionTypeId,
        //    //                                QuestionTypeName = question.Question.QuestionType.Name,
        //    //                                Query = question.Question.Query,
        //    //                                QuestionId = question.Question.Id,
        //    //                                OrderNo = question.OrderNo,
        //    //                                IsDeleted = question.IsDeleted,
        //    //                                Description = question.Question.Description,
        //    //                                IsMandatory = question.Question.IsMandatory,
        //    //                                ModifiedDate = question.Question.ModifiedDate
        //    //                            };
        //    //                            if (question.Question.QuestionTypeId == (int)DTO.Enums.QuestionType.MultipleChoice || question.Question.QuestionTypeId == (int)DTO.Enums.QuestionType.SingleChoice)
        //    //                            {
        //    //                                if (question.Question.AnswerOptions.Where(_ => !_.IsDeleted).Any())
        //    //                                {
        //    //                                    foreach (var answerOption in question.Question.AnswerOptions.Where(_ => !_.IsDeleted).ToList())
        //    //                                    {
        //    //                                        var answerOptionDto = new AnswerOptionDto
        //    //                                        {
        //    //                                            AnswerOptionId = answerOption.Id,
        //    //                                            Option = answerOption.Option
        //    //                                        };
        //    //                                        questionsViewDto.AnswerOptions.Add(answerOptionDto);
        //    //                                    }
        //    //                                }
        //    //                            }
        //    //                            sectionDto.Questions.Add(questionsViewDto);
        //    //                        }
        //    //                        dependentForm.SectionQuestions.Add(sectionDto);
        //    //                    }
        //    //                    demoViewDto.DependentFormsMaster.Add(dependentForm);
        //    //                }
        //    //            }
        //    //        }

        //    //        //Add Submitted Dependent form details
        //    //        var submittedDependentFormContext = _emamiContext.SubmittedForms.AsNoTracking().Where(_ => _.ParentFormId == submittedFormContext.Id).ToList();
        //    //        if (submittedDependentFormContext.Any())
        //    //        {
        //    //            foreach (var form in submittedDependentFormContext)
        //    //            {
        //    //                var dependentForm = new SubmittedDependentFormDto
        //    //                {
        //    //                    SubmittedFormId = form.Id,
        //    //                    FormId = form.FormId,
        //    //                    FormName = form.FormName,
        //    //                    CreatedDate = form.CreatedDate,
        //    //                    DemonstratedBy = GetUserName(form.DemoUserId ?? 0),
        //    //                    DemoId = form.DemoId ?? 0
        //    //                };
        //    //                if (form.SubmittedFormQuestions.Any())
        //    //                {
        //    //                    var dependentSectionContext = form.SubmittedFormQuestions.GroupBy(_ => _.SectionId)
        //    //                                                                       .Select(group => new
        //    //                                                                       {
        //    //                                                                           group.Key,
        //    //                                                                           group.FirstOrDefault().SectionName,
        //    //                                                                           sectionItems = group.ToList()
        //    //                                                                       }).ToList();
        //    //                    foreach (var section in dependentSectionContext)
        //    //                    {
        //    //                        var sectionDto = new SectionDto
        //    //                        {
        //    //                            SectionId = section.Key,
        //    //                            SectionName = section.SectionName
        //    //                        };
        //    //                        foreach (var question in section.sectionItems)
        //    //                        {
        //    //                            var submittedQuestionViewDto = new SubmittedFormQuestionViewDto
        //    //                            {
        //    //                                QuestionId = question.QuestionId,
        //    //                                QuestionTypeId = question.QuestionTypeId,
        //    //                                Question = question.Query,
        //    //                                QuestionTypeName = question.QuestionTypeName
        //    //                            };
        //    //                            if (question.Answers.Any())
        //    //                            {
        //    //                                foreach (var answer in question.Answers.ToList())
        //    //                                {
        //    //                                    if (answer.IsYes != null)
        //    //                                    {
        //    //                                        submittedQuestionViewDto.YesNo = new SubmittedYesNoAnswerViewDto
        //    //                                        {
        //    //                                            IsYes = Convert.ToBoolean(answer.IsYes)
        //    //                                        };
        //    //                                        break;
        //    //                                    }
        //    //                                    else if (!string.IsNullOrEmpty(answer.TextAnswer))
        //    //                                    {
        //    //                                        submittedQuestionViewDto.TextAnswer = new SubmittedTextAnswerViewDto
        //    //                                        {
        //    //                                            TextAnswer = answer.TextAnswer
        //    //                                        };
        //    //                                        break;
        //    //                                    }
        //    //                                    else if (!string.IsNullOrEmpty(answer.AttachmentFileName))
        //    //                                    {
        //    //                                        var submittedAttachmentDto = new SubmittedAttachmentViewDto
        //    //                                        {
        //    //                                            FileName = answer.AttachmentFileName,
        //    //                                            MediaTypeId = answer.MediaTypeId ?? 0
        //    //                                        };
        //    //                                        submittedQuestionViewDto.Attachments.Add(submittedAttachmentDto);
        //    //                                    }
        //    //                                    else
        //    //                                    {
        //    //                                        var inspectionMultiAnswerViewDto = new SubmittedFormMultiAnswerViewDto
        //    //                                        {
        //    //                                            AnswerOptionId = answer.AnswerOptionId ?? 0,
        //    //                                            Option = answer.Option,
        //    //                                            IsSelected = answer.IsSelected,
        //    //                                        };
        //    //                                        submittedQuestionViewDto.AnswerOptions.Add(inspectionMultiAnswerViewDto);
        //    //                                    }
        //    //                                }
        //    //                            }
        //    //                            sectionDto.Questions.Add(submittedQuestionViewDto);
        //    //                        }
        //    //                        dependentForm.Sections.Add(sectionDto);
        //    //                    }
        //    //                    demoViewDto.DependentForms.Add(dependentForm);
        //    //                }
        //    //            }
        //    //        }
        //    //    }
        //    //}

        //    /// <summary>
        //    /// Get Active Customer Id and Name to load Dropdown in Mobile app
        //    /// User table used
        //    /// IsCustomer denoted Customer User
        //    /// </summary>
        //    /// <returns>List of Customers in DropdownDto model</returns>
        //    public ResultDto GetActiveCustomerList()
        //    {
        //        _methodName = "GetActiveCustomerList";
        //        var CustomerList = new List<DropDownDto>();
        //        try
        //        {
        //            CustomerList = _emamiContext.Users.AsNoTracking()
        //                .Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
        //                .Where(_ => /*_.IsCustomer &&*/ _.u.IsActive && _.ur.RoleId == (int)(int)DTO.Enums.Role.Dealer)
        //                                                                .Select(c => new DropDownDto
        //                                                                {
        //                                                                    Id = c.u.Id,
        //                                                                    Name = c.u.Name,
        //                                                                }).ToList();
        //            return _resultService.SuccessObject(CustomerList);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }

        //    public ResultDto GetSubmittedFormsPlantList(LoginUserIdDto loginUserIdDto)
        //    {
        //        //if login user is demo user, submitted understanding form's complaint form list plant should be displayed
        //        _methodName = "GetSubmittedFormsPlantList";
        //        var plantList = new List<DepotDto>();
        //        try
        //        {
        //            var SubmittedFormContext = _emamiContext.ScheduleDemoUsers.AsNoTracking().Where(_ => _.DemoInchargeId == loginUserIdDto.LoginUserId || _.DemoUserId == loginUserIdDto.LoginUserId).Select(_ => _.SubmittedFormId).ToList();
        //            plantList = _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                                .Where(_ => _.CreatedBy == loginUserIdDto.LoginUserId || SubmittedFormContext.Contains(_.SubmittedFormId))
        //                                                                .Distinct()
        //                                                                .GroupBy(_ => _.PlantId)
        //                                                                .ToList()
        //                                                                .Select(group => new DepotDto
        //                                                                {
        //                                                                    Id = group.Key,
        //                                                                    Name = GetPlantName(group.Key)
        //                                                                }).ToList();
        //            return _resultService.SuccessObject(plantList);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }

        //    public ResultDto GetSubmittedFormsStateCityList(LoginUserIdDto loginUserIdDto)
        //    {
        //        _methodName = "GetSubmittedFormsStateCityList";
        //        var resultDto = new ResultDto();
        //        var stateDto = new List<StateDto>();
        //        try
        //        {
        //            var SubmittedFormContext = _emamiContext.ScheduleDemoUsers.AsNoTracking().Where(_ => _.DemoInchargeId == loginUserIdDto.LoginUserId || _.DemoUserId == loginUserIdDto.LoginUserId).Select(_ => _.SubmittedFormId).ToList();
        //            stateDto = _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                        .Where(_ => _.CreatedBy == loginUserIdDto.LoginUserId || SubmittedFormContext.Contains(_.SubmittedFormId))
        //                                                        .Distinct()
        //                                                        .GroupBy(_ => _.StateId)
        //                                                        .Select(group => new StateDto
        //                                                        {
        //                                                            StateId = group.Key,
        //                                                            StateName = group.FirstOrDefault().State.StateName,
        //                                                            Cities = group.ToList().Select(c => new CityDto
        //                                                            {
        //                                                                CityId = c.CityId,
        //                                                                CityName = c.City.CityName
        //                                                            }).Distinct().ToList()
        //                                                        }).ToList();

        //            resultDto.IsSuccess = true;
        //            resultDto.SuccessDto.Response = stateDto;
        //            return resultDto;
        //        }
        //        catch (Exception exception)
        //        {
        //            var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.Exception;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
        //            _logger.Error(message);
        //            return resultDto;
        //        }
        //    }

        //    public ResultDto GetCMSSEUsersList()
        //    {
        //        _methodName = "GetCMSSEUsersList";
        //        var resultDto = new ResultDto();
        //        var userMasterDto = new List<UserMasterDto>();
        //        try
        //        {
        //            var userList = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role_CMS.SalesExecutive).Select(_ => _.User);
        //            userMasterDto = userList.AsNoTracking().Where(_ => _.IsActive)
        //                                                        .ToList()
        //                                                        .Select(c => new UserMasterDto
        //                                                        {
        //                                                            Id = c.Id,
        //                                                            EmployeeCode = c.Code,
        //                                                            EmployeeName = c.Name,
        //                                                            Branch = c.Branch,
        //                                                            VerticalId = c.DivisionId,
        //                                                            Vertical = c.Division != null ? c.Division.Name : string.Empty,
        //                                                            CMSReportingToId = c.CMSReportingToId,
        //                                                            RoleName = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == c.Id).Role.Name
        //                                                        }).ToList();

        //            resultDto.IsSuccess = true;
        //            resultDto.SuccessDto.Response = userMasterDto != null ? userMasterDto.OrderByDescending(_ => _.Id).ToList() : userMasterDto;
        //            return resultDto;
        //        }
        //        catch (Exception exception)
        //        {
        //            var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.Exception;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
        //            _logger.Error(message);
        //            return resultDto;
        //        }
        //    }


        //    public ResultDto GetSubmittedFormListAndDemoDetailsByUserIdForManager(SubmittedFormsInputDto submittedFormsInputDto)
        //    {
        //        _methodName = "GetSubmittedFormListAndDemoDetailsByUserIdForManager";
        //        var submittedFormListDto = new SubmittedFormsPaginationOutputDto();
        //        var UsersReportingTo = new List<long>();
        //        try
        //        {
        //            if (submittedFormsInputDto == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            if (submittedFormsInputDto.LoginUserId == 0)
        //            {
        //                return _resultService.ErrorMessage(Constants.UserIdMissing);
        //            }
        //            if (!_resultService.UserIsAcive(submittedFormsInputDto.LoginUserId))
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidUser);
        //            }
        //            var userRole = GetUserRoleId(submittedFormsInputDto.LoginUserId);
        //            if (userRole == 0)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidUser);
        //            }
        //            var submittedFormContext = new List<SubmittedForm>();

        //            var UserRoleClaims = _emamiContext.RoleClaims.AsNoTracking().Where(_ => _.RoleId == userRole).Select(_ => _.ClaimId);
        //            if (UserRoleClaims == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.ClaimEmpty);
        //            }
        //            if (UserRoleClaims.Contains((int)Claims.ViewComplaints))
        //            {
        //                var UserContext = _emamiContext.Users.AsNoTracking();

        //                if (userRole == (int)DTO.Enums.Role.NationalTrader)
        //                {
        //                    var ZonalHeadIds = UserContext.Where(_ => _.OrganizationReportingToId == submittedFormsInputDto.LoginUserId).Select(a => a.Id).ToList();
        //                    var BdoIds = UserContext.Where(_ => ZonalHeadIds.Contains(_.OrganizationReportingToId.Value)).Select(a => a.Id).ToList();
        //                    UsersReportingTo.Add(submittedFormsInputDto.LoginUserId);
        //                    UsersReportingTo.AddRange(ZonalHeadIds);
        //                    UsersReportingTo.AddRange(BdoIds);
        //                }
        //                else if (userRole == (int)DTO.Enums.Role.ZonalTrader)
        //                {
        //                    var BdoIds = UserContext.Where(_ => _.OrganizationReportingToId == submittedFormsInputDto.LoginUserId).Select(a => a.Id).ToList();
        //                    UsersReportingTo.Add(submittedFormsInputDto.LoginUserId);
        //                    UsersReportingTo.AddRange(BdoIds);
        //                }

        //                submittedFormContext = _emamiContext.SubmittedForms.AsNoTracking()
        //                                                                   .Where(_ => _.ParentFormId == null || _.ParentFormId == 0 && UsersReportingTo.Contains(_.CreatedBy))
        //                                                                   .OrderByDescending(_ => _.CreatedDate).ToList();
        //            }
        //            if (submittedFormContext.Any())
        //            {
        //                //Applying Sorting filters
        //                if (submittedFormsInputDto.CustomerId != 0)
        //                {
        //                    var customerFilteredContext = submittedFormContext.Where(_ => _.UserId == submittedFormsInputDto.CustomerId)
        //                                                                      .OrderByDescending(_ => _.CreatedDate).ToList();
        //                    submittedFormContext.Clear();
        //                    submittedFormContext.AddRange(customerFilteredContext);
        //                }
        //                if (submittedFormsInputDto.DealerId != 0)
        //                {
        //                    var DealerFilteredContext = submittedFormContext.Where(_ => _.DealerId == submittedFormsInputDto.DealerId)
        //                                                                      .OrderByDescending(_ => _.CreatedDate).ToList();
        //                    submittedFormContext.Clear();
        //                    submittedFormContext.AddRange(DealerFilteredContext);
        //                }
        //                if (submittedFormsInputDto.SubmittedDate != null && submittedFormsInputDto.SubmittedDate != DateTime.MinValue)
        //                {
        //                    var dateFilteredContext = submittedFormContext.Where(_ => _.CreatedDate.Date == submittedFormsInputDto.SubmittedDate.Value.Date)
        //                                                                  .OrderByDescending(_ => _.CreatedDate).ToList();
        //                    submittedFormContext.Clear();
        //                    submittedFormContext.AddRange(dateFilteredContext);
        //                }
        //                if (submittedFormsInputDto.EmployeeId != 0)
        //                {
        //                    var employeeFilteredContext = submittedFormContext.Where(_ => _.CreatedBy == submittedFormsInputDto.EmployeeId)
        //                                                                      .OrderByDescending(_ => _.CreatedDate).ToList();
        //                    submittedFormContext.Clear();
        //                    submittedFormContext.AddRange(employeeFilteredContext);
        //                }
        //                if (submittedFormsInputDto.StatusId != 0)
        //                {
        //                    var statusFilteredContext = submittedFormContext.Where(_ => _.FormStatusId == submittedFormsInputDto.StatusId)
        //                                                                    .OrderByDescending(_ => _.CreatedDate).ToList();
        //                    submittedFormContext.Clear();
        //                    submittedFormContext.AddRange(statusFilteredContext);
        //                }
        //                if (submittedFormsInputDto.SkuId != 0)
        //                {
        //                    var submittedFormIds = submittedFormContext.Select(sf => sf.Id).ToList();
        //                    var skuFilteredContext = _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                                    .Where(_ => _.SkuId == submittedFormsInputDto.SkuId &&
        //                                                                    submittedFormIds.Contains(_.SubmittedFormId))
        //                                                                    .Select(_ => _.SubmittedForm)
        //                                                                    .OrderByDescending(_ => _.CreatedDate)
        //                                                                    .ToList();
        //                    submittedFormContext.Clear();
        //                    submittedFormContext.AddRange(skuFilteredContext);

        //                }
        //                if (submittedFormsInputDto.PlantId != 0)
        //                {
        //                    var submittedFormIds = submittedFormContext.Select(sf => sf.Id).ToList();
        //                    var plantFilteredContext = _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                            .Where(_ => _.PlantId == submittedFormsInputDto.PlantId &&
        //                                                            submittedFormIds.Contains(_.SubmittedFormId))
        //                                                            .Select(_ => _.SubmittedForm)
        //                                                            .OrderByDescending(_ => _.CreatedDate)
        //                                                            .ToList();
        //                    submittedFormContext.Clear();
        //                    submittedFormContext.AddRange(plantFilteredContext);
        //                }
        //                if (submittedFormsInputDto.StateId != 0)
        //                {
        //                    var submittedFormIds = submittedFormContext.Select(sf => sf.Id).ToList();
        //                    var stateFilteredContext = _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                            .Where(_ => _.StateId == submittedFormsInputDto.StateId &&
        //                                                            submittedFormIds.Contains(_.SubmittedFormId))
        //                                                            .Select(_ => _.SubmittedForm)
        //                                                            .OrderByDescending(_ => _.CreatedDate)
        //                                                            .ToList();
        //                    submittedFormContext.Clear();
        //                    submittedFormContext.AddRange(stateFilteredContext);
        //                }
        //                if (submittedFormsInputDto.CityId != 0)
        //                {
        //                    var submittedFormIds = submittedFormContext.Select(sf => sf.Id).ToList();
        //                    var cityFilteredContext = _emamiContext.SubmittedFormDetails.AsNoTracking()
        //                                                            .Where(_ => _.CityId == submittedFormsInputDto.CityId &&
        //                                                            submittedFormIds.Contains(_.SubmittedFormId))
        //                                                            .Select(_ => _.SubmittedForm)
        //                                                            .OrderByDescending(_ => _.CreatedDate)
        //                                                            .ToList();
        //                    submittedFormContext.Clear();
        //                    submittedFormContext.AddRange(cityFilteredContext);
        //                }

        //                submittedFormListDto.TotalRecords = submittedFormContext.Count;
        //                //Pagination for output list
        //                if (submittedFormsInputDto.PageSize > 0 && submittedFormsInputDto.StartIndex >= 0 && submittedFormContext.Count > 0)
        //                {
        //                    int CurrentPage = submittedFormsInputDto.StartIndex;
        //                    int PageSize = submittedFormsInputDto.PageSize;
        //                    CurrentPage = Convert.ToInt32(Math.Floor(Convert.ToDecimal(CurrentPage / PageSize)));
        //                    CurrentPage = CurrentPage + 1;
        //                    if (submittedFormsInputDto.StartIndex >= submittedFormContext.Count)
        //                    {
        //                        return _resultService.ErrorMessage(Constants.StartIndexExceedTotalRecords);
        //                    }
        //                    //Apply pagination
        //                    submittedFormContext = submittedFormContext.OrderByDescending(_ => _.Id).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        //                }
        //                foreach (var submittedForm in submittedFormContext)
        //                {
        //                    var submittedViewDto = new SubmittedFormViewDto
        //                    {
        //                        SubmittedFormId = submittedForm.Id,
        //                        CreatedDate = submittedForm.CreatedDate,
        //                        FormId = submittedForm.FormId,
        //                        FormName = submittedForm.FormName,
        //                        ParentFormId = submittedForm.ParentFormId,
        //                        ParentFormName = submittedForm.ParentFormId != null && submittedForm.ParentFormId != 0 ? GetSubmittedFormName(submittedForm.ParentFormId ?? 0) : string.Empty,
        //                        FormStatusName = submittedForm.FormStatus != null ? submittedForm.FormStatus.Name : string.Empty,
        //                        DemonstratedBy = GetUserName(submittedForm.DemoUserId ?? 0),
        //                        RaisedFor = submittedForm.Retailer != null ? submittedForm.Retailer.AccountName : submittedForm.CustomerName,
        //                        FormApprovalStatusId = submittedForm.FormApprovalStatusId ?? 0,
        //                        FormStatusId = submittedForm.FormStatusId ?? 0,
        //                        Remarks = submittedForm.Remarks,
        //                        RaisedBy = GetUserName(submittedForm.CreatedBy),
        //                        CustomerId = submittedForm.UserId ?? 0,
        //                        DealerId = submittedForm.DealerId ?? 0,
        //                        DealerName = submittedForm.DealerName
        //                    };
        //                    //Add Submitted form details
        //                    var submittedformdetails = _emamiContext.SubmittedFormDetails.AsNoTracking().FirstOrDefault(_ => _.SubmittedFormId == submittedForm.Id);
        //                    if (submittedformdetails != null)
        //                    {
        //                        submittedViewDto.SkuId = submittedformdetails.SkuId;
        //                        submittedViewDto.SkuName = submittedformdetails.Sku != null ? submittedformdetails.Sku.SkuName : string.Empty;
        //                        submittedViewDto.PlantId = submittedformdetails.PlantId;
        //                        submittedViewDto.PlantName = GetPlantName(submittedformdetails.PlantId);
        //                        submittedViewDto.StateId = submittedformdetails.StateId;
        //                        submittedViewDto.StateName = submittedformdetails.State != null ? submittedformdetails.State.StateName : string.Empty;
        //                        submittedViewDto.CityId = submittedformdetails.CityId;
        //                        submittedViewDto.CityName = submittedformdetails.City != null ? submittedformdetails.City.CityName : string.Empty;
        //                    }

        //                    var demodetails = _emamiContext.ScheduleDemoUsers.AsNoTracking().Where(_ => _.SubmittedFormId == submittedForm.Id).ToList();
        //                    if (demodetails != null)
        //                    {
        //                        foreach (var demo in demodetails)
        //                        {
        //                            var submittedUnderstandingForms = _emamiContext.SubmittedForms.AsNoTracking().Where(_ => _.DemoId == demo.Id)
        //                                                                                                                 .Select(_ => _.Id).ToList();
        //                            var scheduleDemoEalUserIds = _emamiContext.ScheduleDemoUserMappings.AsNoTracking().Where(_ => _.DemoId == demo.Id).Select(_ => _.EALUserId).ToList();
        //                            var demoDto = new ScheduleDemoOutputDto
        //                            {
        //                                DemoId = demo.Id,
        //                                DemoCreatedBy = GetUserName(demo.CreatedBy),
        //                                DemoDateTime = demo.DemoDate,
        //                                DemonstratorName = demo.DemoUser != null ? demo.DemoUser.Name : string.Empty,
        //                                DemoInchargeName = GetUserName(demo.DemoInchargeId),
        //                                SalesExecutiveName = GetUserName(submittedForm.CreatedBy),
        //                                IsActive = demo.IsActive,
        //                                SubmittedUnderstandingForms = submittedUnderstandingForms,
        //                                UnderstandingFormId = demo.DependentMasterFormId,
        //                                ComplaintFormId = demo.SubmittedFormId,
        //                                ComplaintFormName = demo.SubmittedForm.FormName,
        //                                EALUserId = scheduleDemoEalUserIds,
        //                                DemoInchargeId = demo.DemoInchargeId,
        //                                DemonstratorId = demo.DemoUserId,
        //                                EALUserName = scheduleDemoEalUserIds != null ? GetEALUserName(demo.Id) : string.Empty,
        //                            };
        //                            submittedViewDto.DemoDetails.Add(demoDto);
        //                        }
        //                    }
        //                    GetSubmittedAndDependentMasterFormDetails(submittedForm, submittedViewDto);
        //                    submittedFormListDto.SubmittedFormViewList.Add(submittedViewDto);
        //                }
        //            }
        //            return _resultService.SuccessObject(submittedFormListDto);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }

        //    #endregion

        //    #region Web reports
        //    public ResultDto SubmittedFormReports(SubmittedFormsInputDto submittedFormsInputDto)
        //    {
        //        _methodName = "SubmittedFormReports";
        //        var submittedFormsDto = new List<SubmittedFormReportViewDto>();
        //        try
        //        {
        //            if (submittedFormsInputDto == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidRequest);
        //            }
        //            if (submittedFormsInputDto.FromDate == null || submittedFormsInputDto.FromDate == DateTime.MinValue)
        //            {
        //                return _resultService.ErrorMessage(Constants.FromDateEmpty);
        //            }
        //            if (submittedFormsInputDto.ToDate == null || submittedFormsInputDto.ToDate == DateTime.MinValue)
        //            {
        //                return _resultService.ErrorMessage(Constants.ToDateEmpty);
        //            }
        //            if (submittedFormsInputDto.FromDate > submittedFormsInputDto.ToDate)
        //            {
        //                return _resultService.ErrorMessage(Constants.FromDateInvalid);
        //            }
        //            if (submittedFormsInputDto.LoginUserId == 0)
        //            {
        //                return _resultService.ErrorMessage(Constants.UserIdMissing);
        //            }
        //            if (!_resultService.UserIsAcive(submittedFormsInputDto.LoginUserId))
        //            {
        //                return _resultService.ErrorMessage(Constants.InvalidUser);
        //            }
        //            var fromDate = submittedFormsInputDto.FromDate.Date.AddMilliseconds(1);
        //            var toDate = submittedFormsInputDto.ToDate.Date.AddDays(1).AddSeconds(-1);
        //            var submittedFormContext = new List<SubmittedForm>();
        //            //Complaint Forms                
        //            submittedFormContext = _emamiContext.SubmittedForms.AsNoTracking().Where(_ => _.CreatedDate >= fromDate && _.CreatedDate <= toDate && (_.ParentFormId == null || _.ParentFormId == 0)).OrderByDescending(_ => _.CreatedDate).ToList();

        //            if (submittedFormContext.Any())
        //            {
        //                if (!string.IsNullOrEmpty(submittedFormsInputDto.SearchText))
        //                {
        //                    var submittedFormSearchContext = new List<SubmittedForm>();
        //                    //Submitted Forms search
        //                    var submittedForms = submittedFormContext;
        //                    var formsContext = submittedForms.Where(a => (!string.IsNullOrEmpty(a.FormName) ? a.FormName.ToLower().Contains(submittedFormsInputDto.SearchText.ToLower()) : false) ||
        //                                                                 (a.FormStatus != null ? a.FormStatus.Name.ToLower().Contains(submittedFormsInputDto.SearchText.ToLower()) : false) ||
        //                                                                 (!string.IsNullOrEmpty(a.CustomerName) ? a.CustomerName.ToLower().Contains(submittedFormsInputDto.SearchText.ToLower()) : false) ||
        //                                                                 (!string.IsNullOrEmpty(a.Remarks) ? a.Remarks.ToLower().Contains(submittedFormsInputDto.SearchText.ToLower()) : false))
        //                                                     .AsEnumerable();
        //                    if (formsContext.Any())
        //                    {
        //                        submittedFormSearchContext.AddRange(formsContext);
        //                    }
        //                    //Submitted Forms questions search
        //                    var submittedFormQuestions = submittedFormContext;
        //                    foreach (var form in submittedFormQuestions)
        //                    {
        //                        var questionContext = form.SubmittedFormQuestions.Where(q => q.Query.ToLower().Contains(submittedFormsInputDto.SearchText.ToLower()) ||
        //                                                                                q.SectionName.ToLower().Contains(submittedFormsInputDto.SearchText.ToLower()) ||
        //                                                                                q.QuestionTypeName.ToLower().Contains(submittedFormsInputDto.SearchText.ToLower()))
        //                                                                         .AsEnumerable();
        //                        if (questionContext.Any())
        //                        {
        //                            submittedFormSearchContext.Add(form);
        //                        }
        //                    }
        //                    //Submitted Forms Answers search
        //                    var submittedFormQuestionAnswers = submittedFormContext;
        //                    foreach (var form in submittedFormQuestionAnswers)
        //                    {
        //                        foreach (var question in form.SubmittedFormQuestions.ToList())
        //                        {
        //                            if (question.Answers.Any())
        //                            {
        //                                var answerContext = question.Answers.Where(a => (!string.IsNullOrEmpty(a.TextAnswer) ? a.TextAnswer.ToLower().Contains(submittedFormsInputDto.SearchText.ToLower()) : false) ||
        //                                                                       (!string.IsNullOrEmpty(a.AttachmentFileName) ? a.AttachmentFileName.ToLower().Contains(submittedFormsInputDto.SearchText.ToLower()) : false) ||
        //                                                                       (!string.IsNullOrEmpty(a.Option) ? a.Option.ToLower().Contains(submittedFormsInputDto.SearchText.ToLower()) : false))
        //                                                                .AsEnumerable();
        //                                if (answerContext.Any())
        //                                {
        //                                    submittedFormSearchContext.Add(form);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    //Clear the forms context and add if search contains result
        //                    submittedFormContext.Clear();
        //                    if (submittedFormSearchContext.Any())
        //                    {
        //                        submittedFormContext.AddRange(submittedFormSearchContext);
        //                    }
        //                }
        //                foreach (var submittedForm in submittedFormContext)
        //                {
        //                    var submittedViewDto = new SubmittedFormReportViewDto
        //                    {
        //                        SubmittedFormId = submittedForm.Id,
        //                        CreatedDate = submittedForm.CreatedDate,
        //                        FormId = submittedForm.FormId,
        //                        FormName = submittedForm.FormName,
        //                        ParentFormId = submittedForm.ParentFormId,
        //                        ParentFormName = submittedForm.ParentFormId != null && submittedForm.ParentFormId != 0 ? GetSubmittedFormName(submittedForm.ParentFormId ?? 0) : string.Empty,
        //                        FormApprovalStatusName = (submittedForm.FormApprovalStatusId ?? 0) > 0 ? Utility.GetEnumDescription((DTO.Enums.Status)submittedForm.FormApprovalStatusId) : string.Empty,
        //                        FormStatusName = submittedForm.FormStatus != null ? submittedForm.FormStatus.Name : string.Empty,
        //                        DemonstratedBy = GetUserName(submittedForm.DemoUserId ?? 0),
        //                        RaisedFor = submittedForm.Retailer != null ? submittedForm.Retailer.AccountName : submittedForm.CustomerName,
        //                        DealerName = submittedForm.DealerName,
        //                        Remarks = submittedForm.Remarks ?? submittedForm.Remarks,
        //                        DistrictName = submittedForm.Retailer != null ? submittedForm.Retailer.District != null ? submittedForm.Retailer.District.DistrictName : string.Empty : string.Empty,
        //                        Address = submittedForm.Retailer != null ? submittedForm.Retailer.Address : string.Empty,
        //                        BakeryMaster = submittedForm.Retailer != null ? submittedForm.Retailer.ChefName : string.Empty,
        //                        BakeryMasterNumber = submittedForm.Retailer != null ? submittedForm.Retailer.ChefNumber : string.Empty,
        //                        BakeryOwnerName = submittedForm.Retailer != null ? submittedForm.Retailer.OwnersName : string.Empty,
        //                        BakeryOwnerNumber = submittedForm.Retailer != null ? submittedForm.Retailer.MobileNumber : string.Empty,
        //                    };
        //                    //Add Submitted form details
        //                    var submittedformdetails = _emamiContext.SubmittedFormDetails.AsNoTracking().FirstOrDefault(_ => _.SubmittedFormId == submittedForm.Id);
        //                    if (submittedformdetails != null)
        //                    {
        //                        submittedViewDto.SkuName = submittedformdetails.Sku != null ? submittedformdetails.Sku.SkuName : string.Empty;
        //                        submittedViewDto.PlantName = GetPlantName(submittedformdetails.PlantId);
        //                        submittedViewDto.StateName = submittedformdetails.State != null ? submittedformdetails.State.StateName : string.Empty;
        //                        submittedViewDto.CityName = submittedformdetails.City != null ? submittedformdetails.City.CityName : string.Empty;
        //                    }

        //                    if (submittedForm.SubmittedFormQuestions.Any())
        //                    {
        //                        submittedViewDto.Questions = GetSubmittedFormQuestions(submittedForm);
        //                    }

        //                    //Add Dependent forms and question details
        //                    var dependentFormsContext = _emamiContext.SubmittedForms.AsNoTracking().Where(_ => _.ParentFormId == submittedForm.Id).OrderByDescending(_ => _.CreatedDate).ToList();
        //                    foreach (var dependentform in dependentFormsContext)
        //                    {
        //                        var dependentFormViewDto = new SubmittedFormShortViewDto
        //                        {
        //                            SubmittedDependentFormId = dependentform.Id,
        //                            CreatedDate = dependentform.CreatedDate,
        //                            FormId = dependentform.FormId,
        //                            FormName = dependentform.FormName,
        //                            DemonstratedBy = GetDemonstratorName(dependentform.DemoId ?? 0),
        //                            DemoInchargeName = GetDemoInchargeName(dependentform.DemoId ?? 0),
        //                            EALUserName = GetEALUserName(dependentform.DemoId ?? 0)
        //                        };
        //                        if (dependentform.SubmittedFormQuestions.Any())
        //                        {
        //                            dependentFormViewDto.Questions = GetSubmittedFormQuestions(dependentform);
        //                        }
        //                        submittedViewDto.DependentFormsList.Add(dependentFormViewDto);
        //                    }

        //                    submittedFormsDto.Add(submittedViewDto);
        //                }
        //            }
        //            return _resultService.SuccessObject(submittedFormsDto);
        //        }
        //        catch (Exception exception)
        //        {
        //            return ExceptionResult(exception);
        //        }
        //    }

        //    private static List<SubmittedFormReportQuestionsViewDto> GetSubmittedFormQuestions(SubmittedForm submittedForm)
        //    {
        //        List<SubmittedFormReportQuestionsViewDto> Questions = new List<SubmittedFormReportQuestionsViewDto>();

        //        var groupedSectionContext = submittedForm.SubmittedFormQuestions.GroupBy(_ => _.SectionId)
        //                                                                                                   .Select(group => new
        //                                                                                                   {
        //                                                                                                       group.Key,
        //                                                                                                       group.FirstOrDefault().SectionName,
        //                                                                                                       sectionItems = group.ToList()
        //                                                                                                   }).ToList();
        //        foreach (var section in groupedSectionContext)
        //        {
        //            foreach (var question in section.sectionItems)
        //            {
        //                var questionsDto = new SubmittedFormReportQuestionsViewDto
        //                {
        //                    SectionId = section.Key,
        //                    SectionName = section.SectionName
        //                };
        //                questionsDto.QuestionId = question.QuestionId;
        //                questionsDto.QuestionTypeId = question.QuestionTypeId;
        //                questionsDto.Question = question.Query;
        //                questionsDto.QuestionTypeName = question.QuestionTypeName;

        //                if (question.Answers.Any())
        //                {
        //                    foreach (var answer in question.Answers.ToList())
        //                    {
        //                        if (answer.IsYes != null)
        //                        {
        //                            questionsDto.Answer = answer.IsYes ?? false ? Constants.Yes : Constants.No;
        //                            break;
        //                        }
        //                        else if (!string.IsNullOrEmpty(answer.TextAnswer))
        //                        {
        //                            questionsDto.Answer = answer.TextAnswer;
        //                            break;
        //                        }
        //                        else if (!string.IsNullOrEmpty(answer.AttachmentFileName))
        //                        {
        //                            if (string.IsNullOrEmpty(questionsDto.Answer))
        //                                questionsDto.Answer = answer.AttachmentFileName + " - " + answer.MediaTypeId;
        //                            else
        //                                questionsDto.Answer += "," + answer.AttachmentFileName + " - " + answer.MediaTypeId;
        //                        }
        //                        else
        //                        {
        //                            if (answer.IsSelected ?? false)
        //                            {
        //                                if (string.IsNullOrEmpty(questionsDto.Answer))
        //                                    questionsDto.Answer = answer.Option;
        //                                else
        //                                    questionsDto.Answer += " - " + answer.Option;
        //                            }
        //                        }
        //                    }
        //                    Questions.Add(questionsDto);
        //                }
        //            }
        //        }
        //        return Questions;
        //    }
        //    #endregion

        //    #region Common methods

        private string GetFormName(long formId)
        {
            var formContext = _emamiContext.Forms.AsNoTracking().FirstOrDefault(_ => _.Id == formId);
            if (formContext != null)
            {
                return formContext.Name;
            }
            return string.Empty;
        }

        private string GetSubmittedFormName(long formId)
        {
            var formContext = _emamiContext.SubmittedForms.AsNoTracking().FirstOrDefault(_ => _.Id == formId);
            if (formContext != null)
            {
                return formContext.FormName;
            }
            return string.Empty;
        }

        private string GetUserName(long userId)
        {
            var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == userId);
            if (userContext != null)
            {
                return userContext.Name;
            }
            return string.Empty;
        }

        //    private long GetUserRoleId(long userId)
        //    {
        //        var userContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userId);
        //        if (userContext != null)
        //        {
        //            return userContext.RoleId;
        //        }
        //        return 0;
        //    }

        private string GetPlantName(long plantId)
        {
            var plantContext = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == plantId);
            if (plantContext != null)
            {
                return plantContext.Name;
            }
            return string.Empty;
        }

        //    private string GetQuestionDescription(long questionId)
        //    {
        //        var questionMasterContext = _emamiContext.QuestionMasters.AsNoTracking().FirstOrDefault(_ => _.Id == questionId);
        //        if (questionMasterContext != null)
        //        {
        //            return questionMasterContext.Description;
        //        }
        //        return string.Empty;
        //    }

        //    private bool GetQuestionMandatoryValue(long questionId)
        //    {
        //        var questionMasterContext = _emamiContext.QuestionMasters.AsNoTracking().FirstOrDefault(_ => _.Id == questionId);
        //        if (questionMasterContext != null)
        //        {
        //            return questionMasterContext.IsMandatory;
        //        }
        //        return false;
        //    }

        private string GetDemoInchargeName(long demoId)
        {
            var demoContext = _emamiContext.ScheduleDemoUsers.AsNoTracking().FirstOrDefault(_ => _.Id == demoId);
            if (demoContext != null)
            {
                return GetUserName(demoContext.DemoInchargeId);
            }
            return string.Empty;
        }

        private string GetDemonstratorName(long demoId)
        {
            var demoContext = _emamiContext.ScheduleDemoUsers.AsNoTracking().FirstOrDefault(_ => _.Id == demoId);
            if (demoContext != null)
            {
                return GetUserName(demoContext.DemoUserId);
            }
            return string.Empty;
        }
        private string GetEALUserName(long demoId)
        {
            var demoContext = _emamiContext.ScheduleDemoUserMappings.AsNoTracking().Where(_ => _.DemoId == demoId);
            if (demoContext != null)
            {
                var EALUserSplit = demoContext.Select(a => a.EALUserId).ToList();
                var UsernameList = new List<string>();
                foreach (long id in EALUserSplit)
                {
                    var Username = GetUserName(id);
                    if (!string.IsNullOrEmpty(Username))
                    {
                        UsernameList.Add(Username);
                    }
                }
                return string.Join(",", UsernameList);
            }
            return string.Empty;
        }

        private ResultDto ExceptionResult(Exception exception)
        {
            var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
            var resultDto = new ResultDto();
            resultDto.IsSuccess = false;
            resultDto.ErrorDto.ErrorCode = Constants.Exception;
            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
            _logger.Error(message);
            return resultDto;
        }
        private ResultDto SucessResult(Object obj)
        {
            var resultDto = new ResultDto();
            resultDto.IsSuccess = true;
            resultDto.SuccessDto.Response = obj;
            return resultDto;
        }


        // #endregion
    }
}