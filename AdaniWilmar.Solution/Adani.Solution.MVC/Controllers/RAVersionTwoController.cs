using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using OfficeOpenXml;
using GMCore.Helper;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Adani.Solution.MVC.Models;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.DTO.Enums;
using Adani.Solution.DTO.Common;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Attributes;
using Adani.Solution.DTO;
using Adani.Solution.MVC.ServiceClient;
using Newtonsoft.Json;

namespace Adani.Solution.MVC.Controllers
{
    [TokenAuthorize]
    [CustomRedirect]
    [NoCache]
    public class RAVersionTwoController : BaseController
    {
        private readonly RAVersionTwoClient _raNewVersionClient;

        public RAVersionTwoController()
        {
            _raNewVersionClient = new RAVersionTwoClient { ControllerDelegate = this };
        }

        #region  SchemeDiscount - GeographyBased 

        /// <summary>
        /// Method to Get Geography Based SchemeDiscount List Page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageDiscounts, Claims.ViewDiscounts)]
        public ActionResult SchemeDiscountGeographyList()
        {
            return View();
        }

        /// <summary>
        /// Method to Get Geography Based SchemeDiscount Page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageDiscounts, Claims.ViewDiscounts)]
        public async Task<ActionResult> SchemeDiscountGeography()
        {
            var result = new SchemeDiscountGeographyDto();
            if (Session["GeographyBasedSchemeDiscountId"] != null && UtilityHelper.IntTryToParse(Session["GeographyBasedSchemeDiscountId"].ToString()) > 0)
            {
                result = await _raNewVersionClient.GetGeographyBasedSchemeDiscountByDiscountId(UtilityHelper.LongTryToParse(Session["GeographyBasedSchemeDiscountId"].ToString()));
            }
            return View(result);
        }
        public JsonResult GetDiscountTypeListForDropdown([DataSourceRequest] DataSourceRequest request)
        {
            var resultList = ((DiscountType[])Enum.GetValues(typeof(DiscountType))).Select(c => new DropDownDto() { Id = (int)c, Name = c.Description() }).ToList();
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to Redirect Geography Based SchemeDiscount Page
        /// </summary>
        /// <param name="schemeDiscountId"></param>
        /// <returns></returns>
        public ActionResult GBSDERedirect(string schemeDiscountId = "")
        {
            Session["GeographyBasedSchemeDiscountId"] = schemeDiscountId;
            return RedirectToAction("SchemeDiscountGeography", "RAVersionTwo");
        }

        /// <summary>
        /// Method to Add Or Update Geography Based SchemeDiscount
        /// </summary>
        /// <param name="inputDto"></param>
        /// <param name="VolumeSlabsList"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateGeographyBasedSchemeDiscount(SchemeDiscountGeographyDto inputDto)
        {
            if (inputDto != null)
            {
                inputDto.LoginUserId = UserId;
                inputDto = await _raNewVersionClient.AddOrUpdateGeographyBasedSchemeDiscount(inputDto);
                if (inputDto.PostStatus)
                {
                    TempData["SuccessMessage"] = inputDto.PostMessage;
                    return RedirectToAction("SchemeDiscountGeographyList", "RAVersionTwo");
                }
            }
            return View("SchemeDiscountGeography", inputDto);
        }

        /// <summary>
        /// Method to Get Geography Based SchemeDiscount List Async
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetGeographyBasedSchemeDiscountListAsync([DataSourceRequest] DataSourceRequest request)
        {
            return Json(await _raNewVersionClient.GetKendoGridDataAsync<SchemeDiscountGeographyDto>(GridResultInputDto(request, true), ApiUrl.WebApiUrlGetGeographyBasedSchemeDiscountList));
        }

        /// <summary>
        /// Method to Get SchemeDiscount Geography Details By Id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="schemeDiscountId"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetSchemeDiscountGeographyHierarchyListById([DataSourceRequest] DataSourceRequest request, long schemeDiscountId)
        {
            //ListInputDto inputDto = new ListInputDto { Id = schemeDiscountId, DataSourceRequest = request };
            //return Json(await _raNewVersionClient.GetSchemeDiscountGeographyHierarchyListById(inputDto));

            return Json(await _raNewVersionClient.GetKendoGridDataAsync<SchemeDiscountGeographyMappingDto>(KendoGridResultWithId(request, schemeDiscountId, true), ApiUrl.WebApiUrlGetSchemeDiscountGeographyHierarchyListById));
        }

        /// <summary>
        /// Method to Export Geography Based SchemeDiscount
        /// </summary>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportSchemeDiscountGeography(DateTime fromDate, DateTime toDate)
        {
            var finalResult = new JsonResult();
            DateTime currentDate = DateTime.Now;
            string fileName = "SchemeDiscountGeography" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
            var guidFileName = $"{Guid.NewGuid()}.xlsx";
            try
            {
                var resultList = _raNewVersionClient.ExportSchemeDiscountGeography(new ExcelReportFilterDto { FromDate = fromDate, ToDate = toDate });
                if (resultList.IsAny())
                {
                    using (var package = new ExcelPackage())
                    {
                        var ws = package.Workbook.Worksheets.Add("SchemeDiscountGeography");

                        //Header
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
                        ws.Cells["D2:I2"].Value = "Scheme Discount Geography";
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

        public async Task<ActionResult> UpdateGeographyBasedSchemeDiscountByIsActive(int Id)
        {
            IdDiscountAndBenefitInputDto idInputDto = new IdDiscountAndBenefitInputDto() { Id = Id, LoginUserId = UserId };
            var result = await _raNewVersionClient.UpdateGeographyBasedSchemeDiscountByIsActive(idInputDto);
            return Json(result);
        }

        /// <summary>
        /// Method to Scheme Discount History screen
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageDiscounts, Claims.ViewDiscounts)]
        public ActionResult SchemeDiscountHistory()
        {
            return View();
        }

        /// <summary>
        /// Method to Get Scheme Discount History Geography List
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSchemeDiscountHistoryGeographyListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = _raNewVersionClient.GetSchemeDiscountHistoryGeographyListAsync();
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        /// <summary>
        /// Method to Get Scheme Discount History User List
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSchemeDiscountHistoryUserListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = _raNewVersionClient.GetSchemeDiscountHistoryUserListAsync();
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        /// <summary>
        /// Method to Export Scheme Discount Geography History
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportSchemeDiscountGeographyHistory(DateTime fromDate, DateTime toDate)
        {
            var finalResult = new JsonResult();
            DateTime currentDate = DateTime.Now;
            string fileName = "SchemeDiscountGeography" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
            var guidFileName = $"{Guid.NewGuid()}.xlsx";
            try
            {
                var resultList = _raNewVersionClient.ExportSchemeDiscountGeographyHistory(new ExcelReportFilterDto { FromDate = fromDate, ToDate = toDate });
                if (resultList.IsAny())
                {
                    using (var package = new ExcelPackage())
                    {
                        var ws = package.Workbook.Worksheets.Add("SchemeDiscountGeography");

                        //Header
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
                        ws.Cells["D2:I2"].Value = "Scheme Discount Geography";
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

       
        #endregion

        #region Reporting To Users - Customer Group

        public async Task<ActionResult> GetReportingToRAZonalHeadUsersByCustomerGroup(long verticalId, long customerGroupId)
        {
            var reportingToUsers = new List<DropDownDto>();
            if (verticalId > 0 && customerGroupId > 0)
            {
                var inputDto = new CustomerGroupInputDto { LoginUserId = UserId, VerticalId = verticalId, CustomerGroupId = customerGroupId };
                reportingToUsers = await _raNewVersionClient.GetRAZonalHeadUsersByCustomerGroup(inputDto);
            }
            return Json(reportingToUsers, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetRAZonalHeadUsersByCustomerGroupIdsAndVerticalIds(string verticalIds, string customerGroupIds)
        {
            var reportingToUsers = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(verticalIds) && !string.IsNullOrEmpty(customerGroupIds))
            {
                var inputDto = new DropDownInputDto { VerticalIds = UtilityHelper.ConvertStringToLongList(verticalIds), CustomerGroupIds = UtilityHelper.ConvertStringToLongList(customerGroupIds) };
                reportingToUsers = await _raNewVersionClient.GetRAZonalHeadUsersByCustomerGroupIdsAndVerticalIds(inputDto);
            }
            return Json(reportingToUsers, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetRABDOUsersByZonalHeadIdsAndVerticalIds(string verticalIds, string zonalHeadIds)
        {
            var reportingToUsers = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(verticalIds) && !string.IsNullOrEmpty(zonalHeadIds))
            {
                var inputDto = new DropDownInputDto { UserIds = UtilityHelper.ConvertStringToLongList(zonalHeadIds), VerticalIds = UtilityHelper.ConvertStringToLongList(verticalIds) };
                reportingToUsers = await _raNewVersionClient.GetRABDOUsersByZonalHeadIdsAndVerticalIds(inputDto);
            }
            return Json(reportingToUsers, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Lookup

        public ActionResult GetCustomerListBasedOnCityIdsAndPercentileNumberForDropdown(SurpriseBenefitPercentileInputDto inputDto)
        {
            IList<DropDownDto> result = new List<DropDownDto>();
            if ((!string.IsNullOrEmpty(inputDto.TerritoryIdStringList) ||
                !string.IsNullOrEmpty(inputDto.DistrictIdStringList) ||
                !string.IsNullOrEmpty(inputDto.CityIdStringList) ||
                !string.IsNullOrEmpty(inputDto.CustomerGroupIdStringList)) &&
                 inputDto.PercentileNumber > 0)
            {
                result = _raNewVersionClient.GetCustomerListBasedOnCityIdsAndPercentileNumberForDropdown(inputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to Get Customer List By Customer Group Id And StateTrader For Dropdown
        /// </summary>
        /// <param name="request"></param>
        /// <param name="customerGroupId"></param>
        /// <param name="bdoId"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetCustomerListByCustomerGroupIdsAndBDOsForDropdown(string customerGroupIds, string bdoIds)
        {
            var result = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(customerGroupIds) || !string.IsNullOrEmpty(bdoIds))
            {
                DropDownInputDto inputDto = new DropDownInputDto { CustomerGroupIds = UtilityHelper.ConvertStringToLongList(customerGroupIds), UserIds = UtilityHelper.ConvertStringToLongList(bdoIds) };
                result = await _raNewVersionClient.GetCustomerListByCustomerGroupIdsAndBDOsForDropdown(inputDto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetCustomerListByCustomerGroupIdsCityIdsForDropdown(string cityIds)
        {
            var result = new List<DropDownDto>();
            //if (!string.IsNullOrEmpty(cityIds))
            //{
                DropDownInputDto inputDto = new DropDownInputDto {CityIds = UtilityHelper.ConvertStringToLongList(cityIds) };
                result = await _raNewVersionClient.GetCustomerListByCustomerGroupIdsCityIdsForDropdown(inputDto);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        protected static bool CheckDate(String date)
        {
            string[] columnNames = Settings.DateColumns;
            return columnNames.Any(c => c.ToLower().Trim().Equals(date.ToLower().Trim()));
        }

        

    }
}
