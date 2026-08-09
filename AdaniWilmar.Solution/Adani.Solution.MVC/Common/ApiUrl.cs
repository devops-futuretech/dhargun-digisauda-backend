using System.Collections.Generic;
using System.Linq.Expressions;

namespace Adani.Solution.MVC.Common
{
    public class ApiUrl
    {
        #region Authenticate & Authorize

        //Login
        public const string WebApiUrlPostVerifyToken = "api/authenticate/verify";
        public const string WebApiUrlPostValidateUser = "/api/authorize/user";
        public const string WebApiUrlGetVerticalListBasedonUser = "api/authorize/vertical/user";

        //Change Password
        public const string WebApiUrlPostChangePasswordOtpSent = "api/authorize/user/forgotpassword/otp";
        public const string WebApiUrlPostChangePasswordOtpVerification = "api/authorize/user/resetpassword";
        public const string WebApiUrlPostResendOtp = "api/authorize/user/otp/resend";
        public const string WebApiUrlPostClaimDetails = "api/employees/user/claims";
        //App to Sap Loose Sauda
        public const string WebApiUrlLooseOilSaudaGetSyncData = "api/sap/looseoil/sauda/Sap";

        public const string WebApiUrlPendingContractTrigger = "api/lifting/PendingContractTrigger";

        #endregion

        #region User

        //User
        public const string WebApiUrlGetAllUserList = "api/user/all";
        public const string WebApiUrlGetCountryList = "api/lookups/countries";
        public const string WebApiUrlGetDistrictList = "api/lookups/districts";
        public const string WebApiUrlGetRoleList = "api/roles";
        public const string WebApiUrlGetRegionList = "api/lookups/zone/list";
        public const string WebApiUrlGetDealerBrokerList = "api/lookups/user/dealerbrokerdetails";
        public const string WebApiUrlGetDealersBasedOnState = "api/lookups/user/state/dealers";
        public const string WebApiUrlGetDealerListForPopup = "api/lookups/user/dealerdetails";
        public const string WebApiUrlGetUsersByRoleList = "api/employees/users/byrole";
        public const string WebApiUrlGetDealerDetailsByVertical = "api/lookups/user/dealerdetailsbyvertical";
        public const string WebApiUrlGetDealerAndBrokerListForBDO = "api/lookups/user/dealerbrokerlist";
        public const string WebApiUrlGetCustomerGroupOne = "api/lookups/user/getcustomergroupOne";
        public const string WebApiUrlGetCustomerGroupFive = "api/lookups/getcustomergroupFive";
        public const string WebApiUrlGetSalesOrganization = "api/lookups/getsalesOrganization";
        public const string WebApiUrlGetDistributionChannel = "api/lookups/getdistributionChannel";
        public const string WebApiUrlGetCustomerGroupTwo = "api/lookups/user/getcustomergroupTwo";
        public const string WebApiUrlGetUserLoginHistory = "api/UserController/GetUserLoginHistory/list";
        public const string WebApiUrlImportgeographyDiscount = "api/master/import/geography/discount";

        #endregion

        #region Roles

        //Master
        public const string WebApiUrlSaveType = "api/roles";

        //Role Type
        public const string WebApiUrlPostRoleType = "api/roles/roletypeclaims/add";
        public const string WebApiUrlPutRoleType = "api/roles/roletypeclaims/update";
        public const string WebApiUrlGetClaims = "api/roles/claims";
        public const string WebApiUrlDeleteRoleType = "api/roles/roletypeclaims/delete";
        public const string WebApiUrlGetReportinToRoles = "api/roles/reportingtoroles";


        //Role 
        public const string WebApiUrlPostRole = "api/roles/roleclaims/add";
        public const string WebApiUrlPutRole = "api/roles/roleclaims/update";
        public const string WebApiUrlGetRoleTypeClaims = "api/roles/roletypeclaims";
        public const string WebApiUrlGetRoleClaims = "api/roles/roleclaims";
        public const string WebApiUrlDeleteRole = "api/roles/roleclaims/delete";


        //OrganizationHierarchy 
        public const string WebApiUrlPostOrgHierarchy = "api/roles/roletype/hierarchy";
        public const string WebApiUrlGetOrgHierarchy = "api/roles/orghierarchy";

        //Common
        public const string WebApiUrlGetRoleTypes = "api/roles/roletypes";
        public const string WebApiUrlGetUserByRoleTypes = "api/users/all/roletypes";

        //Role/Reporting to users
        public const string WebApiUrlGetRoleHierarchyByProcess = "api/roles/hierarchy/processid";
        public const string WebApiUrlPostRoleHierarchy = "api/roles/hierarchy/save";
        public const string WebApiUrlGetReportingToUsersByRole = "api/roles/reportingtouser/list";
        public const string WebApiUrlGetOrganizationReportingToUsersByUserId = "api/roles/reportingtousers/userid";
        public const string WebApiUrlGetSalesReportingToUsersByUserId = "api/roles/salesreportingtousers/userid";
        public const string WebApiUrlGetSalesReportingToUsersByCityId = "api/roles/salesreportingtousers/bycityid";
        public const string WebApiUrlGetSalesReportingToUsersByCityDistrictState = "api/roles/salesreportingtousers/bycitydistrictstate";

        public const string WebApiUrlGetReportingToZonalHeadUsersByUserId = "api/roles/reportingToZonalHeadUsers/userid";
        public const string WebApiUrlGetReportingToBDOUsersByUserId = "api/roles/reportingToBDOUsers/userid";

        public const string WebApiUrlGetReportingToRABDOUsersByUserId = "api/roles/reportingToRABDOUsers/userid";
        public const string WebApiUrlGetClaimsByRoleId = "api/roles/getclaimbyroleid";
        public const string WebApiUrlGetClaimsbyRoleTypeId = "api/roles/getclaimbyroletypeid";


        #endregion

        #region Employees

        public const string WebApiUrlGetAddressByPincode = "api/lookups/address/pincode";

        public const string WebApiUrlPostSaveUser = "api/employees/save";
        public const string WebApiUrlGetDealerList = "api/employees/dealer/List";
        public const string WebApiUrlGetBrokerList = "api/employees/broker/List";
        public const string WebApiUrlGetUserMasterList = "api/employees/user/List";
        public const string WebApiUrlPostUpdateUser = "api/employees/update";
        public const string WebApiUrlPostUpdateProfile = "api/employees/profileupload";
        public const string WebApiUrlGetUserExcelExport = "api/employees/user/excelexport";
        public const string WebApiUrlGetDealerListExcelExport = "api/employees/dealer/list/export";
        public const string WebApiUrlDeleteConsentImage = "api/employees/consentimage/delete";
        public const string WebApiUrlPostUploadConsentImage = "api/employees/uploadconsentimage";


        public const string WebApiUrlPostSaveRetailer = "api/employees/retailer/save";
        public const string WebApiUrlPostUpdateRetailer = "api/employees/retailer/update";
        public const string WebApiUrlGetRetailerList = "api/employees/retailer/List";
        public const string WebApiUrlGetRetailerDetailsById = "api/employees/get/retailerid";
        public const string WebApiUrlGetRetailerListWithPagination = "api/employees/retailer/listwithpagination";

        public const string WebApiUrlGetDealerDetailsById = "api/employees/get/dealerid";
        public const string WebApiUrlGetBrokerDetailsById = "api/employees/get/brokerid";
        public const string WebApiUrlGetUserDetailsById = "api/employees/get/userid";

        //Date Range
        public const string WebApiUrlAddDateRange = "api/lookups/addDateRange";
        public const string WebApiUrlGetDateRange = "api/lookups/getDateRange";

        //Broker
        public const string WebApiUrlGetBrokerListddl = "api/employees/broker/ddl";

        //StateTrader
        public const string WebApiUrlGetBDOListddl = "api/user/StateTrader/ddl";
        public const string WebApiUrlGetOverallBDOListddl = "api/user/StateTrader/list";

        //ShipToParty
        public const string WebApiUrlGetShipToPartyDetailsById = "api/employees/get/shipToPartyid";
        public const string WebApiUrlGetExcelExportShipToPartyList = "api/employees/shipToParty/list/export";
        public const string WebApiUrlGetShipToPartyBrokerList = "api/lookups/user/shipToPartybrokerdetails";
        public const string WebApiUrlGetShipToPartyList = "api/employees/shipToParty/List";
        public const string WebApiUrlGetShipToPartyBasedOnState = "api/lookups/user/state/shipToParty";


        #endregion

        #region SalesOrganization 
        public const string WebApiUrlPostAddorUpdateSalesOrganization = "api/master/salesorganization/addorupdate";
        public const string WebApiUrlGetSalesOrganizationList = "api/master/salesorganization/list";
        public const string WebApiUrlGetSalesOrganizationDetailsById = "api/master/get/salesorganizationid";

        #endregion

        #region DistributionChannel 
        public const string WebApiUrlPostAddorUpdateDistributionChannel = "api/master/distributionchannel/addorupdate";
        public const string WebApiUrlGetDistributionChannelList = "api/master/distributionchannel/list";
        public const string WebApiUrlGetDistributionChannelDetailsById = "api/master/get/distributionchannelid";

        #endregion

        #region Master

        public const string WebApiUrlGetDeliveryDetails = "api/master/getconfigdetails";
        public const string WebApiUrlSaveDeliveryDetails = "api/master/add/config";
        public const string WebApiUrlUpdateDeliveryDetails = "api/master/update/config";

        public const string WebApiUrlGetContractDetails = "api/master/getconfigwithcodedetails";
        public const string WebApiUrlPostAddOrUpdateContract = "api/master/addorupdate/configwithcode";

        public const string WebApiUrlGetVerticalDetails = "api/master/vertical/list";
        public const string WebApiUrlPostAddOrUpdateVertical = "api/master/addorupdate/verticals";
        public const string WebApiUrlExportVertical = "api/master/vertical/export";
        public const string WebApiUrlGetVerticalListWithPagination = "api/master/vertical/listwithpagination";
        public const string WebApiUrlGetVerticalById = "api/lookups/getverticals/id";

        public const string WebApiUrlGetOilTypeDetails = "api/master/oiltype/list";
        public const string WebApiUrlPostAddOrUpdateOilType = "api/master/addorupdate/oiltype";
        public const string WebApiUrlExportOilType = "api/master/oiltype/export";
        public const string WebApiUrlGetOilTypeListWithPagination = "api/master/oiltype/listwithpagination";
        public const string WebApiUrlGetOilTypesById = "api/lookups/getoiltypes/id";

        //Plant
        public const string WebApiUrlGetPlantDetails = "api/master/plant/list";
        public const string WebApiUrlPostPlantDetails = "api/master/add/plant";
        public const string WebApiUrlPutPlantDetails = "api/master/update/plant";
        public const string WebApiUrlGetPlantDetailsById = "api/master/plant/details";
        public const string WebApiUrlGetPlantDetailsddl = "api/master/plantlist/ddl";
        public const string WebApiUrlGetPlantDetailbsCitysddl = "api/master/plantlist/ddlbased";
        public const string WebApiUrlExportPlant = "api/master/plant/export";
        public const string WebApiUrlGetPlantListWithPagination = "api/master/plant/listwithpagination";
        public const string WebApiUrlGetPlantListByZH = "api/zh/PlantDepotDetailsByDealer";
        public const string WebApiUrlGetPlantListByNH = "api/NationalHead/PlantDepotDetailsByDealer";
        public const string WebApiUrlGetPlantListByST = "api/mobileDashboard/BDOPlantDepotDetailsByDealer";

        //Depot
        public const string WebApiUrlGetDepotDetails = "api/master/depot/list";
        public const string WebApiUrlPostDepotDetails = "api/master/add/depot";
        public const string WebApiUrlPutDepotDetails = "api/master/update/depot";
        public const string WebApiUrlGetDepotDetailsById = "api/master/depot/details";
        public const string WebApiUrlGetDepotsAndPlants = "api/master/depotplant/list";
        public const string WebApiUrlGetDepotsByPlantId = "api/master/depots/plantid";
        public const string WebApiUrlGetDepotsByPlantIds = "api/master/depots/plantids";
        public const string WebApiUrlExportDepot = "api/master/depot/export";
        public const string WebApiUrlGetDepotListWithPagination = "api/master/depot/listwithpagination";

        //Zone Mapping
        public const string WebApiUrlGetZone = "api/master/zonebyid";
        public const string WebApiUrlGetZoneList = "api/master/zone/list";
        public const string WebApiUrlGetUserRoleList = "api/master/roles/list";
        public const string WebApiUrlGetSubmitFormList = "api/master/Submit/FormList";
        public const string WebApiUrlGetZoneListForDropdown = "api/master/zonelist/ddl";
        public const string WebApiUrlGetZoneStateList = "api/master/zone/statelist";
        public const string WebApiUrlGetStateListByZoneIdForDropdown = "api/master/statelistddl/zoneid";
        public const string WebApiUrlNewZone = "api/master/zone/new";
        public const string WebApiUrlEditZone = "api/master/zone/new";
        public const string WebApiUrlPostZone = "api/master/zone/add";
        public const string WebApiUrlPutZone = "api/master/zone/update";
        public const string WebApiUrlExportZone = "api/master/zone/export";
        public const string WebApiUrlGetStateListByZoneIds = "api/master/statelist/zoneids";

        //Sku Master
        public const string WebApiUrlGetSkuDetailsById = "api/master/get/skuid";
        public const string WebApiUrlPostSaveSku = "api/master/sku/save";
        public const string WebApiUrlPostUpdateSku = "api/master/sku/update";
        public const string WebApiUrlGetSkuList = "api/master/sku/list";
        public const string WebApiUrlGetSkuListWithPagination = "api/master/sku/listwithpagination";

        //State Master
        public const string WebApiUrlGetStateDetailsById = "api/master/state/id";
        public const string WebApiUrlPostSaveState = "api/master/state/save";
        public const string WebApiUrlPostUpdateState = "api/master/state/update";
        public const string WebApiUrlGetStateLists = "api/master/state/list";
        public const string WebApiUrlExportState = "api/master/state/export";
        public const string WebApiUrlGetStateListWithPagination = "api/master/state/listwithpagination";

        //district Master
        public const string WebApiUrlGetDistrictDetailsById = "api/master/district/id";
        public const string WebApiUrlPostSaveDistrict = "api/master/district/save";
        public const string WebApiUrlPostUpdateDistrict = "api/master/district/update";
        public const string WebApiUrlGetDistrictLists = "api/master/district/list";
        public const string WebApiUrlExportDistrict = "api/master/district/export";

        //City Master
        public const string WebApiUrlGetCityDetailsById = "api/master/city/id";
        public const string WebApiUrlPostSaveCity = "api/master/city/save";
        public const string WebApiUrlPostUpdateCity = "api/master/city/update";
        public const string WebApiUrlGetCityLists = "api/master/city/list";
        public const string WebApiUrlExportCity = "api/master/city/export";


        //FreightZone Master
        public const string WebApiUrlGetFreightZoneDetailsById = "api/master/get/freightzoneid";
        public const string WebApiUrlPostSaveFreightZone = "api/master/freightzone/save";
        public const string WebApiUrlPostUpdateFreightZone = "api/master/freightzone/update";
        public const string WebApiUrlGetFreightZoneList = "api/master/freightzone/List";
        public const string WebApiUrlGetFreightZoneListByDepot = "api/master/freightzone/depotid";
        public const string WebApiUrlGetFreightZoneListddl = "api/master/freightzone/ddl";
        public const string WebApiUrlGetFreightZoneListddlByStateZone = "api/master/freightzoneddl/zonestate";
        public const string WebApiUrlGetFreightZoneListddlByStateZoneIds = "api/master/freightzoneddl/zonestateids";
        public const string WebApiUrlGetFreightZoneListByDepotIds = "api/master/freightzone/depotids";
        public const string WebApiUrlExportFreightZone = "api/master/freightzone/export";

        //FreightRoute Master
        public const string WebApiUrlGetFreightRouteDetailsById = "api/master/get/FreightRouteid";
        public const string WebApiUrlPostSaveFreightRoute = "api/master/FreightRoute/save";
        public const string WebApiUrlPostUpdateFreightRoute = "api/master/FreightRoute/update";
        public const string WebApiUrlGetFreightRouteList = "api/master/FreightRoute/List";
        public const string WebApiUrlGetFreightRouteListByZone = "api/master/freightroute/zoneid";
        public const string WebApiUrlExportFreightRoute = "api/master/freightroute/export";

        public const string WebApiUrlGerDistrictBasedOnTerritory = "api/master/district/ddl/territoryIds";
        public const string WebApiUrlGetCityListBasedOnDistrict = "api/master/city/ddl/districtIds";
        public const string WebApiUrlGetFreightRouteByZone = "api/master/freightroutelist";


        public const string WebApiUrlAddVehicleLoadabilities = "api/master/vehicleloadabilities/addorupdate";
        public const string WebApiUrlGetAllVehicleLoadabilities = "api/master/vehicleloadabilities/getAll";
        public const string WebApiUrlGetVehicleLoadabilitiesById = "api/master/vehicleloadabilities/getById";
        public const string WebApiUrlExportVehicleLoadabilitiesList = "api/master/vehicleloadabilities/exportList";



        public const string WebApiUrlGetSalesDocumentType = "api/master/salesdocumenttypeddl";


        public const string WebApiUrlPostSendSms = "api/lookups/sms/send";


        #endregion

        #region Pricing

        public const string WebApiUrlPostSaveMaterialCost = "api/pricing/materialcost/save";
        public const string WebApiUrlPostUpdateMaterialCost = "api/pricing/materialcost/update";
        public const string WebApiUrlGetMaterialCostList = "api/pricing/materialcost/List";
        public const string WebApiUrlGetMaterialCostDetailsById = "api/pricing/get/materialcostid";
        public const string WebApiUrlExportMaterialCost = "api/pricing/materialcost/export";

        public const string WebApiUrlGetRAMaterialCostList = "api/pricing/ramaterialcost/List";
        public const string WebApiUrlPostSaveRAMaterialCost = "api/pricing/ramaterialcost/save";
        public const string WebApiUrlPostUpdateRAMaterialCost = "api/pricing/ramaterialcost/update";
        public const string WebApiUrlGetRAMaterialCostDetailsById = "api/pricing/get/ramaterialcostid";



        public const string WebApiUrlPostSavePackingCost = "api/pricing/packingcost/save";
        public const string WebApiUrlPostUpdatePackingCost = "api/pricing/packingcost/update";
        public const string WebApiUrlGetPackingCostList = "api/pricing/packingcost/List";
        public const string WebApiUrlGetPackingCostDetailsById = "api/pricing/get/packingcostid";
        public const string WebApiUrlExportPackingCost = "api/pricing/packingcost/export";

        public const string WebApiUrlPostSavePrimaryFreight = "api/pricing/primaryfreight/save";
        public const string WebApiUrlPostUpdatePrimaryFreight = "api/pricing/primaryfreight/update";
        public const string WebApiUrlGetPrimaryFreightList = "api/pricing/primaryfreight/List";
        public const string WebApiUrlGetPrimaryFreightDetailsById = "api/pricing/get/primaryfreightid";
        public const string WebApiUrlExportPrimaryFreight = "api/pricing/primaryfreight/export";

        public const string WebApiUrlPostSaveSecondaryFreight = "api/pricing/secondaryfreight/save";
        public const string WebApiUrlPostUpdateSecondaryFreight = "api/pricing/secondaryfreight/update";
        public const string WebApiUrlGetSecondaryFreightList = "api/pricing/secondaryfreight/List";
        public const string WebApiUrlGetSecondaryFreightDetailsById = "api/pricing/get/secondaryfreightid";
        public const string WebApiUrlExportSecondaryFreight = "api/pricing/secondaryfreight/export";

        public const string WebApiUrlPostSaveDepotCost = "api/pricing/depotcost/save";
        public const string WebApiUrlPostUpdateDepotCost = "api/pricing/depotcost/update";
        public const string WebApiUrlGetDepotCostList = "api/pricing/depotcost/List";
        public const string WebApiUrlGetDepotCostDetailsById = "api/pricing/get/depotcostid";
        public const string WebApiUrlExportDepotCost = "api/pricing/depotcost/export";

        public const string WebApiUrlPostSaveDetentionCost = "api/pricing/detentioncost/save";
        public const string WebApiUrlPostUpdateDetentionCost = "api/pricing/detentioncost/update";
        public const string WebApiUrlGetDetentionCostList = "api/pricing/detentioncost/List";
        public const string WebApiUrlGetDetentionCostDetailsById = "api/pricing/get/detentioncostid";
        public const string WebApiUrlExportDetentionCost = "api/pricing/detentioncost/export";

        public const string WebApiUrlPostSaveHoneycombCost = "api/pricing/honeycombcost/save";
        public const string WebApiUrlPostUpdateHoneycombCost = "api/pricing/honeycombcost/update";
        public const string WebApiUrlGetHoneycombCostList = "api/pricing/honeycombcost/List";
        public const string WebApiUrlHoneycombCostExport = "api/pricing/honeycombcost/export";
        public const string WebApiUrlGetHoneycombCostDetailsById = "api/pricing/get/honeycombcostid";

        public const string WebApiUrlPostSaveLoadCapacityConversion = "api/pricing/loadcapacityconversion/save";
        public const string WebApiUrlPostUpdateLoadCapacityConversion = "api/pricing/loadcapacityconversion/update";
        public const string WebApiUrlGetLoadCapacityConversionList = "api/pricing/loadcapacityconversion/List";
        public const string WebApiUrlGetLoadCapacityConversionDetailsById = "api/pricing/get/loadcapacityid";
        public const string WebApiUrlExportLoadCapacityConversion = "api/pricing/loadcapacityconversion/export";

        public const string WebApiUrlPostSaveProfitMargin = "api/pricing/profitmargin/save";
        public const string WebApiUrlPostUpdateProfitMargin = "api/pricing/profitmargin/update";
        public const string WebApiUrlGetProfitMarginList = "api/pricing/profitmargin/List";
        public const string WebApiUrlProfitMarginExport = "api/pricing/profitmargin/export";
        public const string WebApiUrlGetProfitMarginDetailsById = "api/pricing/get/profitmarginid";

        public const string WebApiUrlPostSaveCushionMargin = "api/pricing/cushionmargin/save";
        public const string WebApiUrlPostUpdateCushionMargin = "api/pricing/cushionmargin/update";
        public const string WebApiUrlGetCushionMarginList = "api/pricing/cushionmargin/List";
        public const string WebApiUrlCushionMarginExport = "api/pricing/cushionmargin/export";
        public const string WebApiUrlGetCushionMarginDetailsById = "api/pricing/get/cushionmarginid";

        public const string WebApiUrlPostSaveRaMargin = "api/pricing/ramargin/save";
        public const string WebApiUrlGetRaMarginList = "api/pricing/ramargin/list";
        public const string WebApiUrlRaMarginExport = "api/pricing/ramargin/export";
        public const string WebApiUrlGetRaMarginDetailsById = "api/pricing/get/ramarginid";
        public const string WebApiUrlPostUpdateRaMargin = "api/pricing/ramargin/update";

        public const string WebApiUrlPostSaveSchemeCost = "api/pricing/schemecost/save";
        public const string WebApiUrlPostUpdateSchemeCost = "api/pricing/schemecost/update";
        public const string WebApiUrlGetSchemeCostList = "api/pricing/schemecost/List";
        public const string WebApiUrlGetSchemeCostDetailsById = "api/pricing/get/schemecostid";
        public const string WebApiUrlExportSchemeCost = "api/pricing/schemecost/export";


        public const string WebApiUrlPostSaveOilTransferCost = "api/pricing/oiltransfercost/save";
        public const string WebApiUrlPostUpdateOilTransferCost = "api/pricing/oiltransfercost/update";
        public const string WebApiUrlGetOilTransferCostList = "api/pricing/oiltransfercost/List";
        public const string WebApiUrlGetOilTransferCostDetailsById = "api/pricing/get/oiltransfercostid";

        public const string WebApiUrlPostSaveAdditionalCost = "api/pricing/additionalcost/save";
        public const string WebApiUrlPostUpdateAdditionalCost = "api/pricing/additionalcost/update";
        public const string WebApiUrlGetAdditionalCostList = "api/pricing/additionalcost/List";
        public const string WebApiUrlGetAdditionalCostDetailsById = "api/pricing/get/additionalid";

        //Role Discount
        public const string WebApiUrlPutRoleDiscount = "api/pricing/update/rolediscount";
        public const string WebApiUrlGetRoleDiscountAll = "api/pricing/get/rolediscountall";
        public const string WebApiUrlGetRoleDiscountById = "api/pricing/get/rolediscountbyid";

        //Sku Depot Discount
        public const string WebApiUrlPostSkuDepotDiscount = "api/pricing/add/skudepotdiscount";
        public const string WebApiUrlUpdateSkuDepotDiscount = "api/pricing/update/skudepotdiscount";
        public const string WebApiUrlGetSkuDepotDiscountAll = "api/pricing/get/skudepotdiscountall";
        public const string WebApiUrlGetSkuDepotDiscountById = "api/pricing/get/skudepotdiscountbyid";

        public const string WebApiUrlGetOilTypeDetailsddl = "api/pricing/getoiltypedetailsddl";
        public const string WebApiUrlGetDepotDetailsddl = "api/pricing/getdepotdetailsddl";
        public const string WebApiUrlGetUserDetailsddl = "api/pricing/getuserdetailsddl";
        public const string WebApiUrlGetSkuDetailsddl = "api/pricing/getskudetailsddl";
        public const string WebApiUrlGetSkuDropdownById = "api/pricing/getskudetailsbyId";

        //Ingredient Cost
        public const string WebApiUrlGetIngredientsCostAll = "api/pricing/get/ingredientscostall";
        public const string WebApiUrlAddIngredientCost = "api/pricing/post/addingredientcost";
        public const string WebApiUrlUpdateIngredientCost = "api/pricing/post/updateingredientcost";
        public const string WebApiUrlGetIngredientsCostbyId = "api/pricing/get/ingredientscostbyid";
        public const string WebApiUrlExportIngredientCost = "api/pricing/ingredientcost/export";

        //Sku Ingredient
        public const string WebApiUrlGetSkuIngredientsAll = "api/pricing/get/skuingredientsall";
        public const string WebApiUrlAddSkuIngredients = "api/pricing/post/addskuingredients";
        public const string WebApiUrlUpdateSkuIngredients = "api/pricing/post/updateskuingredients";
        public const string WebApiUrlGetSkuIngredientsbyId = "api/pricing/get/skuingredientsbyid";
        public const string WebApiUrlGetSkuIngredients = "api/pricing/get/skuingredients";
        public const string WebApiUrlGetSkuIngredientsListForExport = "api/pricing/skuIngredientsExport";

        //Role Discount
        public const string WebApiUrlPostAdminRoleDiscount = "api/pricing/post/adminrolediscount";
        public const string WebApiUrlUpdateAdminRoleDiscount = "api/pricing/update/adminrolediscount";
        public const string WebApiUrlGetAdminDiscountById = "api/pricing/get/adminrolediscountbyid";
        public const string WebApiUrlGetAdminRolediscountall = "api/pricing/get/adminrolediscountall";


        //OilTypes and Skus
        public const string WebApiUrlGetRequestDiscountbyId = "api/pricing/get/requestdiscountdetails";
        public const string WebApiUrlUpdateRequestDiscount = "api/pricing/update/requestdiscount";
        public const string WebApiUrlGetRequestDiscountsAll = "api/pricing/get/requestdiscountall";
        public const string WebApiUrlGetRequestDiscountBaseSkuId = "api/pricing/get/requestdiscountbaseskuid";
        public const string WebApiUrlGetRequestDiscountDetailsById = "api/pricing/get/requestdiscountdetailsbyid";

        //Approve Discount
        public const string WebApiUrlGetRequestedDiscounts = "api/pricing/get/getrequesteddiscounts";
        public const string WebApiUrlPostApproveDiscount = "api/pricing/post/approverequestdiscount";

        //PriceNotifyConfiguration
        public const string WebApiUrlGetIncoTermList = "api/lookups/IncoTerm/list";

        #endregion

        #region Lookup

        public const string WebApiUrlGetStateList = "api/lookups/state/list";
        public const string WebApiUrlGetStateListByEmployees = "api/lookups/statebyemployeid/list";
        public const string WebApiUrlGetActiveStateListBasedOnZonalHeadIds = "api/lookups/active/statelist/ZonalHeadId";
        public const string WebApiUrlGetOilPackingTypeList = "api/lookups/oilpackingtype/list";
        public const string WebApiUrlGetOilPackingGroupTypeList = "api/lookups/oilpackinggrouptype/list";
        public const string WebApiUrlGetDistrictListByStateId = "api/lookups/districts/stateid";
        public const string WebApiUrlGetCityListByDistrictId = "api/lookups/cities/districtid";
        public const string WebApiUrlGetCityListByStateId = "api/lookups/cities/stateid";
        public const string WebApiUrlGetCityListByDistrictIdForDropdown = "api/lookups/cityddl/districtid";
        public const string WebApiUrlGetUnMappedDistrictListByStateId = "api/lookups/unmappeddistrict/stateid";
        public const string WebApiUrlGetPackGroupListBySkuId = "api/lookups/oilpackingtype/skuid";
        public const string WebApiUrlGetSkuListByPackGroupId = "api/lookups/skulist/packtypeid";
        public const string WebApiUrlGetOilTypeList = "api/lookups/active/oiltype/list";
        public const string WebApiUrlGetDealersList = "api/lookups/dealers/list";


        //Ingredients - Dropdown Details
        public const string WebApiUrlGetIngredientCostddl = "api/lookups/ingredient/list";
        public const string WebApiUrlGetSkuIngredienOilTypes = "api/lookups/skuingredientoiltypes/verticalid";

        //OilTypes and Skus
        public const string WebApiUrlGetOilTypesBasedOnVerticalId = "api/lookups/oiltypes/verticalid";
        //get oiltypes based on vertical if there is vertical id or gets all oiltypes
        public const string WebApiUrlGetOilTypesBasedOnVertical = "api/lookups/oiltypes/vertical";
        public const string WebApiUrlGetSkusBasedOnOilTypeId = "api/lookups/skus/oiltypeid";
        public const string WebApiUrlGetSkuBasedOnOilTypeSubCategory = "api/lookups/skus/oiltypesubcategory";
        public const string WebApiUrlGetSkuListBasedOnOilTypeIdSubCategoryIdPackGroupIdForDropdown = "api/lookups/skuddl/OiltypeIdSubcategoryIdPackgroupId";
        public const string WebApiUrlGetOilTypeIsRasoiOrNot = "api/lookups/skus/oiltyperasoi";
        public const string WebApiUrlGetOilTypesByVerticalId = "api/lookups/getoiltypes/verticalid";
        public const string WebApiUrlGetOilTypeListByVerticalIdListForDropDown = "api/lookups/oilType/ddl/verticalIds";
        public const string WebApiUrlGetOilPackingTypeListForDropdown = "api/lookups/oilPackingType/ddl";
        public const string WebApiUrlGetVerticalListForDropdown = "api/lookups/Vertical/ddl";
        public const string WebApiUrlGetSkuListByOilTypeIdsPackGroupIdsForDropdown = "api/lookups/sku/ddl/OiltypeIdsAndPackGroupIds";
        public const string WebApiUrlGetSkuBasedOnCombination = "api/lookups/skulist/basedoncombination";
        public const string WebApiUrlGetSkuBasedOnOiltypeCombination = "api/lookups/skulist/basedonoiltypecombination";

        public const string WebApiUrlGetUomList = "api/lookups/uom/list";

        public const string WebApiUrlGetPlantDepotRakeByStateId = "api/lookups/getplantdepotrake/stateid";
        public const string WebApiUrlGetFreightZoneByStateId = "api/lookups/getfreightzone/stateid";

        public const string WebApiUrlGetStatesBasedOnCustomerGroupId = "api/lookups/states/customergroupid";
        //Users
        public const string WebApiUrlGetUsersByRoleIdddl = "api/lookups/users/roleid";
        //Customer
        public const string WebApiUrlGetCustomerByCityIds = "api/lookups/Customer/cityids";

        //Configuration
        public const string WebApiUrlGetConfigurationList = "api/lookups/configuration/list";
        public const string WebApiUrlPostUpdateConfigurationList = "api/lookups/configuration/update";

        //ZonalHeadList
        public const string WebApiUrlGetZonalHeadList = "api/lookups/ZonalTrader/list";
        public const string WebApiUrlGetZonalHeadListNew = "api/lookups/ZonalTrader/listnew";
        public const string WebApiUrlGetNationalHeadList = "api/lookups/NationalTrader/list";
        public const string WebApiUrlGetZonalHeadListByNH = "api/lookups/ZHlist/NationalTrader";
        public const string WebApiUrlGetZonalHeadListByNationalHead = "api/NationalTrader/ZH/list";
        public const string WebApiUrlGetZHBasedOnVertical = "api/lookups/ZonalTrader/vertical";

        //BDOListBasedOnZonalHead
        public const string WebApiUrlGetBDOBasedOnZonalHead = "api/lookups/BDOlist/ZonalTrader";
        public const string WebApiUrlGetZonalHeadBasedonZH = "api/lookups/ddl/zhbasedonNH";
        public const string WebApiUrlGetZonalHeadBasedonZHComb = "api/lookups/ddl/zhbasedonNHComb";

        //DealerListBasedOnBDO
        public const string WebApiUrlGetDealerBasedOnBdo = "api/lookups/Dealerlist/StateTrader";
        public const string WebApiUrlGetDealerCodeBasedOnBdo = "api/lookups/DealerCodelist/StateTrader";


        //Notification
        public const string WebApiUrlGetBdoddlList = "api/lookups/GetBdoddl/list";
        public const string WebApiUrlGetDealerListBasedOnBDO = "api/lookups/DealerList/byBdoIds";
        public const string WebApiUrlPostUpdateNotification = "api/lookups/Notification/update";
        public const string WebApiUrlPostAddNotification = "api/lookups/Notification/add";
        public const string WebApiUrlGetTPNotificationList = "api/lookups/TPNotification/list";
        public const string WebApiUrlGetTPNotificationDetails = "api/lookups/TPNotification/details/id";
        public const string WebApiUrlGetTPNotificationById = "api/lookups/TPNotification/ById";
        public const string WebApiUrlGetMappedDealerListByRaNotificationId = "api/lookups/dealer/List/ByTPNotificationId";
        public const string WebApiUrlGetTPNotificationExport = "api/lookups/TPNotification/Export";

        //SaudaConversionType
        public const string WebApiUrlGetSaudaConversionTypeList = "api/lookups/SaudaConversionType/list";
        public const string WebApiUrlPostUpdateSaudaConversionTypeList = "api/lookups/SaudaConversionType/update";
        //public const string WebApiUrlPostSaudaConversionDetailList = "api/mobileDealersauda/saudaconversion/pendingapprovedlist/StateTrader";
        public const string WebApiUrlPostSaudaConversionDetailList = "api/mobileDealersauda/saudaconversion/pendingapprovedlist";
        public const string WebApiUrlPostSaudaConversionDetailsById = "api/mobileDealersauda/saudaconversion/get/skudetails";
        public const string WebApiUrlGetSkuListByPackGrpId = "api/mobilesauda/skulist/packgroupid";
        public const string WebApiUrlAddConversionUnitandDiffRate = "api/mobilesauda/conversion/unitdnddifferencerate/add";


        //Sauda Extension
        public const string WebApiUrlAddSaudaExtensionPolicy = "api/lookups/saudaExtension/add";
        public const string WebApiUrlListSaudaExtensionPolicy = "api/lookups/saudaExtension/list";
        public const string WebApiUrlListSaudaExtensionDetail = "api/sauda/saudaExtensionDetails/inweb";

        //Delete List creation
        public const string WebApiUrlListDeleteRemark = "api/lookups/deleteremarks/list";
        public const string WebApiUrlAddDeleteListRemark = "api/lookups/deleteremarks/add";

        //configuration for sauda validity based on verticals and email configuration for sauda report
        public const string WebApiUrlPostSaudaValidityAndSaudaReportMailConfiguration = "api/lookups/configuration/saveforsaudavalidityandsaudareportmails";
        public const string WebApiUrlGetVerticalListBasedOnSaudaValidity = "api/lookups/verticallist/basedonsaudavalidity";
        public const string WebApiUrlGetVerticalListAndMails = "api/lookups/verticallistandmails";

        //sauda booking configuration
        public const string WebApiUrlPostSaudaBookingConfiguration = "api/lookups/saudabooking/configuration";
        public const string WebApiUrlGetSaudaBookingConfigurationDetails = "api/lookups/saudabookingconfiguration/details";

        #endregion

        #region SalesTourPlan
        //Financial Year
        public const string WebApiUrlPostFinancialYear = "api/SalesTourPlan/financialyear/add";
        public const string WebApiUrlPutFinancialYear = "api/SalesTourPlan/financialyear/update";
        public const string WebApiUrlViewFinancialYear = "api/SalesTourPlan/financialyear/view";
        public const string WebApiUrlGetFinancialYearList = "api/SalesTourPlan/financialyear";
        public const string WebApiUrlGetActiveFinancialYearList = "api/SalesTourPlan/financialyear/active";

        #region Permanent Journey Plan
        public const string WebApiUrlGetApprovalFlowList = "api/SalesTourPlan/PJP/ApprovalFlow";
        public const string WebApiUrlPostAddPermanentJourneyPlan = "api/SalesTourPlan/PJP/add";
        public const string WebApiUrlPostPermanentJourneyPlanList = "api/SalesTourPlan/PJP/PermanentJourneyPlanList";
        public const string WebApiUrlPostPermanentJourneyPlanDetails = "api/SalesTourPlan/PJP/PermanentJourneyPlanDetail";
        public const string WebApiUrlPostUpdatePermanentJourneyPlan = "api/SalesTourPlan/PJP/update";
        public const string WebApiUrlPostPendingPermanentJourneyPlanList = "api/SalesTourPlan/PJP/PendingPermanentJourneyPlanList";
        public const string WebApiUrldateweekdetails = "api/SalesTourPlan/dateweekdetails";
        public const string WebApiUrlGetCityList = "api/SalesTourPlan/cities";
        public const string WebApiUrlGetPJPMonthList = "api/SalesTourPlan/PJP/Months";
        public const string WebApiUrlPostApprovedPermanentJourneyPlanByUser = "api/SalesTourPlan/PJP/ApprovedPermanentJourneyPlanByUser";
        public const string WebApiUrlPostMonthsByUserPermanentJourneyPlan = "api/SalesTourPlan/PJP/MonthsByUserPermanentJourneyPlan";
        public const string WebApiUrlPostDealersByUserPermanentJourneyPlan = "api/SalesTourPlan/PJP/DealersByUserPermanentJourneyPlan";
        public const string WebApiUrlPostNoVisitByUserPermanentJourneyPlan = "api/SalesTourPlan/PJP/NoVisitByUserPermanentJourneyPlan";
        public const string WebApiUrlGetApprovedOrRejectedPJPList = "api/SalesTourPlan/PJP/GetApprovedOrRejectedPJPList";
        public const string WebApiUrlGetCurrenntFinancialYear = "api/master/CurrentFinancialYear";

        #endregion

        #region HeadQuarters
        public const string WebApiUrlPostHeadQuarters = "api/SalesTourPlan/headquarters/add";
        public const string WebApiUrlPutHeadQuarters = "api/SalesTourPlan/headquarters/update";
        public const string WebApiUrlViewHeadQuarters = "api/SalesTourPlan/headquarters/view";
        public const string WebApiUrlGetHeadQuarters = "api/SalesTourPlan/headquarters";
        public const string WebApiUrlGetActiveHeadQuarters = "api/SalesTourPlan/headquarters/active";
        public const string WebApiUrlExportHeadQuarters = "api/SalesTourPlan/headquarters/export";

        #endregion

        #region Monthly Tour Plan
        public const string WebApiUrlPostAddMonthlyTourPlan = "api/SalesTourPlan/MTP/add";
        public const string WebApiUrlPostUpdateMonthlyTourPlan = "api/SalesTourPlan/MTP/update";
        public const string WebApiUrlPostMonthlyTourPlanDetails = "api/SalesTourPlan/MTP/MonthlyTourPlanDetails";
        public const string WebApiUrlPostMonthlyTourPlanList = "api/SalesTourPlan/MTP/MonthlyTourPlanList";
        public const string WebApiUrlPostPendingMonthlyTourPlanList = "api/SalesTourPlan/MTP/PendingMonthlyTourPlanList";
        public const string WebApiUrlPostMonthlyTourPlanDateCalendar = "api/SalesTourPlan/MTP/MonthlyTourPlanDateCalendar";
        public const string WebApiUrlPostCityByUserPermanentJourneyPlan = "api/SalesTourPlan/MTP/CityByUserPermanentJourneyPlan";

        #endregion

        #region Monthly Tour Plan Deviation
        public const string WebApiUrlPostApprovedMonthlyTourPlanDetailsByUser = "api/SalesTourPlan/MTPDeviation/ApprovedMonthlyTourPlanDetailsByUser";
        public const string WebApiUrlPostApprovedMonthlyTourPlanByUser = "api/SalesTourPlan/MTPDeviation/ApprovedMonthlyTourPlanByUser";
        public const string WebApiUrlPostAddMonthlyPlanDeviation = "api/SalesTourPlan/MTPDeviation/add";
        public const string WebApiUrlPostPendingMonthlyPlanDeviation = "api/SalesTourPlan/MTPDeviation/PendingMonthlyPlanDeviation";
        public const string WebApiUrlPostApprovedMonthlyPlanDeviation = "api/SalesTourPlan/MTPDeviation/ApprovedMonthlyPlanDeviation";
        public const string WebApiUrlPostUpdateMonthlyPlanDeviation = "api/SalesTourPlan/MTPDeviation/UpdateMonthlyPlanDeviation";
        public const string WebApiUrlGetApprovedOrRejectedMTPList = "api/SalesTourPlan/PJP/GetApprovedOrRejectedMTPList";

        public const string WebApiUrlCheckMonthlyPlanDeviationApproveByLoginedUser = "api/SalesTourPlan/MTPDeviation/CheckApproveUserMonthlyPlanDeviation";

        #endregion

        #region User Attendence
        public const string WebApiUrlGetUserAttendenceList = "api/SalesTourPlan/user/Attendence";

        #endregion

        #region Reasons
        public const string WebApiUrlPostReasons = "api/SalesTourPlan/reasons/add";
        public const string WebApiUrlPutReasons = "api/SalesTourPlan/reasons/update";
        public const string WebApiUrlViewReasons = "api/SalesTourPlan/reasons/view";
        public const string WebApiUrlGetReasons = "api/SalesTourPlan/reasons";
        public const string WebApiUrlGetActiveReasons = "api/SalesTourPlan/reasons/active";
        public const string WebApiUrlGetDealer = "api/SalesTourPlan/dealer/id";

        #endregion

        #region User Sauda and Sales Target
        public const string WebApiUrlPostMonthsByFinancialYear = "api/SalesTourPlan/SaudaSalesTarget/MonthsByFinancialYear";
        public const string WebApiUrlPostAddSaudaSalesTarget = "api/SalesTourPlan/SaudaSalesTarget/add";
        public const string WebApiUrlPostUpdateSaudaSalesTarget = "api/SalesTourPlan/SaudaSalesTarget/update";
        public const string WebApiUrlPostViewSaudaSalesTarget = "api/SalesTourPlan/SaudaSalesTarget/view";
        public const string WebApiUrlGetListaudaSalesTarget = "api/SalesTourPlan/SaudaSalesTarget/list";
        public const string WebApiUrlPostListDetailSaudaSalesTarget = "api/SalesTourPlan/SaudaSalesTarget/listdetail";

        #endregion

        #region User OilType Target
        public const string WebApiUrlGetMonthAndYearByFinancialYear = "api/SalesTourPlan/oiltypetarget/monthandyear";
        public const string WebApiUrlGetOilTypeTargetList = "api/SalesTourPlan/oiltypetarget/list";
        public const string WebApiUrlGetOilTypeTargetDetailList = "api/SalesTourPlan/oiltypetarget/listdetail";
        public const string WebApiUrlPostAddOilTypeTarget = "api/SalesTourPlan/oiltypetarget/add";
        public const string WebApiUrlPostUpdateUserOilTypeTarget = "api/SalesTourPlan/oiltypetarget/update";
        public const string WebApiUrlPostGetUserOiltypeTargetdetailbyId = "api/SalesTourPlan/oiltypetarget/details";

        #endregion

        #region User UserCustomerSalesTarget
        public const string WebApiUrlGetUserSalesTargetList = "api/SalesTourPlan/usercustomersalestarget/list";
        public const string WebApiUrlGetUserSalesTargetDetailList = "api/SalesTourPlan/usercustomersalestarget/listdetail";
        public const string WebApiUrlPostAddUserSalesTarget = "api/SalesTourPlan/usercustomersalestarget/add";
        public const string WebApiUrlPostUpdateUserSalesTarget = "api/SalesTourPlan/usercustomersalestarget/update";
        public const string WebApiUrlPostGetUserSalesTargetdetailbyId = "api/SalesTourPlan/usercustomersalestarget/details";
        public const string WebApiUrlPostSaveUserSalesTargetList = "api/SalesTourPlan/usercustomersalestarget/addlist";
        public const string WebApiUrlGetAssignedSalesTargetList = "api/SalesTourPlan/usercustomersalestarget/assignedlist";
        public const string WebApiUrlGetSalesTargetOilTypeList = "api/SalesTourPlan/usercustomersalestarget/oiltypelist";
        public const string WebApiUrlGetOilTypesBasedOnAssignedSalesTarget = "api/SalesTourPlan/salestarget/oiltypelist";

        #endregion

        #region User UserCustomerSaudaTarget
        public const string WebApiUrlGetUserCustomerSaudaTargetList = "api/SalesTourPlan/usercustomersaudatarget/list";
        public const string WebApiUrlGetUserCustomerSaudaTargetDetailList = "api/SalesTourPlan/usercustomersaudatarget/listdetail";
        public const string WebApiUrlPostAddUserCustomerSaudaTarget = "api/SalesTourPlan/usercustomersaudatarget/add";
        public const string WebApiUrlPostUpdateUserCustomerSaudaTarget = "api/SalesTourPlan/usercustomersaudatarget/update";
        public const string WebApiUrlPostGetUserCustomerSaudaTargetdetailbyId = "api/SalesTourPlan/usercustomersaudatarget/details";

        public const string WebApiUrlPostSaveUserCustomerSaudaTargetList = "api/SalesTourPlan/usercustomersaudatarget/addlist";
        public const string WebApiUrlGetAssignedSaudaTargetList = "api/SalesTourPlan/usercustomersaudatarget/assignedlist";
        public const string WebApiUrlGetSaudaTargetOilTypeList = "api/SalesTourPlan/usercustomersaudatarget/oiltypelist";
        public const string WebApiUrlGetOilTypesBasedOnAssignedSaudaTarget = "api/SalesTourPlan/saudatarget/oiltypelist";

        #endregion

        #region User UserCustomerTarget
        public const string WebApiUrlPostSaveUserCustomerTargetList = "api/SalesTourPlan/usercustomertarget/addlist";
        public const string WebApiUrlGetUserTargetLists = "api/SalesTourPlan/usercustomertarget/list";
        public const string WebApiUrlGetUserTargetDetailList = "api/SalesTourPlan/usercustomertarget/listdetail";
        public const string WebApiUrlGetAssignedTargetList = "api/SalesTourPlan/usercustomertarget/assignedlist";
        public const string WebApiUrlPostGetUserTargetdetailbyId = "api/SalesTourPlan/usercustomertarget/details";
        public const string WebApiUrlPostUpdateUserCustomerTargetList = "api/SalesTourPlan/usercustomertarget/update";
        #endregion


        //STP History
        public const string WebApiUrlGetSalesTourPlanPcpHistory = "api/SalesTourPlan/pcp/history";
        public const string WebApiUrlGetSalesTourPlanMtpHistory = "api/SalesTourPlan/mtp/history";

        #endregion

        #region Reverse Auction 

        public const string WebApiUrlPostBidWindowTiming = "api/reverseauction/bidwindowtiming/addorupdate";
        public const string WebApiUrlGetBiddingWindowTimingList = "api/reverseauction/bidwindowtiming/list";
        public const string WebApiUrGetBiddingWindowTimingById = "api/reverseauction/bidwindowtiming/id";
        public const string WebApiUrlGetBiddingWindowTimingListddl = "api/reverseauction/bidwindowtiming/ddl";
        public const string WebApiUrlGetBiddingWindowTimingListByDateddl = "api/reverseauction/bidwindowtimingddl/biddingdate";



        public const string WebApiUrlPostTicker = "api/reverseauction/ticker/addorupdate";
        public const string WebApiUrlGetTickerList = "api/reverseauction/ticker/list";
        public const string WebApiUrGetTickerById = "api/reverseauction/ticker/id";
        public const string WebApiUrlPostTicketSaudaUnmapping = "api/tradeticket/saudaunmapping";
        //public const string WebApiUrlGetDistrictListByStateId = "api/lookups/districts/stateid";
        //public const string WebApiUrlGetCityListByDistrictName = "api/lookups/cities/districtname";
        public const string WebApiUrlGetDealersListByStateId = "api/tradeticket/dealers/stateid";

        #endregion

        #region Request Discount        
        public const string WebApiUrlGetRequestDiscountList = "api/pricing/get/requestdiscountlist";

        #endregion

        #region OilTypes and Skus Dropdown
        public const string WebApiUrlGetOilTypesBasedOnVerticle = "api/lookups/get/oiltypebasedonverticle";
        public const string WebApiUrlGetSkusBasedOnOilType = "api/lookups/skus/skubasedonoiltype";
        public const string WebApiUrlGetSkusBasedOnEmployeeDiscount = "api/lookups/skus/skubasedonemployeediscount";

        #endregion

        #region Premium Discount        
        public const string WebApiUrlPostPremiumDiscount = "api/pricing/premium/addpremium";
        public const string WebApiUrlUpdatePremiumDiscount = "api/pricing/premium/updatepremium";
        public const string WebApiUrlGetPremiumById = "api/pricing/premium/premiumbyid";
        public const string WebApiUrlGetPremiumall = "api/pricing/premium/premiumlist";

        #endregion

        #region Premium Request Discount                
        public const string WebApiUrlUpdatePremiumRequestDiscount = "api/pricing/premium/updatepremiumrequest";
        public const string WebApiUrlGetPremiumRequestById = "api/pricing/premium/premiumrequestbyid";
        public const string WebApiUrlGetPremiumRequestall = "api/pricing/premium/premiumreqestlist";
        public const string WebApiUrlGetSkuPremiumDiscountRequestById = "api/pricing/premium/skubasepremiundiscount";

        #endregion

        #region Approve Pending Request
        public const string WebApiUrlGetPremiumDiscountForPending = "api/pricing/premium/approvepremiumrequestlist";
        public const string WebApiUrlPostApprovePremiumDiscount = "api/pricing/premium/approvepremiumrequestupdate";

        #endregion

        #region Primary Discount Users
        public const string WebApiUrlPostAddPrimaryDiscountForUser = "api/pricing/premiumuser/addpremiumuser";
        public const string WebApiUrlPostUpdatePrimaryDiscountForUser = "api/pricing/premiumuser/updatepremiumuser";
        public const string WebApiUrlGetGetPrimaryDiscountForUserList = "api/pricing/premiumuser/premiumuserlist";
        public const string WebApiUrlGetGetPrimaryDiscountForUserById = "api/pricing/premiumuser/premiumuserbyid";

        #endregion

        #region Final Pricing         
        public const string WebApiUrlFinalPrice = "api/finalprice/list/admin";
        public const string WebApiUrlFinalPriceNew = "api/finalprice/list/adminnew";
        public const string WebApiUrlSaveTraditionaFinalPrice = "api/finalprice/traditional/save";
        public const string WebApiUrlSaveReverseAucationFinalPrice = "api/finalprice/reverseauction/save";
        public const string WebApiUrlGetPublishedPriceDetails = "api/finalprice/publishedprice/list";
        public const string WebApiUrlFinalPriceGenerate = "api/finalprice/pricegenerate/queue";
        public const string WebApiUrlPublishFinalPrice = "api/finalprice/publish";
        public const string WebApiUrlGetPublishedFinalPriceList = "api/finalprice/price/list";
        public const string WebApiUrlGetPublishedFinalPriceErrorList = "api/finalprice/publishedprice/errorlist";

        public const string WebApiUrlStateBasePublishFinalPrice = "api/finalprice/publish/state";

        #endregion

        #region New Final Price Url
        public const string WebApiUrlFinalPriceGenerateSave = "api/finalprice/pricegenerate/save";
        public const string WebApiUrlGetGeneratedPriceAsync = "api/finalprice/getpricegenerate";
        public const string WebApiUrlGetGeneratedPriceList = "api/mobileDealerDashboard/dailyrate/web";
        //public const string WebApiUrlGetGeneratedPriceList = "api/finalprice/getpricegeneratelist";
        public const string WebApiUrlGetGetPriceGenerateDetails = "api/finalprice/getpricegenerate/details";
        public const string WebApiUrlGetStateBasePublishedFinalPriceList = "api/finalprice/price/state/list";
        public const string WebApiUrlStateBaseGetPublishedFinalPriceErrorList = "api/finalprice/publishedprice/state/errorlist";

        #endregion

        #region Sauda

        public const string WebApiUrlGetAllSaudhaList = "api/sauda/admin/listweb";
        public const string WebApiUrlGetSaudhaDetails = "api/sauda/admin/details";
        public const string WebApiUrlUpdateSaudaDetails = "api/sauda/status/change";
        public const string WebApiUrlUpdateSaudaForLoose = "api/sauda/status/changeForLoose";
        public const string WebApiUrlSaudaConversionReprocess = "api/sauda/saudaconversion/reprocess";
        public const string WebApiUrlSaudaConversionReject = "api/sauda/saudaconversion/reject";
        public const string WebApiUrlSaudaExtensionReprocess = "api/sauda/saudaextension/reprocess";
        public const string WebApiUrlLiftingRequestReprocess = "api/sauda/lifting/reprocess";
        public const string WebApiUrlPostGetSaudaLimitRequestHistory = "api/sauda/saudalimit/SaudaLimitsRequestHistory";
        public const string WebApiUrlApproveorRejectSaudaLimitRequest = "api/sauda/saudalimit/ApproveorReject";
        public const string WebApiUrlPostGetSpecialRateRequestHistory = "api/sauda/specialRate";
        public const string WebApiUrlApproveorRejectSpecialRateRequest = "api/sauda/specialRate/ApproveorReject";
        public const string WebApiUrlGetSaudhaBookingTypes = "api/master/sauda/bookingtypes";
        public const string WebApiUrlGetMaterialTypes = "api/master/materialTypes";
        public const string WebApiUrlOilTypes = "api/master/oiltypes";
        public const string WebApiUrlGetTradeTicketSaudaOrdersMappingList = "api/sauda/tradeTicket/saudaOrdersList";
        public const string WebApiUrlGetSaudaOrdersTradeTicketMappingDetails = "api/sauda/saudaOrdersMappingDetails";

        public const string WebApiUrlPostUpdateSaudaDetails = "api/sauda/saudadetails/update";

        public const string WebApiUrlGetSaudaRequestDetails = "api/sauda/admin/saudaorderdetails";
        public const string WebApiUrlProfileImage = "api/master/profileImage";
        #endregion

        #region Trade Ticket
        public const string WebApiUrlListTradeTicket = "api/tradeticket/request/list";
        public const string WebApiUrlGetTradeTicket = "api/tradeticket/request/details";
        public const string WebApiUrlCreateTradeTicket = "api/tradeticket/request/creation";
        public const string WebApiUrlUpdateTradeTicket = "api/tradeticket/request/modification";
        public const string WebApiUrlTradeTicketStatusList = "api/tradeticket/status/list";
        public const string WebApiUrlTradeTicketStatusDetail = "api/tradeticket/status/detail";
        public const string WebApiUrlDeleteTradeTicket = "api/tradeticket/request/delete";
        public const string WebApiUrlExcelExportAllTradeTickets = "api/tradeticket/allTradeTicketsExcelExport";
        public const string WebApiUrlExcelExportTradeTicketStatus = "api/tradeticket/tradeTicketStatusExcelExport";

        //Users
        public const string WebApiUrlGetTradeTicketOilTypes = "api/tradeticket/oiltypes";

        #endregion

        #region Competitors
        public const string WebApiUrlSaveCompetitor = "api/lookups/competitor/save";
        public const string WebApiUrlUpdateCompetitor = "api/lookups/competitor/update";
        public const string WebApiUrlGetCompetitorById = "api/lookups/get/competitorid";
        public const string WebApiUrlGetCompetitorList = "api/lookups/competitor/list";
        public const string WebApiUrlGetSkuBasedOilTypeList = "api/lookups/competitor/skulist";
        public const string WebApiUrlExportCompetitor = "api/lookups/competitor/export";
        public const string WebApiUrlGetCompetitorListWithPagination = "api/lookups/competitor/listwithpagination";

        #endregion

        #region Lifting Request

        public const string WebApiUrlGetLiftingRequestList = "api/lifting/liftingRequestWeb/list";
        public const string WebApiUrlGetLiftingRequestDetails = "api/lifting/liftingRequestWeb/detail";
        public const string WebApiUrlPostLiftingRequestStatusChange = "api/lifting/liftingRequest/statuschange";
        public const string WebApiUrlPostLiftingRequestStatusChanges = "api/lifting/liftingRequest/statuschanges";
        public const string WebApiUrlGetLiftingRequestListForExport = "api/lifting/liftingRequestWeb/export";
        public const string WebApiUrlGetLiftingRequestWithoutEnquiryNumberList = "api/lifting/liftingRequestWeb/WithoutEnquiryNumber/list";
        public const string WebApiUrlPostLiftingRequestAdminApprove = "api/lifting/liftingRequest/admin/approve";


        public const string WebApiUrlGetSaudaOrderLiftingRequestDetails = "api/lifting/saudaorder/liftingrequest";
        public const string WebApiUrlGetSaudaOrderLiftingRequestExcelExport = "api/lifting/saudaorder/liftingrequestexcelexport";


        #endregion

        #region Sauda TradeTicket Mapping

        public const string WebApiUrlTradeTicketDropDownList = "api/tradeticket/dropdown";
        public const string WebApiUrlSaudaOrderList = "api/sauda/orderlist";
        public const string WebApiUrlMapTradeTicketToSaudaOrders = "api/sauda/maptradeTickettosaudaorders";

        #endregion

        #region User Target
        public const string WebApiUrlSaveUserTarget = "api/employees/usertarget/add";
        public const string WebApiUrlUpdateUserTarget = "api/employees/usertarget/update";
        public const string WebApiUrlGetUserTargetById = "api/employees/usertarget/view";
        public const string WebApiUrlGetUserTargetList = "api/employees/usertarget/list";
        public const string WebApiUrlGetUserAssignedToList = "api/employees/usertarget/UserAssignedTo";
        //Sauda List For Admin
        public const string WebApiUrlGetAllSaudhaListForAdmin = "api/sauda/admin/list";

        #endregion

        #region Territory
        public const string WebApiUrlPostAddTerritory = "api/master/territory/save";
        public const string WebApiUrlPostUpdateTerritory = "api/master/territory/update";
        public const string WebApiUrlGerTerritoryById = "api/master/territory/id";
        public const string WebApiUrlGerTerritoryList = "api/master/territory/list";
        public const string WebApiUrlGerTerritoryStateBase = "api/master/territory/stateid";
        public const string WebApiUrlGetDistrictBasedTerritory = "api/master/districts/territoryid";
        public const string WebApiUrlGerTerritoryMappedDistrict = "api/master/territorydistrict/list";
        public const string WebApiUrlExportTerritory = "api/master/territory/export";

        #endregion

        #region New Discount
        public const string WebApiUrlAddDiscountGeography = "api/pricing/geographydiscount/save";
        public const string WebApiUrlGetCityDetailsBasedOnTerritory = "api/pricing/getcitydetails/territoryids";
        public const string WebApiUrlGetStatesBasedOnZone = "api/master/getstates/zoneids";
        public const string WebApiUrlGerTerritoryBasedOnState = "api/master/gerterritory/stateids";
        public const string WebApiUrlGetDistrictBasedOnTerritory = "api/master/getdistrict/territoryIds";
        public const string WebApiUrlGetGeographyList = "api/pricing/getgeography/list";
        public const string WebApiUrlGetGeographyCityList = "api/pricing/getgeographycity/list";
        public const string WebApiUrlGetGeographyDetailsById = "api/pricing/getgeography/id";
        public const string WebApiUrlUpdateDiscountGeography = "api/pricing/geographydiscount/update";
        public const string WebApiUrlGetZonalHeadBasedonZoneState = "api/master/getzonalhead/zonestateids";
        public const string WebApiUrlGetOilTypeBasedonVerticals = "api/master/getoiltype/zhverticals";
        public const string WebApiUrlGetGeographySchemeBasedOnState = "api/master/getgeographyscheme/stateids";



        #endregion

        #region New Discount
        public const string WebApiUrlPostAddDiscountUsers = "api/pricing/discountuser/add";
        public const string WebApiUrlPostUpdateDiscountUsers = "api/pricing/discountuser/update";
        public const string WebApiUrlGetDiscountUsersById = "api/pricing/discountuser/id";
        public const string WebApiUrlGetDiscountUsersList = "api/pricing/discountuser/list";
        public const string WebApiUrlDiscountUsersExport = "api/pricing/discountuser/list/export";
        public const string WebApiUrlGetDiscountUsersDetailList = "api/pricing/discountuserdetails/list";
        public const string WebApiUrlGetAssignedDiscountList = "api/pricing/employeeuserdiscount/list";
        public const string WebApiUrlGetAssignedDiscountById = "api/pricing/employeeuserdiscount/id";
        public const string WebApiUrlPostEmployeeAndUserDiscount = "api/pricing/employeeuserdiscount/add";

        #endregion

        #region New SpecialtyFat Discount
        public const string WebApiUrlAddSpecialtyFatDiscountGeography = "api/pricing/geographydiscount/SpecialtyFat/save";
        public const string WebApiUrlGetSpecialtyFatGeographyList = "api/pricing/getgeography/SpecialtyFat/list";
        public const string WebApiUrlGetSpecialtyFatGeographyCityList = "api/pricing/getgeographycity/SpecialtyFat/list";
        public const string WebApiUrlGetSpecialtyFatGeographyDetailsById = "api/pricing/getgeography/SpecialtyFat/id";
        public const string WebApiUrlUpdateSpecialtyFatDiscountGeography = "api/pricing/geographydiscount/SpecialtyFat/update";
        public const string WebApiUrlGetSpecialtyFatCityBasedOnCityTerritory = "api/pricing/specialtyfat/getgeographycitylist/cityidterritory";

        #endregion

        #region Competitor Analysis

        public const string WebApiUrlPostSaveCompetitorAnalysis = "api/mobilesauda/competitoranalysis/save";
        public const string WebApiUrlPostUpdateCompetitorAnalysis = "api/sauda/competitoranalysis/update";
        public const string WebApiUrlGetCompetitorAnalysisList = "api/sauda/competitoranalysis/List";
        public const string WebApiUrlGetCompetitorAnalysisById = "api/sauda/competitoranalysis/details";
        public const string WebApiUrlGetCompetitorAnalysisDetailsListById = "api/sauda/competitoranalysisdetails/list";
        public const string WebApiUrlPostSaveCompetitorAnalysisApproval = "api/sauda/competitoranalysis/approval";

        #endregion

        #region PriceNotifyConfiguration
        public const string WebApiUrlAddPriceNotifyConfiguration = "api/pricing/pricenotifyconfiguration/save";
        public const string WebApiUrlGetPriceNotifyConfigurationList = "api/pricing/pricenotifyconfiguration/list";
        public const string WebApiUrlGetPriceNotifyConfigurationCityList = "api/pricing/getpricenotifyconfigurationcity/list";
        public const string WebApiUrlGetPriceNotifyConfigurationById = "api/pricing/getpricenotifyconfiguration/id";
        public const string WebApiUrlUpdatePriceNotifyConfiguration = "api/pricing/pricenotifyconfiguration/update";

        #endregion

        #region Sauda Convertion
        public const string WebApiUrlGetSaudaConversionList = "api/sauda/saudaconversion/list";
        public const string WebApiUrlGetSaudaConversionDetails = "api/sauda/saudaconversiondetails/id";
        public const string WebApiUrlGetSaudaConversionAllDetail = "api/sauda/saudaconversiondetailall/id";
        public const string WebApiUrlApproveSaudaConversion = "api/sauda/approvesaudaconversion";
        public const string WebApiUrlGetSaudaConversionListForExport = "api/sauda/saudaconversion/export";
        public const string WebApiUrlGetSaudaConversionNewDetails = "api/sauda/saudaextensiondetails/new/id";
        public const string WebApiUrlGetSaudaConversionUnitAndDiffRateList = "api/mobileDealersauda/saudaconversion/get/unitandbasicrate";

        #endregion

        #region View TP and RA
        public const string WebApiUrlGetTPandRAPricingList = "api/sauda/tpandrapricing/list";

        #endregion

        #region Key Performance Indicator
        public const string WebApiUrlPostAddKeyPerformance = "api/lookups/keyperformance/add";
        public const string WebApiUrlPostUpdateKeyPerformance = "api/lookups/keyperformance/update";
        public const string WebApiUrlGetKeyPerformanceById = "api/lookups/keyperformance/id";
        public const string WebApiUrlGetKeyPerformanceList = "api/lookups/keyperformance/list";

        #endregion

        #region Premium
        public const string WebApiUrlPostAddPremium = "api/pricing/premium/add";
        public const string WebApiUrlPostUpdatePremium = "api/pricing/premium/update";
        public const string WebApiUrlGetPremiumUserById = "api/pricing/premium/id";
        public const string WebApiUrlGetPremiumUserList = "api/pricing/premium/list";
        public const string WebApiUrlGetPremiumUserDetailList = "api/pricing/premiumuserdetails/list";

        public const string WebApiUrlGetAssignedPremiumList = "api/pricing/assignedpremium/list";
        public const string WebApiUrlGetAssignedPremiumById = "api/pricing/getassignedpremium/id";
        public const string WebApiUrlPostAddEmployeeAndUserPremium = "api/pricing/employeeanduserpremium/add";

        #endregion

        #region Today Activities
        public const string WebApiUrlPostTodayActivitiesList = "api/SalesTourPlan/TodayActivities/dealer/list";
        public const string WebApiUrlPostGetPendingSaudaList = "api/SalesTourPlan/ PendingSaudaRemarks/list";

        #endregion

        #region Premium Geography
        public const string WebApiUrlPostAddPremiumGeography = "api/pricing/premiumgeography/add";
        public const string WebApiUrlPostUpdatePremiumGeography = "api/pricing/premiumgeography/update";
        public const string WebApiUrlGetPremiumGeographyDetailsById = "api/pricing/premiumgeography/id";
        public const string WebApiUrlGetPremiumGeographyList = "api/pricing/premiumgeography/list";
        public const string WebApiUrlGetPremiumGeographyCityList = "api/pricing/premiumgeography/citylist";

        #endregion

        #region Today Activity

        public const string WebApiUrlGetProspectiveDealers = "api/SalesTourPlan/todayactivity/prospectivedealers";
        public const string WebApiUrlGetPendingSaudaRemarksList = "api/SalesTourPlan/todayactivity/pendingsauda";
        public const string WebApiUrlGetMarketScenariosList = "api/SalesTourPlan/todayactivity/salesdiscussion";
        public const string WebApiUrlGetCompetitorsList = "api/SalesTourPlan/todayactivity/competitors";
        public const string WebApiUrlGetCompetitorSkuList = "api/SalesTourPlan/todayactivity/competitorssku";
        public const string WebApiUrlGetTodayActivityList = "api/SalesTourPlan/TodayActivities/list";
        public const string WebApiUrlGetWholesellerCompetitorsList = "api/SalesTourPlan/todayactivity/WholeSellerCompetitors";
        public const string WebApiUrlGetProspectiveDealerList = "api/SalesTourPlan/TodayActivities/GetProspectiveDealerList";
        public const string WebApiUrlGetListWholesellerForWeb = "api/SalesTourPlan/todayactivity/WholesellerForWeb";
        public const string WebApiUrlGetListWholesellerSalesDetails = "api/SalesTourPlan/todayactivity/wholeseller/SalesDetails";
        public const string WebApiUrlGetFileAttachmentsList = "api/SalesTourPlan/FileAttachments";

        #endregion

        #region Question

        public const string WebApiUrlPostSaveQuestion = "api/surveyupdates/question/save";
        public const string WebApiUrlPostUpdateQuestion = "api/surveyupdates/question/update";
        public const string WebApiUrlGetQuestionList = "api/surveyupdates/question/List";
        public const string WebApiUrlGetQuestionDetailsById = "api/surveyupdates/get/questionid";
        public const string WebApiUrlGetQuestionSurveyDetailsById = "api/surveyupdates/question/survey";
        #endregion

        #region Sauda Extension

        public const string WebApiUrlGetSaudaExtensionList = "api/sauda/saudaextension/list";
        public const string WebApiUrlExportSaudaExtensionList = "api/sauda/saudaextension/export";
        public const string WebApiUrlGetSaudaExtensionDetails = "api/sauda/saudaextensiondetails/id";
        public const string WebApiUrlGetSaudaExtensionAllDetail = "api/sauda/saudaextensiondetailall/id";
        public const string WebApiUrlApproveSaudaExtension = "api/sauda/approvesaudaextension";

        #endregion

        #region Special Rate Approval

        public const string WebApiUrlPostSaveSpecialRateApproval = "api/sauda/specialrate/approval";
        public const string WebApiUrlGetSpecialRateListWithAccessPermission = "api/sauda/specialrate/List";

        #endregion

        #region Bulletin

        public const string WebApiUrlGetAllBulletins = "api/surveyupdates/bulletin/list";
        public const string WebApiUrlGetBulletinDetailById = "api/surveyupdates/bulletin/get/id";
        public const string WebApiUrlPostAddBulletin = "api/surveyupdates/bulletin/save";
        public const string WebApiUrlPostUpdateBulletin = "api/surveyupdates/bulletin/update";
        public const string WebApiUrlDeleteBulletinsMedia = "api/surveyupdates/bulletin/media/delete";

        #endregion

        #region Feedback
        public const string WebApiUrlGetFeedbackTypeddl = "api/surveyupdates/feedbackType/list";
        public const string WebApiUrlGetAllFeedback = "api/surveyupdates/feedback/list";
        #endregion

        #region Sauda Counter Bid

        public const string WebApiUrlGetSaudaCounterBidDetails = "api/authorize/counterbid/view";
        public const string WebApiUrlPostApproveCounterBid = "api/authorize/counterbid/approve";

        #endregion

        #region Service Notification
        public const string WebApiUrlGetCounterBidNotification = "api/sauda/counterbid/notification";
        public const string WebApiUrlUpdateHoldOrderToReject = "api/sauda/hold_orders/reject";
        public const string WebApiUrlSendRABookingStatus = "api/sauda/status/notification";
        #endregion

        #region Report
        public const string WebApiUrlOilPriceReport = "api/report/oilprice";
        public const string WebApiUrlCostChangeReport = "api/report/costchangereport";
        public const string WebApiUrlSaudaOrdersReport = "api/report/saudaorders";
        public const string WebApiUrlDistributorStockReport = "api/report/distributorstock";
        public const string WebApiUrlMaterialCostOilTypes = "api/lookups/materialcost/oiltypes";
        public const string WebApiUrlSaudaLimitExport = "api/report/saudalimit";
        public const string WebApiUrlCreditLimitExport = "api/report/creditlimit";
        public const string WebApiUrlSalesExport = "api/report/sales/bdowise";
        public const string WebApiUrlSaudaExport = "api/report/sauda/bdowise";
        public const string WebApiUrlIndentExport = "api/report/indentlist";
        public const string WebApiUrlPendingSaudaExport = "api/report/PendingSaudaReport";
        public const string WebApiUrlPendingContractExport = "api/report/pendingcontractreport";
        public const string WebApiUrlGetVerticalId = "api/report/getverticalid";

        public const string WebApiUrlMTPReport = "api/report/MTP";
        public const string WebApiUrlPCPReport = "api/report/PCP";
        public const string WebApiUrlSaudaConversionReport = "api/mobileDealersauda/saudaConversion";
        public const string WebApiUrlGetSaudaCallRecordMappingAttachments = "api/report/saudacallrecordmapping/list";
        public const string WebApiUrlDailyBookingReport = "api/report/dailybooking";
        public const string WebApiUrlGetTruckPlacementTrackerReport = "api/report/truckplacementtracker";

        public const string WebApiUrlSchemeGeographyReport = "api/report/schemegeographyreport";
        public const string WebApiUrlDemandPlanBillingReport = "api/report/demandplanbillingreport";

        #endregion

        #region SubCategory

        public const string WebApiUrlGetSubCategoryDetailsById = "api/master/get/subcategoryid";
        public const string WebApiUrlPostSaveSubCategory = "api/master/subcategory/save";
        public const string WebApiUrlPostUpdateSubCategory = "api/master/subcategory/update";
        public const string WebApiUrlGetSubCategoryList = "api/master/subcategory/list";
        public const string WebApiUrlGetSubCategoryListddl = "api/lookups/subcategory/listddl";
        public const string WebApiUrlExportSubCategory = "api/master/subcategory/export";

        #endregion

        #region Customergroup1 
        public const string WebApiUrlPostAddorUpdateCustomerGroupOne = "api/master/customergroupone/addorupdate";
        public const string WebApiUrlGetCustomerGroupOneList = "api/master/customergroupone/list";
        public const string WebApiUrlGetCustomerGroupOneDetailsById = "api/master/get/customergrouponeid";
        public const string WebApiUrlPostAddorUpdateCustomerGroupTwo = "api/master/customergrouptwo/addorupdate";
        public const string WebApiUrlGetCustomerGroupTwoList = "api/master/customergrouptwo/list";
        public const string WebApiUrlGetCustomerGroupTwoDetailsById = "api/master/get/customergrouptwoid";
        #endregion

        #region Customergroup5 
        public const string WebApiUrlPostAddorUpdateCustomerGroupFive = "api/master/customergroupfive/addorupdate";
        public const string WebApiUrlGetCustomerGroupFiveList = "api/master/customergroupfive/list";
        public const string WebApiUrlGetCustomerGroupFiveDetailsById = "api/master/get/customergroupfiveid";

        #endregion

        #region SpecialityFat Discount User

        public const string WebApiUrlPostAddSpecialityFatDiscountUsers = "api/pricing/specialityfat/discountuser/add";
        public const string WebApiUrlPostUpdateSpecialityFatDiscountUsers = "api/pricing/specialityfat/discountuser/update";
        public const string WebApiUrlGetSpecialityFatDiscountUsersById = "api/pricing/specialityfat/discountuser/id";
        public const string WebApiUrlGetSpecialityFatDiscountUsersList = "api/pricing/specialityfat/discountuser/list";
        public const string WebApiUrlGetSpecialityFatDiscountUsersExportList = "api/pricing/specialityfat/discountuser/export";
        public const string WebApiUrlGetSpecialityFatDiscountUsersDetailList = "api/pricing/specialityfat/discountuserdetails/list";
        public const string WebApiUrlGetSpecialityFatAssignedDiscountList = "api/pricing/specialityfat/assigneddiscount/list";
        public const string WebApiUrlGetSpecialityFatAssignedDiscountExport = "api/pricing/specialityfat/assigneddiscount/export";
        public const string WebApiUrlGetSpecialityFatAssignedDiscountUserDetailsList = "api/pricing/specialityfat/assigneddiscountuserdetails/list";

        public const string WebApiUrlGetSpecialityFatDiscountUsersDetailId = "api/pricing/specialityfat/assigneddiscount/id";
        public const string WebApiUrlGetSpecialityFatAssignedDiscountToUser = "api/pricing/specialityfat/assigneddiscount/employee";
        public const string WebApiUrlPostUpdateRequestQuantityLimit = "api/pricing/specialtyfat/quantityrequest/add";

        #endregion

        #region SpecalityFatQuantityRequest

        public const string WebApiUrlPostUpdateSpecalityFatQuantityRequest = "api/pricing/specialtyfat/quantitylimit/update";
        public const string WebApiUrlPostGetSpecalityFatQuantityRequest = "api/pricing/specialtyfat/quantityrequest/organizationreportingtoId/list";
        public const string WebApiUrlGetSpecialtyFatQuantityRequestStatus = "api/pricing/specialtyfat/quantityrequest/list";
        //public const string WebApiUrlGetSpecialtyFatQuantityRequestStatus = "api/pricing/mobile/quantityrequest/list";

        #endregion

        #region  Auto Allocation

        public const string WebApiUrlGetAutoAllocationUserListByRoleIds = "api/pricing/autoallocationlist/roleids";
        public const string WebApiUrlGetAutoAllocationDetailsByUserId = "api/pricing/autoallocationdetails/userid";
        public const string WebApiUrlPostSaveAutoAllocation = "api/pricing/specalityfatdiscountusers/save";

        #endregion

        #region Rake
        //Premium
        public const string WebApiUrlPostAddRake = "api/master/rake/add";
        public const string WebApiUrlPostUpdateRake = "api/master/rake/update";
        public const string WebApiUrlGetRakeById = "api/master/rake/getbyid";
        public const string WebApiUrlGetRakeList = "api/master/rake/list";
        public const string WebApiUrlExportRake = "api/master/rake/export";
        public const string WebApiUrlGetRakeListWithPagination = "api/master/rake/listwithpagination";

        public const string WebApiUrlGetDepotRakeList = "api/master/depotrake/list";
        public const string WebApiUrlGetDepotRakeByPlantId = "api/master/depotrake/plantid";
        public const string WebApiUrlGetDepotRakePlantddList = "api/master/plantdepotrake/list";
        public const string WebApiUrlGetDepotListAsync = "api/master/depotlist";

        public const string WebApiUrlGetTransportModeBasedonDepotRake = "api/master/gettransportmode/depotrake";
        public const string WebApiUrlGetDepotPlantddList = "api/master/plantdepot/list";
        #endregion

        #region Support - Issue Register

        public const string WebApiUrlPostAddIssue = "api/support/issue/add";
        public const string WebApiUrlGetIssueList = "api/support/issue/list";
        public const string WebApiUrlGetIssueDetailsBySupportId = "api/support/issuedetails/supportid";
        public const string WebApiUrlUpdateIssueStatus = "api/support/status/update";
        public const string WebApiUrlGetIssueCommentsList = "api/support/issuedetails/comments";
        public const string WebApiUrlGetIssueListWithCmts = "api/support/issue/listwithcomments";

        public const string WebApiUrlExportSupportIssues = "api/support/issue/export";
        public const string WebApiUrlGetFeatureList = "api/support/issue/featurelist";
        public const string WebApiUrlGetQueryFromList = "api/support/issue/queryfromlist";


        #endregion

        #region SAP Data Sync

        public const string WebApiUrlSAPDataSync = "api/sauda/syncdata";

        #endregion

        #region RA 2.0

        #region Percentile Number
        public const string WebApiUrlPostAddPercentileNumber = "api/ralookups/PercentileNumber/add";
        public const string WebApiUrlPostUpdatePercentileNumber = "api/ralookups/PercentileNumber/update";
        public const string WebApiUrlGetPercentileNumberById = "api/ralookups/PercentileNumber/id";
        public const string WebApiUrlGetPercentileNumberList = "api/ralookups/PercentileNumber/list";
        public const string WebApiUrlGetPercentileNumberDetailsById = "api/ralookups/PercentileNumber/detailslist";
        public const string WebApiUrlPostExportPercentileNumber = "api/ralookups/PercentileNumber/export";
        #endregion

        #region RA Notification 
        public const string WebApiUrlPostCustomerGroupDDL = "api/ralookups/CustomerGroup/ddllist";
        public const string WebApiUrlPostAddRANotification = "api/ralookups/RANotification/add";
        public const string WebApiUrlPostUpdateRANotification = "api/ralookups/RANotification/update";
        public const string WebApiUrlGetRANotificationById = "api/ralookups/RANotification/id";
        public const string WebApiUrlGetRANotificationList = "api/ralookups/RANotification/list";
        public const string WebApiUrlGetRANotificationDetails = "api/ralookups/RANotification/details/id";
        public const string WebApiUrlGetRANotificationExport = "api/ralookups/RANotification/export";
        public const string WebApiUrlGetDealerListBasedOnCustomerGroup = "api/raVersionTwo/dealer/List/ByCustomerGroup";
        public const string WebApiUrlGetMappedCustomerListByRaNotificationId = "api/ralookups/dealer/List/ByRaNotificationId";

        #endregion

        #region Scheme Discount
        public const string WebApiUrlPostGetCustomersByGroup = "api/ralookups/SchemeDiscount/customers";
        public const string WebApiUrlPostAddSchemeDiscount = "api/ralookups/SchemeDiscount/add";
        public const string WebApiUrlPostUpdateSchemeDiscount = "api/ralookups/SchemeDiscount/update";
        public const string WebApiUrlGetSchemeDiscountList = "api/ralookups/SchemeDiscount/list";
        public const string WebApiUrlGetSchemeDiscountListDetail = "api/ralookups/SchemeDiscount/list/detail";
        public const string WebApiUrlGetSchemeDiscountById = "api/ralookups/SchemeDiscount/Id";
        #endregion

        #region CustomerGroup
        public const string WebApiUrlPostSaveCustomerGroup = "api/raVersionTwo/customerGroup/add";
        public const string WebApiUrlPostUpdateCustomerGroup = "api/raVersionTwo/customerGroup/update";
        public const string WebApiUrlPostRemoveCustomersFromCustomerGroup = "api/raVersionTwo/customers/removeFromGroup";
        public const string WebApiUrlGetCustomerGroupList = "api/raVersionTwo/customerGroup/listwithpagination";
        public const string WebApiUrlGetCustomerGroupByGroupId = "api/raVersionTwo/customerGroup/get/id";
        public const string WebApiUrlGetCustomerGroupDetailListByGroupId = "api/raVersionTwo/customerGroupDetailList/customerGroupId";
        public const string WebApiUrlExportCustomerGroup = "api/raVersionTwo/customerGroup/export";
        public const string WebApiUrlGetCustomerList = "api/raVersionTwo/dealer/List";
        public const string WebApiUrlGetMappedCustomerListByCustomerGroupId = "api/raVersionTwo/customerlist/groupid";
        public const string WebApiUrlGetCustomerListByCustomerGroupIdAndBDOForDropdown = "api/raVersionTwo/customerddl/groupidAndBdoId";
        public const string WebApiUrlGetCustomerListByCustomerGroupIdAndBDOAndPercentileForDropdown = "api/raVersionTwo/customerddl/groupidAndBdoIdAndPercentileNumber";
        public const string WebApiUrlGetCustomerListBasedOnCityIdsAndPercentileNumberForDropdown = "api/raVersionTwo/customerddl/CityIdsAndPercentileNumber";
        public const string WebApiUrlGetCustomerGroupListByVerticalForDropdown = "api/raVersionTwo/customerGroup/ddl/verticalId";
        public const string WebApiUrlGetCustomerGroupListByVerticalIdsForDropdown = "api/raVersionTwo/customerGroup/ddl/verticalIds";
        public const string WebApiUrlGetBiddingWindowCustomerGroupListForddl = "api/raVersionTwo/biddingwindow/customergroupsddl";


        public const string WebApiUrlGetReportingToRAZonalHeadUsersByCustomerGroup = "api/raVersionTwo/reportingToRAZonalHeadUsers/customerGroupId";
        public const string WebApiUrlGetRAZonalHeadUsersByCustomerGroupIdsAndVerticalIds = "api/raVersionTwo/RAZonalHeadUsers/customerGroupIdsAndVerticalIds";
        public const string WebApiUrlGetRABDOUsersByZonalHeadIdsAndVerticalIds = "api/raVersionTwo/RABDOUsers/ZonalHeadIdsAndVerticalIds";
        public const string WebApiUrlGetCustomerListByCustomerGroupIdsAndBDOsForDropdown = "api/raVersionTwo/customer/ddl/customerGroupIdsAndBdoIds";
        public const string WebApiUrlGetCustomerListByCustomerGroupIdsCityIdsForDropdown = "api/raVersionTwo/customer/ddl/customerGroupIdsAndCityIds";

        #endregion

        #region Bidding Window

        public const string WebApiUrlPostSaveBiddingWindow = "api/reverseauction/biddingwindow/save";
        public const string WebApiUrlPostUpdateBiddingWindow = "api/reverseauction/biddingwindow/update";
        public const string WebApiUrlPostGetBiddingWindowDetailById = "api/reverseauction/biddingwindow/byid";
        public const string WebApiUrlPostGetBiddingWindowDetails = "api/reverseauction/biddingwindow/details";
        public const string WebApiUrlPostGetBiddingWindowVolumeDetails = "api/reverseauction/biddingwindow/volumedetails";
        public const string WebApiUrlPostStopBidWindow = "api/reverseauction/biddingwindow/stopbidwindow";
        public const string WebApiUrlExportBidWindow = "api/reverseauction/biddingwindow/export";
        public const string WebApiUrlGetBiddingWindowListForddl = "api/reverseauction/biddingwindow/ddlist";

        #endregion

        #region Scheme Discount - GeographyBased

        public const string WebApiUrlPostSaveGeographyBasedSchemeDiscount = "api/raVersionTwo/GeographyBasedSchemeDiscount/add";
        public const string WebApiUrlPostUpdateGeographyBasedSchemeDiscount = "api/raVersionTwo/GeographyBasedSchemeDiscount/update";
        public const string WebApiUrlGetGeographyBasedSchemeDiscountList = "api/raVersionTwo/GeographyBasedSchemeDiscount/listwithpagination";
        public const string WebApiUrlGetGeographyBasedSchemeDiscountByDiscountId = "api/raVersionTwo/GeographyBasedSchemeDiscount/get/id";
        public const string WebApiUrlGetSchemeDiscountGeographyHierarchyListById = "api/raVersionTwo/GeographyBasedSchemeDiscount/geographyMappingList/discountId";
        public const string WebApiUrlExportGeographyBasedSchemeDiscount = "api/raVersionTwo/GeographyBasedSchemeDiscount/export";
        public const string WebApiUrlPostUpdateSchemeDiscountGeographyListByIsActive = "api/raVersionTwo/UpdateSchemeDiscountGeographyList/ByIsActive";
        #endregion

        #region Conversion Formula

        public const string WebApiUrlPostSaveConversionFormula = "api/reverseauction/conversionformula/save";
        public const string WebApiUrlPostUpdateConversionFormula = "api/reverseauction/conversionformula/update";
        public const string WebApiUrlPostGetConversionFormulaById = "api/reverseauction/conversionformula/id";
        public const string WebApiUrlPostGetConversionDetails = "api/reverseauction/conversionformula/getconversion";
        public const string WebApiUrlPostGetConversionFormulaDetails = "api/reverseauction/conversionformula/getconversiondetails";
        public const string WebApiUrlPostGetBaseSku = "api/reverseauction/basesku";
        public const string WebApiUrlPostGetDerivedSku = "api/reverseauction/derivedsku";
        public const string WebApiUrlPostExport = "api/reverseauction/export";

        #endregion

        #region Base Group Margin

        public const string WebApiUrlPostSaveBaseGroupMargin = "api/ralookups/basegropmargin/add";
        public const string WebApiUrlPostUpdateBaseGroupMargin = "api/ralookups/basegropmargin/update";
        public const string WebApiUrlPostGetBaseGroupMarginById = "api/ralookups/basegropmargin/getbyid";
        public const string WebApiUrlPostBaseGroupMarginList = "api/ralookups/basegropmargin/baselist";
        public const string WebApiUrlPostDerivedGroupMarginList = "api/ralookups/basegropmargin/derivedlist";
        public const string WebApiUrlPostBaseGroupMarginStatesList = "api/ralookups/basegropmargin/stateslist";


        public const string WebApiUrlPostBaseCustomerGroupList = "api/ralookups/basecustomergroup";
        public const string WebApiUrlPostDerivedCustomerGroupList = "api/ralookups/derivedcustomergroup";

        public const string WebApiUrlPostExportBaseGroupMarginList = "api/ralookups/export/basegropmarginlist";
        #endregion

        #region Benefits

        public const string WebApiUrlPostSaveBenefits = "api/raVersionTwo/benefits/add";
        public const string WebApiUrlPostUpdateBenefits = "api/raVersionTwo/benefits/update";
        public const string WebApiUrlGetBenefitsList = "api/raVersionTwo/benefits/listwithpagination";
        public const string WebApiUrlGetBenefitDetailsById = "api/raVersionTwo/benefits/get/id";
        public const string WebApiUrlExportBenefits = "api/raVersionTwo/benefits/export";
        public const string WebApiUrlGetBenefitListByBenefitTypeForDropdown = "api/raVersionTwo/benefitsddl/benefitTypeId";
        public const string WebApiUrlGetBenefitTypeListForDropdown = "api/raVersionTwo/benefitTypes/ddl";

        #endregion

        #region GST

        public const string WebApiUrlGetGSTList = "api/raVersionTwo/gst/listwithpagination";
        public const string WebApiUrlGetGSTById = "api/raVersionTwo/gst/get/id";
        public const string WebApiUrlPostUpdateGst = "api/raVersionTwo/gst/update";
        public const string WebApiUrlPostSaveGst = "api/raVersionTwo/gst/add";
        public const string WebApiUrlExportGST = "api/raVersionTwo/gst/export";
        public const string WebApiUrlGetGSTListById = "api/raVersionTwo/gst/listbyidwithpagination";

        #endregion

        #region Final Price Generate

        public const string WebApiUrlReverseAuctionFinalPriceGenerateSave = "api/finalprice/ra2reverseauction/save";
        public const string WebApiUrlRa2GetGeneratedPriceAsync = "api/finalprice/ra2getpricegenerate";
        public const string WebApiUrlRa2GetGetPriceGenerateDetails = "api/finalprice/ra2getpricegenerate/details";

        #endregion

        #region GPJump
        public const string WebApiUrlGetGPJumpList = "api/ralookups/gpjump/list";
        public const string WebApiUrlGetGPJumpById = "api/ralookups/gpjump/id";
        public const string WebApiUrlPostUpdateGPJump = "api/ralookups/gpjump/update";
        public const string WebApiUrlPostSaveGPJump = "api/ralookups/gpjump/add";
        public const string WebApiUrlExportGPJump = "api/ralookups/gpjump/export";
        #endregion

        #region CounterBidJump
        public const string WebApiUrlGetCounterBidJumpList = "api/ralookups/CounterBid/list";
        public const string WebApiUrlGetCounterBidJumpById = "api/ralookups/CounterBid/id";
        public const string WebApiUrlPostUpdateCounterBidJump = "api/ralookups/CounterBid/update";
        public const string WebApiUrlPostSaveCounterBidJump = "api/ralookups/CounterBid/add";
        public const string WebApiUrlExportCounterBidJump = "api/ralookups/CounterBid/export";
        #endregion

        #region CustomerGroupMappings
        public const string WebApiUrlGetCustomerGroupListNotMappedInCustomerGroupMappings = "api/raVersionTwo/customegroupnotmappedincustomergroupmappings/list";
        public const string WebApiUrlGetDerivedCustomerGroupListNotMappedInCustomerGroupMappings = "api/raVersionTwo/derivedcustomegroupnotmappedincustomergroupmappings/list";
        public const string WebApiUrlPostAddorUpdateCustomerGroupMapping = "api/raVersionTwo/customergroupmappings/addorUpdate";
        public const string WebApiUrlGetCustomerGroupMappingList = "api/raVersionTwo/customergroupmapping/listwithpagination";
        public const string WebApiUrlCustomerGroupMappingListDetailsById = "api/raVersionTwo/customergroupmapping/details/customergroupmappingid";
        public const string WebApiUrlGetCustomerGroupMappingByCustomerGroupMappingId = "api/raVersionTwo/CustomerGroupMapping/get/id";


        #endregion

        #region RA2 Notification
        public const string WebApiUrlPostAddRA2Notification = "api/raVersionTwo/RA2Notification/add";
        public const string WebApiUrlGetRA2NotificationList = "api/raVersionTwo/RA2Notification/listwithpagination";
        public const string WebApiUrlGetRA2NotificationHierarchyListById = "api/raVersionTwo/GetRA2Notification/list/byId";
        public const string WebApiUrlPostUpdateRA2NotificationListByIsActive = "api/raVersionTwo/UpdateRA2NotificationList/ByIsActive";

        #endregion

        #region Maximum Percentage For Sauda Qty Increase

        public const string WebApiUrlPostGetSaudaQuantityList = "api/ralookups/saudaquantityincrease/configlist";
        public const string WebApiUrlPostSaveSaudaQuantityList = "api/ralookups/saudaquantityincrease/save";

        #endregion

        #region RA Sauda Confoguration

        public const string WebApiUrlPostRaSaudaConfigurationList = "api/ralookups/rasaudaconfiguration/list";
        public const string WebApiUrlPostSaveRaSaudaConfiguration = "api/ralookups/rasaudaconfiguration/save";
        public const string WebApiUrlPostUpdateRaSaudaConfiguration = "api/ralookups/rasaudaconfiguration/update";
        public const string WebApiUrlPostGetRaSaudaConfigurationById = "api/ralookups/rasaudaconfiguration/id";

        #endregion

        #region EditDiscountsAndBenefits
        public const string WebApiUrlPostUpdateDiscountAndBenefitListByIsActive = "api/raVersionTwo/UpdateDiscountAndBenefitList/ByIsActive";
        #endregion

        #endregion

        #region MaterialType

        public const string WebApiUrlAddOrUpdateMaterialType = "api/master/materialtype/addorupdate";
        public const string WebApiUrlGetMaterialTypeList = "api/master/materialtype/list";
        public const string WebApiUrlMaterialTypeById = "api/master/materialtype/getById";
        public const string WebApiUrlExportMaterialType = "api/master/materialtype/export";

        #endregion

        #region Sap Sync Manual Trigger

        public const string WebApiUrlSaudaSyncData = "api/sapmanual/sauda/list";

        #endregion

        #region VolumeLoadability

        public const string WebApiUrlAddOrUpdateVolumeLoadability = "api/master/volumeloadability/addorupdate";
        public const string WebApiUrlGetVolumeLoadabilityList = "api/master/volumeloadability/list";
        public const string WebApiUrlVolumeLoadabilityById = "api/master/volumeloadability/getById";
        public const string WebApiUrlExportVolumeLoadability = "api/master/volumeloadability/export";

        #endregion

        #region
        public const string WebApiUrlGetDashboardDetails = "api/employees/dashboard/details";
        public const string WebApiUrlGetUserLoginCount = "api/employees/userlogincount/details";
        public const string WebApiUrlGetUserLoginInfo = "api/employees/DistributorUserLogin/Info";
        public const string WebApiUrlGetSalesLoginInfo = "api/employees/SalesLogin/Info";

        #endregion

        #region LineApiUrl
        //line
        public const string WebApiUrlPostLine = "api/master/line/add";
        public const string WebApiUrlPutLine = "api/master/line/update";
        public const string WebApiUrlGetLineList = "api/master/line/list";
        public const string WebApiUrlGetLineListForGrid = "api/master/line/gridlist";
        public const string WebApiUrlGetLineInfo = "api/master/line/details";
        public const string WebApiUrlExportLine = "api/master/line/export";

        #endregion

        #region VehicleTracking


        public const string WebApiUrlGetDONumber = "api/master/donumber/list";
        public const string WebApiUrlGetSkuData = "api/lookups/salesdata/list";

        #endregion

        #region QpsDiscount

        public const string WebApiUrlQpsDiscount = "api/qps/qpsdiscount/addorupdate";
        public const string WebApiUrlQpsDiscountList = "api/qps/qpsdiscountlist/list";
        public const string WebApiUrlExportQpsDiscount = "api/qps/QpsDiscount/export";
        public const string WebApiUrlGetQPSDiscountListWithPagination = "api/qps/Qpsdiscountlist/listwithpagination";
        //public const string WebApiUrlGetQpsDiscountById = "api/qps/qspgetdatabyid";
        public const string WebApiUrlGetQpsDiscountById = "api/qps/qpsdiscountlist/getbyId";
        #endregion

        #region GamificationDashboard
        public const string WebApiUrlGetGamificationDashboardList = "api/lookups/gamificationdashboard/list";
        public const string WebApiUrlGetGamificationDashboardWithPagination = "api/lookups/get/gamificationdashboard";
        public const string WebApiUrlPostAddOrUpdateGamificationDashboard = "api/lookups/addorupdate/gamificationdashboard";
        public const string WebApiUrlGetGamificationDashboard = "api/master/GamificationDashboardId/gamificationdashboard";
        #endregion

        #region Complaint Management
        public const string WebApiUrlGetSectionDetail = "api/dynamicform/section/detail";
        public const string WebApiUrlSectionSave = "api/dynamicform/section/save";
        public const string WebApiUrlSectionUpdate = "api/dynamicform/section/update";
        public const string WebApiUrlGetSectionList = "api/dynamicform/section/list";
        public const string WebApiUrlGetQuestionDetail = "api/dynamicform/questions/view";
        public const string WebApiUrlQuestionSave = "api/dynamicform/questions";
        public const string WebApiUrlQuestionUpdate = "api/dynamicform/questions/update";
        public const string WebApiUrlGetSectionQuestion = "api/dynamicform/sections/questions";
        public const string WebApiUrlGetSectionQuestionList = "api/dynamicform/sections/questionsList";
        public const string WebApiUrlGetSectionFromQuestionList = "api/dynamicform/sections/questionsFormList";
        public const string WebApiUrlGetQuestionTypeList = "api/dynamicform/questiontypes";
        public const string WebApiUrlGetSubmittedDetailsList = "api/dynamicform/GetSubmittedDetails";
        public const string WebApiUrlGetSubmittedFormDetailsbyId = "api/dynamicform/SubmittedFormDetailsbyId";
        public const string WebApiUrlGetDynamicFormDetailsById = "api/dynamicform/form/view";
        public const string WebApiUrlPostSaveDynamicForm = "api/dynamicform/form/add";
        public const string WebApiUrlGetDynamicFormView = "api/dynamicform/form/View";
        public const string WebApiUrlGetDynamicFormList = "api/dynamicform/form/list";
        public const string WebApiUrlPutUpdateDynamicForm = "api/dynamicform/form/update";

        //Submitted Forms
        public const string WebApiUrlPostViewSubmittedFormList = "api/dynamicform/submitform/view/list";
        public const string WebApiUrlPostViewSubmittedFormDetails = "api/dynamicform/submitform/view";
        public const string WebApiUrlExportsubmittedForm = "api/dynamicform/submittedForm/export";

        #endregion

        #region TANNumber Mobile API 
        public const string WebApiUrlGetTANNumber = "api/master/tannumber/getid";
        public const string WebApiUrlUpdateTANNumber = "api/master/tannumber/update";
        #endregion

        #region Sauda Booking Restriction

        public const string WebApiUrlGetSaudaBookingRestrictionConfigurationList = "api/sauda/get/saudabooking/restrictionlist";
        public const string WebApiUrlRolesForSaudaBookingConfigurationList = "api/roles/get/booking/restrictionroleids";

        #endregion

        #region SaudaConditionalBooking
        public const string WebApiUrlAddCrossAndUpsell = "api/crossandupsell/add"; 
        public const string WebApiUrlUpdateCrossAndUpsell = "api/crossandupsell/update"; 
        public const string WebApiUrlGetSuadaConditionalBookingConfigurationList = "api/crossandupsell/get/list"; 
        public const string WebApiUrlGetSuadaConditionalBookingConfigurationSkusList = "api/crossandupsell/get/skus/list"; 
        public const string WebApiUrlGetSuadaConditionalBookingConfigurationDetails = "api/crossandupsell/get/details";
        public const string WebApiUrlGetSuadaConditionalBookingConfigurationListForReport = "api/crossandupsell/get/report/list";

        #endregion

        #region Sauda Sales Area Restriction

        public const string WebApiUrlGetSaudaSalesAreaRestrictionConfigurationList = "api/sauda/get/saudabooking/salesarearestrictionlist";
        public const string WebApiUrlPostSaudaSalesAreaRestrictionConfiguration = "api/lookups/saudabooking/salesarearestrictionconfiguration";
        public const string WebApiUrlGetSaudaSalesAreaRestrictionConfigurationDetails = "api/lookups/saudasalesarearestrictionconfiguration/details";

        #endregion

        #region Sauda Modification

        public const string WebApiUrlGetSaudhaModificationList = "api/sauda/modification/list";
        public const string WebApiUrlGetSaudhaModificationDetailsById = "api/sauda/modification/detailsbyid";
        public const string WebApiUrlGetSaudhaModificationDetails = "api/sauda/modification/details";
        public const string WebApiUrlGetSaudaModificationReport = "api/sauda/modification/report";
        public const string WebApiUrlChangeSaudaModificationStatus = "api/mobilesauda/saudamodification/statuschange";
        public const string WebApiUrlChangeSaudaModificationStatusForLoose = "api/mobilesauda/saudamodification/statuschangeforloose";

        #endregion
    }
}
