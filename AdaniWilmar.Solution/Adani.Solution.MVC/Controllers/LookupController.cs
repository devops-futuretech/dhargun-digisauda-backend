using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.DTO.Common;
using Adani.Solution.DTO.Enums;
using Adani.Solution.MVC.Attributes;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.Models;
using Adani.Solution.MVC.ServiceClient;
using GMCore.Helper;
using GMCore.Logger;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http.Description;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Configuration;
using WebGrease.Css.Extensions;

namespace Adani.Solution.MVC.Controllers
{
    [TokenAuthorize]
    [CustomRedirect]
    [NoCache]
    public class LookupController : BaseController
    {
        private readonly MasterClient _masterClient;
        private readonly LookupClient _lookupClient;
        private readonly SupportClient _supportClient;
        private readonly MediaClient _mediaClient;
        private const string controllerName = "Lookup Controller";
        private readonly ILogger _logger = Logging.GetLogger(controllerName);
        private string _methodName;


        public LookupController()
        {
            _masterClient = new MasterClient { ControllerDelegate = this };
            _lookupClient = new LookupClient { ControllerDelegate = this };
            _supportClient = new SupportClient { ControllerDelegate = this };
            _mediaClient = new MediaClient { ControllerDelegate = this };
        }

        //[AuthorizeRoles(Role.Admin)]
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Master()
        {
            return View();
        }

        #region Dealer/Direct Party

        /// <summary>
        /// Method to Get Dealer List page
        /// </summary>
        /// <returns></returns>

        [AuthorizeClaims(Claims.ManageDealer, Claims.ViewDealer)]
        public ActionResult DealerList()
        {
            return View();
        }

        /// <summary>
        /// Method to Get Dealer List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetDealerListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            //LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData };
            //var dealersList = await _masterClient.GetDealerListAsync(loginUserIdDto);
            //var resultList = dealersList.ToDataSourceResult(request);
            //return Json(resultList);
            
            
            return Json(await _masterClient.GetKendoGridDataAsync<DealerDto>(GridResultInputDto(request, isToReturnInactiveData), ApiUrl.WebApiUrlGetDealerList));
        }


        /// <summary>
        /// Method to redirect dealer add or update page
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public ActionResult DERedirect(string EncryptedId = "")
        {
            Session["DealerId"] = EncryptedId;
            return RedirectToAction("Dealer", "Lookup");
        }

        public async  Task<ActionResult> DateRangeAdd(DateRangeDTO dateRange)
        {
            var result = new DateRangeDTO();
            result = await _masterClient.GetDateRange(UserId);
            result.PostMessage = dateRange.PostMessage;
            result.PostStatus = dateRange.PostStatus;
           return View(result);
        }

        public async Task<ActionResult> AddOrUpdateDateRange(DateRangeDTO date)
        {
            date.LoginUserId = UserId;
            date.IsActive = true;
            var resultDto =await _masterClient.AddDateRange(date);
           
            if (resultDto.PostStatus)
            {
                TempData["SuccessMessage"] = resultDto.PostMessage;
                var daterange = new DateRangeDTO()
                {
                    PostMessage = resultDto.PostMessage,
                    PostStatus = resultDto.PostStatus
                };
                return RedirectToAction("DateRangeAdd", "Lookup", resultDto);
            }
            else
            {
                TempData["SuccessMessage"] = resultDto.PostMessage;
                return RedirectToAction("DateRangeAdd", "Lookup", resultDto);
            }
            //return RedirectToAction("DateRangeAdd","Lookup",resultDto);
        }

        /// <summary>
        /// Method to get dealer add or update page
        /// </summary>
        /// <returns></returns>        
        [AuthorizeClaims(Claims.ManageDealer, Claims.ViewDealer)]
        public async Task<ActionResult> Dealer()
        {
            var result = new EmployeeDto();
            Session["SelectedDepotIds"] = null;
            Session["SelectedDealerIds"] = null;
            if (!String.IsNullOrEmpty(Session["DealerId"].ToString()))
            {
                result = await _masterClient.GetDealerDetailsById(Session["DealerId"].ToString());
                if (result.SelectedDealerBrokerIds != null && result.SelectedDealerBrokerIds.Any())
                {
                    result.SelecteDealerBrokerIdsString = UtilityHelper.ConvertLongListToCommaSeparatedString(result.SelectedDealerBrokerIds);
                    Session["SelectedDepotIds"] = result.SelectedDealerBrokerIds;
                }
                if (result.SelectedDealerIds != null && result.SelectedDealerIds.Any())
                {
                    result.SelecteDealerIdsString = UtilityHelper.ConvertLongListToCommaSeparatedString(result.SelectedDealerIds);
                    Session["SelectedDealerIds"] = result.SelectedDealerIds;
                }
            }
            return View(result);
        }

        /// <summary>
        /// Method to add or update dealer
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateDealer(EmployeeDto inputDto, IEnumerable<HttpPostedFileBase> files, string DivisionList="")
        //public async Task<ActionResult> AddOrUpdateDealer(string input)
        {

            inputDto.DivisionList = GMCore.Helper.JsonHelper.ConvertJSonToObjectList<DivisionDetailsDto>(DivisionList);
            //var inputDto = GMCore.Helper.JsonHelper.ConvertJSonToObject<EmployeeDto>(input);
            //var files = GMCore.Helper.JsonHelper.ConvertJSonToObject<IEnumerable<HttpPostedFileBase>>(files1);

            //if (!string.IsNullOrEmpty(PickupLocation))
            //{
            //    inputDto.PickupLocation = JsonConvert.DeserializeObject<List<PickUpLoationsDto>>(PickupLocation);
            //}
            if (files != null)
            {
                var fileSizeResult = _supportClient.CheckImageSizeAndType(files);
                if (!fileSizeResult.IsSuccess)
                {
                    inputDto.PostStatus = false;
                    inputDto.PostMessage = fileSizeResult.ErrorDto.Message;
                }
            }

            inputDto.LoginUserId = UserId;
            inputDto.RoleId = (int)Role.Dealer;
            //inputDto.IsDealer = true;
            var result = await _masterClient.AddOrUpdateDealer(inputDto, files);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("DealerList", "Lookup");
            }
            return View("Dealer", result);
        }

        public async Task<ActionResult> DeleteConsentImageAsync(string consentImageId, string fileName)
        {
            var result = new BulletinDto();
            if (consentImageId != null && UtilityHelper.IntTryToParse(consentImageId) > 0)
            {
                result = await _masterClient.DeleteConsentImageAsync(UtilityHelper.IntTryToParse(consentImageId), UserId);
                if (result.PostStatus)
                {
                    var folderName = Enum.GetName(typeof(PageType), 7);
                    _mediaClient.DeleteFile(fileName, folderName);
                    var successMessage = Helper.GetResourceString("msg_DeleteMediaSuccessful");
                    result.PostStatus = true;
                    result.PostMessage = successMessage;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GenerateExcelDealerAsync()
        {
            var stream = new MemoryStream();
            var result = new ResultModel { IsSuccess = false, Message = "Error Occured while exporting Excel. Please retry." };
            string guidFileName = "";
            string fileName = "";


            try
            {
                var dealerDetails = await _masterClient.GetDealerListAsync(new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = true, VerticalId = VerticalId });
                fileName = "DISTRIBUTOR-LIST-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
                //var fileName = $"{Guid.NewGuid()}.xlsx";


                // Create the package and make sure you wrap it in a using statement

                using (var ep = new ExcelPackage())
                {

                    var ws = ep.Workbook.Worksheets.Add("Dealer List");

                    #region Header
                    ws.Cells["A1:F1"].Merge = true;
                    ws.Cells["A1:F1"].Value = Settings.CompanyName;
                    ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["A1:F1"].Style.Font.Bold = true;
                    ws.Cells["A1:F1"].Style.Font.Size = 16;

                    ws.Cells["A2"].Value = "Sheet Name";
                    ws.Cells["A3"].Value = "Total Record Count";
                    ws.Cells["A4"].Value = "Date and Time";



                    for (int j = 2; j <= 5; j++)
                    {
                        ws.Cells["A" + j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["A" + j].Style.Font.Bold = true;
                        ws.Cells["A" + j].Style.Font.Size = 12;

                        ws.Cells["B" + j + ":" + "F" + j].Merge = true;
                        ws.Cells["B" + j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    }



                    ws.Cells["B2"].Value = "Dealer List";
                    ws.Cells["B3"].Value = dealerDetails.Count;
                    ws.Cells["B4"].Value = DateHelper.UtcToIndia(DateTime.UtcNow).ToString("dd-MM-yyyy HH:mm tt");
                    ws.Cells["A4"].Style.Font.Bold = true;
                    ws.Cells["A4"].Style.Font.Size = 12;



                    #endregion


                    ws.Cells["A7:Z" + dealerDetails.Count].LoadFromCollection(dealerDetails, true);
                    ExcelRange range = ws.Cells["A7:Z7"];
                    range.AutoFitColumns();
                    range.Style.Font.Size = 12;
                    range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                    range.Style.Font.Bold = true;
                    int contentIndex = 8;

                    ws.Cells["A7" + ":" + "Z" + contentIndex + dealerDetails.Count].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    ws.Cells.AutoFitColumns();


                    #endregion

                    guidFileName = SaveExcelFileToPath(ep);
                }
                
                //}
                result.IsSuccess = true;
                result.Message = fileName;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "Excel Error" + ex;
            }




            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DealerorBrokerEditRedirect(string role = "", string brokerIdOrDealerId = "")
        {
            if (role == UtilityHelper.GetEnumDescription(DTO.Enums.Role.Dealer))
            {
                Session["DealerId"] = brokerIdOrDealerId;
                return RedirectToAction("Dealer", "Lookup");
            }
            else
            {
                Session["BrokerId"] = brokerIdOrDealerId;
                return RedirectToAction("Broker", "Lookup");
            }

        }

        

        #region Broker

        /// <summary>
        /// Method to Get Broker List page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageBroker, Claims.ViewBroker)]
        public ActionResult BrokerList()
        {
            return View();
        }

        /// <summary>
        /// Method to Get Broker List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetBrokerListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData, VerticalId = VerticalId };
            var result = await _masterClient.GetBrokerListAsync(loginUserIdDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        /// <summary>
        /// Method to Get Broker List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetBrokerListddlAsync(string verticalId/*, long saudaBookingTypeId*/)
        {
            IList<DropDownDto> dropdownDto = new List<DropDownDto>();
            var verticalIds = GMCore.Helper.UtilityHelper.ConvertStringToLongList(verticalId);
            if (verticalId != string.Empty /*&& saudaBookingTypeId > 0*/)
            {
                var inputDto = new DealerBrokerParamDto() { DivisionIds = verticalIds, SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess };
                dropdownDto = await _masterClient.GetBrokerListddlAsync(inputDto);
            }
            return Json(dropdownDto, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to redirect broker add or update page
        /// </summary>
        /// <param name="brokerId"></param>
        /// <returns></returns>
        public ActionResult BERedirect(string EncryptedId = "")
        {
            Session["BrokerId"] = EncryptedId;
            return RedirectToAction("Broker", "Lookup");
        }

        /// <summary>
        /// Method to get broker add or update page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageBroker, Claims.ViewBroker)]
        public async Task<ActionResult> Broker()
        {
            var result = new EmployeeDto();
            Session["SelectedDepotIds"] = null;
            Session["SelectedDealerIds"] = null;
            if (!String.IsNullOrEmpty(Session["BrokerId"].ToString()))
            {
                result = await _masterClient.GetBrokerDetailsById(Session["BrokerId"].ToString());
                if (result.SelectedDealerBrokerIds != null && result.SelectedDealerBrokerIds.Any())
                {
                    result.SelecteDealerBrokerIdsString = UtilityHelper.ConvertLongListToCommaSeparatedString(result.SelectedDealerBrokerIds);
                    Session["SelectedDepotIds"] = result.SelectedDealerBrokerIds;
                }
                if (result.SelectedDealerIds != null && result.SelectedDealerIds.Any())
                {
                    result.SelecteDealerIdsString = UtilityHelper.ConvertLongListToCommaSeparatedString(result.SelectedDealerIds);
                    Session["SelectedDealerIds"] = result.SelectedDealerIds;
                }
            }
            return View(result);
        }

        /// <summary>
        /// Method to add or update Broker
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateBroker(EmployeeDto inputDto, IEnumerable<HttpPostedFileBase> files, string DivisionList = "")
        {
            //if (!string.IsNullOrEmpty(PickupLocation))
            //{
            //    inputDto.PickupLocation = JsonConvert.DeserializeObject<List<PickUpLoationsDto>>(PickupLocation);
            //}
            inputDto.DivisionList = GMCore.Helper.JsonHelper.ConvertJSonToObjectList<DivisionDetailsDto>(DivisionList);
            if (files != null)
            {
                var fileSizeResult = _supportClient.CheckImageSizeAndType(files);
                if (!fileSizeResult.IsSuccess)
                {
                    inputDto.PostStatus = false;
                    inputDto.PostMessage = fileSizeResult.ErrorDto.Message;
                }
            }
            inputDto.LoginUserId = UserId;
            inputDto.RoleId = (int)Role.Broker;
            //inputDto.IsBroker = true;
            var result = await _masterClient.AddOrUpdateBroker(inputDto, files);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("BrokerList", "Lookup");
            }
            return View("Broker", result);
        }
        [HttpPost]
        public async Task<ActionResult> ProfileUpload(IEnumerable<HttpPostedFileBase> files)
        {
            var inputDto = new EmployeeDto();
            if (files != null)
            {
                var fileSizeResult = _supportClient.CheckImageSizeAndType(files);
                if (!fileSizeResult.IsSuccess)
                {
                   
                    inputDto.PostStatus = false;
                    inputDto.PostMessage = fileSizeResult.ErrorDto.Message;
                    TempData["ErrorMessage"] = fileSizeResult.ErrorDto.Message;
                    return RedirectToAction("Index", "Home");
                }
            }
            inputDto.LoginUserId = UserId;
            var result = await _masterClient.UploadProfilePhoto(inputDto, files);

            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = result.PostMessage;
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Secure download handler for profile photos.
        /// Files are stored outside web root and served through this handler to prevent direct access.
        /// This prevents RCE and XSS attacks by ensuring files cannot be executed as scripts.
        /// </summary>
        /// <param name="file">GUID filename of the profile photo</param>
        /// <returns>Image file or 404 if not found</returns>
        public ActionResult DownloadProfilePhoto(string file)
        {
            try
            {
                // Security: Validate input - prevent path traversal
                if (string.IsNullOrWhiteSpace(file))
                {
                    _logger.Warn($"{controllerName} DownloadProfilePhoto: Empty filename parameter");
                    return HttpNotFound();
                }

                // Security: Sanitize filename - remove any path separators or dangerous characters
                var safeFileName = Path.GetFileName(file);
                if (string.IsNullOrWhiteSpace(safeFileName) || safeFileName != file)
                {
                    _logger.Warn($"{controllerName} DownloadProfilePhoto: Path traversal attempt detected: {file}");
                    return HttpNotFound();
                }

                // Security: Validate extension is safe (should only be .jpg, .jpeg, .png)
                var ext = Path.GetExtension(safeFileName)?.ToLowerInvariant() ?? string.Empty;
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                if (!allowedExtensions.Contains(ext))
                {
                    _logger.Warn($"{controllerName} DownloadProfilePhoto: Invalid extension: {ext} for file: {file}");
                    return HttpNotFound();
                }

                // Security: Get secure directory (outside web root)
                string secureDirectory;
                var configured = System.Configuration.ConfigurationManager.AppSettings["UploadMediaSecurePath"];
                if (!string.IsNullOrWhiteSpace(configured) && Path.IsPathRooted(configured))
                {
                    secureDirectory = Path.Combine(configured, "ProfilePhotos");
                }
                else
                {
                    // Fallback to App_Data (not served by IIS static handler)
                    secureDirectory = HostingEnvironment.MapPath("~/App_Data/Uploads/ProfilePhotos");
                    if (string.IsNullOrWhiteSpace(secureDirectory))
                    {
                        secureDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Uploads", "ProfilePhotos");
                    }
                }

                var filePath = Path.Combine(secureDirectory, safeFileName);

                // Security: Additional path validation - ensure file is within the secure directory
                if (!filePath.StartsWith(secureDirectory, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.Warn($"{controllerName} DownloadProfilePhoto: Path traversal attempt: {filePath}");
                    return HttpNotFound();
                }

                // Check if file exists
                if (!System.IO.File.Exists(filePath))
                {
                    _logger.Warn($"{controllerName} DownloadProfilePhoto: File not found: {safeFileName}");
                    return HttpNotFound();
                }

                // Security: Determine content type based on extension (not user input)
                string contentType;
                switch (ext)
                {
                    case ".jpg":
                    case ".jpeg":
                        contentType = "image/jpeg";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                    default:
                        contentType = "application/octet-stream";
                        break;
                }

                // Read file and return with appropriate content type
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, contentType);
            }
            catch (Exception ex)
            {
                _logger.Error($"{controllerName} DownloadProfilePhoto: Error serving file {file}: {ex}");
                return HttpNotFound();
            }
        }


        public async Task<ActionResult> GenerateExcelBrokerAsync()
        {
            var stream = new MemoryStream();
            var result = new ResultModel { IsSuccess = false, Message = "Error Occured while exporting Excel. Please retry." };
            try
            {
                var brokerDetails = await _masterClient.GetBrokerListAsync(new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = true, VerticalId = VerticalId });

                //var fileName = $"{Guid.NewGuid()}.xlsx";                
                var fileName = "BROKER-LIST-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BrokerCode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BrokerName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_MobileNumber"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_MobileNumber2"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Email"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CompanyCode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Zone"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_State"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Territory"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_District"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_City"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Pincode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Address"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_FreightZoneName"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_FreightRouteName"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_TransportMode"));
                   // GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Loadability"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_GSTN"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IncoTerms"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SaudaBookingType"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SaudaLimit"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SaudaValidityPeriod"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Vertical"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PlantTruckCapacity"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DepotTruckCapacity"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Plant"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Depot"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_FssaiNumber"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BDO"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BDOCode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_MappedDealers"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Password"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PlantName"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DepotName"));

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IsActive"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_AdditionalMobileNumber"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Active For Call");
                   // GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ContactPersonName"));

                    foreach (var broker in brokerDetails)
                    {
                        rowIndex++;
                        colIndex = 1;
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.BrokerCode != null ? broker.BrokerCode.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.BrokerName != null ? broker.BrokerName.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.MobileNumber != null ? broker.MobileNumber.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.MobileNumber2 != null ? broker.MobileNumber2.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.Email != null ? broker.Email.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.CompanyCode != null ? broker.CompanyCode.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.Zone != null ? broker.Zone.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.State != null ? broker.State.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.Territory != null ? broker.Territory.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.District != null ? broker.District.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.City != null ? broker.City.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.Pincode != null ? broker.Pincode.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.Address != null ? broker.Address.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.FreightZoneName != null ? broker.FreightZoneName.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.FreightRouteName != null ? broker.FreightRouteName.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.TransportMode != null ? broker.TransportMode.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.PlantTruckCapacities != null ? broker.PlantTruckCapacities.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.GSTN != null ? broker.GSTN.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.Incoterms != null ? broker.Incoterms.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.SaudaBookingType != null ? broker.SaudaBookingType.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.SaudaLimit.ToString());
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.SaudaValidityPeriod.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.VerticalName != null ? broker.VerticalName.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.PlantTruckCapacities != null ? broker.PlantTruckCapacities.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.DepotTruckCapacities != null ? broker.DepotTruckCapacities.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.Plants != null ? broker.Plants.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.Depots != null ? broker.Depots.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.FSSAINumber != null ? broker.FSSAINumber.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.StateTrader != null ? broker.StateTrader.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.BDOCode != null ? broker.BDOCode.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.DealerCodeList != null ? broker.DealerCodeList.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.Password != null ? broker.Password.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.PlantName != null ? broker.PlantName.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.DepotName != null ? broker.DepotName.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.IsActive.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.AdditionalMobileNumber != null ? broker.AdditionalMobileNumber.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], broker.IsActiveForCall.ToString());
                       // GetExcelContent(worksheet.Cells[rowIndex, colIndex++], string.IsNullOrEmpty(broker.ContactPersonName) ? "" : broker.ContactPersonName);
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }

                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    this.Response.AddHeader(
                              "content-disposition",
                              string.Format("attachment;  filename={0}", fileName));
                    this.Response.BinaryWrite(package.GetAsByteArray());
                }
                result.IsSuccess = true;
                result.Message = fileName;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "Excel Error" + ex;
            }
            Response.Flush();
            Response.End();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region StateTrader
        public async Task<JsonResult> GetBDOListddlAsync(string stateIds)
        {
            IList<DropDownDto> dropdownDto = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(stateIds))
            {
                List<long> stateIdList = UtilityHelper.ConvertStringToLongList(stateIds);
                dropdownDto = await _masterClient.GetBDOListddlAsync(stateIdList);
            }
            return Json(dropdownDto, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetBDOOverallListddlAsync(bool isToReturnInactiveData)
        {
            IList<DropDownDto> dropdownDto = new List<DropDownDto>();
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto();
            loginUserIdDto.LoginUserId = UserId;
            dropdownDto = await _masterClient.GetOverallBDOListddlAsync(loginUserIdDto);
            return Json(dropdownDto, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Retailer / Secondary Customer

        /// <summary>
        /// Method to Get Retailer List page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public ActionResult RetailerList()
        {
            return View();
        }

        /// <summary>
        /// Method to Get Retailer List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetRetailerListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            return Json(await _masterClient.GetKendoGridDataAsync<RetailerDto>(GridResultInputDto(request, true), ApiUrl.WebApiUrlGetRetailerListWithPagination));
        }

        public async Task<ActionResult> ExportRetailerListAsync(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                IList<RetailerDto> retailerList = new List<RetailerDto>();
                retailerList = await _masterClient.GetRetailerListAsync(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "RETAILERS-" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_RetailerName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_RetailerCode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_MobileNumber"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Email"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Zone"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_State"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Territory"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_District"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_City"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Address"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Pincode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_FreightZone"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_FreightRoutes"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DistributorName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Active"));

                    ////To set top row as static
                    //worksheet.View.FreezePanes(2, 1);
                    //To implement filters
                    worksheet.Cells["A1:U1"].AutoFilter = true;

                    if (retailerList != null && retailerList.Any())
                    {
                        foreach (var retailer in retailerList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], retailer.Name != null ? retailer.Name.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], retailer.Code != null ? retailer.Code.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], retailer.MobileNumber != null ? retailer.MobileNumber.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], retailer.Email != null ? retailer.Email.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], retailer.Zone != null ? retailer.Zone.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], retailer.State != null ? retailer.State.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], retailer.TerritoryName != null ? retailer.TerritoryName.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], retailer.District != null ? retailer.District.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], retailer.City != null ? retailer.City.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], retailer.Address != null ? retailer.Address.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], retailer.Pincode != null ? retailer.Pincode.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], retailer.FreightZoneName != null ? retailer.FreightZoneName.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], retailer.FreightRouteName != null ? retailer.FreightRouteName.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], retailer.DistributorName != null ? retailer.DistributorName.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], retailer.IsActive == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                        }
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }

                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    //this.Response.Headers.Clear();
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);

                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to redirect retailer add or update page
        /// </summary>
        /// <param name="retailerId"></param>
        /// <returns></returns>
        public ActionResult RetailerEditRedirect(string retailerId = "")
        {
            Session["RetailerId"] = retailerId;
            return RedirectToAction("Retailer", "Lookup");
        }

        /// <summary>
        /// Method to get retailer add or update page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public async Task<ActionResult> Retailer()
        {
            var result = new RetailerDto();
            if (Session["RetailerId"] != null && UtilityHelper.IntTryToParse(Session["RetailerId"].ToString()) > 0)
            {
                result = await _masterClient.GetRetailerDetailsById(UtilityHelper.LongTryToParse(Session["RetailerId"].ToString()));
            }
            return View(result);
        }

        /// <summary>
        /// Method to  add or update retailer 
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateRetailer(RetailerDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _masterClient.AddOrUpdateRetailer(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("RetailerList", "Lookup");
            }
            return View("Retailer", result);
        }

        public async Task<ActionResult> GetDealerDetailsddl([DataSourceRequest] DataSourceRequest request, long freightzoneId = 0, long freightrouteId = 0)
        {
            FreightZoneAndRouteDropDownInputDto inputDto = new FreightZoneAndRouteDropDownInputDto { FreightZoneId = freightzoneId, FreightRouteId = freightrouteId };
            var ealUsersList = await _lookupClient.GetDealerDetailsddl(inputDto);
            return Json(ealUsersList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SKU(Stock Keeping Unit) Master

        /// <summary>
        /// Method to Get Sku List
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public ActionResult SkuList()
        {
            return View();
        }

        /// <summary>
        /// Method to Get Sku List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetSkuListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            return Json(await _masterClient.GetKendoGridDataAsync<SkuDto>(GridResultInputDto(request, isToReturnInactiveData), ApiUrl.WebApiUrlGetSkuListWithPagination));
        }

        /// <summary>
        /// Method to redirect Sku add or update page
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public ActionResult SERedirect(string EncryptedId = "")
        {
            Session["SkuId"] = EncryptedId;
            return RedirectToAction("Sku", "Lookup");
        }

        /// <summary>
        /// Method to get Sku add or update page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public async Task<ActionResult> Sku()
        {
            var result = new SkuDto();
            if (!String.IsNullOrEmpty(Session["SkuId"].ToString()))
            {
                result = await _masterClient.GetSkuDetailsById(Session["SkuId"].ToString());
            }
            //if (result.VerticalId <= 0)
            //    result.VerticalId = VerticalId;
            return View(result);
        }

        /// <summary>
        /// Method to add or update sku
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateSku(SkuDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _masterClient.AddOrUpdateSku(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("SkuList", "Lookup");
            }
            inputDto.PostStatus = result.PostStatus;
            inputDto.PostMessage = result.PostMessage;
            return View("Sku", inputDto);
        }

        /// <summary>
        /// Method to Get Sku List to bind in Dropdown
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetSkuListForDropdown([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData };
            var result = await _masterClient.GetSkuListAsync(loginUserIdDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private ExcelRange GetExcelTitle(ExcelRange cell, string title, int level = 0)
        {
            cell.Value = title ?? string.Empty;

            cell.Style.Border.Top.Style =
                cell.Style.Border.Left.Style =
                    cell.Style.Border.Right.Style = cell.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Color.SetColor(Color.White);
            cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(Color.Gray);
            if (level > 0)
            {
                cell.Style.Fill.BackgroundColor.SetColor(Color.Green);
            }
            return cell;
        }

        private ExcelRange GetExcelContent(ExcelRange cell, string text, int align = 0, int level = 0)
        {
            cell.Value = text ?? string.Empty;

            cell.Style.Border.Top.Style =
                cell.Style.Border.Left.Style =
                    cell.Style.Border.Right.Style = cell.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            if (align == 1)//align right
            {
                cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            }


            if (level > 0)
            {
                cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.LightSeaGreen);
            }
            return cell;
        }

        public async Task<ActionResult> GenerateExcelSKUAsync()
        {
            var stream = new MemoryStream();
            var result = new ResultModel { IsSuccess = false, Message = "Error Occured while exporting Excel. Please retry." };
            try
            {
                var skuDetails = await _masterClient.GetSkuListAsync(new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = true });

                //var fileName = $"{Guid.NewGuid()}.xlsx";
                var fileName = "MATERIAL-LIST-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();

                    var rowIndex = 1;
                    var colIndex = 1;
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SkuId"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SkuName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SkuCode"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "OilType Code");
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OilType"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Sales Organisation");
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Distribution Channel");
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Vertical"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Sales Document Type");
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_TDAndPacktype"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PackSizeQuantity"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PackSize"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PackGroup"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_UOM"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ProcessCost"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SubCategory"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_UOM1_No"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "UOM2 (" + @Helper.GetResourceString("lbl_CaseToNumberConversion") + ")");
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "UOM3 (" + @Helper.GetResourceString("lbl_MetricTonToNumberConversion") + ")");
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_MaterialType"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IsActive"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IsUpdateRequired"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IsRequiredToAttachtt"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_GrossWeight"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PremiumAmount"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_StorageLocation"));


                    foreach (var sku in skuDetails)
                    {
                        rowIndex++;
                        colIndex = 1;
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.Id.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.SkuName != null ? sku.SkuName.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.SkuCode != null ? sku.SkuCode.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.OilTypeCode != null ? sku.OilTypeCode.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.OilType != null ? sku.OilType.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.SalesOrganization != null ? sku.SalesOrganization.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.DistributionChannel != null ? sku.DistributionChannel.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.Vertical != null ? sku.Vertical.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.SalesDocumentType != null ? sku.SalesDocumentType.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.TDAndPacktype != null ? sku.TDAndPacktype.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.Quantity.ToString());
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.QuantityTypeUom != null ? sku.QuantityTypeUom.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.OilPackingType != null ? sku.OilPackingType.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.UOMName != null ? sku.UOMName.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.ProcessCost.ToString());
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.SubCategory != null ? sku.SubCategory.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.UOM1_No.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.ConversionFactor1.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.ConversionFactor2.ToString());
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], !string.IsNullOrEmpty(sku.MaterialTypeName) ? sku.MaterialTypeName.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.IsActive.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.NewlyAdded != null ? sku.NewlyAdded.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.IsRequiredToAttachTT.ToString());
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.GrossWeight.ToString());
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.PremiumAmount != null ? sku.PremiumAmount.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.StorageLocation != null ? sku.StorageLocation.ToString() : string.Empty);
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }

                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    this.Response.AddHeader(
                              "content-disposition",
                              string.Format("attachment;  filename={0}", fileName));
                    this.Response.BinaryWrite(package.GetAsByteArray());
                }
                result.IsSuccess = true;
                result.Message = fileName;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "Excel Error" + ex;
            }
            Response.Flush();
            Response.End();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public async Task<ActionResult> GetSalesDocumentTypeddl()
        {

            var result = await _masterClient.GetSalesDocumentTypeddl();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region User / Employee master

        /// <summary>
        /// Method to get User Master List page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageUser, Claims.ViewUser)]
        public ActionResult UserList()
        {
            return View();
        }

        /// <summary>
        /// Method to get User Master List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetUserMasterListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData, VerticalId = VerticalId };
            var result = await _masterClient.GetUserMasterListAsync(loginUserIdDto);
            var resultList = result.ToDataSourceResult(request);
            //resultList.Total = result.Count;
            return Json(resultList);
        }

        /// <summary>
        /// Method to redirect User add or update page
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult UERedirect(string EncryptedId = "")
        {
            Session["UserId"] = EncryptedId;
            return RedirectToAction("CreateUser", "Lookup");
        }

        /// <summary>
        /// Method to get User add or update page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageUser, Claims.ViewUser)]
        public async Task<ActionResult> CreateUser()
        {
            var result = new EmployeeDto();
            Session["SelectedDealerBrokerIds"] = null;
            if (!string.IsNullOrEmpty(Session["UserId"].ToString()))
            {
                result = await _masterClient.GetUserDetailsById(Session["UserId"].ToString());
                if (result.SelectedDealerBrokerIds != null && result.SelectedDealerBrokerIds.Any())
                {
                    result.SelecteDealerBrokerIdsString = UtilityHelper.ConvertLongListToCommaSeparatedString(result.SelectedDealerBrokerIds);
                    Session["SelectedDealerBrokerIds"] = result.SelectedDealerBrokerIds;
                }
            }
            return View(result);
        }

        /// <summary>
        /// Method to add or update User
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateUser(EmployeeDto inputDto,string DivisionList="")       //EmployeeDto inputDto
        {
            if (!String.IsNullOrEmpty(inputDto.EncryptedId))
            {
                inputDto.EncryptedId = inputDto.EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey);

                inputDto.Id = UtilityHelper.IntTryToParse(decryptedId);
            }
            inputDto.LoginUserId = UserId;
            inputDto.DivisionList=GMCore.Helper.JsonHelper.ConvertJSonToObjectList<DivisionDetailsDto>(DivisionList);
            if (!string.IsNullOrEmpty(inputDto.SelecteDealerBrokerIdsString))
            {
                inputDto.SelectedDealerBrokerIds = UtilityHelper.ConvertStringToLongList(inputDto.SelecteDealerBrokerIdsString);
                inputDto.SelectedDealerBrokerIdsCount = inputDto.SelectedDealerBrokerIds.Count;
            }
            if (!string.IsNullOrEmpty(inputDto.RemovedDealerBrokerIdsString))
            {
                inputDto.RemovedDealerBrokerIds = UtilityHelper.ConvertStringToLongList(inputDto.RemovedDealerBrokerIdsString);
            }
            //inputDto.IsUser = true;
            var result = await _masterClient.AddOrUpdateUser(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                var newResult = new EmployeeDto();
                newResult.PostStatus = true;
                newResult.PostMessage = result.PostMessage;
                //return Json(newResult);
                return RedirectToAction("UserList", "Lookup");
            }
            else
            {
                inputDto.PostStatus = false;
                inputDto.PostMessage = result.PostMessage;
            }

            //return Json(inputDto);
            return View("CreateUser", inputDto);
        }

        #endregion      

        #region Delivery Type
        public ActionResult DeliveryType()
        {
            return View();
        }

        public JsonResult GetDeliveryTypeList()
        {
            var data = _masterClient.GetDeliveryTypeList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetDeliveryTypeDetailsAsync([DataSourceRequest] DataSourceRequest request, int selectedType)
        {
            DeliveryTypeInputDto deliveryTypeDto = new DeliveryTypeInputDto();
            deliveryTypeDto.SelectedTypeId = selectedType;
            deliveryTypeDto.IsToReturnInactiveData = true;
            deliveryTypeDto.LoginUserId = UserId;
            var deliveryList = await _masterClient.GetDeliveryDetailsAsync(deliveryTypeDto);
            var result = deliveryList.ToDataSourceResult(request);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AddDeliveryDetails(DeliveryTypeDto deliveryDto)
        {
            deliveryDto.LoginUserId = UserId;
            var result = await _masterClient.AddOrUpdateDeliveryDetails(deliveryDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Contract Type
        public ActionResult ContractType()
        {
            return View();
        }

        public async Task<ActionResult> GetContractTypeList()
        {
            ContractTypeInputDto contractTypeDto = new ContractTypeInputDto() { SelectedTypeId = (int)SeederDataType.ContractType, LoginUserId = UserId };
            var result = await _masterClient.GetContractDetailsAsync(contractTypeDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetContractDetailsAsync([DataSourceRequest] DataSourceRequest request, int selectedType)
        {
            ContractTypeInputDto contractTypeDto = new ContractTypeInputDto();
            contractTypeDto.SelectedTypeId = selectedType;
            contractTypeDto.IsToReturnInactiveData = true;
            contractTypeDto.LoginUserId = UserId;
            var deliveryList = await _masterClient.GetContractDetailsAsync(contractTypeDto);
            var result = deliveryList.ToDataSourceResult(request);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AddOrUpdateContractDetails(ContractTypeDto contractTypeDto)
        {
            contractTypeDto.LoginUserId = UserId;
            var result = await _masterClient.AddOrUpdateContractDetails(contractTypeDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SalesOrganization

        public ActionResult SalesOrganizationList()
        {
            return View();
        }


        public async Task<ActionResult> GetSalesOrganizationddl()
        {
            
            var result = await _masterClient.GetAllSalesOrganizationddl();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SOEditRedirect(string EncryptedId = "")
        {
            Session["EncryptedId"] = EncryptedId;
            return RedirectToAction("SalesOrganization", "Lookup");
        }


        public async Task<ActionResult> SOListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _masterClient.GetSalesOrganizationListAsync();
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> SalesOrganization()
        {
            var result = new SalesOrganizationDto();
            if (Session["EncryptedId"] != null && !String.IsNullOrEmpty(Session["EncryptedId"].ToString()))
            {
                result = await _masterClient.GetSalesOrganizationDetailsById(Session["EncryptedId"].ToString());
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateSalesOrganization(SalesOrganizationDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _masterClient.AddOrUpdateSalesOrganization(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("SalesOrganizationList", "Lookup");
            }
            inputDto.PostStatus = result.PostStatus;
            inputDto.PostMessage = result.PostMessage;
            return View("SalesOrganization", inputDto);
        }

        public ActionResult ExportSalesOrganization(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");

                var resultList = _masterClient.ExportSalesOrganization(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "SalesOrganization_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    //Header
                    worksheet.Cells["A1:BZ1"].Style.Font.Size = 13;
                    worksheet.Cells["A1:BZ1"].Style.Font.Name = "Calibri";
                    worksheet.Cells["A1:BZ1"].Style.Font.Bold = true;

                    worksheet.Cells.LoadFromCollection(resultList, true);
                    worksheet.Cells.AutoFitColumns();

                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region DistributionChannel

        public ActionResult DistributionChannelList()
        {
            return View();
        }

        public async Task<ActionResult> GetDistributionChannelddl(int saleId)
        {

            var result = await _masterClient.GetAllDistributionChannelddl(saleId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DCEditRedirect(string EncryptedId = "")
        {
            Session["DistributionChannelId"] = EncryptedId;
            return RedirectToAction("DistributionChannel", "Lookup");
        }


        public async Task<ActionResult> DistributionChannelListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _masterClient.GetDistributionChannelListAsync();
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> DistributionChannel()
        {
            var result = new DistributionChannelDto();
            if (!String.IsNullOrEmpty(Session["DistributionChannelId"].ToString()))
            {
                result = await _masterClient.GetDistributionChannelDetailsById(Session["DistributionChannelId"].ToString());
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateDistributionChannel(DistributionChannelDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _masterClient.AddOrUpdateDistributionChannel(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("DistributionChannelList", "Lookup");
            }
            inputDto.PostStatus = result.PostStatus;
            inputDto.PostMessage = result.PostMessage;
            return View("DistributionChannel", inputDto);
        }

        public ActionResult ExportDistributionChannel(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");

                var resultList = _masterClient.ExportDistributionChannel(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "DistributionChannel_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    //Header
                    worksheet.Cells["A1:BZ1"].Style.Font.Size = 13;
                    worksheet.Cells["A1:BZ1"].Style.Font.Name = "Calibri";
                    worksheet.Cells["A1:BZ1"].Style.Font.Bold = true;

                    worksheet.Cells.LoadFromCollection(resultList, true);
                    worksheet.Cells.AutoFitColumns();

                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Vertical
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public ActionResult Vertical()
        {
            return View();
        }

        public async Task<ActionResult> VerticalAddOrUpdate()
        {
            var inputDto = new VerticalDto();
            if (!String.IsNullOrEmpty(Session["Vertical"].ToString()))
            {
                string id = Session["Vertical"].ToString();
                inputDto = await _lookupClient.GetVerticalById(id);
            }
            return View(inputDto);
        }

        public ActionResult VERedirect(string EncryptedId = "")
        {
            Session["Vertical"] = EncryptedId;
            return RedirectToAction("VerticalAddOrUpdate", "Lookup");
        }

        public async Task<ActionResult> GetVerticalListForGridAsync([DataSourceRequest] DataSourceRequest request)
        {
            return Json(await _masterClient.GetKendoGridDataAsync<VerticalDto>(GridResultInputDto(request, true), ApiUrl.WebApiUrlGetVerticalListWithPagination));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<JsonResult> AddOrUpdateVerticalDetails(VerticalDto verticalDto)
        {
            verticalDto.UserId = UserId;
            var result = await _masterClient.AddOrUpdateVerticalDetails(verticalDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> ExportVertical(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {

                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<VerticalDto> resultList = new List<VerticalDto>();
                resultList = await _masterClient.ExportVertical(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "Division_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Sales Organization");
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DistributionChannel"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SalesDocumentType"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SalesOrderDocumentType"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Name"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Code"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("msg_Status"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Zpr4"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.SalesOrganizationName);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.DistributionChannelName);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.SalesDocumentType);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.SalesOrderDocumentType);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Name);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Code.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IsActive == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ZPR4 == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                        }
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    //this.Response.Headers.Clear();
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Oil Type
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public ActionResult OilType()
        {
            return View();
        }

        public async Task<ActionResult> OilTypeAddOrUpdate()
        {
            var inputDto = new OilTypeDto();
            if (!String.IsNullOrEmpty(Session["OilType"].ToString()))
            {
                string id = Session["OilType"].ToString();
                inputDto = await _lookupClient.GetOilTypesById(id);
            }
            return View(inputDto);
        }

        public ActionResult OTERedirect(string EncryptedId = "")
        {
            Session["OilType"] = EncryptedId;
            return RedirectToAction("OilTypeAddOrUpdate", "Lookup");
        }
        public async Task<ActionResult> GetOilTypeListAsync([DataSourceRequest] DataSourceRequest request)
        {
            return Json(await _masterClient.GetKendoGridDataAsync<OilTypeDto>(GridResultInputDto(request, true), ApiUrl.WebApiUrlGetOilTypeListWithPagination));
        }

        public async Task<ActionResult> GetVerticalDetailsdd(int distId)
        {
            LoginUserIdDto inputDto = new LoginUserIdDto { IsToReturnInactiveData = false, LoginUserId = UserId };
            inputDto.DistributionId = distId;
            var verticalList = await _masterClient.GetVerticalDetailsAsync(inputDto);
            //verticalList = verticalList.Where(w => w.Id == (Int64)DTO.Enums.Vertical.Hbc).Select(s => s).ToList();
            return Json(verticalList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<JsonResult> AddOrUpdateOilTypeDetails(OilTypeDto oilTypeDto)
        {
            oilTypeDto.LoginUserId = UserId;
            var result = await _masterClient.AddOrUpdateOilTypeDetails(oilTypeDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetVerticalListWithoutHBCddl()
        {
            LoginUserIdDto inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId, VerticalId = VerticalId };
            var verticalList = await _masterClient.GetVerticalDetailsAsync(inputDto);
            //verticalList = verticalList.Where(_ => _.Id != (int)DTO.Enums.Vertical.Hbc).ToList();
            return Json(verticalList, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ExportOilType(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId, VerticalId = VerticalId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<OilTypeDto> resultList = new List<OilTypeDto>();
                resultList = await _masterClient.ExportOilType(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "OilType_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;

                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "OilType Code");
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Name"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SalesOrganization"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DistributionChannel"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_VerticalName"));
                   // GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_LitreConversion"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IsRasoi"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("msg_Status"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Code);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Name);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.SalesOrganizationName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.DistributionChannelName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.VerticalName.ToString());
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.LitreConversion.ToString());
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IsRasoi == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IsActive == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                        }
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    //this.Response.Headers.Clear();
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Plant Master

        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public ActionResult PlantMasterList()
        {
            return View();
        }

        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public async Task<ActionResult> PlantMaster()
        {
            var inputDto = new DepotDto();
            if (!String.IsNullOrEmpty(Session["PlantId"].ToString()))
            {
                string id = Session["PlantId"].ToString();
                inputDto = await _masterClient.GetPlantDetailsByIdAsync(UserId, id);
            }
            return View(inputDto);
        }

        public ActionResult PMEdit(string EncryptedId="")
        {
            Session["PlantId"] = EncryptedId;
            return RedirectToAction("PlantMaster", "Lookup");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdatePlant(DepotDto inputDto)
        {
            inputDto.UserId = UserId;
            inputDto.IsPlant = true;
            var result = await _masterClient.AddOrUpdatePlantDetails(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("PlantMasterList", "Lookup");
            }
            else
            {
                inputDto.PostStatus = false;
                inputDto.PostMessage = result.PostMessage;
            }
            return View("PlantMaster", inputDto);
        }

        public async Task<ActionResult> GetPlantListAsync([DataSourceRequest] DataSourceRequest request, bool IsToReturnInactiveData)
        {
            return Json(await _masterClient.GetKendoGridDataAsync<DepotDto>(GridResultInputDto(request, IsToReturnInactiveData), ApiUrl.WebApiUrlGetPlantListWithPagination));
        }

        /// <summary>
        /// Method to Get Plant List to Bind in Dropdown
        /// </summary>
        /// <param name="request"></param>
        /// <param name="IsToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetPlantListddlAsync([DataSourceRequest] DataSourceRequest request)
        {
            var plantList = await _masterClient.GetPlantDetailsddlAsync();
            return Json(plantList, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetPlantListddlByLoginUserId([DataSourceRequest] DataSourceRequest request)
        {
            var inputDto = new LoginUserIdDto() {
                LoginUserId=UserId,
                RoleId=RoleId
            };
            var plantList = await _masterClient.GetPlantListddlByLoginUserId(inputDto);
            return Json(plantList, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetPlantListddl([DataSourceRequest] DataSourceRequest request)
        {
            var plantdata = new PlantDDLDto();
            var plantList = await _masterClient.GetPlantDetailsddl(plantdata);
            return Json(plantList, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ExportPlant(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<DepotDto> resultList = new List<DepotDto>();
                resultList = await _masterClient.ExportPlant(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "Plant_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PlantCode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PlantName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Address"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Zone"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_State"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Territory"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_District"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_City"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_EmailId"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_MobileNumber"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Pincode"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Usage"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("msg_Status"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Code);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Name.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Location != null ? item.Location.ToString() : string.Empty);
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ZoneName.ToString());
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.State.ToString());
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.TerritoryName.ToString());
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.District.ToString());
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.City.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Email != null ? item.Email.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.MobileNumber != null ? item.MobileNumber.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.PinCode != null ? item.PinCode.ToString() : string.Empty);
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Usage != null ? item.Usage.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IsActive == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                        }
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    //this.Response.Headers.Clear();
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Depot Master

        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public ActionResult DepotMasterList()
        {
            return View();
        }

        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public async Task<ActionResult> DepotMaster()
        {
            var depot = new DepotDto();
            if (Session["DepotId"] != null && UtilityHelper.IntTryToParse(Session["DepotId"].ToString()) > 0)
            {
                long id = Convert.ToInt32(Session["DepotId"].ToString());
                depot = await _masterClient.GetDepotDetailByIdAsync(UserId, id);
            }
            return View(depot);
        }

        public ActionResult DepotMasterEdit(string depoId)
        {
            Session["DepotId"] = depoId;
            return RedirectToAction("DepotMaster", "Lookup");
        }

        public async Task<ActionResult> GetDepotListForGridAsync([DataSourceRequest] DataSourceRequest request, bool IsToReturnInactiveData)
        {
            return Json(await _masterClient.GetKendoGridDataAsync<DepotDto>(GridResultInputDto(request, IsToReturnInactiveData), ApiUrl.WebApiUrlGetDepotListWithPagination));
        }

        /// <summary>
        /// Method to Get Depot List to Bind in Dropdown
        /// </summary>
        /// <param name="request"></param>
        /// <param name="IsToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetDepotListAsync([DataSourceRequest] DataSourceRequest request, bool IsToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { IsToReturnInactiveData = IsToReturnInactiveData, LoginUserId = UserId };
            var result = await _masterClient.GetDepotDetailsAsync(loginUserIdDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetDepotAndPlantListAsync([DataSourceRequest] DataSourceRequest request, bool IsToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { IsToReturnInactiveData = IsToReturnInactiveData, LoginUserId = UserId };
            var result = await _masterClient.GetDepotsAndPlantsAsync(loginUserIdDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateDepot(DepotDto depotDto)
        {
            depotDto.UserId = UserId;
            var result = await _masterClient.AddOrUpdateDepotDetails(depotDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("DepotMasterList", "Lookup");
            }
            else
            {
                depotDto.PostStatus = false;
                depotDto.PostMessage = result.PostMessage;
            }
            return View("DepotMaster", depotDto);
        }

        public async Task<ActionResult> GetDepotsByPlantIdddlAsync([DataSourceRequest] DataSourceRequest request, long plantId)
        {
            var result = new List<DropDownDto>();
            if (plantId > 0)
            {
                var inputDto = new IdInputDto { Id = plantId };
                result = await _lookupClient.GetDepotsByPlantId(inputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ExportDepot(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<DepotDto> resultList = new List<DepotDto>();
                resultList = await _masterClient.ExportDepot(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "Depot_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DepotCode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DepotName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Address"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Zone"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_State"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Territory"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_District"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_City"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Email"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Pincode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PlantCode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("msg_Status"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Code);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Name.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Location != null ? item.Location.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ZoneName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.State.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.TerritoryName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.District.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.City.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Email != null ? item.Email.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.PinCode != null ? item.PinCode.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.PlantCode.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IsActive == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                        }
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    //this.Response.Headers.Clear();
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Lookup

        /// <summary>
        /// Method to get User Assigned To list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetUsersByRoleAsync([DataSourceRequest] DataSourceRequest request, long userRoleId)
        {
            var inputDto = new IdInputDto();
            inputDto.Id = userRoleId;
            inputDto.LoginUserId = UserId;
            var result = await _masterClient.GetUsersByRoleAsync(inputDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get Get Address By Pincode
        /// </summary>
        /// <param name="pincode"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetAddressByPincode(string pincode)
        {
            var pincodeAddressDto = await _lookupClient.GetAddressByPincode(UserId, pincode);
            return Json(pincodeAddressDto, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get Get Inco Terms List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [NoCache]
        public async Task<JsonResult> GetIncoTermsListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            ContractTypeInputDto contractTypeDto = new ContractTypeInputDto { IsToReturnInactiveData = isToReturnInactiveData, LoginUserId = UserId, SelectedTypeId = (int)SeederDataType.IncoTerms };
            var result = await _masterClient.GetContractDetailsAsync(contractTypeDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get Get Transport Mode List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [NoCache]
        public async Task<JsonResult> GetDealerBrokerTransportModeListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            DeliveryTypeInputDto deliveryTypeDto = new DeliveryTypeInputDto { IsToReturnInactiveData = isToReturnInactiveData, LoginUserId = UserId, SelectedTypeId = (int)MasterDataTypes.TransaportMode };
            var result = await _masterClient.GetDeliveryDetailsAsync(deliveryTypeDto);
            result = result != null ? result.Where(_ => _.Id == (int)DTO.Enums.TransportMode.Truck).ToList() : result;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get Get Transport Mode List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [NoCache]
        public async Task<JsonResult> GetTransportModeListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            DeliveryTypeInputDto deliveryTypeDto = new DeliveryTypeInputDto { IsToReturnInactiveData = isToReturnInactiveData, LoginUserId = UserId, SelectedTypeId = (int)MasterDataTypes.TransaportMode };
            var result = await _masterClient.GetDeliveryDetailsAsync(deliveryTypeDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get Get Sauda booking Type List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [NoCache]
        public async Task<JsonResult> GetSaudaBookingTypeListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            DeliveryTypeInputDto deliveryTypeDto = new DeliveryTypeInputDto { IsToReturnInactiveData = isToReturnInactiveData, LoginUserId = UserId, SelectedTypeId = (int)MasterDataTypes.SaudaBookingType };
            var result = await _masterClient.GetDeliveryDetailsAsync(deliveryTypeDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get Get Pack type List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [NoCache]
        public async Task<JsonResult> GetPackTypeListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            DeliveryTypeInputDto deliveryTypeDto = new DeliveryTypeInputDto { IsToReturnInactiveData = isToReturnInactiveData, LoginUserId = UserId, SelectedTypeId = (int)MasterDataTypes.PackType };
            var result = await _masterClient.GetDeliveryDetailsAsync(deliveryTypeDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get Get Vertical List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [NoCache]
        public async Task<JsonResult> GetVerticalListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto inputDto = new LoginUserIdDto { IsToReturnInactiveData = isToReturnInactiveData, LoginUserId = UserId };
            var result = await _masterClient.GetVerticalDetailsAsync(inputDto);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Method to get state list
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetStateListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _lookupClient.GetStateListAsync();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetStateListWithAllOptionAsync([DataSourceRequest] DataSourceRequest request)
        {
            var stateList = await _lookupClient.GetStateListAsync();
            var allItem = new StateDto { StateId = -1, StateName = "All" };
            if (stateList != null && stateList.Any())
            {
                stateList.Insert(0, allItem);
            }
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get state list
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetOilPackingTypeListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _lookupClient.GetOilPackingTypeListAsync();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetOilPackingGroupListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _lookupClient.GetOilPackingGroupListAsync();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetPackGroupListBySkuIdAsync([DataSourceRequest] DataSourceRequest request, long skuId)
        {
            var result = new List<DropDownDto>();
            if (skuId > 0)
            {
                IdInputDto idInputDto = new IdInputDto { Id = skuId };
                result = await _lookupClient.GetPackGroupListBySkuId(idInputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetSubCategoryList([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _lookupClient.GetSubCategoryList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get city list
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetCityListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _lookupClient.GetCityListAsync();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to Get District List By StateId
        /// </summary>
        /// <param name="request"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetDistrictListByStateIdAsync([DataSourceRequest] DataSourceRequest request, string stateId = "")
        {
            var stateID = 0;
            stateID = !string.IsNullOrEmpty(stateId) ? int.Parse(stateId) : 0;
            var result = await _lookupClient.GetDistrictListByStateIdAsync(stateID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to Get City List By DistrictName
        /// </summary>
        /// <param name="request"></param>
        /// <param name="districtName"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetCityListByDistrictName([DataSourceRequest] DataSourceRequest request, string districtId = "")
        {
            var result = await _lookupClient.GetCityListByDistrictName(districtId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Method to Get City List By DistrictName
        /// </summary>
        /// <param name="request"></param>
        /// <param name="CitiesName"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetCityListByStateName([DataSourceRequest] DataSourceRequest request, string stateIds = "")
        {
            var result = await _lookupClient.GetCityListByStateName(stateIds);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetCityListByDistrictIdForDropdown([DataSourceRequest] DataSourceRequest request, int districtId)
        {
            var result = await _lookupClient.GetCityListByDistrictIdForDropdown(districtId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetOilTypesBasedOnVerticalId(long verticalId, bool isToReturnInactiveData = false)
        {
            var result = new List<DropDownDto>();
            if (verticalId > 0)
            {
                IdInputDto inputDto = new IdInputDto { Id = verticalId };
                result = await _lookupClient.GetOilTypesBasedOnVerticalId(inputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //get oiltypes based on vertical if there is vertical id or gets all oiltypes
        public async Task<JsonResult> GetOilTypesBasedOnVertical(long verticalId, bool isToReturnInactiveData = false)
        {
            IdInputDto inputDto = new IdInputDto { VerticalId = verticalId };
            var result = await _lookupClient.GetOilTypesBasedOnVertical(inputDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetStatesBasedOnCustomerGroupId([DataSourceRequest] DataSourceRequest request, int customerGroupId)
        {
            var result = new List<DropDownDto>();
            if (customerGroupId > 0)
            {
                IdInputDto inputDto = new IdInputDto { Id = customerGroupId };
                result = await _lookupClient.GetStatesBasedOnCustomerGroupId(inputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetSkusBasedOnOilTypeId([DataSourceRequest] DataSourceRequest request, int? oilTypeId, bool isToReturnInactiveData)
        {
            var result = new List<DropDownDto>();
            if (oilTypeId > 0)
            {
                IdInputDto inputDto = new IdInputDto { Id = (int)oilTypeId, IsToReturnInactiveData = isToReturnInactiveData };
                result = await _lookupClient.GetSkusBasedOnOilTypeId(inputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get uom list
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetUomListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _lookupClient.GetUomListAsync();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get Quantity type list
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetUomQuantityTypeListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _lookupClient.GetUomListAsync();
            result = result.Where(_ => _.IsQuantityType).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get Case Uom
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetCaseUomAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<UomDto>();
            result = await _lookupClient.GetUomListAsync();
            List<long> caseUom = new List<long>() { (int)Uom.MT};
            result = result.Where(_ => !caseUom.Contains(_.Id)).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get No Uom
        /// </summary>
        /// <returns></returns>
        public JsonResult GetNosUomAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<DropDownDto>();
            DropDownDto noUom = new DropDownDto { Id = (int)Uom.Nos, Name = Uom.Nos.ToString() };
            result.Add(noUom);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get MT Uom
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMTUomAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<DropDownDto>();
            DropDownDto noUom = new DropDownDto { Id = (int)Uom.MT, Name = Uom.MT.ToString() };
            result.Add(noUom);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get Ltr Uom
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLtrUomAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<DropDownDto>();
            DropDownDto noUom = new DropDownDto { Id = (int)Uom.Ltr, Name = Uom.Ltr.ToString() };
            result.Add(noUom);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get role list
        /// </summary>
        /// <returns></returns>

        // <summary>
        /// Method to get Kg Uom
        /// </summary>
        /// <returns></returns>
        public JsonResult GetKgUomAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<DropDownDto>();
            DropDownDto noUom = new DropDownDto { Id = (int)Uom.Kg, Name = Uom.Kg.ToString() };
            result.Add(noUom);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get role list
        /// </summary>
        /// <returns></returns>
        [NoCache]
        public async Task<JsonResult> GetRoleListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _lookupClient.GetRoleListAsync();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get role list
        /// </summary>
        /// <returns></returns>
        [NoCache]
        public async Task<JsonResult> GetRoleListExceptDealerBrokerAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _lookupClient.GetRoleListAsync();
            result = result.Where(_ => _.Id != (int)DTO.Enums.Role.Dealer && _.Id != (int)Role.Broker && _.Id != (int)Role.Admin && _.Id != (int)Role.ShipToParty).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDealerBrokerPartial(bool isRemoveDealerId, int RoleId = 0)
        {
            if (isRemoveDealerId)
                Session["SelectedDealerIds"] = null;
            return PartialView("_dealerBrokerPartial");
        }

        /// <summary>
        /// Method to Get Dealer List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetDealerBrokerListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData };
            var dealersList = await _masterClient.GetDealerBrokerListAsync(loginUserIdDto);
            var resultList = dealersList.ToDataSourceResult(request);
            return Json(resultList);
        }

        public JsonResult GetProcessListForHierarchyAsync([DataSourceRequest] DataSourceRequest request)
        {
            var resultList = _lookupClient.GetProcessListForHierarchy().Where(w => w.Id != 3);
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetIngredientCostddl([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData, long verticalId)
        {
            var loginUserIdDto = new LoginUserIdDto();
            loginUserIdDto.IsToReturnInactiveData = isToReturnInactiveData;
            loginUserIdDto.LoginUserId = UserId;
            loginUserIdDto.VerticalId = verticalId;
            var result = await _lookupClient.GetIngredientCostddl(loginUserIdDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetSkuListBasedOnOilTypeSubCategoryPackGroupForDropdown([DataSourceRequest] DataSourceRequest request, long oilTypeId, long subCategoryId, long packGroupId)
        {
            var result = new List<DropDownDto>();
            if (oilTypeId > 0)
            {
                SkuDropDownInputDto inputDto = new SkuDropDownInputDto { OilTypeId = oilTypeId, SubCategoryId = subCategoryId, PackGroupId = packGroupId };
                result = await _lookupClient.GetSkuListBasedOnOilTypeIdSubCategoryIdPackGroupIdForDropdown(inputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region ZoneMapping

        /// <summary>
        /// Method to Get Zone List page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public ActionResult ZoneList()
        {
            return View();
        }

        /// <summary>
        /// Method to Get Zone List 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetZoneMappingListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData };
            var zoneList = await _masterClient.GetZoneMappingListAsync(loginUserIdDto);
            var resultList = zoneList.ToDataSourceResult(request);
            return Json(resultList);
        }

        /// <summary>
        /// Hierical grid state data base on zone Id
        /// </summary>
        /// <param name="request"></param> 
        /// <returns></returns>
        public async Task<ActionResult> GetZoneMappingStateListAsync(int zoneId, [DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            var zoneList = await _masterClient.GetZoneMappedStates(zoneId);
            var resultList = zoneList.ToDataSourceResult(request);
            return Json(resultList);
        }

        /// <summary>
        /// Method to get Zone add or update page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public async Task<ActionResult> Zone()
        {
            var result = new AddorUpdateZoneDto();
            if (!String.IsNullOrEmpty(Session["ZoneId"].ToString()))
            {
                result = await _masterClient.GetZoneMappingDetailsById(Session["ZoneId"].ToString());
            }
            else
            {
                result = await _masterClient.GetNewZoneStates();
            }
            return View("Zone", result);
        }


        /// <summary>
        /// Method to redirect Zone add or update page
        /// </summary>
        /// <param name="zoneId"></param> 
        /// <returns></returns>
        public ActionResult ZERedirect(string EncryptedId = "")
        {
            Session["ZoneId"] = EncryptedId;
            return RedirectToAction("Zone", "Lookup");
        }

        /// <summary>
        /// Method to add or update Zone
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateZone(AddorUpdateZoneDto inputDto)
        {
            var result = await _masterClient.AddOrUpdateZoneMapping(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("ZoneList", "Lookup");
            }
            else
            {
                AddorUpdateZoneDto reponse = new AddorUpdateZoneDto();
                TempData["ErrorMessage"] = result.PostMessage;
                if (!String.IsNullOrEmpty(inputDto.EncryptedId))
                {
                    reponse = await _masterClient.GetZoneMappingDetailsById(Session["ZoneId"].ToString());
                }
                else
                {
                    reponse = await _masterClient.GetNewZoneStates();
                }
                reponse.PostMessage = result.PostMessage;
                reponse.PostStatus = false;
                if (result.States != null)
                    result.States.ForEach(s => { var val = reponse.States.FirstOrDefault(r => r.Id == s.Id); val.Checked = s.Checked; });
                return View("zone", reponse);
            }


        }

        public async Task<ActionResult> ExportZone(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<ZoneDto> resultList = new List<ZoneDto>();
                resultList = await _masterClient.ExportZone(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "Zone_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ZoneName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_States"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("msg_Status"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Name.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.States.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.isActive == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                        }
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    //this.Response.Headers.Clear();
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Verticle Oiltype Sku

        public async Task<ActionResult> GetOilTypesBasedOnVerticle([DataSourceRequest] DataSourceRequest request, long verticleId)
        {
            IList<DropDownDto> OilTypesData = new List<DropDownDto>();
            if (verticleId > 0)
            {
                OilTypeInputDto oilTypeInput = new OilTypeInputDto();
                oilTypeInput.VerticalId = verticleId;
                OilTypesData = await _lookupClient.GetOilTypesBasedOnVerticle(oilTypeInput);
            }
            return Json(OilTypesData, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetSkusBasedOnOilType([DataSourceRequest] DataSourceRequest request, long oiltypeId, bool IsToReturnInactiveData = false)
        {
            var skuData = new List<SkuDropDown>();
            if (oiltypeId > 0)
            {
                var skuInputDto = new SkuInputDto();
                skuInputDto.OilTypeId = oiltypeId;
                skuData = await _lookupClient.GetSkusBasedOnOilType(skuInputDto);
            }
            return Json(skuData, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetSkusBasedOnEmployeeDiscount([DataSourceRequest] DataSourceRequest request, long id, bool IsToReturnInactiveData = false)
        {
            var skuData = new List<DropDownDto>();
            if (id > 0)
            {
                var skuInputDto = new SkuInputDto();
                skuInputDto.EmployeeDiscountParentId = id;
                skuData = await _lookupClient.GetSkusBasedOnEmployeeDiscount(skuInputDto);
            }
            return Json(skuData, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Dealer And Broker Details

        public ActionResult DealerBrokerGridPartial(UserDetailsViewModel userDetailsViewModel)
        {
            if (userDetailsViewModel.IsRemoveSelectedDealerIdsFromSession)
                Session["SelectedDealerBrokerIds"] = null;
            return View(userDetailsViewModel);
        }

        /// <summary>
        /// Method to Get Dealer List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetDealerBrokerDetailsList([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData, long verticalId, long saudaBookingTypeId)
        {
            var dealersList = new List<DealerBrokerDto>();
            if (verticalId > 0)
            {
                var inputDto = new ReportingUsersInputDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData, VerticalId = verticalId, SaudaBookingTypeId = saudaBookingTypeId };
                dealersList = await _lookupClient.GetDealerAndBrokerDetails(inputDto);
                if (Session["SelectedDealerBrokerIds"] != null && dealersList.Any())
                {
                    var customerIds = (List<long>)Session["SelectedDealerBrokerIds"];
                    dealersList.Where(w => customerIds.Any(c => c == w.Id)).Select(s => s.IsChecked = true).ToList();
                    dealersList = dealersList.OrderByDescending(o => o.IsChecked).ToList();
                }
            }
            var resultList = dealersList.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> GetDealerBrokerListForBDOAsync([DataSourceRequest] DataSourceRequest request, string verticalId,string SalesOrg,string DistChannel, long userId = 0)
        {
            var dealersList = new List<DealerBrokerDto>();
            //if (verticalId > 0)
            //{

            if ( verticalId != string.Empty && SalesOrg != string.Empty && DistChannel != string.Empty)
            {
                var verticalIds = GMCore.Helper.UtilityHelper.ConvertStringToLongList(verticalId);
                var salesOrganizationIds = GMCore.Helper.UtilityHelper.ConvertStringToLongList(SalesOrg);
                var distributionChannelIds = GMCore.Helper.UtilityHelper.ConvertStringToLongList(DistChannel);

                var inputDto = new ReportingUsersInputDto
                {
                    LoginUserId = UserId,
                    SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                    UserId = userId,
                    DivisionIds = verticalIds,
                    SalesOrganizationIds = salesOrganizationIds,
                    DistributionChannelIds = distributionChannelIds,
                };
                dealersList = await _lookupClient.GetDealerAndBrokerListForBDO(inputDto);
                if (Session["SelectedDealerBrokerIds"] != null && dealersList.Any())
                {
                    var customerIds = (List<long>)Session["SelectedDealerBrokerIds"];
                    dealersList.Where(w => customerIds.Any(c => c == w.Id)).Select(s => s.IsChecked = true).ToList();
                    dealersList = dealersList.OrderByDescending(o => o.IsChecked).ToList();
                }

            }
           
            var resultList = dealersList.ToDataSourceResult(request);
            return Json(resultList);
        }

        #endregion

        #region Dealer Popup

        public ActionResult DealerGridPartial(UserDetailsViewModel userDetailsViewModel)
        {
            if (userDetailsViewModel.IsRemoveSelectedDealerIdsFromSession)
                Session["SelectedDealerIds"] = null;
            return View(userDetailsViewModel);
        }

        /// <summary>
        /// Method to Get Dealer List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetDealerDetailsListForPopup([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData, string verticalId/*, long saudaBookingTypeId*/)
        {
            var verticalIds = GMCore.Helper.UtilityHelper.ConvertStringToLongList(verticalId);
            var loginUserDto = new DealerBrokerParamDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData, DivisionIds = verticalIds, SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess };
            var dealersList = await _lookupClient.GetDealerDetails(loginUserDto);
            if (Session["SelectedDealerIds"] != null && dealersList.Any())
            {
                var customerIds = (List<long>)Session["SelectedDealerIds"];
                dealersList.Where(w => customerIds.Any(c => c == w.Id)).Select(s => s.IsChecked = true).ToList();
                dealersList = dealersList.OrderByDescending(o => o.IsChecked).ToList();
            }
            var resultList = dealersList.ToDataSourceResult(request);
            return Json(resultList);
        }

        #endregion

        #region Dealers based on State Popup

        public ActionResult DealersBasedOnStateGridPartial(UserDetailsViewModel userDetailsViewModel)
        {
            if (userDetailsViewModel.IsRemoveSelectedDealerIdsFromSession)
                Session["SelectedDealerIds"] = null;
            return View(userDetailsViewModel);
        }

        /// <summary>
        /// Method to Get Dealer List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetDealersBasedOnStateForPopup([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData, long verticalId, long stateId)
        {
            var loginUserDto = new DealerBrokerParamDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData, VerticalId = verticalId, StateId = stateId };
            var dealersList = await _masterClient.GetDealersBasedOnStateAsyn(loginUserDto);
            if (Session["SelectedDealerIds"] != null && dealersList.Any())
            {
                var customerIds = (List<long>)Session["SelectedDealerIds"];
                dealersList.Where(w => customerIds.Any(c => c == w.Id)).Select(s => s.IsChecked = true).ToList();
                dealersList = dealersList.OrderByDescending(o => o.IsChecked).ToList();
            }
            var resultList = dealersList.ToDataSourceResult(request);
            return Json(resultList);
        }

        #endregion

        #region Dealer And Broker Details

        public ActionResult DepotGridPartial(UserDetailsViewModel userDetailsViewModel)
        {
            if (userDetailsViewModel.IsRemoveSelectedDealerIdsFromSession)
                Session["SelectedDepotIds"] = null;
            return View(userDetailsViewModel);
        }

        /// <summary>
        /// Method to Get Dealer List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetDealerBrokerDepotList([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            var loginUserIdDto = new LoginUserIdDto();
            loginUserIdDto.IsToReturnInactiveData = isToReturnInactiveData;
            loginUserIdDto.LoginUserId = UserId;
            var depotList = await _masterClient.GetDepotDetailsAsync(loginUserIdDto);
            if (Session["SelectedDepotIds"] != null && depotList != null && depotList.Any())
            {
                var customerIds = (List<long>)Session["SelectedDepotIds"];
                depotList.Where(w => customerIds.Any(c => c == w.Id)).Select(s => s.IsChecked = true).ToList();
                depotList = depotList.OrderByDescending(o => o.IsChecked).ToList();
            }
            var resultList = depotList.ToDataSourceResult(request);
            return Json(resultList);
        }

        #endregion

        #region State

        /// <summary>
        /// Method to Get State List
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public ActionResult States()
        {
            return View();
        }

        /// <summary>
        /// Method to Get Sku List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetMasterStateListAsync([DataSourceRequest] DataSourceRequest request)
        {
            return Json(await _masterClient.GetKendoGridDataAsync<StateDto>(GridResultInputDto(request, true), ApiUrl.WebApiUrlGetStateListWithPagination));
        }

        /// <summary>
        /// Method to redirect state add or update page
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public ActionResult SRedirect(string EncryptedId = "")
        {
            Session["StateId"] = EncryptedId;
            return RedirectToAction("State", "Lookup");
        }

        /// <summary>
        /// Method to get state add or update page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public async Task<ActionResult> State()
        {
            var result = new StateDto();
            if (!String.IsNullOrEmpty(Session["StateId"].ToString()))
            {
                result = await _masterClient.GetStateDetailsById(Session["StateId"].ToString());
            }
            return View(result);
        }

        /// <summary>
        /// Method to add or update state
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> State(StateDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _masterClient.AddOrUpdateState(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("States", "Lookup");
            }
            TempData["ErrorMessage"] = result.PostMessage;
            return View("State", result);
        }

        public async Task<ActionResult> ExportState(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<StateDto> resultList = new List<StateDto>();
                resultList = await _masterClient.ExportStates(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "State_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_State"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("msg_Status"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.StateName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IsActive == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                        }
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    //this.Response.Headers.Clear();
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region District
        /// <summary>
        /// Method to Get District List
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public ActionResult Districts()
        {
            return View();
        }

        /// <summary>
        /// Method to Get Sku List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetMasterDistrictListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _masterClient.GetDistrictListAsync();
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        /// <summary>
        /// Method to redirect District add or update page
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public ActionResult DRedirect(string EncryptedId = "")
        {
            Session["DistrictId"] = EncryptedId;
            return RedirectToAction("District", "Lookup");
        }

        /// <summary>
        /// Method to get District add or update page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public async Task<ActionResult> District()
        {
            var result = new DistrictDto();
            if (!String.IsNullOrEmpty(Session["DistrictId"].ToString()))
            {
                result = await _masterClient.GetDistrictDetailsById(Session["DistrictId"].ToString());
            }
            return View(result);
        }

        /// <summary>
        /// Method to add or update state
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> District(DistrictDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _masterClient.AddOrUpdateDistrict(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ExportDistrict(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<DistrictDto> resultList = new List<DistrictDto>();
                resultList = await _masterClient.ExportDistrict(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "District_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_District"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_State"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("msg_Status"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.DistrictName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.StateName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IsActive == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                        }
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    //this.Response.Headers.Clear();
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region City
        /// <summary>
        /// Method to Get City List
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public ActionResult Cities()
        {
            return View();
        }

        /// <summary>
        /// Method to Get City List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetMasterCityListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _masterClient.GetCityListAsync();
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        /// <summary>
        /// Method to redirect City add or update page
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public ActionResult CRedirect(string EncryptedId = "")
        {
            Session["CityId"] = EncryptedId;
            return RedirectToAction("City", "Lookup");
        }

        /// <summary>
        /// Method to get District add or update page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public async Task<ActionResult> City()
        {
            var result = new CityDto();
            if (!String.IsNullOrEmpty(Session["CityId"].ToString()))
            {
                result = await _masterClient.GetCityDetailsById(Session["CityId"].ToString());
            }
            return View(result);
        }

        /// <summary>
        /// Method to add or update state
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> City(CityDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _masterClient.AddOrUpdateCity(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ExportCity(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<CityDto> resultList = new List<CityDto>();
                resultList = await _masterClient.ExportCity(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "City_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_City"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_District"));
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Territory"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_State"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("msg_Status"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.CityName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.DistrictName.ToString());
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.TerritoryName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.StateName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IsActive == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                        }
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    //this.Response.Headers.Clear();
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Lookup

        public ActionResult RouteAreaMaster()
        {
            return View();
        }

        public async Task<ActionResult> SaudaBookingTypes()
        {
            var result = await _masterClient.GetBookingTypes();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetMaterialTypes()
        {
            var result = await _masterClient.GetMaterialTypes();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetOilTypes()
        {
            var result = await _masterClient.GetOilTypes();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetZoneListddl([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto();
            loginUserIdDto.IsToReturnInactiveData = isToReturnInactiveData;
            var result = await _masterClient.GetZoneMappingListAsync(loginUserIdDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetUserRoleListddl([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto();
            loginUserIdDto.IsToReturnInactiveData = isToReturnInactiveData;
            var result = await _masterClient.GetUserRoleListAsync(loginUserIdDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetZoneListForDropdown([DataSourceRequest] DataSourceRequest request)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto();
            var result = await _masterClient.GetZoneListForDropdown(loginUserIdDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetStateListByZoneIdForDropdown([DataSourceRequest] DataSourceRequest request, long zoneId)
        {
            List<DropDownDto> stateList = new List<DropDownDto>();
            if (zoneId > 0)
                stateList = await _masterClient.GetStateListByZoneIdForDropdown(zoneId);
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetIncoTermddl([DataSourceRequest] DataSourceRequest request)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto();
            var result = await _masterClient.GetIncoTermListAsync(loginUserIdDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetZoneMappedStatesddl([DataSourceRequest] DataSourceRequest request, string zoneId)
        {
            IList<StateDto> stateList = new List<StateDto>();
            int zone = string.IsNullOrEmpty(zoneId) ? 0 : Convert.ToInt32(zoneId);
            if (zone > 0)
                stateList = await _masterClient.GetZoneMappedStates(zone);
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetZoneMappedStateIds([DataSourceRequest] DataSourceRequest request, string zoneIds = " ")
        {
            IList<DropDownDto> stateList = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(zoneIds))
            {
                List<long> zoneids = zoneIds.Split(',').ToList().ConvertAll(long.Parse);
                if (zoneids != null && zoneids.Any())
                    stateList = await _masterClient.GetZoneMappedStatesIds(zoneids);
            }
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Competitor

        /// <summary>
        /// Method to Get Competitor List page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public ActionResult CompetitorList()
        {
            return View();
        }

        /// <summary>
        /// Method to Get Competitor List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetCompetitorListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            return Json(await _masterClient.GetKendoGridDataAsync<CompetitorDto>(GridResultInputDto(request, isToReturnInactiveData), ApiUrl.WebApiUrlGetCompetitorListWithPagination));
        }

        /// <summary>
        /// Method to redirect Competitor add or update page
        /// </summary>
        /// <param name="cushionMarginId"></param>
        /// <returns></returns>
        public ActionResult CERedirect(string EncryptedId = "")
        {
            Session["CompetitorId"] = EncryptedId;
            return RedirectToAction("Competitor", "Lookup");
        }

        /// <summary>
        /// Method to get Competitor add or update page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public async Task<ActionResult> Competitor()
        {
            var result = new CompetitorDto();
            Session["SelectedSkuIds"] = null;
            if (!String.IsNullOrEmpty(Session["CompetitorId"].ToString()))
            {
                result = await _lookupClient.GetCompetitorDetailsById(Session["CompetitorId"].ToString());
                if (result != null && result.SelectedSkuIds != null && result.SelectedSkuIds.Any())
                {
                    Session["SelectedSkuIds"] = result.SelectedSkuIds;
                    result.SelectedOilTypeIdString = string.Join(",", result.SelectedOilTypeIds);
                }
            }
            return View(result);
        }

        /// <summary>
        /// Method to  add or update Competitor
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateCompetitor(CompetitorDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _lookupClient.AddOrUpdateCompetitor(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("CompetitorList", "Lookup");
            }
            return View("Competitor", result);
        }

        /// <summary>
        /// Method to Get Competitor List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetSkuBasedOnOilTypes([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData, List<long> oilTypeIds)
        {
            IList<SkuDto> skuData = new List<SkuDto>();
            if (oilTypeIds != null && oilTypeIds.Any())
            {
                var inputDto = new CompetitorSkuInputDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData, OilTypeIds = oilTypeIds };
                skuData = await _lookupClient.GetSkuBasedOnOilTypesAsync(inputDto);
                if (Session["SelectedSkuIds"] != null && skuData != null && skuData.Any())
                {
                    var skuIds = (List<long>)Session["SelectedSkuIds"];
                    skuData.Where(w => skuIds.Any(a => a == w.Id)).Select(s => s.IsChecked = true).ToList();
                    skuData = skuData.OrderByDescending(o => o.IsChecked).ToList();
                }
            }
            var resultList = skuData.ToDataSourceResult(request);
            return Json(resultList);
        }

        public ActionResult CompetitorSkuGridPartial(CompetitorSkuInputDto inputDto)
        {
            if (inputDto.IsToRemoveSelectedIdFromSession)
                Session["SelectedSkuIds"] = null;
            return View(inputDto);
        }

        public async Task<ActionResult> ExportCompetitors(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<CompetitorDto> resultList = new List<CompetitorDto>();
                resultList = await _lookupClient.ExportCompetitor(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "Competitor_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CompetitorName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Zone"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_StateName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Address"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_MappedSkus"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("msg_Status"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Name.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ZoneName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.StateName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Address.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.MappedSkus.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IsActive == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                        }
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    //this.Response.Headers.Clear();
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region FreightZone Master

        /// <summary>
        /// Method to Get FreightZone List
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public ActionResult FreightZoneList()
        {
            return View();
        }

        /// <summary>
        /// Method to Get FreightZone List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetFreightZoneListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData };
            var result = await _masterClient.GetFreightZoneListAsync(loginUserIdDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        /// <summary>
        /// Method to redirect FreightZone add or update page
        /// </summary>
        /// <param name="freightZoneId"></param>
        /// <returns></returns>
        public ActionResult FreightZoneEditRedirect(string freightZoneId = "")
        {
            Session["FreightZoneId"] = freightZoneId;
            return RedirectToAction("FreightZone", "Lookup");
        }

        /// <summary>
        /// Method to get FreightZone add or update page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public async Task<ActionResult> FreightZone()
        {
            var result = new FreightZoneDto();
            if (Session["FreightZoneId"] != null && UtilityHelper.IntTryToParse(Session["FreightZoneId"].ToString()) > 0)
            {
                result = await _masterClient.GetFreightZoneDetailsById(UtilityHelper.LongTryToParse(Session["FreightZoneId"].ToString()));
            }
            return View(result);
        }

        /// <summary>
        /// Method to add or update FreightZone
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> AddOrUpdateFreightZone(FreightZoneDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _masterClient.AddOrUpdateFreightZone(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("FreightZoneList", "Lookup");
            }
            return View("FreightZone", result);
        }

        /// <summary>
        /// Method to Get FreightZone List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetFreightZoneListByDepot([DataSourceRequest] DataSourceRequest request, int depotId = 0)
        {
            IdInputDto inputDto = new IdInputDto { Id = depotId };
            var result = await _masterClient.GetFreightZoneListByDepot(inputDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to Get FreightZone List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetFreightZoneListByDepotIds([DataSourceRequest] DataSourceRequest request, string selectedDepotIds)
        {
            var result = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(selectedDepotIds))
            {
                List<long> depotIds = selectedDepotIds.Split(',').Select(long.Parse).ToList();
                result = await _masterClient.GetFreightZoneListByDepotIds(depotIds);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get FreightZone list
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetFreightZoneListddlAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _masterClient.GetFreightZoneListddlAsync();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get FreightZone list based on State & Zone
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetFreightZoneListddlByStateZoneAsync([DataSourceRequest] DataSourceRequest request, int stateId = 0, long zoneId = 0)
        {
            var result = new List<DropDownDto>();
            if (stateId > 0 && zoneId > 0)
            {
                FreightZoneInputDto inputDto = new FreightZoneInputDto { ZoneId = zoneId, StateId = stateId };
                result = await _masterClient.GetFreightZoneListddlByStateZoneAsync(inputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetFreightZoneListByStateZoneIdsAsync([DataSourceRequest] DataSourceRequest request, string stateIds = " ", string zoneIds = " ")
        {
            var result = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(stateIds) && !string.IsNullOrEmpty(zoneIds))
            {
                List<long> zoneids = zoneIds.Split(',').ToList().ConvertAll(long.Parse);
                List<int> stateids = stateIds.Split(',').ToList().ConvertAll(Int32.Parse);
                List<int?> Stateids = stateids.Cast<int?>().ToList();
                List<long?> Zoneids = zoneids.Cast<long?>().ToList();
                if (stateids.Count > 0 && zoneids.Count > 0)
                {
                    FreightZoneInputDto inputDto = new FreightZoneInputDto { ZoneIds = Zoneids, StateIds = Stateids };
                    result = await _masterClient.GetFreightZoneListByStateZoneIdsAsync(inputDto);
                }



            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ExportFreightZone(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<FreightZoneDto> resultList = new List<FreightZoneDto>();
                resultList = await _masterClient.ExportFreightZone(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "FreightZone_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Name"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Zone"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_State"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("msg_Status"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Name.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ZoneName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.StateName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IsActive == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                        }
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    //this.Response.Headers.Clear();
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region FreightRoute Master

        /// <summary>
        /// Method to Get FreightRoute List
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public ActionResult FreightRouteList()
        {
            return View();
        }

        /// <summary>
        /// Method to Get FreightRoute List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetFreightRouteListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData };
            var result = await _masterClient.GetFreightRouteListAsync(loginUserIdDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        /// <summary>
        /// Method to redirect FreightRoute add or update page
        /// </summary>
        /// <param name="freightRouteId"></param>
        /// <returns></returns>
        public ActionResult FreightRouteEditRedirect(string freightRouteId = "")
        {
            Session["FreightRouteId"] = freightRouteId;
            return RedirectToAction("FreightRoute", "Lookup");
        }

        /// <summary>
        /// Method to get FreightRoute add or update page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public async Task<ActionResult> FreightRoute()
        {
            var result = new FreightRouteDto();
            if (Session["FreightRouteId"] != null && UtilityHelper.IntTryToParse(Session["FreightRouteId"].ToString()) > 0)
            {
                result = await _masterClient.GetFreightRouteDetailsById(UtilityHelper.LongTryToParse(Session["FreightRouteId"].ToString()));
            }
            return View(result);
        }

        /// <summary>
        /// Method to add or update FreightRoute
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> AddOrUpdateFreightRoute(FreightRouteDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _masterClient.AddOrUpdateFreightRoute(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("FreightRouteList", "Lookup");
            }
            return View("FreightRoute", result);
        }

        /// <summary>
        /// Method to Get FreightZone List to bind in Dropdown
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetFreightRouteListByZone([DataSourceRequest] DataSourceRequest request, int zoneId = 0)
        {
            IdInputDto inputDto = new IdInputDto { Id = zoneId };
            var result = await _masterClient.GetFreightRouteListByZone(inputDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ExportFreightRoute(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<FreightRouteDto> resultList = new List<FreightRouteDto>();
                resultList = await _masterClient.ExportFreightRoute(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "FreightRoute_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Name"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_FreightZone"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("msg_Status"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Name.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.FreightZoneName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IsActive == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                        }
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    //this.Response.Headers.Clear();
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Territories

        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public ActionResult Territories()
        {
            return View();
        }

        /// <summary>
        /// Method to add or update state
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> AddorUpdateTerritory(TerritoryDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _masterClient.AddOrUpdateTerritory(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("Territories", "Lookup");
            }
            return View("Territory", result);
        }

        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public async Task<ActionResult> Territory()
        {
            var result = new TerritoryDto();
            if (Session["TerritoryId"] != null && UtilityHelper.IntTryToParse(Session["TerritoryId"].ToString()) > 0)
            {
                result = await _masterClient.GerTerritoryById(UtilityHelper.IntTryToParse(Session["TerritoryId"].ToString()));
            }
            return View(result);
        }

        public ActionResult TerritoryEditRedirect(string territoryId = "")
        {
            Session["TerritoryId"] = territoryId;
            return RedirectToAction("Territory", "Lookup");
        }

        public async Task<ActionResult> GetTerritoryListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            var loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData };
            var result = await _masterClient.GerTerritoryList(loginUserIdDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> GerTerritoryMappedDistrict([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData, int territoryId)
        {
            var inputDto = new TerritoryDistrictParam { Id = territoryId, IsToReturnInactiveData = isToReturnInactiveData };
            var result = await _masterClient.GerTerritoryMappedDistrict(inputDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> GerTerritoryStateBase([DataSourceRequest] DataSourceRequest request, string stateId)
        {
            IList<DropDownDto> result = new List<DropDownDto>();
            int state = string.IsNullOrEmpty(stateId) ? 0 : Convert.ToInt32(stateId);
            if (state > 0)
                result = await _masterClient.GerTerritoryStateBase(state);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to Get District List By TerritoryId
        /// </summary>
        /// <param name="request"></param>
        /// <param name="territoryId"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetDistrictListBaseTerritory([DataSourceRequest] DataSourceRequest request, int territoryId)
        {
            IList<DistrictDto> result = new List<DistrictDto>();
            if (territoryId > 0)
            {
                result = await _lookupClient.GetDistrictBasedOnTerritory(territoryId);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ExportTerritory(LoginUserIdDto inputDto)
        {
            var finalResult = new JsonResult();
            try
            {
                string fileName = "Territory_" + string.Format(Settings.ReportDateFormat, DateTime.Now.Date).ToUpper() + ".xlsx";
                bool isHeaderBind = false;
                var resultList = await _masterClient.ExportTerritory(inputDto);

                if (resultList != null && resultList.Any())
                {
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        var rowIndex = 5;
                        var colIndex = 1;
                        var childColIndex = 0;

                        #region Header

                        worksheet.Cells["A1:M1"].Merge = true;
                        worksheet.Cells["A1:M1"].Value = "Adanai Agrotech Ltd.";
                        worksheet.Cells["A1:M1"].Value = Settings.CompanyName;
                        worksheet.Cells["A1:M1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1:M1"].Style.Font.Bold = true;
                        worksheet.Cells["A1:M1"].Style.Font.Size = 16;

                        worksheet.Cells["A2"].Value = "Report Name";
                        worksheet.Cells["B2"].Value = "Territory Details";

                        for (int i = 2; i <= 4; i++)
                        {
                            worksheet.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheet.Cells["A" + i].Style.Font.Bold = true;
                            worksheet.Cells["A" + i].Style.Font.Size = 12;

                            worksheet.Cells["B" + i + ":" + "F" + i].Merge = true;
                            worksheet.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        #endregion

                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_TerritoryName"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_State"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("msg_Status"));

                        foreach (var item in resultList)
                        {
                            isHeaderBind = false;
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.TerritoryName);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.StateName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IsActive == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));

                            if (item.DistrictList != null && item.DistrictList.Any())
                            {
                                foreach (var district in item.DistrictList)
                                {
                                    if (!isHeaderBind)
                                    {
                                        rowIndex++;
                                        childColIndex = 2;

                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_District"));
                                        isHeaderBind = true;
                                    }
                                    rowIndex++;
                                    childColIndex = 2;

                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], district.DistrictName);
                                }
                            }
                        }
                        worksheet.Cells.AutoFitColumns();
                        return SaveExcelFileToPath(package, fileName);
                    }
                }
            }
            catch (Exception)
            {
            }
            return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Discount Dropdownloading

        public async Task<ActionResult> GetStatesBasedOnZone([DataSourceRequest] DataSourceRequest request, string SelectedIds)
        {
            IList<StateDto> stateList = new List<StateDto>();
            if (!string.IsNullOrEmpty(SelectedIds))
            {
                var zoneId = SelectedIds.Split(',').Select(int.Parse).ToList();
                if (zoneId != null)
                {
                    List<long> zoneI = new List<long>();
                    stateList = await _masterClient.GetStatesBasedOnZone(zoneId);
                }
            }
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GerTerritoryBasedOnState([DataSourceRequest] DataSourceRequest request, string SelectedIds)
        {
            IList<DropDownDto> territoryList = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(SelectedIds))
            {
                var territoryIds = SelectedIds.Split(',').Select(int.Parse).ToList();

                if (territoryIds.IsAny())
                {
                    territoryList = await _masterClient.GerTerritoryListByStateIdsForDropdown(territoryIds);
                }
            }
            return Json(territoryList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Users By Role

        public async Task<ActionResult> GetHOUsers([DataSourceRequest] DataSourceRequest request)
        {
            var inputDto = new IdInputDto { Id = (int)DTO.Enums.Role.HOSalesAdmin };
            var result = await _lookupClient.GetUsersByRoleIdddl(inputDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetNSMUsers([DataSourceRequest] DataSourceRequest request)
        {
            var inputDto = new IdInputDto { Id = (int)DTO.Enums.Role.NationalSalesManager };
            var result = await _lookupClient.GetUsersByRoleIdddl(inputDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Configuration

        [AuthorizeClaims(Claims.ManageOrganization, Claims.ManageConfiguration, Claims.ViewConfiguration)]
        [HttpGet]
        public async Task<ActionResult> Configuration()
        {
            ConfigurationViewModel configurationViewModel = new ConfigurationViewModel();
            configurationViewModel = await _lookupClient.GetConfigurationList();
            return View(configurationViewModel);
        }

        [AuthorizeClaims(Claims.ManageOrganization, Claims.ManageConfiguration, Claims.ViewConfiguration)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> Configuration(ConfigurationViewModel configurationViewModel)
        {
            var result = new ConfigurationViewModel();
            var Regex = new Regex(@"^[0-9]+([.][0-9]{1,3})?$");
            //var SaudaConversionMaxValue = configurationViewModel.Configurations.FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.SaudaconversionMaxValue);
            //if (SaudaConversionMaxValue != null)
            //{
            //    if (!Regex.IsMatch(SaudaConversionMaxValue.Value.ToString()))
            //    {
            //        configurationViewModel.PostMessage = "Please enter valid Conversion maximum value";
            //        configurationViewModel.PostStatus = false;
            //        return View(configurationViewModel);
            //    }
            //    if (Convert.ToDecimal(SaudaConversionMaxValue.Value.ToString()) < 0)
            //    {
            //        configurationViewModel.PostMessage = "Please enter valid Conversion maximum value";
            //        configurationViewModel.PostStatus = false;
            //        return View(configurationViewModel);
            //    }
            //}
            //var SaudaConversionMinValue = configurationViewModel.Configurations.FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.SaudaconversionMinValue);
            //if (SaudaConversionMinValue != null)
            //{
            //    if (!Regex.IsMatch(SaudaConversionMinValue.Value.ToString()))
            //    {
            //        configurationViewModel.PostMessage = "Please enter valid Conversion minimum value";
            //        configurationViewModel.PostStatus = false;
            //        return View(configurationViewModel);
            //    }
            //    if (Convert.ToDecimal(SaudaConversionMinValue.Value.ToString()) <= 0)
            //    {
            //        configurationViewModel.PostMessage = "Please enter valid Conversion minimum value";
            //        configurationViewModel.PostStatus = false;
            //        return View(configurationViewModel);
            //    }
            //}

            if (configurationViewModel != null)
            {
                result = await _lookupClient.UpdateConfiguration(configurationViewModel);
                if (result.PostStatus)
                {
                    TempData["Message"] = result.PostMessage;
                    RedirectToAction("Configuration", "Lookup");
                }
            }
            return View(result);
        }


        #endregion

        #region Key Performance Indicator

        /// <summary>
        /// Method to Get Key Performance Indicator
        /// </summary>
        /// <returns></returns>        
        [AuthorizeClaims(Claims.ManageOrganization)]
        public ActionResult KeyPerformanceIndicatorList()
        {
            return View();
        }

        /// <summary>
        /// Method to Get Key Performance Indicator List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetKeyPerformanceIndicatorListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData };
            var result = await _lookupClient.GetKeyPerformanceListAsync(loginUserIdDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        /// <summary>
        /// Method to redirect Key Performance Indicator add or update page
        /// </summary>
        /// <param name="cushionMarginId"></param>
        /// <returns></returns>
        public ActionResult KeyPerformanceIndicatorEditRedirect(string keyPerformId = "")
        {
            Session["KeyPerformanceIndicatorId"] = keyPerformId;
            return RedirectToAction("KeyPerformanceIndicator", "Lookup");
        }

        /// <summary>
        /// Method to get Key Performance Indicator add or update page
        /// </summary>
        /// <returns></returns>        
        [AuthorizeClaims(Claims.ManageOrganization)]
        public async Task<ActionResult> KeyPerformanceIndicator()
        {
            var result = new KeyPerformanceDto();
            if (Session["KeyPerformanceIndicatorId"] != null && UtilityHelper.IntTryToParse(Session["KeyPerformanceIndicatorId"].ToString()) > 0)
            {
                var inputDto = new IdInputDto { Id = UtilityHelper.LongTryToParse(Session["KeyPerformanceIndicatorId"].ToString()) };
                result = await _lookupClient.GetKeyPerformanceById(inputDto);
                result.Content = result != null ? Server.HtmlDecode(result.Content) : "";
            }
            return View(result);
        }

        /// <summary>
        /// Method to  add or update Key Performance Indicator
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateKeyPerformanceIndicator(KeyPerformanceDto inputDto)
        {
            if (inputDto != null)
            {
                inputDto.LoginUserId = UserId;
                //inputDto.Content = Server.HtmlDecode(inputDto.Content);
                var result = await _lookupClient.AddOrUpdateKeyPerformance(inputDto);
                if (result.PostStatus)
                {
                    TempData["SuccessMessage"] = result.PostMessage;
                    return RedirectToAction("KeyPerformanceIndicatorList", "Lookup");
                }
                return View("KeyPerformanceIndicator", result);
            }
            else { return View("KeyPerformanceIndicator", inputDto); }
        }

        #endregion

        #region Status
        /// <summary>
        /// Get all order status
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllStatus()
        {
            var statusIds = new long[] { 1, 2, 3, 5, 6 };
            var result = _masterClient.GetAllStatus().Where(w => statusIds.Contains(w.Id));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSaudaListStatusWithAllOption()
        {
            var statusList = new List<DropDownDto>();
            var statusIds = new long[] { 1, 2, 3, 5, 6 };
            var result = _masterClient.GetAllStatus().Where(w => statusIds.Contains(w.Id));
            foreach (var item in result.ToList())
            {
                if (item.Id == (int)DTO.Enums.Status.Pending)
                {
                    item.Name = "Accepted";
                }
                statusList.Add(item);
            }
            var allItem = new DropDownDto { Id = -1, Name = "All" };
            if (statusList != null && statusList.Any())
            {
                statusList.Insert(0, allItem);
            }
            return Json(statusList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSaudaStatus()
        {
            var statusList = new List<DropDownDto>();
            var statusIds = new long[] { (int)DTO.Enums.Status.Pending, (int)DTO.Enums.Status.Approved, (int)DTO.Enums.Status.Rejected, (int)DTO.Enums.Status.Hold, (int)DTO.Enums.Status.Completed };
            var result = _masterClient.GetAllStatus().Where(w => statusIds.Contains(w.Id));
            foreach (var item in result.ToList())
            {
                if (item.Id == (int)DTO.Enums.Status.Pending)
                {
                    item.Name = "Accepted";
                }
                statusList.Add(item);
            }
            return Json(statusList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingApprovedRejectedStatusddl()
        {
            var statusIds = new long[] { 1, 2, 3 };
            var result = _masterClient.GetAllStatus().Where(w => statusIds.Contains(w.Id));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get  order status 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSelectiveStatus()
        {
            var statusIds = new long[] { 1, 2, 3 };
            var result = _masterClient.GetAllStatus().Where(w => statusIds.Contains(w.Id));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get  order status 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSpecialrateCompetitorSelectiveStatus()
        {
            var statusIds = new long[] { 1, 2, 3, 9 };
            var result = _masterClient.GetAllStatus().Where(w => statusIds.Contains(w.Id));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingApprovedStatus()
        {
            var statusIds = new long[] { 1, 2 };
            var result = _masterClient.GetAllStatus().Where(w => statusIds.Contains(w.Id));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingApprovedStatusWithAllOption()
        {
            var statusIds = new long[] { (long)Status.Pending, (long)Status.Approved, (long)Status.Rejected };
            var statusList = _masterClient.GetAllStatus().Where(w => statusIds.Contains(w.Id)).ToList();
            var allItem = new DropDownDto { Id = -1, Name = "All" };
            if (statusList != null && statusList.Any())
            {
                statusList.Insert(0, allItem);
            }
            return Json(statusList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Verticals

        /// <summary>
        /// Get  order status 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllVerticals()
        {
            var result = _masterClient.GetAllVerticals();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Lookup

        /// <summary>
        /// Method to Get District List By StateId
        /// </summary>     
        /// <param name="stateId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetDistrictListByStateIdAsync(string stateId)
        {
            var stateID = 0;
            stateID = !string.IsNullOrEmpty(stateId) ? int.Parse(stateId) : 0;
            var result = await _lookupClient.GetDistrictListByStateIdAsync(stateID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DistrictListPartial(string stateId)
        {
            var territory = new TerritoryDto();
            var stateID = 0;
            stateID = !string.IsNullOrEmpty(stateId) ? int.Parse(stateId) : 0;
            var result = await _lookupClient.GetUnMappedDistrictListByStateId(stateID);
            if (result != null && result.Any())
            {
                result.ToList().ForEach(f =>
                {
                    territory.District.Add(new CheckBoxDto()
                    {
                        Id = f.DistrictId,
                        Name = f.DistrictName
                    });
                });
            }
            return View(territory);
        }

        public async Task<ActionResult> GetUsersByRoleId([DataSourceRequest] DataSourceRequest request, long roleId)
        {
            IList<DropDownDto> result = new List<DropDownDto>();
            if (roleId > 0)
            {
                var inputDto = new IdInputDto { Id = roleId };
                result = await _lookupClient.GetUsersByRoleId(inputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetSkuListByPackGroupIdAsync([DataSourceRequest] DataSourceRequest request, long oilTypeId, long packGroupId)
        {
            IList<DropDownDto> result = new List<DropDownDto>();
            if (oilTypeId > 0 && packGroupId > 0)
            {
                var idInputDto = new SkuDropDownInputDto { OilTypeId = oilTypeId, PackGroupId = packGroupId };
                result = await _lookupClient.GetSkuListByPackGroupIdAsync(idInputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetTradeTicketOilTypes([DataSourceRequest] DataSourceRequest request, long VerticalId)
        {
            IList<DropDownDto> result = new List<DropDownDto>();
            var inputDto = new IdInputDto { Id = VerticalId };
            result = await _lookupClient.GetTradeTicketOilTypes(inputDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetDepotsByPlantIds([DataSourceRequest] DataSourceRequest request, string plantIds)
        {
            var ids = new List<long>();
            IList<DropDownDto> result = new List<DropDownDto>();

            if (!string.IsNullOrEmpty(plantIds))
            {
                ids = plantIds.Split(',').Select(Int64.Parse).ToList();
                if (plantIds != null && plantIds.Any())
                {
                    var inputDto = new DepotDropDownParam() { PlantIds = ids };
                    result = await _lookupClient.GetDepotsByPlantIds(inputDto);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Material Cost page Vertical dropdown loading
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> MaterialCostGetVerticalDetailsdd()
        {
            LoginUserIdDto inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
            var verticalList = await _masterClient.GetVerticalDetailsAsync(inputDto);
            verticalList = verticalList.Where(w => w.Id != (Int64)DTO.Enums.Division.SpecialityFat).Select(s => s).ToList();
            return Json(verticalList, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetSkuIngredienOilTypes([DataSourceRequest] DataSourceRequest request, int? verticalId)
        {
            IList<DropDownDto> result = new List<DropDownDto>();
            if (verticalId > 0)
            {
                IdInputDto inputDto = new IdInputDto { Id = (int)verticalId };
                result = await _lookupClient.GetSkuIngredienOilTypes(inputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> MaterialCostGetOilTypesBasedOnVerticalId([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData, int? verticalId)
        {
            IList<DropDownDto> result = new List<DropDownDto>();
            if (verticalId > 0)
            {
                IdInputDto inputDto = new IdInputDto { Id = (int)verticalId, IsToReturnInactiveData = isToReturnInactiveData };
                result = await _lookupClient.MaterialCostOilTypesBasedOnVerticalId(inputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get SKU dropdown details
        /// </summary>
        /// <param name="request"></param>
        /// <param name="oilTypeId"></param>
        /// <param name="subCategoryId"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetSkuBasedOnOilTypeSubCategory([DataSourceRequest] DataSourceRequest request, bool IsToReturnInactiveData, long oilTypeId, long subCategoryId = 0, long packGroupId = 0)
        {
            IList<DropDownDto> result = new List<DropDownDto>();
            SkuDropDownInputDto inputDto = new SkuDropDownInputDto { OilTypeId = oilTypeId, SubCategoryId = subCategoryId, PackGroupId = packGroupId };

            if (oilTypeId > 0)
            {
                result = await _lookupClient.GetSkuBasedOnOilTypeSubCategory(inputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetSkulistBasedOnCombination([DataSourceRequest] DataSourceRequest request, long distId, long saleId, long divisionId)
        {
            IList<DropDownDto> result = new List<DropDownDto>();
            LoginUserIdDto inputDto = new LoginUserIdDto { SalesOrganizationId = saleId, DistributionChannelId = distId, DivisionId = divisionId , LoginUserId = base.UserId };
            result = await _lookupClient.GetSkulistBasedOnCombination(inputDto);
            result = result.Select(s => new DropDownDto()
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name.Split('/').ToArray().FirstOrDefault(),
            }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<JsonResult> GetOilTypeIsRasoiOrNot(long oilTypeId)
        {
            IdInputDto inputDto = new IdInputDto { Id = oilTypeId };
            var result = await _lookupClient.GetOilTypeIsRasoiOrNot(inputDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetPriceNotifySkusBasedOnOilType([DataSourceRequest] DataSourceRequest request, bool IsToReturnInactiveData, long oiltypeId)
        {
            var skuData = new List<DropDownDto>();
            if (oiltypeId > 0)
            {
                var skuInputDto = new SkuInputDto() { OilTypeId = oiltypeId, IsToReturnInactiveData = IsToReturnInactiveData };
                var result = await _lookupClient.GetSkusBasedOnOilType(skuInputDto);
                if (result != null && result.Any())
                {
                    skuData = result.Select(f => new DropDownDto() { Id = f.SkuId, Name = f.SkuName }).ToList();
                }
            }
            return Json(skuData, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetStateBasedOnZone([DataSourceRequest] DataSourceRequest request, string zoneId)
        {
            var result = new List<DropDownDto>();
            int zone = string.IsNullOrEmpty(zoneId) ? 0 : Convert.ToInt32(zoneId);
            if (zone > 0)
            {
                var stateList = await _masterClient.GetZoneMappedStates(zone);
                if (stateList != null && stateList.Any())
                {
                    result = stateList.ToList().Select(f => new DropDownDto() { Id = f.StateId, Name = f.StateName }).ToList();
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GerDistrictBasedOnTerritory([DataSourceRequest] DataSourceRequest request, string territoryId)
        {
            IList<DropDownDto> result = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(territoryId))
            {
                var territoryIds = territoryId.Split(',').Select(int.Parse).ToList();
                result = await _masterClient.GerDistrictBasedOnTerritory(territoryIds);
            }
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public async Task<JsonResult> GetCityListBasedOnDistrict([DataSourceRequest] DataSourceRequest request, string districtId)
        {
            IList<DropDownDto> result = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(districtId))
            {
                var districtIds = districtId.Split(',').Select(int.Parse).ToList();
                result = await _masterClient.GetCityListBasedOnDistrict(districtIds);
            }
            var jsonResult = Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public async Task<ActionResult> CityListValueMapper(long [] values,string districtId)
        {
            var indices = new List<long>();
            IList<DropDownDto> result = new List<DropDownDto>();

            if (values != null && values.Any())
            {
                var index = 0;

                if (!string.IsNullOrEmpty(districtId))
                {
                    var districtIds = districtId.Split(',').Select(int.Parse).ToList();
                    result = await _masterClient.GetCityListBasedOnDistrict(districtIds);

                    foreach (var item in result)
                    {
                        if (values.Contains(item.Id))
                        {
                            indices.Add(index);
                        }

                        index += 1;
                    }
                }
            }

            return Json(indices, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetFreightRouteByZone([DataSourceRequest] DataSourceRequest request, string freightZoneId)
        {
            IList<DropDownDto> result = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(freightZoneId))
            {
                var freightZoneIds = freightZoneId.Split(',').Select(int.Parse).ToList();
                result = await _masterClient.GetFreightRouteByZone(freightZoneIds);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetStatesBasedOnZoneIds(string SelectedIds)
        {
            var stateList = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(SelectedIds))
            {
                var zoneId = SelectedIds.Split(',').Select(int.Parse).ToList();
                if (zoneId != null)
                {
                    List<long> zoneI = new List<long>();
                    var result = await _masterClient.GetStatesBasedOnZone(zoneId);
                    if (result != null && result.Any())
                    {
                        foreach (var data in result)
                        {
                            stateList.Add(new DropDownDto() { Id = data.StateId, Name = data.StateName });
                        }
                    }
                }
            }
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetZonalHeadBasedonZoneState(string SelectedZoneIds, string SelectedStateIds)
        {
            var zonalHeadList = new List<DropDownDto>();

            ZonalHeadMappingDto zonalHeadMappingDto = new ZonalHeadMappingDto();
            if (!string.IsNullOrEmpty(SelectedZoneIds) && !string.IsNullOrEmpty(SelectedStateIds))
            {
                var zoneId = SelectedZoneIds.Split(',').Select(int.Parse).ToList();
                var stateId = SelectedStateIds.Split(',').Select(int.Parse).ToList();

                if (zoneId != null && stateId != null)
                {
                    List<long> zoneI = new List<long>();
                    List<long> stateI = new List<long>();

                    zonalHeadMappingDto.ZoneIds = zoneId;
                    zonalHeadMappingDto.StateIds = stateId;

                    var result = await _masterClient.GetZonalHeadBasedonZoneState(zonalHeadMappingDto);
                    if (result != null && result.Any())
                    {
                        foreach (var data in result)
                        {
                            zonalHeadList.Add(new DropDownDto() { Id = data.ZonalHeadId, Name = data.ZonalHeadName });
                        }
                    }
                }
            }
            return Json(zonalHeadList, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> GetSaudaOrderLiftingRequestExcelExport(bool IsToReturnInactiveData)
        {
            var finalResult = new JsonResult();
            try
            {
                string fileName = "USER_LIST_" + string.Format(Settings.ReportDateFormat, DateTime.Now.Date).ToUpper() + ".xlsx";
                LoginUserIdDto inputDto = new LoginUserIdDto() { IsToReturnInactiveData = IsToReturnInactiveData, LoginUserId = UserId, VerticalId = VerticalId };
                var userDetails = await _masterClient.GetUserExcelExportList(inputDto);

                if (userDetails != null && userDetails.Any())
                {
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("User List");
                        var rowIndex = 1;
                        var colIndex = 1;

                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_EmployeeName"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_EmployeeCode"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Role"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Vertical"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_MobileNumber"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Email"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CompanyCode"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Designation"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_HQ"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_State"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Zone"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_District"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Territory"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_City"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Address")+ " 1");
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Address")+ " 2");
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Password"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SaudaBookingType"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ReportingTo"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SalesReportingTo"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_UserMappedDealerBrokerCodes"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IsActive"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_AdditionalMobileNumber"));

                        foreach (var user in userDetails)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.EmployeeName != null ? user.EmployeeName.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.EmployeeCode != null ? user.EmployeeCode.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.RoleName != null ? user.RoleName.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.Vertical != null ? user.Vertical.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.MobileNumber != null ? user.MobileNumber.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.Email != null ? user.Email.ToString() : string.Empty);
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.CompanyCode != null ? user.CompanyCode.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.Designation != null ? user.Designation.ToString() : string.Empty);
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.Headquarters != null ? user.Headquarters.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.State != null ? user.State.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.Zone != null ? user.Zone.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.District != null ? user.District.ToString() : string.Empty);
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.Territory != null ? user.Territory.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.City != null ? user.City.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.Address1 != null ? user.Address1.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.Address2 != null ? user.Address2.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.Password != null ? user.Password.ToString() : string.Empty);
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.SaudaBookingType != null ? user.SaudaBookingType.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.OrganizationReportingToName != null ? user.OrganizationReportingToName.ToString() : string.Empty);
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.SalesReportingToName != null ? user.SalesReportingToName.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.CustomerCode != null ? user.CustomerCode.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.IsActive.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], user.AdditionalMobileNumber != null ? user.AdditionalMobileNumber.ToString() : string.Empty);
                        }
                        worksheet.Cells.AutoFitColumns();
                        return SaveExcelFileToPath(package, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                var test = ex.InnerException.Message;
            }
            return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region SubCategory

        /// <summary>
        /// Method to Get SubCategory List
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public ActionResult SubCategoryList()
        {
            return View();
        }

        /// <summary>
        /// Method to Get SubCategory List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetSubCategoryListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            request.Filters = Utility.ToFilterDescriptor(request.Filters);
            KendoGridResult loginUserIdDto = new KendoGridResult { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData, VerticalId = VerticalId, DataSourceRequest = request };
            var result = await _masterClient.GetSubCategoryListAsync(loginUserIdDto);
            //var resultList = result.ToDataSourceResult(request);
            return Json(result);
        }

        /// <summary>
        /// Method to redirect SubCategory add or update page
        /// </summary>
        /// <param name="subCategoryId"></param>
        /// <returns></returns>
        public ActionResult SubCategoryEditRedirect(string subCategoryId = "")
        {
            Session["SubCategoryId"] = subCategoryId;
            return RedirectToAction("SubCategory", "Lookup");
        }

        /// <summary>
        /// Method to get SubCategory add or update page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public async Task<ActionResult> SubCategory()
        {
            var result = new SubCategoryDto();
            if (Session["SubCategoryId"] != null && UtilityHelper.IntTryToParse(Session["SubCategoryId"].ToString()) > 0)
            {
                result = await _masterClient.GetSubCategoryDetailsById(UtilityHelper.LongTryToParse(Session["SubCategoryId"].ToString()));
            }
            return View(result);
        }

        /// <summary>
        /// Method to add or update SubCategory
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateSubCategory(SubCategoryDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _masterClient.AddOrUpdateSubCategory(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("SubCategoryList", "Lookup");
            }
            inputDto.PostStatus = result.PostStatus;
            inputDto.PostMessage = result.PostMessage;
            return View("SubCategory", inputDto);
        }

        public async Task<ActionResult> ExportSubCategory(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<SubCategoryDto> resultList = new List<SubCategoryDto>();
                resultList = await _masterClient.ExportSubCategory(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "SubCategory_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Name"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("msg_Status"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Name.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IsActive == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                        }
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    //this.Response.Headers.Clear();
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Rake

        public ActionResult RakeList()
        {
            return View();
        }

        public async Task<ActionResult> GetRakeListAsync([DataSourceRequest] DataSourceRequest request, bool IsToReturnInactiveData)
        {
            return Json(await _masterClient.GetKendoGridDataAsync<RakeDto>(GridResultInputDto(request, IsToReturnInactiveData), ApiUrl.WebApiUrlGetRakeListWithPagination));
        }

        public ActionResult RakeEditRedirect(string rakeId = "")
        {
            Session["RakeId"] = rakeId;
            return RedirectToAction("Rake", "Lookup");
        }

        public async Task<ActionResult> Rake()
        {
            var result = new RakeDto();
            if (Session["RakeId"] != null && UtilityHelper.IntTryToParse(Session["RakeId"].ToString()) > 0)
            {
                IdInputDto inputDto = new IdInputDto() { Id = UtilityHelper.LongTryToParse(Session["RakeId"].ToString()) };
                result = await _masterClient.GetRakeById(inputDto);
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateRake(RakeDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _masterClient.AddOrUpdateRake(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("RakeList", "Lookup");
            }
            return View("Rake", result);
        }

        public async Task<ActionResult> GetDepotRakeddListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            IdInputDto inputDto = new IdInputDto() { IsToReturnInactiveData = isToReturnInactiveData };
            var result = await _masterClient.GetDepotRakeddList(inputDto, ApiUrl.WebApiUrlGetDepotRakeList);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetDepotRakeByPlantIdAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData, long plantId)
        {
            IList<DepotRakeDto> result = new List<DepotRakeDto>();
            if (plantId > 0)
            {
                IdInputDto inputDto = new IdInputDto() { IsToReturnInactiveData = isToReturnInactiveData, Id = plantId };
                result = await _masterClient.GetDepotRakeddList(inputDto, ApiUrl.WebApiUrlGetDepotRakeByPlantId);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetDepotddlListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto inputDto = new LoginUserIdDto() { IsToReturnInactiveData = isToReturnInactiveData };
            var result = await _masterClient.GetDepotListAsync(inputDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get state list
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetStateddListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var dropDownList = new List<DropDownDto>();
            var result = await _lookupClient.GetStateListAsync();
            if (result != null && result.Any())
            {
                dropDownList = result.Select(s => new DropDownDto() { Id = s.StateId, Name = s.StateName }).OrderBy(o => o.Name).ToList();
            }
            return Json(dropDownList, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetDepotRakePlantddList([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            IdInputDto inputDto = new IdInputDto() { IsToReturnInactiveData = isToReturnInactiveData };
            var result = await _masterClient.GetDepotRakeddList(inputDto, ApiUrl.WebApiUrlGetDepotRakePlantddList);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetDepotPlantddList([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            IdInputDto inputDto = new IdInputDto() { IsToReturnInactiveData = isToReturnInactiveData };
            var result = await _masterClient.GetDepotPlantddList(inputDto, ApiUrl.WebApiUrlGetDepotPlantddList);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ExportRake(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<RakeDto> resultList = new List<RakeDto>();
                resultList = await _masterClient.ExportRake(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "Rake_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_RakeCode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_RakeName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Address"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Zone"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_State"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Territory"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_District"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_City"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Email"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Pincode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Depot"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_AssociatedPlant"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_AssociatedState"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("msg_Status"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Code);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Name.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Location.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ZoneName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.State.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.TerritoryName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.District.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.City.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Email.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.PinCode.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.DepotName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.AssociatedPlantCodes.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.AssociatedStates.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IsActive == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                        }
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    //this.Response.Headers.Clear();
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Lookup

        /// <summary>
        /// Method to Get TransPortMode List based on Depot/Rake
        /// </summary>
        /// <param name="request"></param>
        /// <param name="IsToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetTransportModeBasedonDepotRake([DataSourceRequest] DataSourceRequest request, bool IsToReturnInactiveData, long id)
        {
            IList<DropDownDto> result = new List<DropDownDto>();
            if (id > 0)
            {
                IdInputDto inputDto = new IdInputDto { IsToReturnInactiveData = IsToReturnInactiveData, Id = id };
                result = await _masterClient.GetTransportModeBasedonDepotRake(inputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetOilTypesByVerticalId([DataSourceRequest] DataSourceRequest request, int? verticalId)
        {
            IList<DropDownDto> result = new List<DropDownDto>();
            if (verticalId > 0)
            {
                IdInputDto inputDto = new IdInputDto { Id = (int)verticalId };
                result = await _lookupClient.GetOilTypesByVerticalId(inputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetDepotPlantRakeBasedOnState([DataSourceRequest] DataSourceRequest request, int? stateId)
        {
            IList<DropDownDto> result = new List<DropDownDto>();
            if (stateId > 0)
            {
                IdInputDto inputDto = new IdInputDto { Id = (int)stateId };
                result = await _lookupClient.GetPlantDepotRakeByStateId(inputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetFreightZoneListByStateId([DataSourceRequest] DataSourceRequest request, int stateId)
        {
            IList<DropDownDto> result = new List<DropDownDto>();
            if (stateId > 0)
            {
                IdInputDto inputDto = new IdInputDto { Id = (int)stateId };
                result = await _lookupClient.GetFreightZoneByStateId(inputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetCustomerOnCity([DataSourceRequest] DataSourceRequest request, string CityIds)
        {
            IList<DropDownDto> result = new List<DropDownDto>();

            List<int> CityId = UtilityHelper.ConvertStringToIntList(CityIds);
            if (CityIds != null && CityIds.Any())
            {
                result = await _masterClient.GetCustomerOnCity(CityId);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ShipToParty

        /// <summary>
        /// Method to Get ShipToParty List page
        /// </summary>
        /// <returns></returns>

        [AuthorizeClaims(Claims.ManageShipToParty, Claims.ViewShipToParty)]
        public ActionResult ShipToPartyList()
        {
            return View();
        }

        /// <summary>
        /// Method to Get ShipToParty List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetShipToPartyListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            return Json(await _masterClient.GetKendoGridDataAsync<ShipToPartyDto>(GridResultInputDto(request, isToReturnInactiveData), ApiUrl.WebApiUrlGetShipToPartyList));
        }


        /// <summary>
        /// Method to redirect shipToParty add or update page
        /// </summary>
        /// <param name="shipToPartyId"></param>
        /// <returns></returns>
        public ActionResult SPERedirect(string EncryptedId = "")
        {
            Session["ShipToPartyId"] = EncryptedId;
            return RedirectToAction("ShipToParty", "Lookup");
        }

        /// <summary>
        /// Method to get ShipToParty add or update page
        /// </summary>
        /// <returns></returns>        
        [AuthorizeClaims(Claims.ManageShipToParty, Claims.ViewShipToParty)]
        public async Task<ActionResult> ShipToParty()
        {
            var result = new EmployeeDto();
            Session["SelectedDepotIds"] = null;
            Session["SelectedShipToPartyIds"] = null;
            if (!String.IsNullOrEmpty(Session["ShipToPartyId"].ToString()))
            {
                result = await _masterClient.GetShipToPartyDetailsById(Session["ShipToPartyId"].ToString());
                if (result.SelectedDealerBrokerIds != null && result.SelectedDealerBrokerIds.Any())
                {
                    result.SelecteDealerBrokerIdsString = UtilityHelper.ConvertLongListToCommaSeparatedString(result.SelectedDealerBrokerIds);
                    Session["SelectedDepotIds"] = result.SelectedDealerBrokerIds;
                }
                if (result.SelectedDealerIds != null && result.SelectedDealerIds.Any())
                {
                    result.SelecteDealerIdsString = UtilityHelper.ConvertLongListToCommaSeparatedString(result.SelectedDealerIds);
                    Session["SelectedShipToPartyIds"] = result.SelectedDealerIds;
                }
            }
            return View(result);
        }

        /// <summary>
        /// Method to add or update shipToParty
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateShipToParty(EmployeeDto inputDto, string PickupLocation,string DivisionList="")
        {
            inputDto.LoginUserId = UserId;
            inputDto.RoleId = (int)Role.ShipToParty;
            inputDto.DivisionList = GMCore.Helper.JsonHelper.ConvertJSonToObjectList<DivisionDetailsDto>(DivisionList);
            var result = await _masterClient.AddOrUpdateShipToParty(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("ShipToPartyList", "Lookup");
            }
            return View("ShipToParty", result);
        }

        public async Task<ActionResult> GenerateExcelShipToPartyAsync()
        {
            var stream = new MemoryStream();
            var fileName = "";
            string guidFileName = "";
            var result = new ResultModel { IsSuccess = false, Message = "Error Occured while exporting Excel. Please retry." };
            try
            {
                var shipToPartyDetails = await _masterClient.GetShipToPartyListAsync1(new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = true, VerticalId = VerticalId });
                fileName = "SHIP-TO-PARTY-LIST-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";

                using (var ep = new ExcelPackage())
                {

                    var ws = ep.Workbook.Worksheets.Add("ShipToParty List");

                    #region Header
                    ws.Cells["A1:F1"].Merge = true;
                    ws.Cells["A1:F1"].Value = Settings.CompanyName;
                    ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["A1:F1"].Style.Font.Bold = true;
                    ws.Cells["A1:F1"].Style.Font.Size = 16;

                    ws.Cells["A2"].Value = "Sheet Name";
                    ws.Cells["A3"].Value = "Total Record Count";
                    ws.Cells["A4"].Value = "Date and Time";



                    for (int j = 2; j <= 5; j++)
                    {
                        ws.Cells["A" + j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["A" + j].Style.Font.Bold = true;
                        ws.Cells["A" + j].Style.Font.Size = 12;

                        ws.Cells["B" + j + ":" + "F" + j].Merge = true;
                        ws.Cells["B" + j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    }



                    ws.Cells["B2"].Value = "ShipToParty List";
                    ws.Cells["B3"].Value = shipToPartyDetails.Count;
                    ws.Cells["B4"].Value = DateHelper.UtcToIndia(DateTime.UtcNow).ToString("dd-MM-yyyy HH:mm tt");
                    ws.Cells["A4"].Style.Font.Bold = true;
                    ws.Cells["A4"].Style.Font.Size = 12;



                    #endregion


                    ws.Cells["A7:Q" + shipToPartyDetails.Count].LoadFromCollection(shipToPartyDetails, true);
                    ExcelRange range = ws.Cells["A7:Q7"];
                    range.AutoFitColumns();
                    range.Style.Font.Size = 12;
                    range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                    range.Style.Font.Bold = true;
                    int contentIndex = 8;

                    ws.Cells["A7" + ":" + "Q" + contentIndex + shipToPartyDetails.Count].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    ws.Cells.AutoFitColumns();


                    #endregion

                    guidFileName = SaveExcelFileToPath(ep);
                }

                //}
                result.IsSuccess = true;
                result.Message = fileName;

                #region OldCode
                // Create the package and make sure you wrap it in a using statement
                //using (var package = new ExcelPackage())
                //{
                //    // add a new worksheet to the empty workbook
                //    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                //    Response.ClearHeaders();
                //    Response.ClearContent();
                //    Response.Clear();
                //    var rowIndex = 1;
                //    var colIndex = 1;

                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "ShipToPartyName");
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "ShipToPartyCode");
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_MobileNumber"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Email"));
                //    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CompanyCode"));
                //    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SaudaValidityPeriod"));
                //    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SaudaLimit"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Vertical"));
                //    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SaudaBookingType"));
                //    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PlantTruckCapacity"));
                //    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DepotTruckCapacity"));
                //    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_TransportMode"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Zone"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_State"));
                //    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Territory"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_District"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_City"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Pincode"));
                //    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Address"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Address1"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Address2"));
                //    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_FreightZoneName"));
                //    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_FreightRouteName"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_GSTN"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IncoTerms"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BrokerCode"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Plant"));
                //    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Depot"));
                //   // GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_FssaiNumber"));
                //    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BDO"));
                //    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BDOCode"));
                //    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ShipToParty"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Password"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IsActive"));

                //    foreach (var shipToParty in shipToPartyDetails)
                //    {
                //        rowIndex++;
                //        colIndex = 1;
                //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.Name != null ? shipToParty.Name.ToString() : string.Empty);
                //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.Code != null ? shipToParty.Code.ToString() : string.Empty);
                //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.MobileNumber != null ? shipToParty.MobileNumber.ToString() : string.Empty);
                //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.Email != null ? shipToParty.Email.ToString() : string.Empty);
                //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.CompanyCode != null ? shipToParty.CompanyCode.ToString() : string.Empty);
                //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.SaudaValidityPeriod.ToString());
                //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.SaudaLimit.ToString());
                //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.VerticalName != null ? shipToParty.VerticalName.ToString() : string.Empty);
                //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.SaudaBookingType != null ? shipToParty.SaudaBookingType.ToString() : string.Empty);
                //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.PlantTruckCapacities != null ? shipToParty.PlantTruckCapacities.ToString() : string.Empty);
                //       //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.DepotTruckCapacities != null ? shipToParty.DepotTruckCapacities.ToString() : string.Empty);
                //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.TransportMode != null ? shipToParty.TransportMode.ToString() : string.Empty);
                //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.Zone != null ? shipToParty.Zone.ToString() : string.Empty);
                //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.State != null ? shipToParty.State.ToString() : string.Empty);
                //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.Territory != null ? shipToParty.Territory.ToString() : string.Empty);
                //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.District != null ? shipToParty.District.ToString() : string.Empty);
                //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.City != null ? shipToParty.City.ToString() : string.Empty);
                //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.Pincode != null ? shipToParty.Pincode.ToString() : string.Empty);
                //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.Address != null ? shipToParty.Address.ToString() : string.Empty);
                //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.Address1 != null ? shipToParty.Address1.ToString() : string.Empty);
                //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.Address2 != null ? shipToParty.Address2.ToString() : string.Empty);
                //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.FreightZoneName != null ? shipToParty.FreightZoneName.ToString() : string.Empty);
                //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.FreightRouteName != null ? shipToParty.FreightRouteName.ToString() : string.Empty);
                //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.GSTN != null ? shipToParty.GSTN.ToString() : string.Empty);
                //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.Incoterms != null ? shipToParty.Incoterms.ToString() : string.Empty);
                //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.BrokerCode != null ? shipToParty.BrokerCode.ToString() : string.Empty);
                //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.Plants != null ? shipToParty.Plants.ToString() : string.Empty);
                //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.Depots != null ? shipToParty.Depots.ToString() : string.Empty);
                //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.FSSAINumber != null ? shipToParty.FSSAINumber.ToString() : string.Empty);
                //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.StateTrader != null ? shipToParty.StateTrader.ToString() : string.Empty);
                //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.BDOCode != null ? shipToParty.BDOCode.ToString() : string.Empty);
                //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.ShipToParty != null ? shipToParty.ShipToParty.ToString() : string.Empty);
                //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.Password != null ? shipToParty.Password.ToString() : string.Empty);
                //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], shipToParty.IsActive.ToString());
                //    }

                //    foreach (var workSheet in package.Workbook.Worksheets)
                //    {
                //        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                //        {
                //            try
                //            {
                //                workSheet.Column(i).AutoFit();
                //                workSheet.Column(i).BestFit = true;
                //            }
                //            catch
                //            {
                //                // ignored
                //            }
                //        }

                //        try
                //        {
                //            var cells = workSheet.Cells[workSheet.Dimension.Address];
                //            cells.AutoFitColumns();
                //        }
                //        catch
                //        {
                //            // ignored
                //        }
                //    }

                //    this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //    this.Response.AddHeader(
                //              "content-disposition",
                //              string.Format("attachment;  filename={0}", fileName));
                //    this.Response.BinaryWrite(package.GetAsByteArray());
                //}
                //result.IsSuccess = true;
                //result.Message = fileName;
                #endregion
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "Excel Error" + ex;
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetShipToPartyBrokerTransportModeListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            DeliveryTypeInputDto deliveryTypeDto = new DeliveryTypeInputDto { IsToReturnInactiveData = isToReturnInactiveData, LoginUserId = UserId, SelectedTypeId = (int)MasterDataTypes.TransaportMode };
            var result = await _masterClient.GetDeliveryDetailsAsync(deliveryTypeDto);
            result = result != null ? result.Where(_ => _.Id == (int)DTO.Enums.TransportMode.Truck).ToList() : result;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShipToPartyGridPartial(UserDetailsViewModel userDetailsViewModel)
        {
            if (userDetailsViewModel.IsRemoveSelectedDealerIdsFromSession)
                Session["SelectedDealerIds"] = null;
            return View(userDetailsViewModel);
        }

        public async Task<ActionResult> GetShipToPartyForPopup([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData, string verticalId)
        {
            
            var gridData = GMCore.Helper.JsonHelper.ConvertJSonToObjectList<DivisionDetailsDto>(verticalId);
            var loginUserDto = new DealerBrokerParamDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData,DivisionList=gridData };
            var dealersList = await _masterClient.GetShipToPartyBasedOnVertical(loginUserDto);
            if (Session["SelectedDealerIds"] != null && dealersList.Any())
            {
                var customerIds = (List<long>)Session["SelectedDealerIds"];
                dealersList.Where(w => customerIds.Any(c => c == w.Id)).Select(s => s.IsChecked = true).ToList();
                dealersList = dealersList.OrderByDescending(o => o.IsChecked).ToList();
            }
            var resultList = dealersList.ToDataSourceResult(request);
            return Json(resultList);
        }

          
        
        #region CustomerGroupFive

        public ActionResult CustomerGroupFiveList()
        {
            return View();
        }


        public ActionResult CGFERedirect(string EncryptedId = "")
        {
            Session["CustomerGroupId"] = EncryptedId;
            return RedirectToAction("CustomerGroupFive", "Lookup");
        }


        public async Task<ActionResult> CustomerGroupFiveListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _masterClient.GetCustomerGroupFiveListAsync();
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> CustomerGroupFive()
        {
            var result = new CustomerGroupFiveDto();
            if (!String.IsNullOrEmpty(Session["CustomerGroupId"].ToString()))
            {
                result = await _masterClient.GetCustomerGroupFiveDetailsById(Session["CustomerGroupId"].ToString());
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateCustomerGroupFive(CustomerGroupFiveDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _masterClient.AddOrUpdateCustomerGroupFive(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("CustomerGroupFiveList", "Lookup");
            }
            inputDto.PostStatus = result.PostStatus;
            inputDto.PostMessage = result.PostMessage;
            return View("CustomerGroupFive", inputDto);
        }

        public ActionResult ExportCustomerGroupFive(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");

                var resultList = _masterClient.ExportCustomerGroupFive(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "CustomerGroupFive_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    //Header
                    worksheet.Cells["A1:BZ1"].Style.Font.Size = 13;
                    worksheet.Cells["A1:BZ1"].Style.Font.Name = "Calibri";
                    worksheet.Cells["A1:BZ1"].Style.Font.Bold = true;

                    worksheet.Cells.LoadFromCollection(resultList, true);
                    worksheet.Cells.AutoFitColumns();

                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region CustomerGroupOne and CustomerGroupTwo

        public ActionResult CustomerGroupOneList()
        {
            return View();
        }

        public ActionResult CustomerGroupOneEditRedirect(string CustomerGroupId = "")
        {
            Session["CustomerGroupId"] = CustomerGroupId;
            return RedirectToAction("CustomerGroupOne", "Lookup");
        }

        public async Task<ActionResult> CustomerGroupOne()
        {
            var result = new CustomerGroupOneDto();
            if (Session["CustomerGroupId"] != null && UtilityHelper.IntTryToParse(Session["CustomerGroupId"].ToString()) > 0)
            {
                result = await _masterClient.GetCustomerGroupOneDetailsById(UtilityHelper.LongTryToParse(Session["CustomerGroupId"].ToString()));
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> AddOrUpdateCustomerGroupOne(CustomerGroupOneDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _masterClient.AddOrUpdateCustomerGroupOne(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("CustomerGroupOneList", "Lookup");
            }
            inputDto.PostStatus = result.PostStatus;
            inputDto.PostMessage = result.PostMessage;
            return View("CustomerGroupOne", inputDto);
        }

        public async Task<ActionResult> CustomerGroupOneListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _masterClient.GetCustomerGroupOneListAsync();
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public ActionResult CustomerGroupTwoList()
        {
            return View();
        }

        public ActionResult CustomerGroupTwoEditRedirect(string CustomerGroupId = "")
        {
            Session["CustomerGroupId"] = CustomerGroupId;
            return RedirectToAction("CustomerGroupTwo", "Lookup");
        }

        public async Task<ActionResult> CustomerGroupTwo()
        {
            var result = new CustomerGroupOneDto();
            if (Session["CustomerGroupId"] != null && UtilityHelper.IntTryToParse(Session["CustomerGroupId"].ToString()) > 0)
            {
                result = await _masterClient.GetCustomerGroupTwoDetailsById(UtilityHelper.LongTryToParse(Session["CustomerGroupId"].ToString()));
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> AddOrUpdateCustomerGroupTwo(CustomerGroupOneDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _masterClient.AddOrUpdateCustomerGroupTwo(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("CustomerGroupTwoList", "Lookup");
            }
            inputDto.PostStatus = result.PostStatus;
            inputDto.PostMessage = result.PostMessage;
            return View("CustomerGroupTwo", inputDto);
        }

        public async Task<ActionResult> CustomerGroupTwoListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _masterClient.GetCustomerGroupTwoListAsync();
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> GetAllCustomerGroupOneddl()
        {
            var result = await _masterClient.GetAllCustomerGroupOneddl();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetAllCustomerGroupTwoddl()
        {
            var result = await _masterClient.GetAllCustomerGroupTwoddl();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetAllCustomerGroupFiveddl()
        {
            var result = await _masterClient.GetAllCustomerGroupFiveddl();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportCustomerGroupOne(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");

                var resultList = _masterClient.ExportCustomerGroupOne(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "CustomerGroupOne_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    //Header
                    worksheet.Cells["A1:BZ1"].Style.Font.Size = 13;
                    worksheet.Cells["A1:BZ1"].Style.Font.Name = "Calibri";
                    worksheet.Cells["A1:BZ1"].Style.Font.Bold = true;

                    worksheet.Cells.LoadFromCollection(resultList, true);
                    worksheet.Cells.AutoFitColumns();

                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportCustomerGroupTwo(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");

                var resultList = _masterClient.ExportCustomerGroupTwo(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "CustomerGroupTwo_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    //Header
                    worksheet.Cells["A1:BZ1"].Style.Font.Size = 13;
                    worksheet.Cells["A1:BZ1"].Style.Font.Name = "Calibri";
                    worksheet.Cells["A1:BZ1"].Style.Font.Bold = true;

                    worksheet.Cells.LoadFromCollection(resultList, true);
                    worksheet.Cells.AutoFitColumns();

                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Kendo Grid Data

        public KendoGridResult GridResultInputDto(DataSourceRequest request, bool isToReturnInactiveData)
        {
            request.Filters = Utility.ToFilterDescriptor(request.Filters);
            return new KendoGridResult() { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData, VerticalId = VerticalId, DataSourceRequest = request };
        }

        #endregion

        #region
        public async Task<ActionResult> GetZonalHeadList([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto();
            loginUserIdDto.IsToReturnInactiveData = isToReturnInactiveData;
            var result = await _masterClient.GetZonalHeadListAsync(loginUserIdDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetZonalHeadListNew([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto();
            loginUserIdDto.IsToReturnInactiveData = isToReturnInactiveData;
            loginUserIdDto.LoginUserId = UserId;
            var result = await _masterClient.GetZonalHeadListAsyncNew(loginUserIdDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetNationalHeadList([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto();
            loginUserIdDto.IsToReturnInactiveData = isToReturnInactiveData;
            loginUserIdDto.VerticalId = VerticalId;
            var result = await _masterClient.GetNationalHeadList(loginUserIdDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetZonalHeadListByNH([DataSourceRequest] DataSourceRequest request, string SelectedIds)
        {
            var result = new List<ZoneDto>();
            if (!string.IsNullOrEmpty(SelectedIds))
            {
                var NationalTrader = SelectedIds.Split(',').Select(long.Parse).ToList();
                if (NationalTrader != null)
                {
                    NationalHeadDto inputDto = new NationalHeadDto();
                    inputDto.NHIds = NationalTrader;
                    inputDto.VerticalId = VerticalId;
                    result = await _masterClient.GetZonalHeadListByNH(inputDto);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetZHBasedOnVertical([DataSourceRequest] DataSourceRequest request, long verticalId,long SalesOrganizationId,long DistributionChannelId, string SelectedIds)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto();
            loginUserIdDto.VerticalId = verticalId;
            loginUserIdDto.SalesOrganizationId = SalesOrganizationId;
            loginUserIdDto.DistributionChannelId = DistributionChannelId;
            var result = await _masterClient.GetZHBasedOnVertical(loginUserIdDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetBDOBasedOnZonalheadIds(string SelectedIds)
        {
            var stateList = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(SelectedIds))
            {
                var ZonalTrader = SelectedIds.Split(',').Select(long.Parse).ToList();
                if (ZonalTrader != null)
                {
                    List<long> zoneI = new List<long>();
                    var result = await _masterClient.GetBDOBasedOnZonalhead(ZonalTrader);
                    if (result != null && result.Any())
                    {
                        foreach (var data in result)
                        {
                            stateList.Add(new DropDownDto() { Id = data.Id, Name = data.Name });
                        }
                    }
                }
            }
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetZonalHeadBasedNH(string SelectedIds)
        {
            var zhList = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(SelectedIds))
            {
                var ZonalTrader = long.Parse(SelectedIds);
                if (ZonalTrader != null)
                {
                    List<long> zoneI = new List<long>();
                    zhList = (List<DropDownDto>)await _masterClient.GetZonalHeadBasedNH(ZonalTrader);
                    if (zhList != null && zhList.Any())
                    {
                        return Json(zhList, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(zhList, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetZonalHeadBasedNHComb(string SelectedIds,long SalesOrganizationId,long DistributionChannelId,long DivisionId)
        {
            var zhList = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(SelectedIds))
            {
                var ZonalTrader = long.Parse(SelectedIds);
                if (ZonalTrader != null)
                {
                    var input = new BookedSaudaInputDto() {
                    LoginUserId=ZonalTrader,
                    SalesOrganizationId=SalesOrganizationId,
                    DistributionChannelId=DistributionChannelId,
                    DivisionId=DivisionId
                    };
                    zhList = (List<DropDownDto>)await _masterClient.GetZonalHeadBasedNHComb(input);
                    if (zhList != null && zhList.Any())
                    {
                        return Json(zhList, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(zhList, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetDealerBasedOnBDOIds(string SelectedIds)
         {
            var dealerList = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(SelectedIds))
            {
                var bdoIds = SelectedIds.Split(',').Select(long.Parse).ToList();
                if (bdoIds != null)
                {
                    var result = await _masterClient.GetDealerBasedOnBdo(bdoIds);
                    if (result != null && result.Any())
                    {
                        foreach (var data in result)
                        {
                            dealerList.Add(new DropDownDto() { Id = data.Id, Name = data.Name });
                        }
                    }
                }
            }
            return Json(dealerList, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetDealerCodeBasedOnBDOIds(string SelectedIds)
        {
            var dealerList = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(SelectedIds))
            {
                var bdoIds = SelectedIds.Split(',').Select(long.Parse).ToList();
                if (bdoIds != null)
                {
                    var result = await _masterClient.GetDealerCodeBasedOnBdo(bdoIds);
                    if (result != null && result.Any())
                    {
                        foreach (var data in result)
                        {
                            dealerList.Add(new DropDownDto() { Code = data.Code, Name = data.Name });
                        }
                    }
                }
            }
            return Json(dealerList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        /// <summary>
        /// Method to get state list
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetStateNameAsync([DataSourceRequest] DataSourceRequest request)
        {
            var stateList = await _lookupClient.GetStateListAsync();
            var result = stateList.Select(s => new DropDownDto()
            {
                Id = s.StateId,
                Name = s.StateName
            }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get state list as per employee
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetStateNameAsPerEmployeesAsync(string EmployeeIds)
        {
            var result = new List<DropDownDto>();
            if (EmployeeIds != null && EmployeeIds != "")
            {
                var employeeIds = EmployeeIds.Split(',').Select(s => long.Parse(s)).ToList();
                var inputDto = new LoginUserIdDto { ZonalHeadIds = employeeIds };
                var stateList = await _lookupClient.GetStateListByEmployeeIdsAsync(inputDto);
                result = stateList.Select(s => new DropDownDto()
                {
                    Id = s.StateId,
                    Name = s.StateName
                }).ToList();
            }
            else
            {
                var stateList = await _lookupClient.GetStateListAsync();
                result = stateList.Select(s => new DropDownDto()
                {
                    Id = s.StateId,
                    Name = s.StateName
                }).ToList();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region VehicleLoadabilities

        public ActionResult VehicleLoadabilitiesList()
        {
            return View();
        }


        public async Task<ActionResult> GetVehicleLoadabilitiesList([DataSourceRequest] DataSourceRequest request)
        {

            var result = await _lookupClient.GetVehicleLoadabilitiesListAsync();
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddorUpdateVehicleLoadabilities(VehicleLoadabilitiesDto inputDto)
        {

            inputDto.LoginUserId = UserId;
            var result = await _lookupClient.AddOrUpdateVehicleLoadabilities(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("VehicleLoadabilitiesList", "Lookup");

            }

            return View("VehicleLoadabilities", result);



        }

        public async Task<ActionResult> VehicleLoadabilities()
        {
            var inputDto = new VehicleLoadabilitiesDto();
            if (Session["vehicleloadabilitiesId"] != null && UtilityHelper.IntTryToParse(Session["vehicleloadabilitiesId"].ToString()) > 0)
            {
                long id = Convert.ToInt32(Session["vehicleloadabilitiesId"].ToString());
                inputDto = await _lookupClient.GetVehicleLoadabilitiesByIdAsync(UserId, id);

            }
            return View(inputDto);
        }

        public ActionResult VehicleLoadabilitiesEditRedirect(string vehicleloadabilitiesId = "")
        {
            Session["vehicleloadabilitiesId"] = vehicleloadabilitiesId;
            return RedirectToAction("VehicleLoadabilities", "Lookup");
        }


        public async Task<ActionResult> ExportVehicleLoadabilities(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<VehicleLoadabilitiesDto> resultList = new List<VehicleLoadabilitiesDto>();
                resultList = await _lookupClient.ExportVehicleLoadabilities(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "VehicleLoadabilities_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Zone"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_State"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_FreightZone"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_VehicleSize"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("msg_Status"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ZoneName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.StateName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.FreightZoneName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.VehicleSize.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IsActiveBool == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                        }
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    //this.Response.Headers.Clear();
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region OilType

        public async Task<ActionResult> GetOilTypeOnVerticalList(long SelectedVerticalId = 0, string SelectedZoneIds = "", string SelectedStateIds = "", string SelectedZonalHeadIds = "", string SelectedBDOIds = "")
        {
            var oilTypeList = new List<DropDownDto>();
            OilTypeMappingDto oilTypeMappingDto = new OilTypeMappingDto();

            List<long> zoneId = null;
            List<long> stateId = null;
            List<long> zonalheadId = null;
            List<long> bdoId = null;


            if (!string.IsNullOrEmpty(SelectedZoneIds) && !string.IsNullOrEmpty(SelectedStateIds) && !string.IsNullOrEmpty(SelectedZonalHeadIds) && !string.IsNullOrEmpty(SelectedBDOIds))
            {
                zoneId = SelectedZoneIds.Split(',').Select(long.Parse).ToList();
                stateId = SelectedStateIds.Split(',').Select(long.Parse).ToList();
                zonalheadId = SelectedZonalHeadIds.Split(',').Select(long.Parse).ToList();
                bdoId = SelectedBDOIds.Split(',').Select(long.Parse).ToList();

            }

            if (zoneId != null && stateId != null)
            {

                oilTypeMappingDto.ZoneIds = zoneId;
                oilTypeMappingDto.StateIds = stateId;
                oilTypeMappingDto.ZHIds = zonalheadId;
                oilTypeMappingDto.BDOIds = bdoId;
                oilTypeMappingDto.VerticalId = SelectedVerticalId;

            }
            else
            {
                oilTypeMappingDto.VerticalId = SelectedVerticalId;
            }

            var result = await _masterClient.GetOilTypeBasedonVerticals(oilTypeMappingDto);
            if (result != null && result.Any())
            {
                foreach (var data in result)
                {
                    oilTypeList.Add(new DropDownDto() { Id = data.OilTypeId, Name = data.OilTypeName });
                }
            }

            return Json(oilTypeList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region TPNotification

        public async Task<ActionResult> GetBdoddl()
        {
            LoginUserIdDto inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
            var bdoList = await _lookupClient.GetBdoddlAsync(inputDto);
            return Json(bdoList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NotERedirect(string EncryptedId = "")
        {
            var Id = "";

            if (!String.IsNullOrEmpty(EncryptedId))
            {
                EncryptedId = EncryptedId.Replace(' ', '+');
                Id = UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey);

            }

            Session["NotificationId"] = Id;
            return RedirectToAction("Notification", "Lookup");
        }
        public async Task<ActionResult> Notification()
        {
            var result = new NotificationsDto();
            if (Session["NotificationId"] != null && UtilityHelper.IntTryToParse(Session["NotificationId"].ToString()) > 0)
            {
                var inputDto = new IdInputDto { Id = UtilityHelper.LongTryToParse(Session["NotificationId"].ToString()) };
                result = await _lookupClient.GetTPNotificationById(inputDto);

                result.EncryptedId = UtilityHelper.ConvertToMd5(result.Id.ToString(), SecurityConstants.EncryptionKey);
            }
            return View(result);
        }

        public JsonResult GetNotificationActionList([DataSourceRequest] DataSourceRequest request)
        {
            var resultList = ((NotificationActionTP[])Enum.GetValues(typeof(NotificationActionTP))).Select(c => new DropDownDto() { Id = (int)c, Name = c.Description() }).ToList();
            resultList = resultList.Where(_ => _.Id != 10 && _.Id != 12 && _.Id != 1 && _.Id != 13 && _.Id != 11).ToList();
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NotificationGridPartial()
        {
            return PartialView("NotificationPopup");
        }

        public async Task<ActionResult> GetDealerListBasedOnBDO([DataSourceRequest] DataSourceRequest request, string bdoIds)
        {

            var inputDto = new NotificationInputDto();
            inputDto.BdoIds = UtilityHelper.ConvertStringToLongList(bdoIds);
            inputDto.DataSourceRequest = request;
            var result = await _masterClient.GetKendoGridDataAsync<NotificationDetailDto>(inputDto, ApiUrl.WebApiUrlGetDealerListBasedOnBDO);
            var dealersList = result.Data as List<NotificationDetailDto>;
            result.Data = dealersList;
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> Notification(NotificationsDto inputDto, string NotificationDetailDtoList)
        {
            if (!string.IsNullOrEmpty(NotificationDetailDtoList))
            {
                inputDto.NotificationDetailDtoList = JsonConvert.DeserializeObject<List<NotificationDetailDto>>(NotificationDetailDtoList);
            }

            if (inputDto != null)
            {
                inputDto.CreatedBy = UserId;
                inputDto.NotificationDetailDtoList = JsonConvert.DeserializeObject<List<NotificationDetailDto>>(NotificationDetailDtoList);

                var result = await _lookupClient.AddOrUpdateNotification(inputDto);
                if (result.PostStatus)
                {
                    TempData["SuccessMessage"] = result.PostMessage;
                    return RedirectToAction("NotificationList", "Lookup");
                }
                return View("Notification", result);
            }
            else { return View("Notification", inputDto); }
        }


        public ActionResult NotificationList()
        {
            return View();
        }
        public async Task<ActionResult> GetTPNotificationListAsync([DataSourceRequest] DataSourceRequest request)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId };
            var result = await _lookupClient.GetTPNotificationListAsync(loginUserIdDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> GetTPNotificationDetailsById([DataSourceRequest] DataSourceRequest request, long tpNotificationId)
        {
            var customerGroupDetails = await _lookupClient.GetTPNotificationDetailsById(tpNotificationId);
            var resultList = customerGroupDetails.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> GetMappedDealerListByTPNotificationId([DataSourceRequest] DataSourceRequest request, long tpNotificationId)
        {
            NotificationGridInputDto inputDto = new NotificationGridInputDto { NotificationId = tpNotificationId, DataSourceRequest = request };
            return Json(await _lookupClient.GetMappedDealerListByTPNotificationId(inputDto));
        }

        [HttpPost]
        public async Task<ActionResult> ExportTPNotificationList(bool IsToReturnInactiveData)
        {
            var finalResult = new JsonResult();
            try
            {
                string fileName = "NotificationList_" + string.Format(Settings.ReportDateFormat, DateTime.Now.Date).ToUpper() + ".xlsx";
                bool isHeaderBind = false;
                var resultList = await _lookupClient.ExportTPNotificationList(new LoginUserIdDto { IsToReturnInactiveData = IsToReturnInactiveData });

                if (resultList != null && resultList.Any())
                {
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        var rowIndex = 5;
                        var colIndex = 1;
                        var childColIndex = 0;

                        #region Header

                        worksheet.Cells["A1:M1"].Merge = true;
                        worksheet.Cells["A1:M1"].Value = Settings.CompanyName;
                        worksheet.Cells["A1:M1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1:M1"].Style.Font.Bold = true;
                        worksheet.Cells["A1:M1"].Style.Font.Size = 16;

                        worksheet.Cells["A2"].Value = "Name";
                        worksheet.Cells["B2"].Value = "Notification Details";

                        for (int i = 2; i <= 3; i++)
                        {
                            worksheet.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheet.Cells["A" + i].Style.Font.Bold = true;
                            worksheet.Cells["A" + i].Style.Font.Size = 12;

                            worksheet.Cells["B" + i + ":" + "F" + i].Merge = true;
                            worksheet.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        #endregion
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "SMS");
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Email");
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "In App Notification");
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_NotificationAction"));

                        foreach (var Notification in resultList)
                        {
                            isHeaderBind = false;
                            rowIndex++;
                            colIndex = 1;

                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], Notification.SMS == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], Notification.IsEmail == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], Notification.InAppNotification == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));

                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], Notification.NotificationActions).ToString();

                            if (Notification.NotificationDetailDtoList != null && Notification.NotificationDetailDtoList.Any())
                            {
                                foreach (var NotificationDetail in Notification.NotificationDetailDtoList)
                                {
                                    if (!isHeaderBind)
                                    {
                                        rowIndex++;
                                        childColIndex = 2;

                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_Dealer"));
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_State"));
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_District"));
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_Active"));

                                        isHeaderBind = true;
                                    }
                                    rowIndex++;
                                    childColIndex = 2;
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], NotificationDetail.CustomerName).ToString();
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], NotificationDetail.State).ToString();
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], NotificationDetail.District).ToString();
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], NotificationDetail.IsActive == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));

                                }
                            }
                        }
                        worksheet.Cells.AutoFitColumns();
                        return SaveExcelFileToPath(package, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Lookup 

        public async Task<JsonResult> GetOilTypeListByVerticalIdsForDropDown(string verticalIds)
        {
            var result = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(verticalIds))
            {
                IdInputDto inputDto = new IdInputDto { IdList = UtilityHelper.ConvertStringToLongList(verticalIds) };
                result = await _lookupClient.GetOilTypeListByVerticalIdsForDropDown(inputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetOilPackingTypeListForDropdown()
        {
            var result = await _lookupClient.GetOilPackingTypeListForDropdown();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetVerticalListForDropdown()
        {
            LoginUserIdDto inputDto = new LoginUserIdDto { LoginUserId = UserId, VerticalId = VerticalId };
            var verticalList = await _lookupClient.GetVerticalListForDropdown(inputDto);
            return Json(verticalList, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetSkuListByOilTypeIdsPackGroupIdsForDropdown(string oilTypeIds = "", string packGroupIds = "", long packTypeId = 0)
        {
            IList<DropDownDto> result = new List<DropDownDto>();

            if (!string.IsNullOrEmpty(oilTypeIds))
            {
                DropDownInputDto inputDto = new DropDownInputDto
                {
                    OilTypeIds = UtilityHelper.ConvertStringToLongList(oilTypeIds),
                    PackGroupIds = UtilityHelper.ConvertStringToLongList(packGroupIds),
                    PackTypeId = packTypeId
                };
                result = await _lookupClient.GetSkuListByOilTypeIdsPackGroupIdsForDropdown(inputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get state list
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetAllStateListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var stateList = new List<DropDownDto>();
            var result = await _lookupClient.GetStateListAsync();
            if (result.IsAny())
            {
                result.ForEach(f => stateList.Add(new DropDownDto() { Id = f.StateId, Name = f.StateName }));
            }
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetAllDepotList()
        {
            LoginUserIdDto inputDto = new LoginUserIdDto { LoginUserId = UserId, VerticalId = VerticalId };
            var verticalList = await _masterClient.GetDepotDetailsAsync(inputDto);
            return Json(verticalList, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region SmsSend

        public ActionResult SmsSend()
        {
            SmsInputDto input = new SmsInputDto();
            return View(input);
        }

        public JsonResult GetRoleListForSendSMS([DataSourceRequest] DataSourceRequest request)
        {
            var resultList = ((Role[])Enum.GetValues(typeof(Role))).Select(c => new DropDownDto() { Id = (int)c, Name = c.Description() }).ToList();
            var RolelistForSms = resultList.Where(_ => _.Id == (int)DTO.Enums.Role.StateTrader || _.Id == (int)DTO.Enums.Role.Dealer || _.Id == (int)DTO.Enums.Role.ZonalTrader).ToList();
            return Json(RolelistForSms, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SendSmsCMS(SmsInputDto smsInputDto)
        {
            var result = await _masterClient.SendSms(smsInputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("SmsSend", "Lookup");
            }
            return View("SmsSend", result);
        }


        [HttpPost]
        public async Task<ActionResult> SendSms(SmsInputDto smsInputDto)
        {
            var result = await _masterClient.SendSms(smsInputDto);
            return Json(new { Status = result.PostStatus, Message = result.PostMessage }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllNotificationTypes()
        {
            var result = _masterClient.GetGeneralNotificationTypes();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNotificationTypes()
        {
            var result = _masterClient.GetNotificationTypes();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region sauda conversion type
        public JsonResult GetPendingApprovedStatusddl()
        {
            var statusIds = new long[] { 1, 2 };
            var result = _masterClient.GetAllStatus().Where(w => statusIds.Contains(w.Id));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        [HttpGet]
        public async Task<ActionResult> SaudaConversionType()
        {
            SaudaConversionTypeViewModel conversionTypeViewModel = new SaudaConversionTypeViewModel();
            conversionTypeViewModel = await _lookupClient.GetSaudaConversionList();
            return View(conversionTypeViewModel);
        }

        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        [HttpPost]
        public async Task<ActionResult> SaudaConversionType(SaudaConversionTypeViewModel conversionTypeViewModel)
        {
            var result = new SaudaConversionTypeViewModel();
            if (conversionTypeViewModel != null)
            {
                result = await _lookupClient.UpdateSaudaConversionType(conversionTypeViewModel);
                if (result.PostStatus)
                {
                    TempData["Message"] = result.PostMessage;
                    RedirectToAction("SaudaConversionType", "Lookup");
                }
            }
            return View(result);
        }

        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public ActionResult SaudaConversionDetail()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }
        public async Task<JsonResult> GetSaudaConversionDetailList([DataSourceRequest] DataSourceRequest request, SaudaConversionHistoryInputDto inputDto)
        {
            var result = new List<SaudaConversionSkusDetail>();
            inputDto.LoginUserId = (long)UserId;
            result = await _lookupClient.GetSaudaConversionDetailList(inputDto);
            var gridList = result.ToDataSourceResult(request);
            gridList.Total = result.Count;
            return Json(gridList);
        }

        /// <summary>
        /// Method to Export SaudaConversion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportSaudaConversion(SaudaConversionHistoryInputDto inputDto)
        {
            var finalResult = new JsonResult();
            DateTime currentDate = DateTime.Now;
            string fileName = "SaudaConversion" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
            var guidFileName = $"{Guid.NewGuid()}.xlsx";
            try
            {
                var resultList = _lookupClient.ExportSaudaConversion(inputDto);
                if (resultList.Any())
                {
                    using (var package = new ExcelPackage())
                    {
                        var ws = package.Workbook.Worksheets.Add("SaudaConversion");

                        ws.Cells["A1:I1"].Merge = true;
                        ws.Cells["A1:I1"].Value = Settings.CompanyName;
                        ws.Cells["A1:I1"].Style.Font.Size = 14;
                        ws.Cells["A1:I1"].Style.Font.Name = "Calibri";
                        ws.Cells["A1:I1"].Style.Font.Bold = true;
                        ws.Cells["A1:I1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        ws.Cells["A2:C2"].Merge = true;
                        ws.Cells["A2:C2"].Value = "Report Name";
                        ws.Cells["A2:C2"].Style.Font.Size = 12;
                        ws.Cells["A2:C2"].Style.Font.Name = "Calibri";
                        ws.Cells["A2:C2"].Style.Font.Bold = true;

                        ws.Cells["D2:I2"].Merge = true;
                        ws.Cells["D2:I2"].Value = "Sauda Conversion";
                        ws.Cells["D2:I2"].Style.Font.Size = 12;
                        ws.Cells["D2:I2"].Style.Font.Name = "Calibri";
                        ws.Cells["D2:I2"].Style.Font.Bold = true;

                        ws.Cells["A4:AZ4"].Style.Font.Size = 12;
                        ws.Cells["A4:AZ4"].Style.Font.Name = "Calibri";
                        ws.Cells["A4:AZ4"].Style.Font.Bold = true;
                        ws.Cells["A4"].LoadFromCollection(resultList, true);
                        ws.Cells.AutoFitColumns();

                        return SaveExcelFileToPath(package, fileName);
                    }
                }
            }
            catch (Exception exception)
            {
            }
            return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaudaConversionDetailViewRedirect(int id = 0)
        {
            Session["ConversionId"] = id;
            return RedirectToAction("SaudaConversionDetailView", "Lookup");
        }
        [HttpGet]
        public async Task<ActionResult> SaudaConversionDetailView()
        {
            SaudaConversionDetailViewModel detail = new SaudaConversionDetailViewModel();
            if (Session["ConversionId"] != null && UtilityHelper.IntTryToParse(Session["ConversionId"].ToString()) > 0)
            {
                long id = Convert.ToInt32(Session["ConversionId"].ToString());
                detail = await _lookupClient.GetSaudaConversionDetailsById(id);
            }
            return View(detail);
        }
        #endregion sauda conversion type

        #region extension policy
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        [HttpGet]
        public ActionResult ExtensionPolicy()
        {
            SaudaExtensionPolicyAddDto saudaExtensionPolicyAddDto = new SaudaExtensionPolicyAddDto();
            saudaExtensionPolicyAddDto.VerticalId = VerticalId;
            return View(saudaExtensionPolicyAddDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddExtensionPolicy(SaudaExtensionPolicyAddDto inputDto)
        {
            inputDto.UserId = UserId;
            SaudaExtensionPolicyAddDto extensionDto = new SaudaExtensionPolicyAddDto();
            var result = await _lookupClient.AddSaudaExtensionPolicy(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                inputDto.PostStatus = true;
                inputDto.PostMessage = result.PostMessage;
                inputDto = new SaudaExtensionPolicyAddDto();
            }
            else
            {
                inputDto.PostStatus = false;
                inputDto.PostMessage = result.PostMessage;
            }
            return RedirectToAction("ExtensionPolicy",inputDto);
        }

        public async Task<ActionResult> GetStatesListBasedOnZonalHeadIds([DataSourceRequest] DataSourceRequest request, string ZonalHeadIds = "")
        {
            List<long> ZonalHeadIdsList = new List<long>();
            if (ZonalHeadIds != "")
            {
                ZonalHeadIdsList = ZonalHeadIds.Split(',').Select(Int64.Parse).ToList();
            }
            IList<DropDownDto> stateList = new List<DropDownDto>();
            if (ZonalHeadIdsList != null && ZonalHeadIdsList.Any())
            {
                stateList = await _lookupClient.GetActiveStateListBasedOnZonalHeadIdsAsync(ZonalHeadIdsList);
            }
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetOilTypeddl([DataSourceRequest] DataSourceRequest request)
        {
            var input = new LoginUserIdDto();
            input.LoginUserId = UserId;
            IList<OilTypeDto> oilTypeList = new List<OilTypeDto>();
            oilTypeList = await _lookupClient.GetActiveOilTypeListAsync(input);
            return Json(oilTypeList, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetSaudaExtensionList([DataSourceRequest] DataSourceRequest request, long verticalId)
        {
            var ExtensionList = await _lookupClient.GetSaudaExtensionListClient(verticalId);
            var resultList = ExtensionList.ToDataSourceResult(request);
            return Json(resultList);
        }


        /// <summary>
        /// Method to Export SaudaExtension policy
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportExtensionPolicy(long verticalId)
        {
            var finalResult = new JsonResult();
            DateTime currentDate = DateTime.Now;
            string fileName = "ExtensionPolicy" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
            var guidFileName = $"{Guid.NewGuid()}.xlsx";
            try
            {
                var resultList = _lookupClient.ExportExtensionPolicy(verticalId);
                if (resultList.Any())
                {
                    using (var package = new ExcelPackage())
                    {
                        var ws = package.Workbook.Worksheets.Add("ExtensionPolicy");

                        ws.Cells["A1:I1"].Merge = true;
                        ws.Cells["A1:I1"].Value = Settings.CompanyName;
                        ws.Cells["A1:I1"].Style.Font.Size = 14;
                        ws.Cells["A1:I1"].Style.Font.Name = "Calibri";
                        ws.Cells["A1:I1"].Style.Font.Bold = true;
                        ws.Cells["A1:I1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        ws.Cells["A2:C2"].Merge = true;
                        ws.Cells["A2:C2"].Value = "Report Name";
                        ws.Cells["A2:C2"].Style.Font.Size = 12;
                        ws.Cells["A2:C2"].Style.Font.Name = "Calibri";
                        ws.Cells["A2:C2"].Style.Font.Bold = true;

                        ws.Cells["D2:I2"].Merge = true;
                        ws.Cells["D2:I2"].Value = "Extension Policy";
                        ws.Cells["D2:I2"].Style.Font.Size = 12;
                        ws.Cells["D2:I2"].Style.Font.Name = "Calibri";
                        ws.Cells["D2:I2"].Style.Font.Bold = true;

                        ws.Cells["A4:AZ4"].Style.Font.Size = 12;
                        ws.Cells["A4:AZ4"].Style.Font.Name = "Calibri";
                        ws.Cells["A4:AZ4"].Style.Font.Bold = true;
                        ws.Cells["A4"].LoadFromCollection(resultList, true);
                        ws.Cells.AutoFitColumns();

                        return SaveExcelFileToPath(package, fileName);
                    }
                }
            }
            catch (Exception exception)
            {
            }
            return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SaudaExtensionDetail()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }
        public ActionResult GetSaudaExtensionDetailList([DataSourceRequest] DataSourceRequest request, SaudaExtensionFilterDtoForGrid inputDto)
        {
            inputDto.DataSourceRequest = request;
            var result = _lookupClient.ExportSaudaExtension(inputDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
            //return Json(await _lookupClient.GetKendoGridDataAsync<SaudaBookedSaudaWithExtensionDetailsListDto>(inputDto, ApiUrl.WebApiUrlListSaudaExtensionDetail));
        }

        /// <summary>
        /// Method to Export SaudaExtensionss
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportSaudaExtension(SaudaExtensionFilterDtoForGrid inputDto)
        {
            var finalResult = new JsonResult();
            DateTime currentDate = DateTime.Now;
            string fileName = "SaudaExtension" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
            var guidFileName = $"{Guid.NewGuid()}.xlsx";
            try
            {
                var resultList = _lookupClient.ExportSaudaExtension(inputDto);
                if (resultList.Any())
                {
                    using (var package = new ExcelPackage())
                    {
                        var ws = package.Workbook.Worksheets.Add("SaudaExtension");

                        ws.Cells["A1:I1"].Merge = true;
                        ws.Cells["A1:I1"].Value = Settings.CompanyName;
                        ws.Cells["A1:I1"].Style.Font.Size = 14;
                        ws.Cells["A1:I1"].Style.Font.Name = "Calibri";
                        ws.Cells["A1:I1"].Style.Font.Bold = true;
                        ws.Cells["A1:I1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        ws.Cells["A2:C2"].Merge = true;
                        ws.Cells["A2:C2"].Value = "Report Name";
                        ws.Cells["A2:C2"].Style.Font.Size = 12;
                        ws.Cells["A2:C2"].Style.Font.Name = "Calibri";
                        ws.Cells["A2:C2"].Style.Font.Bold = true;

                        ws.Cells["D2:I2"].Merge = true;
                        ws.Cells["D2:I2"].Value = "Sauda Extension";
                        ws.Cells["D2:I2"].Style.Font.Size = 12;
                        ws.Cells["D2:I2"].Style.Font.Name = "Calibri";
                        ws.Cells["D2:I2"].Style.Font.Bold = true;

                        ws.Cells["A4:AZ4"].Style.Font.Size = 12;
                        ws.Cells["A4:AZ4"].Style.Font.Name = "Calibri";
                        ws.Cells["A4:AZ4"].Style.Font.Bold = true;
                        ws.Cells["A4"].LoadFromCollection(resultList, true);
                        ws.Cells.AutoFitColumns();

                        return SaveExcelFileToPath(package, fileName);
                    }
                }
            }
            catch (Exception exception)
            {
            }
            return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
        }
        #endregion extension policy

        #region UserInactiveRemarks
        public ActionResult DeleteListCreation()
        {
            return View();
        }
        public JsonResult GetRemarksGroup()
        {
            var result = new List<DropDownDto>();
            result = ((DeleteListCreation[])Enum.GetValues(typeof(DeleteListCreation))).Select(c => new DropDownDto() { Id = (int)c, Name = c.Description() }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetRemarks([DataSourceRequest] DataSourceRequest request, IdInputDto inputDto)
        {
            var result = new List<DeleteListCreateDto>();
            result = await _lookupClient.GetDeleteRemarksList(inputDto);
            var gridData = result.ToDataSourceResult(request);
            return Json(gridData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> AddDeleteListRemarks(AddDeleteListRemarks inputDto)
        {
            inputDto.LoginUserId = UserId;
            var resultList = await _lookupClient.AddDeleteListRemarksAsync(inputDto);
            return Json(resultList);
        }
        public async Task<JsonResult> GetInActiveRemarksById(IdInputDto inputDto)
        {
            var gridData = await _lookupClient.GetDeleteRemarksList(inputDto);
            return Json(gridData, JsonRequestBehavior.AllowGet);
        }
        
        #endregion    
  
        #region Sauda validity and Sauda report email configuration

        public async Task<ActionResult> SaudaValidityAndSaudaReportMailConfiguration()
        {
            var result = new SaudaValidityAndSaudaReportMailConfigurationDto();
            var verticalId = VerticalId;
            result = await _lookupClient.GetVerticalListAndMailIds(verticalId);
            result.VerticalId = VerticalId;
            result.RoleId = RoleId;
            return View(result);
        }

        [HttpPost]

        public async Task<ActionResult> SaudaValidityAndSaudaReportMailConfiguration(long verticalId)
        {
            var result = new SaudaValidityAndSaudaReportMailConfigurationDto();
            result = await _lookupClient.GetVerticalListAndMailIds(verticalId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public async Task<ActionResult> AddorUpdateSaudaValidityAndSaudaReportMailConfiguration(SaudaValidityAndSaudaReportMailConfigurationDto inputDto)
        {
            var result = await _lookupClient.SaudaValidityAndSaudaReportMailConfiguration(inputDto);
            TempData["SuccessMessage"] = result.PostMessage;
            return View("SaudaValidityAndSaudaReportMailConfiguration", result);
        }

        #endregion

        #region Material Type

        public ActionResult MaterialTypeList()
        {
            return View();
        }


        public async Task<ActionResult> GetMaterialTypeList([DataSourceRequest] DataSourceRequest request)
        {

            var result = await _lookupClient.GetMaterialTypeListAsync();
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddorUpdateMaterialType(MaterialTypeDto inputDto)
        {

            inputDto.LoginUserId = UserId;
            var result = await _lookupClient.AddOrUpdateMaterialType(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return Json(result);
            }
            return Json(result);
        }

        public async Task<ActionResult> MaterialType()
        {
            var inputDto = new MaterialTypeDto();
            if (Session["MaterialType"] != null && UtilityHelper.IntTryToParse(Session["MaterialType"].ToString()) > 0)
            {
                long id = Convert.ToInt32(Session["MaterialType"].ToString());
                inputDto = await _lookupClient.GetMaterialTypeByIdAsync(UserId, id);
            }
            return View(inputDto);
        }

        public ActionResult MaterialTypeEditRedirect(string materialTypeId = "")
        {
            Session["MaterialType"] = materialTypeId;
            return RedirectToAction("MaterialType", "Lookup");
        }


        public async Task<ActionResult> ExportMaterialType(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<MaterialTypeDto> resultList = new List<MaterialTypeDto>();
                resultList = await _lookupClient.ExportMaterialType(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "MaterialTypes_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_MaterialType"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SalesOrganization"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DistributionChannel"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Vertical"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("msg_Status"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.MaterialType.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.SalesOrganizationName ?? "");
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.DistributionChannelName ?? "");
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.VerticalName != null ? item.VerticalName.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IsActive == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                        }
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    //this.Response.Headers.Clear();
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Volume Loadability

        public ActionResult VolumeLoadabilityList()
        {
            return View();
        }

        public async Task<ActionResult> GetVolumeLoadabilityList([DataSourceRequest] DataSourceRequest request)
        {

            var result = await _lookupClient.GetVolumeLoadabilityListAsync();
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> VolumeLoadability()
        {
            var inputDto = new VolumeLoadability();
            if (Session["volumeloadability"] != null && UtilityHelper.IntTryToParse(Session["volumeloadability"].ToString()) > 0)
            {
                long id = Convert.ToInt32(Session["volumeloadability"].ToString());
                inputDto = await _lookupClient.GetVolumeLoadabilityByIdAsync(UserId, id);
            }
            return View(inputDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateVolumeLoadability(VolumeLoadability inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _lookupClient.AddOrUpdateVolumeLoadability(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("VolumeLoadabilityList", "Lookup");
            }
            return View("VolumeLoadability", result);
        }

        public ActionResult VolumeLoadabilityEditRedirect(string volumeloadabilityId = "")
        {
            Session["volumeloadability"] = volumeloadabilityId;
            return RedirectToAction("VolumeLoadability", "Lookup");
        }


        public async Task<ActionResult> ExportVolumeLoadability()
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                var inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<VolumeLoadabilityGridDataDto> resultList = new List<VolumeLoadabilityGridDataDto>();
                resultList = await _lookupClient.ExportVolumeLoadability(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "VolumeLoadability_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Plant"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Sku"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SkuCode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_VehicleSize"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_MaxAllowableCases(SingleSku)"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_MaxAllowableCases(MultipleSku)"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ValidFrom"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ValidTo"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("msg_Status"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Plant != null ? item.Plant.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Sku != null ? item.Sku.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.SkuCode != null ? item.SkuCode.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.VehicleSize > 0 ? item.VehicleSize.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.MaxAllowableSingleSku > 0 ? item.MaxAllowableSingleSku.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.MaxAllowableMultipleSku > 0 ? item.MaxAllowableMultipleSku.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ValidFrom != null ? item.ValidFrom.ToString("dd'/'MM'/'yyyy") : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ValidTo != null ? item.ValidTo.ToString("dd'/'MM'/'yyyy") : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IsActive == true ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                        }
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    //this.Response.Headers.Clear();
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region SchemeGeographyReport

        public async Task<ActionResult> GetGeographySchemeBasedOnStateIds(string stateIds, DateTime fromDate, DateTime toDate)
        {
            var stateList = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(stateIds))
            {
                var zoneId = stateIds.Split(',').Select(int.Parse).ToList();
                if (zoneId != null)
                {
                    List<long> zoneI = new List<long>();
                    var result = await _masterClient.GetGeographySchemeBasedOnState(zoneId);
                    if (result != null && result.Any())
                    {
                        foreach (var data in result)
                        {
                            stateList.Add(new DropDownDto() { Id = data.Id, Name = data.Name });
                        }
                    }
                }
            }
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region SaudaAgingReport

        [AuthorizeClaims(Claims.SaudaAgingReport)]
        public ActionResult SaudaAgingReport()
        {
            return View();
        }

        #endregion

        [HttpGet]
        public async Task<ActionResult> GetProfileImageUrl()
        {
            var inputDto = new LoginUserIdDto();
            inputDto.LoginUserId = UserId;
            var result = await _masterClient.GetProfileImageUrl(inputDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region SaudaBookingConfiguration

        public ActionResult EditSaudaBookingRedirect(string EncryptedId)
        {
            if(!string.IsNullOrEmpty(EncryptedId))
            {
                Session["EncryptedId"] = EncryptedId;
            }

            return RedirectToAction("SaudaBookingConfiguration","Lookup");
        }

        public async Task<ActionResult> SaudaBookingConfiguration()
        {
            var data = new SaudaBookingConfigurationDto();

            if (Session["EncryptedId"] != null)
            {
                string encryptedId = Session["EncryptedId"].ToString();
                data = await _lookupClient.GetSaudaBookingConfigurationDetails(encryptedId);
                //data.RoleId = (int)DTO.Enums.Role.Dealer;
                //data.RoleIdForST = (int)DTO.Enums.Role.StateTrader;
                //data.RoleIdForZT = (int)DTO.Enums.Role.ZonalTrader;
            }
            else
            {
                Session["EncryptedId"] = null;
            }

            return View(data);
        }

        [HttpPost]
        public async Task<ActionResult> SaudaBookingConfiguration(SaudaBookingConfigurationDto saudaBookingConfiguration)
        {
            saudaBookingConfiguration.LoginUserId = UserId;
            var result = await _masterClient.SaudaBookingConfiguration(saudaBookingConfiguration);
            //var data = await _lookupClient.GetSaudaBookingConfigurationList();
            //data.PostMessage = result.PostMessage;
            //data.RoleId = (int)DTO.Enums.Role.Dealer;
            //data.RoleIdForST = (int)DTO.Enums.Role.StateTrader;
            //data.RoleIdForZT = (int)DTO.Enums.Role.ZonalTrader;
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                Session["EncryptedId"] = null;
                return RedirectToAction("SaudaBookingConfigurationList", "Sauda");
            }

            TempData["ErrorMessage"] = result.PostMessage;
            return View(result);
        }

        public JsonResult GetRoleListForSaudaBookingConfiguration([DataSourceRequest] DataSourceRequest request)
        {
            var resultList = ((Role[])Enum.GetValues(typeof(Role))).Select(c => new DropDownDto() { Id = (int)c, Name = c.Description() }).ToList();
            var RolelistForSms = resultList.Where(_ => _.Id == (int)DTO.Enums.Role.StateTrader || _.Id == (int)DTO.Enums.Role.Dealer || _.Id == (int)DTO.Enums.Role.ZonalTrader).ToList();
            return Json(RolelistForSms, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region SaudaSalesAreaRestrictionConfiguration

        public async Task<ActionResult> SaudaSalesAreaRestrictionConfiguration()
        {
            var data = new SaudaSalesAreaRestrictionDto();

            if (Session["EncryptedId"] != null)
            {
                string encryptedId = Session["EncryptedId"].ToString();
                data = await _lookupClient.GetSaudaSalesAreaRestrictionConfigurationDetails(encryptedId);
            }
            else
            {
                Session["EncryptedId"] = null;
            }

            return View(data);
        }

        [HttpPost]
        public async Task<ActionResult> SaudaSalesAreaRestrictionConfiguration(SaudaSalesAreaRestrictionDto saudaSalesAreaRestrictionConfiguration)
        {
            saudaSalesAreaRestrictionConfiguration.LoginUserId = UserId;
            var result = await _masterClient.SaudaSalesAreaRestrictionConfiguration(saudaSalesAreaRestrictionConfiguration);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                Session["EncryptedId"] = null;
                return RedirectToAction("SaudaSalesAreaRestrictionList", "Sauda");
            }

            TempData["ErrorMessage"] = result.PostMessage;
            return View(result);
        }

        public ActionResult EditSaudaSalesAreaRestrictionRedirect(string EncryptedId)
        {
            if (!string.IsNullOrEmpty(EncryptedId))
            {
                Session["EncryptedId"] = EncryptedId;
            }

            return RedirectToAction("SaudaSalesAreaRestrictionConfiguration", "Lookup");
        }

        #endregion


        #region Line

        public async Task<ActionResult> Line()
        {
            AddAndUpdateLineDto model = new AddAndUpdateLineDto();

            if(!string.IsNullOrEmpty(Convert.ToString(Session["LineId"])))
            {
                model = await _masterClient.GetLineMappingDetailsById(Session["LineId"].ToString());
            }

            return View(model);
        }

        [OutputCache(Duration = 1000)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult>AddAndUpdateLine(AddAndUpdateLineDto model)
        {
            try
            {
                if (model != null)
                {
                    var result = await _masterClient.AddAndUpdateLineDetails(model);
                    if (!result.PostStatus)
                    {
                        model.PostStatus = false;
                        model.PostMessage = result.PostMessage;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View("LineList",model);
        }

        public ActionResult LineList()
        {
            return View();
        }

        public ActionResult LineRedirect(string EncryptedId = null)
        {
            Session["LineId"] = EncryptedId;
            return RedirectToAction("Line", "Lookup");
        }

        public async Task<JsonResult> GetLineListForddl()
        {
            List<LineddlDto> lineList = new List<LineddlDto>();
            try
            {
                lineList = await _masterClient.GetLineListForddl();
                return Json(lineList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(lineList, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetLineListAsync([DataSourceRequest] DataSourceRequest request)
        {
            List<LineGridDto> lineList = new List<LineGridDto>();
            try
            {
                lineList = await _masterClient.GetLineListAsync();
                return Json(lineList.ToDataSourceResult(request));
            }
            catch (Exception)
            {
                return Json(lineList.ToDataSourceResult(request));
            }
        }
        
        public async Task<ActionResult> ExportLine(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            _methodName = "ExportLine";

            try
            {
                inputDto.IsToReturnInactiveData = true;
                inputDto.LoginUserId = UserId;
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                var resultList = await _masterClient.ExportLine(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = $"Line_{currentDate:dd-MMM-yyyy}-{currentDate:hh:mm tt}.xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Line");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_LineId"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_LineName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IsActive"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            int contentColIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, contentColIndex++], item.LineId.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, contentColIndex++], item.LineName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, contentColIndex++], item.IsActive ? @Helper.GetResourceString("msg_true") : @Helper.GetResourceString("msg_false"));
                        }
                    }

                    AutoFitColumns(package);
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    SaveExcelFile(package,savePath);
                }
            }
            catch (Exception exception)
            {
                var message = $"{controllerName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        private void AutoFitColumns(ExcelPackage package)
        {
            foreach (var workSheet in package.Workbook.Worksheets)
            {
                for (int i = 1; i <= workSheet.Dimension.End.Column; i++)
                {
                    workSheet.Column(i).AutoFit();
                    workSheet.Column(i).BestFit = true;
                }

                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
            }
        }

        private void SaveExcelFile(ExcelPackage package, string savePath)
        {
            if (System.IO.File.Exists(savePath))
                System.IO.File.Delete(savePath);

            using (Stream stream = System.IO.File.Create(savePath))
            {
                package.SaveAs(stream);
            }
        }

        #endregion

        #region GamificationDashboard    

        public ActionResult GamificationDashboard()
        {
            return View();
        }
        public async Task<ActionResult> GamificationDashboardAddOrUpdate()
        {
            var inputDto = new GamificationDashboardDto();
            if (!String.IsNullOrEmpty(Session["GamificationDashboard"].ToString()))
            {
                string id = Session["GamificationDashboard"].ToString();
                inputDto = await _lookupClient.GetGamificationDashboardListAsync(id);
            }
            return View(inputDto);
        }

        public ActionResult GamificationDashboardRedirect(string EncryptedId = "")
        {
            Session["GamificationDashboard"] = EncryptedId;
            return RedirectToAction("GamificationDashboardAddOrUpdate", "Lookup");
        }

        public async Task<ActionResult> GetGamificationDashboardAsync([DataSourceRequest] DataSourceRequest request)
        {
            return Json(await _lookupClient.GetKendoGridDataAsync<GamificationDashboardDto>(GridResultInputDto(request, true), ApiUrl.WebApiUrlGetGamificationDashboardWithPagination));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<JsonResult> AddOrUpdateGamificationDashboardDetails(GamificationDashboardDto gamificationDashboardDto)
        {
            gamificationDashboardDto.LoginUserId = UserId;
            var result = await _lookupClient.AddOrUpdateGamificationDashboardDetails(gamificationDashboardDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region QA_Section
        public ActionResult SectionList()
        {
            return View();
        }

        public ActionResult DynamicFormEditRedirect(string formId = "")
        {
            Session["DynamicFormId"] = formId;
            return RedirectToAction("DynamicForm", "Lookup");
        }
        public ActionResult ViewSubmittedFormDetailsRedirect(long? submittedFormId)
        {
            Session["SubmittedFormId"] = submittedFormId;
            return RedirectToAction("ViewSubmittedFormDetails", "Lookup");
        }

        public async Task<ActionResult> ViewSubmittedFormDetails()
        {
            var result = new SubmittedFormViewDto();
            if (Session["SubmittedFormId"] != null && UtilityHelper.LongTryToParse(Session["SubmittedFormId"].ToString()) > 0)
            {
                result = await _lookupClient.GetSubmittedFormDetailsByIdAsync(UtilityHelper.LongTryToParse(Session["SubmittedFormId"].ToString()));
                if (result.ParentFormId == null || result.ParentFormId == 0)
                {
                    result.SubmittedFormTabs.Add(new FormTab()
                    {
                        Header = Settings.ComplaintForm,
                        IsSelected = true,
                        FormId = result.SubmittedFormId,
                        LoadUrl = "/Lookup/TabStripFormPartialView?submittedFormId=" + UtilityHelper.LongTryToParse(Session["SubmittedFormId"].ToString())
                    });
                    var i = 1;
                    foreach (var item in result.DependentFormDetails)
                    {
                        result.SubmittedFormTabs.Add(new FormTab()
                        {
                            Header = Settings.UnderstandingForm + i,
                            IsSelected = false,
                            FormId = item.Id,
                            LoadUrl = "/Lookup/TabStripFormPartialView?submittedFormId=" + item.Id
                        });
                        i++;
                    }
                }
                else
                {
                    result.SubmittedFormTabs.Add(new FormTab()
                    {
                        Header = "Understanding Form",
                        IsSelected = true,
                        FormId = result.SubmittedFormId,
                        LoadUrl = "/Lookup/TabStripFormPartialView?submittedFormId=" + result.SubmittedFormId
                    });
                }
            }
            return View(result);
        }

        public async Task<JsonResult> GetMappedUnMappedFormQuestions([DataSourceRequest] DataSourceRequest request, List<QuestionsViewDto> mappedQuestionList, bool mappedQuestions = false, bool isEdit = false, bool isQuestionDeleted = false)
        {
            //var sectionInputDto = new SectionIdDto();
            //sectionInputDto.SectionId = sectionId;
            if ((/*!isEdit &&*/ !mappedQuestions && ((List<QuestionsViewDto>)Session["MappedQuestionsList"] == null || isQuestionDeleted)) /*|| 
                (isEdit && !mappedQuestions && ((List<QuestionsViewDto>)Session["MappedQuestionsList"] == null || isQuestionDeleted))*/)
            {
                var questionResult = await _lookupClient.GetSectionFormQuestionList();
                var gridData = questionResult.ToDataSourceResult(request);
                return Json(gridData, JsonRequestBehavior.AllowGet);
            }
            var questionViewModelList = new List<QuestionsViewDto>();
            var allSectionsQuestionsViewDto = new List<SectionQuestionsViewDto>();
            //if (sectionId != 0)
            //{
            //Mapped questions of section
            if (mappedQuestionList != null)
            {
                Session["MappedQuestionsList"] = mappedQuestionList;
            }

            if (mappedQuestions)
            {
                foreach (var question in ((IList<QuestionsViewDto>)Session["MappedQuestionsList"]))
                {
                    questionViewModelList.Add(question);
                }
            }
            else
            {
                //All questions under section
                var questionResult = await _lookupClient.GetSectionFormQuestionList();
                if (questionResult != null && (List<QuestionsViewDto>)Session["MappedQuestionsList"] != null)
                {
                    //Check if it is not present in Mapped questions                    
                    var unMappedQuestions = questionResult.Where(_ => !((List<QuestionsViewDto>)Session["MappedQuestionsList"]).Select(map => map.QuestionId).Contains(_.QuestionId)).ToList();
                    foreach (var question in unMappedQuestions)
                    {
                        questionViewModelList.Add(question);
                    }
                }
                //}
            }
            var gridOutputList = questionViewModelList.OrderBy(x => x.QuestionId).ToDataSourceResult(request);
            return Json(gridOutputList, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetMappedUnMappedQuestions([DataSourceRequest] DataSourceRequest request, List<QuestionsViewDto> mappedQuestionList, bool mappedQuestions = false, bool isEdit = false, bool isQuestionDeleted = false)
        {
            //var sectionInputDto = new SectionIdDto();
            //sectionInputDto.SectionId = sectionId;
            if ((/*!isEdit &&*/ !mappedQuestions && ((List<QuestionsViewDto>)Session["MappedQuestionsList"] == null || isQuestionDeleted)) /*|| 
                (isEdit && !mappedQuestions && ((List<QuestionsViewDto>)Session["MappedQuestionsList"] == null || isQuestionDeleted))*/)
            {
                var questionResult = await _lookupClient.GetSectionQuestionList();
                var gridData = questionResult.ToDataSourceResult(request);
                return Json(gridData, JsonRequestBehavior.AllowGet);
            }
            var questionViewModelList = new List<QuestionsViewDto>();
            var allSectionsQuestionsViewDto = new List<SectionQuestionsViewDto>();
            //if (sectionId != 0)
            //{
                //Mapped questions of section
                if (mappedQuestionList != null)
                {
                    Session["MappedQuestionsList"] = mappedQuestionList;
                }

                if (mappedQuestions)
                {
                    foreach (var question in ((IList<QuestionsViewDto>)Session["MappedQuestionsList"]))
                    {
                        questionViewModelList.Add(question);
                    }
                }
                else
                {
                    //All questions under section
                    var questionResult = await _lookupClient.GetSectionQuestionList();
                    if (questionResult != null && (List<QuestionsViewDto>)Session["MappedQuestionsList"] != null)
                    {
                        //Check if it is not present in Mapped questions                    
                        var unMappedQuestions = questionResult.Where(_ => !((List<QuestionsViewDto>)Session["MappedQuestionsList"]).Select(map => map.QuestionId).Contains(_.QuestionId)).ToList();
                        foreach (var question in unMappedQuestions)
                        {
                            questionViewModelList.Add(question);
                        }
                    }
                //}
            }
            var gridOutputList = questionViewModelList.OrderBy(x => x.QuestionId).ToDataSourceResult(request);
            return Json(gridOutputList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSectionMappedQuestions([DataSourceRequest] DataSourceRequest request)
        {
            IList<QuestionsViewDto> result = new List<QuestionsViewDto>();

            if (Session["SectionQuestionGridData"] != null)
            {
                var sectionQuestionsList = (List<SectionQuestionsViewDto>)Session["SectionQuestionGridData"];

                foreach (var section in sectionQuestionsList)
                {
                    //if (section.SectionId == SectionId)
                    //{
                        result = section.Questions.OrderBy(_ => _.OrderId).ToList();
                    //}
                }
            }

            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        /// <summary>
        /// Method to read section empty list
        /// </summary>
        /// <returns></returns>
        public ActionResult SectionEmptyListAsync([DataSourceRequest] DataSourceRequest request, long FormId)
        {
            var result = new List<SectionQuestionsViewDto>();
            if (Session["SectionQuestionGridData"] != null /*&& Session["DynamicFormId"] != null*//* && UtilityHelper.LongTryToParse(Session["DynamicFormId"].ToString()) == FormId*/)
            {
                result = (List<SectionQuestionsViewDto>)Session["SectionQuestionGridData"];
            }
            else
            {
                Session["DynamicFormId"] = null;
                Session["SectionQuestionGridData"] = null;
                Session["MappedQuestionsList"] = null;
            }
            var resultList = result.ToDataSourceResult(request);
            resultList.Total = result.Count;
            return Json(resultList);
        }

        public JsonResult DeleteQuestions(long QuestionId)
        {
            if (Session["SectionQuestionGridData"] != null)
            {
                var storedQuestions = (List<SectionQuestionsViewDto>)Session["SectionQuestionGridData"];
                var deleteItem = storedQuestions?.FirstOrDefault(_ => _.QuestionId == QuestionId);
                if(deleteItem != null)
                {
                    storedQuestions.Remove(deleteItem);
                    Session["SectionQuestionGridData"] = storedQuestions;
                }
                return Json(new { success = true, questions = storedQuestions }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, message = "No questions found in session." }, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> DynamicForm()
        {
            var result = new DynamicFormQuestionDetailsViewModel();
            if (Session["DynamicFormId"] != null && UtilityHelper.LongTryToParse(Session["DynamicFormId"].ToString()) > 0)
            {
                Session["SectionQuestionGridData"] = null;
                Session["MappedQuestionsList"] = null;

                result = await _masterClient.GetDynamicFormDetailsAsync(UtilityHelper.LongTryToParse(Session["DynamicFormId"].ToString()));

                //Take section id, section name, selected question string - keep as json in session., then return session value in empty datasource method
                if ((result.Questions) != null && (result.Questions).Any())
                {
                    var sectionQuestionsList = new List<SectionQuestionsViewDto>();
                    foreach (var section in result.Questions)
                    {
                        SectionQuestionsViewDto sectionQuestionsViewDto = new SectionQuestionsViewDto();
                        var tempQuestions = new List<long>();
                        tempQuestions.AddRange(section.Questions.Select(_ => _.QuestionId).ToList());
                        tempQuestions = tempQuestions.Distinct().ToList();
                        sectionQuestionsViewDto.QuestionId = section.QuestionId;
                        sectionQuestionsViewDto.Query = section.Query;
                        sectionQuestionsViewDto.QuestionTypeName = section.QuestionTypeName;
                        sectionQuestionsViewDto.Questions = section.Questions;

                        sectionQuestionsList.Add(sectionQuestionsViewDto);
                    }
                    if (sectionQuestionsList != null && sectionQuestionsList.Count > 0)
                    {
                        Session["SectionQuestionGridData"] = sectionQuestionsList;
                    }
                }
                result.IsEdit = true;
            }
            else
            {
                Session["DynamicFormId"] = null;
                Session["SectionQuestionGridData"] = null;
                Session["MappedQuestionsList"] = null;
            }
            return View(result);
        }
        //[AuthorizeClaims(Claims.ManageForms, Claims.ViewForms)]
        public ActionResult DynamicFormList()
        {
            return View();
        }

        public async Task<ActionResult> GetMasterDynamicListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _masterClient.GetMasterDynamicListAsync();
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        [HttpPost]
        public async Task<JsonResult> DynamicForm(DynamicFormQuestionDetailsViewModel formDetailsViewModel)
        {
            ModelState.Clear();
            formDetailsViewModel.LoginUserId = UserId;
            formDetailsViewModel = Helper.SanitizeModel<DynamicFormQuestionDetailsViewModel>(formDetailsViewModel);
            formDetailsViewModel.SectionQuestionsList = (List<SectionQuestionsViewDto>)Session["SectionQuestionGridData"];

            var result = await _lookupClient.SaveDynamicFormDetailsAsync(formDetailsViewModel);

            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                if (result.CreateAnother)
                {
                    return Json(new { PostStatus = true, PostMessage = result.PostMessage, redirectUrl = Url.Action("DynamicFormEditRedirect", "Lookup") });
                }
                else
                {
                    return Json(new { PostStatus = true, PostMessage = result.PostMessage, redirectUrl = Url.Action("DynamicFormList", "Lookup") });
                }
            }
            return Json(new { PostStatus = false, PostMessage = result.PostMessage });
        }

        public JsonResult SaveSectionMappedQuestions(List<SectionQuestionsViewDto> selectedQuestionsViewModel)
        {
            List<SectionQuestionsViewDto> SesseionQuestionsViewDto = new List<SectionQuestionsViewDto>();

            if (Session["SectionQuestionGridData"] != null)
            {
                if (selectedQuestionsViewModel != null && selectedQuestionsViewModel.Count > 0)
                {
                    var sectionQuestionsList = (List<SectionQuestionsViewDto>)Session["SectionQuestionGridData"];
                    if (sectionQuestionsList != null)
                    {
                        
                        SesseionQuestionsViewDto.AddRange(sectionQuestionsList);
                        SesseionQuestionsViewDto.AddRange(selectedQuestionsViewModel);
                    }
                    else
                    {
                        SesseionQuestionsViewDto.AddRange(selectedQuestionsViewModel);
                    }
                    Session["SectionQuestionGridData"] = SesseionQuestionsViewDto.OrderBy(_ => _.QuestionId).ToList();
                }
            }
            else
            {
                if (selectedQuestionsViewModel != null && selectedQuestionsViewModel.Count > 0)
                {
                    SesseionQuestionsViewDto.AddRange(selectedQuestionsViewModel);

                    if (SesseionQuestionsViewDto != null && SesseionQuestionsViewDto.Count > 0)
                    {
                        Session["SectionQuestionGridData"] = SesseionQuestionsViewDto.OrderBy(_ => _.QuestionId).ToList();
                    }
                }
            }
            return Json(new { PostStatus = true });
        }

        public async Task<JsonResult> GetCMSSectionListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _lookupClient.GetSectionList();
            var griddata = result.ToDataSourceResult(request);
            return Json(griddata, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SectionEditRedirect(string SectionId)
        {
            Session["SectionId"] = SectionId;
            return RedirectToAction("CreateSection", "Lookup");
        }

        public async Task<ActionResult> CreateSection()
        {
            var result = new SectionModel();
            if (Session["SectionId"] != null && UtilityHelper.IntTryToParse(Session["SectionId"].ToString()) > 0)
            {
                result = await _lookupClient.GetSectionDetailsById(UtilityHelper.LongTryToParse(Session["SectionId"].ToString()));
                Session["SectionId"] = null;
            }
            return View(result);
        }
        public async Task<ActionResult> GetQuestionTypeddl([DataSourceRequest] DataSourceRequest request)
        {
            IList<QuestionTypeDto> questionTypeList = new List<QuestionTypeDto>();
            questionTypeList = await _lookupClient.GetActiveQuestionTypeAsync();
            return Json(questionTypeList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GetSubmittedDetails([DataSourceRequest] DataSourceRequest request, DynamicFormReportFilterInputDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            if (inputDto.roleIds != null && inputDto.roleIds.Any() && inputDto.roleIds.Contains(0))
            {
                inputDto.roleIds.Remove(0);
            }
            var result = await _lookupClient.GetSubmittedDetailsAsync(inputDto);
            var griddata = result.ToDataSourceResult(request);
            return Json(griddata, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GetSubmittedFormDetailsbyId([DataSourceRequest] DataSourceRequest request, long FormId)
        {
            var result = await _lookupClient.GetSubmittedFormDetailsbyId(FormId); 
            var griddata = result.ToDataSourceResult(request);
            return Json(griddata, JsonRequestBehavior.AllowGet);    
        }



        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> AddOrUpdateSection(SectionModel inputDto)
        {
            inputDto.LoginUserId = UserId;
            //inputDto.IsUser = true;            
            var result = await _lookupClient.AddOrUpdateCMSSection(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("SectionList", "Lookup");
            }
            return View("CreateSection", result);
        }

        public ActionResult QuestionEditRedirect(string QuestionId)
        {
            Session["QuestionId"] = QuestionId;
            return RedirectToAction("CreateQuestion", "Lookup");
        }

        public async Task<ActionResult> CreateQuestion()
        {
            var result = new CMSQuestionModel();
            if (Session["QuestionId"] != null && UtilityHelper.IntTryToParse(Session["QuestionId"].ToString()) > 0)
            {
                result = await _lookupClient.GetQuestionDetailsById(UtilityHelper.LongTryToParse(Session["QuestionId"].ToString()));
                result.IsEdit = true;
                if (result.AnswerOptions != null && result.AnswerOptions.Any())
                    result.Option = result.AnswerOptions[0].Option;
                Session["QuestionId"] = null;
            }
            return View(result);
        }
        public ActionResult QuestionList()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> AddOrUpdateQuestion(CMSQuestionModel inputDto)
        {
            System.Diagnostics.Debug.WriteLine($"QuestionTypeId: {inputDto.QuestionTypeId}");
            inputDto.LoginUserId = UserId;

            var result = await _lookupClient.AddOrUpdateCMSQuestion(inputDto);

            if (result.PostStatus)
            {
                if (!result.CreateAnother)
                {
                    TempData["SuccessMessage"] = result.PostMessage;
                    return RedirectToAction("QuestionList", "Lookup");
                }
                else
                {
                    return RedirectToAction("CreateQuestion", "Lookup");
                }
            }
            return View("CreateQuestion", result);
        }
        public async Task<JsonResult> GetCMSQuestionListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _lookupClient.GetSectionQuestionList();
            var gridData = result.ToDataSourceResult(request);
            return Json(gridData, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region View Submitted Forms
        public ActionResult ViewSubmittedForms()
        {
            var result = new SubmittedFormsInputDto();
            return View(result);
        }

        /// <summary>
        /// Method to get all completed Submitted Forms list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetAllSubmittedFormsListForGridAsync([DataSourceRequest] DataSourceRequest request, SubmittedFormsInputDto submittedFormsInputDto)
        {
            IList<SubmittedFormShortViewDto> result = new List<SubmittedFormShortViewDto>();
            SubmittedFormsListViewDto submittedFormsListViewDto = new SubmittedFormsListViewDto();
            if (submittedFormsInputDto.FromDate.Year != 1 && submittedFormsInputDto.ToDate.Year != 1)
            {
                if (request.Filters != null && request.Filters.Any())
                {
                    for (int i = 0; i < request.Filters.Count; i++)
                    {
                        FilterDescriptor filter = request.Filters[i] as FilterDescriptor;
                        if (filter != null && filter.ConvertedValue is DateTime && filter.Operator == FilterOperator.IsEqualTo)
                        {
                            DateTime convertedDateTime = (DateTime)filter.ConvertedValue;
                            CompositeFilterDescriptor newFilter = new CompositeFilterDescriptor
                            {
                                LogicalOperator = FilterCompositionLogicalOperator.And
                            };
                            DateTime lowerBound;
                            DateTime upperBound;

                            lowerBound = convertedDateTime;
                            upperBound = convertedDateTime.AddDays(1).AddSeconds(-1);

                            newFilter.FilterDescriptors.Add(new FilterDescriptor
                            {
                                Member = filter.Member,
                                MemberType = filter.MemberType,
                                Operator = FilterOperator.IsGreaterThanOrEqualTo,
                                Value = lowerBound
                            });

                            newFilter.FilterDescriptors.Add(new FilterDescriptor
                            {
                                Member = filter.Member,
                                MemberType = filter.MemberType,
                                Operator = FilterOperator.IsLessThan,
                                Value = upperBound
                            });

                            request.Filters[i] = newFilter;
                        }
                    }
                    result = Session["SubmittedFormsList"] != null ? (List<SubmittedFormShortViewDto>)Session["SubmittedFormsList"] : result;
                }
                else
                {
                    submittedFormsInputDto.LoginUserId = UserId;
                    submittedFormsListViewDto = await _lookupClient.GetAllSubmittedFormsListForGridAsync(submittedFormsInputDto);
                    if (submittedFormsListViewDto.PostStatus)
                    {
                        result = submittedFormsListViewDto.SubmittedFormsShortView;
                        if (result == null)
                            return Json(result);
                        if (result != null && result.Any())
                        {
                            result.ForEach(form =>
                            {
                                form.CreatedDate = ConvertUTCToIndiaTime(form.CreatedDate);
                            });
                        }
                        Session["SubmittedFormsList"] = result;
                    }
                    else
                    {
                        ModelState.AddModelError("", submittedFormsListViewDto.PostMessage);
                        return Json(result.AsQueryable().ToDataSourceResult(request, ModelState));
                    }
                }
            }

            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }
        #endregion

        public async Task<ActionResult> ExportFromSubmitList(DynamicFormReportFilterInputDto inputDto)
        {
            inputDto.LoginUserId = UserId;

            try
            {
                var fileName = "SubmittedFormDetails_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
                bool isHeaderBind = false;
                var resultList = await _lookupClient.ExportSubmittedFormDetails(inputDto);

                if (resultList != null && resultList.Any())
                {
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Submitted Forms");
                        var rowIndex = 2;
                        var colIndex = 1;
                        var childColIndex = 0;

                        #region Header

                        worksheet.Cells["A1:D1"].Merge = true;
                        worksheet.Cells["A1:D1"].Value = "Submitted Form Report";
                        worksheet.Cells["A1:D1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1:D1"].Style.Font.Bold = true;
                        worksheet.Cells["A1:D1"].Style.Font.Size = 16;

                        worksheet.Cells["A2"].Style.Font.Bold = true;
                        worksheet.Cells["A2"].Style.Font.Size = 12;
                        worksheet.Cells["A2"].Value = "Form Name";
                        worksheet.Cells["B2"].Style.Font.Bold = true;
                        worksheet.Cells["B2"].Style.Font.Size = 12;
                        worksheet.Cells["B2"].Value = "Customer Name";
                        worksheet.Cells["C2"].Style.Font.Bold = true;
                        worksheet.Cells["C2"].Style.Font.Size = 12;
                        worksheet.Cells["C2"].Value = "User Role Type";
                        worksheet.Cells["D2"].Style.Font.Bold = true;
                        worksheet.Cells["D2"].Style.Font.Size = 12;
                        worksheet.Cells["D2"].Value = "Submitted Date";

                        //worksheet.Cells["E2"].Value = "Question";
                        //worksheet.Cells["F2"].Value = "Question Type";
                        //worksheet.Cells["G2"].Value = "Answer";

                        #endregion

                        foreach (var item in resultList)
                        {
                            colIndex = 1; 
                            rowIndex++;

                            worksheet.Cells[rowIndex, colIndex++].Value = item.FormName;
                            worksheet.Cells[rowIndex, colIndex++].Value = item.CustomerName;
                            worksheet.Cells[rowIndex, colIndex++].Value = item.UserRoleType;
                            worksheet.Cells[rowIndex, colIndex++].Value = item.CreatedDate.ToString("dd-MM-yyyy");

                            if (item.QuestionAnswer != null && item.QuestionAnswer.Any())
                            {
                                foreach (var question in item.QuestionAnswer)
                                {
                                    if (!isHeaderBind)
                                    {
                                        rowIndex++;
                                        childColIndex = 2; 

                                        worksheet.Cells[rowIndex, childColIndex++].Value = "Question";
                                        worksheet.Cells[rowIndex, childColIndex++].Value = "Question Type";
                                        worksheet.Cells[rowIndex, childColIndex++].Value = "Answer";

                                        worksheet.Cells[rowIndex, 2, rowIndex, 4].Style.Font.Bold = true;
                                        worksheet.Cells[rowIndex, 2, rowIndex, 4].Style.Font.Size = 12;

                                        isHeaderBind = true;
                                    }
                                    rowIndex++;
                                    childColIndex = 2;

                                    worksheet.Cells[rowIndex, childColIndex++].Value = question.Query;
                                    worksheet.Cells[rowIndex, childColIndex++].Value = question.QuestionTypeName;
                                    worksheet.Cells[rowIndex, childColIndex++].Value = question.Answer;
                                }
                                rowIndex++;
                            }
                            else
                            {
                                rowIndex++;
                            }
                        }

                        worksheet.Cells.AutoFitColumns();
                        return SaveExcelFileToPath(package, fileName);
                    }
                }
                else
                {
                    return Json(new { FileGuid = "", FileName = "", ErrorMessage = "No records found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { FileGuid = "", FileName = "", ErrorMessage = "File generation failed" }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Pack Type 

        /// <summary>
        /// Get pack type dropdown
        /// </summary>
        /// <returns></returns> 
        public async Task<ActionResult> GetPackTypeddl()
        {
            var result = await _lookupClient.GetPackTypeddl();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [AuthorizeClaims(Claims.ManageSauda)]
        public ActionResult SaudaModificationList()
        {
            var verticalIdInModel = new SaudaUpdateDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId,
            };
            return View(verticalIdInModel);
        }

        public async Task<ActionResult> GetSaudaModificationList([DataSourceRequest] DataSourceRequest request, DateTime fromDate, DateTime todate, int statusId, int dataFilter, long divisionId, long salesOrganizationId, long DistributionChannelId, long OilTypeId, long SkuId, long ZoneId, long StateId, long DistrictId, long CityId)
        {
            var saudaFilterDto = new SaudaListFilterDto() { LoginUserId = UserId, RoleId = RoleId, FromDate = fromDate, ToDate = todate, StatusId = statusId, DataFilter = dataFilter, SalesOrganizationId = salesOrganizationId, DistributionChannelId = DistributionChannelId, DivisionId = divisionId, DataSourceRequest = request, OilTypeId = OilTypeId, SkuId = SkuId, ZoneId = ZoneId, StateId = StateId, DistrictId = DistrictId, CityId = CityId };
            var result = await _lookupClient.GetSaudaModificationListAsync(saudaFilterDto);
            if (result != null && result.Data != null)
            {
                var saudaList = result.Data as List<SaudaListDto>;
                if (saudaList != null && saudaList.Any())
                {
                    saudaList.ForEach(f =>
                    {
                        f.BiddingDate = ConvertUTCToIndiaTime(f.BiddingDate);
                        f.DataFilter = dataFilter;
                    });
                    result.Data = saudaList;
                }
            }
            return Json(result);
        }

        [NoCache]
        public async Task<JsonResult> SaudaModificationsDetailsById([DataSourceRequest] DataSourceRequest request, int SaudaId)
        {
            List<SaudaModificationNewItemDto> saudamodificationdetails = new List<SaudaModificationNewItemDto>();
            if (SaudaId > 0)
            {
                IdInputDto idInputDto = new IdInputDto { Id = SaudaId };
                SaudaModificationsListsDto result = await _lookupClient.SaudaModificationsDetailsById(idInputDto);

                if (result.SaudaModificationNewItemsList != null)
                {
                    saudamodificationdetails = result.SaudaModificationNewItemsList;
                    var resultList = saudamodificationdetails.ToDataSourceResult(request);
                    resultList.Total = saudamodificationdetails.Count;
                    return Json(resultList);
                }
            }
            return Json(saudamodificationdetails);
        }

        public ActionResult SaudhaModificationDetailsView(string EncryptedId = "")
        {
            var saudaNo = "";
            if (!String.IsNullOrEmpty(EncryptedId))
            {
                EncryptedId = EncryptedId.Replace(' ', '+');
                saudaNo = UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey);

            }
            Session["saudaModificationId"] = saudaNo;
            return RedirectToAction("SaudhaModificationDetails", "Lookup");
        }

        public async Task<ActionResult> SaudhaModificationDetails()
        {
            var result = new SaudaListDto();
            if (Session["saudaModificationId"] != null && UtilityHelper.IntTryToParse(Session["saudaModificationId"].ToString()) > 0)
            {
                var saudaModificationDto = new SaudaDetailInputDto { SaudaId = UtilityHelper.IntTryToParse(Session["saudaModificationId"].ToString()), UserId = this.UserId };
                result = await _lookupClient.GetSaudhaModificationDetails(saudaModificationDto);
            }
            result.EncryptedId = UtilityHelper.ConvertToMd5(result.SaudaId.ToString(), SecurityConstants.EncryptionKey);

            result.RoleId = RoleId;
            return View(result);
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<JsonResult> UpdateSaudhaModificationStatus(SaudaUpdateDto saudaUpdateDto)
        {
            saudaUpdateDto.ModifiedBy = UserId;
            saudaUpdateDto.LoginUserId = UserId;

            if (saudaUpdateDto.EncryptedIds.Count() > 0)
            {
                foreach (var id in saudaUpdateDto.EncryptedIds)
                {
                    var Id = id.Replace(' ', '+');
                    var decryptedId = UtilityHelper.ConvertMd5ToString(Id, SecurityConstants.EncryptionKey);
                    saudaUpdateDto.SaudaModificationIds.Add(UtilityHelper.IntTryToParse(decryptedId));
                }
            }

            saudaUpdateDto = await _lookupClient.UpdateSaudhaModificationStatus(saudaUpdateDto);
            return Json(saudaUpdateDto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<JsonResult> UpdateSaudhaModificationStatusForLoose(SaudaUpdateDto saudaUpdateDto)
        {
            saudaUpdateDto.ModifiedBy = UserId;
            saudaUpdateDto.LoginUserId = UserId;

            if (saudaUpdateDto.EncryptedIds.Count() > 0)
            {
                foreach (var id in saudaUpdateDto.EncryptedIds)
                {
                    var Id = id.Replace(' ', '+');
                    var decryptedId = UtilityHelper.ConvertMd5ToString(Id, SecurityConstants.EncryptionKey);
                    saudaUpdateDto.SaudaModificationIds.Add(UtilityHelper.IntTryToParse(decryptedId));
                }
            }

            saudaUpdateDto = await _lookupClient.UpdateSaudhaModificationStatusForLoose(saudaUpdateDto);
            return Json(saudaUpdateDto, JsonRequestBehavior.AllowGet);
        }
    }
}
