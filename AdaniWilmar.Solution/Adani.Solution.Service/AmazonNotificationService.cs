using System;
using System.Configuration;
using Amazon;
using System.Web;
using GMCore.Logger;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService.Model;
using Amazon.SimpleNotificationService;
using System.Net.Mail;
using System.IO;
using System.Net.Mime;
using Amazon.Runtime;
using MimeKit;
using Adani.Solution.Service.Common;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using System.Text;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web.Hosting;
using System.Data;
using OfficeOpenXml;
using GMCore.Helper;
using Adani.Solution.MVC.Common;
using System.Reflection;

namespace Adani.Solution.Service
{
    public class AmazonNotificationService
    {
        private readonly ILogger _logger = Logging.GetLogger("Lookup Service");
        private const string ServiceName = "Amazon Notification Service";
        private string _methodName;


        private Amazon.SimpleNotificationService.AmazonSimpleNotificationServiceClient _client { get; set; }
        private AmazonSimpleNotificationServiceClient snsClient { get; set; }

        public AmazonNotificationService()
        {
            //var amazonS3Config = new AmazonSimpleNotificationServiceConfig();
            //var newRegion = RegionEndpoint.GetBySystemName(Constants.AWSRegionName);
            //amazonS3Config.RegionEndpoint = newRegion;
            //_client = new AmazonSimpleNotificationServiceClient(Constants.AWSEmailAccessKey, Constants.AWSEmailSecretKey, amazonS3Config);
            //snsClient = new AmazonSimpleNotificationServiceClient(Amazon.RegionEndpoint.USWest2);
        }

        public static String CreateSNSTopic(AmazonSimpleNotificationServiceClient snsClient)
        {
            //create a new SNS topic
            CreateTopicRequest createTopicRequest = new CreateTopicRequest(Constants.AWSTopic);
            CreateTopicResponse createTopicResponse = snsClient.CreateTopic(createTopicRequest);
            //get request id for CreateTopicRequest from SNS metadata		
            Console.WriteLine("CreateTopicRequest - " + createTopicResponse.ResponseMetadata.RequestId);
            return createTopicResponse.TopicArn;
        }
     
        #region SFTP Email file send

        public void SendNotification(string subject, string syncMessage, string syncType, SapDataSyncResultDto result = null, List<string> filePath = null)
        {
            if (syncMessage != null)
            {
                //syncMessage = syncMessage != null ? syncMessage.Replace("{0}", syncType) : syncType;
                var mobilenumbersList = ConsoleSettings.SapDataSyncMobileNumbers.Split(',');
                var toEmailIds = ConsoleSettings.SapDataSyncIsToMailId ? ConsoleSettings.ToEmail.Split(',').ToList() : ToEmailIs(syncType);

                var sbPlainText = new StringBuilder();
                if (syncType == ConsoleSettings.SaudaConversion_ValidationMsg)
                {
                    sbPlainText.AppendLine(syncMessage);
                }
                else
                {
                    sbPlainText.AppendLine(string.Concat($"Data Sync Started DateTime: ", result != null ? result.SyncStartedDateTime.ToString("dd-MM-yyyy hh:mm:ss") : string.Empty));
                    sbPlainText.AppendLine(string.Concat("<br/>Data Sync Completed DateTime: ", result != null ? result.SyncCompletedDateTime.ToString("dd-MM-yyyy hh:mm:ss") : string.Empty));
                    sbPlainText.AppendLine(string.Concat("<br/>Data Retrieved: ", result != null ? result.OutstandingResult.DataRetrieved : 0));
                    sbPlainText.AppendLine(string.Concat("<br/> Data Synced: ", result != null ? result.OutstandingResult.DataSynced : 0));
                    if (Constants.ErrorSapMessage == syncMessage || Constants.SapSyncSuccessMessage == syncMessage)
                    {
                        sbPlainText.AppendLine(string.Concat("<br/>Message: ", syncMessage));
                    }
                    else
                    {
                        var errormessageList = syncMessage.Split(',').ToList();
                        long count = 1;
                        if (errormessageList.IsAny())
                        {
                            sbPlainText.AppendLine(string.Concat("<br/>Message: "));
                            foreach (var msg in errormessageList)
                            {
                                sbPlainText.AppendLine(string.Concat("<br/>" + count + ".", msg));
                                count = count + 1;
                            }
                        }
                    }
                    var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow).ToString("MM_dd_yyyy_hh_mm_ss_ffffff");
                    //var CurrentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    //yyyy-MM-dd hh:mm:ss.SSS
                    //var date = string.Format("{0:dd-MMM-yyyy hh:mm:ss}", CurrentDate);
                    var TotalRecordString = result.TotalInputRecordDetailsResponse != null ?  JsonConvert.SerializeObject(result.TotalInputRecordDetailsResponse) : string.Empty;
                    var ErrorListRecordString = result.ErrorDetailsResponse != null ? JsonConvert.SerializeObject(result.ErrorDetailsResponse) : string.Empty;
                    var SuccessListRecordString = result.SuccessRecordDetailsResponse != null ? JsonConvert.SerializeObject(result.SuccessRecordDetailsResponse) : string.Empty;
                    string fileNameforTotalRecords = "TotalInputRecords_" + syncType + "_" + currentDate + ".xlsx";
                    string fileNameforSuccessRecords = "SuccessRecords_" + syncType + "_" + currentDate + ".xlsx";
                    string fileNameforErrorRecords = "ErrorRecords_" + syncType + "_" + currentDate + ".xlsx";
                    var recordWithFilename = new List<HANAEmailAttachmentFileDetails>();
                    recordWithFilename.Add(new HANAEmailAttachmentFileDetails { RecordList = TotalRecordString, FilenameList = fileNameforTotalRecords });
                    recordWithFilename.Add(new HANAEmailAttachmentFileDetails { RecordList = SuccessListRecordString, FilenameList = fileNameforSuccessRecords });
                    recordWithFilename.Add(new HANAEmailAttachmentFileDetails { RecordList = ErrorListRecordString, FilenameList = fileNameforErrorRecords });
                    filePath = new List<string>();
                    var templatePath = Path.Combine(ConsoleSettings.SapPhysicalPath, "FinalPriceTemplateForAllRecords.xlsx");
                    foreach (var data in recordWithFilename)
                    {
                        data.RecordList = data.RecordList.Replace("[]", string.Empty);
                        var localFilepath = Path.Combine(ConsoleSettings.SapPhysicalPath, data.FilenameList);
                        // Check if file already exists. If yes, delete it.     
                        if (File.Exists(localFilepath))
                        {
                            File.Delete(localFilepath);
                        }

                        if(!string.IsNullOrEmpty(data.RecordList))
                        {
                            //DataTable dataTable = (DataTable)JsonConvert.DeserializeObject(data.RecordList, (typeof(DataTable)));

                            DataTable dataTable;
                            if (syncType == ConsoleSettings.SaudaCreationSubject)
                            {
                                string name = "ItemData";
                                if (data.FilenameList.Contains("TotalInputRecords"))
                                {
                                    //resultList.Add((SaudaCreateSAPToAPPDto)result.TotalInputRecordDetailsResponse);
                                    dataTable = CreateNestedDataTable<SaudaCreateSAPToAPPDto, SAPDataItemDataSapToApp>((List<SaudaCreateSAPToAPPDto>)result.TotalInputRecordDetailsResponse, name);
                                }else if (data.FilenameList.Contains("SuccessRecords"))
                                {
                                    //resultList.Add((SaudaCreateSAPToAPPDto)result.SuccessRecordDetailsResponse);
                                    dataTable = CreateNestedDataTable<SaudaCreateSAPToAPPDto, SAPDataItemDataSapToApp>((List<SaudaCreateSAPToAPPDto>)result.SuccessRecordDetailsResponse, name);
                                }
                                else
                                {
                                    //resultList.Add((SaudaCreateSAPToAPPDto)result.ErrorDetailsResponse);
                                    dataTable = CreateNestedDataTable<SaudaCreateSAPToAPPDto, SAPDataItemDataSapToApp>((List<SaudaCreateSAPToAPPDto>)result.ErrorDetailsResponse, name);
                                }


                                
                            }else if(syncType == ConsoleSettings.LiftingInquiry)
                            {
                                string name = "ItemData";
                                if (data.FilenameList.Contains("TotalInputRecords"))
                                {
                                    List<SalesOrderCreate> collection = (List<SalesOrderCreate>)result.TotalInputRecordDetailsResponse;
                                    dataTable = CreateNestedDataTable<SalesOrderCreate, ItemDataDTO>(collection, name);
                                }
                                else if (data.FilenameList.Contains("SuccessRecords"))
                                {
                                    List<SalesOrderCreate> collection = (List<SalesOrderCreate>)result.SuccessRecordDetailsResponse;
                                    dataTable = CreateNestedDataTable<SalesOrderCreate, ItemDataDTO>(collection, name);
                                }
                                else
                                {
                                    List<SalesOrderCreate> collection = (List<SalesOrderCreate>)result.ErrorDetailsResponse;
                                    dataTable = CreateNestedDataTable<SalesOrderCreate, ItemDataDTO>(collection, name);
                                }
                            }else if (syncType == ConsoleSettings.InvoiceFolder)
                            {
                                string name = "ItemData";
                                if (data.FilenameList.Contains("TotalInputRecords"))
                                {
                                    List<InvoiceDto> collection = (List<InvoiceDto>)result.TotalInputRecordDetailsResponse;
                                    dataTable = CreateNestedDataTable<InvoiceDto, InvoiceDetailsDto>(collection, name);
                                }
                                else if (data.FilenameList.Contains("SuccessRecords"))
                                {
                                    List<InvoiceDto> collection = (List<InvoiceDto>)result.SuccessRecordDetailsResponse;
                                    dataTable = CreateNestedDataTable<InvoiceDto, InvoiceDetailsDto>(collection, name);
                                }
                                else
                                {
                                    List<InvoiceDto> collection = (List<InvoiceDto>)result.ErrorDetailsResponse;
                                    dataTable = CreateNestedDataTable<InvoiceDto, InvoiceDetailsDto>(collection, name);
                                }
                            }
                            else
                            {
                                dataTable = (DataTable)JsonConvert.DeserializeObject(data.RecordList, (typeof(DataTable)));
                            }

                           

                            var indexof = data.FilenameList.IndexOf('_');
                            using (var ep = new ExcelPackage())
                            {
                                var ws = ep.Workbook.Worksheets.Add(data.FilenameList.Substring(0, indexof));
                                ws.Cells["A1:BZ1"].Style.Font.Bold = true;
                                ws.Cells["A1:BZ1"].Style.Font.Size = 12;
                                ws.Name = data.FilenameList.Substring(0, indexof);
                                ws.Cells.LoadFromDataTable(dataTable, true);
                                ws.Cells.AutoFitColumns();

                                using (Stream stream = System.IO.File.Create(localFilepath))
                                {
                                    ep.SaveAs(stream);
                                }
                            }
                            //return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);

                            //var indexof = data.FilenameList.IndexOf('_');
                            //using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                            //{
                            //    var ws = ep.Workbook.Worksheets[1];
                            //    ws.Cells["A1:BZ1"].Style.Font.Bold = true;
                            //    ws.Cells["A1:BZ1"].Style.Font.Size = 12;
                            //    ws.Name = data.FilenameList.Substring(0, indexof);
                            //    ws.Cells.LoadFromDataTable(dataTable, true);
                            //    ws.Cells.AutoFitColumns();

                            //    using (Stream stream = System.IO.File.Create(localFilepath))
                            //    {
                            //        ep.SaveAs(stream);
                            //    }
                            //}

                            filePath.Add(localFilepath);
                        }
                    }
                    if (!string.IsNullOrEmpty(TotalRecordString))
                    {
                        string fileNameforException = "InuputDetails_" + syncType + "_" + currentDate + ".txt";
                        var localFilepath = Path.Combine(ConsoleSettings.SapPhysicalPath, fileNameforException);
                        // Check if file already exists. If yes, delete it.     
                        if (File.Exists(localFilepath))
                        {
                            File.Delete(localFilepath);
                        }

                        // Create a new file     
                        using (StreamWriter sw = File.CreateText(localFilepath))
                        {
                            sw.WriteLine("Input Record : {0}", TotalRecordString);
                        }

                        filePath.Add(localFilepath);
                    }
                    if (!string.IsNullOrEmpty(result.ExceptionMessage))
                    {
                        string fileNameforException = "ExceptionDetails_" + syncType + "_" + currentDate + ".txt";
                        var localFilepath = Path.Combine(ConsoleSettings.SapPhysicalPath, fileNameforException);
                        // Check if file already exists. If yes, delete it.     
                        if (File.Exists(localFilepath))
                        {
                            File.Delete(localFilepath);
                        }

                        // Create a new file     
                        using (StreamWriter sw = File.CreateText(localFilepath))
                        {
                            sw.WriteLine("Exception Message : {0}", result.ExceptionMessage);
                        }

                        filePath.Add(localFilepath);
                    }
                }
                var emailTemplate = ConsoleSettings.Emailtemplate;
                var replaceEmailTemplate = emailTemplate.Replace(ConsoleSettings.ReplaceMainContent, sbPlainText.ToString());
                //replaceEmailTemplate = replaceEmailTemplate.Replace("logo", ConsoleSettings.LogoPath);
                var replaceMobileTemplates = string.Empty;
                if (ConsoleSettings.UATEmail)
                {
                    subject = string.Concat(subject, "||", "UAT");
                }
                SftpSendEmail(toEmailIds, subject, replaceEmailTemplate, "", true, replaceEmailTemplate, filePath);
                // var sftpConnection = new SftpConnectorService();
                //sftpConnection.DeleteLocalFile(filePath);

                //foreach (var mobileNumber in mobilenumbersList)
                //{
                //    if (mobileTemplate != null && !string.IsNullOrEmpty(mobileNumber))
                //    {
                //        _notificationService.SendMessage(replaceMobileTemplates, mobileNumber.Trim());
                //    }
                //}
            }

        }
        public DataTable CreateNestedDataTable<TOuter, TInner>(IEnumerable<TOuter> list, string innerListPropertyName)
        {
            PropertyInfo[] outerProperties = typeof(TOuter).GetProperties().Where(pi => pi.Name != innerListPropertyName).ToArray();
            PropertyInfo[] innerProperties = typeof(TInner).GetProperties();
            MethodInfo innerListGetter = typeof(TOuter).GetProperty(innerListPropertyName).GetGetMethod(true);



            DataTable table = new DataTable();
            foreach (PropertyInfo pi in outerProperties)
            {
                table.Columns.Add(pi.Name, Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType);
            }
            foreach (PropertyInfo pi in innerProperties)
            {
                table.Columns.Add(pi.Name, Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType);
            }



            // iterate through outer items
            foreach (TOuter outerItem in list)
            {
                var innerList = innerListGetter.Invoke(outerItem, null) as IEnumerable<TInner>;
                if (innerList == null || innerList.Count() == 0)
                {
                    // outer item has no inner items
                    DataRow row = table.NewRow();
                    foreach (PropertyInfo pi in outerProperties)
                    {
                        row[pi.Name] = pi.GetValue(outerItem, null) ?? DBNull.Value;
                    }
                    table.Rows.Add(row);
                }
                else
                {
                    // iterate through inner items
                    foreach (object innerItem in innerList)
                    {
                        DataRow row = table.NewRow();
                        foreach (PropertyInfo pi in outerProperties)
                        {
                            row[pi.Name] = pi.GetValue(outerItem, null) ?? DBNull.Value;
                        }
                        foreach (PropertyInfo pi in innerProperties)
                        {
                            row[pi.Name] = pi.GetValue(innerItem, null) ?? DBNull.Value;
                        }
                        table.Rows.Add(row);
                    }
                }
            }



            return table;
        }
        public List<string> ToEmailIs(string syncMessage)
        {
            var toEMailIds = new List<string>();
            switch (syncMessage)
            {
                case ConsoleSettings.DirectTradeTiketFolder:
                    toEMailIds = "awlsauda@adaniwilmar.in,awlsauda@gmail.com".Split(',').ToList();
                    break;
                case ConsoleSettings.DirectSaudaFolder:
                    toEMailIds = "awlsauda@adaniwilmar.in,awlsauda@gmail.com".Split(',').ToList();
                    break;
                case ConsoleSettings.DODeleteFolder:
                    toEMailIds = "awlsauda@adaniwilmar.in,awlsauda@gmail.com".Split(',').ToList();
                    break;
                case ConsoleSettings.DOUpdateFolder:
                    toEMailIds = "awlsauda@adaniwilmar.in,awlsauda@gmail.com".Split(',').ToList();
                    break;
                case ConsoleSettings.InvoiceFolder:
                    toEMailIds = "awlsauda@adaniwilmar.in,awlsauda@gmail.com".Split(',').ToList();
                    break;
                case ConsoleSettings.InvoiceCancelAndReturnFolder:
                    toEMailIds = "awlsauda@adaniwilmar.in,awlsauda@gmail.com".Split(',').ToList();
                    break;
                case ConsoleSettings.LiftingInquiry:
                    toEMailIds = "awlsauda@adaniwilmar.in,awlsauda@gmail.com".Split(',').ToList();
                    break;
                case ConsoleSettings.LiftingRequestFolder:
                    toEMailIds = "awlsauda@adaniwilmar.in,awlsauda@gmail.com".Split(',').ToList();
                    break;
                case ConsoleSettings.InvoicePaymentStatus:
                    toEMailIds = "awlsauda@adaniwilmar.in,awlsauda@gmail.com".Split(',').ToList();
                    break;
                case ConsoleSettings.SaudaFolder:
                    toEMailIds = "awlsauda@adaniwilmar.in,awlsauda@gmail.com".Split(',').ToList();
                    break;
                case ConsoleSettings.SaudaAmendmentFolder:
                    toEMailIds = "awlsauda@adaniwilmar.in,awlsauda@gmail.com".Split(',').ToList();
                    break;
                case ConsoleSettings.SaudaApproval:
                    toEMailIds = "awlsauda@adaniwilmar.in,awlsauda@gmail.com".Split(',').ToList();
                    break;
                case ConsoleSettings.SaudaLimitFolder:
                    toEMailIds = "awlsauda@adaniwilmar.in,awlsauda@gmail.com".Split(',').ToList();
                    break;
                case ConsoleSettings.TradeTicketFolder:
                    toEMailIds = "awlsauda@adaniwilmar.in,awlsauda@gmail.com".Split(',').ToList();
                    break;
                case ConsoleSettings.SaudaConversionSubject:
                    toEMailIds = "awlsauda@adaniwilmar.in,awlsauda@gmail.com".Split(',').ToList();
                    break;
                default:
                    toEMailIds = "awlsauda@adaniwilmar.in,awlsauda@gmail.com".Split(',').ToList();
                    break;
            }
            return toEMailIds;
        }

        public ResultDto SftpSendEmail(List<string> toEmailIds, string subject, string plainBody = "", string qrCode = "", bool isHtml = false, string htmlContent = "", List<string> filePaths = null, bool isCc = false, List<string> ccEmailId = null)
        {
            _methodName = "SendEmail";
            var message = $"{ServiceName} Service-Method {_methodName}";
            _logger.Info(message + " Email Start");
            var resultDto = new ResultDto();
            try
            {
                if (ConsoleSettings.IsEmail)
                {
                    htmlContent = htmlContent.Replace("[cid:logo]", ConfigurationManager.AppSettings["EmailFooterImageUrl"]);
                    htmlContent = htmlContent.Replace("[cid:logo2]", ConfigurationManager.AppSettings["EmailFooterImageUrl1"]);

                    if (toEmailIds != null && toEmailIds.Count > 0)
                    {
                        string tomails = string.Join(",", toEmailIds);

                        using (MailMessage mailMessage = new MailMessage())
                        {
                            MailAddress mailFrom = new MailAddress(Constants.SmtpFromMailAddress);
                            mailMessage.From = mailFrom;
                            mailMessage.Subject = subject;
                            mailMessage.Body = htmlContent;
                            mailMessage.IsBodyHtml = true;
                            mailMessage.To.Add(tomails);
                            if (!string.IsNullOrEmpty(Constants.CCEmail))
                            {
                                mailMessage.CC.Add(Constants.CCEmail);
                            }

                            ////create Alrternative HTML view
                            //AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mailMessage.Body, null, "text/html");
                            ////Add Image
                            //LinkedResource theEmailImage = new LinkedResource(ConfigurationManager.AppSettings["EmailFooterImageUrl"]);
                            //theEmailImage.ContentId = "footer";

                            ////Add the Image to the Alternate view
                            //htmlView.LinkedResources.Add(theEmailImage);

                            ////Add view to the Email Message
                            //mailMessage.AlternateViews.Add(htmlView);
                            if (filePaths != null)
                            {
                                foreach (var filePath in filePaths)
                                {
                                    var attachment = new System.Net.Mail.Attachment(filePath);
                                    mailMessage.Attachments.Add(attachment);
                                }
                            }
                            SmtpClient smtp = new SmtpClient()
                            {
                                Host = Constants.SmtpHostServerName,
                                Port = Convert.ToInt32(Constants.SmtpNetworkCredentialPort),
                                EnableSsl = Constants.SmtpEnableSsl,
                                UseDefaultCredentials = true
                            };

                            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential()
                            {
                                UserName = Constants.SmtpNetworkCredentialUserName,
                                Password = Constants.SmtpNetworkCredentialPassword
                            };

                            smtp.Credentials = NetworkCred;
                            smtp.Send(mailMessage);
                            //Task.Run(() => smtp.Send(mailMessage));
                            _logger.Info("Email Fired " + mailMessage);
                        }

                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = Constants.EmailSendSuccessfully;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.EmailSendError;
                    }
                }
            }
            catch (Exception exception)
            {
                message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;

            }
            return resultDto;
        }

        #endregion        

        #region Email Send
        public ResultDto SendEmail(List<string> toEmailIds, string subject, string plainBody = "", string htmlContent = "", bool isHtml = false, string qrCode = "", bool isAttachment = false, string filePath = "", bool isCc = false, List<string> ccEmailId = null)
        {
            _methodName = "SendEmail";
            var message = $"{ServiceName} Service-Method {_methodName}";
            _logger.Info(message + " Email Start");
            var resultDto = new ResultDto();
            try
            {
                if (ConsoleSettings.IsEmail)
                {
                    htmlContent = htmlContent.Replace("[cid:logo]", ConfigurationManager.AppSettings["EmailFooterImageUrl"]);
                    //htmlContent = htmlContent.Replace("cid:logo2", ConfigurationManager.AppSettings["EmailFooterImageUrl1"]);

                    if (toEmailIds != null && toEmailIds.Count > 0)
                    {
                        string tomails = string.Join(",", toEmailIds);

                        using (MailMessage mailMessage = new MailMessage())
                        {
                            MailAddress mailFrom = new MailAddress(Constants.SmtpFromMailAddress);
                            mailMessage.From = mailFrom;
                            mailMessage.Subject = subject;
                            mailMessage.Body = htmlContent;
                            mailMessage.IsBodyHtml = true;
                            mailMessage.To.Add(tomails);
                            if (!string.IsNullOrEmpty(Constants.CCEmail))
                            {
                                mailMessage.CC.Add(Constants.CCEmail);
                            }

                            ////create Alrternative HTML view
                            //AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mailMessage.Body, null, "text/html");
                            ////Add Image
                            //LinkedResource theEmailImage = new LinkedResource(ConfigurationManager.AppSettings["EmailFooterImageUrl"]);
                            //theEmailImage.ContentId = "footer";

                            ////Add the Image to the Alternate view
                            //htmlView.LinkedResources.Add(theEmailImage);

                            ////Add view to the Email Message
                            //mailMessage.AlternateViews.Add(htmlView);
                            if (filePath != null && filePath != "")
                            {
                                var attachment = new System.Net.Mail.Attachment(filePath);
                                mailMessage.Attachments.Add(attachment);
                            }
                            SmtpClient smtp = new SmtpClient()
                            {
                                Host = Constants.SmtpHostServerName,
                                Port = Convert.ToInt32(Constants.SmtpNetworkCredentialPort),
                                EnableSsl = Constants.SmtpEnableSsl,
                                UseDefaultCredentials = true
                            };

                            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential()
                            {
                                UserName = Constants.SmtpNetworkCredentialUserName,
                                Password = Constants.SmtpNetworkCredentialPassword
                            };

                            smtp.Credentials = NetworkCred;
                            smtp.Send(mailMessage);
                            //Task.Run(() => smtp.Send(mailMessage));
                            _logger.Info("Email Fired " + mailMessage);
                        }

                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = Constants.EmailSendSuccessfully;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.EmailSendError;
                    }
                }
            }
            catch (Exception exception)
            {
                message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;

            }
            return resultDto;
        }

        public async Task<ResultDto> SendEmailAsync(List<string> toEmailIds, string subject, string plainBody = "", string htmlContent = "", bool isHtml = false, string qrCode = "", bool isAttachment = false, string filePath = "", bool isCc = false, List<string> ccEmailId = null)
        {
            _methodName = "SendEmail";
            var message = $"{ServiceName} Service-Method {_methodName}";
            _logger.Info(message + "Email Start");
            var resultDto = new ResultDto();
            try
            {
                if (ConsoleSettings.IsEmail)
                {
                    htmlContent = htmlContent.Replace("cid:footer", ConfigurationManager.AppSettings["EmailFooterImageUrl"]);
                    if (Constants.AwsEmail)
                    {
                        var fromEmail = Constants.FromEmail;
                        //var fromDisplayName = Constants.FromDisplayName;
                        var amazonS3Config = new AmazonSimpleEmailServiceConfig();
                        var newRegion = RegionEndpoint.GetBySystemName(Constants.AWSRegionName);
                        amazonS3Config.RegionEndpoint = newRegion;

                        using (var client = new AmazonSimpleEmailServiceClient(Constants.AWSEmailAccessKey, Constants.AWSEmailSecretKey, amazonS3Config))
                        {

                            var emailRequest = new SendEmailRequest()
                            {
                                Source = fromEmail,
                                Destination = new Destination(),
                                Message = new Message(),
                            };

                            var body = new Body()
                            {
                                Html = new Content(htmlContent),
                                Text = new Content(plainBody),

                            };

                            foreach (var toMailId in toEmailIds)
                            {
                                emailRequest.Destination.ToAddresses.Add(toMailId);
                            }

                            if (isCc && ccEmailId != null)
                            {
                                foreach (var cc in ccEmailId)
                                {
                                    emailRequest.Destination.CcAddresses.Add(cc);
                                }
                            }
                            emailRequest.Message.Subject = new Content(subject);
                            emailRequest.Message.Body = body;
                            await client.SendEmailAsync(emailRequest);
                            _logger.Info("Email Fired " + emailRequest);

                            resultDto.IsSuccess = true;
                            resultDto.SuccessDto.Response = Constants.EmailSendSuccessfully;
                        }
                    }
                    else
                    {
                        if (toEmailIds != null && toEmailIds.Count > 0)
                        {
                            string tomails = string.Join(",", toEmailIds);

                            using (MailMessage mailMessage = new MailMessage())
                            {
                                MailAddress mailFrom = new MailAddress(Constants.SmtpFromMailAddress);
                                mailMessage.From = mailFrom;
                                mailMessage.Subject = subject;
                                mailMessage.Body = htmlContent;
                                mailMessage.IsBodyHtml = true;
                                mailMessage.To.Add(tomails);

                                ////create Alrternative HTML view
                                //AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mailMessage.Body, null, "text/html");
                                ////Add Image
                                //LinkedResource theEmailImage = new LinkedResource(ConfigurationManager.AppSettings["EmailFooterImageUrl"]);
                                //theEmailImage.ContentId = "footer";

                                ////Add the Image to the Alternate view
                                //htmlView.LinkedResources.Add(theEmailImage);

                                ////Add view to the Email Message
                                //mailMessage.AlternateViews.Add(htmlView);

                                SmtpClient smtp = new SmtpClient()
                                {
                                    Host = Constants.SmtpHostServerName,
                                    Port = Convert.ToInt32(Constants.SmtpNetworkCredentialPort),
                                    EnableSsl = Constants.SmtpEnableSsl,
                                    UseDefaultCredentials = true
                                };

                                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential()
                                {
                                    UserName = Constants.SmtpNetworkCredentialUserName,
                                    Password = Constants.SmtpNetworkCredentialPassword
                                };

                                smtp.Credentials = NetworkCred;
                                await smtp.SendMailAsync(mailMessage);
                                //Task.Run(() => smtp.Send(mailMessage));
                                _logger.Info("Email Fired " + mailMessage);
                            }

                            resultDto.IsSuccess = true;
                            resultDto.SuccessDto.Response = Constants.EmailSendSuccessfully;
                        }
                        else
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Message = Constants.EmailSendError;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.Error("Email exception");
                message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;

            }
            _logger.Info("Email End");
            return resultDto;
        }
        #endregion

        #region Pushnotification
        public string CreatePushNotificationEndpointForDevice(string deviceId)
        {
            var message = $"{ServiceName} Service-Method {_methodName}";
            _logger.Info(message + "Push Notification End Point Start");
            string applicationARN = Constants.applicationARNForPushNotification;
            var resp = _client.CreatePlatformEndpoint(new CreatePlatformEndpointRequest() { PlatformApplicationArn = applicationARN, Token = deviceId });
            return resp.EndpointArn;
        }

        public void SendPush(string endpointArn, string msg)
        {
            var message = $"{ServiceName} Service-Method {_methodName}";
            _logger.Info(message + "Push Notification Start");
            var pushMsg = new PublishRequest();
            pushMsg.Message = msg;
            pushMsg.TargetArn = endpointArn;
            Task.Run(() => _client.Publish(pushMsg));
            _logger.Info("Push Notification End");
        }

        private ResultDto SendPushNotification(bool iosDevice, string topic, string title, string body)
        {
            _methodName = "SendPushNotification";
            var message = $"{ServiceName} Service-Method {_methodName}";
            _logger.Info(message + "Push Notification Start");
            var resultDto = new ResultDto();
            try
            {
                // this endpoint is for android devices   
                var gcmARN = "arn:aws:sns:us-east-1:501401665234:endpoint/GCM/nameoftopic/***********************";
                // this endpoint is for ios devices   
                var apnsARN = "arn:aws:sns:us-east-1:502682123213:endpoint/APNS_SANDBOX/nameoftopic/*************";
                var checkedButton = iosDevice;
                // Creating the SNS client   
                var snsClient = new AmazonSimpleNotificationServiceClient();
                // Creating the topic request and the topic and response  
                var topicRequest = new CreateTopicRequest
                {
                    Name = topic
                };
                var topicResponse = snsClient.CreateTopic(topicRequest);
                var topicAttrRequest = new SetTopicAttributesRequest
                {
                    TopicArn = topicResponse.TopicArn,
                    AttributeName = "SNSTopic",
                    AttributeValue = "SNS Test AttrValue"
                };
                snsClient.SetTopicAttributes(topicAttrRequest);
                // Subscribe to the endpoint of the topic  
                var subscribeRequest = new SubscribeRequest()
                {
                    TopicArn = topicResponse.TopicArn,
                    Protocol = "application", // important to chose the protocol as I am sending notification to applications I have chosen application here.  
                    Endpoint = iosDevice ? apnsARN : gcmARN
                };
                var res = snsClient.Subscribe(subscribeRequest);
                // Publishing the request to the endpoint (takecare of the protocol that is must is sending the json then use json else use sns, email, sqs etc. as per your requirement)   
                PublishRequest publishReq = new PublishRequest()
                {
                    TargetArn = subscribeRequest.Endpoint,
                    MessageStructure = "json",
                    Message = body
                };
                Task.Run(() => snsClient.Publish(publishReq));
                //PublishResponse response = ;
                //if (response != null && response.MessageId != null)
                //{
                //        resultDto.IsSuccess = true;
                //        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                //}
            }
            catch (Exception exception)
            {
                _logger.Error("Push Notification exception");
                message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;

            }
            _logger.Info("Push Notification End");
            return resultDto;
        }
        #endregion

        #region Send SMS
        public string SendMessage(string smsMessage, string contactNumber, string TemplateId = "")
        {
            _methodName = "SendMessage";
            var message = $"{ServiceName} Service-Method {_methodName}";
            _logger.Info(message + "Send SMS Start");
            try
            {

                if (ConsoleSettings.IsSMS)
                {
                    if (!string.IsNullOrEmpty(TemplateId))
                    {
                        var httpClient = new HttpClient();

                        // Basic Auth header
                        var authString = $"{Constants.AirtelSmsUserName}:{Constants.AirtelSmsPassword}";
                        var authBytes = System.Text.Encoding.UTF8.GetBytes(authString);
                        httpClient.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue(
                                "Basic", Convert.ToBase64String(authBytes));

                        var payload = new
                        {
                            customerId = Constants.AirtelSmsUserName,
                            destinationAddress = contactNumber,
                            message = smsMessage,
                            sourceAddress = Constants.SmsSourceAddress,
                            messageType = Constants.SmsMessageType,
                            dltTemplateId = TemplateId,
                            entityId = Constants.Smsentity_id
                        };

                        var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                        var response = httpClient.PostAsync(Constants.SmsOtpUrl, content).Result;
                        return response.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.Error("Send SMS exception");
                message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);

            }
            _logger.Info("Send SMS End");
            return string.Empty;
        }

        public async Task<ResultDto> SendMessageAsync(string smsMessage, string contactNumber, string TemplateId = "")
        {
            var result = new ResultDto();
            _methodName = "SendMessageAsync";
            var message = $"{ServiceName} Service-Method {_methodName}";
            _logger.Info(message + "Send SMS Start");
            try
            {
                if (ConsoleSettings.IsSMS)
                {
                    if (!string.IsNullOrEmpty(TemplateId))
                    {
                        var httpClient = new HttpClient();
                        var SmsUrl = $"http://msg.cellapps.com/API/WebSMS/Http/v1.0a/index.php?username={Constants.SmsCodeZUserName}&password={Constants.SmsCodeZPassword}&sender=AWLSMS&to={contactNumber}&message={smsMessage}&reqid=1&format=TXT&route_id=&Template_ID={TemplateId}&PE_ID={Constants.PEID}";
                        var response = httpClient.GetAsync(SmsUrl).Result;
                        await response.Content.ReadAsStringAsync();
                        result.IsSuccess = true;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.Error("Send SMS exception");
                message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
            }
            _logger.Info("Send SMS End");
            return result;
        }

        #endregion
    }
}
