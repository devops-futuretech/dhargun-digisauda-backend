using System;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net;
using System.Collections.Generic;
using GMCore.Logger;
using GMCore.Authenticate;
using GMCore.Helper;
using Adani.Solution.DTO;
using Adani.Solution.Service;
using Adani.Solution.Service.Common;
using Adani.Solution.DTO.Common;
using Adani.Solution.API.App_Start;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/employees")]
    public class EmployeeController : BaseApiController
    {
        private const string ServiceName = "Employee Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IEmployeeService _employeeService;
        private string _methodName;

        public EmployeeController(IEmployeeService employeeService) : base(ServiceName)
        {
            _methodName = "Employee Controller";
            try
            {
                _employeeService = employeeService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        

        #region User

        /// <summary>
        /// Method to Save User
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveUser", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult SaveUser([FromBody]string inputKey)
        {
            _methodName = "SaveUser";
            return Result(inputKey, _methodName, (EmployeeDto x) => { return _employeeService.SaveUser(x); });
        }

        /// <summary>
        /// Method to Get User Master List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserMasterList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserMasterList([FromBody]string inputKey)
        {
            _methodName = "GetUserMasterList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _employeeService.GetUserMasterList(x); });
        }

        /// <summary>
        /// Method to Get User Claims
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/claims")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserRoleClaims", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserRoleClaims([FromBody]string inputKey)
        {
            _methodName = "GetUserRoleClaims";
            return Result(inputKey, _methodName, (UserIdDto x) => { return _employeeService.GetUserRoleClaims(x); });
        }

        /// <summary>
        /// Method to get Get User Details By Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get/userid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserDetailsById([FromBody]string inputKey)
        {
            _methodName = "GetUserDetailsById";
            return Result(inputKey, _methodName, (string x) => { return _employeeService.GetUserDetailsById(x); });
        }

        /// <summary>
        /// Method to Get User Master List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserDetails([FromBody]string inputKey)
        {
            _methodName = "GetUserDetails";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _employeeService.GetUserDetails(x); });
        }

        /// <summary>
        /// Method to Update User
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateUser", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult UpdateUser([FromBody]string inputKey)
        {
            _methodName = "UpdateUser";
            return Result(inputKey, _methodName, ((EmployeeDto inputDto) => { return _employeeService.UpdateUser(inputDto); }));
        }

        [HttpPost]
        [Route("profileupload")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ProfileUpload", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult ProfileUpload([FromBody] string inputKey)
        {
            _methodName = "ProfileUpload";
            return Result(inputKey, _methodName, ((EmployeeDto inputDto) => { return _employeeService.ProfileUpload(inputDto); }));
        }
        #endregion

        #region Dealer

        /// <summary>
        /// Method to Get Dealer List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("dealer/list/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerListExcelExport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerListExcelExport([FromBody]string inputKey)
        {
            _methodName = "GetDealerListExcelExport";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _employeeService.GetDealerList(x); });            
        }

        /// <summary>
        /// Method to Get Dealer List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("dealer/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerList([FromBody]string inputKey)
        {
            _methodName = "GetDealerList";
            //return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _employeeService.GetDealerList(x); });
            return KendoGridResult(inputKey, _methodName, (KendoGridResult x) => { return _employeeService.GetDealerListWithPaging(x); });
        }

        /// <summary>
        /// Method to get Get Dealer Details By Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get/dealerid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerDetailsById([FromBody]string inputKey)
        {
            _methodName = "GetDealerDetailsById";
            return Result(inputKey, _methodName, (string x) => { return _employeeService.GetDealerDetailsById(x); });
        }


        /// <summary>
        /// Method to delete ConsentImage
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("consentimage/delete")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DeleteConsentImage", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DeleteConsentImage([FromBody] string inputKey)
        {
            _methodName = "DeleteConsentImage";
            return Result(inputKey, _methodName, (BulletinInputDto x) => { return _employeeService.DeleteConsentImage(x); });
        }

        [HttpPost]
        [Route("uploadconsentimage")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UploadConsentImage", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UploadConsentImage([FromBody] string inputKey)
        {
            _methodName = "UploadConsentImage";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            return Result(inputKey, _methodName, (List<DealerConsentImageUploadDto> x) => { return _employeeService.UploadConsentImage(x); });
        }
        #endregion

        #region Broker

        /// <summary>
        /// Method to Get Broker List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("broker/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBrokerList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBrokerList([FromBody]string inputKey)
        {
            _methodName = "GetBrokerList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _employeeService.GetBrokerList(x); });
        }

        /// <summary>
        /// Method to get Get Broker Details By Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get/brokerid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBrokerDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBrokerDetailsById([FromBody]string inputKey)
        {
            _methodName = "GetBrokerDetailsById";
            return Result(inputKey, _methodName, (string x) => { return _employeeService.GetBrokerDetailsById(x); });
        }

        /// <summary>
        /// Method to Get Broker List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>  
        [HttpPost]
        [Route("broker/ddl")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBrokerListddl", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBrokerListddl([FromBody]string inputKey)
        {
            _methodName = "GetBrokerListddl";
            return Result(inputKey, _methodName, (DealerBrokerParamDto x) => { return _employeeService.GetBrokerListddl(x); });
        }
        #endregion    


        /// <summary>
        /// Method to Get User Master List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("bdo/statistics")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBDOStatistics", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBDOStatistics([FromBody]string inputKey)
        {
            _methodName = "GetBDOStatistics";
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _employeeService.GetBDOStatistics(x); });
        }

        /// <summary>
        /// Method to Key Performance Indicator 
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("kpi")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetKeyPerformanceIndicator", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetKeyPerformanceIndicator([FromBody]string inputKey)
        {
            _methodName = "GetKeyPerformanceIndicator";
            return Result(inputKey, _methodName, ((IdInputDto inputDto) => { return _employeeService.GetKeyPerformanceIndicator(inputDto); }));
        }

        #region UserTarget
        /// <summary>
        /// Method to Get Reporting To by user 
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("usertarget/UserAssignedTo")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserAssignedTo", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserAssignedTo([FromBody]string inputKey)
        {
            _methodName = "GetUserAssignedTo";
            return Result(inputKey, _methodName, ((IdInputDto inputDto) => { return _employeeService.GetUserAssignedTo(inputDto); }));
        }

        /// <summary>
        /// Method to add user target
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("usertarget/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddUserTarget", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddUserTarget([FromBody]string inputKey)
        {
            _methodName = "AddUserTarget";
            return Result(inputKey, _methodName, ((AddUserTargetDto inputDto) => { return _employeeService.AddUserTarget(inputDto); }));
        }

        /// <summary>
        /// Method to update user target 
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("usertarget/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateUserTarget", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateUserTarget([FromBody]string inputKey)
        {
            _methodName = "UpdateUserTarget";
            return Result(inputKey, _methodName, ((UpdateUserTargetDto inputDto) => { return _employeeService.UpdateUserTarget(inputDto); }));
        }

        /// <summary>
        /// Method to user target list
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("usertarget/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserTargetList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserTargetList([FromBody]string inputKey)
        {
            _methodName = "GetUserTargetList";
            return Result(inputKey, _methodName, ((LoginUserIdDto inputDto) => { return _employeeService.GetUserTargetList(inputDto); }));
        }

        /// <summary>
        /// Method to user target view
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("usertarget/view")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserTargetById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserTargetById([FromBody]string inputKey)
        {
            _methodName = "GetUserTargetById";
            return Result(inputKey, _methodName, ((IdInputDto inputDto) => { return _employeeService.GetUserTargetById(inputDto); }));
        }
        #endregion

        /// <summary>
        /// Method to get Get Users By Role
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("users/byrole")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUsersByRole", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUsersByRole([FromBody]string inputKey)
        {
            _methodName = "GetUsersByRole";
            return Result(inputKey, _methodName, ((IdInputDto inputDto) => { return _employeeService.GetUsersByRole(inputDto); }));
        }

        [HttpPost]
        [Route("chart/SaudaSales")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ChartSaudaAndSalesDetailsByOilTypes", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ChartSaudaAndSalesDetailsByOilTypes([FromBody]string inputKey)
        {
            _methodName = "ChartSaudaAndSalesDetailsByOilTypes";
            return Result(inputKey, _methodName, (ChartSaudaSalesByOilTypeInputDto x) => { return _employeeService.ChartSaudaAndSalesDetailsByOilTypes(x); });
        }

        [HttpPost]
        [Route("chart/SaudaApproval")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ChartSaudaApprovalDetailsByOilTypes", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ChartSaudaApprovalDetailsByOilTypes([FromBody]string inputKey)
        {
            _methodName = "ChartSaudaApprovalDetailsByOilTypes";
            return Result(inputKey, _methodName, (ChartSaudaSalesByOilTypeInputDto x) => { return _employeeService.ChartSaudaApprovalDetailsByOilTypes(x); });
        }

        [HttpPost]
        [Route("ZonalTrader/bdolist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBDOListByZonalHead", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBDOListByZonalHead([FromBody]string inputKey)
        {
            _methodName = "GetBDOListByZonalHead";
            return Result(inputKey, _methodName, ((IdInputDto inputDto) => { return _employeeService.GetBDOListByZonalHead(inputDto); }));
        }

        /// <summary>
        /// Method to Get User Master List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/excelexport")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserExcelExportList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserExcelExportList([FromBody]string inputKey)
        {
            _methodName = "GetUserExcelExportList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _employeeService.GetUserExcelExportList(x); });
        }

        #region ShipToParty

        /// <summary>
        /// Method to Get ShipToParty List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("shipToParty/list/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetShipToPartyListExcelExport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetShipToPartyListExcelExport([FromBody]string inputKey)
        {
            _methodName = "GetShipToPartyListExcelExport";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _employeeService.GetShipToPartyListExcelExport(x); });
        }

        /// <summary>
        /// Method to Get ShipToParty List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("shipToParty/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetShipToPartyList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetShipToPartyList([FromBody]string inputKey)
        {
            _methodName = "GetShipToPartyList";
            return KendoGridResult(inputKey, _methodName, (KendoGridResult x) => { return _employeeService.GetShipToPartyListWithPaging(x); });
        }

        /// <summary>
        /// Method to get Get ShipToParty Details By Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get/shipToPartyid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetShipToPartyDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetShipToPartyDetailsById([FromBody]string inputKey)
        {
            _methodName = "GetShipToPartyDetailsById";
            return Result(inputKey, _methodName, (string x) => { return _employeeService.GetShipToPartyDetailsById(x); });
        }

        #endregion

        [HttpPost]
        [Route("StateTrader/statistics1")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBDOStatistics", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBDOStatistics1(SaudaFilterDto inputKey)
        {
            _methodName = "GetBDOStatistics";
            //return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _employeeService.GetBDOStatistics(x); });
            var result = new ResultDto();
            //return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _mobileDashboardServices.DueForTomorrowList(x); });
            result = _employeeService.GetBDOStatistics(inputKey);
            return Ok(result);
        }

        [HttpPost]
        [Route("kpi1")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetKeyPerformanceIndicator", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetKeyPerformanceIndicator1(IdInputDto inputKey)
        {
            _methodName = "GetKeyPerformanceIndicator";
            //return Result(inputKey, _methodName, ((IdInputDto inputDto) => { return _employeeService.GetKeyPerformanceIndicator(inputDto); }));
            var result = new ResultDto();
            result = _employeeService.GetKeyPerformanceIndicator(inputKey);
            return Ok(result);
        }

        #region Get Dealer List with pagination

        /// <summary>
        /// Method to Get Dealer List with pagination
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("dealer/listwithpagination")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerListWithPagination", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerListWithPagination([FromBody]string inputKey)
        {
            _methodName = "GetDealerListWithPagination";
            return Result(inputKey, _methodName, (DealerListInputDto x) => { return _employeeService.GetDealerListWithPagination(x); });
        }

        #endregion

        #region Dealer, Broker, ShipToParty - Admin App

        [HttpPost]
        [Route("dealer/listwithpagination/adminapp")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerListWithPaginationAdminApp", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerListWithPaginationAdminApp([FromBody]string inputKey)
        {
            _methodName = "GetDealerListWithPaginationAdminApp";
            return Result(inputKey, _methodName, (DealerListInputDto x) => { return _employeeService.GetDealerListWithPaginationAdminApp(x); });
        }

        [HttpPost]
        [Route("broker/listwithpagination/adminapp")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBrokerListWithPaginationAdminApp", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBrokerListWithPaginationAdminApp([FromBody]string inputKey)
        {
            _methodName = "GetBrokerListWithPaginationAdminApp";
            return Result(inputKey, _methodName, (DealerListInputDto x) => { return _employeeService.GetBrokerListWithPaginationAdminApp(x); });
        }

        [HttpPost]
        [Route("shiptparty/listwithpagination/adminapp")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetShipToPartyListWithPaginationAdminApp", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetShipToPartyListWithPaginationAdminApp([FromBody]string inputKey)
        {
            _methodName = "GetShipToPartyListWithPaginationAdminApp";
            return Result(inputKey, _methodName, (DealerListInputDto x) => { return _employeeService.GetShipToPartyListWithPaginationAdminApp(x); });
        }

        [HttpPost]
        [Route("user/listwithpagination/adminapp")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserListWithPaginationAdminApp", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserListWithPaginationAdminApp([FromBody]string inputKey)
        {
            _methodName = "GetUserListWithPaginationAdminApp";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _employeeService.GetUserListWithPaginationAdminApp(x); });
        }

        [HttpPost]
        [Route("DealerAndBroker/list/Vertical")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerAndBrokerList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerAndBrokerList([FromBody]string inputKey)
        {
            _methodName = "GetDealerAndBrokerList";
            return Result(inputKey, _methodName, (DealerBrokerParamDto x) => { return _employeeService.GetDealerAndBrokerList(x); });
        }

        [HttpPost]
        [Route("Dealer/list/Vertical")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerListByVertical", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerListByVertical([FromBody]string inputKey)
        {
            _methodName = "GetDealerListByVertical";
            return Result(inputKey, _methodName, (DealerBrokerParamDto x) => { return _employeeService.GetDealerListByVertical(x); });
        }

        [HttpPost]
        [Route("ShipToParty/list/Vertical")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetShipToPartyListBasedOnVertical", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetShipToPartyListBasedOnVertical([FromBody]string inputKey)
        {
            _methodName = "GetShipToPartyListBasedOnVertical";
            return Result(inputKey, _methodName, (DealerBrokerParamDto x) => { return _employeeService.GetShipToPartyListBasedOnVertical(x); });
        }

        #endregion

        #region Dashboard Portal

        [HttpPost]
        [Route("dashboard/details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDashboardDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDashboardDetails([FromBody] string inputKey)
        {
            _methodName = "GetDashboardDetails";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _employeeService.GetDashboardDetails(x); });
        }

        
        [HttpPost]
        [Route("DistributorUserLogin/Info")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDashboardUserInfo", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDashboardUserInfo([FromBody] string inputKey)
        {
            _methodName = "GetDashboardUserInfo";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _employeeService.GetDashboardUserInfo(x); });
        }

        [HttpPost]
        [Route("SalesLogin/Info")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDashboardSalesUserInfo", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDashboardSalesUserInfo([FromBody] string inputKey)
        {
            _methodName = "GetDashboardSalesUserInfo";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _employeeService.GetDashboardSalesUserInfo(x); });
        }

        #endregion
    }
}
