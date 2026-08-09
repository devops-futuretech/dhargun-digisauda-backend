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
using System.IO;
using System.Configuration;
using System.Web;

namespace Adani.Solution.Service
{
    public interface IMediaService
    {
        Task<ResultDto> UploadMedia(HttpPostedFile file, string imageFileName, int pageId, long recordId);
        Task<ResultDto> UploadMediaAndReturnFileName(HttpPostedFile file, string imageFileName, int pageId);
    }
    public class MediaService : IMediaService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Lookup Service");
        private const string ServiceName = "Media Service";
        private readonly IResultService _resultService;
        private readonly IMobileSTPService _stpService;
        private string _methodName;

        public MediaService(IAdaniContext emamiContext, IMobileSTPService stpService, IResultService resultService)
        {
            try
            {
                _emamiContext = emamiContext;
                _resultService = resultService;
                _stpService = stpService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for Media Service", exception);
            }
        }

        public async Task<ResultDto> UploadMedia(HttpPostedFile file, string imageFileName, int pageId, long recordId)
        {
            _methodName = "UploadMedia";
            try
            {
                _logger.Info($"Save media service {DateTime.Now}");
                var folderName = string.Empty;
                //if (file.ContentLength > Config.MaxFileSize)
                //{
                //    return _resultService.ErrorMessage(Constants.MaxFileSize);
                //}

                if (pageId == (int)DTO.Enums.PageType.Competitor)
                {
                    folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.Competitor);
                }
                else if (pageId == (int)DTO.Enums.PageType.ProspectiveDealer)
                {
                    folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.ProspectiveDealer);
                }
                else if (pageId == (int)DTO.Enums.PageType.Dealer)
                {
                    folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.Dealer);
                }
                else if (pageId == (int)DTO.Enums.PageType.Support)
                {
                    folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.Support);
                }
                var directory = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(@"~/" + ConfigurationManager.AppSettings["UploadAttachments"]), folderName);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var guid = Guid.NewGuid();
                var ext = Path.GetExtension(imageFileName);
                imageFileName = guid + ext;
                var filename = Path.Combine(directory, imageFileName);
                //Deletion exists file  
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                _logger.Info($"File write started {DateTime.Now}");
                file.SaveAs(filename);
                _logger.Info($"File write completed {DateTime.Now}");
                var imageNameAddDto = new ImageNameAddDto
                {
                    Url = imageFileName,
                    PageId = pageId
                };
                var result = _stpService.SaveCompetitorImageName(imageNameAddDto);
                if (!result.IsSuccess)
                {
                    return result;
                }
                return result;

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public async Task<ResultDto> UploadMediaAndReturnFileName(HttpPostedFile file, string imageFileName, int pageId)
        {
            _methodName = "UploadMedia";
            try
            {
                _logger.Info($"Save media service {DateTime.Now}");
                var folderName = string.Empty;
                //if (file.ContentLength > Config.MaxFileSize)
                //{
                //    return _resultService.ErrorMessage(Constants.MaxFileSize);
                //}

                if (pageId == (int)DTO.Enums.PageType.Competitor)
                {
                    folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.Competitor);
                }
                else if (pageId == (int)DTO.Enums.PageType.ProspectiveDealer)
                {
                    folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.ProspectiveDealer);
                }
                else if (pageId == (int)DTO.Enums.PageType.Dealer)
                {
                    folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.Dealer);
                }
                else if (pageId == (int)DTO.Enums.PageType.Support)
                {
                    folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.Support);
                }
                else if (pageId == (int)DTO.Enums.PageType.DynamicFormAttachments)
                {
                    folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.DynamicFormAttachments);
                }
                else if (pageId == (int)DTO.Enums.PageType.AudioFiles)
                {
                    folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.AudioFiles);
                }
                else if (pageId == (int)DTO.Enums.PageType.ImagesSaudaMappingwithCallRecording)
                {
                    folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.ImagesSaudaMappingwithCallRecording);
                }
                var directory = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(@"~/" + ConfigurationManager.AppSettings["UploadAttachments"]), folderName);

                if(pageId == (int)DTO.Enums.PageType.ImagesSaudaMappingwithCallRecording || pageId == (int)DTO.Enums.PageType.AudioFiles)
                {
                    directory = Config.WebsitePhysicalPath + Path.Combine(ConfigurationManager.AppSettings["UploadMediaPaths"], folderName);
                }

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                var guid = Guid.NewGuid();
                var ext = Path.GetExtension(imageFileName);
                imageFileName = guid + ext;
                var filename = Path.Combine(directory, imageFileName);
                //Deletion exists file  
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                _logger.Info($"File write started {DateTime.Now}");
                file.SaveAs(filename);
                _logger.Info($"File write completed {DateTime.Now}");
                var imageNameAddDto = new ImageNameAddDto
                {
                    Url = imageFileName,
                    PageId = pageId
                };
                var result = new ResultDto();
                result.IsSuccess = true;
                result.SuccessDto.Response = imageFileName;
                return result;

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        
    }
}
