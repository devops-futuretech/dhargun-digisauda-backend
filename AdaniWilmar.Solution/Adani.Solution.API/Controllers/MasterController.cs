using GMCore.Authenticate;
using Adani.Solution.DTO;
using Adani.Solution.Service;
using System;
using System.Web.Http;
using System.Web.Http.Description;
using GMCore.Helper;
using Adani.Solution.Service.Common;
using System.Net;
using Adani.Solution.DTO.Common;
using System.Collections.Generic;
using Adani.Solution.API.App_Start;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/master")]
    public class MasterController : BaseApiController
    {
        private const string ServiceName = "Master Controller";
        private readonly IMasterService _masterService;
        private string _methodName;

        public MasterController(IMasterService masterService) : base(ServiceName)
        {
            _masterService = masterService;
        }

        #region Config

        [HttpPost]
        [Route("getconfigdetails")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetDelivertDetails([FromBody] string inputKey)
        {
            _methodName = "GetDelivertDetails";
            return Result(inputKey, _methodName, (DeliveryTypeInputDto s) => { return _masterService.GetDeliveryDetails(s); });
        }

        [HttpPost]
        [Route("add/config")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "AddDelivertDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddDelivertDetails([FromBody] string inputKey)
        {
            _methodName = "AddDelivertDetails";
            return Result(inputKey, _methodName, (DeliveryTypeDto s) => { return _masterService.InsertDeliveryDetails(s); });
        }

        [HttpPost]
        [Route("update/config")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "UpdateDelivertDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateDelivertDetails([FromBody] string inputKey)
        {
            _methodName = "UpdateDelivertDetails";
            return Result(inputKey, _methodName, (DeliveryTypeDto s) => { return _masterService.UpdateDeliveryDetails(s); });
        }

        [HttpPost]
        [Route("getconfigwithcodedetails")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetContractDetails([FromBody] string inputKey)
        {
            _methodName = "GetContractDetails";
            return Result(inputKey, _methodName, (ContractTypeInputDto s) => { return _masterService.GetContractDetails(s); });
        }

        [HttpPost]
        [Route("addorupdate/configwithcode")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "AddContractDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddContractDetails([FromBody] string inputKey)
        {
            _methodName = "AddContractDetails";
            return Result(inputKey, _methodName, (ContractTypeDto s) => { return _masterService.AddOrUpdateContract(s); });
        }

        [HttpGet]
        [Route("serverdatetime")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "AddDelivertDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetServerDateTime([FromBody] string inputKey)
        {
            _methodName = "GetServerDateTime";
            return Result(_methodName, () => { return _masterService.GetServerDateTime(); });
        }

        #endregion

        #region Vertical

        [HttpPost]
        [Route("vertical/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetVerticalList([FromBody] string inputKey)
        {
            _methodName = "GetVerticalList";
            return Result(inputKey, _methodName, (LoginUserIdDto s) => { return _masterService.GetVerticals(s); });
        }

        [HttpPost]
        [Route("vertical/listwithpagination")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetVerticalListWithPagination([FromBody] string inputKey)
        {
            _methodName = "GetVerticalListWithPagination";
            return KendoGridResult(inputKey, _methodName, (KendoGridResult x) => { return _masterService.GetVerticalListWithPagination(x); });
        }

        [HttpPost]
        [Route("addorupdate/verticals")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "AddOrUpdateVerticals", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddOrUpdateVerticals([FromBody] string inputKey)
        {
            _methodName = "AddOrUpdateVerticals";
            return Result(inputKey, _methodName, (VerticalDto s) => { return _masterService.AddOrUpdateVerticals(s); });
        }

        [HttpPost]
        [Route("vertical/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportVertical", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportVertical([FromBody] string inputKey)
        {
            _methodName = "ExportVertical";
            return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.ExportVertical(x); });
        }

        #endregion

        #region OilType

        [HttpPost]
        [Route("oiltype/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetOilTypeList([FromBody] string inputKey)
        {
            _methodName = "GetOilTypeList";
            return Result(inputKey, _methodName, (LoginUserIdDto s) => { return _masterService.GetOilType(s); });
        }
        [HttpPost]
        [Route("oiltype/listbsdlogin")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetOilTypeListBasedOnLogin([FromBody] string inputKey)
        {
            _methodName = "GetOilTypeListBasedOnLogin";
            return Result(inputKey, _methodName, (LoginUserIdDto s) => { return _masterService.GetOilTypeListBasedOnLogin(s); });
        }
        [HttpPost]
        [Route("oiltype/listwithpagination")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetOilTypeListWithPagination([FromBody] string inputKey)
        {
            _methodName = "GetOilTypeListWithPagination";
            return KendoGridResult(inputKey, _methodName, (KendoGridResult x) => { return _masterService.GetOilTypeListWithPagination(x); });
        }

        [HttpPost]
        [Route("addorupdate/oiltype")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "AddOrUpdateOiltype", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddOrUpdateOiltype([FromBody] string inputKey)
        {
            _methodName = "AddOrUpdateOiltype";
            return Result(inputKey, _methodName, (OilTypeDto s) => { return _masterService.AddOrUpdateOiltype(s); });
        }

        [HttpPost]
        [Route("oiltype/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportOilType", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportOilType([FromBody] string inputKey)
        {
            _methodName = "ExportOilType";
            return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.ExportOilType(x); });
        }

        #endregion

        #region Plant 

        [HttpPost]
        [Route("plant/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetPlantList([FromBody] string inputKey)
        {
            _methodName = "GetPlantList";
            return Result(inputKey, _methodName, (LoginUserIdDto s) => { return _masterService.GetPlantMaster(s); });
        }

        [HttpPost]
        [Route("add/plant")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddPlantDetails", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult AddPlantDetails([FromBody] string inputKey)
        {
            _methodName = "AddPlantDetails";
            return Result(inputKey, _methodName, (DepotDto s) => { return _masterService.AddPlants(s); });
        }

        [HttpPost]
        [Route("update/plant")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdatePlantDetails", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult UpdatePlantDetails([FromBody] string inputKey)
        {
            _methodName = "UpdatePlantDetails";
            return Result(inputKey, _methodName, (DepotDto s) => { return _masterService.UpdatePlants(s); });
        }

        [HttpPost]
        [Route("plant/details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPlantDetailById", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetPlantDetailById([FromBody] string inputKey)
        {
            _methodName = "GetPlantDetailById";
            return Result(inputKey, _methodName, (DepotDto s) => { return _masterService.GetPlantMasterById(s); });
        }

        [Route("plantlist/ddl")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetPlantMasterddl()
        {
            _methodName = "GetPlantMasterddl";
            return Result(_methodName, () => { return _masterService.GetPlantMasterddl(); });
        }

        [HttpPost]
        [Route("plantlist/ddlbased")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetPlantMasterddlBased([FromBody] string inputKey)
        {
            _methodName = "GetPlantMasterddl";
            return Result(inputKey,_methodName, (PlantDDLDto x) => { return _masterService.GetPlantMasterddlbased(x); });
        }

        [HttpPost]
        [Route("plant/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportPlant", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportPlant([FromBody] string inputKey)
        {
            _methodName = "ExportPlant";
            return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.ExportPlantMaster(x); });
        }

        [HttpPost]
        [Route("plant/listwithpagination")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetPlantListWithPagination([FromBody] string inputKey)
        {
            _methodName = "GetPlantListWithPagination";
            return KendoGridResult(inputKey, _methodName, (KendoGridResult x) => { return _masterService.GetPlantListWithPagination(x); });
        }

        #endregion

        #region Depot 

        [HttpPost]
        [Route("depot/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetDepotList([FromBody] string inputKey)
        {
            _methodName = "GetDepotList";
            return Result(inputKey, _methodName, (LoginUserIdDto s) => { return _masterService.GetDepotMaster(s); });
        }

        [HttpPost]
        [Route("add/depot")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddDepotDetails", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult AddDepotDetails([FromBody] string inputKey)
        {
            _methodName = "AddDepotDetails";
            return Result(inputKey, _methodName, (DepotDto s) => { return _masterService.AddDepots(s); });
        }

        [HttpPost]
        [Route("update/depot")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateDepotDetails", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult UpdateDepotDetails([FromBody] string inputKey)
        {
            _methodName = "UpdateDepotDetails";
            return Result(inputKey, _methodName, (DepotDto s) => { return _masterService.UpdateDepots(s); });
        }

        [HttpPost]
        [Route("depot/details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDepotDetailById", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetDepotDetailById([FromBody] string inputKey)
        {
            _methodName = "GetDepotDetailById";
            return Result(inputKey, _methodName, (DepotDto s) => { return _masterService.GetDepotMasterById(s); });
        }

        [HttpPost]
        [Route("depotplant/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetDepotAndPlantList([FromBody] string inputKey)
        {
            _methodName = "GetDepotAndPlantList";
            return Result(inputKey, _methodName, (LoginUserIdDto s) => { return _masterService.GetDepotAndPlantList(s); });
        }

        [HttpPost]
        [Route("depot/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportDepot", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportDepot([FromBody] string inputKey)
        {
            _methodName = "ExportDepot";
            return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.ExportDepot(x); });
        }

        [HttpPost]
        [Route("depot/listwithpagination")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetDepotListWithPagination([FromBody] string inputKey)
        {
            _methodName = "GetDepotListWithPagination";
            return KendoGridResult(inputKey, _methodName, (KendoGridResult x) => { return _masterService.GetDepotListWithPagination(x); });
        }

        #endregion

        #region Zone 

        [HttpPost]
        [Route("zonebyid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetZoneById", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetZoneById([FromBody] string inputKey)
        {
            _methodName = "GetZonGetZoneeList";
            return Result(inputKey, _methodName, ((string zoneId) => { return _masterService.EditZone(zoneId); }));
        }

        [HttpPost]
        [Route("zone/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetZoneList", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetZoneList()
        {
            _methodName = "GetZoneList";
            return Result(_methodName, (() => { return _masterService.GetZoneList(); }));
        }

        [HttpPost]
        [Route("roles/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetRolesList", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetRolesList()
        {
            _methodName = "GetRolesList";
            return Result(_methodName, (() => { return _masterService.GetRolesList(); }));
        }

        [HttpPost]
        [Route("zonelist/ddl")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetZoneListForDropdown", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetZoneListForDropdown()
        {
            _methodName = "GetZoneListForDropdown";
            return Result(_methodName, (() => { return _masterService.GetZoneListForDropdown(); }));
        }

        [HttpPost]
        [Route("statelistddl/zoneid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetStateListByZoneIdForDropdown", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetStateListByZoneIdForDropdown([FromBody] string inputKey)
        {
            _methodName = "GetStateListByZoneIdForDropdown";
            return Result(inputKey, _methodName, ((int n) => { return _masterService.GetStateListByZoneIdForDropdown(n); }));
        }

        [HttpPost]
        [Route("zone/statelist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetZoneStateList", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetZoneStateList([FromBody] string inputKey)
        {
            _methodName = "GetZoneStateList";
            return Result(inputKey, _methodName, ((int n) => { return _masterService.GetZoneStateList(n); }));
        }



        [HttpPost]
        [Route("statelist/zoneids")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetStateListByZoneIds", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetStateListByZoneIds([FromBody] string inputKey)
        {
            _methodName = "GetStateListByZoneIds";
            return Result(inputKey, _methodName, ((List<long> n) => { return _masterService.GetStateListByZoneIds(n); }));
        }

        [HttpGet]
        [Route("zone/new")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "NewZone", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult NewZone([FromBody] string inputKey)
        {
            _methodName = "NewZone";
            return Result(_methodName, (() => { return _masterService.NewZone(); }));
        }

        [HttpGet]
        [Route("zone/edit")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "EditZone", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult EditZone([FromBody] string inputKey)
        {
            _methodName = "EditZone";
            return Result(inputKey, _methodName, ((string z) => { return _masterService.EditZone(z); }));
        }

        [HttpPost]
        [Route("zone/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddZone", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult AddZone([FromBody] string inputKey)
        {
            _methodName = "AddZone";
            return Result(inputKey, _methodName, ((AddorUpdateZoneDto z) => { return _masterService.AddZone(z); }));
        }

        [HttpPost]
        [Route("zone/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateZone", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult UpdateZone([FromBody] string inputKey)
        {
            _methodName = "UpdateZone";
            return Result(inputKey, _methodName, ((AddorUpdateZoneDto z) => { return _masterService.UpdateZone(z); }));
        }

        [HttpPost]
        [Route("zone/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportZone", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportZone([FromBody] string inputKey)
        {
            _methodName = "ExportZone";
            return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.ExportZone(x); });
        }

        #endregion

        #region Sku

        /// <summary>
        /// Method to Save Sku
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("sku/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveSku", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveSku([FromBody] string inputKey)
        {
            _methodName = "SaveSku";
            return Result(inputKey, _methodName, (SkuDto x) => { return _masterService.SaveSku(x); });
        }

        /// <summary>
        /// Method to Get Sku List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("sku/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuList([FromBody] string inputKey)
        {
            _methodName = "GetSkuList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.GetSkuList(x); });
        }

        [HttpPost]
        [Route("sku/listwithpagination")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuListWithPagination", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuListWithPagination([FromBody] string inputKey)
        {
            _methodName = "GetSkuListWithPagination";
            return KendoGridResult(inputKey, _methodName, (KendoGridResult x) => { return _masterService.GetSkuListWithPagination(x); });
        }

        /// <summary>
        /// Method to get Get Sku Details By Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get/skuid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuDetailsById([FromBody] string inputKey)
        {
            _methodName = "GetSkuDetailsById";
            return Result(inputKey, _methodName, (string x) => { return _masterService.GetSkuDetailsById(x); });
        }

        /// <summary>
        /// Method to Update Sku
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("sku/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateSku", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateSku([FromBody] string inputKey)
        {
            _methodName = "UpdateSku";
            return Result(inputKey, _methodName, (SkuDto x) => { return _masterService.UpdateSku(x); });
        }

        #endregion

        #region Lookup -DDL

        [HttpGet]
        [Route("sauda/bookingtypes")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBookingTypes", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult Bookingtypes()
        {
            _methodName = "Bookingtypes";
            return Result(_methodName, () => { return _masterService.GetBookingTypes(); });
        }

        [HttpGet]
        [Route("materialTypes")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetMaterialTypes", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetMaterialTypes()
        {
            _methodName = "GetMaterialTypes";
            return Result(_methodName, () => { return _masterService.GetMaterialTypes(); });
        }

        [HttpGet]
        [Route("oiltypes")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetOilTypes", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetOilTypes()
        {
            _methodName = "GetOilTypes";
            return Result(_methodName, () => { return _masterService.GetOilTypes(); });
        }

        #endregion

        [HttpGet]
        [Route("salesdocumenttypeddl")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSalesDocumentTypes", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSalesDocumentTypes()
        {
            _methodName = "GetSalesDocumentTypes";
            return Result(_methodName, () => { return _masterService.GetSalesDocumentTypes(); });
        }

        #region State
        /// <summary>
        /// Method to Save State
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("state/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveState", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult Savestate([FromBody] string inputKey)
        {
            _methodName = "Savestate";
            return Result(inputKey, _methodName, (AddStateDto x) => { return _masterService.AddStates(x); });
        }

        /// <summary>
        /// Method to Get state List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("state/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetStateList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetStateList([FromBody] string inputKey)
        {
            _methodName = "GetStateList";
            return Result(_methodName, () => { return _masterService.GetStates(); });
        }


        /// <summary>
        /// Method to get Get state Details By Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("state/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetStateDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetStateDetailsById([FromBody] string inputKey)
        {
            _methodName = "GetStateDetailsById";
            return Result(inputKey, _methodName, (UpdateStateDto x) => { return _masterService.ViewState(x); });
        }

        /// <summary>
        /// Method to Update state
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("state/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateState", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateState([FromBody] string inputKey)
        {
            _methodName = "UpdateState";
            return Result(inputKey, _methodName, (UpdateStateDto x) => { return _masterService.UpdateStates(x); });
        }

        [HttpPost]
        [Route("state/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportState", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportState([FromBody] string inputKey)
        {
            _methodName = "ExportState";
            return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.ExportStates(x); });
        }

        [HttpPost]
        [Route("state/listwithpagination")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetStateListWithPagination([FromBody] string inputKey)
        {
            _methodName = "GetStateListWithPagination";
            return KendoGridResult(inputKey, _methodName, (KendoGridResult x) => { return _masterService.GetStateListWithPagination(x); });
        }

        #endregion

        #region Districts
        /// <summary>
        /// Method to Save district
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("district/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "Savedistrict", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult Savedistrict([FromBody] string inputKey)
        {
            _methodName = "Savedistrict";
            return Result(inputKey, _methodName, (AddDistrictDto x) => { return _masterService.AddDistrict(x); });
        }

        /// <summary>
        /// Method to Get district List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("district/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDistrictList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDistrictList([FromBody] string inputKey)
        {
            _methodName = "GetDistrictList";
            return Result(_methodName, () => { return _masterService.GetDistricts(); });
        }


        /// <summary>
        /// Method to get Get district Details By Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("district/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDistrictDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDistrictDetailsById([FromBody] string inputKey)
        {
            _methodName = "GetDistrictDetailsById";
            return Result(inputKey, _methodName, (UpdateDistrictDto x) => { return _masterService.ViewDistrict(x); });
        }

        /// <summary>
        /// Method to Update district
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("district/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateDistrict", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateDistrict([FromBody] string inputKey)
        {
            _methodName = "UpdateDistrict";
            return Result(inputKey, _methodName, (UpdateDistrictDto x) => { return _masterService.UpdateDistrict(x); });
        }

        [HttpPost]
        [Route("district/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportDistrict", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportDistrict([FromBody] string inputKey)
        {
            _methodName = "ExportDistrict";
            return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.ExportDistrict(x); });
        }

        #endregion

        #region City

        /// <summary>
        /// Method to Save City
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("city/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "Savecity", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult Savecity([FromBody] string inputKey)
        {
            _methodName = "Savecity";
            return Result(inputKey, _methodName, (AddCityDto x) => { return _masterService.AddCity(x); });
        }

        /// <summary>
        /// Method to Get City List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("city/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCityList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCityList([FromBody] string inputKey)
        {
            _methodName = "GetCityList";
            return Result(_methodName, () => { return _masterService.GetCities(); });
        }


        /// <summary>
        /// Method to get Get City Details By Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("city/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCityDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCityDetailsById([FromBody] string inputKey)
        {
            _methodName = "GetDistrictDetailsById";
            return Result(inputKey, _methodName, (UpdateCityDto x) => { return _masterService.ViewCity(x); });
        }

        /// <summary>
        /// Method to Update City
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("city/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateCity", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateCity([FromBody] string inputKey)
        {
            _methodName = "UpdateCity";
            return Result(inputKey, _methodName, (UpdateCityDto x) => { return _masterService.UpdateCity(x); });
        }

        [HttpPost]
        [Route("city/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportCity", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportCity([FromBody] string inputKey)
        {
            _methodName = "ExportCity";
            return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.ExportCity(x); });
        }

        #endregion

        #region Territories

        [HttpPost]
        [Route("territory/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddTerritory", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult AddTerritory([FromBody] string inputKey)
        {
            _methodName = "AddTerritory";
            return Result(inputKey, _methodName, (TerritoryDto x) => { return _masterService.AddTerritory(x); });
        }

        [HttpPost]
        [Route("territory/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateTerritory", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult UpdateTerritory([FromBody] string inputKey)
        {
            _methodName = "UpdateTerritory";
            return Result(inputKey, _methodName, (TerritoryDto x) => { return _masterService.UpdateTerritory(x); });
        }

        [HttpPost]
        [Route("territory/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GerTerritoryById", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GerTerritoryById([FromBody] string inputKey)
        {
            _methodName = "GerTerritoryById";
            return Result(inputKey, _methodName, (int x) => { return _masterService.GerTerritoryById(x); });
        }

        [HttpPost]
        [Route("territory/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GerTerritoryList", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GerTerritoryList([FromBody] string inputKey)
        {
            _methodName = "GerTerritoryList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.GerTerritoryList(x); });
        }

        [HttpPost]
        [Route("territorydistrict/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GerTerritoryMappedDistrict", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GerTerritoryMappedDistrict([FromBody] string inputKey)
        {
            _methodName = "GerTerritoryMappedDistrict";
            return Result(inputKey, _methodName, (TerritoryDistrictParam x) => { return _masterService.GerTerritoryMappedDistrict(x); });
        }

        [HttpPost]
        [Route("territory/stateid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GerTerritoryStateBase", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GerTerritoryStateBase([FromBody] string inputKey)
        {
            _methodName = "GerTerritoryStateBase";
            return Result(inputKey, _methodName, (int x) => { return _masterService.GerTerritoryListByStateForDropdown(x); });
        }

        /// <summary>
        /// Method to Get District List By TerritoryId
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("districts/territoryid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDistrictListBaseTerritory", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDistrictListBaseTerritory([FromBody] string inputKey)
        {
            _methodName = "GetDistrictListBaseTerritory";
            return Result(inputKey, _methodName, (int x) => { return _masterService.GetDistrictListBaseTerritoryForDropdown(x); });
        }

        [HttpPost]
        [Route("getstates/zoneids")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetStatesBasedOnZone", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GetStatesBasedOnZone([FromBody] string inputKey)
        {
            _methodName = "GetStatesBasedOnZone";
            return Result(inputKey, _methodName, (List<int> x) => { return _masterService.GetStatesBasedOnZone(x); });
        }

        [HttpPost]
        [Route("gerterritory/stateids")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetTerritoryListByStateIdsForDropdown", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GetTerritoryListByStateIdsForDropdown([FromBody] string inputKey)
        {
            _methodName = "GetTerritoryListByStateIdsForDropdown";
            return Result(inputKey, _methodName, (List<int> x) => { return _masterService.GetTerritoryListByStateIdsForDropdown(x); });
        }

        [HttpPost]
        [Route("territory/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportTerritory", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportTerritory([FromBody] string inputKey)
        {
            _methodName = "ExportTerritory";
            return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.ExportTerritory(x); });
        }

        #endregion

        #region ZonalTrader

        [HttpPost]
        [Route("getzonalhead/zonestateids")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetZonalHeadBasedonZoneState", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GetZonalHeadBasedonZoneState([FromBody] string inputKey)
        {
            _methodName = "GetZonalHeadBasedonZoneState";
            return Result(inputKey, _methodName, (ZonalHeadMappingDto x) => { return _masterService.GetZonalHeadBasedonZoneState(x); });
        }

        #endregion

        #region OilTypes Based On Verticals

        [HttpPost]
        [Route("getoiltype/zhverticals")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetOilTypeBasedonVerticals", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GetOilTypeBasedonVerticals([FromBody] string inputKey)
        {
            _methodName = "GetOilTypeBasedonVerticals";
            return Result(inputKey, _methodName, (OilTypeMappingDto x) => { return _masterService.GetOilTypeBasedonVerticals(x); });
        }

        #endregion

        #region Lookup

        [HttpPost]
        [Route("incoterm/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetIncoTermsList", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetIncoTermsList()
        {
            _methodName = "GetIncoTermsList";
            return Result(_methodName, (() => { return _masterService.GetIncoTermsList(); }));
        }

        [HttpPost]
        [Route("incotermlist/userid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetIncotermListBasedOnUser", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GetIncotermListBasedOnUser([FromBody] string inputKey)
        {
            _methodName = "GetIncotermListBasedOnUser";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.GetIncotermListBasedOnUser(x); });
        }

        [HttpPost]
        [Route("plantdepotlist/userid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPlantDepotListBasedOnUser", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GetPlantDepotListBasedOnUser([FromBody] string inputKey)
        {
            _methodName = "GetPlantDepotListBasedOnUser";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.GetPlantDepotListBasedOnUser(x); });
        }

        [HttpPost]
        [Route("depots/plantid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDepotsByPlantId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDepotsByPlantId([FromBody] string inputKey)
        {
            _methodName = "GetDepotsByPlantId";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _masterService.GetDepotsByPlantId(x); });
        }

        [HttpPost]
        [Route("plantdepot/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetDepotPlantddList([FromBody]string inputKey)
        {
            _methodName = "GetDepotPlantddList";
            return Result(inputKey, _methodName, (IdInputDto s) => { return _masterService.GetDepotPlantddList(s); });
        }


        [HttpGet]
        [Route("CurrentFinancialYear")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetCurrentFinancialYear([FromBody] string inputKey)
        {
            _methodName = "GetCurrentFinancialYear";
            return Result(_methodName, () => { return _masterService.GetCurrentFinancialYear(); });
        }

        [HttpPost]
        [Route("Notification")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetNotification([FromBody] string inputKey)
        {
            _methodName = "GetNotification";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.GetNotification(x); });
        }

        [HttpPost]
        [Route("request/notification")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetRequestNotification([FromBody] string inputKey)
        {
            _methodName = "GetRequestNotification";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.GetRequestNotification(x); });
        }

        /// <summary>
        /// Get Depts Based on Plant Ids
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("district/ddl/territoryIds")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDistrictListByTerritoryIdsForDropdown", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDistrictListByTerritoryIdsForDropdown([FromBody] string inputKey)
        {
            _methodName = "GetDistrictListByTerritoryIdsForDropdown";
            return Result(inputKey, _methodName, (List<int> x) => { return _masterService.GetDistrictListByTerritoryIdsForDropdown(x); });
        }

        [HttpPost]
        [Route("city/ddl/districtIds")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCityListByDistrictIdsForDropdown", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GetCityListByDistrictIdsForDropdown([FromBody] string inputKey)
        {
            _methodName = "GetCityListByDistrictIdsForDropdown";
            return Result(inputKey, _methodName, (List<int> x) => { return _masterService.GetCityListByDistrictIdsForDropdown(x); });
        }

      
        #region SubCategory

        /// <summary>
        /// Method to Save SubCategory
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("subcategory/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveSubCategory", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveSubCategory([FromBody] string inputKey)
        {
            _methodName = "SaveSubCategory";
            return Result(inputKey, _methodName, (SubCategoryDto x) => { return _masterService.SaveSubCategory(x); });
        }

        /// <summary>
        /// Method to Get SubCategory List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("subcategory/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSubCategoryList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSubCategoryList([FromBody] string inputKey)
        {
            _methodName = "GetSubCategoryList";
            return KendoGridResult(inputKey, _methodName, (KendoGridResult x) => { return _masterService.GetSubCategoryList(x); });
        }


        /// <summary>
        /// Method to get Get SubCategory Details By Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get/subcategoryid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSubCategoryDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSubCategoryDetailsById([FromBody] string inputKey)
        {
            _methodName = "GetSubCategoryDetailsById";
            return Result(inputKey, _methodName, (long x) => { return _masterService.GetSubCategoryDetailsById(x); });
        }

        /// <summary>
        /// Method to Update SubCategory
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("subcategory/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateSubCategory", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateSubCategory([FromBody] string inputKey)
        {
            _methodName = "UpdateSubCategory";
            return Result(inputKey, _methodName, (SubCategoryDto x) => { return _masterService.UpdateSubCategory(x); });
        }

        [HttpPost]
        [Route("subcategory/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportSubCategory", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportSubCategory([FromBody] string inputKey)
        {
            _methodName = "ExportSubCategory";
            return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.ExportSubCategory(x); });
        }

        #endregion

        /// <summary>
        /// Get Depts Based on Plant Ids
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("depots/plantids")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDepotsByPlantIds", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDepotsByPlantIds([FromBody] string inputKey)
        {
            _methodName = "GetDepotsByPlantIds";
            return Result(inputKey, _methodName, (DepotDropDownParam x) => { return _masterService.GetDepotsByPlantIds(x); });
        }

        #endregion

        [HttpPost]
        [Route("gettransportmode/depotrake")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetTransportModeBasedonDepotRake([FromBody] string inputKey)
        {
            _methodName = "GetTransportModeBasedonDepotRake";
            return Result(inputKey, _methodName, (IdInputDto s) => { return _masterService.GetTransportModeBasedonDepotRake(s); });
        }

        #region SalesOrganization

        [HttpPost]
        [Route("salesorganization/addorupdate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddorUpdateSalesOrganization", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddorUpdateSalesOrganization([FromBody] string inputKey)
        {
            _methodName = "AddorUpdateSalesOrganization";
            return Result(inputKey, _methodName, (SalesOrganizationDto x) => { return _masterService.AddorUpdateSalesOrganization(x); });
        }

        [HttpGet]
        [Route("salesorganization/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSalesOrganizationList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSalesOrganizationList([FromBody] string inputKey)
        {
            _methodName = "GetSalesOrganizationList";
            return Result(_methodName, () => { return _masterService.GetSalesOrganizationList(); });
        }

        [HttpPost]
        [Route("get/salesorganizationid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSalesOrganizationDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSalesOrganizationDetailsById([FromBody] string inputKey)
        {
            _methodName = "GetSalesOrganizationDetailsById";
            return Result(inputKey, _methodName, (string x) => { return _masterService.GetSalesOrganizationDetailsById(x); });
        }

        #endregion


        #region DistributionChannel

        [HttpPost]
        [Route("distributionchannel/addorupdate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddorUpdateDistributionChannel", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddorUpdateDistributionChannel([FromBody] string inputKey)
        {
            _methodName = "AddorUpdateDistributionChannel";
            return Result(inputKey, _methodName, (DistributionChannelDto x) => { return _masterService.AddorUpdateDistributionChannel(x); });
        }

        [HttpGet]
        [Route("distributionchannel/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDistributionChannelList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDistributionChannelList([FromBody] string inputKey)
        {
            _methodName = "GetDistributionChannelList";
            return Result(_methodName, () => { return _masterService.GetDistributionChannelList(); });
        }

        [HttpPost]
        [Route("get/distributionchannelid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDistributionChannelDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDistributionChannelDetailsById([FromBody] string inputKey)
        {
            _methodName = "GetDistributionChannelDetailsById";
            return Result(inputKey, _methodName, (string x) => { return _masterService.GetDistributionChannelDetailsById(x); });
        }

        #endregion

        #region CustomerGroup5

        [HttpPost]
        [Route("customergroupfive/addorupdate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddorUpdateCustomerGroupFive", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddorUpdateCustomerGroupFive([FromBody] string inputKey)
        {
            _methodName = "AddorUpdateCustomerGroupFive";
            return Result(inputKey, _methodName, (CustomerGroupFiveDto x) => { return _masterService.AddorUpdateCustomerGroupFive(x); });
        }

        [HttpGet]
        [Route("customergroupfive/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCustomerGroupfiveList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCustomerGroupFiveList([FromBody] string inputKey)
        {
            _methodName = "GetCustomerGroupFiveList";
            return Result(_methodName, () => { return _masterService.GetCustomerGroupFiveList(); });
        }

        [HttpPost]
        [Route("get/customergroupfiveid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCustomerGroupfiveDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCustomerGroupFiveDetailsById([FromBody] string inputKey)
        {
            _methodName = "GetCustomerGrouponFiveDetailsById";
            return Result(inputKey, _methodName, (string x) => { return _masterService.GetCustomerGroupFiveDetailsById(x); });
        }

        #endregion


        #region VehicleLoadabilities

        [HttpPost]
        [Route("vehicleloadabilities/addorupdate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddOrUpdatevehicleloadabilities", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddOrUpdateVehicleLoadabilities([FromBody] string inputKey)
        {
            _methodName = "AddOrUpdateVehicleLoadabilities";
            return Result(inputKey, _methodName, (VehicleLoadabilitiesDto x) => { return _masterService.AddOrUpdateVehicleLoadabilities(x); });

        }

        [HttpGet]
        [Route("vehicleloadabilities/getAll")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetVehicleLoadabilitiesList()
        {
            _methodName = "GetVehicleLoadabilitiesList";
            return Result(_methodName, () => { return _masterService.GetVehicleLoadabilitiesList(); });
        }

        [HttpPost]
        [Route("vehicleloadabilities/getById")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetVehicleLoadabilitiesById", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetVehicleLoadabilitiesById([FromBody] string inputKey)
        {
            _methodName = "GetVehicleLoadabilitiesById";
            return Result(inputKey, _methodName, (VehicleLoadabilitiesDto s) => { return _masterService.GetVehicleLoadabilitiesById(s); });
        }

        [HttpPost]
        [Route("vehicleloadabilities/exportList")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportVehicleLoadabiliities", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportVehicleLoadabiliities([FromBody] string inputKey)
        {
            _methodName = "ExportVehicleLoadabiliities";
            return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.ExportVehicleLoadabiliities(x); });
        }
        #endregion

        [HttpPost]
        [Route("oiltype/list1")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetOilTypeList1(LoginUserIdDto inputKey)
        {
            _methodName = "GetOilTypeList";
            var result = new ResultDto();
            //return Result(inputKey, _methodName, (LoginUserIdDto s) => { return _masterService.GetOilType(s); });
            result = _masterService.GetOilType(inputKey);
            return Ok(result);
        }

       
        //#region MaterialType

        //[HttpPost]
        //[Route("materialtype/addorupdate")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "AddOrUpdateMaterialType", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult AddOrUpdateMaterialType([FromBody] string inputKey)
        //{
        //    _methodName = "AddOrUpdateMaterialType";
        //    return Result(inputKey, _methodName, (MaterialTypeDto x) => { return _masterService.AddOrUpdateMaterialType(x); });

        //}

        //[HttpGet]
        //[Route("materialtype/list")]
        //[ResponseType(typeof(ContentDto))]
        //public IHttpActionResult GetMaterialTypeList()
        //{
        //    _methodName = "GetMaterialTypeList";
        //    return Result(_methodName, () => { return _masterService.GetMaterialTypeList(); });
        //}

        //[HttpPost]
        //[Route("materialtype/getById")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetMaterialTypeById", Message = "The request has been declined for security reasons.", Seconds = 5)]
        //public IHttpActionResult GetMaterialTypeById([FromBody] string inputKey)
        //{
        //    _methodName = "GetMaterialTypeById";
        //    return Result(inputKey, _methodName, (MaterialTypeDto s) => { return _masterService.GetMaterialTypeById(s); });
        //}

        //[HttpPost]
        //[Route("materialtype/export")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "ExportMaterialType", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult ExportMaterialType([FromBody] string inputKey)
        //{
        //    _methodName = "ExportMaterialType";
        //    return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.ExportMaterialType(x); });
        //}
        //#endregion

        #region VolumeLoadability

        [HttpPost]
        [Route("volumeloadability/addorupdate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddOrUpdateMaterialType", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddOrUpdateVolumeLoadability([FromBody] string inputKey)
        {
            _methodName = "AddOrUpdateVolumeLoadability";
            return Result(inputKey, _methodName, (VolumeLoadability x) => { return _masterService.AddOrUpdateVolumeLoadability(x); });

        }

        [HttpGet]
        [Route("volumeloadability/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetVolumeLoadabilityList()
        {
            _methodName = "GetVolumeLoadabilityList";
            return Result(_methodName, () => { return _masterService.GetVolumeLoadabilityList(); });
        }

        [HttpPost]
        [Route("volumeloadability/getById")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetVolumeLoadabilityById", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetVolumeLoadabilityById([FromBody] string inputKey)
        {
            _methodName = "GetMaterialTypeById";
            return Result(inputKey, _methodName, (VolumeLoadability s) => { return _masterService.GetVolumeLoadabilityById(s); });
        }

        [HttpPost]
        [Route("volumeloadability/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportVolumeLoadability", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportVolumeLoadability([FromBody] string inputKey)
        {
            _methodName = "ExportVolumeLoadability";
            return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _masterService.ExportVolumeLoadability(x); });
        }
        #endregion

        #region SchemeGeographyReport

        [HttpPost]
        [Route("getgeographyscheme/stateids")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetGeographySchemeBasedOnState", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GetGeographySchemeBasedOnState([FromBody] string inputKey)
        {
            _methodName = "GetGeographySchemeBasedOnState";
            return Result(inputKey, _methodName, (List<int> x) => { return _masterService.GetGeographySchemeBasedOnState(x); });
        }

        #endregion

        [HttpPost]
        [Route("profileImage")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetProfileImageUrl", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetProfileImageUrl([FromBody] string inputKey)
        {
           
            _methodName = "GetProfileImageUrl";
            return Result(inputKey, _methodName, (UserProfileDto s) => { return _masterService.GetProfileImageUrl(s); });
                      
        }

        [HttpPost]
        [Route("line/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "LineAdd", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult AddLineDetails([FromBody] string inputKey)
        {
            _methodName = "AddLine";
            return Result(inputKey, _methodName, ((AddAndUpdateLineDto z) => { return _masterService.AddLineDetails(z); }));
        }

        [HttpPost]
        [Route("line/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "Lineupdate", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult UpdateLineDetails([FromBody] string inputKey)
        {
            _methodName = "LineUpdate";
            return Result(inputKey, _methodName, ((AddAndUpdateLineDto z) => { return _masterService.UpdateLineDetails(z); }));
        }

        [HttpPost]
        [Route("line/details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "Lineupdate", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetLineDetailsById([FromBody] string inputKey)
        {
            _methodName = "LineUpdate";
            return Result(inputKey, _methodName, ((string z) => { return _masterService.GetLineDetailsById(z); }));
        }

        [HttpGet]
        [Route("line/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetLineListForddl", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetLineListForddl()
        {
            _methodName = "GetLineListForddl";
            return Result(_methodName, (() => { return _masterService.GetLineListForddl(); }));
        }

        [HttpGet]
        [Route("line/gridlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetLineList", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetLineListForGrid([FromBody] string inputKey)
        {
            _methodName = "GetLineListForddl";
            return Result(_methodName, (() => { return _masterService.GetLineListForGrid(); }));
        }    
        
        [HttpPost]
        [Route("donumber/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportLine", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDONumberListByDistributorId([FromBody] string inputKey)
        {
            _methodName = "GetDONumberList";
            return KendoGridResult(inputKey, _methodName, (List<string> x) => { return _masterService.GetDONumberListByDistributorId(x); });
        }

        #region  GamificationDashboard
        //[HttpGet]
        //[Route("GamificationDashboardId/gamificationdashboard")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetGamificationDashboard", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GamificationDashboard()
        //{
        //    _methodName = "GamificationDashboard";
        //    return Result(_methodName, () => { return _masterService.GetGamificationDashboard(); });
        //}

        [HttpPost]
        [Route("GamificationDashboardId/gamificationdashboard")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetGamificationDashboard", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GetGamificationDashboard([FromBody] string inputKey)
        {
            _methodName = "GetGamificationDashboard";
            return Result(inputKey, _methodName, (GamificationDashboardDto x) => { return _masterService.GetGamificationDashboard(x); });
        }

        [HttpGet]
        [Route("gcpapi/gamificationdashboard")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GCPApidata", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult GCPApidata()
        {
            _methodName = "GCPApidata";
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                _masterService.GCPApidata();
            });
            return Ok();
            //return Result(_methodName, () => { return _masterService.GCPApidata(); });
        }

        #endregion

        #region TANNumber Mobile API 

        [HttpPost]
        [Route("tannumber/getid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetTANNumber", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetTANNumber([FromBody] string inputKey)
        {
            _methodName = "GetTANNumber";
            return Result(inputKey, _methodName, (DealerTANDto x) => { return _masterService.GetTANNumber(x); });
        }

        /// <summary>
        /// Method to Update SubCategory
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("tannumber/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateTANNumber", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateTANNumber([FromBody] string inputKey)
        {
            _methodName = "UpdateTANNumber";
            return Result(inputKey, _methodName, (DealerTANDto x) => { return _masterService.UpdateTANNumber(x); });
        }
        #endregion

        [HttpGet]
        [Route("validatecalendar")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "AddDelivertDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ValidateCalendar([FromBody] string inputKey)
        {
            _methodName = "ValidateCalendar";
            return Result(_methodName, () => { return _masterService.ValidateCalendar(); });
        }

        #region Account Statement

        [HttpPost]
        [Route("accountstatement/count")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AccountStatementCount", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult AccountStatementCount([FromBody] string inputKey)
        {
            _methodName = "AccountStatementCount";
            return Result(inputKey, _methodName, ((CustomerAccountStatementDto z) => { return _masterService.AccountStatementCount(z); }));
        }

        /// <summary>
        /// Method to Update SubCategory
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("accountstatementstatus/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateAccountStatementStatus", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateAccountStatementStatus([FromBody] string inputKey)
        {
            _methodName = "UpdateAccountStatementStatus";
            return Result(inputKey, _methodName, (CustomerAccountStatementDto x) => { return _masterService.UpdateAccountStatementStatus(x); });
        }
        #endregion


        #region SAPEmail Statement

        [HttpPost]
        [Route("email/statement/save")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult AddAndUpdateSAPEmailStatement([FromBody] string inputKey)
        {
            _methodName = "AddAndUpdateSAPEmailStatement";
            return Result(inputKey,_methodName,(SAPEmailStatementInputDto x) => { return _masterService.AddAndUpdateSAPEmailStatement(x).Result; }); 
        }


        [HttpPost]
        [Route("update/emailstatement/status")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult UpdateSAPEmailStatementStatus([FromBody] SAPEmailStatementDStatusDto inputDto)
        {
            _methodName = "AddAndUpdateSAPEmailStatement";
            return Ok(_masterService.UpdateEmailStatementSAPStatus(inputDto).Result);
        }


        [HttpPost]
        [Route("import/geography/discount")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult ImportGeographyDiscount([FromBody] List<GeographyDiscountImportStatus> geographyDiscounts)
        {
            _methodName = "ImportGeographyDiscount";
            var result = new SuccessDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                _masterService.ImportGeographyDiscount(geographyDiscounts);
            });

            result.Response = geographyDiscounts;
            result.Message = Constants.GeographyDiscountInprogress;
            return Ok(result);
        }

        #endregion
    }
}

