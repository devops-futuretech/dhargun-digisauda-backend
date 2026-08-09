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
    [RoutePrefix("api/lookups")]
    public class LookupController : BaseApiController
    {
        private const string ServiceName = "Lookup Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly ILookupService _lookupService;
        private string _methodName;

        public LookupController(ILookupService lookupService) : base(ServiceName)
        {
            _methodName = "Lookup Controller";
            try
            {
                _lookupService = lookupService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        #region Lookup

        /// <summary>
        /// Method to Get State List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("state/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetStateList()
        {
            _methodName = "GetStateList";
            return Result(_methodName, () => { return _lookupService.GetStateList(); });
        }

        /// <summary>
        /// Method to Get State List by employee ids
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("statebyemployeid/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetStateListByEmployeeIds([FromBody] string inputKey)
        {
            _methodName = "GetStateListByEmployeeIds";
            return Result(inputKey, _methodName, (LoginUserIdDto input) => { return _lookupService.GetStateListByEmployees(input); });
        }

        [HttpGet]
        [Route("active/state/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetActiveStateList()
        {
            _methodName = "GetActiveStateList";
            return Result(_methodName, () => { return _lookupService.GetActiveStateList(); });
        }

        [HttpPost]
        [Route("active/state/user")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetUserState([FromBody] string inputKey)
        {
            _methodName = "GetUserState";
            
            return Result(inputKey, _methodName, (IdInputDto input) => { return _lookupService.GetActiveUserState(input); });
        }

        [HttpGet]
        [Route("active/state/city/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetActiveStateCityList()
        {
            _methodName = "GetActiveStateCityList";
            return Result(_methodName, () => { return _lookupService.GetActiveStateCityList(); });
        }



        [HttpPost]
        [Route("active/statelist/ZonalHeadId")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetActiveStateListBasedOnZonalHeadIds([FromBody]string inputKey)
        {
            _methodName = "GetActiveStateListBasedOnZonalHeadIds";
            return Result(inputKey,_methodName, (List<long> zonalHeadIds) => { return _lookupService.GetActiveStateListBasedOnZonalHeadIds(zonalHeadIds); });
        }
        [HttpPost]
        [Route("active/oiltype/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetActiveOilTypeList", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetActiveOilTypeList([FromBody] string inputKey)
        {
            _methodName = "GetActiveOilTypeList";
            return Result(inputKey, _methodName, ((LoginUserIdDto id) => { return _lookupService.GetActiveOilTypeList(id); }));
        }
        //[HttpGet]
        //[Route("active/oiltype/list")]
        //[ResponseType(typeof(ContentDto))]
        //public IHttpActionResult GetActiveOilTypeList()
        //{
        //    _methodName = "GetActiveOilTypeList";
        //    return Result(_methodName, () => { return _lookupService.GetActiveOilTypeList(); });
        //}
        [HttpPost]
        [Route("getoiltypes/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetOilTypesById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetOilTypesById([FromBody] string inputKey)
        {
            _methodName = "GetOilTypesById";
            return Result(inputKey, _methodName, (string x) => { return _lookupService.GetOilTypesById(x); });
        }

        [HttpPost]
        [Route("getverticals/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetVerticalsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetVerticalsById([FromBody] string inputKey)
        {
            _methodName = "GetVerticalsById";
            return Result(inputKey, _methodName, (string x) => { return _lookupService.GetVerticalsById(x); });
        }


        /// <summary>
        /// Method to Get IncoTerm List
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("IncoTerm/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetIncoTermList([FromBody]string inputKey)
        {
            _methodName = "GetIncoTermList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _lookupService.GetIncoTermList(x); });
        }

        /// <summary>
        /// Method to Get OilPackingType List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("oilpackingtype/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetOilPackingTypeList()
        {
            _methodName = "GetOilPackingTypeList";
            return Result(_methodName, () => { return _lookupService.GetOilPackingTypeList(); });
        }

        /// <summary>
        /// Method to Get OilPackingGroupType List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("oilpackinggrouptype/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetOilPackingGroupTypeList()
        {
            _methodName = "GetOilPackingGroupTypeList";
            return Result(_methodName, () => { return _lookupService.GetOilPackingGroupTypeList(); });
        }

        /// <summary>
        /// Method to Get city List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("city/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetCityList()
        {
            _methodName = "GetCityList";
            return Result(_methodName, () => { return _lookupService.GetCityList(); });
        }

        /// <summary>
        /// Method to Get District List By StateId
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("districts/stateid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDistrictListByStateId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDistrictListByStateId([FromBody]string inputKey)
        {
            _methodName = "GetDistrictListByStateId";
            return Result(inputKey, _methodName, (int x) => { return _lookupService.GetDistrictListByStateId(x); });
        }

        /// <summary>
        /// Method to Get City List By DistrictName
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("cities/districtid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCityListByDistrictId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCityListByDistrictId([FromBody]string inputKey)
        {
            _methodName = "GetCityListByDistrictId";
            return Result(inputKey, _methodName, (int x) => { return _lookupService.GetCityListByDistrictId(x); });
        }


        /// <summary>
        /// Method to Get City List By DistrictName
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("cities/stateid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCityListByStateId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCityListByStateId([FromBody] string inputKey)
        {
            _methodName = "GetCityListByStateId";
            return Result(inputKey, _methodName, (int x) => { return _lookupService.GetCityListByStateId(x); });
        }

        [HttpPost]
        [Route("cityddl/districtid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCityListByDistrictId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCityListByDistrictIdForDropdown([FromBody]string inputKey)
        {
            _methodName = "GetCityListByDistrictId";
            return Result(inputKey, _methodName, (int x) => { return _lookupService.GetCityListByDistrictIdForDropdown(x); });
        }

      

        /// <summary>
        /// Method to Get OilTypes Based On Vertical Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("oiltypes/verticalid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetOilTypesBasedOnVerticalId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetOilTypesBasedOnVerticalId([FromBody]string inputKey)
        {
            _methodName = "GetOilTypesBasedOnVerticalId";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _lookupService.GetOilTypesBasedOnVerticalId(x); });
        }

        //get oiltypes based on vertical if there is vertical id or gets all oiltypes
        [HttpPost]
        [Route("oiltypes/vertical")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetOilTypesBasedOnVertical", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetOilTypesBasedOnVertical([FromBody] string inputKey)
        {
            _methodName = "GetOilTypesBasedOnVertical";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _lookupService.GetOilTypesBasedOnVertical(x); });
        }

        [HttpPost]
        [Route("states/customergroupid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetStatesBasedOnCustomerGroupId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetStatesBasedOnCustomerGroupId([FromBody]string inputKey)
        {
            _methodName = "GetStatesBasedOnCustomerGroupId";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _lookupService.GetStatesBasedOnCustomerGroupId(x); });
        }

        /// <summary>
        /// Method to Get Skus Based On OilType Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("skus/oiltypeid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkusBasedOnOilTypeId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkusBasedOnOilTypeId([FromBody]string inputKey)
        {
            _methodName = "GetSkusBasedOnOilTypeId";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _lookupService.GetSkusBasedOnOilTypeId(x); });
        }

        /// <summary>
        /// Method to Get uom List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("uom/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetUomList()
        {
            _methodName = "GetUomList";
            return Result(_methodName, () => { return _lookupService.GetUomList(); });
        }

        [HttpPost]
        [Route("skus/skubasedonoiltype")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkusBasedOnOilType", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkusBasedOnOilType([FromBody]string inputKey)
        {
            _methodName = "GetSkusBasedOnOilType";
            return Result(inputKey, _methodName, (SkuInputDto x) => { return _lookupService.GetSkusBasedOnOilType(x); });
        }


        [HttpPost]
        [Route("skus/skubasedonemployeediscount")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkusBasedOnEmployeeDiscount", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkusBasedOnEmployeeDiscount([FromBody] string inputKey)
        {
            _methodName = "GetSkusBasedOnEmployeeDiscount";
            return Result(inputKey, _methodName, (SkuInputDto x) => { return _lookupService.GetSkusBasedOnEmployeeDiscount(x); });
        }

        [HttpPost]
        [Route("skus/skubasedonoiltype/unit")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkusUnitBasedOnOilType", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkusUnitBasedOnOilType([FromBody]string inputKey)
        {
            _methodName = "GetSkusUnitBasedOnOilType";
            return Result(inputKey, _methodName, (SkuInputDto x) => { return _lookupService.GetSkusUnitBasedOnOilType(x); });
        }

        [HttpPost]
        [Route("user/dealerbrokerdetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerAndBrokerDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerAndBrokerDetails([FromBody]string inputKey)
        {
            _methodName = "GetDealerAndBrokerDetails";
            return Result(inputKey, _methodName, (ReportingUsersInputDto x) => { return _lookupService.GetDealerAndBrokerDetails(x); });
        }

        [HttpPost]
        [Route("user/dealerdetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerDetails([FromBody]string inputKey)
        {
            _methodName = "GetDealerDetails";
            return Result(inputKey, _methodName, (LoginDealerIdDto x) => { return _lookupService.GetDealerDetails(x); });
        }

        [HttpPost]
        [Route("user/state/dealers")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealersBasedOnState", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealersBasedOnState([FromBody]string inputKey)
        {
            _methodName = "GetDealersBasedOnState";
            return Result(inputKey, _methodName, (ReportingUsersInputDto x) => { return _lookupService.GetDealersBasedOnState(x); });
        }

        [HttpPost]
        [Route("Customer/cityids")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCustomerOnCity", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCustomerOnCity([FromBody]string inputKey)
        {
            _methodName = "GetCustomerOnCity";
            return Result(inputKey, _methodName, (List<int> x) => { return _lookupService.GetCustomerOnCity(x); });
        }

        #endregion


        #region User

        [HttpPost]
        [Route("users/roleid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUsersByRoleIdddl", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUsersByRoleIdddl([FromBody]string inputKey)
        {
            _methodName = "GetUsersByRoleIdddl";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _lookupService.GetUsersByRoleIdddl(x); });
        }

        #endregion

        #region Lookup

        /// <summary>
        /// Method to Get District List By StateId
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("unmappeddistrict/stateid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUnMappedDistrictListByStateId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUnMappedDistrictListByStateId([FromBody]string inputKey)
        {
            _methodName = "GetUnMappedDistrictListByStateId";
            return Result(inputKey, _methodName, (int x) => { return _lookupService.GetUnMappedDistrictListByStateId(x); });
        }

        [HttpPost]
        [Route("oilpackingtype/skuid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPackGroupListBySkuId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPackGroupListBySkuId([FromBody]string inputKey)
        {
            _methodName = "GetPackGroupListBySkuId";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _lookupService.GetPackGroupListBySkuId(x); });
        }

        [HttpGet]
        [Route("subcategory/listddl")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetSubCategoryListddl()
        {
            _methodName = "GetSubCategoryList";
            return Result(_methodName, () => { return _lookupService.GetSubCategoryListddl(); });
        }

        /// <summary>
        /// Method to Get Configuration List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("configuration/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetConfigurationList()
        {
            _methodName = "GetConfigurationList";
            return Result(_methodName, () => { return _lookupService.GetConfigurationList(); });
        }

        /// <summary>
        /// Method to Update Configuration
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("configuration/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateConfiguration", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateConfiguration([FromBody]string inputKey)
        {
            _methodName = "UpdateConfiguration";
            return Result(inputKey, _methodName, (List<ConfigurationDto> x) => { return _lookupService.UpdateConfiguration(x); });
        }

        [HttpPost]
        [Route("skulist/packtypeid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuListByPackGroupId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuListByPackGroupId([FromBody]string inputKey)
        {
            _methodName = "GetSkuListByPackGroupId";
            return Result(inputKey, _methodName, (SkuDropDownInputDto x) => { return _lookupService.GetSkuListByPackGroupId(x); });
        }

        [HttpPost]
        [Route("user/dealerdetailsbyvertical")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerDetailsByVertical", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerDetailsByVertical([FromBody]string inputKey)
        {
            _methodName = "GetDealerDetailsByVertical";
            return Result(inputKey, _methodName, (DealerBrokerParamDto x) => { return _lookupService.GetDealerDetailsByVertical(x); });
        }

        [HttpPost]
        [Route("skuddl/OiltypeIdSubcategoryIdPackgroupId")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuListBasedOnOilTypeIdSubCategoryIdPackGroupIdForDropdown", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuListBasedOnOilTypeIdSubCategoryIdPackGroupIdForDropdown([FromBody]string inputKey)
        {
            _methodName = "GetSkuListBasedOnOilTypeIdSubCategoryIdPackGroupIdForDropdown";
            return Result(inputKey, _methodName, (SkuDropDownInputDto x) => { return _lookupService.GetSkuListBasedOnOilTypeIdSubCategoryIdPackGroupIdForDropdown(x); });
        }

        #endregion

        [HttpPost]
        [Route("addDateRange")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddDateRange", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddDateRange([FromBody] string inputKey)
        {
            _methodName = "AddDateRange";
            return Result(inputKey, _methodName, (DateRangeDTO x) => { return _lookupService.AddDateRange(x); });
        }

        [HttpPost]
        [Route("getDateRange")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDateRange", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDateRange([FromBody] string inputKey)
        {
            _methodName = "GetDateRange";
            return Result(inputKey, _methodName, (long x) => { return _lookupService.GetDateRange(x); });
        }

        #region Key Performance Indicator

        [HttpPost]
        [Route("keyperformance/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddKeyPerformance", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddKeyPerformance([FromBody]string inputKey)
        {
            _methodName = "AddKeyPerformance";
            return Result(inputKey, _methodName, (KeyPerformanceDto x) => { return _lookupService.AddKeyPerformance(x); });
        }

        [HttpPost]
        [Route("keyperformance/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateKeyPerformance", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateKeyPerformance([FromBody]string inputKey)
        {
            _methodName = "UpdateKeyPerformance";
            return Result(inputKey, _methodName, (KeyPerformanceDto x) => { return _lookupService.UpdateKeyPerformance(x); });
        }

        [HttpPost]
        [Route("keyperformance/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetKeyPerformanceById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetKeyPerformanceById([FromBody]string inputKey)
        {
            _methodName = "GetKeyPerformanceById";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _lookupService.GetKeyPerformanceById(x); });
        }

        [HttpPost]
        [Route("keyperformance/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetKeyPerformanceList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetKeyPerformanceList([FromBody]string inputKey)
        {
            _methodName = "GetKeyPerformanceList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _lookupService.GetKeyPerformanceList(x); });
        }

        #endregion

        #region Dealer and Broker List

        [HttpPost]
        [Route("user/dealerbrokerlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerAndBrokerListForBDO", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerAndBrokerListForBDO([FromBody]string inputKey)
        {
            _methodName = "GetDealerAndBrokerListForBDO";
            return Result(inputKey, _methodName, (ReportingUsersInputDto x) => { return _lookupService.GetDealerAndBrokerListForBDO(x); });
        }

        #endregion

        #region Sku Ingredient OilTypes
        /// <summary>
        /// Method to Get OilTypes Based On Vertical Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("skuingredientoiltypes/verticalid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuIngredienOilTypes", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuIngredienOilTypes([FromBody]string inputKey)
        {
            _methodName = "GetSkuIngredienOilTypes";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _lookupService.GetSkuIngredienOilTypes(x); });
        }
        #endregion

        #region Dropdown

        /// <summary>
        /// Method to Get Material Cost OilTypes Based On Vertical Id and Config
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("materialcost/oiltypes")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "MaterialCostOilTypesBasedOnVerticalId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult MaterialCostOilTypesBasedOnVerticalId([FromBody]string inputKey)
        {
            _methodName = "MaterialCostOilTypesBasedOnVerticalId";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _lookupService.MaterialCostOilTypesBasedOnVerticalId(x); });
        }

        /// <summary>
        /// Method to Get Skus Based On OilType Id & Sub category
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("skus/oiltypesubcategory")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuBasedOnOilTypeSubCategory", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuBasedOnOilTypeSubCategory([FromBody]string inputKey)
        {
            _methodName = "GetSkuBasedOnOilTypeSubCategory";
            return Result(inputKey, _methodName, (SkuDropDownInputDto x) => { return _lookupService.GetSkuBasedOnOilTypeSubCategoryForDropdown(x); });
        }

        //[HttpPost]
        //[Route("skus/oiltyperasoi")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetOilTypeIsRasoiOrNot", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetOilTypeIsRasoiOrNot([FromBody] string inputKey)
        //{
        //    _methodName = "GetOilTypeIsRasoiOrNot";
        //    return Result(inputKey, _methodName, (IdInputDto x) => { return _lookupService.GetOilTypeIsRasoiOrNot(x); });
        //}

        /// <summary>
        /// Method to Get OilTypes Based On Vertical Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getoiltypes/verticalid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetOilTypesByVerticalId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetOilTypesByVerticalId([FromBody]string inputKey)
        {
            _methodName = "GetOilTypesByVerticalId";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _lookupService.GetOilTypesByVerticalId(x); });
        }

        #endregion

        #region Ship to party

        [HttpPost]
        [Route("user/state/shipToParty")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetShipToPartysBasedOnState", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetShipToPartysBasedOnState([FromBody]string inputKey)
        {
            _methodName = "GetShipToPartysBasedOnState";
            return Result(inputKey, _methodName, (DealerBrokerParamDto x) => { return _lookupService.GetShipToPartyListBasedOnVertical(x); });
        }

        #endregion

        #region Lookup

        [HttpPost]
        [Route("getplantdepotrake/stateid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPlantDepotRakeByStateId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPlantDepotRakeByStateId([FromBody]string inputKey)
        {
            _methodName = "GetPlantDepotRakeByStateId";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _lookupService.GetPlantDepotRakeByStateId(x); });
        }

      

        [HttpGet]
        [Route("getsalesOrganization")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSalesOrganization", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSalesOrganization()
        {
            _methodName = "GetSalesOrganization";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _lookupService.GetSalesOrganization();
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("getdistributionChannel")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDistributionChannel", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDistributionChannel([FromBody] string inputKey)
        {
            _methodName = "GetDistributionChannel";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _lookupService.GetDistributionChannel(x); });
            //var result = new ResultDto();
            //var errorDto = new ErrorDto();
            //var successDto = new SuccessDto();
            //var contentDto = new ContentDto();
            //try
            //{
            //    _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            //    result = _lookupService.GetDistributionChannel();
            //}
            //catch (Exception exception)
            //{
            //    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
            //    _logger.Error(message);
            //    result.IsSuccess = false;
            //    result.ErrorDto.ErrorCode = Constants.Exception;
            //    result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            //}
            //if (result.ErrorDto.ErrorCode == Constants.Exception)
            //{
            //    return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            //}
            //if (result.IsSuccess)
            //{
            //    successDto.Response = result.SuccessDto.Response;
            //    contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
            //    return Ok(contentDto);
            //}
            //else
            //{
            //    errorDto.ErrorCode = result.ErrorDto.ErrorCode;
            //    errorDto.Message = result.ErrorDto.Message;
            //    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
            //    return Ok(contentDto);
            //}
        }

        [HttpGet]
        [Route("getcustomergroupFive")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCustomerGroupFive", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCustomerGroupFive()
        {
            _methodName = "GetCustomerGroupFive";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _lookupService.GetCustomerGroupFive();
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("oilType/ddl/verticalIds")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetOilTypeListByVerticalIdsForDropDown", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetOilTypeListByVerticalIdsForDropDown([FromBody]string inputKey)
        {
            _methodName = "GetOilTypeListByVerticalIdsForDropDown";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _lookupService.GetOilTypeListByVerticalIdsForDropDown(x); });
        }

        [HttpGet]
        [Route("oilPackingType/ddl")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetOilPackingTypeListForDropdown()
        {
            _methodName = "GetOilPackingTypeListForDropdown";
            return Result(_methodName, () => { return _lookupService.GetOilPackingTypeListForDropdown(); });
        }

        [HttpPost]
        [Route("Vertical/ddl")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetVerticalListForDropdown([FromBody]string inputKey)
        {
            _methodName = "GetVerticalListForDropdown";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _lookupService.GetVerticalListForDropdown(x); });
        }

        [HttpPost]
        [Route("sku/ddl/OiltypeIdsAndPackGroupIds")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuListByOilTypeIdsPackGroupIdsForDropdown", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuListByOilTypeIdsPackGroupIdsForDropdown([FromBody]string inputKey)
        {
            _methodName = "GetSkuListByOilTypeIdsPackGroupIdsForDropdown";
            return Result(inputKey, _methodName, (DropDownInputDto x) => { return _lookupService.GetSkuListByOilTypeIdsPackGroupIdsForDropdown(x); });
        }

        #endregion

        #region GetBDOBasedOnZonalheadIds
        [HttpPost]
        [Route("ZonalTrader/listnew")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetZonalHeadListNew", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetZonalHeadListNew([FromBody] string inputKey)
        {
            _methodName = "GetZonalHeadListNew";
            return Result(inputKey, _methodName, ((LoginUserIdDto id) => { return _lookupService.GetZonalHeadListNew(id); }));
        }
        [HttpPost]
        [Route("ZonalTrader/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetZonalHeadList", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetZonalHeadList()
        {
            _methodName = "GetZonalHeadList";
            return Result(_methodName, (() => { return _lookupService.GetZonalHeadList(); }));
        }
        [HttpPost]
        [Route("sku/listbyid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuList", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetSkuList([FromBody] string inputKey)
        {
            _methodName = "GetSkuList";
            return Result(inputKey,_methodName, ((FinalPriceSkuInputDto id) => { return _lookupService.GetSkuListData(id); }));
        }


        [HttpPost]
        [Route("ZonalTrader/vertical")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetZHBasedOnVertical", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetZHBasedOnVertical([FromBody] string inputKey)
        {
            _methodName = "GetZHBasedOnVertical";
            return Result(inputKey,_methodName, ((LoginUserIdDto x) => { return _lookupService.GetZHBasedOnVertical(x); }));
        }

        [HttpPost]
        [Route("BDOlist/ZonalTrader")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBDOBasedOnZonalHead", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GetBDOBasedOnZonalHead([FromBody]string inputKey)
        {
            _methodName = "GetBDOBasedOnZonalHead";
            return Result(inputKey, _methodName, (List<long> x) => { return _lookupService.GetBDOBasedOnZonalHead(x); });
        }

        [HttpPost]
        [Route("ddl/zhbasedonNH")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetZonalHeadBasedNH", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GetZonalHeadBasedNH([FromBody] string inputKey)
        {
            _methodName = "GetZonalHeadBasedNH";
            return Result(inputKey, _methodName, (long x) => { return _lookupService.GetZonalHeadBasedNH(x); });
        }

        [HttpPost]
        [Route("ddl/zhbasedonNHComb")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetZonalHeadBasedNHComb", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GetZonalHeadBasedNHComb([FromBody] string inputKey)
        {
            _methodName = "GetZonalHeadBasedNHComb";
            return Result(inputKey, _methodName, (BookedSaudaInputDto x) => { return _lookupService.GetZonalHeadBasedNHComb(x); });
        }

        [HttpPost]
        [Route("Dealerlist/StateTrader")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerListBasedOnBdo", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GetDealerListBasedOnBdo([FromBody] string inputKey)
        {
            _methodName = "GetDealerListBasedOnBdo";
            return Result(inputKey, _methodName, (List<long> x) => { return _lookupService.GetDealerBasedOnBdo(x); });
        }
        [HttpPost]
        [Route("DealerCodelist/StateTrader")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerListBasedOnBdo", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GetDealerCodeListBasedOnBdo([FromBody] string inputKey)
        {
            _methodName = "GetDealerListBasedOnBdo";
            return Result(inputKey, _methodName, (List<long> x) => { return _lookupService.GetDealerCodeBasedOnBdo(x); });
        }
        #endregion

        [HttpPost]
        [Route("skus/mobile/oiltypesubcategory")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuBasedOnOilTypeSubCategoryForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuBasedOnOilTypeSubCategoryForMobile([FromBody]string inputKey)
        {
            _methodName = "GetSkuBasedOnOilTypeSubCategoryForMobile";
            return Result(inputKey, _methodName, (SkuDropDownInputDto x) => { return _lookupService.GetSkuBasedOnOilTypeSubCategoryForMobile(x); });
        }


        #region TPNotification
        [HttpPost]
        [Route("GetBdoddl/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBdoddlList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBdoddlList([FromBody]string inputKey)
        {
            _methodName = "GetBdoddlList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _lookupService.GetBdoddlList(x); });
        }

        
        [HttpPost]
        [Route("DealerList/byBdoIds")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerListBasedOnBDO", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerListBasedOnBDO([FromBody]string inputKey)
        {
            _methodName = "GetDealerListBasedOnBDO";
            return KendoGridResult(inputKey, _methodName, (NotificationInputDto x) => { return _lookupService.GetDealerListBasedOnBDO(x); });
        }

        [HttpPost]
        [Route("Notification/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddNotification", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddNotification([FromBody]string inputKey)
        {
            _methodName = "AddNotification";
            return Result(inputKey, _methodName, (NotificationsDto x) => { return _lookupService.AddNotification(x); });
        }
        [HttpPost]
        [Route("Notification/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateNotification", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateNotification([FromBody]string inputKey)
        {
            _methodName = "UpdateNotification";
            return Result(inputKey, _methodName, (NotificationsDto x) => { return _lookupService.UpdateTPNotification(x); });
        }
        [HttpPost]
        [Route("TPNotification/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetTPNotificationList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetTPNotificationList([FromBody]string inputKey)
        {
            _methodName = "GetTPNotificationList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _lookupService.GetTPNotificationList(x); });
        }
        [HttpPost]
        [Route("TPNotification/details/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetTPNotificationDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetTPNotificationDetailsById([FromBody]string inputKey)
        {
            _methodName = "GetTPNotificationDetailsById";
            return Result(inputKey, _methodName, (long id) => { return _lookupService.GetTPNotificationDetailsById(id); });
        }
        [HttpPost]
        [Route("TPNotification/ById")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetTPNotificationById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetTPNotificationById([FromBody]string inputKey)
        {
            _methodName = "GetTPNotificationById";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _lookupService.GetTPNotificationById(x); });
        }

        [HttpPost]
        [Route("dealer/List/ByTPNotificationId")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetMappedDealerListByTPNotificationId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetMappedDealerListByTPNotificationId([FromBody]string inputKey)
        {
            _methodName = "GetMappedDealerListByTPNotificationId";
            return KendoGridResult(inputKey, _methodName, (NotificationGridInputDto x) => { return _lookupService.GetMappedDealerListByTPNotificationId(x); });
        }

        [HttpPost]
        [Route("TPNotification/Export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportTPNotificationList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportTPNotificationList([FromBody]string inputKey)
        {
            _methodName = "ExportTPNotificationList";
            return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _lookupService.ExportTPNotificationList(x); });
        }

        [HttpPost]
        [Route("sms/send")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SendSmsNotification", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SendSmsNotification([FromBody]string inputKey)
        {
            _methodName = "SendSmsNotification";
            return Result(inputKey, _methodName, (SmsInputDto x) => { return _lookupService.SendNotification(x); });            
        }
        #endregion

        /// <summary>
        /// Method to Get Conversion Type List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SaudaConversionType/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetSaudaConversionList()
        {
            _methodName = "GetSaudaConversionList";
            return Result(_methodName, () => { return _lookupService.GetSaudaConversionList(); });
        }

        /// <summary>
        /// Method to Update Conversion type
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaudaConversionType/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateSaudaConversionType", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateSaudaConversionType([FromBody]string inputKey)
        {
            _methodName = "UpdateSaudaConversionType";
            return Result(inputKey, _methodName, (List<SaudaConversionTypeDto> x) => { return _lookupService.UpdateSaudaConversionType(x); });
        }
        [HttpPost]
        [Route("saudaExtension/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddSaudaExtensionPolicy", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddSaudaExtensionPolicy([FromBody]string inputKey)
        {
            _methodName = "AddSaudaExtensionPolicy";
            return Result(inputKey, _methodName, (SaudaExtensionPolicyAddDto x) => { return _lookupService.AddSaudaExtensionPolicy(x); });
        }

        [HttpPost]
        [Route("saudaExtension/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetSaudaExtensionList([FromBody] string inputKey)
        {
            _methodName = "GetSaudaExtensionList";
            return Result(inputKey, _methodName, (long x) => { return _lookupService.GetSaudaExtensionList(x); });
        }

        [HttpPost]
        [Route("dealers/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetDealersDetailsList([FromBody]string inputKey)
        {
            _methodName = "GetDealersDetailsList";
            return Result(inputKey, _methodName, (FreightZoneAndRouteDropDownInputDto x) => { return _lookupService.GetDealersDetailsList(x); });
        }

        #region Delete list creation
        [HttpPost]
        [Route("deleteremarks/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetRemarksGroup([FromBody]string inputKey)
        {
            _methodName = "GetRemarksGroup";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _lookupService.GetRemarksGroup(x); });
        }
        [HttpPost]
        [Route("deleteremarks/add")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult AddRemarksGroup([FromBody]string inputKey)
        {
            _methodName = "AddRemarksGroup";
            return Result(inputKey, _methodName, (AddDeleteListRemarks x) => { return _lookupService.AddDeleteListRemarks(x); });
        }

        #endregion

        #region Permission checking - Verticals

        [HttpPost]
        [Route("vertical/checkpermission")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult CheckPermissionForVertical([FromBody] string inputKey)
        {
            _methodName = "CheckPermissionForVertical";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _lookupService.CheckPermissionForVertical(x); });
        }
        #endregion

        #region Sauda validity and Sauda report email configuration

        [HttpPost]
        [Route("configuration/saveforsaudavalidityandsaudareportmails")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult SaveConfigurationforSaudaValidityAndSaudaReportMails([FromBody] string inputKey)
        {
            _methodName = "SaveConfigurationforSaudaValidityAndSaudaReportMails";
            return Result(inputKey, _methodName, (SaudaValidityAndSaudaReportMailConfigurationDto x) => { return _lookupService.SaveConfigurationforSaudaValidityAndSaudaReportMails(x); });
        }

        [HttpGet]
        [Route("verticallist/basedonsaudavalidity")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetVerticalListBasedOnSaudaValidity()
        {
            _methodName = "GetVerticalListBasedOnSaudaValidity";
            return Result(_methodName, () => { return _lookupService.GetVerticalListBasedOnSaudaValidity(); });
        }

        [HttpPost]
        [Route("verticallistandmails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetVerticalListAndMailIds", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetVerticalListAndMailIds([FromBody] string inputKey)
        {
            _methodName = "GetVerticalListAndMailIds";
            return Result(inputKey, _methodName, (long x) => { return _lookupService.GetVerticalListAndMailIds(x); });
        }
        #endregion

        [HttpPost]
        [Route("ZHlist/NationalTrader")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetZonalHeadListByNH", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GetZonalHeadListByNH([FromBody]string inputKey)
        {
            _methodName = "GetZonalHeadListByNH";
            return Result(inputKey, _methodName, (NationalHeadDto x) => { return _lookupService.GetZonalHeadListByNH(x); });
        }

        [HttpPost]
        [Route("NationalTrader/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetNationalHeadUserList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetNationalHeadUserList([FromBody]string inputKey)
        {
            _methodName = "GetNationalHeadUserList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _lookupService.GetNationalHeadUserList(x); });
        }

        [HttpGet]
        [Route("oilpackingtype/listwithall")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetOilPackingTypeListWithAll()
        {
            _methodName = "GetOilPackingTypeListWithAll";
            return Result(_methodName, () => { return _lookupService.GetOilPackingTypeListWithAll(); });
        }

        [HttpPost]
        [Route("plant/stateid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPlantBasedOnStateId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPlantBasedOnStateId([FromBody] string inputKey)
        {
            _methodName = "GetPlantBasedOnStateId";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _lookupService.GetPlantBasedOnStateId(x); });
        }


        #region Competitors

        /// <summary>
        /// Method to Save Competitors
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("competitor/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveCompititor", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveCompititor([FromBody] string inputKey)
        {
            _methodName = "SaveCompititor";
            return Result(inputKey, _methodName, (CompetitorDto x) => { return _lookupService.SaveCompititor(x); });
        }

        /// <summary>
        /// Method to Get RaMargin List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("competitor/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCompititors", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCompititors([FromBody] string inputKey)
        {
            _methodName = "GetCompititors";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _lookupService.GetCompititors(x); });
        }

        /// <summary>
        /// Method to get Get Competitors Details By Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get/competitorid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCompititorById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCompititorById([FromBody] string inputKey)
        {
            _methodName = "GetCompititorById";
            return Result(inputKey, _methodName, (string x) => { return _lookupService.GetCompititorById(x); });
        }

        /// <summary>
        /// Method to Update Competitors
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("competitor/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateCompititors", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateCompititors([FromBody] string inputKey)
        {
            _methodName = "UpdateCompititors";
            return Result(inputKey, _methodName, (CompetitorDto x) => { return _lookupService.UpdateCompititors(x); });
        }

        /// <summary>
        /// Method to Get RaMargin List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("competitor/skulist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuBasedOnOilTypes", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuBasedOnOilTypes([FromBody] string inputKey)
        {
            _methodName = "GetSkuBasedOnOilTypes";
            return Result(inputKey, _methodName, (CompetitorSkuInputDto x) => { return _lookupService.GetSkuBasedOnOilTypes(x); });
        }

        [HttpPost]
        [Route("competitor/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportCompetitor", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportCompetitor([FromBody] string inputKey)
        {
            _methodName = "ExportCompetitor";
            return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _lookupService.ExportCompetitor(x); });
        }

        [HttpPost]
        [Route("competitor/listwithpagination")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCompetitorListWithPagination", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCompetitorListWithPagination([FromBody] string inputKey)
        {
            _methodName = "GetCompetitorListWithPagination";
            return KendoGridResult(inputKey, _methodName, (KendoGridResult x) => { return _lookupService.GetCompetitorListWithPagination(x); });
        }

        #endregion

        #region Competitor

        [HttpPost]
        [Route("bdo/competitors")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCompetitorList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCompetitorList([FromBody] string inputKey)
        {
            _methodName = "GetCompetitorList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _lookupService.GetCompetitorList(x); });
        }
        #endregion

        [HttpPost]
        [Route("skulist/basedoncombination")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuBasedOnCombination", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuBasedOnCombination([FromBody] string inputKey)
        {
            _methodName = "GetSkuBasedOnCombination";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _lookupService.GetSkuBasedOnCombination(x); });
        }

        #region SaudaBookingConfiguration

        [HttpPost]
        [Route("saudabooking/configuration")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaBookingConfiguration", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaBookingConfiguration([FromBody] string inputKey)
        {
            _methodName = "SaudaBookingConfiguration";
            return Result(inputKey, _methodName, (SaudaBookingConfigurationDto x) => { return _lookupService.SaudaBookingConfiguration(x); });
        }

        [HttpPost]
        [Route("saudabooking/mobile/configuration")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaBookingConfigurationForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaBookingConfigurationForMobile([FromBody] string inputKey)
        {
            _methodName = "SaudaBookingConfigurationForMobile";
            return Result(inputKey, _methodName, (SaudaBookingConfigurationDto x) => { return _lookupService.SaudaBookingConfigurationForMobile(x); });
        }

        #region GetSaudaBookingConfigurationList_Old
        //[HttpGet]
        //[Route("saudabookingconfiguration/list")]
        //[ResponseType(typeof(ContentDto))]
        //public IHttpActionResult GetSaudaBookingConfigurationList()
        //{
        //    _methodName = "GetSaudaBookingConfigurationList";
        //    return Result(_methodName, () => { return _lookupService.GetSaudaBookingConfigurationList(); });
        //}
        #endregion

        [HttpPost]
        [Route("saudabookingconfiguration/details")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetSaudaBookingConfigurationDetails([FromBody] string InputKey)
        {
            _methodName = "GetSaudaBookingConfigurationList";
            return Result(InputKey,_methodName, (string x) => { return _lookupService.GetSaudaBookingConfigurationDetails(x); });
        }

        [HttpPost]
        [Route("saudabooking/configuration/rolewise")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaBookingConfigurationRolewise", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaBookingConfigurationRolewise([FromBody] string inputKey)
        {
            _methodName = "SaudaBookingConfigurationRolewise";
            return Result(inputKey, _methodName, (UserInputDto x) => { return _lookupService.SaudaBookingConfigurationRolewise(x); });
        }

        #endregion

        [HttpPost]
        [Route("salesdata/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuDataWithLiftingandDoNumber", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuDataWithLiftingandDoNumber([FromBody] string inputKey)
        {
            _methodName = "GetSkuDataWithLiftingandDoNumber";
            return Result(inputKey, _methodName, (LiftingSkuInputDto x) => { return _lookupService.GetSkuDataWithLiftingandDoNumber(x); });
        }

        #region GamificationDashboard

        [HttpPost]
        [Route("gamificationdashboard/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetGamificationDashboardList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetGamificationDashboardList([FromBody] string inputKey)
        {
            _methodName = "GetGamificationDashboardList";
            return Result(inputKey, _methodName, (string x) => { return _lookupService.GetGamificationDashboardList(x); });
        }

        [HttpPost]
        [Route("get/gamificationdashboard")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetGamificationDashboardWithPagination([FromBody] string inputKey)
        {
            _methodName = "GetGamificationDashboardWithPagination";
            return KendoGridResult(inputKey, _methodName, (KendoGridResult x) => { return _lookupService.GetGamificationDashboardWithPagination(x); });
        }
        
        [HttpPost]
        [Route("addorupdate/gamificationdashboard")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "AddOrUpdateOiltype", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddOrUpdateGamificationDashboardDetails([FromBody] string inputKey)
        {
            _methodName = "AddOrUpdateGamificationDashboardDetails";
            return Result(inputKey, _methodName, (GamificationDashboardDto s) => { return _lookupService.AddOrUpdateGamificationDashboardDetails(s); });
        }
        #endregion

        #region SaudaSalesAreaRestrictionConfiguration

        [HttpPost]
        [Route("saudabooking/salesarearestrictionconfiguration")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaSalesAreaRestrictionConfiguration", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaSalesAreaRestrictionConfiguration([FromBody] string inputKey)
        {
            _methodName = "SaudaSalesAreaRestrictionConfiguration";
            return Result(inputKey, _methodName, (SaudaSalesAreaRestrictionDto x) => { return _lookupService.SaudaSalesAreaRestrictionConfiguration(x); });
        }

        [HttpPost]
        [Route("saudasalesarearestrictionconfiguration/details")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetSaudaSalesAreaRestrictionConfigurationDetails([FromBody] string InputKey)
        {
            _methodName = "GetSaudaSalesAreaRestrictionConfigurationDetails";
            return Result(InputKey, _methodName, (string x) => { return _lookupService.GetSaudaSalesAreaRestrictionConfigurationDetails(x); });
        }

        #endregion
    }
}
