using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
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
    [RoutePrefix("api/raVersionTwo")]
    public class RAVersionTwoController : BaseApiController
    {
        private const string ServiceName = "RAVersionTwo Controller";
        private readonly IRAVersionTwoService _raNewVersionService;
        private string _methodName;

        public RAVersionTwoController(IRAVersionTwoService raNewVersionService) : base(ServiceName)
        {
            _raNewVersionService = raNewVersionService;
        }
               

        #region SchemeDiscount - GeographyBased 

        /// <summary>
        /// Method to Save Geography Based SchemeDiscount
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GeographyBasedSchemeDiscount/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveSchemeDiscountGeography", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult SaveSchemeDiscountGeography([FromBody]string inputKey)
        {
            _methodName = "SaveSchemeDiscountGeography";
            return Result(inputKey, _methodName, (SchemeDiscountGeographyDto s) => { return _raNewVersionService.SaveSchemeDiscountGeography(s); });
        }

        /// <summary>
        /// Method to Update Geography Based SchemeDiscount
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GeographyBasedSchemeDiscount/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateSchemeDiscountGeography", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult UpdateSchemeDiscountGeography([FromBody]string inputKey)
        {
            _methodName = "UpdateSchemeDiscountGeography";
            return Result(inputKey, _methodName, (SchemeDiscountGeographyDto s) => { return _raNewVersionService.UpdateSchemeDiscountGeography(s); });
        }

        /// <summary>
        /// Method to Get Geography Based SchemeDiscount List With Pagination
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GeographyBasedSchemeDiscount/listwithpagination")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetGeographyBasedSchemeDiscountListWithPagination", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetGeographyBasedSchemeDiscountListWithPagination([FromBody]string inputKey)
        {
            _methodName = "GetGeographyBasedSchemeDiscountListWithPagination";
            return KendoGridResult(inputKey, _methodName, (KendoGridResult x) => { return _raNewVersionService.GetGeographyBasedSchemeDiscountListWithPagination(x); });
        }

        /// <summary>
        /// Method to Get SchemeDiscount Geography Details By Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GeographyBasedSchemeDiscount/geographyMappingList/discountId")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSchemeDiscountGeographyHierarchyListById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSchemeDiscountGeographyHierarchyListById([FromBody]string inputKey)
        {
            //_methodName = "GetSchemeDiscountGeographyHierarchyListById";
            //return Result(inputKey, _methodName, (ListInputDto inputDto) => { return _raNewVersionService.GetSchemeDiscountGeographyHierarchyListById(inputDto); });

            return KendoGridResult(inputKey, _methodName, (KendoGridResult inputDto) => { return _raNewVersionService.GetSchemeDiscountGeographyHierarchyListById(inputDto); });
        }

        /// <summary>
        /// Method to Get Geography Based SchemeDiscount By Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GeographyBasedSchemeDiscount/get/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetGeographyBasedSchemeDiscountById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetGeographyBasedSchemeDiscountById([FromBody]string inputKey)
        {
            _methodName = "GetGeographyBasedSchemeDiscountById";
            return Result(inputKey, _methodName, (long id) => { return _raNewVersionService.GetGeographyBasedSchemeDiscountById(id); });
        }

        /// <summary>
        /// Method to Export Geography Based SchemeDiscount
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GeographyBasedSchemeDiscount/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportGeographyBasedSchemeDiscount", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportGeographyBasedSchemeDiscount([FromBody]string inputKey)
        {
            _methodName = "ExportGeographyBasedSchemeDiscount";
            return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _raNewVersionService.ExportGeographyBasedSchemeDiscount(x); });
        }
        [HttpPost]
        [Route("UpdateSchemeDiscountGeographyList/ByIsActive")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateSchemeDiscountGeographyListByIsActive", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateSchemeDiscountGeographyListByIsActivee([FromBody]string inputKey)
        {
            _methodName = "UpdateSchemeDiscountGeographyListByIsActive";
            return Result(inputKey, _methodName, (IdDiscountAndBenefitInputDto x) => { return _raNewVersionService.UpdateSchemeDiscountGeographyListByIsActive(x); });
        }
        #endregion

        #region Reporting To Users - Customer Group

        [HttpPost]
        [Route("reportingToRAZonalHeadUsers/customerGroupId")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetReportingToRAZonalHeadUsersByCustomerGroup", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetReportingToRAZonalHeadUsersByCustomerGroup([FromBody]string inputKey)
        {
            _methodName = "GetReportingToRAZonalHeadUsersByCustomerGroup";
            return Result(inputKey, _methodName, (CustomerGroupInputDto x) => { return _raNewVersionService.GetRAZonalHeadUsersByCustomerGroup(x); });
        }

        [HttpPost]
        [Route("RAZonalHeadUsers/customerGroupIdsAndVerticalIds")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetRAZonalHeadUsersByCustomerGroupIdsAndVerticalIds", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetRAZonalHeadUsersByCustomerGroupIdsAndVerticalIds([FromBody]string inputKey)
        {
            _methodName = "GetRAZonalHeadUsersByCustomerGroupIdsAndVerticalIds";
            return Result(inputKey, _methodName, (DropDownInputDto x) => { return _raNewVersionService.GetRAZonalHeadUsersByCustomerGroupIdsAndVerticalIds(x); });
        }

        [HttpPost]
        [Route("RABDOUsers/ZonalHeadIdsAndVerticalIds")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetRABDOUsersByZonalHeadIdsAndVerticalIds", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetRABDOUsersByZonalHeadIdsAndVerticalIds([FromBody]string inputKey)
        {
            _methodName = "GetRABDOUsersByZonalHeadIdsAndVerticalIds";
            return Result(inputKey, _methodName, (DropDownInputDto x) => { return _raNewVersionService.GetRABDOUsersByZonalHeadIdsAndVerticalIds(x); });
        }

        /// <summary>
        /// Method to Get Customer List By Customer Group Id And StateTrader For Dropdown
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("customer/ddl/customerGroupIdsAndBdoIds")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCustomerListByCustomerGroupIdsAndBDOsForDropdown", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCustomerListByCustomerGroupIdsAndBDOsForDropdown([FromBody]string inputKey)
        {
            _methodName = "GetCustomerListByCustomerGroupIdsAndBDOsForDropdown";
            return KendoGridResult(inputKey, _methodName, (DropDownInputDto x) => { return _raNewVersionService.GetCustomerListByCustomerGroupIdsAndBDOsForDropdown(x); });
        }

        /// <summary>
        /// Method to Get Customer List By Customer Group Id And City Ids For Dropdown
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("customer/ddl/customerGroupIdsAndCityIds")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCustomerListByCustomerGroupIdsCityIdsForDropdown", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCustomerListByCustomerGroupIdsCityIdsForDropdown([FromBody]string inputKey)
        {
            _methodName = "GetCustomerListByCustomerGroupIdsCityIdsForDropdown";
            return KendoGridResult(inputKey, _methodName, (DropDownInputDto x) => { return _raNewVersionService.GetCustomerListByCustomerGroupIdsCityIdsForDropdown(x); });
        }

        #endregion

  
    }
}
