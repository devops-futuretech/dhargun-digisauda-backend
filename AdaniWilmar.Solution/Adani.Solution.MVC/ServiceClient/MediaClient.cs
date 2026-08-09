using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Controllers;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Web;
using System.Linq;
using GMCore.Logger;
using Sandboxable.Microsoft.WindowsAzure.Storage;
using Sandboxable.Microsoft.WindowsAzure.Storage.Blob;
using GMCore.Helper;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Enums;

namespace Adani.Solution.MVC.ServiceClient
{
    public class MediaClient : BaseClient
    {


        private const string ServiceName = "Media Client";
        private readonly ILogger _logger = Logging.GetLogger("MediaClient");
        private string _methodName;

        public readonly CloudStorageAccount StorageAccount;
        public readonly CloudBlobClient BlobClient;

        public MediaClient()
        {
            var isAzure = Convert.ToBoolean(ConfigurationManager.AppSettings["IsAzureStorage"]);
            if (isAzure)
            {
                StorageAccount = CloudStorageAccount.Parse(ConfigHelper.StorageConnectionString);
                BlobClient = StorageAccount.CreateCloudBlobClient();
            }
        }

        /// <summary>
        /// Method to upload the profile photo
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public ResultViewModel UploadMediaFile(IEnumerable<HttpPostedFileBase> files, string fileName, string folderName, string container, bool isVideo = false)
        {
            var result = new ResultViewModel();

            try
            {
                foreach (var file in files)
                {
                    var fileExtension = Path.GetExtension(file.FileName);
                    if (file == null || file.ContentLength <= 0) continue;
                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        fileName = files != null && files.Any() ? Guid.NewGuid().ToString() : string.Empty;
                    }
                    var fileFullPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, string.Concat(fileName, fileExtension));

                    //Path.Combine(ControllerDelegate.Server.MapPath(ConfigHelper.UploadMediaPath),
                    //       string.Concat(fileName, Path.GetExtension(file.FileName)));

                    file.SaveAs(fileFullPath);

                    var mediaFileItem = new MediaModel
                    {
                        Stream = file.InputStream,
                        FileExtension = fileExtension,
                        MetadataList = new Dictionary<string, string> { { "Profile", file.FileName } },
                        ProfileOrMediaContainer = container.ToLower(),
                        ProfileOrMediaSuffix = ConfigHelper.BlobSuffixForUserProfile,
                        FileName = fileName
                    };

                    var mediaOutputResult = UploadBlob(mediaFileItem);

                    if (mediaOutputResult.IsSuccess && !isVideo)
                    {
                        if (file.ContentType.Contains(Settings.ImageFileContains))
                        {
                            var image = Image.FromStream(file.InputStream);

                            var thumb = image.GetThumbnailImage(Settings.ThumbnailWidth, Settings.ThumbnailHeight, () => false, IntPtr.Zero);

                            var stream = new MemoryStream();
                            thumb.Save(stream, image.RawFormat);

                            mediaFileItem.Stream = stream;
                            mediaFileItem.FileExtension = ConfigHelper.ImageExtension;
                            mediaFileItem.IsThumbnail = true;
                            thumb.Dispose();
                            image.Dispose();
                        }

                        var mediaThumbnailOutputResult = UploadBlob(mediaFileItem);
                    }
                    if (mediaOutputResult.IsSuccess)
                    {
                        if (System.IO.File.Exists(fileFullPath)) System.IO.File.Delete(fileFullPath);
                    }
                    result.ImageFileList.Add(string.Concat(fileName, fileExtension));
                    fileName = string.Empty;
                }
                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                _logger.Error("Error in upload the media files" + exception);
                result.IsSuccess = false;
                result.ErrorDto.Message = exception.Message;
            }
            return result;
        }

        /// <summary>
        /// Upload the blob in a (existing) container
        /// </summary>
        /// <param name="MediaModel"></param>
        /// <returns></returns>
        public MediaResultDto UploadBlob(MediaModel mediaModel)
        {
            var result = new MediaResultDto();
            try
            {
                var memoryStream = mediaModel.Stream;
                // After copying the contents to stream, initialize it's position back to zeroth location
                memoryStream.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);

                string blobName;

                if (mediaModel.IsThumbnail)
                {
                    blobName = string.Concat(mediaModel.FileName,
                                ConfigHelper.ThumbnailSuffix,
                               mediaModel.FileExtension);
                }
                else
                {
                    blobName = string.Concat(mediaModel.FileName,
                               mediaModel.FileExtension);
                }

                _logger.Info("Upload blob : " + blobName + " in container : " + mediaModel.ProfileOrMediaContainer);
                var container = GetContainer(mediaModel.ProfileOrMediaContainer, BlobContainerPublicAccessType.Off);
                var blob = container.GetBlockBlobReference(blobName);

                //Add as many possible metadata here
                foreach (var variable in mediaModel.MetadataList.ToList())
                {
                    blob.Metadata.Add(variable.Key, variable.Value);
                }

                blob.Properties.ContentType = GetBlobContentType(mediaModel.FileExtension);
                blob.UploadFromStream(mediaModel.Stream, mediaModel.Stream.Length);

                result.IsSuccess = true;
                result.FileName = blobName;

                var sasBlobToken = string.Empty;

                SharedAccessBlobPolicy adHocSAS = new SharedAccessBlobPolicy()
                {
                    // When the start time for the SAS is omitted, the start time is assumed to be the time when the storage service receives the request.
                    // Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.
                    SharedAccessExpiryTime = DateTime.Now.AddHours(24),
                    Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List,
                };

                sasBlobToken = blob.GetSharedAccessSignature(adHocSAS);


                if (mediaModel.IsThumbnail)
                {
                    result.ThumbnailUrl = blob.Uri.AbsoluteUri;
                }
                else
                {
                    result.MediaUrl = blob.Uri.AbsoluteUri + (!string.IsNullOrEmpty(sasBlobToken) ? sasBlobToken : string.Empty);
                }
                return result;
            }
            catch (StorageException exception)
            {
                _logger.Error("Error in Upload Blob", exception);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Settings.UploadToBlobFailed;
                return result;
            }

        }

        private CloudBlobContainer GetContainer(string profileOrMediaContainer, BlobContainerPublicAccessType blobContainerPublicAccessType)
        {
            try
            {
                _logger.Info("Fetch Container : " + profileOrMediaContainer);
                var container = BlobClient.GetContainerReference(profileOrMediaContainer.ToLower());
                container.CreateIfNotExists();
                _logger.Info("Set Public Access to container : " + profileOrMediaContainer);
                var permissions = new BlobContainerPermissions { PublicAccess = blobContainerPublicAccessType };
                container.SetPermissions(permissions);
                return container;
            }
            catch (StorageException exception)
            {
                _logger.Error("Error in get  Container", exception);
                return null;
            }
        }



        /// <summary>
        /// Delete the uploaded or existing blob in a container
        /// </summary>
        /// <param name="profileOrMediaContainer"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        public bool DeleteBlob(MediaModel mediaModel)
        {
            try
            {
                //blobName and profileOrMediaContainer mandatory in MediaModel

                _logger.Info("Delete blob : " + mediaModel.BlobName + " in container : " + mediaModel.ProfileOrMediaContainer);
                var container = GetContainer(mediaModel.ProfileOrMediaContainer, BlobContainerPublicAccessType.Off);
                var blob = container.GetBlockBlobReference(mediaModel.BlobName);
                blob.DeleteIfExists();
                return true;
            }
            catch (StorageException exception)
            {
                _logger.Error("Error in delete  Blob", exception);
                return false;
            }
        }

        /// <summary>
        /// Get the Uri of the uploaded or existing blob in a container
        /// </summary>
        /// <param name="mediaModel"></param>
        /// <returns></returns>
        public string GetBlobUri(MediaModel mediaModel)
        {
            try
            {
                //blobName and profileOrMediaContainer mandatory in MediaModel

                _logger.Info("Get blob uri : " + mediaModel.BlobName + " in container : " + mediaModel.ProfileOrMediaContainer);
                var container = GetContainer(mediaModel.ProfileOrMediaContainer, BlobContainerPublicAccessType.Off);
                var blob = container.GetBlockBlobReference(mediaModel.BlobName);
                GetBlobContentType(blob);

                //return blob.Exists() ? blob.Uri.AbsoluteUri : string.Empty;

                SharedAccessBlobPolicy adHocSAS = new SharedAccessBlobPolicy()
                {
                    // When the start time for the SAS is omitted, the start time is assumed to be the time when the storage service receives the request.
                    // Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.
                    SharedAccessExpiryTime = DateTime.Now.AddHours(24),
                    Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List,
                };

                var sasBlobToken = blob.GetSharedAccessSignature(adHocSAS);

                return blob.Exists() ? blob.Uri.AbsoluteUri + sasBlobToken : string.Empty;

            }
            catch (StorageException exception)
            {
                _logger.Error("Error in get  Blob uri", exception);
                return string.Empty;
            }
        }

        private void GetBlobContentType(CloudBlockBlob blob)
        {
            switch (Path.GetExtension(blob.Uri.AbsoluteUri))
            {
                case ".mp4":
                    blob.Properties.ContentType = "video/mp4";
                    blob.SetProperties();
                    break;

                case ".jpg":
                    blob.Properties.ContentType = "image/jpg";
                    blob.SetProperties();
                    break;
                case ".jpeg":
                    blob.Properties.ContentType = "image/jpeg";
                    blob.SetProperties();
                    break;
                case ".png":
                    blob.Properties.ContentType = "image/png";
                    blob.SetProperties();
                    break;
                case ".tif":
                    blob.Properties.ContentType = "image/tif";
                    blob.SetProperties();
                    break;
                case ".xml":
                    blob.Properties.ContentType = "text/xml";
                    blob.SetProperties();
                    break;
            }

        }

        private string GetBlobContentType(string extension)
        {
            switch (extension)
            {
                case ".mp4":
                    return "video/mp4";
                case ".jpg":
                    return "image/jpg";
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".tif":
                    return "image/tif";
                case ".xml":
                    return "text/xml";
            }
            return string.Empty;
        }

        public string GetImageUrl(string fileName, string container, bool isThumbnail = false)
        {
            var imageUrl = string.Empty;
            try
            {
                string blobName = string.Empty;
                if (isThumbnail)
                {
                    blobName = fileName +
                                ConfigHelper.ThumbnailSuffix +
                               ConfigHelper.ImageExtension;
                }
                else
                {
                    blobName = fileName + ConfigHelper.ImageExtension;
                }
                var mediaModel = new MediaModel
                {
                    BlobName = blobName,
                    ProfileOrMediaContainer = container
                };
                imageUrl = GetBlobUri(mediaModel);
            }
            catch (StorageException exception)
            {
                _logger.Error("Error in get  blob uri", exception);
                return imageUrl;
            }
            return imageUrl;
        }


        /// <summary>
        /// Method to save media file
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public List<MediaDto> SaveMediaFile(IEnumerable<HttpPostedFileBase> files, string folderName, IEnumerable<HttpPostedFileBase> video = null)
        {
            var mediaFileItemList = new List<MediaDto>();
            var fileCount = 0;
            var fileName = string.Empty;
            var isAzure = Convert.ToBoolean(ConfigurationManager.AppSettings["IsAzureStorage"]);
            var allFiles = new List<HttpPostedFileBase>();
            try
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var mediaFileItem = new MediaDto();

                        var fileExtension = Path.GetExtension(file.FileName);
                        if (string.IsNullOrWhiteSpace(fileName))
                        {
                            fileName = files != null && files.Any() ? Guid.NewGuid().ToString() : string.Empty;
                        }
                        if (file == null || file.ContentLength <= 0) continue;

                        var fileSize = Math.Round((((decimal)file.ContentLength / (decimal)1024) / (decimal)1024), 1);

                        if (!file.ContentType.Contains(Settings.ImageFileContains) && !file.ContentType.Contains(Settings.VideoFileContains) && !file.ContentType.Contains(Settings.PdfFileContains))
                        {
                            mediaFileItem.IsSuccess = false;
                            mediaFileItem.Message = Helper.GetResourceString("msg_BulletinImageOnlyAccepted");
                            mediaFileItemList.Add(mediaFileItem);
                            continue;
                        }

                        if (fileSize > Settings.ImageFileSize)
                        {
                            mediaFileItem.IsSuccess = false;
                            mediaFileItem.Message = string.Format(Helper.GetResourceString("msg_BulletinImageSizeExceed"), Settings.BulletinImageFileSize);
                            mediaFileItemList.Add(mediaFileItem);
                            continue;
                        }

                        var directory = Path.Combine(ControllerDelegate.Server.MapPath(ConfigurationManager.AppSettings["UploadMediaPath"]), folderName);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        fileCount = fileCount + 1;
                        mediaFileItem.FileName = string.Concat(fileName, fileExtension);

                        var fileFullPath = string.Empty;
                        var mediaTypeId = 0;
                        if (file.ContentType.Contains(Settings.ImageFileContains))
                        {
                            mediaTypeId = (int)MediaType.Image;
                            fileFullPath = Path.Combine(ControllerDelegate.Server.MapPath(ConfigHelper.UploadMediaPath),
                                folderName, string.Concat(fileName, Path.GetExtension(file.FileName)));
                        }
                        else if (file.ContentType.Contains(Settings.VideoFileContains))
                        {
                            mediaTypeId = (int)MediaType.Video;
                            fileFullPath = Path.Combine(ControllerDelegate.Server.MapPath(ConfigHelper.UploadMediaPath),
                               folderName,string.Concat(fileName, Path.GetExtension(file.FileName)));
                        }
                        else if (file.ContentType.Contains(Settings.PdfFileContains))
                        {
                            mediaTypeId = (int)MediaType.Pdf;
                            fileFullPath = Path.Combine(ControllerDelegate.Server.MapPath(ConfigHelper.UploadMediaPath),
                                folderName, string.Concat(fileName, Path.GetExtension(file.FileName)));
                        }


                        if (isAzure)
                        {
                            allFiles = new List<HttpPostedFileBase>();
                            allFiles.Add(file);
                            //result = UploadMediaFile(allFiles, fileName, folderName, folderName, false);
                        }
                        else
                        {
                            file.SaveAs(fileFullPath);

                            mediaFileItem.MediaTypeId = mediaTypeId;

                            mediaFileItem.Stream = file.InputStream;
                            mediaFileItem.FileExtension = Path.GetExtension(file.FileName);

                            mediaFileItem.MetadataList = new Dictionary<string, string> { { "Extension", Path.GetExtension(file.FileName) }, { "Month", DateTime.Now.ToString("MMM") } };

                            if (ConfigHelper.IsThumnailImageCreation)
                            {
                                var thumbFileFullPath = Path.Combine(ControllerDelegate.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, string.Concat(fileName, Settings.ThumbString, ConfigHelper.ImageExtension));
                                var image = Image.FromFile(fileFullPath);
                                var thumb = image.GetThumbnailImage(Settings.ThumbnailWidth, Settings.ThumbnailHeight, () => false, IntPtr.Zero);
                                thumb.Save(thumbFileFullPath);
                                mediaFileItem.IsThumbnail = true;
                                thumb.Dispose();
                                image.Dispose();

                            }
                            mediaFileItem.IsSuccess = true;
                            fileName = string.Empty;
                        }
                        mediaFileItemList.Add(mediaFileItem);
                    }

                }
                //if (video != null)
                //{
                //    foreach (var file in video)
                //    {
                //        var mediaFileItem = new MediaDto();

                //        var videoFileExtension = Path.GetExtension(file.FileName);
                //        if (file == null || file.ContentLength <= 0) continue;

                //        var fileSize = Math.Round((((decimal)file.ContentLength / (decimal)1024) / (decimal)1024), 1);

                //        if (!file.ContentType.Contains(Settings.VideoFileContains))
                //        {
                //            mediaFileItem.IsSuccess = false;
                //            mediaFileItem.Message = Helper.GetResourceString("msg_VideoFileOnlyAccepted");
                //            mediaFileItemList.Add(mediaFileItem);
                //            continue;
                //        }

                //        if (fileSize > Settings.VideoFileSize)
                //        {
                //            mediaFileItem.IsSuccess = false;
                //            mediaFileItem.Message = string.Format(Helper.GetResourceString("msg_VideoFileSizeExceed"), Settings.VideoFileSize);
                //            mediaFileItemList.Add(mediaFileItem);
                //            continue;
                //        }

                //        var directory = Path.Combine(ControllerDelegate.Server.MapPath(ConfigurationManager.AppSettings["UploadMediaPath"]), folderName);
                //        if (!Directory.Exists(directory))
                //        {
                //            Directory.CreateDirectory(directory);
                //        }
                //        if (isAzure)
                //        {
                //            allFiles = new List<HttpPostedFileBase>();
                //            allFiles.Add(file);
                //            result = UploadMediaFile(allFiles, fileName, folderName, folderName, true);
                //        }
                //        else
                //        {
                //            var fileFullPath = Path.Combine(ControllerDelegate.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, string.Concat(fileName, videoFileExtension));
                //            file.SaveAs(fileFullPath);
                //            mediaFileItem.IsSuccess = true;
                //            fileName = string.Empty;
                //        }
                //        mediaFileItemList.Add(mediaFileItem);
                //    }
                //}
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return mediaFileItemList;
        }

        /// <summary>
        /// Method to delete file
        /// </summary>       
        /// <returns></returns>
        public void DeleteFile(string fileName, string folderName)
        {

            _methodName = "DeleteFile";
            var isAzure = Convert.ToBoolean(ConfigurationManager.AppSettings["IsAzureStorage"]);
            if (isAzure)
            {
                var mediaModel = new MediaModel { BlobName = fileName, ProfileOrMediaContainer = folderName };
                DeleteBlob(mediaModel);
            }
            else
            {
                try
                {
                    _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                    if(folderName == Enum.GetName(typeof(PageType), 7))
                    {
                        var consentImagefilePath = ConfigHelper.ApiBaseUrlPath + Path.Combine(ConfigHelper.UploadAttachment, folderName, fileName);
                        if (System.IO.File.Exists(consentImagefilePath))
                        {
                            System.IO.File.Delete(consentImagefilePath);
                        }
                    }
                    var filePath = Path.Combine(ControllerDelegate.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, fileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    var thumbFilePath = Path.Combine(ControllerDelegate.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, fileName.Replace(fileName, string.Concat(fileName.Split('.')[0], Settings.ThumbString, ConfigHelper.ImageExtension)));
                    if (System.IO.File.Exists(thumbFilePath))
                    {
                        System.IO.File.Delete(thumbFilePath);
                    }

                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                }
            }

        }

        /// <summary>
        /// Method to delete file
        /// </summary>       
        /// <returns></returns>
        public void DeleteMultipleFile(List<string> file, string folderName)
        {

            _methodName = "DeleteMultipleFile";
            foreach (var fileName in file)
            {
                var isAzure = Convert.ToBoolean(ConfigurationManager.AppSettings["IsAzureStorage"]);
                if (isAzure)
                {
                    var mediaModel = new MediaModel { BlobName = fileName, ProfileOrMediaContainer = folderName };
                    DeleteBlob(mediaModel);
                }
                else
                {
                    try
                    {
                        _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                        var filePath = Path.Combine(ControllerDelegate.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, fileName);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }

                        var thumbFilePath = Path.Combine(ControllerDelegate.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, fileName.Replace(ConfigHelper.ImageExtension, string.Concat(Settings.ThumbString, ConfigHelper.ImageExtension)));
                        if (System.IO.File.Exists(thumbFilePath))
                        {
                            System.IO.File.Delete(thumbFilePath);
                        }

                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                }
            }
        }

        /// <summary>
        /// Method to get file
        /// </summary>       
        /// <returns></returns>
        public string GetMediaFile(string fileName, string folderName, bool isThumbnail = false, bool isVideo = false)
        {
            _methodName = "GetMediaFile";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var filePath = Settings.DefaultNoImageUrl;
            if (!string.IsNullOrEmpty(fileName))
            {
                var fileDetaills = fileName.Split('.');
                if (fileDetaills.Count() > 1 && !isVideo && isThumbnail)
                {
                    fileName = string.Concat(fileDetaills[0], ConfigHelper.ThumbnailSuffix, ConfigHelper.ImageExtension);
                }
                var isAzure = Convert.ToBoolean(ConfigurationManager.AppSettings["IsAzureStorage"]);
                if (isAzure)
                {

                    var mediaModel = new MediaModel { BlobName = fileName, ProfileOrMediaContainer = folderName };
                    filePath = GetBlobUri(mediaModel);
                }
                else
                {
                    if (System.IO.File.Exists(Path.Combine(ControllerDelegate.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, fileName)))
                    {
                        filePath = Path.Combine("..", ConfigHelper.UploadMediaPath, folderName, fileName);
                    }
                }
                if (string.IsNullOrEmpty(filePath))
                {
                    filePath = Settings.DefaultNoImageUrl;
                }
            }
            return filePath;
        }

        /// <summary>
        /// Method to check the image size
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public ResultDto CheckImageSizeType(IEnumerable<HttpPostedFileBase> files)
        {
            var result = new ResultDto();
            var fileSizeValid = true;
            var fileTypeValid = true;
            var errorMessage = string.Empty;

            foreach (var file in files)
            {
                if (file == null || file.ContentLength <= 0) continue;

                var ext = Path.GetExtension(file.FileName);

                //if (!file.ContentType.Contains(Settings.ImageFileContains) && !file.ContentType.Contains(Settings.VideoFileContains))
                if (!Settings.BulletinFileFormatArray.Contains(ext.ToLower()))
                {
                    fileTypeValid = false;
                }

                var fileSize = Math.Round((((decimal)file.ContentLength / (decimal)1024) / (decimal)1024), 1);

                if (fileSize > Settings.BulletinImageFileSize)
                {
                    fileSizeValid = false;
                }
            }

            result.IsSuccess = true;

            if (!fileSizeValid)
            {
                errorMessage = string.Format(Helper.GetResourceString("msg_ImageSizeExceed"), Settings.BulletinImageFileSize);
                result.IsSuccess = false;
            }
            else if (!fileTypeValid)
            {
                errorMessage = Helper.GetResourceString("msg_ImageSizeExceed");
                result.IsSuccess = false;
            }

            result.ErrorDto.Message = errorMessage;
            return result;
        }


    }

}