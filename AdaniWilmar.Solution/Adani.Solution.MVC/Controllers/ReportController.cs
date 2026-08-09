using GMCore.Logger;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Enums;
using Adani.Solution.MVC.Attributes;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.ServiceClient;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Drawing;
using Kendo.Mvc.UI;
using Adani.Solution.DTO.Common;
using System.Data;
using Adani.Solution.MVC.Models;
using GMCore.Helper;
using Kendo.Mvc.Extensions;
using System.IO.Compression;
using System.Data.SqlClient;

namespace Adani.Solution.MVC.Controllers
{
    [TokenAuthorize]
    [CustomRedirect]
    [NoCache]
    public class ReportController : BaseController
    {
        private const string ServiceName = "Report Controller";
        private readonly ReportClient _reportClient;
        private readonly MasterClient _masterClient;
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;

        public ReportController()
        {
            _reportClient = new ReportClient { ControllerDelegate = this };
            _masterClient = new MasterClient { ControllerDelegate = this };
        }

        /// <summary>
        /// Method to get Material Cost List page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult OilPriceReport()
        {
            return View();
        }

        [AuthorizeClaims(Claims.ViewReports)]
        [HttpPost]
        public async Task<ActionResult> GetOilPriceReport(OilPriceReportInputDto inputDto)
        {
            var data = await _reportClient.GetOilPriceReportAsync(inputDto);
            return PartialView(data);
        }
        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult CostChangeReport()
        {
            return View();
        }

        [AuthorizeClaims(Claims.ViewReports)]
        [HttpPost]
        public async Task<ActionResult> GetCostChangeReport(ReportInputDto inputDto)
        {
            var data = await _reportClient.GetCostChangeReport(inputDto);
            return PartialView(data);
        }

        #region Distributor Stock Report
        [AuthorizeClaims(Claims.DistributorStockReport)]
        public ActionResult DistributorStockReport()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }

        public async Task<ActionResult> GetDistributorStockReport(DateTime fromDate, DateTime toDate, List<long> stateIds, int verticalId, long salesorganizationId, long distributionChannelId, long oilTypeId)
        {
            _methodName = "GetDistributorStockReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "DISTRIBUTOR-STOCK-REPORT-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";

            try
            {
                DistributorStockReportInputDto inputDto = new DistributorStockReportInputDto()
                {
                    RoleId = RoleId,
                    LoginUserId = UserId,
                    FromDate = fromDate,
                    ToDate = toDate,
                    StateIds = stateIds,
                    VerticalId = verticalId,
                    SalesOrganizationId = salesorganizationId,
                    DistributionChannelId = distributionChannelId,
                    OilTypeId = oilTypeId
                };
                var publishData = await _reportClient.GetDistributorStockReportAsync(inputDto);

                if (publishData != null && publishData.Any())
                {
                    using (var ep = new ExcelPackage())
                    {
                        var ws = ep.Workbook.Worksheets.Add("Sheet1");
                        ws.Name = "Distributor Stock Report";

                        ws.Cells["A1:K1"].Style.Font.Size = 13;
                        ws.Cells["A1:K1"].Style.Font.Name = "Calibri";
                        ws.Cells["A1:K1"].Style.Font.Bold = true;
                        ws.Cells.LoadFromCollection(publishData, true);

                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sauda Order Report
        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult SaudaOrderReport()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }

        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult UserLoginHistory()
        {
          return View();
        }

        public async Task<ActionResult> UserLoginHistoryReport(DateTime fromDate, DateTime toDate)
        {
            _methodName = "UserLoginHistoryReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "UserLoginHistory-REPORT-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";

            try
            {
                UserLoginHistoryDto inputputDto = new UserLoginHistoryDto() { FromDate = fromDate, ToDate = toDate, LoginUserId = UserId };
                var publishData = await _reportClient.GetUserLoginHistoryReport(inputputDto);

                if (publishData != null)
                {
                    //var result = publishData.FirstOrDefault();

                    string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFoloderPath + "FinalPriceTemplateForAllRecords.xlsx");

                    using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = "Distibutor Usage Count";
                        var ws2 = ep.Workbook.Worksheets.Add("Sheet2");
                        ws2.Name = "Distibutor Usage Report";
                        var ws3 = ep.Workbook.Worksheets.Add("Sheet3");
                        ws3.Name = "AWL User Usage Report";

                        #region Header
                        //  ws.Cells["A1:F1"].Merge = true;
                        //  ws.Cells["A1:F1"].Value = "Adani Wilmar Ltd.";
                        //  ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        //  ws.Cells["A1:F1"].Style.Font.Bold = true;
                        //  ws.Cells["A1:F1"].Style.Font.Size = 16;

                        //  ws.Cells["A2"].Value = "Report Name";
                        //  ws.Cells["A3"].Value = "From Date";
                        //  ws.Cells["A4"].Value = "To Date";
                        //  ws.Cells["A5"].Value = "Total Record Count";
                        ////  ws.Cells["A6"].Value = "Vertical";

                        //  for (int i = 2; i <= 6; i++)
                        //  {
                        //      ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        //      ws.Cells["A" + i].Style.Font.Bold = true;
                        //      ws.Cells["A" + i].Style.Font.Size = 12;

                        //      ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                        //      ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        //  }

                        //  ws.Cells["B2"].Value = "Contract";
                        //  ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                        //  ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                        //  ws.Cells["B5"].Value = publishData.Count;
                        //  ws.Cells["B6"].Value = verticalId == (int)DTO.Enums.Division.Hbc ? "Hbc" : verticalId == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        #endregion

                        ws.Cells["A1:AP1"].Style.Font.Size = 13;
                        ws.Cells["A1:AP1"].Style.Font.Name = "Calibri";
                        ws.Cells["A1:AP1"].Style.Font.Bold = true;
                        ws.Cells.LoadFromDataTable(publishData.Tables[0], true);

                        ws2.Cells["A1:AP1"].Style.Font.Size = 13;
                        ws2.Cells["A1:AP1"].Style.Font.Name = "Calibri";
                        ws2.Cells["A1:AP1"].Style.Font.Bold = true;
                        ws2.Cells.LoadFromDataTable(publishData.Tables[1], true);

                        ws3.Cells["A1:AP1"].Style.Font.Size = 13;
                        ws3.Cells["A1:AP1"].Style.Font.Name = "Calibri";
                        ws3.Cells["A1:AP1"].Style.Font.Bold = true;
                        ws3.Cells.LoadFromDataTable(publishData.Tables[2], true);

                        #region Old Binding Method
                        /*int headerIndex = 8;
                                        ws.Cells["A" + headerIndex].Value = "OilType";
                                        ws.Cells["B" + headerIndex].Value = "Material Description";
                                        ws.Cells["C" + headerIndex].Value = "Material Code";
                                        ws.Cells["D" + headerIndex].Value = "Material Qty";
                                        ws.Cells["E" + headerIndex].Value = "UOM";
                                        ws.Cells["F" + headerIndex].Value = "Material Qty(MT)";
                                        ws.Cells["G" + headerIndex].Value = "Pack Group";
                                        ws.Cells["H" + headerIndex].Value = "State";
                                        ws.Cells["I" + headerIndex].Value = "Customer Code";
                                        ws.Cells["J" + headerIndex].Value = "Customer Name";
                                        //ws.Cells["K" + headerIndex].Value = "Route Name";
                                        ws.Cells["K" + headerIndex].Value = "Plant Name";
                                        ws.Cells["L" + headerIndex].Value = "Incoterms";
                                        //ws.Cells["N" + headerIndex].Value = "Depot Code";
                                        //ws.Cells["O" + headerIndex].Value = "Depot Name";
                                        ws.Cells["M" + headerIndex].Value = "Broker Code";
                                        ws.Cells["N" + headerIndex].Value = "Broker Name";
                                        ws.Cells["O" + headerIndex].Value = "App Contract Time";
                                        ws.Cells["P" + headerIndex].Value = "App Contract Date";
                                        ws.Cells["Q" + headerIndex].Value = "Contract Valid From";
                                        ws.Cells["R" + headerIndex].Value = "Contract Valid To";
                                        //ws.Cells["V" + headerIndex].Value = "Material Cost";
                                        ws.Cells["S" + headerIndex].Value = "Premium";
                                        ws.Cells["T" + headerIndex].Value = "TD";
                                        ws.Cells["U" + headerIndex].Value = "LTD";
                                        //ws.Cells["Z" + headerIndex].Value = "Margin Cost TP";
                                        //ws.Cells["AA" + headerIndex].Value = "Packing Cost";
                                        //ws.Cells["AB" + headerIndex].Value = "Honeycomb cost";
                                        //ws.Cells["AC" + headerIndex].Value = "Primary Freight";
                                        //ws.Cells["AD" + headerIndex].Value = "Secondary Freight";
                                        //ws.Cells["AE" + headerIndex].Value = "Depot Cost";
                                        //ws.Cells["AF" + headerIndex].Value = "Detention charges";
                                        //ws.Cells["AG" + headerIndex].Value = "PR00";
                                        //ws.Cells["AH" + headerIndex].Value = "FRC1";
                                        ws.Cells["V" + headerIndex].Value = "Basic Rate";
                                        ws.Cells["W" + headerIndex].Value = "Total Value";
                                        ws.Cells["X" + headerIndex].Value = "SalesOrganization";
                                        ws.Cells["Y" + headerIndex].Value = "DistributionChannel";
                                        ws.Cells["Z" + headerIndex].Value = "Division";
                                        //ws.Cells["AL" + headerIndex].Value = "Actual Packing Cost";
                                        ws.Cells["AA" + headerIndex].Value = "Employee Code";
                                        ws.Cells["AB" + headerIndex].Value = "Employee Name";
                                        ws.Cells["AC" + headerIndex].Value = "Remarks";
                                        //ws.Cells["AP" + headerIndex].Value = "Realization Per case";
                                        //ws.Cells["AQ" + headerIndex].Value = "Realization Per MT";
                                        //ws.Cells["AR" + headerIndex].Value = "Brokerage";
                                        //ws.Cells["AS" + headerIndex].Value = "Realization Per case Post Brokerage";
                                        //ws.Cells["AD" + headerIndex].Value = "SKU WISE Weight";
                                        //ws.Cells["AE" + headerIndex].Value = "Tax paid";
                                        ws.Cells["AD" + headerIndex].Value = "Sauda Type";
                                        //ws.Cells["AG" + headerIndex].Value = "Pack Size";
                                        //ws.Cells["AX" + headerIndex].Value = "Margin Cost RA";
                                        ws.Cells["AE" + headerIndex].Value = "Status";
                                        ws.Cells["AF" + headerIndex].Value = "Special Rate";
                                        //ws.Cells["BA" + headerIndex].Value = "Cushion Margin";
                                        //ws.Cells["BB" + headerIndex].Value = "Scheme Cost";
                                        ws.Cells["AG" + headerIndex].Value = "OilType";
                                        //ws.Cells["AK" + headerIndex].Value = "Material Type";
                                        //ws.Cells["AY" + headerIndex].Value = "Purchase";
                                        //ws.Cells["AZ" + headerIndex].Value = "Purchase Total";
                                        //ws.Cells["BB" + headerIndex].Value = "Area";
                                        //ws.Cells["BD" + headerIndex].Value = "Margin PMT line item";                       
                                        //ws.Cells["BE" + headerIndex].Value = "RA Discount Total";
                                        //ws.Cells["BF" + headerIndex].Value = "Customer Group Margin";
                                        //ws.Cells["BG" + headerIndex].Value = "RA Premium With Tax";
                                        //ws.Cells["BH" + headerIndex].Value = "RA Premium (Without Tax )";
                                        //ws.Cells["BI" + headerIndex].Value = "Additional Cost";
                                        //ws.Cells["BJ" + headerIndex].Value = "OilTransfer Cost";
                                        //ws.Cells["BK" + headerIndex].Value = "SKU Conversion(With Tax)";
                                        //ws.Cells["BL" + headerIndex].Value = "SKU Conversion(Without Tax)";
                                        //ws.Cells["BM" + headerIndex].Value = "Customer Group One";
                                        ws.Cells["AH" + headerIndex].Value = "Customer Group Five";
                                        ws.Cells["AI" + headerIndex].Value = "Sauda Number";
                                        ws.Cells["AJ" + headerIndex].Value = "App Booking No";
                                        ws.Cells["AK" + headerIndex].Value = "App Id";

                                        ExcelRange range = ws.Cells["A7:BJ7"];
                                        range.AutoFitColumns();
                                        range.Style.Font.Size = 12;
                                        range.Style.Font.Bold = true;
                                        int contentIndex = 9;

                                        foreach (var data in publishData)
                                        {
                                            ws.Cells["A" + contentIndex].Value = data.OilType; //Product Group
                                            ws.Cells["B" + contentIndex].Value = data.SkuName; //  "Material Description";
                                            ws.Cells["C" + contentIndex].Value = data.SkuCode; //  "Material Code";
                                            ws.Cells["D" + contentIndex].Value = data.BidQuantityCase; //  "Material Qty";
                                            ws.Cells["E" + contentIndex].Value = data.UOM; //  "UOM";
                                            ws.Cells["F" + contentIndex].Value = data.BidQuantity; //  "Material Qty(MT)";
                                            ws.Cells["G" + contentIndex].Value = data.PackGroup; //  "Product Group";
                                            ws.Cells["H" + contentIndex].Value = data.State; //  "State";
                                            ws.Cells["I" + contentIndex].Value = data.CustomerCode; // "Customer Code";
                                            ws.Cells["J" + contentIndex].Value = data.CustomerName; //  "Customer Name";
                                            //ws.Cells["K" + contentIndex].Value = data.FreightRoute; //  "Route Name";
                                            ws.Cells["K" + contentIndex].Value = data.PlantName; //  "Plant Name";
                                            ws.Cells["L" + contentIndex].Value = data.Incoterms; //  "Incoterms";
                                            //ws.Cells["N" + contentIndex].Value = data.DepotCode; //  "Depot Code";
                                            //ws.Cells["O" + contentIndex].Value = data.DepotName; //  "Depot Name";
                                            ws.Cells["M" + contentIndex].Value = data.BrokerCode; //  "Broker Code";
                                            ws.Cells["N" + contentIndex].Value = data.BrokerName; //  "Broker Name";
                                            ws.Cells["O" + contentIndex].Value = data.BiddingTime.ToString("hh\\:mm\\:ss"); //App Contract Time
                                            ws.Cells["P" + contentIndex].Value = Settings.DateFormats(data.BiddingDate, Settings.ReportDateFormat); //  "App Contract Date";
                                            ws.Cells["Q" + contentIndex].Value = Settings.DateFormats(data.ValidFromDate, Settings.ReportDateFormat); //  "Contract Valid From";
                                            ws.Cells["R" + contentIndex].Value = Settings.DateFormats(data.ValidToDate, Settings.ReportDateFormat); //  "Contract Valid To";
                                            //ws.Cells["V" + contentIndex].Value = data.MaterialCost; //  "Material Cost";
                                            ws.Cells["S" + contentIndex].Value = data.Premium;  // "Premium";
                                            ws.Cells["T" + contentIndex].Value = data.TD;  // "TD";
                                            ws.Cells["U" + contentIndex].Value = data.LTDValue;  // "LTD";
                                            //ws.Cells["Z" + contentIndex].Value = data.MarginCostTP;  // "Margin Cost TP";
                                            //ws.Cells["AA" + contentIndex].Value = data.PackingCost; // "Packing Cost";
                                            //ws.Cells["AB" + contentIndex].Value = data.HoneycombCost; // "Honeycomb cost";    
                                            //ws.Cells["AC" + contentIndex].Value = data.PrimaryFreight; // "Primary Freight";
                                            //ws.Cells["AD" + contentIndex].Value = data.SecondaryFreight;  // "Secondary Freight";
                                            //ws.Cells["AE" + contentIndex].Value = data.DepotCost;  // "Depot Cost";
                                            //ws.Cells["AF" + contentIndex].Value = data.DetentionCharges; // "Detention charges";
                                            //ws.Cells["AG" + contentIndex].Value = data.PR00; //  "PR00";
                                            //ws.Cells["AH" + contentIndex].Value = data.FRC1; //  "FRC1";
                                            ws.Cells["V" + contentIndex].Value = data.SaleRate; //  "Sale Rate";
                                            ws.Cells["W" + contentIndex].Value = data.TotalValue;  // "Total Value";
                                            ws.Cells["X" + contentIndex].Value = data.SalesOrganization;  // "Vertical";
                                            ws.Cells["Y" + contentIndex].Value = data.DistributionChannel;  // "Vertical";
                                            ws.Cells["Z" + contentIndex].Value = data.Vertical;  // "Vertical";
                                            //ws.Cells["AL" + contentIndex].Value = data.ActualPackingCost;  // "Actual Packing Cost";
                                            ws.Cells["AA" + contentIndex].Value = data.EmployeeCode;  // "Employee Code";
                                            ws.Cells["AB" + contentIndex].Value = data.EmployeeName;  // "Employee Name";
                                            ws.Cells["AC" + contentIndex].Value = data.Remarks; //Remarks

                                            //if (data.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                            //{
                                            //    ws.Cells["AP" + contentIndex].Value = Utility.DecimalFormatTwo(data.RealizationPerCase);  // "Realization Per case";
                                            //}
                                            //else
                                            //{
                                            //ws.Cells["AP" + contentIndex].Value = Math.Round(data.RealizationPerCase);  // "Realization Per case";
                                            ////}
                                            //ws.Cells["AQ" + contentIndex].Value = Math.Round(data.RealizationPerMt); //  "Realization Per MT";
                                            //ws.Cells["AR" + contentIndex].Value = data.Brokerage; //Brokerage
                                            //ws.Cells["AS" + contentIndex].Value = Math.Round(data.RealizationPerCasePostBrokerage); //Realization Per case Post Brokerage
                                            //ws.Cells["AD" + contentIndex].Value = Math.Round(data.SkuWiseWeight, 3); //SKU WISE Weight
                                            //ws.Cells["AE" + contentIndex].Value = data.TaxPaid; //Tax paid
                                            ws.Cells["AD" + contentIndex].Value = data.SaudaBookingType;  // "Sauda Type";
                                            //ws.Cells["AG" + contentIndex].Value = data.PackSize; //  "Pack Size";
                                            //ws.Cells["AX" + contentIndex].Value = data.MarginCostRA;  // "Margin Cost RA";
                                            ws.Cells["AE" + contentIndex].Value = data.Status.ToLower() == DTO.Enums.Status.Pending.ToString().ToLower()
                                                ? "Accepted" : data.Status;  // "Status";
                                            ws.Cells["AF" + contentIndex].Value = data.SpecialRate;  // "Special Rate";
                                            //ws.Cells["BA" + contentIndex].Value = data.CushionMargin; //Cushion Margin
                                            //ws.Cells["BB" + contentIndex].Value = data.SchemeCost;
                                            ws.Cells["AG" + contentIndex].Value = data.OilType;
                                           //ws.Cells["AK" + contentIndex].Value = data.MaterialType;

                                            //ws.Cells["AX" + contentIndex].Value = Math.Round(data.RealizationTotal); //Realization total
                                            //ws.Cells["AY" + contentIndex].Value = data.Purchase; //Purchase
                                            //ws.Cells["AZ" + contentIndex].Value = data.PurchaseTotal; //Purchase total
                                            //ws.Cells["BB" + contentIndex].Value = data.Area; //Area
                                            //ws.Cells["BD" + contentIndex].Value = data.MarginPMTLineItem; //Margin PMT line item

                                            //ws.Cells["BE" + contentIndex].Value = data.RaTotalDiscount;
                                            //ws.Cells["BF" + contentIndex].Value = data.CustomerGroupMargin;

                                            //ws.Cells["BG" + contentIndex].Value = data.RAPremiumWithTax;
                                            //ws.Cells["BH" + contentIndex].Value = data.RAPremiumWithoutTax;

                                            //ws.Cells["BI" + contentIndex].Value = data.AdditionalCost; //Realization Per MT Post Brokerage
                                            //ws.Cells["BJ" + contentIndex].Value = data.OilTransferCost; //Final realization
                                            //if (!data.IsBaseSauda)
                                            //{
                                            //    ws.Cells["BK" + contentIndex].Value = data.SkuAllocationPremiumWithTax; //Realization Per MT Post Brokerage
                                            //    ws.Cells["BL" + contentIndex].Value = data.SkuAllocationPremiumWithoutTax; //Final realization
                                            //}
                                            //ws.Cells["BM" + contentIndex].Value = data.CustomerGroupOne;
                                            ws.Cells["AH" + contentIndex].Value = data.CustomerGroupFive;
                                            ws.Cells["AI" + contentIndex].Value = data.SaudaNumber; // Sauda Number
                                            ws.Cells["AJ" + contentIndex].Value = data.AppBookingNo; //  "App Booking No";
                                            ws.Cells["AK" + contentIndex].Value = data.SaudaOrderId; //App Id

                                            contentIndex++;
                                        }*/
                        #endregion

                        ws.Cells.AutoFitColumns();
                        ws2.Cells.AutoFitColumns();
                        ws3.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetSaudaOrderDetailsReport(DateTime fromDate, DateTime toDate, List<long> stateIds, int verticalId, List<long> statusIds, long salesorganizationId, long distributionChannelId)
        {
            _methodName = "GetSaudaOrderDetailsReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "CONTRACT-REPORT-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";
            
            try
            {
                SaudaOrderReportInputputDto inputputDto = new SaudaOrderReportInputputDto() {RoleId=RoleId, FromDate = fromDate, ToDate = toDate, StateIds = stateIds, VerticalId = verticalId, StatusIds = statusIds, SalesOrganizationId = salesorganizationId, DistributionChannelId = distributionChannelId, LoginUserId = UserId };
                var publishData = await _reportClient.GetNewSaudaOrderDetailsReportAsync(inputputDto);

                if (publishData != null && publishData.Any())
                {
                    var result = publishData.FirstOrDefault();

                    string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFoloderPath + "FinalPriceTemplate.xlsx");

                    using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = "Contract";

                        #region Header
                        //  ws.Cells["A1:F1"].Merge = true;
                        //  ws.Cells["A1:F1"].Value = "Adani Wilmar Ltd.";
                        //  ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        //  ws.Cells["A1:F1"].Style.Font.Bold = true;
                        //  ws.Cells["A1:F1"].Style.Font.Size = 16;

                        //  ws.Cells["A2"].Value = "Report Name";
                        //  ws.Cells["A3"].Value = "From Date";
                        //  ws.Cells["A4"].Value = "To Date";
                        //  ws.Cells["A5"].Value = "Total Record Count";
                        ////  ws.Cells["A6"].Value = "Vertical";

                        //  for (int i = 2; i <= 6; i++)
                        //  {
                        //      ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        //      ws.Cells["A" + i].Style.Font.Bold = true;
                        //      ws.Cells["A" + i].Style.Font.Size = 12;

                        //      ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                        //      ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        //  }

                        //  ws.Cells["B2"].Value = "Contract";
                        //  ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                        //  ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                        //  ws.Cells["B5"].Value = publishData.Count;
                        //  ws.Cells["B6"].Value = verticalId == (int)DTO.Enums.Division.Hbc ? "Hbc" : verticalId == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        #endregion

                        ws.Cells["A1:AZ1"].Style.Font.Size = 13;
                        ws.Cells["A1:AZ1"].Style.Font.Name = "Calibri";
                        ws.Cells["A1:AZ1"].Style.Font.Bold = true;
                        ws.Cells.LoadFromCollection(publishData, true);

                        #region Old Binding Method
                        /*int headerIndex = 8;
                                        ws.Cells["A" + headerIndex].Value = "OilType";
                                        ws.Cells["B" + headerIndex].Value = "Material Description";
                                        ws.Cells["C" + headerIndex].Value = "Material Code";
                                        ws.Cells["D" + headerIndex].Value = "Material Qty";
                                        ws.Cells["E" + headerIndex].Value = "UOM";
                                        ws.Cells["F" + headerIndex].Value = "Material Qty(MT)";
                                        ws.Cells["G" + headerIndex].Value = "Pack Group";
                                        ws.Cells["H" + headerIndex].Value = "State";
                                        ws.Cells["I" + headerIndex].Value = "Customer Code";
                                        ws.Cells["J" + headerIndex].Value = "Customer Name";
                                        //ws.Cells["K" + headerIndex].Value = "Route Name";
                                        ws.Cells["K" + headerIndex].Value = "Plant Name";
                                        ws.Cells["L" + headerIndex].Value = "Incoterms";
                                        //ws.Cells["N" + headerIndex].Value = "Depot Code";
                                        //ws.Cells["O" + headerIndex].Value = "Depot Name";
                                        ws.Cells["M" + headerIndex].Value = "Broker Code";
                                        ws.Cells["N" + headerIndex].Value = "Broker Name";
                                        ws.Cells["O" + headerIndex].Value = "App Contract Time";
                                        ws.Cells["P" + headerIndex].Value = "App Contract Date";
                                        ws.Cells["Q" + headerIndex].Value = "Contract Valid From";
                                        ws.Cells["R" + headerIndex].Value = "Contract Valid To";
                                        //ws.Cells["V" + headerIndex].Value = "Material Cost";
                                        ws.Cells["S" + headerIndex].Value = "Premium";
                                        ws.Cells["T" + headerIndex].Value = "TD";
                                        ws.Cells["U" + headerIndex].Value = "LTD";
                                        //ws.Cells["Z" + headerIndex].Value = "Margin Cost TP";
                                        //ws.Cells["AA" + headerIndex].Value = "Packing Cost";
                                        //ws.Cells["AB" + headerIndex].Value = "Honeycomb cost";
                                        //ws.Cells["AC" + headerIndex].Value = "Primary Freight";
                                        //ws.Cells["AD" + headerIndex].Value = "Secondary Freight";
                                        //ws.Cells["AE" + headerIndex].Value = "Depot Cost";
                                        //ws.Cells["AF" + headerIndex].Value = "Detention charges";
                                        //ws.Cells["AG" + headerIndex].Value = "PR00";
                                        //ws.Cells["AH" + headerIndex].Value = "FRC1";
                                        ws.Cells["V" + headerIndex].Value = "Basic Rate";
                                        ws.Cells["W" + headerIndex].Value = "Total Value";
                                        ws.Cells["X" + headerIndex].Value = "SalesOrganization";
                                        ws.Cells["Y" + headerIndex].Value = "DistributionChannel";
                                        ws.Cells["Z" + headerIndex].Value = "Division";
                                        //ws.Cells["AL" + headerIndex].Value = "Actual Packing Cost";
                                        ws.Cells["AA" + headerIndex].Value = "Employee Code";
                                        ws.Cells["AB" + headerIndex].Value = "Employee Name";
                                        ws.Cells["AC" + headerIndex].Value = "Remarks";
                                        //ws.Cells["AP" + headerIndex].Value = "Realization Per case";
                                        //ws.Cells["AQ" + headerIndex].Value = "Realization Per MT";
                                        //ws.Cells["AR" + headerIndex].Value = "Brokerage";
                                        //ws.Cells["AS" + headerIndex].Value = "Realization Per case Post Brokerage";
                                        //ws.Cells["AD" + headerIndex].Value = "SKU WISE Weight";
                                        //ws.Cells["AE" + headerIndex].Value = "Tax paid";
                                        ws.Cells["AD" + headerIndex].Value = "Sauda Type";
                                        //ws.Cells["AG" + headerIndex].Value = "Pack Size";
                                        //ws.Cells["AX" + headerIndex].Value = "Margin Cost RA";
                                        ws.Cells["AE" + headerIndex].Value = "Status";
                                        ws.Cells["AF" + headerIndex].Value = "Special Rate";
                                        //ws.Cells["BA" + headerIndex].Value = "Cushion Margin";
                                        //ws.Cells["BB" + headerIndex].Value = "Scheme Cost";
                                        ws.Cells["AG" + headerIndex].Value = "OilType";
                                        //ws.Cells["AK" + headerIndex].Value = "Material Type";
                                        //ws.Cells["AY" + headerIndex].Value = "Purchase";
                                        //ws.Cells["AZ" + headerIndex].Value = "Purchase Total";
                                        //ws.Cells["BB" + headerIndex].Value = "Area";
                                        //ws.Cells["BD" + headerIndex].Value = "Margin PMT line item";                       
                                        //ws.Cells["BE" + headerIndex].Value = "RA Discount Total";
                                        //ws.Cells["BF" + headerIndex].Value = "Customer Group Margin";
                                        //ws.Cells["BG" + headerIndex].Value = "RA Premium With Tax";
                                        //ws.Cells["BH" + headerIndex].Value = "RA Premium (Without Tax )";
                                        //ws.Cells["BI" + headerIndex].Value = "Additional Cost";
                                        //ws.Cells["BJ" + headerIndex].Value = "OilTransfer Cost";
                                        //ws.Cells["BK" + headerIndex].Value = "SKU Conversion(With Tax)";
                                        //ws.Cells["BL" + headerIndex].Value = "SKU Conversion(Without Tax)";
                                        //ws.Cells["BM" + headerIndex].Value = "Customer Group One";
                                        ws.Cells["AH" + headerIndex].Value = "Customer Group Five";
                                        ws.Cells["AI" + headerIndex].Value = "Sauda Number";
                                        ws.Cells["AJ" + headerIndex].Value = "App Booking No";
                                        ws.Cells["AK" + headerIndex].Value = "App Id";

                                        ExcelRange range = ws.Cells["A7:BJ7"];
                                        range.AutoFitColumns();
                                        range.Style.Font.Size = 12;
                                        range.Style.Font.Bold = true;
                                        int contentIndex = 9;

                                        foreach (var data in publishData)
                                        {
                                            ws.Cells["A" + contentIndex].Value = data.OilType; //Product Group
                                            ws.Cells["B" + contentIndex].Value = data.SkuName; //  "Material Description";
                                            ws.Cells["C" + contentIndex].Value = data.SkuCode; //  "Material Code";
                                            ws.Cells["D" + contentIndex].Value = data.BidQuantityCase; //  "Material Qty";
                                            ws.Cells["E" + contentIndex].Value = data.UOM; //  "UOM";
                                            ws.Cells["F" + contentIndex].Value = data.BidQuantity; //  "Material Qty(MT)";
                                            ws.Cells["G" + contentIndex].Value = data.PackGroup; //  "Product Group";
                                            ws.Cells["H" + contentIndex].Value = data.State; //  "State";
                                            ws.Cells["I" + contentIndex].Value = data.CustomerCode; // "Customer Code";
                                            ws.Cells["J" + contentIndex].Value = data.CustomerName; //  "Customer Name";
                                            //ws.Cells["K" + contentIndex].Value = data.FreightRoute; //  "Route Name";
                                            ws.Cells["K" + contentIndex].Value = data.PlantName; //  "Plant Name";
                                            ws.Cells["L" + contentIndex].Value = data.Incoterms; //  "Incoterms";
                                            //ws.Cells["N" + contentIndex].Value = data.DepotCode; //  "Depot Code";
                                            //ws.Cells["O" + contentIndex].Value = data.DepotName; //  "Depot Name";
                                            ws.Cells["M" + contentIndex].Value = data.BrokerCode; //  "Broker Code";
                                            ws.Cells["N" + contentIndex].Value = data.BrokerName; //  "Broker Name";
                                            ws.Cells["O" + contentIndex].Value = data.BiddingTime.ToString("hh\\:mm\\:ss"); //App Contract Time
                                            ws.Cells["P" + contentIndex].Value = Settings.DateFormats(data.BiddingDate, Settings.ReportDateFormat); //  "App Contract Date";
                                            ws.Cells["Q" + contentIndex].Value = Settings.DateFormats(data.ValidFromDate, Settings.ReportDateFormat); //  "Contract Valid From";
                                            ws.Cells["R" + contentIndex].Value = Settings.DateFormats(data.ValidToDate, Settings.ReportDateFormat); //  "Contract Valid To";
                                            //ws.Cells["V" + contentIndex].Value = data.MaterialCost; //  "Material Cost";
                                            ws.Cells["S" + contentIndex].Value = data.Premium;  // "Premium";
                                            ws.Cells["T" + contentIndex].Value = data.TD;  // "TD";
                                            ws.Cells["U" + contentIndex].Value = data.LTDValue;  // "LTD";
                                            //ws.Cells["Z" + contentIndex].Value = data.MarginCostTP;  // "Margin Cost TP";
                                            //ws.Cells["AA" + contentIndex].Value = data.PackingCost; // "Packing Cost";
                                            //ws.Cells["AB" + contentIndex].Value = data.HoneycombCost; // "Honeycomb cost";    
                                            //ws.Cells["AC" + contentIndex].Value = data.PrimaryFreight; // "Primary Freight";
                                            //ws.Cells["AD" + contentIndex].Value = data.SecondaryFreight;  // "Secondary Freight";
                                            //ws.Cells["AE" + contentIndex].Value = data.DepotCost;  // "Depot Cost";
                                            //ws.Cells["AF" + contentIndex].Value = data.DetentionCharges; // "Detention charges";
                                            //ws.Cells["AG" + contentIndex].Value = data.PR00; //  "PR00";
                                            //ws.Cells["AH" + contentIndex].Value = data.FRC1; //  "FRC1";
                                            ws.Cells["V" + contentIndex].Value = data.SaleRate; //  "Sale Rate";
                                            ws.Cells["W" + contentIndex].Value = data.TotalValue;  // "Total Value";
                                            ws.Cells["X" + contentIndex].Value = data.SalesOrganization;  // "Vertical";
                                            ws.Cells["Y" + contentIndex].Value = data.DistributionChannel;  // "Vertical";
                                            ws.Cells["Z" + contentIndex].Value = data.Vertical;  // "Vertical";
                                            //ws.Cells["AL" + contentIndex].Value = data.ActualPackingCost;  // "Actual Packing Cost";
                                            ws.Cells["AA" + contentIndex].Value = data.EmployeeCode;  // "Employee Code";
                                            ws.Cells["AB" + contentIndex].Value = data.EmployeeName;  // "Employee Name";
                                            ws.Cells["AC" + contentIndex].Value = data.Remarks; //Remarks

                                            //if (data.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                            //{
                                            //    ws.Cells["AP" + contentIndex].Value = Utility.DecimalFormatTwo(data.RealizationPerCase);  // "Realization Per case";
                                            //}
                                            //else
                                            //{
                                            //ws.Cells["AP" + contentIndex].Value = Math.Round(data.RealizationPerCase);  // "Realization Per case";
                                            ////}
                                            //ws.Cells["AQ" + contentIndex].Value = Math.Round(data.RealizationPerMt); //  "Realization Per MT";
                                            //ws.Cells["AR" + contentIndex].Value = data.Brokerage; //Brokerage
                                            //ws.Cells["AS" + contentIndex].Value = Math.Round(data.RealizationPerCasePostBrokerage); //Realization Per case Post Brokerage
                                            //ws.Cells["AD" + contentIndex].Value = Math.Round(data.SkuWiseWeight, 3); //SKU WISE Weight
                                            //ws.Cells["AE" + contentIndex].Value = data.TaxPaid; //Tax paid
                                            ws.Cells["AD" + contentIndex].Value = data.SaudaBookingType;  // "Sauda Type";
                                            //ws.Cells["AG" + contentIndex].Value = data.PackSize; //  "Pack Size";
                                            //ws.Cells["AX" + contentIndex].Value = data.MarginCostRA;  // "Margin Cost RA";
                                            ws.Cells["AE" + contentIndex].Value = data.Status.ToLower() == DTO.Enums.Status.Pending.ToString().ToLower()
                                                ? "Accepted" : data.Status;  // "Status";
                                            ws.Cells["AF" + contentIndex].Value = data.SpecialRate;  // "Special Rate";
                                            //ws.Cells["BA" + contentIndex].Value = data.CushionMargin; //Cushion Margin
                                            //ws.Cells["BB" + contentIndex].Value = data.SchemeCost;
                                            ws.Cells["AG" + contentIndex].Value = data.OilType;
                                           //ws.Cells["AK" + contentIndex].Value = data.MaterialType;

                                            //ws.Cells["AX" + contentIndex].Value = Math.Round(data.RealizationTotal); //Realization total
                                            //ws.Cells["AY" + contentIndex].Value = data.Purchase; //Purchase
                                            //ws.Cells["AZ" + contentIndex].Value = data.PurchaseTotal; //Purchase total
                                            //ws.Cells["BB" + contentIndex].Value = data.Area; //Area
                                            //ws.Cells["BD" + contentIndex].Value = data.MarginPMTLineItem; //Margin PMT line item

                                            //ws.Cells["BE" + contentIndex].Value = data.RaTotalDiscount;
                                            //ws.Cells["BF" + contentIndex].Value = data.CustomerGroupMargin;

                                            //ws.Cells["BG" + contentIndex].Value = data.RAPremiumWithTax;
                                            //ws.Cells["BH" + contentIndex].Value = data.RAPremiumWithoutTax;

                                            //ws.Cells["BI" + contentIndex].Value = data.AdditionalCost; //Realization Per MT Post Brokerage
                                            //ws.Cells["BJ" + contentIndex].Value = data.OilTransferCost; //Final realization
                                            //if (!data.IsBaseSauda)
                                            //{
                                            //    ws.Cells["BK" + contentIndex].Value = data.SkuAllocationPremiumWithTax; //Realization Per MT Post Brokerage
                                            //    ws.Cells["BL" + contentIndex].Value = data.SkuAllocationPremiumWithoutTax; //Final realization
                                            //}
                                            //ws.Cells["BM" + contentIndex].Value = data.CustomerGroupOne;
                                            ws.Cells["AH" + contentIndex].Value = data.CustomerGroupFive;
                                            ws.Cells["AI" + contentIndex].Value = data.SaudaNumber; // Sauda Number
                                            ws.Cells["AJ" + contentIndex].Value = data.AppBookingNo; //  "App Booking No";
                                            ws.Cells["AK" + contentIndex].Value = data.SaudaOrderId; //App Id

                                            contentIndex++;
                                        }*/
                        #endregion

                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult SaudaReport()
        {
            return View();
        }



        public async Task<ActionResult> SaudaExportAsync(SaudaOrderReportInputputDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {

                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<SaudaBDOWiseReportDto> saudaList = new List<SaudaBDOWiseReportDto>();
                saudaList = await _reportClient.SaudaExportAsync(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "SAUDA-REPORT" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
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
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SalesPersonCode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SalesPersonName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PartyCode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PartyName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OilType"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BPInMT"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BPInCase"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CPInMT"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CPInCase"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_TotalSalesInMT"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_TotalSalesInCase"));

                    ////To set top row as static
                    //worksheet.View.FreezePanes(2, 1);
                    ////To implement filters
                    //worksheet.Cells["A1:AQ1"].AutoFilter = true;

                    if (saudaList != null && saudaList.Any())
                    {
                        var bdoList = saudaList.Select(_ => _.BDOCode).Distinct().ToList();
                        foreach (var StateTrader in bdoList)
                        {
                            var bdoWiseSaudaList = saudaList.Where(_ => _.BDOCode == StateTrader);
                            foreach (var sales in bdoWiseSaudaList)
                            {
                                rowIndex++;
                                colIndex = 1;
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.BDOCode != null ? sales.BDOCode.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.BDOName != null ? sales.BDOName.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.DealerCode != null ? sales.DealerCode.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.DealerName != null ? sales.DealerName.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.OilTypeName != null ? sales.OilTypeName.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.BPInMT != null ? sales.BPInMT.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.BPInCase != null ? sales.BPInCase.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.CPInMT != null ? sales.CPInMT.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.CPInCase != null ? sales.CPInCase.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.TotalSalesInMT != null ? sales.TotalSalesInMT.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.TotalSalesInCase != null ? sales.TotalSalesInCase.ToString() : string.Empty);
                            }
                            rowIndex++;
                            colIndex = 6;
                            worksheet.Cells["A" + rowIndex.ToString() + ":E" + rowIndex.ToString()].Merge = true;
                            worksheet.Cells["A" + rowIndex.ToString() + ":E" + rowIndex.ToString()].Value = @Helper.GetResourceString("lbl_SubTotal");
                            worksheet.Cells["A" + rowIndex.ToString() + ":E" + rowIndex.ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells["A" + rowIndex.ToString() + ":K" + rowIndex.ToString()].Style.Font.Bold = true;
                            worksheet.Cells["A" + rowIndex.ToString() + ":K" + rowIndex.ToString()].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells["A" + rowIndex.ToString() + ":K" + rowIndex.ToString()].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], bdoWiseSaudaList.Select(_ => _.BPInMT != null ? _.BPInMT : 0).Sum().ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], bdoWiseSaudaList.Select(_ => _.BPInCase != null ? _.BPInCase : 0).Sum().ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], bdoWiseSaudaList.Select(_ => _.CPInMT != null ? _.CPInMT : 0).Sum().ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], bdoWiseSaudaList.Select(_ => _.CPInCase != null ? _.CPInCase : 0).Sum().ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], bdoWiseSaudaList.Select(_ => _.TotalSalesInMT != null ? _.TotalSalesInMT : 0).Sum().ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], bdoWiseSaudaList.Select(_ => _.TotalSalesInCase != null ? _.TotalSalesInCase : 0).Sum().ToString());
                        }
                        rowIndex++;
                        colIndex = 6;
                        worksheet.Cells["A" + rowIndex.ToString() + ":E" + rowIndex.ToString()].Merge = true;
                        worksheet.Cells["A" + rowIndex.ToString() + ":E" + rowIndex.ToString()].Value = @Helper.GetResourceString("lbl_GrandTotal");
                        worksheet.Cells["A" + rowIndex.ToString() + ":E" + rowIndex.ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A" + rowIndex.ToString() + ":K" + rowIndex.ToString()].Style.Font.Bold = true;
                        worksheet.Cells["A" + rowIndex.ToString() + ":K" + rowIndex.ToString()].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells["A" + rowIndex.ToString() + ":K" + rowIndex.ToString()].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaList.Select(_ => _.BPInMT != null ? _.BPInMT : 0).Sum().ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaList.Select(_ => _.BPInCase != null ? _.BPInCase : 0).Sum().ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaList.Select(_ => _.CPInMT != null ? _.CPInMT : 0).Sum().ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaList.Select(_ => _.CPInCase != null ? _.CPInCase : 0).Sum().ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaList.Select(_ => _.TotalSalesInMT != null ? _.TotalSalesInMT : 0).Sum().ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaList.Select(_ => _.TotalSalesInCase != null ? _.TotalSalesInCase : 0).Sum().ToString());
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
        #region Stock Report


        [AuthorizeClaims(Claims.LoadingStockReport)]
        public ActionResult StockReport()
        {
            return View();
        }
        public async Task<ActionResult> GetStockReport(SaleReportDto inputputDto)
        {
            _methodName = "GetDepotCostReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "Sale-REPORT-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = string.Empty;
            try
            {

                var publishData = _reportClient.GetStockReport(inputputDto);
                //var publishData = new List<SaleReportDto>();
                if (publishData != null /*&& publishData.Any()*/)
                {
                    var result = publishData.FirstOrDefault();
                    using (var ep = new ExcelPackage())
                    {
                        var ws = ep.Workbook.Worksheets.Add("Sheet1");
                        ws.Name = "Stock Report";

                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "Total Record Count";
                        ws.Cells["A4"].Value = "Plant";
                        for (int i = 2; i <= 4; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        ws.Cells["B2"].Value = "Stock Report by Plant/Depot";
                        //ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                        //ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B3"].Value = publishData.Count;
                        ws.Cells["B4"].Value = publishData[0].PlantName;
                        //ws.Cells["B6"].Value = verticalIds == (int)DTO.Enums.Division.Hbc ? "Hbc" : verticalIds == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        #endregion

                        int headerIndex = 6;
                        ws.Cells["A" + headerIndex].Value = "Plant";
                        ws.Cells["B" + headerIndex].Value = "Name";
                        ws.Cells["C" + headerIndex].Value = "Material";
                        ws.Cells["D" + headerIndex].Value = "Material Description";
                        ws.Cells["E" + headerIndex].Value = "SLoc";
                        ws.Cells["F" + headerIndex].Value = "BUn";
                        ws.Cells["G" + headerIndex].Value = "Unrestricted";
                        ws.Cells["H" + headerIndex].Value = "Quality Insp";
                        ws.Cells["I" + headerIndex].Value = "Blocked";
                        ws.Cells["J" + headerIndex].Value = "Trans.Tfr";
                        ws.Cells["K" + headerIndex].Value = "Message";

                        ExcelRange range = ws.Cells["A6:K6"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        int contentIndex = 7;
                        foreach (var data in publishData)
                        {
                            ws.Cells["A" + contentIndex].Value = data.PlantName;
                            ws.Cells["B" + contentIndex].Value = data.Name;
                            ws.Cells["C" + contentIndex].Value = data.Material;
                            ws.Cells["D" + contentIndex].Value = data.MaterialDescription;
                            ws.Cells["E" + contentIndex].Value = data.SLoc;
                            ws.Cells["F" + contentIndex].Value = data.BUn;
                            ws.Cells["G" + contentIndex].Value = data.Unrestricted;
                            ws.Cells["H" + contentIndex].Value = data.QualityInsp;
                            ws.Cells["I" + contentIndex].Value = data.Blocked;
                            ws.Cells["J" + contentIndex].Value = data.TransTfr;
                            ws.Cells["K" + contentIndex].Value = data.Message;
                            contentIndex++;
                        }
                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Excel

        /// <summary>
        /// Save to excel file specified path
        /// </summary>
        /// <param name="excelPackage"></param>
        /// <returns></returns>
        

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


        //[AuthorizeClaims(Claims.ViewReports)]
        public ActionResult MarginReport()
        {
            ExcelReportFilterDto roleIdDto = new ExcelReportFilterDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId
            };
            return View(roleIdDto);
        }
        public async Task<ActionResult> GetMarginReport(DateTime fromDate, DateTime toDate, string statusId, string stateIds, string marginTypeId, string verticalId)
        {
            _methodName = "GetMarginReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "MARGIN-REPORT-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";
            try
            {
                //verticalId = VerticalId.ToString();
                long tempMarginTypeId = marginTypeId == "" ? 0 : Convert.ToInt32(marginTypeId);
                long tempVerticalId = verticalId == "" ? 0 : Convert.ToInt32(verticalId);
                ExcelReportFilterDto inputputDto = new ExcelReportFilterDto()
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    StatusIds = statusId,
                    StateIds = stateIds,
                    MarginTypeId = tempMarginTypeId,
                    VerticalId = tempVerticalId

                };

                using (var ep = new ExcelPackage())
                {
                    var ws = ep.Workbook.Worksheets.Add("Sheet1");
                    if (tempMarginTypeId == (int)DTO.Enums.MarginReport.PlantwiseSauda)
                    {
                        ws.Name = "Plant wise Sauda";
                    }
                    else if (tempMarginTypeId == (int)DTO.Enums.MarginReport.StateOilMargin)
                    {
                        ws.Name = "State & Oil Margin";
                    }
                    else if (tempMarginTypeId == (int)DTO.Enums.MarginReport.SaudaReport)
                    {
                        ws.Name = "Margin";
                    }
                    else
                    {
                        ws.Name = "Business Margin";
                    }

                    if (tempMarginTypeId == (int)DTO.Enums.MarginReport.BusinessMargin)
                    {
                        var publishData = _reportClient.GetMarginReport(inputputDto);
                        if (publishData != null && publishData.Any())
                        {
                            var result = publishData.FirstOrDefault();
                            #region Header
                            ws.Cells["A1:F1"].Merge = true;
                            ws.Cells["A1:F1"].Value = Settings.CompanyName;
                            ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells["A1:F1"].Style.Font.Bold = true;
                            ws.Cells["A1:F1"].Style.Font.Size = 16;

                            ws.Cells["A2"].Value = "Report Name";
                            ws.Cells["A3"].Value = "From Date";
                            ws.Cells["A4"].Value = "To Date";
                            ws.Cells["A5"].Value = "Total Record Count";
                            ws.Cells["A6"].Value = "Vertical";

                            for (int i = 2; i <= 6; i++)
                            {
                                ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                ws.Cells["A" + i].Style.Font.Bold = true;
                                ws.Cells["A" + i].Style.Font.Size = 12;

                                ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                                ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            }

                            ws.Cells["B2"].Value = "Business Margin";
                            ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                            ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                            ws.Cells["B5"].Value = publishData.Count;
                            ws.Cells["B6"].Value = tempVerticalId == (int)DTO.Enums.Division.Hbc ? "Hbc" : tempVerticalId == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                            #endregion

                            int headerIndex = 8;
                            ws.Cells["A" + headerIndex].Value = "Product Group";
                            ws.Cells["B" + headerIndex].Value = "Pack Size";
                            ws.Cells["C" + headerIndex].Value = "Material Qty(MT)";
                            ws.Cells["D" + headerIndex].Value = "MarginPMT";

                            ExcelRange range = ws.Cells["A7:D7"];
                            range.AutoFitColumns();
                            range.Style.Font.Size = 12;
                            range.Style.Font.Bold = true;
                            int contentIndex = 9;
                            foreach (var data in publishData)
                            {
                                ws.Cells["A" + contentIndex].Value = data.ProductGroup;
                                ws.Cells["B" + contentIndex].Value = data.PackSize;
                                ws.Cells["C" + contentIndex].Value = data.MaterialQtyInMT;
                                ws.Cells["D" + contentIndex].Value = data.MarginPMT;
                                contentIndex++;
                            }
                            ws.Cells.AutoFitColumns();
                            guidFileName = SaveExcelFileToPath(ep);
                        }
                    }
                    else if (tempMarginTypeId == (int)DTO.Enums.MarginReport.SaudaReport)
                    {
                        List<long> statusarray = new List<long>();
                        List<long> statearray = new List<long>();
                        if (stateIds != null)
                        {
                            string[] splitstate = stateIds.Split(',');
                            for (int runs = 0; runs < splitstate.Length; runs++)
                            {
                                statearray.Add(Convert.ToInt32(splitstate[runs]));
                            }
                        }

                        if (statusId != null)
                        {
                            string[] splitStatus = statusId.Split(',');
                            for (int runs = 0; runs < splitStatus.Length; runs++)
                            {
                                statusarray.Add(Convert.ToInt32(splitStatus[runs]));
                            }
                        }
                        SaudaOrderReportInputputDto saudainputputDto = new SaudaOrderReportInputputDto() { FromDate = fromDate, ToDate = toDate, StateIds = statearray, VerticalId = Convert.ToInt32(tempVerticalId), StatusIds = statusarray };
                        var saudapublishData = await _reportClient.GetSaudaOrderDetailsReportAsync(saudainputputDto);

                        if (saudapublishData != null && saudapublishData.Any())
                        {
                            #region Header
                            ws.Cells["A1:F1"].Merge = true;
                            ws.Cells["A1:F1"].Value = Settings.CompanyName;
                            ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells["A1:F1"].Style.Font.Bold = true;
                            ws.Cells["A1:F1"].Style.Font.Size = 16;

                            ws.Cells["A2"].Value = "Report Name";
                            ws.Cells["A3"].Value = "From Date";
                            ws.Cells["A4"].Value = "To Date";
                            ws.Cells["A5"].Value = "Total Record Count";
                            ws.Cells["A6"].Value = "Vertical";
                            for (int i = 2; i <= 6; i++)
                            {
                                ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                ws.Cells["A" + i].Style.Font.Bold = true;
                                ws.Cells["A" + i].Style.Font.Size = 12;

                                ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                                ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            }

                            ws.Cells["B2"].Value = "Margin";
                            ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                            ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                            ws.Cells["B5"].Value = saudapublishData.Count;
                            ws.Cells["B6"].Value = tempVerticalId == (int)DTO.Enums.Division.Hbc ? "Hbc" : tempVerticalId == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                            #endregion

                            int headerIndex = 8;
                            ws.Cells["A" + headerIndex].Value = "Product Group";
                            ws.Cells["B" + headerIndex].Value = "Material Description";
                            ws.Cells["C" + headerIndex].Value = "Material Code";
                            ws.Cells["D" + headerIndex].Value = "Material Qty";
                            ws.Cells["E" + headerIndex].Value = "UOM";
                            ws.Cells["F" + headerIndex].Value = "Material Qty(MT)";
                            ws.Cells["G" + headerIndex].Value = "Pack Type";
                            ws.Cells["H" + headerIndex].Value = "State";
                            ws.Cells["I" + headerIndex].Value = "Customer Code";
                            ws.Cells["J" + headerIndex].Value = "Customer Name";
                            ws.Cells["K" + headerIndex].Value = "Route Name";
                            ws.Cells["L" + headerIndex].Value = "Plant Name";
                            ws.Cells["M" + headerIndex].Value = "Incoterms";
                            ws.Cells["N" + headerIndex].Value = "Depot Code";
                            ws.Cells["O" + headerIndex].Value = "Depot Name";
                            ws.Cells["P" + headerIndex].Value = "Broker Code";
                            ws.Cells["Q" + headerIndex].Value = "Broker Name";
                            ws.Cells["R" + headerIndex].Value = "App Contract No";
                            ws.Cells["S" + headerIndex].Value = "App Contract Time";
                            ws.Cells["T" + headerIndex].Value = "App Contract Date";
                            ws.Cells["U" + headerIndex].Value = "Contract Valid From";
                            ws.Cells["V" + headerIndex].Value = "Contract Valid To";
                            ws.Cells["W" + headerIndex].Value = "Material Cost";
                            ws.Cells["X" + headerIndex].Value = "Premium";
                            ws.Cells["Y" + headerIndex].Value = "TD";
                            ws.Cells["Z" + headerIndex].Value = "LTD";
                            ws.Cells["AA" + headerIndex].Value = "Margin Cost TP";
                            ws.Cells["AB" + headerIndex].Value = "Packing Cost";
                            ws.Cells["AC" + headerIndex].Value = "Honeycomb cost";
                            ws.Cells["AD" + headerIndex].Value = "Primary Freight";
                            ws.Cells["AE" + headerIndex].Value = "Secondary Freight";
                            ws.Cells["AF" + headerIndex].Value = "Depot Cost";
                            ws.Cells["AG" + headerIndex].Value = "Detention charges";
                            ws.Cells["AH" + headerIndex].Value = "PR00";
                            ws.Cells["AI" + headerIndex].Value = "FRC1";
                            ws.Cells["AJ" + headerIndex].Value = "Sale Rate";
                            ws.Cells["AK" + headerIndex].Value = "Total Value";
                            ws.Cells["AL" + headerIndex].Value = "Vertical";
                            ws.Cells["AM" + headerIndex].Value = "Actual Packing Cost";
                            ws.Cells["AN" + headerIndex].Value = "Employee Code";
                            ws.Cells["AO" + headerIndex].Value = "Employee Name";
                            ws.Cells["AP" + headerIndex].Value = "Remarks";
                            ws.Cells["AQ" + headerIndex].Value = "Realization Per case";
                            ws.Cells["AR" + headerIndex].Value = "Realization Per MT";
                            ws.Cells["AS" + headerIndex].Value = "Brokerage";
                            ws.Cells["AT" + headerIndex].Value = "Realization Per case Post Brokerage";
                            ws.Cells["AU" + headerIndex].Value = "SKU WISE Weight";
                            ws.Cells["AV" + headerIndex].Value = "Realization Per MT Post Brokerage";
                            ws.Cells["AW" + headerIndex].Value = "Final realization";
                            ws.Cells["AX" + headerIndex].Value = "Realization total";
                            ws.Cells["AY" + headerIndex].Value = "Purchase";
                            ws.Cells["AZ" + headerIndex].Value = "Purchase Total";
                            ws.Cells["BA" + headerIndex].Value = "Tax paid";
                            ws.Cells["BB" + headerIndex].Value = "Area";
                            ws.Cells["BC" + headerIndex].Value = "Sauda Type";
                            ws.Cells["BD" + headerIndex].Value = "Margin PMT line item";
                            ws.Cells["BE" + headerIndex].Value = "Pack Size";
                            ws.Cells["BF" + headerIndex].Value = "Margin Cost RA";
                            ws.Cells["BG" + headerIndex].Value = "Status";
                            ws.Cells["BH" + headerIndex].Value = "Special Rate";
                            ws.Cells["BI" + headerIndex].Value = "Cushion Margin";
                            ws.Cells["BJ" + headerIndex].Value = "Scheme Cost";
                            ws.Cells["BK" + headerIndex].Value = "OilType";
                            ws.Cells["BL" + headerIndex].Value = "Material Type";

                            ws.Cells["BM" + headerIndex].Value = "RA Discount Total";
                            ws.Cells["BN" + headerIndex].Value = "Customer Group Margin";
                            ws.Cells["BO" + headerIndex].Value = "RA Premium With Tax";
                            ws.Cells["BP" + headerIndex].Value = "RA Premium (Without Tax )";
                            ws.Cells["BQ" + headerIndex].Value = "Additional Cost";
                            ws.Cells["BR" + headerIndex].Value = "OilTransfer Cost";
                            ws.Cells["BS" + headerIndex].Value = "SKU Conversion(With Tax)";
                            ws.Cells["BT" + headerIndex].Value = "SKU Conversion(Without Tax)";
                            ws.Cells["BU" + headerIndex].Value = "Customer Group One";
                            ws.Cells["BV" + headerIndex].Value = "Customer Group Two";

                            ExcelRange range = ws.Cells["A7:BK7"];
                            range.AutoFitColumns();
                            range.Style.Font.Size = 12;
                            range.Style.Font.Bold = true;
                            int contentIndex = 9;

                            foreach (var data in saudapublishData)
                            {
                                ws.Cells["A" + contentIndex].Value = data.OilType; //Product Group
                                ws.Cells["B" + contentIndex].Value = data.SkuName; //  "Material Description";
                                ws.Cells["C" + contentIndex].Value = data.SkuCode; //  "Material Code";
                                ws.Cells["D" + contentIndex].Value = data.BidQuantityCase; //  "Material Qty";
                                ws.Cells["E" + contentIndex].Value = data.UOM; //  "UOM";
                                ws.Cells["F" + contentIndex].Value = data.BidQuantity; //  "Material Qty(MT)";
                                ws.Cells["G" + contentIndex].Value = data.PackGroup; //  "Product Group";
                                ws.Cells["H" + contentIndex].Value = data.State; //  "State";
                                ws.Cells["I" + contentIndex].Value = data.CustomerCode; // "Customer Code";
                                ws.Cells["J" + contentIndex].Value = data.CustomerName; //  "Customer Name";
                                ws.Cells["K" + contentIndex].Value = data.FreightRoute; //  "Route Name";
                                ws.Cells["L" + contentIndex].Value = data.PlantName; //  "Plant Name";
                                ws.Cells["M" + contentIndex].Value = data.Incoterms; //  "Incoterms";
                                ws.Cells["N" + contentIndex].Value = data.DepotCode; //  "Depot Code";
                                ws.Cells["O" + contentIndex].Value = data.DepotName; //  "Depot Name";
                                ws.Cells["P" + contentIndex].Value = data.BrokerCode; //  "Broker Code";
                                ws.Cells["Q" + contentIndex].Value = data.BrokerName; //  "Broker Name";
                                ws.Cells["R" + contentIndex].Value = data.SaudaNumber; //  "App Contract No";
                                ws.Cells["S" + contentIndex].Value = data.BiddingTime.ToString("hh\\:mm\\:ss"); //App Contract Time
                                ws.Cells["T" + contentIndex].Value = Settings.DateFormats(data.BiddingDate, Settings.ReportDateFormat); //  "App Contract Date";
                                ws.Cells["U" + contentIndex].Value = Settings.DateFormats(data.ValidFromDate, Settings.ReportDateFormat); //  "Contract Valid From";
                                ws.Cells["V" + contentIndex].Value = Settings.DateFormats(data.ValidToDate, Settings.ReportDateFormat); //  "Contract Valid To";
                                ws.Cells["W" + contentIndex].Value = data.MaterialCost; //  "Material Cost";
                                ws.Cells["X" + contentIndex].Value = data.Premium;  // "Premium";
                                ws.Cells["Y" + contentIndex].Value = data.TD;  // "TD";
                                ws.Cells["Z" + contentIndex].Value = data.LTDValue;  // "LTD";
                                ws.Cells["AA" + contentIndex].Value = data.MarginCostTP;  // "Margin Cost TP";
                                ws.Cells["AB" + contentIndex].Value = data.PackingCost; // "Packing Cost";
                                ws.Cells["AC" + contentIndex].Value = data.HoneycombCost; // "Honeycomb cost";    
                                ws.Cells["AD" + contentIndex].Value = data.PrimaryFreight; // "Primary Freight";
                                ws.Cells["AE" + contentIndex].Value = data.SecondaryFreight;  // "Secondary Freight";
                                ws.Cells["AF" + contentIndex].Value = data.DepotCost;  // "Depot Cost";
                                ws.Cells["AG" + contentIndex].Value = data.DetentionCharges; // "Detention charges";
                                ws.Cells["AH" + contentIndex].Value = data.PR00; //  "PR00";
                                ws.Cells["AI" + contentIndex].Value = data.FRC1; //  "FRC1";
                                ws.Cells["AJ" + contentIndex].Value = data.SaleRate; //  "Sale Rate";
                                ws.Cells["AK" + contentIndex].Value = data.TotalValue;  // "Total Value";
                                ws.Cells["AL" + contentIndex].Value = data.Vertical;  // "Vertical";
                                ws.Cells["AM" + contentIndex].Value = data.ActualPackingCost;  // "Actual Packing Cost";
                                ws.Cells["AN" + contentIndex].Value = data.EmployeeCode;  // "Employee Code";
                                ws.Cells["AO" + contentIndex].Value = data.EmployeeName;  // "Employee Name";
                                ws.Cells["AP" + contentIndex].Value = data.Remarks; //Remarks
                                ws.Cells["AQ" + contentIndex].Value = Math.Round(data.RealizationPerCase, 2);  // "Realization Per case";
                                ws.Cells["AR" + contentIndex].Value = Math.Round(data.RealizationPerMt, 2); //  "Realization Per MT";
                                ws.Cells["AS" + contentIndex].Value = data.Brokerage; //Brokerage
                                ws.Cells["AT" + contentIndex].Value = Math.Round(data.RealizationPerCasePostBrokerage, 2); //Realization Per case Post Brokerage
                                ws.Cells["AU" + contentIndex].Value = Math.Round(data.SkuWiseWeight, 3); //SKU WISE Weight
                                ws.Cells["AV" + contentIndex].Value = Math.Round(data.RealizationPerMTPostBrokerage, 2); //Realization Per MT Post Brokerage
                                ws.Cells["AW" + contentIndex].Value = Math.Round(data.FinalRealization, 2); //Final realization
                                ws.Cells["AX" + contentIndex].Value = Math.Round(data.RealizationTotal, 2); //Realization total
                                ws.Cells["AY" + contentIndex].Value = data.Purchase; //Purchase
                                ws.Cells["AZ" + contentIndex].Value = data.PurchaseTotal; //Purchase total
                                ws.Cells["BA" + contentIndex].Value = data.TaxPaid; //Tax paid
                                ws.Cells["BB" + contentIndex].Value = data.Area; //Area
                                ws.Cells["BC" + contentIndex].Value = data.SaudaBookingType;  // "Sauda Type";
                                ws.Cells["BD" + contentIndex].Value = data.MarginPMTLineItem; //Margin PMT line item
                                ws.Cells["BE" + contentIndex].Value = data.PackSize; //  "Pack Size";
                                ws.Cells["BF" + contentIndex].Value = data.MarginCostRA;  // "Margin Cost RA";
                                ws.Cells["BG" + contentIndex].Value = data.Status.ToLower() == DTO.Enums.Status.Pending.ToString().ToLower()
                                    ? "Accepted" : data.Status;  // "Status";
                                ws.Cells["BH" + contentIndex].Value = data.SpecialRate;  // "Special Rate";
                                ws.Cells["BI" + contentIndex].Value = data.CushionMargin; //Cushion Margin
                                ws.Cells["BJ" + contentIndex].Value = data.SchemeCost;
                                ws.Cells["BK" + contentIndex].Value = data.OilType;
                                ws.Cells["BL" + contentIndex].Value = data.MaterialType;

                                ws.Cells["BM" + contentIndex].Value = data.RaTotalDiscount;
                                ws.Cells["BN" + contentIndex].Value = data.CustomerGroupMargin;
                                if (data.IsBaseSauda)
                                {
                                    ws.Cells["BO" + contentIndex].Value = data.RAPremiumWithTax;
                                    ws.Cells["BP" + contentIndex].Value = data.RAPremiumWithoutTax;
                                }
                                ws.Cells["BQ" + contentIndex].Value = data.AdditionalCost;
                                ws.Cells["BR" + contentIndex].Value = data.OilTransferCost;
                                if (!data.IsBaseSauda)
                                {
                                    ws.Cells["BS" + contentIndex].Value = data.SkuAllocationPremiumWithTax;
                                    ws.Cells["BT" + contentIndex].Value = data.SkuAllocationPremiumWithoutTax;
                                }
                                ws.Cells["BU" + contentIndex].Value = data.CustomerGroupOne;
                                ws.Cells["BV" + contentIndex].Value = data.CustomerGroupTwo;

                                contentIndex++;
                            }
                            ws.Cells.AutoFitColumns();
                            guidFileName = SaveExcelFileToPath(ep);
                        }
                    }
                    else
                    {
                        var publishData = _reportClient.GetMarginReport(inputputDto);
                        if (publishData != null && publishData.Any())
                        {
                            var result = publishData.FirstOrDefault();
                            #region Header
                            ws.Cells["A1:F1"].Merge = true;
                            ws.Cells["A1:F1"].Value = Settings.CompanyName;
                            ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells["A1:F1"].Style.Font.Bold = true;
                            ws.Cells["A1:F1"].Style.Font.Size = 16;

                            ws.Cells["A2"].Value = "Report Name";
                            ws.Cells["A3"].Value = "From Date";
                            ws.Cells["A4"].Value = "To Date";
                            ws.Cells["A5"].Value = "Total Record Count";
                            ws.Cells["A6"].Value = "Vertical";
                            for (int i = 2; i <= 6; i++)
                            {
                                ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                ws.Cells["A" + i].Style.Font.Bold = true;
                                ws.Cells["A" + i].Style.Font.Size = 12;

                                ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                                ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            }

                            ws.Cells["B2"].Value = "Margin";
                            ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                            ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                            ws.Cells["B5"].Value = publishData.Count;
                            ws.Cells["B6"].Value = tempVerticalId == (int)DTO.Enums.Division.Hbc ? "Hbc" : tempVerticalId == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                            #endregion

                            int headerIndex = 8;
                            ws.Cells["A" + headerIndex].Value = "Plant";
                            ws.Cells["B" + headerIndex].Value = "State";
                            ws.Cells["C" + headerIndex].Value = "Material Description";
                            ws.Cells["D" + headerIndex].Value = "Material Qty";
                            ws.Cells["E" + headerIndex].Value = "Material Qty(MT)";
                            ws.Cells["F" + headerIndex].Value = "RealizationPMT";
                            ws.Cells["G" + headerIndex].Value = "PurchasePMT";
                            ws.Cells["H" + headerIndex].Value = "MarginPMT";

                            ExcelRange range = ws.Cells["A7:H7"];
                            range.AutoFitColumns();
                            range.Style.Font.Size = 12;
                            range.Style.Font.Bold = true;
                            int contentIndex = 9;
                            foreach (var data in publishData)
                            {
                                ws.Cells["A" + contentIndex].Value = data.PlantName;
                                ws.Cells["B" + contentIndex].Value = data.StateName;
                                ws.Cells["C" + contentIndex].Value = data.SkuName;
                                ws.Cells["D" + contentIndex].Value = data.MaterialQtyInCase;
                                ws.Cells["E" + contentIndex].Value = data.MaterialQtyInMT;
                                ws.Cells["F" + contentIndex].Value = data.RealizationPMT;
                                ws.Cells["G" + contentIndex].Value = data.PurchasePMT;
                                ws.Cells["H" + contentIndex].Value = data.MarginPMT;
                                contentIndex++;
                            }
                            ws.Cells.AutoFitColumns();
                            guidFileName = SaveExcelFileToPath(ep);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMarginReportType([DataSourceRequest] DataSourceRequest request)
        {
            var salesPersonList = _reportClient.GetMarginReportType();
            return Json(salesPersonList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStatusWithAllOption()
        {
            var statusList = new List<DropDownDto>();
            var statusIds = new long[] { (int)DTO.Enums.Status.Pending, (int)DTO.Enums.Status.Approved, (int)DTO.Enums.Status.Rejected, (int)DTO.Enums.Status.Completed };
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

        public ActionResult DepotCostReport()
        {
            ExcelReportFilterDto roleIdDto = new ExcelReportFilterDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId
            };
            return View(roleIdDto);
        }
        public async Task<ActionResult> GetDepotCostReport(DateTime fromDate, DateTime toDate, long verticalIds)
        {
            _methodName = "GetDepotCostReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "DEPOT-COST-REPORT-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";
            try
            {
                ExcelReportFilterDto inputputDto = new ExcelReportFilterDto()
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    VerticalId = verticalIds
                };
                var publishData = _reportClient.GetDepotCostReport(inputputDto);
                if (publishData != null && publishData.Any())
                {
                    var result = publishData.FirstOrDefault();
                    using (var ep = new ExcelPackage())
                    {
                        var ws = ep.Workbook.Worksheets.Add("Sheet1");
                        ws.Name = "Depot Cost Report";

                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "From Date";
                        ws.Cells["A4"].Value = "To Date";
                        ws.Cells["A5"].Value = "Total Record Count";
                        ws.Cells["A6"].Value = "Vertical";
                        for (int i = 2; i <= 6; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        ws.Cells["B2"].Value = "Depot Cost Report";
                        ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B5"].Value = publishData.Count;
                        ws.Cells["B6"].Value = verticalIds == (int)DTO.Enums.Division.Hbc ? "Hbc" : verticalIds == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        #endregion

                        int headerIndex = 8;
                        ws.Cells["A" + headerIndex].Value = "Date Of Upload";
                        ws.Cells["B" + headerIndex].Value = "Depot Code";
                        ws.Cells["C" + headerIndex].Value = "Depot Name";
                        ws.Cells["D" + headerIndex].Value = "Material Code";
                        ws.Cells["E" + headerIndex].Value = "SKU Description";
                        ws.Cells["F" + headerIndex].Value = "State Name";
                        ws.Cells["G" + headerIndex].Value = "Depot cost (Rs/CASE)";
                        ws.Cells["H" + headerIndex].Value = "Depot cost (Rs/MT)";

                        ExcelRange range = ws.Cells["A7:H7"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        int contentIndex = 9;
                        foreach (var data in publishData)
                        {
                            ws.Cells["A" + contentIndex].Value = data.DateOfUpload.ToShortDateString();
                            ws.Cells["B" + contentIndex].Value = data.DepotCode;
                            ws.Cells["C" + contentIndex].Value = data.DepotName;
                            ws.Cells["D" + contentIndex].Value = data.MaterialCode;
                            ws.Cells["E" + contentIndex].Value = data.MaterialDescription;
                            ws.Cells["F" + contentIndex].Value = data.StateName;
                            ws.Cells["G" + contentIndex].Value = data.DepotCostPerCase;
                            ws.Cells["H" + contentIndex].Value = data.DepotCostPerMT;
                            contentIndex++;
                        }
                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DetentionCostReport()
        {
            ExcelReportFilterDto roleIdDto = new ExcelReportFilterDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId
            };
            return View(roleIdDto);
        }
        public async Task<ActionResult> GetDetentionCostReport(DateTime fromDate, DateTime toDate, long verticalIds)
        {
            _methodName = "GetDetentionCostReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "DETENTION-COST-REPORT-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";
            try
            {
                ExcelReportFilterDto inputputDto = new ExcelReportFilterDto()
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    VerticalId = verticalIds
                };
                var publishData = _reportClient.GetDetentionCostReport(inputputDto);
                if (publishData != null && publishData.Any())
                {
                    var result = publishData.FirstOrDefault();
                    using (var ep = new ExcelPackage())
                    {
                        var ws = ep.Workbook.Worksheets.Add("Sheet1");
                        ws.Name = "Detention Cost Report";

                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "From Date";
                        ws.Cells["A4"].Value = "To Date";
                        ws.Cells["A5"].Value = "Total Record Count";
                        ws.Cells["A6"].Value = "Vertical";
                        for (int i = 2; i <= 6; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        ws.Cells["B2"].Value = "Detention Cost Report";
                        ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B5"].Value = publishData.Count;
                        ws.Cells["B6"].Value = verticalIds == (int)DTO.Enums.Division.Hbc ? "Hbc" : verticalIds == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        #endregion

                        int headerIndex = 8;
                        ws.Cells["A" + headerIndex].Value = "Date Of Upload";
                        ws.Cells["B" + headerIndex].Value = "Depot Code";
                        ws.Cells["C" + headerIndex].Value = "Depot Name";
                        ws.Cells["D" + headerIndex].Value = "Material Code";
                        ws.Cells["E" + headerIndex].Value = "Material Description";
                        ws.Cells["F" + headerIndex].Value = "State Name";
                        ws.Cells["G" + headerIndex].Value = "Detention cost (Rs/CASE)";
                        ws.Cells["H" + headerIndex].Value = "Detention cost (Rs/MT)";

                        ExcelRange range = ws.Cells["A7:H7"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        int contentIndex = 9;
                        foreach (var data in publishData)
                        {
                            ws.Cells["A" + contentIndex].Value = data.DateOfUpload.ToShortDateString();
                            ws.Cells["B" + contentIndex].Value = data.DepotCode;
                            ws.Cells["C" + contentIndex].Value = data.DepotName;
                            ws.Cells["D" + contentIndex].Value = data.MaterialCode;
                            ws.Cells["E" + contentIndex].Value = data.MaterialDescription;
                            ws.Cells["F" + contentIndex].Value = data.StateName;
                            ws.Cells["G" + contentIndex].Value = data.DepotCostPerCase;
                            ws.Cells["H" + contentIndex].Value = data.DepotCostPerMT;
                            contentIndex++;
                        }
                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TargetVsAchievementReport()
        {
            var result = new ExcelReportFilterDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }
        public async Task<ActionResult> GetTargetVsAchievementReport(DateTime fromDate, DateTime toDate, long verticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetTargetVsAchievementReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "TargetVsAchievement-Report-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";
            try
            {
                ExcelReportFilterDto inputputDto = new ExcelReportFilterDto()
                {
                    LoginUserId = UserId,
                    FromDate = fromDate,
                    ToDate = toDate,
                    VerticalId = verticalId,
                    SalesOrganizationId = SalesOrganizationId,
                    DistributionChannelId = DistributionChannelId
                };
                var publishData = _reportClient.GetTargetVsAchievementReport(inputputDto);
                if (publishData != null && publishData.Any())
                {
                    var result = publishData.FirstOrDefault();

                     using (var ep = new ExcelPackage())
                {
                    var ws = ep.Workbook.Worksheets.Add("TodayPricingDetails");

                    //Header
                    ws.Cells["A1:BZ1"].Style.Font.Size = 13;
                    ws.Cells["A1:BZ1"].Style.Font.Name = "Calibri";
                    ws.Cells["A1:BZ1"].Style.Font.Bold = true;

                    ws.Cells.LoadFromCollection(publishData, true);
                    ws.Cells.AutoFitColumns();
                    guidFileName = SaveExcelFileToPath(ep);
                }
                    //using (var ep = new ExcelPackage())
                    //{
                    //    var ws = ep.Workbook.Worksheets.Add("Sheet1");
                    //    ws.Name = "Target Vs Achievement Report";




                    //    #region Header
                    //    ws.Cells["A1:F1"].Merge = true;
                    //    ws.Cells["A1:F1"].Value = "Adani Wilmar Ltd.";
                    //    ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //    ws.Cells["A1:F1"].Style.Font.Bold = true;
                    //    ws.Cells["A1:F1"].Style.Font.Size = 16;

                    //    ws.Cells["A2"].Value = "Report Name";
                    //    ws.Cells["A3"].Value = "From Date";
                    //    ws.Cells["A4"].Value = "To Date";
                    //    ws.Cells["A5"].Value = "Total Record Count";

                    //    for (int i = 2; i <= 5; i++)
                    //    {
                    //        ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    //        ws.Cells["A" + i].Style.Font.Bold = true;
                    //        ws.Cells["A" + i].Style.Font.Size = 12;

                    //        ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                    //        ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    //    }

                    //    ws.Cells["B2"].Value = "Report";
                    //    ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                    //    ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                    //    ws.Cells["B5"].Value = publishData.Count;

                    //    #endregion

                    //    int headerIndex = 7;
                    //    ws.Cells["A" + headerIndex].Value = "State";
                    //    ws.Cells["B" + headerIndex].Value = "Zonal Head";
                    //    ws.Cells["C" + headerIndex].Value = "StateTrader/KAM";
                    //    ws.Cells["D" + headerIndex].Value = "Target";
                    //    ws.Cells["E" + headerIndex].Value = "Achievement";
                    //    ws.Cells["F" + headerIndex].Value = "Achievement %";

                    //    ExcelRange range = ws.Cells["A7:F7"];
                    //    range.AutoFitColumns();
                    //    range.Style.Font.Size = 12;
                    //    range.Style.Font.Bold = true;
                    //    int contentIndex = 8;
                    //    foreach (var data in publishData)
                    //    {
                    //        ws.Cells["A" + contentIndex].Value = data.StateName;
                    //        ws.Cells["B" + contentIndex].Value = data.ZonalTrader;
                    //        ws.Cells["C" + contentIndex].Value = data.BDOKAM;
                    //        ws.Cells["D" + contentIndex].Value = data.Target;
                    //        ws.Cells["E" + contentIndex].Value = data.Achievement;
                    //        ws.Cells["F" + contentIndex].Value = data.AchievementPercentage;
                    //        contentIndex++;
                    //    }
                    //    ws.Cells.AutoFitColumns();
                    //    guidFileName = SaveExcelFileToPath(ep);
                    //}
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MTPVsDSRDeviationReport()
        {
            var result = new ExcelReportFilterDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
                LoginUserId = UserId,
                OrganizationReportingToId = OrganizationReportingToId
            };
            return View(result);
        }

        public async Task<ActionResult> GetMTPVsDSRDeviationReport(DateTime fromDate, DateTime toDate, string stateIds, string BDOIds, long verticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetPCPVsMTPDeviationReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "MTPVsDSRDeviation-Report-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";
            try
            {
                ExcelReportFilterDto inputputDto = new ExcelReportFilterDto()
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    StateIds = stateIds,
                    BDOIds = BDOIds,
                    VerticalId = verticalId,
                    SalesOrganizationId = SalesOrganizationId,
                    DistributionChannelId = DistributionChannelId
                };
                var publishData = _reportClient.GetMTPVsDSRDeviationReport(inputputDto);
                if (publishData.IsAny())
                {
                    var result = publishData.FirstOrDefault();
                    using (var ep = new ExcelPackage())
                    {
                        var ws = ep.Workbook.Worksheets.Add("Sheet1");
                        ws.Name = "MTP Vs DSR Deviation Report";

                        #region Header
                        ws.Cells["A1:I1"].Merge = true;
                        ws.Cells["A1:I1"].Value = Settings.CompanyName;
                        ws.Cells["A1:I1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:I1"].Style.Font.Bold = true;
                        ws.Cells["A1:I1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "From Date";
                        ws.Cells["A4"].Value = "To Date";
                        ws.Cells["A5"].Value = "Total Record Count";

                        for (int i = 2; i <= 5; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "I" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        ws.Cells["B2"].Value = "MTP Vs DSR Deviation Report";
                        ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B5"].Value = publishData.Count;


                        #endregion



                        var resultList = publishData.Select(s => new MTPVsDSRDeviationExportDto()
                        {
                            Sno = s.Sno,
                            BDOName = s.BDOName,
                            CityName = s.CityName,
                            Month1ActualVisitCount = s.Month1ActualVisitCount,
                            Month1PlannedVisitCount = s.Month1PlannedVisitCount,
                            Month2ActualVisitCount = s.Month2ActualVisitCount,
                            Month2PlannedVisitCount = s.Month2PlannedVisitCount,
                            Month3ActualVisitCount = s.Month3ActualVisitCount,
                            Month3PlannedVisitCount = s.Month3PlannedVisitCount

                        });

                        ws.Cells["A7:I" + (7 + resultList.Count())].LoadFromCollection(resultList, true);

                        //int headerIndex = 7;
                        //ws.Cells["A" + headerIndex].Value = "Sl No";
                        //ws.Cells["B" + headerIndex].Value = "StateTrader Name";
                        //ws.Cells["C" + headerIndex].Value = "City Name";
                        //ws.Cells["D" + headerIndex].Value = "Planned Visit Count for Month - 1";
                        //ws.Cells["E" + headerIndex].Value = "Actual Visit Count for Month - 1";
                        //ws.Cells["F" + headerIndex].Value = "Planned Visit Count for Month - 2";
                        //ws.Cells["G" + headerIndex].Value = "Actual Visit Count for Month - 2";
                        //ws.Cells["H" + headerIndex].Value = "Planned Visit Count for Month - 3";
                        //ws.Cells["I" + headerIndex].Value = "Actual Visit Count for Month - 3";

                        ExcelRange range = ws.Cells["A7:I7"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        //int contentIndex = 8;
                        //foreach (var data in publishData)
                        //{
                        //    ws.Cells["A" + contentIndex].Value = data.Sno;
                        //    ws.Cells["B" + contentIndex].Value = data.BDOName;
                        //    ws.Cells["C" + contentIndex].Value = data.CityName;
                        //    ws.Cells["D" + contentIndex].Value = data.Month1PlannedVisitCount;
                        //    ws.Cells["E" + contentIndex].Value = data.Month1ActualVisitCount;
                        //    ws.Cells["F" + contentIndex].Value = data.Month2PlannedVisitCount;
                        //    ws.Cells["G" + contentIndex].Value = data.Month2ActualVisitCount;
                        //    ws.Cells["H" + contentIndex].Value = data.Month3PlannedVisitCount;
                        //    ws.Cells["I" + contentIndex].Value = data.Month3ActualVisitCount;
                        //    contentIndex++;
                        //}
                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PCPVsMTPDeviationReport()
        {
            var result = new ExcelReportFilterDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
                LoginUserId = UserId,
                OrganizationReportingToId = OrganizationReportingToId
            };
            return View(result);
        }

        public async Task<ActionResult> GetPCPVsMTPDeviationReport(DateTime fromDate, DateTime toDate, string stateIds, string BDOIds, long verticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetPCPVsMTPDeviationReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "PCPVsMTPDeviation-Report-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";
            try
            {
                ExcelReportFilterDto inputputDto = new ExcelReportFilterDto()
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    StateIds = stateIds,
                    BDOIds = BDOIds,
                    VerticalId = verticalId,
                    SalesOrganizationId = SalesOrganizationId,
                    DistributionChannelId = DistributionChannelId

                };
                var publishData = _reportClient.GetPCPVsMTPDeviationReport(inputputDto);
                if (publishData.IsAny())
                {
                    var result = publishData.FirstOrDefault();
                    using (var ep = new ExcelPackage())
                    {
                        var ws = ep.Workbook.Worksheets.Add("Sheet1");
                        ws.Name = "PCP vs MTP Deviation Report";



                        #region Header
                        ws.Cells["A1:I1"].Merge = true;
                        ws.Cells["A1:I1"].Value = Settings.CompanyName;
                        ws.Cells["A1:I1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:I1"].Style.Font.Bold = true;
                        ws.Cells["A1:I1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "From Date";
                        ws.Cells["A4"].Value = "To Date";
                        ws.Cells["A5"].Value = "Total Record Count";

                        var resultList = publishData.Select(s => new MTPVsDSRDeviationExportDto()
                        { 
                            Sno=s.Sno,
                            BDOName=s.BDOName,
                            CityName=s.CityName,
                            Month1ActualVisitCount=s.Month1ActualVisitCount,
                            Month1PlannedVisitCount=s.Month1PlannedVisitCount,
                            Month2ActualVisitCount=s.Month2ActualVisitCount,
                            Month2PlannedVisitCount=s.Month2PlannedVisitCount,
                            Month3ActualVisitCount=s.Month3ActualVisitCount,
                            Month3PlannedVisitCount=s.Month3PlannedVisitCount
                        
                        });

                        for (int i = 2; i <= 5; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "I" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        ws.Cells["B2"].Value = "PCP Vs MTP Deviation Report";
                        ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B5"].Value = publishData.Count;

                        #endregion


                        ws.Cells["A7:I" + (7 + publishData.Count)].LoadFromCollection(resultList, true);

                        ExcelRange range = ws.Cells["A7:I7"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;

                        //int headerIndex = 7;
                        //ws.Cells["A" + headerIndex].Value = "Sl No";
                        //ws.Cells["B" + headerIndex].Value = "StateTrader Name";
                        //ws.Cells["C" + headerIndex].Value = "City Name";
                        //ws.Cells["D" + headerIndex].Value = "Planned Visit Count for Month - 1";
                        //ws.Cells["E" + headerIndex].Value = "Actual Visit Count for Month - 1";
                        //ws.Cells["F" + headerIndex].Value = "Planned Visit Count for Month - 2";
                        //ws.Cells["G" + headerIndex].Value = "Actual Visit Count for Month - 2";
                        //ws.Cells["H" + headerIndex].Value = "Planned Visit Count for Month - 3";
                        //ws.Cells["I" + headerIndex].Value = "Actual Visit Count for Month - 3";

                        //ExcelRange range = ws.Cells["A7:I7"];
                        //range.AutoFitColumns();
                        //range.Style.Font.Size = 12;
                        //range.Style.Font.Bold = true;
                        //int contentIndex = 8;
                        //foreach (var data in publishData)
                        //{
                        //    ws.Cells["A" + contentIndex].Value = data.Sno;
                        //    ws.Cells["B" + contentIndex].Value = data.BDOName;
                        //    ws.Cells["C" + contentIndex].Value = data.CityName;
                        //    ws.Cells["D" + contentIndex].Value = data.Month1PlannedVisitCount;
                        //    ws.Cells["E" + contentIndex].Value = data.Month1ActualVisitCount;
                        //    ws.Cells["F" + contentIndex].Value = data.Month2PlannedVisitCount;
                        //    ws.Cells["G" + contentIndex].Value = data.Month2ActualVisitCount;
                        //    ws.Cells["H" + contentIndex].Value = data.Month3PlannedVisitCount;
                        //    ws.Cells["I" + contentIndex].Value = data.Month3ActualVisitCount;
                        //    contentIndex++;
                        //}
                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CompetitorSaleReport()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }
        public async Task<ActionResult> GetCompetitorSaleReport(DateTime fromDate, DateTime toDate, long verticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetCompetitorSaleReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "CompetitorRate-Report-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";
            try
            {
                ExcelReportFilterDto inputputDto = new ExcelReportFilterDto()
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    VerticalId = VerticalId,
                    SalesOrganizationId = SalesOrganizationId,
                    DistributionChannelId = DistributionChannelId
                };
                var publishData = _reportClient.GetCompetitorRateReport(inputputDto);
                if (publishData.Rows.Count != 0)
                {
                    using (var ep = new ExcelPackage())
                    {
                        var ws = ep.Workbook.Worksheets.Add("Sheet1");
                        ws.Name = "Competitor Sale Report";

                        #region Header
                        ws.Cells["A1:I1"].Merge = true;
                        ws.Cells["A1:I1"].Value = Settings.CompanyName;
                        ws.Cells["A1:I1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:I1"].Style.Font.Bold = true;
                        ws.Cells["A1:I1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "From Date";
                        ws.Cells["A4"].Value = "To Date";
                        ws.Cells["A5"].Value = "Total Record Count";

                        for (int i = 2; i <= 5; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "I" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        ws.Cells["B2"].Value = "Daily Status Report";
                        ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B5"].Value = publishData.Rows.Count;

                        #endregion
                        int headerIndex = 7;

                        publishData.Columns.Remove("StateId");
                        publishData.Columns.Remove("ProductId");

                        ws.Cells["A7:E" + (7 + publishData.Rows.Count)].LoadFromDataTable(publishData, true);

                        //string[] Alphabets = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                        //int AlphabetIndex = 0;
                        //foreach (DataColumn column in publishData.Columns)
                        //{
                        //    ws.Cells[Alphabets[AlphabetIndex] + headerIndex].Value = column.ColumnName;
                        //    AlphabetIndex = AlphabetIndex + 1;
                        //}

                        //
                        ExcelRange range = ws.Cells["A7:E7"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;

                        //
                        //int contentIndex = 8;
                        //foreach (DataRow data in publishData.Rows)
                        //{
                        //    int AlphabetIndex1 = 0;
                        //    foreach (DataColumn column in publishData.Columns)
                        //    {
                        //        ws.Cells[Alphabets[AlphabetIndex1] + contentIndex].Value = data[column.ColumnName];
                        //        AlphabetIndex1 = AlphabetIndex1 + 1;
                        //    }
                        //    contentIndex++;
                        //}
                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DailyStatusReport()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }
        public async Task<ActionResult> GetDailySalesReport(DateTime fromDate, DateTime toDate, long verticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetDailySalesReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "DailySales-Report-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";
            try
            {
                ExcelReportFilterDto inputputDto = new ExcelReportFilterDto()
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    VerticalId = verticalId,
                    SalesOrganizationId = SalesOrganizationId,
                    DistributionChannelId = DistributionChannelId
                };
                var publishData = _reportClient.GetDailyStatusReport(inputputDto);
                if (publishData != null && publishData.Rows.Count > 0)
                {
                    using (var ep = new ExcelPackage())
                    {
                        var ws = ep.Workbook.Worksheets.Add("Sheet1");
                        ws.Name = "Daily Status Report";

                        #region Header
                        ws.Cells["A1:I1"].Merge = true;
                        ws.Cells["A1:I1"].Value = Settings.CompanyName;
                        ws.Cells["A1:I1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:I1"].Style.Font.Bold = true;
                        ws.Cells["A1:I1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "From Date";
                        ws.Cells["A4"].Value = "To Date";
                        ws.Cells["A5"].Value = "Total Record Count";

                        for (int i = 2; i <= 5; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "I" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        ws.Cells["B2"].Value = "Daily Status Report";
                        ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B5"].Value = publishData.Rows.Count;

                        #endregion
                        int headerIndex = 7;
                        string[] Alphabets = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                        int AlphabetIndex = 0;
                        foreach (DataColumn column in publishData.Columns)
                        {
                            ws.Cells[Alphabets[AlphabetIndex] + headerIndex].Value = column.ColumnName;
                            AlphabetIndex = AlphabetIndex + 1;
                        }

                        ExcelRange range = ws.Cells["A7:I7"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        int contentIndex = 8;
                        //foreach (var data in publishData.Rows)
                        //{
                        //    ws.Cells["A" + contentIndex].Value = data.Sno;
                        //    ws.Cells["B" + contentIndex].Value = data.BDOName;
                        //    ws.Cells["C" + contentIndex].Value = data.CityName;
                        //    ws.Cells["D" + contentIndex].Value = data.Month1PlannedVisitCount;
                        //    ws.Cells["E" + contentIndex].Value = data.Month1ActualVisitCount;
                        //    ws.Cells["F" + contentIndex].Value = data.Month2PlannedVisitCount;
                        //    ws.Cells["G" + contentIndex].Value = data.Month2ActualVisitCount;
                        //    ws.Cells["H" + contentIndex].Value = data.Month3PlannedVisitCount;
                        //    ws.Cells["I" + contentIndex].Value = data.Month3ActualVisitCount;
                        //    contentIndex++;
                        //}
                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PriceReleaseAuditReport()
        {
            ExcelReportFilterDto roleIdDto = new ExcelReportFilterDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId
            };
            return View(roleIdDto);
        }
        public async Task<ActionResult> GetPriceReleaseAuditReport(DateTime fromDate, DateTime toDate, string verticalId, long plantId, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetPriceReleaseAuditReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "PriceReleaseAudit-Report-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";
            try
            {
                long tempVerticalId = verticalId == "" ? 0 : Convert.ToInt32(verticalId);
                ExcelReportFilterDto inputputDto = new ExcelReportFilterDto()
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    VerticalId = tempVerticalId,
                    PlantId = plantId,
                    SalesOrganizationId = SalesOrganizationId,
                    DistributionChannelId = DistributionChannelId
                };
                var publishData = _reportClient.GetPriceReleaseAuditReport(inputputDto);
                if (publishData != null && publishData.Any())
                {
                    using (var ep = new ExcelPackage())
                    {
                        var ws = ep.Workbook.Worksheets.Add("Sheet1");
                        ws.Name = "Price Release Audit Report";

                        #region Header
                        ws.Cells["A1:I1"].Merge = true;
                        ws.Cells["A1:I1"].Value = Settings.CompanyName;
                        ws.Cells["A1:I1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:I1"].Style.Font.Bold = true;
                        ws.Cells["A1:I1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "From Date";
                        ws.Cells["A4"].Value = "To Date";
                        ws.Cells["A5"].Value = "Total Record Count";
                        ws.Cells["A6"].Value = "Vertical";
                        for (int i = 2; i <= 6; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "I" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        ws.Cells["B2"].Value = "Price Release Audit Report";
                        ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B5"].Value = publishData.Count;
                        ws.Cells["B6"].Value = tempVerticalId == (int)DTO.Enums.Division.Hbc ? "Hbc" : tempVerticalId == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        #endregion
                        int headerIndex = 8;

                        ws.Cells["A" + headerIndex].Value = "Date";
                        ws.Cells["B" + headerIndex].Value = "State Name";
                        ws.Cells["C" + headerIndex].Value = "Vertical";
                        ws.Cells["D" + headerIndex].Value = "Plant";
                        ws.Cells["E" + headerIndex].Value = "Oil Type";
                        ws.Cells["F" + headerIndex].Value = "Material Cost Update time";
                        ws.Cells["G" + headerIndex].Value = "Price Generate time";
                        ws.Cells["H" + headerIndex].Value = "Price Release time";
                        ws.Cells["I" + headerIndex].Value = "Time Gap between Cost upload and Generate";
                        ws.Cells["J" + headerIndex].Value = "Time Gap between Cost upload and release";
                        ws.Cells["K" + headerIndex].Value = "Time gap between Generate and release";

                        ExcelRange range = ws.Cells["A7:K7"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        int contentIndex = 9;
                        foreach (var data in publishData)
                        {
                            ws.Cells["A" + contentIndex].Value = data.Date.ToShortDateString();
                            ws.Cells["B" + contentIndex].Value = data.StateName;
                            ws.Cells["C" + contentIndex].Value = data.Vertical;
                            ws.Cells["D" + contentIndex].Value = data.Plant;
                            ws.Cells["E" + contentIndex].Value = data.OilType;
                            ws.Cells["F" + contentIndex].Value = data.MaterialCostUpdateTime;
                            ws.Cells["G" + contentIndex].Value = data.PriceGenerateTime;
                            ws.Cells["H" + contentIndex].Value = data.PriceReleaseTime;
                            ws.Cells["I" + contentIndex].Value = data.TimeGapCostuploadandGenerate;
                            ws.Cells["J" + contentIndex].Value = data.TimeGapCostuploadandRelease;
                            ws.Cells["K" + contentIndex].Value = data.TimegapGenerateandrelease;
                            contentIndex++;
                        }
                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaudaExecutionAuditReport()
        {
            ExcelReportFilterDto roleIdDto = new ExcelReportFilterDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId
            };
            return View(roleIdDto);
        }

        public async Task<ActionResult> GetSaudaExecutionAuditReport(DateTime fromDate, DateTime toDate, string verticalId, long plantId, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetSaudaExecutionAuditReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "SaudaExecution-Report-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";
            try
            {
                long tempVerticalId = verticalId == "" ? 0 : Convert.ToInt32(verticalId);
                ExcelReportFilterDto inputputDto = new ExcelReportFilterDto()
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    VerticalId = tempVerticalId,
                    PlantId = plantId,
                    SalesOrganizationId = SalesOrganizationId,
                    DistributionChannelId = DistributionChannelId
                };
                var publishData = _reportClient.GetSaudaExecutionAuditReport(inputputDto);
                if (publishData != null && publishData.Any())
                {
                    using (var ep = new ExcelPackage())
                    {
                        var ws = ep.Workbook.Worksheets.Add("Sheet1");
                        ws.Name = "Sauda Execution Audit Report";

                        #region Header
                        ws.Cells["A1:I1"].Merge = true;
                        ws.Cells["A1:I1"].Value = Settings.CompanyName;
                        ws.Cells["A1:I1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:I1"].Style.Font.Bold = true;
                        ws.Cells["A1:I1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "From Date";
                        ws.Cells["A4"].Value = "To Date";
                        ws.Cells["A5"].Value = "Total Record Count";
                        ws.Cells["A6"].Value = "Vertical";
                        for (int i = 2; i <= 6; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "I" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        ws.Cells["B2"].Value = "Sauda Execution Audit Report";
                        ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B5"].Value = publishData.Count;
                        ws.Cells["B6"].Value = tempVerticalId == (int)DTO.Enums.Division.Hbc ? "Hbc" : tempVerticalId == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        #endregion
                        int headerIndex = 8;

                        ws.Cells["A" + headerIndex].Value = "App Booking Id";
                        ws.Cells["B" + headerIndex].Value = "Plant";
                        ws.Cells["C" + headerIndex].Value = "Division";
                        ws.Cells["D" + headerIndex].Value = "Sauda Number";
                        ws.Cells["E" + headerIndex].Value = "Sauda Booked By";
                        ws.Cells["F" + headerIndex].Value = "SKU Code";
                        ws.Cells["G" + headerIndex].Value = "Date of sauda Booking in app";
                        ws.Cells["H" + headerIndex].Value = "Time of sauda Booking in app";
                        ws.Cells["I" + headerIndex].Value = "TT Creation Date";
                        ws.Cells["J" + headerIndex].Value = "TT Creation Time";
                        ws.Cells["K" + headerIndex].Value = "Sauda - TT attached date";
                        ws.Cells["L" + headerIndex].Value = "Sauda - TT attached time";
                        ws.Cells["M" + headerIndex].Value = "Sauda creation date";
                        ws.Cells["N" + headerIndex].Value = "Sauda creation time";
                        ws.Cells["O" + headerIndex].Value = "Sauda Release date";
                        ws.Cells["P" + headerIndex].Value = "Sauda Release time";
                        ws.Cells["Q" + headerIndex].Value = "Time laspse between Sauda booking and sauda release";

                        ExcelRange range = ws.Cells["A7:Q7"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        int contentIndex = 9;
                        foreach (var data in publishData)
                        {
                            ws.Cells["A" + contentIndex].Value = data.AppBookingId;
                            ws.Cells["B" + contentIndex].Value = data.Plant;
                            ws.Cells["C" + contentIndex].Value = data.Division;
                            ws.Cells["D" + contentIndex].Value = data.SaudaNumber;
                            ws.Cells["E" + contentIndex].Value = data.SaudaBookedBy;
                            ws.Cells["F" + contentIndex].Value = data.SkuCode;
                            ws.Cells["G" + contentIndex].Value = data.SaudaBookingDate == null ? "" : Convert.ToDateTime(data.SaudaBookingDate).ToShortDateString();
                            ws.Cells["H" + contentIndex].Value = data.SaudaBookingTime;
                            ws.Cells["I" + contentIndex].Value = data.TradeTicketDate == null ? "" : Convert.ToDateTime(data.TradeTicketDate).ToShortDateString();
                            ws.Cells["J" + contentIndex].Value = data.TradeTicketTime;
                            ws.Cells["K" + contentIndex].Value = data.SaudaTTAttachedDate == null ? "" : Convert.ToDateTime(data.SaudaTTAttachedDate).ToShortDateString();
                            ws.Cells["L" + contentIndex].Value = data.SaudaTTAttachedTime;
                            ws.Cells["M" + contentIndex].Value = data.SaudaCreationDate == null ? "" : Convert.ToDateTime(data.SaudaCreationDate).ToShortDateString();
                            ws.Cells["N" + contentIndex].Value = data.SaudaCreationTime;
                            ws.Cells["O" + contentIndex].Value = data.SaudaReleaseDate == null ? "" : Convert.ToDateTime(data.SaudaReleaseDate).ToShortDateString();
                            ws.Cells["P" + contentIndex].Value = data.SaudaReleaseTime;
                            ws.Cells["Q" + contentIndex].Value = data.TimeGapSaudabookingandrelease;
                            contentIndex++;
                        }
                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sauda Limit

        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult SaudaLimitReport()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
                LoginUserId = UserId,
                OrganizationReportingToId = OrganizationReportingToId
            };
            return View(result);
        }

        public async Task<ActionResult> SaudaLimitExportAsync(List<long> stateIds, long divisionId, string dealerCode, string zhId, string bdoId, long salesorganizationId, long distributionChannelId)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                var zhead = zhId.Split(',').Select(long.Parse).ToList();
                var bdo = bdoId.Split(',').Select(long.Parse).ToList();
                var zh = zhead.SingleOrDefault(r => r == 0);
                zhead.Remove(zh);
                var StateTrader = bdo.SingleOrDefault(r => r == 0);
                bdo.Remove(StateTrader);

               
                var inputDto = new ReportFilterDto()
                {
                    LoginUserId=UserId,
                    RoleId=RoleId,
                    zhId = zhead,
                    bdoId = bdo,
                    StateIds = stateIds,
                    DivisionId = divisionId,
                    dealerCode = dealerCode,
                    SalesOrganizationId = salesorganizationId,
                    DistributionChannelId = distributionChannelId
                };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<SaudaLimitDto> saudaLimitList = new List<SaudaLimitDto>();
                saudaLimitList = await _reportClient.SaudaLimitExportAsync(inputDto);

                DateTime currentDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                fileName = "CONTRACT-LIMIT-" + string.Format("{0:dd-MMM-yyyy}", currentDate.Date) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

                if (saudaLimitList.IsAny())
                {



                    var resultList = saudaLimitList.Select(s => new SaudaLimitExportDto()
                    { 
                        CustomerCode=s.CustomerCode,
                        CustomerName=s.Name,
                        State=s.State,
                        Empoloyee=s.Employee,
                        SaudaOrderQuantity=s.SaudaOrderQtyCase,
                        PendingContratQuantity=s.PendingContractQtyCase,
                        ContractLimt=s.SaudaLimit,
                        SaudaOrderQuantityInMt=s.SaudaOrderQty,
                        PendingContractInMt=s.PendingContractQty,
                        AvailableContractInMt=s.AvailableSaudaLimit,
                        SalesOrganization=s.SalesOrganizationName,
                        DistributionChannel=s.DistributionChannelName,
                        Division=s.DivisionName
                    
                    }).ToList();
                    using (var package = new ExcelPackage())
                    {
                        // add a new worksheet to the empty workbook
                        var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        Response.ClearHeaders();
                        Response.ClearContent();
                        Response.Clear();
                        #region Header
                        worksheet.Cells["A1:F1"].Merge = true;
                        worksheet.Cells["A1:F1"].Value = Settings.CompanyName;
                        worksheet.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1:F1"].Style.Font.Bold = true;
                        worksheet.Cells["A1:F1"].Style.Font.Size = 16;

                        worksheet.Cells["A2"].Value = "Report Name";
                        worksheet.Cells["A3"].Value = "Total Record Count";
                        for (int i = 2; i <= 4; i++)
                        {
                            worksheet.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheet.Cells["A" + i].Style.Font.Bold = true;
                            worksheet.Cells["A" + i].Style.Font.Size = 12;

                            worksheet.Cells["B" + i + ":" + "F" + i].Merge = true;
                            worksheet.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        worksheet.Cells["B2"].Value = "ContractLimitReport";
                        worksheet.Cells["B3"].Value = saudaLimitList.Count;
                        //worksheet.Cells["B4"].Value = inputDto.verticalIds == (int)DTO.Enums.Division.Hbc ? "Hbc" : inputDto.verticalIds == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        #endregion

                        worksheet.Cells["A6:M" + (6 + saudaLimitList.Count)].LoadFromCollection(resultList, true);

                        worksheet.Cells["A6:M6"].Style.Font.Size = 13;
                        worksheet.Cells["A6:M6"].Style.Font.Name = "Calibri";
                        worksheet.Cells["A6:M6"].Style.Font.Bold = true;

                        //var rowIndex = 6;
                        //var colIndex = 1;
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CustomerCode"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CustomerName"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_State"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Employee"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Sauda Order Quantity");
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Pending Contract Quantity");
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SaudaLimit"));
                        ////GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PendingContract"));
                        ////GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PendingDO"));
                        ////GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PendingOBD"));
                        ////GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Pending Quantity In Portal");
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Sauda Order Quantity In MT");
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Pending Contract Quantity In MT");
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_AvailableSaudaLimit"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Sales Organization");
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Distribution Channel");
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Division");

                        ////To set top row as static
                        //worksheet.View.FreezePanes(2, 1);
                        ////To implement filters
                        //worksheet.Cells["A1:AQ1"].AutoFilter = true;

                        //if (saudaLimitList != null && saudaLimitList.Any())
                        //{
                        //    foreach (var saudaLimit in saudaLimitList)
                        //    {
                        //        rowIndex++;
                        //        colIndex = 1;
                        //        if (saudaLimit != null)
                        //        {
                        //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaLimit.CustomerCode != null ? saudaLimit.CustomerCode.ToString() : string.Empty);
                        //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaLimit.Name != null ? saudaLimit.Name.ToString() : string.Empty);
                        //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaLimit.State.ToString());
                        //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaLimit.Employee != null ? saudaLimit.Employee.ToString() : string.Empty);
                        //            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaLimit.PendingContract.ToString());
                        //            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaLimit.PendingDO.ToString());
                        //            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaLimit.PendingOBD.ToString());
                        //            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaLimit.PendingQuantityInPortal.ToString());
                        //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaLimit.SaudaOrderQtyCase.ToString());
                        //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaLimit.PendingContractQtyCase.ToString());
                        //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaLimit.SaudaLimit.ToString());
                        //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaLimit.SaudaOrderQty.ToString());
                        //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaLimit.PendingContractQty.ToString());
                        //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaLimit.AvailableSaudaLimit.ToString());
                        //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaLimit.SalesOrganizationName.ToString());
                        //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaLimit.DistributionChannelName.ToString());
                        //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudaLimit.DivisionName.ToString());
                        //        }


                        //    }
                        //}

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
                    return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
                }

                // Create the package and make sure you wrap it in a using statement
               

            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Sales Report
        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult SalesReport()
        {
            return View();
        }

        public async Task<ActionResult> SalesExportAsync(SalesReportInputDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {

                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<SalesBDOWiseReportDto> salesList = new List<SalesBDOWiseReportDto>();
                salesList = await _reportClient.SalesExportAsync(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "SALES-REPORT" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
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
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SalesPersonCode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SalesPersonName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PartyCode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PartyName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OilType"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BPInMT"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BPInCase"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CPInMT"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CPInCase"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_TotalSalesInMT"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_TotalSalesInCase"));

                    ////To set top row as static
                    //worksheet.View.FreezePanes(2, 1);
                    ////To implement filters
                    //worksheet.Cells["A1:AQ1"].AutoFilter = true;

                    if (salesList != null && salesList.Any())
                    {
                        var bdoList = salesList.Select(_ => _.BDOCode).Distinct().ToList();
                        foreach (var StateTrader in bdoList)
                        {
                            var bdoWiseSalesList = salesList.Where(_ => _.BDOCode == StateTrader);
                            foreach (var sales in bdoWiseSalesList)
                            {
                                rowIndex++;
                                colIndex = 1;
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.BDOCode != null ? sales.BDOCode.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.BDOName != null ? sales.BDOName.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.DealerCode != null ? sales.DealerCode.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.DealerName != null ? sales.DealerName.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.OilTypeName != null ? sales.OilTypeName.ToString() : string.Empty);
                                //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.BPInMT != null ? sales.BPInMT.ToString() : string.Empty);
                                //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.BPInCase != null ? sales.BPInCase.ToString() : string.Empty);
                                //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.CPInMT != null ? sales.CPInMT.ToString() : string.Empty);
                                //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.CPInCase != null ? sales.CPInCase.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.TotalSalesInMT != null ? sales.TotalSalesInMT.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.TotalSalesInCase != null ? sales.TotalSalesInCase.ToString() : string.Empty);
                            }
                            rowIndex++;
                            colIndex = 6;
                            worksheet.Cells["A" + rowIndex.ToString() + ":E" + rowIndex.ToString()].Merge = true;
                            worksheet.Cells["A" + rowIndex.ToString() + ":E" + rowIndex.ToString()].Value = @Helper.GetResourceString("lbl_SubTotal");
                            worksheet.Cells["A" + rowIndex.ToString() + ":E" + rowIndex.ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells["A" + rowIndex.ToString() + ":K" + rowIndex.ToString()].Style.Font.Bold = true;
                            worksheet.Cells["A" + rowIndex.ToString() + ":K" + rowIndex.ToString()].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells["A" + rowIndex.ToString() + ":K" + rowIndex.ToString()].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], bdoWiseSalesList.Select(_ => _.BPInMT != null ? _.BPInMT : 0).Sum().ToString());
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], bdoWiseSalesList.Select(_ => _.BPInCase != null ? _.BPInCase : 0).Sum().ToString());
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], bdoWiseSalesList.Select(_ => _.CPInMT != null ? _.CPInMT : 0).Sum().ToString());
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], bdoWiseSalesList.Select(_ => _.CPInCase != null ? _.CPInCase : 0).Sum().ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], bdoWiseSalesList.Select(_ => _.TotalSalesInMT != null ? _.TotalSalesInMT : 0).Sum().ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], bdoWiseSalesList.Select(_ => _.TotalSalesInCase != null ? _.TotalSalesInCase : 0).Sum().ToString());
                        }
                        rowIndex++;
                        colIndex = 6;
                        worksheet.Cells["A" + rowIndex.ToString() + ":E" + rowIndex.ToString()].Merge = true;
                        worksheet.Cells["A" + rowIndex.ToString() + ":E" + rowIndex.ToString()].Value = @Helper.GetResourceString("lbl_GrandTotal");
                        worksheet.Cells["A" + rowIndex.ToString() + ":E" + rowIndex.ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A" + rowIndex.ToString() + ":K" + rowIndex.ToString()].Style.Font.Bold = true;
                        worksheet.Cells["A" + rowIndex.ToString() + ":K" + rowIndex.ToString()].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells["A" + rowIndex.ToString() + ":K" + rowIndex.ToString()].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], salesList.Select(_ => _.BPInMT != null ? _.BPInMT : 0).Sum().ToString());
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], salesList.Select(_ => _.BPInCase != null ? _.BPInCase : 0).Sum().ToString());
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], salesList.Select(_ => _.CPInMT != null ? _.CPInMT : 0).Sum().ToString());
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], salesList.Select(_ => _.CPInCase != null ? _.CPInCase : 0).Sum().ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], salesList.Select(_ => _.TotalSalesInMT != null ? _.TotalSalesInMT : 0).Sum().ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], salesList.Select(_ => _.TotalSalesInCase != null ? _.TotalSalesInCase : 0).Sum().ToString());
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

        #region IndentList/ Lifting list Report

        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult IndentReport()
        {
            IndentReportInputDto roleIdDto = new IndentReportInputDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId
            };
            return View(roleIdDto);
        }

        public async Task<ActionResult> IndentReportExportAsync(IndentReportInputDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto.LoginUserId = UserId;
                inputDto.RoleId = RoleId;
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<LiftingListReportDto> resultList = new List<LiftingListReportDto>();
                resultList = _reportClient.IndentReportExport(inputDto);

                if (resultList != null && resultList.Any())
                {
                    DateTime currentDate = DateTime.Now;
                    fileName = "SALES_ORDER" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                    guidFileName = $"{Guid.NewGuid()}.xlsx";

                    // Create the package and make sure you wrap it in a using statement
                    using (var package = new ExcelPackage())
                    {
                        //// add a new worksheet to the empty workbook
                        //var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        //Response.ClearHeaders();
                        //Response.ClearContent();
                        //Response.Clear();
                        //var rowIndex = 1;
                        //var colIndex = 1;

                        var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        //var rowIndex = 8;
                        //var colIndex = 1;

                        #region Header

                        worksheet.Cells["A1:M1"].Merge = true;
                        worksheet.Cells["A1:M1"].Value = Settings.CompanyName;
                        worksheet.Cells["A1:M1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1:M1"].Style.Font.Bold = true;
                        worksheet.Cells["A1:M1"].Style.Font.Size = 16;

                        worksheet.Cells["A2"].Value = "Report Name";
                        worksheet.Cells["A3"].Value = "From date";
                        worksheet.Cells["A4"].Value = "To Date";
                        //  worksheet.Cells["A5"].Value = "Vertical";
                        worksheet.Cells["A5"].Value = "Status";


                        string statusName = inputDto.StatusId == -1
                            ? Utility.GetEnumFromString<Status>((int)Status.Pending, (int)Status.Approved)
                            : Utility.GetEnumFromString<Status>(inputDto.StatusId); ;

                        var displayText = string.Empty;
                        if (inputDto.IsAfterDeliverOrderNumber)
                        {
                            displayText = "Sales Order report after creation of Delivery Order No.";
                        }
                        else
                        {
                            displayText = "Sales Order report before creation of Delivery Order No.";
                        }

                        worksheet.Cells["B2"].Value = "Sales Order Details";
                        worksheet.Cells["B3"].Value = string.Format(Settings.ReportDateFormat, inputDto.StartDate);
                        worksheet.Cells["B4"].Value = string.Format(Settings.ReportDateFormat, inputDto.EndDate);
                        //  worksheet.Cells["B5"].Value = inputDto.verticalIds == (int)DTO.Enums.Division.Hbc ? "Hbc" : inputDto.verticalIds == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        worksheet.Cells["B5"].Value = displayText;
                        worksheet.Cells["B5"].Style.Font.Bold = true;

                        for (int i = 2; i <= 5; i++)
                        {
                            worksheet.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheet.Cells["A" + i].Style.Font.Bold = true;
                            worksheet.Cells["A" + i].Style.Font.Size = 12;

                            worksheet.Cells["B" + i + ":" + "F" + i].Merge = true;
                            worksheet.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        #endregion

                        #region Data
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IndentReceivedDate"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IndentReceivedTime"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Sales Order Request Number");
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BDOName"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DealerCode"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DealerName"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ShipToPartyCode"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ShipToPartyName"));
                        ////GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Destination"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_State"));
                        ////GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_VehicleSize"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_QuantityPerMT"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_QuantityPerCase"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SkuCode"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SkuName"));
                        ////GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_GrossWeight"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Sales Order Number");
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Plant Name");
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Contract Number");
                        ////if (inputDto.IsAfterDeliverOrderNumber)
                        ////{
                        ////    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Inquiry Number");

                        ////}
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DeliveryOrderNumber"));
                        ////GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Status1"));
                        ////GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Status2"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Status"));


                        //if (resultList != null && resultList.Any())
                        //{
                        //    foreach (var item in resultList)
                        //    {
                        //        rowIndex++;
                        //        colIndex = 1;
                        //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IndentReceivedDate != null ? string.Format(Settings.GridDateFormat, item.IndentReceivedDate).ToString() : string.Empty);
                        //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IndentReceivedTime != null ? item.IndentReceivedTime : string.Empty);
                        //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IndentNo != null ? item.IndentNo.ToString() : string.Empty);
                        //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.BDOName != null ? item.BDOName.ToString() : string.Empty);
                        //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.DealerCode != null ? item.DealerCode.ToString() : string.Empty);
                        //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.DealerName != null ? item.DealerName.ToString() : string.Empty);
                        //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ShipToPartyCode != null ? item.ShipToPartyCode.ToString() : string.Empty);
                        //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ShipToPartyName != null ? item.ShipToPartyName.ToString() : string.Empty);
                        //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Destination != null ? item.Destination.ToString() : string.Empty);
                        //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.State != null ? item.State.ToString() : string.Empty);
                        //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.VehicleSize.ToString());
                        //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.LiftingQuantityInMT.ToString());
                        //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.LiftingQuantityCase.ToString());
                        //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.SkuCode.ToString());
                        //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.SkuName.ToString());
                        //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.GrossWeight.ToString());
                        //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.InquiryNumber != null ? item.InquiryNumber.ToString() : string.Empty);
                        //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.PlantOrDepotName != null ? item.PlantOrDepotName.ToString() : string.Empty);
                        //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ContractNumber != null ? item.ContractNumber.ToString() : string.Empty);
                        //        //if (inputDto.IsAfterDeliverOrderNumber)
                        //        //{
                        //        //    //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.InquiryNumber != null ? item.InquiryNumber.ToString() : string.Empty);

                        //        //}
                        //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.DeliveryOrderNumber != null ? item.DeliveryOrderNumber.ToString() : string.Empty);
                        //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Status1 != null ? item.Status1.ToString() : string.Empty);
                        //        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Status2 != null ? item.Status2.ToString() : string.Empty);
                        //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Status != null ? item.Status.ToString() : string.Empty);
                        //    }
                        //}

                        //foreach (var workSheet in package.Workbook.Worksheets)
                        //{
                        //    for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        //    {
                        //        try
                        //        {
                        //            workSheet.Column(i).AutoFit();
                        //            workSheet.Column(i).BestFit = true;
                        //        }
                        //        catch { }
                        //    }

                        //    try
                        //    {
                        //        var cells = workSheet.Cells[workSheet.Dimension.Address];
                        //        cells.AutoFitColumns();
                        //    }
                        //    catch { }
                        //}
                        ////this.Response.Headers.Clear();

                        #endregion Data

                        var newResult = resultList.Select(_ => new IndentReportDto()
                        {
                            BDOName = _.BDOName,
                            ContractNumber = _.ContractNumber,
                            DealerCode = _.DealerCode,
                            DealerName = _.DealerName,
                            DeliveryOrderNumber = _.DeliveryOrderNumber,
                            IndentNo = _.IndentNo,
                            IndentReceivedDate = _.IndentReceivedDate.ToString("dd-MM-yyyy hh:mm:ss"),
                            IndentReceivedTime = _.IndentReceivedTime,
                            InquiryNumber = _.InquiryNumber,
                            LiftingQuantityCase = _.LiftingQuantityCase,
                            LiftingQuantityInMT = _.LiftingQuantityInMT,
                            PlantOrDepotName = _.PlantOrDepotName,
                            ShipToPartyCode = _.ShipToPartyCode,
                            ShipToPartyName = _.ShipToPartyName,
                            SkuCode = _.SkuCode,
                            SkuName = _.SkuName,
                            State = _.State,
                            Status = _.Status,
                            CreatedBy=_.CreatedByName,
                            IsSapSalesOrder=_.IsSAPSalesOrder
                        });


                        //worksheet.Cells["A6:M" + (6 + saudaLimitList.Count)].LoadFromCollection(resultList, true);

                       

                        worksheet.Cells["A7:T"+(7+newResult.Count())].LoadFromCollection<IndentReportDto>(newResult,true);
                        //worksheet.Cells["A7:R7"].Style.Font.Size = 13;
                        //worksheet.Cells["A7:R7"].Style.Font.Name = "Calibri";
                        //worksheet.Cells["A7:R7"].Style.Font.Bold = true;

                        ExcelRange range = worksheet.Cells["A7:T7"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;

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
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region MonthlyTourPlan
        public ActionResult MonthlyTourPlan()
        {
            RoleIdDto roleIdDto = new RoleIdDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId,
                LoginUserId = UserId,
                OrganizationReportingToId = OrganizationReportingToId
            };
            return View(roleIdDto);
        }
        public async Task<ActionResult> GetMonthlyTourPlanReports(DateTime fromDate, DateTime toDate, string zhId, string bdoId, long verticalId,  long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetMonthlyTourPlanReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "MONTHLYTOURPLAN-REPORT-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";

            try
            {
                var zhead = new List<long>();
                if (!string.IsNullOrEmpty(zhId))
                {
                    zhead = zhId.Split(',').Select(long.Parse).ToList();
                }
                var bdo = new List<long>();
                if (!string.IsNullOrEmpty(bdoId))
                {
                    bdo = bdoId.Split(',').Select(long.Parse).ToList();
                }
                //var zhead = zhId.Split(',').Select(long.Parse).ToList();
                //var bdo = bdoId.Split(',').Select(long.Parse).ToList();
                var zh = zhead.SingleOrDefault(r => r == 0);
                zhead.Remove(zh);
                var StateTrader = bdo.SingleOrDefault(r => r == 0);
                bdo.Remove(StateTrader);
                MonthlyTourPlanReportInputDto inputputDto = new MonthlyTourPlanReportInputDto() { FromDate = fromDate, ToDate = toDate, ZonalHeadIds = zhead, VerticalId = verticalId, BDOIds = bdo, SalesOrganizationId = SalesOrganizationId, DistributionChannelId = DistributionChannelId };
                var publishData = await _reportClient.GetMTPDetailsReportAsync(inputputDto);
                if (publishData.IsAny())
                {
                    var result = publishData.FirstOrDefault();

                    string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFoloderPath + "FinalPriceTemplate.xlsx");

                    var resultList = publishData.Select(s => new MonthlyTourPlanExportDto()
                    {
                        MTPNumber = s.MTPNumber,
                        CreatedDate = s.CreatedDate.ToString("dd-MM-yyyy HH:mm:ss"),
                        ZonalHeadName = s.ZonalHeadName,
                        StateTraderName = s.BDOName,
                        Date = Settings.DateFormats(s.Date, Settings.ReportDateFormat),
                        Day = (s.Date).DayOfWeek.ToString(),
                        City = s.City,
                        Area = s.Area,
                        Distributor = s.Dealer,
                        Remarks = s.Remarks,
                        InHQVisit = s.InHQNoVisitName
                    }).ToList();

                    using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = "Monthly Tour Plan";

                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "From Date";
                        ws.Cells["A4"].Value = "To Date";
                        ws.Cells["A5"].Value = "Total Record Count";
                        //ws.Cells["A6"].Value = "Vertical";
                        for (int i = 2; i <= 6; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }
                        //var VerticalIdInInt = Convert.ToInt32(verticalId);
                        ws.Cells["B2"].Value = "MonthlyTourPlan";
                        ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B5"].Value = publishData.Count;
                        //ws.Cells["B6"].Value = VerticalIdInInt == (int)DTO.Enums.Division.Hbc ? "Hbc" : VerticalIdInInt == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        #endregion

                        //int headerIndex = 8;
                        //ws.Cells["A" + headerIndex].Value = "MTP Number";
                        //ws.Cells["B" + headerIndex].Value = "Created Date";
                        //ws.Cells["C" + headerIndex].Value = "ZonalHeadName";
                        //ws.Cells["D" + headerIndex].Value = "BDOName";
                        //ws.Cells["E" + headerIndex].Value = "Date";
                        //ws.Cells["F" + headerIndex].Value = "Day";
                        //ws.Cells["G" + headerIndex].Value = "City";
                        //ws.Cells["H" + headerIndex].Value = "Area";
                        //ws.Cells["I" + headerIndex].Value = "Dealer";
                        ////ws.Cells["J" + headerIndex].Value = "Headquaters";
                        //ws.Cells["K" + headerIndex].Value = "Remarks";
                        //ws.Cells["L" + headerIndex].Value = "In HQ / No Visit";
                        ws.Cells["A8:K" + (8 + publishData.Count)].LoadFromCollection(resultList, true);
                        ExcelRange range = ws.Cells["A8:K8"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        //int contentIndex = 9;

                        //foreach (var data in publishData)
                        //{
                        //    ws.Cells["A" + contentIndex].Value = data.MTPNumber; //"MTPNumber"
                        //    ws.Cells["B" + contentIndex].Value = Settings.DateFormats(data.CreatedDate, Settings.ReportDateFormat); //"CreatedDate"
                        //    ws.Cells["C" + contentIndex].Value = data.ZonalHeadName; //"ZonalHeadName"
                        //    ws.Cells["D" + contentIndex].Value = data.BDOName; //  "BDOName";
                        //    ws.Cells["E" + contentIndex].Value = Settings.DateFormats(data.Date, Settings.ReportDateFormat); ; //  "BDOName";
                        //    ws.Cells["F" + contentIndex].Value = (data.Date).DayOfWeek; //  "Date";
                        //    ws.Cells["G" + contentIndex].Value = data.City; //  "City";
                        //    ws.Cells["H" + contentIndex].Value = data.Area; //  "Area";
                        //    ws.Cells["I" + contentIndex].Value = data.Dealer; //  "Dealer";
                        //    //ws.Cells["J" + contentIndex].Value = data.Headquarters; //  "Headquarters";
                        //    ws.Cells["K" + contentIndex].Value = data.Remarks; //  "Remarks";
                        //    ws.Cells["L" + contentIndex].Value = data.InHQNoVisitName;
                        //    contentIndex++;
                        //}

                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PermanentCoveragePlan
        public ActionResult PermanentCoveragePlan()
        {
            RoleIdDto roleIdDto = new RoleIdDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId,
                LoginUserId = UserId,
                OrganizationReportingToId = OrganizationReportingToId
            };
            return View(roleIdDto);
        }
        public async Task<ActionResult> GetPermanentCoveragePlanReports(DateTime fromDate, DateTime toDate,  long verticalId, string zhId, string bdoId, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetPermanentCoveragePlanReports";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "PERMANENTCOVERAGEPLAN-REPORT-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";

            try
            {
                var zhead = new List<long>();
                if (!string.IsNullOrEmpty(zhId))
                {
                    zhead = zhId.Split(',').Select(long.Parse).ToList();
                }
                var bdo = new List<long>();
                if (!string.IsNullOrEmpty(bdoId))
                {
                    bdo = bdoId.Split(',').Select(long.Parse).ToList();
                }
                
                var zh = zhead.SingleOrDefault(r => r == 0);
                zhead.Remove(zh);
                var StateTrader = bdo.SingleOrDefault(r => r == 0);
                bdo.Remove(StateTrader);

                PermanentCoveragePlanReportInputDto inputputDto = new PermanentCoveragePlanReportInputDto() { FromDate = fromDate, ToDate = toDate, ZonalHeadIds = zhead, VerticalId = verticalId, BDOIds = bdo, SalesOrganizationId = SalesOrganizationId, DistributionChannelId = DistributionChannelId };
                var publishData = await _reportClient.GetPCPDetailsReportAsync(inputputDto);
                if (publishData.IsAny())
                {
                    var result = publishData.FirstOrDefault();

                    string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFoloderPath + "FinalPriceTemplate.xlsx");

                    var resultList = publishData.Select(s => new PCPExport()
                    {
                        PCPNumber = s.PCPNumber,
                        CreatedDate = s.CreatedDate.ToString("dd-MM-yyyy HH:mm:ss"),
                        ZonalHeadname = s.ZonalHeadName,
                        StateTraderName = s.BDOName,
                        Year = s.Year,
                        EffectiveFrom = s.EffectiveFrom.ToString("dd-MM-yyyy HH:mm:ss"),
                        EffectiveTo = s.EffectiveTo.ToString("dd-MM-yyyy HH:mm:ss"),
                        State = s.State,
                        District = s.District,
                        City = s.City,
                        Dealer = s.Dealer,
                        NoOfSubDealer=s.NoOfSubDealer,
                        NoOfWholeSeller=s.NoOfWholeSeller,
                        NoOfVisit=s.NoOfVisit.ToString(),
                        InHQVisit=s.InHQNoVisitName,
                        Remarks=s.Remarks



                    }).ToList() ;

                    using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = "Permanent Coverage Plan";

                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "From Date";
                        ws.Cells["A4"].Value = "To Date";
                        ws.Cells["A5"].Value = "Total Record Count";
                        //ws.Cells["A6"].Value = "Vertical";
                        for (int i = 2; i <= 6; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }
                        //var VerticalIdInInt = Convert.ToInt32(verticalId);
                        ws.Cells["B2"].Value = "PermanentCoveragePlan";
                        ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat); ;
                        ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat);
                        ws.Cells["B5"].Value = publishData.Count;
                        //ws.Cells["B6"].Value = VerticalIdInInt == (int)DTO.Enums.Division.Hbc ? "Hbc" : VerticalIdInInt == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        #endregion

                        ws.Cells["A8:Q" + (8 + publishData.Count)].LoadFromCollection(resultList,true);


                        //int headerIndex = 8;
                        //ws.Cells["A" + headerIndex].Value = "PCP Number";
                        //ws.Cells["B" + headerIndex].Value = "Created Date";
                        //ws.Cells["C" + headerIndex].Value = "Zonal Head Name";
                        //ws.Cells["D" + headerIndex].Value = "StateTrader Name";
                        //ws.Cells["E" + headerIndex].Value = "Year";
                        //ws.Cells["F" + headerIndex].Value = "Effective From";
                        //ws.Cells["G" + headerIndex].Value = "Effective To";
                        //ws.Cells["H" + headerIndex].Value = "State";
                        //ws.Cells["I" + headerIndex].Value = "Territory";
                        //ws.Cells["J" + headerIndex].Value = "District";
                        //ws.Cells["K" + headerIndex].Value = "City";
                        //ws.Cells["L" + headerIndex].Value = "Dealer";
                        //ws.Cells["M" + headerIndex].Value = "No Of Sub Dealer";
                        //ws.Cells["N" + headerIndex].Value = "No Of Whole Seller";
                        //ws.Cells["O" + headerIndex].Value = "Number of Visit";
                        //ws.Cells["P" + headerIndex].Value = "In HQ / No Visit";
                        //ws.Cells["Q" + headerIndex].Value = "Remarks";

                        ExcelRange range = ws.Cells["A8:Q8"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        //int contentIndex = 9;

                        //foreach (var data in publishData)
                        //{
                        //    ws.Cells["A" + contentIndex].Value = data.PCPNumber; //"PCPNumber"
                        //    ws.Cells["B" + contentIndex].Value = Settings.DateFormats(data.CreatedDate, Settings.ReportDateFormat); //"CreatedDate"
                        //    ws.Cells["C" + contentIndex].Value = data.ZonalHeadName; //"ZonalHeadName"
                        //    ws.Cells["D" + contentIndex].Value = data.BDOName; //  "BDOName";
                        //    ws.Cells["E" + contentIndex].Value = data.Year; //  "BDOName";
                        //    ws.Cells["F" + contentIndex].Value = Settings.DateFormats(data.EffectiveFrom, Settings.ReportDateFormat);  //  "EffectiveFrom";
                        //    ws.Cells["G" + contentIndex].Value = Settings.DateFormats(data.EffectiveTo, Settings.ReportDateFormat);  //  "EffectiveTo";
                        //    ws.Cells["H" + contentIndex].Value = data.State; //  "State";
                        //    ws.Cells["I" + contentIndex].Value = data.Territory; //  "Territory";
                        //    ws.Cells["J" + contentIndex].Value = data.District; //  "District";
                        //    ws.Cells["K" + contentIndex].Value = data.City; //  "City";
                        //    ws.Cells["L" + contentIndex].Value = data.Dealer; // "Dealer";
                        //    ws.Cells["M" + contentIndex].Value = data.NoOfSubDealer; //  "NoOfSubDealer";
                        //    ws.Cells["N" + contentIndex].Value = data.NoOfWholeSeller; //  "NoOfWholeSeller";
                        //    ws.Cells["O" + contentIndex].Value = data.NoOfVisit; //  "NoOfVisit";
                        //    ws.Cells["P" + contentIndex].Value = data.InHQNoVisitName; //  "InHQNoVisitName";
                        //    ws.Cells["Q" + contentIndex].Value = data.Remarks; //  "Remarks";
                        //    contentIndex++;
                        //}

                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region MonthlyReport

        public JsonResult GetMonthlyReportList([DataSourceRequest] DataSourceRequest request)
        {
            var approveList = ((MonthlyReport[])Enum.GetValues(typeof(MonthlyReport))).Select(c => new EnumModel() { EntityTypeId = (int)c, Name = c.Description().ToString() }).OrderBy(_ => _.Name).ToList();
            return Json(approveList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MonthlyReport()
        {

            MonthlyReportInputDto roleIdDto = new MonthlyReportInputDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId
            };
            return View(roleIdDto);
        }

        public JsonResult MonthlyReportExcelDownload(MonthlyReportInputDto monthlyReportInputDto)
        {
            JsonResult jsonResult = new JsonResult();
            monthlyReportInputDto.RoleId = RoleId;
            monthlyReportInputDto.LoginUserId = UserId;
            if (monthlyReportInputDto.ReportId == (int)DTO.Enums.MonthlyReport.InvoiceReport)
            {
                var InvoiceReportList = _reportClient.MonthWiseInvoiceExportToList(monthlyReportInputDto);
                if (InvoiceReportList != null && InvoiceReportList.Rows.Count > 0)
                {
                    string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFoloderPath + "FinalPriceTemplate.xlsx");

                    using (ExcelPackage ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = Utility.GetEnumFromString<MonthlyReport>(3);


                        ExcelRange range = ws.Cells["A1:BZ1"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "Total Record Count";
                        //ws.Cells["A4"].Value = "Vertical";
                        for (int i = 2; i <= 3; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        ws.Cells["B2"].Value = "MonthlyReport";
                        ws.Cells["B3"].Value = InvoiceReportList.Rows.Count;
                        // ws.Cells["B4"].Value = monthlyReportInputDto.VerticalId == (int)DTO.Enums.Division.Hbc ? "Hbc" : monthlyReportInputDto.VerticalId == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        #endregion
                        ws.Cells["A5"].LoadFromDataTable(InvoiceReportList, true);
                        ws.Cells.AutoFitColumns();
                        string fileName = "MONTHWISE INVOICE REPORT-" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now).ToUpper() + ".xlsx";
                        jsonResult = SaveExcelFileToPath(ep, fileName);
                        ep.Dispose();
                    }
                }
                else
                {
                    return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
                }

            }
            if (monthlyReportInputDto.ReportId == (int)DTO.Enums.MonthlyReport.LiftingRequest)
            {
                var LiftingRequestList = _reportClient.MonthWiseLiftingRequestExportToList(monthlyReportInputDto);
                if (LiftingRequestList != null && LiftingRequestList.Rows.Count > 0)
                {
                    string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFoloderPath + "FinalPriceTemplate.xlsx");

                    using (ExcelPackage ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = Utility.GetEnumFromString<MonthlyReport>(2);


                        ExcelRange range = ws.Cells["A1:BZ1"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "Total Record Count";
                        //ws.Cells["A4"].Value = "Vertical";
                        for (int i = 2; i <= 3; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        ws.Cells["B2"].Value = "MonthlyReport";
                        ws.Cells["B3"].Value = LiftingRequestList.Rows.Count;
                        // ws.Cells["B4"].Value = monthlyReportInputDto.VerticalId == (int)DTO.Enums.Division.Hbc ? "Hbc" : monthlyReportInputDto.VerticalId == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        #endregion
                        ws.Cells["A5"].LoadFromDataTable(LiftingRequestList, true);
                        ws.Cells.AutoFitColumns();
                        string fileName = "MONTHWISE SALES ORDER REQUEST REPORT-" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now).ToUpper() + ".xlsx";
                        jsonResult = SaveExcelFileToPath(ep, fileName);
                        ep.Dispose();
                    }
                }
                else
                {
                    return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            if (monthlyReportInputDto.ReportId == (int)DTO.Enums.MonthlyReport.SaudaReport)
            {
                var SaudaReportList = _reportClient.MonthWiseSaudaExportToList(monthlyReportInputDto);
                if (SaudaReportList != null && SaudaReportList.Rows.Count > 0)
                {
                    string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFoloderPath + "FinalPriceTemplate.xlsx");

                    using (ExcelPackage ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = Utility.GetEnumFromString<MonthlyReport>(1);
                        ExcelRange range = ws.Cells["A1:BZ1"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "Total Record Count";
                        //  ws.Cells["A4"].Value = "Vertical";
                        for (int i = 2; i <= 3; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        ws.Cells["B2"].Value = "MonthlyReport";
                        ws.Cells["B3"].Value = SaudaReportList.Rows.Count;
                        // ws.Cells["B4"].Value = monthlyReportInputDto.VerticalId == (int)DTO.Enums.Division.Hbc ? "Hbc" : monthlyReportInputDto.VerticalId == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        #endregion
                        ws.Cells["A5"].LoadFromDataTable(SaudaReportList, true);
                        ws.Cells.AutoFitColumns();
                        string fileName = "MONTHWISE SAUDA REPORT-" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now).ToUpper() + ".xlsx";
                        jsonResult = SaveExcelFileToPath(ep, fileName);
                        ep.Dispose();
                    }
                }
                else
                {
                    return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            return jsonResult;
        }

        #endregion

        #region PendingContractReport

        [AuthorizeClaims(Claims.ViewReports)]
        public async Task<ActionResult> PendingContractReport()
        {
            PendingContractReportDto inputDto = new PendingContractReportDto();
            inputDto = await _reportClient.GetVerticalIdAsync(UserId);
            inputDto.VerticalId = VerticalId;
            inputDto.RoleId = RoleId;
            inputDto.LoginUserId = UserId;
            return View(inputDto);
        }

        public ActionResult PendingContractExportAsync(PendingContractReportDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto.LoginUserId = UserId;
                List<PendingContractReportOutputDto> pendingContractList = new List<PendingContractReportOutputDto>();
                pendingContractList = _reportClient.PendingContractExportAsync(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "OPEN CONTRACT-REPORT" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";

                if (pendingContractList != null && pendingContractList.Any())
                {
                    var result = pendingContractList.FirstOrDefault();

                    string serverFolderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFolderPath + "FinalPriceTemplate.xlsx");

                    using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = "Pending Contract Report";

                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A5"].Value = "Total Record Count";
                        ws.Cells["A6"].Value = "Vertical";
                        for (int i = 2; i <= 6; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        ws.Cells["B2"].Value = "PendingContractReport";
                        ws.Cells["B5"].Value = pendingContractList.Count;
                        ws.Cells["B6"].Value = inputDto.VerticalId == (int)DTO.Enums.Division.Hbc ? "Hbc" : inputDto.VerticalId == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        #endregion

                        int headerIndex = 8;
                        ws.Cells["A" + headerIndex].Value = "Plant Name";
                        ws.Cells["B" + headerIndex].Value = "State";
                        ws.Cells["C" + headerIndex].Value = "Customer Code";
                        ws.Cells["D" + headerIndex].Value = "Customer Name";
                        ws.Cells["E" + headerIndex].Value = "Material Code";
                        ws.Cells["F" + headerIndex].Value = "Material Desc";
                        ws.Cells["G" + headerIndex].Value = "Oil Type";
                        ws.Cells["H" + headerIndex].Value = "Pending Qty Cases";
                        ws.Cells["I" + headerIndex].Value = "Pending Qty(MT)";
                        ws.Cells["J" + headerIndex].Value = "Basic Rate Per Case";
                        ws.Cells["K" + headerIndex].Value = "Inco Terms";
                        ws.Cells["L" + headerIndex].Value = "Contract No";
                        ws.Cells["M" + headerIndex].Value = "SAP Contract No";
                        ws.Cells["N" + headerIndex].Value = "Sauda Date";
                        ws.Cells["O" + headerIndex].Value = "Contract Valid From";
                        ws.Cells["P" + headerIndex].Value = "Contract Valid To";
                        ws.Cells["Q" + headerIndex].Value = "Broker Name";

                        ExcelRange range = ws.Cells["A7:BJ7"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        int contentIndex = 9;

                        //To implement filters
                        ws.Cells["A1:Q1"].AutoFilter = true;

                        foreach (var data in pendingContractList)
                        {
                            ws.Cells["A" + contentIndex].Value = data.PlantName; //Plant Name
                            ws.Cells["B" + contentIndex].Value = data.State; //  "State";
                            ws.Cells["C" + contentIndex].Value = data.CustomerCode; // "Customer Code";
                            ws.Cells["D" + contentIndex].Value = data.CustomerName; //  "Customer Name";
                            ws.Cells["E" + contentIndex].Value = data.MaterialCode; //  "Material Code";
                            ws.Cells["F" + contentIndex].Value = data.MaterialDescription; //  "Material Description";
                            ws.Cells["G" + contentIndex].Value = data.OilType; //  "Oil Type";
                            ws.Cells["H" + contentIndex].Value = data.PendingQtyCases;  //  "Pending Qty";
                            ws.Cells["I" + contentIndex].Value = data.PendingQty_MT; //  "Pending Qty(MT)";
                            ws.Cells["J" + contentIndex].Value = data.BasicRatePerCase;// "Basic Rate Per Case"
                            ws.Cells["K" + contentIndex].Value = data.IncoTerms; //  "Inco Terms";
                            ws.Cells["L" + contentIndex].Value = data.ContractNo; //  "Contract No";
                            ws.Cells["M" + contentIndex].Value = data.SAPContractNo; //  "SAPContractNo";
                            ws.Cells["N" + contentIndex].Value = data.SaudaDate.ToShortDateString(); //  "SaudaDate";
                            ws.Cells["O" + contentIndex].Value = data.ContractValidFrom.ToShortDateString(); //  "Contract Valid From";
                            ws.Cells["P" + contentIndex].Value = data.ContractValidTo.ToShortDateString(); //  "Contract Valid To";
                            ws.Cells["Q" + contentIndex].Value = data.BrokerName; //  "Broker Name";

                            contentIndex++;
                        }

                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
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

        #region DSRReport

        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult DSRReport()
        {
            var UserDTO = new UserInputDto
            {
                RoleId = RoleId,
                Name = UserName,
                UserId = UserId,
                VerticalId = VerticalId
            };
            return View(UserDTO);
        }


        public JsonResult GetDSRReportList([DataSourceRequest] DataSourceRequest request)
        {
            var approveList = ((DSRReportType[])Enum.GetValues(typeof(DSRReportType))).Select(c => new EnumModel() { EntityTypeId = (int)c, Name = c.Description().ToString() }).ToList();
            return Json(approveList, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> DSRReportExportAsync(DSRReportInputdto dSRReportInputdto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<DSRReportDTO> resultList = new List<DSRReportDTO>();
                resultList = _reportClient.DSRReportExport(dSRReportInputdto);

                if (resultList.IsAny())
                {
                    DateTime currentDate = DateTime.Now;
                    fileName = "DSR_REPORT" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                    guidFileName = $"{Guid.NewGuid()}.xlsx";

                    // Create the package and make sure you wrap it in a using statement
                    using (var package = new ExcelPackage())
                    {
                        //// add a new worksheet to the empty workbook
                        //var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        //Response.ClearHeaders();
                        //Response.ClearContent();
                        //Response.Clear();
                        //var rowIndex = 1;
                        //var colIndex = 1;

                        var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        var rowIndex = 7;
                        var colIndex = 1;

                        #region Header

                        worksheet.Cells["A1:M1"].Merge = true;
                        worksheet.Cells["A1:M1"].Value = Settings.CompanyName;
                        worksheet.Cells["A1:M1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1:M1"].Style.Font.Bold = true;
                        worksheet.Cells["A1:M1"].Style.Font.Size = 16;

                        worksheet.Cells["A2"].Value = "Report Name";
                        worksheet.Cells["A3"].Value = "From date";
                        worksheet.Cells["A4"].Value = "To Date";
                        //worksheet.Cells["A5"].Value = "Status";
                        worksheet.Cells["B3"].Value = string.Format(Settings.ReportDateFormat, dSRReportInputdto.FromDate);
                        worksheet.Cells["B4"].Value = string.Format(Settings.ReportDateFormat, dSRReportInputdto.ToDate);
                        for (int i = 2; i <= 5; i++)
                        {
                            worksheet.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheet.Cells["A" + i].Style.Font.Bold = true;
                            worksheet.Cells["A" + i].Style.Font.Size = 12;

                            worksheet.Cells["B" + i + ":" + "F" + i].Merge = true;
                            worksheet.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        #endregion

                        if (dSRReportInputdto.ReportType == (int)DTO.Enums.DSRReportType.DealerVisit)
                        {


                            worksheet.Cells["B2"].Value = UtilityHelper.GetEnumDescription(DTO.Enums.DSRReportType.DealerVisit) + " Report";


                            var result = resultList.Select(s => new DSRDealerVisitDto()
                            {
                                Date=s.Date != null ? string.Format(Settings.GridDateFormat, s.Date).ToString() : string.Empty,
                                DistributorName=s.DealerName,
                                PendingSaudaNumber=s.PendingSaudaNO,
                                PendingSaudaRemarks=s.PendingSaudaNORemarks,
                                MarketScenario=s.MarketScenarioTitle,
                                MarketScenarioRemarks=s.MarketScenarioRemarks,
                                CompetitorName=s.CompetitorName,
                                ProductName=s.ProductName,
                                Rate=s.Rate.ToString()
                                
                            });


                            worksheet.Cells["A7:J" + (7 + resultList.Count)].LoadFromCollection(result, true);


                            ExcelRange range = worksheet.Cells["A7:j7"];
                            range.AutoFitColumns();
                            range.Style.Font.Size = 12;
                            range.Style.Font.Bold = true;
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Date");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "DealerName");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "PendingSaudaNumber");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "PendingSaudaNumberRemarks");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "MarketScenarioTitle");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "MarketScenarioRemarks");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "CompetitorName");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "ProductName");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Quantity");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Rate");



                            //if (resultList != null && resultList.Any())
                            //{
                            //    foreach (var item in resultList)
                            //    {
                            //        rowIndex++;
                            //        colIndex = 1;
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Date != null ? string.Format(Settings.GridDateFormat, item.Date).ToString() : string.Empty);
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.DealerName != null ? item.DealerName : string.Empty);
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.PendingSaudaNORemarks.ToString());
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.PendingSaudaNORemarks != null ? item.PendingSaudaNORemarks.ToString() : string.Empty);
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.MarketScenarioTitle != null ? item.MarketScenarioTitle.ToString() : string.Empty);
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.MarketScenarioRemarks != null ? item.MarketScenarioRemarks.ToString() : string.Empty);
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.CompetitorName != null ? item.CompetitorName.ToString() : string.Empty);
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ProductName != null ? item.ProductName.ToString() : string.Empty);
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Quantity.ToString());
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Rate.ToString());


                            //    }
                            //}

                        }

                        if (dSRReportInputdto.ReportType == (int)DTO.Enums.DSRReportType.Wholesaler)
                        {
                            worksheet.Cells["B2"].Value = UtilityHelper.GetEnumDescription(DTO.Enums.DSRReportType.Wholesaler) + " Report";

                            var result = resultList.Select(s => new DSRWholeSalerReport() 
                            { 
                                Date= s.Date != null ? string.Format(Settings.GridDateFormat, s.Date).ToString() : string.Empty,
                                StateTrader=s.BDOName,
                                DealerName=s.DealerName,
                                WholeSalerName=s.WholeSellerName,
                                OilType=s.OilType,
                                Skuname=s.SkuName,
                                QuantityMT=s.QtyperCase.ToString(),
                                Price=s.Price.ToString()

                            
                            }                        
                            
                            );

                            worksheet.Cells["A7:H" + (7 + resultList.Count)].LoadFromCollection(result, true);

                            ExcelRange range = worksheet.Cells["A7:H7"];
                            range.AutoFitColumns();
                            range.Style.Font.Size = 12;
                            range.Style.Font.Bold = true;

                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Date");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "BDOName");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "DealerName");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "WholeSalerName");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "OilType");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "SkuName");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "QuantityPerMT");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Price");
                            //if (resultList != null && resultList.Any())
                            //{
                            //    foreach (var item in resultList)
                            //    {
                            //        rowIndex++;
                            //        colIndex = 1;
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Date != null ? string.Format(Settings.GridDateFormat, item.Date).ToString() : string.Empty);
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.BDOName != null ? item.BDOName : string.Empty);
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.DealerName != null ? item.DealerName : string.Empty);
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.WholeSellerName != null ? item.WholeSellerName : string.Empty);
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.OilType != null ? item.OilType : string.Empty);
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.SkuName != null ? item.SkuName : string.Empty);
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.QtyperCase.ToString());
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Price.ToString());


                            //    }
                            //}

                        }

                        if (dSRReportInputdto.ReportType == (int)DTO.Enums.DSRReportType.ProspectiveDealer)
                        {
                            worksheet.Cells["B2"].Value = UtilityHelper.GetEnumDescription(DTO.Enums.DSRReportType.ProspectiveDealer) + " Report";

                            var result = resultList.Select(s => new ProspectiveDealerExport()
                            {
                                Date = s.Date != null ? string.Format(Settings.GridDateFormat, s.Date).ToString() : string.Empty,
                                ProspectName = s.ProspectName,
                                MobileNumber = s.MobileNumber.ToString(),
                                Email = s.Email,
                                Address = s.Address,
                                ProspectiveSales = s.ProspectiveSales.ToString(),
                                ProspectiveIntrestLevel = s.ProspectiveInterestLevel.ToString(),
                                BusinessPotentialyear=s.BusinessPotentialPeryear.ToString()

                            }) ;

                            worksheet.Cells["A7:H" + (7 + resultList.Count)].LoadFromCollection(result, true);

                            ExcelRange range = worksheet.Cells["A7:H7"];
                            range.AutoFitColumns();
                            range.Style.Font.Size = 12;
                            range.Style.Font.Bold = true;

                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Date");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "ProspectName");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "MobileNumber");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Email");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Address");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "ProspectiveSales");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "ProspectiveInterestLevel");
                            //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "BusinessPotentialPeryear");
                            //if (resultList != null && resultList.Any())
                            //{
                            //    foreach (var item in resultList)
                            //    {
                            //        rowIndex++;
                            //        colIndex = 1;
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Date != null ? string.Format(Settings.GridDateFormat, item.Date).ToString() : string.Empty);
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ProspectName != null ? item.ProspectName : string.Empty);
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.MobileNumber.ToString());
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Email != null ? item.Email : string.Empty);
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Address != null ? item.Address : string.Empty);
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ProspectiveSales.ToString());
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ProspectiveInterestLevel.ToString());
                            //        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.BusinessPotentialPeryear.ToString());


                            //    }
                            //}

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
                                catch { }
                            }

                            try
                            {
                                var cells = workSheet.Cells[workSheet.Dimension.Address];
                                cells.AutoFitColumns();
                            }
                            catch { }
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
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PendingContracts

        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult PendingContracts()
        {
            RoleIdDto roleIdDto = new RoleIdDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId
            };
            return View(roleIdDto);
        }

        public ActionResult GetPendingContracts([DataSourceRequest] DataSourceRequest request, long verticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            var pendingContractsList = _reportClient.GetPendingContractsList(UserId, RoleId, verticalId, SalesOrganizationId, DistributionChannelId);
            var resultList = pendingContractsList.ToDataSourceResult(request);
            return Json(resultList);
        }

        public ActionResult PendingContractsExportAsync(long verticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {

                var pendingContractsList = _reportClient.GetPendingContractsList(UserId, RoleId, verticalId, SalesOrganizationId, DistributionChannelId);

                DateTime currentDate = DateTime.Now;
                fileName = "Open Contract -REPORT" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";

                if (pendingContractsList != null && pendingContractsList.Any())
                {

                    var resultList = pendingContractsList.Select(s => new PendingContractExportDto()
                    {
                        SaudaNumber=s.SaudaNumber,
                        CustomerName=s.CustomerName,
                        CustomerCode=s.CustomerCode,
                        MaterialCode=s.MaterialCode,
                        BasicPrice = s.BasicRate,
                        TotalPrice=s.TotalPrice,
                        ContractValidTo=s.ContractValidTo.ToString("dd-MM-yyyy"),
                        SalesOrganization=s.SalesOrganization,
                        DistributionChannel=s.DistributionChannel,
                        Division=s.Division,
                        PendingQuantityInMT=s.PendingQuantityInMT,
                        OpenSalesOrderQuantity=s.OpenSalesOrderQuantity,
                        PendingQuantityInCase=s.PendingQuantityInCase,
                        
                        CreatedDate=s.CreatedDate.ToString("dd-MM-yyyy HH:mm:ss")

                    }).ToList() ;
                    string serverFolderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFolderPath + "FinalPriceTemplate.xlsx");

                    using (var ep = new ExcelPackage())
                    {
                      

                        

                        var newList = new List<PendingContractExportDto>();
                        if (resultList.Count > 50000)
                        {
                            for (var i = 0; resultList.IsAny(); i++)
                            {
                                var ws = ep.Workbook.Worksheets.Add("Open Contract Report" + i);
                                #region Header
                                ws.Cells["A1:F1"].Merge = true;
                                ws.Cells["A1:F1"].Value = Settings.CompanyName;
                                ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                ws.Cells["A1:F1"].Style.Font.Bold = true;
                                ws.Cells["A1:F1"].Style.Font.Size = 16;

                                ws.Cells["A2"].Value = "Report Name";
                                ws.Cells["A3"].Value = "Total Record Count";
                                ws.Cells["A4"].Value = "Date and Time";



                                for (int j = 2; j <= 5; j++)
                                {
                                    ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                    ws.Cells["A" + i].Style.Font.Bold = true;
                                    ws.Cells["A" + i].Style.Font.Size = 12;

                                    ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                                    ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                }



                                ws.Cells["B2"].Value = "Open Contract Report";
                                ws.Cells["B3"].Value = pendingContractsList.Count;
                                ws.Cells["B4"].Value = DateHelper.UtcToIndia(DateTime.UtcNow).ToString("dd-MM-yyyy HH:mm tt");
                                ws.Cells["A4"].Style.Font.Bold = true;
                                ws.Cells["A4"].Style.Font.Size = 12;



                                #endregion


                                newList = resultList.Take(50000).ToList();
                                resultList = resultList.Skip(50000).ToList();
                                ws.Cells["A7:L" + newList.Count].LoadFromCollection(newList, true);
                                ExcelRange range = ws.Cells["A7:BJ7"];
                                range.AutoFitColumns();
                                range.Style.Font.Size = 12;
                                range.Style.Font.Bold = true;
                                int contentIndex = 8;

                               
                                ws.Cells["A7" + ":" + "L" + contentIndex + newList.Count].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells.AutoFitColumns();

                            }
                           


                        }
                        else
                        {
                            var ws = ep.Workbook.Worksheets.Add("Open Contract Report");

                            #region Header
                            ws.Cells["A1:F1"].Merge = true;
                            ws.Cells["A1:F1"].Value = Settings.CompanyName;
                            ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells["A1:F1"].Style.Font.Bold = true;
                            ws.Cells["A1:F1"].Style.Font.Size = 16;

                            ws.Cells["A2"].Value = "Report Name";
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



                            ws.Cells["B2"].Value = "Open Contract Report";
                            ws.Cells["B3"].Value = pendingContractsList.Count;
                            ws.Cells["B4"].Value = DateHelper.UtcToIndia(DateTime.UtcNow).ToString("dd-MM-yyyy HH:mm tt");
                            ws.Cells["A4"].Style.Font.Bold = true;
                            ws.Cells["A4"].Style.Font.Size = 12;



                            #endregion


                            ws.Cells["A7:L" + pendingContractsList.Count].LoadFromCollection(resultList, true);
                            ExcelRange range = ws.Cells["A7:BJ7"];
                            range.AutoFitColumns();
                            range.Style.Font.Size = 12;
                            range.Style.Font.Bold = true;
                            int contentIndex = 8;

                            ws.Cells["A7" + ":" + "L" + contentIndex + pendingContractsList.Count].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            ws.Cells.AutoFitColumns();

                        }


                        #region oldcode

                        //int headerIndex = 7;
                        //ws.Cells["A" + headerIndex].Value = "Sauda Number";
                        //ws.Cells["B" + headerIndex].Value = "Customer Name";
                        //ws.Cells["C" + headerIndex].Value = "Customer Code";
                        //ws.Cells["D" + headerIndex].Value = "Material Code";
                        //ws.Cells["E" + headerIndex].Value = "Basic Price";
                        //ws.Cells["F" + headerIndex].Value = "Total Price";
                        //ws.Cells["G" + headerIndex].Value = "Contract ValidTo";
                        //ws.Cells["H" + headerIndex].Value = "SalesOrganization";
                        //ws.Cells["I" + headerIndex].Value = "DistributionChannel";
                        //ws.Cells["J" + headerIndex].Value = "Division";
                        //ws.Cells["K" + headerIndex].Value = "Pending Quantity In Case";
                        //ws.Cells["L" + headerIndex].Value = "Created Date";
                        //ws.Cells["L" + headerIndex].Value = "Pending Quantity";
                        //ws.Cells["M" + headerIndex].Value = "Pending Quantity In Case";
                        //ws.Cells["N" + headerIndex].Value = "Basic Rate";
                        //ws.Cells["O" + headerIndex].Value = "IncoTerms1";
                        //ws.Cells["P" + headerIndex].Value = "Deal App Id";
                        //ws.Cells["Q" + headerIndex].Value = "SaudaNumber";
                        //ws.Cells["R" + headerIndex].Value = "SaudaDate";
                        //ws.Cells["S" + headerIndex].Value = "Contract ValidFrom";
                        //ws.Cells["T" + headerIndex].Value = "Contract ValidTo";
                        //ws.Cells["U" + headerIndex].Value = "Broker Name";
                        //ws.Cells["V" + headerIndex].Value = "MaterialGroup Description 5";
                        //ws.Cells["W" + headerIndex].Value = "ReleaseStatus";
                        //ws.Cells["X" + headerIndex].Value = "Pack Type";
                        //ws.Cells["Y" + headerIndex].Value = "Validity";
                        //ws.Cells["Z" + headerIndex].Value = "Aging By Days";
                        //ws.Cells["AA" + headerIndex].Value = "SalesOrganization";
                        //ws.Cells["AB" + headerIndex].Value = "SalesOrgDescription";
                        //ExcelRange range = ws.Cells["A7:BJ7"];
                        //range.AutoFitColumns();
                        //range.Style.Font.Size = 12;
                        //range.Style.Font.Bold = true;
                        //int contentIndex = 8;
                        //To implement filters
                        //ws.Cells["A1:Q1"].AutoFilter = true;

                        //foreach (var data in pendingContractsList)
                        //{
                        //    ws.Cells["A" + contentIndex].Value = data.SaudaNumber;
                        //    ws.Cells["B" + contentIndex].Value = data.CustomerName;
                        //    ws.Cells["C" + contentIndex].Value = data.CustomerCode;
                        //    ws.Cells["D" + contentIndex].Value = data.MaterialCode;
                        //    ws.Cells["E" + contentIndex].Value = data.BasicRate;
                        //    ws.Cells["F" + contentIndex].Value = data.TotalPrice;
                        //    ws.Cells["G" + contentIndex].Value = data.ContractValidTo.ToString("dd-MMM-yyyy");
                        //    ws.Cells["H" + contentIndex].Value = data.SalesOrganization;
                        //    ws.Cells["I" + contentIndex].Value = data.DistributionChannel;
                        //    ws.Cells["J" + contentIndex].Value = data.Division;
                        //    ws.Cells["K" + contentIndex].Value = data.PendingQuantityInCase;
                        //    ws.Cells["L" + contentIndex].Value = data.CreatedDate.ToString("dd-MMM-yyyy");
                        //    //ws.Cells["L" + contentIndex].Value = data.Division;
                        //    //ws.Cells["M" + contentIndex].Value = data.PendingQuantityInCase;
                        //    //ws.Cells["N" + contentIndex].Value = data.CreatedDate;
                        //    //ws.Cells["O" + contentIndex].Value = data.IncoTerms1;
                        //    //ws.Cells["P" + contentIndex].Value = data.SaudaOrderId;
                        //    //ws.Cells["Q" + contentIndex].Value = data.SaudaNumber;
                        //    //ws.Cells["R" + contentIndex].Value = data.SaudaDate != null ? string.Format(Settings.GridDateFormatForSalesRegisterAndPendingContracts, data.SaudaDate).ToString() : string.Empty;
                        //    //ws.Cells["S" + contentIndex].Value = data.ContractValidFrom != null ? string.Format(Settings.GridDateFormatForSalesRegisterAndPendingContracts, data.ContractValidFrom).ToString() : string.Empty;
                        //    //ws.Cells["T" + contentIndex].Value = data.ContractValidTo != null ? string.Format(Settings.GridDateFormatForSalesRegisterAndPendingContracts, data.ContractValidTo).ToString() : string.Empty;
                        //    //ws.Cells["U" + contentIndex].Value = data.BrokerName;
                        //    //ws.Cells["V" + contentIndex].Value = data.MaterialGroupDescription5;
                        //    //ws.Cells["W" + contentIndex].Value = data.ReleaseStatus;
                        //    //ws.Cells["X" + contentIndex].Value = data.PackGroup;
                        //    //ws.Cells["Y" + contentIndex].Value = data.Validity != null ? data.Validity : string.Empty;
                        //    //ws.Cells["Z" + contentIndex].Value = data.AgingByDays;
                        //    //ws.Cells["AA" + contentIndex].Value = data.SalesOrganization != null ? data.SalesOrganization : string.Empty;
                        //    //ws.Cells["AB" + contentIndex].Value = data.SalesOrgDescription != null ? data.SalesOrgDescription : string.Empty;
                        //    contentIndex++;
                        //}

                        #endregion
                        
                        guidFileName = SaveExcelFileToPath(ep);
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

        #region PendingContractTrigger

        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult PendingContractTrigger()
        {
            RoleIdDto roleIdDto = new RoleIdDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId,
                LoginUserId = UserId,
                OrganizationReportingToId = OrganizationReportingToId
            };
            return View(roleIdDto);
        }

        public ActionResult GetPendingContractTrigger(ContractOBRInputDto inputDto)
        {
            var dealerIdList = new List<OpenContractRequestDTO>();
            
            if (!string.IsNullOrEmpty(inputDto.DealerIds))
            {
                dealerIdList = inputDto.DealerIds.Split(',').Select(s => new OpenContractRequestDTO { 
                SoldToParty=s
                }).ToList();
            }

            ResultDto result = new ResultDto();
            //var contractDto = new OpenContractRequestDTOList()
            //{
            //    OpenContractBalReq = dealerIdList,
            //    DistChannel = inputDto.DistChnlId,
            //    SalesOrg = inputDto.SalesOrgId,
            //    Division = inputDto.DivisionId
            //};
            var pendingContractsList = _reportClient.GetPendingContractTrigger(inputDto,dealerIdList);
            result = pendingContractsList.Result;

            //foreach (var dealId in dealerIdList)
            //{
            //    var contractDto = new OpenContractRequestDTOList()
            //    {
            //        OpenContractBalReq = dealId,
            //        DistChnlId = inputDto.DistChnlId,
            //        SalesOrgId = inputDto.SalesOrgId,
            //        DivisionId = inputDto.DivisionId
            //    };
            //}  
                
            return Json(result);
        }

        #endregion

        #region PendingContractComparision

        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult PendingContractComparision()
        {
            RoleIdDto roleIdDto = new RoleIdDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId
            };
            return View(roleIdDto);
        }

        public ActionResult GetPendingContractComparision([DataSourceRequest] DataSourceRequest request, long verticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            var pendingContractsList = _reportClient.GetPendingContractComparisionList(verticalId, SalesOrganizationId, DistributionChannelId);
            var resultList = pendingContractsList.ToDataSourceResult(request);
            return Json(resultList);
        }

        public ActionResult PendingContractComparisionExportAsync(long VerticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {

                var pendingContractsList = _reportClient.GetPendingContractComparisionList(VerticalId, SalesOrganizationId, DistributionChannelId);

                DateTime currentDate = DateTime.Now;
                fileName = "PENDING CONTRACT COMPARISON -REPORT" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";

                if (pendingContractsList != null && pendingContractsList.Any())
                {
                    string serverFolderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFolderPath + "FinalPriceTemplate.xlsx");

                    using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = "Pending Contract Comparison Report";

                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A5"].Value = "Total Record Count";

                        for (int i = 2; i <= 5; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }



                        ws.Cells["B2"].Value = "Pending Contract Comparision Report";
                        ws.Cells["B5"].Value = pendingContractsList.Count;

                        ws.Cells["A6"].Value = " Report from SAP data ";
                        ws.Cells["A6" + ":" + "L6"].Merge = true;
                        ws.Cells["A6"].Style.Font.Bold = true;
                        ws.Cells["A6"].Style.Font.Size = 12;
                        ws.Cells["A6" + ":" + "L6"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells["A6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["M6"].Value = " Report from Portal data ";
                        ws.Cells["M6" + ":" + "X6"].Merge = true;
                        ws.Cells["M6"].Style.Font.Bold = true;
                        ws.Cells["M6"].Style.Font.Size = 12;
                        ws.Cells["M6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["M6" + ":" + "Z6"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        #endregion


                        int headerIndex = 7;
                        ws.Cells["A" + headerIndex].Value = "Dealer Code";
                        ws.Cells["B" + headerIndex].Value = "Dealer Name";
                        ws.Cells["C" + headerIndex].Value = "Broker Code";
                        ws.Cells["D" + headerIndex].Value = "Contract Number";
                        ws.Cells["E" + headerIndex].Value = "Contract Date";
                        ws.Cells["F" + headerIndex].Value = "Material Code";
                        ws.Cells["G" + headerIndex].Value = "Material Description";
                        ws.Cells["H" + headerIndex].Value = "OilType";
                        ws.Cells["I" + headerIndex].Value = "Contract Qunatity";
                        ws.Cells["J" + headerIndex].Value = "Despatch Quantity";
                        ws.Cells["K" + headerIndex].Value = "Pending Quantity";
                        ws.Cells["L" + headerIndex].Value = "Pending QuantityMT";
                        ws.Cells["M" + headerIndex].Value = "Dealer Code";
                        ws.Cells["N" + headerIndex].Value = "Dealer Name";
                        ws.Cells["O" + headerIndex].Value = "Broker Code";
                        ws.Cells["P" + headerIndex].Value = "Contract Number";
                        ws.Cells["Q" + headerIndex].Value = "Contract Date";
                        ws.Cells["R" + headerIndex].Value = "Material Code";
                        ws.Cells["S" + headerIndex].Value = "Material Description";
                        ws.Cells["T" + headerIndex].Value = "OilType";
                        ws.Cells["U" + headerIndex].Value = "Contract Qunatity";
                        ws.Cells["V" + headerIndex].Value = "Despatch Quantity";
                        ws.Cells["W" + headerIndex].Value = "Pending Quantity";
                        ws.Cells["X" + headerIndex].Value = "Pending QuantityMT";
                        ws.Cells["Y" + headerIndex].Value = "Status";
                        ws.Cells["Z" + headerIndex].Value = "ActionToTaken";



                        ExcelRange range = ws.Cells["A7:BJ7"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        int contentIndex = 8;



                        //To implement filters
                        ws.Cells["A1:Q1"].AutoFilter = true;

                        foreach (var data in pendingContractsList)
                        {
                            ws.Cells["A" + contentIndex].Value = data.SAPDealerCode;
                            ws.Cells["B" + contentIndex].Value = data.SAPDealerName;
                            ws.Cells["C" + contentIndex].Value = data.SAPBrokerCode;
                            ws.Cells["D" + contentIndex].Value = data.SAPContractNumber;
                            ws.Cells["E" + contentIndex].Value = data.SAPContractDate;
                            ws.Cells["F" + contentIndex].Value = data.SAPMaterialCode;
                            ws.Cells["G" + contentIndex].Value = data.SAPMaterialDescription;
                            ws.Cells["H" + contentIndex].Value = data.SAPOilType;
                            ws.Cells["I" + contentIndex].Value = data.SAPContractQuantity;
                            ws.Cells["J" + contentIndex].Value = data.SAPDespatchQuantity;
                            ws.Cells["K" + contentIndex].Value = data.SAPPendingQuantity;
                            ws.Cells["L" + contentIndex].Value = data.SAPPendingQuantityMT;
                            ws.Cells["M" + contentIndex].Value = data.DealerCode;
                            ws.Cells["N" + contentIndex].Value = data.DealerName;
                            ws.Cells["O" + contentIndex].Value = data.BrokerCode;
                            ws.Cells["P" + contentIndex].Value = data.ContractNumber;
                            ws.Cells["Q" + contentIndex].Value = data.ContractDate;
                            ws.Cells["R" + contentIndex].Value = data.MaterialCode;
                            ws.Cells["S" + contentIndex].Value = data.MaterialDescription;
                            ws.Cells["T" + contentIndex].Value = data.OilType;
                            ws.Cells["U" + contentIndex].Value = data.ContractQuantity;
                            ws.Cells["V" + contentIndex].Value = data.DespatchQuantity;
                            ws.Cells["W" + contentIndex].Value = data.PendingQuantity;
                            ws.Cells["X" + contentIndex].Value = data.PendingQuantityMT;
                            ws.Cells["Y" + contentIndex].Value = data.Status;
                            ws.Cells["Z" + contentIndex].Value = data.ActionToTaken;
                            contentIndex++;
                        }
                        ws.Cells["A7" + ":" + "z" + contentIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
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

        #region SalesRegister

        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult SalesRegister()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }

        public ActionResult GetSalesRegister([DataSourceRequest] DataSourceRequest request, DateTime monthStartdate, DateTime monthEnddate, long verticalId, long SalesOrganizationId, long DistributionChannelId)
        {
           // int days = DateTime.DaysInMonth(monthStartdate.Year, monthStartdate.Month) - 1;
           // DateTime monthLastdate = monthStartdate.AddDays(days);
            var salesRegisterList = _reportClient.GetSalesRegisterList(UserId, RoleId, monthStartdate, monthEnddate, verticalId, SalesOrganizationId, DistributionChannelId);
            var resultList = salesRegisterList.ToDataSourceResult(request);
            return Json(resultList);
        }

        public ActionResult SalesRegisterExportAsync(DateTime monthStartdate , DateTime monthEnddate, long verticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                //int days = DateTime.DaysInMonth(monthStartdate.Year, monthStartdate.Month) - 1;
                //DateTime monthLastdate = monthStartdate.AddDays(days);
                var salesRegisterList = _reportClient.GetSalesRegisterList(UserId, RoleId, monthStartdate, monthEnddate, verticalId, SalesOrganizationId, DistributionChannelId);

                DateTime currentDate = DateTime.Now;
                fileName = "SALES REGISTER -REPORT" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";

                if (salesRegisterList != null && salesRegisterList.Any())
                {
                    string serverFolderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFolderPath + "FinalPriceTemplate.xlsx");
                    var resultList = salesRegisterList.Select(s => new SalesRegisterExportDto()
                    {
                        MaterialCode = s.MaterialCode,
                        MaterialName = s.MaterialName,
                        DistributorCode = s.CustomerCode,
                        DistributorName = s.CustomerName,
                        QuantityMT = s.QuantityMT,
                        InvoiceType = s.InvoiceType,
                        InvoiceNumber = s.InvBillNumber,
                        InvoiceDate = s.InvoiceDate.ToString("dd-MM-yyyy HH:mm:ss"),
                        TotalGST = s.TotalGST,
                        TotalAmount = s.TotalAmount,
                        CreatedDate=s.CreatedDate.ToString("dd-MM-yyyy HH:mm:ss"),
                        SalesOrganization = s.SalesOrganization,
                        DistributionChannel = s.DistributionChannel,
                        Division = s.Vertical,
                        BrokerName=s.SecondBrokerName,
                        ContractNumber=s.Contractnumber,
                        DeliveryNumber=s.DeliveryNo,
                        LRNo=s.LRNo,
                        OrderNumber=s.OrderNumber,
                        VehicleNumber=s.Vehicleno,
                        ShiptToParty=s.ShiptoParty
                    }).ToList();
                    using (var ep = new ExcelPackage())
                    {

                        var newList = new List<SalesRegisterExportDto>();
                        if (resultList.Count > 50000)
                        {
                            for (var j = 0; resultList.IsAny(); j++)
                            {
                                var ws = ep.Workbook.Worksheets.Add("Sales Register Report" + j);
                                #region Header
                                ws.Cells["A1:F1"].Merge = true;
                                ws.Cells["A1:F1"].Value = Settings.CompanyName;
                                ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                ws.Cells["A1:F1"].Style.Font.Bold = true;
                                ws.Cells["A1:F1"].Style.Font.Size = 16;

                                ws.Cells["A2"].Value = "Report Name";
                                ws.Cells["A3"].Value = "Total Record Count";
                                ws.Cells["A4"].Value = "Date and Time";
                                for (int i = 2; i <= 5; i++)
                                {
                                    ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                    ws.Cells["A" + i].Style.Font.Bold = true;
                                    ws.Cells["A" + i].Style.Font.Size = 12;

                                    ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                                    ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                }
                                ws.Cells["B2"].Value = "Sales Register Report";
                                ws.Cells["B3"].Value = salesRegisterList.Count;
                                ws.Cells["B4"].Value = DateHelper.UtcToIndia(DateTime.UtcNow).ToString("dd-MM-yyyy h:mm tt");

                                ws.Cells["A4"].Style.Font.Bold = true;
                                ws.Cells["A4"].Style.Font.Size = 12;

                                #endregion

                                ExcelRange range = ws.Cells["A7:T7"];
                                range.AutoFitColumns();
                                range.Style.Font.Size = 12;
                                range.Style.Font.Bold = true;
                                int contentIndex = 8;


                                newList = resultList.Take(50000).ToList();
                                resultList = resultList.Skip(50000).ToList();

                                ws.Cells["A7:T" + (contentIndex + salesRegisterList.Count)].LoadFromCollection(newList, true);

                                ws.Cells["A7" + ":" + "T" + (salesRegisterList.Count + 7)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells.AutoFitColumns();

                            }



                        }
                        else
                        {
                            var ws = ep.Workbook.Worksheets.Add("Sales Register Report");
                            ws.Name = "Sales Register Report";


                            #region Header
                            ws.Cells["A1:F1"].Merge = true;
                            ws.Cells["A1:F1"].Value = Settings.CompanyName;
                            ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells["A1:F1"].Style.Font.Bold = true;
                            ws.Cells["A1:F1"].Style.Font.Size = 16;

                            ws.Cells["A2"].Value = "Report Name";
                            ws.Cells["A3"].Value = "Total Record Count";
                            ws.Cells["A4"].Value = "Date and Time";
                            for (int i = 2; i <= 5; i++)
                            {
                                ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                ws.Cells["A" + i].Style.Font.Bold = true;
                                ws.Cells["A" + i].Style.Font.Size = 12;

                                ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                                ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            }
                            ws.Cells["B2"].Value = "Sales Register Report";
                            ws.Cells["B3"].Value = salesRegisterList.Count;
                            ws.Cells["B4"].Value = DateHelper.UtcToIndia(DateTime.UtcNow).ToString("dd-MM-yyyy h:mm tt");

                            ws.Cells["A4"].Style.Font.Bold = true;
                            ws.Cells["A4"].Style.Font.Size = 12;

                            #endregion


                            ExcelRange range = ws.Cells["A7:U7"];
                            range.AutoFitColumns();
                            range.Style.Font.Size = 12;
                            range.Style.Font.Bold = true;
                            int contentIndex = 8;

                            ws.Cells["A7:U" + (contentIndex + salesRegisterList.Count)].LoadFromCollection(resultList, true);

                            ws.Cells["A7" + ":" + "U" + (salesRegisterList.Count + 7)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            ws.Cells.AutoFitColumns();

                        }

                      

                        guidFileName = SaveExcelFileToPath(ep);





                        #region oldcode

                        //int headerIndex = 7;

                        //ws.Cells["A" + headerIndex].Value = "Material Code";
                        //ws.Cells["B" + headerIndex].Value = "Material Name";
                        //ws.Cells["C" + headerIndex].Value = "Distributor Code";
                        //ws.Cells["D" + headerIndex].Value = "Distributor Name";
                        //ws.Cells["E" + headerIndex].Value = "QuantityMT";
                        //ws.Cells["F" + headerIndex].Value = "InvoiceType";
                        //ws.Cells["G" + headerIndex].Value = "Invoice Number";
                        //ws.Cells["H" + headerIndex].Value = "Invoice Date";
                        //ws.Cells["I" + headerIndex].Value = "Total GST";
                        //ws.Cells["J" + headerIndex].Value = "Total Amount";
                        //ws.Cells["K" + headerIndex].Value = "Sales Organization";
                        //ws.Cells["L" + headerIndex].Value = "Distribution Channel";
                        //ws.Cells["M" + headerIndex].Value = "Division";
                        //To implement filters
                        //ws.Cells["A1:Q1"].AutoFilter = true;

                        //foreach (var data in salesRegisterList)
                        //{
                        //    ws.Cells["A" + contentIndex].Value = data.MaterialCode;
                        //    ws.Cells["B" + contentIndex].Value = data.MaterialName;
                        //    ws.Cells["C" + contentIndex].Value = data.CustomerCode;
                        //    ws.Cells["D" + contentIndex].Value = data.CustomerName;
                        //    ws.Cells["E" + contentIndex].Value = data.QuantityMT;
                        //    ws.Cells["F" + contentIndex].Value = data.InvoiceType;
                        //    ws.Cells["G" + contentIndex].Value = data.InvBillNumber;
                        //    ws.Cells["H" + contentIndex].Value = data.InvoiceDate != null ? string.Format(Settings.GridDateFormatForSalesRegisterAndPendingContracts, data.InvoiceDate).ToString() : string.Empty;
                        //    ws.Cells["I" + contentIndex].Value = data.TotalGST;
                        //    ws.Cells["J" + contentIndex].Value = data.TotalAmount;
                        //    //ws.Cells["J" + contentIndex].Value = data.BillingDate != null ? string.Format(Settings.GridDateFormatForSalesRegisterAndPendingContracts, data.BillingDate).ToString() : string.Empty;
                        //    ws.Cells["K" + contentIndex].Value = data.SalesOrganization;
                        //    ws.Cells["L" + contentIndex].Value = data.DistributionChannel;
                        //    ws.Cells["M" + contentIndex].Value = data.Vertical;
                        //    //ws.Cells["N" + contentIndex].Value = data.BillToParty;
                        //    //ws.Cells["O" + contentIndex].Value = data.BillToPartyDescription;
                        //    //ws.Cells["P" + contentIndex].Value = data.ShipTo;
                        //    //ws.Cells["Q" + contentIndex].Value = data.ShipToPartyDescription;
                        //    //ws.Cells["R" + contentIndex].Value = data.MaterialCode;
                        //    //ws.Cells["S" + contentIndex].Value = data.ItemName;
                        //    //ws.Cells["T" + contentIndex].Value = data.StateofBillparty;
                        //    //ws.Cells["U" + contentIndex].Value = data.StateofShipparty;
                        //    //ws.Cells["V" + contentIndex].Value = data.Packtype;
                        //    //ws.Cells["W" + contentIndex].Value = data.PacktypeDesc;
                        //    //ws.Cells["X" + contentIndex].Value = data.PackgroupName;
                        //    //ws.Cells["Y" + contentIndex].Value = data.TotalValueWithGST;
                        //    //ws.Cells["Z" + contentIndex].Value = data.Vehicleno;
                        //    //ws.Cells["AA" + contentIndex].Value = data.TransporterName;
                        //    //ws.Cells["AB" + contentIndex].Value = data.Vertical;
                        //    //ws.Cells["AC" + contentIndex].Value = data.VerticalDesc;
                        //    //ws.Cells["AD" + contentIndex].Value = data.BillingTime;
                        //    contentIndex++;
                        //}

                        #endregion


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

        #region SalesRegisterComparison

        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult SalesRegisterComparison()
        {
            RoleIdDto roleIdDto = new RoleIdDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId
            };
            return View(roleIdDto);
        }

        public ActionResult GetSalesRegisterComparison([DataSourceRequest] DataSourceRequest request, long verticalId, DateTime fromDate, DateTime toDate, long SalesOrganizationId, long DistributionChannelId)
        {
            var pendingContractsList = _reportClient.GetSalesRegisterComparisonList(verticalId, fromDate, toDate, SalesOrganizationId, DistributionChannelId);
            var resultList = pendingContractsList.ToDataSourceResult(request);
            return Json(resultList);
        }


        public ActionResult SalesRegisterComparisonExportAsync(long VerticalId, DateTime fromDate, DateTime toDate, long SalesOrganizationId, long DistributionChannelId)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {

                var salesRegisterList = _reportClient.GetSalesRegisterComparisonList(VerticalId, fromDate, toDate, SalesOrganizationId, DistributionChannelId);

                DateTime currentDate = DateTime.Now;
                fileName = "SALES  REGISTER COMPARISON -REPORT" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";

                if (salesRegisterList != null && salesRegisterList.Any())
                {
                    string serverFolderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFolderPath + "FinalPriceTemplate.xlsx");

                    using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = "Sales Register Comparison Report";

                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A5"].Value = "Total Record Count";

                        for (int i = 2; i <= 5; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }



                        ws.Cells["B2"].Value = "Sales Register Comparision Report";
                        ws.Cells["B5"].Value = salesRegisterList.Count;

                        ws.Cells["A6"].Value = " Report from SAP data ";
                        ws.Cells["A6" + ":" + "J6"].Merge = true;
                        ws.Cells["A6"].Style.Font.Bold = true;
                        ws.Cells["A6"].Style.Font.Size = 12;
                        ws.Cells["A6" + ":" + "J6"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells["A6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["K6"].Value = " Report from Portal data ";
                        ws.Cells["K6" + ":" + "L6"].Merge = true;
                        ws.Cells["K6"].Style.Font.Bold = true;
                        ws.Cells["K6"].Style.Font.Size = 12;
                        ws.Cells["K6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["K6" + ":" + "L6"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        #endregion


                        int headerIndex = 7;
                        ws.Cells["A" + headerIndex].Value = "Billing Type";
                        ws.Cells["B" + headerIndex].Value = "Contract Number";
                        ws.Cells["C" + headerIndex].Value = "Do Number";
                        ws.Cells["D" + headerIndex].Value = "Bill Number";
                        ws.Cells["E" + headerIndex].Value = "Billing Date";
                        ws.Cells["F" + headerIndex].Value = "Quantity In Case";
                        ws.Cells["G" + headerIndex].Value = "Quantity";
                        ws.Cells["H" + headerIndex].Value = "Oil Type Desc";
                        ws.Cells["I" + headerIndex].Value = "Bill To Party";
                        ws.Cells["J" + headerIndex].Value = "Bill To Party Description";
                        ws.Cells["K" + headerIndex].Value = "State of Shipparty";
                        ws.Cells["L" + headerIndex].Value = "Bill Number";
                        ws.Cells["M" + headerIndex].Value = "Quantity In Case";
                        ws.Cells["N" + headerIndex].Value = "Status";
                        ws.Cells["O" + headerIndex].Value = "ActionToTaken";




                        ExcelRange range = ws.Cells["A7:BJ7"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        int contentIndex = 8;

                        //To implement filters
                        ws.Cells["A1:Q1"].AutoFilter = true;

                        foreach (var data in salesRegisterList)
                        {
                            ws.Cells["A" + contentIndex].Value = data.BillingType;
                            ws.Cells["B" + contentIndex].Value = data.Contractnumber;
                            ws.Cells["C" + contentIndex].Value = data.DoNumber;
                            ws.Cells["D" + contentIndex].Value = data.BillNumber;
                            ws.Cells["E" + contentIndex].Value = data.BillingDate != null ? string.Format(Settings.GridDateFormat, data.BillingDate).ToString() : string.Empty;
                            ws.Cells["F" + contentIndex].Value = data.QuantityCase;
                            ws.Cells["G" + contentIndex].Value = data.QuantityMT;
                            ws.Cells["H" + contentIndex].Value = data.OilTypeDesc;
                            ws.Cells["I" + contentIndex].Value = data.BillToParty;
                            ws.Cells["J" + contentIndex].Value = data.BillToPartyDescription;
                            ws.Cells["K" + contentIndex].Value = data.StateofShipparty;
                            ws.Cells["L" + contentIndex].Value = data.InvBillNumber;
                            ws.Cells["M" + contentIndex].Value = data.InvQuantityInCase;
                            ws.Cells["N" + contentIndex].Value = data.Status;
                            ws.Cells["O" + contentIndex].Value = data.ActionToTaken;
                            contentIndex++;
                        }
                        ws.Cells["A7" + ":" + "O" + contentIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
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

        #region RA Sauda Report

        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult RaSaudaOrderReport()
        {
            return View();
        }

        public ActionResult GetRaSaudaOrderDetailsReport(DateTime fromDate, DateTime toDate, List<long> stateIds, int verticalId, List<long> statusIds)
        {
            string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
            var saudaResult = _reportClient.GetRaSaudaOrderReport(fromDate, toDate, stateIds, verticalId, statusIds);

            DateTime currentDate = DateTime.Now;
            string fileName = "RA-SAUDA-REPORT" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
            var guidFileName = $"{Guid.NewGuid()}.xlsx";

            using (var ep = new ExcelPackage())
            {
                var ws = ep.Workbook.Worksheets.Add("SaudaOrders");

                //Header
                ws.Cells["A1:BZ1"].Style.Font.Size = 13;
                ws.Cells["A1:BZ1"].Style.Font.Name = "Calibri";
                ws.Cells["A1:BZ1"].Style.Font.Bold = true;

                ws.Cells.LoadFromCollection(saudaResult, true);
                ws.Cells.AutoFitColumns();
                guidFileName = SaveExcelFileToPath(ep);
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Sauda Conversion Report 

        [AuthorizeClaims(Claims.SaudaConversionReport)]
        public ActionResult SaudaConversionReport()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }

        public async Task<ActionResult> GetSaudaConversionReport(SaudaConversionReportInputDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                string serverFolderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                var saudaConversionDetails = await _reportClient.GetSaudaConversionReport(inputDto);

                if (saudaConversionDetails != null && saudaConversionDetails.Any())
                {
                    DateTime currentDate = DateTime.Now;
                    fileName = "SaudaConversionReport_" + string.Format("{0:dd-MMM-yyyy hh:mm:ss}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                    guidFileName = $"{Guid.NewGuid()}.xlsx";
                    // Create the package and make sure you wrap it in a using statement
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("SaudaConversionReport");
                        #region Header
                        worksheet.Cells["A1:F1"].Merge = true;
                        worksheet.Cells["A1:F1"].Value = Settings.CompanyName;
                        worksheet.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1:F1"].Style.Font.Bold = true;
                        worksheet.Cells["A1:F1"].Style.Font.Size = 16;

                        worksheet.Cells["A2"].Value = "Report Name";
                        worksheet.Cells["A3"].Value = "From Date";
                        worksheet.Cells["A4"].Value = "To Date";
                        worksheet.Cells["A5"].Value = "Total Record Count";
                        worksheet.Cells["A6"].Value = "Status";

                        for (int i = 2; i <= 6; i++)
                        {
                            worksheet.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheet.Cells["A" + i].Style.Font.Bold = true;
                            worksheet.Cells["A" + i].Style.Font.Size = 12;

                            worksheet.Cells["B" + i + ":" + "F" + i].Merge = true;
                            worksheet.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        worksheet.Cells["B2"].Value = "Sauda Conversion Report";
                        worksheet.Cells["B3"].Value = Settings.DateFormats(inputDto.FromDate, Settings.ReportDateFormat).ToUpper();
                        worksheet.Cells["B4"].Value = Settings.DateFormats(inputDto.ToDate, Settings.ReportDateFormat).ToUpper();
                        worksheet.Cells["B5"].Value = saudaConversionDetails.Count;
                        worksheet.Cells["B6"].Value = inputDto.StatusIds.Count == 0 ? "All" : inputDto.StatusIds.Contains((int)DTO.Enums.Status.Pending) ? "Pending" : "Approved";
                        #endregion
                        var rowIndex = 8;
                        foreach (var item in saudaConversionDetails)
                        {
                            // add a new worksheet to the empty workbook
                            //Complaint form name as Worksheet name
                            Response.ClearHeaders();
                            Response.ClearContent();
                            Response.Clear();

                            var colIndex = 1;

                            GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "SaudaConversion Number");
                            GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DealerName"));
                            GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ZonalHead"));
                            GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BDOName"));
                            GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ConversionDate"));
                            GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Sku"));
                            GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PlantOrDepotCode"));
                            GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PlantOrDepotName"));
                            GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SaudaConversionQuatityInMt"));
                            GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SaudaConversionQuantityInCase"));
                            GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Remarks"));

                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.SkuConversionId.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.DealerName);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ZonalHeadName);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.BDOName);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ConversionCreatedDate.ToString("dd'/'MM'/'yyyy hh:mm tt"));
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.SkuName);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.PlantOrDepotCode);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.PlantOrDepotName);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.SaudaQuantityInMT.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.SaudaQuantityInSku.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Remarks);

                            var childcolumnIndex = 2;
                            rowIndex++;
                            var subgrid = item.FromSkus;
                            GetExcelTitle(worksheet.Cells[rowIndex, childcolumnIndex++], "From Sku");
                            GetExcelTitle(worksheet.Cells[rowIndex, childcolumnIndex++], @Helper.GetResourceString("lbl_SaudaNumber"));
                            GetExcelTitle(worksheet.Cells[rowIndex, childcolumnIndex++], @Helper.GetResourceString("lbl_SaudaConversionQuatityInMt"));
                            GetExcelTitle(worksheet.Cells[rowIndex, childcolumnIndex++], @Helper.GetResourceString("lbl_SaudaConversionQuantityInCase"));
                            GetExcelTitle(worksheet.Cells[rowIndex, childcolumnIndex++], @Helper.GetResourceString("lbl_BaseRate"));
                            GetExcelTitle(worksheet.Cells[rowIndex, childcolumnIndex++], @Helper.GetResourceString("lbl_Remarks"));

                            foreach (var child in subgrid)
                            {
                                rowIndex++;
                                childcolumnIndex = 2;
                                GetExcelContent(worksheet.Cells[rowIndex, childcolumnIndex++], child.SkuName);
                                GetExcelContent(worksheet.Cells[rowIndex, childcolumnIndex++], child.SaudaNumber);
                                GetExcelContent(worksheet.Cells[rowIndex, childcolumnIndex++], child.SaudaQuantityInMT.ToString());
                                GetExcelContent(worksheet.Cells[rowIndex, childcolumnIndex++], child.SaudaQuantityInSku.ToString());
                                GetExcelContent(worksheet.Cells[rowIndex, childcolumnIndex++], child.BaseRate.ToString());
                                GetExcelContent(worksheet.Cells[rowIndex, childcolumnIndex++], child.Remarks);
                            }


                            rowIndex++;
                            childcolumnIndex = 2;
                            var subgrid1 = item.ToSkus;
                            GetExcelTitle(worksheet.Cells[rowIndex, childcolumnIndex++], "To Sku");
                            GetExcelTitle(worksheet.Cells[rowIndex, childcolumnIndex++], @Helper.GetResourceString("lbl_SaudaNumber"));
                            GetExcelTitle(worksheet.Cells[rowIndex, childcolumnIndex++], @Helper.GetResourceString("lbl_SaudaConversionQuatityInMt"));
                            GetExcelTitle(worksheet.Cells[rowIndex, childcolumnIndex++], @Helper.GetResourceString("lbl_SaudaConversionQuantityInCase"));
                            GetExcelTitle(worksheet.Cells[rowIndex, childcolumnIndex++], @Helper.GetResourceString("lbl_BaseRate"));
                            GetExcelTitle(worksheet.Cells[rowIndex, childcolumnIndex++], @Helper.GetResourceString("lbl_Remarks"));


                            foreach (var child in subgrid1)
                            {
                                rowIndex++;
                                childcolumnIndex = 2;
                                GetExcelContent(worksheet.Cells[rowIndex, childcolumnIndex++], child.SkuName);
                                GetExcelContent(worksheet.Cells[rowIndex, childcolumnIndex++], child.SaudaNumber);
                                GetExcelContent(worksheet.Cells[rowIndex, childcolumnIndex++], child.SaudaQuantityInMT.ToString());
                                GetExcelContent(worksheet.Cells[rowIndex, childcolumnIndex++], child.SaudaQuantityInSku.ToString());
                                GetExcelContent(worksheet.Cells[rowIndex, childcolumnIndex++], child.BaseRate.ToString());
                                GetExcelContent(worksheet.Cells[rowIndex, childcolumnIndex++], child.Remarks);
                            }
                            rowIndex++;
                        }
                        rowIndex++;

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
                        string savePath = Path.Combine(serverFolderPath, guidFileName);
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

            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Sauda Modification Report

        [AuthorizeClaims(Claims.SaudaConversionReport)]
        public ActionResult SaudaModificationReport()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }

        public async Task<ActionResult> GetSaudaModificationReport(DateTime fromDate, DateTime toDate, List<long> stateIds, int verticalId, List<long> statusIds, long salesorganizationId, long distributionChannelId)
        {
            _methodName = "GetSaudaModificationReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "SAUDA-MODIFICATION-REPORT-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";

            try
            {
                SaudaOrderReportInputputDto inputputDto = new SaudaOrderReportInputputDto() { RoleId = RoleId, FromDate = fromDate, ToDate = toDate, StateIds = stateIds, VerticalId = verticalId, StatusIds = statusIds, SalesOrganizationId = salesorganizationId, DistributionChannelId = distributionChannelId, LoginUserId = UserId };
                var publishData = await _reportClient.GetSaudaModificationReportAsync(inputputDto);

                if (publishData != null && publishData.Any())
                {
                    string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFoloderPath + "FinalPriceTemplate.xlsx");

                    using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = "Sauda Modification";

                        #region Header
                        ws.Cells["A1:Z1"].Merge = true;
                        ws.Cells["A1:Z1"].Value = Settings.CompanyName;
                        ws.Cells["A1:Z1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:Z1"].Style.Font.Bold = true;
                        ws.Cells["A1:Z1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["B2"].Value = "Sauda Modification Report";
                        ws.Cells["A3"].Value = "From Date";
                        ws.Cells["B3"].Value = fromDate.ToString(Settings.DateFormat);
                        ws.Cells["A4"].Value = "To Date";
                        ws.Cells["B4"].Value = toDate.ToString(Settings.DateFormat);
                        ws.Cells["A5"].Value = "Total Record Count";
                        ws.Cells["B5"].Value = publishData.Count();

                        for (int i = 2; i <= 5; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;
                        }
                        #endregion

                        #region Column Headers
                        int rowIndex = 7;
                        ws.Cells[rowIndex, 1].Value = "Sauda Number";
                        ws.Cells[rowIndex, 2].Value = "Booked No";
                        ws.Cells[rowIndex, 3].Value = "Modification No";
                        ws.Cells[rowIndex, 4].Value = "Modification Date";
                        ws.Cells[rowIndex, 5].Value = "Dealer Name";
                        ws.Cells[rowIndex, 6].Value = "Zone";
                        ws.Cells[rowIndex, 7].Value = "State";
                        ws.Cells[rowIndex, 8].Value = "District";
                        ws.Cells[rowIndex, 9].Value = "City";
                        ws.Cells[rowIndex, 10].Value = "Oil Type";
                        ws.Cells[rowIndex, 11].Value = "Pack Type";
                        ws.Cells[rowIndex, 12].Value = "Material Name";
                        ws.Cells[rowIndex, 13].Value = "Material Code";
                        ws.Cells[rowIndex, 14].Value = "Quantity In Case";
                        ws.Cells[rowIndex, 15].Value = "Quantity In MT";
                        ws.Cells[rowIndex, 16].Value = "Price";
                        ws.Cells[rowIndex, 17].Value = "Discount";
                        ws.Cells[rowIndex, 18].Value = "Status";
                        ws.Cells[rowIndex, 19].Value = "Created By";

                        // Style header row
                        ws.Cells[rowIndex, 1, rowIndex, 18].Style.Font.Bold = true;
                        ws.Cells[rowIndex, 1, rowIndex, 18].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[rowIndex, 1, rowIndex, 18].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        ws.Cells[rowIndex, 1, rowIndex, 18].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        #endregion

                        #region Data Rows
                        rowIndex = 8;
                        foreach (var item in publishData)
                        {
                            ws.Cells[rowIndex, 1].Value = item.SaudaNumber ?? "";
                            ws.Cells[rowIndex, 2].Value = item.SaudaBookedNumber;
                            ws.Cells[rowIndex, 3].Value = item.SaudaModificationNumber;
                            ws.Cells[rowIndex, 4].Value = item.ModificationDate != null ? item.ModificationDate.Value.ToString(Settings.DateFormat) : "";
                            ws.Cells[rowIndex, 5].Value = item.DealerName ?? "";
                            ws.Cells[rowIndex, 6].Value = item.Zone ?? "";
                            ws.Cells[rowIndex, 7].Value = item.State ?? "";
                            ws.Cells[rowIndex, 8].Value = item.District ?? "";
                            ws.Cells[rowIndex, 9].Value = item.City ?? "";
                            ws.Cells[rowIndex, 10].Value = item.OilTypeName ?? "";
                            ws.Cells[rowIndex, 11].Value = item.OilPackGroupTypeName ?? "";
                            ws.Cells[rowIndex, 12].Value = item.MaterialName ?? "";
                            ws.Cells[rowIndex, 13].Value = item.MaterialCode ?? "";
                            ws.Cells[rowIndex, 14].Value = item.QuantityInCase;
                            ws.Cells[rowIndex, 15].Value = item.QuantityInMT;
                            ws.Cells[rowIndex, 16].Value = item.Price;
                            ws.Cells[rowIndex, 17].Value = item.Discount;
                            ws.Cells[rowIndex, 18].Value = item.Status ?? "";
                            ws.Cells[rowIndex, 19].Value = item.CreatedBy ?? "";

                            rowIndex++;
                        }
                        #endregion

                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }
                else
                {
                    guidFileName = "";
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region New Sauda Order Report

        [AuthorizeClaims(Claims.NewSaudaReport)]
        public ActionResult NewSaudaReport()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }

        public async Task<ActionResult> GetNewSaudaReport(DateTime fromDate, DateTime toDate, long verticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                // var vertical = VerticalId;
                var loginUserId = UserId;
                var roleId = RoleId;
                var saudaList = _reportClient.GetNewSaudaReport(verticalId, fromDate, toDate, loginUserId, roleId, SalesOrganizationId, DistributionChannelId);

                DateTime currentDate = DateTime.Now;
                fileName = "NEW SAUDA -REPORT" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";

                if (saudaList != null && saudaList.Any())
                {
                    string serverFolderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFolderPath + "FinalPriceTemplate.xlsx");

                    using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = "New Sauda Report";

                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "Total Record Count";

                        for (int i = 2; i <= 3; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }



                        ws.Cells["B2"].Value = "New Sauda Report";
                        ws.Cells["B3"].Value = saudaList.Count;
                        #endregion


                        int headerIndex = 5;
                        ws.Cells["A" + headerIndex].Value = "Sauda Number";
                        ws.Cells["B" + headerIndex].Value = "Booked Number";
                        ws.Cells["C" + headerIndex].Value = "Plant";
                        ws.Cells["D" + headerIndex].Value = "OilType";
                        ws.Cells["E" + headerIndex].Value = "Sku Name";
                        ws.Cells["F" + headerIndex].Value = "Sku Code";
                        ws.Cells["G" + headerIndex].Value = "Quantity In Case";
                        ws.Cells["H" + headerIndex].Value = "Quantity";
                        ws.Cells["I" + headerIndex].Value = "Bidding Date";
                        ws.Cells["J" + headerIndex].Value = "Dealer Name";
                        ws.Cells["K" + headerIndex].Value = "Sauda Bid Price";
                        ws.Cells["L" + headerIndex].Value = "IncoTrems";
                        ws.Cells["M" + headerIndex].Value = "Freight Route";
                        ws.Cells["N" + headerIndex].Value = "Status";
                        ws.Cells["O" + headerIndex].Value = "Sauda Booking Type";
                        ws.Cells["P" + headerIndex].Value = "Created By";
                        ws.Cells["Q" + headerIndex].Value = "State";
                        ws.Cells["R" + headerIndex].Value = "StateTrader Name";
                        ws.Cells["S" + headerIndex].Value = "StateTrader Code";
                        ws.Cells["T" + headerIndex].Value = "Dealer Code";




                        ExcelRange range = ws.Cells["A5:T5"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        int contentIndex = 6;

                        //To implement filters
                        ws.Cells["A1:Q1"].AutoFilter = true;

                        foreach (var data in saudaList)
                        {
                            ws.Cells["A" + contentIndex].Value = data.SaudaNumber != null ? data.SaudaNumber : string.Empty;
                            ws.Cells["B" + contentIndex].Value = data.BookedNumber;
                            ws.Cells["C" + contentIndex].Value = data.Plant;
                            ws.Cells["D" + contentIndex].Value = data.OilTypeName;
                            ws.Cells["E" + contentIndex].Value = data.SkuName;
                            ws.Cells["F" + contentIndex].Value = data.SkuCode;
                            ws.Cells["G" + contentIndex].Value = data.QuantityInCase;
                            ws.Cells["H" + contentIndex].Value = data.QuantityInMT;
                            ws.Cells["I" + contentIndex].Value = data.BiddingDate != null ? string.Format(Settings.GridDateFormat, data.BiddingDate).ToString() : string.Empty; ;
                            ws.Cells["J" + contentIndex].Value = data.DealerName;
                            ws.Cells["K" + contentIndex].Value = data.SaudaBidPrice;
                            ws.Cells["L" + contentIndex].Value = data.Incoterms;
                            ws.Cells["M" + contentIndex].Value = data.FreightRoute;
                            ws.Cells["N" + contentIndex].Value = data.Status;
                            ws.Cells["O" + contentIndex].Value = data.BookingType;
                            ws.Cells["P" + contentIndex].Value = data.CreatedBy;
                            ws.Cells["Q" + contentIndex].Value = data.State;
                            ws.Cells["R" + contentIndex].Value = data.BdoName;
                            ws.Cells["S" + contentIndex].Value = data.BdoCode;
                            ws.Cells["T" + contentIndex].Value = data.DealerCode;
                            contentIndex++;
                        }
                        ws.Cells["A5" + ":" + "T5" + contentIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
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

        #region Call Recording Details Report

        [AuthorizeClaims(Claims.CallRecordingReport)]
        public ActionResult CallRecordingReport()
        {
            RoleIdDto roleIdDto = new RoleIdDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId
            };
            return View(roleIdDto);
        }

        public ActionResult GetCallRecordingDetails([DataSourceRequest] DataSourceRequest request, DateTime fromDate, DateTime toDate, List<long> ZonalHeadIds, List<long> BdoIds, List<long> DealerIds, long verticalId, long SalesOrganizationId, long DistributionChannelId)
        {

            var callRecordingList = _reportClient.GetCallRecordingList(fromDate, toDate, ZonalHeadIds, BdoIds, DealerIds, verticalId, UserId, SalesOrganizationId, DistributionChannelId);
            var resultList = callRecordingList.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> GetCallRecordingDetailsReport(DateTime fromDate, DateTime toDate, List<long> ZonalHeadIds, List<long> BdoIds, List<long> DealerIds, long verticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {


                //var vertical = VerticalId;
                var loginUserId = UserId;
                var roleId = RoleId;
                var callRecordingList = _reportClient.GetCallRecordingList(fromDate, toDate, ZonalHeadIds, BdoIds, DealerIds, verticalId, loginUserId, SalesOrganizationId, DistributionChannelId);

                DateTime currentDate = DateTime.Now;
                fileName = "Call Recording Details -REPORT" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";

                if (callRecordingList.IsAny())
                {
                    string serverFolderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFolderPath + "FinalPriceTemplate.xlsx");

                    var resultList = callRecordingList.Select(s => new CallRecordingExportDto()
                    {
                        CalledBy=s.CalledBy,
                        CalledTo=s.CalledTo,
                        CallRecordedTime= s.CallRecordedDate != null ? string.Format(Settings.GridDateFormat, s.CallRecordedDate).ToString() : string.Empty,
                        CallRecordingDate = s.CallRecordedDate != null ? string.Format(Settings.GridTimeFormat12, s.CallRecordedDate).ToString() : string.Empty,
                        Audiofile= !string.IsNullOrEmpty(s.CallRecordedFileName) ? ConfigHelper.AudioFileInExcelUrl + s.CallRecordedFileName + "&fileDownloadName=" + s.CalledTo + "_" + string.Format(Settings.GridDateTimeFormat, s.CallRecordedDate).ToString() :String.Empty,
                    }).ToList();

                    using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = "Call Recording Details";

                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "Total Record Count";

                        for (int i = 2; i <= 3; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }



                        ws.Cells["B2"].Value = "Call Recording Details";
                        ws.Cells["B3"].Value = callRecordingList.Count;
                        #endregion

                        ws.Cells["A5:F" + (5 + callRecordingList.Count())].LoadFromCollection(resultList,true);
                        //foreach (var data in callRecordingList)
                        //{

                        //    var audioFilesNetworkPath = "";
                        //    if (!string.IsNullOrEmpty(data.CallRecordedFileName))
                        //    {
                        //        var networkPathAudioFile = ConfigHelper.AudioFileInExcelUrl + data.CallRecordedFileName + "&fileDownloadName=" + data.CalledTo + "_" + string.Format(Settings.GridDateTimeFormat, data.CallRecordedDate).ToString();
                        //        if (!string.IsNullOrEmpty(networkPathAudioFile))
                        //        {
                        //            audioFilesNetworkPath = networkPathAudioFile;
                        //        }

                        //    }
                        //    ws.Cells["A" + contentIndex].Value = data.CalledBy;
                        //    ws.Cells["B" + contentIndex].Value = data.CalledTo;
                        //    ws.Cells["C" + contentIndex].Value = data.CallRecordedDate != null ? string.Format(Settings.GridDateFormat, data.CallRecordedDate).ToString() : string.Empty;
                        //    ws.Cells["D" + contentIndex].Value = data.CallRecordedDate != null ? string.Format(Settings.GridTimeFormat12, data.CallRecordedDate).ToString() : string.Empty;
                        //    ws.Cells["E" + contentIndex].Value = audioFilesNetworkPath != null ? audioFilesNetworkPath : string.Empty;
                        //    //ws.Cells["F" + contentIndex].Value = data.ZonalHeadName;
                        //    contentIndex++;
                        //}

                        //int headerIndex = 5;
                        //ws.Cells["A" + headerIndex].Value = "Called By";
                        //ws.Cells["B" + headerIndex].Value = "Called To";
                        //ws.Cells["C" + headerIndex].Value = "Call Recorded Date";
                        //ws.Cells["D" + headerIndex].Value = "Call Recorded Time";
                        //ws.Cells["E" + headerIndex].Value = "Audio File";
                        //ws.Cells["F" + headerIndex].Value = "Zonal Head Name";

                        ExcelRange range = ws.Cells["A5:T5"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        int contentIndex = 6;

                        //To implement filters
                        //ws.Cells["A1:Q1"].AutoFilter = true;

                       
                        //ws.Cells["A5" + ":" + "T5" + contentIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
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

        public JsonResult GetCallRecordedFileName(long audioFileId)
        {
            JsonResult jsonResult = new JsonResult();
            try
            {
                var callRecordedFileName = _reportClient.GetCallRecordedFileName(audioFileId);
                var resultList = callRecordedFileName;
                return Json(resultList);
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
            return jsonResult;
        }

        [AuthorizeClaims(Claims.CallRecordingReport)]
        public ActionResult SaudaCallRecordMappingReport()
        {
            RoleIdDto roleIdDto = new RoleIdDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId
            };
            return View(roleIdDto);
        }

        public ActionResult GetSaudaCallRecordMappingDetails([DataSourceRequest] DataSourceRequest request, DateTime fromDate, DateTime toDate, List<long> ZonalHeadIds, List<long> BdoIds, List<long> DealerIds, long VerticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            var callRecordingList = _reportClient.GetSaudaCallRecordMappingList(fromDate, toDate, ZonalHeadIds, BdoIds, DealerIds, VerticalId, SalesOrganizationId, DistributionChannelId);
            var resultList = callRecordingList.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> GetSaudaCallRecordMappingReport(DateTime fromDate, DateTime toDate, List<long> ZonalHeadIds, List<long> BdoIds, List<long> DealerIds, long VerticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                var resultList = new List<CallRecordMapDto>();
                var vertical = VerticalId;
                var loginUserId = UserId;
                var roleId = RoleId;
                var callRecordingList = _reportClient.GetSaudaCallRecordMappingList(fromDate, toDate, ZonalHeadIds, BdoIds, DealerIds, vertical, SalesOrganizationId, DistributionChannelId);

                DateTime currentDate = DateTime.Now;
                fileName = "Sauda Call Record Mapping Details -REPORT" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";

                if (callRecordingList.IsAny())
                {
                    string serverFolderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFolderPath + "FinalPriceTemplate.xlsx");

                    using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = "Sauda Call Record Mapping Details";

                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "Total Record Count";

                        for (int i = 2; i <= 3; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }



                        ws.Cells["B2"].Value = "Sauda Call Record Mapping Details";
                        ws.Cells["B3"].Value = callRecordingList.Count;
                        #endregion


                        //int headerIndex = 5;
                        //ws.Cells["A" + headerIndex].Value = "Customer Code";
                        //ws.Cells["B" + headerIndex].Value = "Customer Name";
                        //ws.Cells["C" + headerIndex].Value = "ZonalTrader Name";
                        //ws.Cells["D" + headerIndex].Value = "StateTrader Name";
                        //ws.Cells["E" + headerIndex].Value = "Sauda BookingId";
                        //ws.Cells["F" + headerIndex].Value = "Sauda Number";
                        //ws.Cells["G" + headerIndex].Value = "Sauda Booked Date";
                        //ws.Cells["H" + headerIndex].Value = "Images";
                        //ws.Cells["I" + headerIndex].Value = "Audio Files";

                        ExcelRange range = ws.Cells["A5:T5"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        int contentIndex = 6;

                        //To implement filters
                        //ws.Cells["A1:Q1"].AutoFilter = true;

                        foreach (var data in callRecordingList)
                        {
                            var mapData = new CallRecordMapDto();
                            List<string> ImageNetworkPath = new List<string>();
                            List<string> AudioFilesNetworkPath = new List<string>();
                            var ImagePathsList = string.IsNullOrEmpty(data.ImagePaths) ? new List<string>() : data.ImagePaths.Split(',').ToList();
                            var AudioFilesList = string.IsNullOrEmpty(data.AudioFiles) ? new List<string>() : data.AudioFiles.Split(',').ToList();
                            if (ImagePathsList.IsAny())
                            {
                                foreach (var item in ImagePathsList)
                                {
                                    var networkPath = Settings.GetMediaPathForExcelCallRecording((int)PageType.ImagesSaudaMappingwithCallRecording, item);
                                    if (!string.IsNullOrEmpty(networkPath))
                                    {
                                        ImageNetworkPath.Add(networkPath);
                                    }
                                }
                            }

                            if (AudioFilesList.IsAny())
                            {
                                foreach (var audio in AudioFilesList)
                                {
                                    var audioName = audio.Split('_');
                                    var networkPathAudioFile = ConfigHelper.AudioFileInExcelUrl + audioName[0] + "&fileDownloadName=" + data.CustomerName + "_" + string.Format(Settings.GridDateTimeFormat, audioName[1]).ToString();
                                    if (!string.IsNullOrEmpty(networkPathAudioFile))
                                    {
                                        AudioFilesNetworkPath.Add(networkPathAudioFile);
                                    }
                                }
                            }

                            mapData.CustomerCode = data.CustomerCode;
                            mapData.CustomerName = data.CustomerName;
                            mapData.ZonaltradeName = data.ZonalHeadName;
                            mapData.StateTraderName = data.BdoName;
                            mapData.SaudaBookinId = data.SaudaId.ToString();
                            mapData.SaudaNumber = data.SaudaNumber;
                            mapData.SaudaBookedDate = data.SaudaBookedDate != null ? string.Format(Settings.GridDateFormat, data.SaudaBookedDate).ToString() : string.Empty;
                            mapData.Images= ImageNetworkPath.IsAny() ? string.Join(",", ImageNetworkPath) : string.Empty;
                            mapData.AudioFiles= AudioFilesNetworkPath.IsAny() ? string.Join(",", AudioFilesNetworkPath) : string.Empty;

                            resultList.Add(mapData);
                            //ws.Cells["A" + contentIndex].Value = data.CustomerCode;
                            //ws.Cells["B" + contentIndex].Value = data.CustomerName;
                            //ws.Cells["C" + contentIndex].Value = data.ZonalHeadName;
                            //ws.Cells["D" + contentIndex].Value = data.BdoName;
                            //ws.Cells["E" + contentIndex].Value = data.SaudaId > 0 ? data.SaudaId.ToString() : string.Empty;
                            //ws.Cells["F" + contentIndex].Value = data.SaudaNumber != null ? data.SaudaNumber : string.Empty;
                            //ws.Cells["G" + contentIndex].Value = data.SaudaBookedDate != null ? string.Format(Settings.GridDateFormat, data.SaudaBookedDate).ToString() : string.Empty;
                            //ws.Cells["H" + contentIndex].Value = ImageNetworkPath.IsAny() ? string.Join(",", ImageNetworkPath) : string.Empty;
                            //ws.Cells["I" + contentIndex].Value = AudioFilesNetworkPath.IsAny() ? string.Join(",", AudioFilesNetworkPath) : string.Empty;
                            //contentIndex++;
                        }

                        ws.Cells["A5:I" + (5 + resultList.Count())].LoadFromCollection(resultList, true);
                        //ws.Cells["A5" + ":" + "T5" + contentIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
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

        public ActionResult GetCallRecordedBasedOnSauda(long saudaId)
        {
            var resultList = new CallRecordingDto();
            try
            {
                var callRecordedFileName = _reportClient.GetCallRecordedBasedOnSauda(saudaId);
                resultList = callRecordedFileName.Result;
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
            return PartialView("_CallRecordingAttachmentDetails", resultList);
        }

        public ActionResult GetCallRecordedAttachmentsBasedOnSaudaForBulkDownload(string EncryptedId)
        {

            long saudaId = 0;

            if (!String.IsNullOrEmpty(EncryptedId))
            {
                EncryptedId = EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey);

                saudaId = UtilityHelper.IntTryToParse(decryptedId);
            }

            var resultList = new CallRecordingDto();
            var callRecordedFileName = _reportClient.GetCallRecordedBasedOnSauda(saudaId);
            resultList = callRecordedFileName.Result;
            DateTime currentDate = DateTime.Now;
            int contentType;
            var zipFilename = "Attachments_" + resultList.FileDownloadName + "_" + string.Format("{0:dd-MMM-yyyy hh:mm tt}", resultList.CallRecordedDate) + ".zip";
            using (var memoryStream = new MemoryStream())
            {
                if (resultList.CallRecordingListOutput.IsAny())
                {
                    using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var data in resultList.CallRecordingListOutput)
                        {
                            if (data.MediaTypeId == (int)MediaType.Audio)
                            {
                                contentType = (int)PageType.AudioFiles;
                            }
                            else
                            {
                                contentType = (int)PageType.ImagesSaudaMappingwithCallRecording;
                            }
                            var attachmentWithNetworkPath = Settings.GetMediaPathForBulkDownload(contentType, data.CallRecordedFileName);
                            ziparchive.CreateEntryFromFile(attachmentWithNetworkPath, data.CallRecordedFileName);
                        }
                    }
                }
                return File(memoryStream.ToArray(), "application/zip", zipFilename);
            }
        }
        #endregion

        #region Daily Booking Report

        [AuthorizeClaims(Claims.DailyBookingReport)]
        public ActionResult DailyBookingReport()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }

        public async Task<ActionResult> GetDailyBookingReport(DateTime fromDate, DateTime toDate, List<long> stateIds, long verticalId, List<long> statusIds, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetDailyBookingReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "Daily Booking-REPORT-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";

            try
            {
                SaudaOrderReportInputputDto inputputDto = new SaudaOrderReportInputputDto() { FromDate = fromDate, ToDate = toDate, StateIds = stateIds, VerticalId = verticalId, StatusIds = statusIds, SalesOrganizationId = SalesOrganizationId, DistributionChannelId = DistributionChannelId };
                var publishData = await _reportClient.GetDailyBookingReport(inputputDto);

                if (publishData != null && publishData.Any())
                {
                    var result = publishData.FirstOrDefault();

                    string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFoloderPath + "FinalPriceTemplate.xlsx");

                    using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = "Daily Booking";

                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "From Date";
                        ws.Cells["A4"].Value = "To Date";
                        ws.Cells["A5"].Value = "Total Record Count";
                        ws.Cells["A6"].Value = "Vertical";

                        for (int i = 2; i <= 6; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        ws.Cells["B2"].Value = "Daily Booking";
                        ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B5"].Value = publishData.Count;
                        ws.Cells["B6"].Value = verticalId == (int)DTO.Enums.Division.Hbc ? "Hbc" : verticalId == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        #endregion

                        int headerIndex = 8;
                        ws.Cells["A" + headerIndex].Value = "Product Group";
                        ws.Cells["B" + headerIndex].Value = "Material Description";
                        ws.Cells["C" + headerIndex].Value = "Material Code";
                        ws.Cells["D" + headerIndex].Value = "Material Qty";
                        ws.Cells["E" + headerIndex].Value = "UOM";
                        ws.Cells["F" + headerIndex].Value = "Material Qty(MT)";
                        ws.Cells["G" + headerIndex].Value = "Product Group";
                        ws.Cells["H" + headerIndex].Value = "State";
                        ws.Cells["I" + headerIndex].Value = "Customer Code";
                        ws.Cells["J" + headerIndex].Value = "Customer Name";
                        ws.Cells["K" + headerIndex].Value = "Route Name";
                        ws.Cells["L" + headerIndex].Value = "Plant Name";
                        ws.Cells["M" + headerIndex].Value = "Incoterms";
                        ws.Cells["N" + headerIndex].Value = "Depot Code";
                        ws.Cells["O" + headerIndex].Value = "Depot Name";
                        ws.Cells["P" + headerIndex].Value = "Broker Code";
                        ws.Cells["Q" + headerIndex].Value = "Broker Name";
                        ws.Cells["R" + headerIndex].Value = "App Contract Time";
                        ws.Cells["S" + headerIndex].Value = "App Contract Date";
                        ws.Cells["T" + headerIndex].Value = "Contract Valid From";
                        ws.Cells["U" + headerIndex].Value = "Contract Valid To";
                        ws.Cells["V" + headerIndex].Value = "Material Cost";
                        ws.Cells["W" + headerIndex].Value = "Premium";
                        ws.Cells["X" + headerIndex].Value = "TD";
                        ws.Cells["Y" + headerIndex].Value = "LTD";
                        ws.Cells["Z" + headerIndex].Value = "Margin Cost TP";
                        ws.Cells["AA" + headerIndex].Value = "Packing Cost";
                        ws.Cells["AB" + headerIndex].Value = "Honeycomb cost";
                        ws.Cells["AC" + headerIndex].Value = "Primary Freight";
                        ws.Cells["AD" + headerIndex].Value = "Secondary Freight";
                        ws.Cells["AE" + headerIndex].Value = "Depot Cost";
                        ws.Cells["AF" + headerIndex].Value = "Detention charges";
                        ws.Cells["AG" + headerIndex].Value = "PR00";
                        ws.Cells["AH" + headerIndex].Value = "FRC1";
                        ws.Cells["AI" + headerIndex].Value = "Sale Rate";
                        ws.Cells["AJ" + headerIndex].Value = "Total Value";
                        ws.Cells["AK" + headerIndex].Value = "Vertical";
                        ws.Cells["AL" + headerIndex].Value = "Actual Packing Cost";
                        ws.Cells["AM" + headerIndex].Value = "Employee Code";
                        ws.Cells["AN" + headerIndex].Value = "Employee Name";
                        ws.Cells["AO" + headerIndex].Value = "Remarks";
                        ws.Cells["AP" + headerIndex].Value = "Realization Per case";
                        ws.Cells["AQ" + headerIndex].Value = "Realization Per MT";
                        ws.Cells["AR" + headerIndex].Value = "Brokerage";
                        ws.Cells["AS" + headerIndex].Value = "Realization Per case Post Brokerage";
                        ws.Cells["AT" + headerIndex].Value = "SKU WISE Weight";
                        ws.Cells["AU" + headerIndex].Value = "Tax paid";
                        ws.Cells["AV" + headerIndex].Value = "Sauda Type";
                        ws.Cells["AW" + headerIndex].Value = "Pack Size";
                        ws.Cells["AX" + headerIndex].Value = "Margin Cost RA";
                        ws.Cells["AY" + headerIndex].Value = "Status";
                        ws.Cells["AZ" + headerIndex].Value = "Special Rate";
                        ws.Cells["BA" + headerIndex].Value = "Cushion Margin";
                        ws.Cells["BB" + headerIndex].Value = "Scheme Cost";
                        ws.Cells["BC" + headerIndex].Value = "OilType";
                        ws.Cells["BD" + headerIndex].Value = "Material Type";
                        //ws.Cells["AY" + headerIndex].Value = "Purchase";
                        //ws.Cells["AZ" + headerIndex].Value = "Purchase Total";
                        //ws.Cells["BB" + headerIndex].Value = "Area";
                        //ws.Cells["BD" + headerIndex].Value = "Margin PMT line item";                       
                        ws.Cells["BE" + headerIndex].Value = "RA Discount Total";
                        ws.Cells["BF" + headerIndex].Value = "Customer Group Margin";
                        ws.Cells["BG" + headerIndex].Value = "RA Premium With Tax";
                        ws.Cells["BH" + headerIndex].Value = "RA Premium (Without Tax )";
                        ws.Cells["BI" + headerIndex].Value = "Additional Cost";
                        ws.Cells["BJ" + headerIndex].Value = "OilTransfer Cost";
                        ws.Cells["BK" + headerIndex].Value = "SKU Conversion(With Tax)";
                        ws.Cells["BL" + headerIndex].Value = "SKU Conversion(Without Tax)";
                        ws.Cells["BM" + headerIndex].Value = "Customer Group One";
                        ws.Cells["BN" + headerIndex].Value = "Customer Group Two";
                        ws.Cells["BO" + headerIndex].Value = "Sauda Number";
                        ws.Cells["BP" + headerIndex].Value = "App Booking No";
                        ws.Cells["BQ" + headerIndex].Value = "App Id";

                        ExcelRange range = ws.Cells["A7:BJ7"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        int contentIndex = 9;

                        foreach (var data in publishData)
                        {
                            ws.Cells["A" + contentIndex].Value = data.OilType; //Product Group
                            ws.Cells["B" + contentIndex].Value = data.SkuName; //  "Material Description";
                            ws.Cells["C" + contentIndex].Value = data.SkuCode; //  "Material Code";
                            ws.Cells["D" + contentIndex].Value = data.BidQuantityCase; //  "Material Qty";
                            ws.Cells["E" + contentIndex].Value = data.UOM; //  "UOM";
                            ws.Cells["F" + contentIndex].Value = data.BidQuantity; //  "Material Qty(MT)";
                            ws.Cells["G" + contentIndex].Value = data.PackGroup; //  "Product Group";
                            ws.Cells["H" + contentIndex].Value = data.State; //  "State";
                            ws.Cells["I" + contentIndex].Value = data.CustomerCode; // "Customer Code";
                            ws.Cells["J" + contentIndex].Value = data.CustomerName; //  "Customer Name";
                            ws.Cells["K" + contentIndex].Value = data.FreightRoute; //  "Route Name";
                            ws.Cells["L" + contentIndex].Value = data.PlantName; //  "Plant Name";
                            ws.Cells["M" + contentIndex].Value = data.Incoterms; //  "Incoterms";
                            ws.Cells["N" + contentIndex].Value = data.DepotCode; //  "Depot Code";
                            ws.Cells["O" + contentIndex].Value = data.DepotName; //  "Depot Name";
                            ws.Cells["P" + contentIndex].Value = data.BrokerCode; //  "Broker Code";
                            ws.Cells["Q" + contentIndex].Value = data.BrokerName; //  "Broker Name";
                            ws.Cells["R" + contentIndex].Value = data.BiddingTime.ToString("hh\\:mm\\:ss"); //App Contract Time
                            ws.Cells["S" + contentIndex].Value = Settings.DateFormats(data.BiddingDate, Settings.ReportDateFormat); //  "App Contract Date";
                            ws.Cells["T" + contentIndex].Value = Settings.DateFormats(data.ValidFromDate, Settings.ReportDateFormat); //  "Contract Valid From";
                            ws.Cells["U" + contentIndex].Value = Settings.DateFormats(data.ValidToDate, Settings.ReportDateFormat); //  "Contract Valid To";
                            ws.Cells["V" + contentIndex].Value = data.MaterialCost; //  "Material Cost";
                            ws.Cells["W" + contentIndex].Value = data.Premium;  // "Premium";
                            ws.Cells["X" + contentIndex].Value = data.TD;  // "TD";
                            ws.Cells["Y" + contentIndex].Value = data.LTDValue;  // "LTD";
                            ws.Cells["Z" + contentIndex].Value = data.MarginCostTP;  // "Margin Cost TP";
                            ws.Cells["AA" + contentIndex].Value = data.PackingCost; // "Packing Cost";
                            ws.Cells["AB" + contentIndex].Value = data.HoneycombCost; // "Honeycomb cost";    
                            ws.Cells["AC" + contentIndex].Value = data.PrimaryFreight; // "Primary Freight";
                            ws.Cells["AD" + contentIndex].Value = data.SecondaryFreight;  // "Secondary Freight";
                            ws.Cells["AE" + contentIndex].Value = data.DepotCost;  // "Depot Cost";
                            ws.Cells["AF" + contentIndex].Value = data.DetentionCharges; // "Detention charges";
                            ws.Cells["AG" + contentIndex].Value = data.PR00; //  "PR00";
                            ws.Cells["AH" + contentIndex].Value = data.FRC1; //  "FRC1";
                            ws.Cells["AI" + contentIndex].Value = data.SaleRate; //  "Sale Rate";
                            ws.Cells["AJ" + contentIndex].Value = data.TotalValue;  // "Total Value";
                            ws.Cells["AK" + contentIndex].Value = data.Vertical;  // "Vertical";
                            ws.Cells["AL" + contentIndex].Value = data.ActualPackingCost;  // "Actual Packing Cost";
                            ws.Cells["AM" + contentIndex].Value = data.EmployeeCode;  // "Employee Code";
                            ws.Cells["AN" + contentIndex].Value = data.EmployeeName;  // "Employee Name";
                            ws.Cells["AO" + contentIndex].Value = data.Remarks; //Remarks

                            //if (data.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                            //{
                            //    ws.Cells["AP" + contentIndex].Value = Utility.DecimalFormatTwo(data.RealizationPerCase);  // "Realization Per case";
                            //}
                            //else
                            //{
                            ws.Cells["AP" + contentIndex].Value = Math.Round(data.RealizationPerCase);  // "Realization Per case";
                            //}
                            ws.Cells["AQ" + contentIndex].Value = Math.Round(data.RealizationPerMt); //  "Realization Per MT";
                            ws.Cells["AR" + contentIndex].Value = data.Brokerage; //Brokerage
                            ws.Cells["AS" + contentIndex].Value = Math.Round(data.RealizationPerCasePostBrokerage); //Realization Per case Post Brokerage
                            ws.Cells["AT" + contentIndex].Value = Math.Round(data.SkuWiseWeight, 3); //SKU WISE Weight
                            ws.Cells["AU" + contentIndex].Value = data.TaxPaid; //Tax paid
                            ws.Cells["AV" + contentIndex].Value = data.SaudaBookingType;  // "Sauda Type";
                            ws.Cells["AW" + contentIndex].Value = data.PackSize; //  "Pack Size";
                            ws.Cells["AX" + contentIndex].Value = data.MarginCostRA;  // "Margin Cost RA";
                            ws.Cells["AY" + contentIndex].Value = data.Status.ToLower() == DTO.Enums.Status.Pending.ToString().ToLower()
                                ? "Accepted" : data.Status;  // "Status";
                            ws.Cells["AZ" + contentIndex].Value = data.SpecialRate;  // "Special Rate";
                            ws.Cells["BA" + contentIndex].Value = data.CushionMargin; //Cushion Margin
                            ws.Cells["BB" + contentIndex].Value = data.SchemeCost;
                            ws.Cells["BC" + contentIndex].Value = data.OilType;
                            ws.Cells["BD" + contentIndex].Value = data.MaterialType;

                            //ws.Cells["AX" + contentIndex].Value = Math.Round(data.RealizationTotal); //Realization total
                            //ws.Cells["AY" + contentIndex].Value = data.Purchase; //Purchase
                            //ws.Cells["AZ" + contentIndex].Value = data.PurchaseTotal; //Purchase total
                            //ws.Cells["BB" + contentIndex].Value = data.Area; //Area
                            //ws.Cells["BD" + contentIndex].Value = data.MarginPMTLineItem; //Margin PMT line item

                            ws.Cells["BE" + contentIndex].Value = data.RaTotalDiscount;
                            ws.Cells["BF" + contentIndex].Value = data.CustomerGroupMargin;

                            ws.Cells["BG" + contentIndex].Value = data.RAPremiumWithTax;
                            ws.Cells["BH" + contentIndex].Value = data.RAPremiumWithoutTax;

                            ws.Cells["BI" + contentIndex].Value = data.AdditionalCost; //Realization Per MT Post Brokerage
                            ws.Cells["BJ" + contentIndex].Value = data.OilTransferCost; //Final realization
                            if (!data.IsBaseSauda)
                            {
                                ws.Cells["BK" + contentIndex].Value = data.SkuAllocationPremiumWithTax; //Realization Per MT Post Brokerage
                                ws.Cells["BL" + contentIndex].Value = data.SkuAllocationPremiumWithoutTax; //Final realization
                            }
                            ws.Cells["BM" + contentIndex].Value = data.CustomerGroupOne;
                            ws.Cells["BN" + contentIndex].Value = data.CustomerGroupTwo;
                            ws.Cells["BO" + contentIndex].Value = data.SaudaNumber; // Sauda Number
                            ws.Cells["BP" + contentIndex].Value = data.AppBookingNo; //  "App Booking No";
                            ws.Cells["BQ" + contentIndex].Value = data.SaudaOrderId; //App Id

                            contentIndex++;
                        }

                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Credit Limit

        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult CreditLimtReport()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
                LoginUserId = UserId,
                OrganizationReportingToId = OrganizationReportingToId
            };
            return View(result);
        }

        public async Task<ActionResult> GetCreditLimitList([DataSourceRequest] DataSourceRequest request, List<long> stateIds, long verticalIds, string dealerCode, string zhId, string bdoId, long SalesOrganizationId, long DistributionChannelId)
        {
            var zhead = zhId.Split(',').Select(long.Parse).ToList();
            var bdo = bdoId.Split(',').Select(long.Parse).ToList();
            var zh = zhead.SingleOrDefault(r => r == 0);
            zhead.Remove(zh);
            var StateTrader = bdo.SingleOrDefault(r => r == 0);
            bdo.Remove(StateTrader);
            var inputdto = new ReportFilterDto()
            {
                LoginUserId=UserId,
                RoleId=RoleId,
                zhId = zhead,
                bdoId = bdo,
                StateIds = stateIds,
                DivisionId = verticalIds,
                dealerCode = dealerCode,
                SalesOrganizationId = SalesOrganizationId,
                DistributionChannelId = DistributionChannelId
            };
            inputdto.DataSourceRequest = request;
            var creditLimitList = await _reportClient.CreditLimitExportAsync(inputdto);
            //var resultList = creditLimitList.ToDataSourceResult(request);
            return Json(creditLimitList);
        }

        public async Task<ActionResult> CreditLimitExportAsync(List<long> stateIds, long verticalIds, string dealerCode, string zhId, string bdoId, long SalesOrganizationId, long DistributionChannelId)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                var zhead = zhId.Split(',').Select(long.Parse).ToList();
                var bdo = bdoId.Split(',').Select(long.Parse).ToList();
                var zh = zhead.SingleOrDefault(r => r == 0);
                zhead.Remove(zh);
                var StateTrader = bdo.SingleOrDefault(r => r == 0);
                bdo.Remove(StateTrader);
                var inputdto = new ReportFilterDto()
                {
                    LoginUserId=UserId,
                    RoleId=RoleId,
                    zhId = zhead,
                    bdoId = bdo,
                    StateIds = stateIds,
                    DivisionId = verticalIds,
                    dealerCode = dealerCode,
                    SalesOrganizationId = SalesOrganizationId,
                    DistributionChannelId = DistributionChannelId
                };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<HANACreditMasterDto> creditLimitList = new List<HANACreditMasterDto>();
                creditLimitList = await _reportClient.GetCreditLimitAsync(inputdto);

                //creditLimitList=(List<HANACreditMasterDto>)result.Data;
                if (creditLimitList != null && creditLimitList.Any())
                {
                    DateTime currentDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                    fileName = "CREDIT-LIMIT-" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                    guidFileName = $"{Guid.NewGuid()}.xlsx";

                    var resultList = creditLimitList.Select(s => new CreditLimitExportDto() 
                    {
                        CustomerCode=s.CustomerCode,
                        CustomerName=s.CustomerName,
                        CreditLimt=s.CreditLimit,
                        CreditExposure=s.CreditExposure,
                        GrossExposure=s.GrossExposure,
                        OpenExposure=s.OpenExposure,
                        TotalReceivable=s.TotalReceivable,
                        OverDue=s.Overdue,
                        TommorrowDue=s.TomorrowsDue

                    }).ToList();

                    // Create the package and make sure you wrap it in a using statement
                    using (var package = new ExcelPackage())
                    {
                        // add a new worksheet to the empty workbook
                        var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        Response.ClearHeaders();
                        Response.ClearContent();
                        Response.Clear();
                        #region Header
                        worksheet.Cells["A1:F1"].Merge = true;
                        worksheet.Cells["A1:F1"].Value = Settings.CompanyName;
                        worksheet.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1:F1"].Style.Font.Bold = true;
                        worksheet.Cells["A1:F1"].Style.Font.Size = 16;

                        worksheet.Cells["A2"].Value = "Report Name";
                        worksheet.Cells["A3"].Value = "Total Record Count";
                        // worksheet.Cells["A4"].Value = "Vertical";
                        for (int i = 2; i <= 4; i++)
                        {
                            worksheet.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheet.Cells["A" + i].Style.Font.Bold = true;
                            worksheet.Cells["A" + i].Style.Font.Size = 12;

                            worksheet.Cells["B" + i + ":" + "F" + i].Merge = true;
                            worksheet.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        worksheet.Cells["B2"].Value = "CreditLimit";
                        worksheet.Cells["B3"].Value = creditLimitList.Count;
                        //  worksheet.Cells["B4"].Value = verticalIds == (int)DTO.Enums.Division.Hbc ? "Hbc" : verticalIds == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        #endregion
                        //var rowIndex = 6;
                        //var colIndex = 1;
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CustomerCode"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CustomerName"));
                        ////GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Credit Account Number");
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Credit Limit(In Lakhs)");
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Credit Exposure(In Lakhs)");
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Gross Exposure(In Lakhs)");
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Open Exposure(In Lakhs)");
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Total Receivable(In Lakhs)");
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Available Limit(In Lakhs)");
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Over Due");
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Tomorrow's Due");

                        ////To set top row as static
                        //worksheet.View.FreezePanes(2, 1);
                        ////To implement filters
                        //worksheet.Cells["A1:AQ1"].AutoFilter = true;
                        worksheet.Cells["A6:J" + (creditLimitList.Count+6)].LoadFromCollection(resultList,true);


                        ExcelRange range = worksheet.Cells["A6:J6"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        int contentIndex = 9;

                        //foreach (var creditlimit in creditLimitList)
                        //{
                        //    rowIndex++;
                        //    colIndex = 1;
                        //    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], creditlimit.CustomerCode != null ? creditlimit.CustomerCode.ToString() : string.Empty);
                        //    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], creditlimit.CustomerName != null ? creditlimit.CustomerName.ToString() : string.Empty);
                        //    //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], creditlimit.CreditAccountNumber);
                        //    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], creditlimit.CreditLimit.ToString());
                        //    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], creditlimit.CreditExposure.ToString());
                        //    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], creditlimit.GrossExposure.ToString());
                        //    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], creditlimit.OpenExposure.ToString());
                        //    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], creditlimit.TotalReceivable.ToString());
                        //    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], creditlimit.AvailableCreditLimit.ToString());
                        //    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], creditlimit.Overdue.ToString());
                        //    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], creditlimit.TomorrowsDue.ToString());
                        //}


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

            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  Filler Sku Report

        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult FillerSkuReport()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
                LoginUserId = UserId,
                OrganizationReportingToId = OrganizationReportingToId
            };
            return View(result);
        }

        public ActionResult FillerSkuReportExportAsync(long verticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<FillerSkuOutputDto> fillerskulist = new List<FillerSkuOutputDto>();
                fillerskulist = _reportClient.GetFillerSkuList(verticalId, SalesOrganizationId, DistributionChannelId);

                DateTime currentDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                fileName = "Filler-Sku-" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";


                if (fillerskulist != null && fillerskulist.Any())
                {
                    guidFileName = $"{Guid.NewGuid()}.xlsx";
                    // Create the package and make sure you wrap it in a using statement
                    using (var package = new ExcelPackage())
                    {
                        // add a new worksheet to the empty workbook
                        var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        Response.ClearHeaders();
                        Response.ClearContent();
                        Response.Clear();
                        #region Header
                        worksheet.Cells["A1:F1"].Merge = true;
                        worksheet.Cells["A1:F1"].Value = Settings.CompanyName;
                        worksheet.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1:F1"].Style.Font.Bold = true;
                        worksheet.Cells["A1:F1"].Style.Font.Size = 16;

                        worksheet.Cells["A2"].Value = "Report Name";
                        worksheet.Cells["A3"].Value = "Total Record Count";
                        worksheet.Cells["A4"].Value = "Vertical";
                        for (int i = 2; i <= 4; i++)
                        {
                            worksheet.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheet.Cells["A" + i].Style.Font.Bold = true;
                            worksheet.Cells["A" + i].Style.Font.Size = 12;

                            worksheet.Cells["B" + i + ":" + "F" + i].Merge = true;
                            worksheet.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        worksheet.Cells["B2"].Value = "FillerSkuReport";
                        worksheet.Cells["B3"].Value = fillerskulist.Count;
                        worksheet.Cells["B4"].Value = verticalId == (int)DTO.Enums.Division.Hbc ? "Hbc" : verticalId == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : verticalId == (int)DTO.Enums.LooseVertical.Loose ? "Loose" : "All";
                        #endregion
                        var rowIndex = 6;
                        var colIndex = 1;
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CustomerCode"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CustomerName"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SkuCode"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Sku"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PackType"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_QuantityPerCase"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_FromDate"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ToDate"));


                        ////To set top row as static
                        //worksheet.View.FreezePanes(2, 1);
                        ////To implement filters
                        //worksheet.Cells["A1:AQ1"].AutoFilter = true;

                        var ToDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        var dateBeforeThreeMonths = DateHelper.UtcToIndia(DateTime.UtcNow.AddDays(ConfigHelper.DateBeforeThreeMonths));
                        foreach (var data in fillerskulist)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], data.DealerCode != null ? data.DealerCode.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], data.DealerName != null ? data.DealerName.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], data.SkuCode.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], data.SkuName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], data.PackType.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], data.BidedCases.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], dateBeforeThreeMonths != null ? string.Format(Settings.GridDateFormat, dateBeforeThreeMonths).ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], ToDate != null ? string.Format(Settings.GridDateFormat, ToDate).ToString() : string.Empty);
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

            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region SKU Wise Premium Amount Report

        public ActionResult SKUPremiumAmountReport()
        {
            ExcelReportFilterDto roleIdDto = new ExcelReportFilterDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId
            };
            return View(roleIdDto);
        }

        public async Task<ActionResult> GetSKUWisePremiumAmountReport(long verticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetSKUWisePremiumAmountReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "SKU-PREMIUM_AMOUNT-REPORT-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";
            try
            {
                ExcelReportFilterDto inputputDto = new ExcelReportFilterDto()
                {
                    //FromDate = fromDate,
                    //ToDate = toDate,
                    VerticalId = verticalId,
                    SalesOrganizationId = SalesOrganizationId,
                    DistributionChannelId = DistributionChannelId
                };
                var publishData = _reportClient.GetSKUWisePremiumAmountReport(inputputDto);
                if (publishData != null && publishData.Any())
                {
                    var result = publishData.FirstOrDefault();
                    using (var ep = new ExcelPackage())
                    {
                        var ws = ep.Workbook.Worksheets.Add("Sheet1");
                        ws.Name = "SKU Premium Amount Report";

                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        //ws.Cells["A3"].Value = "From Date";
                        //ws.Cells["A4"].Value = "To Date";
                        ws.Cells["A3"].Value = "Total Record Count";
                        ws.Cells["A4"].Value = "Vertical";
                        for (int i = 2; i <= 4; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        ws.Cells["B2"].Value = "SKU Premium Amount Report";
                        //ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                        //ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B3"].Value = publishData.Count;
                        ws.Cells["B4"].Value = verticalId == (int)DTO.Enums.Division.Hbc ? "Hbc" : verticalId == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        #endregion

                        int headerIndex = 6;
                        ws.Cells["A" + headerIndex].Value = "Vertical";
                        ws.Cells["B" + headerIndex].Value = "SKU Code";
                        ws.Cells["C" + headerIndex].Value = "SKU Name";
                        ws.Cells["D" + headerIndex].Value = "Premium Amount";

                        ExcelRange range = ws.Cells["A6:H6"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        int contentIndex = 7;
                        foreach (var data in publishData)
                        {
                            ws.Cells["A" + contentIndex].Value = data.Divisions;
                            ws.Cells["B" + contentIndex].Value = data.SkuCode;
                            ws.Cells["C" + contentIndex].Value = data.SkuName;
                            ws.Cells["D" + contentIndex].Value = data.PremiumAmount;
                            contentIndex++;
                        }
                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region SchemeGeographyReport

        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult SchemeGeographyReport()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }

        public async Task<ActionResult> GetSchemeGeographyDetailsReport(DateTime fromDate, DateTime toDate, List<long> stateIds, long verticalId, List<long> geographySchemeIds, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetSchemeGeographyDetailsReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "Scheme Geography-REPORT-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";

            try
            {
                SchemeGeographyReportInputputDto inputputDto = new SchemeGeographyReportInputputDto()
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    StateIds = stateIds,
                    VerticalId = verticalId,
                    GeographySchemeIds = geographySchemeIds,
                    SalesOrganizationId = SalesOrganizationId,
                    DistributionChannelId = DistributionChannelId
                };

                var publishData = await _reportClient.GetSchemeGeographyDetailsReportAsync(inputputDto);

                if (publishData.IsAny())
                {

                    var resultList = publishData.Select(s => new SchemeGeographyReportExportDto()
                    {
                        Bdoname = s.BDOName,
                        DealerCode = s.DealerCode,
                        DealerName = s.DealerName,
                        Progress = s.Progress.ToString(),
                        DateAchieved = string.Format(s.AchievedDate.ToString(), "dd/MM/yyyy HH:mm"),
                        Ranking = s.Ranking.ToString(),
                        SchemeName = s.SchemeName,
                        SkuCode = s.SkuCode,
                        SkuName = s.SkuName,
                        TargetAchieved = s.AchievedQuantity.ToString(),
                        TargetQuantity=s.TargetQuantity.ToString()
                    }) ;
                    var result = publishData.FirstOrDefault();

                    string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFoloderPath + "FinalPriceTemplate.xlsx");

                    using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = "SchemeGeographyReport";

                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "From Date";
                        ws.Cells["A4"].Value = "To Date";
                        ws.Cells["A5"].Value = "Total Record Count";
                        //ws.Cells["A6"].Value = "Vertical";

                        for (int i = 2; i <= 6; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        ws.Cells["B2"].Value = "Scheme Geography Report";
                        ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B5"].Value = publishData.Count;
                        //ws.Cells["B6"].Value = verticalId == (int)DTO.Enums.Division.Hbc ? "Hbc" : verticalId == (int)DTO.Enums.Division.SpecialityFat ? "SpecialityFat" : "All";
                        #endregion


                        ws.Cells["A7:K" + (7 + resultList.Count())].LoadFromCollection(resultList, true);
                        //int headerIndex = 8;
                        //ws.Cells["A" + headerIndex].Value = "Scheme name";
                        //ws.Cells["B" + headerIndex].Value = "Sku Code";
                        //ws.Cells["C" + headerIndex].Value = "Sku Name";
                        //ws.Cells["D" + headerIndex].Value = "Dealer Code";
                        //ws.Cells["E" + headerIndex].Value = "Dealer Name";
                        //// ws.Cells["F" + headerIndex].Value = "StateTrader Code";
                        //ws.Cells["G" + headerIndex].Value = "StateTrader Name";
                        //ws.Cells["H" + headerIndex].Value = "Target Quantity";
                        //ws.Cells["I" + headerIndex].Value = "Target Achieved";
                        //ws.Cells["J" + headerIndex].Value = "Date Achieved";
                        //ws.Cells["K" + headerIndex].Value = "Progress %";
                        //ws.Cells["L" + headerIndex].Value = "Ranking";


                        ExcelRange range = ws.Cells["A7:k7"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        int contentIndex = 9;

                        //foreach (var data in publishData)
                        //{
                        //    ws.Cells["A" + contentIndex].Value = data.SchemeName; //Scheme Name
                        //    ws.Cells["B" + contentIndex].Value = data.SkuCode; //  Sku Code
                        //    ws.Cells["C" + contentIndex].Value = data.SkuName; //  Sku Name
                        //    ws.Cells["D" + contentIndex].Value = data.DealerCode; //  Dealer Code
                        //    ws.Cells["E" + contentIndex].Value = data.DealerName; //  Dealer Name
                        //                                                          // ws.Cells["F" + contentIndex].Value = data.BDOCode; //  StateTrader Code
                        //    ws.Cells["G" + contentIndex].Value = data.BDOName; //  StateTrader Name
                        //    ws.Cells["H" + contentIndex].Value = data.TargetQuantity; // Target Quantity
                        //    ws.Cells["I" + contentIndex].Value = data.AchievedQuantity; // AchievedQuantity
                        //    ws.Cells["J" + contentIndex].Value = string.Format(data.AchievedDate.ToString(), "dd/MM/yyyy HH:mm"); //  Achieved Date
                        //    ws.Cells["K" + contentIndex].Value = data.Progress; //  Progress
                        //    ws.Cells["L" + contentIndex].Value = data.Ranking; //  Ranking

                        //    contentIndex++;
                        //}

                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region DemandPlanBillingReport

        [AuthorizeClaims(Claims.ViewReports)]
        public ActionResult DemandPlanBillingReport()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }

        public async Task<ActionResult> GetDemandPlanBillingDetailsReport(DateTime fromDate, DateTime toDate)
        {
            _methodName = "GetSchemeGeographyDetailsReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "Demand Plan Billing-REPORT-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = "";

            try
            {
                DemandPlanBillingReportInputputDto inputputDto = new DemandPlanBillingReportInputputDto()
                {
                    FromDate = fromDate,
                    ToDate = toDate
                };

                var publishData = await _reportClient.GetDemandPlanBillingDetailsReportAsync(inputputDto);

                if (publishData != null && publishData.Any())
                {
                    var result = publishData.FirstOrDefault();

                    string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFoloderPath + "FinalPriceTemplate.xlsx");

                    using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = "DemandPlanBillingReport";

                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "From Date";
                        ws.Cells["A4"].Value = "To Date";
                        ws.Cells["A5"].Value = "Total Record Count";

                        for (int i = 2; i <= 6; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        ws.Cells["B2"].Value = "Demand Plan Billing Report";
                        ws.Cells["B3"].Value = Settings.DateFormats(fromDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B4"].Value = Settings.DateFormats(toDate, Settings.ReportDateFormat).ToUpper();
                        ws.Cells["B5"].Value = publishData.Count;
                        #endregion

                        int headerIndex = 8;
                        ws.Cells["A" + headerIndex].Value = "RowLabel";
                        ws.Cells["B" + headerIndex].Value = "RowLabel1";
                        ws.Cells["C" + headerIndex].Value = "Date";


                        ExcelRange range = ws.Cells["A7:BJ7"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;
                        int contentIndex = 9;

                        foreach (var data in publishData)
                        {
                            ws.Cells["A" + contentIndex].Value = data.RowLabel; //RowLabel
                            ws.Cells["B" + contentIndex].Value = data.RowLabel1; //RowLabel1
                            ws.Cells["C" + contentIndex].Value = string.Format(data.Date.ToString(), "dd/MM/yyyy HH:mm");
                            contentIndex++;
                        }

                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                guidFileName = "";
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region SaudaAgingReport

        [AuthorizeClaims(Claims.SaudaAgingReport)]
        public ActionResult SaudaAgingReport()
        {

            return View();
        }

        public async Task<ActionResult> SaudaAgingExportAsync(SaudaAgingReportDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {

                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                // List<SalesBDOWiseReportDto> salesList = new List<SalesBDOWiseReportDto>();

                List<SaudaAgingReportExportDto> salesList = new List<SaudaAgingReportExportDto>();
                salesList = _reportClient.SaudaAgingReport(inputDto);

                //  List<SaudaAgingReportDto> salesList = new List<SaudaAgingReportDto>();

                DateTime currentDate = DateTime.Now;
                fileName = "SAUDA_AGING-REPORT" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
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

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BaseDepot"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Party"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PartyName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_City"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PoDate"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ContractNumber"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_MaterialDescription"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Date"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SaudaAging"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ContractEndDate"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ContractQuantity"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OsQuantity"));

                    ////To set top row as static
                    //worksheet.View.FreezePanes(2, 1);
                    ////To implement filters
                    //worksheet.Cells["A1:AQ1"].AutoFilter = true;

                    if (salesList != null && salesList.Any())
                    {
                        var bdoList = salesList;//.Select(_ => _.BDOCode).Distinct().ToList();
                        foreach (var StateTrader in bdoList)
                        {
                            var bdoWiseSalesList = salesList;//.Where(_ => _.BDOCode == StateTrader);
                            foreach (var sales in bdoWiseSalesList)
                            {
                                rowIndex++;
                                colIndex = 1;

                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.BaseDepot != null ? sales.BaseDepot.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.Party != null ? sales.Party.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.PartyName != null ? sales.PartyName.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.City != null ? sales.City.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.PODate != null ? sales.PODate.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.ContractNumber != null ? sales.ContractNumber.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.MaterialDescription != null ? sales.MaterialDescription.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.Date != null ? sales.Date.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.SaudaAging != null ? sales.SaudaAging.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.ContractEndDate != null ? sales.ContractEndDate.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.ContractQuantity != null ? sales.ContractQuantity.ToString() : string.Empty);
                                GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sales.OSQuantity != null ? sales.OSQuantity.ToString() : string.Empty);

                            }

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

        #region GPSTrackingReport

        [AuthorizeClaims(Claims.GPSTrackingReport)]
        public ActionResult GPSTrackingReport()
        {

            return View();
        }

        public async Task<ActionResult> GPSTrackingExportAsync(GPSTrackingDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {

                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                // List<SalesBDOWiseReportDto> salesList = new List<SalesBDOWiseReportDto>();

                List<GPSTrackingDto> resultList = new List<GPSTrackingDto>();
                resultList = _reportClient.GPSTrackingReport(inputDto);

                //  List<GPSTrackingReportDto> salesList = new List<GPSTrackingReportDto>();

                DateTime currentDate = DateTime.Now;
                fileName = "GPSTracking-REPORT" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
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

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Latitude"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Longitude"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;

                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Latitude);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Longitude);

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

        #region CrossAndUpsell Report

        public ActionResult CrossAndUpsellSaudaOrderReport()
        {
            RoleIdDto inputDto = new RoleIdDto { RoleId = RoleId,LoginUserId=UserId};
            return View(inputDto);
        }

        [HttpPost]
        public async Task<ActionResult> GetCrossAndUpsellSaudaOrderReport(SaudaConditionalBookingReportInputDto inputDto)
        {
            _methodName = "GetCrossAndUpsellSaudaOrderReportAsync";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string fileName = "CrossAndUpsellContract_Report_" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            string guidFileName = string.Empty;

            try
            {
                var crossAndUpsellContractData = await _reportClient.GetCrossAndUpsellSaudaOrderReportAsync(inputDto);

                if (crossAndUpsellContractData != null && crossAndUpsellContractData.Any())
                {
                    string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFoloderPath + "FinalPriceTemplate.xlsx");

                    using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = "Contract";

                        ws.Cells["A1:AY1"].Style.Font.Size = 13;
                        ws.Cells["A1:AY1"].Style.Font.Name = "Calibri";
                        ws.Cells["A1:AY1"].Style.Font.Bold = true;
                        ws.Cells.LoadFromCollection(crossAndUpsellContractData, true);

                        ws.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }

                return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}