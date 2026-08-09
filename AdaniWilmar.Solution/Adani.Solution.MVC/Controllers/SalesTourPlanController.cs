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
using Adani.Solution.MVC.Common;
using Adani.Solution.DTO.Enums;
using Adani.Solution.DTO;
using System.Linq;
using System;
using System.Web.Hosting;
using System.IO;
using OfficeOpenXml;
using System.Drawing;

namespace Adani.Solution.MVC.Controllers
{
    [TokenAuthorize]
    [CustomRedirect]
    [NoCache]
    public class SalesTourPlanController : BaseController
    {
        private readonly SalesTourPlanClient _salesTourPlanClient;
        private readonly MasterClient _masterClient;

        public SalesTourPlanController()
        {
            _salesTourPlanClient = new SalesTourPlanClient { ControllerDelegate = this };
            _masterClient = new MasterClient { ControllerDelegate = this };
        }

        #region Permanent Journey Plan
        public async Task<ActionResult> PermanentJourneyPlan()
        {
            var result = new PermanentJouneyPlanViewModel();
            if (Session["PermanentJourneyPlanId"] != null && UtilityHelper.LongTryToParse(Session["PermanentJourneyPlanId"].ToString()) > 0)
            {
                result = await _salesTourPlanClient.GetPermanentJourneyPlanDetailsAsync(UtilityHelper.LongTryToParse(Session["PermanentJourneyPlanId"].ToString()));
                if (UserId == result.CreatedBy)
                {
                    //if (result.StatusId != (int)DTO.Enums.PermanentJourneyPlanStatus.Approved)
                    //{
                    result.IsEditableForCreatedUser = Settings.PJPFlag;
                    //}
                }
                else
                {
                    if (result.StatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Pending)
                    {
                        result.IsApprover = Settings.PJPFlag;
                    }
                }
                if (result.IsEditableForCreatedUser == 0 && result.IsApprover == 0)
                {
                    if (RoleId == (int)DTO.Enums.Role.Admin)
                    {
                        result.IsEditableForAdmin = Settings.PJPFlag;
                    }

                }
            }

             result.EncryptedId = UtilityHelper.ConvertToMd5(result.PJPId.ToString(), SecurityConstants.EncryptionKey);
            //else
            //{
            var Employeeresult = new EmployeeDto();
            var FinancialYear = new FinancialYearDto();
            var EncryptedId = UtilityHelper.ConvertToMd5(UserId.ToString(), SecurityConstants.EncryptionKey);
            Employeeresult = await _masterClient.GetUserDetailsById(EncryptedId);
            FinancialYear = await _salesTourPlanClient.GetCurrenntFinancialYearAsync();
            result.StateId = Employeeresult.StateId;
            result.TerritoryId = Employeeresult.TerritoryId;
            result.DistrictId = Employeeresult.DistrictId;
            result.CityId = Employeeresult.CityId;
            result.FinancialYearId = FinancialYear.Id;
            //}
            return View(result);
        }

        public async Task<JsonResult> LoginBDODetails(int Id)
        {
            var result = new EmployeeDto();
            var EncryptedId = UtilityHelper.ConvertToMd5(UserId.ToString(), SecurityConstants.EncryptionKey);
            result = await _masterClient.GetUserDetailsById(EncryptedId);
            return Json(result);
        }

        /// <summary>
        /// Method to get Active Financial Year list
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetActiveFinancialYearListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var salesPersonList = await _salesTourPlanClient.GetActiveFinancialYearListAsync();
            return Json(salesPersonList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to PJP post action
        /// </summary>
        /// <returns></returns>

        [AuthorizeClaims(Claims.ManageSalesTourPlan, Claims.ViewSalesTourPlan)]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<JsonResult> PermanentJourneyPlan(PermanentJouneyPlanViewModel pJPViewModel)
        {
            pJPViewModel.LoginUserId = UserId;

            if (!String.IsNullOrEmpty(pJPViewModel.EncryptedId))
            {
                pJPViewModel.EncryptedId = pJPViewModel.EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(pJPViewModel.EncryptedId, SecurityConstants.EncryptionKey);

                pJPViewModel.PJPId = UtilityHelper.IntTryToParse(decryptedId);
            }

            pJPViewModel = Helper.SanitizeModel(pJPViewModel);
            pJPViewModel = await _salesTourPlanClient.PermanentJourneyPlanAsync(pJPViewModel);
            Session["PJPViewModel"] = pJPViewModel;
            return Json(pJPViewModel, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Method to get all retailer list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetDealerListForDropdownAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.GetAllDealerListForDropdownAsync();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get all retailer list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetDealerListByPJPAsync([DataSourceRequest] DataSourceRequest request, long PJPId, long CityId)
        {
            var responce = new List<DealerDto>();
            if ( PJPId > 0)
            {
                responce = await _salesTourPlanClient.DealersByUserPermanentJourneyPlan(PJPId, CityId);
            }
            return Json(responce, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetNoVisitListByPJPAsync([DataSourceRequest] DataSourceRequest request, long PJPId)
        {
            var responce = new List<DealerDto>();
            if (PJPId > 0)
            {
                responce = await _salesTourPlanClient.GetNoVisitListByPJP(PJPId);
            }
            return Json(responce, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get district list
        /// </summary>
        /// <returns></returns>
        [NoCache]
        public async Task<JsonResult> GetPJPDistrictListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.GetDistrictListAsync(31);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Method to get district list
        /// </summary>
        /// <returns></returns>
        [NoCache]
        public async Task<JsonResult> GetCityListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.GetCityListAsync();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Method to get Date Week Details list
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetDateWeekDetailsListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var salesPersonList = await _salesTourPlanClient.GetDateWeekDetailsListAsync();
            return Json(salesPersonList, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Permanent Journey Plans
        [AuthorizeClaims(Claims.ManageSalesTourPlan, Claims.ViewSalesTourPlan)]
        public ActionResult PermanentJourneyPlans()
        {
            var result = new PermanentJouneyPlanViewModel();
            if (Session["PJPViewModel"] != null)
            {
                var pjpModel = Session["PJPViewModel"] as PermanentJouneyPlanViewModel;
                result.PostMessage = pjpModel.PostMessage;
                result.PostStatus = true;
            }
            Session["PJPViewModel"] = null;
            Session["PermanentJourneyPlanId"] = null;
            return View(result);
        }
        /// <summary>
        /// Method to get PermanentJournyPlan list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetPermanentJourneyPlanListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.GetPermanentJourneyPlanList(UserId);
            var gridList = result.ToDataSourceResult(request);
            gridList.Total = result.Count;
            return Json(gridList);
        }

        /// <summary>
        /// Method to Permanent Journey Plan add and edit redirect action
        /// </summary>
        /// <param name="pjpId"></param>
        /// <returns></returns>
        public ActionResult PJPERedirect(string EncryptedId = "")
        {
            var id = "";
            if (!String.IsNullOrEmpty(EncryptedId))
            {
                EncryptedId = EncryptedId.Replace(' ', '+');
                id = UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey);

            }

            Session["PermanentJourneyPlanId"] = id;
            return RedirectToAction("PermanentJourneyPlan", "SalesTourPlan");
        }

        public ActionResult PendingPermanentJourneyPlans()
        {
            var result = new PermanentJouneyPlanViewModel();
            return View(result);
        }
        /// <summary>
        /// Method to get PermanentJournyPlan list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetPendingPermanentJourneyPlanListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.GetPendingPermanentJourneyPlanList(UserId);
            var gridList = result.ToDataSourceResult(request);
            gridList.Total = result.Count;
            return Json(gridList);
        }
        /// <summary>
        /// Method to get PermanentJournyPlan list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetApprovedOrRejectedPJPList([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.GetApprovedOrRejectedPJPList(UserId);
            var gridList = result.ToDataSourceResult(request);
            gridList.Total = result.Count;
            return Json(gridList);
        }
        #endregion

        #region Monthly Tour Plan
        [AuthorizeClaims(Claims.ManageSalesTourPlan, Claims.ViewSalesTourPlan)]
        public async Task<ActionResult> MonthlyTourPlan()
        {
            var result = new MonthlyTourPlanViewModel();
            if (Session["MonthlyTourPlanId"] != null && UtilityHelper.LongTryToParse(Session["MonthlyTourPlanId"].ToString()) > 0)
            {
                result = await _salesTourPlanClient.GetMonthlyTourPlanDetailsAsync(UtilityHelper.LongTryToParse(Session["MonthlyTourPlanId"].ToString()));
                if (UserId == result.LoginUserId)
                {
                    if (result.StatusId != (int)DTO.Enums.MonthlyTourPlanStatus.Approved)
                    {
                        result.IsEditableForCreatedUser = Settings.PJPFlag;
                    }
                }
                else
                {
                    if (result.StatusId == (int)DTO.Enums.MonthlyTourPlanStatus.Pending)
                    {
                        result.IsApprover = Settings.PJPFlag;
                    }
                }
                if (result.IsEditableForCreatedUser == 0 && result.IsApprover == 0)
                {
                    if (RoleId == (int)DTO.Enums.Role.Admin)
                    {
                        result.IsEditableForAdmin = Settings.PJPFlag;
                    }
                }
            }
            result.EncryptedId = UtilityHelper.ConvertToMd5(result.MTPId.ToString(), SecurityConstants.EncryptionKey);
            //result.HeadquartersId = HeadquartersId;
            return View(result);
        }

        /// <summary>
        /// Method to PJP post action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<JsonResult> MonthlyTourPlan(MonthlyTourPlanViewModel mtpViewModel)
        {
            mtpViewModel.LoginUserId = UserId;

            if (!String.IsNullOrEmpty(mtpViewModel.EncryptedId))
            {
                mtpViewModel.EncryptedId = mtpViewModel.EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(mtpViewModel.EncryptedId, SecurityConstants.EncryptionKey);

                mtpViewModel.MTPId = UtilityHelper.IntTryToParse(decryptedId);
            }

            mtpViewModel = Helper.SanitizeModel(mtpViewModel);
            mtpViewModel = await _salesTourPlanClient.MonthlyTourPlanAsync(mtpViewModel);
            Session["MTPViewModel"] = mtpViewModel;
            return Json(mtpViewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get PermanentJournyPlan list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetApprovedOrRejectedMTPList([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.GetApprovedOrRejectedMTPList(UserId);
            var gridList = result.ToDataSourceResult(request);
            gridList.Total = result.Count;
            return Json(gridList);
        }
        #endregion
        #region Monthly Tour Plans
        [AuthorizeClaims(Claims.ManageSalesTourPlan, Claims.ViewSalesTourPlan)]
        public ActionResult MonthlyTourPlans()
        {
            var result = new MonthlyTourPlanViewModel();
            if (Session["MTPViewModel"] != null)
            {
                var pjpModel = Session["MTPViewModel"] as MonthlyTourPlanViewModel;
                result.PostMessage = pjpModel.PostMessage;
                result.PostStatus = true;
            }
            Session["MTPViewModel"] = null;
            Session["MonthlyTourPlanId"] = null;
            return View(result);
        }
        /// <summary>
        /// Method to get Monthly Tour Plan list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetMonthlyTourPlanListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.GetMonthlyTourPlanList(UserId);
            var gridList = result.ToDataSourceResult(request);
            gridList.Total = result.Count;
            return Json(gridList);
        }

        /// <summary>
        /// Method to Monthly Tour Plan add and edit redirect action
        /// </summary>
        /// <param name="pjpId"></param>
        /// <returns></returns>
        public ActionResult MTPERedirect(string EncryptedId = "")
        {
            var id = "";

            if (!String.IsNullOrEmpty(EncryptedId))
            {
                EncryptedId = EncryptedId.Replace(' ', '+');
                id = UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey);

            }


            Session["MonthlyTourPlanId"] = id;
            return RedirectToAction("MonthlyTourPlan", "SalesTourPlan");
        }

        public ActionResult PendingMonthlyTourPlans()
        {
            var result = new MonthlyTourPlanViewModel();
            return View(result);
        }
        /// <summary>
        /// Method to get Pending Monthly Tour Plan list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetPendingMonthlyTourPlanListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.GetPendingMonthlyTourPlanList(UserId);
            var gridList = result.ToDataSourceResult(request);
            gridList.Total = result.Count;
            return Json(gridList);
        }
        #endregion

        #region Financial Year
        /// <summary>
        /// Method to financial year get action
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> FinancialYear()
        {
            var result = new FinancialYearViewModel();
            if (Session["financialYearId"] != null && UtilityHelper.LongTryToParse(Session["financialYearId"].ToString()) > 0)
            {
                result = await _salesTourPlanClient.GetFinancialYearDetailsAsync(UtilityHelper.LongTryToParse(Session["financialYearId"].ToString()));

                result.EncryptedId = UtilityHelper.ConvertToMd5(result.Id.ToString(), SecurityConstants.EncryptionKey);


            }
            return View(result);
        }

        /// <summary>
        /// Method to financial year post action
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageOrganization)]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> FinancialYear(FinancialYearViewModel financialyearViewModel)
        {

            if (!String.IsNullOrEmpty(financialyearViewModel.EncryptedId))
            {
                financialyearViewModel.EncryptedId = financialyearViewModel.EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(financialyearViewModel.EncryptedId, SecurityConstants.EncryptionKey);

                financialyearViewModel.Id = UtilityHelper.IntTryToParse(decryptedId);
            }


            financialyearViewModel = Helper.SanitizeModel<FinancialYearViewModel>(financialyearViewModel);
            var result = await _salesTourPlanClient.SaveFinancialYearAsync(financialyearViewModel);
            if (result.PostStatus)
            {
                Session["financialYearViewModel"] = result;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to financial year add and edit redirect action
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult FYERedirect(string EncryptedId = "")
        {
            var id = "";

            if (!String.IsNullOrEmpty(EncryptedId))
            {
                EncryptedId = EncryptedId.Replace(' ', '+');
                id = UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey);

            }

            Session["financialYearId"] = id;
            return RedirectToAction("FinancialYear", "SalesTourPlan");
        }

        /// <summary>
        /// Method to Financial YEears get action
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageOrganization)]
        public ActionResult FinancialYears()
        {
            Session["financialYearId"] = null;
            var result = new FinancialYearViewModel();
            if (Session["financialYearViewModel"] != null)
            {
                var financialYearViewModel = Session["financialYearViewModel"] as FinancialYearViewModel;
                result.PostMessage = financialYearViewModel.PostMessage;
                result.PostStatus = true;
            }
            Session["financialYearViewModel"] = null;
            return View(result);
        }

        /// <summary>
        /// Method to get financial year list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetFinancialYearListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.GetFinancialYearListAsync();
            if (result == null)
                return Json(result);
            var gridResult = result.ToDataSourceResult(request);
            gridResult.Total = result.Count;
            return Json(gridResult);
        }

        public async Task<ActionResult> GetFinancialYearList([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.GetFinancialYearListAsync();
            if (result == null)
                return Json(result);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get ststus list
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetMonthList([DataSourceRequest] DataSourceRequest request, long YearId)
        {
            var result = await _salesTourPlanClient.GetPJPMonths(YearId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get Approved PermanentJourneyPlan By User list
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> ApprovedPermanentJourneyPlanByUser([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.ApprovedPermanentJourneyPlanByUser(UserId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get Months By User PermanentJourneyPlan list
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> MonthsByUserPermanentJourneyPlan([DataSourceRequest] DataSourceRequest request, long PJPId)
        {
            var result = await _salesTourPlanClient.MonthsByUserPermanentJourneyPlan(PJPId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get Dealers By User PermanentJourneyPlan list
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> DealersByUserPermanentJourneyPlan([DataSourceRequest] DataSourceRequest request, long PJPId)
        {
            var result = await _salesTourPlanClient.DealersByUserPermanentJourneyPlan(PJPId, 0);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Headquarters
        /// <summary>
        /// Method to Headquarters get action
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public async Task<ActionResult> Headquarter()
        {
            TempData["headQuartersViewModel"] = null;
            var result = new HeadQuartersViewModel();
            if (Session["HeadquarterId"] != null && UtilityHelper.LongTryToParse(Session["HeadquarterId"].ToString()) > 0)
            {
                result = await _salesTourPlanClient.GetHeadquarterDetailsByIdAsync(UtilityHelper.LongTryToParse(Session["HeadquarterId"].ToString()));
            }
            return View(result);
        }

        /// <summary>
        /// Method to Headquarters post action
        /// </summary>
        /// <returns></returns>        
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> Headquarter(HeadQuartersViewModel headquartersViewModel)
        {
            headquartersViewModel.CreatedBy = UserId;
            headquartersViewModel = Helper.SanitizeModel<HeadQuartersViewModel>(headquartersViewModel);
            headquartersViewModel = await _salesTourPlanClient.HeadQuartersAsync(headquartersViewModel);
            if (headquartersViewModel.PostStatus)
            {
                TempData["headQuartersViewModel"] = headquartersViewModel;
                return RedirectToAction("Headquarters");
            }
            return View(headquartersViewModel);
        }

        /// <summary>
        /// Method to Headquarters add and edit redirect action
        /// </summary>
        /// <param name="headquarterId"></param>
        /// <returns></returns>
        public ActionResult HeadquarterEditRedirect(string headquarterId = "")
        {
            Session["HeadquarterId"] = headquarterId;
            return RedirectToAction("Headquarter", "SalesTourPlan");
        }

        /// <summary>
        /// Method to Headquarters get action
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public ActionResult Headquarters()
        {
            var result = new HeadQuartersViewModel();
            if (TempData["headQuartersViewModel"] != null)
            {
                var roleTypeClaim = TempData["HeadQuartersViewModel"] as HeadQuartersViewModel;
                result.PostMessage = roleTypeClaim.PostMessage;
                result.PostStatus = true;
            }
            return View(result);
        }

        /// <summary>
        /// Method to get Headquarters list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetAllHeadquarterListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.GetAllHeadQuartersListAsync();
            var gridList = result.ToDataSourceResult(request);
            gridList.Total = result.Count;
            return Json(gridList);
        }

        /// <summary>
        /// Method to get active Headquarters list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetActiveHeadquartersListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.GetActiveHeadQuartersListAsync();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ExportHeadQuarters(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<HeadquartersDto> resultList = new List<HeadquartersDto>();
                resultList = await _salesTourPlanClient.ExportHeadQuarters(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "Headquarters_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
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
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Address"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Zone"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_State"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Territory"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_District"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_City"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("msg_Status"));

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Name.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Address.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Zone.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.State.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Territory.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.District.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.City.ToString());
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

        #region Reasons
        /// <summary>
        /// Method to Reason get action
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public async Task<ActionResult> Reason()
        {
            TempData["reasonViewModel"] = null;
            var result = new ReasonsViewModel();
            if (Session["ReasonId"] != null && UtilityHelper.LongTryToParse(Session["ReasonId"].ToString()) > 0)
            {
                result = await _salesTourPlanClient.GetReasonDetailsByIdAsync(UtilityHelper.LongTryToParse(Session["ReasonId"].ToString()));
            }
            return View(result);
        }

        /// <summary>
        /// Method to Reason post action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> Reason(ReasonsViewModel reasonViewModel)
        {
            reasonViewModel.CreatedBy = UserId;
            reasonViewModel = Helper.SanitizeModel<ReasonsViewModel>(reasonViewModel);
            reasonViewModel = await _salesTourPlanClient.ReasonsAsync(reasonViewModel);
            if (reasonViewModel.PostStatus)
            {
                TempData["reasonViewModel"] = reasonViewModel;
                return RedirectToAction("Reasons");
            }
            return View(reasonViewModel);
        }

        /// <summary>
        /// Method to Reasons add and edit redirect action
        /// </summary>
        /// <param name="reasonId"></param>
        /// <returns></returns>
        public ActionResult ReasonEditRedirect(string reasonId = "")
        {
            Session["ReasonId"] = reasonId;
            return RedirectToAction("Reason", "SalesTourPlan");
        }

        /// <summary>
        /// Method to Reasons get action
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageMaster, Claims.ViewMaster)]
        public ActionResult Reasons()
        {
            var result = new ReasonsViewModel();
            if (TempData["reasonViewModel"] != null)
            {
                var roleTypeClaim = TempData["reasonViewModel"] as ReasonsViewModel;
                result.PostMessage = roleTypeClaim.PostMessage;
                result.PostStatus = true;
            }
            return View(result);
        }

        /// <summary>
        /// Method to get Reasons list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetAllReasonListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.GetAllReasonsListAsync();
            var gridList = result.ToDataSourceResult(request);
            gridList.Total = result.Count;
            return Json(gridList);
        }

        /// <summary>
        /// Method to get active Reasons list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetActiveReasonsListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.GetActiveReasonsListAsync();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetReasonNameById(long reasonId)
        {
            var result = await _salesTourPlanClient.GetActiveReasonsListAsync();
            var reasonName = result.Where(_ => _.Id == reasonId).FirstOrDefault() != null ? result.Where(_ => _.Id == reasonId).FirstOrDefault().Reason : String.Empty ;
            return Json(reasonName, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetDealerListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.GetDealerListAsync();
            return Json(result.MTPDealerDetail, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetDealerNameById(long dealerId)
        {
            var result = await _salesTourPlanClient.GetDealerListAsync();
            var dealerName = result.MTPDealerDetail.Where(_ => _.Id == dealerId).FirstOrDefault() != null ? result.MTPDealerDetail.Where(_ => _.Id == dealerId).FirstOrDefault().Dealer :String.Empty;
            return Json(dealerName, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Method to PJP post action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> MonthlyTourPlanDateCalendar(PermanentJourneyPlanDetailsDto pjpViewModel)
        {
            FinancialYearDto financialYearDto = new FinancialYearDto();
            pjpViewModel = Helper.SanitizeModel(pjpViewModel);
            financialYearDto = await _salesTourPlanClient.MonthlyTourPlanDateCalendar(pjpViewModel);
            financialYearDto.EffectiveFromstring = financialYearDto.EffectiveFrom.ToShortDateString();
            financialYearDto.EffectiveTostring = financialYearDto.EffectiveTo.ToShortDateString();
            return Json(financialYearDto, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Method to PJP post action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> PermanentJourneyPlanDateRangeCalendar(FinancialYearIdDto financialYearIdDto)
        {
            FinancialYearDto financialYearDto = new FinancialYearDto();
            financialYearIdDto = Helper.SanitizeModel(financialYearIdDto);
            financialYearDto = await _salesTourPlanClient.GetFinancialYearDetailsAsync(financialYearIdDto.FinancialYearid);
            financialYearDto.EffectiveFromstring = financialYearDto.EffectiveFrom.ToShortDateString();
            financialYearDto.EffectiveTostring = financialYearDto.EffectiveTo.ToShortDateString();
            return Json(financialYearDto, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to PJP post action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> CityByUserPermanentJourneyPlan(PermanentJourneyPlanDetailsDto pjpViewModel)
        {
            List<CityDto> cityDto = new List<CityDto>();
            pjpViewModel = Helper.SanitizeModel(pjpViewModel);
            cityDto = await _salesTourPlanClient.CityByUserPermanentJourneyPlan(pjpViewModel);
            return Json(cityDto, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Pending Monthly Plan Deviation

        [AuthorizeClaims(Claims.ManageSalesTourPlan, Claims.ViewSalesTourPlan)]
        public ActionResult MonthlyTourPlanDeviation()
        {
            var result = new MonthlyPlanDeviationViewModel();
            return View(result);
        }

        /// <summary>
        /// Method to PJP post action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<JsonResult> MonthlyTourPlanDeviation(MonthlyPlanDeviationViewModel monthlyPlanDeviationViewModel)
        {
            monthlyPlanDeviationViewModel.CreatedBy = UserId;
            monthlyPlanDeviationViewModel = Helper.SanitizeModel(monthlyPlanDeviationViewModel);
            monthlyPlanDeviationViewModel = await _salesTourPlanClient.AddMonthlyPlanDeviationAsync(monthlyPlanDeviationViewModel);
            Session["MTPDeviationViewModel"] = monthlyPlanDeviationViewModel;
            return Json(monthlyPlanDeviationViewModel, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Method to get PermanentJournyPlan list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetMonthlyTourPlanDetailsListAsync([DataSourceRequest] DataSourceRequest request, long MTPId)
        {
            var result = await _salesTourPlanClient.ApprovedMonthlyTourPlanDetailsByUserAsync(MTPId);
            result.Select(w => w.Reasons = string.Empty).ToList();
            var gridList = result.ToDataSourceResult(request);
            gridList.Total = result.Count;
            return Json(gridList);
        }

        /// <summary>
        /// Method to get Approved MTP By User list
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> ApprovedMonthlyTourPlanByUserDDL([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.ApprovedMonthlyTourPlanByUserDDL(UserId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeClaims(Claims.ManageSalesTourPlan, Claims.ViewSalesTourPlan)]
        public async Task<ActionResult> MonthlyTourPlanDeviationApproval()
        {
            var result = new MonthlyPlanDeviationViewModel();
            var loginUserId = UserId;
            result = await _salesTourPlanClient.CheckMonthlyPlanDeviationApproveByLoginedUser(loginUserId);
            if (Session["MTPDeviationViewModel"] != null)
            {
                var pjpModel = Session["MTPDeviationViewModel"] as MonthlyPlanDeviationViewModel;
                result.PostMessage = pjpModel.PostMessage;
                result.PostStatus = true;
            }
            Session["MTPDeviationViewModel"] = null;
            return View(result);
        }

        /// <summary>
        /// Method to PJP post action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<JsonResult> MonthlyTourPlanDeviationApproval(MonthlyPlanDeviationViewModel monthlyPlanDeviationViewModel)
        {
            monthlyPlanDeviationViewModel.CreatedBy = UserId;
            monthlyPlanDeviationViewModel = Helper.SanitizeModel(monthlyPlanDeviationViewModel);
            monthlyPlanDeviationViewModel = await _salesTourPlanClient.UpdateMonthlyPlanDeviationAsync(monthlyPlanDeviationViewModel);
            return Json(monthlyPlanDeviationViewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get Approved MTP By User list
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> PendingMonthlyPlanDeviationAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.PendingMonthlyPlanDeviation(UserId);
            var gridList = result.ToDataSourceResult(request);
            gridList.Total = result.Count;
            return Json(gridList);
        }

        /// <summary>
        /// Method to get Approved MTP By User list
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ApprovedMonthlyPlanDeviationAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.ApprovedMonthlyPlanDeviation(UserId);
            var gridList = result.ToDataSourceResult(request);
            gridList.Total = result.Count;
            return Json(gridList);
        }

        /// <summary>
        /// Method to get status list
        /// </summary>
        /// <returns></returns>
        public JsonResult GetApproveList([DataSourceRequest] DataSourceRequest request)
        {
            var approveList = ((MonthlyPlanDeviationStatus[])Enum.GetValues(typeof(MonthlyPlanDeviationStatus))).Select(c => new EnumModel() { EntityTypeId = (int)c, Name = c.ToString() }).ToList();
            return Json(approveList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //public ActionResult TodayActivities()
        //{
        //    var result = new MonthlyTourPlanViewModel();
        //    return View(result);
        //}

        #region User Target
        /// <summary>
        /// Method to User Target get action
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Target()
        {
            var result = new UserTargetDto();
            if (Session["TargetId"] != null && UtilityHelper.LongTryToParse(Session["TargetId"].ToString()) > 0)
            {
                var inputDto = new IdInputDto();
                inputDto.Id = UtilityHelper.LongTryToParse(Session["TargetId"].ToString());
                inputDto.LoginUserId = UserId;
                result = await _salesTourPlanClient.GetTargetDetailsById(inputDto);
            }
            return View(result);
        }

        /// <summary>
        /// Method to Target post action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> Target(UserTargetDto userTargetDto)
        {
            userTargetDto.LoginUserId = UserId;
            userTargetDto.AssignedFromId = UserId;
            userTargetDto = Helper.SanitizeModel<UserTargetDto>(userTargetDto);
            userTargetDto = await _salesTourPlanClient.AddOrUpdateUserTarget(userTargetDto);
            Session["userTargetDto"] = userTargetDto;
            return Json(userTargetDto, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to Targets add and edit redirect action
        /// </summary>
        /// <param name="TargetId"></param>
        /// <returns></returns>
        public ActionResult TargetEditRedirect(string targetId = "")
        {
            Session["TargetId"] = targetId;
            return RedirectToAction("Target", "SalesTourPlan");
        }

        /// <summary>
        /// Method to Reasons get action
        /// </summary>
        /// <returns></returns>
        public ActionResult Targets()
        {
            var result = new UserTargetDto();
            if (Session["userTargetDto"] != null)
            {
                var roleTypeClaim = Session["userTargetDto"] as UserTargetDto;
                result.PostMessage = roleTypeClaim.PostMessage;
                result.PostStatus = true;
            }
            return View(result);
        }

        /// <summary>
        /// Method to get target list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetAllTargetListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var inputDto = new IdInputDto();
            inputDto.LoginUserId = UserId;
            var result = await _salesTourPlanClient.GetUserTargetList(inputDto);
            var gridList = result.ToDataSourceResult(request);
            gridList.Total = result.Count;
            return Json(gridList);
        }

        /// <summary>
        /// Method to get User Assigned To list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetUserAssignedToAsync([DataSourceRequest] DataSourceRequest request,long verticalId,long SalesOrganizationId,long DistributionChannelId,int StateId)
        {
            var inputDto = new IdInputDto();
            inputDto.Id = RoleId;
            inputDto.LoginUserId = UserId;
            inputDto.SalesOrganizationId = SalesOrganizationId;
            inputDto.DistributionChannelId = DistributionChannelId;
            inputDto.DivisionId = verticalId;
            inputDto.StateId = StateId;
            var result = await _salesTourPlanClient.GetUserAssignedToList(inputDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region User Sales and Sauda Target
        public async Task<ActionResult> UserSalesSaudaTarget()
        {
            var result = new UserSalesSaudaTargetDto();
            if (Session["userid"] != null && UtilityHelper.LongTryToParse(Session["userid"].ToString()) > 0 && Session["financialyearid"] != null && UtilityHelper.LongTryToParse(Session["financialyearid"].ToString()) > 0)
            {
                var inputDto = new IdInputDto();
                inputDto.Id = UtilityHelper.LongTryToParse(Session["financialyearid"].ToString());
                inputDto.LoginUserId = UtilityHelper.LongTryToParse(Session["userid"].ToString());
                result = await _salesTourPlanClient.UserSalesSaudaTargetdetailbyId(inputDto);
            }
            return View(result);
        }

        public async Task<ActionResult> UserSaleTargetDetail(int FinancialYearId)
        {
            var result = new UserSalesSaudaTargetDto();
            var resultdetail = new List<UserSalesSaudaTargetDetailDto>();
            var inputDto = new FinancialYearIdDto();
            inputDto.FinancialYearid = FinancialYearId;
            resultdetail = await _salesTourPlanClient.UserSaleTargetDetail(inputDto);
            result.UserSalesSaudaTargetDetail = resultdetail;
            return PartialView(result);
        }


        /// <summary>
        /// Method to Reason post action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> UserSalesSaudaTarget(UserSalesSaudaTargetDto userSalesSaudaTargetDto)
        {
            userSalesSaudaTargetDto.CreatedBy = UserId;
            userSalesSaudaTargetDto = Helper.SanitizeModel<UserSalesSaudaTargetDto>(userSalesSaudaTargetDto);
            userSalesSaudaTargetDto = await _salesTourPlanClient.AddUserSalesSaudaTargetAsync(userSalesSaudaTargetDto);
            if (userSalesSaudaTargetDto.PostStatus)
            {
                return RedirectToAction("UserSalesSaudaTargets");
            }
            return View(userSalesSaudaTargetDto);
        }

        public ActionResult UserSalesSaudaTargets()
        {
            var result = new UserSalesSaudaTargetDto();
            return View(result);
        }

        /// <summary>
        /// Method to get category list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> UserSalesSaudaTargetList([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.UserSalesSaudaTargetList();
            var gridList = result.ToDataSourceResult(request);
            gridList.Total = result.Count;
            return Json(gridList);
        }

        /// <summary>
        /// Method to get category list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> UserSalesSaudaTargetDetailList([DataSourceRequest] DataSourceRequest request, long userid, int financialyearid)
        {
            var result = await _salesTourPlanClient.UserSalesSaudaTargetDetailList(userid, financialyearid);
            var gridList = result.ToDataSourceResult(request);
            gridList.Total = result.Count;
            return Json(gridList);
        }

        /// <summary>
        /// Method to Targets add and edit redirect action
        /// </summary>
        /// <param name="TargetId"></param>
        /// <returns></returns>
        public ActionResult UserSalesSaudaTargetEditRedirect(long userid = 0, int financialyearid = 0)
        {
            Session["userid"] = userid;
            Session["financialyearid"] = financialyearid;
            return RedirectToAction("UserSalesSaudaTarget", "SalesTourPlan");
        }
        #endregion

        /// <summary>
        /// Method to get Headquarters list for drop down
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetAllHeadquarterForddl()
        {
            var result = await _salesTourPlanClient.GetAllHeadQuartersListAsync();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region UserOiltypeTarget

        public async Task<ActionResult> GetMonthAndYearByFinancialYear(int financialYearId)
        {
            var result = new UserCustomerSalesTargetDto();
            var inputDto = new FinancialYearIdDto { FinancialYearid = financialYearId };
            result.UserTargetDetail = await _salesTourPlanClient.GetMonthAndYearByFinancialYear(inputDto);
            return PartialView("_userSalesTargetPartial", result);
        }

        public async Task<ActionResult> GetOilTypeTargetMonthsByFinantialYear(int financialYearId)
        {
            var result = new UserOilTypeTargetDto();
            var inputDto = new FinancialYearIdDto { FinancialYearid = financialYearId };
            result.UserOiltypeTargetDetail = await _salesTourPlanClient.GetMonthAndYearByFinancialYear(inputDto);
            return PartialView("_userOilTypeTargetPartial", result);
        }

        public ActionResult UserOiltypeTargetList()
        {
            var result = new UserOilTypeTargetDto(); ;
            return View(result);
        }

        public async Task<ActionResult> GetUserOiltypeTargetListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _salesTourPlanClient.UserOiltypeTargetList();
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public async Task<ActionResult> GetUserOiltypeTargetDetailListAsync([DataSourceRequest] DataSourceRequest request, long userid, int financialyearid)
        {
            var result = await _salesTourPlanClient.UserOiltypeTargetDetailList(userid, financialyearid);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public async Task<ActionResult> UserOiltypeTarget()
        {
            var result = new UserOilTypeTargetDto();
            if (Session["userid"] != null && UtilityHelper.LongTryToParse(Session["userid"].ToString()) > 0 && Session["financialyearid"] != null && UtilityHelper.LongTryToParse(Session["financialyearid"].ToString()) > 0)
            {
                var inputDto = new UserTargetIdDto
                {
                    FinancialYearId = UtilityHelper.LongTryToParse(Session["financialyearid"].ToString()),
                    AssignedToUserId = UtilityHelper.LongTryToParse(Session["userid"].ToString())
                };
                result = await _salesTourPlanClient.GetUserOiltypeTargetdetailbyId(inputDto);
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> AddOrUpdateUserOiltypeTarget(UserOilTypeTargetDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            inputDto = Helper.SanitizeModel<UserOilTypeTargetDto>(inputDto);
            inputDto = await _salesTourPlanClient.AddUserOiltypeTarget(inputDto);
            if (inputDto.PostStatus)
            {
                TempData["SuccessMessage"] = inputDto.PostMessage;
                return RedirectToAction("UserOiltypeTargetList");
            }
            return View("UserOiltypeTarget", inputDto);
        }

        public ActionResult UserOiltypeTargetEditRedirect(long userid = 0, int financialyearid = 0)
        {
            Session["userid"] = userid;
            Session["financialyearid"] = financialyearid;
            return RedirectToAction("UserOiltypeTarget", "SalesTourPlan");
        }

        #endregion       

        #region Todays Activities

        //public ActionResult TodayActivities()
        //{
        //    return View();
        //}


        /// <summary>
        /// Method to get Todays Activities Tour Plan list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetTodayActivitiesListAsync([DataSourceRequest] DataSourceRequest request, TodayActivitiesInputDto todayActivitiesInputDto)
        {
            if (todayActivitiesInputDto != null)
            {
                todayActivitiesInputDto.LoginUserId = UserId;
            }
            var result = await _salesTourPlanClient.GetTodayActivitiesListAsync(todayActivitiesInputDto);
            var gridList = result.ToDataSourceResult(request);
            gridList.Total = result.Count;
            return Json(gridList);
        }

        public ActionResult TodayActivity()
        {
            return View();
        }

        /// <summary>
        /// Method to get Pending Sauda list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetPendingSaudaListAsync([DataSourceRequest] DataSourceRequest request)
        {
            PendingSaudaInputDto pendingSaudaInputDto = new PendingSaudaInputDto();
            if (Session["DealerID"] != null)
            {
                pendingSaudaInputDto.DealerId = UtilityHelper.LongTryToParse(Session["DealerID"].ToString());
            }
            var result = await _salesTourPlanClient.GetPendingSaudaListAsync(pendingSaudaInputDto);
            var gridList = result.ToDataSourceResult(request);
            gridList.Total = result.Count;
            return Json(gridList);
        }

        #endregion

        #region Today Activity

        [AuthorizeClaims(Claims.ManageSalesTourPlan, Claims.ViewSalesTourPlan)]
        public ActionResult TodayActivities()
        {
            var result = new MonthlyTourPlanViewModel();
            result.LoginUserId = UserId;
            return View(result);
        }

        [AuthorizeClaims(Claims.ManageSalesTourPlan, Claims.ViewSalesTourPlan)]
        public ActionResult ActivityDetails()
        {
            var result = new SalesTourPlanParamDto();
            result.DealerId = Session["DelaerId"] != null ? UtilityHelper.LongTryToParse(Session["DelaerId"].ToString()) : 0;
            result.CreatedDate = Session["CreatedDate"] != null ? (DateTime)Session["CreatedDate"] : DateTime.Now;
            return View(result);
        }

        [AuthorizeClaims(Claims.ManageSalesTourPlan, Claims.ViewSalesTourPlan)]
        public ActionResult EditTodayActivity(string EncryptedId, DateTime createdDate)
        {
            var dealerId = "";
            if (!String.IsNullOrEmpty(EncryptedId))
            {
                EncryptedId = EncryptedId.Replace(' ', '+');
                dealerId = UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey);

            }

            Session["DelaerId"] = dealerId;
            Session["CreatedDate"] = createdDate;
            return RedirectToAction("ActivityDetails");
        }

        public async Task<ActionResult> GetProspectiveDealersAsync([DataSourceRequest] DataSourceRequest request, long dealerId)
        {
            var inputDto = new SalesTourPlanParamDto() { DealerId = dealerId, CreatedDate = DateTime.Now };
            var result = await _salesTourPlanClient.GetProspectiveDealers(inputDto);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public async Task<ActionResult> GetPendingSaudaRemarksList([DataSourceRequest] DataSourceRequest request, long dealerId, DateTime searchDate)
        {
            var inputDto = new SalesTourPlanParamDto() { DealerId = dealerId, CreatedDate = searchDate };
            var result = await _salesTourPlanClient.GetPendingSaudaRemarksList(inputDto);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public async Task<ActionResult> GetMarketScenariosList([DataSourceRequest] DataSourceRequest request, long dealerId, DateTime searchDate)
        {
            var inputDto = new SalesTourPlanParamDto() { DealerId = dealerId, CreatedDate = searchDate };
            var result = await _salesTourPlanClient.GetMarketScenariosList(inputDto);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public async Task<ActionResult> GetCompetitorsList([DataSourceRequest] DataSourceRequest request, long dealerId, DateTime searchDate)
        {
            var inputDto = new SalesTourPlanParamDto() { DealerId = dealerId, CreatedDate = searchDate };
            var result = await _salesTourPlanClient.GetCompetitorsList(inputDto);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public async Task<ActionResult> GetWholesellerCompetitorsList([DataSourceRequest] DataSourceRequest request, long dealerId, DateTime searchDate)
        {
            var inputDto = new SalesTourPlanParamDto() { DealerId = dealerId, CreatedDate = searchDate };
            var result = await _salesTourPlanClient.GetWholesellerCompetitorsList(inputDto);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public async Task<ActionResult> GetProspectiveDealerList([DataSourceRequest] DataSourceRequest request, long dealerId, DateTime searchDate)
        {
            var inputDto = new SalesTourPlanParamDto() { DealerId = dealerId, CreatedDate = searchDate };
            var result = await _salesTourPlanClient.GetProspectiveDealerList(inputDto);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public async Task<ActionResult> GetSecondarySalesFortheDayByWholesellerForWeb([DataSourceRequest] DataSourceRequest request, long dealerId, DateTime searchDate)
        {
            var inputDto = new SecondarySalesInputDto() { EmployeeId = dealerId, VisitDate = searchDate };
            var result = await _salesTourPlanClient.GetSecondarySalesFortheDayByWholesellerForWeb(inputDto);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public async Task<ActionResult> GetCompetitorSkuList([DataSourceRequest] DataSourceRequest request, long competitorsId)
        {
            var inputDto = new SalesTourPlanParamDto() { Id = competitorsId };
            var result = await _salesTourPlanClient.GetCompetitorSkuList(inputDto);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public async Task<ActionResult> CompetitorAttachments(int Id)
        {
            var inputDto = new AttachmentInputDto() { RecordId = Id, PageId = (int)DTO.Enums.PageType.Competitor };
            var result = await _salesTourPlanClient.GetFileAttachments(inputDto);
            return PartialView(result);
        }

        public async Task<ActionResult> ProspectiveDealerAttachments(int Id)
        {
            var inputDto = new AttachmentInputDto() { RecordId = Id, PageId = (int)DTO.Enums.PageType.ProspectiveDealer };
            var result = await _salesTourPlanClient.GetFileAttachments(inputDto);
            return PartialView("CompetitorAttachments", result);
        }

        public async Task<ActionResult> GetSecondarySalesDetails([DataSourceRequest] DataSourceRequest request, long WholesellerId, DateTime searchDate)
        {
            var inputDto = new WholesellerSecondarySalesInputDto() { WholesellerId = WholesellerId, VisitDate = searchDate };
            var result = await _salesTourPlanClient.GetSecondarySalesDetails(inputDto);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public async Task<ActionResult> GetTodayActivitiesList([DataSourceRequest] DataSourceRequest request, long bdoId, DateTime searchDate)
        {
            if (!(Helper.CheckClaims(Claims.ManageOrganization)))
                bdoId = UserId;
            var inputDto = new TodayActivitiesInputDto() { LoginUserId = bdoId, TodayDate = searchDate };
            var result = await _salesTourPlanClient.GetTodayActivitiesList(inputDto);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        #endregion

        [AuthorizeClaims(Claims.ManageAttendance, Claims.ViewAttendance)]
        public ActionResult UserAttendence()
        {
            return View();
        }

        #region User Attendence
        public async Task<ActionResult> GetMonthsByFinancialYear(int financialYearId)
        {
            var inputDto = new FinancialYearIdDto { FinancialYearid = financialYearId };
            var data = (await _salesTourPlanClient.GetMonthAndYearByFinancialYear(inputDto)).Select(s => new DropDownDto() { Id = s.MonthId, Name = s.MonthAndYear });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetUserAttendence(UserAttendenceInputDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _salesTourPlanClient.GetUserAttendence(inputDto);
            return PartialView(result);

        }
        #endregion

        #region STP History

        [HttpGet]
        public async Task<ActionResult> GetSalesTourPlanPcpHistory(long id)
        {
            var result = await _salesTourPlanClient.GetSalesTourPlanPcpHistory(id);
            return PartialView("SalesTourPlanPcpHistory", result);
        }

        [HttpGet]
        public async Task<ActionResult> GetSalesTourPlanMtpHistory(long id)
        {
            var result = await _salesTourPlanClient.GetSalesTourPlanMtpHistory(id);
            return PartialView("SalesTourPlanMtpHistory", result);
        }

        public JsonResult GetSTPVisitType([DataSourceRequest] DataSourceRequest request)
        {
            var salesPersonList = _salesTourPlanClient.GetSTPVisitType();
            return Json(salesPersonList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Excel Download

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

        public async Task<ActionResult> GenerateExcelSKUAsync()
        {
            var stream = new MemoryStream();
            var result = new ResultModel { IsSuccess = false, Message = "Error Occured while exporting Excel. Please retry." };
            try
            {
                var skuDetails = await _masterClient.GetSkuListAsync(new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = true });

                //var fileName = $"{Guid.NewGuid()}.xlsx";
                var fileName = "SKU-LIST-" + string.Format(Settings.ReportDateFormat, DateTime.Now).ToUpper() + ".xlsx";

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
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SkuId"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SkuName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SkuCode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OilType"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Vertical"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_TDAndPacktype"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PackSizeQuantity"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PackSize"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_PackGroup"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ProcessCost"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SubCategory"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_UOM1_No"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "UOM2 (" + @Helper.GetResourceString("lbl_CaseToNumberConversion") + ")");
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "UOM3 (" + @Helper.GetResourceString("lbl_MetricTonToNumberConversion") + ")");
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_MaterialType"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IsActive"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IsUpdateRequired"));


                    foreach (var sku in skuDetails)
                    {
                        rowIndex++;
                        colIndex = 1;
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.Id.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.SkuName != null ? sku.SkuName.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.SkuCode != null ? sku.SkuCode.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.OilType != null ? sku.OilType.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.VerticalCode != null ? sku.VerticalCode.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.TDAndPacktype != null ? sku.TDAndPacktype.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.Quantity.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.QuantityTypeUom != null ? sku.QuantityTypeUom.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.OilPackingType != null ? sku.OilPackingType.ToString() : string.Empty);
                        //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.ProcessCost.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.SubCategory != null ? sku.SubCategory.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.UOM1_No.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.ConversionFactor1.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.ConversionFactor2.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], !string.IsNullOrEmpty(sku.MaterialTypeName) ? sku.MaterialTypeName.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.IsActive.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], sku.NewlyAdded != null ? sku.NewlyAdded.ToString() : string.Empty);

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
    }
}