using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMCore.Helper;
using System.Web;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.IO;
using System.Configuration;
using Adani.Solution.DTO.Enums;
using System.Globalization;

namespace Adani.Solution.Service
{
    public interface ISupportService
    {
        #region Mobile
        ResultDto GetCategoriesForSupport(LoginUserIdDto inputDto);
        ResultDto AddSupportMobile(SupportAddInputDto inputDto);
        #endregion

        #region Web

        ResultDto IssueRegisterForWeb(IssueRegisterDto inputDto);
        ResultDto GetIssueListWithPaging(SupportFilterInputDto inputDto);
        ResultDto GetIssueDetailsBySupportId(IssueDetailInputDto inputDto);
        ResultDto UpdateSupportIssueStatus(IssueStatusUpdateDto inputDto);
        ResultDto GetIssueCommentsList(long supportId);
        ResultDto GetIssueListWithCmts(SupportFilterInputDto inputDto);
        ResultDto ExportSupportIssues(SupportFilterInputDto inputDto);
        ResultDto GetFeatureList();
        ResultDto GetQueryFromList();

        #endregion
    }

    public class SupportService : ISupportService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Support Service");
        private const string ServiceName = "Support Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;

        public SupportService(IAdaniContext emamiContext, IResultService resultService, INotificationService notificationService)
        {
            try
            {
                _emamiContext = emamiContext;
                _resultService = resultService;
                _notificationService = notificationService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for Support Service", exception);
            }
        }

        #region Mobile

        public ResultDto GetCategoriesForSupport(LoginUserIdDto inputDto)
        {
            _methodName = "GetCategoriesForSupport";
            var SupportCategoryDto = new SupportCategoryDto();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                SupportCategoryDto.IssueTypes = UtilityHelper.EnumToList<DTO.Enums.Device>().Select(item => new DropDownDto()
                {
                    Name = UtilityHelper.GetEnumDescription(item),
                    Id = (int)item
                })/*.OrderBy(n => n.Name)*/.ToList();
                SupportCategoryDto.SeverityTypes = UtilityHelper.EnumToList<DTO.Enums.SeverityType>().Select(item => new DropDownDto()
                {
                    Name = UtilityHelper.GetEnumDescription(item),
                    Id = (int)item
                })/*.OrderBy(n => n.Name)*/.ToList();
                var SupportFeatures = _emamiContext.Configurations.AsNoTracking().Where(_ => _.Isactive && _.Id == (int)DTO.Enums.Configuration.SupportFeatures)
                                   .Select(_ => new DropDownDto
                                   {
                                       Id = _.Id,
                                       Name = _.Value
                                   }).FirstOrDefault();

                if (SupportFeatures != null)
                {
                    string[] components = SupportFeatures.Name.Split(',');

                    for (int i = 0; i < components.Length; i++)
                    {
                        SupportCategoryDto.Modules.Add(new DropDownDto { Id = i + 1, Name = components[i] });

                    }
                }

                return _resultService.SuccessObject(SupportCategoryDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto AddSupportMobile(SupportAddInputDto inputDto)
        {
            _methodName = "AddSupportMobile";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (string.IsNullOrEmpty(inputDto.Description))
                {
                    return _resultService.ErrorMessage(Constants.SupportDescriptionMissing);
                }
                if (inputDto.ComponentId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.ComponentMissing);
                }
                if (inputDto.FeatureId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.ModuleMissing);
                }
                if (inputDto.ImpactId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.SeverityMissing);
                }

                var supportContext = new Support()
                {
                    Description = inputDto.Description,
                    //IssueTypeId = inputDto.ComponentId,
                    SeverityTypeId = inputDto.ImpactId,
                    ModuleId = inputDto.FeatureId,
                    Feature=inputDto.Feature,
                    StatusId = (int)DTO.Enums.SupportStatus.Open,
                    //DeviceId = (int)DTO.Enums.Device.App,
                    DeviceId = inputDto.ComponentId,
                    StateId = userContext.StateId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    CreatedBy = userContext.Id,
                };
                _emamiContext.Supports.Add(supportContext);
                _emamiContext.SaveChanges();

                string folderName = DTO.Enums.PageType.Support.ToString();
                string mediaPath = Path.Combine(ConfigurationManager.AppSettings["UploadAttachments"], folderName);
                foreach (var attachment in inputDto.Attachments)
                {
                    var supportAttachmentContext = new SupportAttachment()
                    {
                        SupportId = supportContext.Id,
                        FileName = attachment,
                        MediaPath = Path.Combine(mediaPath, attachment),
                        MediaTypeId = (int)DTO.Enums.MediaType.Image,
                        CreatedBy = userContext.Id,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    };
                    _emamiContext.SupportAttachments.Add(supportAttachmentContext);
                }
                _emamiContext.SaveChanges();

                var configMailIds = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.SupportEmail);

                var amazonNotificationService = new AmazonNotificationService();

                if (_resultService.IsEmail())
                {
                    var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SupportIssueSubmittedEmail);
                    if (emailTemplate != null && !string.IsNullOrEmpty(userContext.Email))
                    {
                        var toUser = new List<string>();
                        string[] mailIds = configMailIds.Value.Split(',');

                        for (int i = 0; i < mailIds.Length; i++)
                        {
                            toUser.Add(mailIds[i]);
                        }
                        StringBuilder sb = new StringBuilder();
                        var supportEmailContext = _emamiContext.Supports.AsNoTracking().FirstOrDefault(_ => _.Id == supportContext.Id);
                        sb.Append("<tr><td width=50% ><b>Feature</b></td><td width=50%> " + supportEmailContext.Feature + "</td></tr><tr><td width=50%><b>Component</b></td><td width=50%>" + UtilityHelper.GetEnumDescription((DTO.Enums.Device)supportEmailContext.DeviceId) + "</td></tr><tr><td width=50%><b>Impact</b></td><td width=50%>" + UtilityHelper.GetEnumDescription((DTO.Enums.SeverityType)supportEmailContext.SeverityTypeId) + "</td></tr><tr><td width=50%><b> Description </b></td><td width=50%>" + supportEmailContext.Description + "</td></tr><tr><td width=50%><b> Created By </b></td><td width = 50%> " + userContext.Name + "</td></tr>)");
                        var replaceEmailTemplate = emailTemplate.PlainTemplate.Replace(Constants.UserName, userContext.Name).Replace(Constants.Message, sb.ToString());
                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, replaceEmailTemplate);
                        toUser.Add(userContext.Email.Trim());
                        amazonNotificationService.SendEmail(toUser, Constants.SupportIssueSubmittedSubject, string.Empty, htmlTemplate, true);
                    }
                }

                return _resultService.SuccessMessage(Constants.SupportSavedSuccess);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region Support - Web

        public ResultDto IssueRegisterForWeb(IssueRegisterDto inputDto)
        {
            _methodName = "IssueRegisterForWeb";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (string.IsNullOrEmpty(inputDto.Description))
                {
                    return _resultService.ErrorMessage(Constants.SupportDescriptionMissing);
                }
                if (inputDto.ComponentId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.ComponentMissing);
                }
                //if (inputDto.ComponentId <= 0)
                //{
                //    return _resultService.ErrorMessage(Constants.IssueTypeMissing);
                //}
                if (inputDto.FeatureId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.ModuleMissing);
                }
                if (inputDto.ImpactId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.SeverityMissing);
                }

                string folderName = DTO.Enums.PageType.Support.ToString();
                var mediaFileItemList = new List<SupportAttachmentDto>();
                foreach (var attachment in inputDto.Attachments)
                {
                    string ImagePath = string.Empty;
                    if (attachment != null)
                    {
                        var directory = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(@"~/" + ConfigurationManager.AppSettings["UploadAttachments"]), folderName);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        var ext = Path.GetExtension(attachment.FileName);
                        attachment.FileName = Guid.NewGuid() + ext;
                        var filename = Path.Combine(directory, attachment.FileName);
                        string mediaPath = Path.Combine(ConfigurationManager.AppSettings["UploadAttachments"], folderName);
                        attachment.MediaPath = Path.Combine(mediaPath, attachment.FileName);

                        //Deletion exists file  
                        if (File.Exists(filename))
                        {
                            File.Delete(filename);
                        }
                        File.WriteAllBytes(filename, attachment.FileByteArray);
                    }
                }
                var supportContext = new Support()
                {
                    Description = inputDto.Description,

                    //IssueTypeId = inputDto.ComponentId,
                    SeverityTypeId = inputDto.ImpactId,
                    ModuleId = inputDto.FeatureId,
                    Feature=inputDto.Feature,
                    StatusId = (int)DTO.Enums.SupportStatus.Open,
                    //DeviceId = (int)DTO.Enums.Device.Portal,
                    DeviceId = inputDto.ComponentId,
                    StateId = userContext.StateId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    CreatedBy = inputDto.LoginUserId,
                };
                _emamiContext.Supports.Add(supportContext);
                _emamiContext.SaveChanges();

                foreach (var attachment in inputDto.Attachments)
                {
                    var supportAttachmentContext = new SupportAttachment()
                    {
                        SupportId = supportContext.Id,
                        FileName = attachment.FileName,
                        MediaPath = attachment.MediaPath,
                        MediaTypeId = attachment.MediaTypeId,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    };
                    _emamiContext.SupportAttachments.Add(supportAttachmentContext);
                }
                _emamiContext.SaveChanges();

                var configMailIds = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_=>_.Id == (int)DTO.Enums.Configuration.SupportEmail);

                var amazonNotificationService = new AmazonNotificationService();

                if (_resultService.IsEmail())
                {
                    var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SupportIssueSubmittedEmail);
                    if (emailTemplate != null && !string.IsNullOrEmpty(userContext.Email))
                    {
                        var toUser = new List<string>();
                        string[] mailIds = configMailIds.Value.Split(',');

                        for (int i = 0; i < mailIds.Length; i++)
                        {
                            toUser.Add(mailIds[i]);
                        }
                        StringBuilder sb = new StringBuilder();
                        var supportEmailContext = _emamiContext.Supports.AsNoTracking().FirstOrDefault(_ => _.Id == supportContext.Id);
                        sb.Append("<table><tr><td width=50% ><b>Feature</b></td><td width=50%> " + supportEmailContext.Feature + "</td></tr><tr><td width=50%><b>Component</b></td><td width=50%>" + UtilityHelper.GetEnumDescription((DTO.Enums.Device)supportEmailContext.DeviceId) + "</td></tr><tr><td width=50%><b>Impact</b></td><td width=50%>" + UtilityHelper.GetEnumDescription((DTO.Enums.SeverityType)supportEmailContext.SeverityTypeId) + "</td></tr><tr><td width=50%><b> Description </b></td><td width=50%>" + supportEmailContext.Description + "</td></tr><tr><td width=50%><b> Created By </b></td><td width = 50%> " + userContext.Name + "</td></tr></table>");
                        var replaceEmailTemplate = emailTemplate.PlainTemplate.Replace(Constants.UserName, userContext.Name).Replace(Constants.Message,sb.ToString());
                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, replaceEmailTemplate);
                        toUser.Add(userContext.Email.Trim());
                        amazonNotificationService.SendEmail(toUser, Constants.SupportIssueSubmittedSubject, string.Empty, htmlTemplate, true);
                    }
                }
                return _resultService.SuccessMessage(Constants.SupportSavedSuccess);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetIssueListWithPaging(SupportFilterInputDto inputDto)
        {
            _methodName = "GetIssueListWithPaging";
            var resultDto = new ResultDto();

            List<IssueRegisterDto> result = new List<IssueRegisterDto>();

            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var supportContext = _emamiContext.Supports.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(inputDto.FromDate) <= DbFunctions.TruncateTime(_.CreatedDate)
                    && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)).ToList();

                if (inputDto.RaisedBy > 0)
                {
                    if (inputDto.RaisedBy == (int)IssueRaisedUser.Distributors)
                    {
                        //Dealer
                        var dealerUserIds = _emamiContext.UserRoles.AsNoTracking()
                            .Where(_ => _.RoleId == (int)DTO.Enums.Role.Dealer).Select(_ => _.User.Id);
                        supportContext = supportContext.Where(_ => dealerUserIds.Contains(_.CreatedBy)).ToList();
                    }
                    else if (inputDto.RaisedBy == (int)IssueRaisedUser.EALEmployee)
                    {
                        //Users Except Dealer & Broker
                        var userIdsExceptDealerAndBroker = _emamiContext.UserRoles.AsNoTracking()
                                .Where(_ => _.RoleId != (int)DTO.Enums.Role.Dealer && _.RoleId != (int)DTO.Enums.Role.Broker).Select(_ => _.User.Id);
                        supportContext = supportContext.Where(_ => userIdsExceptDealerAndBroker.Contains(_.CreatedBy)).ToList();
                    }
                }

                if (inputDto.QueryFrom > 0) //Device
                {
                    if (inputDto.QueryFrom == (int)Device.Portal)
                    {
                        supportContext = supportContext.Where(_ => _.DeviceId == (int)Device.Portal).ToList();
                    }
                    else if (inputDto.QueryFrom == (int)Device.SalesApp)
                    {
                        supportContext = supportContext.Where(_ => _.DeviceId == (int)Device.SalesApp).ToList();
                    }
                    else if (inputDto.QueryFrom == (int)Device.ManagerApp)
                    {
                        supportContext = supportContext.Where(_ => _.DeviceId == (int)Device.ManagerApp).ToList();
                    }
                    else if (inputDto.QueryFrom == (int)Device.DealerApp)
                    {
                        supportContext = supportContext.Where(_ => _.DeviceId == (int)Device.DealerApp).ToList();
                    }
                    else if (inputDto.QueryFrom == (int)Device.All)
                    {
                        supportContext = supportContext.ToList();
                    }
                }

                result = supportContext.Select(_ => new IssueRegisterDto
                {
                    Id = _.Id,
                    Description = _.Description,
                    ImpactId = _.SeverityTypeId,
                    Impact = _.SeverityTypeId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.SeverityType)_.SeverityTypeId) : string.Empty,
                    FeatureId = _.ModuleId,
                    Feature = _.Feature,
                    StatusId = _.StatusId,
                    ComponentId = _.DeviceId,
                    Component = _.DeviceId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.Device)_.DeviceId) : string.Empty,
                    StateId = _.StateId,
                    Status = _.StatusId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.SupportStatus)_.StatusId) : string.Empty,
                    State = _.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(s => s.Id == _.StateId).StateName : string.Empty,
                    CreatedDateTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(_.CreatedDate, DateTimeKind.Unspecified), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                    ModifiedDateTime = _.ModifiedDate,
                    IssueRaisedByUserName = _emamiContext.Users.AsNoTracking().FirstOrDefault(u => u.Id == _.CreatedBy).Name,
                    ResolvedDateTime = _.StatusId == (int)SupportStatus.Resolved || _.StatusId == (int)SupportStatus.Closed ? string.Format("{0:dd-MMM-yyyy hh:mm tt}", _.ModifiedDate) : string.Empty,
                    TimeTakenToResolve = _.StatusId == (int)SupportStatus.Resolved || _.StatusId == (int)SupportStatus.Closed ? (_.CreatedDate - _.ModifiedDate.Value).ToString(@"hh\:mm\:ss") : string.Empty,
                    DeviceId = _.DeviceId,
                    //IssueFromDevice = UtilityHelper.GetEnumDescription((DTO.Enums.Device)_.DeviceId),

                }).OrderByDescending(o => o.Id).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result.ToDataSourceResult(inputDto.DataSourceRequest);
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

        public ResultDto GetIssueListWithCmts(SupportFilterInputDto inputDto)
        {
            //this method is for Mobile App purpose
            _methodName = "GetIssueListWithCmts";
            var resultDto = new ResultDto();


            IssueListDto issueListDto = new IssueListDto();
            List<IssueListDto> result = new List<IssueListDto>();

            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            inputDto.StatusId = 0;
            inputDto.RaisedBy = (int)IssueRaisedUser.All;
            //inputDto.QueryFrom = (int)Device.App;

            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);

                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var supportContext = _emamiContext.Supports.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(inputDto.FromDate) <= DbFunctions.TruncateTime(_.CreatedDate)
                    && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)).ToList();

                if (inputDto.RaisedBy > 0)
                {
                    if (inputDto.RaisedBy == (int)IssueRaisedUser.Distributors)
                    {
                        //Dealer
                        var dealerUserIds = _emamiContext.UserRoles.AsNoTracking()
                            .Where(_ => _.RoleId == (int)DTO.Enums.Role.Dealer).Select(_ => _.User.Id);
                        supportContext = supportContext.Where(_ => dealerUserIds.Contains(_.CreatedBy)).ToList();
                    }
                    else if (inputDto.RaisedBy == (int)IssueRaisedUser.EALEmployee)
                    {
                        //Users Except Dealer & Broker
                        var userIdsExceptDealerAndBroker = _emamiContext.UserRoles.AsNoTracking()
                                .Where(_ => _.RoleId != (int)DTO.Enums.Role.Dealer && _.RoleId != (int)DTO.Enums.Role.Broker).Select(_ => _.User.Id);
                        supportContext = supportContext.Where(_ => userIdsExceptDealerAndBroker.Contains(_.CreatedBy)).ToList();
                    }
                }

                if (inputDto.QueryFrom > 0) //Device
                {
                    if (inputDto.QueryFrom == (int)Device.Portal)
                    {
                        supportContext = supportContext.Where(_ => _.DeviceId == (int)Device.Portal).ToList();
                    }
                    
                }

                var hierarchialUserList = supportContext.Where(_ => _.CreatedBy == inputDto.LoginUserId).ToList();

                foreach (var issueList in hierarchialUserList)
                {
                    var userName = _emamiContext.Users.FirstOrDefault(u => u.Id == issueList.CreatedBy)?.Name;
                    issueListDto = new IssueListDto
                    {
                        Id = issueList.Id,
                        Description = issueList.Description,
                        ComponentId = issueList.DeviceId,
                        Component = issueList.DeviceId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.Device)issueList.DeviceId) : string.Empty,
                        ImpactId = issueList.SeverityTypeId,
                        Impact = issueList.SeverityTypeId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.SeverityType)issueList.SeverityTypeId) : string.Empty,
                        FeatureId = issueList.ModuleId,
                        Feature = issueList.Feature,
                        StatusId = issueList.StatusId,
                        Status = issueList.StatusId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.SupportStatus)issueList.StatusId) : string.Empty,
                        StateId = issueList.StateId,
                        State = issueList.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(s => s.Id == issueList.StateId)?.StateName : string.Empty,
                        CreatedDateTime = issueList.CreatedDate,
                        ModifiedDateTime = issueList.ModifiedDate,
                        IssueRaisedByUserName = userName,
                        ResolvedDateTime = issueList.StatusId == (int)SupportStatus.Resolved || issueList.StatusId == (int)SupportStatus.Closed ? string.Format("{0:dd-MMM-yyyy hh:mm tt}", issueList.ModifiedDate) : string.Empty,
                        TimeTakenToResolve = issueList.StatusId == (int)SupportStatus.Resolved || issueList.StatusId == (int)SupportStatus.Closed ? (issueList.CreatedDate - issueList.ModifiedDate.Value).ToString(@"hh\:mm\:ss") : string.Empty,
                        //DeviceId = issueList.DeviceId,
                        //IssueFromDevice = Enum.GetName(typeof(Device), issueList.DeviceId),
                        //IssueFromDevice = issueList.DeviceId > 0 ? Enum.GetName(typeof(Device), issueList.DeviceId) : string.Empty,


                    };

                    issueListDto.Attachments = _emamiContext.SupportAttachments.Where(_ => _.SupportId == issueList.Id)
                            .Select(_ => new SupportAttachmentDto { MediaId = _.Id, MediaPath = _.MediaPath, MediaTypeId = _.MediaTypeId, FileName = _.FileName }).ToList();

                    issueListDto.Comments = _emamiContext.IssueComment.AsNoTracking().Where(_ => _.SupportId == issueList.Id).ToList()
                        .Select(_ => new IssueCommentsDto
                        {
                            CommentId = _.Id,
                            SupportId = _.SupportId,
                            Comments = _.Comments,
                            CommentedBy = _emamiContext.Users.FirstOrDefault(u => u.Id == _.CreatedBy)?.Name,
                            CommentedDate = _.CreatedDate,
                        }).ToList();

                    result.Add(issueListDto);

                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result.OrderByDescending(o => o.Id);
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

        public ResultDto GetIssueDetailsBySupportId(IssueDetailInputDto inputDto)
        {
            _methodName = "GetIssueDetailsBySupportId";
            var resultDto = new ResultDto();
            IssueRegisterDto result = new IssueRegisterDto();

            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.SupportId == 0)
                {
                    return _resultService.ErrorMessage(Constants.SupportIdMissing);
                }
                result = _emamiContext.Supports.AsNoTracking()
                    .Where(_ => _.Id == inputDto.SupportId).ToList()
                    .Select(_ => new IssueRegisterDto
                    {
                        Id = _.Id,
                        Description = _.Description,
                        ComponentId = _.DeviceId,
                        Component = _.DeviceId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.Device)_.DeviceId) : string.Empty,
                        ImpactId = _.SeverityTypeId,
                        Impact = _.SeverityTypeId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.SeverityType)_.SeverityTypeId) : string.Empty,
                        FeatureId = _.ModuleId,
                        //Feature = _.ModuleId > 0 ? Enum.GetName(typeof(Module), _.ModuleId) : string.Empty,
                        Feature = _.Feature,
                        StatusId = _.StatusId,
                        Status = _.StatusId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.SupportStatus)_.StatusId) : string.Empty,
                        StateId = _.StateId,
                        CreatedDateTime = _.CreatedDate,
                        State = _.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(s => s.Id == _.StateId).StateName : string.Empty,
                        IssueRaisedByUserName = _emamiContext.Users.AsNoTracking().FirstOrDefault(u => u.Id == _.CreatedBy).Name,
                        ResolvedDateTime = _.StatusId == (int)SupportStatus.Resolved || _.StatusId == (int)SupportStatus.Closed ? string.Format("{0:dd-MMM-yyyy hh:mm tt}", _.ModifiedDate) : string.Empty,
                        TimeTakenToResolve = _.StatusId == (int)SupportStatus.Resolved || _.StatusId == (int)SupportStatus.Closed ? (_.CreatedDate - _.ModifiedDate.Value).ToString(@"hh\:mm\:ss") : string.Empty,

                    }).FirstOrDefault();

                if (result != null)
                {
                    result.Attachments = _emamiContext.SupportAttachments.Where(_ => _.SupportId == inputDto.SupportId)
                        .Select(_ => new SupportAttachmentDto { MediaId = _.Id, MediaPath = _.MediaPath, MediaTypeId = _.MediaTypeId, FileName = _.FileName }).ToList();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result;
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

        public ResultDto UpdateSupportIssueStatus(IssueStatusUpdateDto inputDto)
        {
            _methodName = "UpdateSupportIssueStatus";
            var resultDto = new ResultDto();
            var saudaDetails = new SaudaDetailOutputDto();
            try
            {
                if (inputDto == null || (inputDto.SupportId == 0) || (inputDto.StatusId == 0))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var supportContext = _emamiContext.Supports.FirstOrDefault(f => f.Id == inputDto.SupportId);
                if (supportContext != null)
                {
                    supportContext.StatusId = inputDto.StatusId;
                    supportContext.ModifiedBy = inputDto.ModifiedBy;
                    supportContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.SaveChanges();
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                //var supportContext = _emamiContext.Supports.FirstOrDefault(f => f.Id == inputDto.SupportId);
                if (inputDto.IssueComments != null)
                {

                    var issueCommentContext = new IssueComment()
                    {
                        SupportId = inputDto.SupportId,
                        Comments = inputDto.IssueComments,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        CreatedBy = inputDto.ModifiedBy,
                    };
                    _emamiContext.IssueComment.Add(issueCommentContext);
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

        public ResultDto GetIssueCommentsList(long supportId)
        {
            _methodName = "GetIssueCommentsList";
            var resultDto = new ResultDto();
            var OutputListdto = new List<IssueCommentsDto>();
            try
            {
                var comments =
                               from support in _emamiContext.Supports.AsNoTracking()
                               join supportComments in _emamiContext.IssueComment.AsNoTracking() on support.Id equals supportComments.SupportId
                               join userMapping in _emamiContext.Users.AsNoTracking() on supportComments.CreatedBy equals userMapping.Id
                               where supportComments.SupportId == supportId
                               select new IssueCommentsDto
                               {
                                   SupportId = supportComments.SupportId,
                                   CommentId = supportComments.Id,
                                   Comments = supportComments.Comments,
                                   CommentedBy = userMapping.Name,
                                   CommentedDate = supportComments.CreatedDate,
                                   UserId = (long)supportComments.CreatedBy,
                               };
                if(comments != null && comments.Any())
                {
                    foreach (var item in comments)
                    {
                        var dto = new IssueCommentsDto();
                        dto.SupportId = item.SupportId;
                        dto.CommentId = item.CommentId;
                        dto.Comments = item.Comments;
                        dto.CommentedBy = item.CommentedBy;
                        dto.CommentedDate = item.CommentedDate;
                        dto.UserId = (long)item.UserId;
                        dto.CommentedDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(item.CommentedDate, DateTimeKind.Unspecified), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                        OutputListdto.Add(dto);
                    }
                }

                resultDto.SuccessDto.Response = OutputListdto.ToList();
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

        public ResultDto ExportSupportIssues(SupportFilterInputDto inputDto)
        {
            _methodName = "ExportSupportIssues";
            var resultDto = new ResultDto();

            IssueListDto issueListDto = new IssueListDto();
            List<IssueListDto> result = new List<IssueListDto>();

            try
            {
                var supportContext = _emamiContext.Supports.AsNoTracking()
                  .Where(_ => DbFunctions.TruncateTime(inputDto.FromDate) <= DbFunctions.TruncateTime(_.CreatedDate)
                  && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)).ToList();

                if (inputDto.RaisedBy > 0)
                {
                    if (inputDto.RaisedBy == (int)IssueRaisedUser.Distributors)
                    {
                        //Dealer
                        var dealerUserIds = _emamiContext.UserRoles.AsNoTracking()
                            .Where(_ => _.RoleId == (int)DTO.Enums.Role.Dealer).Select(_ => _.User.Id);
                        supportContext = supportContext.Where(_ => dealerUserIds.Contains(_.CreatedBy)).ToList();
                    }
                    else if (inputDto.RaisedBy == (int)IssueRaisedUser.EALEmployee)
                    {
                        //Users Except Dealer & Broker
                        var userIdsExceptDealerAndBroker = _emamiContext.UserRoles.AsNoTracking()
                                .Where(_ => _.RoleId != (int)DTO.Enums.Role.Dealer && _.RoleId != (int)DTO.Enums.Role.Broker).Select(_ => _.User.Id);
                        supportContext = supportContext.Where(_ => userIdsExceptDealerAndBroker.Contains(_.CreatedBy)).ToList();
                    }
                }
                if (inputDto.QueryFrom > 0) //Device
                {
                    if (inputDto.QueryFrom == (int)Device.Portal)
                    {
                        supportContext = supportContext.Where(_ => _.DeviceId == (int)Device.Portal).ToList();
                    }
                    else if (inputDto.QueryFrom == (int)Device.SalesApp)
                    {
                        supportContext = supportContext.Where(_ => _.DeviceId == (int)Device.SalesApp).ToList();
                    }
                    else if (inputDto.QueryFrom == (int)Device.ManagerApp)
                    {
                        supportContext = supportContext.Where(_ => _.DeviceId == (int)Device.ManagerApp).ToList();
                    }
                    else if (inputDto.QueryFrom == (int)Device.DealerApp)
                    {
                        supportContext = supportContext.Where(_ => _.DeviceId == (int)Device.DealerApp).ToList();
                    }
                    else if (inputDto.QueryFrom == (int)Device.All)
                    {
                        supportContext = supportContext.ToList();
                    }
                }

                foreach (var issueList in supportContext)
                {
                    var userName = _emamiContext.Users.FirstOrDefault(u => u.Id == issueList.CreatedBy)?.Name;

                    issueListDto = new IssueListDto
                    {
                        Id = issueList.Id,
                        Description = issueList.Description,
                        ComponentId = issueList.DeviceId,
                        Component = issueList.DeviceId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.Device)issueList.DeviceId) : string.Empty,

                        ImpactId = issueList.SeverityTypeId,
                        Impact = issueList.SeverityTypeId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.SeverityType)issueList.SeverityTypeId) : string.Empty,
                        FeatureId = issueList.ModuleId,
                        Feature = issueList.Feature!=null?issueList.Feature:string.Empty,
                        StatusId = issueList.StatusId,
                        Status = issueList.StatusId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.SupportStatus)issueList.StatusId) : string.Empty,

                        StateId = issueList.StateId,
                        State = issueList.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(s => s.Id == issueList.StateId)?.StateName : string.Empty,
                        CreatedDateTime = issueList.CreatedDate,
                        ModifiedDateTime = issueList.ModifiedDate,
                        IssueRaisedByUserName = userName,
                        ResolvedDateTime = issueList.StatusId == (int)SupportStatus.Resolved || issueList.StatusId == (int)SupportStatus.Closed ? string.Format("{0:dd-MMM-yyyy hh:mm tt}", issueList.ModifiedDate) : string.Empty,
                        TimeTakenToResolve = issueList.StatusId == (int)SupportStatus.Resolved || issueList.StatusId == (int)SupportStatus.Closed ? (issueList.CreatedDate - issueList.ModifiedDate.Value).ToString(@"hh\:mm\:ss") : string.Empty,
                        //DeviceId = issueList.DeviceId,
                        //IssueFromDevice = issueList.DeviceId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.IssueType)issueList.DeviceId) : string.Empty,

                    };

                    issueListDto.Comments = _emamiContext.IssueComment.AsNoTracking().Where(_ => _.SupportId == issueList.Id).ToList()
                        .Select(_ => new IssueCommentsDto
                        {
                            CommentId = _.Id,
                            SupportId = _.SupportId,
                            Comments = _.Comments,
                            CommentedBy = _emamiContext.Users.FirstOrDefault(u => u.Id == _.CreatedBy)?.Name,
                            CommentedDate = _.CreatedDate,
                        }).ToList();

                    result.Add(issueListDto);

                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }

            return resultDto;
        }

        public ResultDto GetFeatureList()
        {
            _methodName = "GetFeatureList";
            List<DropDownDto> outputDto = new List<DropDownDto>();

            try
            {
                var SupportFeatures = _emamiContext.Configurations.AsNoTracking().Where(_ => _.Isactive && _.Id == (int)DTO.Enums.Configuration.SupportFeatures)
                                   .Select(_ => new DropDownDto
                                   {
                                       Id = _.Id,
                                       Name = _.Value
                                   }).FirstOrDefault();

                if (SupportFeatures != null)
                {
                    string[] components = SupportFeatures.Name.Split(',');

                    for (int i = 0; i < components.Length; i++) {
                        outputDto.Add(new DropDownDto { Id = i+1, Name = components[i] });

                    }
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

        public ResultDto GetQueryFromList()
        {
            _methodName = "GetQueryFromList";
            List<DropDownDto> outputDto = new List<DropDownDto>();

            try
            {
                var queryFromList = UtilityHelper.EnumToList<DTO.Enums.Device>();
                outputDto = queryFromList.Select(item => new DropDownDto()
                {
                    Name = UtilityHelper.GetEnumDescription(item),
                    Id = (int)item
                })/*.OrderBy(n => n.Name)*/.ToList();

                return _resultService.SuccessObject(outputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

    }
}
