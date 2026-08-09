using System.Collections.Generic;
using System.Web.Mvc;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Adani.Solution.MVC.Models;
using Adani.Solution.MVC.ServiceClient;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.Attributes;
using Adani.Solution.DTO;
using System.Linq;
using Adani.Solution.DTO.Enums;
//using System.Configuration;
//using GMCore.Helper;

namespace Adani.Solution.MVC.Controllers
{
    [TokenAuthorize]
    [CustomRedirect]
    [NoCache]
    public class RoleController : BaseController
    {
        private readonly RoleClient _roleClient;

        public RoleController()
        {
            _roleClient = new RoleClient { ControllerDelegate = this };
        }

        #region RoleType 

        /// <summary>
        ///  Method to get the role type
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageRoles)]
        //[AuthorizeRoles(Adani.Solution.DTO.Enums.Role.Admin)]
        public async Task<ActionResult> RoleType()
        {
            var roleTypeClaimDto = new RoleTypeClaimViewModel();
            roleTypeClaimDto.ClaimDto = await _roleClient.GetClaimDetailsAsync();
            if (TempData["RoleTypeClaimViewModel"] != null)
            {
                var roleTypeClaim = TempData["RoleTypeClaimViewModel"] as RoleTypeClaimViewModel;
                roleTypeClaimDto.PostMessage = roleTypeClaim.PostMessage;
                roleTypeClaimDto.PostStatus = true;
            }
            return View(roleTypeClaimDto);
        }

        [NoCache]
        public JsonResult GetClaimsListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var roleTypesList = _roleClient.GetClaimDetailsAsync().Result;
            return Json(roleTypesList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClaimsbyRoleId(long RoleId)
        {
            var inputdto = new RoleIdDto()
            {
                RoleId = RoleId
            };
            var result = _roleClient.GetClaimsbyRoleId(inputdto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClaimsbyRoleTypeId(long RoleId)
        {
            var inputdto = new RoleIdDto()
            {
                RoleId = RoleId
            };
            var result = _roleClient.GetClaimsbyRoleTypeId(inputdto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  Method to create the role 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> RoleType(RoleTypeClaimViewModel roleTypeClaimDto)
        {
            roleTypeClaimDto.LoginUserId = UserId;
            roleTypeClaimDto = await _roleClient.RoleTypeAsync(roleTypeClaimDto);
            if (roleTypeClaimDto.PostStatus)
            {
                TempData["RoleTypeClaimViewModel"] = roleTypeClaimDto;
                return RedirectToAction("RoleType");
            }
            return View(roleTypeClaimDto);
        }

        /// <summary>
        ///  Method to get the update role type
        /// </summary>
        /// <returns></returns>
        //[AuthorizeClaims(Claims.ManageRoles)]
        //[AuthorizeRoles(Adani.Solution.DTO.Enums.Role.Admin)]
        [AuthorizeClaims(Claims.ManageRoles)]
        public async Task<ActionResult> UpdateRoleType()
        {
            var roleTypeClaimUpdateViewModel = new RoleTypeClaimUpdateViewModel();
            roleTypeClaimUpdateViewModel.SystemRoleTypeClaimsDto = await _roleClient.GetRoleTypeClaimsAsync();
            if (TempData["RoleTypeClaimUpdateViewModel"] != null)
            {
                var roleTypeClaim = TempData["RoleTypeClaimUpdateViewModel"] as RoleTypeClaimUpdateViewModel;
                roleTypeClaimUpdateViewModel.PostMessage = roleTypeClaim.PostMessage;
                roleTypeClaimUpdateViewModel.PostStatus = true;
            }
            return View(roleTypeClaimUpdateViewModel);
        }

        /// <summary>
        ///  Method to update the role 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> UpdateRoleType(RoleTypeClaimUpdateViewModel roleTypeClaimUpdateViewModel)
        {
            if (!roleTypeClaimUpdateViewModel.IsSearch)
            {

                roleTypeClaimUpdateViewModel = await _roleClient.UpdateRoleTypeAsync(roleTypeClaimUpdateViewModel, UserId);
               if (roleTypeClaimUpdateViewModel.PostStatus)
                {
                    TempData["RoleTypeClaimUpdateViewModel"] = roleTypeClaimUpdateViewModel;
                    return RedirectToAction("UpdateRoleType");
                }
                else
                {
                    roleTypeClaimUpdateViewModel.SystemRoleTypeClaimsDto = await _roleClient.GetRoleTypeClaimsAsync();
                }
            }
            else
            {
                roleTypeClaimUpdateViewModel.SearchText = (roleTypeClaimUpdateViewModel.SearchText != null) ? roleTypeClaimUpdateViewModel.SearchText : string.Empty;
                roleTypeClaimUpdateViewModel.RoleTypeUpdate = new List<RoleTypeUpdateViewModel>();
                roleTypeClaimUpdateViewModel.SystemRoleTypeClaimsDto = await _roleClient.GetRoleTypeClaimsAsync();
                roleTypeClaimUpdateViewModel.SystemRoleTypeClaimsDto.SystemRoleTypes = roleTypeClaimUpdateViewModel.SystemRoleTypeClaimsDto.SystemRoleTypes.OrderBy(m => m.RoleTypeName.ToLower()
                          .Contains(roleTypeClaimUpdateViewModel.SearchText.ToLower()) ? 0 : 1).ThenBy(m => m.RoleTypeName).ToList();

            }
            return View(roleTypeClaimUpdateViewModel);
        }

        /// <summary>
        /// Method to role type delete and redirection
        /// </summary>
        /// <param name="roleTypeIdDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public async Task<ActionResult> RoleTypeDelete(long roleTypeId)
        {
            var roleTypeIdDto = new RoleTypeIdDto
            {
                RoleTypeId = roleTypeId,
                LoginUserId = UserId
            };
            var roleTypeClaimViewModel = await _roleClient.RoleTypeDeleteAsync(roleTypeIdDto);
            if (roleTypeClaimViewModel.PostStatus)
            {
                TempData["RoleTypeClaimUpdateViewModel"] = roleTypeClaimViewModel;
            }
            return Json(roleTypeClaimViewModel, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Role
        /// <summary>
        ///  Method to get the role type
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageRoles)]
        //[AuthorizeRoles(Adani.Solution.DTO.Enums.Role.Admin)]
        public async Task<ActionResult> Role()
        {
            var roleViewModel = new RoleViewModel();
            roleViewModel.ClaimDto = await _roleClient.GetClaimDetailsAsync();
            if (TempData["RoleViewModel"] != null)
            {
                var roleTypeClaim = TempData["RoleViewModel"] as RoleViewModel;
                roleViewModel.PostMessage = roleTypeClaim.PostMessage;
                roleViewModel.PostStatus = true;
            }
            roleViewModel.RoleTypeId = 1;
            return View(roleViewModel);
        }

        [NoCache]
        public JsonResult GetRoleTypesAsync([DataSourceRequest] DataSourceRequest request)
        {
            var roleTypesList = _roleClient.GetRoleTypesAsync().Result;
            return Json(roleTypesList, JsonRequestBehavior.AllowGet);
        }

        [NoCache]
        public JsonResult GetRoleTypesExceptAdminAsync([DataSourceRequest] DataSourceRequest request)
        {
            var roleTypesList = _roleClient.GetRoleTypesAsync().Result;
            roleTypesList = roleTypesList.Where(_ => _.Name != DTO.Enums.Role.Admin.ToString()).ToList();
            return Json(roleTypesList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  Method to create the role 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> Role(RoleViewModel roleViewModel)
        {
            roleViewModel.LoginUserId = UserId;
            roleViewModel = await _roleClient.RoleAsync(roleViewModel);
            if (roleViewModel.PostStatus)
            {
                TempData["RoleViewModel"] = roleViewModel;
                return RedirectToAction("Role");
            }
            return View(roleViewModel);
        }
        /// <summary>
        ///  Method to get claim details 
        /// </summary>
        /// <param name="roleTypeId"></param>
        /// <returns></returns>
        [NoCache]
        public JsonResult GetRoleTypeClaimsDetails(int roleTypeId)
        {
            var claimsDetailsList = _roleClient.GetRoleTypeClaimsDetailsAsync(roleTypeId).Result;
            return Json(claimsDetailsList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  Method to get the update role type
        /// </summary>
        /// <returns></returns>
        //[AuthorizeClaims(Claims.ManageRoles)]
        //[AuthorizeRoles(Adani.Solution.DTO.Enums.Role.Admin)]
        [AuthorizeClaims(Claims.ManageRoles)]
        public async Task<ActionResult> UpdateRole()
        {
            var roleClaimUpdateViewModel = new RoleClaimUpdateViewModel();
            roleClaimUpdateViewModel.RoleClaimViewDto = await _roleClient.GetRoleClaimsAsync();
            if (TempData["RoleClaimUpdateViewModel"] != null)
            {
                var roleTypeClaim = TempData["RoleClaimUpdateViewModel"] as RoleClaimUpdateViewModel;
                roleClaimUpdateViewModel.PostMessage = roleTypeClaim.PostMessage;
                roleClaimUpdateViewModel.PostStatus = true;
            }
            return View(roleClaimUpdateViewModel);
        }

        /// <summary>
        ///  Method to update the role 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> UpdateRole(RoleClaimUpdateViewModel roleClaimUpdateViewModel)
        {
            if (!roleClaimUpdateViewModel.IsSearch)
            {
                roleClaimUpdateViewModel = await _roleClient.UpdateRoleAsync(roleClaimUpdateViewModel, UserId);
                if (roleClaimUpdateViewModel.PostStatus)
                {
                    var claimDetails = await _roleClient.GetClaimDetailsByIdAsync(UserId);
                    if (claimDetails != null && claimDetails.Where(_ => _.IsApplied).Any())
                    {
                        Session["UserClaims"] = claimDetails;
                    }
                    TempData["RoleClaimUpdateViewModel"] = roleClaimUpdateViewModel;
                    return RedirectToAction("UpdateRole");
                }
                else
                {
                    roleClaimUpdateViewModel.RoleClaimViewDto = await _roleClient.GetRoleClaimsAsync();
                }
            }
            else
            {
                roleClaimUpdateViewModel.SearchText = (roleClaimUpdateViewModel.SearchText != null) ? roleClaimUpdateViewModel.SearchText : string.Empty;
                roleClaimUpdateViewModel.RoleClaimViewDto = await _roleClient.GetRoleClaimsAsync();
                roleClaimUpdateViewModel.RoleClaimViewDto.RoleClaimsAndRoleTypeClaims = roleClaimUpdateViewModel.RoleClaimViewDto.RoleClaimsAndRoleTypeClaims.OrderBy(m => m.RoleName.ToLower()
                          .Contains(roleClaimUpdateViewModel.SearchText.ToLower()) ? 0 : 1).ThenBy(m => m.RoleName).ToList();
            }
            return View(roleClaimUpdateViewModel);
        }

        /// <summary>
        /// Method to role delete and redirection
        /// </summary>
        /// <param name="roleIdDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public async Task<ActionResult> RoleDelete(int roleId)
        {

            var roleIdDto = new RoleIdDto
            {
                RoleId = roleId,
                LoginUserId = UserId
            };
            var roleViewModel = await _roleClient.RoleDeleteAsync(roleIdDto);
            if (roleViewModel.PostStatus)
            {
                TempData["RoleClaimUpdateViewModel"] = roleViewModel;
            }
            return Json(roleViewModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region OrganizationHierarchy

        /// <summary>
        ///  Method to get the update organization hierarchy
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageRoles)]
        //[AuthorizeRoles(Adani.Solution.DTO.Enums.Role.Admin)]
        public ActionResult OrganizationHierarchy()
        {
            var roleTypeHierarchyViewModel = new RoleTypeHierarchyViewModel();
            if (TempData["RoleTypeHierarchyViewModel"] != null)
            {
                var roleTypeClaim = TempData["RoleTypeHierarchyViewModel"] as RoleTypeHierarchyViewModel;
                roleTypeHierarchyViewModel.PostMessage = roleTypeClaim.PostMessage;
                roleTypeHierarchyViewModel.PostStatus = true;
            }
            return View(roleTypeHierarchyViewModel);
        }

        /// <summary>
        ///  Method to update the organization hierarchy 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<JsonResult> OrganizationHierarchy(List<int> roleTypeDto)
        {
            var roleTypeHierarchyViewModel = await _roleClient.OrganizationHierarchyAsync(roleTypeDto);
            return Json(roleTypeHierarchyViewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get organization hierarchy list
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetOrgHierarchyListAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _roleClient.GetRoleTypesAsync();
            var roleTypeDto = result.ConvertAll(x => new RoleTypeHierarchyModel { Id = x.Id, Description = x.Description, Name = x.Name, LevelNo = x.LevelNo });

            var gridOutputList = roleTypeDto.OrderBy(x => x.LevelNo).ToDataSourceResult(request);
            return Json(gridOutputList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get hierarchy details
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageRoles)]
        //[AuthorizeRoles(Adani.Solution.DTO.Enums.Role.Admin)]
        public ActionResult HierarchyChart()
        {
            var roleTypeHierarchyViewModel = new RoleTypeHierarchyViewModel();
            return View(roleTypeHierarchyViewModel);
        }

        /// <summary>
        /// Method to get hierarchy details
        /// </summary>
        /// <returns></returns>
        public ActionResult HierarchyChartPop()
        {
            var roleTypeHierarchyViewModel = new RoleTypeHierarchyViewModel();
            return PartialView(roleTypeHierarchyViewModel);
        }

        /// <summary>
        /// Method to get hierarchy details
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetHierarchyChartAsync()
        {
            var result = await _roleClient.GetHierarchyChartAsync();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [NoCache]
        public JsonResult GetRolesAsync([DataSourceRequest] DataSourceRequest request)
        {
            var roleTypesList = _roleClient.GetRolesAsync().Result;
            return Json(roleTypesList, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetReportingToRoles([DataSourceRequest] DataSourceRequest request, long roleId)
        {
            var reportinRoles = new List<DropDownDto>();
            if (roleId > 0)
            {
                var idInputDto = new IdInputDto();
                idInputDto.Id = roleId;
                reportinRoles = await _roleClient.GetReportingToRoles(idInputDto);
            }
            return Json(reportinRoles, JsonRequestBehavior.AllowGet);
        }

        #region RoleHierarchy

        /// <summary>
        ///  Method to get the update RoleHierarchy
        /// </summary>
        /// <returns></returns>
        //[AuthorizeClaims(Claims.ManageOrganization)]
        //[AuthorizeRoles(Adani.Solution.DTO.Enums.Role.Admin)]
        [AuthorizeClaims(Claims.ManageRoles)]
        public ActionResult RoleHierarchy()
        {
            var RoleTypeHierarchyViewModel = new RoleHierarchyViewModel();
            if (TempData["RoleHierarchyViewModel"] != null)
            {
                var roleTypeClaim = TempData["RoleHierarchyViewModel"] as RoleHierarchyViewModel;
                RoleTypeHierarchyViewModel.PostMessage = roleTypeClaim.PostMessage;
                RoleTypeHierarchyViewModel.PostStatus = true;
            }
            return View(RoleTypeHierarchyViewModel);
        }

        /// <summary>
        ///  Method to update the RoleHierarchy 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<JsonResult> RoleHierarchy(List<int> roleTypeDto )
        {
            var roleTypeHierarchyViewModel = await _roleClient.RoleHierarchyAsync(roleTypeDto, UserId);
            return Json(roleTypeHierarchyViewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get RoleHierarchy list
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetRoleHierarchyListAsync([DataSourceRequest] DataSourceRequest request /* , int processId = 0, int verticalId = 0 */)
        {
            var inputDto = new RoleHierarchyParamDto { /* ProcessId = processId, VerticalId = verticalId */};
            var result = await _roleClient.GetRoleHierarchyByProcess(inputDto);
            var roleTypeDto = result.ConvertAll(x => new RoleHierarchyModel { Id = x.Id, Description = x.Description, Name = x.Name, LevelNo = x.LevelNo });

            var gridOutputList = roleTypeDto.OrderBy(x => x.LevelNo).ToDataSourceResult(request);
            return Json(gridOutputList, JsonRequestBehavior.AllowGet);
        }


        [AuthorizeClaims(Claims.ManageOrganization)]
        public async Task<ActionResult> GetOrganizationReportingToUsers([DataSourceRequest] DataSourceRequest request, long roleId,
      string divisionId, string salesOrganizationId, string distributionChannelId)
        {
            var reportinRoles = new List<DropDownDto>();
            var result = new List<object>();
            //var verticalIds = new List<long>();
            if (roleId > 0 && divisionId != string.Empty && salesOrganizationId != string.Empty && distributionChannelId != string.Empty)
            {
                var verticalIds = GMCore.Helper.UtilityHelper.ConvertStringToLongList(divisionId);
                var salesOrganizationIds = GMCore.Helper.UtilityHelper.ConvertStringToLongList(salesOrganizationId);
                var distributionChannelIds = GMCore.Helper.UtilityHelper.ConvertStringToLongList(distributionChannelId);
                //string encryptionKey = ConfigurationManager.AppSettings["EncryptionKey"];
                //string vectorKey = ConfigurationManager.AppSettings["VectorKey"];

                var inputDto = new ReportingUsersInputDto
                {
                    RoleId = roleId,
                    ProcessId = (int)DTO.Enums.HierarchyProcess.Organization,
                    DivisionIds = verticalIds,
                    SalesOrganizationIds = salesOrganizationIds,
                    DistributionChannelIds = distributionChannelIds,
                };
                var fullList = await _roleClient.GetReportingToUsersByRole(inputDto);

                if (fullList != null && fullList.Any())
                {
                    result = fullList.Select(x => new
                    {
                        //Id = EncryptDecryptHelper.Encrypt(x.Id.ToString(), encryptionKey, vectorKey), // CHANGED
                        Id= x.Id,
                        Name = x.Name
                    }).ToList<object>();

                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetSalesReportingToUsers([DataSourceRequest] DataSourceRequest request, long roleId, string verticalId)
        {
            var reportinRoles = new List<DropDownDto>();
            
            var verticalIds=GMCore.Helper.UtilityHelper.ConvertStringToLongList(verticalId);
            if (roleId > 0 && verticalId != string.Empty)
            {
               
                var inputDto = new ReportingUsersInputDto { RoleId = roleId, ProcessId = (int)DTO.Enums.HierarchyProcess.Sales, DivisionIds = verticalIds };
                reportinRoles = await _roleClient.GetReportingToUsersByRole(inputDto);
            }
            return Json(reportinRoles, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetSpecialityFatReportingToUsers([DataSourceRequest] DataSourceRequest request, long roleId)
        {
            var reportinRoles = new List<DropDownDto>();
            if (roleId > 0)
            {
                var inputDto = new ReportingUsersInputDto { RoleId = roleId, ProcessId = (int)DTO.Enums.HierarchyProcess.SpecialityFat };
                reportinRoles = await _roleClient.GetReportingToUsersByRole(inputDto);
            }
            return Json(reportinRoles, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetCMSReportingToUsers([DataSourceRequest] DataSourceRequest request, long roleId)
        {
            var reportinRoles = new List<DropDownDto>();
            if (roleId > 0)
            {
                var inputDto = new ReportingUsersInputDto { RoleId = roleId, ProcessId = (int)DTO.Enums.HierarchyProcess.ComplaintManagementSystem, VerticalId = (int)DTO.Enums.Division.SpecialityFat };
                reportinRoles = await _roleClient.GetReportingToUsersByRole(inputDto);
            }
            return Json(reportinRoles, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Reporting to users

        public async Task<ActionResult> GetReportingToUsersByLoggedInUserId([DataSourceRequest] DataSourceRequest request)
        {
            var inputDto = new ReportingUsersInputDto { LoginUserId = UserId };
            var reportinRoles = await _roleClient.GetOrganizationReportingToUsersByUserId(inputDto);
            return Json(reportinRoles, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetSalesReportingToUsersByLoggedInUserId([DataSourceRequest] DataSourceRequest request)
        {
            var inputDto = new ReportingUsersInputDto { LoginUserId = UserId, RoleId = RoleId };
            var reportinRoles = await _roleClient.GetSalesReportingToUsersByUserId(inputDto);
            return Json(reportinRoles, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetSalesReportingToUsersByCityId([DataSourceRequest] DataSourceRequest request, long cityId)
        {
            var reportinRoles = new List<DropDownDto>();
            if (cityId > 0)
            {
                var inputDto = new ReportingUsersInputDto { LoginUserId = UserId, RoleId = RoleId, CityId = cityId };
                reportinRoles = await _roleClient.GetSalesReportingToUsersByCityId(inputDto);
            }
            return Json(reportinRoles, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetSalesReportingToUsersByCityStateDistrict([DataSourceRequest] DataSourceRequest request, long cityId,long districtId,long stateId)
        {
            var reportinRoles = new List<DropDownDto>();
            if (cityId > 0 || stateId>0 || districtId >0)
            {
                var inputDto = new ReportingUsersInputDto { LoginUserId = UserId, RoleId = RoleId, CityId = cityId ,DistrictId= (int)districtId,StateId=stateId};
                reportinRoles = await _roleClient.GetSalesReportingToUsersByCityStateDistrict(inputDto);
            }
            return Json(reportinRoles, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetReportingToZonalHeadUsersByUserId([DataSourceRequest] DataSourceRequest request, long verticalId)
        {
            var reportingToUsers = new List<DropDownDto>();
            if (verticalId > 0)
            {
                var inputDto = new ReportingUsersInputDto { LoginUserId = UserId, VerticalId = verticalId };
                reportingToUsers = await _roleClient.GetReportingToZonalHeadUsersByUserId(inputDto);
            }
            return Json(reportingToUsers, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetReportingToBDOUsersByUserId([DataSourceRequest] DataSourceRequest request, long userId, long verticalId)
        {
            var reportingToUsers = new List<DropDownDto>();
            if (userId > 0 && verticalId > 0)
            {
                var inputDto = new ReportingUsersInputDto { UserId = userId, VerticalId = verticalId };
                reportingToUsers = await _roleClient.GetReportingToBDOUsersByUserId(inputDto);
            }
            return Json(reportingToUsers, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetReportingToRABDOUsersByUserId([DataSourceRequest] DataSourceRequest request, long userId, long verticalId)
        {
            var reportingToUsers = new List<DropDownDto>();
            if (userId > 0 && verticalId > 0)
            {
                var inputDto = new ReportingUsersInputDto { UserId = userId, VerticalId = verticalId };
                reportingToUsers = await _roleClient.GetReportingToRABDOUsersByUserId(inputDto);
            }
            return Json(reportingToUsers, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}