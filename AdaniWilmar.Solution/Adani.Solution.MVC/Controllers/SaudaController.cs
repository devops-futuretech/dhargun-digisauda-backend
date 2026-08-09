using Adani.Solution.DTO;
using Adani.Solution.DTO.Enums;
using Adani.Solution.MVC.Attributes;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.Models;
using Adani.Solution.MVC.ServiceClient;
using GMCore.Helper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Hosting;
using Adani.Solution.MVC.Common;
using Adani.Solution.DTO.Common;

namespace Adani.Solution.MVC.Controllers
{
    [TokenAuthorize]
    [CustomRedirect]
    [NoCache]
    public class SaudaController : BaseController
    {
        private readonly SaudaClient _saudaClient;

        public SaudaController()
        {
            _saudaClient = new SaudaClient { ControllerDelegate = this };
        }

        #region SaudaLimit
        /// <summary>
        /// Method to Get Sauda limit
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageSaudaLimit)]
        public ActionResult SaudaLimit()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }

        /// <summary>
        /// Method to Get Sauda limit List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetSaudaLimitListAsync([DataSourceRequest] DataSourceRequest request, SaudaLimitInputDto saudaLimitInputDto)
        {
            var result = new DataSourceResult();
            //var result = new List<SaudaLimitRequestHistoryDto>();
            if (saudaLimitInputDto != null && saudaLimitInputDto.FromDate != null && saudaLimitInputDto.ToDate != null
                && saudaLimitInputDto.FromDate != DateTime.MinValue && saudaLimitInputDto.ToDate != DateTime.MinValue)
            {
                saudaLimitInputDto.LoginUserId = UserId;
                saudaLimitInputDto.DataSourceRequest = request;
                result = await _saudaClient.GetSaudaLimitListAsync(saudaLimitInputDto);
            }
            //    if (result != null && result[0] != null && result[0].PostStatus == false)
            //    {
            //        ModelState.AddModelError("SaudaLimit", result[0].PostMessage);
            //        return Json(result.AsQueryable().ToDataSourceResult(request, ModelState));
            //    }

            //}
            //var resultList = result.ToDataSourceResult(request);
            return Json(result);
        }


        /// <summary>
        /// Method to ApproveSaudaLimit
        /// </summary>
        /// <param name="questionIdDto"></param>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageSaudaLimit)]
        [HttpPost]
        public async Task<ActionResult> ApproveorRejectSaudaLimit(List<SaudaLimitRequestDetailDto> checkedLimitRequestIds, int status, string remark = null)
        {

            var saudaLimitRequestDto = new SaudaLimitRequestDto()
            {
                LimitRequest = checkedLimitRequestIds,
                Remark = remark ?? null,
                Status = status
            };

            var saudaApprovalViewModel = await _saudaClient.ApproveorRejectSaudaLimit(saudaLimitRequestDto);
            return Json(saudaApprovalViewModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SpecialRate

        /// <summary>
        /// Method to Get Sauda limit
        /// </summary>
        /// <returns></returns>
        public ActionResult SpecialRateApproval()
        {
            return View();
        }

        /// <summary>
        /// Method to Get Sauda limit List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetSpecialRateApprovalListAsync([DataSourceRequest] DataSourceRequest request, SpecialRateAddInputDto specialRateApprovalInputDto)
        {
            var result = new List<SpecialRateApprovalOutputDto>();
            if (specialRateApprovalInputDto != null && specialRateApprovalInputDto.FromDate != null && specialRateApprovalInputDto.ToDate != null)
            {
                specialRateApprovalInputDto.LoginUserId = UserId;
                result = await _saudaClient.GetSpecialRateApprovalListAsync(specialRateApprovalInputDto);
                if (result != null && result[0] != null && result[0].PostStatus == false)
                {
                    ModelState.AddModelError("SpecialRate", result[0].PostMessage);
                    return Json(result.AsQueryable().ToDataSourceResult(request, ModelState));
                }
            }
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }


        /// <summary>
        /// Method to ApproveSaudaLimit
        /// </summary>
        /// <param name="questionIdDto"></param>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ApproveSpecialRate)]
        [HttpPost]
        public async Task<ActionResult> ApproveorRejectSpecialRate(string checkedSpecialRateRequestIds, int status, string remark = null)
        {

            var specialRateRequestDto = new SpecialRateRequestDto()
            {
                SpecialRateRequest = UtilityHelper.ConvertStringToLongList(checkedSpecialRateRequestIds),
                Remark = remark ?? null,
                LoginUserId = UserId,
                Status = status
            };

            var saudaApprovalViewModel = await _saudaClient.ApproveorRejectSpecialRate(specialRateRequestDto);
            return Json(saudaApprovalViewModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Dealer & StateTrader - Special Rate Approval

        [AuthorizeClaims(Claims.ViewSpecialRate)]
        public ActionResult SpecialRateApprovalList()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }

        [HttpPost]
        public async Task<ActionResult> ZHProceedSpecialRateForApproval(SpecialRateApprovalDto inputDto)
        {
            var roleId = RoleId;
            inputDto.RequestedBy = UserId;
            inputDto.LoginUserId = UserId;
            inputDto.StatusId = (int)Status.RequestForApproval;

            var result = await _saudaClient.SaveSpecialRateApproval(inputDto);
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> ApproveSpecialRate(SpecialRateApprovalDto inputDto)
        {
            var roleId = RoleId;
            inputDto.RequestedBy = UserId;
            inputDto.LoginUserId = UserId;
            inputDto.StatusId = (int)Status.Approved;

            var result = await _saudaClient.SaveSpecialRateApproval(inputDto);
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> RejectSpecialRate(SpecialRateApprovalDto inputDto)
        {
            var roleId = RoleId;
            inputDto.RequestedBy = UserId;
            inputDto.LoginUserId = UserId;
            inputDto.StatusId = (int)Status.Rejected;

            var result = await _saudaClient.SaveSpecialRateApproval(inputDto);
            return Json(result);
        }

        public async Task<ActionResult> GetSpecialRateApprovalList([DataSourceRequest] DataSourceRequest request, SpecialRateAddInputDto specialRateApprovalInputDto)
        {
            var result = new List<SpecialRateApprovalOutputDto>();
            if (specialRateApprovalInputDto != null && specialRateApprovalInputDto.FromDate != null && specialRateApprovalInputDto.ToDate != null)
            {
                specialRateApprovalInputDto.LoginUserId = UserId;
                // specialRateApprovalInputDto.VerticalId = VerticalId;
                result = await _saudaClient.GetSpecialRateApprovalListWithAccessPermission(specialRateApprovalInputDto);
            }
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        #endregion

        #region Sauda Convertion

        [AuthorizeClaims(Claims.ViewSaudaConversion)]
        public ActionResult SaudaConversionList()
        {
            return View();
        }

        [AuthorizeClaims(Claims.ViewSaudaConversion)]
        public async Task<ActionResult> SaudaConversionDetail()
        {
            var result = new SaudaConversionDetailForAdminDto();
            if (Session["SaudaConvertionId"] != null && UtilityHelper.IntTryToParse(Session["SaudaConvertionId"].ToString()) > 0)
            {
                var inputDto = new SaudaConversionDetailInputDto() { SaudaConversionId = UtilityHelper.LongTryToParse(Session["SaudaConvertionId"].ToString()) };
                result = await _saudaClient.WebApiUrlGetSaudaConversionAllDetail(inputDto);
            }
            return View(result);
        }

        public ActionResult SaudaConversionEdit(string saudaConvertionId)
        {
            Session["SaudaConvertionId"] = saudaConvertionId;
            return RedirectToAction("SaudaConversionDetail", "Sauda");
        }

        public async Task<ActionResult> GetSaudaConversionList([DataSourceRequest] DataSourceRequest request, DateTime fromDate, DateTime todate, int statusId)
        {
            var inputDto = new SaudaConvertionFilterDto() { FromDate = fromDate, ToDate = todate, StatusId = statusId, VerticalId = VerticalId };
            var result = await _saudaClient.GetSaudaConversionList(inputDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> GetSaudaConversionDetails([DataSourceRequest] DataSourceRequest request, long saudaConvertionId)
        {
            var inputDto = new SaudaConversionDetailInputDto { SaudaConversionId = saudaConvertionId };
            var result = await _saudaClient.GetSaudaConversionDetails(inputDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> GetSaudaConversionDetailsNew([DataSourceRequest] DataSourceRequest request, long saudaConvertionId)
        {
            var inputDto = new SaudaConversionDetailInputDto { SaudaConversionId = saudaConvertionId };
            var result = await _saudaClient.GetSaudaConversionDetailsNew(inputDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<JsonResult> UpdateSaudhaConversionStatus(SaudaConversionUpdateDto saudaUpdateDto)
        {
            saudaUpdateDto.ModifiedBy = UserId;
            if (saudaUpdateDto.SaudaId != 0)
                saudaUpdateDto.SaudaIds.Add(saudaUpdateDto.SaudaId);
            saudaUpdateDto = await _saudaClient.ApproveSaudaConversion(saudaUpdateDto);
            return Json(saudaUpdateDto, JsonRequestBehavior.AllowGet);
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
            cell.Style.Fill.BackgroundColor.SetColor(Color.Blue);
            if (level > 0)
            {
                cell.Style.Fill.BackgroundColor.SetColor(Color.Green);
            }
            return cell;
        }
        private ExcelRange GetExcelContent(ExcelRange cell, string text, int align = 0)
        {
            cell.Value = text ?? string.Empty;

            cell.Style.Border.Top.Style =
                cell.Style.Border.Left.Style =
                    cell.Style.Border.Right.Style = cell.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            if (align == 1)//align right
            {
                cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            }

            return cell;
        }

        public async Task<ActionResult> GenerateExcelSaudaConversionAsync(DateTime fromDate, DateTime todate, int statusId)
        {
            var stream = new MemoryStream();
            var result = new ResultModel { IsSuccess = false, Message = "Error Occured while exporting Excel. Please retry." };
            try
            {
                var inputDto = new SaudaConvertionFilterDto() { FromDate = fromDate, ToDate = todate, StatusId = statusId };
                var saudaConversionDetails = await _saudaClient.GetSaudaConversionListAsync(inputDto);

                var fileName = "SaudaExtension.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    var rowIndex = 1;
                    var colIndex = 1;
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DealerName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SaudaId"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Sku"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_QuotedPrice"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BidQuantity"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BidQuantityCases"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BidPrice"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BidPricePerCase"));

                    foreach (var saudaConversion in saudaConversionDetails)
                    {
                        rowIndex++;
                        colIndex = 1;
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaConversion.DealerName != null ? saudaConversion.DealerName.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaConversion.SaudaId.ToString());
                        foreach (var saudaConversionDetail in saudaConversion.SaudaOrderDetailsList)
                        {
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaConversionDetail.SkuName != null ? saudaConversion.DealerName.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaConversionDetail.QuotedPrice.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaConversionDetail.BidQuantity.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaConversionDetail.BidQuantityCases.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaConversionDetail.BidPrice.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaConversionDetail.BidPricePerCase.ToString());

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
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region View TP and RA

        public ActionResult TraditionalProcessPricingList()
        {
            return View();
        }

        public ActionResult ReverseAuctionPricingList()
        {
            return View();
        }

        public async Task<ActionResult> GetTPandRAPricingList([DataSourceRequest] DataSourceRequest request, DateTime createdDate, DateTime biddingDate, int saudaBookingTypeId, long biddingWindowId = 0)
        {
            var inputDto = new PricingTPandRAInputDto() { CreatedDate = createdDate, BiddingDate = biddingDate, LoginUserId = UserId, SaudaBookingTypeId = saudaBookingTypeId, BiddingWindowId = biddingWindowId, VerticalId = VerticalId };
            var result = await _saudaClient.GetTPandRAPricingList(inputDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        #endregion

        #region Sauda Extension

        [AuthorizeClaims(Claims.ViewSaudaExtension)]
        public ActionResult SaudaExtensionList()
        {
            return View();
        }

        [AuthorizeClaims(Claims.ViewSaudaExtension)]
        public async Task<ActionResult> SaudaExtensionDetail()
        {
            var result = new SaudaConversionDetailForAdminDto();
            if (Session["SaudaExtensionId"] != null && UtilityHelper.IntTryToParse(Session["SaudaExtensionId"].ToString()) > 0)
            {
                var inputDto = new SaudaConversionDetailInputDto() { SaudaConversionId = UtilityHelper.LongTryToParse(Session["SaudaExtensionId"].ToString()) };
                result = await _saudaClient.WebApiUrlGetSaudaExtensionAllDetail(inputDto);
            }
            return View(result);
        }

        public ActionResult SaudaExtensionEdit(string saudaExtensionId)
        {
            Session["SaudaExtensionId"] = saudaExtensionId;
            return RedirectToAction("SaudaExtensionDetail", "Sauda");
        }

        public async Task<ActionResult> GetSaudaExtensionList([DataSourceRequest] DataSourceRequest request, DateTime fromDate, DateTime todate, int statusId)
        {
            var inputDto = new SaudaConvertionFilterDto() { FromDate = fromDate, ToDate = todate, StatusId = statusId, VerticalId = VerticalId };
            var result = await _saudaClient.GetSaudaExtensionList(inputDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> GetSaudaExtensionDetails([DataSourceRequest] DataSourceRequest request, long saudaConvertionId)
        {
            var inputDto = new SaudaConversionDetailInputDto { SaudaConversionId = saudaConvertionId };
            var result = await _saudaClient.GetSaudaExtensionDetails(inputDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<JsonResult> UpdateSaudhaExtensionStatus(SaudaConversionUpdateDto saudaUpdateDto)
        {
            saudaUpdateDto.ModifiedBy = UserId;
            if (saudaUpdateDto.SaudaId != 0)
                saudaUpdateDto.SaudaIds.Add(saudaUpdateDto.SaudaId);
            saudaUpdateDto = await _saudaClient.ApproveSaudaExtension(saudaUpdateDto);
            return Json(saudaUpdateDto, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GenerateExcelExtensionAsync(DateTime fromDate, DateTime todate, int statusId)
        {
            var stream = new MemoryStream();
            var result = new ResultModel { IsSuccess = false, Message = "Error Occured while exporting Excel. Please retry." };
            try
            {
                var inputDto = new SaudaConvertionFilterDto() { FromDate = fromDate, ToDate = todate, StatusId = statusId };
                var saudaExtensionDetails = await _saudaClient.GetSaudaExtensionList(inputDto);
                //var saudaExtensionDetails = await _saudaClient.ExportSaudaExtensionList(inputDto);

                var fileName = $"{Guid.NewGuid()}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    var rowIndex = 1;
                    var colIndex = 1;
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DealerName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_City"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Plant"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IncoTerm"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SaudaNumber"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ExpiryDate"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ValidFrom"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ExtendToDate"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Status"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IsExtension"));

                    foreach (var saudaExtension in saudaExtensionDetails)
                    {
                        rowIndex++;
                        colIndex = 1;
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.DealerName != null ? saudaExtension.DealerName.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.CityName != null ? saudaExtension.CityName.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.PlantName != null ? saudaExtension.PlantName.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.IncoTerm != null ? saudaExtension.IncoTerm.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.SaudaNumber != null ? saudaExtension.SaudaNumber.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.ExpiryDate.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.ValidFrom.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.ExtendToDate.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.StatusName != null ? saudaExtension.StatusName.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.IsExtension.ToString());
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
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SaudaExtensionExportAsync(SaudaConvertionFilterDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {

                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                var saudaExtensionDetails = await _saudaClient.ExportSaudaExtensionList(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "SAUDA-EXTENSION-" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
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
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DealerName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_City"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Plant"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IncoTerm"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SaudaNumber"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ExpiryDate"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ValidFrom"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ExtendToDate"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SkuName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SkuCode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_FinalPrice"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_QuantityPerMT"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_QuantityPerCase"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PendingQuantityMT"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PendingQuantityCase"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PricePerCase"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Price"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Status"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IsExtension"));

                    ////To set top row as static
                    //worksheet.View.FreezePanes(2, 1);
                    ////To implement filters
                    //worksheet.Cells["A1:AQ1"].AutoFilter = true;

                    if (saudaExtensionDetails != null && saudaExtensionDetails.Any())
                    {
                        foreach (var saudaExtension in saudaExtensionDetails)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.DealerName != null ? saudaExtension.DealerName.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.CityName != null ? saudaExtension.CityName.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.PlantName != null ? saudaExtension.PlantName.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.IncoTerm != null ? saudaExtension.IncoTerm.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.SaudaNumber != null ? saudaExtension.SaudaNumber.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.ExpiryDate.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.ValidFrom.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.ExtendToDate.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.SkuName != null ? saudaExtension.SkuName.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.SkuCode != null ? saudaExtension.SkuCode.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.QuotedPrice.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.BidQuantity.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.BidQuantityCases.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.PendingQuantity.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.PendingQuantityCases.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.BidPricePerCase.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.BidPrice.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.StatusName != null ? saudaExtension.StatusName.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaExtension.IsExtension.ToString());
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

        #region Sauda Conversion

        public ActionResult SaudaConversionAndBaseRateList()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }

        public async Task<JsonResult> GetSaudaConversionUnitAndDiffRateList([DataSourceRequest] DataSourceRequest request, DateTime FromDate, DateTime ToDate, long VerticalId)
        {
            var result = new List<SaudaConversionUnitAndDiffRateDto>();

            var inputDto = new SaudaConversionUnitAndDiffRateInputDto()
            {
                FromDate = FromDate,
                ToDate = ToDate,
                VerticalId = VerticalId,
                LoginUserId = UserId
            };
            result = await _saudaClient.GetSaudaConversionUnitAndDiffRateList(inputDto);
            var griddata = result.ToDataSourceResult(request);
            return Json(griddata, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaudaConversionUnitAndBaseRate()
        {
            var model = new SaudaConversionUnitAndDiffRateModel();
            model.VerticalId = VerticalId;
            return View(model);
        }

        public ActionResult GetPackGroupList([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<DropDownDto>() { new DropDownDto() { Id = 1, Name = "BP" }, new DropDownDto() { Id = 2, Name = "CP" } };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetSkuListByPackGroupId([DataSourceRequest] DataSourceRequest request, SkuDropDownInputDto inputDto)
        {
            var result = await _saudaClient.GetSkuListByPackGroupId(inputDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> AddSaudaConversionUnitandDiffRate(SaudaConversionUnitAndDiffRateModel inputDto)
        {
            inputDto.LoginUserId = UserId;
            if (inputDto.StateIdsInString != "")
            {
                inputDto.StateIds = inputDto.StateIdsInString.Split(',').Select(Int64.Parse).ToList();
            }
            if (inputDto.SourceIdsInString != "")
            {
                inputDto.SourceIds = inputDto.SourceIdsInString.Split(',').Select(Int64.Parse).ToList();
            }
            var result = await _saudaClient.AddSaudaConversionUnitandDiffRate(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                var newResult = new SaudaConversionUnitAndDiffRateModel();
                newResult.PostStatus = true;
                newResult.PostMessage = result.PostMessage;

                return Json(newResult);
            }
            else
            {
                inputDto.PostStatus = false;
                inputDto.PostMessage = result.PostMessage;
            }

            return Json(inputDto);
        }

        /// <summary>
        /// Method to Export Sauda Conversion Unit And DiffRate
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportSaudaConversionUnitAndDiffRate(ExcelExportInputDto inputDto)
        {
            var finalResult = new JsonResult();
            DateTime currentDate = DateTime.Now;
            string fileName = "SaudaConversionUnitAndDiffRate" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
            var guidFileName = $"{Guid.NewGuid()}.xlsx";
            try
            {
                //inputDto.VerticalId = VerticalId;
                var resultList = _saudaClient.ExportSaudaConversionUnitAndDiffRate(inputDto);
                if (resultList.Any())
                {
                    using (var package = new ExcelPackage())
                    {
                        var ws = package.Workbook.Worksheets.Add("SaudaConversionUnitAndDiffRate");

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
                        ws.Cells["D2:I2"].Value = "Sauda Conversion Unit And Rate Difference";
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

        #endregion Sauda Conversion

        #region Competitor Analysis


        /// <summary>
        /// Method to redirect CompetitorAnalysis add or update page
        /// </summary>
        /// <param name="competitorAnalysisId"></param>
        /// <returns></returns>
        public ActionResult CompetitorAnalysisEditRedirect(string competitorAnalysisId = "")
        {
            Session["CompetitorAnalysisId"] = competitorAnalysisId;
            return RedirectToAction("SaveCompetitorAnalysis", "Sauda");
        }

        /// <summary>
        /// Method to Get CompetitorAnalysis List page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ViewPriceDiscovery)]
        public ActionResult CompetitorAnalysisList()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }

        /// <summary>
        /// Method to Get CompetitorAnalysis List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetCompetitorAnalysisListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData, long verticalId)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData, VerticalId = verticalId, DataSourceRequest = request };
            var result = await _saudaClient.GetCompetitorAnalysisListAsync(loginUserIdDto);
            //var resultList = result.ToDataSourceResult(request);
            return Json(result);
        }

        public async Task<ActionResult> GetCompetitorAnalysisDetailsListAsync([DataSourceRequest] DataSourceRequest request, long competitorAnalysisId)
        {
            var result = await _saudaClient.GetCompetitorAnalysisDetailsListById(competitorAnalysisId);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        /// <summary>
        /// Method to get CompetitorAnalysis add or update page
        /// </summary>
        /// <returns></returns>
        //[AuthorizeClaims(Claims.ManageCompetitor, Claims.ViewCompetitor)]
        public ActionResult SaveCompetitorAnalysis()
        {
            var result = new CompetitorAnalysisDto();
            return View(result);
        }

        /// <summary>
        /// Method to  add or update CompetitorAnalysis  
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> AddOrUpdateCompetitorAnalysis(CompetitorAnalysisDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            CompetitorAnalysisAddDto competitorAnalysisAddDto = new CompetitorAnalysisAddDto
            {
                SkuId = inputDto.SkuId,
                OilTypeId = inputDto.OilTypeId,
                StatusId = 1,
                Margin = 0,
                EmamiPrice = inputDto.EmamiPrice,
                WorkableQuantity = 10,
                WorkablePrice = 10,
                Remarks = "test Remarks",
                LoginUserId = inputDto.LoginUserId
            };
            competitorAnalysisAddDto.CompetitorAnalysisDetailsList = new List<CompetitorAnalysisDetailsAddDto> {
            new CompetitorAnalysisDetailsAddDto
            {
                CompetitorId = inputDto.CompetitorId,
                SaudaRate = 10,
                MarketOperatingPrice = 10,
            }};

            List<CompetitorAnalysisAddDto> competitorAnalysisAddDtos = new List<CompetitorAnalysisAddDto>();
            competitorAnalysisAddDtos.Add(competitorAnalysisAddDto);

            CompetitorAnalysisInputDto competitorAnalysisInputDto = new CompetitorAnalysisInputDto { CompetitorAnalysisList = competitorAnalysisAddDtos };
            var result = await _saudaClient.AddOrUpdateCompetitorAnalysis(competitorAnalysisInputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("CompetitorAnalysisList", "Sauda");
            }
            return View("SaveCompetitorAnalysis", result);
        }

        public ActionResult CompetitorAnalysisDetailsEditRedirect(string competitorAnalysisId = "")
        {
            Session["CompetitorAnalysisId"] = competitorAnalysisId;
            return RedirectToAction("ApproveCompetitorAnalysis", "Sauda");
        }

        /// <summary>
        /// Method to get action for view request
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ViewPriceDiscovery)]
        public async Task<ActionResult> ApproveCompetitorAnalysis()
        {
            var result = new CompetitorAnalysisViewDto();
            if (Session["CompetitorAnalysisId"] != null && !string.IsNullOrEmpty(Session["CompetitorAnalysisId"].ToString()))
            {
                IdInputDto idInputDto = new IdInputDto { Id = UtilityHelper.LongTryToParse(Session["CompetitorAnalysisId"].ToString()), LoginUserId = UserId };
                result = await _saudaClient.GetCompetitorAnalysisById(idInputDto);
            }
            result.LoginUserId = UserId;
            result.RoleId = RoleId;
            return View(result);
        }

        [HttpPost]
        public async Task<ActionResult> CompetitorAnalysisRequestForApproval(CompetitorAnalysisApprovalDto inputDto)
        {
            var roleId = RoleId;
            inputDto.RequestedBy = UserId;
            inputDto.LoginUserId = UserId;
            inputDto.StatusId = (int)Status.RequestForApproval;

            var result = await _saudaClient.SaveCompetitorAnalysisApproval(inputDto);
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> ApproveCompetitorAnalysis(CompetitorAnalysisApprovalDto inputDto)
        {
            var roleId = RoleId;
            inputDto.RequestedBy = UserId;
            inputDto.LoginUserId = UserId;
            inputDto.StatusId = (int)Status.Approved;

            var result = await _saudaClient.SaveCompetitorAnalysisApproval(inputDto);
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> RejectCompetitorAnalysis(CompetitorAnalysisApprovalDto inputDto)
        {
            var roleId = RoleId;
            inputDto.RequestedBy = UserId;
            inputDto.LoginUserId = UserId;
            inputDto.StatusId = (int)Status.Rejected;

            var result = await _saudaClient.SaveCompetitorAnalysisApproval(inputDto);
            return Json(result);
        }

        public async Task<ActionResult> GetCompetitorListddlAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData };
            var result = await _saudaClient.GetCompetitorListAsync(loginUserIdDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region SuadaBookingRestriction List

        [HttpGet]
        /// <summary>
        /// To get sauda booking configuration list
        /// </summary>
        /// <returns></returns>
        public ActionResult SaudaBookingConfigurationList()
        {
            Session["EncryptedId"] = null;
            return View();
        }

        public async Task<JsonResult> GetSuadaBookingRestrictionListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var saudaRestrictionList = await _saudaClient.GetSuadaBookingRestrictionListAsync(UserId);
            return Json(saudaRestrictionList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetRoleListForSaudaBookingConfiguration([DataSourceRequest] DataSourceRequest request)
        {
            var resultList = await _saudaClient.GetRoleListForSaudaBookingConfiguration();
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> ExportSaudaBookingConfiguration()
        {
            var finalResult = new JsonResult();
            DateTime currentDate = DateTime.Now;
            string fileName = "SuadaBookingConfiguration" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
            var guidFileName = $"{Guid.NewGuid()}.xlsx";

            try
            {
                var resultList = await _saudaClient.GetSuadaBookingRestrictionListAsync(UserId);

                if (resultList.IsAny())
                {
                    var saudaBookingExportData =
                        resultList.Select(_ => new SaudaBookingConfigurationExportDto
                    {
                        Id = _.Id,
                        RoleName = _.RoleName,
                        UserName = _.UserNames,
                        OilType = _.OilTypeNames,
                        StartDate = _.StartDate.ToString("yyyy-MM-dd hh:mm"),
                        IsActive = _.IsActive ? "True" : "False"
                    }).ToList();

                    using (var package = new ExcelPackage())
                    {
                        var ws = package.Workbook.Worksheets.Add("SuadaBookingConfiguration");

                        //Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.Font.Size = 14;
                        ws.Cells["A1:F1"].Style.Font.Name = "Calibri";
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        ws.Cells["A2:B2"].Merge = true;
                        ws.Cells["A2:B2"].Value = "Report Name";
                        ws.Cells["A2:B2"].Style.Font.Size = 12;
                        ws.Cells["A2:B2"].Style.Font.Name = "Calibri";
                        ws.Cells["A2:B2"].Style.Font.Bold = true;

                        ws.Cells["C2:F2"].Merge = true;
                        ws.Cells["C2:F2"].Value = "Sauda Booking Configuration";
                        ws.Cells["C2:F2"].Style.Font.Size = 12;
                        ws.Cells["C2:F2"].Style.Font.Name = "Calibri";
                        ws.Cells["C2:F2"].Style.Font.Bold = true;

                        ws.Cells["A4:AZ4"].Style.Font.Size = 12;
                        ws.Cells["A4:AZ4"].Style.Font.Name = "Calibri";
                        ws.Cells["A4:AZ4"].Style.Font.Bold = true;
                        ws.Cells["A4"].LoadFromCollection(saudaBookingExportData, true);
                        ws.Cells.AutoFitColumns();

                        return SaveExcelFileToPath(package, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region SuadaSalesAreaRestriction

        [HttpGet]
        /// <summary>
        /// To get sauda sales area Restriciton list
        /// </summary>
        /// <returns></returns>
        public ActionResult SaudaSalesAreaRestrictionList()
        {
            Session["EncryptedId"] = null;
            return View();
        }

        public async Task<JsonResult> GetSuadaSalesAreaRestrictionListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var saudaRestrictionList = await _saudaClient.GetSuadaSalesAreaRestrictionListAsync(UserId);
            return Json(saudaRestrictionList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> ExportSaudaSalesAreaRestrictionConfiguration()
        {
            var finalResult = new JsonResult();
            DateTime currentDate = DateTime.Now;
            string fileName = "SaudaSalesAreaRestrictionConfiguration" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
            var guidFileName = $"{Guid.NewGuid()}.xlsx";

            try
            {
                var resultList = await _saudaClient.GetSuadaSalesAreaRestrictionListAsync(UserId);

                if (resultList.IsAny())
                {
                    var saudaSalesAreaRestrictionExportData =
                        resultList.Select(_ => new SaudaSalesAreaRestrictionConfigurationExportDto
                        {
                            Id = _.Id,
                            SalesOrganizationName = _.SalesOrganizationName,
                            DistributionChannelName = _.DistributionChannelName,
                            DivisionName = _.DivisionName,
                            TimeRestrictionString = _.TimeRestrictionString,
                            ValidFrom = _.ValidFrom.ToString("yyyy-MM-dd hh:mm"),
                            ValidTo = _.ValidTo.ToString("yyyy-MM-dd hh:mm")
                        }).ToList();

                    using (var package = new ExcelPackage())
                    {
                        var ws = package.Workbook.Worksheets.Add("SaudaSalesAreaRestrictionConfiguration");

                        //Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.Font.Size = 14;
                        ws.Cells["A1:F1"].Style.Font.Name = "Calibri";
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        ws.Cells["A2:B2"].Merge = true;
                        ws.Cells["A2:B2"].Value = "Report Name";
                        ws.Cells["A2:B2"].Style.Font.Size = 12;
                        ws.Cells["A2:B2"].Style.Font.Name = "Calibri";
                        ws.Cells["A2:B2"].Style.Font.Bold = true;

                        ws.Cells["C2:F2"].Merge = true;
                        ws.Cells["C2:F2"].Value = "Sauda Sales Area Restriction Configuration";
                        ws.Cells["C2:F2"].Style.Font.Size = 12;
                        ws.Cells["C2:F2"].Style.Font.Name = "Calibri";
                        ws.Cells["C2:F2"].Style.Font.Bold = true;

                        ws.Cells["A4:AZ4"].Style.Font.Size = 12;
                        ws.Cells["A4:AZ4"].Style.Font.Name = "Calibri";
                        ws.Cells["A4:AZ4"].Style.Font.Bold = true;
                        ws.Cells["A4"].LoadFromCollection(saudaSalesAreaRestrictionExportData, true);
                        ws.Cells.AutoFitColumns();

                        return SaveExcelFileToPath(package, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}