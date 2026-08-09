using System;
using System.Linq;
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


namespace Adani.Solution.MVC.Controllers
{
    [TokenAuthorize]
    [CustomRedirect]
    [NoCache]
    public class TargetController : BaseController
    {
        private readonly TargetClient _targetClient;

        public TargetController()
        {
            _targetClient = new TargetClient { ControllerDelegate = this };
        }


        #region UserCustomerSalesTarget

        public async Task<ActionResult> GetUserCustomerSalesTargetMonthsByFinancialYear(int financialYearId)
        {
            var result = new UserCustomerSalesTargetDto();
            var inputDto = new FinancialYearIdDto { FinancialYearid = financialYearId };
            result.UserTargetDetail = await _targetClient.GetMonthAndYearByFinancialYear(inputDto);
            return PartialView("_userCustomerSalesTargetPartial", result);
        }

        [AuthorizeClaims(Claims.ManageTarget, Claims.ViewTarget)]
        public ActionResult UserCustomerSalesTargetList()
        {
            var result = new UserCustomerSalesTargetDto
            {
                LoginUserId = UserId,
                LoginUserName = UserName
            };

            return View(result);
        }

        [AuthorizeClaims(Claims.ManageTarget, Claims.ViewTarget)]
        public ActionResult UserAssignedSalesTargetList()
        {
            var result = new UserCustomerSalesTargetDto
            {
                LoginUserId = UserId,
                LoginUserName = UserName
            };

            return View(result);
        }

        public async Task<ActionResult> GetAssignedSalesTargetListAsync([DataSourceRequest] DataSourceRequest request)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = true, VerticalId = VerticalId };
            var result = await _targetClient.GetAssignedSalesTargetListAsync(loginUserIdDto);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public async Task<ActionResult> GetUserCustomerSalesTargetListAsync([DataSourceRequest] DataSourceRequest request)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = true, VerticalId = VerticalId };
            var result = await _targetClient.UserSalesTargetList(loginUserIdDto);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public ActionResult GetSalesTargetEmptyListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<MapSalesTargetDetailDto>();
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public async Task<ActionResult> GetUserCustomerSalesTargetDetailListAsync([DataSourceRequest] DataSourceRequest request, int oiltypeId, long userid = 0, int financialyearid = 0)
        {
            var result = await _targetClient.UserSalesTargetDetailList(userid, financialyearid, oiltypeId);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public async Task<ActionResult> GetSalesTargetOilTypeListAsync([DataSourceRequest] DataSourceRequest request, long userid, int financialyearid)
        {
            var result = await _targetClient.GetSalesTargetOilTypeListAsync(userid, financialyearid);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        [AuthorizeClaims(Claims.ManageTarget, Claims.ViewTarget)]
        public async Task<ActionResult> UserCustomerSalesTarget()
        {
            var result = new UserCustomerSalesTargetDto();
            if (Session["AssignedToUserId"] != null && UtilityHelper.LongTryToParse(Session["AssignedToUserId"].ToString()) > 0
                && Session["FinancialYearId"] != null && UtilityHelper.LongTryToParse(Session["FinancialYearId"].ToString()) > 0
                && Session["OilTypeId"] != null && UtilityHelper.LongTryToParse(Session["OilTypeId"].ToString()) > 0)
            {
                var inputDto = new UserTargetIdDto
                {
                    FinancialYearId = UtilityHelper.LongTryToParse(Session["FinancialYearId"].ToString()),
                    AssignedToUserId = UtilityHelper.LongTryToParse(Session["AssignedToUserId"].ToString()),
                    OilTypeId = UtilityHelper.LongTryToParse(Session["OilTypeId"].ToString())
                };
                result = await _targetClient.GetUserSalesTargetdetailbyId(inputDto);

                result.EncryptedId = UtilityHelper.ConvertToMd5(result.Id.ToString(), SecurityConstants.EncryptionKey);

                result.LoginUserId = UserId;
                result.LoginUserName = UserName;

                if (result.VerticalId == null || result.VerticalId <= 0)
                    result.VerticalId = VerticalId;

                return View("UpdateUserCustomerSalesTarget", result);
            }
            if (result.VerticalId == null || result.VerticalId <= 0)
                result.VerticalId = VerticalId;

            result.LoginUserId = UserId;
            result.LoginUserName = UserName;
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateUserCustomerSalesTarget(UserCustomerSalesTargetDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            inputDto = Helper.SanitizeModel(inputDto);


            if (!String.IsNullOrEmpty(inputDto.EncryptedId))
            {
                inputDto.EncryptedId = inputDto.EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey);

                inputDto.Id = UtilityHelper.IntTryToParse(decryptedId);
            }

            inputDto = await _targetClient.UpdateUserSalesTarget(inputDto);
            if (inputDto.PostStatus)
            {
                TempData["SuccessMessage"] = inputDto.PostMessage;
                return RedirectToAction("UserCustomerSalesTargetList");
            }
            return View("UserCustomerSalesTarget", inputDto);
        }

        public ActionResult UserCustomerSalesTargetEditRedirect(string EncryptedId = "", int financialyearid = 0, int oiltypeId = 0)
        {
            var userId = "";
            if (!String.IsNullOrEmpty(EncryptedId))
            {
                EncryptedId = EncryptedId.Replace(' ', '+');
                userId = UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey);

            }


            Session["AssignedToUserId"] = userId;
            Session["FinancialYearId"] = financialyearid;
            Session["OilTypeId"] = oiltypeId;
            return RedirectToAction("UserCustomerSalesTarget", "Target");
        }

        [HttpPost]
        public async Task<ActionResult> SaveUserCustomerSalesTarget(List<MapSalesTargetDetailDto> inputDtoList)
        {
            UserCustomerSalesTargetDto result = new UserCustomerSalesTargetDto();
            int userId = UserId;
            if (inputDtoList != null && inputDtoList.Any())
            {
                inputDtoList.Select(c => { c.LoginUserId = UserId; return c; }).ToList();
                result = await _targetClient.SaveUserSalesTargetList(inputDtoList);
            }
            if (result.PostStatus)
            {
                if (string.IsNullOrEmpty(result.ExistRecords))
                    TempData["SuccessMessage"] = result.PostMessage;
                else
                    TempData["ErrorMessage"] = result.ExistRecords;

                return Json(new { IsSuccess = true, redirectUrl = Url.Action("UserCustomerSalesTargetList", "Target") });
            }
            else
                return Json(new { IsSuccess = false, Message = result.PostMessage });
        }

        public async Task<ActionResult> GetOilTypesBasedOnAssignedSalesTarget([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData, int financialYearId, int? verticalId)
        {
            var result = new List<DropDownDto>();
            if (verticalId > 0 && financialYearId > 0)
            {
                var inputdto = new UserTargetIdDto { IsToReturnInactiveData = isToReturnInactiveData, FinancialYearId = financialYearId, AssignedToUserId = UserId, VerticalId = (int)verticalId, RoleTypeId = RoleTypeId };
                result = await _targetClient.GetOilTypesBasedOnAssignedSalesTarget(inputdto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetOilTypesBasedOnAssignedSaudaTarget([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData, int financialYearId, int? verticalId)
        {
            var result = new List<DropDownDto>();
            if (verticalId > 0 && financialYearId > 0)
            {
                var inputdto = new UserTargetIdDto { IsToReturnInactiveData = isToReturnInactiveData, FinancialYearId = financialYearId, AssignedToUserId = UserId, VerticalId = (int)verticalId, RoleId = RoleId, RoleTypeId = RoleTypeId };
                result = await _targetClient.GetOilTypesBasedOnAssignedSaudaTarget(inputdto);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region UserCustomerSaudaTarget

        public async Task<ActionResult> GetUserCustomerSaudaTargetMonthsByFinancialYear(int financialYearId)
        {
            var result = new UserCustomerSaudaTargetDto();
            var inputDto = new FinancialYearIdDto { FinancialYearid = financialYearId };
            result.UserTargetDetail = await _targetClient.GetMonthAndYearByFinancialYear(inputDto);
            return PartialView("_userCustomerSaudaTargetPartial", result);
        }

        [AuthorizeClaims(Claims.ManageTarget, Claims.ViewTarget)]
        public ActionResult UserCustomerSaudaTargetList()
        {
            var result = new UserCustomerSaudaTargetDto
            {
                LoginUserId = UserId,
                LoginUserName = UserName
            };

            return View(result);
        }

        [AuthorizeClaims(Claims.ManageTarget, Claims.ViewTarget)]
        public ActionResult UserAssignedSaudaTargetList()
        {
            var result = new UserCustomerSaudaTargetDto
            {
                LoginUserId = UserId,
                LoginUserName = UserName
            };

            return View(result);
        }

        public async Task<ActionResult> GetAssignedSaudaTargetListAsync([DataSourceRequest] DataSourceRequest request)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = true, VerticalId = VerticalId };
            var result = await _targetClient.GetAssignedSaudaTargetListAsync(loginUserIdDto);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public async Task<ActionResult> GetUserCustomerSaudaTargetListAsync([DataSourceRequest] DataSourceRequest request)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = true, VerticalId = VerticalId };
            var result = await _targetClient.UserCustomerSaudaTargetList(loginUserIdDto);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public ActionResult GetCustomerSaudaTargetEmptyListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<MapSaudaTargetDetailDto>();
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public async Task<ActionResult> GetUserCustomerSaudaTargetDetailListAsync([DataSourceRequest] DataSourceRequest request, int oiltypeId, long userid = 0, int financialyearid = 0)
        {
            var result = await _targetClient.UserCustomerSaudaTargetDetailList(userid, financialyearid, oiltypeId);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        public async Task<ActionResult> GetSaudaTargetOilTypeListAsync([DataSourceRequest] DataSourceRequest request, long userid, int financialyearid)
        {
            var result = await _targetClient.GetSaudaTargetOilTypeListAsync(userid, financialyearid);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        [AuthorizeClaims(Claims.ManageTarget, Claims.ViewTarget)]
        public async Task<ActionResult> UserCustomerSaudaTarget()
        {
            var result = new UserCustomerSaudaTargetDto();
            if (Session["AssignedToUserId"] != null && UtilityHelper.LongTryToParse(Session["AssignedToUserId"].ToString()) > 0
                && Session["FinancialYearId"] != null && UtilityHelper.LongTryToParse(Session["FinancialYearId"].ToString()) > 0
                && Session["OilTypeId"] != null && UtilityHelper.LongTryToParse(Session["OilTypeId"].ToString()) > 0)
            {
                var inputDto = new UserTargetIdDto
                {
                    FinancialYearId = UtilityHelper.LongTryToParse(Session["FinancialYearId"].ToString()),
                    AssignedToUserId = UtilityHelper.LongTryToParse(Session["AssignedToUserId"].ToString()),
                    OilTypeId = UtilityHelper.LongTryToParse(Session["OilTypeId"].ToString())
                };
                result = await _targetClient.GetUserCustomerSaudaTargetdetailbyId(inputDto);
                result.LoginUserId = UserId;
                result.EncryptedId = UtilityHelper.ConvertToMd5(result.Id.ToString(), SecurityConstants.EncryptionKey);
                result.LoginUserName = UserName;

                if (result.VerticalId == null || result.VerticalId <= 0)
                    result.VerticalId = VerticalId;

                return View("UpdateUserCustomerSaudaTarget", result);
            }
            if (result.VerticalId == null || result.VerticalId <= 0)
                result.VerticalId = VerticalId;

            result.LoginUserId = UserId;
            result.LoginUserName = UserName;
            return View(result);
        }

        [AuthorizeClaims(Claims.ManageTarget, Claims.ViewTarget)]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(true)]
        public async Task<ActionResult> AddOrUpdateUserCustomerSaudaTarget(UserCustomerSaudaTargetDto inputDto)
        {
            inputDto.LoginUserId = UserId;

            if (!String.IsNullOrEmpty(inputDto.EncryptedId))
            {
                inputDto.EncryptedId = inputDto.EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey);

                inputDto.Id = UtilityHelper.IntTryToParse(decryptedId);
            }

            inputDto = Helper.SanitizeModel(inputDto);
            inputDto = await _targetClient.UpdateUserCustomerSaudaTarget(inputDto);
            if (inputDto.PostStatus)
            {
                TempData["SuccessMessage"] = inputDto.PostMessage;
                return RedirectToAction("UserCustomerSaudaTargetList");
            }
            return View("UserCustomerSaudaTarget", inputDto);
        }

        public ActionResult UserCustomerSaudaTargetEditRedirect(string EncryptedId = "", int financialyearid = 0, int oiltypeId = 0)
        {
            var userid = "";

            if (!String.IsNullOrEmpty(EncryptedId))
            {
                EncryptedId = EncryptedId.Replace(' ', '+');
                userid = UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey);
            }

            Session["AssignedToUserId"] = userid;
            Session["FinancialYearId"] = financialyearid;
            Session["OilTypeId"] = oiltypeId;
            return RedirectToAction("UserCustomerSaudaTarget", "Target");
        }

        [HttpPost]
        public async Task<ActionResult> SaveUserCustomerSaudaTarget(List<MapSaudaTargetDetailDto> inputDtoList)
        {
            UserCustomerSaudaTargetDto result = new UserCustomerSaudaTargetDto();
            int userId = UserId;
            if (inputDtoList != null && inputDtoList.Any())
            {
                inputDtoList.Select(c => { c.LoginUserId = UserId; return c; }).ToList();
                result = await _targetClient.SaveUserCustomerSaudaTargetList(inputDtoList);
            }
            //if (result.PostStatus)
            //{
            //    TempData["SuccessMessage"] = result.PostMessage;
            //}
            //return RedirectToAction("UserCustomerSaudaTargetList", "Target");
            if (result.PostStatus)
            {
                if (string.IsNullOrEmpty(result.ExistRecords))
                    TempData["SuccessMessage"] = result.PostMessage;
                else
                    TempData["ErrorMessage"] = result.ExistRecords;

                return Json(new { IsSuccess = true, redirectUrl = Url.Action("UserCustomerSaudaTargetList", "Target") });
            }
            else
                return Json(new { IsSuccess = false, Message = result.PostMessage });
        }


        #endregion

        #region CustomerTarget
        [AuthorizeClaims(Claims.ManageCustomerTarget, Claims.ViewCustomerTarget)]
        public async Task<ActionResult> UserCustomerTarget()
        {
            var result = new UserCustomerTargetDto();
            if (Session["AssignedToUserId"] != null && UtilityHelper.LongTryToParse(Session["AssignedToUserId"].ToString()) > 0
                && Session["FinancialYearId"] != null && UtilityHelper.LongTryToParse(Session["FinancialYearId"].ToString()) > 0)
            {
                var inputDto = new UserTargetIdDto
                {
                    FinancialYearId = UtilityHelper.LongTryToParse(Session["FinancialYearId"].ToString()),
                    AssignedToUserId = UtilityHelper.LongTryToParse(Session["AssignedToUserId"].ToString())
                };
                result = await _targetClient.GetUserTargetdetailbyId(inputDto);
                result.LoginUserId = UserId;
                result.LoginUserName = UserName;

                return View(result);
            }
            result.LoginUserId = UserId;
            result.LoginUserName = UserName;
            return View(result);
        }
        [HttpPost]
        public async Task<ActionResult> SaveUserCustomerTarget(List<MapSalesTargetDetailDto> inputDtoList)
        {
            UserCustomerTargetDto result = new UserCustomerTargetDto();
            int userId = UserId;
            if (inputDtoList != null && inputDtoList.Any())
            {
                inputDtoList.Select(c => { c.LoginUserId = UserId; return c; }).ToList();
                result = await _targetClient.SaveUserCustomerTarget(inputDtoList);
            }
            if (result.PostStatus)
            {
                if (string.IsNullOrEmpty(result.ExistRecords))
                    TempData["SuccessMessage"] = result.PostMessage;
                else
                    TempData["ErrorMessage"] = result.ExistRecords;

                return Json(new { IsSuccess = true, redirectUrl = Url.Action("UserCustomerTargetList", "Target") });
            }
            else
                return Json(new { IsSuccess = false, Message = result.PostMessage });
        }

        [AuthorizeClaims(Claims.ManageCustomerTarget, Claims.ViewCustomerTarget)]
        public ActionResult UserCustomerTargetList()
        {
            var result = new UserCustomerTargetDto
            {
                LoginUserId = UserId,
                LoginUserName = UserName
            };

            return View(result);
        }

        public ActionResult UserCustomerTargetEditRedirect(long userid = 0, int financialyearid = 0, int oiltypeId = 0)
        {
            Session["AssignedToUserId"] = userid;
            Session["FinancialYearId"] = financialyearid;
            return RedirectToAction("UserCustomerTarget", "Target");
        }
        public async Task<ActionResult> GetUserCustomerTargetListAsync([DataSourceRequest] DataSourceRequest request)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = true, VerticalId = VerticalId };
            var result = await _targetClient.UserTargetList(loginUserIdDto);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }
        public async Task<ActionResult> GetUserCustomerTargetDetailListAsync([DataSourceRequest] DataSourceRequest request, long userid = 0, int financialyearid = 0)
        {
            var result = await _targetClient.UserTargetDetailList(userid, financialyearid);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }

        [AuthorizeClaims(Claims.ManageCustomerTarget, Claims.ViewCustomerTarget)]
        public ActionResult UserAssignedTargetList()
        {
            var result = new UserCustomerSalesTargetDto
            {
                LoginUserId = UserId,
                LoginUserName = UserName
            };

            return View(result);
        }
        public async Task<ActionResult> GetAssignedCustomerTargetListAsync([DataSourceRequest] DataSourceRequest request)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = true, VerticalId = VerticalId };
            var result = await _targetClient.GetAssignedTargetListAsync(loginUserIdDto);
            var gridList = result.ToDataSourceResult(request);
            return Json(gridList);
        }
        public async Task<ActionResult> GetUserCustomerTargetMonthsByFinancialYear(int financialYearId)
        {
            var result = new UserCustomerTargetDto();
            var inputDto = new FinancialYearIdDto { FinancialYearid = financialYearId };
            result.UserTargetDetail = await _targetClient.GetMonthAndYearByFinancialYear(inputDto);
            return PartialView("_userCustomerTargetPartial", result);
        }
        #endregion
    }
}
