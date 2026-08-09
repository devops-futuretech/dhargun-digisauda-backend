using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using GMCore.Helper;
using GMCore.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Adani.Solution.DTO;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.Models;
using Adani.Solution.DTO.Common;
using Adani.Solution.DTO.Enums;
using Kendo.Mvc.UI;
using System.Web;
using System.IO;
using System.Web.Hosting;
using System.Configuration;

namespace Adani.Solution.MVC.ServiceClient
{
    public class SupportClient : BaseClient
    {
        private const string ServiceName = "Support Client";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;

        // Note: reuse same allowed image rules as MediaClient — do not alter existing validation elsewhere.
        private static readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png" };
        private static readonly string[] _allowedImageMimeTypes = { "image/jpeg", "image/pjpeg", "image/jpg", "image/png", "image/x-png" };


        #region Dropdown - Lookups

        public List<DropDownDto> GetIssueTypeListForDropdown()
        {
            _logger.Info("MasterClient-GetIssueTypeList:");
            var typeList = new List<DropDownDto>();
            foreach (var unitDetailsItem in Utility.EnumToList<IssueType>())
            {
                var unitItem = new DropDownDto
                {
                    Name = Utility.GetEnumDescription(unitDetailsItem),
                    Id = (int)unitDetailsItem
                };
                typeList.Add(unitItem);
            }
            return typeList.Any() ? typeList.OrderBy(x => x.Name).ToList() : typeList;

        }

        public List<DropDownDto> GetSeverityListForDropdown()
        {
            _methodName = "GetSeverityListForDropdown";
            var typeList = new List<DropDownDto>();

            foreach (var unitDetailsItem in Utility.EnumToList<SeverityType>())
            {
                var unitItem = new DropDownDto
                {
                    Name = Utility.GetEnumDescription(unitDetailsItem),
                    Id = (int)unitDetailsItem
                };
                typeList.Add(unitItem);
            }
            return typeList.Any() ? typeList.OrderBy(x => x.Name).ToList() : typeList;

        }

        /// <summary>
        /// Method to Get Feature List 
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<DropDownDto>> GetFeatureListForDropdown(LoginUserIdDto inputDto)
        {
            _methodName = "GetFeatureList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var apiUrl = ApiUrl.WebApiUrlGetFeatureList;
            return await GetListAsync<DropDownDto>(apiUrl, inputDto);
        }

        public async Task<IList<DropDownDto>> GetQueryFromListForDropdown(LoginUserIdDto inputDto)
        {
            _methodName = "GetQueryFromList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var apiUrl = ApiUrl.WebApiUrlGetQueryFromList;
            return await GetListAsync<DropDownDto>(apiUrl, inputDto);
        }

        public List<DropDownDto> GetModuleListForDropdown()
        {
            _methodName = "GetModuleListForDropdown";
            var typeList = new List<DropDownDto>();

            foreach (var unitDetailsItem in Utility.EnumToList<Module>())
            {
                var unitItem = new DropDownDto
                {
                    Name = Utility.GetEnumDescription(unitDetailsItem),
                    Id = (int)unitDetailsItem
                };
                typeList.Add(unitItem);
            }
            return typeList.Any() ? typeList.OrderBy(x => x.Name).ToList() : typeList;
           
        }

        public List<DropDownDto> GetSupportIssueStatusListForDropdown()
        {
            _methodName = "GetSupportIssueStatusListForDropdown";
            var outputDto = new List<DropDownDto>();
            try
            {
                outputDto = ((DTO.Enums.SupportStatus[])Enum.GetValues(typeof(DTO.Enums.SupportStatus)))
                    .Select(c => new DropDownDto() { Id = (int)c, Name = c.ToString() }).ToList();
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return outputDto;
        }

        public List<DropDownDto> GetRaisedByListForDropdown()
        {
            _methodName = "GetRaisedByListForDropdown";
            var typeList = new List<DropDownDto>();

            foreach (var unitDetailsItem in Utility.EnumToList<IssueRaisedUser>())
            {
                var unitItem = new DropDownDto
                {
                    Name = Utility.GetEnumDescription(unitDetailsItem),
                    Id = (int)unitDetailsItem
                };
                typeList.Add(unitItem);
            }
            return typeList.Any() ? typeList.OrderBy(x => x.Name).ToList() : typeList;

        }

        #endregion

        #region Support - Issue Register

        public async Task<DataSourceResult> GetSupportIssueListAsync(SupportFilterInputDto inputDto)
        {
            _methodName = "GetSupportIssueListAsync";
            var response = await GetKendoGridResultAsync<IssueRegisterDto>(ApiUrl.WebApiUrlGetIssueList, inputDto);
            return response;
        }

        public async Task<DataSourceResult> GetSupportIssueListWithCmtsAsync(SupportFilterInputDto inputDto)
        {
            _methodName = "GetSupportIssueListWithCmtsAsync";
            var response = await GetKendoGridResultAsync<IssueListDto>(ApiUrl.WebApiUrlGetIssueListWithCmts, inputDto);
            return response;
        }

        public async Task<IList<IssueCommentsDto>> GetIssueCommentsListAsync(long supportId)
        {
            _methodName = "GetIssueCommentsListAsync";
            string apiUrl = ApiUrl.WebApiUrlGetIssueCommentsList;
            return await GetListAsync<IssueCommentsDto>(apiUrl, supportId);
        }

        

       
        /// <summary>
        /// Attempt to validate basic image signature (JPEG/PNG).
        /// Uses a seekable stream; if stream is not seekable we try to copy into a MemoryStream.
        /// This is the same safety check already applied in MediaClient; we do not alter validation logic.
        /// </summary>
        private bool IsValidImageSignature(Stream stream, string extension)
        {
            if (stream == null) return false;

            // Make sure we have a seekable stream for header inspection
            Stream probe = stream;
            MemoryStream temp = null;
            try
            {
                if (!probe.CanSeek)
                {
                    temp = new MemoryStream();
                    probe.CopyTo(temp);
                    temp.Seek(0, SeekOrigin.Begin);
                    probe = temp;
                }

                if (!probe.CanSeek) return false;
                var original = probe.Position;
                probe.Seek(0, SeekOrigin.Begin);
                var header = new byte[8];
                var read = probe.Read(header, 0, header.Length);
                probe.Seek(original, SeekOrigin.Begin);

                // JPEG
                if (read >= 3 && header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF)
                {
                    return extension == ".jpg" || extension == ".jpeg";
                }
                // PNG
                if (read >= 8 &&
                    header[0] == 0x89 &&
                    header[1] == 0x50 &&
                    header[2] == 0x4E &&
                    header[3] == 0x47 &&
                    header[4] == 0x0D &&
                    header[5] == 0x0A &&
                    header[6] == 0x1A &&
                    header[7] == 0x0A)
                {
                    return extension == ".png";
                }

                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                temp?.Dispose();
            }
        }

        /// <summary>
        /// Build a secure handler URL to return to callers (same as used above).
        /// Keep this internal to avoid coupling with controller URL helpers.
        /// </summary>
        private string BuildSecureHandlerUrl(string fileName, string folderName)
        {
            return $"/Support/Download?file={HttpUtility.UrlEncode(fileName)}&folder={HttpUtility.UrlEncode(folderName)}";
        }

        public ResultDto CheckImageSizeAndType(IEnumerable<HttpPostedFileBase> files)
        {
            var result = new ResultDto();
            var fileSizeValid = true;
            var fileTypeValid = true;
            var errorMessage = string.Empty;

            bool invalidExtension = false;
            bool invalidMimeOrContent = false;

            if (files == null || !files.Any())
            {
                result.IsSuccess = false;
                result.ErrorDto.Message = Helper.GetResourceString("msg_PleaseUploadImg");
                return result;
            }

            // Strict whitelist: Only allow safe image extensions
            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var allowedImageMimeTypes = new[] { "image/jpeg", "image/pjpeg", "image/jpg", "image/png", "image/x-png" };

            // Blocked dangerous extensions that could lead to RCE or XSS
            var blockedExtensions = new[] { ".aspx", ".ashx", ".php", ".js", ".dll", ".exe", ".html", ".htm", ".asp", ".jsp", ".cfm", ".cgi", ".pl", ".py", ".rb", ".sh", ".bat", ".cmd", ".ps1", ".vbs" };

            foreach (var file in files)
            {
                if (file == null || file.ContentLength <= 0) continue;

                // Get extension and sanitize filename
                var originalFileName = file.FileName ?? string.Empty;
                var ext = Path.GetExtension(originalFileName)?.ToLowerInvariant() ?? string.Empty;
                
                // Security: Check for null bytes (path traversal attempt)
                if (originalFileName.Contains("\0") || ext.Contains("\0"))
                {
                    _logger.Warn($"{ServiceName} CheckImageSizeAndType: Null byte detected in filename: {originalFileName}");
                    fileTypeValid = false;
                    invalidExtension = true;
                    continue;
                }

                // Security: Check for double extensions (e.g., .jpg.aspx, .png.php)
                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);
                if (!string.IsNullOrEmpty(fileNameWithoutExt))
                {
                    var secondExt = Path.GetExtension(fileNameWithoutExt)?.ToLowerInvariant();
                    if (!string.IsNullOrEmpty(secondExt) && blockedExtensions.Contains(secondExt))
                    {
                        _logger.Warn($"{ServiceName} CheckImageSizeAndType: Double extension attack detected: {originalFileName}");
                        fileTypeValid = false;
                        invalidExtension = true;
                        continue;
                    }
                }

                // Security: Explicitly block dangerous extensions
                if (blockedExtensions.Contains(ext))
                {
                    _logger.Warn($"{ServiceName} CheckImageSizeAndType: Blocked dangerous extension: {ext} for file: {originalFileName}");
                    fileTypeValid = false;
                    invalidExtension = true;
                    continue;
                }

                // Security: Only allow whitelisted image extensions
                if (!allowedImageExtensions.Contains(ext))
                {
                    _logger.Warn($"{ServiceName} CheckImageSizeAndType: Disallowed extension: {ext} for file: {originalFileName}");
                    fileTypeValid = false;
                    invalidExtension = true;
                    continue;
                }

                // Validate MIME type (Content-Type header can be spoofed, but still check)
                var contentType = (file.ContentType ?? string.Empty).ToLowerInvariant();
                if (!allowedImageMimeTypes.Contains(contentType))
                {
                    _logger.Warn($"{ServiceName} CheckImageSizeAndType: MIME type mismatch: {contentType} for file: {originalFileName}");
                    fileTypeValid = false;
                    invalidMimeOrContent = true;
                    continue;
                }

                // Critical: Validate magic bytes (file signature) - this is the most important check
                try
                {
                    // Reset stream position if possible
                    if (file.InputStream.CanSeek)
                    {
                        file.InputStream.Seek(0, SeekOrigin.Begin);
                    }

                    if (!IsValidImageSignature(file.InputStream, ext))
                    {
                        _logger.Warn($"{ServiceName} CheckImageSizeAndType: Magic byte validation failed for: {originalFileName}");
                        fileTypeValid = false;
                        invalidMimeOrContent = true;
                        continue;
                    }

                    // Reset stream position again after validation
                    if (file.InputStream.CanSeek)
                    {
                        file.InputStream.Seek(0, SeekOrigin.Begin);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Warn($"{ServiceName} CheckImageSizeAndType: Error validating file signature for {originalFileName}: {ex}");
                    fileTypeValid = false;
                    invalidMimeOrContent = true;
                    continue;
                }
                
                // Validate file size
                var fileSize = Math.Round((((decimal)file.ContentLength / 1024m) / 1024m), 1);
                if (fileSize > Settings.BulletinImageFileSize)
                {
                    fileSizeValid = false;
                }
            }

            // Set result based on validation
            if (!fileSizeValid)
            {
                errorMessage = string.Format(Helper.GetResourceString("msg_ImageFileSizeExceed"), Settings.BulletinImageFileSize);
                result.IsSuccess = false;
            }
            else if (!fileTypeValid)
            {
                if (invalidExtension)
                {
                    errorMessage = "Invalid file format. Only JPG, JPEG and PNG images are allowed. Dangerous file types are blocked.";
                }
                else if (invalidMimeOrContent)
                {
                    errorMessage = "Invalid image content. The uploaded file does not match a valid image format. File signature validation failed.";
                }
                else
                {
                    errorMessage = Helper.GetResourceString("msg_PleaseSelectImageFormat");
                }
                result.IsSuccess = false;
            }
            else
            {
                result.IsSuccess = true;
            }

            result.ErrorDto.Message = errorMessage;
            return result;
        }
        public async Task<IssueRegisterDto> SaveSupportIssue(IssueRegisterDto inputDto, IEnumerable<HttpPostedFileBase> files)
        {
            _methodName = "SaveSupportIssue";
            if (files != null)
            {
                List<SupportAttachmentDto> attachments = new List<SupportAttachmentDto>();
                foreach (var file in files)
                {
                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    byte[] byteArray = target.ToArray();

                    SupportAttachmentDto attachment = new SupportAttachmentDto
                    {
                        FileName = file.FileName,
                        FileExtension = Path.GetExtension(file.FileName),
                        MediaTypeId = (int)MediaType.Image,
                        FileByteArray = byteArray
                    };
                    attachments.Add(attachment);
                }
                inputDto.Attachments = attachments;
                return await AddOrUpdate<IssueRegisterDto>(ApiUrl.WebApiUrlPostAddIssue, inputDto, Helper.GetResourceString("msg_IssueSubmittedSuccess"), Helper.GetResourceString("msg_IssueSubmitError"));
            }
            else
            {
                inputDto.PostStatus = false;
                inputDto.PostMessage = Helper.GetResourceString("msg_PleaseSelectAnyMedia");
            }
            return inputDto;
        }

        public async Task<DataSourceResult> GetKendoGridDataAsync<T>(KendoGridResult inputDto, string apiUrl) where T : class
        {
            var result = await GetKendoGridResultAsync<T>(apiUrl, inputDto);
            return result;
        }

        public async Task<IssueRegisterDto> GetIssueDetailsBySupportId(IssueDetailInputDto inputDto)
        {
            _methodName = "GetIssueDetailsBySupportId";
            var response = await GetByInputDto<IssueRegisterDto>(ApiUrl.WebApiUrlGetIssueDetailsBySupportId, inputDto);
            return response;
        }

        public async Task<IssueStatusUpdateDto> UpdateIssueStatus(IssueStatusUpdateDto inputDto)
        {
            _methodName = "UpdateIssueStatus";
            var addOrUpdateMessage = Helper.GetResourceString("msg_StatusUpdateSuccess");
            var apiUrl = ApiUrl.WebApiUrlUpdateIssueStatus;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, Helper.GetResourceString("msg_StatusUpdateError"));
        }

        public async Task<List<IssueRegisterDto>> ExportSupportIssues(SupportFilterInputDto inputDto)
        {
            _methodName = "ExportSupportIssues";
            var result = await GetListAsync<IssueRegisterDto>(ApiUrl.WebApiUrlExportSupportIssues, inputDto);
            return result.ToList();
        }
        #endregion


    }
}