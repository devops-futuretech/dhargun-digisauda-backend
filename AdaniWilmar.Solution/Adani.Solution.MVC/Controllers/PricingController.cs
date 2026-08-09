using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using GMCore.Helper;
using Adani.Solution.MVC.Models;
using Adani.Solution.MVC.ServiceClient;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.Attributes;
using Adani.Solution.DTO.Enums;
using Adani.Solution.DTO;
using Newtonsoft.Json.Linq;
using System.Linq;
using OfficeOpenXml;
using System.IO;
using System.Web.Hosting;
using System.Drawing;
using Adani.Solution.DTO.Common;
using Kendo.Mvc;
using Adani.Solution.MVC.Common;
using System.Data.SqlClient;
using System.Text;
using Dapper;
using GMCore.Logger;
using System.Data;

namespace Adani.Solution.MVC.Controllers
{
    [TokenAuthorize]
    [CustomRedirect]
    [NoCache]
    public class PricingController : BaseController
    {
        private readonly PricingClient _pricingClient;
        private readonly ImportClient _importClient;
        private const string ServiceName = "Pricing Controller";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;

        static string connectionString = ConfigHelper.SPConnectionString;

        public PricingController()
        {
            _pricingClient = new PricingClient { ControllerDelegate = this };
            _importClient = new ImportClient { ControllerDelegate = this };
        }



        #region Margin - ProfitMargin / CushionMargin

        public ActionResult MarginList()
        {
            return View();
        }

        public ActionResult GetMarginListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<ProfitMarginDto>();
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public ActionResult MarginEditRedirect(string brokerId = "")
        {
            Session["MarginId"] = brokerId;
            return RedirectToAction("Margin", "Pricing");
        }

        public ActionResult Margin()
        {
            var result = new ProfitMarginDto();
            if (Session["MarginId"] != null && UtilityHelper.IntTryToParse(Session["MarginId"].ToString()) > 0)
            {
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public ActionResult AddOrUpdateMargin(ProfitMarginDto brokerInputDto)
        {
            ProfitMarginDto brokerInput = new ProfitMarginDto();
            return View("Margin", brokerInput);
        }

        #endregion

        #region Role Discount

        public ActionResult RoleDiscountList()
        {
            return View();
        }

        public async Task<ActionResult> RoleDiscount()
        {
            var roleDiscount = new RoleDiscountDto();
            if (Session["RoleDiscount"] != null)
            {
                roleDiscount = await _pricingClient.GetRoleBasedDiscountById(UtilityHelper.LongTryToParse(Session["RoleDiscount"].ToString()));
            }
            return View(roleDiscount);
        }

        public ActionResult RoleDiscountEdit(string roleId)
        {
            Session["RoleDiscount"] = roleId;
            return RedirectToAction("RoleDiscount", "Pricing");
        }

        public async Task<ActionResult> GetRoleDiscountListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            RoleDiscountDto roleDiscountDto = new RoleDiscountDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData };
            var result = await _pricingClient.GetRoleBasedDiscounts(roleDiscountDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> UpdateRoleDiscount(RoleDiscountDto roleDiscountDto)
        {
            roleDiscountDto.LoginUserId = UserId;
            var result = await _pricingClient.UpdateRoleBasedDiscount(roleDiscountDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("RoleDiscountList", "Pricing");
            }
            return View("RoleDiscount", roleDiscountDto);
        }

        #endregion

        #region Customer Role Discount

        [AuthorizeClaims(Claims.ManageDiscounts, Claims.ViewDiscounts)]
        public ActionResult CustomerDiscountList()
        {
            return View();
        }

        [AuthorizeClaims(Claims.ManageDiscounts, Claims.ViewDiscounts)]
        public async Task<ActionResult> CustomerDiscount()
        {
            var skuDiscount = new SkuDepotDiscountDto();
            if (Session["CusDiscountId"] != null && Session["DiscountType"] != null)
            {
                CustomerDiscountinputDto discountDto = new CustomerDiscountinputDto()
                {
                    Id = UtilityHelper.LongTryToParse(Session["CusDiscountId"].ToString()),
                    DiscountType = Convert.ToInt32(Session["DiscountType"].ToString())
                };
                skuDiscount = await _pricingClient.GetSkuDepotBasedDiscountById(discountDto);
            }
            return View(skuDiscount);
        }

        public ActionResult CustomerDiscountEdit(string discountId, string discountType)
        {
            Session["CusDiscountId"] = discountId;
            Session["DiscountType"] = discountType;
            return RedirectToAction("CustomerDiscount", "Pricing");
        }

        public async Task<ActionResult> GetCustomerDiscountListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData, int discountType)
        {
            CustomerDiscountinputDto discountinputDto = new CustomerDiscountinputDto
            {
                LoginUserId = UserId,
                IsToReturnInactiveData = isToReturnInactiveData,
                DiscountType = discountType
            };
            var result = await _pricingClient.GetSkuDepotBasedDiscounts(discountinputDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> AddOrUpdateCustomerDiscount(SkuDepotDiscountDto skuDiscountDto)
        {
            skuDiscountDto.LoginUserId = UserId;
            var result = await _pricingClient.AddOrUpdateSkuDepotDiscount(skuDiscountDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("CustomerDiscountList", "Pricing");
            }
            return View("CustomerDiscount", skuDiscountDto);
        }

        public async Task<ActionResult> GetOilTypeDetailsddlAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            OilTypeDto oilTypeDto = new OilTypeDto();
            oilTypeDto.IsToReturnInactiveData = isToReturnInactiveData;
            oilTypeDto.LoginUserId = UserId;
            var result = await _pricingClient.GetOilTypeDetailsddl(oilTypeDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetDepotDetailsddlAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            DepotDto depotDto = new DepotDto();
            depotDto.IsToReturnActiveData = isToReturnInactiveData;
            depotDto.UserId = UserId;
            var result = await _pricingClient.GetDepotDetailsddl(depotDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetUserDetailsddlAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            var loginUserIdDto = new LoginUserIdDto()
            {
                IsToReturnInactiveData = isToReturnInactiveData,
                LoginUserId = UserId
            };
            var result = await _pricingClient.GetUserDetailsddl(loginUserIdDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GetSkuDetailsddlAsync(long oilTypeId, bool isToReturn)
        {
            OilTypeDto oilTypeDto = new OilTypeDto() { SelectedOilTypeId = oilTypeId, IsToReturnInactiveData = isToReturn };
            var result = await _pricingClient.GetSkuDetailsddl(oilTypeDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region RoleDiscounts
        [AuthorizeClaims(Claims.ManageDiscounts, Claims.ViewDiscounts)]
        public ActionResult RoleDiscountsList()
        {
            return View();
        }

        [AuthorizeClaims(Claims.ManageDiscounts, Claims.ViewDiscounts)]
        public async Task<ActionResult> RoleDiscounts()
        {
            var roleDisocuntDto = new RoleDisocuntDto();
            if (Session["RoleDiscountId"] != null && Session["OilTypeId"] != null)
            {
                var roleDisocuntInputDto = new RoleDisocuntInputDto()
                {
                    RoleId = UtilityHelper.LongTryToParse(Session["RoleDiscountId"].ToString()),
                    OilTypeId = UtilityHelper.LongTryToParse(Session["OilTypeId"].ToString())
                };
                roleDisocuntDto = await _pricingClient.GetRoleDiscountbyId(roleDisocuntInputDto);
            }
            SkuDropDown skuDropDown = new SkuDropDown();
            ViewData["defaultSku"] = skuDropDown;
            return View(roleDisocuntDto);
        }

        public ActionResult RoleDiscountsEdit(string roleDiscountId, string oilTypeId)
        {
            Session["RoleDiscountId"] = roleDiscountId;
            Session["OilTypeId"] = oilTypeId;
            return RedirectToAction("RoleDiscounts", "Pricing");
        }

        public async Task<ActionResult> GetRoleDiscountsListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData, long roleId)
        {
            IList<RoleDisocuntDto> roleDetails = new List<RoleDisocuntDto>();
            if (roleId > 0)
            {
                var roleDisocuntInputDto = new RoleDisocuntInputDto()
                {
                    LoginUserId = UserId,
                    IsToReturnInactiveData = isToReturnInactiveData,
                    RoleId = roleId
                };
                roleDetails = await _pricingClient.GetRoleDiscountsAll(roleDisocuntInputDto);
            }
            var resultList = roleDetails.ToDataSourceResult(request);
            return Json(resultList);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        public async Task<ActionResult> AddOrUpdateRoleDiscount(RoleDisocuntDto roleDisocunt, List<SkuDiscounts> skuDiscounts)
        {
            roleDisocunt.LoginUserId = UserId;
            roleDisocunt.SkuDiscounts = skuDiscounts;
            var result = await _pricingClient.AddOrUpdateRoleDiscount(roleDisocunt);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Request Discount
        [AuthorizeClaims(Claims.ManageDiscounts, Claims.ViewDiscounts)]
        public ActionResult RequestDiscountList()
        {
            return View();
        }

        [AuthorizeClaims(Claims.ManageDiscounts, Claims.ViewDiscounts)]
        public async Task<ActionResult> RequestDiscount()
        {
            var requestDisocuntDto = new RequestDisocuntDto();
            if (Session["RequestDiscountId"] != null)
            {
                requestDisocuntDto = await _pricingClient.GetRequestDiscountbyId(UtilityHelper.LongTryToParse(Session["RequestDiscountId"].ToString()));
            }
            return View(requestDisocuntDto);
        }

        public ActionResult RequestDiscountEdit(string requestDiscountId)
        {
            Session["RequestDiscountId"] = requestDiscountId;
            return RedirectToAction("RequestDiscount", "Pricing");
        }

        public async Task<ActionResult> GetRequestDiscountList([DataSourceRequest] DataSourceRequest request, long roleId)
        {
            RequestDisocuntInputDto requestParam = new RequestDisocuntInputDto
            {
                LoginUserId = UserId,
                RoleId = roleId
            };
            var result = await _pricingClient.GetRequestDiscountList(requestParam);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> UpdateRequestDiscount(RequestDisocuntDto requestDisocuntDto)
        {
            requestDisocuntDto.LoginUserId = UserId;
            var result = await _pricingClient.UpdateRequestDiscount(requestDisocuntDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("RequestDiscountList", "Pricing");
            }
            return View("RequestDiscount", requestDisocuntDto);
        }

        [HttpPost]
        public async Task<ActionResult> GetRequestDetails(long roleId, long skuId)
        {
            RequestDisocuntInputDto requestParam = new RequestDisocuntInputDto
            {
                LoginUserId = UserId,
                RoleId = roleId,
                SkuId = skuId
            };
            var result = await _pricingClient.GetRequestDiscountDetailsById(requestParam);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Approve Discount
        [AuthorizeClaims(Claims.ApproveDiscount)]
        public ActionResult ApproveDiscount()
        {
            return View();
        }

        public async Task<ActionResult> GetRequestedDiscountDetailListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData, long roleId)
        {
            var inputDto = new RequestDisocuntInputDto { RoleId = roleId };
            var result = await _pricingClient.GetRequestedDiscounts(inputDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        [HttpPost]
        public async Task<ActionResult> ApproveRequestDiscount(long id, string reason, int reasonType)
        {
            var requestParam = new ApproveRequestDiscountDto
            {
                Id = id,
                LoginUserId = UserId,
                Reason = reason,
                ReasonType = reasonType
            };
            var result = await _pricingClient.ApproveRequestedDiscount(requestParam);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Final Pricing
        public ActionResult TraditionalFinalPricing()
        {
            var model = new SkuFinalpriceListInputDto() { VerticalId = VerticalId };
            return View(model);
        }

        public ActionResult ReverseAuactionFinalPricing()
        {
            var model = new SkuFinalpriceListInputDto() { VerticalId = VerticalId };
            return View(model);
        }

        //List<SkuFinalpriceListOutputDto>
        /// <summary>
        /// Output param is <see cref="List{DTO.SkuFinalpriceListOutputDto}
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> SearchFinalPrice(SkuFinalpriceListInputDto dto)
        {
            dto.BiddingDate = DateTime.Now;
            var data = await _pricingClient.SearchFinalPricingNew(dto);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> SaveTraditionalFinalPricing(SaveFinalPricngInputDto dto)
        {
            var result = await _pricingClient.SaveTraditionalFinalPricing(dto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> SaveReverseAucationFinalPricing(SaveFinalPricngInputDto dto)
        {
            var result = await _pricingClient.SaveReverseAucationFinalPricing(dto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Geography Discounts

        [AuthorizeClaims(Claims.ManageGeographyDiscount, Claims.ViewGeographyDiscount)]
        public ActionResult GeographyDiscountList()
        {
            var data = new LoginUserIdDto()
            {
                RoleId = RoleId
            };
            return View(data);
        }

        [AuthorizeClaims(Claims.ManageGeographyDiscount, Claims.ViewGeographyDiscount)]
        public async Task<ActionResult> GeographyDiscount()
        {
            var result = new DiscountInputDto();
            if (Session["GeographyDiscountId"] != null && UtilityHelper.IntTryToParse(Session["GeographyDiscountId"].ToString()) > 0)
            {
                result = await _pricingClient.GetGeographyDetailsById(UtilityHelper.LongTryToParse(Session["GeographyDiscountId"].ToString()));

                result.EncryptedId = UtilityHelper.ConvertToMd5(result.Id.ToString(), SecurityConstants.EncryptionKey);
                result.RoleId = RoleId;
            }

            if (result.DivisionId <= 0)
                result.DivisionId = VerticalId;

            return View(result);
        }

        public ActionResult GDEdit(string EncryptedId)
        {
            var geographyDiscountId = "";
            if (!String.IsNullOrEmpty(EncryptedId))
            {
                EncryptedId = EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey);
                geographyDiscountId = decryptedId;
            }

            Session["GeographyDiscountId"] = geographyDiscountId;
            return RedirectToAction("GeographyDiscount", "Pricing");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateGeographyDiscount(DiscountInputDto model)
        {
            var result = new DiscountInputDto();
            if (model != null)
            {

                if (!String.IsNullOrEmpty(model.EncryptedId))
                {
                    model.EncryptedId = model.EncryptedId.Replace(' ', '+');
                    var decryptedId = UtilityHelper.ConvertMd5ToString(model.EncryptedId, SecurityConstants.EncryptionKey);

                    model.Id = UtilityHelper.IntTryToParse(decryptedId);
                }

                model.LoginUserId = UserId;
                result = await _pricingClient.AddDiscountGeography(model);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetCityDetailsBasedOnTerritory([DataSourceRequest] DataSourceRequest request, List<long> territoryIds)
        {
            IList<CityDetails> cityDetails = new List<CityDetails>();
            if (territoryIds != null && territoryIds.Count > 0)
            {
                var inputDto = new TerritoryId { TerritoryIds = territoryIds };
                cityDetails = await _pricingClient.GetCityDetailsBasedOnTerritory(inputDto);
            }
            var resultList = cityDetails.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> GetGeographyList([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData, DateTime Date, string ZoneIds, string StateIds, string DistrictIds, string CityIds,string Status)
        {
            var loginUserIdDto = new LoginUserIdDto()
            {
                IsToReturnInactiveData = isToReturnInactiveData,
                LoginUserId = UserId,
                Date = Date,
                DataSourceRequest = request,
                ZoneIds = ZoneIds,
                StateIds = StateIds,
                DistrictIds = DistrictIds,
                CityIds = CityIds,
                Status = Status
            };

            var geographyList = await _pricingClient.GetGeographyList(loginUserIdDto);
            return Json(geographyList);
        }

        public async Task<ActionResult> GetGeographyCityList([DataSourceRequest] DataSourceRequest request, long id, string ZoneIds, string StateIds, string DistrictIds, string CityIds)
        {
            var inputDto = new GeographyCityListParam()
            {
                Id = id,
                PageNumber = request.Page,
                PageSize = request.PageSize,
                ZoneIds = ZoneIds,
                StateIds = StateIds,
                DistrictIds = DistrictIds,
                CityIds = CityIds
            };

            IList<CityDetails> geographyList = await _pricingClient.GetGeographyCityList(inputDto);
            var resultList = new { Data = geographyList, Total = geographyList.Select(_ => _.TotalRows).FirstOrDefault() };
            return Json(resultList);
        }

        /// <summary>
        /// Method to Export Geography Discounts
        /// </summary>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportGeographyDiscount(DateTime fromDate)
        {
            var finalResult = new JsonResult();
            DateTime currentDate = DateTime.Now;
            string fileName = "GeographyDiscount" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
            var guidFileName = $"{Guid.NewGuid()}.xlsx";
            try
            {
                var resultList = _pricingClient.ExportGeographyDiscount(new ExcelReportFilterDto { FromDate = fromDate });
                if (resultList.IsAny())
                {
                    using (var package = new ExcelPackage())
                    {
                        var ws = package.Workbook.Worksheets.Add("GeographyDiscount");

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
                        ws.Cells["D2:I2"].Value = "Geography Discount";
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

        #region Discount Users        

        [AuthorizeClaims(Claims.ManageDiscounts, Claims.ViewDiscounts)]
        public ActionResult DiscountUserList()
        {
            var data = new LoginUserIdDto()
            {
                RoleId = RoleId,
            };
            return View(data);
        }

        public async Task<ActionResult> GetDiscountUserListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData, DateTime Date)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData, Date = Date };
            var result = await _pricingClient.GetDiscountUserList(loginUserIdDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }
        public async Task<ActionResult> DiscountUserListExport(long Id, DateTime Date)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = true, Date = Date };
            var result = await _pricingClient.DiscountUserExport(loginUserIdDto);
            //var saudaFilterDto = new SaudaListFilterDto() { LoginUserId = UserId, FromDate = fromDate, ToDate = todate, StatusId = statusId, DataFilter = dataFilter, SalesOrganizationId = salesOrganizationId, DistributionChannelId = DistributionChannelId, DivisionId = divisionId };
            //var result = await _reverseAuctionClient.GetSaudaListExport(saudaFilterDto);
            string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
            //var saudaResult = _reportClient.GetRaSaudaOrderReport(fromDate, toDate, stateIds, verticalId, statusIds);

            string fileName = "UserDiscountList" + string.Format(Settings.ReportDateFormat, DateTime.Now.Date).ToUpper() + ".xlsx";
            bool isHeaderBind = false;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                var rowIndex = 7;
                var colIndex = 1;
                var childColIndex = 0;

                #region Header


                worksheet.Cells["A1:M1"].Merge = true;
                worksheet.Cells["A1:M1"].Value = Settings.CompanyName;
                worksheet.Cells["A1:M1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A1:M1"].Style.Font.Bold = true;
                worksheet.Cells["A1:M1"].Style.Font.Size = 16;

                worksheet.Cells["A2"].Value = "Report Name";

                worksheet.Cells["B2"].Value = "UserDiscount Details";
                //worksheet.Cells["B3"].Value = string.Format(Settings.ReportDateFormat, fromDate);
                //worksheet.Cells["B4"].Value = string.Format(Settings.ReportDateFormat, todate);

                for (int i = 2; i <= 4; i++)
                {
                    worksheet.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Cells["A" + i].Style.Font.Bold = true;
                    worksheet.Cells["A" + i].Style.Font.Size = 12;

                    worksheet.Cells["B" + i + ":" + "F" + i].Merge = true;
                    worksheet.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                }

                #endregion

                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Sales Organisation");
                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Distribution Channel");
                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Division Name");
                //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OilType"));
                //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "OilType Code");
                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ValidFrom"));
                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ValidTo"));

                foreach (var userDiscount in result)
                {
                    isHeaderBind = false;
                    rowIndex++;
                    colIndex = 1;
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], userDiscount.SalesOrganization);
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], userDiscount.DistributionChannel.ToString());
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], userDiscount.Division.ToString());
                    //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], userDiscount.OilTypeName.ToString());
                    //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], userDiscount.OilTypeCode);
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], userDiscount.ValidFrom.ToString("dd-MMM-yyyy HH:mm"));
                    //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.SAPCreationDate != null ? saudhaList.SAPCreationDate.ToString(Settings.DateFormat) : string.Empty);
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], userDiscount.ValidTo.ToString("dd-MMM-yyyy HH:mm"));

                    if (userDiscount.DiscountSkuDataList != null && userDiscount.DiscountSkuDataList.Any())
                    {
                        foreach (var discountdetails in userDiscount.DiscountSkuDataList)
                        {
                            if (!isHeaderBind)
                            {
                                rowIndex++;
                                childColIndex = 2;

                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_SkuName"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_SkuCode"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_State"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_Discount"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_EmployeeName"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_Email"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_MobileNumber"));

                                isHeaderBind = true;
                            }
                            rowIndex++;
                            childColIndex = 2;
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], discountdetails.SkuName);
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], discountdetails.SkuCode);
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], discountdetails.State);
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], discountdetails.Discount.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], discountdetails.EmployeeName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], discountdetails.Email.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], discountdetails.MobileNumber.ToString());

                        }
                    }
                }
                worksheet.Cells.AutoFitColumns();
                return SaveExcelFileToPath(package, fileName);
            }
            return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> GetDiscountUserDetailListAsync([DataSourceRequest] DataSourceRequest request, long discountId)
        {
            var inputDto = new GeographyCityListParam { Id = discountId, ParentId = discountId, IsRequestFromWeb = true };
            var result = await _pricingClient.GetDiscountUserDetailList(inputDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public ActionResult DiUERedirect(string EncryptedId = "")
        {
            var discountId = "";
            if (!String.IsNullOrEmpty(EncryptedId))
            {
                EncryptedId = EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey);
                discountId = decryptedId;
            }



            Session["DiscountId"] = discountId;
            return RedirectToAction("DiscountUser", "Pricing");
        }

        [AuthorizeClaims(Claims.ManageDiscounts, Claims.ViewDiscounts)]
        public async Task<ActionResult> DiscountUser()
        {
            var result = new DiscountUserDto();


            if (Session["DiscountId"] != null && UtilityHelper.IntTryToParse(Session["DiscountId"].ToString()) > 0)
            {
                result = await _pricingClient.GetDiscountUserById(UtilityHelper.LongTryToParse(Session["DiscountId"].ToString()));

                result.EncryptedId = UtilityHelper.ConvertToMd5(result.Id.ToString(), SecurityConstants.EncryptionKey);

            }
            result.RoleId = RoleId;
            if (!(result.DivisionId > 0))
                result.DivisionId = VerticalId;
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateDiscountUser(DiscountUserDto model)
        {
            var result = new DiscountUserDto();
            if (model != null)
            {
                model.LoginUserId = UserId;

                if (!String.IsNullOrEmpty(model.EncryptedId))
                {
                    model.EncryptedId = model.EncryptedId.Replace(' ', '+');
                    var decryptedId = UtilityHelper.ConvertMd5ToString(model.EncryptedId, SecurityConstants.EncryptionKey);

                    model.Id = UtilityHelper.IntTryToParse(decryptedId);
                }

                result = await _pricingClient.AddOrUpdateDiscountUser(model);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        [AuthorizeClaims(Claims.ViewAssignedDiscount)]
        public ActionResult EmployeeUserDiscountList()
        {
            var data = new LoginUserIdDto()
            {
                RoleId = RoleId
            };
            return View(data);
        }

        public async Task<ActionResult> EmployeeUserDiscountListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData, DateTime Date)
        {
            var inputDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData, Date = Date, IsRequestFromWeb = true };
            var result = await _pricingClient.GetEmployeeAndUserDiscountList(inputDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> EmployeeUserDiscount()
        {
            var result = new EmployeeUserDiscountDto();
            if (Session["EmpDiscountId"] != null && UtilityHelper.IntTryToParse(Session["EmpDiscountId"].ToString()) > 0)
            {
                var inputDto = new IdInputDto() { Id = UtilityHelper.LongTryToParse(Session["EmpDiscountId"].ToString()) };
                result = await _pricingClient.GetEmployeeAndUserDiscountById(inputDto);
                result.RoleId = RoleId;
                result.EncryptedId = UtilityHelper.ConvertToMd5(result.Id.ToString(), SecurityConstants.EncryptionKey);

            }
            return View(result);
        }

        public ActionResult EUDEditRed(string EncryptedId = "")
        {
            var discountId = "";
            if (!String.IsNullOrEmpty(EncryptedId))
            {
                EncryptedId = EncryptedId.Replace(' ', '+');
                discountId = UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey);
            }

            Session["EmpDiscountId"] = discountId;
            return RedirectToAction("EmployeeUserDiscount", "Pricing");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> AddOrUpdateEmployeeUserDiscount(EmployeeUserDiscountDto inputDto)
        {
            inputDto.LoginUserId = UserId;

            if (!String.IsNullOrEmpty(inputDto.EncryptedId))
            {
                inputDto.EncryptedId = inputDto.EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey);

                inputDto.Id = UtilityHelper.IntTryToParse(decryptedId);
            }

            var result = await _pricingClient.AddEmployeeAndUserDiscount(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("EmployeeUserDiscountList", "Pricing");
            }
            return View("EmployeeUserDiscount", result);
        }

        #endregion

        #region   PriceNotifyConfiguration

        [AuthorizeClaims(Claims.ManagePriceNotification)]
        public ActionResult PriceNotifyConfigurationList()
        {
            return View();
        }

        [AuthorizeClaims(Claims.ManagePriceNotification)]
        public async Task<ActionResult> PriceNotifyConfiguration()
        {
            var result = new PriceNotifyConfigurationDto();
            Session["CityIds"] = null;
            if (Session["PriceNotifyId"] != null && UtilityHelper.IntTryToParse(Session["PriceNotifyId"].ToString()) > 0)
            {
                result = await _pricingClient.GetPriceNotifyConfiguratioDetailsById(UtilityHelper.LongTryToParse(Session["PriceNotifyId"].ToString()));
                if (result != null)
                {
                    Session["CityIds"] = result.CityId;
                    result.CityIdstr = UtilityHelper.ConvertLongListToCommaSeparatedString(result.CityId.ToList());
                }
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> AddorUpdatePriceNotifyConfiguration(PriceNotifyConfigurationDto model)
        {
            var result = new PriceNotifyConfigurationDto();
            if (model != null)
            {
                model.LoginUserId = UserId;
                result = await _pricingClient.AddorUpdatePriceNotifyConfiguration(model);
                if (result.PostStatus)
                {
                    TempData["SuccessMessage"] = result.PostMessage;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to Get Price Notify Configuration List 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetPriceNotifyConfigurationList([DataSourceRequest] DataSourceRequest request, SaudaLimitInputDto saudaLimitInputDto)
        {
            IList<PriceNotifyConfigurationDto> result = new List<PriceNotifyConfigurationDto>();
            if (saudaLimitInputDto != null && saudaLimitInputDto.FromDate != null && saudaLimitInputDto.ToDate != null)
            {
                saudaLimitInputDto.LoginUserId = UserId;
                result = await _pricingClient.GetPriceNotifyConfigurationListAsync(saudaLimitInputDto);
            }
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> GetPriceNotifyConfigurationCityList([DataSourceRequest] DataSourceRequest request, long id)
        {
            var inputDto = new IdInputDto() { Id = id };
            IList<CityDetails> geographyList = await _pricingClient.GetPriceNotifyConfigurationCityList(inputDto);
            var resultList = geographyList.ToDataSourceResult(request);
            return Json(resultList);
        }

        public ActionResult PNCEdit(string priceNotifyId)
        {
            Session["PriceNotifyId"] = priceNotifyId;
            return RedirectToAction("PriceNotifyConfiguration", "Pricing");
        }

        #endregion

        #region SpecialityFat Geography Discounts

        [AuthorizeClaims(Claims.ManageSpecialtyFatQuantityGeography)]
        public ActionResult SpecialityFatGeographyDiscountList()
        {
            return View();
        }

        public async Task<ActionResult> GetSpecialityFatGeographyList([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            var loginUserIdDto = new LoginUserIdDto() { IsToReturnInactiveData = isToReturnInactiveData, LoginUserId = UserId };
            IList<SpecialityFatDiscountOutputDto> geographyList = await _pricingClient.GetSpecialtyFatGeographyList(loginUserIdDto);
            var resultList = geographyList.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> GetSpecialityFatGeographyCityList([DataSourceRequest] DataSourceRequest request, long id)
        {
            var inputDto = new GeographyCityListParam() { Id = id };
            IList<CityDetails> geographyList = await _pricingClient.GetSpecialtyFatGeographyCityList(inputDto);
            var resultList = geographyList.ToDataSourceResult(request);
            return Json(resultList);
        }

        public ActionResult SpecialityFatGeographyDiscountEdit(string geographyDiscountId)
        {
            Session["SpecialityFatGeographyDiscountId"] = geographyDiscountId;
            return RedirectToAction("SpecialityFatGeographyDiscount", "Pricing");
        }

        [AuthorizeClaims(Claims.ManageSpecialtyFatQuantityGeography)]
        public async Task<ActionResult> SpecialityFatGeographyDiscount()
        {
            var result = new SpecialityFatDiscountInputDto();
            if (Session["SpecialityFatGeographyDiscountId"] != null && UtilityHelper.IntTryToParse(Session["SpecialityFatGeographyDiscountId"].ToString()) > 0)
            {
                result = await _pricingClient.GetSpecialtyFatGeographyDetailsById(UtilityHelper.LongTryToParse(Session["SpecialityFatGeographyDiscountId"].ToString()));
            }
            if (!(result.VerticleId > 0))
                result.VerticleId = VerticalId;
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> AddOrUpdateSpecialityFatGeographyDiscount(SpecialityFatDiscountInputDto model)
        {
            var result = new SpecialityFatDiscountInputDto();
            if (model != null)
            {
                model.LoginUserId = UserId;
                result = await _pricingClient.AddSpecialtyFatDiscountGeography(model);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetSpecialtyFatCityDetailsBasedOnCityTerritory([DataSourceRequest] DataSourceRequest request, List<long> territoryIds)
        {
            IList<CityDetails> cityDetails = new List<CityDetails>();
            var cityIdsNew = new List<long>();
            if (territoryIds != null && territoryIds.Count > 0)
            {
                var inputDto = new TerritoryId { TerritoryIds = territoryIds };
                cityDetails = await _pricingClient.GetSpecialtyFatCityDetailsBasedOnCityTerritory(inputDto);
            }
            var resultList = cityDetails.ToDataSourceResult(request);
            return Json(resultList);
        }

        #endregion

        #region SpecialityFat User Discount

        [AuthorizeClaims(Claims.ManageSpecialtyFatQuantityUser)]
        public ActionResult SpecialityFatDiscountUserList()
        {
            return View();
        }

        public async Task<ActionResult> GetSpecialityFatDiscountUserListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            var verticalId = VerticalId;
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData, VerticalId = verticalId };
            var result = await _pricingClient.GetSpecialityFatDiscountUserList(loginUserIdDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }
        public async Task<ActionResult> SpecialityFatDiscountUserExportAsync()
        {
            var verticalId = VerticalId;
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId };
            var result = await _pricingClient.SpecialityFatDiscountUserExportAsync(loginUserIdDto);

            string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
            //var saudaResult = _reportClient.GetRaSaudaOrderReport(fromDate, toDate, stateIds, verticalId, statusIds);

            string fileName = "EmployeeQuantityLimit_" + string.Format(Settings.ReportDateFormat, DateTime.Now.Date).ToUpper() + ".xlsx";
            bool isHeaderBind = false;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                var rowIndex = 7;
                var colIndex = 1;
                var childColIndex = 0;

                #region Header


                worksheet.Cells["A1:M1"].Merge = true;
                worksheet.Cells["A1:M1"].Value = Settings.CompanyName;
                worksheet.Cells["A1:M1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A1:M1"].Style.Font.Bold = true;
                worksheet.Cells["A1:M1"].Style.Font.Size = 16;

                worksheet.Cells["A2"].Value = "Report Name";

                worksheet.Cells["B2"].Value = "Employee Quantity Limit_";

                for (int i = 2; i <= 4; i++)
                {
                    worksheet.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Cells["A" + i].Style.Font.Bold = true;
                    worksheet.Cells["A" + i].Style.Font.Size = 12;

                    worksheet.Cells["B" + i + ":" + "F" + i].Merge = true;
                    worksheet.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                }

                #endregion

                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OilType"));
                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ValidFrom"));
                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ValidTo"));


                foreach (var discount in result)
                {
                    isHeaderBind = false;
                    rowIndex++;
                    colIndex = 1;
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], discount.OilTypeName);
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], discount.ValidFrom.ToString());
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], discount.ValidTo.ToString());


                    if (discount.InnerList.IsAny())
                    {
                        foreach (var saudaorders in discount.InnerList)
                        {
                            if (!isHeaderBind)
                            {
                                rowIndex++;
                                childColIndex = 2;

                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_SkuName"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_SkuCode"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_EmployeeName"));
                                //GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_Designation"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_MobileNumber"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_Email"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_Quantity"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_RemainingQuantity"));
                                isHeaderBind = true;
                            }
                            rowIndex++;
                            childColIndex = 2;
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.SkuName);
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.SkuCode);
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.EmployeeName);
                            //GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.Designation!=null ? saudaorders.Designation.ToString():String.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.MobileNumber.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.Email.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.Quantity.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.RemainingQuantity.ToString());
                        }
                    }
                }
                worksheet.Cells.AutoFitColumns();
                return SaveExcelFileToPath(package, fileName);
            }
            return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);

        }
        public async Task<ActionResult> GetSpecialityFatDiscountUserDetailListAsynx([DataSourceRequest] DataSourceRequest request, long discountId)
        {
            var inputDto = new GeographyCityListParam { Id = discountId, ParentId = discountId };
            var result = await _pricingClient.GetSpecialityFatDiscountUserDetailList(inputDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public ActionResult SDUEditRedirect(string EncryptedId = "")
        {
            EncryptedId = EncryptedId.Replace(' ', '+');
            var decryptedId = UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey);

            var discountId = UtilityHelper.IntTryToParse(decryptedId);
            Session["SpecialityFatDiscountId"] = discountId;
            return RedirectToAction("SpecialityFatDiscountUser", "Pricing");
        }

        [AuthorizeClaims(Claims.ManageSpecialtyFatQuantityUser)]
        public async Task<ActionResult> SpecialityFatDiscountUser()
        {
            var result = new SpecialityFatDiscountUserDto();
            if (Session["SpecialityFatDiscountId"] != null && UtilityHelper.IntTryToParse(Session["SpecialityFatDiscountId"].ToString()) > 0)
            {
                result = await _pricingClient.GetSpecialityFatDiscountUserById(UtilityHelper.LongTryToParse(Session["SpecialityFatDiscountId"].ToString()));

                result.EncryptedId = UtilityHelper.ConvertToMd5(result.Id.ToString(), SecurityConstants.EncryptionKey);

            }

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateSpecialityFatDiscountUser(SpecialityFatDiscountUserDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            if (!String.IsNullOrEmpty(inputDto.EncryptedId))
            {
                inputDto.EncryptedId = inputDto.EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey);

                inputDto.Id = UtilityHelper.IntTryToParse(decryptedId);
            }


            var result = await _pricingClient.AddOrUpdateSpecialityFatDiscountUser(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("SpecialityFatDiscountUserList", "Pricing");
            }
            return View("SpecialityFatDiscountUser", result);
        }

        [AuthorizeClaims(Claims.ViewSpecialtyFatAssignedQuantity)]
        public ActionResult SpecialityFatEmployeeDiscountList()
        {
            SpecialtyFatQuantityRequestDto input = new SpecialtyFatQuantityRequestDto();
            return View(input);
        }

        public async Task<ActionResult> SpecialityFatEmployeeDiscountListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            var verticalId = VerticalId;
            var inputDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData, VerticalId = verticalId };
            var result = await _pricingClient.GetSpecialityFatEmployeeDiscountList(inputDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }
        public async Task<ActionResult> SpecialityFatEmployeeDiscountExportAsync()
        {
            var specialityFatDiscountUsers = await _pricingClient.GetSpecialityFatEmployeeDiscountExport(new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = true, VerticalId = VerticalId });

            //var fileName = $"{Guid.NewGuid()}.xlsx";
            var fileName = "Assigned-Qty-Limit" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";
            var stream = new MemoryStream();
            var result = new ResultModel { IsSuccess = false, Message = "Error Occured while exporting Excel. Please retry." };
            bool isHeaderBind = false;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                var rowIndex = 7;
                var colIndex = 1;
                var childColIndex = 0;

                #region Header


                worksheet.Cells["A1:M1"].Merge = true;
                worksheet.Cells["A1:M1"].Value = Settings.CompanyName;
                worksheet.Cells["A1:M1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A1:M1"].Style.Font.Bold = true;
                worksheet.Cells["A1:M1"].Style.Font.Size = 16;

                worksheet.Cells["A2"].Value = "Report Name";

                worksheet.Cells["B2"].Value = "Assigned Qty Limit Details";

                for (int i = 2; i <= 4; i++)
                {
                    worksheet.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Cells["A" + i].Style.Font.Bold = true;
                    worksheet.Cells["A" + i].Style.Font.Size = 12;

                    worksheet.Cells["B" + i + ":" + "F" + i].Merge = true;
                    worksheet.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                }

                #endregion

                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OilType"));
                //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "OilType Code");
                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_EmployeeName"));
                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ValidFrom"));
                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ValidTo"));
                //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OilType"));
                //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SAPCreationDate"));
                //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_RatePerMT"));

                foreach (var saudhaList in specialityFatDiscountUsers)
                {
                    isHeaderBind = false;
                    rowIndex++;
                    colIndex = 1;
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.OilTypeName);
                    //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.OilTypeCode.ToString());
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.EmployeeName.ToString());
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.ValidFrom.ToString());
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.ValidTo.ToString());
                    //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.saudhaListOilTypes);
                    //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.SAPCreationDate != null ? saudhaList.SAPCreationDate.ToString(Settings.DateFormat) : string.Empty);
                    //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.RatePerMT.ToString());

                    if (saudhaList.SpecialityFatDiscountDetails != null && saudhaList.SpecialityFatDiscountDetails.Any())
                    {
                        foreach (var saudaorders in saudhaList.SpecialityFatDiscountDetails)
                        {
                            if (!isHeaderBind)
                            {
                                rowIndex++;
                                childColIndex = 2;


                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_SkuName"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_SkuCode"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], "Quantity Limit (MT)");

                                isHeaderBind = true;
                            }
                            rowIndex++;
                            childColIndex = 2;
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.SkuName);
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.SkuCode);
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.QuantityLimit.ToString());

                        }
                    }
                }
                worksheet.Cells.AutoFitColumns();
                return SaveExcelFileToPath(package, fileName);
            }
            return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);

        }
        public async Task<ActionResult> GetSpecialityFatDiscountEmployeeDetailListAsynx([DataSourceRequest] DataSourceRequest request, long discountId)
        {
            var inputDto = new GeographyCityListParam { Id = discountId, ParentId = discountId, VerticalId = VerticalId };
            var result = await _pricingClient.GetSpecialityFatDiscountEmployeeDetailListAsynx(inputDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> SpecialityFatEmployeeDiscount()
        {
            var result = new SpecialityFatEmployeeDiscountDto();
            if (Session["SpecialityFatEmpDiscountId"] != null && UtilityHelper.IntTryToParse(Session["SpecialityFatEmpDiscountId"].ToString()) > 0)
            {
                var inputDto = new IdInputDto() { Id = UtilityHelper.LongTryToParse(Session["SpecialityFatEmpDiscountId"].ToString()) };
                result = await _pricingClient.GetSpecialityFatEmployeeDiscountById(inputDto);
                result.EncryptedId = UtilityHelper.ConvertToMd5(result.Id.ToString(), SecurityConstants.EncryptionKey);
            }
            result.RoleId = RoleId;
            return View(result);
        }

        public ActionResult SpeEDEdit(string EncryptedId = "")
        {
            var discountId = "";

            if (!String.IsNullOrEmpty(EncryptedId))
            {
                EncryptedId = EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey);
                discountId = decryptedId;


            }

            Session["SpecialityFatEmpDiscountId"] = discountId;
            return RedirectToAction("SpecialityFatEmployeeDiscount", "Pricing");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> AddOrUpdateSpecialityFatEmployeeDiscount(SpecialityFatEmployeeDiscountDto inputDto)
        {
            inputDto.LoginUserId = UserId;


            if (!String.IsNullOrEmpty(inputDto.EncryptedId))
            {
                inputDto.EncryptedId = inputDto.EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey);

                inputDto.Id = UtilityHelper.IntTryToParse(decryptedId);
            }

            var result = await _pricingClient.AddSpecialityFatEmployeeDiscount(inputDto);


            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("SpecialityFatEmployeeDiscountList", "Pricing");
            }
            return View("SpecialityFatEmployeeDiscount", result);
        }

        [HttpPost]
        public async Task<ActionResult> SaveRequestQuantityLimit(SpecialtyFatQuantityRequestDto inputDto)
        {
            inputDto.UserId = UserId;
            inputDto.LoginUserId = UserId;
            inputDto.VerticleId = VerticalId;
            var result = await _pricingClient.SaveRequestQuantityLimit(inputDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region SpecalityFat Quantity Request

        [AuthorizeClaims(Claims.ViewSpecialtyFatQtyRequestStatus)]
        public ActionResult SpecalityFatQuantityRequestList()
        {
            var specialtyFatQuantityRequestDto = new SpecialtyFatQuantityRequestDto()
            {
                LoginUserId = UserId,
                RoleId = RoleId
            };
            return View(specialtyFatQuantityRequestDto);
        }

        /// <summary>
        /// Method to Approve SpecalityFat Quantity Request
        /// </summary>
        /// <param name="questionIdDto"></param>
        /// <returns></returns>
        //[AuthorizeClaims(Claims.ManageSaudaLimit)]
        [HttpPost]
        public async Task<ActionResult> ApproveorRejectSpecalityFatQuantityRequest(IList<long> checkedLimitRequestIds, int status, string remark = null)
        {
            var specialtyFatQuantityRequestDto = new SpecialtyFatQuantityRequestDto()
            {
                QuantityRequestIds = checkedLimitRequestIds,
                Remarks = remark ?? null,
                StatusId = status,
                LoginUserId = UserId,
                RoleId = RoleId
            };

            var saudaApprovalViewModel = await _pricingClient.ApproveorRejectSpecalityFatQuantityRequest(specialtyFatQuantityRequestDto);
            return Json(saudaApprovalViewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to Get SpecalityFat Quantity Request List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetSpecalityFatQuantityRequestListAsync([DataSourceRequest] DataSourceRequest request, SpecialtyFatQuantityRequestSearchDto specialtyFatQuantityRequestSearchDto)
        {
            var result = new List<SpecialtyFatQuantityRequestDto>();
            if (specialtyFatQuantityRequestSearchDto != null)
            {
                specialtyFatQuantityRequestSearchDto.LoginUserId = UserId;
                specialtyFatQuantityRequestSearchDto.VerticalId = VerticalId;
                result = await _pricingClient.GetSpecalityFatQuantityRequestListAsync(specialtyFatQuantityRequestSearchDto);
                if (result != null && result.Count > 0 && result[0].PostStatus == false)
                {
                    ModelState.AddModelError("SaudaLimit", result[0].PostMessage);
                    return Json(result.AsQueryable().ToDataSourceResult(request, ModelState));
                }

            }
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        /// <summary>
        /// Get  order status 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSpecalityFatSelectiveStatus()
        {
            var statusIds = new long[] { (int)DTO.Enums.Status.Pending, (int)DTO.Enums.Status.RequestForApproval, (int)DTO.Enums.Status.Approved, (int)DTO.Enums.Status.Rejected };
            var result = _pricingClient.GetAllStatus().Where(w => statusIds.Contains(w.Id));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SpecialtyFatQuantityRequestStatus()
        {
            return View();
        }

        public async Task<ActionResult> GetSpecialtyFatQuantityRequestStatusAsync([DataSourceRequest] DataSourceRequest request)
        {
            IList<SpecialtyFatQuantityRequestDto> result = new List<SpecialtyFatQuantityRequestDto>();
            var inputDto = new SpecialtyFatQuantityRequestSearchDto() { LoginUserId = UserId, VerticalId = VerticalId };
            result = await _pricingClient.GetSpecialtyFatQuantityRequestStatusAsync(inputDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        #endregion

        #region Auto Allocation

        [AuthorizeClaims(Claims.AutoAllocation)]
        public ActionResult AutoAllocation()
        {
            Session["autoAllocation"] = null;
            var result = new AutoAllocationInputDto();
            if (Session["autoAllocationResult"] != null)
            {
                var autoallocationresult = Session["autoAllocationResult"] as SaveAutoAllocationDetailDto;
                if (autoallocationresult != null)
                {
                    result.PostStatus = autoallocationresult.PostStatus;
                    result.PostMessage = autoallocationresult.PostMessage;
                }
            }
            Session["autoAllocationResult"] = null;
            return View(result);
        }

        public async Task<ActionResult> GetAutoAllocationUserListAsync([DataSourceRequest] DataSourceRequest request, string roleIds)
        {
            List<AutoAllocationDto> result = new List<AutoAllocationDto>();
            if (!string.IsNullOrEmpty(roleIds))
            {
                AutoAllocationInputDto autoAllocationInputDto = new AutoAllocationInputDto { RoleIds = roleIds, VerticalId = (int)DTO.Enums.Division.SpecialityFat };
                result = await _pricingClient.GetAutoAllocationUserListByRoleIds(autoAllocationInputDto);
            }
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> GetAutoAllocationDetailsByUserIdAsync([DataSourceRequest] DataSourceRequest request, long userId, long averageDays)
        {
            List<AutoAllocationDetailDto> result = new List<AutoAllocationDetailDto>();
            if (userId > 0)
            {
                AutoAllocationInputDto autoAllocationInputDto = new AutoAllocationInputDto { UserId = userId, VerticalId = (int)DTO.Enums.Division.SpecialityFat, AverageDays = averageDays };
                result = await _pricingClient.GetAutoAllocationDetailsByUserId(autoAllocationInputDto);
            }
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> SaveAutoAllocation(string validFrom, string ValidTo)
        {
            var result = new SaveAutoAllocationDetailDto();
            Session["autoAllocationResult"] = null;
            if (Session["autoAllocation"] != null)
            {
                var autoallocation = Session["autoAllocation"] as List<AutoAllocationDetailDto>;
                if (autoallocation.Count > 0)
                {
                    foreach (var item in autoallocation)
                    {
                        item.ValidFrom = Convert.ToDateTime(validFrom);
                        item.ValidTo = Convert.ToDateTime(ValidTo);
                        item.LoginUserId = UserId;
                    }
                    result = await _pricingClient.SaveAutoAllocation(autoallocation);
                    Session["autoAllocationResult"] = result;
                }
                else
                {
                    result.PostStatus = false;
                    result.PostMessage = Helper.GetResourceString("msg_Selectatlestonetoproceed");
                }
            }
            else
            {
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_Selectatlestonetoproceed");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public bool SetToSession(AutoAllocationDetailDto autoAllocationDetailDto)
        {
            List<AutoAllocationDetailDto> autoAllocations = new List<AutoAllocationDetailDto>();
            if (Session["autoAllocation"] != null)
            {
                var autoallocation = Session["autoAllocation"] as List<AutoAllocationDetailDto>;
                autoAllocations.AddRange(autoallocation);
            }

            if (autoAllocationDetailDto.IsChecked == false)
            {
                var checkexist = autoAllocations.Where(_ => _.UserId == autoAllocationDetailDto.UserId && _.SkuId == autoAllocationDetailDto.SkuId).ToList();
                if (checkexist != null && checkexist.Count > 0)
                {
                    autoAllocations.RemoveAll(_ => _.UserId == autoAllocationDetailDto.UserId && _.SkuId == autoAllocationDetailDto.SkuId);
                }
            }
            else
            {
                autoAllocations.Add(autoAllocationDetailDto);
            }
            Session["autoAllocation"] = autoAllocations;
            return true;
        }

        #endregion


        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }


        #region Generate Final Price

        [AuthorizeClaims(Claims.ManageTPFinalPrice)]
        public ActionResult TraditionalProcessFinalPriceGenerate()
        {
            var model = new SkuFinalpriceListInputDto() { VerticalId = VerticalId };
            return View(model);
        }


        //List<SkuFinalpriceListOutputDto>
        /// <summary>
        /// Output param is <see cref="List{DTO.SkuFinalpriceListOutputDto}
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GenerateFinalPriceList(SkuFinalpriceListInputDto dto)
        {
            dto.BiddingDate = DateTime.Now;
            var data = await _pricingClient.GenerateFinalPriceListAsync(dto);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Publish Price               

        [AuthorizeClaims(Claims.ManageTPFinalPrice)]
        public ActionResult TraditionalProcessPublishedPrice()
        {
            return View();
        }


        public async Task<ActionResult> GetPublishedPriceDetailsAsync([DataSourceRequest] DataSourceRequest request, DateTime searchDate, long bookingTypeId)
        {
            var inputDto = new PricePublishInputDto() { LoginUserId = UserId, SearchDate = searchDate, SaudaBookingTypeId = bookingTypeId };
            var result = await _pricingClient.GetPublishedPriceDetails(inputDto);
            if (result != null && result.Any())
            {
                result.ForEach(f =>
                {
                    f.StartDate = ConvertUTCToIndiaTime(f.StartDate);
                    f.EndDate = ConvertUTCToIndiaTime(f.EndDate);
                    f.PublishDate = ConvertUTCToIndiaTime(f.PublishDate);
                });
            }
            var resultList = result.ToDataSourceResult(request);
            //return Json(resultList);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //List<SkuFinalpriceListOutputDto>
        /// <summary>
        /// Output param is <see cref="List{DTO.SkuFinalpriceListOutputDto}
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> SearchFinalPriceList(SkuFinalpriceListInputDto dto)
        {
            dto.BiddingDate = DateTime.Now;
            var data = await _pricingClient.SearchFinalPriceList(dto);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Traditional Process and Revers Auction final price publish
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<JsonResult> PublishFinalPrice(FinalPricePublishDto inputDto)
        {
            inputDto.BiddingDate = DateTime.Now;
            inputDto.LoginUserId = UserId;
            var data = await _pricingClient.PublishFinalPrice(inputDto);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetPublishedFinalPriceList(long publishId, long FinalPriceRecordCount)
        {
            var publishData = new List<PricingDto>();
            List<PricingDto> temppricingDto = new List<PricingDto>();
            double loopCount = Math.Round(Convert.ToDouble(FinalPriceRecordCount / 50000.00));
            int SkipCount = 0;
            //for (int i = 0; i < loopCount; i++)
            //{
            //    var inputDto = new FinalPricePublishDto() { LoginUserId = UserId, PublishId = publishId, FinalPriceRecordCount = FinalPriceRecordCount, SkipCount = SkipCount };
            //    publishData = await _pricingClient.GetPublishedFinalPriceListAsync(inputDto);
            //    SkipCount = SkipCount + 50000;
            //    temppricingDto.AddRange(publishData);
            //}

            var inputDto = new FinalPricePublishDto() { LoginUserId = UserId, PublishId = publishId, FinalPriceRecordCount = FinalPriceRecordCount, SkipCount = SkipCount };
            temppricingDto = await _pricingClient.GetPublishedFinalPriceListAsync(inputDto);

            string fileGuid = Guid.NewGuid().ToString(); ;
            string fileName = "";
            string guidFileName = "";

            if (temppricingDto != null && temppricingDto.Any())
            {
                var result = temppricingDto.FirstOrDefault();
                //result.StartDate = ConvertUTCToIndiaTime(result.StartDate);
                //result.EndDate = ConvertUTCToIndiaTime(result.EndDate);
                //result.StartDate = ConvertUTCToIndiaTime(result.StartDate);

                //if (result.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess)
                fileName = "-SUCCESS-" + string.Format("{0:dd-MMM-yyyy}", result.StartDate).ToUpper() + "TIME-" + string.Format("{0:hh:mm tt}", result.StartDate) + "-TO-" + string.Format("{0:hh:mm tt}", result.EndDate) + ".xlsx";
                //else if (result.SaudaBookingTypeId == (int)SaudaBookingTypes.ReverseAuction)
                //    fileName = result.SaudaBookingType.Replace(" ", "").ToUpper() + "-SUCCESS-" + string.Format("{0:dd-MMM-yyyy}", result.StartDate).ToUpper() + "TIME-" + string.Format("{0:hh:mm tt}", result.StartDate) + "-TO-" + string.Format("{0:hh:mm tt}", result.EndDate) + ".xlsx";

                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                string templatePath = Path.Combine(serverFoloderPath + "FinalPriceTemplate.xlsx");

                using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                {
                    var ws = ep.Workbook.Worksheets[1];
                    ws.Name = "Final Price";
                    #region Header
                    ws.Cells["A1:F1"].Merge = true;
                    ws.Cells["A1:F1"].Value = Settings.CompanyName;
                    ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["A1:F1"].Style.Font.Bold = true;
                    ws.Cells["A1:F1"].Style.Font.Size = 16;

                    ws.Cells["A2"].Value = "Report Name";
                    ws.Cells["A3"].Value = "Process Start Date Time";
                    ws.Cells["A4"].Value = "Process End Date Time";
                    ws.Cells["A5"].Value = "Status";
                    //if (result.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess)
                    //{
                    ws.Cells["A6"].Value = "Total Record Count";
                    //}
                    //else if (result.SaudaBookingTypeId == (int)SaudaBookingTypes.ReverseAuction)
                    //{
                    //    ws.Cells["A6"].Value = "Bidding Window Timing";
                    //    ws.Cells["A7"].Value = "Total Record Count";
                    //}

                    //ws.Cells["B2"].Value = result.SaudaBookingType;
                    ws.Cells["B3"].Value = string.Format("{0:dd-MMM-yyyy hh:mm tt}", result.StartDate);
                    ws.Cells["B4"].Value = string.Format("{0:dd-MMM-yyyy hh:mm tt}", result.EndDate);
                    ws.Cells["B5"].Value = result.Status;
                    //if (result.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess)
                    //{
                    ws.Cells["B6"].Value = temppricingDto.Count;
                    // }
                    //else if (result.SaudaBookingTypeId == (int)SaudaBookingTypes.ReverseAuction)
                    //{
                    //    ws.Cells["B6"].Value = result.BiddingWindowTiming;
                    //    ws.Cells["B7"].Value = temppricingDto.Count;
                    //}

                    for (int i = 2; i <= 7; i++)
                    {
                        ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["A" + i].Style.Font.Bold = true;
                        ws.Cells["A" + i].Style.Font.Size = 12;

                        ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                        ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    }

                    #endregion

                    int headerIndex = 8;
                    ws.Cells["A" + headerIndex].Value = "Sku Name";
                    ws.Cells["B" + headerIndex].Value = "Oil Type Name";
                    ws.Cells["C" + headerIndex].Value = "Sauda Booking Type";
                    ws.Cells["D" + headerIndex].Value = "Oil Packing Type";
                    ws.Cells["E" + headerIndex].Value = "State";
                    //ws.Cells["F" + headerIndex].Value = "Transport Mode";
                    ws.Cells["G" + headerIndex].Value = "Loadability";
                    ws.Cells["H" + headerIndex].Value = "Plant";
                    //ws.Cells["I" + headerIndex].Value = "Depot";
                    //ws.Cells["J" + headerIndex].Value = "Frieght Zone";
                    //ws.Cells["K" + headerIndex].Value = "Frieght Route";
                    ws.Cells["L" + headerIndex].Value = "Publish Date";
                    ws.Cells["M" + headerIndex].Value = "Material Cost";
                    ws.Cells["N" + headerIndex].Value = "Packing Cost";
                    ws.Cells["O" + headerIndex].Value = "Primary Frieght";
                    ws.Cells["P" + headerIndex].Value = "Depot Secondary Frieght";
                    ws.Cells["Q" + headerIndex].Value = "Plant Secondary Frieght";
                    ws.Cells["R" + headerIndex].Value = "Depot Cost";
                    ws.Cells["S" + headerIndex].Value = "Detention Cost";
                    ws.Cells["T" + headerIndex].Value = "Honeycomb Cost";
                    ws.Cells["U" + headerIndex].Value = "Margin";
                    ws.Cells["V" + headerIndex].Value = "Cushion Margin";
                    ws.Cells["W" + headerIndex].Value = "Scheme Cost Recovery";

                    //if (result.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess)
                    //{
                    //ws.Cells["X" + headerIndex].Value = "Ex Plant Price";
                    //ws.Cells["Y" + headerIndex].Value = "For Depot Price";
                    //ws.Cells["Z" + headerIndex].Value = "For Plant Price";
                    //ws.Cells["AA" + headerIndex].Value = "Ex Depot Price";
                    //ws.Cells["AB" + headerIndex].Value = "ExRake Price";
                    //ws.Cells["AC" + headerIndex].Value = "For Rake Price";
                    //ws.Cells["AC" + headerIndex].Value = "Loadability";
                    //}
                    //else if (result.SaudaBookingTypeId == (int)SaudaBookingTypes.ReverseAuction)
                    //{
                    //    ws.Cells["X" + headerIndex].Value = "Discount";
                    //    ws.Cells["Y" + headerIndex].Value = "Premium";
                    //    ws.Cells["Z" + headerIndex].Value = "Process Cost";
                    //    ws.Cells["AA" + headerIndex].Value = "Sum Of Ingredient Cost";
                    //    ws.Cells["AB" + headerIndex].Value = "TpPrice";
                    //    ws.Cells["AC" + headerIndex].Value = "Ra Margin";
                    //    ws.Cells["AD" + headerIndex].Value = "Base Rate";
                    //    ws.Cells["AE" + headerIndex].Value = "XMargin";
                    //    ws.Cells["AF" + headerIndex].Value = "Final Rate";
                    //    ws.Cells["AG" + headerIndex].Value = "Ex Plant Price";
                    //    ws.Cells["AH" + headerIndex].Value = "For Depot Price";
                    //    ws.Cells["AI" + headerIndex].Value = "For Plant Price";
                    //    ws.Cells["AJ" + headerIndex].Value = "Ex Depot Price";
                    //    ws.Cells["AK" + headerIndex].Value = "Clearance Rate";
                    //    ws.Cells["AL" + headerIndex].Value = "Counter BidOffer";
                    //    ws.Cells["AM" + headerIndex].Value = "Counter BidLimit";
                    //    ws.Cells["AN" + headerIndex].Value = "BpCp Jumb";
                    //    ws.Cells["AO" + headerIndex].Value = "ExRake Price";
                    //    ws.Cells["AP" + headerIndex].Value = "For Rake Price";
                    //}

                    ExcelRange range = ws.Cells["A8:AQ8"];
                    range.AutoFitColumns();
                    range.Style.Font.Size = 12;
                    range.Style.Font.Bold = true;
                    int contentIndex = 9;

                    string createdDate = "";
                    //if (result.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess)
                    //    createdDate = string.Format("{0:dd-MMM-yyyy}", result.StartDate);
                    //else if (result.SaudaBookingTypeId == (int)SaudaBookingTypes.ReverseAuction)
                    //    createdDate = string.Format("{0:dd-MMM-yyyy}", result.BiddingDate);
                    //createdDate = string.Format("{0:dd-MMM-yyyy}", result.BiddingDate);

                    foreach (var data in temppricingDto)
                    {
                        ws.Cells["A" + contentIndex].Value = data.SkuName;  // "Sku Name";
                        ws.Cells["B" + contentIndex].Value = data.OilTypeName; // "Oil Type Name";
                        //ws.Cells["C" + contentIndex].Value = data.SaudaBookingType; // "Sauda Booking Type";
                        ws.Cells["D" + contentIndex].Value = data.OilPackingType; // "Oil Packing Type";
                        ws.Cells["E" + contentIndex].Value = data.State; // "State";
                        //ws.Cells["F" + contentIndex].Value = data.TransportMode; // "Transport Mode";
                        ws.Cells["G" + contentIndex].Value = Helper.DecimalFormat4(data.Loadability); // "Loadability";
                        ws.Cells["H" + contentIndex].Value = data.Plant; // "Plant";
                        //ws.Cells["I" + contentIndex].Value = data.Depot; // "Depot";
                        //ws.Cells["J" + contentIndex].Value = data.FrieghtZone; // "Frieght Zone";
                        //ws.Cells["K" + contentIndex].Value = data.FrieghtRoute; //"Frieght Route";
                        //if (result.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess)
                        //    ws.Cells["L" + contentIndex].Value = createdDate; //"Created Date";
                        //else if (result.SaudaBookingTypeId == (int)SaudaBookingTypes.ReverseAuction)
                        //    ws.Cells["L" + contentIndex].Value = createdDate; //"Bidding Date";
                        ws.Cells["L" + contentIndex].Value = createdDate; //"Bidding Date";
                        ws.Cells["M" + contentIndex].Value = data.MaterialCost; //"Material Cost";
                        ws.Cells["N" + contentIndex].Value = data.PackingCost; // "Packing Cost";
                        ws.Cells["O" + contentIndex].Value = data.PrimaryFrieght; // "Primary Frieght";
                        ws.Cells["P" + contentIndex].Value = data.SecondaryFrieght; // "Secondary Frieght";
                        ws.Cells["Q" + contentIndex].Value = data.PlantSecondaryFrieght; // "Plant Secondary Frieght";
                        ws.Cells["R" + contentIndex].Value = data.DepotCost; // "Depot Cost";
                        ws.Cells["S" + contentIndex].Value = data.DetentionCost; // "Detention Cost";
                        ws.Cells["T" + contentIndex].Value = data.HoneycombCost; // "Honeycomb Cost";
                        ws.Cells["U" + contentIndex].Value = data.Margin; // "Margin";
                        ws.Cells["V" + contentIndex].Value = data.CushionMargin; // "Cushion Margin";
                        ws.Cells["W" + contentIndex].Value = data.SchemeCostRecovery; // "Scheme Cost Recovery"; 

                        //if (result.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess)
                        //{
                        //    ws.Cells["X" + contentIndex].Value = data.ExPlantPrice; // "Ex Plant Price";
                        //    ws.Cells["Y" + contentIndex].Value = data.ForDepotPrice; // "For Depot Price";
                        //    ws.Cells["Z" + contentIndex].Value = data.ForPlantPrice; // "For Plant Price";
                        //    ws.Cells["AA" + contentIndex].Value = data.ExDepotPrice; // "Ex Depot Price";
                        //    ws.Cells["AB" + contentIndex].Value = data.ExRakePrice; // "ExRake Price";
                        //    ws.Cells["AC" + contentIndex].Value = data.ForRakePrice; // "For Rake Price";
                        //}
                        //else if (result.SaudaBookingTypeId == (int)SaudaBookingTypes.ReverseAuction)
                        //{
                        //    ws.Cells["X" + contentIndex].Value = data.Discount; // "Discount";
                        //    ws.Cells["Y" + contentIndex].Value = data.Premium; // "Premium";
                        //    ws.Cells["Z" + contentIndex].Value = data.ProcessCost; // "ProcessCost";
                        //    ws.Cells["AA" + contentIndex].Value = data.SumOfIngredientCost; // "Sum Of Ingredient Cost";
                        //    ws.Cells["AB" + contentIndex].Value = data.TpPrice; // "TpPrice";
                        //    ws.Cells["AC" + contentIndex].Value = data.RaMargin; // "Ra Margin";
                        //    ws.Cells["AD" + contentIndex].Value = data.BaseRate; // "Base Rate";
                        //    ws.Cells["AE" + contentIndex].Value = data.XMargin; // "XMargin";
                        //    ws.Cells["AF" + contentIndex].Value = data.FinalRate; // "Final Rate";
                        //    ws.Cells["AG" + contentIndex].Value = data.ExPlantPrice; // "Ex Plant Price";
                        //    ws.Cells["AH" + contentIndex].Value = data.ForDepotPrice; // "For Depot Price";
                        //    ws.Cells["AI" + contentIndex].Value = data.ForPlantPrice; // "For Plant Price";
                        //    ws.Cells["AJ" + contentIndex].Value = data.ExDepotPrice; // "Ex Depot Price";
                        //    ws.Cells["AK" + contentIndex].Value = data.ClearanceRate; // "Clearance Rate";
                        //    ws.Cells["AL" + contentIndex].Value = data.CounterBidOffer; // "Counter BidOffer";
                        //    ws.Cells["AM" + contentIndex].Value = data.CounterBidLimit; // "Counter BidLimit";
                        //    ws.Cells["AN" + contentIndex].Value = data.BpCpJumb; // "BpCp Jumb";
                        //    ws.Cells["AO" + contentIndex].Value = data.ExRakePrice; // "ExRake Price";
                        //    ws.Cells["AP" + contentIndex].Value = data.ForRakePrice; // "For Rake Price";
                        //}
                        contentIndex++;
                    }

                    ws.Cells.AutoFitColumns();

                    //string path = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    //if (!Directory.Exists(path))
                    //{
                    //    Directory.CreateDirectory(path);
                    //}

                    guidFileName = fileGuid + ".xlsx";
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);

                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            ep.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            ep.SaveAs(stream);
                        }
                    }

                    //ep.Save();
                    //fileGuid = Guid.NewGuid().ToString();
                    //TempData[fileGuid] = ep.GetAsByteArray();
                }
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GetPriceGenerateErrorList(PriceErrorMessageDto inputDto)
        {
            string bookingTypeName = "";
            string fileGuid = "";
            string fileName = "";
            string guidFileName = "";

            var pricingInputDto = new PricePublishInputDto { Id = inputDto.Id };
            var pricingErrorData = await _pricingClient.GetPublishedPriceErrorDetails(pricingInputDto);
            inputDto.ErrorMessage = pricingErrorData != null && pricingErrorData.Any() ? pricingErrorData.FirstOrDefault().ErrorMessage : string.Empty;
            if (!string.IsNullOrEmpty(inputDto.ErrorMessage))
            {
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                string templatePath = Path.Combine(serverFoloderPath + "FinalPriceTemplate.xlsx");

                using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                {
                    if (inputDto.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess)
                        fileName = SaudaBookingTypes.TraditionalProcess.ToString().Replace(" ", "").ToUpper() + "-ERROR-" + string.Format("{0:dd-MMM-yyyy}", inputDto.StartDate).ToUpper() + "TIME-" + string.Format("{0:hh:mm tt}", inputDto.StartDate) + "-TO-" + string.Format("{0:hh:mm tt}", inputDto.EndDate) + ".xlsx";
                    //else if (inputDto.SaudaBookingTypeId == (int)SaudaBookingTypes.ReverseAuction)
                    //    fileName = SaudaBookingTypes.ReverseAuction.ToString().Replace(" ", "").ToUpper() + "-ERROR-" + string.Format("{0:dd-MMM-yyyy}", inputDto.StartDate).ToUpper() + "TIME-" + string.Format("{0:hh:mm tt}", inputDto.StartDate) + "-TO-" + string.Format("{0:hh:mm tt}", inputDto.EndDate) + ".xlsx";

                    var ws = ep.Workbook.Worksheets[1];
                    ws.Name = inputDto.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess ? "Traditional Process Final Price" : "Reverse Auction Final Price";

                    #region Header
                    ws.Cells["A1:F1"].Merge = true;
                    ws.Cells["A1:F1"].Value = Settings.CompanyName;
                    ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["A1:F1"].Style.Font.Bold = true;
                    ws.Cells["A1:F1"].Style.Font.Size = 16;

                    ws.Cells["A2"].Value = "Report Name";
                    ws.Cells["A3"].Value = "Process Start Date Time";
                    ws.Cells["A4"].Value = "Process End Date Time";
                    ws.Cells["A5"].Value = "Status";
                    if (inputDto.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess)
                    {
                        bookingTypeName = SaudaBookingTypes.TraditionalProcess.ToString();
                        //ws.Cells["A6"].Value = "Total Record Count";
                    }
                    //else if (inputDto.SaudaBookingTypeId == (int)SaudaBookingTypes.ReverseAuction)
                    //{
                    //    bookingTypeName = SaudaBookingTypes.ReverseAuction.ToString();
                    //    ws.Cells["A6"].Value = "Bidding Window Timing";
                    //    //ws.Cells["A7"].Value = "Total Record Count";
                    //}

                    ws.Cells["B2"].Value = bookingTypeName;
                    ws.Cells["B3"].Value = string.Format("{0:dd-MMM-yyyy hh:mm tt}", inputDto.StartDate);
                    ws.Cells["B4"].Value = string.Format("{0:dd-MMM-yyyy hh:mm tt}", inputDto.EndDate);
                    ws.Cells["B5"].Value = inputDto.Status;
                    if (inputDto.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess)
                    {
                        //ws.Cells["B6"].Value = 0;
                    }
                    //else if (inputDto.SaudaBookingTypeId == (int)SaudaBookingTypes.ReverseAuction)
                    //{
                    //    ws.Cells["B6"].Value = inputDto.BiddingWindowTiming;
                    //    //ws.Cells["B7"].Value = 0;
                    //}

                    for (int i = 2; i <= 7; i++)
                    {
                        ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["A" + i].Style.Font.Bold = true;
                        ws.Cells["A" + i].Style.Font.Size = 12;

                        ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                        ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    }

                    #endregion

                    #region Message                    

                    int headerIndex = 8;
                    int index = 0;

                    ws.Cells["A" + headerIndex].Value = "Sku Name";
                    ws.Cells["B" + headerIndex].Value = "Sku Code";
                    ws.Cells["C" + headerIndex].Value = "Depot Name";
                    ws.Cells["D" + headerIndex].Value = "State Name";
                    ws.Cells["E" + headerIndex].Value = "Freight Route Name";
                    ws.Cells["F" + headerIndex].Value = "Transport Mode Name";
                    ws.Cells["G" + headerIndex].Value = "Load Capacity";
                    ws.Cells["H" + headerIndex].Value = "Missing Data";

                    ExcelRange range = ws.Cells["A8:H8"];
                    range.AutoFitColumns();
                    range.Style.Font.Size = 12;
                    range.Style.Font.Bold = true;

                    var errorList = inputDto.ErrorMessage.Split('|');
                    foreach (var error in errorList)
                    {
                        var errorResult = error.Split('~');
                        if (errorResult != null && errorResult.Any())
                        {
                            headerIndex++;
                            ws.Cells["A" + headerIndex].Value = errorResult.Length > 0 ? errorResult[index].ToString() : "";
                            ws.Cells["B" + headerIndex].Value = errorResult.Length > 1 ? errorResult[index + 1].ToString() : "";
                            ws.Cells["C" + headerIndex].Value = errorResult.Length > 2 ? errorResult[index + 2].ToString() : "";
                            ws.Cells["D" + headerIndex].Value = errorResult.Length > 3 ? errorResult[index + 3].ToString() : "";
                            ws.Cells["E" + headerIndex].Value = errorResult.Length > 4 ? errorResult[index + 4].ToString() : "";
                            ws.Cells["F" + headerIndex].Value = errorResult.Length > 5 ? errorResult[index + 5].ToString() : "";
                            ws.Cells["G" + headerIndex].Value = errorResult.Length > 6 ? errorResult[index + 6].ToString() : "";
                            ws.Cells["H" + headerIndex].Value = errorResult.Length > 7 ? errorResult[index + 7].ToString() : "";
                        }
                    }

                    #endregion

                    ws.Cells.AutoFitColumns();

                    guidFileName = fileGuid + ".xlsx";
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);

                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            ep.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            ep.SaveAs(stream);
                        }
                    }

                    //ep.Save();
                    //fileGuid = Guid.NewGuid().ToString();
                    //TempData[fileGuid] = ep.GetAsByteArray();
                }
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        #endregion      

        public ActionResult EditingCustomRead([DataSourceRequest] DataSourceRequest request)
        {
            return Json("");
        }

        public KendoGridResult GridResultInputDto(DataSourceRequest request, bool isToReturnInactiveData)
        {
            request.Filters = Utility.ToFilterDescriptor(request.Filters);
            return new KendoGridResult() { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData, VerticalId = VerticalId, DataSourceRequest = request };
        }


        #region New FinalPrice - State Based



        [AuthorizeClaims(Claims.ManagePricing)]
        public ActionResult TodayPricingDetails()
        {
            var inputdto = new PricePublistInputDataDto
            {
                RoleId = RoleId
            };
            return View(inputdto);
        }
        public async Task<ActionResult> ExportPricingDetails(PricePublistInputDataDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto.RoleId = RoleId;
                inputDto.LoginUserId = UserId;
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                //var resultList = new DataTable();
                var resultList = await _pricingClient.GetGeneratedPriceList1(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "PricingDetailsList_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";
                if (resultList.IsAny())
                {
                    using (var ep = new ExcelPackage())
                    {
                        var newList = new List<FinalPriceGenerateExportDto>();
                        if (resultList.Count > 100000)
                        {
                            for (var i = 0; resultList.IsAny(); i++)
                            {
                                var ws = ep.Workbook.Worksheets.Add("TodayPricingDetails" + i);

                                //Header
                                ws.Cells["A1:BZ1"].Style.Font.Size = 13;
                                ws.Cells["A1:BZ1"].Style.Font.Name = "Calibri";
                                ws.Cells["A1:BZ1"].Style.Font.Bold = true;

                                newList = resultList.Take(100000).ToList();
                                resultList = resultList.Skip(100000).ToList();

                                ws.Cells.LoadFromCollection(newList, true);
                                ws.Cells.AutoFitColumns();
                            }



                        }
                        else
                        {
                            var ws = ep.Workbook.Worksheets.Add("TodayPricingDetails");

                            //Header
                            ws.Cells["A1:BZ1"].Style.Font.Size = 13;
                            ws.Cells["A1:BZ1"].Style.Font.Name = "Calibri";
                            ws.Cells["A1:BZ1"].Style.Font.Bold = true;


                            ws.Cells.LoadFromCollection(resultList, true);
                            ws.Cells.AutoFitColumns();
                        }



                        //var ws1 = ep.Workbook.Worksheets.Add("TodayPricingDetails2");
                        //ws1.Cells["A1:BZ1"].Style.Font.Size = 13;
                        //ws1.Cells["A1:BZ1"].Style.Font.Name = "Calibri";
                        //ws1.Cells["A1:BZ1"].Style.Font.Bold = true;

                        //ws1.Cells.LoadFromCollection(resultList, true);
                        //ws1.Cells.AutoFitColumns();
                        guidFileName = SaveExcelFileToPath(ep);
                    }
                }


                // Create the package and make sure you wrap it in a using statement
                //using (var package = new ExcelPackage())
                //{
                //    // add a new worksheet to the empty workbook
                //    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                //    var rowIndex = 5;
                //    var colIndex = 1;
                //    var childColIndex = 0;



                //    worksheet.Cells["A1:M1"].Merge = true;
                //    worksheet.Cells["A1:M1"].Value = "Adanai Agrotech Ltd.";
                //    worksheet.Cells["A1:M1"].Value = "Adani Wilmar Ltd.";
                //    worksheet.Cells["A1:M1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                //    worksheet.Cells["A1:M1"].Style.Font.Bold = true;
                //    worksheet.Cells["A1:M1"].Style.Font.Size = 16;

                //    worksheet.Cells["A2"].Value = "Report Name";
                //    worksheet.Cells["B2"].Value = "Today Pricing Details";

                //    for (int i = 2; i <= 4; i++)
                //    {
                //        worksheet.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                //        worksheet.Cells["A" + i].Style.Font.Bold = true;
                //        worksheet.Cells["A" + i].Style.Font.Size = 12;

                //        worksheet.Cells["B" + i + ":" + "F" + i].Merge = true;
                //        worksheet.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                //    }

                //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CreatedDate"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SAP_PricingCode"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SkuName"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SkuCode"));
                //    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OilTypeCode"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OilTypeName"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OilPackingType"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PlantCode"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PlantName"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Price"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SalesOrganization"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DistributionChannel"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Division"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ValidFrom"));
                //    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ValidTo"));

                //    if (resultList != null && resultList.Any())
                //    {
                //        foreach (var item in resultList)
                //        {
                //            rowIndex++;
                //            colIndex = 1;
                //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.CreatedDate.ToString("dd-MMM-yyyy hh:mm tt"));
                //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.SAPPricingCode.ToString());
                //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.SkuName !=null ? item.SkuName.ToString() : String.Empty);
                //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.SkuCode != null ? item.SkuCode.ToString() : String.Empty);
                //           // GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.OilTypeCode != null ? item.OilTypeCode.ToString() : String.Empty);
                //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.OilTypeName != null ? item.OilTypeName.ToString() : String.Empty);
                //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.OilPackingType != null ? item.OilPackingType.ToString() : String.Empty);
                //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.PlantCode != null ? item.PlantCode.ToString():String.Empty);
                //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.PlantName!=null ? item.PlantName.ToString() : String.Empty);
                //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Price.ToString());
                //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.SalesOrganizationName != null ? item.SalesOrganizationName.ToString() : String.Empty);
                //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.DistributionChannelName != null ? item.DistributionChannelName.ToString() : String.Empty);
                //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.DivisionName != null ? item.DivisionName.ToString() : String.Empty);
                //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ValidFrom.ToString());
                //            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ValidTo.ToString());
                //        }
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
                //    //this.Response.Headers.Clear();
                //    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                //    if (System.IO.File.Exists(savePath))
                //    {
                //        System.IO.File.Delete(savePath);
                //        using (Stream stream = System.IO.File.Create(savePath))
                //        {
                //            package.SaveAs(stream);
                //        }
                //    }
                //    else
                //    {
                //        using (Stream stream = System.IO.File.Create(savePath))
                //        {
                //            package.SaveAs(stream);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetTodayPricingList([DataSourceRequest] DataSourceRequest request, PricePublistInputDataDto inputDto)
        {
            //var inputDto = new PricePublishInputDto() { LoginUserId = UserId, RoleId = RoleId, SearchDate = searchDate, SaudaBookingTypeId = bookingTypeId, VerticalId = VerticalId };
            inputDto.RoleId = RoleId;
            inputDto.LoginUserId = UserId;
            //inputDto.DataSourceRequest = request;
            var result = await _pricingClient.GetGeneratedPriceList1(inputDto);
            var resultList = result.ToDataSourceResult(request);
            //if (result != null && result.Any())
            //{
            //    result.ForEach(f =>
            //    {
            //        f.PricingDate = ConvertUTCToIndiaTime(f.PricingDate);
            //    });
            //}
            //var resultList = result.ToDataSourceResult(request);
            return Json(result.ToDataSourceResult(request));
            //var resultList = result.ToDataSourceResult(request);
            //var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;
        }

        public async Task<ActionResult> GetGeneratedPriceAsync([DataSourceRequest] DataSourceRequest request, DateTime searchDate, long bookingTypeId)
        {
            var inputDto = new PricePublishInputDto() { LoginUserId = UserId, RoleId = RoleId, SearchDate = searchDate, SaudaBookingTypeId = bookingTypeId, VerticalId = VerticalId };
            var result = await _pricingClient.GetGeneratedPriceAsync(inputDto);
            if (result != null && result.Any())
            {
                result.ForEach(f =>
                {
                    f.PricingDate = ConvertUTCToIndiaTime(f.PricingDate);
                });
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public async Task<ActionResult> GetGeneratedPriceDetailsAsync([DataSourceRequest] DataSourceRequest request, long pricingId, DateTime searchDate)
        {
            var inputDto = new PricePublishInputDto() { LoginUserId = UserId, Id = pricingId, SearchDate = searchDate };
            var result = await _pricingClient.GetGeneratedPriceDetailsAsync(inputDto);

            if (result != null && result.Any())
            {
                result.ForEach(f =>
                {
                    f.StartDate = ConvertUTCToIndiaTime(f.StartDate);
                    f.EndDate = ConvertUTCToIndiaTime(f.EndDate);
                    f.PublishDate = ConvertUTCToIndiaTime(f.PublishDate);
                });
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Traditional Process and Revers Auction final price publish
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<JsonResult> StateBasePublishFinalPrice(FinalPricePublishDto inputDto)
        {
            inputDto.BiddingDate = DateTime.Now;
            inputDto.LoginUserId = UserId;
            var data = await _pricingClient.StateBasePublishFinalPrice(inputDto);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPublishedFinalPriceListNew(long publishId, long FinalPriceRecordCount, DateTime SearchDate)
        {
            JsonResult jsonResult = new JsonResult();
            int skipCount = 0;
            int takeCount = ConfigHelper.RecordCountForExcelSheet;
            decimal divider = Convert.ToDecimal(string.Format("{0:0.0}", takeCount));
            decimal count = 0;
            try
            {
                var totalDataCount = GetTotalPricingCountForStateBased(SearchDate, publishId);

                if (totalDataCount < takeCount)
                    count = 1;
                else
                    count = Math.Round(totalDataCount / divider) + 2;

                if (count > 0)
                {
                    string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFoloderPath + "FinalPriceTemplateForAllRecords.xlsx");
                    string fileName = "";
                    string savePath = "";
                    string guidFileName = "";
                    using (ExcelPackage ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        ep.Workbook.Worksheets.Delete("Sheet1");
                        for (int i = 0; i < count; i++)
                        {
                            var ws = ep.Workbook.Worksheets.Add("Sheet" + (i + 1).ToString());
                        }
                        fileName = "TRADITIONAL-PROCESS-STATE-BASED-SUCCESS-" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now).ToUpper() + ".xlsx";
                        guidFileName = Guid.NewGuid().ToString() + ".xlsx";
                        savePath = Path.Combine(serverFoloderPath, guidFileName);
                        if (System.IO.File.Exists(savePath))
                        {
                            System.IO.File.Delete(savePath);
                            using (Stream stream = System.IO.File.Create(savePath))
                            {
                                ep.SaveAs(stream);
                            }
                        }
                        else
                        {
                            using (Stream stream = System.IO.File.Create(savePath))
                            {
                                ep.SaveAs(stream);
                            }
                        }
                    }
                    using (ExcelPackage ep = new ExcelPackage(new FileInfo(savePath)))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            using (SqlConnection conn = new SqlConnection(connectionString))
                            {
                                conn.Open();
                                string SP_Name = "SP_TPStateBasedDownload";
                                SqlCommand cmd = new SqlCommand(SP_Name, conn);
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@Skip", skipCount);
                                cmd.Parameters.AddWithValue("@Take", takeCount);
                                cmd.Parameters.AddWithValue("@publishId", publishId);
                                cmd.Parameters.AddWithValue("@SearchDate", SearchDate);
                                cmd.CommandTimeout = 0;
                                SqlDataReader sqlDataReader = cmd.ExecuteReader();

                                var ws = ep.Workbook.Worksheets["Sheet" + (i + 1).ToString()];
                                ws.Cells["A1:BZ1"].Style.Font.Bold = true;
                                ws.Cells["A1:BZ1"].Style.Font.Size = 12;
                                ws.Cells["A1"].LoadFromDataReader(sqlDataReader, true);
                                //ws.Cells.AutoFitColumns();
                                ep.Save();
                            }
                            skipCount += takeCount;
                        }
                        jsonResult = Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet); ;
                    }
                }
                else
                    jsonResult = Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
            return jsonResult;
        }

        [HttpPost]
        public async Task<ActionResult> GetStateBasePriceGenerateErrorList(long priceId, long saudaBookingTypeId)
        {
            string bookingTypeName = "";
            string fileGuid = "";
            string fileName = "";
            string guidFileName = "";
            PriceErrorMessageDto inputDto = new PriceErrorMessageDto() { Id = priceId };
            var pricingInputDto = new PricePublishInputDto { Id = inputDto.Id };

            var pricingErrorData = await _pricingClient.GetStateBasePublishedPriceErrorDetails(pricingInputDto);
            var priceData = pricingErrorData != null && pricingErrorData.Any() ? pricingErrorData.FirstOrDefault() : new PricePublishesDto();

            if (!string.IsNullOrEmpty(priceData.ErrorMessage))
            {
                inputDto.SaudaBookingTypeId = saudaBookingTypeId;
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                string templatePath = Path.Combine(serverFoloderPath + "FinalPriceTemplate.xlsx");

                using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                {
                    fileName = Utility.GetEnumFromString<SaudaBookingTypes>(inputDto.SaudaBookingTypeId).ToUpper() + "-ERROR-" + string.Format("{0:dd-MMM-yyyy}", inputDto.StartDate).ToUpper() + "TIME-" + string.Format("{0:hh:mm tt}", inputDto.StartDate) + "-TO-" + string.Format("{0:hh:mm tt}", inputDto.EndDate) + ".xlsx";
                    var ws = ep.Workbook.Worksheets[1];
                    ws.Name = Utility.GetEnumFromString<SaudaBookingTypes>(inputDto.SaudaBookingTypeId);

                    #region Header
                    ws.Cells["A1:F1"].Merge = true;
                    ws.Cells["A1:F1"].Value = Settings.CompanyName;
                    ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["A1:F1"].Style.Font.Bold = true;
                    ws.Cells["A1:F1"].Style.Font.Size = 16;

                    ws.Cells["A2"].Value = "Report Name";
                    ws.Cells["A3"].Value = "Process Start Date Time";
                    ws.Cells["A4"].Value = "Process End Date Time";
                    ws.Cells["A5"].Value = "Status";
                    if (inputDto.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess)
                    {
                        bookingTypeName = SaudaBookingTypes.TraditionalProcess.ToString();
                        ws.Cells["A6"].Value = "Total Record Count";
                    }
                    //else if (inputDto.SaudaBookingTypeId == (int)SaudaBookingTypes.ReverseAuction)
                    //{
                    //    bookingTypeName = SaudaBookingTypes.ReverseAuction.ToString();
                    //    //ws.Cells["A6"].Value = "Bidding Window Timing";
                    //    //ws.Cells["A7"].Value = "Total Record Count";
                    //}

                    ws.Cells["B2"].Value = bookingTypeName;
                    ws.Cells["B3"].Value = string.Format("{0:dd-MMM-yyyy hh:mm tt}", priceData.StartDate);
                    ws.Cells["B4"].Value = string.Format("{0:dd-MMM-yyyy hh:mm tt}", priceData.EndDate);
                    ws.Cells["B5"].Value = Utility.GetEnumFromString<PricePublishStatus>(priceData.StatusId);
                    ws.Cells["B6"].Value = Utility.CalculateErrorMessageCount(priceData.ErrorMessage);
                    if (inputDto.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess)
                    {
                        //ws.Cells["B6"].Value = 0;
                    }
                    //else if (inputDto.SaudaBookingTypeId == (int)SaudaBookingTypes.ReverseAuction)
                    //{
                    //    ws.Cells["B6"].Value = priceData.BiddingWindowTiming;
                    //    //ws.Cells["B7"].Value = 0;
                    //}

                    for (int i = 2; i <= 7; i++)
                    {
                        ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells["A" + i].Style.Font.Bold = true;
                        ws.Cells["A" + i].Style.Font.Size = 12;

                        ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                        ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    }

                    #endregion

                    #region Message                    

                    int headerIndex = 8;
                    int index = 0;

                    ws.Cells["A" + headerIndex].Value = "Sku Name";
                    ws.Cells["B" + headerIndex].Value = "Sku Code";
                    ws.Cells["C" + headerIndex].Value = "Depot Name";
                    ws.Cells["D" + headerIndex].Value = "State Name";
                    ws.Cells["E" + headerIndex].Value = "Freight Route Name";
                    ws.Cells["F" + headerIndex].Value = "Transport Mode Name";
                    ws.Cells["G" + headerIndex].Value = "Load Capacity";
                    ws.Cells["H" + headerIndex].Value = "Missing Data";

                    ExcelRange range = ws.Cells["A8:H8"];
                    range.AutoFitColumns();
                    range.Style.Font.Size = 12;
                    range.Style.Font.Bold = true;

                    if (priceData.ErrorMessage.Contains("|"))
                    {
                        var errorList = priceData.ErrorMessage.Split('|');
                        foreach (var error in errorList)
                        {
                            var errorResult = error.Split('~');
                            if (errorResult != null && errorResult.Any())
                            {
                                headerIndex++;
                                ws.Cells["A" + headerIndex].Value = errorResult.Length > 0 ? errorResult[index].ToString() : "";
                                ws.Cells["B" + headerIndex].Value = errorResult.Length > 1 ? errorResult[index + 1].ToString() : "";
                                ws.Cells["C" + headerIndex].Value = errorResult.Length > 2 ? errorResult[index + 2].ToString() : "";
                                ws.Cells["D" + headerIndex].Value = errorResult.Length > 3 ? errorResult[index + 3].ToString() : "";
                                ws.Cells["E" + headerIndex].Value = errorResult.Length > 4 ? errorResult[index + 4].ToString() : "";
                                ws.Cells["F" + headerIndex].Value = errorResult.Length > 5 ? errorResult[index + 5].ToString() : "";
                                ws.Cells["G" + headerIndex].Value = errorResult.Length > 6 ? errorResult[index + 6].ToString() : "";
                                ws.Cells["H" + headerIndex].Value = errorResult.Length > 7 ? errorResult[index + 7].ToString() : "";
                            }
                        }
                    }
                    else
                    {
                        headerIndex++;
                        ws.Cells["H" + headerIndex].Value = priceData.ErrorMessage;
                    }

                    #endregion

                    ws.Cells.AutoFitColumns();

                    guidFileName = fileGuid + ".xlsx";
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);

                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            ep.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            ep.SaveAs(stream);
                        }
                    }
                }
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Zone - FinalPriceDownload

        public JsonResult ZoneBasedFinalPriceDownload(long priceId, DateTime SearchDate)
        {
            JsonResult jsonResult = new JsonResult();
            int skipCount = 0;
            int takeCount = ConfigHelper.RecordCountForExcelSheet;
            decimal divider = Convert.ToDecimal(string.Format("{0:0.0}", takeCount));
            decimal count = 0;

            try
            {
                var totalDataCount = GetTotalPricingCountForZoneBased(SearchDate, priceId);

                if (totalDataCount < takeCount)
                    count = 1;
                else
                    count = Math.Round(totalDataCount / divider) + 2;

                if (count > 0)
                {
                    string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                    string templatePath = Path.Combine(serverFoloderPath + "FinalPriceTemplateForAllRecords.xlsx");
                    string fileName = "";
                    string savePath = "";
                    string guidFileName = "";
                    using (ExcelPackage ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        ep.Workbook.Worksheets.Delete("Sheet1");
                        for (int i = 0; i < count; i++)
                        {
                            var ws = ep.Workbook.Worksheets.Add("Sheet" + (i + 1).ToString());
                        }
                        fileName = "TRADITIONAL-PROCESS-SUCCESS-" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now).ToUpper() + ".xlsx";
                        guidFileName = Guid.NewGuid().ToString() + ".xlsx";
                        savePath = Path.Combine(serverFoloderPath, guidFileName);
                        if (System.IO.File.Exists(savePath))
                        {
                            System.IO.File.Delete(savePath);
                            using (Stream stream = System.IO.File.Create(savePath))
                            {
                                ep.SaveAs(stream);
                            }
                        }
                        else
                        {
                            using (Stream stream = System.IO.File.Create(savePath))
                            {
                                ep.SaveAs(stream);
                            }
                        }
                    }
                    using (ExcelPackage ep = new ExcelPackage(new FileInfo(savePath)))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            using (SqlConnection conn = new SqlConnection(connectionString))
                            {
                                conn.Open();
                                string SP_Name = "GetFinalPriceDataExport";
                                SqlCommand cmd = new SqlCommand(SP_Name, conn);
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                //cmd.Parameters.AddWithValue("@Skip", skipCount);
                                //cmd.Parameters.AddWithValue("@Take", takeCount);
                                cmd.Parameters.AddWithValue("@PriceId", priceId);
                                //cmd.Parameters.AddWithValue("@SearchDate", SearchDate);
                                cmd.CommandTimeout = 0;
                                SqlDataReader sqlDataReader = cmd.ExecuteReader();

                                var ws = ep.Workbook.Worksheets["Sheet" + (i + 1).ToString()];
                                ws.Cells["A1:BZ1"].Style.Font.Bold = true;
                                ws.Cells["A1:BZ1"].Style.Font.Size = 12;
                                ws.Cells["A1"].LoadFromDataReader(sqlDataReader, true);
                                //ws.Cells.AutoFitColumns();
                                ep.Save();
                            }
                            skipCount += takeCount;
                        }
                        jsonResult = Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet); ;
                    }
                }
                else
                    jsonResult = Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
            return jsonResult;
        }

        public long GetTotalPricingCountForZoneBased(DateTime SearchDate, long PriceId)
        {
            var result = 0;
            try
            {
                var todayDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                if (SearchDate.Date == todayDate.Date)
                {
                    using (SqlConnection query = new SqlConnection(connectionString))
                    {
                        result = query.QueryFirstOrDefault<int>(@"select Count(Id) from TodayPricings as p
                        where p.PublishId in
                        (Select pgd.Id From PriceGenerates pg Join PriceGenerateDetails pgd on pg.Id = pgd.PriceGenerateId Where pg.Id = @PriceId)
                        ", new
                        {
                            PriceId = PriceId
                        });
                    }
                }
                else if (SearchDate.Date < todayDate.Date)
                {
                    using (SqlConnection query = new SqlConnection(connectionString))
                    {
                        result = query.QueryFirstOrDefault<int>(@"select Count(Id) from PricingBackups as p
                        where p.PublishId in
                        (Select pgd.Id From PriceGenerates pg Join PriceGenerateDetails pgd on pg.Id = pgd.PriceGenerateId Where pg.Id = @PriceId)
                        ", new
                        {
                            PriceId = PriceId
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
            return result;

        }

        public long GetTotalPricingCountForStateBased(DateTime SearchDate, long PublishId)
        {
            var result = 0;
            try
            {
                var todayDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                if (SearchDate.Date == todayDate.Date)
                {
                    using (SqlConnection query = new SqlConnection(connectionString))
                    {
                        result = query.QueryFirstOrDefault<int>("Select Count(Id) as CountOfRecords From TodayPricings Where PublishId = @PublishId ", new
                        {
                            PublishId = PublishId
                        });
                    }
                }
                else if (SearchDate.Date < todayDate.Date)
                {
                    using (SqlConnection query = new SqlConnection(connectionString))
                    {
                        result = query.QueryFirstOrDefault<int>("Select Count(Id) as CountOfRecords From PricingBackups Where PublishId = @PublishId ", new
                        {
                            PublishId = PublishId
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
            return result;

        }

        public JsonResult ZoneBasedFinalPriceDownloadForAllRecords()
        {
            JsonResult jsonResult = new JsonResult();
            long totalCountValue = 0;
            long count = 0;
            long skip = 0;
            long take = 0;
            long Max = 100000;
            long size = 0;
            var sheetnumber = 2;
            var getTotalCountBasedOnCurrentDateFromPricings = _importClient.GetTotalCountBasedOnCurrentDateFromPricings();
            foreach (var x in getTotalCountBasedOnCurrentDateFromPricings)
            {
                totalCountValue = x.CountOfRecords;
            }
            skip = count;
            take = totalCountValue > Max ? Max : totalCountValue;
            if (totalCountValue > 0)
            {
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                string templatePath = Path.Combine(serverFoloderPath + "FinalPriceTemplateForAllRecords.xlsx");
                using (ExcelPackage ep = new ExcelPackage(new FileInfo(templatePath)))
                {
                    while (count < totalCountValue)
                    {


                        var finalPriceList = _importClient.TpFinalPriceExportForAllRecordsToList(skip, take);

                        if (count == 0)
                        {
                            var ws = ep.Workbook.Worksheets["Sheet1"];



                            ExcelRange range = ws.Cells["A1:BZ1"];
                            range.AutoFitColumns();
                            range.Style.Font.Size = 12;
                            range.Style.Font.Bold = true;

                            ws.Cells["A1"].LoadFromCollection(finalPriceList, true);
                            ws.Cells.AutoFitColumns();

                        }
                        else
                        {
                            var ws = ep.Workbook.Worksheets.Add("Sheet" + sheetnumber.ToString());
                            ws.Name = "Sheet" + sheetnumber.ToString();


                            ExcelRange range = ws.Cells["A1:BZ1"];
                            range.AutoFitColumns();
                            range.Style.Font.Size = 12;
                            range.Style.Font.Bold = true;

                            ws.Cells["A1"].LoadFromCollection(finalPriceList, true);
                            ws.Cells.AutoFitColumns();
                            sheetnumber++;
                        }
                        count = count + take;
                        size = totalCountValue - count < Max ? totalCountValue - count : Max;
                        skip = skip + take;

                        take = size;

                    }

                    string fileName = "TRADITIONAL-PROCESS-SUCCESS-" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now).ToUpper() + ".xlsx";
                    jsonResult = SaveExcelFileToPath(ep, fileName);
                    ep.Dispose();
                }
            }

            return jsonResult;
        }



        #endregion

    }
}