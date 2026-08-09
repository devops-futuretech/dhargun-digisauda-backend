using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.Service.Common;
using GMCore.Helper;
using GMCore.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using Adani.Solution.DTO.Enums;
using System.Configuration;
using System.Text;
using Adani.Solution.DTO.Common;
using System.Globalization;
using System.Web;
using System.IO;
using Dapper;
using System.Data;

namespace Adani.Solution.Service
{
    public interface ISAPIntegrationService
    {
        ResultDto SaveStateCityDistrict(string decryptedString);
        void SaveCustomer(HANASAPCustomerDtoList inputdto);
        void GetTradeTicketDetails();
        void UpdateTradeTicketNumber(string decryptedString);
        void GetSaudaDetails(List<long> saudaIds, bool IsApproval);
        void SendSaudaModificationInfoToSAP(List<long> saudaModificationIds, bool IsApproval);
        void UpdateSaudaNumber(List<HANASaudaCommonFunctionList> inputdto);
        void GetSaudaApprovalDetails(List<long> SaudaOrderIds);
        void SaveInvoice(InvoiceDto inputdto);
        // void SaveSaudaLimit(HANASaudaLimitDtoList inputdto);
        void SaudaAmendment(HANASaudaAmendmentDtoList inputdto);
        void GetSaudaLimitDetails();
        //void SaveSku(string decryptedString);
        void SaveCreditMaster(HANACreditMasterDtoList inputdto);
        void SaveDepot(string decryptedString);
        void SaveCustomerLedger(HANACustomerLedgerDtoList inputdto);
        void DODelete(string decryptedString);
        void DOUpdate(string decryptedString);
        void CreateTradeTicket(TradeTicketListDto tradeTicketListDto);
        void SaudaCreate(SaudaCreateSAPToAPPDto SaudaListDetails);
        ResultDto ContractTrigger(OpenContractRequestDTOList inputDto);
        void InvoiceStatusChange(string decryptedString);
        void LiftingRequestEnquiryNumberUpdate(List<HANASaudaCommonFunctionList> inputdto);
        void GetLiftingRequestEnquiryNumberOutboundDetails(List<long> liftingRequestId, bool IsReprocess);
        void GetSpecialityFatTradeTicketDetails();
        void CreateTradeTicketSF(string decryptedString);
        // void PendingContractReport(PendingContractListDto pendingContractList);
        void SalesReport(AWLSalesRegisterOutputDto inputdto);
        void GetSaudaConversionDetails(List<long> SaudaConversionId);
        void SaudaConversionNumberUpdate(HANASaudaConversionDtoList inputdto);
        void SaudaExtensionUpdate(List<HANASaudaCommonFunctionList> inputdto);
        void GetSaudaExtensionDetails(List<long> SaudaExtensionId, bool IsReprocess);
        void ChequeInventoryReport(HANAChequeStatusDtoList inputdto);
        //void GetSaudaDetailsForLooseVertical(List<long> saudaOrderIds);
        //void SaudaRelease(string decryptedString);
        void SaudaApprovalConfirmation(List<HANASaudaCommonFunctionList> inputdto);
        void SaveSkuDetails(HANASAPSku inputdto);
        //void TruckPlacementTrackerReport(TruckPlacementTrackerList truckPlacementTrackerList);
        ResultDto SaudaReleaseAPPToSAP(List<string> saudaNumbers);
        ResultDto SaudaConversionAPPToSAP(List<string> saudaNumbers);
        void SaudaConversionSAPToAPP(SaudaConversionSAPToAPPDto inputDto);
        void SaudaReleaseSAPToAPP(SaudaReleaseSAPToAPPDto inputDto);
        ResultDto SaudaExtenstionAPPToSAP(List<string> saudaNumbers, long extensionDays);
        void SavePricingDetails(HANAPricing inputdto);
        void LiftingRequestInvoicNoUpdate(List<HANASaudaCommonFunctionList> inputdto);
        void LiftingRequestDeliveryNoUpdate(List<HANASaudaCommonFunctionList> inputdto);
        void LiftRequestCreateSapToApp(SalesOrderCreate inputdto);
        void ContractOpenBalanceRequest(List<OpenContractRequestDTO> inputDto, string salesOrg, string distChannel, string division);
        void ContractOpenBalanceResponce(HANAOpenBalAndOpenContractDTOList inputdto);
        void SaudaLimitResponce(HANASaudaLimitList inputDto);
        void SaveOverduePayment(HANACustomerLedgerDtoList inputdto);
        void PendingContractAutoTrigger();
        void CustomerLedgerRequest(SAPCustomerLedgerRequestDTO inputDto);
        void CustomerLedgerAutoTrigger();
        void EmployeeRequestActiveUsers();
        void EmployeeRequestInActiveUsers();
        ResultDto SaveCallRecordingOfCustomers(CallRecordingInputDto inputDto, HttpPostedFile file, string imageFileName, int pageId);
        ResultDto DialerMobileNumberByBDODetails(CallRecordingGetInputDto inputDto);
        void SaudaExpiredNotification();
        void OverDueNotification();
        ResultDto GetDealerDetailsByBDO(CallRecordingGetInputDto inputDto);
        ResultDto GetBDODetailsWithMasterData(CallRecordingGetInputDto inputDto);
        void AccountStatement(List<SAPAccountStatementDto> inputDto);

        void ContractAvilableLimitCalculate(string saudaNumber);
        void UpdateSaudaChange(List<HANASaudaCommonFunctionList> inputdto);

    }
    public class SAPIntegrationService : ISAPIntegrationService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("SAPIntegration Service");
        private const string ServiceName = "SAPIntegration Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly ISftpConnectorService _sftpConnectorService;
        private readonly INotificationService _notificationService;


        public SAPIntegrationService(IAdaniContext salesContext, IResultService resultService, ISftpConnectorService sftpConnectorService, INotificationService notificationService)
        {
            try
            {
                _emamiContext = salesContext;
                _resultService = resultService;
                _sftpConnectorService = sftpConnectorService;
                _notificationService = notificationService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for Lookup Service", exception);
            }
        }

        #region Old Methods 

        #region StateCityDistrict
        /// <summary>
        /// Method to save State City District
        /// </summary>
        /// <param name="decryptedString"></param>
        /// <returns></returns>
        public ResultDto SaveStateCityDistrict(string decryptedString)
        {
            _methodName = "StateCityDistrict";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var customerSyncData = string.Empty;
            var messageSync = string.Empty;
            var sapDataSyncResultDto = new SapDataSyncResultDto();
            sapDataSyncResultDto.SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
            var dataSynced = 0;
            var resultDto = new ResultDto();
            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
            var customerDtoList = JsonConvert.DeserializeObject<List<SAPCustomerDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = customerDtoList.Count;
            try
            {
                var userList = new List<User>();
                var errorCustomerList = new List<SAPCustomerDto>();
                var userId = UtilityHelper.LongTryToParse(jarray[0]["loginUserId"].ToString());
                var errorMessageList = new List<string>();
                foreach (var customerDto in customerDtoList)
                {
                    var stateId = 0;
                    var districtId = 0;
                    var errorMessage = customerDto.Code;

                    var stateContext = _emamiContext.State.AsNoTracking().FirstOrDefault(_ => _.StateName.ToLower() == customerDto.State.ToLower());
                    if (stateContext == null && !string.IsNullOrEmpty(customerDto.State.ToLower()))
                    {
                        var state = new State
                        {
                            CountryId = 1,
                            StateName = customerDto.State,
                            IsActive = true,
                            CreatedBy = userId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.State.Add(state);
                        _emamiContext.SaveChanges();
                        stateId = state.Id;
                    }
                    else if (stateContext != null)
                    {
                        stateId = stateContext.Id;
                    }

                    var districtContext = _emamiContext.District.AsNoTracking().FirstOrDefault(_ => _.DistrictName.ToLower() == customerDto.District.ToLower());
                    if (districtContext == null && !string.IsNullOrEmpty(customerDto.District.ToLower()) && stateId != 0)
                    {
                        var district = new District
                        {
                            StateId = stateId,
                            DistrictName = customerDto.District,
                            IsActive = true,
                            CreatedBy = userId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.District.Add(district);
                        _emamiContext.SaveChanges();
                        districtId = district.Id;
                    }
                    else if (districtContext != null)
                    {
                        districtId = districtContext.Id;
                    }

                    var cityContext = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.CityName.ToLower() == customerDto.City.ToLower());
                    if (cityContext == null && !string.IsNullOrEmpty(customerDto.City.ToLower()) && districtId != 0)
                    {
                        var city = new City
                        {
                            DistrictId = districtId,
                            CityName = customerDto.City,
                            IsActive = true,
                            //TerritoryId = 1,
                            CreatedBy = userId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.City.Add(city);
                        _emamiContext.SaveChanges();
                    }
                }
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                if (errorMessageList.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Response = sapDataSyncResultDto;
                    resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                }
                else
                {
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = sapDataSyncResultDto;
                    resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                }

                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                messageSync = message;
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = messageSync;
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion

        #region Depot
        public void SaveDepot(string decryptedString)
        {
            _methodName = "SaveDepot";
            _logger.Info($"SAP Service Start : {ServiceName} Controller-Method {_methodName} ");
            var resultDto = new ResultDto();
            var errorList = new List<SAPDepotDto>();
            var liftingSyncData = string.Empty;
            var messageSync = string.Empty;
            var sapDataSyncResultDto = new SapDataSyncResultDto();
            sapDataSyncResultDto.SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
            var dataSynced = 0;
            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
            var userId = UtilityHelper.LongTryToParse(jarray[0]["loginUserId"].ToString());
            var syncFolder = jarray[0]["syncFolder"].ToString();
            var subject = jarray[0]["subject"].ToString();
            var folderPath = ConsoleSettings.InboundDirectoryPath(syncFolder);
            var inputDto = _sftpConnectorService.GetSFTPFile(folderPath, syncFolder);
            var depotDtoList = !string.IsNullOrEmpty(inputDto.Response.ToString()) ? (List<SAPDepotDto>)inputDto.Response : new List<SAPDepotDto>();
            subject = string.Concat(subject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = depotDtoList.Count;
            try
            {
                if (depotDtoList != null && depotDtoList.Any())
                {
                    var depotList = new List<Depot>();
                    var errorMessageList = new List<string>();
                    var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    using (var _emamiContext = new AdaniContext())
                    {
                        #region Get Common Datas
                        var cityList = _emamiContext.City.AsNoTracking();
                        var stateList = _emamiContext.State.AsNoTracking();
                        var depotContextList = _emamiContext.Depots.AsNoTracking();
                        #endregion
                        foreach (var depot in depotDtoList)
                        {
                            var errorFlag = true;
                            var errorMessage = string.Concat("Depot Code : ", depot.PlantCode);
                            if (depot == null)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.InvalidRequest, errorMessage);
                                errorFlag = false;
                            }
                            if (string.IsNullOrEmpty(depot.PlantCode))
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.DepotCodeIsEmpty, errorMessage);
                                errorFlag = false;
                            }

                            var cityContext = cityList.FirstOrDefault(_ => _.CityName.ToLower() == depot.City.ToLower());
                            var stateContext = stateList.FirstOrDefault(_ => _.StateName.ToLower() == depot.StateName.ToLower());

                            if (errorFlag)
                            {
                                var codeExist = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Code == depot.PlantCode);
                                if (codeExist != null)
                                {
                                    var sqlUpdate = "UPDATE Depots SET Email = @Email,Name = @Name,ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy, " +
                                        "IsSAPDataSyncOrNot =@IsSAPDataSyncOrNot,IsPlant=@IsPlant WHERE Id = @Id";
                                    var parameters = new[]{
                                new SqlParameter("@Email", depot.Email),
                                new SqlParameter("@Name", depot.Name),
                                new SqlParameter("@IsSAPDataSyncOrNot", true),
                                new SqlParameter("@ModifiedDate", currentDate),
                                new SqlParameter("@ModifiedBy", userId),
                                new SqlParameter("@Id", codeExist.Id),
                                new SqlParameter("@IsPlant", depot.IsPlant)
                            };
                                    _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                }
                                else
                                {
                                    var codeListExist = depotList.FirstOrDefault(_ => _.Code == depot.PlantCode);
                                    if (codeListExist == null)
                                    {
                                        var depotDto = new Depot
                                        {
                                            Code = depot.PlantCode,
                                            //CityId = cityContext != null ? cityContext.Id : 0,
                                            //DistrictId = cityContext != null ? cityContext.DistrictId : 0,
                                            Email = depot.Email,
                                            Name = depot.Name,
                                            //StateId = stateContext != null ? stateContext.Id : 0,
                                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                            CreatedBy = userId,
                                            IsSAPData = true,
                                            IsSAPDataSyncOrNot = true,
                                            IsActive = false,
                                            IsPlant = depot.IsPlant
                                        };
                                        if (depotDto != null)
                                        {
                                            depotList.Add(depotDto);
                                        }
                                    }
                                }
                                dataSynced++;
                            }
                            else
                            {
                                errorMessageList.Add(errorMessage);
                                errorList.Add(depot);
                            }
                        }
                        if (null != depotList && depotList.Any())
                        {
                            _emamiContext.BulkInsertProxy(depotList);
                        }
                        _emamiContext.SaveChanges();
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorList;
                    }

                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Concat(Constants.ServiceErrorMessage, string.Join(",", errorMessageList));
                    }
                    else
                    {
                        sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, inputDto, subject);
                }
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception in Service: {exception} SAP Service";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = string.Concat(Constants.ServiceErrorMessage, message);
                _logger.Error(message);
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, inputDto, subject);
            }
        }
        #endregion

        #region CustomerBroker
        /// <summary>
        /// Method to save customer
        /// </summary>
        /// <param name="employeeDto"></param>
        /// <returns></returns>
        public void SaveCustomer(HANASAPCustomerDtoList inputdto)
        {
            _methodName = "SaveCustomer";
            _logger.Info($"SAP Service Start : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputdto)}");
            var resultDto = new ResultDto();
            var errorList = new List<HANASAPCustomerDto>();
            var errorMessageList = new List<string>();
            var dataSynced = 0;
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var syncType = ConsoleSettings.CustomerFolder;
            var subject = string.Concat(ConsoleSettings.CustomerFolder, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            long userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var customerDtoList = inputdto.SAPUserList != null ? inputdto.SAPUserList : new List<HANASAPCustomerDto>();
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = customerDtoList.Count;
            var customerSyncData = string.Empty;
            try
            {
                var userList = new List<User>();
                if (customerDtoList != null && customerDtoList.Any())
                {
                    using (var _emamiContext = new AdaniContext())
                    {
                        #region Get Common Datas
                        var verticalContextList = _emamiContext.Divisions.AsNoTracking();
                        var cityContextList = _emamiContext.City.AsNoTracking();
                        var stateContextList = _emamiContext.State.AsNoTracking();
                        var zoneContextList = _emamiContext.ZoneStateMappings.AsNoTracking();
                        var districtContextList = _emamiContext.District.AsNoTracking();
                        #endregion

                        foreach (var customerDto in customerDtoList)
                        {

                            if (customerDto.AccountGroup == "ZOSH")
                            {
                                customerDto.RoleId = (int)DTO.Enums.Role.ShipToParty;
                                customerDto.CustomerGroup = string.Empty;
                                subject = string.Concat("Ship To Party", " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
                            }
                            else
                            {
                                customerDto.RoleId = (int)DTO.Enums.Role.Broker;
                                customerDto.CustomerGroup = UtilityHelper.GetEnumDescription(CustomerGroup.Broker);
                                subject = string.Concat("Broker", " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
                            }
                            var errorFlag = true;
                            var errorMessage = string.Concat("Customer Code : ", customerDto.Code, "-", customerDto.VerticalCode);
                            if (customerDto == null)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.InvalidRequest, errorMessage);
                                errorFlag = false;
                            }
                            if (string.IsNullOrEmpty(customerDto.Code))
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.CustomerCodeIsEmpty, errorMessage);
                                errorFlag = false;
                            }
                            if (string.IsNullOrEmpty(customerDto.Name1))
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.CustomerNameIsEmpty, errorMessage);
                                errorFlag = false;
                            }
                            var verticalContext = verticalContextList.FirstOrDefault(_ => _.Code.ToLower() == customerDto.VerticalCode.ToLower());
                            if (verticalContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.VerticalCodeIsEmpty, customerDto.VerticalCode), errorMessage);
                                errorFlag = false;
                            }
                            var cityContext = cityContextList.FirstOrDefault(_ => _.CityName.ToLower() == customerDto.City.ToLower());
                            if (cityContext != null)
                            {
                                customerDto.CityId = cityContext.Id;
                            }
                            //else
                            //{
                            //    errorMessage = Constants.BindErrorMessage(Constants.CityNameNotMatch +" - " + customerDto.City, errorMessage);
                            //    errorFlag = false;
                            //}
                            var stateContext = stateContextList.FirstOrDefault(_ => _.StateName.ToLower() == customerDto.State.ToLower());
                            if (stateContext != null)
                            {
                                customerDto.StateId = stateContext.Id;
                            }
                            var stateId = stateContext != null ? stateContext.Id : 0;
                            var zoneContext = zoneContextList.FirstOrDefault(_ => _.StateId == stateId);
                            //else
                            //{
                            //    errorMessage = Constants.BindErrorMessage(Constants.StateNameNotMatch + " - " + customerDto.State, errorMessage);
                            //    errorFlag = false;
                            //}
                            var districtContext = districtContextList.FirstOrDefault(_ => _.DistrictName.ToLower() == customerDto.District.ToLower());
                            if (districtContext != null)
                            {
                                customerDto.DistrictId = districtContext.Id;
                            }
                            //else
                            //{
                            //    errorMessage = Constants.BindErrorMessage(Constants.DistrictNameNotMatch + " - " + customerDto.District, errorMessage);
                            //    errorFlag = false;
                            //}
                            if (errorFlag)
                            {
                                var verticalId = verticalContext != null ? verticalContext.Id : 0;
                                var codeExist = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Code == customerDto.Code && _.CustomerGroup == customerDto.CustomerGroup
                                //&& _.DivisionId == verticalId
                                );
                                if (codeExist != null)
                                {

                                    var sqlUpdate = "UPDATE Users SET Name = @Name,Region = @Region,Street = @Street,ADRNR = @ADRNR,GSTN = @GSTN,MobileNumber = @MobileNumber," +
                                        "Email = @Email,CentralDeletionFlag = @CentralDeletionFlag,IsSAPData = @IsSAPData,ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy, " +
                                        "IsSAPDataSyncOrNot =@IsSAPDataSyncOrNot,FSSAINumber=@FSSAINumber WHERE Id = @Id";
                                    var parameters = new[]{
                                new SqlParameter("@Name", string.IsNullOrEmpty(string.Concat(customerDto.Name1)) ? codeExist.Name : string.Concat(customerDto.Name1, "-", customerDto.Name2).TrimEnd('-')),
                                new SqlParameter("@Region", string.IsNullOrEmpty(customerDto.Region) ? codeExist.Region : customerDto.Region),
                                new SqlParameter("@Street", string.IsNullOrEmpty(customerDto.Street) ? codeExist.Street : customerDto.Street),
                                new SqlParameter("@ADRNR", string.IsNullOrEmpty(customerDto.ADRNR) ? codeExist.ADRNR : customerDto.ADRNR),
                                new SqlParameter("@GSTN", string.IsNullOrEmpty(customerDto.GSTN) ? string.IsNullOrEmpty(codeExist.GSTN) ? string.Empty : codeExist.GSTN : customerDto.GSTN),
                                new SqlParameter("@MobileNumber", string.IsNullOrEmpty(customerDto.MobileNumber) ? codeExist.MobileNumber : customerDto.MobileNumber),
                                new SqlParameter("@Email", string.IsNullOrEmpty(customerDto.Email) ? string.IsNullOrEmpty(customerDto.Email) ? string.Empty : codeExist.Email : customerDto.Email),
                                new SqlParameter("@CentralDeletionFlag", string.IsNullOrEmpty(customerDto.CentralDeletionFlag) ? string.IsNullOrEmpty(codeExist.CentralDeletionFlag) ? string.Empty : codeExist.CentralDeletionFlag : customerDto.CentralDeletionFlag),
                                new SqlParameter("@IsSAPData", true),
                                new SqlParameter("@IsSAPDataSyncOrNot", true),
                                new SqlParameter("@ModifiedDate", currentDate),
                                new SqlParameter("@ModifiedBy", userId),
                                new SqlParameter("@Id", codeExist.Id),
                                new SqlParameter("@FSSAINumber", customerDto.FSSAINumber),
                            };
                                    _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                }
                                else
                                {
                                    var codeListExist = userList.FirstOrDefault(_ => _.Code == customerDto.Code && _.CustomerGroup == customerDto.CustomerGroup
                                    //&& _.DivisionId == verticalId
                                    );
                                    if (codeListExist == null)
                                    {
                                        var user = new User
                                        {
                                            Code = customerDto.Code,
                                            Name = !string.IsNullOrEmpty(customerDto.Name2) ? string.Concat(customerDto.Name1, "-", customerDto.Name2).TrimEnd('-') : customerDto.Name1,
                                            Password = "JXJK14rJK/nCUGdsaZIc2w==",
                                            City = customerDto.City,
                                            Region = customerDto.Region,
                                            Street = customerDto.Street,
                                            ADRNR = customerDto.ADRNR,
                                            GSTN = customerDto.GSTN,
                                            MobileNumber = customerDto.MobileNumber,
                                            Email = customerDto.Email,
                                            State = customerDto.State,
                                            //CentralDeletionFlag = customerDto.CentralDeletionFlag,
                                            //IsSAPData = true,
                                            //IsSAPDataSyncOrNot = true,
                                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                            CreatedBy = userId,
                                            StateId = customerDto.StateId,
                                            CityId = customerDto.CityId,
                                            DistrictId = customerDto.DistrictId,
                                            IsActive = customerDto.RoleId == (int)DTO.Enums.Role.ShipToParty ? true : false,
                                            CustomerGroup = customerDto.CustomerGroup,
                                            //DivisionId = verticalContext.Id,
                                            FSSAINumber = customerDto.FSSAINumber,
                                            ZoneId = zoneContext != null ? zoneContext.ZoneId : 0,
                                            //  TerritoryId = districtContext != null ? districtContext.TerritoryId : 0,
                                            District = customerDto.District,
                                        };
                                        if (user != null)
                                        {
                                            userList.Add(user);
                                        }
                                    }


                                }
                                dataSynced++;
                            }
                            else
                            {
                                errorMessageList.Add(errorMessage);
                                errorList.Add(customerDto);
                            }

                        }

                        if (null != userList && userList.Any())
                        {
                            _emamiContext.BulkInsertProxy(userList);
                        }
                        _emamiContext.SaveChanges();

                        //UserRole,UserDepotMapping, DealerLocation table insert
                        var userRoleList = new List<UserRole>();
                        var userDepotMappingList = new List<UserDepotMapping>();
                        var dealerLocationList = new List<DealerLocation>();
                        #region Get Common Datas                   
                        var usersList = _emamiContext.Users.AsNoTracking();
                        var userRoleContextList = _emamiContext.UserRoles.AsNoTracking();
                        var depotContextList = _emamiContext.Depots.AsNoTracking();
                        var userDepotMappingContextList = _emamiContext.UserDepotMapping.AsNoTracking();
                        var dealerLocationContextList = _emamiContext.DealerLocation.AsNoTracking();
                        #endregion
                        foreach (var customerDto in customerDtoList)
                        {
                            customerSyncData = customerDto.RoleId == (int)Adani.Solution.DTO.Enums.Role.Broker ? "Broker" : "Customer";
                            var verticalContext = verticalContextList.FirstOrDefault(_ => _.Code.ToLower() == customerDto.VerticalCode.ToLower());
                            var verticalId = verticalContext != null ? verticalContext.Id : 0;
                            var customerExist = usersList.FirstOrDefault(_ => _.Code == customerDto.Code
                            //&& _.DivisionId == verticalId
                            );
                            if (customerExist != null)
                            {
                                var userRoleExist = userRoleContextList.FirstOrDefault(_ => _.RoleId == customerDto.RoleId && _.UserId == customerExist.Id);
                                var userRoleListExist = userRoleList.FirstOrDefault(_ => _.RoleId == customerDto.RoleId && _.UserId == customerExist.Id);
                                if (userRoleExist == null && userRoleListExist == null)
                                {
                                    var userRole = new UserRole
                                    {
                                        RoleId = customerDto.RoleId,
                                        UserId = customerExist.Id,
                                        CreatedBy = userId,
                                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                        IsSAPData = true,

                                    };
                                    userRoleList.Add(userRole);

                                }
                                var depotExist = depotContextList.FirstOrDefault(_ => _.Code == customerDto.DeliveringPlant);
                                if (depotExist != null)
                                {
                                    var userDepotMappingExist = userDepotMappingContextList.FirstOrDefault(_ => _.DepotId == depotExist.Id && _.UserId == customerExist.Id);
                                    var userDepotMappingListExist = userDepotMappingList.FirstOrDefault(_ => _.DepotId == depotExist.Id && _.UserId == customerExist.Id);
                                    if (userDepotMappingExist == null && userDepotMappingListExist == null && UtilityHelper.LongTryToParse(customerDto.DeliveringPlant) != 0)
                                    {
                                        var userDepotMapping = new UserDepotMapping
                                        {
                                            DepotId = depotExist.Id,
                                            UserId = customerExist.Id,
                                            CreatedBy = userId,
                                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                            IsSAPData = true,
                                        };
                                        userDepotMappingList.Add(userDepotMapping);
                                    }
                                }
                                else if (depotExist == null && customerDto.RoleId == (int)DTO.Enums.Role.Broker)
                                {
                                    errorMessageList.Add(string.Format(Constants.DeportNotExistEmpty, customerDto.DeliveringPlant, customerDto.Code));
                                }

                                var dealerLocationExist = dealerLocationContextList.FirstOrDefault(_ => _.StateId == customerDto.StateId && _.CityId == customerDto.CityId && _.DistrictId == customerDto.DistrictId && _.UserId == customerExist.Id);
                                var dealerLocationListExist = dealerLocationList.FirstOrDefault(_ => _.StateId == customerDto.StateId && _.CityId == customerDto.CityId && _.DistrictId == customerDto.DistrictId && _.UserId == customerExist.Id);
                                if (dealerLocationExist == null && dealerLocationListExist == null)
                                {
                                    var dealerLocation = new DealerLocation
                                    {
                                        StateId = customerDto.StateId,
                                        CityId = customerDto.CityId,
                                        DistrictId = customerDto.DistrictId,
                                        UserId = customerExist.Id,
                                        CreatedBy = userId,
                                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                        IsSAPData = true,

                                    };
                                    dealerLocationList.Add(dealerLocation);
                                }
                            }
                        }
                        if (userRoleList != null && userRoleList.Any())
                        {
                            _emamiContext.BulkInsertProxy(userRoleList);
                        }
                        if (userDepotMappingList != null && userDepotMappingList.Any())
                        {
                            _emamiContext.BulkInsertProxy(userDepotMappingList);
                        }
                        if (dealerLocationList != null && dealerLocationList.Any())
                        {
                            _emamiContext.BulkInsertProxy(dealerLocationList);
                        }
                        _emamiContext.SaveChanges();
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorList;
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = customerDtoList;
                        sapDataSyncResultDto.SuccessRecordDetailsResponse = customerDtoList.Except(errorList).ToList();
                    }

                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    }
                    else
                    {
                        sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                    }
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncType, null, subject);
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = customerDtoList;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                _logger.Error(message);
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncType, null, subject);
            }
        }

        #endregion

        #region Sku
        //public void SaveSku(string decryptedString)
        //{
        //    _methodName = "Sku";
        //    _logger.Info($"SAP Service Start : {ServiceName} Controller-Method {_methodName}");
        //    var resultDto = new ResultDto();
        //    var liftingSyncData = string.Empty;
        //    var messageSync = string.Empty;
        //    var sapDataSyncResultDto = new SapDataSyncResultDto();
        //    var errorList = new List<SAPSkuDto>();
        //    sapDataSyncResultDto.SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
        //    var dataSynced = 0;
        //    var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
        //    var userId = UtilityHelper.LongTryToParse(jarray[0]["loginUserId"].ToString());
        //    var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //    var syncFolder = jarray[0]["syncFolder"].ToString();
        //    var subject = jarray[0]["subject"].ToString();
        //    var folderPath = ConsoleSettings.InboundDirectoryPath(syncFolder);
        //    var inputDto = _sftpConnectorService.GetSFTPFile(folderPath, syncFolder);
        //    var skuDtoList = !string.IsNullOrEmpty(inputDto.Response.ToString()) ? (List<SAPSkuDto>)inputDto.Response : new List<SAPSkuDto>();
        //    subject = string.Concat(subject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
        //    sapDataSyncResultDto.OutstandingResult.DataRetrieved = skuDtoList.Count;
        //    try
        //    {
        //        if (skuDtoList != null && skuDtoList.Any())
        //        {
        //            var skuList = new List<Sku>();
        //            var errorMessageList = new List<string>();
        //            using (var _emamiContext = new EmamiContext())
        //            {
        //                #region Get Common Datas
        //                var verticalContextList = _emamiContext.Divisions.AsNoTracking();
        //                var oilTypeContextList = _emamiContext.OilTypes.AsNoTracking();
        //                var packTypeContextList = _emamiContext.PackTypes.AsNoTracking();
        //                var packGroupsContextList = _emamiContext.OilPackingTypes.AsNoTracking();
        //                //var materialTypesContextList = _emamiContext.MaterialTypes.AsNoTracking();
        //                #endregion

        //                foreach (var sku in skuDtoList)
        //                {
        //                    var errorFlag = true;
        //                    var errorMessage = string.Concat("Sku Code : ", sku.SkuCode);
        //                    if (sku == null)
        //                    {
        //                        errorMessage = Constants.BindErrorMessage(Constants.InvalidRequest, errorMessage);
        //                        errorFlag = false;
        //                    }
        //                    if (string.IsNullOrEmpty(sku.SkuCode))
        //                    {
        //                        errorMessage = Constants.BindErrorMessage(Constants.SkuCodeIsEmpty, errorMessage);
        //                        errorFlag = false;
        //                    }

        //                    var verticalContext = verticalContextList.FirstOrDefault(_ => _.Code == sku.VerticalCode);
        //                    if (verticalContext == null)
        //                    {
        //                        errorMessage = Constants.BindErrorMessage(string.Format(Constants.VerticalCodeIsEmpty, sku.VerticalCode), errorMessage);
        //                        errorFlag = false;
        //                    }
        //                    var oilTypeContext = oilTypeContextList.FirstOrDefault(_ => _.SAPCode == sku.OilTypeCode);
        //                    if (oilTypeContext == null)
        //                    {
        //                        errorMessage = Constants.BindErrorMessage(string.Format(Constants.OilTypeCodeIsEmpty, sku.OilTypeCode), errorMessage);
        //                        errorFlag = false;
        //                    }
        //                    var packTypeContext = packTypeContextList.FirstOrDefault(_ => _.SAPCode == sku.PackTypeCode);
        //                    if (packTypeContext == null)
        //                    {
        //                        errorMessage = Constants.BindErrorMessage(string.Format(Constants.PackTypeTypeCodeIsEmpty, sku.PackTypeCode), errorMessage);
        //                        errorFlag = false;
        //                    }
        //                    var packGroupsContext = packGroupsContextList.FirstOrDefault(_ => _.SAPName == sku.PackGroups);
        //                    if (packGroupsContext == null)
        //                    {
        //                        errorMessage = Constants.BindErrorMessage(string.Format(Constants.PackGroupTypeCodeIsEmpty, sku.PackTypeCode), errorMessage);
        //                        errorFlag = false;
        //                    }
        //                    //var materialTypesContext = materialTypesContextList.FirstOrDefault(_ => _.Name == sku.MaterialType);
        //                    //if (materialTypesContext == null)
        //                    //{
        //                    //    errorMessage = Constants.BindErrorMessage(string.Format(Constants.MaterialTypesCodeIsEmpty, sku.MaterialType), errorMessage);
        //                    //    errorFlag = false;
        //                    //}
        //                    if (errorFlag)
        //                    {
        //                        var codeExist = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.SkuCode == sku.SkuCode && _.OilTypeId == oilTypeContext.Id && _.DivisionId == verticalContext.Id);
        //                        if (codeExist == null)
        //                        {
        //                            var skuListCheck = skuList.FirstOrDefault(_ => _.SkuCode == sku.SkuCode && _.OilTypeId == oilTypeContext.Id && _.DivisionId == verticalContext.Id);
        //                            if (skuListCheck == null)
        //                            {
        //                                var skuDto = new Sku
        //                                {
        //                                    SkuCode = sku.SkuCode,
        //                                    SkuName = sku.MaterialDescription,
        //                                    OilTypeId = oilTypeContext.Id,
        //                                    PackTypeId = packTypeContext.Id,
        //                                    DivisionId = verticalContext.Id,
        //                                    DivisionGroupId = UtilityHelper.LongTryToParse(sku.VerticalGroupCode),
        //                                    PackGroupId = packGroupsContext != null ? packGroupsContext.Id : 0,
        //                                    //MaterialTypeId = materialTypesContext != null ? materialTypesContext.Id : 0,
        //                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        //                                    CreatedBy = userId,
        //                                    IsSAPData = true,
        //                                    IsSAPDataSyncOrNot = true,
        //                                    IsActive = false
        //                                };
        //                                if (skuDto != null)
        //                                {
        //                                    skuList.Add(skuDto);
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            var sqlUpdate = "UPDATE Skus SET SkuName = @SkuName,OilTypeId = @OilTypeId,PackTypeId = @PackTypeId,PackGroupId=@PackGroupId,ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy, " +
        //                                "VerticalGroupId =@VerticalGroupId,MaterialTypeId =@MaterialTypeId WHERE Id = @Id";
        //                            var parameters = new[]{
        //                        new SqlParameter("@SkuName", sku.MaterialDescription),
        //                        new SqlParameter("@OilTypeId", oilTypeContext != null ? oilTypeContext.Id : codeExist.OilTypeId),
        //                        new SqlParameter("@PackTypeId", packTypeContext != null ? packTypeContext.Id:codeExist.PackTypeId),
        //                        new SqlParameter("@PackGroupId", packGroupsContext != null ? packGroupsContext.Id : codeExist.PackGroupId),
        //                        //new SqlParameter("@MaterialTypeId", materialTypesContext != null ? materialTypesContext.Id : codeExist.MaterialTypeId),
        //                        new SqlParameter("@VerticalGroupId", UtilityHelper.LongTryToParse(sku.VerticalGroupCode)),
        //                        new SqlParameter("@ModifiedDate", currentDate),
        //                        new SqlParameter("@ModifiedBy", userId),
        //                        new SqlParameter("@Id", codeExist.Id)
        //                    };
        //                            _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
        //                        }
        //                        dataSynced++;
        //                    }
        //                    else
        //                    {
        //                        errorMessageList.Add(errorMessage);
        //                        errorList.Add(sku);
        //                    }
        //                }
        //                if (null != skuList && skuList.Any())
        //                {
        //                    _emamiContext.BulkInsertProxy(skuList);
        //                }
        //                _emamiContext.SaveChanges();
        //                var skuUomMappingList = new List<SkuUomMapping>();

        //                #region Get Common Datas
        //                var skuContextList = _emamiContext.Skus.AsNoTracking();
        //                var uomContextList = _emamiContext.Uom.AsNoTracking();
        //                var skuUomMappingContextList = _emamiContext.SkuUomMapping.AsNoTracking();
        //                #endregion

        //                foreach (var sku in skuDtoList)
        //                {
        //                    var errorFlags = true;
        //                    var errorMessage = string.Empty;
        //                    var verticalContext = verticalContextList.FirstOrDefault(_ => _.Code == sku.VerticalCode);
        //                    var verticalId = verticalContext != null ? verticalContext.Id : 0;

        //                    var oilTypeContext = oilTypeContextList.FirstOrDefault(_ => _.SAPCode == sku.OilTypeCode);
        //                    var oilTypeId = oilTypeContext != null ? oilTypeContext.Id : 0;

        //                    var packTypeContext = packTypeContextList.FirstOrDefault(_ => _.SAPCode == sku.PackTypeCode);
        //                    var packTypeId = packTypeContext != null ? packTypeContext.Id : 0;
        //                    var skuContext = skuContextList.FirstOrDefault(_ => _.SkuCode == sku.SkuCode && _.OilTypeId == oilTypeId && _.DivisionId == verticalId && _.PackTypeId == packTypeId);
        //                    if (skuContext == null)
        //                    {
        //                        errorMessage = Constants.BindErrorMessage(string.Format(Constants.SkuDetailsIsEmpty, sku.SkuCode), errorMessage);
        //                        errorFlags = false;
        //                    }
        //                    var uomContext = uomContextList.FirstOrDefault(_ => _.SAPName == sku.ConvertionType);
        //                    if (uomContext == null)
        //                    {
        //                        errorMessage = Constants.BindErrorMessage(string.Format(Constants.ConvertionTypeIsEmpty, sku.ConvertionType), errorMessage);
        //                        errorFlags = false;
        //                    }
        //                    if (null != skuContext && errorFlags)
        //                    {
        //                        var skuUomMappingContext = skuUomMappingContextList.FirstOrDefault(_ => _.SkuId == skuContext.Id && _.UomId == uomContext.Id);
        //                        if (skuUomMappingContext == null)
        //                        {
        //                            var skuUomMappingDto = new SkuUomMapping
        //                            {
        //                                ConversionFactor = sku.ConvertionFactor,
        //                                SkuId = skuContext.Id,
        //                                RelationUomId = (int)DTO.Enums.Uom.Nos,
        //                                UomId = uomContext.Id,
        //                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        //                                CreatedBy = userId,
        //                            };
        //                            if (skuUomMappingDto != null)
        //                            {
        //                                skuUomMappingList.Add(skuUomMappingDto);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            var sqlUpdate = "UPDATE SkuUomMappings SET ConversionFactor = @ConversionFactor WHERE Id = @Id";
        //                            var parameters = new[]{
        //                            new SqlParameter("@ConversionFactor", sku.ConvertionFactor),
        //                            new SqlParameter("@Id", skuUomMappingContext.Id)
        //                        };
        //                            _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
        //                        }

        //                    }
        //                    else
        //                    {
        //                        if (!string.IsNullOrEmpty(errorMessage))
        //                        {
        //                            errorMessageList.Add(errorMessage);
        //                            errorList.Add(sku);
        //                        }
        //                    }
        //                }
        //                if (null != skuUomMappingList && skuUomMappingList.Any())
        //                {
        //                    _emamiContext.BulkInsertProxy(skuUomMappingList);
        //                }
        //                _emamiContext.SaveChanges();
        //                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
        //                sapDataSyncResultDto.ErrorDetailsResponse = errorList;
        //            }

        //            if (errorMessageList.Any())
        //            {
        //                resultDto.IsSuccess = false;
        //                resultDto.ErrorDto.Response = sapDataSyncResultDto;
        //                resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
        //            }
        //            else
        //            {
        //                resultDto.IsSuccess = true;
        //                resultDto.SuccessDto.Response = sapDataSyncResultDto;
        //                resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
        //            }
        //            sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
        //            _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, inputDto, subject);
        //        }

        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        resultDto.IsSuccess = false;
        //        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
        //        resultDto.ErrorDto.Response = sapDataSyncResultDto;
        //        resultDto.ErrorDto.ErrorCode = Constants.Exception;
        //        resultDto.ErrorDto.Message = string.Concat(Constants.ServiceErrorMessage, message);
        //        _logger.Error(message);
        //        _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, inputDto, subject);
        //    }
        //}
        #endregion

        #region TradeTicket 

        public void CreateTradeTicket(TradeTicketListDto tradeTicketListDto)
        {
            _methodName = "CreateTradeTicket";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(tradeTicketListDto)}");
            var resultDto = new ResultDto();
            // var errorList = new List<LiftingRequestInquiryNumberDto>();
            var errorRecordList = new List<ErrorHANATradeTicketViewDto>();
            var errorMessageList = new List<string>();
            var dataSynced = 0;
            var inputdto = new SAPDataResponseDto();
            var syncType = ConsoleSettings.TradeTicketFolder;
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var subject = string.Concat(ConsoleSettings.TradeTicketCreateSubject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            var userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = tradeTicketListDto.TradeTicketList.Count;
            try
            {
                if (tradeTicketListDto != null && tradeTicketListDto.TradeTicketList != null && tradeTicketListDto.TradeTicketList.Any())
                {

                    List<ErrorHANATradeTicketViewDto> inputList = tradeTicketListDto.TradeTicketList.Select(a => new ErrorHANATradeTicketViewDto { BookingType = a.BookingType, ContractDate = a.ContractDate, ContractQuantity = a.ContractQuantity, ContractType = a.ContractType, MaterialType = a.MaterialType, PlantOrVendor = a.PlantOrVendor, TotalCost = a.TotalCost, TotalOilCost = a.TotalOilCost, TotalProcessCost = a.TotalProcessCost, TradeTicketNumber = a.TradeTicketNumber, UnitOfMeasure = a.UnitOfMeasure, ValidFrom = a.ValidFrom, ValidTo = a.ValidTo, Vertical = a.Vertical, OtherElement = a.OtherElement, TTStatus = a.TTStatus }).ToList();
                    using (var _emamiContext = new AdaniContext())
                    {
                        #region Get Common Datas                       
                        var depotList = _emamiContext.Depots.AsNoTracking();
                        var contractTypeList = _emamiContext.ContractTypes.AsNoTracking();
                        //var materialTypeList = _emamiContext.MaterialTypes.AsNoTracking();
                        var bookingTypeList = _emamiContext.BookingTypes.AsNoTracking();
                        var unitOfMeasureList = _emamiContext.Uom.AsNoTracking();
                        var verticalsData = _emamiContext.Divisions.AsNoTracking();
                        #endregion
                        // var TradeTicketListForEmail = new List<TradeTicket>();
                        foreach (var tradeTicket in inputList)
                        {
                            var errorFlag = true;
                            var errorMessage = string.Empty;
                            if (tradeTicket == null)
                            {
                                errorMessage = Constants.InvalidRequest;
                                errorFlag = false;
                            }
                            if (string.IsNullOrEmpty(tradeTicket.TradeTicketNumber))
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.TradeTicketNumberIsEmpty, errorMessage);
                                errorFlag = false;
                            }
                            var depotsContext = depotList.FirstOrDefault(_ => _.Code == tradeTicket.PlantOrVendor);
                            if (depotsContext == null)
                            {
                                if (string.IsNullOrEmpty(tradeTicket.TradeTicketNumber))
                                {
                                    errorMessage = Constants.BindErrorMessage(string.Format(Constants.DepotCodeSaudaIsEmpty, tradeTicket.PlantOrVendor), errorMessage);
                                    errorFlag = false;
                                }
                                else
                                {
                                    errorMessage = Constants.BindErrorMessage(string.Format(Constants.DepotCodeSaudaIsEmpty + Constants.ForSpecificTT + tradeTicket.TradeTicketNumber, tradeTicket.PlantOrVendor), errorMessage);
                                    errorFlag = false;
                                }
                            }

                            var contractTypeContext = contractTypeList.FirstOrDefault(_ => _.Name == tradeTicket.ContractType);
                            if (contractTypeContext == null)
                            {
                                if (string.IsNullOrEmpty(tradeTicket.TradeTicketNumber))
                                {
                                    errorMessage = Constants.BindErrorMessage(string.Format(Constants.MaterialTypesIsEmpty, tradeTicket.ContractType), errorMessage);
                                    errorFlag = false;
                                }
                                else
                                {
                                    errorMessage = Constants.BindErrorMessage(string.Format(Constants.MaterialTypesIsEmpty + Constants.ForSpecificTT + tradeTicket.TradeTicketNumber, tradeTicket.ContractType), errorMessage);
                                    errorFlag = false;
                                }
                            }
                            if (string.IsNullOrEmpty(tradeTicket.MaterialType))
                            {
                                if (string.IsNullOrEmpty(tradeTicket.TradeTicketNumber))
                                {
                                    errorMessage = Constants.BindErrorMessage(Constants.MaterialTypesIsEmpty, errorMessage);
                                    errorFlag = false;
                                }
                                else
                                {
                                    errorMessage = Constants.BindErrorMessage(Constants.MaterialTypesIsEmpty + Constants.ForSpecificTT + tradeTicket.TradeTicketNumber, errorMessage);
                                    errorFlag = false;
                                }
                            }
                            //var materialTypeContext = materialTypeList.FirstOrDefault(_ => _.Name == tradeTicket.MaterialType && _.IsActive);
                            //if (materialTypeContext == null)
                            //{
                            //    var materialTypeDto = new MaterialType
                            //    {
                            //        Name = tradeTicket.MaterialType,
                            //        IsActive = true
                            //    };
                            //    _emamiContext.MaterialTypes.Add(materialTypeDto);
                            //    _emamiContext.SaveChanges();
                            //    materialTypeContext = materialTypeDto;
                            //}
                            if (string.IsNullOrEmpty(tradeTicket.BookingType))
                            {
                                if (string.IsNullOrEmpty(tradeTicket.TradeTicketNumber))
                                {
                                    errorMessage = Constants.BindErrorMessage(Constants.BookingTypeIsEmpty, errorMessage);
                                    errorFlag = false;
                                }
                                else
                                {
                                    errorMessage = Constants.BindErrorMessage(Constants.BookingTypeIsEmpty + Constants.ForSpecificTT + tradeTicket.TradeTicketNumber, errorMessage);
                                    errorFlag = false;
                                }
                            }
                            var bookingTypeContext = bookingTypeList.FirstOrDefault(_ => _.Name == tradeTicket.BookingType);
                            if (bookingTypeContext == null)
                            {
                                var bookingTypeDto = new BookingType
                                {
                                    Name = tradeTicket.BookingType,
                                    IsActive = true
                                };
                                _emamiContext.BookingTypes.Add(bookingTypeDto);
                                _emamiContext.SaveChanges();
                                bookingTypeContext = bookingTypeDto;
                            }
                            var unitOfMeasureContext = unitOfMeasureList.FirstOrDefault(_ => _.SAPName == tradeTicket.UnitOfMeasure);
                            if (unitOfMeasureContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.UOMCodeIsEmpty, tradeTicket.UnitOfMeasure), errorMessage);
                                errorFlag = false;
                            }
                            var veticalContext = verticalsData.FirstOrDefault(_ => _.Code == tradeTicket.Vertical);
                            if (veticalContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.VerticalIsEmpty, tradeTicket.Vertical), errorMessage);
                                errorFlag = false;
                            }
                            if (errorFlag)
                            {
                                var tradeTicketContext = _emamiContext.TradeTicket.FirstOrDefault(_ => _.TradeTicketNumber == tradeTicket.TradeTicketNumber);
                                var verticalId = veticalContext.Id;
                                decimal OilTypeCost = 0;
                                decimal ProcessCost = 0;

                                var tradeTicketId = (long)0;
                                if (tradeTicketContext == null)
                                {
                                    var tradeTicketViewDto = new TradeTicket
                                    {
                                        ContractQuantity = tradeTicket.ContractQuantity,
                                        ContractDate = tradeTicket.ContractDate,
                                        DepotId = depotsContext.Id,
                                        OtherElement = tradeTicket.OtherElement,
                                        TradeTicketNumber = tradeTicket.TradeTicketNumber,
                                        ValidFrom = tradeTicket.ValidFrom,
                                        ValidTo = tradeTicket.ValidTo,
                                        BookingTypeId = bookingTypeContext.Id,
                                        //MaterialTypeId = materialTypeContext.Id,
                                        ContractTypeId = Convert.ToInt32(contractTypeContext.Id),
                                        UomId = unitOfMeasureContext.Id,
                                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                        CreatedBy = userId,
                                        IsSAPDataSync = true,
                                        DivisionId = verticalId,
                                        TTStatus = tradeTicket.TTStatus
                                    };
                                    _emamiContext.TradeTicket.Add(tradeTicketViewDto);
                                    _emamiContext.SaveChanges();
                                    tradeTicketId = tradeTicketViewDto.Id;
                                    //TradeTicketListForEmail.Add(tradeTicketViewDto);
                                }
                                else
                                {
                                    tradeTicketContext.ContractQuantity = tradeTicket.ContractQuantity;
                                    tradeTicketContext.ContractDate = tradeTicket.ContractDate;
                                    tradeTicketContext.DepotId = depotsContext.Id;
                                    tradeTicketContext.OtherElement = tradeTicket.OtherElement;
                                    tradeTicketContext.ValidFrom = tradeTicket.ValidFrom;
                                    tradeTicketContext.ValidTo = tradeTicket.ValidTo;
                                    tradeTicketContext.BookingTypeId = bookingTypeContext.Id;
                                    //tradeTicketContext.MaterialTypeId = materialTypeContext.Id;
                                    tradeTicketContext.ContractTypeId = Convert.ToInt32(contractTypeContext.Id);
                                    tradeTicketContext.UomId = unitOfMeasureContext.Id;
                                    tradeTicketContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    tradeTicketContext.ModifiedBy = userId;
                                    tradeTicketContext.TTStatus = tradeTicket.TTStatus;
                                    _emamiContext.SaveChanges();
                                    tradeTicketId = tradeTicketContext.Id;
                                    // TradeTicketListForEmail.Add(tradeTicketContext);
                                }
                                //var tradeTicketDetailsList = _emamiContext.TradeTicketDetails.Where(_ => _.TradeTicketId == tradeTicketId).ToList();
                                //var tradeTicketDetailsCount = 1;
                                //foreach (var item in tradeTicket.TradeTicketDetail)
                                //{
                                //    errorMessage = TradeTicketDetails(item.PRICE, item.MATERIAL_TYPE, tradeTicketId, item.PRCOST, item.PROPORTION, errorMessage, userId, tradeTicketDetailsCount, tradeTicketDetailsList, verticalId);
                                //    //var oilCostPercentage = item.PRICE * (item.PROPORTION) / 100;
                                //    //OilTypeCost = OilTypeCost + oilCostPercentage;
                                //    //if (verticalId == (int)Emami.Solution.DTO.Enums.Vertical.Hbc)
                                //    //{
                                //    //    ProcessCost = ProcessCost + item.PRCOST;
                                //    //}
                                //    //else
                                //    //{
                                //    //    ProcessCost = ProcessCost + (item.PRCOST * (item.PROPORTION) / 100);
                                //    //}

                                //    //tradeTicketDetailsCount++;
                                //}
                                //var tradeTicketDetailsExists = _emamiContext.TradeTicketDetails.Where(_ => _.TradeTicketId == tradeTicketId).ToList();
                                //if (tradeTicketDetailsExists.IsAny())
                                //{
                                var tradeTicketsContext = _emamiContext.TradeTicket.FirstOrDefault(_ => _.Id == tradeTicketId);
                                if (tradeTicketsContext != null)
                                {
                                    tradeTicketsContext.TotalCost = tradeTicket.TotalCost;
                                    tradeTicketsContext.TotalOilCost = tradeTicket.TotalOilCost;
                                    tradeTicketsContext.TotalProcessCost = tradeTicket.TotalProcessCost;
                                    _emamiContext.SaveChanges();
                                }
                                dataSynced++;
                                //}
                                //else
                                //{
                                //    var tradeTicketParent = _emamiContext.TradeTicket.FirstOrDefault(_ => _.Id == tradeTicketId);
                                //    _emamiContext.TradeTicket.Remove(tradeTicketParent);
                                //    _emamiContext.SaveChanges();
                                //}
                            }

                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                errorMessageList.Add(errorMessage);
                                errorRecordList.Add(tradeTicket);
                            }

                        }

                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = inputList;
                        sapDataSyncResultDto.SuccessRecordDetailsResponse = inputList.Except(errorRecordList).ToList();
                    }

                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    }
                    else
                    {
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncType, null, subject);
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = tradeTicketListDto;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncType, null, subject);
            }
        }

        public string TradeTicketDetails(decimal oilCost, string tradeTicketOilType, long tradeTicketId, decimal processCost, decimal proportionValues, string errorMessage, long userId, int oilTypeNumber, List<TradeTicketDetails> tradeTicketDetailsList, long verticalId)
        {
            var errorFlagDetails = true;
            using (var _emamiContext = new AdaniContext())
            {
                var tradeTicketOilTypesContext = _emamiContext.TradeTicketOilTypes.AsNoTracking().FirstOrDefault(_ => _.SAPId == tradeTicketOilType && _.IsActive && _.DivisionId == verticalId);
                if (tradeTicketOilTypesContext == null && !string.IsNullOrEmpty(tradeTicketOilType))
                {
                    errorMessage = Constants.BindErrorMessage(string.Format(Constants.OilTypeCodeIsEmpty, tradeTicketOilType), errorMessage);
                    errorFlagDetails = false;
                }
                if (errorFlagDetails && !string.IsNullOrEmpty(tradeTicketOilType))
                {
                    if (tradeTicketOilTypesContext != null && tradeTicketOilTypesContext.Id != 0)
                    {

                        if (tradeTicketDetailsList == null)
                        {
                            var tradeTicketDetailsDto = new TradeTicketDetails
                            {
                                TradeTicketId = tradeTicketId,
                                OilCost = oilCost,
                                TradeTicketOilTypeId = tradeTicketOilTypesContext.Id,
                                ProcessCost = processCost,
                                Proportion = proportionValues,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                CreatedBy = userId
                            };
                            _emamiContext.TradeTicketDetails.Add(tradeTicketDetailsDto);
                        }
                        else
                        {
                            if (tradeTicketDetailsList.Count >= oilTypeNumber)
                            {
                                var tradeTiketDetail = tradeTicketDetailsList[oilTypeNumber - 1];
                                var tradeTicketDetailsContext = _emamiContext.TradeTicketDetails.FirstOrDefault(_ => _.Id == tradeTiketDetail.Id);
                                if (tradeTicketDetailsContext.TradeTicketOilTypeId == tradeTicketOilTypesContext.Id)
                                {
                                    tradeTicketDetailsContext.OilCost = oilCost;
                                    tradeTicketDetailsContext.TradeTicketOilTypeId = tradeTicketOilTypesContext.Id;
                                    tradeTicketDetailsContext.ProcessCost = processCost;
                                    tradeTicketDetailsContext.Proportion = proportionValues;
                                    tradeTicketDetailsContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    tradeTicketDetailsContext.ModifiedBy = userId;
                                }
                            }
                            else
                            {
                                var tradeTicketDetailsDto = new TradeTicketDetails
                                {
                                    TradeTicketId = tradeTicketId,
                                    OilCost = oilCost,
                                    TradeTicketOilTypeId = tradeTicketOilTypesContext.Id,
                                    ProcessCost = processCost,
                                    Proportion = proportionValues,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    CreatedBy = userId
                                };
                                _emamiContext.TradeTicketDetails.Add(tradeTicketDetailsDto);
                            }


                        }
                    }
                    _emamiContext.SaveChanges();
                }
            }
            return errorMessage;
        }
        /// <summary>
        /// Method to get trade ticket details
        /// </summary>       
        /// <returns></returns>
        public void GetTradeTicketDetails()
        {
            _methodName = "GetTradeTicketDetails";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var tradeTicketViewDtoList = new List<SAPTradeTicketViewDto>();
            var syncFolder = ConsoleSettings.TradeTicketFolder;
            var subject = ConsoleSettings.TradeTicketSubject;
            var csvFileName = ConsoleSettings.TradeTicketNewCsv;
            try
            {
                using (var _emamiContext = new AdaniContext())
                {
                    #region Get Common Datas
                    var tradeTicketContext = _emamiContext.TradeTicket.AsNoTracking().Where(_ => _.IsSAPDataSync == false && _.DivisionId == (int)Adani.Solution.DTO.Enums.Division.Hbc).ToList();
                    var tradeTicketDetailsList = _emamiContext.TradeTicketDetails.AsNoTracking();
                    var depotList = _emamiContext.Depots.AsNoTracking();
                    var contractTypeList = _emamiContext.ContractTypes.AsNoTracking();
                    //var materialTypeList = _emamiContext.MaterialTypes.AsNoTracking();
                    var bookingTypeList = _emamiContext.BookingTypes.AsNoTracking();
                    var unitOfMeasureList = _emamiContext.Uom.AsNoTracking();
                    #endregion

                    if (tradeTicketContext != null && tradeTicketContext.Any())
                    {
                        foreach (var tradeTicket in tradeTicketContext)
                        {
                            var depotContext = depotList.FirstOrDefault(_ => _.Id == tradeTicket.DepotId);
                            var tradeTicketViewDto = new SAPTradeTicketViewDto
                            {
                                Id = tradeTicket.Id,
                                ContractQuantity = Math.Round(tradeTicket.ContractQuantity, 2),
                                ContractDate = tradeTicket.ContractDate,
                                PlantOrVendor = depotContext != null ? depotContext.Code : string.Empty,
                                OtherElement = tradeTicket.OtherElement,
                                TradeTicketNumber = tradeTicket.TradeTicketNumber,
                                ValidFrom = tradeTicket.ValidFrom,
                                ValidTo = tradeTicket.ValidTo,
                                IsModified = string.IsNullOrEmpty(tradeTicket.TradeTicketNumber) ? false : true
                            };
                            var tradeTicketDetailsContext = tradeTicketDetailsList.Where(_ => _.TradeTicketId == tradeTicket.Id).ToList();
                            if (tradeTicketDetailsContext != null)
                            {
                                foreach (var tradeTicketDetails in tradeTicketDetailsContext)
                                {

                                    var tradeTicketDetailsDto = new SAPTradeTicketDetailsDto
                                    {
                                        TradeTicketId = tradeTicketDetails.TradeTicketId,
                                        TradeTicketDetailsId = tradeTicketDetails.Id,
                                        //OilCost = Math.Round(tradeTicketDetails.OilCost, 2),
                                        //OilTypeId = tradeTicketDetails.TradeTicketOilTypeId,
                                        //OilType = tradeTicketDetails.TradeTicketOilType.SAPId,
                                        //ProcessCost = Math.Round(tradeTicket.TotalProcessCost, 2),
                                        //Proportion = Convert.ToString(tradeTicketDetails.Proportion),
                                    };
                                    tradeTicketViewDto.TradeTicketDetail.Add(tradeTicketDetailsDto);
                                }
                            }
                            var contractTypeContext = contractTypeList.FirstOrDefault(_ => _.Id == tradeTicket.ContractTypeId);
                            if (contractTypeContext != null)
                            {
                                tradeTicketViewDto.ContractType = contractTypeContext.Name;
                            }
                            //var materialTypeContext = materialTypeList.FirstOrDefault(_ => _.Id == tradeTicket.MaterialTypeId && _.IsActive);
                            //if (materialTypeContext != null)
                            //{
                            //    tradeTicketViewDto.MaterialType = materialTypeContext.Name;
                            //}
                            var bookingTypeContext = bookingTypeList.FirstOrDefault(_ => _.Id == tradeTicket.BookingTypeId);
                            if (bookingTypeContext != null)
                            {
                                tradeTicketViewDto.BookingType = bookingTypeContext.Name;
                            }
                            var unitOfMeasureContext = unitOfMeasureList.FirstOrDefault(_ => _.Id == tradeTicket.UomId);
                            if (unitOfMeasureContext != null)
                            {
                                tradeTicketViewDto.UnitOfMeasure = unitOfMeasureContext.SAPName;
                            }
                            tradeTicketViewDtoList.Add(tradeTicketViewDto);
                            var sqlUpdate = "UPDATE TradeTickets SET IsSAPDataSync = @IsSAPDataSync WHERE Id = @Id";
                            var parameters = new[]{
                                    new SqlParameter("@IsSAPDataSync", true),
                                    new SqlParameter("@Id", tradeTicket.Id)
                        };
                            _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                        }
                        _emamiContext.SaveChanges();
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = tradeTicketViewDtoList;
                        _sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
                    }
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = string.Concat(Constants.ServiceErrorMessage, message);
                var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _logger.Error(message);
                _sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
            }
        }

        public void UpdateTradeTicketNumber(string decryptedString)
        {
            _methodName = "UpdateTradeTicketNumber";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var customerSyncData = string.Empty;
            var messageSync = string.Empty;
            var sapDataSyncResultDto = new SapDataSyncResultDto();
            var errorList = new List<TradeTicketNumberDto>();
            sapDataSyncResultDto.SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
            var dataSynced = 0;
            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
            var userId = UtilityHelper.LongTryToParse(jarray[0]["loginUserId"].ToString());
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var syncFolder = jarray[0]["syncFolder"].ToString();
            var subject = jarray[0]["subject"].ToString();
            var folderPath = ConsoleSettings.InboundDirectoryPath(syncFolder);
            var inputDto = _sftpConnectorService.GetSFTPFile(folderPath, syncFolder);
            var tradeTicketViewDtoList = !string.IsNullOrEmpty(inputDto.Response.ToString()) ? (List<TradeTicketNumberDto>)inputDto.Response : new List<TradeTicketNumberDto>();
            subject = string.Concat(subject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = tradeTicketViewDtoList.Count;
            try
            {

                if (tradeTicketViewDtoList != null && tradeTicketViewDtoList.Any())
                {
                    var errorMessageList = new List<string>();
                    using (var _emamiContext = new AdaniContext())
                    {
                        foreach (var tradeTicket in tradeTicketViewDtoList)
                        {
                            var errorFlag = true;
                            var errorMessage = string.Empty;
                            if (tradeTicket == null)
                            {
                                errorMessage = Constants.InvalidRequest;
                                errorFlag = false;
                            }
                            if (string.IsNullOrEmpty(tradeTicket.TradeTicketNumber))
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.TradeTicketNumberIsEmpty + " App_Id: " + tradeTicket.Id, errorMessage);
                                errorFlag = false;
                            }

                            if (errorFlag)
                            {
                                var sqlUpdate = "UPDATE TradeTickets SET TradeTicketNumber = @TradeTicketNumber ,ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy WHERE Id = @Id";
                                var parameters = new[]{
                                    new SqlParameter("@TradeTicketNumber", tradeTicket.TradeTicketNumber),
                                    new SqlParameter("@Id", tradeTicket.Id),
                                    new SqlParameter("@ModifiedDate", currentDate),
                                    new SqlParameter("@ModifiedBy", userId),
                            };
                                _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                dataSynced++;
                            }
                            else
                            {
                                errorMessageList.Add(errorMessage);
                                errorList.Add(tradeTicket);
                            }

                        }
                        _emamiContext.SaveChanges();
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorList;
                    }

                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    }
                    else
                    {
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, inputDto, subject);
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = string.Concat(Constants.ServiceErrorMessage, message);
                _logger.Error(message);
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, inputDto, subject);
            }
        }
        #endregion

        #region Trade Ticket Speciality Fat
        /// <summary>
        /// Method to get trade ticket details
        /// </summary>       
        /// <returns></returns>
        public void GetSpecialityFatTradeTicketDetails()
        {
            _methodName = "GetSpecialityFatTradeTicketDetails";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var tradeTicketViewDtoList = new List<SAPTradeTicketViewDto>();
            var syncFolder = ConsoleSettings.SpecialityFatTradeTicketFolder;
            var subject = ConsoleSettings.SpecialityFatTradeTicketSubject;
            var csvFileName = ConsoleSettings.SpecialityFatTradeTicketNewCsv;
            try
            {
                using (var _emamiContext = new AdaniContext())
                {
                    #region Get Common Datas
                    var tradeTicketContext = _emamiContext.TradeTicket.AsNoTracking().Where(_ => _.IsSAPDataSync == false && _.DivisionId == (int)Adani.Solution.DTO.Enums.Division.SpecialityFat).ToList();
                    var tradeTicketDetailsList = _emamiContext.TradeTicketDetails.AsNoTracking();
                    var depotList = _emamiContext.Depots.AsNoTracking();
                    var contractTypeList = _emamiContext.ContractTypes.AsNoTracking();
                    //var materialTypeList = _emamiContext.MaterialTypes.AsNoTracking();
                    var bookingTypeList = _emamiContext.BookingTypes.AsNoTracking();
                    var unitOfMeasureList = _emamiContext.Uom.AsNoTracking();
                    #endregion

                    if (tradeTicketContext != null && tradeTicketContext.Any())
                    {
                        foreach (var tradeTicket in tradeTicketContext)
                        {
                            var depotContext = depotList.FirstOrDefault(_ => _.Id == tradeTicket.DepotId);
                            var tradeTicketViewDto = new SAPTradeTicketViewDto
                            {
                                Id = tradeTicket.Id,
                                ContractQuantity = tradeTicket.ContractQuantity,
                                ContractDate = tradeTicket.ContractDate,
                                PlantOrVendor = depotContext != null ? depotContext.Code : string.Empty,
                                OtherElement = tradeTicket.OtherElement,
                                TradeTicketNumber = tradeTicket.TradeTicketNumber,
                                ValidFrom = tradeTicket.ValidFrom,
                                ValidTo = tradeTicket.ValidTo,
                                IsModified = string.IsNullOrEmpty(tradeTicket.TradeTicketNumber) ? false : true
                            };
                            var tradeTicketDetailsContext = tradeTicketDetailsList.Where(_ => _.TradeTicketId == tradeTicket.Id).ToList();
                            if (tradeTicketDetailsContext != null)
                            {
                                foreach (var tradeTicketDetails in tradeTicketDetailsContext)
                                {

                                    var tradeTicketDetailsDto = new SAPTradeTicketDetailsDto
                                    {
                                        TradeTicketId = tradeTicketDetails.TradeTicketId,
                                        TradeTicketDetailsId = tradeTicketDetails.Id,
                                        //OilCost = Math.Round(tradeTicketDetails.OilCost, 2),
                                        //OilTypeId = tradeTicketDetails.TradeTicketOilTypeId,
                                        //OilType = tradeTicketDetails.TradeTicketOilType.SAPId,
                                        //ProcessCost = Math.Round(tradeTicket.TotalProcessCost, 2),
                                        //Proportion = Convert.ToString(tradeTicketDetails.Proportion),
                                    };
                                    tradeTicketViewDto.TradeTicketDetail.Add(tradeTicketDetailsDto);
                                }
                            }
                            var contractTypeContext = contractTypeList.FirstOrDefault(_ => _.Id == tradeTicket.ContractTypeId);
                            if (contractTypeContext != null)
                            {
                                tradeTicketViewDto.ContractType = contractTypeContext.Name;
                            }
                            //var materialTypeContext = materialTypeList.FirstOrDefault(_ => _.Id == tradeTicket.MaterialTypeId);
                            //if (materialTypeContext != null)
                            //{
                            //    tradeTicketViewDto.MaterialType = materialTypeContext.Name;
                            //}
                            var bookingTypeContext = bookingTypeList.FirstOrDefault(_ => _.Id == tradeTicket.BookingTypeId);
                            if (bookingTypeContext != null)
                            {
                                tradeTicketViewDto.BookingType = bookingTypeContext.Name;
                            }
                            var unitOfMeasureContext = unitOfMeasureList.FirstOrDefault(_ => _.Id == tradeTicket.UomId);
                            if (unitOfMeasureContext != null)
                            {
                                tradeTicketViewDto.UnitOfMeasure = unitOfMeasureContext.SAPName;
                            }
                            tradeTicketViewDtoList.Add(tradeTicketViewDto);
                            var sqlUpdate = "UPDATE TradeTickets SET IsSAPDataSync = @IsSAPDataSync WHERE Id = @Id";
                            var parameters = new[]{
                                    new SqlParameter("@IsSAPDataSync", true),
                                    new SqlParameter("@Id", tradeTicket.Id)
                        };
                            _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                        }
                        _emamiContext.SaveChanges();
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = tradeTicketViewDtoList;
                        _sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
                    }
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = string.Concat(Constants.ServiceErrorMessage, message);
                var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _logger.Error(message);
                _sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
            }
        }

        public void CreateTradeTicketSF(string decryptedString)
        {
            _methodName = "CreateTradeTicketSF";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var customerSyncData = string.Empty;
            var messageSync = string.Empty;
            var sapDataSyncResultDto = new SapDataSyncResultDto();
            var errorRecordList = new List<SAPTradeTicketViewDto>();
            sapDataSyncResultDto.SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
            var dataSynced = 0;
            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
            var userId = UtilityHelper.LongTryToParse(jarray[0]["loginUserId"].ToString());
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var syncFolder = jarray[0]["syncFolder"].ToString();
            var subject = jarray[0]["subject"].ToString();
            var folderPath = ConsoleSettings.InboundDirectoryPath(syncFolder);
            var inputDto = _sftpConnectorService.GetSFTPFile(folderPath, syncFolder);
            var tradeTicketViewDtoList = !string.IsNullOrEmpty(inputDto.Response.ToString()) ? (List<SAPTradeTicketViewDto>)inputDto.Response : new List<SAPTradeTicketViewDto>();
            subject = string.Concat(subject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = tradeTicketViewDtoList.Count;
            try
            {
                if (tradeTicketViewDtoList != null && tradeTicketViewDtoList.Any())
                {
                    var errorMessageList = new List<string>();
                    using (var _emamiContext = new AdaniContext())
                    {
                        #region Get Common Datas                       
                        var depotList = _emamiContext.Depots.AsNoTracking();
                        var contractTypeList = _emamiContext.ContractTypes.AsNoTracking();
                        //var materialTypeList = _emamiContext.MaterialTypes.AsNoTracking();
                        var bookingTypeList = _emamiContext.BookingTypes.AsNoTracking();
                        var unitOfMeasureList = _emamiContext.Uom.AsNoTracking();
                        #endregion
                        foreach (var tradeTicket in tradeTicketViewDtoList)
                        {
                            var errorFlag = true;
                            var errorMessage = string.Empty;
                            if (tradeTicket == null)
                            {
                                errorMessage = Constants.InvalidRequest;
                                errorFlag = false;
                            }
                            if (string.IsNullOrEmpty(tradeTicket.TradeTicketNumber))
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.TradeTicketNumberIsEmpty, errorMessage);
                                errorFlag = false;
                            }
                            var depotsContext = depotList.FirstOrDefault(_ => _.Code == tradeTicket.PlantOrVendor);
                            if (depotsContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.DepotCodeSaudaIsEmpty, tradeTicket.PlantOrVendor), errorMessage);
                                errorFlag = false;
                            }

                            var contractTypeContext = contractTypeList.FirstOrDefault(_ => _.Name == tradeTicket.ContractType);
                            if (contractTypeContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.MaterialTypesIsEmpty, tradeTicket.ContractType), errorMessage);
                                errorFlag = false;
                            }
                            //var materialTypeContext = materialTypeList.FirstOrDefault(_ => _.Name == tradeTicket.MaterialType);
                            //if (materialTypeContext == null)
                            //{
                            //    var materialTypeDto = new MaterialType
                            //    {
                            //        Name = tradeTicket.MaterialType,
                            //        IsActive = true
                            //    };
                            //    _emamiContext.MaterialTypes.Add(materialTypeDto);
                            //    _emamiContext.SaveChanges();
                            //    materialTypeContext = materialTypeDto;
                            //}
                            var bookingTypeContext = bookingTypeList.FirstOrDefault(_ => _.Name == tradeTicket.BookingType);
                            if (bookingTypeContext == null)
                            {
                                var bookingTypeDto = new BookingType
                                {
                                    Name = tradeTicket.MaterialType,
                                    IsActive = true
                                };
                                _emamiContext.BookingTypes.Add(bookingTypeDto);
                                _emamiContext.SaveChanges();
                                bookingTypeContext = bookingTypeDto;
                            }
                            var unitOfMeasureContext = unitOfMeasureList.FirstOrDefault(_ => _.SAPName == tradeTicket.UnitOfMeasure);
                            if (unitOfMeasureContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.DepotCodeSaudaIsEmpty, tradeTicket.PlantOrVendor), errorMessage);
                                errorFlag = false;
                            }
                            if (errorFlag)
                            {
                                //var tradeTicketContext = _emamiContext.TradeTicket.FirstOrDefault(_ => _.TradeTicketNumber == tradeTicket.TradeTicketNumber);

                                //var oilCostPercentage1 = tradeTicket.PRICE1 * (tradeTicket.PROPORTION1) / 100;
                                //var oilCostPercentage2 = tradeTicket.PRICE2 * (tradeTicket.PROPORTION2) / 100;
                                //var oilCostPercentage3 = tradeTicket.PRICE3 * (tradeTicket.PROPORTION3) / 100;
                                //var oilCostPercentage4 = tradeTicket.PRICE4 * (tradeTicket.PROPORTION4) / 100;
                                //var oilCostPercentage5 = tradeTicket.PRICE5 * (tradeTicket.PROPORTION5) / 100;
                                //var oilCostPercentage6 = tradeTicket.PRICE6 * (tradeTicket.PROPORTION6) / 100;
                                //var oilCostPercentage7 = tradeTicket.PRICE7 * (tradeTicket.PROPORTION7) / 100;
                                //var oilCostPercentage8 = tradeTicket.PRICE8 * (tradeTicket.PROPORTION8) / 100;
                                //var oilCostPercentage9 = tradeTicket.PRICE9 * (tradeTicket.PROPORTION9) / 100;
                                //var oilCostPercentage10 = tradeTicket.PRICE10 * (tradeTicket.PROPORTION10) / 100;

                                //var OilTypeCost = oilCostPercentage1 + oilCostPercentage2 + oilCostPercentage3 + oilCostPercentage4 + oilCostPercentage5 + oilCostPercentage6 + oilCostPercentage7 + oilCostPercentage8 + oilCostPercentage9 + oilCostPercentage10;

                                //var ProcessCost = (tradeTicket.PRCOST1 * (tradeTicket.PROPORTION1) / 100) + (tradeTicket.PRCOST2 * (tradeTicket.PROPORTION2) / 100) + (tradeTicket.PRCOST3 * (tradeTicket.PROPORTION3) / 100) +
                                //    (tradeTicket.PRCOST4 * (tradeTicket.PROPORTION4) / 100) + (tradeTicket.PRCOST5 * (tradeTicket.PROPORTION5) / 100) + (tradeTicket.PRCOST6 * (tradeTicket.PROPORTION6) / 100) +
                                //    (tradeTicket.PRCOST7 * (tradeTicket.PROPORTION7) / 100) + (tradeTicket.PRCOST8 * (tradeTicket.PROPORTION8) / 100) + (tradeTicket.PRCOST9 * (tradeTicket.PROPORTION9) / 100) +
                                //    (tradeTicket.PRCOST10 * (tradeTicket.PROPORTION10) / 100);

                                //var tradeTicketId = (long)0;
                                //if (tradeTicketContext == null)
                                //{
                                //    var tradeTicketViewDto = new TradeTicket
                                //    {
                                //        ContractQuantity = tradeTicket.ContractQuantity,
                                //        ContractDate = tradeTicket.ContractDate,
                                //        DepotId = depotsContext.Id,
                                //        OtherElement = tradeTicket.OtherElement,
                                //        TradeTicketNumber = tradeTicket.TradeTicketNumber,
                                //        ValidFrom = tradeTicket.ValidFrom,
                                //        ValidTo = tradeTicket.ValidTo,
                                //        BookingTypeId = bookingTypeContext.Id,
                                //        MaterialTypeId = materialTypeContext.Id,
                                //        ContractTypeId = Convert.ToInt32(contractTypeContext.Id),
                                //        UomId = unitOfMeasureContext.Id,
                                //        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                //        CreatedBy = userId,
                                //        IsSAPDataSync = true,
                                //        VerticalId = (int)Emami.Solution.DTO.Enums.Vertical.SpecialityFat,
                                //        TotalCost = OilTypeCost + ProcessCost,
                                //        TotalOilCost = OilTypeCost,
                                //        TotalProcessCost = ProcessCost
                                //    };
                                //    _emamiContext.TradeTicket.Add(tradeTicketViewDto);
                                //    _emamiContext.SaveChanges();
                                //    tradeTicketId = tradeTicketViewDto.Id;
                                //}
                                //else
                                //{
                                //    tradeTicketContext.ContractQuantity = tradeTicket.ContractQuantity;
                                //    tradeTicketContext.ContractDate = tradeTicket.ContractDate;
                                //    tradeTicketContext.DepotId = depotsContext.Id;
                                //    tradeTicketContext.OtherElement = tradeTicket.OtherElement;
                                //    tradeTicketContext.ValidFrom = tradeTicket.ValidFrom;
                                //    tradeTicketContext.ValidTo = tradeTicket.ValidTo;
                                //    tradeTicketContext.BookingTypeId = bookingTypeContext.Id;
                                //    tradeTicketContext.MaterialTypeId = materialTypeContext.Id;
                                //    tradeTicketContext.ContractTypeId = Convert.ToInt32(contractTypeContext.Id);
                                //    tradeTicketContext.UomId = unitOfMeasureContext.Id;
                                //    tradeTicketContext.ModifiedDate = currentDate;
                                //    tradeTicketContext.ModifiedBy = userId;
                                //    tradeTicketContext.TotalCost = OilTypeCost + ProcessCost;
                                //    tradeTicketContext.TotalOilCost = OilTypeCost;
                                //    tradeTicketContext.TotalProcessCost = ProcessCost;
                                //    _emamiContext.SaveChanges();
                                //    tradeTicketId = tradeTicketContext.Id;
                                //}
                                //var verticalId = (int)DTO.Enums.Vertical.SpecialityFat;
                                //var tradeTicketDetailsList = _emamiContext.TradeTicketDetails.Where(_ => _.TradeTicketId == tradeTicketId).ToList();
                                //errorMessage = TradeTicketDetails(tradeTicket.PRICE1, tradeTicket.MATERIAL_TYPE1, tradeTicketId, tradeTicket.PRCOST1, tradeTicket.PROPORTION1, errorMessage, userId, 1, tradeTicketDetailsList, verticalId);
                                //errorMessage = TradeTicketDetails(tradeTicket.PRICE2, tradeTicket.MATERIAL_TYPE2, tradeTicketId, tradeTicket.PRCOST2, tradeTicket.PROPORTION2, errorMessage, userId, 2, tradeTicketDetailsList, verticalId);
                                //errorMessage = TradeTicketDetails(tradeTicket.PRICE3, tradeTicket.MATERIAL_TYPE3, tradeTicketId, tradeTicket.PRCOST3, tradeTicket.PROPORTION3, errorMessage, userId, 3, tradeTicketDetailsList, verticalId);
                                //errorMessage = TradeTicketDetails(tradeTicket.PRICE4, tradeTicket.MATERIAL_TYPE4, tradeTicketId, tradeTicket.PRCOST4, tradeTicket.PROPORTION4, errorMessage, userId, 4, tradeTicketDetailsList, verticalId);
                                //errorMessage = TradeTicketDetails(tradeTicket.PRICE5, tradeTicket.MATERIAL_TYPE5, tradeTicketId, tradeTicket.PRCOST5, tradeTicket.PROPORTION5, errorMessage, userId, 5, tradeTicketDetailsList, verticalId);
                                //errorMessage = TradeTicketDetails(tradeTicket.PRICE6, tradeTicket.MATERIAL_TYPE6, tradeTicketId, tradeTicket.PRCOST6, tradeTicket.PROPORTION6, errorMessage, userId, 6, tradeTicketDetailsList, verticalId);
                                //errorMessage = TradeTicketDetails(tradeTicket.PRICE7, tradeTicket.MATERIAL_TYPE7, tradeTicketId, tradeTicket.PRCOST7, tradeTicket.PROPORTION7, errorMessage, userId, 7, tradeTicketDetailsList, verticalId);
                                //errorMessage = TradeTicketDetails(tradeTicket.PRICE8, tradeTicket.MATERIAL_TYPE8, tradeTicketId, tradeTicket.PRCOST8, tradeTicket.PROPORTION8, errorMessage, userId, 8, tradeTicketDetailsList, verticalId);
                                //errorMessage = TradeTicketDetails(tradeTicket.PRICE9, tradeTicket.MATERIAL_TYPE9, tradeTicketId, tradeTicket.PRCOST9, tradeTicket.PROPORTION9, errorMessage, userId, 9, tradeTicketDetailsList, verticalId);
                                //errorMessage = TradeTicketDetails(tradeTicket.PRICE10, tradeTicket.MATERIAL_TYPE10, tradeTicketId, tradeTicket.PRCOST10, tradeTicket.PROPORTION10, errorMessage, userId, 10, tradeTicketDetailsList, verticalId);
                                //dataSynced++;
                            }

                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                errorMessageList.Add(errorMessage);
                                errorRecordList.Add(tradeTicket);
                            }

                        }
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                    }

                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    }
                    else
                    {
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, inputDto, subject);
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = string.Concat(Constants.ServiceErrorMessage, message);
                _logger.Error(message);
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, inputDto, subject);
            }
        }
        #endregion     

        #region SaudaApproval

        /// <summary>
        /// Method to get sauda approval details
        /// </summary>       
        /// <returns></returns>
        public void GetSaudaApprovalDetails(List<long> SaudaOrderIds)
        {
            _methodName = "GetSaudaApprovalDetails";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var saudaStatusDtoList = new HANASaudaStatusDto();
            var syncFolder = ConsoleSettings.SaudaApproval;
            var subject = string.Concat(ConsoleSettings.SaudaApprovalSubject, " ", ConsoleSettings.AppToSapDataSyncEmailSubject);
            var csvFileName = ConsoleSettings.SaudaApprovalCsv;
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            try
            {
                using (var _emamiContext = new AdaniContext())
                {
                    var SaudaOrderContext = from saudaOrder in _emamiContext.SaudaOrders.AsNoTracking()
                                                //join saudaOrder in _emamiContext.SaudaOrders.AsNoTracking() on sauda.Id equals saudaOrder.SaudaId
                                            where saudaOrder.IsSAPDataSyncApproval == false && SaudaOrderIds.Contains(saudaOrder.Id) && ((saudaOrder.StatusId == (int)DTO.Enums.Status.Approved || saudaOrder.StatusId == (int)DTO.Enums.Status.Rejected) && !string.IsNullOrEmpty(saudaOrder.SaudaNumber))
                                            select saudaOrder;

                    //var SaudaContext = _emamiContext.Sauda.AsNoTracking().Where(_ => _.IsSAPDataSyncApproval == false && _.StatusId == (int)DTO.Enums.SaudaStatus.NotReleased || _.StatusId == (int)DTO.Enums.SaudaStatus.Released).ToList();
                    if (SaudaOrderContext != null && SaudaOrderContext.Any())
                    {
                        sapDataSyncResultDto.OutstandingResult.DataRetrieved = SaudaOrderContext.ToList().Count;
                        sapDataSyncResultDto.OutstandingResult.DataSynced = SaudaOrderContext.ToList().Count;
                        foreach (var saudaOrder in SaudaOrderContext.ToList())
                        {
                            var saudaStatusDto = new SaudaStatusDto
                            {
                                SaudaNumber = saudaOrder.SaudaNumber,
                                SaudaStatusId = saudaOrder.StatusId
                            };
                            saudaStatusDtoList.Header.Add(saudaStatusDto);
                            var sqlUpdate = "UPDATE SaudaOrders SET IsSAPDataSyncApproval = @IsSAPDataSyncApproval, ModifiedDate = @ModifiedDate WHERE Id = @Id";
                            var parameters = new[]{
                                    new SqlParameter("@IsSAPDataSyncApproval", true),
                                    new SqlParameter("@Id", saudaOrder.Id),
                                    new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow))
                        };
                            _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                        }
                        _emamiContext.SaveChanges();
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaStatusDtoList.Header;
                        if (saudaStatusDtoList.Header.IsAny())
                        {
                            var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.SaudaApprovalHanaApiUrl, saudaStatusDtoList);
                            var status = response.StatusCode;
                            if (status.ToString() == "Accepted")
                            {
                                resultDto.IsSuccess = true;
                                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                                resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                                resultDto.SuccessDto.Response = sapDataSyncResultDto;
                            }
                            else
                            {
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto.Message = "Sauda Approval data sent to SAP Failed" + status.ToString();
                                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                            }
                            _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, null, subject);
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaStatusDtoList.Header;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = string.Concat(Constants.ServiceErrorMessage, message);
                _logger.Error(message);
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
            }
        }

        public void SaudaApprovalConfirmation(List<HANASaudaCommonFunctionList> inputdto)
        {
            _methodName = "SaudaApprovalConfirmation";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputdto)}");
            var resultDto = new ResultDto();
            var errorRecordList = new List<HANASaudaCommonFunctionList>();
            var errorMessageList = new List<string>();
            var dataSynced = 0;
            var synctype = ConsoleSettings.SaudaApproval;
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var subject = string.Concat(ConsoleSettings.SaudaApprovalConfirmationSubject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            long userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var saudaViewDtoList = inputdto != null ? inputdto : new List<HANASaudaCommonFunctionList>();
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = saudaViewDtoList.Count;
            try
            {
                if (saudaViewDtoList != null && saudaViewDtoList.Any())
                {

                    using (var _emamiContext = new AdaniContext())
                    {
                        var saudaNumbers = saudaViewDtoList.Select(_ => _.SAP_Document_No).ToList();
                        var saudaList = _emamiContext.SaudaOrders.AsNoTracking().Where(sauda => saudaNumbers.Contains(sauda.SaudaNumber)).Select(_ => new SaudaApprovalConfirmationDto
                        {
                            AppId = _.Id,
                            SaudaNumber = _.SaudaNumber
                        }).ToList();
                        foreach (var sauda in saudaViewDtoList)
                        {
                            var errorFlag = true;
                            var errorMessage = string.Empty;
                            if (sauda == null)
                            {
                                errorMessage = Constants.InvalidRequest;
                                errorFlag = false;
                            }
                            if (string.IsNullOrEmpty(sauda.SAP_Document_No))
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.SaudaNumberIsEmpty + " App_Id: " + sauda.Impiger_Request_No, errorMessage);
                                errorFlag = false;
                            }

                            if (errorFlag)
                            {
                                var sqlUpdate = "UPDATE SaudaOrders SET Remarks = @Remarks,IsSaudaApprovalSyncConfirmation = @IsSaudaApprovalSyncConfirmation ,ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy , IsSaudaApprovalStatusFromSap =  @IsSaudaApprovalStatusFromSap WHERE SaudaNumber = @SaudaNumber";
                                var parameters = new[]{
                                    new SqlParameter("@Remarks", sauda.Message),
                                    new SqlParameter("@IsSaudaApprovalStatusFromSap", sauda.Status),
                                    new SqlParameter("@SaudaNumber", sauda.SAP_Document_No),
                                    new SqlParameter("@ModifiedDate", currentDate),
                                    new SqlParameter("@ModifiedBy", userId),
                                    new SqlParameter("@IsSaudaApprovalSyncConfirmation", true)
                                    };
                                _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                dataSynced++;

                                var AppId = saudaList.FirstOrDefault(_ => _.SaudaNumber == sauda.SAP_Document_No).AppId;

                                //var remark = new Remarks
                                //{
                                //    Description = sauda.Message,
                                //    TableName = "SaudaOrders",
                                //    TableId = AppId,
                                //    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                //    CreatedBy = userId,
                                //    IsActive = true
                                //};
                                //_emamiContext.Remarks.Add(remark);

                                if (!sauda.Status)
                                {
                                    errorRecordList.Add(sauda);
                                }
                            }
                            else
                            {
                                errorMessageList.Add(errorMessage);
                                errorRecordList.Add(sauda);
                            }
                        }
                        _emamiContext.SaveChanges();
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaViewDtoList;
                        sapDataSyncResultDto.SuccessRecordDetailsResponse = saudaViewDtoList.Except(errorRecordList).ToList();
                    }

                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    }
                    else
                    {
                        if (saudaViewDtoList.Select(a => a.Status).All(s => s))
                        {
                            resultDto.IsSuccess = true;
                            resultDto.SuccessDto.Response = sapDataSyncResultDto;
                            resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                        }
                        else
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Response = sapDataSyncResultDto;
                            resultDto.ErrorDto.Message = Constants.SapSyncSuccessMessage;
                        }
                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    //_sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaViewDtoList;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                //_sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }

        #endregion

        #region Sauda Amendment
        public void SaudaAmendment(HANASaudaAmendmentDtoList inputdto)
        {
            _methodName = "SaudaAmendment";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputdto)}");
            var resultDto = new ResultDto();
            var errorRecordList = new List<SAPSaudaAmendmentDto>();
            var errorMessageList = new List<string>();
            var dataSynced = 0;
            var synctype = ConsoleSettings.SaudaAmendmentSubject;
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var subject = string.Concat(ConsoleSettings.SaudaAmendmentSubject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            long userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var saudaViewDtoList = inputdto != null && inputdto.SAPSaudaAmendmentList != null ? inputdto.SAPSaudaAmendmentList : new List<SAPSaudaAmendmentDto>();
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = saudaViewDtoList.Count;
            try
            {
                if (saudaViewDtoList != null && saudaViewDtoList.Any())
                {
                    using (var _emamiContext = new AdaniContext())
                    {

                        #region Get Common Datas

                        var saudaNumber = saudaViewDtoList.Select(s => s.SaudaNumber).Distinct().ToList();
                        var saudaOrdersList = _emamiContext.SaudaOrders.AsNoTracking()
                            .Where(_ => saudaNumber.Contains(_.SaudaNumber))
                            .Select(s => new { SaudaNumber = s.SaudaNumber, SaudaId = s.SaudaId, PricingId = s.PricingId, BrokerId = s.BrokerId });

                        var verticalList = _emamiContext.Divisions.AsNoTracking();
                        //var soldToPartyList = _emamiContext.Users.AsNoTracking();
                        var soldToPartyList = (from s in _emamiContext.Users
                                               join role in _emamiContext.UserRoles on s.Id equals role.UserId
                                               where role.RoleId != (int)DTO.Enums.Role.ShipToParty
                                               select new { Id = s.Id, Code = s.Code/*, VerticalId = s.DivisionId*/ }).ToList();

                        //var shipToPartyList = _emamiContext.Users.AsNoTracking();
                        var shipToPartyList = (from s in _emamiContext.Users
                                               join role in _emamiContext.UserRoles on s.Id equals role.UserId
                                               where role.RoleId != (int)DTO.Enums.Role.ShipToParty
                                               select new { Id = s.Id, Code = s.Code/*, VerticalId = s.DivisionId*/ }).ToList();

                        var skuList = _emamiContext.Skus.AsNoTracking();
                        var depotList = _emamiContext.Depots.AsNoTracking();
                        var incoTermList = _emamiContext.IncoTerms.AsNoTracking();
                        var uomList = _emamiContext.Uom.AsNoTracking();
                        var dealerRoleList = _emamiContext.UserRoles.AsNoTracking();
                        var userCustomerMappingList = _emamiContext.UserCustomerMapping.AsNoTracking();
                        var depotsList = _emamiContext.Depots.AsNoTracking();
                        #endregion

                        foreach (var sauda in saudaViewDtoList)
                        {
                            var errorFlag = true;
                            var errorMessage = string.Empty;
                            if (sauda == null)
                            {
                                errorMessage = Constants.InvalidRequest;
                                errorFlag = false;
                            }
                            if (string.IsNullOrEmpty(sauda.SaudaNumber))
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.SaudaNumberIsEmpty, errorMessage);
                                errorFlag = false;
                            }
                            var saudaOrdersContext = saudaOrdersList.FirstOrDefault(_ => _.SaudaNumber == sauda.SaudaNumber);
                            if (saudaOrdersContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.SaudaNumberIsNotEmpty, sauda.SaudaNumber), errorMessage);
                                errorFlag = false;
                            }
                            var verticalContext = verticalList.FirstOrDefault(_ => _.Code == sauda.Vertical);
                            if (verticalContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.VerticalCodeIsEmpty, sauda.Vertical), errorMessage);
                                errorFlag = false;
                            }
                            var verticalId = verticalContext != null ? verticalContext.Id : 0;
                            var soldToPartyContext = soldToPartyList.FirstOrDefault(_ => _.Code == sauda.SoldToParty /*&& _.VerticalId == verticalId*/);
                            if (soldToPartyContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.SoldToPartyIsNotEmpty, sauda.SoldToParty), errorMessage);
                                errorFlag = false;
                            }
                            var shipToPartyContext = shipToPartyList.FirstOrDefault(_ => _.Code == sauda.ShipToParty /*&& _.VerticalId == verticalId*/);
                            if (shipToPartyContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.ShipToPartyIsNotEmpty, sauda.ShipToParty), errorMessage);
                                errorFlag = false;
                            }
                            var skuContext = skuList.FirstOrDefault(_ => _.SkuCode == sauda.SkuCode && _.DivisionId == verticalId);
                            if (skuContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.SkuDetailsIsEmpty, sauda.SkuCode), errorMessage);
                                errorFlag = false;
                            }
                            var depots = depotList.FirstOrDefault(_ => _.Code == sauda.DepotCode);
                            if (depots == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.DepotCodeSaudaIsEmpty, sauda.DepotCode), errorMessage);
                                errorFlag = false;
                            }
                            var incoTerms = incoTermList.FirstOrDefault(_ => _.SAPName == sauda.INCO1);
                            var uomContext = uomList.FirstOrDefault(_ => _.SAPName == sauda.Uom);

                            if (errorFlag)
                            {
                                long DealerTypeId = 0;
                                string IncotermsType = string.Empty;
                                long BrokerId = 0;
                                var dealerRole = dealerRoleList.FirstOrDefault(_ => _.UserId == shipToPartyContext.Id);
                                if (dealerRole != null)
                                {
                                    DealerTypeId = dealerRole.RoleId == (int)DTO.Enums.Role.Broker ? (int)DTO.Enums.DealerType.Broker : (int)DTO.Enums.DealerType.Direct;
                                    if (dealerRole.RoleId == (int)DTO.Enums.Role.Broker)
                                    {
                                        BrokerId = shipToPartyContext.Id;
                                    }
                                    else
                                    {
                                        var soldToPartyRole = dealerRoleList.FirstOrDefault(_ => _.UserId == soldToPartyContext.Id);
                                        if (soldToPartyRole.RoleId == (int)DTO.Enums.Role.Broker)
                                        {
                                            BrokerId = soldToPartyContext.Id;
                                        }
                                        else
                                        {
                                            BrokerId = saudaOrdersContext.BrokerId;
                                        }
                                        //var BrokerContext = (from ucm in userCustomerMappingList
                                        //                     join ur in dealerRoleList on ucm.UserId equals ur.UserId
                                        //                     where ur.RoleId == (int)DTO.Enums.Role.Broker
                                        //                     && ucm.CustomerId == shipToPartyContext.Id
                                        //                     select new
                                        //                     {
                                        //                         BrokerId = ucm.UserId
                                        //                     }).FirstOrDefault();

                                        //if (BrokerContext != null)
                                        //{
                                        //    BrokerId = BrokerContext.BrokerId;
                                        //}
                                    }
                                    var BdoContext = (from ucm in userCustomerMappingList
                                                      join ur in dealerRoleList on ucm.UserId equals ur.UserId
                                                      where ur.RoleId == (int)DTO.Enums.Role.StateTrader
                                                      && ucm.CustomerId == shipToPartyContext.Id
                                                      select new
                                                      {
                                                          BdoId = ucm.UserId
                                                      }).FirstOrDefault();

                                    if (BdoContext != null)
                                    {
                                        userId = BdoContext.BdoId;
                                    }
                                    else
                                    {
                                        userId = shipToPartyContext.Id;
                                    }
                                }

                                var exPlantPrice = (decimal)0;
                                var forDepotPrice = (decimal)0;
                                var forPlantPrice = (decimal)0;
                                var exDepotPrice = (decimal)0;
                                var exRakePrice = (decimal)0;
                                var forRakePrice = (decimal)0;
                                if (incoTerms.Id == (int)DTO.Enums.IncoTerms.ExDepot)
                                {
                                    exDepotPrice = sauda.BidAmount + sauda.Rate1;
                                }
                                else if (incoTerms.Id == (int)DTO.Enums.IncoTerms.ExPlant)
                                {
                                    exPlantPrice = sauda.BidAmount + sauda.Rate1;
                                }
                                else if (incoTerms.Id == (int)DTO.Enums.IncoTerms.ExRake)
                                {
                                    exRakePrice = sauda.BidAmount + sauda.Rate1;
                                }
                                else if (incoTerms.Id == (int)DTO.Enums.IncoTerms.ForDepot)
                                {
                                    forDepotPrice = sauda.BidAmount + sauda.Rate1;
                                }
                                else if (incoTerms.Id == (int)DTO.Enums.IncoTerms.ForPlant)
                                {
                                    forPlantPrice = sauda.BidAmount + sauda.Rate1;
                                }
                                else if (incoTerms.Id == (int)DTO.Enums.IncoTerms.ForRake)
                                {
                                    forRakePrice = sauda.BidAmount + sauda.Rate1;
                                }

                                var pricingsUpdate = "UPDATE Pricings SET ForDepotPrice = @ForDepotPrice,ForPlantPrice=@ForPlantPrice,ForRakePrice=@ForRakePrice," +
                                    " ExDepotPrice=@ExDepotPrice,ExPlantPrice=@ExPlantPrice,ExRakePrice = @ExRakePrice,ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy WHERE Id = @Id";
                                var pricingsParameters = new[]{
                                    new SqlParameter("@ForDepotPrice", forDepotPrice),
                                    new SqlParameter("@ForPlantPrice", forPlantPrice),
                                    new SqlParameter("@ForRakePrice", forRakePrice),
                                    new SqlParameter("@ExDepotPrice", exDepotPrice),
                                    new SqlParameter("@ExPlantPrice", exPlantPrice),
                                    new SqlParameter("@ExRakePrice", exRakePrice),
                                    new SqlParameter("@Id", saudaOrdersContext.PricingId),
                                    new SqlParameter("@ModifiedDate", currentDate),
                                    new SqlParameter("@ModifiedBy", userId),
                                    };
                                _emamiContext.BulkUpdateProxy(pricingsUpdate, pricingsParameters);


                                //if(saudaOrdersContext.BidQuantityCase != 0)
                                //{
                                var bidPrice = ((sauda.BidAmount + sauda.Rate1) * sauda.Quantity);
                                var quantityMT = _resultService.ConvertCasetoMetricTon(sauda.Quantity, skuContext.Id);
                                var plantDepotId = depots.Id;
                                long depotIdForrake = 0;
                                if (incoTerms.Id == (int)DTO.Enums.IncoTerms.ExRake || incoTerms.Id == (int)DTO.Enums.IncoTerms.ForRake)
                                {
                                    depotIdForrake = depots.Id;
                                    var rakeContext = depotsList.FirstOrDefault(_ => _.DepotId == depots.Id);
                                    if (rakeContext != null)
                                        plantDepotId = rakeContext.Id;
                                }

                                var sqlUpdate = "UPDATE SaudaOrders SET ValidToDate = @ValidToDate,SkuId=@SkuId,OilTypeId=@OilTypeId,UomId=@UomId,BidQuantity=@BidQuantity,BidQuantityCase=@BidQuantityCase," +
                                    "PlantId=@PlantId,BrokerId=@BrokerId,Incoterms1=@Incoterms1,ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy,BidPrice =@BidPrice,QuotedPrice=@BidPrice,DepotIdForRake=@DepotIdForRake WHERE SaudaNumber = @SaudaNumber";
                                var parameters = new[]{
                                    new SqlParameter("@ValidToDate", sauda.ToDate),
                                    new SqlParameter("@SkuId", skuContext.Id),
                                    new SqlParameter("@OilTypeId", skuContext.OilTypeId),
                                    new SqlParameter("@UomId", uomContext != null ? uomContext.Id : 0),
                                    new SqlParameter("@BidQuantity", quantityMT),
                                    new SqlParameter("@BidQuantityCase", sauda.Quantity),
                                    new SqlParameter("@PlantId", plantDepotId),
                                    new SqlParameter("@BrokerId", BrokerId),
                                    new SqlParameter("@Incoterms1", sauda.INCO1),
                                    new SqlParameter("@Incoterms2", incoTerms != null? incoTerms.Id : 0),
                                    new SqlParameter("@SaudaNumber", sauda.SaudaNumber),
                                    new SqlParameter("@ModifiedDate", currentDate),
                                    new SqlParameter("@ModifiedBy", userId),
                                    new SqlParameter("@BidPrice", bidPrice),
                                    new SqlParameter("@DepotIdForRake", depotIdForrake),
                                    };
                                _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);

                                var saudaUpdate = "UPDATE Saudas SET UserId = @UserId ,ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy WHERE Id = @Id";
                                var saudaParameters = new[]{
                                    new SqlParameter("@UserId", shipToPartyContext.Id),
                                    new SqlParameter("@Id", saudaOrdersContext.SaudaId),
                                    new SqlParameter("@ModifiedDate", currentDate),
                                    new SqlParameter("@ModifiedBy", userId),
                                    };
                                _emamiContext.BulkUpdateProxy(saudaUpdate, saudaParameters);
                                dataSynced++;
                                //}

                            }
                            else
                            {
                                errorMessageList.Add(errorMessage);
                                errorRecordList.Add(sauda);
                            }
                        }
                        _emamiContext.SaveChanges();
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaViewDtoList;
                        sapDataSyncResultDto.SuccessRecordDetailsResponse = saudaViewDtoList.Except(errorRecordList).ToList();
                    }
                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    }
                    else
                    {
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
                }


            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaViewDtoList;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
                _logger.Error(message);
            }
        }
        #endregion            

        #region SaudaLimit Master

        /// <summary>
        /// Method to get sauda limit details
        /// </summary>       
        /// <returns></returns>
        public void GetSaudaLimitDetails()
        {
            _methodName = "GetSaudaLimitDetails";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var saudaLimitDtoList = new List<SAPSaudaLimitDto>();
            var syncFolder = ConsoleSettings.SaudaLimitFolder;
            var subject = ConsoleSettings.SaudaLimitSubject;
            var csvFileName = ConsoleSettings.SaudaLimitCsv;
            try
            {
                using (var _emamiContext = new AdaniContext())
                {
                    var saudaLimitContext = _emamiContext.SaudaLimit.AsNoTracking().Where(_ => _.IsSAPDataSyncOrNot == false && _.StatusId == (int)DTO.Enums.Status.Approved).ToList();
                    if (saudaLimitContext != null && saudaLimitContext.Any())
                    {
                        #region Get Common Datas
                        var customerContextList = _emamiContext.Users.AsNoTracking();
                        #endregion
                        foreach (var saudaLimit in saudaLimitContext)
                        {
                            var customerContext = customerContextList.FirstOrDefault(_ => _.Id == saudaLimit.UserId);
                            var saudaLimitDto = new SAPSaudaLimitDto
                            {
                                CustomerName = customerContext != null ? customerContext.Name : string.Empty,
                                CustomerCode = customerContext != null ? customerContext.Code : string.Empty,
                                PartnerFunction = customerContext != null ? customerContext.Code == UtilityHelper.GetEnumDescription(DTO.Enums.CustomerGroup.Customer) ? Constants.PartnerFunctionBroker : Constants.PartnerFunctionCustomer : string.Empty,
                                CustomerTotalQuantity = saudaLimit.RequestedLimit + saudaLimit.ActualLimit,
                                //VerticalCode = customerContext != null ? customerContext.Division.Code : string.Empty,
                                UOM = "MT"
                            };
                            saudaLimitDtoList.Add(saudaLimitDto);

                            var sqlUpdate = "UPDATE SaudaLimits SET IsSAPDataSyncOrNot = @IsSAPDataSyncOrNot WHERE Id = @Id";
                            var parameters = new[]{
                                    new SqlParameter("@IsSAPDataSyncOrNot", true),
                                    new SqlParameter("@Id", saudaLimit.Id),
                        };
                            _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                        }
                        _emamiContext.SaveChanges();
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = saudaLimitDtoList;
                        _sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
                    }
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = string.Concat(Constants.ServiceErrorMessage, message);
                _logger.Error(message);
                var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
            }
        }

        //public void SaveSaudaLimit(HANASaudaLimitDtoList inputDto)
        //{
        //    _methodName = "SaveSaudaLimit";
        //    _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
        //    _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputDto)}");
        //    var resultDto = new ResultDto();
        //    var sapDataSyncResultDto = new SapDataSyncResultDto();
        //    var errorRecordList = new List<HANASaudaLimitDto>();
        //    sapDataSyncResultDto.SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
        //    var dataSynced = 0;
        //    var synctype = ConsoleSettings.SaudaLimitSubject;
        //    var userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
        //    var subject = string.Concat(ConsoleSettings.SaudaLimitSubject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
        //    var saudaLimitDtoList = inputDto != null && inputDto.SaudaLimitList != null ? inputDto.SaudaLimitList : new List<HANASaudaLimitDto>();
        //    sapDataSyncResultDto.OutstandingResult.DataRetrieved = inputDto.SaudaLimitList.Count;
        //    try
        //    {
        //        if (saudaLimitDtoList != null && saudaLimitDtoList.Any())
        //        {
        //            var saudaLimitList = new List<SaudaLimit>();
        //            var errorMessageList = new List<string>();
        //            using (var _emamiContext = new EmamiContext())
        //            {
        //                #region Get Common Datas
        //                var verticalContextList = _emamiContext.Divisions.AsNoTracking();
        //                //var customerContextList = _emamiContext.Users.AsNoTracking();
        //                var customerContextList = (from s in _emamiContext.Users
        //                                           join role in _emamiContext.UserRoles on s.Id equals role.UserId
        //                                           where role.RoleId != (int)DTO.Enums.Role.ShipToParty
        //                                           select new { Id = s.Id, Code = s.Code, VerticalId = s.DivisionId, CustomerGroup = s.CustomerGroup, SaudaLimit = s.SaudaLimit }).ToList();
        //                #endregion
        //                foreach (var saudaLimit in saudaLimitDtoList)
        //                {
        //                    var errorFlag = true;
        //                    var errorMessage = string.Empty;
        //                    //if (saudaLimit == null)
        //                    //{
        //                    //    errorMessage = Constants.BindErrorMessage(Constants.InvalidRequest, errorMessage);
        //                    //    errorFlag = false;
        //                    //}
        //                    if (string.IsNullOrEmpty(saudaLimit.CustomerCode))
        //                    {
        //                        errorMessage = Constants.BindErrorMessage(Constants.CustomerCodeIsEmpty, errorMessage);
        //                        errorFlag = false;
        //                    }
        //                    if (string.IsNullOrEmpty(saudaLimit.VerticalCode))
        //                    {
        //                        errorMessage = Constants.BindErrorMessage(Constants.VerticalCodeEmpty, errorMessage);
        //                        errorFlag = false;
        //                    }
        //                    //if (saudaLimit.CustomerTotalQuantity == 0)
        //                    //{
        //                    //    errorMessage = Constants.BindErrorMessage(Constants.SaudaLimitIsEmpty, errorMessage);
        //                    //    errorFlag = false;
        //                    //}
        //                    var verticalContext = verticalContextList.FirstOrDefault(_ => _.Code.ToLower() == saudaLimit.VerticalCode.ToLower());
        //                    if (verticalContext == null)
        //                    {
        //                        errorMessage = Constants.BindErrorMessage(string.Format(Constants.VerticalCodeIsEmpty, saudaLimit.VerticalCode), errorMessage);
        //                        errorFlag = false;
        //                    }
        //                    var verticalId = verticalContext != null ? verticalContext.Id : 0;
        //                    var customerContext = customerContextList.FirstOrDefault(_ => _.Code == saudaLimit.CustomerCode /*&& _.CustomerGroup == saudaLimit.PartnerFunction*/ && _.VerticalId == verticalId);
        //                    if (customerContext == null)
        //                    {
        //                        errorMessage = Constants.BindErrorMessage(string.Format(Constants.CustomerCodeNotExist, saudaLimit.CustomerCode), errorMessage);
        //                        errorFlag = false;
        //                    }
        //                    if (errorFlag)
        //                    {
        //                        var saudaLimitDto = new SaudaLimit
        //                        {
        //                            //ActualLimit = customerContext.SaudaLimit,
        //                            //RequestedLimit = saudaLimit.CustomerTotalQuantity,
        //                            UserId = customerContext.Id,
        //                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        //                            CreatedBy = userId,
        //                            StatusId = (int)DTO.Enums.Status.Approved,
        //                            IsSAPData = true,
        //                            IsSAPDataSyncOrNot = true,
        //                            PendingContract = saudaLimit.PendCont,
        //                            PendingDO = saudaLimit.PendDO,
        //                            PendingOBD = saudaLimit.PendOBD
        //                        };

        //                        if (saudaLimitDto != null)
        //                        {
        //                            saudaLimitList.Add(saudaLimitDto);
        //                        }
        //                        dataSynced++;
        //                    }
        //                    else
        //                    {
        //                        errorMessageList.Add(errorMessage);
        //                        errorRecordList.Add(saudaLimit);
        //                    }
        //                }
        //                var suadaLimitdetailsDelete = "DELETE FROM SaudaLimits";
        //                var listOfStrings = new List<string>();
        //                object[] arrayOfStrings = listOfStrings.ToArray();
        //                _emamiContext.BulkUpdateProxy(suadaLimitdetailsDelete, arrayOfStrings);
        //                if (null != saudaLimitList && saudaLimitList.Any())
        //                {
        //                    _emamiContext.BulkInsertProxy(saudaLimitList);
        //                }
        //                _emamiContext.SaveChanges();
        //                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
        //                sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
        //                sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaLimitDtoList;
        //                sapDataSyncResultDto.SuccessRecordDetailsResponse = saudaLimitDtoList.Except(errorRecordList).ToList();
        //            }

        //            if (errorMessageList.Any())
        //            {
        //                resultDto.IsSuccess = false;
        //                resultDto.ErrorDto.Response = sapDataSyncResultDto;
        //                resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
        //            }
        //            else
        //            {

        //                resultDto.IsSuccess = true;
        //                resultDto.SuccessDto.Response = sapDataSyncResultDto;
        //                resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
        //            }
        //            sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
        //            _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
        //        }

        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        resultDto.IsSuccess = false;
        //        sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaLimitDtoList;
        //        resultDto.ErrorDto.ErrorCode = Constants.Exception;
        //        resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
        //        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
        //        sapDataSyncResultDto.ExceptionMessage = message;
        //        resultDto.ErrorDto.Response = sapDataSyncResultDto;
        //        _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
        //    }
        //}


        #endregion

        #region Credit Master
        public void SaveCreditMaster(HANACreditMasterDtoList inputDto)
        {
            _methodName = "CreditMaster";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputDto)}");
            var resultDto = new ResultDto();
            var sapDataSyncResultDto = new SapDataSyncResultDto();
            var errorRecordList = new List<HANACreditMasterDto>();
            sapDataSyncResultDto.SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
            var dataSynced = 0;
            var synctype = ConsoleSettings.CreditMasterSubject;
            var userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var creditMasterDtoList = inputDto != null && inputDto.CreditMasterList != null ? inputDto.CreditMasterList : new List<HANACreditMasterDto>();
            var subject = string.Concat(ConsoleSettings.CreditMasterSubject, ConsoleSettings.SAPToAppDataSyncEmailSubject);
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = creditMasterDtoList.Count;
            try
            {
                if (creditMasterDtoList != null && creditMasterDtoList.Any())
                {
                    var userCreditMasterList = new List<UserCreditMaster>();
                    var errorMessageList = new List<string>();
                    using (var _emamiContext = new AdaniContext())
                    {
                        #region Get Common Datas
                        //var verticalContextList = _emamiContext.DivisionDetails.AsNoTracking();
                        //var customerContextList = _emamiContext.Users.AsNoTracking();
                        var customerContextList = (from s in _emamiContext.Users
                                                   join role in _emamiContext.UserRoles on s.Id equals role.UserId
                                                   where role.RoleId != (int)DTO.Enums.Role.ShipToParty
                                                   select new { Id = s.Id, Code = s.Code/*, VerticalId = s.DivisionId*/, CustomerGroup = s.CustomerGroup }).ToList();
                        #endregion

                        foreach (var creditMaster in creditMasterDtoList)
                        {
                            var errorFlag = true;
                            var errorMessage = string.Concat("Customer Code : ", creditMaster.CustomerCode);
                            if (creditMaster == null)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.InvalidRequest, errorMessage);
                                errorFlag = false;
                            }
                            //if (creditMaster.CreditLimit != 0 && creditMaster.CreditExposure == 0)
                            //{
                            //    errorMessage = Constants.BindErrorMessage(Constants.CreditLimitAndCreditExposureIsEmpty, errorMessage);
                            //    errorFlag = false;
                            //}
                            //var verticalContext = verticalContextList.FirstOrDefault(_ => _.CCArea.ToLower() == creditMaster.CCreditArea.ToLower());
                            //if (verticalContext == null)
                            //{
                            //    errorMessage = Constants.BindErrorMessage(string.Format(Constants.VerticalCodeIsEmpty, creditMaster.CCreditArea), errorMessage);
                            //    errorFlag = false;
                            //}
                            //var verticalId = verticalContext != null ? verticalContext.DivisionId : 0;
                            var customerContext = customerContextList.FirstOrDefault(_ => _.Code == creditMaster.CustomerCode);
                            if (customerContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.CustomerCodeNotExist, creditMaster.CustomerCode), errorMessage);
                                errorFlag = false;
                            }
                            if (errorFlag)
                            {
                                var userCreditMasterDto = new UserCreditMaster
                                {

                                    UserId = customerContext.Id,
                                    //CCreditArea = creditMaster.CCreditArea,
                                    CreditAccountNumber = creditMaster.CreditAccountNumber,
                                    RiskCat = creditMaster.RiskCat,
                                    Curr = creditMaster.Curr,
                                    CreditLimit = creditMaster.CreditLimit,
                                    CreditExposure = creditMaster.CreditExposure,
                                    SalesValue = creditMaster.SalesValue,
                                    TotalReceivable = creditMaster.TotalReceivable,
                                    SaudaDepC = creditMaster.SaudaDepC,
                                    SecDepH = creditMaster.SecDepH,
                                    BankGuarM = creditMaster.BankGuarM,
                                    DueToday = creditMaster.DueToday,
                                    TomorrowsDue = creditMaster.TomorrowsDue,
                                    Overdue = creditMaster.Overdue,
                                    NotDue = creditMaster.NotDue,
                                    NextIntRev = creditMaster.NextIntRev,
                                    Blocked = creditMaster.Blocked,
                                    TotalLimit = creditMaster.TotalLimit,
                                    IndividLimit = creditMaster.IndividLimit,
                                    AvailableCreditLimit = creditMaster.AvailableCreditLimit,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    CreatedBy = userId,
                                    IsSAPData = true,
                                    Isactive = true
                                };
                                if (userCreditMasterDto != null)
                                {
                                    userCreditMasterList.Add(userCreditMasterDto);
                                }
                                dataSynced++;
                                var userCreditMasterContext = _emamiContext.UserCreditMaster.AsNoTracking().Where(_ => _.UserId == customerContext.Id && _.Isactive).ToList();
                                if (userCreditMasterContext != null)
                                {
                                    foreach (var data in userCreditMasterContext)
                                    {
                                        var sqlUpdate = "UPDATE UserCreditMasters SET Isactive = @Isactive,ModifiedDate=@ModifiedDate, ModifiedBy=@ModifiedBy WHERE Id = @Id";
                                        var parameters = new[]{
                                        new SqlParameter("@Isactive", false),
                                        new SqlParameter("@Id", data.Id),
                                        new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow)),
                                        new SqlParameter("@ModifiedBy", userId)
                                        };
                                        _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                    }
                                }
                            }
                            else
                            {
                                errorMessageList.Add(errorMessage);
                                errorRecordList.Add(creditMaster);
                            }
                        }
                        if (null != userCreditMasterList && userCreditMasterList.Any())
                        {
                            _emamiContext.BulkInsertProxy(userCreditMasterList);
                        }
                        _emamiContext.SaveChanges();
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = creditMasterDtoList;
                        sapDataSyncResultDto.SuccessRecordDetailsResponse = creditMasterDtoList.Except(errorRecordList).ToList();
                    }

                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    }
                    else
                    {
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = creditMasterDtoList;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }
        #endregion              

        #region DODelete     
        public void DODelete(string decryptedString)
        {
            _methodName = "DODelete";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var liftingSyncData = string.Empty;
            var messageSync = string.Empty;
            var sapDataSyncResultDto = new SapDataSyncResultDto();
            var errorRecordList = new List<SAPDoDeleteDto>();
            sapDataSyncResultDto.SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
            var dataSynced = 0;
            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
            var userId = UtilityHelper.LongTryToParse(jarray[0]["loginUserId"].ToString());
            var syncFolder = jarray[0]["syncFolder"].ToString();
            var subject = jarray[0]["subject"].ToString();
            var folderPath = ConsoleSettings.InboundDirectoryPath(syncFolder);
            var inputDto = _sftpConnectorService.GetSFTPFile(folderPath, syncFolder);
            var doDeleteDtoList = !string.IsNullOrEmpty(inputDto.Response.ToString()) ? (List<SAPDoDeleteDto>)inputDto.Response : new List<SAPDoDeleteDto>();
            subject = string.Concat(subject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = doDeleteDtoList.Count;
            try
            {
                if (doDeleteDtoList != null && doDeleteDtoList.Any())
                {

                    var errorMessageList = new List<string>();
                    using (var _emamiContext = new AdaniContext())
                    {
                        #region Get Common Datas
                        var saudaOrderLiftingRequestMappingContextList = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking();
                        #endregion
                        foreach (var doDetails in doDeleteDtoList)
                        {
                            var errorFlag = true;
                            var errorMessage = string.Concat("DO Number : ", doDetails.DONumber);
                            if (doDetails == null)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.InvalidRequest, errorMessage);
                                errorFlag = false;
                            }
                            var saudaOrderLiftingRequestMappingContext = saudaOrderLiftingRequestMappingContextList.FirstOrDefault(_ => _.DeliveryOrderNumber == doDetails.DONumber);
                            if (saudaOrderLiftingRequestMappingContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.DONumberIsNotEmpty, doDetails.DONumber), errorMessage);
                                errorFlag = false;
                            }
                            if (errorFlag)
                            {
                                dataSynced++;


                                var sqlDelete = "UPDATE SaudaOrderLiftingRequestMappings SET StatusId = @StatusId  WHERE Id = @Id";
                                var deleteParameters = new[]{
                                 new SqlParameter("@StatusId", (int)DTO.Enums.Status.Deleted),
                                new SqlParameter("@Id", saudaOrderLiftingRequestMappingContext.Id)
                                };
                                _emamiContext.BulkUpdateProxy(sqlDelete, deleteParameters);

                                if (saudaOrderLiftingRequestMappingContext.SaudaOrderId != 0)
                                {
                                    var saudaOrdersUpdate = "UPDATE SaudaOrders SET StatusId = @StatusId  WHERE Id = @Id";
                                    var saudaOrdersUpdateParameters = new[]{
                                     new SqlParameter("@StatusId", (int)DTO.Enums.Status.Approved),
                                     new SqlParameter("@Id", saudaOrderLiftingRequestMappingContext.SaudaOrderId)};
                                    _emamiContext.BulkUpdateProxy(saudaOrdersUpdate, saudaOrdersUpdateParameters);
                                }


                                decimal liftingQuantityCase = 0;
                                decimal liftingQuantity = 0;
                                var liftingRequestDetailContext = _emamiContext.LiftingRequestDetails.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderLiftingRequestMappingContext.LiftingRequestDetailId);
                                if (liftingRequestDetailContext != null)
                                {
                                    var actualLiftingQuantityCase = liftingRequestDetailContext.LiftingQuantityCase - saudaOrderLiftingRequestMappingContext.LiftingQuantityCase;
                                    liftingQuantityCase = actualLiftingQuantityCase > 0 ? actualLiftingQuantityCase : 0;
                                    liftingQuantity = liftingQuantityCase > 0 ? _resultService.ConvertCasetoMetricTon(liftingQuantityCase, liftingRequestDetailContext.SkuId) : 0;
                                }

                                var sqlUpdate = "UPDATE LiftingRequestDetails SET LiftingQuantity =@LiftingQuantity, LiftingQuantityCase=@LiftingQuantityCase,  ModifiedDate=@ModifiedDate, ModifiedBy=@ModifiedBy WHERE Id = @Id";
                                var parameters = new[]{
                                new SqlParameter("@LiftingQuantity", liftingQuantity),
                                new SqlParameter("@LiftingQuantityCase", liftingQuantityCase),
                                new SqlParameter("@Id", saudaOrderLiftingRequestMappingContext.LiftingRequestDetailId),
                                new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow)),
                                new SqlParameter("@ModifiedBy", userId)
                            };
                                _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                            }
                            else
                            {
                                errorMessageList.Add(errorMessage);
                                errorRecordList.Add(doDetails);
                            }
                        }
                        _emamiContext.SaveChanges();
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                    }
                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    }
                    else
                    {
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, inputDto, subject);
                }
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = string.Concat(Constants.ServiceErrorMessage, message);
                _logger.Error(message);
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, inputDto, subject);
            }
        }
        #endregion

        #region DOUpdate        
        public void DOUpdate(string decryptedString)
        {
            _methodName = "DOUpdate";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var liftingSyncData = string.Empty;
            var messageSync = string.Empty;
            var sapDataSyncResultDto = new SapDataSyncResultDto();
            var errorRecordList = new List<SAPDoUpdateDto>();
            sapDataSyncResultDto.SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
            var dataSynced = 0;
            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
            var userId = UtilityHelper.LongTryToParse(jarray[0]["loginUserId"].ToString());
            var syncFolder = jarray[0]["syncFolder"].ToString();
            var subject = jarray[0]["subject"].ToString();
            var folderPath = ConsoleSettings.InboundDirectoryPath(syncFolder);
            var inputDto = _sftpConnectorService.GetSFTPFile(folderPath, syncFolder);
            var doUpdateDtoList = !string.IsNullOrEmpty(inputDto.Response.ToString()) ? (List<SAPDoUpdateDto>)inputDto.Response : new List<SAPDoUpdateDto>();
            subject = string.Concat(subject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = doUpdateDtoList.Count;
            try
            {
                var saudaOrderLiftingRequestMappingList = new List<SaudaOrderLiftingRequestMapping>();
                var liftingRequestDetails = new List<LiftingRequestDetails>();

                if (doUpdateDtoList != null && doUpdateDtoList.Any())
                {
                    var errorMessageList = new List<string>();
                    using (var _emamiContext = new AdaniContext())
                    {
                        #region Get Common Data's

                        var doVerticals = doUpdateDtoList.Select(s => s.Vertical.ToLower()).Distinct().ToList();
                        var VerticalsData = _emamiContext.Divisions.AsNoTracking()
                            .Where(_ => doVerticals.Contains(_.Code.ToLower()))
                            .Select(s => new { Id = s.Id, Code = s.Code }).ToList();

                        var doUoms = doUpdateDtoList.Select(s => s.Uom).Distinct().ToList();
                        var UomData = _emamiContext.Uom.AsNoTracking()
                            .Where(_ => doUoms.Contains(_.SAPName))
                            .Select(s => new UomDto { Id = s.Id, SAPName = s.SAPName }).ToList();

                        var doMaterialNumber = doUpdateDtoList.Select(s => s.MaterialNumber).Distinct().ToList();
                        var SkusData = _emamiContext.Skus.AsNoTracking()
                            .Where(_ => doMaterialNumber.Contains(_.SkuCode))
                            .Select(s => new SkuDto { Id = s.Id, OilTypeId = s.OilType.Id, SkuCode = s.SkuCode, VerticalId = s.DivisionId }).ToList();

                        var doSoldToParty = doUpdateDtoList.Select(s => s.SoldToParty).Distinct().ToList();

                        var UsersData = (from user in _emamiContext.Users
                                         join role in _emamiContext.UserRoles on user.Id equals role.UserId
                                         where doSoldToParty.Contains(user.Code) && role.RoleId != (int)DTO.Enums.Role.ShipToParty
                                         select new UserDto { Id = user.Id/*, VerticalId = user.DivisionId*/, Code = user.Code }).ToList();

                        var doSaudaNumber = doUpdateDtoList.Select(s => s.SaudaNumber).Distinct().ToList();
                        var SaudaOrdersData = _emamiContext.SaudaOrders.AsNoTracking()
                            .Where(_ => doSaudaNumber.Contains(_.SaudaNumber))
                            .Select(s => new SaudaOrderDto { Id = s.Id, SaudaNumber = s.SaudaNumber, SkuId = s.SkuId, BidQuantityCase = s.BidQuantityCase }).ToList();

                        //var doDONumber = doUpdateDtoList.Select(s => s.DONumber).Distinct().ToList();
                        //var saudaIds = SaudaOrdersData.Select(s => s.Id).ToList();
                        //doDONumber.Contains(_.DeliveryOrderNumber) && 
                        //var SaudaOrderLiftingRequestMappingData1 = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking()
                        //    .Where(_ => saudaIds.Contains(_.SaudaOrderId))
                        //    .Select(s => new { Id = s.Id, SaudaOrderId = s.SaudaOrderId, DeliveryOrderNumber = s.DeliveryOrderNumber, LiftingQuantityCase = s.LiftingQuantityCase }).ToList();

                        //var saudaSkuIds = SaudaOrdersData.Select(s => s.SkuId).ToList();
                        //var SaudaOrderSkusData = _emamiContext.Skus.AsNoTracking()
                        //    .Where(_ => saudaSkuIds.Contains(_.Id))
                        //    .Select(s => new { Id = s.Id, PackGroupId = s.PackGroupId }).ToList();

                        //var SkuUomMappingData = _emamiContext.SkuUomMapping.AsNoTracking()
                        //    .Where(_ => saudaSkuIds.Contains(_.SkuId))
                        //    .Select(s => new { SkuId = s.SkuId, UomId = s.UomId, RelationUomId = s.RelationUomId, ConversionFactor = s.ConversionFactor });

                        #endregion

                        foreach (var doDetails in doUpdateDtoList)
                        {
                            var errorFlag = true;
                            var errorMessage = string.Concat("DO Number : ", doDetails.DONumber);
                            if (doDetails == null)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.InvalidRequest, errorMessage);
                                errorFlag = false;
                            }

                            if (string.IsNullOrEmpty(doDetails.DONumber))
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.InvalidRequest, errorMessage);
                                errorFlag = false;
                            }

                            var skuDetails = SkusData.FirstOrDefault(_ => _.SkuCode == doDetails.MaterialNumber);
                            long skuDetailsId = 0;
                            if (skuDetails != null)
                            {
                                skuDetailsId = skuDetails.Id;
                            }

                            //var verticalContext = _emamiContext.Verticals.AsNoTracking().FirstOrDefault(_ => _.Code.ToLower() == doDetails.Vertical.ToLower());
                            var verticalContext = VerticalsData.FirstOrDefault(_ => _.Code.ToLower() == doDetails.Vertical.ToLower());
                            var verticalId = verticalContext == null ? 0 : verticalContext.Id;

                            //var UOMId = _emamiContext.Uom.AsNoTracking().FirstOrDefault(_ => _.SAPName == doDetails.Uom);
                            var UOMId = UomData.FirstOrDefault(_ => _.SAPName == doDetails.Uom);

                            //var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.SkuCode == doDetails.MaterialNumber && _.VerticalId == verticalId);
                            var skuContext = SkusData.FirstOrDefault(_ => _.SkuCode == doDetails.MaterialNumber && _.VerticalId == verticalId);

                            var skuId = skuContext == null ? 0 : skuContext.Id;
                            decimal quantityMT = 0;
                            decimal quantityCase = 0;
                            if (!string.IsNullOrEmpty(doDetails.Enquiry))
                            {
                                var liftingRequestDetailNotInDONumberContext = _emamiContext.LiftingRequestDetails.FirstOrDefault(_ => _.EnquiryNumber == doDetails.Enquiry && _.SkuId == skuDetailsId);
                                if (liftingRequestDetailNotInDONumberContext != null && string.IsNullOrEmpty(liftingRequestDetailNotInDONumberContext.DeliveryOrderNumber))
                                {
                                    if (!string.IsNullOrEmpty(doDetails.DONumber))
                                    {
                                        liftingRequestDetailNotInDONumberContext.DeliveryOrderNumber = doDetails.DONumber;
                                        _emamiContext.SaveChanges();
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(doDetails.DONumber))
                            {
                                var liftingRequestDetailNotInDONumberContext = _emamiContext.LiftingRequestDetails.FirstOrDefault(_ => _.DeliveryOrderNumber == doDetails.DONumber && _.SkuId == skuDetailsId);
                                if (liftingRequestDetailNotInDONumberContext != null && string.IsNullOrEmpty(liftingRequestDetailNotInDONumberContext.EnquiryNumber))
                                {
                                    if (!string.IsNullOrEmpty(doDetails.Enquiry))
                                    {
                                        liftingRequestDetailNotInDONumberContext.EnquiryNumber = doDetails.Enquiry;
                                        _emamiContext.SaveChanges();
                                    }
                                }
                            }

                            var liftingRequestDetailEnquiryNumberContext = _emamiContext.LiftingRequestDetails.AsNoTracking().FirstOrDefault(_ => _.DeliveryOrderNumber == doDetails.DONumber && _.EnquiryNumber == doDetails.Enquiry && _.SkuId == skuDetailsId);
                            var linearSaudaOrderLiftingRequestMappingContext = _emamiContext.SaudaOrderLiftingRequestMapping.FirstOrDefault(_ => _.DeliveryOrderNumber == doDetails.DONumber);
                            //EnquiryNumber With remarks update
                            if (string.IsNullOrEmpty(doDetails.DONumber) && !string.IsNullOrEmpty(doDetails.Enquiry))
                            {

                                if (liftingRequestDetailEnquiryNumberContext != null)
                                {
                                    var sqlUpdate = "UPDATE LiftingRequestDetails SET ModifiedDate=@ModifiedDate," +
                                    "Remarks=@EnquiryRemarks,EnquiryNumber=@EnquiryNumber, ModifiedBy=@ModifiedBy WHERE Id = @Id";
                                    var parameters = new[]{
                                            new SqlParameter("@Id", liftingRequestDetailEnquiryNumberContext.Id),
                                            new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow)),
                                            new SqlParameter("@ModifiedBy", userId),
                                            new SqlParameter("@EnquiryRemarks", doDetails.Reason),
                                            new SqlParameter("@EnquiryNumber", doDetails.Enquiry)
                                    };
                                    _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                    _emamiContext.SaveChanges();
                                    dataSynced++;
                                }
                                else
                                {
                                    errorMessageList.Add(Constants.BindErrorMessage(string.Format(Constants.EnquiryNumberNotFound, doDetails.Enquiry), errorMessage));
                                    errorRecordList.Add(doDetails);
                                }
                            }
                            if (!string.IsNullOrEmpty(doDetails.DONumber) && string.IsNullOrEmpty(doDetails.Enquiry))
                            {
                                //Exist DO number Not In Enquirey Number
                                if (UOMId != null && UOMId.Id == (int)DTO.Enums.Uom.MT && skuId != 0)
                                {
                                    quantityMT = doDetails.OrderQuantity;
                                    quantityCase = _resultService.ConvertMetricTontoNosOrCase(doDetails.OrderQuantity, skuId, (int)DTO.Enums.Uom.Case);
                                }
                                else if (UOMId != null && UOMId.Id == (int)DTO.Enums.Uom.Nos && skuId != 0)
                                {
                                    quantityMT = _resultService.ConvertCasetoMetricTon(doDetails.OrderQuantity, skuId);
                                    quantityCase = doDetails.OrderQuantity;
                                }
                                else if (UOMId != null && UOMId.Id == (int)DTO.Enums.Uom.Case && skuId != 0)
                                {
                                    quantityMT = _resultService.ConvertCasetoMetricTon(doDetails.OrderQuantity, skuId);
                                    quantityCase = doDetails.OrderQuantity;
                                }
                                if (linearSaudaOrderLiftingRequestMappingContext == null)
                                {

                                    var userContext = UsersData.FirstOrDefault(_ => _.Code == doDetails.SoldToParty && _.VerticalId == verticalId);
                                    if (userContext == null)
                                    {
                                        errorMessage = Constants.BindErrorMessage(string.Format(Constants.SoldToPartyNotFound, doDetails.SoldToParty), errorMessage);
                                        errorFlag = false;
                                    }

                                    var saudaOrdersContext = SaudaOrdersData.FirstOrDefault(_ => _.SaudaNumber == doDetails.SaudaNumber);
                                    if (saudaOrdersContext == null)
                                    {
                                        errorMessage = Constants.BindErrorMessage(string.Format(Constants.SaudaNumberIsNotEmpty, doDetails.SaudaNumber), errorMessage);
                                        errorFlag = false;
                                    }

                                    if (skuContext == null)
                                    {
                                        errorMessage = Constants.BindErrorMessage(string.Format(Constants.SkuDetailsIsEmpty, doDetails.MaterialNumber), errorMessage);
                                        errorFlag = false;
                                    }
                                    if (errorFlag)
                                    {
                                        NewLiftingRequestCreate(userContext, userId, skuContext, quantityMT, quantityCase, UOMId, doDetails, saudaOrdersContext);
                                        dataSynced++;
                                    }
                                    else
                                    {
                                        errorRecordList.Add(doDetails);
                                    }
                                }
                                else
                                {
                                    //Update DO Details and Sauda Order Lifting Request Mapping                                   

                                    var liftingRequestDetailsId = UpdateLiftingRequestDetails(linearSaudaOrderLiftingRequestMappingContext.LiftingRequestDetailId, UsersData, verticalId, userId, skuContext, quantityMT, quantityCase, UOMId, doDetails);
                                    linearSaudaOrderLiftingRequestMappingContext.LiftingQuantity = quantityMT;
                                    linearSaudaOrderLiftingRequestMappingContext.LiftingQuantityCase = quantityCase;
                                    linearSaudaOrderLiftingRequestMappingContext.UomId = Convert.ToInt32(UOMId.Id);
                                    linearSaudaOrderLiftingRequestMappingContext.LiftingRequestDetailId = liftingRequestDetailsId;
                                    linearSaudaOrderLiftingRequestMappingContext.ModifiedBy = userId;
                                    linearSaudaOrderLiftingRequestMappingContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    _emamiContext.SaveChanges();
                                    dataSynced++;
                                }
                            }
                            else
                            {
                                if (errorFlag)
                                {
                                    if (UOMId != null && UOMId.Id == (int)DTO.Enums.Uom.MT && skuId != 0)
                                    {
                                        quantityMT = doDetails.OrderQuantity;
                                        quantityCase = _resultService.ConvertMetricTontoNosOrCase(doDetails.OrderQuantity, skuId, (int)DTO.Enums.Uom.Case);
                                    }
                                    else if (UOMId != null && UOMId.Id == (int)DTO.Enums.Uom.Nos && skuId != 0)
                                    {
                                        quantityMT = _resultService.ConvertCasetoMetricTon(doDetails.OrderQuantity, skuId);
                                        quantityCase = doDetails.OrderQuantity;
                                    }
                                    else if (UOMId != null && UOMId.Id == (int)DTO.Enums.Uom.Case && skuId != 0)
                                    {
                                        quantityMT = _resultService.ConvertCasetoMetricTon(doDetails.OrderQuantity, skuId);
                                        quantityCase = doDetails.OrderQuantity;
                                    }
                                    //Enquiry Number not exist
                                    if (liftingRequestDetailEnquiryNumberContext == null)
                                    {
                                        //var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Code == doDetails.SoldToParty && _.VerticalId == verticalId);

                                        var userContext = UsersData.FirstOrDefault(_ => _.Code == doDetails.SoldToParty && _.VerticalId == verticalId);
                                        if (userContext == null)
                                        {
                                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.SoldToPartyNotFound, doDetails.SoldToParty), errorMessage);
                                            errorFlag = false;
                                        }

                                        //var saudaOrdersContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == doDetails.SaudaNumber);
                                        var saudaOrdersContext = SaudaOrdersData.FirstOrDefault(_ => _.SaudaNumber == doDetails.SaudaNumber);
                                        if (saudaOrdersContext == null)
                                        {
                                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.SaudaNumberIsNotEmpty, doDetails.SaudaNumber), errorMessage);
                                            errorFlag = false;
                                        }

                                        if (skuContext == null)
                                        {
                                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.SkuDetailsIsEmpty, doDetails.MaterialNumber), errorMessage);
                                            errorFlag = false;
                                        }
                                        if (errorFlag)
                                        {
                                            //DoNumber Not Exist
                                            if (linearSaudaOrderLiftingRequestMappingContext == null)
                                            {
                                                NewLiftingRequestCreate(userContext, userId, skuContext, quantityMT, quantityCase, UOMId, doDetails, saudaOrdersContext);
                                                dataSynced++;
                                            }
                                            else
                                            {
                                                //Update DO Details and Sauda Order Lifting Request Mapping  
                                                var liftingRequestDetailsId = UpdateLiftingRequestDetails(linearSaudaOrderLiftingRequestMappingContext.LiftingRequestDetailId, UsersData, verticalId, userId, skuContext, quantityMT, quantityCase, UOMId, doDetails);


                                                linearSaudaOrderLiftingRequestMappingContext.LiftingQuantity = quantityMT;
                                                linearSaudaOrderLiftingRequestMappingContext.LiftingQuantityCase = quantityCase;
                                                linearSaudaOrderLiftingRequestMappingContext.UomId = Convert.ToInt32(UOMId.Id);
                                                linearSaudaOrderLiftingRequestMappingContext.LiftingRequestDetailId = liftingRequestDetailsId;
                                                linearSaudaOrderLiftingRequestMappingContext.ModifiedBy = userId;
                                                linearSaudaOrderLiftingRequestMappingContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                                _emamiContext.SaveChanges();
                                                dataSynced++;
                                            }
                                        }
                                        else
                                        {
                                            errorRecordList.Add(doDetails);
                                        }
                                    }
                                    else
                                    {
                                        //Check DO Number not in Sauda Order mapping exist                                       
                                        if (linearSaudaOrderLiftingRequestMappingContext == null)
                                        {
                                            //var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Code == doDetails.SoldToParty && _.VerticalId == verticalId);
                                            var userContext = UsersData.FirstOrDefault(_ => _.Code == doDetails.SoldToParty && _.VerticalId == verticalId);
                                            if (userContext == null)
                                            {
                                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.SoldToPartyNotFound, doDetails.SoldToParty), errorMessage);
                                                errorFlag = false;
                                            }

                                            //var saudaOrdersContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == doDetails.SaudaNumber);
                                            var saudaOrdersContext = SaudaOrdersData.FirstOrDefault(_ => _.SaudaNumber == doDetails.SaudaNumber);
                                            if (saudaOrdersContext == null)
                                            {
                                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.SaudaNumberIsNotEmpty, doDetails.SaudaNumber), errorMessage);
                                                errorFlag = false;
                                            }

                                            if (skuContext == null)
                                            {
                                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.SkuDetailsIsEmpty, doDetails.MaterialNumber), errorMessage);
                                                errorFlag = false;
                                            }
                                            if (errorFlag)
                                            {

                                                var saudaLimitDto = new SaudaOrderLiftingRequestMapping
                                                {
                                                    DeliveryOrderNumber = doDetails.DONumber,
                                                    LiftingQuantityCase = quantityCase,
                                                    LiftingQuantity = quantityMT,
                                                    SaudaOrderId = saudaOrdersContext.Id,
                                                    UomId = Convert.ToInt32(UOMId.Id),
                                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                    CreatedBy = userId,
                                                    LiftingRequestDetailId = liftingRequestDetailEnquiryNumberContext.Id,
                                                    StatusId = (int)DTO.Enums.Status.Inprogress
                                                };
                                                _emamiContext.SaudaOrderLiftingRequestMapping.Add(saudaLimitDto);


                                                var sqlUpdateLiftingRequestDetails = "UPDATE LiftingRequestDetails SET LiftingQuantity =@LiftingQuantity, LiftingQuantityCase=@LiftingQuantityCase,  ModifiedDate=@ModifiedDate," +
                                                "Remarks=@Remarks,ModifiedBy=@ModifiedBy WHERE Id = @Id";
                                                var parameters = new[]{
                                                    new SqlParameter("@LiftingQuantity", quantityMT),
                                                    new SqlParameter("@LiftingQuantityCase", quantityCase),
                                                    new SqlParameter("@Id", liftingRequestDetailEnquiryNumberContext.Id),
                                                    new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow)),
                                                    new SqlParameter("@ModifiedBy", userId),
                                                    new SqlParameter("@Remarks", doDetails.Reason),
                                                    //new SqlParameter("@EnquiryNumber", doDetails.Enquiry)
                                                    };
                                                _emamiContext.BulkUpdateProxy(sqlUpdateLiftingRequestDetails, parameters);
                                                _emamiContext.SaveChanges();
                                                dataSynced++;
                                            }
                                            else
                                            {
                                                errorRecordList.Add(doDetails);
                                            }
                                        }
                                        else
                                        {
                                            //Update DO Details and Sauda Order Lifting Request Mapping
                                            //decimal liftingQuantityCase = 0;
                                            //decimal liftingQuantity = 0;
                                            //var liftingRequestDetailContext = _emamiContext.LiftingRequestDetails.AsNoTracking().FirstOrDefault(_ => _.Id == linearSaudaOrderLiftingRequestMappingContext.LiftingRequestDetailId);
                                            //if (liftingRequestDetailContext != null)
                                            //{
                                            //    var actualLiftingQuantityCase = (liftingRequestDetailContext.LiftingQuantityCase - linearSaudaOrderLiftingRequestMappingContext.LiftingQuantityCase) + quantityCase;
                                            //    liftingQuantityCase = actualLiftingQuantityCase > 0 ? actualLiftingQuantityCase : 0;
                                            //    liftingQuantity = liftingQuantityCase > 0 ? _resultService.ConvertCasetoMetricTon(liftingQuantityCase, liftingRequestDetailContext.SkuId) : 0;

                                            //    var sqlUpdate = "UPDATE LiftingRequestDetails SET LiftingQuantity =@LiftingQuantity, LiftingQuantityCase=@LiftingQuantityCase,  ModifiedDate=@ModifiedDate," +
                                            //    "Remarks=@Remarks,ModifiedBy=@ModifiedBy WHERE Id = @Id";
                                            //    var parameters = new[]{
                                            //    new SqlParameter("@LiftingQuantity", liftingQuantity),
                                            //    new SqlParameter("@LiftingQuantityCase", liftingQuantityCase),
                                            //    new SqlParameter("@Id", linearSaudaOrderLiftingRequestMappingContext.LiftingRequestDetailId),
                                            //    new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow)),
                                            //    new SqlParameter("@ModifiedBy", userId),
                                            //    new SqlParameter("@Remarks", doDetails.Reason),
                                            //    //new SqlParameter("@EnquiryNumber", doDetails.Enquiry)

                                            //    };
                                            //    _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                            //}
                                            //else
                                            //{
                                            //    var userContext = UsersData.FirstOrDefault(_ => _.Code == doDetails.SoldToParty && _.VerticalId == verticalId);
                                            //    var liftingRequestContext = new LiftingRequest
                                            //    {
                                            //        LiftingDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                            //        UserId = userContext.Id,
                                            //        LiftingStatusId = (int)DTO.Enums.LiftingRequestStatus.Inprogress,
                                            //        StatusId = (int)DTO.Enums.Status.Approved,
                                            //        CreatedBy = userId,
                                            //        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                            //        IsSAPDataSync = true
                                            //    };
                                            //    _emamiContext.LiftingRequest.Add(liftingRequestContext);
                                            //    _emamiContext.SaveChanges();
                                            //    liftingRequestContext.LiftingRequestNumber = liftingRequestContext.Id.ToString();

                                            //    if (errorFlag)
                                            //    {
                                            //        var liftingReq = new LiftingRequestDetails
                                            //        {
                                            //            LiftingRequestId = liftingRequestContext.Id,
                                            //            SkuId = skuContext.Id,
                                            //            OilTypeId = Convert.ToInt32(skuContext.OilTypeId),
                                            //            LiftingQuantity = quantityMT,
                                            //            LiftingQuantityCase = quantityCase,
                                            //            DeliveryOrderNumber = doDetails.DONumber,
                                            //            UomId = Convert.ToInt32(UOMId.Id),
                                            //            CreatedBy = userId,
                                            //            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                            //            Remarks = doDetails.Reason,
                                            //            EnquiryNumber = doDetails.Enquiry
                                            //        };
                                            //        _emamiContext.LiftingRequestDetails.Add(liftingReq);
                                            //        _emamiContext.SaveChanges();
                                            //        linearSaudaOrderLiftingRequestMappingContext.LiftingRequestDetailId = liftingReq.Id;
                                            //    }
                                            //}
                                            //Update DO Details and Sauda Order Lifting Request Mapping                                            
                                            var liftingRequestDetailsId = UpdateLiftingRequestDetails(linearSaudaOrderLiftingRequestMappingContext.LiftingRequestDetailId, UsersData, verticalId, userId, skuContext, quantityMT, quantityCase, UOMId, doDetails);


                                            linearSaudaOrderLiftingRequestMappingContext.LiftingQuantity = quantityMT;
                                            linearSaudaOrderLiftingRequestMappingContext.LiftingQuantityCase = quantityCase;
                                            linearSaudaOrderLiftingRequestMappingContext.LiftingRequestDetailId = liftingRequestDetailsId;
                                            linearSaudaOrderLiftingRequestMappingContext.UomId = Convert.ToInt32(UOMId.Id);
                                            linearSaudaOrderLiftingRequestMappingContext.ModifiedBy = userId;
                                            linearSaudaOrderLiftingRequestMappingContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                            _emamiContext.SaveChanges();
                                            dataSynced++;
                                        }
                                    }

                                }
                                else
                                {
                                    errorMessageList.Add(errorMessage);
                                    errorRecordList.Add(doDetails);
                                }
                            }
                        }
                        _emamiContext.SaveChanges();
                        //SaudaOrders status Update
                        foreach (var liftingRequest in doUpdateDtoList)
                        {
                            //var saudaOrdersContext = _emamiContext.SaudaOrders.FirstOrDefault(_ => _.SaudaNumber == liftingRequest.SaudaNumber);
                            var saudaOrdersContext = SaudaOrdersData.FirstOrDefault(_ => _.SaudaNumber == liftingRequest.SaudaNumber);
                            if (saudaOrdersContext != null)
                            {
                                var saudaOrderLiftingRequestMappingContext = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => _.SaudaOrderId == saudaOrdersContext.Id
                                 && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected).ToList();
                                var result = saudaOrderLiftingRequestMappingContext
                                    .GroupBy(l => l.SaudaOrderId)
                                    .Select(cl => new SaudaOrderLiftingRequestMapping
                                    {
                                        SaudaOrderId = cl.First().SaudaOrderId,
                                        LiftingQuantityCase = cl.Sum(c => c.LiftingQuantityCase),
                                    }).FirstOrDefault();
                                if (result != null)
                                {
                                    //var skusContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrdersContext.SkuId);
                                    decimal biddingQuantity = saudaOrdersContext.BidQuantityCase;
                                    var sqlUpdate = "UPDATE SaudaOrders SET StatusId = @StatusId,ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy WHERE Id = @Id";
                                    var parameters = new[]{
                                    new SqlParameter("@StatusId", result.LiftingQuantityCase >= biddingQuantity ? (int)DTO.Enums.Status.Completed : (int)DTO.Enums.Status.Approved),
                                    new SqlParameter("@Id", saudaOrdersContext.Id),
                                    new SqlParameter("@ModifiedDate",  DateHelper.UtcToIndia(DateTime.UtcNow)),
                                    new SqlParameter("@ModifiedBy", userId)
                                    };
                                    _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                }
                            }
                            else
                            {
                                errorMessageList.Add(string.Format(Constants.SaudaNumberIsNotEmpty, liftingRequest.SaudaNumber));
                            }
                        }
                        _emamiContext.SaveChanges();
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList.GroupBy(x => x.DONumber).Select(y => y.First()).ToList();
                    }


                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    }
                    else
                    {
                        sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                    }
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, inputDto, subject);
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = string.Concat(Constants.ServiceErrorMessage, message);
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _logger.Error(message);
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, inputDto, subject);
            }
        }

        public void NewLiftingRequestCreate(UserDto userContext, long userId, SkuDto skuContext, decimal quantityMT, decimal quantityCase, UomDto UOMId, SAPDoUpdateDto doDetails, SaudaOrderDto saudaOrdersContext)
        {
            using (var _emamiContext = new AdaniContext())
            {
                var liftingRequestContext = new LiftingRequest
                {
                    LiftingDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    UserId = userContext.Id,
                    LiftingStatusId = (int)DTO.Enums.LiftingRequestStatus.Inprogress,
                    StatusId = (int)DTO.Enums.Status.Approved,
                    CreatedBy = userId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    IsSAPDataSync = true
                };

                _emamiContext.LiftingRequest.Add(liftingRequestContext);
                _emamiContext.SaveChanges();
                liftingRequestContext.LiftingRequestNumber = liftingRequestContext.Id.ToString();
                int i = 0;
                i = i + 10;

                var liftingReq = new LiftingRequestDetails
                {
                    LiftingRequestId = liftingRequestContext.Id,
                    ItemNo = i.ToString(),
                    SkuId = skuContext.Id,
                    OilTypeId = Convert.ToInt32(skuContext.OilTypeId),
                    LiftingQuantity = quantityMT,
                    LiftingQuantityCase = quantityCase,
                    DeliveryOrderNumber = doDetails.DONumber,
                    UomId = Convert.ToInt32(UOMId.Id),
                    CreatedBy = userId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    Remarks = doDetails.Reason,
                    EnquiryNumber = doDetails.Enquiry
                };
                _emamiContext.LiftingRequestDetails.Add(liftingReq);
                _emamiContext.SaveChanges();

                if (!string.IsNullOrEmpty(doDetails.DONumber))
                {
                    var saudaLimitDto = new SaudaOrderLiftingRequestMapping
                    {
                        DeliveryOrderNumber = doDetails.DONumber,
                        LiftingQuantityCase = quantityCase,
                        LiftingQuantity = quantityMT,
                        SaudaOrderId = saudaOrdersContext.Id,
                        UomId = Convert.ToInt32(UOMId.Id),
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        CreatedBy = userId,
                        LiftingRequestDetailId = liftingReq.Id,
                        StatusId = (int)DTO.Enums.Status.Inprogress
                    };
                    _emamiContext.SaudaOrderLiftingRequestMapping.Add(saudaLimitDto);
                    _emamiContext.SaveChanges();
                }
            }

        }
        public long UpdateLiftingRequestDetails(long liftingRequestDetailId, List<UserDto> UsersData, long verticalId, long userId, SkuDto skuContext, decimal quantityMT, decimal quantityCase, UomDto UOMId, SAPDoUpdateDto doDetails)
        {
            var errorFlag = true;
            using (var _emamiContext = new AdaniContext())
            {
                var liftingRequestDetailContext = _emamiContext.LiftingRequestDetails.AsNoTracking().FirstOrDefault(_ => _.DeliveryOrderNumber == doDetails.DONumber && _.EnquiryNumber == doDetails.Enquiry && _.SkuId == skuContext.Id);
                if (liftingRequestDetailContext == null)
                {
                    var userContext = UsersData.FirstOrDefault(_ => _.Code == doDetails.SoldToParty && _.VerticalId == verticalId);
                    if (userContext == null)
                    {
                        errorFlag = false;
                    }
                    if (errorFlag)
                    {
                        var liftingRequestContext = new LiftingRequest
                        {
                            LiftingDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            UserId = userContext.Id,
                            LiftingStatusId = (int)DTO.Enums.LiftingRequestStatus.Inprogress,
                            StatusId = (int)DTO.Enums.Status.Approved,
                            CreatedBy = userId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            IsSAPDataSync = true
                        };

                        _emamiContext.LiftingRequest.Add(liftingRequestContext);
                        _emamiContext.SaveChanges();
                        liftingRequestContext.LiftingRequestNumber = liftingRequestContext.Id.ToString();

                        int i = 0;
                        i = i + 10;
                        var liftingReq = new LiftingRequestDetails
                        {
                            LiftingRequestId = liftingRequestContext.Id,
                            ItemNo = i.ToString(),
                            SkuId = skuContext.Id,
                            OilTypeId = Convert.ToInt32(skuContext.OilTypeId),
                            LiftingQuantity = quantityMT,
                            LiftingQuantityCase = quantityCase,
                            DeliveryOrderNumber = doDetails.DONumber,
                            UomId = Convert.ToInt32(UOMId.Id),
                            CreatedBy = userId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            Remarks = doDetails.Reason,
                            EnquiryNumber = doDetails.Enquiry
                        };
                        _emamiContext.LiftingRequestDetails.Add(liftingReq);
                        _emamiContext.SaveChanges();
                        liftingRequestDetailId = liftingReq.Id;
                    }
                }
                else
                {
                    var sqlUpdate = "UPDATE LiftingRequestDetails SET LiftingQuantity =@LiftingQuantity, LiftingQuantityCase=@LiftingQuantityCase,  ModifiedDate=@ModifiedDate," +
                    "Remarks=@Remarks,ModifiedBy=@ModifiedBy,EnquiryNumber=@EnquiryNumber WHERE Id = @Id";
                    var parameters = new[]{
                                            new SqlParameter("@LiftingQuantity", quantityMT),
                                            new SqlParameter("@LiftingQuantityCase", quantityCase),
                                            new SqlParameter("@Id", liftingRequestDetailId),
                                            new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow)),
                                            new SqlParameter("@ModifiedBy", userId),
                                            new SqlParameter("@Remarks", doDetails.Reason),
                                            new SqlParameter("@EnquiryNumber", doDetails.Enquiry)

                                            };
                    _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                }
            }
            return liftingRequestDetailId;
        }

        #endregion

        #region Invoice Status Change

        public void InvoiceStatusChange(string decryptedString)
        {
            _methodName = "InvoiceStatusChange";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var errorList = new List<SAPInvoiceStatusDto>();
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var errorMessageList = new List<string>();
            var dataSynced = 0;

            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
            var userId = UtilityHelper.LongTryToParse(jarray[0]["loginUserId"].ToString());

            var syncFolder = jarray[0]["syncFolder"].ToString();
            var subject = jarray[0]["subject"].ToString();
            var folderPath = ConsoleSettings.InboundDirectoryPath(syncFolder);
            var inputDto = _sftpConnectorService.GetSFTPFile(folderPath, syncFolder);
            var invoiceStatusList = !string.IsNullOrEmpty(inputDto.Response.ToString()) ? (List<SAPInvoiceStatusDto>)inputDto.Response : new List<SAPInvoiceStatusDto>();
            subject = string.Concat(subject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = invoiceStatusList.Count;
            try
            {

                using (var _emamiContext = new AdaniContext())
                {
                    if (invoiceStatusList != null && invoiceStatusList.Any())
                    {
                        foreach (var invoice in invoiceStatusList)
                        {
                            var invoiceData = _emamiContext.Invoices.FirstOrDefault(f => f.BillingDocument == invoice.InvoiceNumber);

                            if (invoiceData != null)
                            {
                                //  invoiceData.PaymentStatus = string.IsNullOrEmpty(invoice.PaymentStatus) ? false : true;
                                invoiceData.ModifiedBy = userId;
                                invoiceData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                dataSynced++;
                            }
                            else
                            {
                                errorList.Add(invoice);
                                errorMessageList.Add(string.Format(Constants.InvoiceNumberNotFound, invoice.InvoiceNumber));
                            }
                        }
                        _emamiContext.SaveChanges();
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorList;
                        if (errorMessageList.Any())
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Response = sapDataSyncResultDto;
                            resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                        }
                        else
                        {

                            resultDto.IsSuccess = true;
                            resultDto.SuccessDto.Response = sapDataSyncResultDto;
                            resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                        }
                        sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                        _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, inputDto, subject);
                    }
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = message;
                _logger.Error(message);
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, inputDto, subject);
            }
        }

        #endregion              

        #region Sauda Conversion

        /// <summary>
        /// Method to get sauda details
        /// </summary>       
        /// <returns></returns>
        public void GetSaudaConversionDetails(List<long> SaudaConversionId)
        {
            _methodName = "GetSaudaConversionDetails";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var syncFolder = ConsoleSettings.SaudaConversion;
            var subject = string.Concat(ConsoleSettings.SaudaConversionSubject, " ", ConsoleSettings.AppToSapDataSyncEmailSubject);
            var csvFileName = ConsoleSettings.SaudaConversionCsv;
            var resultDto = new ResultDto();
            var saudaConversionViewDtoList = new HANASaudaConversion();
            var SaudaConversionContext = new List<SaudaConversionSku>();
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var secondaryFreightNotInFlag = true;
            var sbPlainText = new StringBuilder();
            sbPlainText.Append("<p>Dear Admin,<br>The Sales Truck Load Quantity value is not entered in Load Capacity master screen. Kindly check the list of Deport, FreightZones and FreightRoutes and enter the Secondary Freight value.  <br>");
            sbPlainText.Append("<p><br></p><div style='padding-bottom: 50px;'>" +
                "<table text-align=left border=1  width=100% align=center cellpadding=10 style='border-collapse:collapse'>" +
                "<tr><td><b><center>SKU Name</center></b></td><td><b><center>Load Capacity</center></b></td></tr>");
            try
            {
                using (var _emamiContext = new AdaniContext())
                {
                    SaudaConversionContext = (from saudaConversion in _emamiContext.SaudaConversionSkus.AsNoTracking()
                                                  //join saudaConversiondetail in _emamiContext.SaudaConversionSkuDetails.AsNoTracking() on saudaConversion.Id equals saudaConversiondetail.SaudaConversionSkuId
                                              where saudaConversion.IsSAPDataSync == false && SaudaConversionId.Contains(saudaConversion.Id)
                                              select saudaConversion).ToList();

                    if (SaudaConversionContext != null && SaudaConversionContext.Any())
                    {

                        #region Temp Data fetch
                        var userIdList = SaudaConversionContext.Select(s => s.DealerId).Distinct().ToList();
                        var UsersList = _emamiContext.Users.AsNoTracking()
                            .Where(_ => userIdList.Contains(_.Id))
                            .Select(s => new
                            {
                                UserCode = s.Code,
                                UserId = s.Id,
                                //s.FreightZoneId, s.FreightRouteId, 
                                s.Loadability,
                                s.DepotLoadability
                            });

                        var CustomerTruckCapacityContext = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(_ => userIdList.Contains(_.UserId));

                        var depotIdList = SaudaConversionContext.Select(s => s.DepotId).Distinct().ToList();
                        var plantIdList = SaudaConversionContext.Select(s => s.PlantId).Distinct().ToList();
                        if (depotIdList != null)
                        {
                            depotIdList.AddRange(plantIdList);
                        }
                        var todayDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        var secondaryFreightsList = _emamiContext.SecondaryFreights.AsNoTracking()
                            .Where(_ => DbFunctions.TruncateTime(todayDate) >= DbFunctions.TruncateTime(_.ValidFrom) &&
                                (DbFunctions.TruncateTime(todayDate) <= DbFunctions.TruncateTime(_.ValidTo)) && _.IsActive)
                            .Select(s => new { s.DepotId, s.FreightRouteId, s.IsActive, s.FreightZoneId, s.SalesFreight, s.ValidFrom, s.ValidTo, s.Capacity });

                        secondaryFreightsList = secondaryFreightsList.AsNoTracking().Where(_ => depotIdList.Contains(_.DepotId));

                        var primaryFreightsList = _emamiContext.PrimaryFreights.AsNoTracking()
                            .Where(_ => DbFunctions.TruncateTime(todayDate) >= DbFunctions.TruncateTime(_.ValidFrom) &&
                                (DbFunctions.TruncateTime(todayDate) <= DbFunctions.TruncateTime(_.ValidTo)) && _.IsActive)
                            .Select(s => new { s.DepotId, s.PlantId, s.IsActive, s.SalesFreight, s.ValidFrom, s.ValidTo, s.LoadCapacity });

                        var loadCapacityConversionList = _emamiContext.LoadCapacityConversion.AsNoTracking()
                            .Where(_ => DbFunctions.TruncateTime(todayDate) >= DbFunctions.TruncateTime(_.ValidFrom) &&
                                (DbFunctions.TruncateTime(todayDate) <= DbFunctions.TruncateTime(_.ValidTo)) && _.IsActive)
                            .Select(s => new { s.OilTypeId, s.OilType, s.IsActive, s.SkuId, s.ValidFrom, s.ValidTo, s.LoadCapacity, s.ActualLoadQuantity });

                        var saudaConversionSkuIdList = SaudaConversionContext.Select(s => s.Id).Distinct().ToList();
                        var saudaConversionSkuDetailsList = _emamiContext.SaudaConversionSkuDetails.AsNoTracking()
                            .Where(_ => saudaConversionSkuIdList.Contains(_.SaudaConversionSkuId))
                            .Select(s => new
                            {
                                s.ToSkuId,
                                s.ToQuantityInMt,
                                s.ToQuantityInSku,
                                s.SaudaConversionSkuId,
                                s.SaudaConversionUnitAndDifferenceRateDetailsId
                            });

                        var SaudaConversionUnitAndDifferenceRateDetailsIdList = saudaConversionSkuDetailsList.Select(s => s.SaudaConversionUnitAndDifferenceRateDetailsId).Distinct().ToList();
                        var saudaConversionUnitAndDifferenceRateDetails = _emamiContext.SaudaConversionUnitAndDifferenceRateDetails.AsNoTracking()
                            .Where(_ => SaudaConversionUnitAndDifferenceRateDetailsIdList.Contains(_.Id))
                                .Select(s => new
                                {
                                    s.ToSkuId,
                                    s.BasicRate,
                                    s.ToUnit,
                                    s.Id,
                                    s.SaudaConversionUnitAndDifferenceRateId
                                });

                        var SaudaConversionUnitAndDifferenceRateIdList = saudaConversionUnitAndDifferenceRateDetails.Select(s => s.SaudaConversionUnitAndDifferenceRateId).Distinct().ToList();
                        var saudaConversionUnitAndDifferenceRates = _emamiContext.SaudaConversionUnitAndDifferenceRates.AsNoTracking()
                            .Where(_ => SaudaConversionUnitAndDifferenceRateIdList.Contains(_.Id))
                                .Select(s => new
                                {
                                    s.FromSkuId,
                                    s.FromUnit,
                                    s.Id
                                });

                        var deportList = _emamiContext.Depots.AsNoTracking()
                            .Where(_ => depotIdList.Contains(_.Id))
                            .Select(s => new
                            {
                                s.Code,
                                s.Name,
                                s.Id
                            });

                        //var freightRoutesList = _emamiContext.FreightRoutes.AsNoTracking()
                        //    .Select(s => new
                        //    {
                        //        s.Name,
                        //        s.Id
                        //    });

                        //var freightZonesList = _emamiContext.FreightZones.AsNoTracking()
                        //    .Select(s => new
                        //    {
                        //        s.Name,
                        //        s.Id
                        //    });

                        var skuList = _emamiContext.Skus.AsNoTracking()
                            .Select(s => new
                            {
                                s.SkuCode,
                                s.SkuName,
                                s.Id,
                                s.PackGroupId
                            });

                        var plantDepotMappingsList = _emamiContext.PlantDepotMapping.AsNoTracking()
                           .Select(s => new
                           {
                               s.PlantId,
                               s.DepotId,
                               s.Id
                           });
                        #endregion

                        foreach (var saudaConversion in SaudaConversionContext.ToList())
                        {
                            decimal TempFRC1 = 0;
                            // decimal TempLoadability = 0;
                            List<decimal> TempLoadability = new List<decimal>();
                            var ErrorFlag = true;
                            var dealerContext = UsersList.FirstOrDefault(_ => _.UserId == saudaConversion.DealerId);
                            var CustomerTruckCapacities = CustomerTruckCapacityContext.Where(_ => _.UserId == saudaConversion.DealerId);
                            if (saudaConversion.DepotId != 0)
                            {
                                // TempLoadability = dealerContext.DepotLoadability;
                                TempLoadability = CustomerTruckCapacities.Where(_ => (_.StorageTypeId == (int)DTO.Enums.StorageType.Depot) || (_.StorageTypeId == (int)DTO.Enums.StorageType.Rake)).Select(_ => _.TruckCapacity).ToList();
                                var plantMapping = plantDepotMappingsList.Where(_ => _.DepotId == saudaConversion.DepotId).ToList();
                                if (plantMapping != null && plantMapping.Count != 1)
                                {
                                    ErrorFlag = false;
                                }
                            }
                            else
                            {
                                // TempLoadability = dealerContext.Loadability;
                                TempLoadability = CustomerTruckCapacities.Where(_ => (_.StorageTypeId == (int)DTO.Enums.StorageType.Plant)).Select(_ => _.TruckCapacity).ToList();
                            }
                            if (ErrorFlag)
                            {
                                var SecondaryFreightContext = secondaryFreightsList.FirstOrDefault(_ => _.DepotId == saudaConversion.DepotId
                            //&&                            _.FreightZoneId == dealerContext.FreightZoneId && _.FreightRouteId == dealerContext.FreightRouteId 
                            && TempLoadability.Contains(_.Capacity));

                                if (SecondaryFreightContext == null)
                                {
                                    SecondaryFreightContext = secondaryFreightsList.FirstOrDefault(_ => _.DepotId == saudaConversion.PlantId
                                    //&&                                    _.FreightZoneId == dealerContext.FreightZoneId && _.FreightRouteId == dealerContext.FreightRouteId 
                                    && TempLoadability.Contains(_.Capacity));
                                }

                                var primaryFreightContext = primaryFreightsList.FirstOrDefault(_ => _.DepotId == saudaConversion.DepotId && TempLoadability.Contains(_.LoadCapacity));
                                if (primaryFreightContext == null)
                                {
                                    primaryFreightContext = primaryFreightsList.FirstOrDefault(_ => _.PlantId == saudaConversion.PlantId && TempLoadability.Contains(_.LoadCapacity));
                                }
                                if (saudaConversion != null)
                                {
                                    if (SecondaryFreightContext != null)
                                    {
                                        TempFRC1 = SecondaryFreightContext == null ? 0 : SecondaryFreightContext.SalesFreight;
                                        var TempPraimaryFreight = primaryFreightContext == null ? 0 : primaryFreightContext.SalesFreight;
                                        var saudaListValue = saudaConversionViewDtoList.Header.FirstOrDefault(_ => _.SaudaConversionId == saudaConversion.Id);
                                        if (saudaListValue == null)
                                        {
                                            var isLoadCapacityAll = true;
                                            var SaudaConversionDetailContext = saudaConversionSkuDetailsList.Where(_ => _.SaudaConversionSkuId == saudaConversion.Id).ToList();
                                            foreach (var detail in SaudaConversionDetailContext)
                                            {
                                                decimal TempPROO = 0;
                                                decimal TempToUnit = 0;
                                                decimal TempUnitDiffereance = 0;
                                                decimal TempFromUnit = 0;
                                                var fromSku = skuList.FirstOrDefault(_ => _.Id == saudaConversion.SkuId);
                                                var toSku = skuList.FirstOrDefault(_ => _.Id == detail.ToSkuId);
                                                //Load Capacity Validation for SKU and load capacity should be mapped against dealer and for this only the secondary freight should be added
                                                var loadCapacityConversionContext = loadCapacityConversionList.FirstOrDefault(_ => _.SkuId == detail.ToSkuId && SecondaryFreightContext.Capacity == _.LoadCapacity &&
                                                 TempLoadability.Contains(_.LoadCapacity));
                                                if (loadCapacityConversionContext != null & isLoadCapacityAll)
                                                {
                                                    var SaudaConversionUnitAndDifferenceRateDetailContext = saudaConversionUnitAndDifferenceRateDetails.FirstOrDefault(_ => _.Id == detail.SaudaConversionUnitAndDifferenceRateDetailsId);
                                                    if (SaudaConversionUnitAndDifferenceRateDetailContext != null)
                                                    {
                                                        var saudaConversionUnitAndDifferenceRateContext = saudaConversionUnitAndDifferenceRates.FirstOrDefault(_ => _.Id == SaudaConversionUnitAndDifferenceRateDetailContext.SaudaConversionUnitAndDifferenceRateId);
                                                        if (saudaConversionUnitAndDifferenceRateContext != null)
                                                        {
                                                            TempUnitDiffereance = saudaConversionUnitAndDifferenceRateContext.FromUnit /
                                                                SaudaConversionUnitAndDifferenceRateDetailContext.ToUnit;
                                                            TempFromUnit = saudaConversionUnitAndDifferenceRateContext.FromUnit;
                                                        }
                                                        TempPROO = SaudaConversionUnitAndDifferenceRateDetailContext.BasicRate;
                                                        TempToUnit = SaudaConversionUnitAndDifferenceRateDetailContext.ToUnit;
                                                    }
                                                    var deportContext = deportList.FirstOrDefault(_ => _.Id == saudaConversion.PlantId);
                                                    if (deportContext == null)
                                                    {
                                                        deportContext = deportList.FirstOrDefault(_ => _.Id == saudaConversion.DepotId);
                                                    }

                                                    decimal fromQuantity = 0;
                                                    decimal toQuantity = 0;
                                                    var loadQuantity = loadCapacityConversionContext != null ? loadCapacityConversionContext.ActualLoadQuantity : 0;
                                                    if (SaudaConversionDetailContext.Count == 1)
                                                    {
                                                        fromQuantity = Math.Round(saudaConversion.QuantityInSku, 0);
                                                        toQuantity = Math.Round(detail.ToQuantityInSku, 0);
                                                    }
                                                    else
                                                    {
                                                        fromQuantity = Math.Round(detail.ToQuantityInSku / TempUnitDiffereance, 0);
                                                        toQuantity = Math.Round(fromQuantity * TempUnitDiffereance, 0);
                                                    }
                                                    //If it is plant primary freight 
                                                    decimal primaryFreight = 0;
                                                    if (saudaConversion.PlantId != 0)
                                                    {
                                                        primaryFreight = 0;
                                                    }
                                                    else
                                                    {
                                                        primaryFreight = TempPraimaryFreight != 0 && loadQuantity != 0 ? Math.Round(TempPraimaryFreight / loadQuantity, 2) : 0;
                                                    }

                                                    var saudaConversionViewDto = new HANASaudaConversionViewDto()
                                                    {
                                                        SaudaConversionId = saudaConversion.Id,
                                                        //DealerId = saudaConversion.DealerId,
                                                        Dealer = dealerContext.UserCode,
                                                        // PlantId = deportContext != null ? deportContext.Id : 0,
                                                        Plant = deportContext != null ? deportContext.Code : string.Empty,
                                                        // OldSkuId = saudaConversion.SkuId,
                                                        OldMaterialNumber = fromSku != null ? fromSku.SkuCode : string.Empty,
                                                        OldQuantityInCase = fromQuantity,
                                                        // OldQuantityInMT = detail.ToQuantityInMt,
                                                        // NewSkuId = detail.ToSkuId,
                                                        NewMaterialNumber = toSku != null ? toSku.SkuCode : string.Empty,
                                                        NewQuantityInCase = toQuantity,
                                                        //NewQuantityInMT = detail.ToQuantityInMt,
                                                        PROO = TempPROO,
                                                        FRC1 = TempFRC1 != 0 && loadQuantity != 0 ? Math.Round(TempFRC1 / loadQuantity, 2) : 0,
                                                        PrimaryFright = primaryFreight,
                                                        ToUnit = Math.Round(TempUnitDiffereance, 3),
                                                        PackGroup = toSku.PackGroupId == (int)PackGroupType.Premium ? "NOS" : "C/S"
                                                    };
                                                    saudaConversionViewDtoList.Header.Add(saudaConversionViewDto);
                                                    var sqlUpdate = "UPDATE SaudaConversionSkus SET IsSAPDataSync = @IsSAPDataSync , ModifiedDate = @ModifiedDate WHERE Id = @Id";
                                                    var parameters = new[]{
                                                        new SqlParameter("@IsSAPDataSync", true),
                                                        new SqlParameter("@Id", saudaConversion.Id),
                                                        new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow))
                                                    };
                                                    _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                                }
                                                else
                                                {
                                                    var TruckCapacities = string.Join(",", TempLoadability);
                                                    sbPlainText.Append("<tr><td width=30% style='padding: 10px;'>" + toSku.SkuName + "</td>" +
                                                        "<td width=40% style='padding: 10px;'><p>" + TruckCapacities + "</p></td></tr>");
                                                    var remarks = "Load capacity master is not available for " + TruckCapacities + " with " + toSku.SkuName;
                                                    saudaConversionViewDtoList.Header = saudaConversionViewDtoList.Header.Where(sku => sku.SaudaConversionId != saudaConversion.Id).ToList();
                                                    var sqlUpdate = "UPDATE SaudaConversionSkus SET IsSAPDataSync = @IsSAPDataSync,Remarks = @Remarks WHERE Id = @Id";
                                                    var parameters = new[]{
                                                        new SqlParameter("@IsSAPDataSync", false),
                                                        new SqlParameter("@Remarks", remarks),
                                                        new SqlParameter("@Id", saudaConversion.Id)};

                                                    _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);

                                                    secondaryFreightNotInFlag = false;
                                                    isLoadCapacityAll = false;

                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var TruckCapacities = string.Join(",", TempLoadability);
                                        var remarks = "Secondary freight master is not available for " + TruckCapacities;
                                        saudaConversionViewDtoList.Header = saudaConversionViewDtoList.Header.Where(sku => sku.SaudaConversionId != saudaConversion.Id).ToList();
                                        var sqlUpdate = "UPDATE SaudaConversionSkus SET IsSAPDataSync = @IsSAPDataSync,Remarks = @Remarks  WHERE Id = @Id";
                                        var parameters = new[]{
                                                        new SqlParameter("@IsSAPDataSync", false),
                                                        new SqlParameter("@Remarks", remarks),
                                                        new SqlParameter("@Id", saudaConversion.Id)};

                                        _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                    }
                                }

                            }
                            else
                            {
                                var sqlUpdate = "UPDATE SaudaConversionSkus SET IsSAPDataSync = 1,IsNotSyncToSAP = 1,IsApproved = 1 , ModifiedDate = @ModifiedDate WHERE Id = @Id";
                                var parameters = new[]{
                                                new SqlParameter("@IsSAPDataSync", true),
                                                new SqlParameter("@Id", saudaConversion.Id),
                                                new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow))
                                            };
                                _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                            }

                        }
                        sbPlainText.Append("</table></div></p>");
                        _emamiContext.SaveChanges();
                        sapDataSyncResultDto.OutstandingResult.DataRetrieved = saudaConversionViewDtoList.Header.Count;
                        sapDataSyncResultDto.OutstandingResult.DataSynced = saudaConversionViewDtoList.Header.Count;
                        if (!secondaryFreightNotInFlag)
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Message = sbPlainText.ToString();
                            _sftpConnectorService.GetDataAsync(resultDto, ConsoleSettings.SaudaConversion_ValidationMsg, subject, csvFileName);
                        }
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaConversionViewDtoList.Header;
                        if (saudaConversionViewDtoList.Header.IsAny())
                        {
                            var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.SaudaConversionHanaApiUrl, saudaConversionViewDtoList);
                            var status = response.StatusCode;
                            sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                            if (status.ToString() == "Accepted")
                            {
                                resultDto.IsSuccess = true;
                                resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                                resultDto.SuccessDto.Response = sapDataSyncResultDto;
                            }
                            else
                            {
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto.Message = "Sauda Conversion data sent to SAP Failed" + status.ToString();
                                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                            }
                            _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, null, subject);
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaConversionViewDtoList.Header;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _logger.Error(message);
                _sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
            }
        }

        public void SaudaConversionNumberUpdate(HANASaudaConversionDtoList inputdto)
        {
            _methodName = "SaudaConversionNumberUpdate";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputdto)}");
            var resultDto = new ResultDto();
            var errorMessageList = new List<string>();
            var errorRecordList = new List<SAPSaudaConversionDto>();
            var dataSynced = 0;
            long userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var synctype = ConsoleSettings.SaudaConversionSubject;
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var subject = string.Concat(ConsoleSettings.SaudaConversionSubject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            var saudaViewDtoList = inputdto != null && inputdto.SaudaConversionList != null ? inputdto.SaudaConversionList : new List<SAPSaudaConversionDto>();
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = saudaViewDtoList.Count;

            try
            {
                if (saudaViewDtoList != null && saudaViewDtoList.Any())
                {

                    using (var _emamiContext = new AdaniContext())
                    {

                        var saudaConversionSkuList = new List<SaudaConversionSku>();
                        var saudaConversionSkuDetailList = new List<SaudaConversionSkuDetail>();

                        var skuList = _emamiContext.Skus.AsNoTracking()
                            .Select(s => new
                            {
                                s.SkuCode,
                                s.SkuName,
                                s.Id
                            });

                        var saudaConversionSkuSaudaNumberList = saudaViewDtoList.Select(s => s.SaudaNumber).Distinct().ToList();
                        var saudaConversionSkusSaudaNumberList = _emamiContext.SaudaConversionSkus.AsNoTracking()
                            .Where(_ => saudaConversionSkuSaudaNumberList.Contains(_.SaudaNumber))
                            .Select(s => new
                            {
                                s.SaudaNumber,
                                s.Id,
                                s.PlantId,
                                s.DealerId,
                                s.DepotId,
                                s.OilTypeId
                            });


                        var saudaConversionSkuDetailsSaudaNumberList = _emamiContext.SaudaConversionSkuDetails.AsNoTracking()
                            .Where(_ => saudaConversionSkuSaudaNumberList.Contains(_.ToSaudaNumber))
                            .Select(s => new
                            {
                                s.ToSaudaNumber,
                                s.Id,
                                s.SaudaConversionUnitAndDifferenceRateDetailsId,
                                s.SaudaConversionSkuId
                            });

                        var saudaConversionSkuIdList = saudaViewDtoList.Select(s => s.SaudaConversionSkusId).Distinct().ToList();
                        var saudaConversionSkuDetailsList = _emamiContext.SaudaConversionSkuDetails.AsNoTracking()
                            .Where(_ => saudaConversionSkuIdList.Contains(_.SaudaConversionSkuId))
                            .Select(s => new
                            {
                                s.ToSaudaNumber,
                                s.Id,
                                s.SaudaConversionUnitAndDifferenceRateDetailsId,
                                s.SaudaConversionSkuId,
                                s.ToSkuId
                            });

                        foreach (var sauda in saudaViewDtoList)
                        {
                            var errorFlag = true;
                            var errorMessage = string.Empty;
                            if (sauda == null)
                            {
                                errorMessage = Constants.InvalidRequest;
                                errorFlag = false;
                            }
                            if (string.IsNullOrEmpty(sauda.SkuCode))
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.SkuCodeIsEmpty + " App_Id: " + sauda.SaudaConversionSkusId, errorMessage);
                                errorFlag = false;
                            }
                            var sku = skuList.FirstOrDefault(_ => _.SkuCode == sauda.SkuCode);
                            if (sku == null)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.SkuCodeIsEmpty + " Sku Code: " + sauda.SkuCode, errorMessage);
                                errorFlag = false;
                            }
                            if (string.IsNullOrEmpty(sauda.SaudaNumber))
                            {
                                var sqlUpdate = "UPDATE SaudaConversionSkus SET Remarks = @Remarks,IsApproved=@IsApproved,TradeTicketNumber = @TradeTicketNumber ,ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy,SaudaConversionUpdateFromSap = @SaudaConversionUpdateFromSap WHERE Id = @Id";
                                var parameters = new[]{
                                            new SqlParameter("@Remarks", sauda.Remarks),
                                            new SqlParameter("@IsApproved", sauda.Status),
                                            new SqlParameter("@TradeTicketNumber", sauda.TradeTicketNumber),
                                            new SqlParameter("@Id", sauda.SaudaConversionSkusId),
                                            new SqlParameter("@ModifiedDate", currentDate),
                                            new SqlParameter("@ModifiedBy", userId),
                                            new SqlParameter("@SaudaConversionUpdateFromSap", true)
                                            };
                                _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                errorMessage = Constants.BindErrorMessage(Constants.SaudaNumberIsEmpty + " App_Id: " + sauda.SaudaConversionSkusId + "SAP Error : " + sauda.Remarks, errorMessage);
                                errorFlag = false;
                            }
                            if (errorFlag)
                            {
                                var saudaConversionSkuContext = _emamiContext.SaudaConversionSkus.AsNoTracking().FirstOrDefault(_ => _.Id == sauda.SaudaConversionSkusId);
                                if (sauda.SaudaType)
                                {
                                    var saudaConversionUpdateFlag = false;
                                    var quantityMT = _resultService.ConvertCasetoMetricTon(sauda.Quantity, sku.Id);
                                    if (saudaConversionSkuContext != null && saudaConversionSkuContext.SaudaNumber == null)
                                    {
                                        var sqlUpdate = "UPDATE SaudaConversionSkus SET SaudaNumber = @SaudaNumber,BaseRate=@BaseRate,QuantityInMt = @QuantityInMt," +
                                            "QuantityInSku= @QuantityInSku, Remarks = @Remarks,IsApproved=@IsApproved ,TradeTicketNumber = @TradeTicketNumber ," +
                                            "ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy , SaudaConversionUpdateFromSap = @SaudaConversionUpdateFromSap WHERE Id = @Id";
                                        var parameters = new[]{
                                            new SqlParameter("@SaudaNumber", sauda.SaudaNumber),
                                            new SqlParameter("@BaseRate", sauda.BaseRate),
                                            new SqlParameter("@TradeTicketNumber", sauda.TradeTicketNumber),
                                            new SqlParameter("@IsApproved", sauda.Status),
                                            new SqlParameter("@QuantityInMt", quantityMT),
                                            new SqlParameter("@QuantityInSku", sauda.Quantity),
                                            new SqlParameter("@Remarks", sauda.Remarks),
                                            new SqlParameter("@Id", sauda.SaudaConversionSkusId),
                                            new SqlParameter("@ModifiedDate", currentDate),
                                            new SqlParameter("@ModifiedBy", userId),
                                            new SqlParameter("@SaudaConversionUpdateFromSap", true)};
                                        _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                        saudaConversionUpdateFlag = true;
                                    }

                                    var saudaConversionSkuWithSaudaNumber = _emamiContext.SaudaConversionSkus.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == sauda.SaudaNumber && _.Id == sauda.SaudaConversionSkusId);
                                    if (saudaConversionSkuWithSaudaNumber != null)
                                    {
                                        var sqlUpdate = "UPDATE SaudaConversionSkus SET SaudaNumber = @SaudaNumber,BaseRate=@BaseRate,QuantityInMt = @QuantityInMt," +
                                           "QuantityInSku= @QuantityInSku, Remarks = @Remarks,IsApproved=@IsApproved ,TradeTicketNumber = @TradeTicketNumber ," +
                                           "ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy, SaudaConversionUpdateFromSap = @SaudaConversionUpdateFromSap WHERE Id = @Id";
                                        var parameters = new[]{
                                            new SqlParameter("@SaudaNumber", sauda.SaudaNumber),
                                            new SqlParameter("@BaseRate", sauda.BaseRate),
                                            new SqlParameter("@TradeTicketNumber", sauda.TradeTicketNumber),
                                            new SqlParameter("@IsApproved", sauda.Status),
                                            new SqlParameter("@QuantityInMt", quantityMT),
                                            new SqlParameter("@QuantityInSku", sauda.Quantity),
                                            new SqlParameter("@Remarks", sauda.Remarks),
                                            new SqlParameter("@Id", saudaConversionSkuWithSaudaNumber.Id),
                                            new SqlParameter("@ModifiedDate", currentDate),
                                            new SqlParameter("@ModifiedBy", userId),
                                            new SqlParameter("@SaudaConversionUpdateFromSap", true)};
                                        _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                        saudaConversionUpdateFlag = true;
                                    }
                                    if (!saudaConversionUpdateFlag)
                                    {
                                        var saudaConversionContext = saudaConversionSkuList.FirstOrDefault(_ => _.SaudaNumber == sauda.SaudaNumber && _.Id == sauda.SaudaConversionSkusId);
                                        if (saudaConversionContext == null)
                                        {
                                            var saudaConversionSku = new SaudaConversionSku
                                            {
                                                SaudaNumber = sauda.SaudaNumber,
                                                BaseRate = sauda.BaseRate,
                                                OilTypeId = saudaConversionSkuContext.OilTypeId,
                                                PlantId = saudaConversionSkuContext.PlantId,
                                                QuantityInMt = quantityMT,
                                                QuantityInSku = sauda.Quantity,
                                                SaudaConversionSkuHeaderId = saudaConversionSkuContext.Id,
                                                SkuId = sku.Id,
                                                Remarks = sauda.Remarks,
                                                IsSAPDataSync = true,
                                                DealerId = saudaConversionSkuContext.DealerId,
                                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                CreatedBy = userId,
                                                DepotId = saudaConversionSkuContext.DepotId,
                                                TradeTicketNumber = sauda.TradeTicketNumber
                                            };
                                            saudaConversionSkuList.Add(saudaConversionSku);
                                        }
                                    }
                                }
                                else
                                {
                                    var updateFlag = false;
                                    var quantityMT = _resultService.ConvertCasetoMetricTon(sauda.Quantity, sku.Id);
                                    var saudaConversionSkuDetails = saudaConversionSkuDetailsList.AsNoTracking().FirstOrDefault(_ => _.SaudaConversionSkuId == sauda.SaudaConversionSkusId && _.ToSkuId == sku.Id);
                                    if (saudaConversionSkuDetails != null && saudaConversionSkuDetails.ToSaudaNumber == null)
                                    {
                                        var sqlUpdate = "UPDATE SaudaConversionSkuDetails SET ToSaudaNumber = @SaudaNumber,ToBaseRate=@BaseRate, " +
                                           "ToQuantityInMt = @ToQuantityInMt,ToQuantityInSku = @ToQuantityInSku,TradeTicketNumber = @TradeTicketNumber ," +
                                           "ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy , Remarks = @Remarks" +
                                           " WHERE SaudaConversionSkuId = @SaudaConversionSkuId and ToSkuId = @ToSkuId";
                                        var parameters = new[]{
                                            new SqlParameter("@SaudaNumber", sauda.SaudaNumber),
                                            new SqlParameter("@TradeTicketNumber", sauda.TradeTicketNumber),
                                            new SqlParameter("@BaseRate", sauda.BaseRate),
                                            new SqlParameter("@Remarks", sauda.Remarks),
                                            new SqlParameter("@ToQuantityInMt", quantityMT),
                                            new SqlParameter("@ToQuantityInSku", sauda.Quantity),
                                            new SqlParameter("@SaudaConversionSkuId", sauda.SaudaConversionSkusId),
                                            new SqlParameter("@ToSkuId", sku.Id),
                                            new SqlParameter("@ModifiedDate", currentDate),
                                            new SqlParameter("@ModifiedBy", userId),
                                            };
                                        _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                        updateFlag = true;
                                    }

                                    var saudaConversionSkuDetailsWithSaudaNumber = saudaConversionSkuDetailsSaudaNumberList.AsNoTracking().FirstOrDefault(_ => _.ToSaudaNumber == sauda.SaudaNumber && _.SaudaConversionSkuId == sauda.SaudaConversionSkusId);
                                    if (saudaConversionSkuDetailsWithSaudaNumber != null)
                                    {
                                        var sqlUpdate = "UPDATE SaudaConversionSkuDetails SET ToSaudaNumber = @SaudaNumber,ToBaseRate=@BaseRate, " +
                                           "ToQuantityInMt = @ToQuantityInMt,ToQuantityInSku = @ToQuantityInSku,TradeTicketNumber = @TradeTicketNumber ,ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy ,  Remarks = @Remarks" +
                                           " WHERE Id = @Id";
                                        var parameters = new[]{
                                            new SqlParameter("@SaudaNumber", sauda.SaudaNumber),
                                            new SqlParameter("@TradeTicketNumber", sauda.TradeTicketNumber),
                                            new SqlParameter("@BaseRate", sauda.BaseRate),
                                            new SqlParameter("@Remarks", sauda.Remarks),
                                            new SqlParameter("@ToQuantityInMt", quantityMT),
                                            new SqlParameter("@ToQuantityInSku", sauda.Quantity),
                                            new SqlParameter("@SaudaConversionSkuId", sauda.SaudaConversionSkusId),
                                            new SqlParameter("@Id", saudaConversionSkuDetailsWithSaudaNumber.Id),
                                            new SqlParameter("@ModifiedDate", currentDate),
                                            new SqlParameter("@ModifiedBy", userId),
                                           };
                                        _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                        updateFlag = true;
                                    }
                                    if (!updateFlag)
                                    {
                                        var saudaConversionContext = saudaConversionSkuDetailList.FirstOrDefault(_ => _.ToSaudaNumber == sauda.SaudaNumber && _.SaudaConversionSkuId == sauda.SaudaConversionSkusId);
                                        if (saudaConversionContext == null)
                                        {
                                            var saudaConversionSku = new SaudaConversionSkuDetail
                                            {
                                                ToSaudaNumber = sauda.SaudaNumber,
                                                TradeTicketNumber = sauda.TradeTicketNumber,
                                                ToBaseRate = sauda.BaseRate,
                                                ToQuantityInMt = quantityMT,
                                                ToQuantityInSku = sauda.Quantity,
                                                SaudaConversionSkuId = saudaConversionSkuContext.Id,
                                                ToSkuId = sku.Id,
                                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                CreatedBy = userId,
                                                SaudaConversionUnitAndDifferenceRateDetailsId = saudaConversionSkuDetails != null ? saudaConversionSkuDetails.SaudaConversionUnitAndDifferenceRateDetailsId : 0,
                                                Remarks = sauda.Remarks
                                            };
                                            saudaConversionSkuDetailList.Add(saudaConversionSku);
                                        }
                                    }
                                }
                                dataSynced++;

                                if (!sauda.Status)
                                {
                                    errorRecordList.Add(sauda);
                                }
                            }
                            else
                            {
                                errorMessageList.Add(errorMessage);
                                errorRecordList.Add(sauda);
                            }
                        }
                        if (saudaConversionSkuList.Any())
                        {
                            _emamiContext.BulkInsertProxy(saudaConversionSkuList);
                        }
                        if (saudaConversionSkuDetailList.Any())
                        {
                            _emamiContext.BulkInsertProxy(saudaConversionSkuDetailList);
                        }
                        _emamiContext.SaveChanges();
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaViewDtoList;
                        sapDataSyncResultDto.SuccessRecordDetailsResponse = saudaViewDtoList.Except(errorRecordList).ToList();
                    }


                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    }
                    else
                    {
                        if (saudaViewDtoList.Select(_ => _.Status).All(a => a))
                        {
                            resultDto.IsSuccess = true;
                            resultDto.SuccessDto.Response = sapDataSyncResultDto;
                            resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                        }
                        else
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Response = sapDataSyncResultDto;
                            resultDto.ErrorDto.Message = Constants.SapSyncSuccessMessage;
                        }
                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaViewDtoList;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }

        #endregion

        #region Sauda Extension

        /// <summary>
        /// Method to get sauda details
        /// </summary>       
        /// <returns></returns>
        public void GetSaudaExtensionDetails(List<long> SaudaExtensionId, bool IsReprocess)
        {
            _methodName = "GetSaudaExtensionDetails";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var syncFolder = ConsoleSettings.SaudaExtension;
            var subject = string.Concat(ConsoleSettings.SaudaExtensionionSubject, " ", ConsoleSettings.AppToSapDataSyncEmailSubject);
            var csvFileName = ConsoleSettings.SaudaExtensionCsv;
            var resultDto = new ResultDto();
            var saudaExtensionViewDtoList = new HANASaudaExtensionList();
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            try
            {
                var saudaExtensionContext = new List<SaudaExtensionDetailsApproval>();
                using (var _emamiContext = new AdaniContext())
                {
                    var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                    if (IsReprocess)
                    {
                        saudaExtensionContext = (from saudaConversion in _emamiContext.SaudaExtensionDetailsApprovals.AsNoTracking()
                                                 where saudaConversion.IsSAPDataSync == false && SaudaExtensionId.Contains(saudaConversion.Id)
                                                 select saudaConversion).ToList();
                    }
                    else
                    {
                        saudaExtensionContext = (from saudaConversion in _emamiContext.SaudaExtensionDetailsApprovals.AsNoTracking()
                                                 where saudaConversion.IsSAPDataSync == false && DbFunctions.TruncateTime(saudaConversion.CreatedDate) == DbFunctions.TruncateTime(currentDate)
                                                 select saudaConversion).ToList();
                    }

                    if (saudaExtensionContext != null && saudaExtensionContext.Any())
                    {
                        sapDataSyncResultDto.OutstandingResult.DataRetrieved = saudaExtensionContext.Count;
                        sapDataSyncResultDto.OutstandingResult.DataSynced = saudaExtensionContext.Count;
                        foreach (var saudaExtension in saudaExtensionContext.ToList())
                        {
                            saudaExtension.ExtentionDateCount = saudaExtension.ExtentionDateCount.Replace("d", "");
                            saudaExtension.ExtentionDateCount = saudaExtension.ExtentionDateCount.Replace("a", "");
                            saudaExtension.ExtentionDateCount = saudaExtension.ExtentionDateCount.Replace("y", "");
                            saudaExtension.ExtentionDateCount = saudaExtension.ExtentionDateCount.Replace("s", "");
                            var saudaConversionViewDto = new HANASaudaExtensionDetails()
                            {
                                SaudaNumber = saudaExtension.SaudaNumber,
                                ExtensionDate = ConsoleSettings.AddBusinessDays(saudaExtension.SaudaValidTo, Convert.ToInt64(saudaExtension.ExtentionDateCount)),
                                //SaudaExtensionDetailsApprovalId = saudaExtension.Id
                            };
                            saudaExtensionViewDtoList.Header.Add(saudaConversionViewDto);

                            var sqlUpdate = "UPDATE SaudaExtensionDetailsApprovals SET IsSAPDataSync = @IsSAPDataSync , ModifiedDate = @ModifiedDate WHERE Id = @Id";
                            var parameters = new[]{
                                    new SqlParameter("@IsSAPDataSync", true),
                                    new SqlParameter("@Id", saudaExtension.Id),
                                    new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow))
                                    };
                            _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                        }
                        _emamiContext.SaveChanges();
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaExtensionViewDtoList.Header;
                        if (saudaExtensionViewDtoList.Header.IsAny())
                        {
                            var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.SaudaExtensionHanaApiUrl, saudaExtensionViewDtoList);
                            var status = response.StatusCode;
                            sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                            if (status.ToString() == "Accepted")
                            {
                                resultDto.IsSuccess = true;
                                resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                                resultDto.SuccessDto.Response = sapDataSyncResultDto;
                            }
                            else
                            {
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto.Message = "Sauda Extension data sent to SAP Failed" + status.ToString();
                                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                            }
                            _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, null, subject);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaExtensionViewDtoList.Header;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _logger.Error(message);
                _sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
            }
        }

        public void SaudaExtensionUpdate(List<HANASaudaCommonFunctionList> inputdto)
        {
            _methodName = "SaudaExtensionUpdate";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputdto)}");

            var resultDto = new ResultDto();
            var errorMessageList = new List<string>();
            var errorRecordList = new List<HANASaudaCommonFunctionList>();
            var dataSynced = 0;
            long userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var synctype = ConsoleSettings.SaudaExtensionionSubject;
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var subject = string.Concat(ConsoleSettings.SaudaExtensionionSubject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            var saudaViewDtoList = inputdto != null ? inputdto : new List<HANASaudaCommonFunctionList>();
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = saudaViewDtoList.Count;

            try
            {
                if (saudaViewDtoList != null && saudaViewDtoList.Any())
                {

                    using (var _emamiContext = new AdaniContext())
                    {

                        #region Temp Data fetch
                        var saudaNumberList = saudaViewDtoList.Select(s => s.SAP_Document_No).Distinct().ToList();
                        var saudaList = _emamiContext.Sauda.AsNoTracking()
                            .Where(_ => saudaNumberList.Contains(_.SaudaNumber))
                            .Select(s => new { s.SaudaNumber, s.Id, });

                        var saudaExtensionDetailsApprovalIdList = saudaViewDtoList.Select(s => s.SAP_Document_No).Distinct().ToList();
                        var saudaExtensionDetailsApprovalList = _emamiContext.SaudaExtensionDetailsApprovals.AsNoTracking()
                            .Where(_ => saudaExtensionDetailsApprovalIdList.Contains(_.SaudaNumber))
                            .Select(s => new { s.SaudaNumber, s.Id, s.RequestDate, s.SaudaValidTo, s.ExtentionDateCount, s.SaudaOrderId, s.SkuCode });
                        #endregion

                        foreach (var sauda in saudaViewDtoList)
                        {

                            var errorFlag = true;
                            var errorMessage = string.Empty;
                            if (sauda == null)
                            {
                                errorMessage = Constants.InvalidRequest;
                                errorFlag = false;
                            }
                            if (string.IsNullOrEmpty(sauda.SAP_Document_No))
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.SaudaNumberIsEmpty, errorMessage);
                                errorFlag = false;
                            }
                            var saudaContext = saudaList.FirstOrDefault(_ => _.SaudaNumber == sauda.SAP_Document_No);
                            if (saudaContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.SaudaNumberIsNotEmpty + " Sauda Number: " + sauda.SAP_Document_No + " Remarks : " + sauda.Message, errorMessage);
                                //errorMessageList.Add(errorMessage);
                                //errorRecordList.Add(sauda);
                                errorFlag = false;
                            }
                            if (errorFlag)
                            {

                                var sqlUpdate = "UPDATE SaudaExtensionDetailsApprovals SET SAPRemarks = @Remarks,IsApproval=@IsApproval,ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy,SaudaExtensionUpdateFromSap = @SaudaExtensionUpdateFromSap WHERE SaudaNumber = @SaudaNumber";
                                var parameters = new[]{
                                    new SqlParameter("@IsApproval", sauda.Status),
                                    new SqlParameter("@Remarks", sauda.Message),
                                    new SqlParameter("@SaudaNumber", sauda.SAP_Document_No),
                                    new SqlParameter("@ModifiedDate", currentDate),
                                    new SqlParameter("@ModifiedBy", userId),
                                    new SqlParameter("@SaudaExtensionUpdateFromSap", true)};
                                _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);

                                if (sauda.Status)
                                {

                                    var saudaExtensionDetailsApprovalContext = saudaExtensionDetailsApprovalList.FirstOrDefault(_ => _.SaudaNumber == sauda.SAP_Document_No);
                                    if (saudaExtensionDetailsApprovalContext != null && saudaExtensionDetailsApprovalContext.SaudaOrderId != 0)
                                    {
                                        var saudaNumber = saudaExtensionDetailsApprovalContext.SaudaNumber + "_" + saudaExtensionDetailsApprovalContext.SkuCode;
                                        var sudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(x => x.SaudaNumber == saudaNumber);
                                        var saudaUpdate = "UPDATE SaudaOrders SET ValidToDate = @ValidToDate,ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy WHERE Id = @Id";
                                        var saudaParameters = new[]{
                                    new SqlParameter("@ValidToDate", ConsoleSettings.AddBusinessDays(saudaExtensionDetailsApprovalContext.SaudaValidTo,Convert.ToInt64(saudaExtensionDetailsApprovalContext.ExtentionDateCount))),
                                    new SqlParameter("@Id", sudaOrderContext.Id),
                                    new SqlParameter("@ModifiedDate", currentDate),
                                    new SqlParameter("@ModifiedBy", userId)};
                                        _emamiContext.BulkUpdateProxy(saudaUpdate, saudaParameters);
                                    }
                                }
                                else
                                {
                                    errorRecordList.Add(sauda);
                                }
                                dataSynced++;
                            }
                            else
                            {
                                errorMessageList.Add(errorMessage);
                                errorRecordList.Add(sauda);
                            }
                        }
                        _emamiContext.SaveChanges();
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaViewDtoList;
                        sapDataSyncResultDto.SuccessRecordDetailsResponse = saudaViewDtoList.Except(errorRecordList).ToList();
                    }
                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    }
                    else
                    {
                        if (saudaViewDtoList.Select(_ => _.Status).All(a => a))
                        {
                            resultDto.IsSuccess = true;
                            resultDto.SuccessDto.Response = sapDataSyncResultDto;
                            resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                        }
                        else
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Response = sapDataSyncResultDto;
                            resultDto.ErrorDto.Message = Constants.SapSyncSuccessMessage;
                        }

                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
                }
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaViewDtoList;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }
        #endregion

        #region Sku Details

        /// <summary>
        /// Save Sku Details
        /// </summary>
        /// <param name="inputdto"></param>
        public void SaveSkuDetails(HANASAPSku inputdto)
        {
            _methodName = "SaveSkuDetails";
            _logger.Info($"SAP Service Start : {ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var errorMessageList = new List<string>();
            var errorRecordList = new List<HANASAPSkuDto>();
            var dataSynced = 0;
            long userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var synctype = ConsoleSettings.SaudaExtensionionSubject;
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var subject = string.Concat(ConsoleSettings.SkuSubject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            var skuDtoList = inputdto != null && inputdto.SkuList != null ? inputdto.SkuList : new List<HANASAPSkuDto>();
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = skuDtoList.Count;
            try
            {
                if (skuDtoList != null && skuDtoList.Any())
                {
                    var skuList = new List<Sku>();
                    using (var _emamiContext = new AdaniContext())
                    {
                        #region Get Common Datas
                        var verticalContextList = _emamiContext.Divisions.AsNoTracking();
                        var oilTypeContextList = _emamiContext.OilTypes.AsNoTracking();
                        var packTypeContextList = _emamiContext.PackTypes.AsNoTracking();
                        var packGroupsContextList = _emamiContext.OilPackingTypes.AsNoTracking();
                        //var materialTypesContextList = _emamiContext.MaterialTypes.AsNoTracking();
                        var subCategoryContextList = _emamiContext.SubCategory.AsNoTracking();
                        var uomContextList = _emamiContext.Uom.AsNoTracking();
                        #endregion

                        foreach (var sku in skuDtoList)
                        {
                            var errorFlag = true;
                            var errorMessage = string.Concat("Sku Code : ", sku.SkuCode);
                            if (sku == null)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.InvalidRequest, errorMessage);
                                errorFlag = false;
                            }
                            if (string.IsNullOrEmpty(sku.SkuCode))
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.SkuCodeIsEmpty, errorMessage);
                                errorFlag = false;
                            }

                            var verticalContext = verticalContextList.FirstOrDefault(_ => _.Code == sku.VerticalCode);
                            if (verticalContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.VerticalCodeIsEmpty, sku.VerticalCode), errorMessage);
                                errorFlag = false;
                            }
                            var oilTypeContext = oilTypeContextList.FirstOrDefault(_ => _.SAPCode == sku.OilTypeCode);
                            if (oilTypeContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.OilTypeCodeIsEmpty, sku.OilTypeCode), errorMessage);
                                errorFlag = false;
                            }
                            var packTypeContext = packTypeContextList.FirstOrDefault(_ => _.SAPCode == sku.PackTypeCode);
                            if (packTypeContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.PackTypeTypeCodeIsEmpty, sku.PackTypeCode), errorMessage);
                                errorFlag = false;
                            }
                            var packGroupsContext = packGroupsContextList.FirstOrDefault(_ => _.SAPName == sku.PackGroups);
                            if (packGroupsContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.PackGroupTypeCodeIsEmpty, sku.PackTypeCode), errorMessage);
                                errorFlag = false;
                            }
                            //var materialTypesContext = materialTypesContextList.FirstOrDefault(_ => _.Name == sku.MaterialType);
                            //if (materialTypesContext == null)
                            //{
                            //    errorMessage = Constants.BindErrorMessage(string.Format(Constants.MaterialTypesCodeIsEmpty, sku.MaterialType), errorMessage);
                            //    errorFlag = false;
                            //}
                            var subCategoryContext = subCategoryContextList.FirstOrDefault(_ => _.Name == sku.SubCategory);
                            if (subCategoryContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.SubCategoryIsEmpty, sku.SubCategory), errorMessage);
                                errorFlag = false;
                            }
                            var uomContext = uomContextList.FirstOrDefault(_ => _.SAPName == sku.PackSize);
                            if (uomContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.PackSizeIsEmpty, sku.PackSize), errorMessage);
                                errorFlag = false;
                            }

                            if (errorFlag)
                            {
                                var codeExist = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.SkuCode == sku.SkuCode && _.OilTypeId == oilTypeContext.Id && _.DivisionId == verticalContext.Id);
                                if (codeExist == null)
                                {
                                    var skuListCheck = skuList.FirstOrDefault(_ => _.SkuCode == sku.SkuCode && _.OilTypeId == oilTypeContext.Id && _.DivisionId == verticalContext.Id);
                                    if (skuListCheck == null)
                                    {
                                        var skuDto = new Sku
                                        {
                                            SkuCode = sku.SkuCode,
                                            SkuName = sku.MaterialDescription,
                                            OilTypeId = oilTypeContext.Id,
                                            PackTypeId = packTypeContext.Id,
                                            DivisionId = verticalContext.Id,
                                            DivisionGroupId = UtilityHelper.LongTryToParse(sku.VerticalGroupCode),
                                            PackGroupId = packGroupsContext != null ? packGroupsContext.Id : 0,
                                            //MaterialTypeId = materialTypesContext != null ? materialTypesContext.Id : 0,
                                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                            CreatedBy = userId,
                                            IsSAPData = true,
                                            IsSAPDataSyncOrNot = true,
                                            IsActive = false,
                                            SubCategoryId = subCategoryContext.Id,
                                            Quantity = sku.PackSizeQuantity,
                                            UomId = uomContext.Id,
                                            ProcessCost = sku.ProcessCost,
                                            IsBaseSku = sku.IsBaseSku,
                                            IsRequiredToAttachTT = sku.IsRequiredToAttachTT,
                                            PremiumAmount = sku.PremiumAmount,
                                            StorageLocation = sku.StorageLocation
                                        };
                                        if (skuDto != null)
                                        {
                                            skuList.Add(skuDto);
                                        }
                                    }
                                }
                                else
                                {
                                    var sqlUpdate = "UPDATE Skus SET SkuName = @SkuName,OilTypeId = @OilTypeId,PackTypeId = @PackTypeId,PackGroupId=@PackGroupId," +
                                        "ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy, " +
                                        "VerticalGroupId =@VerticalGroupId,MaterialTypeId =@MaterialTypeId," +
                                        "SubCategoryId=@SubCategoryId,Quantity=@Quantity,UomId=@UomId,ProcessCost=@ProcessCost, DocumentType=@DocumentType," +
                                        "IsBaseSku=@IsBaseSku,IsRequiredToAttachTT=@IsRequiredToAttachTT,PremiumAmount=@PremiumAmount,StorageLocation=@StorageLocation " +
                                        "WHERE Id = @Id";
                                    var parameters = new[]{
                                        new SqlParameter("@Id", codeExist.Id),
                                        new SqlParameter("@SkuName", sku.MaterialDescription),
                                        new SqlParameter("@OilTypeId", oilTypeContext != null ? oilTypeContext.Id : codeExist.OilTypeId),
                                        new SqlParameter("@PackTypeId", packTypeContext != null ? packTypeContext.Id:codeExist.PackTypeId),
                                        new SqlParameter("@PackGroupId", packGroupsContext != null ? packGroupsContext.Id : codeExist.PackGroupId),
                                        //new SqlParameter("@MaterialTypeId", materialTypesContext != null ? materialTypesContext.Id : codeExist.MaterialTypeId),
                                        new SqlParameter("@VerticalGroupId", UtilityHelper.LongTryToParse(sku.VerticalGroupCode)),
                                        new SqlParameter("@SubCategoryId", subCategoryContext!= null? subCategoryContext.Id : codeExist.SubCategoryId),
                                        new SqlParameter("@Quantity", sku.PackSizeQuantity),
                                        new SqlParameter("@UomId", subCategoryContext!= null?uomContext.Id: codeExist.UomId),
                                        new SqlParameter("@ProcessCost", sku.ProcessCost),
                                        new SqlParameter("@DocumentType", sku.DocumentType),
                                        new SqlParameter("@IsBaseSku", sku.IsBaseSku),
                                        new SqlParameter("@IsRequiredToAttachTT", sku.IsRequiredToAttachTT),
                                        new SqlParameter("@ModifiedDate", currentDate),
                                        new SqlParameter("@ModifiedBy", userId),
                                        new SqlParameter("@PremiumAmount", sku.PremiumAmount),
                                        new SqlParameter("@StorageLocation", sku.StorageLocation)
                                    };
                                    _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                }
                                dataSynced++;
                            }
                            else
                            {
                                errorMessageList.Add(errorMessage);
                                errorRecordList.Add(sku);
                            }
                        }
                        if (null != skuList && skuList.Any())
                        {
                            _emamiContext.BulkInsertProxy(skuList);
                        }
                        _emamiContext.SaveChanges();
                        var skuUomMappingList = new List<SkuUomMapping>();

                        #region Get Common Datas
                        var skuContextList = _emamiContext.Skus.AsNoTracking();
                        //var uomContextList = _emamiContext.Uom.AsNoTracking();
                        var skuUomMappingContextList = _emamiContext.SkuUomMapping.AsNoTracking();
                        #endregion

                        foreach (var sku in skuDtoList)
                        {
                            var errorFlags = true;
                            var errorMessage = string.Empty;
                            var verticalContext = verticalContextList.FirstOrDefault(_ => _.Code == sku.VerticalCode);
                            var verticalId = verticalContext != null ? verticalContext.Id : 0;

                            var oilTypeContext = oilTypeContextList.FirstOrDefault(_ => _.SAPCode == sku.OilTypeCode);
                            var oilTypeId = oilTypeContext != null ? oilTypeContext.Id : 0;

                            var packTypeContext = packTypeContextList.FirstOrDefault(_ => _.SAPCode == sku.PackTypeCode);
                            var packTypeId = packTypeContext != null ? packTypeContext.Id : 0;
                            var skuContext = skuContextList.FirstOrDefault(_ => _.SkuCode == sku.SkuCode && _.OilTypeId == oilTypeId && _.DivisionId == verticalId && _.PackTypeId == packTypeId);
                            if (skuContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.SkuDetailsIsEmpty, sku.SkuCode), errorMessage);
                                errorFlags = false;
                            }
                            foreach (var skuDetails in sku.SkuConvertionFactor)
                            {
                                var uomContext = uomContextList.FirstOrDefault(_ => _.SAPName == skuDetails.UOM);
                                if (uomContext == null)
                                {
                                    errorMessage = Constants.BindErrorMessage(string.Format(Constants.ConvertionTypeIsEmpty, sku.ConvertionType), errorMessage);
                                    errorFlags = false;
                                }
                                if (null != skuContext && errorFlags)
                                {
                                    var skuUomMappingContext = skuUomMappingContextList.FirstOrDefault(_ => _.SkuId == skuContext.Id && _.UomId == uomContext.Id);
                                    if (skuUomMappingContext == null)
                                    {
                                        var skuUomMappingDto = new SkuUomMapping
                                        {
                                            // ConversionFactor = skuDetails.ConvertionFactor,
                                            SkuId = skuContext.Id,
                                            RelationUomId = (int)DTO.Enums.Uom.Nos,
                                            UomId = uomContext.Id,
                                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                            CreatedBy = userId,
                                        };
                                        if (skuUomMappingDto != null)
                                        {
                                            skuUomMappingList.Add(skuUomMappingDto);
                                        }
                                    }
                                    else
                                    {
                                        var sqlUpdate = "UPDATE SkuUomMappings SET ConversionFactor = @ConversionFactor WHERE Id = @Id";
                                        var parameters = new[]{
                                            new SqlParameter("@ConversionFactor", skuDetails.ConvertionFactor),
                                            new SqlParameter("@Id", skuUomMappingContext.Id)
                                        };
                                        _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(errorMessage))
                                    {
                                        errorMessageList.Add(errorMessage);
                                        errorRecordList.Add(sku);
                                    }
                                }
                            }

                        }
                        if (null != skuUomMappingList && skuUomMappingList.Any())
                        {
                            _emamiContext.BulkInsertProxy(skuUomMappingList);
                        }
                        _emamiContext.SaveChanges();
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                    }

                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    }
                    else
                    {
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;

                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = string.Concat(Constants.ServiceErrorMessage, message);
                _logger.Error(message);
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }
        #endregion

        #region Saud Release APP To SAP

        public ResultDto SaudaReleaseAPPToSAP(List<string> saudaNumbers)
        {
            _methodName = "SaudaReleaseAPPToSAP";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var syncFolder = ConsoleSettings.SaudaExtension;
            var subject = string.Concat(ConsoleSettings.SaudaExtensionionSubject, " ", ConsoleSettings.AppToSapDataSyncEmailSubject);
            var csvFileName = ConsoleSettings.SaudaExtensionCsv;
            var resultDto = new ResultDto();
            //var saudaExtensionViewDtoList = new HANASaudaExtensionList();
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            try
            {
                var saudaExtensionContext = new List<SaudaExtensionDetailsApproval>();
                using (var _emamiContext = new AdaniContext())
                {
                    var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    if (saudaNumbers != null && saudaNumbers.Any())
                    {
                        foreach (var saudaNumber in saudaNumbers)
                        {
                            var sqlUpdate = "UPDATE SaudaOrders SET IsSAPDataSync = @IsSAPDataSync , ModifiedDate = @ModifiedDate WHERE SaudaNumber = @saudaNumber";
                            var parameters = new[]{
                                            new SqlParameter("@IsSAPDataSync", true),
                                            new SqlParameter("@saudaNumber", saudaNumber),
                                            new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow))

                                            };
                            _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                        }
                        _emamiContext.SaveChanges();

                        //sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaExtensionViewDtoList.Header;
                        //if (saudaExtensionViewDtoList.Header.IsAny())
                        //{
                        var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.SaudaReleaseApiUrl, saudaNumbers);
                        var status = response.StatusCode;
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        if (status.ToString() == "Accepted" || status.ToString() == "OK")
                        {
                            resultDto.IsSuccess = true;
                            resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                        }
                        else
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Message = "Sauda Release SAP Failed" + status.ToString();
                        }
                        _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, null, subject);
                        //}
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                //sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaExtensionViewDtoList.Header;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _logger.Error(message);
                _sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
            }
            return resultDto;
        }

        #endregion

        #region Sauda Release SAP To APP

        public void SaudaReleaseSAPToAPP(SaudaReleaseSAPToAPPDto inputDto)
        {
            _methodName = "SaudaReleaseSAPToAPP";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();

            var synctype = ConsoleSettings.SaudaRelease;
            var sapDataSyncResultDto = new SapDataSyncResultDto();
            var subject = string.Concat(ConsoleSettings.SaudaRelease, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            try
            {
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var dataSynced = 0;
                using (var _emamiContext = new AdaniContext())
                {
                    var saudaOrder = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == inputDto.SaudaNumber);

                    var sqlUpdate = "UPDATE SaudaOrders SET StatusId =@StatusId ,ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy WHERE SaudaNumber = @SaudaNumber";
                    var parameters = new[]{
                                    new SqlParameter("@SaudaNumber", inputDto.SaudaNumber),
                                    new SqlParameter("@StatusId", inputDto.SaudaStatusId),
                                    new SqlParameter("@ModifiedDate", currentDate),
                                    new SqlParameter("@ModifiedBy", 1),
                    };
                    _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);

                    //var remark = new Remarks
                    //{
                    //    Description = inputDto.Remarks,
                    //    TableName = "SaudaOrders",
                    //    TableId = saudaOrder.Id,
                    //    CreatedDate = currentDate,
                    //    CreatedBy = 1,
                    //    IsActive = true
                    //};
                    //_emamiContext.Remarks.Add(remark);

                    _emamiContext.SaveChanges();
                    dataSynced++;
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = string.Concat(Constants.ServiceErrorMessage, message);
                _logger.Error(message);
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }

        #endregion

        #region Sauda Conversion APP To SAP

        public ResultDto SaudaConversionAPPToSAP(List<string> saudaNumbers)
        {
            _methodName = "SaudaConversionAPPToSAP";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var syncFolder = ConsoleSettings.SaudaExtension;
            var subject = string.Concat(ConsoleSettings.SaudaExtensionionSubject, " ", ConsoleSettings.AppToSapDataSyncEmailSubject);
            var csvFileName = ConsoleSettings.SaudaExtensionCsv;
            var resultDto = new ResultDto();
            //var saudaExtensionViewDtoList = new HANASaudaExtensionList();
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            try
            {
                var saudaExtensionContext = new List<SaudaExtensionDetailsApproval>();
                using (var _emamiContext = new AdaniContext())
                {
                    var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    if (saudaNumbers != null && saudaNumbers.Any())
                    {
                        foreach (var saudaNumber in saudaNumbers)
                        {
                            var sqlUpdate = "UPDATE SaudaOrders SET IsSAPDataSync = @IsSAPDataSync , ModifiedDate = @ModifiedDate WHERE SaudaNumber = @saudaNumber";
                            var parameters = new[]{
                                new SqlParameter("@IsSAPDataSync", true),
                                new SqlParameter("@saudaNumber", saudaNumber),
                                new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow))
                            };
                            _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                        }
                        _emamiContext.SaveChanges();
                        //sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaExtensionViewDtoList.Header;
                        //if (saudaExtensionViewDtoList.Header.IsAny())
                        //{
                        var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.SaudaConversionApiUrl, saudaNumbers);
                        var status = response.StatusCode;
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        if (status.ToString() == "Accepted" || status.ToString() == "OK")
                        {
                            resultDto.IsSuccess = true;
                            resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                        }
                        else
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Message = "Sauda Conversion SAP Failed" + status.ToString();
                        }
                        _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, null, subject);
                        //}
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                //sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaExtensionViewDtoList.Header;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _logger.Error(message);
                _sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
            }
            return resultDto;
        }

        #endregion

        #region Sauda Conversion SAP To APP

        public void SaudaConversionSAPToAPP(SaudaConversionSAPToAPPDto inputDto)
        {
            _methodName = "SaudaConversionSAPToAPP";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();

            var synctype = ConsoleSettings.SaudaRelease;
            var sapDataSyncResultDto = new SapDataSyncResultDto();
            var subject = string.Concat(ConsoleSettings.SaudaRelease, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            try
            {
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var dataSynced = 0;
                using (var _emamiContext = new AdaniContext())
                {
                    var saudaOrder = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == inputDto.SaudaNumber);

                    var sqlUpdate = "UPDATE SaudaOrders SET StatusId =@StatusId ,ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy WHERE SaudaNumber = @SaudaNumber";
                    var parameters = new[]{
                                    new SqlParameter("@SaudaNumber", inputDto.SaudaNumber),
                                    new SqlParameter("@StatusId", inputDto.SaudaStatusId),
                                    new SqlParameter("@ModifiedDate", currentDate),
                                    new SqlParameter("@ModifiedBy", 1),
                    };
                    _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);

                    //var remark = new Remarks
                    //{
                    //    Description = inputDto.Remarks,
                    //    TableName = "SaudaOrders",
                    //    TableId = saudaOrder.Id,
                    //    CreatedDate = currentDate,
                    //    CreatedBy = 1,
                    //    IsActive = true
                    //};
                    //_emamiContext.Remarks.Add(remark);

                    //_emamiContext.SaveChanges();
                    dataSynced++;
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = string.Concat(Constants.ServiceErrorMessage, message);
                _logger.Error(message);
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }

        #endregion

        #region Saud Extenstion APP To SAP

        public ResultDto SaudaExtenstionAPPToSAP(List<string> saudaNumbers, long extensionDays)
        {
            _methodName = "SaudaExtenstionAPPToSAP";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var syncFolder = ConsoleSettings.SaudaExtension;
            var subject = string.Concat(ConsoleSettings.SaudaExtensionionSubject, " ", ConsoleSettings.AppToSapDataSyncEmailSubject);
            var csvFileName = ConsoleSettings.SaudaExtensionCsv;
            var resultDto = new ResultDto();
            //var saudaExtensionViewDtoList = new HANASaudaExtensionList();
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            try
            {
                using (var _emamiContext = new AdaniContext())
                {
                    var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    if (saudaNumbers != null && saudaNumbers.Any())
                    {
                        var saudaConversionViewDto = new HANASaudaExtensionAPPToSAP()
                        {
                            SaudaNumbers = saudaNumbers,
                            ExtensionDays = extensionDays,
                            //ExtensionDate = ConsoleSettings.AddBusinessDays(saudaExtension.SaudaValidTo,extensionDays),
                        };
                        var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.SaudaExtensionApiUrl, saudaConversionViewDto);
                        var status = response.StatusCode;
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        if (status.ToString() == "Accepted" || status.ToString() == "OK")
                        {
                            resultDto.IsSuccess = true;
                            resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                        }
                        else
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Message = "Sauda Extension data sent to SAP Failed" + status.ToString();
                        }
                        _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, null, subject);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                //sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaExtensionViewDtoList.Header;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _logger.Error(message);
                _sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
            }
            return resultDto;
        }

        #endregion

        public void LiftingRequestInvoicNoUpdate(List<HANASaudaCommonFunctionList> inputdto)
        {
            _methodName = "LiftingRequestInvoicNoUpdate";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputdto)}");
            var resultDto = new ResultDto();
            var errorRecordList = new List<HANASaudaCommonFunctionList>();
            var errorMessageList = new List<string>();
            var dataSynced = 0;
            var synctype = ConsoleSettings.LiftingRequestInquiryNumberUpdateSubject;
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var subject = string.Concat(synctype, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            long userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var liftingRequestList = inputdto != null ? inputdto : new List<HANASaudaCommonFunctionList>();
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = liftingRequestList.Count;
            try
            {
                using (var _emamiContext = new AdaniContext())
                {
                    if (liftingRequestList != null && liftingRequestList.Any())
                    {
                        foreach (var liftingData in liftingRequestList)
                        {

                            if (!string.IsNullOrEmpty(liftingData.SAP_Document_No))
                            {
                                var sqlUpdate = "UPDATE LiftingRequests SET SAPInvoiceNo = @SAPInvoiceNo , " +
                                           "ModifiedBy=@ModifiedBy,ModifiedDate = @ModifiedDate WHERE SapDocumentNo = @SAP_Document_No";
                                var parameters = new[]{
                                        new SqlParameter("@SAP_Document_No", liftingData.SAPRefDoc),
                                        new SqlParameter("@ModifiedBy", userId),
                                        new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow)),
                                        new SqlParameter("@SAPInvoiceNo", liftingData.SAP_Document_No)
                                };
                                _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                dataSynced++;

                                if (!liftingData.Status)
                                {
                                    errorRecordList.Add(liftingData);
                                }
                            }

                        }
                        _emamiContext.SaveChanges();
                    }
                    else
                    {
                        errorMessageList.Add(Constants.InvalidRequest);
                    }

                    sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                    sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                    sapDataSyncResultDto.TotalInputRecordDetailsResponse = liftingRequestList;
                    sapDataSyncResultDto.SuccessRecordDetailsResponse = liftingRequestList.Except(errorRecordList).ToList();
                }

                if (errorMessageList.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Response = sapDataSyncResultDto;
                    resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList)}");
                }
                else
                {
                    if (liftingRequestList.Select(_ => _.Status).All(a => a))
                    {
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = Constants.SapSyncSuccessMessage;
                    }

                }
                sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = liftingRequestList;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                _logger.Error(message);
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }
        public void ChequeInventoryReport(HANAChequeStatusDtoList inputDto)
        {
            _methodName = "ChequeInventoryReport";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputDto)}");
            var resultDto = new ResultDto();
            var errorList = new List<SAPChequeStatusDto>();
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var errorMessageList = new List<string>();
            var dataSynced = 0;
            var userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var synctype = ConsoleSettings.ChequeInventoryReportSubject;
            var chequeStatusDtoList = inputDto != null && inputDto.SAPChequeStatusDtos != null ? inputDto.SAPChequeStatusDtos : new List<SAPChequeStatusDto>();
            var subject = string.Concat(ConsoleSettings.ChequeInventoryReportSubject, ConsoleSettings.SAPToAppDataSyncEmailSubject);
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = chequeStatusDtoList.Count;
            try
            {
                using (var _emamiContext = new AdaniContext())
                {
                    var chequeStatusDto = new List<Adani.Solution.Data.ChequeInventoryDetail>();
                    if (chequeStatusDtoList != null && chequeStatusDtoList.Any())
                    {

                        var dealerCode = chequeStatusDtoList.Select(s => s.DealerCode).Distinct().ToList();
                        var userData = _emamiContext.Users.AsNoTracking()
                            .Where(_ => dealerCode.Contains(_.Code))
                            .Select(s => new { s.Id, s.Code, s.Name }).ToList();

                        foreach (var chequeStatus in chequeStatusDtoList)
                        {
                            var userContext = userData.FirstOrDefault(user => user.Code == chequeStatus.DealerCode);

                            var chequeStatusContext = new Adani.Solution.Data.ChequeInventoryDetail
                            {
                                ControllingArea = chequeStatus.ControllingArea,
                                UserId = userContext != null ? userContext.Id : 0,
                                UserCode = chequeStatus.DealerCode,
                                UserName = chequeStatus.DealerName,
                                ChequeNo = chequeStatus.ChequeNo,
                                NameOfBank = chequeStatus.NameOfBank,
                                BranchName = chequeStatus.BranchName,
                                CreatedBy = userId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ModifiedBy = userId,
                                ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                            };
                            chequeStatusDto.Add(chequeStatusContext);
                            dataSynced++;
                        }
                        var chequeInventoryDetailsDelete = "DELETE FROM ChequeInventoryDetails";
                        var listOfStrings = new List<string>();
                        object[] arrayOfStrings = listOfStrings.ToArray();
                        _emamiContext.BulkUpdateProxy(chequeInventoryDetailsDelete, arrayOfStrings);
                    }
                    if (null != chequeStatusDto && chequeStatusDto.Any())
                    {
                        _emamiContext.BulkInsertProxy(chequeStatusDto);
                    }
                    _emamiContext.SaveChanges();
                    sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                    sapDataSyncResultDto.ErrorDetailsResponse = errorList;
                    sapDataSyncResultDto.TotalInputRecordDetailsResponse = chequeStatusDtoList;
                    sapDataSyncResultDto.SuccessRecordDetailsResponse = chequeStatusDtoList.Except(errorList).ToList();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = sapDataSyncResultDto;
                resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = chequeStatusDtoList;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                _logger.Error(message);
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }
        #endregion

        #region AWL Interface

        #region Sauda

        /// <summary>
        /// Method to get sauda details
        /// </summary>       
        /// <returns></returns>
        public void GetSaudaDetails(List<long> saudaIds, bool IsApproval)
        {
            _methodName = "GetSaudaDetails";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var syncFolder = ConsoleSettings.SaudaFolder;
            var errorMessageList = new List<string>();
            var syncType = ConsoleSettings.SaudaCreationSubjectAppToSap;
            var subject = string.Concat(syncType, " ", ConsoleSettings.AppToSapDataSyncEmailSubject);
            var csvFileName = ConsoleSettings.SaudaHBCCreationCsv;
            var resultDto = new ResultDto();
            var saudaViewDtoList = new HANASaudaViewList();
            var SaudaContext = new List<Sauda>();
            var SaudaListContext = new List<Sauda>();
            var inputDtoJson = string.Empty;
            try
            {
                using (var _emamiContext = new AdaniContext())
                {
                    if (saudaIds != null)
                    {
                        IQueryable<Sauda> SaudaContextBasedOnVerticals;

                        if (IsApproval)
                        {
                            SaudaContextBasedOnVerticals = from saudaOrder in _emamiContext.Sauda.AsNoTracking()
                                                           join so in _emamiContext.SaudaOrders.AsNoTracking() on saudaOrder.Id equals so.SaudaId
                                                           where saudaIds.Contains(saudaOrder.Id) && !so.IsSAPDataSync //&& oilType.DivisionId == VerticalId //&& oilType.VerticalId == (int)DTO.Enums.Vertical.Hbc
                                                           select saudaOrder;
                        }
                        else
                        {
                            SaudaContextBasedOnVerticals = from saudaOrder in _emamiContext.Sauda.AsNoTracking()
                                                               //join oilType in _emamiContext.OilTypes.AsNoTracking() on saudaOrder.OilTypeId equals oilType.Id
                                                           where saudaIds.Contains(saudaOrder.Id)  //&& oilType.DivisionId == VerticalId //&& oilType.VerticalId == (int)DTO.Enums.Vertical.Hbc
                                                           select saudaOrder;
                        }

                        var data = SaudaContextBasedOnVerticals.Distinct().ToList();
                        SaudaContext.AddRange(data);
                    }
                    //else
                    //{
                    //    var SaudaContextBasedOnVerticals = from saudaOrder in _emamiContext.SaudaOrders.AsNoTracking()
                    //                                       join oilType in _emamiContext.OilTypes.AsNoTracking() on saudaOrder.OilTypeId equals oilType.Id
                    //                                       where saudaOrder.IsSapSauda == false && saudaOrder.IsSAPDataSync == false && saudaOrder.StatusId == (int)DTO.Enums.Status.Pending //&& oilType.DivisionId == VerticalId //&& oilType.VerticalId == (int)DTO.Enums.Vertical.Hbc
                    //                                       select saudaOrder;


                    //    SaudaContext.AddRange(SaudaContextBasedOnVerticals);
                    //}

                    //}
                    //else
                    //{
                    //    var SaudaContextHbc = from saudaOrder in _emamiContext.SaudaOrders.AsNoTracking()
                    //                          join oilType in _emamiContext.OilTypes.AsNoTracking() on saudaOrder.OilTypeId equals oilType.Id
                    //                          where saudaOrder.IsSapSauda == false && saudaOrder.IsSAPDataSync == false && saudaOrder.StatusId == (int)DTO.Enums.Status.Pending && oilType.DivisionId == (int)DTO.Enums.Vertical.Hbc//&& oilType.VerticalId == (int)DTO.Enums.Vertical.Hbc
                    //                          select saudaOrder;

                    //    var SaudaContextSpf = from saudaOrder in _emamiContext.SaudaOrders.AsNoTracking()
                    //                          join oilType in _emamiContext.OilTypes.AsNoTracking() on saudaOrder.OilTypeId equals oilType.Id
                    //                          where saudaOrder.IsSapSauda == false && saudaOrder.IsSAPDataSync == false && saudaOrder.StatusId == (int)DTO.Enums.Status.Pending && oilType.DivisionId == (int)DTO.Enums.Vertical.SpecialityFat
                    //                          select saudaOrder;

                    //    var SaudaContextLoose = from saudaOrder in _emamiContext.SaudaOrders.AsNoTracking()
                    //                            join oilType in _emamiContext.OilTypes.AsNoTracking() on saudaOrder.OilTypeId equals oilType.Id
                    //                            where saudaOrder.IsSapSauda == false && saudaOrder.IsSAPDataSync == false && saudaOrder.StatusId == (int)DTO.Enums.Status.Pending && oilType.DivisionId == (int)DTO.Enums.LooseVertical.Loose
                    //                            select saudaOrder;

                    //    SaudaContext.AddRange(SaudaContextHbc);
                    //    SaudaContext.AddRange(SaudaContextSpf);
                    //    SaudaContext.AddRange(SaudaContextLoose);
                    //}
                    if (SaudaContext != null && SaudaContext.Any())
                    {

                        var allSaudaIds = SaudaContext.Select(x => x.Id).Distinct();
                        var _SaudaListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => allSaudaIds.Contains(_.SaudaId)).ToList();
                        SaudaListContext.AddRange(SaudaContext);


                        #region Get Common Datas
                        var customerList = _emamiContext.Users.AsNoTracking();
                        var skuList = _emamiContext.Skus.AsNoTracking();

                        var deliveryPrioritiesList = _emamiContext.DeliveryPriorities.AsNoTracking();
                        var soldToPartyList = _emamiContext.Users.AsNoTracking();
                        var depotList = _emamiContext.Depots.AsNoTracking();

                        var incoTermList = _emamiContext.IncoTerms.AsNoTracking();
                        var pricingList = _emamiContext.Pricing.AsNoTracking();
                        var skuUomMappingList = _emamiContext.SkuUomMapping.AsNoTracking();
                        var salesOrganization = _emamiContext.SalesOrganization.AsNoTracking().ToList();
                        var divisions = _emamiContext.Divisions.AsNoTracking().ToList();
                        var distributionChannel = _emamiContext.DistributionChannel.AsNoTracking().ToList();


                        #endregion
                        _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName} Common Data Completed");
                        foreach (var sauda in SaudaListContext)
                        {
                            var saudaOrders = _SaudaListContext.Where(x => x.SaudaId == sauda.Id).FirstOrDefault();
                            var customerContext = customerList.FirstOrDefault(_ => _.Id == sauda.UserId);
                            var brokerContext = customerList.FirstOrDefault(_ => _.Id == saudaOrders.BrokerId);
                            var incoTerms = incoTermList.FirstOrDefault(_ => _.Id == saudaOrders.Incoterms2);
                            var salesOrganizationContext = salesOrganization.FirstOrDefault(_ => _.Id == saudaOrders.SalesOrganizationId);
                            var divisionsContext = divisions.FirstOrDefault(_ => _.Id == saudaOrders.DivisionId);
                            var distributionChannelContext = distributionChannel.FirstOrDefault(_ => _.Id == saudaOrders.DistributionChannelId);

                            var saudaViewDto = new SaudaCreateDto
                            {
                                TaskIdentification = sauda.SaudaNumber != null && sauda.SaudaNumber != "" ? "U" : "I",
                                DocumentType = /*"ZCOP"*/ divisionsContext != null ? divisionsContext.SalesDocumentType : string.Empty,
                                SalesOrg = salesOrganizationContext != null ? salesOrganizationContext.Code : string.Empty, //SalesOrganisation
                                DistCh = distributionChannelContext != null ? distributionChannelContext.Code : string.Empty, //DistributionChannel
                                Division = divisionsContext != null ? divisionsContext.Code : string.Empty,
                                ValidFrom = saudaOrders.ValidFromDate.ToString("dd.MM.yyyy"),
                                ValidTo = saudaOrders.ValidToDate.ToString("dd.MM.yyyy"),
                                SoldTo = customerContext != null ? customerContext.Code : string.Empty,
                                ShipTo = customerContext != null ? customerContext.Code : string.Empty,
                                BillTo = customerContext != null ? customerContext.Code : string.Empty,
                                Payer = customerContext != null ? customerContext.Code : string.Empty,
                                Broker = saudaOrders.BrokerId > 0 ? brokerContext.Code : ConsoleSettings.DirectBroker,
                                INCO1 = incoTerms.SAPName,
                                INCO2 = incoTerms.Name,
                                PONumber = sauda.Id.ToString() + sauda.BiddingDate.ToString("dd/MM/yyyy"),
                                PODate = sauda.BiddingDate.ToString("dd.MM.yyyy"),
                                ImpigerRequestNo = sauda.Id.ToString(),
                                SAPContractNo = sauda.SaudaNumber != null || sauda.SaudaNumber != "" ? sauda.SaudaNumber : string.Empty,
                            };

                            var _SAPDataItemDataList = new List<SAPDataItemData>();
                            var saudaOrderList = _SaudaListContext.Where(x => x.SaudaId == sauda.Id).ToList();
                            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName} Sauda Table");
                            foreach (var _saudaOrders in saudaOrderList)

                            {
                                var errorMessage = string.Empty;
                                if (_saudaOrders != null)
                                {
                                    var skusContext = skuList.FirstOrDefault(_ => _.Id == _saudaOrders.SkuId);

                                    //var deliveryPrioritiesContext = deliveryPrioritiesList.FirstOrDefault(_ => _.Id == sauda.DeliveryPriorityId);

                                    var saudaListValue = saudaViewDtoList.Header.FirstOrDefault(_ => Convert.ToInt64(_.ImpigerRequestNo) == _saudaOrders.SaudaId);
                                    if (saudaListValue == null)
                                    {
                                        var soldToPartyContext = soldToPartyList.FirstOrDefault(_ => _.Id == _saudaOrders.BrokerId);
                                        var _skusContext = skuList.FirstOrDefault(_ => _.Id == _saudaOrders.SkuId);

                                        var depots = depotList.FirstOrDefault(_ => _.Id == _saudaOrders.PlantId);

                                        var pricingContext = pricingList.FirstOrDefault(_ => _.Id == _saudaOrders.PricingId);

                                        //decimal condTypeRate1 = 0;
                                        //decimal condTypeRate2 = 0;
                                        //decimal condTypeRate3 = 0;
                                        //decimal condTypeRate4 = 0;
                                        //if (pricingContext != null && skusContext != null)
                                        //{
                                        //    var skuUomMappingContext = skuUomMappingList.FirstOrDefault(_ => _.SkuId == saudaOrders.SkuId && _.UomId == (int)DTO.Enums.Uom.MT && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                                        //    if (skuUomMappingContext != null)
                                        //    {
                                        //        //MT to Nos Convertion Calculation for 1 MT to BidQuantity
                                        //        var primaryFrieghtMTCost = pricingContext.PrimaryFrieght;
                                        //        var secondaryFrieghtCost = pricingContext.SecondaryFrieght;
                                        //        var depotCostCost = pricingContext.DepotCost;
                                        //        var detentionCost = pricingContext.DetentionCost;
                                        //        var materialCost = pricingContext.MaterialCost;
                                        //        var processCost = pricingContext.ProcessCost;
                                        //        var packingCost = pricingContext.PackingCost;
                                        //        //var bidPriceForSingleMT = saudaOrders.BidPrice / saudaOrders.BidQuantityCase;

                                        //        var saleRateDto = SaleRateCalculation(saudaOrders.Id);
                                        //        condTypeRate1 = saleRateDto.PR00;
                                        //        condTypeRate2 = saleRateDto.FRC1;

                                        //        condTypeRate3 = materialCost + processCost + packingCost;
                                        //        condTypeRate4 = materialCost + processCost + packingCost;
                                        //    }
                                        //}
                                        decimal biddingQuantity = _saudaOrders.BidQuantityCase;
                                        //if (skusContext.PackGroupId == (int)PackGroupType.BulkPacking)
                                        //{

                                        //}
                                        //var skuUomMapping = skuUomMappingList.FirstOrDefault(_ => _.SkuId == saudaOrders.SkuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                                        //if (skuUomMapping != null)
                                        //{
                                        //    biddingQuantity = saudaOrders.BidQuantityCase * skuUomMapping.ConversionFactor;
                                        //}


                                        //var customerGroupOneContext = new CustomerGroupFive();
                                        //if (customerGroupOne != null && customerContext != null)
                                        //{
                                        //customerGroupOneContext = customerGroupOne.FirstOrDefault(cg => cg.Id == customerContext.CustomerGroupOneId);

                                        //var verticalId = skusContext != null ? skusContext.Division.Id : 0;
                                        //var customerPOType = string.Empty;
                                        //if (verticalId == (int)DTO.Enums.Vertical.SpecialityFat)
                                        //{
                                        //    customerPOType = "COG";
                                        //}
                                        //else if (verticalId == (int)DTO.Enums.Vertical.Hbc)
                                        //{
                                        //    customerPOType = "COA";
                                        //}
                                        //else if (verticalId == (int)DTO.Enums.Vertical.Rasoi)
                                        //{
                                        //    customerPOType = "COC";
                                        //}

                                        string uomName = string.Empty;
                                        var skuuom = _emamiContext.SkuUomMapping.FirstOrDefault(_ => _.SkuId == skusContext.Id);
                                        if (skuuom != null && skuuom.UomId != null && skuuom.UomId > 0)
                                        {
                                            var uom = _emamiContext.Uom.FirstOrDefault(_ => _.Id == skuuom.UomId);
                                            if (uom != null)
                                            {
                                                uomName = uom.SAPName;
                                            }
                                        }

                                        var pricepercae = _saudaOrders.BidPrice / biddingQuantity;
                                        var _SAPDataItemData = new SAPDataItemData
                                        {
                                            Qty = Math.Round(biddingQuantity, 2).ToString("#.##"),
                                            //UOM = skusContext.Uom.SAPName ?? "",
                                            UOM = uomName ?? "",
                                            Material = skusContext.SkuCode ?? "",
                                            StorageLocation = skusContext.StorageLocation,
                                            Plant = depots.Code,
                                            ConditionType = divisionsContext.ZPR4 ? "ZPR4" : "ZPR1",
                                            Amount = pricepercae.ToString("#.###")
                                        };

                                        _SAPDataItemDataList.Add(_SAPDataItemData);
                                        //if (saudaViewDto.VerticalId == (int)DTO.Enums.Vertical.SpecialityFat)
                                        //{
                                        //    var deportCode = string.Empty;
                                        //    if (saudaViewDto.UserDepotMapping == "4000" && depots.IsPlant && (saudaViewDto.Sku == "400001004" || saudaViewDto.Sku == "400001005"))
                                        //    {
                                        //        saudaViewDto.PickingPoint = "130";
                                        //    }
                                        //    else if (saudaViewDto.UserDepotMapping == "4000" && depots.IsPlant)
                                        //    {
                                        //        saudaViewDto.PickingPoint = "200";
                                        //    }
                                        //    else if (!depots.IsPlant)
                                        //    {
                                        //        saudaViewDto.PickingPoint = "130";
                                        //    }
                                        //}

                                        //saudaViewDtoList.Header.Add(saudaViewDto);
                                        var sqlUpdate = "UPDATE SaudaOrders SET IsSAPDataSync = @IsSAPDataSync , ModifiedDate = @ModifiedDate WHERE Id = @Id";
                                        //var parameters = new[]{
                                        //    new SqlParameter("@IsSAPDataSync", true),
                                        //    new SqlParameter("@Id", saudaOrders.Id),
                                        //    new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow))

                                        //    };
                                        //_emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                        using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                        {
                                            var modifiedDate = DateHelper.UtcToIndia(DateTime.Now);
                                            var result = conn.Execute(sqlUpdate, new
                                            {
                                                IsSAPDataSync = true,
                                                ModifiedDate = modifiedDate,
                                                Id = _saudaOrders.Id

                                            });
                                        }
                                        //}
                                        //}
                                    }


                                }
                            }
                            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName} Completed");
                            saudaViewDto.ItemData.AddRange(_SAPDataItemDataList);

                            var json = JsonConvert.SerializeObject(saudaViewDto, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                            // _logger.Info($"Json Input : {JsonConvert.SerializeObject(saudaViewDto)}");
                            saudaViewDtoList.Header.Add(saudaViewDto);
                            var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.SaudaCreationHanaApiUrl, saudaViewDto);
                            var status = response.StatusCode;
                        } //END foreach SaudaContext 

                        _emamiContext.SaveChanges();


                    }
                }
                sapDataSyncResultDto.OutstandingResult.DataRetrieved = SaudaContext.Count;
                sapDataSyncResultDto.OutstandingResult.DataSynced = saudaViewDtoList.Header.Count;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaViewDtoList.Header;
                sapDataSyncResultDto.SuccessRecordDetailsResponse = saudaViewDtoList.Header;
                //if (saudaViewDtoList.Header.IsAny())
                //{
                //    var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.SaudaCreationHanaApiUrl, saudaViewDtoList);
                //    var status = response.StatusCode;
                //    sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                //    if (status.ToString() == "Accepted")
                //    {
                //        resultDto.IsSuccess = true;
                //        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                //        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                //    }
                //    else
                //    {
                //        resultDto.IsSuccess = false;
                //        resultDto.ErrorDto.Message = "Sauda Creation data sent to SAP Failed" + status.ToString();
                //        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                //    }
                //}

                if (errorMessageList.IsAny())
                {
                    resultDto.IsSuccess = false;
                    sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                    resultDto.ErrorDto.Response = sapDataSyncResultDto;
                    resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList)}");
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncType, null, subject);
                }
                else
                {
                    resultDto.IsSuccess = true;
                    sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                    resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                    resultDto.SuccessDto.Response = sapDataSyncResultDto;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncType, null, subject);
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaViewDtoList.Header;
                sapDataSyncResultDto.ErrorDetailsResponse = saudaViewDtoList.Header;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;

                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncType, null, subject);
                //_sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
            }
        }

        public void SendSaudaModificationInfoToSAP(List<long> saudaModificationIds, bool IsApproval)
        {
            _methodName = "SendSaudaModificationInfoToSAP";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var errorMessageList = new List<string>();
            var syncType = ConsoleSettings.SaudaCreationSubjectAppToSap; // Can be updated to a specific modification subject if needed
            var subject = string.Concat("Sauda Modification ", ConsoleSettings.AppToSapDataSyncEmailSubject);
            var resultDto = new ResultDto();
            var saudaViewDtoList = new HANASaudaModificationViewList();
            try
            {
                using (var _emamiContext = new AdaniContext())
                {
                    if (saudaModificationIds != null && saudaModificationIds.Any())
                    {
                        var saudaModifications = _emamiContext.SaudaModifications.AsNoTracking()
                            .Where(sm => saudaModificationIds.Contains(sm.Id))
                            .ToList();

                        if (saudaModifications != null && saudaModifications.Any())
                        {
                            #region Get Common Datas
                            var customerList = _emamiContext.Users.AsNoTracking();
                            var skuList = _emamiContext.Skus.AsNoTracking();
                            var depotList = _emamiContext.Depots.AsNoTracking();
                            var incoTermList = _emamiContext.IncoTerms.AsNoTracking();
                            var skuUomMappingList = _emamiContext.SkuUomMapping.AsNoTracking();
                            var salesOrganization = _emamiContext.SalesOrganization.AsNoTracking().ToList();
                            var divisions = _emamiContext.Divisions.AsNoTracking().ToList();
                            var distributionChannel = _emamiContext.DistributionChannel.AsNoTracking().ToList();
                            #endregion

                            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName} Common Data Completed");

                            foreach (var saudaModification in saudaModifications)
                            {
                                // Get the original Sauda by SaudaNumber
                                var sauda = _emamiContext.Sauda.AsNoTracking()
                                    .Where(s => s.SaudaNumber == saudaModification.SaudaNumber)
                                    .FirstOrDefault();

                                if (sauda == null)
                                {
                                    errorMessageList.Add($"Sauda not found for SaudaModification Id: {saudaModification.Id}");
                                    continue;
                                }

                                // Get SaudaModificationLines
                                var modificationLines = _emamiContext.SaudaModificationLines.AsNoTracking()
                                    .Where(ml => ml.SaudaModificationId == saudaModification.Id)
                                    .ToList();

                                if (modificationLines == null || !modificationLines.Any())
                                {
                                    errorMessageList.Add($"No modification lines found for SaudaModification Id: {saudaModification.Id}");
                                    continue;
                                }

                                var lineIds = modificationLines.Select(ml => ml.Id).ToList();

                                // Get new items (SaudaModificationItems)
                                var newItems = _emamiContext.SaudaModificationItems.AsNoTracking()
                                    .Where(mi => lineIds.Contains(mi.SaudaModificationLineId))
                                    .ToList();

                                if (newItems == null || !newItems.Any())
                                {
                                    errorMessageList.Add($"No modification items found for SaudaModification Id: {saudaModification.Id}");
                                    continue;
                                }

                                // Get old items (SaudaModificationOldItems) to determine which items were modified
                                var oldItems = _emamiContext.SaudaModificationOldItems.AsNoTracking()
                                    .Where(oi => lineIds.Contains(oi.SaudaModificationLineId))
                                    .ToList();

                                // Get original SaudaOrders for this Sauda
                                var saudaOrders = _emamiContext.SaudaOrders.AsNoTracking()
                                    .Where(so => so.SaudaId == sauda.Id)
                                    .ToList();

                                // Get first SaudaOrder for header information (assuming all orders have same header info)
                                var firstSaudaOrder = saudaOrders.FirstOrDefault();
                                if (firstSaudaOrder == null)
                                {
                                    errorMessageList.Add($"No SaudaOrders found for Sauda Id: {sauda.Id}");
                                    continue;
                                }

                                var customerContext = customerList.FirstOrDefault(_ => _.Id == sauda.UserId);
                                var brokerContext = customerList.FirstOrDefault(_ => _.Id == firstSaudaOrder.BrokerId);
                                var incoTerms = incoTermList.FirstOrDefault(_ => _.Id == firstSaudaOrder.Incoterms2);
                                var salesOrganizationContext = salesOrganization.FirstOrDefault(_ => _.Id == firstSaudaOrder.SalesOrganizationId);
                                var divisionsContext = divisions.FirstOrDefault(_ => _.Id == firstSaudaOrder.DivisionId);
                                var distributionChannelContext = distributionChannel.FirstOrDefault(_ => _.Id == firstSaudaOrder.DistributionChannelId);

                                var saudaViewDto = new SaudaModificationCreateDto
                                {
                                    TaskIdentification = "U", // Always "U" for modification as per requirement
                                    DocumentType = divisionsContext != null ? divisionsContext.SalesDocumentType : string.Empty, // As per requirement
                                    SalesOrg = salesOrganizationContext != null ? salesOrganizationContext.Code : string.Empty,
                                    DistCh = distributionChannelContext != null ? distributionChannelContext.Code : string.Empty,
                                    Division = divisionsContext != null ? divisionsContext.Code : string.Empty,
                                    ValidFrom = firstSaudaOrder.ValidFromDate.ToString("dd.MM.yyyy"),
                                    ValidTo = firstSaudaOrder.ValidToDate.ToString("dd.MM.yyyy"),
                                    SoldTo = customerContext != null ? customerContext.Code : string.Empty,
                                    ShipTo = customerContext != null ? customerContext.Code : string.Empty,
                                    BillTo = customerContext != null ? customerContext.Code : string.Empty,
                                    Payer = customerContext != null ? customerContext.Code : string.Empty,
                                    Broker = firstSaudaOrder.BrokerId > 0 && brokerContext != null ? brokerContext.Code : ConsoleSettings.DirectBroker,
                                    INCO1 = incoTerms != null ? incoTerms.SAPName : string.Empty,
                                    INCO2 = incoTerms != null ? incoTerms.Name : string.Empty,
                                    PONumber = sauda.Id.ToString() + sauda.BiddingDate.ToString("dd/MM/yyyy"),
                                    PODate = sauda.BiddingDate.ToString("dd.MM.yyyy"),
                                    ImpigerRequestNo = sauda.Id.ToString(),
                                    SAPContractNo = sauda.SaudaNumber != null && sauda.SaudaNumber != "" ? sauda.SaudaNumber : null
                                };

                                var _SAPDataItemDataList = new List<SAPDataModificationItemData>();

                                // Process each new item
                                foreach (var newItem in newItems)
                                {
                                    var skusContext = skuList.FirstOrDefault(_ => _.Id == newItem.skuId);
                                    if (skusContext == null)
                                    {
                                        errorMessageList.Add($"SKU not found for SaudaModificationItem Id: {newItem.Id}");
                                        continue;
                                    }

                                    // Check if this item exists in old items (modified item)
                                    var matchingOldItem = oldItems.FirstOrDefault(oi => oi.skuId == newItem.skuId);
                                    string itemNumber = string.Empty;

                                    if (matchingOldItem != null)
                                    {
                                        // Item was modified, find the corresponding SaudaOrder to get ItemNumber
                                        var matchingSaudaOrder = saudaOrders.FirstOrDefault(so => so.SkuId == newItem.skuId);
                                        if (matchingSaudaOrder != null && !string.IsNullOrEmpty(matchingSaudaOrder.SaudaNumber))
                                        {
                                            itemNumber = matchingSaudaOrder.SaudaNumber;
                                        }
                                    }
                                    // If matchingOldItem is null, it's a completely new item, so itemNumber remains empty string

                                    // Get depot/plant from the matching SaudaOrder or use a default
                                    var itemSaudaOrder = saudaOrders.FirstOrDefault(so => so.SkuId == newItem.skuId);
                                    var depots = itemSaudaOrder != null 
                                        ? depotList.FirstOrDefault(_ => _.Id == itemSaudaOrder.PlantId)
                                        : depotList.FirstOrDefault(_ => _.Id == firstSaudaOrder.PlantId);

                                    // Get UOM
                                    string uomName = string.Empty;
                                    var skuuom = skuUomMappingList.FirstOrDefault(_ => _.SkuId == newItem.skuId);
                                    if (skuuom != null && skuuom.UomId != null && skuuom.UomId > 0)
                                    {
                                        var uom = _emamiContext.Uom.AsNoTracking().FirstOrDefault(_ => _.Id == skuuom.UomId);
                                        if (uom != null)
                                        {
                                            uomName = uom.SAPName;
                                        }
                                    }

                                    // Calculate amount per case
                                    decimal biddingQuantity = newItem.QuantityInCase;
                                    //decimal pricePerCase = biddingQuantity > 0 ? (newItem.Price / biddingQuantity) : 0;
                                    decimal pricePerCase = newItem.Price - newItem.Discount;


                                    var _SAPDataItemData = new SAPDataModificationItemData
                                    {
                                        ItemNumber = itemNumber,
                                        Qty = Math.Round(biddingQuantity, 2).ToString("0.##"),
                                        UOM = uomName ?? "",
                                        Material = skusContext.SkuCode ?? "",
                                        StorageLocation = skusContext.StorageLocation,
                                        Plant = depots != null ? depots.Code : string.Empty,
                                        ConditionType = divisionsContext != null && divisionsContext.ZPR4 ? "ZPR4" : "ZPR1",
                                        Amount = pricePerCase.ToString("#.###")
                                    };

                                    _SAPDataItemDataList.Add(_SAPDataItemData);
                                }

                                if (_SAPDataItemDataList.Any())
                                {
                                    saudaViewDto.ItemData.AddRange(_SAPDataItemDataList);

                                    var json = JsonConvert.SerializeObject(saudaViewDto, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                                    saudaViewDtoList.Header.Add(saudaViewDto);
                                    var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.SaudaCreationHanaApiUrl, saudaViewDto);
                                    var status = response.StatusCode;

                                    // Update IsSentToSAP flag
                                    var modificationToUpdate = _emamiContext.SaudaModifications.FirstOrDefault(sm => sm.Id == saudaModification.Id);
                                    if (modificationToUpdate != null)
                                    {
                                        modificationToUpdate.IsSentToSAP = true;
                                        modificationToUpdate.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    }
                                }
                            }

                            _emamiContext.SaveChanges();
                        }
                    }
                }

                sapDataSyncResultDto.OutstandingResult.DataRetrieved = saudaModificationIds != null ? saudaModificationIds.Count : 0;
                sapDataSyncResultDto.OutstandingResult.DataSynced = saudaViewDtoList.Header.Count;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaViewDtoList.Header;
                sapDataSyncResultDto.SuccessRecordDetailsResponse = saudaViewDtoList.Header;

                if (errorMessageList.IsAny())
                {
                    resultDto.IsSuccess = false;
                    sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                    resultDto.ErrorDto.Response = sapDataSyncResultDto;
                    resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList)}");
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncType, null, subject);
                }
                else
                {
                    resultDto.IsSuccess = true;
                    sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                    resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                    resultDto.SuccessDto.Response = sapDataSyncResultDto;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncType, null, subject);
                }
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaViewDtoList.Header;
                sapDataSyncResultDto.ErrorDetailsResponse = saudaViewDtoList.Header;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;

                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncType, null, subject);
            }
        }

        public void UpdateSaudaChange(List<HANASaudaCommonFunctionList> inputdto)
        {
            _methodName = "UpdateSaudaChange";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputdto)}");
            var resultDto = new ResultDto();
            var errorRecordList = new List<HANASaudaCommonFunctionList>();
            var errorMessageList = new List<string>();
            var dataSynced = 0;
            var syncType = ConsoleSettings.SaudaUpdateSubject;
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var subject = string.Concat(ConsoleSettings.SaudaUpdateSubject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            long userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var saudaViewDtoList = inputdto != null ? inputdto : new List<HANASaudaCommonFunctionList>();
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = saudaViewDtoList.Count;

            try
            {
                if (saudaViewDtoList != null && saudaViewDtoList.Any())
                {

                    using (var _emamiContext = new AdaniContext())
                    {
                        foreach (var sauda in saudaViewDtoList)
                        {
                            var errorFlag = true;
                            var errorMessage = string.Empty;
                            if (sauda == null)
                            {
                                errorMessage = Constants.InvalidRequest;
                                errorFlag = false;
                            }
                            

                            if (errorFlag)
                            {
                                if (sauda.Status)
                                {
                                    var saudaOrderList = _emamiContext.SaudaOrders.Where(x => x.SaudaId == sauda.Impiger_Request_No).ToList();
                                    if (saudaOrderList != null && saudaOrderList.Any())
                                    {

                                        var user = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == sauda.SAP_Document_No);

                                        var usersDataContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.Id == user.UserId);
                                        if (usersDataContext != null)
                                        {
                                            List<OpenContractRequestDTO> data = new List<OpenContractRequestDTO>();
                                            var inputDto = new OpenContractRequestDTO
                                            {
                                                SoldToParty = usersDataContext.Code
                                            };
                                            data.Add(inputDto);
                                            #region Open Contract Data Delete
                                            var pendingContractsDelete = "DELETE FROM PendingContracts WHERE UserId =" + usersDataContext.Id + " and SalesOrgId =" + user.SalesOrganization.Id + " and DistChnlId =" + user.DistributionChannel.Id + " and DivisionId =" + user.Division.Id;
                                            _logger.Info($"Query Pending Contracts Delete : {pendingContractsDelete}");
                                            using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                            {
                                                var result = conn.Execute(pendingContractsDelete, new
                                                {

                                                });
                                            }
                                            #endregion

                                            ContractOpenBalanceRequest(data, user.SalesOrganization.Code, user.DistributionChannel.Code, user.Division.Code);
                                        }
                                    }
                                }
                                else
                                {

                                    var saudaApprovalJoin = (from approval in _emamiContext.SaudaModificationApprovals
                                                             join mod in _emamiContext.SaudaModifications on approval.SaudaModificationId equals mod.Id
                                                             where mod.SaudaNumber == sauda.Impiger_Request_No.ToString()
                                                             orderby approval.Id descending
                                                             select new { Approval = approval, Modification = mod }).FirstOrDefault();
                                    if (saudaApprovalJoin != null)
                                    {
                                        var now = DateHelper.UtcToIndia(DateTime.UtcNow);

                                        // set the approval status and metadata (use appropriate status id)
                                        saudaApprovalJoin.Approval.StatusId = (int)DTO.Enums.Status.Pending;
                                        saudaApprovalJoin.Approval.ModifiedDate = now;

                                        _emamiContext.SaveChanges();
                                    }
                                    else
                                    {
                                        _logger.Info($"No SaudaModificationApproval found for SaudaNumber={sauda.Impiger_Request_No}");
                                    }

                                    errorRecordList.Add(sauda);
                                }

                                var remark = new Remarks
                                {
                                    Description = sauda.Message,
                                    TableName = "SaudaModifications",
                                    TableId = sauda.Impiger_Request_No,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    CreatedBy = userId,
                                    IsActive = true
                                };
                                _emamiContext.Remarks.Add(remark);
                                dataSynced++;
                            }
                            else
                            {
                                errorMessageList.Add(errorMessage);
                                errorRecordList.Add(sauda);
                            }
                        }
                        _emamiContext.SaveChanges();
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaViewDtoList;
                        sapDataSyncResultDto.SuccessRecordDetailsResponse = saudaViewDtoList.Except(errorRecordList).ToList();
                    }

                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                        _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList)}");
                    }
                    else
                    {
                        if (saudaViewDtoList.Select(a => a.SAP_Document_No).All(s => !string.IsNullOrEmpty(s)))
                        {
                            resultDto.IsSuccess = true;
                            resultDto.SuccessDto.Response = sapDataSyncResultDto;
                            resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                        }
                        else
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Response = sapDataSyncResultDto;
                            resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                        }
                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncType, null, subject);
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaViewDtoList;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncType, null, subject);
            }
        }

        public void UpdateSaudaNumber(List<HANASaudaCommonFunctionList> inputdto)
        {
            _methodName = "UpdateSaudaNumber";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputdto)}");
            var resultDto = new ResultDto();
            var errorRecordList = new List<HANASaudaCommonFunctionList>();
            var errorMessageList = new List<string>();
            var dataSynced = 0;
            var syncType = ConsoleSettings.SaudaNumberUpdateSubject;
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var subject = string.Concat(ConsoleSettings.SaudaNumberUpdateSubject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            long userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var saudaViewDtoList = inputdto != null ? inputdto : new List<HANASaudaCommonFunctionList>();
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = saudaViewDtoList.Count;

            try
            {
                if (saudaViewDtoList != null && saudaViewDtoList.Any())
                {

                    using (var _emamiContext = new AdaniContext())
                    {
                        foreach (var sauda in saudaViewDtoList)
                        {
                            var errorFlag = true;
                            var errorMessage = string.Empty;
                            if (sauda == null)
                            {
                                errorMessage = Constants.InvalidRequest;
                                errorFlag = false;
                            }
                            //if (string.IsNullOrEmpty(sauda.SaudaNumber))
                            //{
                            //    errorMessage = Constants.BindErrorMessage(Constants.SaudaNumberIsEmpty + " App_Id: " + sauda.AppId, errorMessage);
                            //    errorFlag = false;
                            //}

                            if (errorFlag)
                            {
                                if (!string.IsNullOrEmpty(sauda.SAP_Document_No))
                                {
                                    var sqlUpdate = "UPDATE Saudas SET SaudaNumber = @SaudaNumber ,ModifiedBy = @ModifiedBy,ModifiedDate = @ModifiedDate WHERE Id = @Id";
                                    //var parameters = new[]{
                                    //          new SqlParameter("@SaudaNumber", sauda.SAP_Document_No),
                                    //          new SqlParameter("@Id", sauda.Impiger_Request_No),
                                    //          new SqlParameter("@ModifiedBy", userId),
                                    //          new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow))
                                    //          };

                                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                    {
                                        var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                        var result = conn.Execute(sqlUpdate, new
                                        {
                                            SaudaNumber = sauda.SAP_Document_No,
                                            ModifiedBy = userId,
                                            ModifiedDate = modifiedDate,
                                            Id = sauda.Impiger_Request_No
                                        });
                                    }
                                    //_emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                    var skuList = _emamiContext.Skus.AsNoTracking();
                                    var saudaOrderList = _emamiContext.SaudaOrders.Where(x => x.SaudaId == sauda.Impiger_Request_No).ToList();
                                    if (saudaOrderList != null && saudaOrderList.Any())
                                    {
                                        var skuIds = saudaOrderList.Select(s => s.SkuId).Distinct().ToList();
                                        var skusData = _emamiContext.Skus
                                          .Where(_ => skuIds.Contains(_.Id))
                                          .Select(s => new { SkuCode = s.SkuCode, Id = s.Id });
                                        foreach (var item in saudaOrderList)
                                        {
                                            var sku = skusData.FirstOrDefault(_ => _.Id == item.SkuId);
                                            var SaudaNumber = string.Concat(sauda.SAP_Document_No, "_", sku.SkuCode);
                                            var saudaOrderUpdate = "UPDATE SaudaOrders SET ModifiedBy = @ModifiedBy,ModifiedDate = @ModifiedDate, IsSapSaudaNumberUpdateSync = @IsSapSaudaNumberUpdateSync WHERE Id = @Id";
                                            //var saudaOrderparameters = new[]{                                                
                                            //      new SqlParameter("@Id", item.Id),
                                            //      new SqlParameter("@ModifiedBy", userId),
                                            //      new SqlParameter("@IsSapSaudaNumberUpdateSync", true),
                                            //      new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow))
                                            //      };
                                            //_emamiContext.BulkUpdateProxy(saudaOrderUpdate, saudaOrderparameters);
                                            using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                            {

                                                var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                                var result = conn.Execute(saudaOrderUpdate, new
                                                {
                                                    ModifiedBy = userId,
                                                    ModifiedDate = modifiedDate,
                                                    IsSapSaudaNumberUpdateSync = true,
                                                    Id = item.Id
                                                });

                                            }

                                        }

                                        var user = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == sauda.SAP_Document_No);

                                        var usersDataContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.Id == user.UserId);
                                        if (usersDataContext != null)
                                        {
                                            List<OpenContractRequestDTO> data = new List<OpenContractRequestDTO>();
                                            var inputDto = new OpenContractRequestDTO
                                            {
                                                SoldToParty = usersDataContext.Code
                                            };
                                            data.Add(inputDto);
                                            //ContractOpenBalanceRequest(data);
                                            #region Open Contract Data Delete
                                            var pendingContractsDelete = "DELETE FROM PendingContracts WHERE UserId =" + usersDataContext.Id + " and SalesOrgId =" + user.SalesOrganization.Id + " and DistChnlId =" + user.DistributionChannel.Id + " and DivisionId =" + user.Division.Id;
                                            _logger.Info($"Query Pending Contracts Delete : {pendingContractsDelete}");
                                            using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                            {
                                                var result = conn.Execute(pendingContractsDelete, new
                                                {

                                                });
                                            }
                                            #endregion

                                            ContractOpenBalanceRequest(data, user.SalesOrganization.Code, user.DistributionChannel.Code, user.Division.Code);
                                        }
                                    }
                                }
                                else
                                {
                                    var sqlSaudasUpdate = "UPDATE Saudas SET StatusId = @StatusId ,ModifiedBy = @ModifiedBy,ModifiedDate = @ModifiedDate WHERE Id = @Id";
                                    //var parametersSaudas = new[]{
                                    // new SqlParameter("@StatusId", (int)DTO.Enums.Status.Pending),
                                    //  new SqlParameter("@Id", sauda.Impiger_Request_No),
                                    //  new SqlParameter("@ModifiedBy", userId),
                                    //  new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow))
                                    //  };
                                    //_emamiContext.BulkUpdateProxy(sqlSaudasUpdate, parametersSaudas);


                                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                    {

                                        var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                        var result = conn.Execute(sqlSaudasUpdate, new
                                        {
                                            StatusId = (int)DTO.Enums.Status.Pending,
                                            ModifiedBy = userId,
                                            ModifiedDate = modifiedDate,
                                            Id = sauda.Impiger_Request_No
                                        });

                                    }

                                    var saudaOrderList = _emamiContext.SaudaOrders.Where(x => x.SaudaId == sauda.Impiger_Request_No).ToList();
                                    if (saudaOrderList != null && saudaOrderList.Any())
                                    {
                                        using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                        {

                                            foreach (var item in saudaOrderList)
                                            {

                                                var sqlUpdate = "UPDATE SaudaOrders SET ModifiedBy = @ModifiedBy , ModifiedDate = @ModifiedDate,IsSapSaudaNumberUpdateSync = @IsSapSaudaNumberUpdateSync, StatusId = @StatusId WHERE Id = @Id";
                                                //var parameters = new[]{
                                                //  new SqlParameter("@Id", item.Id),
                                                //  new SqlParameter("@ModifiedBy", userId),
                                                //  new SqlParameter("@IsSapSaudaNumberUpdateSync", true),
                                                //  new SqlParameter("@StatusId", (int)DTO.Enums.Status.Pending),
                                                //  new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow))
                                                //  };

                                                //_emamiContext.BulkUpdateProxy(sqlUpdate, parameters);

                                                var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                                var result = conn.Execute(sqlUpdate, new
                                                {
                                                    ModifiedBy = userId,
                                                    ModifiedDate = modifiedDate,
                                                    IsSapSaudaNumberUpdateSync = true,
                                                    StatusId = (int)DTO.Enums.Status.Pending,
                                                    Id = item.Id
                                                });

                                            }



                                        }

                                    }

                                    var saudaapproval = _emamiContext.SaudaApproval.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.SaudaId == sauda.Impiger_Request_No);
                                    saudaapproval.StatusId = (int)DTO.Enums.Status.Pending;
                                    _emamiContext.SaveChanges();

                                    errorRecordList.Add(sauda);
                                }

                                var remark = new Remarks
                                {
                                    Description = sauda.Message,
                                    TableName = "Saudas",
                                    TableId = sauda.Impiger_Request_No,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    CreatedBy = userId,
                                    IsActive = true
                                };
                                _emamiContext.Remarks.Add(remark);
                                dataSynced++;
                            }
                            else
                            {
                                errorMessageList.Add(errorMessage);
                                errorRecordList.Add(sauda);
                            }
                        }
                        _emamiContext.SaveChanges();
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaViewDtoList;
                        sapDataSyncResultDto.SuccessRecordDetailsResponse = saudaViewDtoList.Except(errorRecordList).ToList();
                    }

                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                        _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList)}");
                    }
                    else
                    {
                        if (saudaViewDtoList.Select(a => a.SAP_Document_No).All(s => !string.IsNullOrEmpty(s)))
                        {
                            resultDto.IsSuccess = true;
                            resultDto.SuccessDto.Response = sapDataSyncResultDto;
                            resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                        }
                        else
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Response = sapDataSyncResultDto;
                            resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                        }
                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncType, null, subject);
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaViewDtoList;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncType, null, subject);
            }
        }


        public void SaudaCreate(SaudaCreateSAPToAPPDto SaudaViewList)
        {
            _methodName = "SaudaCreate";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(SaudaViewList)}");
            var resultDto = new ResultDto();
            var errorModelList = new SaudaCreateSAPToAPPDto();
            var errorMessageList = new List<string>();
            var errorMessageList1 = new List<string>();
            var dataSynced = 0;
            var inputdto = new List<SaudaCreateSAPToAPPDto>();
            var synctype = ConsoleSettings.SaudaCreationSubject;
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var subject = string.Concat(ConsoleSettings.SaudaCreationSubject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            long userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var saudaViewDtoList = SaudaViewList != null && SaudaViewList != null ? SaudaViewList : new SaudaCreateSAPToAPPDto();
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = saudaViewDtoList.ItemData.Count > 0 ? saudaViewDtoList.ItemData.Count : 1; // saudaViewDtoList.Count;
            var successList = new List<SaudaCreateSAPToAPPDto>();
            var errorList = new List<SaudaCreateSAPToAPPDto>();

            var successData = new SaudaCreateSAPToAPPDto();
            var errorData = new SaudaCreateSAPToAPPDto();

            inputdto.Add(saudaViewDtoList);
            try
            {
                if (SaudaViewList != null)
                {
                    using (var _emamiContext = new AdaniContext())
                    {

                        #region Get Common Datas


                        var salesOrganizationList = SaudaViewList.SalesOrg;
                        var salesOrganizationData = _emamiContext.SalesOrganization.AsNoTracking()
                           .Where(_ => salesOrganizationList.Contains(_.Code))
                           .Select(s => new { Id = s.Id, Code = s.Code });

                        var distributionChannelList = SaudaViewList.DistCh;
                        var distributionChannelData = _emamiContext.DistributionChannel.AsNoTracking()
                           .Where(_ => distributionChannelList.Contains(_.Code))
                           .Select(s => new { Id = s.Id, Code = s.Code, s.SalesOrganizationId });

                        var divisionList = SaudaViewList.Division;
                        var divisionData = _emamiContext.Divisions.AsNoTracking()
                           .Where(_ => divisionList.Contains(_.Code))
                           .Select(s => new { Id = s.Id, Code = s.Code, s.SalesOrganizationId, s.DistributionChannelId });

                        var salesOrganizationDataContext = salesOrganizationData.FirstOrDefault(_ => _.Code == SaudaViewList.SalesOrg);
                        var salesOrganisationId = salesOrganizationDataContext != null ? salesOrganizationDataContext.Id : 0;

                        var distributionChannelDataContext = distributionChannelData.FirstOrDefault(_ => _.Code == SaudaViewList.DistCh && _.SalesOrganizationId == salesOrganisationId);
                        var distributionChannelId = distributionChannelDataContext != null ? distributionChannelDataContext.Id : 0;

                        var divisionDataContext = divisionData.FirstOrDefault(_ => _.Code == SaudaViewList.Division && _.SalesOrganizationId == salesOrganisationId && _.DistributionChannelId == distributionChannelId);
                        var divisionId = divisionDataContext != null ? divisionDataContext.Id : 0;

                        //var salesOrganisationId = _emamiContext.SalesOrganization.AsNoTracking().Where(_ => _.Code == SaudaViewList.SalesOrg.ToString()).Select(_ => _.Id).FirstOrDefault();
                        //var distributionChannelId = _emamiContext.DistributionChannel.AsNoTracking().Where(_ => _.Code == SaudaViewList.DistCh.ToString() && _.SalesOrganizationId == salesOrganisationId).Select(_ => _.Id).FirstOrDefault();
                        //var divisionId = _emamiContext.Divisions.AsNoTracking().Where(_ => _.Code == SaudaViewList.Division.ToString() && _.SalesOrganizationId == salesOrganisationId && _.DistributionChannelId == distributionChannelId).Select(_ => _.Id).FirstOrDefault();
                        var saudaSoldToParty = "";
                        if (saudaViewDtoList.SoldTo != null)
                        {
                            saudaSoldToParty = saudaViewDtoList.SoldTo.TrimStart('0');
                        }


                        var UsersDataSoldParty = (from s in _emamiContext.Users
                                                  join role in _emamiContext.UserRoles on s.Id equals role.UserId
                                                  join d in _emamiContext.UserDivisionMappings on s.Id equals d.UserId
                                                  where saudaSoldToParty == (s.Code) && role.RoleId != (int)DTO.Enums.Role.ShipToParty
                                                  select new { s.Id, s.Code, s.SaudaBookingTypeId, d.DivisionId, d.SalesOrganizationId, d.DistributionChannelId }).ToList();
                        var saudaShipToParty = "";
                        if (saudaViewDtoList.ShipTo != null)
                        {
                            saudaShipToParty = saudaViewDtoList.ShipTo.TrimStart('0');
                        }


                        var UsersDataShipToParty = (from s in _emamiContext.Users
                                                    join role in _emamiContext.UserRoles on s.Id equals role.UserId
                                                    join d in _emamiContext.UserDivisionMappings on s.Id equals d.UserId
                                                    where saudaShipToParty.Contains(s.ShipToPartyCode) && role.RoleId == (int)DTO.Enums.Role.ShipToParty
                                                    select new
                                                    {
                                                        Id = s.Id,
                                                        Code = s.ShipToPartyCode,
                                                        SaudaBookingTypeId = s.SaudaBookingTypeId,
                                                        TransportModeId = s.TransportModeId,
                                                        //FreightRouteId = s.FreightRouteId,
                                                        d.DivisionId,
                                                        d.SalesOrganizationId,
                                                        d.DistributionChannelId
                                                    }).ToList();

                        var userIds = UsersDataShipToParty.Select(s => s.Id).Distinct().ToList();
                        var UserRolesData = _emamiContext.UserRoles
                            .Where(_ => userIds.Contains(_.UserId))
                            .Select(s => new { UserId = s.UserId, RoleId = s.RoleId });

                        var saudaSkuCode = saudaViewDtoList.ItemData.Select(x => x.Material);

                        var SkusData = _emamiContext.Skus.AsNoTracking()
                            .Where(_ => saudaSkuCode.Contains(_.SkuCode))
                            .Select(s => new
                            {
                                Id = s.Id,
                                OilTypeId = s.OilTypeId,
                                PackGroupId = s.PackGroupId,
                                SkuCode = s.SkuCode,
                                DivisionId = s.DivisionId,
                                s.SalesOrganizationId,
                                s.DistributionChannelId
                            });
                        if (saudaViewDtoList.INCO1 == null)
                        {
                            saudaViewDtoList.INCO1 = "Empty";
                        }
                        var saudaINCO1 = saudaViewDtoList.INCO1;
                        var IncoTermsData = _emamiContext.IncoTerms.AsNoTracking()
                            .Where(_ => saudaINCO1.Contains(_.SAPName))
                            .Select(s => new { Id = s.Id, SAPName = s.SAPName });

                        var saudaUom = saudaViewDtoList.ItemData.Select(x => x.UOM);
                        var UomData = _emamiContext.Uom.AsNoTracking()
                            .Where(_ => saudaUom.Contains(_.SAPName))
                            .Select(s => new { Id = s.Id, SAPName = s.SAPName });

                        var saudaUserDepotMapping = saudaViewDtoList.ItemData.Select(x => x.Plant);
                        var DepotsData = _emamiContext.Depots.AsNoTracking()
                            .Where(_ => saudaUserDepotMapping.Contains(_.Code) && _.IsPlant)
                            .Select(s => new { Id = s.Id, Code = s.Code });

                        var saudaBroker = saudaViewDtoList.Broker.TrimStart('0');
                        var BrokerContextData = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                                 join ur in _emamiContext.UserRoles.AsNoTracking() on ucm.UserId equals ur.UserId
                                                 join u in _emamiContext.Users.AsNoTracking() on ucm.UserId equals u.Id
                                                 where saudaBroker == u.Code && ur.RoleId == (int)DTO.Enums.Role.Broker
                                                 select new
                                                 {
                                                     CustomerId = ucm.CustomerId,
                                                     BrokerId = ucm.UserId,
                                                     BrokerCode = u.Code
                                                 }).ToList();

                        var BdoContextData = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                              join ur in _emamiContext.UserRoles.AsNoTracking() on ucm.UserId equals ur.UserId
                                              join udivm in _emamiContext.UserDivisionMappings.AsNoTracking() on ucm.UserId equals udivm.UserId
                                              where ur.RoleId == (int)DTO.Enums.Role.StateTrader
                                              select new
                                              {
                                                  CustomerId = ucm.CustomerId,
                                                  BdoId = ucm.UserId,
                                                  SalesOrganizationId = udivm.SalesOrganizationId,
                                                  DistributionChannelId = udivm.DistributionChannelId,
                                                  DivisionId = udivm.DivisionId
                                              }).ToList();

                        #endregion

                        var sauda = saudaViewDtoList;


                        var errorFlag = true;
                        var errorMessage = string.Empty;

                        if (sauda == null)
                        {
                            errorMessage = Constants.InvalidRequest;
                            errorFlag = false;
                        }

                        if (string.IsNullOrEmpty(sauda.SAPContractNo))
                        {
                            errorMessage = Constants.BindErrorMessage(Constants.SaudaNumberIsEmpty, errorMessage);
                            errorFlag = false;
                        }

                        if (salesOrganisationId == 0)
                        {
                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.SalesOrganisationIsEmpty, SaudaViewList.SalesOrg), errorMessage);
                            errorFlag = false;
                        }
                        if (distributionChannelId == 0)
                        {
                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.DistributionChannelIsEmpty, SaudaViewList.DistCh), errorMessage);
                            errorFlag = false;
                        }
                        if (divisionId == 0)
                        {
                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.DivisionIsEmpty, SaudaViewList.Division), errorMessage);
                            errorFlag = false;
                        }
                        sauda.SoldTo = sauda.SoldTo.TrimStart('0');
                        var soldToPartyContext = UsersDataSoldParty.FirstOrDefault(_ => _.Code == sauda.SoldTo && _.DivisionId == divisionId && _.DistributionChannelId == distributionChannelId
                        && _.SalesOrganizationId == salesOrganisationId);
                        if (soldToPartyContext == null)
                        {
                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.SoldToPartyIsNotEmpty, sauda.SoldTo), errorMessage);
                            errorFlag = false;
                        }
                        sauda.ShipTo = sauda.ShipTo.TrimStart('0');
                        var shipToPartyContext = UsersDataShipToParty.FirstOrDefault(_ => _.Code == sauda.ShipTo
                        //&& _.DivisionId == divisionId && _.DistributionChannelId == distributionChannelId
                        //&& _.SalesOrganizationId == salesOrganisationId
                        );
                        //if (shipToPartyContext == null)
                        //{
                        //    errorMessage = Constants.BindErrorMessage(string.Format(Constants.ShipToPartyIsNotEmpty, sauda.ShipTo), errorMessage);
                        //    errorFlag = false;
                        //}
                        //else
                        //{
                        //    if (shipToPartyContext.SaudaBookingTypeId == null)
                        //    {
                        //        errorMessage = Constants.BindErrorMessage(string.Format(Constants.SaudaBookingTypeIsEmpty, sauda.ShipTo), errorMessage);
                        //        errorFlag = false;
                        //    }
                        //}

                        var incoTerms = IncoTermsData.FirstOrDefault(_ => _.SAPName == sauda.INCO1);
                        if (incoTerms == null)
                        {
                            var incoTermsCreate = new Data.Entities.IncoTerms
                            {
                                Code = sauda.INCO1,
                                IsActive = false,
                                Name = sauda.INCO1,
                                SAPName = sauda.INCO1,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                CreatedBy = userId
                            };
                            _emamiContext.IncoTerms.Add(incoTermsCreate);
                            _emamiContext.SaveChanges();

                            IncoTermsData = _emamiContext.IncoTerms.AsNoTracking()
                            .Where(_ => saudaINCO1.Contains(_.SAPName))
                            .Select(s => new { Id = s.Id, SAPName = s.SAPName });
                            incoTerms = IncoTermsData.FirstOrDefault(_ => _.SAPName == sauda.INCO1);
                        }
                        sauda.Broker = sauda.Broker.TrimStart('0');
                        var brokerContext = BrokerContextData.FirstOrDefault(_ => _.BrokerCode == sauda.Broker);

                        BdoContextData = BdoContextData.Where(_ => _.SalesOrganizationId == salesOrganisationId && _.DistributionChannelId == distributionChannelId && _.DivisionId == divisionId).ToList();
                        //if (brokerContext == null)
                        //{
                        //    errorMessage = Constants.BindErrorMessage(string.Format(Constants.BrokerIsNotEmpty, sauda.Broker), errorMessage);
                        //    errorFlag = false;
                        //}

                        //var uomContext = _emamiContext.Uom.AsNoTracking().FirstOrDefault(_ => _.SAPName == sauda.Uom);                      

                        if (errorFlag)
                        {
                            //long DealerTypeId = 0;
                            string IncotermsType = string.Empty;
                            long BrokerId = 0;

                            //if (broker != null)
                            //{
                            //DealerTypeId = dealerRole.RoleId == (int)DTO.Enums.Role.Broker ? (int)DealerType.Broker : (int)DealerType.Direct;
                            if (brokerContext != null)
                            {
                                BrokerId = brokerContext.BrokerId;
                            }
                            else
                            {
                                //var BrokerContext = (from ucm in _emamiContext.UserCustomerMapping
                                //                     join ur in _emamiContext.UserRoles on ucm.UserId equals ur.UserId
                                //                     where ur.RoleId == (int)DTO.Enums.Role.Broker
                                //                     && ucm.CustomerId == shipToPartyContext.Id
                                //                     select new
                                //                     {
                                //                         BrokerId = ucm.UserId
                                //                     }).FirstOrDefault();                                                                        

                                var BrokerContext = BrokerContextData.FirstOrDefault(f => f.CustomerId == shipToPartyContext.Id);
                                if (BrokerContext != null)
                                {
                                    BrokerId = BrokerContext.BrokerId;
                                }
                            }

                            //var BdoContext = (from ucm in _emamiContext.UserCustomerMapping
                            //                  join ur in _emamiContext.UserRoles on ucm.UserId equals ur.UserId
                            //                  where ur.RoleId == (int)DTO.Enums.Role.StateTrader
                            //                  && ucm.CustomerId == shipToPartyContext.Id
                            //                  select new
                            //                  {
                            //                      BdoId = ucm.UserId
                            //                  }).FirstOrDefault();

                            var BdoContext = BdoContextData.FirstOrDefault(f => f.CustomerId == soldToPartyContext.Id);
                            if (BdoContext != null)
                            {
                                userId = BdoContext.BdoId;
                            }
                            else
                            {
                                userId = shipToPartyContext.Id;
                            }
                            //}
                            if (shipToPartyContext != null)
                            {
                                var transportModeId = shipToPartyContext.TransportModeId == null ? 0 : UtilityHelper.LongTryToParse(shipToPartyContext.TransportModeId.ToString());

                                if (incoTerms.Id == (int)DTO.Enums.IncoTerms.ExRake || incoTerms.Id == (int)DTO.Enums.IncoTerms.ForRake)
                                {
                                    transportModeId = (int)DTO.Enums.TransportMode.Rake;
                                }
                            }
                            var saudaContext = _emamiContext.Sauda.FirstOrDefault(_ => _.SaudaNumber == sauda.SAPContractNo);

                            //var saudaOrderId = saudaOrdersContext == null ? 0 : saudaOrdersContext.SaudaId;
                            long saudaId = 0;
                            //var saudaContext = _emamiContext.Sauda.FirstOrDefault(_ => _.Id == saudaOrderId);
                            //Nikil Told not send from SAP
                            //var PODate = DateTime.ParseExact(sauda.PODate, "yyyyMMdd", CultureInfo.InvariantCulture);

                            using (DbContextTransaction transaction = _emamiContext.Database.BeginTransaction())
                            {
                                try
                                {
                                    if (saudaContext == null)
                                    {
                                        var divisionContext = _emamiContext.Divisions.FirstOrDefault(_ => _.Code == SaudaViewList.Division);

                                        var saudaCreate = new Sauda
                                        {
                                            UserId = soldToPartyContext.Id,
                                            BiddingDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                            IsSAPDataSync = true,
                                            IsSAPDataSyncApproval = true,
                                            //ContractTypeId = 0,
                                            //DeliveryPriorityId = 0,
                                            //MaximumNumberDeliveries = 0,
                                            //UserDepotMappingId = 0,
                                            SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                            CreatedBy = userId,
                                            IsSapSauda = true,
                                            SaudaNumber = sauda.SAPContractNo,
                                            DistributionChannelId = distributionChannelId,
                                            SalesOrganizationId = salesOrganisationId,
                                            DivisionId = divisionId,
                                            SalesDocumentType = divisionContext != null ? divisionContext.SalesDocumentType : string.Empty,
                                            StatusId = (int)DTO.Enums.Status.Approved,
                                            BdoId = userId
                                        };
                                        _emamiContext.Sauda.Add(saudaCreate);
                                        _emamiContext.SaveChanges();
                                        saudaId = saudaCreate.Id;
                                    }
                                    else
                                    {
                                        saudaId = saudaContext.Id;
                                        saudaContext.UserId = soldToPartyContext.Id;
                                        saudaContext.IsSAPDataSync = true;
                                        saudaContext.IsSAPDataSyncApproval = true;
                                        // saudaContext.ConditionType2 = sauda.ConditionType2;
                                        // saudaContext.ConditionType1 = sauda.ConditionType1;
                                        //saudaContext.ContractTypeId = 0;
                                        //saudaContext.DeliveryPriorityId = 0;
                                        //saudaContext.MaximumNumberDeliveries = 0;
                                        //saudaContext.UserDepotMappingId = 0;
                                        saudaContext.SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess;
                                        saudaContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                        saudaContext.ModifiedBy = userId;
                                        saudaContext.StatusId = (int)DTO.Enums.Status.Approved;
                                        _emamiContext.SaveChanges();
                                    }

                                    var ValidFrom = DateTime.ParseExact(sauda.ValidFrom, "yyyyMMdd", CultureInfo.InvariantCulture);
                                    var ValidTo = DateTime.ParseExact(sauda.ValidTo, "yyyyMMdd", CultureInfo.InvariantCulture);

                                    // int i = 0;
                                    foreach (var saudaorder in sauda.ItemData)
                                    {
                                        var errorFlag1 = true;
                                        errorMessage = string.Empty;
                                        var plant = DepotsData.FirstOrDefault(_ => _.Code == saudaorder.Plant);
                                        var uomContext = UomData.FirstOrDefault(_ => _.SAPName == saudaorder.UOM);
                                        //var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.SkuCode == sauda.Sku && _.VerticalId == verticalId);

                                        saudaorder.ItemNo = saudaorder.ItemNo.TrimStart('0');
                                        // && _.DivisionId == divisionId && _.DistributionChannelId == distributionChannelId && _.SalesOrganizationId == salesOrganisationId

                                        var skuContext = SkusData.FirstOrDefault(_ => _.SalesOrganizationId == salesOrganisationId && _.DistributionChannelId == distributionChannelId && _.DivisionId == divisionId && _.SkuCode == saudaorder.Material);
                                        if (skuContext == null)
                                        {
                                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.SkuDetailsIsEmpty, sauda.SAPContractNo + "-" + saudaorder.Material), errorMessage);
                                            errorFlag = false;
                                            errorFlag1 = false;
                                        }
                                        if (plant == null)
                                        {
                                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.PlantNotExist, sauda.SAPContractNo + "-" + saudaorder.Plant, SaudaViewList.SalesOrg, SaudaViewList.DistCh, SaudaViewList.Division, saudaorder.Qty), errorMessage);
                                            errorFlag = false;
                                            errorFlag1 = false;
                                        }
                                        if (uomContext == null)
                                        {
                                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.UOMCodeIsNotEmpty, sauda.SAPContractNo + "-" + saudaorder.UOM, SaudaViewList.SalesOrg, SaudaViewList.DistCh, SaudaViewList.Division, saudaorder.Qty), errorMessage);
                                            errorFlag = false;
                                            errorFlag1 = false;
                                        }

                                        decimal TpPrice = Convert.ToDecimal(saudaorder.Amount);
                                        decimal Quantity = Convert.ToDecimal(saudaorder.Qty);

                                        if (errorFlag1)
                                        {
                                            var saudaOrdersItemDataContext = _emamiContext.SaudaOrders.FirstOrDefault(_ => _.SaudaNumber == saudaorder.ItemNo && _.SaudaId == saudaId);//&& skuContext.Id == _.SkuId 
                                            var quantityMT = _resultService.ConvertCasetoMetricTon(Quantity, skuContext.Id);
                                            var bidCost = (TpPrice) / Quantity;

                                            long pricingId = 0;
                                            _logger.Info($"Before assignment: PR10GST={saudaorder.PR10GST}, PR10Amount={saudaorder.PR10Amount}, SaudaNumber={saudaorder.ItemNo}");
                                            if (saudaOrdersItemDataContext == null)
                                            {
                                                //Pricing Entry
                                                var pricing = new Pricing
                                                {
                                                    SkuId = skuContext.Id,
                                                    OilTypeId = UtilityHelper.LongTryToParse(skuContext.OilTypeId.ToString()),
                                                    OilPackingTypeId = UtilityHelper.LongTryToParse(skuContext.PackGroupId.ToString()),
                                                    Price = bidCost,
                                                    CreatedBy = userId,
                                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                    ModifiedBy = userId,
                                                    ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                    ValidFrom = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                    ValidTo = DateHelper.UtcToIndia(DateTime.UtcNow)
                                                    //PlantId = plant.PlantId,
                                                    //SalesOrganizationId = pricingLiveContext.SalesOrganizationId,
                                                    //DistributionChannelId = pricingLiveContext.DistributionChannelId,
                                                    //DivisionId = pricingLiveContext.DivisionId,
                                                    //SAPPricingCode = pricingLiveContext.SAPPricingCode,
                                                    //ValidFrom = pricingLiveContext.ValidFrom,
                                                    //ValidTo = pricingLiveContext.ValidTo,
                                                };
                                                //if (pricingDto != null)
                                                //{
                                                //    todayPricingList.Add(pricingDto);
                                                //}

                                                //var pricing = new Pricing
                                                //{
                                                //    SkuId = skuContext.Id,
                                                //    OilTypeId = UtilityHelper.LongTryToParse(skuContext.OilTypeId.ToString()),
                                                //    OilPackingTypeId = UtilityHelper.LongTryToParse(skuContext.PackGroupId.ToString()),
                                                //    ForDepotPrice = forDepotPrice,
                                                //    ForPlantPrice = forPlantPrice,
                                                //    ForRakePrice = forRakePrice,
                                                //    ExDepotPrice = exDepotPrice,
                                                //    ExPlantPrice = exPlantPrice,
                                                //    ExRakePrice = exRakePrice,
                                                //    TransportModeId = transportModeId,
                                                //    SaudaBookingTypeId = UtilityHelper.LongTryToParse(shipToPartyContext.SaudaBookingTypeId.ToString()),
                                                //    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                //    CreatedBy = userId
                                                //};
                                                _emamiContext.Pricing.Add(pricing);
                                                _emamiContext.SaveChanges();
                                                pricingId = pricing.Id;

                                                // i = i + 10;
                                                var saudaOrderCreate = new SaudaOrder
                                                {
                                                    SaudaId = saudaId,
                                                    SkuId = skuContext.Id,
                                                    OilTypeId = (long)skuContext.OilTypeId,
                                                    BidQuantity = quantityMT,
                                                    BidQuantityCase = Quantity,
                                                    QuotedPrice = TpPrice,
                                                    BidPrice = TpPrice,
                                                    //TradeTicketNumber = sauda.TradeTicketNumber,
                                                    //SaudaNumber = saudaorder.ItemNo,
                                                    SaudaNumber = saudaorder.ItemNo,
                                                    StatusId = (int)DTO.Enums.Status.Approved,
                                                    //SaudaStatusId = (int)DTO.Enums.Status.Pending,
                                                    //CustomerPONumber = sauda.PONumber,
                                                    Incoterms1 = incoTerms.SAPName,
                                                    Incoterms2 = incoTerms.Id,
                                                    PlantId = plant.Id, //plantDepotId,
                                                    BrokerId = BrokerId,
                                                    SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                                                    ValidFromDate = ValidFrom,
                                                    ValidToDate = ValidTo,
                                                    UomId = uomContext.Id,
                                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                    CreatedBy = userId,
                                                    PricingId = pricingId,
                                                    // DealerLocationId = UtilityHelper.LongTryToParse(shipToPartyContext.FreightRouteId.ToString()),
                                                    // Proo = TpPrice,
                                                    // Frc1 = sauda.Rate2,
                                                    IsSAPDataSyncApproval = true,
                                                    IsSAPDataSync = true,
                                                    // DepotIdForRake = 0, //depotIdForrake,
                                                    IsSapSauda = true,
                                                    SalesOrganizationId = salesOrganisationId,
                                                    DistributionChannelId = distributionChannelId,
                                                    DivisionId = divisionId,
                                                    PRAmount = Convert.ToDecimal(saudaorder.PR10Amount),
                                                    PRGST = Convert.ToDecimal(saudaorder.PR10GST),
                                                    QuotedPriceBeforeSAPDiscount = Convert.ToDecimal(saudaorder.PR10Amount)
                                                };
                                                _emamiContext.SaudaOrders.Add(saudaOrderCreate);
                                                _logger.Info($"After assignment: PR10GST={saudaorder.PR10GST}, PR10Amount={saudaorder.PR10Amount}, SaudaNumber={saudaorder.ItemNo}");


                                            }
                                            else
                                            {

                                                var pricingContext = _emamiContext.Pricing.FirstOrDefault(_ => _.Id == saudaOrdersItemDataContext.PricingId);
                                                if (pricingContext != null)
                                                {
                                                    pricingContext.Price = bidCost;
                                                    pricingContext.SkuId = skuContext.Id;
                                                    pricingContext.ModifiedBy = userId;
                                                    pricingContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                                }
                                                //var bidCost = (sauda.BidAmount + sauda.Rate2) * saudaorder.Qty;

                                                saudaOrdersItemDataContext.SkuId = skuContext.Id;
                                                saudaOrdersItemDataContext.BidQuantity = quantityMT;
                                                saudaOrdersItemDataContext.BidQuantityCase = Quantity;
                                                saudaOrdersItemDataContext.QuotedPrice = TpPrice;
                                                saudaOrdersItemDataContext.BidPrice = TpPrice;
                                                saudaOrdersItemDataContext.StatusId = (int)DTO.Enums.Status.Approved;
                                                // saudaOrdersItemDataContext.SaudaStatusId = (int)DTO.Enums.Status.Pending;

                                                saudaOrdersItemDataContext.UomId = uomContext.Id;
                                                saudaOrdersItemDataContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                                saudaOrdersItemDataContext.ModifiedBy = userId;

                                                saudaOrdersItemDataContext.IsSAPDataSyncApproval = true;
                                                saudaOrdersItemDataContext.IsSAPDataSync = true;
                                              
                                                saudaOrdersItemDataContext.IsSapSauda = true;
                                                saudaOrdersItemDataContext.PRAmount = saudaorder.PR10Amount != null ? Convert.ToDecimal(saudaorder.PR10Amount) : 0;
                                                saudaOrdersItemDataContext.PRGST = saudaorder.PR10GST != null ? Convert.ToDecimal(saudaorder.PR10GST) : 0;
                                            }
                                            _logger.Info($"AfterElse assignment: PR10GST={saudaorder.PR10GST}, PR10Amount={saudaorder.PR10Amount}, SaudaNumber={saudaorder.ItemNo}");
                                            dataSynced++;
                                            successData.ItemData.Add(saudaorder);
                                        }
                                        else
                                        {
                                            errorMessageList1.Add(errorMessage);
                                            errorData.ItemData.Add(saudaorder);
                                            //errorMessageList1.Add(errorMessage + " \"  " + saudaorder.Material + " \"  ");
                                            //errorModelList = (sauda);
                                        }
                                    }

                                    var usersDataContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.Id == soldToPartyContext.Id);
                                    if (usersDataContext != null)
                                    {
                                        var contractOpenRequestRaise = new ContractOpenRequestRaiseDto
                                        {
                                            UserId = usersDataContext.Id,
                                            UserCode = usersDataContext.Code,
                                            SalesOrganizationId = soldToPartyContext.SalesOrganizationId,
                                            DistributionChannelId = soldToPartyContext.DistributionChannelId,
                                            DivisionId = soldToPartyContext.DivisionId,
                                            SalesOrganizationCode = salesOrganizationDataContext.Code,
                                            DistributionChannelCode = distributionChannelDataContext.Code,
                                            DivisionCode = divisionDataContext.Code
                                        };
                                        ContractOpenRequestRaise(contractOpenRequestRaise);
                                    }

                                    if (errorMessageList1.Count == sauda.ItemData.Count)
                                    {
                                        transaction.Rollback();
                                    }
                                    else
                                    {
                                        _emamiContext.SaveChanges();
                                        transaction.Commit();
                                    }
                                }
                                catch (Exception exception)
                                {
                                    transaction.Rollback();
                                    var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                                    resultDto.IsSuccess = false;
                                    _logger.Error("Sauda Create SAP to APP Exception" + message);
                                    sapDataSyncResultDto.TotalInputRecordDetailsResponse = inputdto;
                                    resultDto.ErrorDto.ErrorCode = Constants.Exception;
                                    resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                                    sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    sapDataSyncResultDto.ExceptionMessage = message;
                                    resultDto.ErrorDto.Response = sapDataSyncResultDto;
                                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
                                }
                            }
                        }  //dataSynced++;
                        else
                        {
                            errorMessageList.Add(errorMessage);
                            errorModelList = sauda;
                        }
                        if (successData.ItemData.Count > 0)
                        {
                            successList.Add(new SaudaCreateSAPToAPPDto()
                            {
                                TaskIdentification = saudaViewDtoList.TaskIdentification,
                                DocumentType = saudaViewDtoList.DocumentType,
                                Division = saudaViewDtoList.Division,
                                SalesOrg = saudaViewDtoList.SalesOrg,
                                DistCh = saudaViewDtoList.DistCh,
                                ValidFrom = saudaViewDtoList.ValidFrom,
                                ValidTo = saudaViewDtoList.ValidTo,
                                SoldTo = saudaViewDtoList.SoldTo,
                                ShipTo = saudaViewDtoList.ShipTo,
                                BillTo = saudaViewDtoList.BillTo,
                                Payer = saudaViewDtoList.Payer,
                                Broker = saudaViewDtoList.Broker,
                                INCO1 = saudaViewDtoList.INCO1,
                                INCO2 = saudaViewDtoList.INCO2,
                                PONumber = saudaViewDtoList.PONumber,
                                PODate = saudaViewDtoList.PODate,
                                ImpigerRequestNo = saudaViewDtoList.ImpigerRequestNo,
                                SAPContractNo = saudaViewDtoList.SAPContractNo,
                                ItemData = successData.ItemData
                            });

                        }
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        //sapDataSyncResultDto.ErrorDetailsResponse = errorModelList;
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = inputdto; // saudaViewDtoList;


                        sapDataSyncResultDto.SuccessRecordDetailsResponse = successList;
                    }


                    if (errorMessageList.Any() || errorMessageList1.Any())
                    {
                        resultDto.IsSuccess = false;
                        if (errorData.ItemData.Count > 0)
                        {
                            errorList.Add(new SaudaCreateSAPToAPPDto()
                            {
                                TaskIdentification = saudaViewDtoList.TaskIdentification,
                                DocumentType = saudaViewDtoList.DocumentType,
                                Division = saudaViewDtoList.Division,
                                SalesOrg = saudaViewDtoList.SalesOrg,
                                DistCh = saudaViewDtoList.DistCh,
                                ValidFrom = saudaViewDtoList.ValidFrom,
                                ValidTo = saudaViewDtoList.ValidTo,
                                SoldTo = saudaViewDtoList.SoldTo,
                                ShipTo = saudaViewDtoList.ShipTo,
                                BillTo = saudaViewDtoList.BillTo,
                                Payer = saudaViewDtoList.Payer,
                                Broker = saudaViewDtoList.Broker,
                                INCO1 = saudaViewDtoList.INCO1,
                                INCO2 = saudaViewDtoList.INCO2,
                                PONumber = saudaViewDtoList.PONumber,
                                PODate = saudaViewDtoList.PODate,
                                ImpigerRequestNo = saudaViewDtoList.ImpigerRequestNo,
                                SAPContractNo = saudaViewDtoList.SAPContractNo,
                                ItemData = errorData.ItemData
                            });
                        }
                        else
                        {
                            errorList.Add(errorModelList);
                        }

                        sapDataSyncResultDto.ErrorDetailsResponse = errorList;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList1);
                        resultDto.ErrorDto.Message = resultDto.ErrorDto.Message + " " + string.Join(",", errorMessageList);
                        _logger.Info($"Error Message : {resultDto.ErrorDto.Message}");
                    }
                    else
                    {
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;

                        //if (errorMessageList1.Any())
                        //{
                        //    resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        //    resultDto.ErrorDto.Message = string.Join(",", errorMessageList1);
                        //    _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList1)}");
                        //}
                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                _logger.Error("Sauda Create SAP to APP Exception" + message);
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = inputdto;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }

        #endregion       

        #region Pricing Details SAP To APP

        /// <summary>
        /// Save Pricing Details
        /// </summary>
        /// <param name="inputdto"></param>
        public void SavePricingDetails(HANAPricing inputdto)
        {
            _methodName = "SavePricingDetails";
            _logger.Info($"SAP Service Start : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputdto)}");
            var resultDto = new ResultDto();
            var errorMessageList = new List<string>();
            var errorRecordList = new List<SapPricingDTO>();
            var dataSynced = 0;
            long userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var synctype = ConsoleSettings.PricingSubject;
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var subject = string.Concat(synctype, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            var pricingDtoList = inputdto != null && inputdto.PriceControl_Details != null ? inputdto.PriceControl_Details : new List<SapPricingDTO>();
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = pricingDtoList.Count;
            try
            {

                if (pricingDtoList != null && pricingDtoList.Any())
                {
                    var todayPricingList = new List<TodayPricing>();
                    using (var _emamiContext = new AdaniContext())
                    {
                        var skuCodes = pricingDtoList.Select(_ => _.Material).Distinct().ToList();
                        var plantCodes = pricingDtoList.Select(_ => _.FromPlant).Distinct().ToList();
                        var distributionChannels = pricingDtoList.Select(_ => _.Distributor_Channel).Distinct().ToList();
                        var divisions = pricingDtoList.Select(_ => _.Division).Distinct().ToList();
                        var salesOrganizations = pricingDtoList.Select(_ => _.SalesOrg).Distinct().ToList();

                        var skuContext = _emamiContext.Skus.AsNoTracking().Where(_ => skuCodes.Contains(_.SkuCode)).ToList();
                        var distChnlContext = _emamiContext.DistributionChannel.AsNoTracking().Where(_ => distributionChannels.Contains(_.Code)).ToList();
                        var divisionContext = _emamiContext.Divisions.AsNoTracking().Where(_ => divisions.Contains(_.Code)).ToList();
                        var salesOrgContext = _emamiContext.SalesOrganization.AsNoTracking().Where(_ => salesOrganizations.Contains(_.Code)).ToList();
                        var plantContext = _emamiContext.Depots.AsNoTracking().Where(_ => plantCodes.Contains(_.Code)).ToList();


                        foreach (var price in pricingDtoList)
                        {

                            var ValidFrom = DateTime.ParseExact(price.Valid_From.Replace('.', '/'), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            var ValidTo = DateTime.ParseExact(price.Valid_To.Replace('.', '/'), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            var salesOrganizationId = salesOrgContext.FirstOrDefault(_ => _.Code == price.SalesOrg) != null ? salesOrgContext.FirstOrDefault(_ => _.Code == price.SalesOrg).Id : 0;
                            var distributionChannelId = distChnlContext.FirstOrDefault(_ => _.Code == price.Distributor_Channel && _.SalesOrganizationId == salesOrganizationId) != null ? distChnlContext.FirstOrDefault(_ => _.Code == price.Distributor_Channel && _.SalesOrganizationId == salesOrganizationId).Id : 0;
                            var divisionId = divisionContext.FirstOrDefault(_ => _.Code == price.Division && _.SalesOrganizationId == salesOrganizationId && _.DistributionChannelId == distributionChannelId) != null ? divisionContext.FirstOrDefault(_ => _.Code == price.Division && _.SalesOrganizationId == salesOrganizationId && _.DistributionChannelId == distributionChannelId).Id : 0;
                            var skuId = skuContext.FirstOrDefault(_ => _.SkuCode == price.Material && _.DivisionId == divisionId && _.SalesOrganizationId == salesOrganizationId && _.DistributionChannelId == distributionChannelId) != null ? skuContext.FirstOrDefault(_ => _.SkuCode == price.Material && _.DivisionId == divisionId && _.SalesOrganizationId == salesOrganizationId && _.DistributionChannelId == distributionChannelId).Id : 0;
                            var plantId = plantContext.FirstOrDefault(_ => _.Code == price.FromPlant) != null ? plantContext.FirstOrDefault(_ => _.Code == price.FromPlant).Id : 0;
                            var oilTypeId = skuContext.FirstOrDefault(_ => _.SkuCode == price.Material && _.DivisionId == divisionId && _.SalesOrganizationId == salesOrganizationId && _.DistributionChannelId == distributionChannelId) != null ? (long)skuContext.FirstOrDefault(_ => _.SkuCode == price.Material && _.DivisionId == divisionId && _.SalesOrganizationId == salesOrganizationId && _.DistributionChannelId == distributionChannelId).OilTypeId : 0;
                            var oilPackingTypeId = skuContext.FirstOrDefault(_ => _.SkuCode == price.Material && _.DivisionId == divisionId && _.SalesOrganizationId == salesOrganizationId && _.DistributionChannelId == distributionChannelId) != null ? (long)skuContext.FirstOrDefault(_ => _.SkuCode == price.Material && _.DivisionId == divisionId && _.SalesOrganizationId == salesOrganizationId && _.DistributionChannelId == distributionChannelId).PackGroupId : 0;
                            var errorMessage = "";
                            var errorFlag = true;

                            if (salesOrganizationId == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.SalesOrganisationIsEmpty, price.SalesOrg), errorMessage);
                                errorFlag = false;
                            }
                            if (distributionChannelId == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.DistributionChannelIsEmpty, price.Distributor_Channel), errorMessage);
                                errorFlag = false;
                            }

                            if (divisionId == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.DivisionIsEmpty, price.Division), errorMessage);
                                errorFlag = false;
                            }
                            if (skuId == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(price.Material + " " + Constants.SKUsNotAvailable, errorMessage);
                                errorFlag = false;
                            }
                            if (plantId == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.PlantNotExist, price.FromPlant), errorMessage);
                                errorFlag = false;
                            }
                            if (errorFlag)
                            {
                                //var codeExist = _emamiContext.TodayPricing.AsNoTracking().FirstOrDefault(_ => _.SAPPricingCode == price.Condition_RecordNo);

                                //if (codeExist == null)
                                //{
                                  //  var priceListCheck = todayPricingList.FirstOrDefault(_ => _.SAPPricingCode == price.Condition_RecordNo);

                                    //if (priceListCheck == null)
                                    //{
                                        var pricingDto = new TodayPricing
                                        {
                                            SAPPricingCode = price.Condition_RecordNo,
                                            Price = price.Amount,
                                            SalesOrganizationId = salesOrganizationId,
                                            DistributionChannelId = distributionChannelId,
                                            DivisionId = divisionId,
                                            SkuId = skuId,
                                            PlantId = plantId,
                                            OilTypeId = oilTypeId,
                                            OilPackingTypeId = oilPackingTypeId,
                                            ValidFrom = ValidFrom,
                                            ValidTo = ValidTo,
                                            DepotCode = price.DepotCode,
                                            PlantCode = price.FromPlant,
                                            DistributionChannel = price.Distributor_Channel,
                                            Division = price.Division,
                                            PerUnit = price.PricingUnit,
                                            SalesOrganization = price.SalesOrg,
                                            SkuCode = price.Material,
                                            CreatedBy = userId,
                                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                            ModifiedBy = userId,
                                            ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                        };
                                        if (pricingDto != null)
                                        {
                                            todayPricingList.Add(pricingDto);
                                        }
                                   // }
                               // }
                                //else
                                //{
                                //    var sqlUpdate = "UPDATE TodayPricings SET SAPPricingCode = @SAPPricingCode ,Price = @Price,PlantCode = @PlantCode,ValidFrom=@ValidFrom , ValidTo =@ValidTo,DepotCode =@DepotCode," +
                                //    "DistributionChannel=@DistributionChannel,Division=@Division,PerUnit=@PerUnit,SalesOrganization=@SalesOrganization, SkuCode=@SkuCode,ModifiedDate=@ModifiedDate,ModifiedBy=@ModifiedBy " +
                                //    " WHERE Id = @Id";
                                //    //var parameters = new[]{
                                //    //        new SqlParameter("@Id", codeExist.Id),
                                //    //        new SqlParameter("@SAPPricingCode", price.Condition_RecordNo),
                                //    //        new SqlParameter("@Price", price.Amount),
                                //    //        new SqlParameter("@PlantCode", price.FromPlant),
                                //    //        new SqlParameter("@ValidFrom", ValidFrom),
                                //    //        new SqlParameter("@ValidTo", ValidTo),
                                //    //        new SqlParameter("@DepotCode", price.DepotCode == null ? string.Empty : price.DepotCode ),
                                //    //        new SqlParameter("@DistributionChannel", price.Distributor_Channel),
                                //    //        new SqlParameter("@Division", price.Division),
                                //    //        new SqlParameter("@PerUnit", price.PricingUnit),
                                //    //        new SqlParameter("@SalesOrganization", price.SalesOrg),
                                //    //        new SqlParameter("@SkuCode", price.Material),
                                //    //        new SqlParameter("@SalesOrganizationId", salesOrganizationId),
                                //    //        new SqlParameter("@DistributionChannelId", distributionChannelId),
                                //    //        new SqlParameter("@DivisionId", divisionId),
                                //    //        new SqlParameter("@SkuId", skuId),
                                //    //        new SqlParameter("@PlantId", plantId),
                                //    //        new SqlParameter("@ModifiedDate", currentDate),
                                //    //        new SqlParameter("@ModifiedBy", userId)

                                //    //    };
                                //    //_emamiContext.BulkUpdateProxy(sqlUpdate, parameters);

                                //    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                //    {

                                //        var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                //        var result = conn.Execute(sqlUpdate, new
                                //        {
                                //            SAPPricingCode = price.Condition_RecordNo,
                                //            Price = price.Amount,
                                //            PlantCode = price.FromPlant,
                                //            ValidFrom = ValidFrom,
                                //            ValidTo = ValidTo,
                                //            DepotCode = price.DepotCode == null ? string.Empty : price.DepotCode,
                                //            DistributionChannel = price.Distributor_Channel,
                                //            Division = price.Division,
                                //            PerUnit = price.PricingUnit,
                                //            SalesOrganizationId = salesOrganizationId,
                                //            SalesOrganization = price.SalesOrg,
                                //            DistributionChannelId = distributionChannelId,
                                //            DivisionId = divisionId,
                                //            SkuId = skuId,
                                //            PlantId = plantId,
                                //            ModifiedDate = modifiedDate,
                                //            ModifiedBy = userId,
                                //            SkuCode = price.Material,
                                //            Id = codeExist.Id
                                //        });

                                //    }
                                //}
                            }
                            else
                            {
                                errorMessageList.Add(errorMessage);
                                errorRecordList.Add(price);
                            }


                            dataSynced++;

                        }
                        if (null != todayPricingList && todayPricingList.Any())
                        {
                            _emamiContext.BulkInsertProxy(todayPricingList);
                        }
                        _emamiContext.SaveChanges();

                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                        if (todayPricingList.IsAny())
                        {
                            sapDataSyncResultDto.SuccessRecordDetailsResponse = todayPricingList;
                        }
                        else
                        {
                            sapDataSyncResultDto.SuccessRecordDetailsResponse = pricingDtoList;
                        }
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = pricingDtoList;
                    }

                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                        _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList)}");
                    }
                    else
                    {
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;

                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = string.Concat(Constants.ServiceErrorMessage, message);
                _logger.Error(message);
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }
        #endregion

        #region OpenContract and OpenBalance
        public void PendingContractAutoTrigger()
        {
            _methodName = "PendingContractAutoTrigger";
            var resultDto = new ResultDto();
            try
            {
                var PendingContractDeleteHours = ConsoleSettings.PendingContractDeleteHours;
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                currentDate = currentDate.AddHours(PendingContractDeleteHours);

                _logger.Info($"Query Pending Contracts Delete : {currentDate.ToString()}");
                #region Open Contract Data Delete
                var pendingContractsDelete = "DELETE FROM PendingContracts WHERE CreatedDate <=" + "'" + currentDate + "'";
                _logger.Info($"Query Pending Contracts Delete : {pendingContractsDelete}");
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {
                    var result = conn.Execute(pendingContractsDelete, new
                    {

                    });
                }
                #endregion

                using (var _emamiContext = new AdaniContext())
                {
                    var divisionsContext = _emamiContext.Divisions.AsNoTracking().Where(_ => _.IsActive).Select(_ => new { DivisionId = _.Id, _.SalesOrganizationId, _.DistributionChannelId }).Distinct().ToList();

                    var userContext = (from u in _emamiContext.Users.AsNoTracking()
                                       join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                                       where u.IsActive && ur.RoleId == (int)Adani.Solution.DTO.Enums.Role.Dealer
                                       select new { u.Name, u.Code, u.Id }).ToList();
                    if (userContext != null)
                    {
                        var customerList = userContext.Select(s => s.Id).Distinct().ToList();
                        var usersData = _emamiContext.UserDivisionMappings.AsNoTracking()
                           .Where(_ => customerList.Contains(_.UserId))
                           .Select(s => new {
                               Code = s.User.Code,
                               Division = s.Division.Code,
                               DistributionChannel = s.DistributionChannel.Code,
                               SalesOrganization = s.SalesOrganization.Code,
                               DivisionId = s.Division.Id,
                               DistributionChannelId = s.DistributionChannel.Id,
                               SalesOrganizationId = s.SalesOrganization.Id
                           }).ToList();
                        //foreach (var item in userContext)
                        //{
                        //    var usersDataContext = usersData.Where(_ => _.UserId == item.Id);
                        //if (usersData != null && usersData.Any())
                        //{
                        foreach (var divisionDetails in divisionsContext)
                        {

                            var inputDto = usersData.Where(_ => _.SalesOrganizationId == divisionDetails.SalesOrganizationId && _.DistributionChannelId == divisionDetails.DistributionChannelId && _.DivisionId == divisionDetails.DivisionId);
                            if (inputDto != null && inputDto.IsAny())
                            {
                                var soldToPartyList = inputDto.Select(userDetails => new OpenContractRequestDTO
                                {
                                    SoldToParty = userDetails.Code != null ? userDetails.Code : string.Empty
                                }).ToList();
                                var combinationData = inputDto.FirstOrDefault();
                                ContractOpenBalanceRequest(soldToPartyList, combinationData.SalesOrganization, combinationData.DistributionChannel, combinationData.Division);
                            }

                        }

                        //}                           
                        //}
                    }
                }
                resultDto.SuccessDto.Response = Constants.SuccessMessage;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
        }
        public ResultDto ContractTrigger(OpenContractRequestDTOList inputDto)
        {
            _methodName = "ContractTrigger";
            var resultDto = new ResultDto();
            try
            {
                ContractOpenBalanceRequest(inputDto.Records, inputDto.SalesOrg, inputDto.DistChannel, inputDto.Division);

                resultDto.SuccessDto.Response = Constants.SuccessMessage;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public void ContractOpenBalanceRequest(List<OpenContractRequestDTO> data, string salesOrg, string distChannel, string division)
        {
            _methodName = "ContractOpenBalanceRequest";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            //var syncFolder = ConsoleSettings.SaudaFolder;
            //var errorMessageList = new List<string>();
            //var subject = string.Concat("Contract Balance Request", " ", ConsoleSettings.AppToSapDataSyncEmailSubject);
            //var csvFileName = ConsoleSettings.SaudaHBCCreationCsv;
            var resultDto = new ResultDto();
            //var saudaViewDtoList = new HANASaudaViewList();
            //var SaudaContext = new List<SaudaOrder>();
            //var SaudaListContext = new List<Sauda>();
            //List<OpenContractRequestDTO> data = new List<OpenContractRequestDTO>();
            var totalinput = new List<OpenContractRequestDTO>();
            //var inputDtoJson = string.Empty;
            try
            {
                //using (var _emamiContext = new EmamiContext())
                //{
                //var description = UtilityHelper.GetEnumDescription(DTO.Enums.Configuration.OpenContractTimeInHrs);
                //var configurationContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == description);
                //double value = 0;
                //if (configurationContext != null)
                //{
                //    value = Convert.ToDouble(configurationContext.Value);
                //}

                //var userCreditcontext = _emamiContext.PendingContracts.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.DealerId);

                //if(userCreditcontext != null)
                //{

                //}
                //DateTime expirationTime = userCreditcontext.CreatedDate.AddHours(-value);
                //TimeSpan timeRemaining = expirationTime - userCreditcontext.CreatedDate;
                //if (timeRemaining < TimeSpan.Zero)
                //{
                //var usercontext = _emamiContext.Users.AsNoTracking().FirstOrDefault(user => user.Id == inputDto.DealerId);

                //var userdivisionMapping = _emamiContext.UserDivisionMappings.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.DealerId && _.SalesOrganizationId == inputDto.SalesOrgId
                //  && _.DistributionChannelId == inputDto.DistChnlId && _.DivisionId == inputDto.DivisionId);

                //if (inputDto != null)
                //{
                //    data = inputDto.Select(s => new OpenContractRequestDTO()
                //    {
                //        SoldToParty = s.UserCode,
                //        DistChannel = s.DistChnl,
                //        Division = s.Division,
                //        SalesOrg = s.SalesOrg
                //    }).ToList();
                //}

                if (data.IsAny())
                {
                    int batchCount = ConsoleSettings.BatchCount;
                    var loopcount = Math.Ceiling(Convert.ToDecimal(data.Count()) / Convert.ToDecimal(batchCount));
                    int skip = 0;
                    for (int i = 0; i < loopcount; i++)
                    {

                        var perRequestdata = data.Skip(skip).Take(batchCount).ToList();
                        var requestData = new OpenContractRequestDTOList()
                        {
                            Records = perRequestdata,
                            DistChannel = distChannel,
                            Division = division,
                            SalesOrg = salesOrg
                        };
                        //var openContractRequestInputDto = new OpenContractRequestInputDto { OpenContractBalReq = requestData };

                        //var json = JsonConvert.SerializeObject(openContractRequestInputDto, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                        // _logger.Info($"Json Input : {JsonConvert.SerializeObject(openContractRequestInputDto)}");
                        var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.OpenContractApiUrl, requestData);
                        var status = response.StatusCode;
                        _logger.Info($"Responce : {status}");
                        skip += batchCount;
                    }
                }
                //var data = new OpenContractRequestDTO()
                //{
                //    SoldToParty = usercontext != null ? usercontext.Code : string.Empty,
                //    DistChannel = userdivisionMapping.DistributionChannelId > 0 ? userdivisionMapping.DistributionChannel.Code : string.Empty,
                //    Division = userdivisionMapping.DivisionId > 0 ? userdivisionMapping.Division.Code : string.Empty,
                //    SalesOrg = userdivisionMapping.SalesOrganizationId > 0 ? userdivisionMapping.SalesOrganization.Code : string.Empty
                //};


                //}
                totalinput.AddRange(data);
                sapDataSyncResultDto.OutstandingResult.DataRetrieved = totalinput.Count();
                sapDataSyncResultDto.OutstandingResult.DataSynced = totalinput.Count();
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = totalinput;
                sapDataSyncResultDto.SuccessRecordDetailsResponse = totalinput;

                //}
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = sapDataSyncResultDto;
                resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                //   _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, null, subject);

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = data;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _logger.Error(message);
                // _sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
            }
        }

        /// <summary>
        /// Save Pricing Details
        /// </summary>
        /// <param name="inputdto"></param>
        public void ContractOpenBalanceResponce(HANAOpenBalAndOpenContractDTOList inputdto)
        {
            _methodName = "ContractOpenBalanceResponce";
            _logger.Info($"SAP Service Start : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputdto)}");
            var resultDto = new ResultDto();
            var errorMessageList = new List<string>();
            var errorMessageOpenBalList = new List<string>();
            var errorRecordList = new List<OpenContract>();
            var errorRecordOpenBalList = new List<OpenBal>();
            var successRecordList = new List<OpenContract>();
            var successRecordOpenBalList = new List<OpenBal>();
            var dataSynced = 0;
            long userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var synctype = "Contract Balance Response";
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var subject = string.Concat(synctype, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            //var openBalandopenContractDtoList = inputdto != null && inputdto.Records != null ? inputdto.Records.OpenContract : new List<OpenContract>();
            //var openContractDtoList = inputdto != null && inputdto.OpenContract != null ? inputdto.OpenContract : new List<OpenContract>();


            try
            {
                var salesOrganizationContext = new SalesOrganization();
                var distributionChannelContext = new DistributionChannel();
                var divisionsContext = new Data.Entities.Division();
                var skuData = new List<SkuDto>();
                var skuUomMappingData = new List<SkuUomMappingDto>();
                var userContextOpenBalList = new List<UserDto>();
                var userContextOpenContractList = new List<UserDto>();
                List<UserDto> userlistInOpenBal;
                List<UserDto> userlistInOpenContract;
                long salesOrganisationId;
                long distributionChannelId;
                long divisionId;
                var usersInOpenBal = inputdto.Records != null && inputdto.Records.OpenBal != null ? inputdto.Records.OpenBal.Select(s => s.SoldToParty.TrimStart('0')).ToList() : new List<string>();
                var usersInOpenContract = inputdto.Records != null && inputdto.Records.OpenContract != null ? inputdto.Records.OpenContract.Select(s => s.SoldToParty.TrimStart('0')).Distinct().ToList() : new List<string>();
                #region Get Common Datas
                using (var _emamiContext = new AdaniContext())
                {
                    salesOrganizationContext = _emamiContext.SalesOrganization.AsNoTracking().FirstOrDefault(_ => _.Code == inputdto.Records.SalesOrg);
                    salesOrganisationId = salesOrganizationContext != null ? salesOrganizationContext.Id : 0;
                    distributionChannelContext = _emamiContext.DistributionChannel.AsNoTracking().FirstOrDefault(_ => _.Code == inputdto.Records.DistChannel && _.SalesOrganizationId == salesOrganisationId);
                    distributionChannelId = distributionChannelContext != null ? distributionChannelContext.Id : 0;
                    divisionsContext = _emamiContext.Divisions.AsNoTracking().FirstOrDefault(d => d.Code == inputdto.Records.Division && d.SalesOrganizationId == salesOrganisationId && d.DistributionChannelId == distributionChannelId);
                    divisionId = divisionsContext != null ? divisionsContext.Id : 0;

                    userlistInOpenBal = (from u in _emamiContext.Users.AsNoTracking()
                                         where usersInOpenBal.Contains(u.Code)
                                         select new UserDto { Name = u.Name, Code = u.Code, Id = u.Id }).ToList();

                    userContextOpenBalList = (from u in userlistInOpenBal
                                              join ud in _emamiContext.UserDivisionMappings.AsNoTracking() on u.Id equals ud.UserId
                                              where ud.SalesOrganizationId == salesOrganisationId
                                              && ud.DistributionChannelId == distributionChannelId && ud.DivisionId == divisionId
                                              select new UserDto { Name = u.Name, Code = u.Code, Id = u.Id }).ToList();
                }
                #endregion

                var errorFlag = true;
                var errorMessage = string.Empty;
                if (inputdto == null)
                {
                    errorMessage = Constants.InvalidRequest;
                    errorFlag = false;
                }

                if (salesOrganisationId == 0)
                {
                    errorMessage = Constants.BindErrorMessage(Constants.SalesOrganisationIsEmpty + " App_Id: " + inputdto.Records.SalesOrg, errorMessage);
                    errorFlag = false;
                }
                if (distributionChannelId == 0)
                {
                    errorMessage = Constants.BindErrorMessage(Constants.DistributionChannelIsEmpty + " App_Id: " + inputdto.Records.DistChannel, errorMessage);
                    errorFlag = false;
                }

                if (divisionId == 0)
                {
                    errorMessage = Constants.BindErrorMessage(Constants.DivisionIsEmpty + " App_Id: " + inputdto.Records.Division, errorMessage);
                    errorFlag = false;
                }
                if (errorFlag && inputdto.Records.OpenBal != null && inputdto.Records.OpenBal.Any())
                {
                    sapDataSyncResultDto.OutstandingResult.DataRetrieved = inputdto.Records.OpenBal.Count();
                    var userCreditMasterList = new List<UserCreditMaster>();
                    foreach (var soldtoparty in usersInOpenBal)
                    {
                        var openBalList = inputdto.Records.OpenBal.Where(_ => _.SoldToParty.TrimStart('0') == soldtoparty.TrimStart('0')).ToList();
                        var userContext = userContextOpenBalList.FirstOrDefault(_ => _.Code == soldtoparty.TrimStart('0'));
                        if (userContext == null)
                        {
                            errorMessage = Constants.BindErrorMessage(Constants.UserNotFound + " App_Id: " + soldtoparty, errorMessage);
                            errorFlag = false;
                        }
                        if (errorFlag)
                        {
                            #region Open Balance data delete                        
                            var userCreditMasterDelete = "DELETE FROM UserCreditMasters WHERE UserId =" + userContext.Id;
                            //+ " and SalesOrgId =" + salesOrganisationId + " and DistChnlId =" + distributionChannelId + " and DivisionId =" + divisionId;

                            using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                            {
                                var result = conn.Execute(userCreditMasterDelete, new
                                {

                                });
                            }
                            #endregion


                            if (openBalList != null && openBalList.Any())
                            {
                                foreach (var data in openBalList)
                                {
                                    var pricingDto = new UserCreditMaster
                                    {
                                        UserId = userContext.Id,
                                        SalesOrgId = salesOrganisationId,
                                        DivisionId = divisionId,
                                        DistChnlId = distributionChannelId,
                                        CreditLimit = ConsoleSettings.StringToDecimalTryParse(data.CreditLimit),
                                        OpenOrders = ConsoleSettings.StringToDecimalTryParse(data.OpenOrders),
                                        DeliveryValue = ConsoleSettings.StringToDecimalTryParse(data.DeliveryValue),
                                        BillingDocumentValue = ConsoleSettings.StringToDecimalTryParse(data.BillingDocumentValue),
                                        CreditExposure = ConsoleSettings.StringToDecimalTryParse(data.TotalExposure),
                                        AvailableCreditLimit = ConsoleSettings.StringToDecimalTryParse(data.CreditLimit) - ConsoleSettings.StringToDecimalTryParse(data.TotalExposure),
                                        CreatedBy = userId,
                                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                        ModifiedBy = userId,
                                        ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                        Isactive=true,
                                        IsSAPData=true,
                                        SalesValue=0,
                                        TotalReceivable=0,
                                        SaudaDepC=0,
                                        SecDepH=0,
                                        BankGuarM=0,
                                        AdvanceA=0,
                                        DueToday=0,
                                        TomorrowsDue=0,
                                        Overdue=0,
                                        NotDue=0,
                                        NextIntRev=string.Empty,
                                        Blocked=string.Empty,
                                        TotalLimit=0,
                                        IndividLimit=0
                                    };
                                    userCreditMasterList.Add(pricingDto);
                                    successRecordOpenBalList.Add(data);
                                    dataSynced++;

                                }

                            }
                        }
                        else
                        {
                            errorMessageOpenBalList.Add(errorMessage);
                            errorRecordOpenBalList.AddRange(openBalList);
                        }
                    }
                    if (null != userCreditMasterList && userCreditMasterList.Any())
                    {
                        //var userCreditMaster = Constants.ToDataTable(userCreditMasterList);
                        //using (var conn = new SqlConnection(Config.DBConnectionString))
                        //{
                        //    conn.Open();
                        //    conn.Execute("SP_AddOpenCreditMasters", new { OpenCreditMaster = userCreditMaster.AsTableValuedParameter("UDTT_OpenCreditMaster") },
                        //        commandType: CommandType.StoredProcedure);
                        //}
                        using (var _emamiContext = new AdaniContext())
                        {
                            _emamiContext.BulkInsertProxy(userCreditMasterList);
                            _emamiContext.SaveChanges();
                        }
                    }
                }
                else
                {
                    errorMessageOpenBalList.Add(errorMessage);
                    errorRecordOpenBalList.AddRange(inputdto.Records.OpenBal);
                }

                if (errorMessageOpenBalList.Any())
                {
                    sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                    sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                    sapDataSyncResultDto.TotalInputRecordDetailsResponse = inputdto.Records.OpenBal;
                    sapDataSyncResultDto.SuccessRecordDetailsResponse = successRecordOpenBalList;
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;

                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Response = sapDataSyncResultDto;
                    resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList)}");
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
                }

                #region Get Common Datas
                using (var _emamiContext = new AdaniContext())
                {
                    userlistInOpenContract = (from u in _emamiContext.Users.AsNoTracking()
                                              where usersInOpenContract.Contains(u.Code)
                                              select new UserDto { Name = u.Name, Code = u.Code, Id = u.Id }).ToList();

                    userContextOpenContractList = (from u in userlistInOpenContract
                                                   join ud in _emamiContext.UserDivisionMappings.AsNoTracking() on u.Id equals ud.UserId
                                                   where ud.SalesOrganizationId == salesOrganisationId
                                                   && ud.DistributionChannelId == distributionChannelId && ud.DivisionId == divisionId
                                                   select new UserDto { Name = u.Name, Code = u.Code, Id = u.Id }).ToList();

                    skuData = _emamiContext.Skus.AsNoTracking()
                        //.Where(_ => skuCode.Contains(_.SkuCode))
                        .Select(s => new SkuDto
                        {
                            Id = s.Id,
                            SalesOrganizationId = s.SalesOrganizationId,
                            DivisionId = s.DivisionId,
                            DistributionChannelId = s.DistributionChannelId,
                            SkuCode = s.SkuCode
                        }).ToList();
                    var skuIds = skuData.Select(s => s.Id).Distinct().ToList();
                    skuUomMappingData = _emamiContext.SkuUomMapping.AsNoTracking()
                        .Where(_ => skuIds.Contains(_.SkuId))
                        .Select(s => new SkuUomMappingDto
                        {
                            Id = s.Id,
                            ConversionFactor = s.ConversionFactor,
                            ConversionFactor1 = s.ConversionFactor1,
                            ConversionFactor2 = s.ConversionFactor2,
                            SkuId = s.SkuId,
                            UomId = s.UomId
                        }).ToList();
                }
                #endregion

                dataSynced = 0;
                if (errorFlag && inputdto.Records.OpenContract != null && inputdto.Records.OpenContract.Any())
                {
                    sapDataSyncResultDto.OutstandingResult.DataRetrieved = inputdto.Records.OpenContract.Count();
                    var pendingContractList = new List<PendingContract>();
                    //var pendingContractsDelete = "DELETE FROM PendingContracts WHERE CreatedDate <=" + "'" + currentDate + "'";
                    //_logger.Info($"Query Pending Contracts Delete : {pendingContractsDelete}");
                    //using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    //{
                    //    var result = conn.Execute(pendingContractsDelete, new
                    //    {

                    //    });
                    //}
                    foreach (var soldtoparty in usersInOpenContract)
                    {
                        errorFlag = true;
                        var userContext = userContextOpenContractList.FirstOrDefault(_ => _.Code == soldtoparty.TrimStart('0'));
                        var openContractList = inputdto.Records.OpenContract.Where(_ => _.SoldToParty.TrimStart('0') == soldtoparty.TrimStart('0')).ToList();
                        if (userContext == null)
                        {
                            errorMessage = Constants.BindErrorMessage(Constants.UserNotFound + " App_Id: " + soldtoparty, errorMessage);
                            errorFlag = false;
                        }
                        if (errorFlag)
                        {
                            #region Open Contract Data Delete
                            var pendingContractsDelete = "DELETE FROM PendingContracts WHERE UserId =" + userContext.Id + " and SalesOrgId =" + salesOrganisationId + " and DistChnlId =" + distributionChannelId + " and DivisionId =" + divisionId;
                            _logger.Info($"Query Pending Contracts Delete : {pendingContractsDelete}");
                            using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                            {
                                var result = conn.Execute(pendingContractsDelete, new
                                {

                                });
                            }
                            #endregion
                            if (openContractList != null && openContractList.Any())
                            {
                                foreach (var openContract in openContractList)
                                {
                                    errorFlag = true;
                                    var ValidTo = openContract.ValidTo.Replace('.', '-');
                                    var Contract_CreatedDate = openContract.Contract_CreatedDate.Replace('.', '-');
                                    if (ConsoleSettings.ContractDateConditionCheck)
                                    {
                                        if (string.IsNullOrEmpty(openContract.OpenQTY) || ConsoleSettings.StringToDecimalTryParse(openContract.OpenQTY) < 0)
                                        {
                                            errorFlag = false;
                                        }

                                        DateTime contractdatetime = DateTime.ParseExact(openContract.Contract_CreatedDate.Replace('.', '-'), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                        DateTime financialyear = new DateTime(2022, 4, 1);
                                        if (contractdatetime < financialyear)
                                        {
                                            errorFlag = false;
                                        }
                                    }

                                    if (errorFlag)
                                    {
                                        var skuContext = skuData.FirstOrDefault(_ => _.SkuCode == openContract.Material && _.SalesOrganizationId == salesOrganisationId && _.DistributionChannelId == distributionChannelId && _.DivisionId == divisionId);
                                        if (skuContext != null)
                                        {
                                           
                                            DateTime contractdatetime = DateTime.ParseExact(openContract.Contract_CreatedDate.Replace('.', '-'), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                                DateTime ValidTodatetime = DateTime.ParseExact(openContract.ValidTo.Replace('.', '-'), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                                var pricingDto = new PendingContract
                                            {
                                                UserId = userContext.Id,
                                                CustomerCode = userContext.Code,
                                                CustomerName = userContext.Name,
                                                SalesOrgId = salesOrganisationId,
                                                DivisionId = divisionId,
                                                DistChnlId = distributionChannelId,
                                                SaudaNumber = openContract.SaudaNumber,
                                                MaterialCode = openContract.Material,
                                                ContractValidTo = ValidTodatetime,
                                                ContractValidFrom = contractdatetime,
                                                BasicRate = (ConsoleSettings.StringToDecimalTryParse(openContract.OpenQTY) > 0) ? ConsoleSettings.StringToDecimalTryParse(openContract.Price) / ConsoleSettings.StringToDecimalTryParse(openContract.OpenQTY) : 0,
                                                PendingQuantityInCase = ConsoleSettings.StringToDecimalTryParse(openContract.OpenQTY),
                                                OpenSalesOrderQuantity= ConsoleSettings.StringToDecimalTryParse(openContract.OpenSOQTY),
                                                SaudaQuantity = _resultService.ConvertCasetoMetricTonWithoutDB(ConsoleSettings.StringToDecimalTryParse(openContract.OpenQTY), skuContext.Id, skuUomMappingData),
                                                CreatedBy = userId,
                                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                ModifiedBy = userId,
                                                ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                TotalValue = ConsoleSettings.StringToDecimalTryParse(openContract.Price)
                                            };
                                            pendingContractList.Add(pricingDto);
                                            dataSynced++;
                                            successRecordList.Add(openContract);
                                        }
                                        else
                                        {
                                            errorMessageList.Add(string.Format(Constants.SkuIsNotInPortal, openContract.SaudaNumber, openContract.Material, inputdto.Records.SalesOrg, inputdto.Records.DistChannel, inputdto.Records.Division, openContract.OpenQTY));
                                            errorRecordList.Add(openContract);
                                            errorFlag = false;
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            errorMessageList.Add(errorMessage);
                            errorRecordList.AddRange(inputdto.Records.OpenContract);
                        }
                    }
                    if (null != pendingContractList && pendingContractList.Any())
                    {
                            //var pendingContract = Constants.ToDataTable(pendingContractList);
                            //using (var conn = new SqlConnection(Config.DBConnectionString))
                            //{
                            //    conn.Open();
                            //    conn.Execute("SP_AddPendingContracts", new { PendingContracts = pendingContract.AsTableValuedParameter("UDTT_PendingContracts") },
                            //        commandType: CommandType.StoredProcedure);
                            //}
                            using (var _emamiContext = new AdaniContext())
                            {
                                _emamiContext.BulkInsertProxy(pendingContractList);
                                _emamiContext.SaveChanges();
                            }
                    
                    }
                }
                else
                {
                    errorMessageList.Add(errorMessage);
                    errorRecordList.AddRange(inputdto.Records.OpenContract);
                }
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = inputdto.Records.OpenContract;
                sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                sapDataSyncResultDto.SuccessRecordDetailsResponse = successRecordList;
                if (errorMessageList.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Response = sapDataSyncResultDto;
                    resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList)}");
                }
                else
                {
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = sapDataSyncResultDto;
                    resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;

                }
                sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, "Open Contract Responce", null, subject);

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = string.Concat(Constants.ServiceErrorMessage, message);
                _logger.Error(message);
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }

        #endregion

        #region Sauda Limit AWL       

        public void SaudaLimitResponce(HANASaudaLimitList inputDto)
        {
            _methodName = "SaudaLimitResponce";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputDto)}");
            var resultDto = new ResultDto();
            var sapDataSyncResultDto = new SapDataSyncResultDto();
            var errorRecordList = new List<HANASaudaLimitDto>();
            sapDataSyncResultDto.SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
            var dataSynced = 0;
            var synctype = ConsoleSettings.SaudaLimitSubject;
            var userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var subject = string.Concat(ConsoleSettings.SaudaLimitSubject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            var saudaLimitDtoList = inputDto.ContractLimit_Details != null ? inputDto.ContractLimit_Details : new List<HANASaudaLimitDto>();
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = 1;
            try
            {
                if (saudaLimitDtoList != null)
                {
                    var saudaLimitList = new List<SaudaLimit>();
                    var errorMessageList = new List<string>();
                    using (var _emamiContext = new AdaniContext())
                    {
                        //#region Get Common Datas
                        //var verticalContextList = _emamiContext.Divisions.AsNoTracking();
                        ////var customerContextList = _emamiContext.Users.AsNoTracking();
                        //var customerContextList = (from s in _emamiContext.Users
                        //                           join role in _emamiContext.UserRoles on s.Id equals role.UserId
                        //                           where role.RoleId != (int)DTO.Enums.Role.ShipToParty
                        //                           select new { Id = s.Id, Code = s.Code, VerticalId = s.DivisionId, CustomerGroup = s.CustomerGroup, SaudaLimit = s.SaudaLimit }).ToList();
                        //#endregion
                        foreach (var saudaLimit in saudaLimitDtoList)
                        {
                            var errorFlag = true;
                            var errorMessage = string.Empty;

                            if (inputDto == null)
                            {
                                errorMessage = Constants.InvalidRequest;
                                errorFlag = false;
                            }

                            if (saudaLimit.CustomerNo == null)
                            {
                                errorMessage = "CustomerNo is null";
                                errorFlag = false;
                            }
                            else
                            {
                                saudaLimit.CustomerNo = saudaLimit.CustomerNo.TrimStart('0');
                                // _logger.Info($"Foreach in : " + saudaLimit.CustomerNo);
                            }

                            var userContext = _emamiContext.Users.FirstOrDefault(_ => _.Code == saudaLimit.CustomerNo && _.IsActive);
                            if (userContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.UserNotFound + " App_Id: " + saudaLimit.CustomerNo, errorMessage);
                                errorFlag = false;
                            }
                            var divisionContext = _emamiContext.Divisions.AsNoTracking().Where(_ => _.Code == saudaLimit.Division).Select(a => a.Id).ToList();
                            //var divisionId = divisionContext == null ? 0 : divisionContext.Id;
                            if (userContext != null && divisionContext.IsAny())
                            {
                                var userDivisionMappingContext = _emamiContext.UserDivisionMappings.Where(_ => divisionContext.Contains(_.DivisionId) && _.UserId == userContext.Id);
                                if (userDivisionMappingContext == null)
                                {
                                    errorMessage = Constants.BindErrorMessage(Constants.DivisionNotMappedFound + " UserCode: " + saudaLimit.CustomerNo, errorMessage);
                                    errorFlag = false;
                                }

                                // _logger.Info($"Foreach in : " + errorMessage);
                                if (errorFlag)
                                {
                                    saudaLimit.End_Date = saudaLimit.End_Date.TrimStart(' ');
                                    var endDate = DateTime.ParseExact(saudaLimit.End_Date.Replace('.', '/'), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    var saudaLimitContext = _emamiContext.SaudaLimit.FirstOrDefault(_ => _.UserId == userContext.Id && _.Division == saudaLimit.Division);

                                    if (saudaLimitContext == null)
                                    {
                                        //  _logger.Info($"Success in 2: " + saudaLimit.CustomerNo);
                                        var saudaLimitDto = new SaudaLimit
                                        {
                                            UserCode = saudaLimit.CustomerNo,
                                            UserId = userContext.Id,
                                            Currency = saudaLimit.Currency_Key,
                                            Description = saudaLimit.Description,
                                            Name = saudaLimit.Name,
                                            OldQty = ConsoleSettings.StringToDecimalTryParse(saudaLimit.Old_Qty),
                                            LimitQty = ConsoleSettings.StringToDecimalTryParse(saudaLimit.Limit_Qty),
                                            TargetValue = ConsoleSettings.StringToDecimalTryParse(saudaLimit.Target_Value),
                                            UOM = saudaLimit.Base_Unit_of_Measure,
                                            Division = saudaLimit.Division,
                                            OldValue = ConsoleSettings.StringToDecimalTryParse(saudaLimit.Old_Value),
                                            ActualLimit = ConsoleSettings.StringToDecimalTryParse(saudaLimit.Limit_Qty),
                                            //RequestedLimit = saudaLimit.CustomerTotalQuantity,                                   
                                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                            CreatedBy = userId,
                                            StatusId = (int)DTO.Enums.Status.Approved,
                                            IsSAPData = true,
                                            IsSAPDataSyncOrNot = true,
                                            EndDate = endDate,
                                            //PendingContract = saudaLimit.PendCont,
                                            //PendingDO = saudaLimit.PendDO,
                                            //PendingOBD = saudaLimit.PendOBD
                                        };

                                        if (userDivisionMappingContext != null)
                                        {
                                            userDivisionMappingContext.ToList().ForEach(_ => _.SaudaLimit = ConsoleSettings.StringToDecimalTryParse(saudaLimit.Limit_Qty));
                                        }

                                        if (saudaLimitDto != null)
                                        {
                                            _logger.Info($"Save : " + saudaLimit.CustomerNo);
                                            saudaLimitList.Add(saudaLimitDto);
                                        }
                                    }
                                    else
                                    {
                                        var sqlUpdate = "UPDATE SaudaLimits SET UserCode = @UserCode ,Currency = @Currency,Description = @Description,Name=@Name ," +
                                            "LimitQty =@LimitQty,OldQty =@OldQty,EndDate=@EndDate,TargetValue=@TargetValue,UOM=@UOM,OldValue=@OldValue," +
                                            "ActualLimit=@ActualLimit,ModifiedDate =@ModifiedDate, ModifiedBy=@ModifiedBy" +
                                            " WHERE Id = @Id";

                                        //var parameters = new[]{
                                        //            new SqlParameter("@Id", saudaLimitContext.Id),
                                        //            new SqlParameter("@UserCode", saudaLimit.CustomerNo != null ? saudaLimit.CustomerNo : string.Empty ),
                                        //            new SqlParameter("@Currency", saudaLimit.Currency_Key != null ? saudaLimit.Currency_Key : string.Empty),
                                        //            new SqlParameter("@Description", saudaLimit.Description != null ?  saudaLimit.Description : string.Empty),
                                        //            new SqlParameter("@Name", saudaLimit.Name != null ? saudaLimit.Name : string.Empty),
                                        //            new SqlParameter("@OldQty", ConsoleSettings.StringToDecimalTryParse(saudaLimit.Old_Qty)),
                                        //            new SqlParameter("@LimitQty", ConsoleSettings.StringToDecimalTryParse(saudaLimit.Limit_Qty)),
                                        //            new SqlParameter("@TargetValue", ConsoleSettings.StringToDecimalTryParse(saudaLimit.Target_Value)),
                                        //            new SqlParameter("@UOM", saudaLimit.Base_Unit_of_Measure != null ? saudaLimit.Base_Unit_of_Measure : string.Empty),
                                        //            new SqlParameter("@OldValue", ConsoleSettings.StringToDecimalTryParse(saudaLimit.Old_Value)),
                                        //            new SqlParameter("@ActualLimit", ConsoleSettings.StringToDecimalTryParse(saudaLimit.Limit_Qty)),
                                        //            new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow)),
                                        //            new SqlParameter("@ModifiedBy", userId),
                                        //            new SqlParameter("@EndDate", endDate),
                                        //        };
                                        //_emamiContext.BulkUpdateProxy(sqlUpdate, parameters);

                                        using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                        {

                                            var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                            var result = conn.Execute(sqlUpdate, new
                                            {
                                                UserCode = saudaLimit.CustomerNo != null ? saudaLimit.CustomerNo : string.Empty,
                                                Currency = saudaLimit.Currency_Key != null ? saudaLimit.Currency_Key : string.Empty,
                                                Description = saudaLimit.Description != null ? saudaLimit.Description : string.Empty,
                                                Name = saudaLimit.Name != null ? saudaLimit.Name : string.Empty,
                                                LimitQty = ConsoleSettings.StringToDecimalTryParse(saudaLimit.Limit_Qty),
                                                OldQty = ConsoleSettings.StringToDecimalTryParse(saudaLimit.Old_Qty),
                                                EndDate = endDate,
                                                TargetValue = ConsoleSettings.StringToDecimalTryParse(saudaLimit.Target_Value),
                                                UOM = saudaLimit.Base_Unit_of_Measure != null ? saudaLimit.Base_Unit_of_Measure : string.Empty,
                                                OldValue = ConsoleSettings.StringToDecimalTryParse(saudaLimit.Old_Value),
                                                ActualLimit = ConsoleSettings.StringToDecimalTryParse(saudaLimit.Limit_Qty),
                                                ModifiedDate = modifiedDate,
                                                ModifiedBy = userId,
                                                Id = saudaLimitContext.Id,
                                            });

                                        }
                                        // _logger.Info($"update : " + saudaLimit.CustomerNo);
                                        if (userDivisionMappingContext != null)
                                        {
                                            userDivisionMappingContext.ToList().ForEach(_ => _.SaudaLimit = ConsoleSettings.StringToDecimalTryParse(saudaLimit.Limit_Qty));
                                        }
                                    }

                                    dataSynced++;


                                }
                                else
                                {
                                    // _logger.Info($"error : " + saudaLimit.CustomerNo);
                                    errorMessageList.Add(errorMessage);
                                    errorRecordList.Add(saudaLimit);
                                }
                            }
                            else
                            {
                                // _logger.Info($"error : " + saudaLimit.CustomerNo);
                                errorMessageList.Add(errorMessage);
                                errorRecordList.Add(saudaLimit);
                            }


                            //var suadaLimitdetailsDelete = "DELETE FROM SaudaLimits";
                            //var listOfStrings = new List<string>();
                            //object[] arrayOfStrings = listOfStrings.ToArray();
                            //_emamiContext.BulkUpdateProxy(suadaLimitdetailsDelete, arrayOfStrings);

                        }

                        if (null != saudaLimitList && saudaLimitList.Any())
                        {
                            _emamiContext.BulkInsertProxy(saudaLimitList);
                        }
                        _emamiContext.SaveChanges();
                        _logger.Info($"Success");
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaLimitDtoList;
                        sapDataSyncResultDto.SuccessRecordDetailsResponse = saudaLimitDtoList.Except(errorRecordList).ToList();
                    }

                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                        _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList)}");
                    }
                    else
                    {

                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaLimitDtoList;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _logger.Error(message);
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }
        #endregion

        #region Sales Order && DeliveryNo Update

        public void LiftingRequestEnquiryNumberUpdate(List<HANASaudaCommonFunctionList> inputdto)
        {
            _methodName = "LiftingRequestEnquiryNumberUpdate";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputdto)}");
            var resultDto = new ResultDto();
            var errorRecordList = new List<HANASaudaCommonFunctionList>();
            var errorMessageList = new List<string>();
            var dataSynced = 0;
            var synctype = ConsoleSettings.LiftingRequestInquiryNumberUpdateSubject;
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var subject = string.Concat(synctype, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            long userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var liftingRequestList = inputdto != null ? inputdto : new List<HANASaudaCommonFunctionList>();
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = liftingRequestList.Count;
            try
            {
                using (var _emamiContext = new AdaniContext())
                {
                    if (liftingRequestList != null && liftingRequestList.Any())
                    {
                        foreach (var liftingData in liftingRequestList)
                        {

                            if (!string.IsNullOrEmpty(liftingData.SAP_Document_No))
                            {
                                var sqlUpdate = "UPDATE LiftingRequests SET SAPDocumentNo = @SAPDocumentNo," +
                                           "ModifiedBy=@ModifiedBy,ModifiedDate = @ModifiedDate WHERE Id = @Id";
                                //var parameters = new[]{
                                //        new SqlParameter("@SAPDocumentNo", liftingData.SAP_Document_No),
                                //        //new SqlParameter("@EnquiryRemarks", liftingData.Message),
                                //        new SqlParameter("@ModifiedBy", userId),
                                //        new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow)),
                                //        new SqlParameter("@Id", liftingData.Impiger_Request_No),
                                //        //new SqlParameter("@Status", liftingData.Status),
                                //        //new SqlParameter("@EnquiryNumberSyncFromSap", true)
                                //};
                                //_emamiContext.BulkUpdateProxy(sqlUpdate, parameters);

                                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                {

                                    var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    var result = conn.Execute(sqlUpdate, new
                                    {
                                        SAPDocumentNo = liftingData.SAP_Document_No,
                                        ModifiedBy = userId,
                                        ModifiedDate = modifiedDate,
                                        Id = liftingData.Impiger_Request_No
                                    });

                                }

                                var liftingDetailsIds = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.LiftingRequestId == liftingData.Impiger_Request_No).ToList();
                                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                {

                                    foreach (var data in liftingDetailsIds)
                                    {
                                        var sqlUpdatedetails = "UPDATE LiftingRequestDetails SET EnquiryRemarks = @EnquiryRemarks, " +
                                                   "ModifiedBy=@ModifiedBy,ModifiedDate = @ModifiedDate,ReprocessStatusId = @Status,EnquiryNumberSyncFromSap = @EnquiryNumberSyncFromSap  WHERE Id = @Id";
                                        //var parametersdetails = new[]{
                                        //    new SqlParameter("@EnquiryRemarks", liftingData.Message),
                                        //    new SqlParameter("@ModifiedBy", userId),
                                        //    new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow)),
                                        //    new SqlParameter("@Id", data.Id),
                                        //    new SqlParameter("@Status", liftingData.Status),
                                        //    new SqlParameter("@EnquiryNumberSyncFromSap", true)
                                        // };
                                        //_emamiContext.BulkUpdateProxy(sqlUpdatedetails, parametersdetails);
                                        var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                        var result = conn.Execute(sqlUpdatedetails, new
                                        {
                                            EnquiryRemarks = liftingData.Message,
                                            ModifiedBy = userId,
                                            ModifiedDate = modifiedDate,
                                            Id = data.Id,
                                            Status = liftingData.Status,
                                            EnquiryNumberSyncFromSap = true
                                        });


                                        //Sales order created quantity update for saudaorder table
                                        //var LiftingRequestContext = _emamiContext.LiftingRequest.AsNoTracking().FirstOrDefault(_ => _.Id == liftingData.Impiger_Request_No);
                                        //if (LiftingRequestContext != null)
                                        //{
                                        // Contract Limit Check Update
                                        ContractAvilableLimitCalculate(data.SaudaNumber);
                                        //}

                                    }

                                }

                                _emamiContext.SaveChanges();

                                dataSynced++;

                                if (!liftingData.Status)
                                {
                                    errorRecordList.Add(liftingData);
                                }
                            }
                            else
                            {
                                errorMessageList.Add("Sap document no is empty");
                                var liftingDetailsIds = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.LiftingRequestId == liftingData.Impiger_Request_No).Select(_ => _.Id).ToList();

                                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                {
                                    foreach (var data in liftingDetailsIds)
                                    {
                                        var sqlUpdatedetails = "UPDATE LiftingRequestDetails SET EnquiryRemarks = @EnquiryRemarks, " +
                                                   "ModifiedBy=@ModifiedBy,ModifiedDate = @ModifiedDate,ReprocessStatusId = @Status,EnquiryNumberSyncFromSap = @EnquiryNumberSyncFromSap  WHERE Id = @Id";
                                        //   var parametersdetails = new[]{
                                        //   new SqlParameter("@EnquiryRemarks", liftingData.Message),
                                        //   new SqlParameter("@ModifiedBy", userId),
                                        //   new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow)),
                                        //   new SqlParameter("@Id", data),
                                        //   new SqlParameter("@Status", (int)DTO.Enums.Status.Pending),
                                        //   new SqlParameter("@EnquiryNumberSyncFromSap", true)
                                        //};
                                        //   _emamiContext.BulkUpdateProxy(sqlUpdatedetails, parametersdetails);

                                        var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                        var result = conn.Execute(sqlUpdatedetails, new
                                        {
                                            EnquiryRemarks = liftingData.Message,
                                            ModifiedBy = userId,
                                            ModifiedDate = modifiedDate,
                                            Status = (int)DTO.Enums.Status.Pending,
                                            EnquiryNumberSyncFromSap = true,
                                            Id = data
                                        });
                                    }


                                }


                                _emamiContext.SaveChanges();
                            }

                            var liftingRequestDetailsContext = _emamiContext.LiftingRequestDetails.AsNoTracking().FirstOrDefault(_ => _.LiftingRequestId == liftingData.Impiger_Request_No);
                            var liftingRequestContext = _emamiContext.LiftingRequest.AsNoTracking().FirstOrDefault(_ => _.Id == liftingRequestDetailsContext.LiftingRequestId);
                            var usersDataContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.Id == liftingRequestContext.UserId);
                            var salesOrganizationDataContext = _emamiContext.SalesOrganization.AsNoTracking().FirstOrDefault(_ => _.Id == liftingRequestDetailsContext.SalesOrganizationId);
                            var distributionChannelDataContext = _emamiContext.DistributionChannel.AsNoTracking().FirstOrDefault(_ => _.Id == liftingRequestDetailsContext.DistributionhannelId && _.SalesOrganizationId == liftingRequestDetailsContext.SalesOrganizationId);
                            var divisionDataContext = _emamiContext.Divisions.AsNoTracking().FirstOrDefault(_ => _.Id == liftingRequestDetailsContext.DivisionId && _.SalesOrganizationId == liftingRequestDetailsContext.SalesOrganizationId &&
                            _.DistributionChannelId == liftingRequestDetailsContext.DistributionhannelId);

                            if (usersDataContext != null)
                            {
                                var contractOpenRequestRaise = new ContractOpenRequestRaiseDto
                                {
                                    UserId = usersDataContext.Id,
                                    UserCode = usersDataContext.Code,
                                    SalesOrganizationId = liftingRequestDetailsContext.SalesOrganizationId,
                                    DistributionChannelId = liftingRequestDetailsContext.DistributionhannelId,
                                    DivisionId = liftingRequestDetailsContext.DivisionId,
                                    SalesOrganizationCode = salesOrganizationDataContext.Code,
                                    DistributionChannelCode = distributionChannelDataContext.Code,
                                    DivisionCode = divisionDataContext.Code
                                };
                                ContractOpenRequestRaise(contractOpenRequestRaise);
                            }
                        }

                    }
                    else
                    {
                        errorMessageList.Add(Constants.InvalidRequest);
                    }

                    sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                    sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                    sapDataSyncResultDto.TotalInputRecordDetailsResponse = liftingRequestList;
                    sapDataSyncResultDto.SuccessRecordDetailsResponse = liftingRequestList.Except(errorRecordList).ToList();
                }

                if (errorMessageList.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Response = sapDataSyncResultDto;
                    resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList)}");
                }
                else
                {
                    if (liftingRequestList.Select(_ => _.Status).All(a => a))
                    {
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = Constants.SapSyncSuccessMessage;
                    }

                }
                sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = liftingRequestList;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                _logger.Error(message);
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }

        public void LiftingRequestDeliveryNoUpdate(List<HANASaudaCommonFunctionList> inputdto)
        {
            _methodName = "LiftingRequestDeliveryNoUpdate";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputdto)}");
            var resultDto = new ResultDto();
            var errorRecordList = new List<HANASaudaCommonFunctionList>();
            var errorMessageList = new List<string>();
            var dataSynced = 0;
            var synctype = "Delivery Order Number Update";
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var subject = string.Concat(synctype, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            long userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var liftingRequestList = inputdto != null ? inputdto : new List<HANASaudaCommonFunctionList>();
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = liftingRequestList.Count;
            try
            {
                var LiftingRequestDtoList = new List<LiftingRequestDto>();
                using (var _emamiContext = new AdaniContext())
                {
                    #region Get Common Data
                    var sapDocumentNo = liftingRequestList.Select(s => s.SAPRefDoc).Distinct().ToList();
                    LiftingRequestDtoList = _emamiContext.LiftingRequest.AsNoTracking()
                        .Where(_ => sapDocumentNo.Contains(_.SAPDocumentNo))
                        .Select(s => new LiftingRequestDto { SAPDeliveryNo = s.SAPDeliveryNo, SAPDocumentNo = s.SAPDocumentNo, LiftingId = s.Id }).ToList();
                    #endregion
                }
                if (liftingRequestList != null && liftingRequestList.Any())
                {
                    foreach (var liftingData in liftingRequestList)
                    {

                        if (!string.IsNullOrEmpty(liftingData.SAP_Document_No))
                        {
                            var liftingRequestContext = LiftingRequestDtoList.FirstOrDefault(_ => _.SAPDocumentNo == liftingData.SAPRefDoc);
                            if (liftingRequestContext != null)
                            {
                                if (liftingRequestContext.SAPDeliveryNo != null)
                                {
                                    var delivaryNumberList = liftingRequestContext.SAPDeliveryNo.Split(',');
                                    var delivaryNumber = delivaryNumberList.FirstOrDefault(stringToCheck => stringToCheck.Contains(liftingData.SAP_Document_No));
                                    if (delivaryNumber == null)
                                    {
                                        liftingData.SAP_Document_No = string.Concat(liftingRequestContext.SAPDeliveryNo, ",", liftingData.SAP_Document_No);
                                    }
                                    else
                                    {
                                        liftingData.SAP_Document_No = liftingRequestContext.SAPDeliveryNo;
                                    }
                                }
                            }
                            var errorFlag = true;
                            if (string.IsNullOrEmpty(liftingData.SAP_Document_No))
                            {
                                errorMessageList.Add(string.Format(Constants.SAPDeliveryNoIsMissing, liftingData.SAPRefDoc));
                                errorRecordList.Add(liftingData);
                                errorFlag = false;
                            }
                            if (string.IsNullOrEmpty(liftingData.SAPRefDoc))
                            {
                                errorMessageList.Add(string.Format(Constants.SAPDocumentNoIsEmpty, liftingData.SAPRefDoc));
                                errorRecordList.Add(liftingData);
                                errorFlag = false;
                            }
                            if (errorFlag)
                            {
                                var sqlUpdate = "UPDATE LiftingRequests SET SAPDeliveryNo = @SAPDeliveryNo, " +
                                       "ModifiedBy=@ModifiedBy,ModifiedDate = @ModifiedDate WHERE SapDocumentNo = @SAP_Document_No";
                                //var parameters = new[]{
                                //    new SqlParameter("@SAP_Document_No", liftingData.SAPRefDoc),
                                //    new SqlParameter("@ModifiedBy", userId),
                                //    new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow)),
                                //    new SqlParameter("@SAPDeliveryNo", liftingData.SAP_Document_No)
                                //};
                                //_emamiContext.BulkUpdateProxy(sqlUpdate, parameters);

                                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                {

                                    var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    var result = conn.Execute(sqlUpdate, new
                                    {
                                        SAPDeliveryNo = liftingData.SAP_Document_No,
                                        ModifiedBy = userId,
                                        ModifiedDate = modifiedDate,
                                        SAP_Document_No = liftingData.SAPRefDoc
                                    });

                                }
                                dataSynced++;
                                if (!liftingData.Status)
                                {
                                    errorRecordList.Add(liftingData);
                                }
                            }
                        }

                    }
                    //_emamiContext.SaveChanges();
                }
                else
                {
                    errorMessageList.Add(Constants.InvalidRequest);
                }

                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = liftingRequestList;
                sapDataSyncResultDto.SuccessRecordDetailsResponse = liftingRequestList.Except(errorRecordList).ToList();
                //}

                if (errorMessageList.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Response = sapDataSyncResultDto;
                    resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList)}");
                }
                else
                {
                    if (liftingRequestList.Select(_ => _.Status).All(a => a))
                    {
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = Constants.SapSyncSuccessMessage;
                    }

                }
                sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = liftingRequestList;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                _logger.Error(message);
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }

        public void GetLiftingRequestEnquiryNumberOutboundDetails(List<long> liftingRequestId, bool IsReprocess)
        {
            _methodName = "GetLiftingRequestEnquiryNumberOutboundDetails";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var liftingRequestViewDtoList = new HANALiftingRequestEnquiryNumber();
            var syncFolder = ConsoleSettings.LiftingInquiry;
            var subject = string.Concat(ConsoleSettings.LiftingInquiry, " ", ConsoleSettings.AppToSapDataSyncEmailSubject);
            var csvFileName = ConsoleSettings.InquiryrCsv;
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            try
            {
                using (var _emamiContext = new AdaniContext())
                {
                    if (IsReprocess)
                    {
                        var liftingRequestContext = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => liftingRequestId.Contains(_.Id)).ToList();
                        if (liftingRequestContext != null && liftingRequestContext.Any())
                        {
                            #region Get Common Datas
                            var customerList = _emamiContext.Users.AsNoTracking();
                            var liftingRequestDetailsList = _emamiContext.LiftingRequestDetails.AsNoTracking();
                            var skusList = _emamiContext.Skus.AsNoTracking();
                            var skuUomMappingList = _emamiContext.SkuUomMapping.AsNoTracking();
                            var vehicleLodabilityList = _emamiContext.VehicleLodability.AsNoTracking();
                            var PlantAndDepotList = _emamiContext.Depots.AsNoTracking();
                            var sauda = _emamiContext.Sauda.AsNoTracking();
                            var divisions = _emamiContext.Divisions.AsNoTracking();
                            // var salesOrganizationcontext = _emamiContext.SalesOrganization.AsNoTracking();
                            //var salesDocumentType = _emamiContext.SalesDocumentType.AsNoTracking();
                            //var salesDocumentType = _emamiContext.SalesDocumentType.AsNoTracking();
                            
                            #endregion
                            foreach (var liftingRequest in liftingRequestContext)
                            {
                                var customerContext = customerList.FirstOrDefault(_ => _.Id == liftingRequest.UserId);
                                var shipToParty = customerList.FirstOrDefault(_ => _.Id == liftingRequest.ShipToPartyId);
                                var plant = PlantAndDepotList.FirstOrDefault(_ => _.Id == liftingRequest.PlantId);

                                var saudaContext = new Sauda();
                                var liftingRequestDetailsContext = liftingRequestDetailsList.Where(_ => _.LiftingRequestId == liftingRequest.Id && _.StatusId != (int)DTO.Enums.Status.Rejected && _.StatusId != (int)DTO.Enums.Status.Deleted).ToList();
                                if (liftingRequestDetailsContext != null)
                                {
                                    var saudanumberfromSalesOrderTable = liftingRequestDetailsContext.FirstOrDefault().SaudaNumber;
                                    saudaContext = sauda.FirstOrDefault(_ => _.SaudaNumber == saudanumberfromSalesOrderTable);
                                }

                                var liftingRequestViewDto = new SalesOrderCreate
                                {
                                    TaskIdentification = "I",
                                    ImpigerRequestNo = liftingRequest.Id.ToString(),
                                    SalesOrg = saudaContext != null && saudaContext.SalesOrganization.Code != null ? saudaContext.SalesOrganization.Code : string.Empty,
                                    SoldTo = customerContext != null ? customerContext.Code : string.Empty,
                                    DocumentType = saudaContext != null ? divisions.FirstOrDefault(_ => _.Id == saudaContext.DivisionId).SalesOrderDocumentType : string.Empty,
                                    //SAPContractNo = liftingRequest.SaudaNumber,
                                    //RequiredQuantity = biddingQuantity,
                                    //MaterialNumber = skusContext != null ? skusContext.SkuCode : string.Empty,
                                    //UOM = skusContext.DivisionId == (int)DTO.Enums.LooseVertical.Loose ? ((liftingRequestDetails.OilType.Name == "CATTLE FEED SUPPLEMENT ( BERGAFAT T-300)" && liftingRequestDetails.Sku.PackType.Name == "BAG") ? "BAG" : (liftingRequestDetails.Sku.PackType.Name == "Loose") ? "MT" : string.Empty) : skusContext.PackGroupId == (int)PackGroupType.Premium ? "NOS" : "C/S",
                                    //CreatedDate = liftingRequest.CreatedDate,
                                    ShipTo = shipToParty != null ? shipToParty.ShipToPartyCode : string.Empty,
                                    //LiftingRequestDate = liftingRequest.CreatedDate,
                                    ApproveDate = liftingRequest.CreatedDate != null ? liftingRequest.CreatedDate.Date.ToString("dd.MM.yyyy") : "",
                                    //ApproveTime = liftingRequest.ModifiedDate != null ? liftingRequest.ModifiedDate.Value.TimeOfDay : DateTime.MinValue.TimeOfDay,
                                    CustomerText = liftingRequest.CustomerRemarks,
                                    //VehicleSize = vehicleLodabilityContext != null ? vehicleLodabilityContext.VehicleSize : 0,
                                    //Payer = customerContext != null ? customerContext.Code : string.Empty,
                                    //VerticalCode = skusContext != null ? skusContext.Division.Code : string.Empty,
                                    //DepotCode = liftingRequest.PlantId != 0 ? PlantAndDepotList.FirstOrDefault(_ => _.Id == liftingRequest.PlantId).Code : liftingRequest.DepotId != 0 ? (PlantAndDepotList.FirstOrDefault(_ => _.Id == liftingRequest.DepotId).Code) : string.Empty,
                                    //CustomerGroupOneCode = customerContext.CustomerGroupOneId > 0 ? CustomerGroupOne.FirstOrDefault(_ => _.Id == customerContext.CustomerGroupOneId).GroupCode : string.Empty
                                    };
                                var _SAPDataItemDataList = new List<ItemDataDTO>();

                                if (liftingRequestDetailsContext != null)
                                {
                                    foreach (var liftingRequestDetails in liftingRequestDetailsContext)
                                    {
                                        var skusContext = skusList.FirstOrDefault(_ => _.Id == liftingRequestDetails.SkuId);
                                        decimal biddingQuantity = liftingRequestDetails.LiftingQuantityCase;
                                        //var skuUomId = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skusContext.Id).UomId;
                                        //var UOM = _emamiContext.Uom.AsNoTracking().FirstOrDefault(_ => _.Id == skuUomId).SAPName;
                                        long? skuUomId = 0;
                                        if (skusContext != null)
                                        {
                                            var skuUomMapping = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skusContext.Id);
                                            if (skuUomMapping != null)
                                            {
                                                skuUomId = skuUomMapping.UomId;
                                            }
                                        }
                                        string UOM = string.Empty;
                                        if (skuUomId.HasValue)
                                        {
                                            var uom = _emamiContext.Uom.AsNoTracking().FirstOrDefault(_ => _.Id == skuUomId.Value);
                                            if (uom != null)
                                            {
                                                UOM = uom.SAPName;
                                            }
                                        }
                                        var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == liftingRequestDetails.SaudaOrderId);
                                        var itemNo = saudaOrderContext != null ? saudaOrderContext.SaudaNumber : string.Empty;
                                        //if (skusContext.PackGroupId == (int)PackGroupType.Premium)
                                        //{
                                        //    var skuUomMapping = skuUomMappingList.FirstOrDefault(_ => _.SkuId == liftingRequestDetails.SkuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                                        //    if (skuUomMapping != null)
                                        //    {
                                        //        biddingQuantity = liftingRequestDetails.LiftingQuantityCase * skuUomMapping.ConversionFactor;
                                        //    }
                                        //}
                                        var _SAPDataItemData = new ItemDataDTO
                                        {
                                            Qty = biddingQuantity.ToString("#.###"),
                                            UOM = UOM ?? "",
                                            Material = skusContext.SkuCode ?? "",
                                            Plant = plant.Code,
                                            ItemNo = liftingRequestDetails.SaudaNumber + "/" + itemNo
                                        };

                                        _SAPDataItemDataList.Add(_SAPDataItemData);


                                        //liftingRequestViewDtoList.Header.Add(liftingRequestViewDto);
                                    }
                                }

                                liftingRequestViewDto.ItemData.AddRange(_SAPDataItemDataList);

                                var sqlUpdate = "UPDATE LiftingRequests SET IsSAPDataSync = @IsSAPDataSync , ModifiedDate = @ModifiedDate WHERE Id = @Id";
                                //var parameters = new[]{
                                //    new SqlParameter("@IsSAPDataSync", true),
                                //    new SqlParameter("@Id", liftingRequest.Id),
                                //    new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow))
                                //};
                                //_emamiContext.BulkUpdateProxy(sqlUpdate, parameters);


                                                                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                {

                                    var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    var result = conn.Execute(sqlUpdate, new
                                    {
                                        IsSAPDataSync = true,
                                        ModifiedDate = modifiedDate,
                                        Id = liftingRequest.Id
                                    });

                                }


                                var json = JsonConvert.SerializeObject(liftingRequestViewDto, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                                //_logger.Info($"Json Input : {JsonConvert.SerializeObject(liftingRequestViewDto)}");
                                //saudaViewDtoList.Header.Add(saudaViewDto);
                                var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.LiftingInquiryHanaApiUrl, liftingRequestViewDto);
                                var status = response.StatusCode;
                            }
                            _emamiContext.SaveChanges();
                            sapDataSyncResultDto.OutstandingResult.DataRetrieved = liftingRequestViewDtoList.Header.Count;
                            sapDataSyncResultDto.OutstandingResult.DataSynced = liftingRequestViewDtoList.Header.Count;
                            sapDataSyncResultDto.TotalInputRecordDetailsResponse = liftingRequestViewDtoList.Header;
                            //if (liftingRequestViewDtoList.Header.IsAny())
                            //{
                            //    var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.LiftingInquiryHanaApiUrl, liftingRequestViewDtoList);
                            //    var status = response.StatusCode;
                            //    sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                            //    if (status.ToString() == "Accepted")
                            //    {
                            //        resultDto.IsSuccess = true;
                            //        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                            //        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                            //    }
                            //    else
                            //    {
                            //        resultDto.IsSuccess = false;
                            //        resultDto.ErrorDto.Message = "Lifting Enquiry data sent to SAP Failed" + status.ToString();
                            //        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                            //    }
                            //    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, null, subject);
                            //}
                            }
                    }
                    else
                    {
                        var liftingRequestContext = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => _.IsSAPDataSync == false && _.StatusId == (int)DTO.Enums.Status.Approved && liftingRequestId.Contains(_.Id)).ToList();
                        if (liftingRequestContext != null && liftingRequestContext.Any())
                        {
                            #region Get Common Datas
                            var customerList = _emamiContext.Users.AsNoTracking();
                            var liftingRequestDetailsList = _emamiContext.LiftingRequestDetails.AsNoTracking();
                            var skusList = _emamiContext.Skus.AsNoTracking();
                            var skuUomMappingList = _emamiContext.SkuUomMapping.AsNoTracking();
                            var vehicleLodabilityList = _emamiContext.VehicleLodability.AsNoTracking();
                            var PlantAndDepotList = _emamiContext.Depots.AsNoTracking();
                            var sauda = _emamiContext.Sauda.AsNoTracking();
                            var divisions = _emamiContext.Divisions.AsNoTracking();
                            // var salesOrganizationcontext = _emamiContext.SalesOrganization.AsNoTracking();
                            //var salesDocumentType = _emamiContext.SalesDocumentType.AsNoTracking();
                            var usercustomeMappingContext = _emamiContext.UserCustomerMapping.AsNoTracking();

                            #endregion
                            foreach (var liftingRequest in liftingRequestContext)
                            {
                                var customerContext = customerList.FirstOrDefault(_ => _.Id == liftingRequest.UserId);
                                var shipToParty = customerList.FirstOrDefault(_ => _.Id == liftingRequest.ShipToPartyId);
                                var plant = PlantAndDepotList.FirstOrDefault(_ => _.Id == liftingRequest.PlantId);

                                var saudaContext = new Sauda();
                                var liftingRequestDetailsContext = liftingRequestDetailsList.Where(_ => _.LiftingRequestId == liftingRequest.Id && _.StatusId != (int)DTO.Enums.Status.Rejected && _.StatusId != (int)DTO.Enums.Status.Deleted).ToList();
                                if (liftingRequestDetailsContext != null)
                                {
                                    var saudanumberfromSalesOrderTable = liftingRequestDetailsContext.FirstOrDefault().SaudaNumber;
                                    saudaContext = sauda.FirstOrDefault(_ => _.SaudaNumber == saudanumberfromSalesOrderTable);
                                }

                                var liftingRequestViewDto = new SalesOrderCreate
                                {
                                    ImpigerRequestNo = liftingRequest.Id.ToString(),
                                    SalesOrg = saudaContext != null && saudaContext.SalesOrganization != null && saudaContext.SalesOrganization.Code != null ? saudaContext.SalesOrganization.Code : string.Empty,
                                    SoldTo = customerContext != null ? customerContext.Code : string.Empty,
                                    DocumentType = saudaContext != null ? divisions.FirstOrDefault(_ => _.Id == saudaContext.DivisionId).SalesOrderDocumentType : string.Empty,
                                    // SAPContractNo = liftingRequest.SaudaNumber,
                                    //RequiredQuantity = biddingQuantity,
                                    //MaterialNumber = skusContext != null ? skusContext.SkuCode : string.Empty,
                                    //UOM = skusContext.DivisionId == (int)DTO.Enums.LooseVertical.Loose ? ((liftingRequestDetails.OilType.Name == "CATTLE FEED SUPPLEMENT ( BERGAFAT T-300)" && liftingRequestDetails.Sku.PackType.Name == "BAG") ? "BAG" : (liftingRequestDetails.Sku.PackType.Name == "Loose") ? "MT" : string.Empty) : skusContext.PackGroupId == (int)PackGroupType.Premium ? "NOS" : "C/S",
                                    //CreatedDate = liftingRequest.CreatedDate,
                                    ShipTo = shipToParty != null ? shipToParty.ShipToPartyCode : string.Empty,
                                    //LiftingRequestDate = liftingRequest.CreatedDate,
                                    ApproveDate = liftingRequest.CreatedDate != null ? liftingRequest.CreatedDate.Date.ToString("dd.MM.yyyy") : "",
                                    //ApproveTime = liftingRequest.ModifiedDate != null ? liftingRequest.ModifiedDate.Value.TimeOfDay : DateTime.MinValue.TimeOfDay,
                                    CustomerText = liftingRequest.CustomerRemarks,
                                    TaskIdentification = "I"
                                    //VehicleSize = vehicleLodabilityContext != null ? vehicleLodabilityContext.VehicleSize : 0,
                                    //Payer = customerContext != null ? customerContext.Code : string.Empty,
                                    //VerticalCode = skusContext != null ? skusContext.Division.Code : string.Empty,
                                    //DepotCode = liftingRequest.PlantId != 0 ? PlantAndDepotList.FirstOrDefault(_ => _.Id == liftingRequest.PlantId).Code : liftingRequest.DepotId != 0 ? (PlantAndDepotList.FirstOrDefault(_ => _.Id == liftingRequest.DepotId).Code) : string.Empty,
                                    //CustomerGroupOneCode = customerContext.CustomerGroupOneId > 0 ? CustomerGroupOne.FirstOrDefault(_ => _.Id == customerContext.CustomerGroupOneId).GroupCode : string.Empty
                                   };
                                var _SAPDataItemDataList = new List<ItemDataDTO>();

                                if (liftingRequestDetailsContext != null)
                                {
                                    foreach (var liftingRequestDetails in liftingRequestDetailsContext)
                                    {
                                        var skusContext = skusList.FirstOrDefault(_ => _.Id == liftingRequestDetails.SkuId);
                                        decimal biddingQuantity = liftingRequestDetails.LiftingQuantityCase;
                                        //var skuUomId = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skusContext.Id).UomId;
                                        //var UOM = _emamiContext.Uom.AsNoTracking().FirstOrDefault(_ => _.Id == skuUomId).SAPName;
                                        long? skuUomId = 0;
                                        if (skusContext != null)
                                        {
                                            var skuUomMapping = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skusContext.Id);
                                            if (skuUomMapping != null)
                                            {
                                                skuUomId = skuUomMapping.UomId;
                                            }
                                        }
                                        string UOM = string.Empty;
                                        if(skuUomId.HasValue)
                                        {
                                            var uom = _emamiContext.Uom.AsNoTracking().FirstOrDefault(_ => _.Id == skuUomId.Value);
                                            if (uom != null)
                                            {
                                                UOM = uom.SAPName;
                                            }
                                        }
                                        var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == liftingRequestDetails.SaudaOrderId);
                                        var itemNo = saudaOrderContext != null ? saudaOrderContext.SaudaNumber : string.Empty;
                                        //if (skusContext.PackGroupId == (int)PackGroupType.Premium)
                                        //{
                                        //    var skuUomMapping = skuUomMappingList.FirstOrDefault(_ => _.SkuId == liftingRequestDetails.SkuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                                        //    if (skuUomMapping != null)
                                        //    {
                                        //        biddingQuantity = liftingRequestDetails.LiftingQuantityCase * skuUomMapping.ConversionFactor;
                                        //    }
                                        //}
                                        var _SAPDataItemData = new ItemDataDTO
                                        {
                                            Qty = biddingQuantity.ToString("#.###"),
                                            UOM = UOM ?? "",
                                            Material = skusContext.SkuCode ?? "",
                                            Plant = plant.Code,
                                            ItemNo = liftingRequestDetails.SaudaNumber + "/" + itemNo
                                        };

                                        _SAPDataItemDataList.Add(_SAPDataItemData);


                                        //liftingRequestViewDtoList.Header.Add(liftingRequestViewDto);
                                    }
                                }

                                liftingRequestViewDto.ItemData.AddRange(_SAPDataItemDataList);

                                var sqlUpdate = "UPDATE LiftingRequests SET IsSAPDataSync = @IsSAPDataSync , ModifiedDate = @ModifiedDate WHERE Id = @Id";
                                //var parameters = new[]{
                                //    new SqlParameter("@IsSAPDataSync", true),
                                //    new SqlParameter("@Id", liftingRequest.Id),
                                //    new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow))
                                //};
                                //_emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                
                                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                {

                                    var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    var result = conn.Execute(sqlUpdate, new
                                    {
                                        IsSAPDataSync = true,
                                        ModifiedDate = modifiedDate,
                                        Id = liftingRequest.Id
                                    });

                                }

                                var json = JsonConvert.SerializeObject(liftingRequestViewDto, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                                //_logger.Info($"Json Input : {JsonConvert.SerializeObject(liftingRequestViewDto)}");
                                //saudaViewDtoList.Header.Add(saudaViewDto);
                                var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.LiftingInquiryHanaApiUrl, liftingRequestViewDto);
                                var status = response.StatusCode;
                            }
                            _emamiContext.SaveChanges();
                            sapDataSyncResultDto.OutstandingResult.DataRetrieved = liftingRequestViewDtoList.Header.Count;
                            sapDataSyncResultDto.OutstandingResult.DataSynced = liftingRequestViewDtoList.Header.Count;
                            sapDataSyncResultDto.TotalInputRecordDetailsResponse = liftingRequestViewDtoList.Header;
                            //if (liftingRequestViewDto != null)
                            //{
                            //    var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.LiftingInquiryHanaApiUrl, liftingRequestViewDtoList);
                            //    var status = response.StatusCode;
                            //    sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                            //    if (status.ToString() == "Accepted")
                            //    {
                            //        resultDto.IsSuccess = true;
                            //        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                            //        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                            //    }
                            //    else
                            //    {
                            //        resultDto.IsSuccess = false;
                            //        resultDto.ErrorDto.Message = "Lifting Enquiry data sent to SAP Failed" + status.ToString();
                            //        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                            //    }
                            //resultDto.IsSuccess = true;
                            //resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                            //resultDto.SuccessDto.Response = sapDataSyncResultDto;
                            //_sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, null, subject);
                            //}
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = liftingRequestViewDtoList.Header;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                _sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
            }
        }

        public void LiftRequestCreateSapToApp(SalesOrderCreate salesOrderCreate)
        {
            _methodName = "LiftRequestCreateSapToApp";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(salesOrderCreate)}");
            var resultDto = new ResultDto();
            var errorModelList = new List<SalesOrderCreate>();
            var errorMessageList = new List<string>();
            var errorMessageList1 = new List<string>();
            var dataSynced = 0;
            var inputdto = new SAPDataResponseDto();
            var synctype = ConsoleSettings.LiftingInquiry;
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var subject = string.Concat(ConsoleSettings.LiftingInquiry, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            long userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var salesOrderCreateList = salesOrderCreate != null && salesOrderCreate != null ? salesOrderCreate : new SalesOrderCreate();
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = salesOrderCreate.ItemData.Count > 0 ? salesOrderCreate.ItemData.Count : 1; // saudaViewDtoList.Count;
            var inputData = new List<SalesOrderCreate>();
            var successList = new List<SalesOrderCreate>();
            var errorList = new List<SalesOrderCreate>();
            SalesOrderCreate successData = new SalesOrderCreate();
            //successData.ItemData=new List<ItemDataDTO>();
            SalesOrderCreate errorData = new SalesOrderCreate();

            //errorData.ItemData= new List<ItemDataDTO>(); ;
            inputData.Add(salesOrderCreate);
            try
            {
                if (salesOrderCreateList != null)
                {
                    using (var _emamiContext = new AdaniContext())
                    {
                        var errorFlag = true;
                        var errorMessage = string.Empty;

                        #region CommonData 
                        var skuContext = _emamiContext.Skus.AsNoTracking();
                        var PlantAndDepotList = _emamiContext.Depots.AsNoTracking();

                        var salesOrganisationId = _emamiContext.SalesOrganization.AsNoTracking().Where(_ => _.Code == salesOrderCreateList.SalesOrg.ToString()).Select(_ => _.Id).FirstOrDefault();
                        if (salesOrderCreateList.SoldTo != null)
                        {
                            salesOrderCreateList.SoldTo = salesOrderCreateList.SoldTo.TrimStart('0');
                        }


                        var saudaSoldToParty = salesOrderCreateList.SoldTo;

                        var UsersDataSoldParty = (from s in _emamiContext.Users
                                                  join role in _emamiContext.UserRoles on s.Id equals role.UserId
                                                  join d in _emamiContext.UserDivisionMappings on s.Id equals d.UserId
                                                  where saudaSoldToParty == (s.Code) && role.RoleId != (int)DTO.Enums.Role.ShipToParty
                                                  select new { s.Id, s.Code, s.SaudaBookingTypeId, d.DivisionId, d.SalesOrganizationId, d.DistributionChannelId }).ToList();

                        var saudaUom = salesOrderCreateList.ItemData.Select(x => x.UOM);
                        var UomData = _emamiContext.Uom.AsNoTracking()
                            .Where(_ => saudaUom.Contains(_.SAPName))
                            .Select(s => new { Id = s.Id, SAPName = s.SAPName });

                        #endregion

                        //if (string.IsNullOrEmpty(salesOrderCreateList.SAPContractNo))
                        //{
                        //    errorMessage = Constants.BindErrorMessage(Constants.SaudaNumberIsEmpty, errorMessage);
                        //    errorFlag = false;
                        //}

                        if (salesOrganisationId == 0)
                        {
                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.SalesOrganisationIsEmpty, salesOrderCreateList.SalesOrg), errorMessage);
                            errorFlag = false;
                        }

                        var soldToPartyContext = UsersDataSoldParty.FirstOrDefault(_ => _.Code == salesOrderCreateList.SoldTo
                           && _.SalesOrganizationId == salesOrganisationId);

                        if (soldToPartyContext == null)
                        {
                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.SoldToPartyIsNotEmpty, salesOrderCreateList.SoldTo), errorMessage);
                            errorFlag = false;
                        }
                        var plant = salesOrderCreate.ItemData.Select(_ => _.Plant).FirstOrDefault();
                        var plantContext = PlantAndDepotList.FirstOrDefault(_ => _.Code == plant);
                        if (plantContext == null)
                        {
                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.PlantNotExist, plant), errorMessage);
                            errorFlag = false;
                        }
                        if (errorFlag)
                        {
                            var liftingContext = _emamiContext.LiftingRequest.FirstOrDefault(_ => _.SAPDocumentNo == salesOrderCreate.SalesOrderNo);
                            long liftingRequestId = 0;

                            var approveDate = DateTime.ParseExact(salesOrderCreate.ApproveDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                            using (DbContextTransaction transaction = _emamiContext.Database.BeginTransaction())
                            {
                                try
                                {
                                    if (liftingContext == null)
                                    {
                                        var salesordecreate = new LiftingRequest
                                        {
                                            UserId = soldToPartyContext.Id,
                                            ApproveDate = approveDate,
                                            LiftingDate = approveDate,
                                            IsSAPDataSync = true,
                                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                            CreatedBy = userId,
                                            //SaudaNumber = salesOrderCreate.SAPContractNo,
                                            CustomerRemarks = salesOrderCreate.CustomerText,
                                            PlantId = plantContext.Id,
                                            SAPDocumentNo = salesOrderCreate.SalesOrderNo,
                                            StatusId = (int)DTO.Enums.Status.Approved,
                                            // SaudaId = saudaData != null ? saudaData.Id : 0
                                            IsSAPSalesOrder = true
                                        };
                                        _emamiContext.LiftingRequest.Add(salesordecreate);
                                        _emamiContext.SaveChanges();
                                        liftingRequestId = salesordecreate.Id;
                                        salesordecreate.LiftingRequestNumber = salesordecreate.Id.ToString();
                                    }
                                    else
                                    {
                                        liftingRequestId = liftingContext.Id;
                                        liftingContext.UserId = soldToPartyContext.Id;
                                        liftingContext.ApproveDate = approveDate;
                                        liftingContext.LiftingDate = approveDate;
                                        liftingContext.CustomerRemarks = salesOrderCreate.CustomerText;
                                        //  liftingContext.SaudaNumber = salesOrderCreate.SAPContractNo;
                                        liftingContext.PlantId = PlantAndDepotList.FirstOrDefault(_ => _.Code == plant).Id;
                                        liftingContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                        liftingContext.ModifiedBy = userId;
                                        liftingContext.StatusId = (int)DTO.Enums.Status.Approved;
                                        liftingContext.IsSAPSalesOrder = true;
                                        //  liftingContext.SaudaId = saudaData != null ? saudaData.Id : 0;
                                        _emamiContext.SaveChanges();
                                    }

                                    foreach (var salesorderdetails in salesOrderCreate.ItemData)
                                    {
                                        var errorFlag1 = true;
                                        errorMessage = string.Empty;
                                        salesorderdetails.ItemNo = salesorderdetails.ItemNo.TrimStart('0');
                                        if (salesorderdetails.UOM == "KAR")
                                        {
                                            salesorderdetails.UOM = "CAR";
                                        }
                                        var uomContext = UomData.FirstOrDefault(_ => _.SAPName == salesorderdetails.UOM);
                                        if (uomContext == null)
                                        {
                                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.UOMCodeIsNotEmpty, salesorderdetails.UOM), errorMessage);
                                            errorFlag1 = false;
                                        }
                                        if (string.IsNullOrEmpty(salesorderdetails.ItemNo))
                                        {
                                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.SaudaNumberIsNotEmpty, salesorderdetails.ItemNo) + " for " + salesorderdetails.Material, errorMessage);
                                            errorFlag1 = false;
                                        }
                                        var saudanumber = string.Empty;
                                        var itemno = string.Empty;

                                        saudanumber = salesorderdetails.ItemNo.Split('/').FirstOrDefault();
                                        itemno = salesorderdetails.ItemNo.Split('/').LastOrDefault();
                                        itemno = itemno.TrimStart('0');

                                        if (string.IsNullOrEmpty(saudanumber) && string.IsNullOrEmpty(itemno))
                                        {
                                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.SaudaNumberIsNotEmpty, saudanumber), errorMessage);
                                            errorFlag1 = false;
                                            //errorMessageList1.Add(errorMessage);
                                        }

                                        var saudaData = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == saudanumber);
                                        var sku = new Sku();
                                        if (saudaData == null)
                                        {
                                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.SaudaNumberIsNotEmpty, saudanumber), errorMessage);
                                            errorFlag1 = false;
                                            errorFlag = false;
                                        }
                                        else
                                        {
                                            sku = skuContext.FirstOrDefault(_ => _.SkuCode == salesorderdetails.Material && _.SalesOrganizationId == saudaData.SalesOrganizationId && _.DistributionChannelId == saudaData.DistributionChannelId && _.DivisionId == saudaData.DivisionId);

                                            if (sku == null)
                                            {
                                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.SkuDetailsIsEmpty, salesorderdetails.Material), errorMessage);
                                                errorFlag1 = false;
                                                errorFlag = false;
                                            }
                                        }
                                        if (errorFlag1)
                                        {

                                            var saudaOrderdata = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.SaudaId == saudaData.Id && _.SkuId == sku.Id);
                                            var liftingRequestDetailsContext = _emamiContext.LiftingRequestDetails.FirstOrDefault(_ => _.LiftingRequestId == liftingRequestId && sku.Id == _.SkuId && _.ItemNo == itemno && _.SaudaNumber == saudaData.SaudaNumber);
                                            if (liftingRequestDetailsContext == null)
                                            {

                                                var liftingrequestdetailsContext = new LiftingRequestDetails()
                                                {
                                                    ItemNo = itemno,
                                                    SkuId = sku.Id,
                                                    UomId = uomContext.Id,
                                                    LiftingQuantityCase = Convert.ToDecimal(salesorderdetails.Qty),
                                                    LiftingQuantity = _resultService.ConvertCasetoMetricTon(Convert.ToDecimal(salesorderdetails.Qty), sku.Id),
                                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                    CreatedBy = userId,
                                                    LiftingRequestId = liftingRequestId,
                                                    OilTypeId = sku.OilTypeId ?? 0,
                                                    SaudaNumber = saudanumber,
                                                    SaudaOrderId = saudaOrderdata != null ? saudaOrderdata.Id : 0,
                                                    SalesOrganizationId = saudaData != null ? saudaData.SalesOrganizationId : 0,
                                                    DistributionhannelId = saudaData != null ? saudaData.DistributionChannelId : 0,
                                                    DivisionId = saudaData != null ? saudaData.DivisionId : 0
                                                };
                                                _emamiContext.LiftingRequestDetails.Add(liftingrequestdetailsContext);

                                            }
                                            else
                                            {
                                                liftingRequestDetailsContext.LiftingQuantityCase = Convert.ToDecimal(salesorderdetails.Qty);
                                                liftingRequestDetailsContext.LiftingQuantity = _resultService.ConvertCasetoMetricTon(Convert.ToDecimal(salesorderdetails.Qty), sku.Id);
                                                liftingRequestDetailsContext.SkuId = sku.Id;
                                                liftingRequestDetailsContext.UomId = uomContext.Id;
                                                liftingRequestDetailsContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                                liftingRequestDetailsContext.ModifiedBy = userId;
                                                liftingRequestDetailsContext.OilTypeId = sku.OilTypeId ?? 0;
                                                liftingRequestDetailsContext.SalesOrganizationId = saudaData.SalesOrganizationId;
                                                liftingRequestDetailsContext.DistributionhannelId = saudaData.DistributionChannelId;
                                                liftingRequestDetailsContext.DivisionId = saudaData.DivisionId;
                                            }
                                            successData.ItemData.Add(salesorderdetails);
                                            // Contract Limit Check Update
                                            ContractAvilableLimitCalculate(saudanumber);
                                            dataSynced++;
                                        }
                                        else
                                        {
                                            errorData.ItemData.Add(salesorderdetails);
                                            errorMessageList1.Add(errorMessage);
                                            //errorMessageList1.Add(errorMessage + " \"  " + saudaorder.Material + " \"  ");
                                            //errorModelList = (sauda);
                                        }
                                    }

                                    var usersDataContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.Id == soldToPartyContext.Id);
                                    var salesOrganizationDataContext = _emamiContext.SalesOrganization.AsNoTracking().FirstOrDefault(_ => _.Id == soldToPartyContext.SalesOrganizationId);
                                    var distributionChannelDataContext = _emamiContext.DistributionChannel.AsNoTracking().FirstOrDefault(_ => _.Id == soldToPartyContext.DistributionChannelId && _.SalesOrganizationId == soldToPartyContext.SalesOrganizationId);
                                    var divisionDataContext = _emamiContext.Divisions.AsNoTracking().FirstOrDefault(_ => _.Id == soldToPartyContext.DivisionId && _.SalesOrganizationId == soldToPartyContext.SalesOrganizationId &&
                                    _.DistributionChannelId == soldToPartyContext.DistributionChannelId);

                                    if (usersDataContext != null)
                                    {
                                        var contractOpenRequestRaise = new ContractOpenRequestRaiseDto
                                        {
                                            UserId = usersDataContext.Id,
                                            UserCode = usersDataContext.Code,
                                            SalesOrganizationId = soldToPartyContext.SalesOrganizationId,
                                            DistributionChannelId = soldToPartyContext.DistributionChannelId,
                                            DivisionId = soldToPartyContext.DivisionId,
                                            SalesOrganizationCode = salesOrganizationDataContext.Code,
                                            DistributionChannelCode = distributionChannelDataContext.Code,
                                            DivisionCode = divisionDataContext.Code
                                        };
                                        ContractOpenRequestRaise(contractOpenRequestRaise);
                                    }

                                    if (errorMessageList1.Count == salesOrderCreate.ItemData.Count)
                                    {
                                        transaction.Rollback();
                                    }
                                    else
                                    {
                                        _emamiContext.SaveChanges();
                                        transaction.Commit();
                                    }

                                }
                                catch (Exception exception)
                                {
                                    transaction.Rollback();
                                    var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                                    resultDto.IsSuccess = false;
                                    _logger.Error("Sales Order SAP to APP Exception" + message);
                                    sapDataSyncResultDto.TotalInputRecordDetailsResponse = inputData;
                                    resultDto.ErrorDto.ErrorCode = Constants.Exception;
                                    resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                                    sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    sapDataSyncResultDto.ExceptionMessage = message;
                                    resultDto.ErrorDto.Response = sapDataSyncResultDto;
                                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
                                }
                            }
                        }  //dataSynced++;
                        else
                        {
                            errorMessageList.Add(errorMessage);
                            errorModelList = inputData;
                        }
                        if (successData.ItemData.Count > 0)
                        {
                            successList.Add(new SalesOrderCreate()
                            {
                                TaskIdentification = salesOrderCreate.TaskIdentification,
                                SAPContractNo = salesOrderCreate.SAPContractNo,
                                ImpigerRequestNo = salesOrderCreate.ImpigerRequestNo,
                                DocumentType = salesOrderCreate.DocumentType,
                                SalesOrg = salesOrderCreate.SalesOrg,
                                SoldTo = salesOrderCreate.SoldTo,
                                ShipTo = salesOrderCreate.ShipTo,
                                ApproveDate = salesOrderCreate.ApproveDate,
                                CustomerText = salesOrderCreate.CustomerText,
                                SalesOrderNo = salesOrderCreate.SalesOrderNo,
                                ItemData = successData.ItemData
                            });

                        }

                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = inputData; // saudaViewDtoList;
                        sapDataSyncResultDto.SuccessRecordDetailsResponse = successList;
                    }

                    if (errorMessageList.Any() || errorMessageList1.Any())
                    {
                        if (errorData.ItemData.Count > 0)
                        {
                            errorList.Add(new SalesOrderCreate()
                            {
                                TaskIdentification = salesOrderCreate.TaskIdentification,
                                SAPContractNo = salesOrderCreate.SAPContractNo,
                                ImpigerRequestNo = salesOrderCreate.ImpigerRequestNo,
                                DocumentType = salesOrderCreate.DocumentType,
                                SalesOrg = salesOrderCreate.SalesOrg,
                                SoldTo = salesOrderCreate.SoldTo,
                                ShipTo = salesOrderCreate.ShipTo,
                                ApproveDate = salesOrderCreate.ApproveDate,
                                CustomerText = salesOrderCreate.CustomerText,
                                SalesOrderNo = salesOrderCreate.SalesOrderNo,
                                ItemData = errorData.ItemData
                            });
                        }
                        else
                        {
                            errorList.AddRange(errorModelList);
                        }

                        sapDataSyncResultDto.ErrorDetailsResponse = errorList;
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                        resultDto.ErrorDto.Message = resultDto.ErrorDto.Message + " " + string.Join(",", errorMessageList1);
                        _logger.Info($"Error Message : {resultDto.ErrorDto.Message}");
                    }
                    else
                    {
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;

                        //if (errorMessageList1.Any())
                        //{
                        //    resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        //    resultDto.ErrorDto.Message = string.Join(",", errorMessageList1);
                        //    _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList1)}");
                        //}
                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
                }
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = inputData;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }

        #endregion

        #region Invoice
        /// <summary>
        /// Method to save invoice
        /// </summary>
        /// <param name="inputdto"></param>
        /// <returns></returns>
        public void SaveInvoice(InvoiceDto inputdto)
        {
            _methodName = "SaveInvoice";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputdto)}");
            var resultDto = new ResultDto();
            var errorRecordList = new List<HANASAPInvoiceDto>();
            var errorMessageList = new List<string>();
            var dataSynced = 0;
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var synctype = ConsoleSettings.InvoiceFolder;
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var subject = string.Concat(ConsoleSettings.InvoiceFolder, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            long userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var invoiceDto = inputdto != null ? inputdto : new InvoiceDto();
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = inputdto.ItemData.Count;


            var inputList = new List<InvoiceDto>();
            inputList.Add(invoiceDto);

            var successList = new List<InvoiceDto>();
            var errorList = new List<InvoiceDto>();

            var errorData = new InvoiceDto();
            var successData = new InvoiceDto();

            try
            {
                var invoiceList = new List<Invoice>();
                var errorInvoiceList = new List<HANASAPInvoiceDto>();
                var invoiceSalesOrderList = new List<InvoiceSalesOrder>();


                if (invoiceDto != null)
                {
                    //foreach (var item in invoiceDtoList)
                    //{
                    //    var sourcePath = RemotePath.EscapeFileMask(folderPath + "/" + item.PdfFileName);
                    //    inputDto.SourceFileName.Add(sourcePath);
                    //}
                    using (var _emamiContext = new AdaniContext())
                    {
                        #region Get Common Datas                      

                        var invBillingDocument = invoiceDto.SAPDocNumber;
                        var InvoicesData = _emamiContext.Invoices.AsNoTracking()
                            .Where(_ => invBillingDocument.Contains(_.BillingDocument))
                            .Select(s => new { Id = s.Id, BillingDocument = s.BillingDocument, s.SAPDocumentNo });

                        var invoiceIds = InvoicesData.Select(s => s.Id).ToList();
                        var InvoiceDetailsData = _emamiContext.InvoiceDetails.AsNoTracking()
                            .Where(_ => invoiceIds.Contains(_.InvoiceId))
                            .Select(s => new { Id = s.Id, InvoiceId = s.InvoiceId, MaterialNumber = s.MaterialNumber, s.ItemNo });

                        var invMaterialNumber = invoiceDto.ItemData.Select(s => s.Material).Distinct().ToList();
                        var SkusData = _emamiContext.Skus.AsNoTracking()
                            .Where(_ => invMaterialNumber.Contains(_.SkuCode))
                            .Select(s => new { Id = s.Id, SkuCode = s.SkuCode, OilTypeId = s.OilType.Id });

                        //var liftingRequestId = long.Parse(inputdto.ImpigerReqNo);

                        #endregion

                        subject = string.Concat(ConsoleSettings.InvoiceFolder, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
                        var errorFlag = true;
                        var errorMessage = invoiceDto.SAPRefDoc + "-" + invoiceDto.SAPRefDoc;
                        if (invoiceDto == null)
                        {
                            errorMessage = Constants.BindErrorMessage(Constants.InvalidRequest, errorMessage);
                            errorFlag = false;
                        }
                        if (string.IsNullOrEmpty(invoiceDto.SAPRefDoc))
                        {
                            errorMessage = Constants.BindErrorMessage(Constants.InvoiceIsEmpty, errorMessage);
                            errorFlag = false;
                        }
                        if (string.IsNullOrEmpty(invoiceDto.SAPDocNumber))
                        {
                            errorMessage = Constants.BindErrorMessage(Constants.SapDocumentNoIsEmpty, errorMessage);
                            errorFlag = false;
                        }

                        if (errorFlag)
                        {
                            var salesOrderList = invoiceDto.SAPRefDoc.Split(',');
                            if (salesOrderList != null && salesOrderList.Any())
                            {
                                long invoiceId = 0;
                                foreach (var sapRefDoc in salesOrderList)
                                {
                                    var sapDocumentNo = sapRefDoc.TrimStart();
                                    var LiftingRequestData = _emamiContext.LiftingRequest.AsNoTracking().OrderByDescending(_ => _.SAPDocumentNo == sapDocumentNo).FirstOrDefault();
                                    var invoiceContextSave = InvoicesData.FirstOrDefault(_ => _.BillingDocument == invoiceDto.SAPDocNumber && _.SAPDocumentNo == sapDocumentNo);
                                    //var dueDate = DateTime.ParseExact(invoiceDto.DueDate.Replace('.', '/'), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    var invoiceDate = DateTime.ParseExact(invoiceDto.Invoice_Date.Replace('.', '/'), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    if (invoiceContextSave != null)
                                    {
                                        var sqlUpdate = "UPDATE Invoices SET UserId =@UserId,BillingDocument = @BillingDocument,InvoiceDate = @InvoiceDate,TotalInvoice = @TotalInvoice,SAPDocumentNo = @SAPDocumentNo," +
                                                        "ModifiedDate =@ModifiedDate,ModifiedBy = @ModifiedBy, LiftingRequestId=@LiftingRequestId WHERE Id = @Id";
                                        //        var parameters = new[]{
                                        //    new SqlParameter("@UserId", LiftingRequestData != null ? LiftingRequestData.UserId : 0),
                                        //    new SqlParameter("@BillingDocument",invoiceDto.SAPDocNumber),
                                        //    new SqlParameter("@InvoiceDate", invoiceDate),
                                        //    new SqlParameter("@TotalInvoice", ConsoleSettings.StringToDecimalTryParse(invoiceDto.Invoice_Amount)),
                                        //    new SqlParameter("@SAPDocumentNo", sapDocumentNo),
                                        //    new SqlParameter("@ModifiedDate", currentDate),
                                        //    new SqlParameter("@ModifiedBy", userId),
                                        //    new SqlParameter("@Id", invoiceContextSave.Id),
                                        //    new SqlParameter("@LiftingRequestId", LiftingRequestData.Id),
                                        //    new SqlParameter("@Status", invoiceDto.Message)

                                        //};
                                        using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                        {

                                            var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                            var result = conn.Execute(sqlUpdate, new
                                            {
                                                UserId = LiftingRequestData != null ? LiftingRequestData.UserId : 0,
                                                BillingDocument = invoiceDto.SAPDocNumber,
                                                InvoiceDate = invoiceDate,
                                                TotalInvoice = ConsoleSettings.StringToDecimalTryParse(invoiceDto.Invoice_Amount),
                                                SAPDocumentNo = sapDocumentNo,
                                                ModifiedDate = modifiedDate,
                                                ModifiedBy = userId,
                                                LiftingRequestId = LiftingRequestData.Id,
                                                Status = invoiceDto.Message,
                                                Id = invoiceContextSave.Id
                                            });

                                        }
                                        invoiceId = invoiceContextSave.Id;
                                        //_emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                    }
                                    else
                                    {
                                        var invoiceListContext = invoiceList.FirstOrDefault(_ => _.BillingDocument == invoiceDto.SAPDocNumber);
                                        if (invoiceListContext == null)
                                        {
                                            var invoice = new Invoice
                                            {
                                                UserId = LiftingRequestData != null ? LiftingRequestData.UserId : 0,
                                                LiftingRequestId = LiftingRequestData.Id,
                                                BillingDocument = invoiceDto.SAPDocNumber,
                                                InvoiceDate = invoiceDate,
                                                TotalInvoice = ConsoleSettings.StringToDecimalTryParse(invoiceDto.Invoice_Amount),
                                                IsSAPDataSync = true,
                                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                CreatedBy = userId,
                                                SAPDocumentNo = sapDocumentNo,
                                                Status = invoiceDto.Message
                                            };
                                            _emamiContext.Invoices.Add(invoice);
                                            _emamiContext.SaveChanges();
                                            invoiceId = invoice.Id;
                                        }

                                    }
                                    var invoiceSales = new InvoiceSalesOrder
                                    {
                                        InvoiceId = invoiceId,
                                        SalesOrderNumber = sapDocumentNo,
                                        BillingDocument = invoiceDto.SAPDocNumber,
                                        LiftingRequestId = LiftingRequestData.Id
                                    };
                                    invoiceSalesOrderList.Add(invoiceSales);
                                }
                            }
                            else
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.SalesOrderNoIsEmpty, invoiceDto.SAPDocNumber), errorMessage);
                            }
                        }
                        else
                        {
                            errorMessageList.Add(errorMessage);
                            //errorRecordList.Add(invoiceDto);
                        }



                        //}
                        if (null != invoiceList && invoiceList.Any())
                        {
                            _emamiContext.BulkInsertProxy(invoiceList);
                        }
                        _emamiContext.SaveChanges();

                        var invoiceDetailList = new List<InvoiceDetail>();

                        InvoicesData = _emamiContext.Invoices.AsNoTracking()
                            .Where(_ => invBillingDocument.Contains(_.BillingDocument))
                            .Select(s => new { Id = s.Id, BillingDocument = s.BillingDocument, s.SAPDocumentNo });

                        foreach (var invoiceDetails in invoiceDto.ItemData)
                        {
                            var salesOrderList = invoiceDetails.ItemNo.Split('/');
                            if (salesOrderList != null && salesOrderList.Length == 2)
                            {
                                var salesOrderNumber = salesOrderList[0];
                                var invoiceContext = invoiceSalesOrderList.FirstOrDefault(_ => _.BillingDocument == invoiceDto.SAPDocNumber && _.SalesOrderNumber == salesOrderNumber);
                                if (invoiceContext != null)
                                {
                                    var errorFlagDetails = true;
                                    var itemNo = salesOrderList[1].TrimStart('0');
                                    var skuIds = SkusData.Where(_ => _.SkuCode == invoiceDetails.Material).Select(_ => _.Id).ToList();
                                    var invoiceDetailsContext = InvoiceDetailsData.FirstOrDefault(_ => _.MaterialNumber == invoiceDetails.Material && _.InvoiceId == invoiceContext.InvoiceId && _.ItemNo == itemNo);
                                    var liftingDetails = _emamiContext.LiftingRequestDetails.AsNoTracking().FirstOrDefault(_ => _.LiftingRequestId == invoiceContext.LiftingRequestId && skuIds.Contains(_.SkuId));

                                    if (liftingDetails == null)
                                    {
                                        errorMessageList.Add(string.Format(" {0} Material does not exist in Sales Order ", invoiceDetails.Material));
                                        errorFlagDetails = false;
                                    }


                                    if (errorFlagDetails)
                                    {
                                        var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == liftingDetails.SkuId);

                                        var quantityMT = _resultService.ConvertCasetoMetricTon(ConsoleSettings.StringToDecimalTryParse(invoiceDetails.Bill_Qty), skuContext.Id);
                                        if (invoiceDetailsContext != null)
                                        {
                                            var sqlUpdate = "UPDATE InvoiceDetails SET MaterialNumber = @MaterialNumber,QuantityInCase =@QuantityInCase,ActualBilledQuantity = @ActualBilledQuantity," +
                                            "SkuId =@SkuId,OilTypeId=@OilTypeId," +
                                            "ModifiedDate=@ModifiedDate,ModifiedBy=@ModifiedBy," +
                                            "SalesOrganizationId=@SalesOrganizationId,DistributionChannelId=@DistributionChannelId,DivisionId=@DivisionId" +
                                            " WHERE Id = @Id";
                                            //var parameters = new[]{
                                            //new SqlParameter("@MaterialNumber", invoiceDetails.Material),
                                            //new SqlParameter("@QuantityInCase", ConsoleSettings.StringToDecimalTryParse(invoiceDetails.Bill_Qty)),
                                            //new SqlParameter("@ActualBilledQuantity", quantityMT),
                                            //new SqlParameter("@ModifiedDate", currentDate),
                                            //new SqlParameter("@ModifiedBy", userId),
                                            //new SqlParameter("@Id", invoiceDetailsContext.Id),
                                            //new SqlParameter("@SkuId", liftingDetails != null ? liftingDetails.SkuId : 0),
                                            //new SqlParameter("@SalesOrganizationId", skuContext != null ? skuContext.SalesOrganizationId : 0),
                                            //new SqlParameter("@DistributionChannelId", skuContext != null ? skuContext.DistributionChannelId : 0),
                                            //new SqlParameter("@DivisionId", skuContext != null ? skuContext.DivisionId : 0),
                                            //new SqlParameter("@OilTypeId", skuContext != null ? UtilityHelper.LongTryToParse(skuContext.OilTypeId.ToString()) : 0)
                                            //};
                                            //_emamiContext.BulkUpdateProxy(sqlUpdate, parameters);

                                            using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                            {

                                                var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                                var result = conn.Execute(sqlUpdate, new
                                                {
                                                    MaterialNumber = invoiceDetails.Material,
                                                    QuantityInCase = ConsoleSettings.StringToDecimalTryParse(invoiceDetails.Bill_Qty),
                                                    ActualBilledQuantity = quantityMT,
                                                    SkuId = liftingDetails != null ? liftingDetails.SkuId : 0,
                                                    OilTypeId = skuContext != null ? UtilityHelper.LongTryToParse(skuContext.OilTypeId.ToString()) : 0,
                                                    ModifiedDate = modifiedDate,
                                                    ModifiedBy = userId,
                                                    SalesOrganizationId = skuContext != null ? skuContext.SalesOrganizationId : 0,
                                                    DistributionChannelId = skuContext != null ? skuContext.DistributionChannelId : 0,
                                                    DivisionId = skuContext != null ? skuContext.DivisionId : 0,
                                                    Id = invoiceDetailsContext.Id

                                                });

                                            }


                                        }
                                        else
                                        {
                                            var invoiceDetailsListContext = invoiceDetailList.FirstOrDefault(_ => _.MaterialNumber == invoiceDetails.Material && _.InvoiceId == invoiceContext.InvoiceId && _.ItemNo == itemNo);
                                            if (invoiceDetailsListContext == null)
                                            {
                                                var invoiceDetail = new InvoiceDetail
                                                {
                                                    MaterialNumber = invoiceDetails.Material,
                                                    QuantityInCase = ConsoleSettings.StringToDecimalTryParse(invoiceDetails.Bill_Qty),
                                                    ActualBilledQuantity = quantityMT,
                                                    InvoiceId = invoiceContext.InvoiceId,
                                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                    CreatedBy = userId,
                                                    SkuId = liftingDetails != null ? liftingDetails.SkuId : 0,
                                                    OilTypeId = skuContext != null ? UtilityHelper.LongTryToParse(skuContext.OilTypeId.ToString()) : 0,
                                                    ItemNo = itemNo,
                                                    SalesOrganizationId = skuContext != null ? skuContext.SalesOrganizationId : 0,
                                                    DistributionChannelId = skuContext != null ? skuContext.DistributionChannelId : 0,
                                                    DivisionId = skuContext != null ? skuContext.DivisionId : 0
                                                };
                                                if (invoiceDetail != null)
                                                {
                                                    invoiceDetailList.Add(invoiceDetail);
                                                }
                                            }

                                        }
                                        // Contract Limit Check Update
                                        var liftingRequest = _emamiContext.LiftingRequest.AsNoTracking().FirstOrDefault(_ => _.SAPDocumentNo == salesOrderNumber);

                                        if (liftingRequest != null)
                                        {
                                            var liftingRequestDetails = _emamiContext.LiftingRequestDetails.AsNoTracking().FirstOrDefault(_ => _.LiftingRequestId == liftingRequest.Id && _.SkuId == skuContext.Id);
                                            if (liftingRequestDetails != null)
                                            {
                                                ContractAvilableLimitCalculate(liftingRequestDetails.SaudaNumber);
                                            }
                                        }
                                        dataSynced++;
                                        successData.ItemData.Add(invoiceDetails);
                                    }
                                    else
                                    {
                                        errorData.ItemData.Add(invoiceDetails);
                                        // errorRecordList.Add(invoiceDto);
                                    }
                                }
                            }
                            else
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.SalesOrderNoIsEmpty, invoiceDto.SAPDocNumber), errorMessage);
                            }

                        }
                        if (null != invoiceDetailList && invoiceDetailList.Any())
                        {
                            _emamiContext.BulkInsertProxy(invoiceDetailList);
                        }
                        _emamiContext.SaveChanges();

                        var inputData = new List<InvoiceDto>() {
                            inputdto
                        };

                        if (successData.ItemData.Count > 0)
                        {
                            successList.Add(new InvoiceDto()
                            {
                                SAPDocNumber = inputdto.SAPDocNumber,
                                Sap_Document_Number = inputdto.Sap_Document_Number,
                                InvoiceNumber = inputdto.InvoiceNumber,
                                SAPRefDoc = inputdto.SAPRefDoc,
                                ImpigerReqNo = inputdto.ImpigerReqNo,
                                Invoice_Date = inputdto.Invoice_Date,
                                Invoice_Amount = inputdto.Invoice_Amount,
                                Message = inputdto.Message,
                                ItemData = successData.ItemData
                            });
                        }


                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        //sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = inputList;
                        sapDataSyncResultDto.SuccessRecordDetailsResponse = successList;
                        var sqlInvoiceDelete = "Delete from Invoices Where Id not in (Select InvoiceId From InvoiceDetails)";
                        var listOfStrings = new List<string>();
                        object[] arrayOfStrings = listOfStrings.ToArray();
                        //_emamiContext.BulkUpdateProxy(sqlInvoiceDelete, arrayOfStrings);


                        using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                        {

                            var result = conn.Execute(sqlInvoiceDelete, new
                            {

                            });

                        }
                    }

                    if (errorMessageList.Any())
                    {
                        if (errorData.ItemData.Count > 0)
                        {
                            errorList.Add(new InvoiceDto()
                            {
                                SAPDocNumber = inputdto.SAPDocNumber,
                                Sap_Document_Number = inputdto.Sap_Document_Number,
                                InvoiceNumber = inputdto.InvoiceNumber,
                                SAPRefDoc = inputdto.SAPRefDoc,
                                ImpigerReqNo = inputdto.ImpigerReqNo,
                                Invoice_Date = inputdto.Invoice_Date,
                                Invoice_Amount = inputdto.Invoice_Amount,
                                Message = inputdto.Message,
                                ItemData = errorData.ItemData
                            });
                        }

                        sapDataSyncResultDto.ErrorDetailsResponse = errorList;
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                        _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList)}");
                    }
                    else
                    {
                        sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
                }
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = inputList;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }

        public void ContractAvilableLimitCalculate(string saudaNumber)
        {
            using (var _emamiContext = new AdaniContext())
            {
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                //Sales Order Quntity Update
                var saudaContext = _emamiContext.Sauda.FirstOrDefault(_ => _.SaudaNumber == saudaNumber);
                // var LiftingRequestIds = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => _.SaudaNumber == saudaNumber).Select(s => s.Id).ToList();
                if (/*LiftingRequestIds != null &&*/ saudaContext != null)
                {
                    var LiftingRequestDetailsContext = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.SaudaNumber == saudaNumber).ToList();
                    if (LiftingRequestDetailsContext != null && LiftingRequestDetailsContext.Any())
                    {
                        var salesOrderQuntityresult = LiftingRequestDetailsContext
                          .GroupBy(l => l.SaudaOrderId)
                          .Select(cl => new
                          {
                              SkuId = cl.First().SkuId,
                              LiftingQuantityCase = cl.Sum(c => c.LiftingQuantityCase),
                              LiftingQuantity = cl.Sum(s => s.LiftingQuantity),
                              SaudaOrderId = cl.FirstOrDefault().SaudaOrderId
                          });
                        foreach (var item in salesOrderQuntityresult)
                        {
                            var saudaOrdersContext = _emamiContext.SaudaOrders.FirstOrDefault(_ => _.SkuId == item.SkuId && _.Id == item.SaudaOrderId && _.StatusId != (int)DTO.Enums.Status.Completed);
                            if (saudaOrdersContext != null)
                            {
                                saudaOrdersContext.StatusId = saudaOrdersContext.BidQuantityCase <= item.LiftingQuantityCase ? (int)DTO.Enums.Status.Completed : (int)DTO.Enums.Status.Approved;
                                saudaOrdersContext.SalesOrderQuantityCase = item.LiftingQuantityCase;
                                saudaOrdersContext.SalesOrderQuantity = item.LiftingQuantity;
                                _emamiContext.SaveChanges();
                            }
                        }
                        var saudaOrdersContextList = _emamiContext.SaudaOrders.Where(_ => _.SaudaId == saudaContext.Id && _.StatusId != (int)DTO.Enums.Status.Completed);
                        if (saudaOrdersContextList == null)
                        {
                            saudaContext.StatusId = (int)DTO.Enums.Status.Completed;
                            _emamiContext.SaveChanges();
                        }
                    }
                }

                //Invoice Quntity Update
                var sapDocumentNos = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.SaudaNumber == saudaNumber && _.LiftingRequest.SAPDocumentNo != null).Select(s => s.LiftingRequest.SAPDocumentNo).Distinct().ToList();
                var invoiceContextIds = _emamiContext.Invoices.AsNoTracking().Where(_ => sapDocumentNos.Contains(_.SAPDocumentNo)).Select(s => s.Id).ToList();
                if (invoiceContextIds != null && saudaContext != null)
                {
                    var invoiceDetailsContext = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => invoiceContextIds.Contains(_.InvoiceId)).ToList();
                    if (invoiceDetailsContext != null && invoiceDetailsContext.Any())
                    {
                        var invoiceQuntityresult = invoiceDetailsContext
                          .GroupBy(l => l.SkuId)
                          .Select(cl => new InvoiceSalesOrderDataDto()
                          {
                              SkuId = cl.First().SkuId,
                              InvoiceQuantityCase = cl.Sum(c => c.QuantityInCase),
                              InvoiceQuantity = cl.Sum(s => s.ActualBilledQuantity),
                          }).ToList();
                        foreach (var item in invoiceQuntityresult)
                        {
                            var saudaOrdersContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SkuId == item.SkuId && _.SaudaId == saudaContext.Id).ToList();
                            if (saudaOrdersContext.IsAny())
                            {
                                foreach (var data in saudaOrdersContext)
                                {
                                    if (data.BidQuantityCase >= item.InvoiceQuantityCase)
                                    {
                                        var saudaOrdersSkusContext = _emamiContext.SaudaOrders.FirstOrDefault(_ => _.SkuId == item.SkuId && _.Id == data.Id);
                                        saudaOrdersSkusContext.InvoiceQuantityCase = item.InvoiceQuantityCase;
                                        saudaOrdersSkusContext.InvoiceQuantity = item.InvoiceQuantity;
                                        _emamiContext.SaveChanges();
                                        item.InvoiceQuantityCase = 0;
                                    }
                                    else
                                    {
                                        var saudaOrdersSkusContext = _emamiContext.SaudaOrders.FirstOrDefault(_ => _.SkuId == item.SkuId && _.Id == data.Id);
                                        saudaOrdersSkusContext.InvoiceQuantityCase = data.BidQuantityCase;
                                        saudaOrdersSkusContext.InvoiceQuantity = data.BidQuantity;
                                        _emamiContext.SaveChanges();
                                        item.InvoiceQuantityCase = item.InvoiceQuantityCase - data.BidQuantityCase;

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region  Sales Report


        public void SalesReport(AWLSalesRegisterOutputDto inputdto)
        {
            _methodName = "SalesReport";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputdto)}");
            var resultDto = new ResultDto();
            var errorMessageList = new List<string>();
            var dataSynced = 0;
            long userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var synctype = ConsoleSettings.SalesReportSubject;
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var subject = string.Concat(ConsoleSettings.SalesReportSubject, " ", ConsoleSettings.SAPToAppDataSyncEmailSubject);
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = inputdto.Records.Count;
            var salesList = inputdto != null && inputdto.Records != null ? inputdto.Records : new List<AWLSalesRegister>();
            try
            {
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = salesList;
                using (var _emamiContext = new AdaniContext())
                {
                    var salesRegDto = new List<SalesRegister>();
                    if (salesList != null && salesList.Any())
                    {
                        //var verticalCodeList = salesList
                        //   .GroupBy(u => u.sa)
                        //   .Select(grp => grp.Select(s => s.SalesOrganization))
                        //   .ToList();

                        //var description = UtilityHelper.GetEnumDescription(DTO.Enums.Configuration.ReportSyncDateValidationInHours);
                        //var configurationContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == description).Value;
                        //var value = Convert.ToDouble(configurationContext);
                        //var CurrentDate = DateHelper.UtcToIndia(DateTime.UtcNow.AddHours(-value));
                        //if (salesList.FirstOrDefault().BatchNo == 1)
                        //{
                        //if (verticalCodeList != null)
                        //{
                        //    var salesDate = salesList.FirstOrDefault();
                        //    foreach (var item in verticalCodeList)
                        //    {
                        //        if (item != null && item.Any() && item.FirstOrDefault() != string.Empty)
                        //        {
                        //            var invoiceDetailsDelete = "DELETE FROM SalesRegisters WHERE BillingDate >='" + salesDate.FromDate.Value.ToString("yyyy-MM-dd") + "' and BillingDate <= '" + salesDate.ToDate.Value.ToString("yyyy-MM-dd") + "' and SalesOrganization =" + item.FirstOrDefault();
                        //            var listOfStrings = new List<string>();
                        //            object[] arrayOfStrings = listOfStrings.ToArray();
                        //            _logger.Info($"query {invoiceDetailsDelete}");
                        //            //_logger.Info($"date {CurrentDate}");
                        //            _emamiContext.BulkUpdateProxy(invoiceDetailsDelete, arrayOfStrings);
                        //        }
                        //    }
                        //}
                        // }

                        var invoiceNumber = salesList.Select(s => s.Invoice_Number).Distinct().ToList();
                        var invoiceData = _emamiContext.Invoices.AsNoTracking()
                            .Where(_ => invoiceNumber.Contains(_.BillingDocument))
                            .Select(s => new SalesRegisterOutputDto { Id = s.Id, BillNumber = s.BillingDocument }).ToList();

                        var saudaSkuCode = salesList.Select(s => s.Material).Distinct().ToList();
                        var SkusData = _emamiContext.Skus.AsNoTracking()
                            .Where(_ => saudaSkuCode.Contains(_.SkuCode))
                            .Select(s => new {
                                Id = s.Id,
                                OilTypeId = s.OilTypeId,
                                PackGroupId = s.PackGroupId,
                                SkuCode = s.SkuCode,
                                DivisionId = s.DivisionId,
                                s.SalesOrganizationId,
                                s.DistributionChannelId
                            });

                        var salesOrganizationList = salesList.Select(s => s.Sales_Org).Distinct().ToList();
                        var salesOrganizationData = _emamiContext.SalesOrganization.AsNoTracking()
                           .Where(_ => salesOrganizationList.Contains(_.Code))
                           .Select(s => new { Id = s.Id, Code = s.Code });

                        var distributionChannelList = salesList.Select(s => s.Distribution_Channel).Distinct().ToList();
                        var distributionChannelData = _emamiContext.DistributionChannel.AsNoTracking()
                           .Where(_ => distributionChannelList.Contains(_.Code))
                           .Select(s => new { Id = s.Id, Code = s.Code, s.SalesOrganizationId });

                        var divisionList = salesList.Select(s => s.Division).Distinct().ToList();
                        var divisionData = _emamiContext.Divisions.AsNoTracking()
                           .Where(_ => divisionList.Contains(_.Code))
                           .Select(s => new { Id = s.Id, Code = s.Code, s.SalesOrganizationId, s.DistributionChannelId });

                        var customerList = salesList.Select(s => s.Customer.TrimStart('0')).Distinct().ToList();
                        var usersData = _emamiContext.Users.AsNoTracking()
                           .Where(_ => customerList.Contains(_.Code))
                           .Select(s => new { Id = s.Id, Code = s.Code });

                        foreach (var salesReport in salesList)
                        {
                            var errorMessage = string.Empty;
                            var errorFlag = true;
                            salesReport.Customer = salesReport.Customer.TrimStart('0');
                            var invoiceContext = invoiceData.FirstOrDefault(sauda => sauda.BillNumber == salesReport.Invoice_Number);
                            var salesOrganizationDataContext = salesOrganizationData.FirstOrDefault(_ => _.Code == salesReport.Sales_Org);
                            var salesOrganizationId = salesOrganizationDataContext != null ? salesOrganizationDataContext.Id : 0;

                            var distributionChannelDataContext = distributionChannelData.FirstOrDefault(_ => _.Code == salesReport.Distribution_Channel && _.SalesOrganizationId == salesOrganizationId);
                            var distributionChannelId = distributionChannelDataContext != null ? distributionChannelDataContext.Id : 0;

                            var divisionDataContext = divisionData.FirstOrDefault(_ => _.Code == salesReport.Division && _.SalesOrganizationId == salesOrganizationId && _.DistributionChannelId == distributionChannelId);
                            var divisionId = divisionDataContext != null ? divisionDataContext.Id : 0;
                            var skuContext = SkusData.FirstOrDefault(_ => _.SkuCode == salesReport.Material && _.SalesOrganizationId == salesOrganizationId &&
                            _.DistributionChannelId == distributionChannelId && _.DivisionId == divisionId);
                            var skuId = skuContext != null ? skuContext.Id : 0;
                            var userContext = usersData.FirstOrDefault(_ => _.Code == salesReport.Customer);
                            var dealerId = userContext != null ? userContext.Id : 0;
                            var invoiceDate = DateTime.ParseExact(salesReport.Invoice_Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                            //decimal quantityMT = 0;
                            //if (skuContext != null)
                            //{
                            //    quantityMT = _resultService.ConvertCasetoMetricTon(salesReport.QuantityCase, skuContext.Id);
                            //}

                            if (salesOrganizationId == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.SalesOrganisationIsEmpty + " App_Id: " + salesReport.Sales_Org, errorMessage);
                                errorFlag = false;
                            }
                            if (distributionChannelId == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.DistributionChannelIsEmpty + " App_Id: " + salesReport.Distribution_Channel, errorMessage);
                                errorFlag = false;
                            }

                            if (divisionId == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.DivisionIsEmpty + " App_Id: " + salesReport.Division, errorMessage);
                                errorFlag = false;
                            }

                            if (errorFlag)
                            {
                                var quantityCase = _resultService.ConvertMetricTonToQuantityCase(ConsoleSettings.StringToDecimalTryParse(salesReport.Qty_TON), skuId);
                                var salesReg = new SalesRegister
                                {
                                    InvoiceId = invoiceContext != null ? invoiceContext.Id : 0,
                                    UserId = dealerId,
                                    MaterialCode = salesReport.Material,
                                    CustomerCode = salesReport.Customer,
                                    QuantityCase = quantityCase,
                                    QuantityMT = ConsoleSettings.StringToDecimalTryParse(salesReport.Qty_TON),
                                    InvoiceType = salesReport.Invoice_Type,
                                    InvoiceNumber = salesReport.Invoice_Number,
                                    InvoiceDate = invoiceDate,
                                    TotalGST = salesReport.Total_GST,
                                    TotalAmount = salesReport.Total_Amount,
                                    SalesOrganization = salesReport.Sales_Org,
                                    DistributionChannel = salesReport.Distribution_Channel,
                                    Division = salesReport.Division,
                                    SalesOrganizationId = salesOrganizationId,
                                    DistributionChannelId = distributionChannelId,
                                    DivisionId = divisionId,
                                    CreatedBy = userId,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    ModifiedBy = userId,
                                    ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    SkuId = skuId,
                                    DeliveryNumber=salesReport.Delivery_Number,
                                    BrokerName=salesReport.Broker_Name,
                                    ContractNumber=salesReport.Contract_Number,
                                    LRNo=salesReport.LR_No,
                                    OrderNumber=salesReport.Order_Number,
                                    VehicleNumber=salesReport.Vehicle_No,
                                    ShiptoParty=salesReport.ShiptoParty
                                };
                                salesRegDto.Add(salesReg);
                                dataSynced++;
                            }
                            else
                            {
                                errorMessageList.Add(errorMessage);
                            }
                        }

                    }
                    else
                    {
                        errorMessageList.Add(Constants.InvalidRequest);
                    }

                    if (null != salesRegDto && salesRegDto.Any())
                    {
                        _emamiContext.BulkInsertProxy(salesRegDto);
                    }
                    _emamiContext.SaveChanges();
                    sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                    sapDataSyncResultDto.SuccessRecordDetailsResponse = salesRegDto;
                }

                if (errorMessageList.Any())
                {

                    sapDataSyncResultDto.ErrorDetailsResponse = salesList;
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Response = sapDataSyncResultDto;
                    resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList)}");
                }
                else
                {
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = sapDataSyncResultDto;
                    resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                }
                sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = salesList;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                _logger.Error(message);
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }



        #endregion

        #region CustomerLedger

        public void CustomerLedgerAutoTrigger()
        {
            _methodName = "CustomerLedgerAutoTrigger";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            //var syncFolder = ConsoleSettings.SaudaFolder;
            var errorMessageList = new List<string>();
            var subject = string.Concat(ConsoleSettings.CustomerLedgerFolder, " ", ConsoleSettings.AppToSapDataSyncEmailSubject);
            //var csvFileName = ConsoleSettings.SaudaHBCCreationCsv;
            var resultDto = new ResultDto();
            var saudaViewDtoList = new HANASaudaViewList();
            var SaudaContext = new List<SaudaOrder>();
            var SaudaListContext = new List<Sauda>();
            var inputDtoJson = string.Empty;
            try
            {
                var PendingContractDeleteHours = ConsoleSettings.PendingContractDeleteHours;
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                currentDate = currentDate.AddHours(PendingContractDeleteHours);

                _logger.Info($"Query Customer Ledger Delete : {currentDate.ToString()}");
                #region Open Contract Data Delete
                //var customerLedgerDelete = "DELETE FROM CustomerLedgers WHERE CreatedDate <=" + "'" + currentDate + "'";
                //_logger.Info($"Query Customer Ledger Delete : {customerLedgerDelete}");
                //using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                //{
                //    var result = conn.Execute(customerLedgerDelete, new
                //    {

                //    });
                //}
                #endregion

                using (var _emamiContext = new AdaniContext())
                {

                    //var saudaContext = _emamiContext.Sauda.AsNoTracking().Select(_ => new { _.UserId, _.SalesOrganizationId, _.DistributionChannelId, _.DivisionId }).Distinct().ToList();
                    var userContext = (from u in _emamiContext.Users
                                       join ur in _emamiContext.UserRoles on u.Id equals ur.UserId
                                       where u.IsActive && ur.RoleId == (int)Adani.Solution.DTO.Enums.Role.Dealer
                                       select new { u.Code, u.Id }).ToList();
                    if (userContext != null)
                    {
                        //var customerList = userContext.Select(s => s.Id).Distinct().ToList();
                        //var usersData = _emamiContext.Users.AsNoTracking()
                        //   .Where(_ => customerList.Contains(_.Id))
                        //   .Select(s => new { Id = s.Id, Code = s.Code, s.CompanyCode, s.IsActive });

                        //foreach (var item in userContext)
                        //{
                        //var usersDataContext = usersData.FirstOrDefault(_ => _.IsActive && _.Id == item.Id);
                        //if (usersDataContext != null)
                        //{
                        var records = userContext.Select(s => new SAPCustomerLedgerRequestDTO()
                        {
                            Customer_Number = s.Code,
                            Company_Code = Config.Company_Code
                        }).ToList();

                        if (records.IsAny())
                        {
                            int batchCount = ConsoleSettings.BatchCount;
                            var loopcount = Math.Ceiling(Convert.ToDecimal(records.Count()) / Convert.ToDecimal(batchCount));
                            int skip = 0;
                            for (int i = 0; i < loopcount; i++)
                            {
                                var perRequestdata = records.Skip(skip).Take(batchCount).ToList();
                                var sapCustomerLedgerRequestListDTO = new SAPCustomerLedgerRequestListDTO { Records = perRequestdata };
                                //var json = JsonConvert.SerializeObject(perRequestdata, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                                //_logger.Info($"Json Input : {JsonConvert.SerializeObject(sapCustomerLedgerRequestListDTO)}");
                                var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.CustomerLedgerRequestApiUrl, sapCustomerLedgerRequestListDTO);
                                var status = response.StatusCode;
                                _logger.Info($"Responce : {status}");
                                skip += batchCount;
                            }
                        }


                        //}
                        // }
                    }
                }

                //  _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, null, subject);

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaViewDtoList.Header;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _logger.Error(message);
                //_sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
            }
        }

        public void CustomerLedgerRequest(SAPCustomerLedgerRequestDTO inputDto)
        {
            _methodName = "CustomerLedgerRequest";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            //var syncFolder = ConsoleSettings.SaudaFolder;           
            var subject = string.Concat(ConsoleSettings.CustomerLedgerFolder, " ", ConsoleSettings.AppToSapDataSyncEmailSubject);
            //var csvFileName = ConsoleSettings.SaudaHBCCreationCsv;
            var resultDto = new ResultDto();
            var saudaViewDtoList = new HANASaudaViewList();
            try
            {
                using (var _emamiContext = new AdaniContext())
                {
                    var data = new SAPCustomerLedgerRequestDTO()
                    {
                        Customer_Number = inputDto.Customer_Number,
                        Company_Code = Config.Company_Code
                    };
                    var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.CustomerLedgerRequestApiUrl, data);
                    var status = response.StatusCode;
                    _logger.Info($"Responce : {status}");

                }
                // _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, null, subject);

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = saudaViewDtoList.Header;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _logger.Error(message);
                // _sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
            }
        }

        /// <summary>
        /// Method to save customer ledger
        /// </summary>
        /// <param name="decryptedString"></param>
        /// <returns></returns>
        public void SaveCustomerLedger(HANACustomerLedgerDtoList inputdto)
        {
            _methodName = "SaveCustomerLedger";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputdto)}");
            var invoiceSyncData = string.Empty;
            var messageSync = string.Empty;
            var sapDataSyncResultDto = new SapDataSyncResultDto();
            var errorRecordList = new List<HANACustomerLedgerDto>();
            sapDataSyncResultDto.SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
            var dataSynced = 0;
            var resultDto = new ResultDto();
            var synctype = ConsoleSettings.CustomerLedgerFolder;
            var customerLedgerDtoList = inputdto != null && inputdto.Records != null ? inputdto.Records : new List<HANACustomerLedgerDto>();
            var subject = string.Concat(ConsoleSettings.CustomerLedgerFolder, ConsoleSettings.SAPToAppDataSyncEmailSubject);
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = customerLedgerDtoList.Count;
            try
            {
                var customerLedgerList = new List<CustomerLedgerUDTDto>();
                var userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
                var errorMessageList = new List<string>();
                if (customerLedgerDtoList != null && customerLedgerDtoList.Any())
                {
                    var userContextList = new List<UserDto>();
                    // var userContext = new UserDto();
                    // var customerCodeList = customerLedgerDtoList.FirstOrDefault();
                    var customerLedgersDetailsData = new List<CustomerLedgerDetailsDto>();
                    var customerList = customerLedgerDtoList.Select(s => s.Customer_Code.TrimStart('0')).Distinct().ToList();
                    using (var _emamiContext = new AdaniContext())
                    {
                        #region Get Common Datas                        
                        userContextList = (from s in _emamiContext.Users.AsNoTracking()
                                           join role in _emamiContext.UserRoles.AsNoTracking() on s.Id equals role.UserId
                                           where role.RoleId != (int)DTO.Enums.Role.ShipToParty && customerList.Contains(s.Code)
                                           select new UserDto { Id = s.Id, Code = s.Code, CustomerGroup = s.CustomerGroup }).ToList();
                        //userContext = userContextList.FirstOrDefault(_ => _.Code.ToLower() == customerCodeList.Customer_Code.ToLower());
                        //var usersId = userContext.Id;
                        //customerLedgersDetailsData =
                        //    (from c in _emamiContext.CustomerLedgerDetails.AsNoTracking()                             
                        //     where c.UserId == userContext.Id
                        //     select new CustomerLedgerDetailsDto { Id = c.Id, Balance = c.Balance, UserId = c.UserId }).ToList(); 

                        //_emamiContext.CustomerLedgerDetails.AsNoTracking()
                        //   .Where(_ => _.UserId == userContext.Id)
                        //   .Select(s => new CustomerLedgerDetails() { Id = s.Id, Balance = s.Balance, UserId=s.UserId }).ToList();


                        #endregion
                    }


                    //foreach(var code in customerCodeList)
                    //{
                    if (userContextList.IsAny())
                    {
                        string listOfIdsJoined = "(" + String.Join(",", userContextList.Select(s => s.Id).ToArray()) + ")";
                        #region Delete CustomerLedger Data                           
                        var customerLedgerMasterDelete = "DELETE FROM CustomerLedgers WHERE UserId in " + listOfIdsJoined;
                        using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                        {
                            var result = conn.Execute(customerLedgerMasterDelete, new
                            {
                            });
                        }
                        _logger.Info($"Query customerLedgerMasterDelete Master Delete {customerLedgerMasterDelete}");
                        #endregion
                    }
                    foreach (var customerLedger in customerLedgerDtoList)
                    {
                        var errorFlag = true;
                        var errorMessage = "";
                        if (customerLedger == null)
                        {
                            errorMessage = Constants.BindErrorMessage(Constants.InvalidRequest, errorMessage);
                            errorFlag = false;
                        }
                        customerLedger.Customer_Code = customerLedger.Customer_Code.TrimStart('0');
                        errorMessage = customerLedger.Customer_Code;
                        if (string.IsNullOrEmpty(customerLedger.Customer_Code))
                        {
                            errorMessage = Constants.BindErrorMessage(Constants.CustomerCodeIsEmpty, errorMessage);
                            errorFlag = false;
                        }
                        if (string.IsNullOrEmpty(customerLedger.Ref_Number))
                        {
                            errorMessage = Constants.BindErrorMessage(Constants.RefNumberIsEmpty, errorMessage);
                            errorFlag = false;
                        }
                        var userContext = userContextList.FirstOrDefault(_ => _.Code.ToLower() == customerLedger.Customer_Code.ToLower());
                        if (userContext == null)
                        {
                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.CustomerCodeNotExist, customerLedger.Customer_Code), errorMessage);
                            errorFlag = false;
                        }
                        if (customerLedger.Ref_Number == null)
                        {
                            errorMessage = Constants.BindErrorMessage(string.Format(Constants.CustomerCodeNotExist, customerLedger.Customer_Code), errorMessage);
                            errorFlag = false;
                        }
                        if (errorFlag)
                        {
                            var postingDate = customerLedger.Document_Date.Replace('.', '-');
                            var dueDate = customerLedger.Due_Date.Replace('.', '-');
                            decimal credit = 0;
                            decimal debit = 0;
                            var isPassitive = customerLedger.Amount.Contains('-');
                            if (!isPassitive)
                            {
                                debit = ConsoleSettings.StringToDecimalTryParse(customerLedger.Amount);
                            }
                            else
                            {
                                credit = ConsoleSettings.StringToDecimalTryParse(customerLedger.Amount);
                            }

                            var customerLedgerCotext = new CustomerLedgerUDTDto
                            {
                                UserCode = customerLedger.Customer_Code,
                                UserId = userContext != null ? userContext.Id : 0,
                                CompanyCode = customerLedger.Company_Code,
                                DocumentType = customerLedger.Document_Type,
                                Credit = credit,
                                Debit = debit,
                                PostingDate = postingDate.ToString(),
                                Reference = customerLedger.Ref_Number,
                                Balance = ConsoleSettings.StringToDecimalTryParse(customerLedger.Amount),
                                DueDate = dueDate.ToString(),
                                Currency = customerLedger.Currency,
                                // CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                CreatedBy = userId,
                                // ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ModifiedBy = userId
                            };
                            customerLedgerList.Add(customerLedgerCotext);
                            dataSynced++;


                        }
                        else
                        {
                            errorMessageList.Add(errorMessage);
                            errorRecordList.Add(customerLedger);
                        }
                    }

                    if (null != customerLedgerList && customerLedgerList.Any())
                    {
                        var customerLedger = Constants.ToDataTable(customerLedgerList);
                        using (var conn = new SqlConnection(Config.DBConnectionString))
                        {
                            conn.Open();
                            conn.Execute("SP_CustomerLedgers", new { CustomerLedger = customerLedger.AsTableValuedParameter("UDTT_CustomerLedgers") },
                                commandType: CommandType.StoredProcedure);
                        }
                    }


                    var customerLedgerdata = customerLedgerList.GroupBy(a => a.UserId).ToList();
                    using (var _emamiContext = new AdaniContext())
                    {
                        if (customerLedgerdata != null)
                        {
                            foreach (var data in customerLedgerdata)
                            {
                                var customerLedgerDetails = _emamiContext.CustomerLedgerDetails.FirstOrDefault(_ => _.UserId == data.Key);
                                var customerLedgerMaster = string.Empty;
                                var CurrentBalance = data.Sum(_ => _.Debit) + data.Sum(_ => _.Credit);
                                if (customerLedgerDetails != null)
                                {
                                    customerLedgerMaster = "UPDATE CustomerLedgerDetails SET Balance = " + CurrentBalance + ",ModifiedBy = " + userId + ",ModifiedDate = GETDATE() Where UserId =" + data.Key;
                                    //customerLedgerDetails.Balance = CurrentBalance;
                                }
                                else
                                {
                                    customerLedgerMaster = "INSERT INTO CustomerLedgerDetails  (Balance,UserId,CreatedBy,CreatedDate ) VALUES (" + CurrentBalance + "," + data.Key + "," + userId + ",GETDATE())";
                                    //var customerLedgerdetails = new CustomerLedgerDetails()
                                    //{
                                    //    UserId = data.Key,
                                    //    Balance = CurrentBalance
                                    //};
                                    //_emamiContext.CustomerLedgerDetails.Add(customerLedgerdetails);
                                }

                                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                {
                                    var result = conn.Execute(customerLedgerMaster, new
                                    {
                                    });
                                }
                            }
                        }

                        //foreach (var data in customerLedgerdata)
                        //{
                        //var customerLedgerDetails = customerLedgersDetailsData.FirstOrDefault(_ => _.UserId == customerLedgerdata.UserId);


                        //}
                    }

                    sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                    sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                    sapDataSyncResultDto.TotalInputRecordDetailsResponse = customerLedgerDtoList;
                    sapDataSyncResultDto.SuccessRecordDetailsResponse = customerLedgerDtoList.Except(errorRecordList).ToList();
                    //}

                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                        _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList)}");
                        //foreach (var errorItem in errorMessageList)
                        //{
                        //    var userCode = errorItem.Split('-');
                        //    var customerLedgerDetails = customerLedgerDtoList.FirstOrDefault(_ => _.UserCode == userCode[0].Trim());
                        //    if (customerLedgerDetails != null)
                        //    {
                        //        var localFile = ConsoleSettings.SystemPath(customerLedgerDetails.PdfFileName, ConsoleSettings.CustomerLedgerPdfFolder, true).ToString();
                        //        inputDto.ErrorPdf.Add(localFile);
                        //    }
                        //}
                    }
                    else
                    {
                        sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = customerLedgerDtoList;
                sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }

        /// <summary>
        /// Method to save customer ledger
        /// </summary>
        /// <param name="decryptedString"></param>
        /// <returns></returns>
        public void SaveOverduePayment(HANACustomerLedgerDtoList inputdto)
        {
            _methodName = "SaveOverduePayment";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputdto)}");
            var invoiceSyncData = string.Empty;
            var messageSync = string.Empty;
            var sapDataSyncResultDto = new SapDataSyncResultDto();
            var errorRecordList = new List<HANACustomerLedgerDto>();
            sapDataSyncResultDto.SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
            var dataSynced = 0;
            var resultDto = new ResultDto();
            var synctype = ConsoleSettings.CustomerLedgerFolder;
            var customerLedgerDtoList = inputdto != null && inputdto.Records != null ? inputdto.Records : new List<HANACustomerLedgerDto>();
            var subject = string.Concat(ConsoleSettings.CustomerLedgerFolder, ConsoleSettings.SAPToAppDataSyncEmailSubject);
            sapDataSyncResultDto.OutstandingResult.DataRetrieved = customerLedgerDtoList.Count;
            try
            {
                var customerLedgerList = new List<OverduePayment>();
                var userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
                var errorMessageList = new List<string>();
                if (customerLedgerDtoList != null && customerLedgerDtoList.Any())
                {
                    //foreach (var item in customerLedgerDtoList)
                    //{
                    //    var sourcePath = RemotePath.EscapeFileMask(folderPath + "/" + item.PdfFileName);
                    //    inputDto.SourceFileName.Add(sourcePath);
                    //}
                    using (var _emamiContext = new AdaniContext())
                    {
                        #region Delate User Data

                        //Open Balance data delete
                        var overduePaymentDelete = "DELETE FROM OverduePayments";
                        var listOfStrings = new List<string>();
                        object[] arrayOfStrings = listOfStrings.ToArray();
                        //_emamiContext.BulkUpdateProxy(overduePaymentDelete, arrayOfStrings);


                        using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                        {

                            var result = conn.Execute(overduePaymentDelete, new
                            {

                            });

                        }


                        _logger.Info($"Query Overdue Payment Delete");
                        #endregion

                        #region Get Common Datas
                        var customerList = customerLedgerDtoList.Select(s => s.Customer_Code.TrimStart('0')).Distinct().ToList();
                        var userContextList = (from s in _emamiContext.Users
                                               join role in _emamiContext.UserRoles on s.Id equals role.UserId
                                               where role.RoleId != (int)DTO.Enums.Role.ShipToParty && customerList.Contains(s.Code)
                                               select new { Id = s.Id, Code = s.Code/*, VerticalId = s.DivisionId*/, CustomerGroup = s.CustomerGroup }).ToList();
                        #endregion

                        foreach (var customerLedger in customerLedgerDtoList)
                        {
                            var errorFlag = true;
                            customerLedger.Customer_Code = customerLedger.Customer_Code.TrimStart('0');
                            var errorMessage = customerLedger.Customer_Code;
                            if (customerLedger == null)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.InvalidRequest, errorMessage);
                                errorFlag = false;
                            }

                            if (string.IsNullOrEmpty(customerLedger.Customer_Code))
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.CustomerCodeIsEmpty, errorMessage);
                                errorFlag = false;
                            }
                            var userContext = userContextList.FirstOrDefault(_ => _.Code.ToLower() == customerLedger.Customer_Code.ToLower());
                            if (userContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(string.Format(Constants.CustomerCodeNotExist, customerLedger.Customer_Code), errorMessage);
                                errorFlag = false;
                            }
                            if (errorFlag)
                            {

                                var postingDate = DateTime.ParseExact(customerLedger.Document_Date.Replace('.', '/'), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                var dueDate = DateTime.ParseExact(customerLedger.Due_Date.Replace('.', '/'), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                var customerLedgerCotext = new OverduePayment
                                {
                                    UserCode = customerLedger.Customer_Code,
                                    UserId = userContext != null ? userContext.Id : 0,
                                    CompanyCode = customerLedger.Company_Code,
                                    DocumentType = customerLedger.Document_Type,
                                    PostingDate = postingDate,
                                    Reference = customerLedger.Ref_Number,
                                    Balance = ConsoleSettings.StringToDecimalTryParse(customerLedger.Amount),
                                    DueDate = dueDate,
                                    Currency = customerLedger.Currency,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    CreatedBy = userId
                                };
                                customerLedgerList.Add(customerLedgerCotext);
                                dataSynced++;

                            }
                            else
                            {
                                errorMessageList.Add(errorMessage);
                                errorRecordList.Add(customerLedger);
                            }
                        }
                        if (null != customerLedgerList && customerLedgerList.Any())
                        {
                            _emamiContext.BulkInsertProxy(customerLedgerList);
                        }
                        _emamiContext.SaveChanges();
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = customerLedgerDtoList;
                        sapDataSyncResultDto.SuccessRecordDetailsResponse = customerLedgerDtoList.Except(errorRecordList).ToList();
                    }

                    if (errorMessageList.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Response = sapDataSyncResultDto;
                        resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                        _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList)}");
                        //foreach (var errorItem in errorMessageList)
                        //{
                        //    var userCode = errorItem.Split('-');
                        //    var customerLedgerDetails = customerLedgerDtoList.FirstOrDefault(_ => _.UserCode == userCode[0].Trim());
                        //    if (customerLedgerDetails != null)
                        //    {
                        //        var localFile = ConsoleSettings.SystemPath(customerLedgerDetails.PdfFileName, ConsoleSettings.CustomerLedgerPdfFolder, true).ToString();
                        //        inputDto.ErrorPdf.Add(localFile);
                        //    }

                        //}
                    }
                    else
                    {
                        sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = sapDataSyncResultDto;
                        resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                    }
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = customerLedgerDtoList;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, synctype, null, subject);
            }
        }

        #endregion

        #region Darwinbox
        public void EmployeeRequestActiveUsers()
        {
            _methodName = "EmployeeRequestActiveUsers";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var syncFolder = ConsoleSettings.SaudaFolder;
            var errorMessageList = new List<string>();
            var dataSynced = 0;
            var subject = string.Concat(ConsoleSettings.EmployeeRequestActiveUsersSubject, " ", ConsoleSettings.AppToSapDataSyncEmailSubject);
            var csvFileName = ConsoleSettings.EmployeeRequestActiveUsersCsv;
            var resultDto = new ResultDto();
            var result = new DarwinboxAPIResponsetDTO();
            try
            {
                using (var _emamiContext = new AdaniContext())
                {

                    var LastModifiedDate = DateTime.Now.AddDays(Config.LastModifiedDate).ToString("dd-MM-yyyy HH:mm:ss");
                    var data = new DarwinboxAPIRequestDTO()
                    {
                        api_key = Constants.ActiveUserAPIKey,
                        datasetKey = Constants.ActiveUserDatasetKey,
                        last_modified = LastModifiedDate
                    };

                    //var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    _logger.Info($"Json Input : {JsonConvert.SerializeObject(data)}");
                    var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.DarwinboxAPIUrl, data, true);
                    var json = response.Content.ReadAsStringAsync().Result;
                    var status = response.StatusCode;
                    result = JsonConvert.DeserializeObject<DarwinboxAPIResponsetDTO>(json.ToString(), UtilityHelper.GetJsonSettings());
                    _logger.Info($"Responce : {status}");
                    if (result != null && result.employee_data.IsAny())
                    {
                        var userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
                        #region Get Common Datas
                        var customerList = result.employee_data.Select(s => s.employee_id.TrimStart('0')).Distinct().ToList();
                        var userContextList = (from s in _emamiContext.Users
                                               where customerList.Contains(s.Code)
                                               select new { Id = s.Id, Code = s.Code }).ToList();
                        #endregion
                        var userList = new List<User>();
                        var errorRecordList = new List<DarwinboxEmployeeListDTO>();
                        var errorFlag = true;
                        foreach (var userDeails in result.employee_data)
                        {
                            var errorMessage = userDeails.employee_id;
                            if (userDeails == null)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.InvalidRequest, errorMessage);
                                errorFlag = false;
                            }

                            if (string.IsNullOrEmpty(userDeails.employee_id))
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.UserCodeIsEmpty, errorMessage);
                                errorFlag = false;
                            }
                            if (errorFlag)
                            {
                                var userContextSave = userContextList.FirstOrDefault(_ => _.Code == userDeails.employee_id);
                                if (userContextSave != null)
                                {
                                    var sqlUpdate = "UPDATE Users SET Name =@Name,DepartmentName = @DepartmentName,DirectManagerEmployee = @DirectManagerEmployeeId,OfficeCountry = @OfficeCountry," +
                                                     "ModifiedDate =@ModifiedDate,ModifiedBy = @ModifiedBy WHERE Id = @Id";
                                    //    var parameters = new[]{
                                    //    new SqlParameter("@Name", userDeails.full_name != null ? userDeails.full_name : string.Empty),
                                    //    new SqlParameter("@DepartmentName",userDeails.department_name != null ? userDeails.department_name : string.Empty),                                    
                                    //    new SqlParameter("@DirectManagerEmployeeId", userDeails.direct_manager_name != null ? userDeails.direct_manager_name : string.Empty),
                                    //    new SqlParameter("@OfficeCountry", userDeails.office_area != null ? userDeails.office_area : string.Empty ),
                                    //    new SqlParameter("@Id", userContextSave.Id),
                                    //    new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow)),
                                    //    new SqlParameter("@ModifiedBy", userId),

                                    //};
                                    //    _emamiContext.BulkUpdateProxy(sqlUpdate, parameters);
                                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                    {

                                        var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                        var result1 = conn.Execute(sqlUpdate, new
                                        {
                                            Name = userDeails.full_name != null ? userDeails.full_name : string.Empty,
                                            DepartmentName = userDeails.department_name != null ? userDeails.department_name : string.Empty,
                                            DirectManagerEmployeeId = userDeails.direct_manager_name != null ? userDeails.direct_manager_name : string.Empty,
                                            OfficeCountry = userDeails.office_area != null ? userDeails.office_area : string.Empty,
                                            ModifiedDate = modifiedDate,
                                            ModifiedBy = userId,
                                            Id = userContextSave.Id
                                        });

                                    }
                                }
                                else
                                {
                                    var customerLedgerCotext = new User
                                    {
                                        Code = userDeails.employee_id,
                                        Name = userDeails.full_name,
                                        DepartmentName = userDeails.department_name,
                                        DirectManagerEmployee = userDeails.direct_manager_name,
                                        OfficeCountry = userDeails.office_area,
                                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                        CreatedBy = userId
                                    };
                                    userList.Add(customerLedgerCotext);
                                    dataSynced++;
                                }
                            }
                            else
                            {
                                errorMessageList.Add(errorMessage);
                                errorRecordList.Add(userDeails);
                            }
                        }

                        if (null != userList && userList.Any())
                        {
                            _emamiContext.BulkInsertProxy(userList);
                        }
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = result.employee_data;
                        sapDataSyncResultDto.SuccessRecordDetailsResponse = result.employee_data.Except(errorRecordList).ToList();

                    }
                    //}
                }

                if (errorMessageList.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Response = sapDataSyncResultDto;
                    resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList)}");

                }
                else
                {
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = sapDataSyncResultDto;
                    resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                }
                sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;

                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, null, subject);

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = result.employee_data;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _logger.Error(message);
                _sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
            }
        }

        public void EmployeeRequestInActiveUsers()
        {
            _methodName = "EmployeeRequestInActiveUsers";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var sapDataSyncResultDto = new SapDataSyncResultDto() { SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow) };
            var syncFolder = ConsoleSettings.SaudaFolder;
            var errorMessageList = new List<string>();
            var dataSynced = 0;
            var subject = string.Concat(ConsoleSettings.EmployeeRequestActiveUsersSubject, " ", ConsoleSettings.AppToSapDataSyncEmailSubject);
            var csvFileName = ConsoleSettings.EmployeeRequestActiveUsersCsv;
            var resultDto = new ResultDto();
            var result = new DarwinboxAPIResponsetDTO();
            try
            {
                using (var _emamiContext = new AdaniContext())
                {

                    var LastModifiedDate = DateTime.Now.AddDays(Config.LastModifiedDate).ToString("dd-MM-yyyy HH:mm:ss");
                    var data = new DarwinboxAPIRequestDTO()
                    {
                        api_key = Constants.InActiveUserAPIKey,
                        datasetKey = Constants.InActiveUserDatasetKey,
                        last_modified = LastModifiedDate
                    };

                    //var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    _logger.Info($"Json Input : {JsonConvert.SerializeObject(data)}");
                    var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.DarwinboxAPIUrl, data, true);
                    var json = response.Content.ReadAsStringAsync().Result;
                    var status = response.StatusCode;
                    result = JsonConvert.DeserializeObject<DarwinboxAPIResponsetDTO>(json.ToString(), UtilityHelper.GetJsonSettings());
                    _logger.Info($"Responce : {status}");
                    if (result != null && result.employee_data.IsAny())
                    {
                        var userId = Convert.ToInt32(ConfigurationManager.AppSettings["UserId"]);
                        #region Get Common Datas
                        var customerList = result.employee_data.Select(s => s.employee_id.TrimStart('0')).Distinct().ToList();
                        var userContextList = (from s in _emamiContext.Users
                                               where customerList.Contains(s.Code)
                                               select new { Id = s.Id, Code = s.Code }).ToList();
                        #endregion
                        var userList = new List<User>();
                        var errorRecordList = new List<DarwinboxEmployeeListDTO>();
                        var errorFlag = true;
                        foreach (var userDeails in result.employee_data)
                        {
                            var errorMessage = userDeails.employee_id;
                            if (userDeails == null)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.InvalidRequest, errorMessage);
                                errorFlag = false;
                            }

                            if (string.IsNullOrEmpty(userDeails.employee_id))
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.UserCodeIsEmpty, errorMessage);
                                errorFlag = false;
                            }
                            if (errorFlag)
                            {
                                var userContextSave = userContextList.FirstOrDefault(_ => _.Code == userDeails.employee_id);
                                if (userContextSave != null)
                                {
                                    var sqlUpdate = "UPDATE Users SET IsActive =@IsActive,Remarks=@Remarks, " +
                                                     "ModifiedDate =@ModifiedDate,ModifiedBy = @ModifiedBy WHERE Id = @Id";
                                    //    var parameters = new[]{
                                    //    new SqlParameter("@IsActive", false),
                                    //    new SqlParameter("@Remarks", "Deactivated from Darwinbox"),                                    
                                    //    new SqlParameter("@Id", userContextSave.Id),
                                    //    new SqlParameter("@ModifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow)),
                                    //    new SqlParameter("@ModifiedBy", userId),
                                    //};
                                    //_emamiContext.BulkUpdateProxy(sqlUpdate, parameters);

                                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                                    {

                                        var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                        var result1 = conn.Execute(sqlUpdate, new
                                        {
                                            IsActive = false,
                                            Remarks = "Deactivated from Darwinbox",
                                            ModifiedDate = modifiedDate,
                                            ModifiedBy = userId,
                                            Id = userContextSave.Id
                                        });

                                    }
                                }
                            }
                            else
                            {
                                errorMessageList.Add(errorMessage);
                                errorRecordList.Add(userDeails);
                            }
                        }

                        if (null != userList && userList.Any())
                        {
                            _emamiContext.BulkInsertProxy(userList);
                        }
                        sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                        sapDataSyncResultDto.ErrorDetailsResponse = errorRecordList;
                        sapDataSyncResultDto.TotalInputRecordDetailsResponse = result.employee_data;
                        sapDataSyncResultDto.SuccessRecordDetailsResponse = result.employee_data.Except(errorRecordList).ToList();

                    }
                    //}
                }

                if (errorMessageList.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Response = sapDataSyncResultDto;
                    resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                    _logger.Info($"Error Message : {JsonConvert.SerializeObject(errorMessageList)}");

                }
                else
                {
                    sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = sapDataSyncResultDto;
                    resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                }
                sapDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;

                _sftpConnectorService.SyncProcessForSucessAndFailed(resultDto, syncFolder, null, subject);

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                sapDataSyncResultDto.TotalInputRecordDetailsResponse = result.employee_data;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                sapDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                sapDataSyncResultDto.ExceptionMessage = message;
                resultDto.ErrorDto.Response = sapDataSyncResultDto;
                _logger.Error(message);
                _sftpConnectorService.GetDataAsync(resultDto, syncFolder, subject, csvFileName);
            }
        }
        #endregion

        #region OverDue 
        public void SaudaExpiredNotification()
        {
            _methodName = "SaudaExpiredNotification";
            try
            {

                var isEmail = false;
                var isSms = false;
                var isPushnotification = false;
                using (var _emamiContext = new AdaniContext())
                {
                    var SaudaExpireList = _emamiContext.Configurations.FirstOrDefault(x => x.Id == (int)DTO.Enums.Configuration.SaudaExpireNotification);
                    if (SaudaExpireList != null)
                    {
                        var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaExpiryNoificationEmail);
                        var dateList = SaudaExpireList.Value.Split(',');
                        using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                        {
                            AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                            foreach (var item in dateList)
                            {
                                //string notificationAction = Utility.GetEnumFromString<DTO.Enums.NotificationActions>(1);
                                var resultSet = conn.QueryMultiple("SaudaExpiredNotification", new
                                {
                                    DateRemainder = item
                                }, null, commandType: CommandType.StoredProcedure);

                                if (resultSet != null)
                                {
                                    var notificationList = resultSet.Read<SaudaExpiredNotificationAwlDto>().ToList();


                                    foreach (var sauda in notificationList)
                                    {
                                        var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.UserName, sauda.UserName).Replace(Constants.OrderNumber, sauda.SaudaNumber).Replace(Constants.Date, sauda.CreatedDate)
                                            .Replace(Constants.Expirydate, sauda.ExpiredDate).ToString();
                                        var smsTemplate = emailTemplate.SMSTemplate.Replace(Constants.UserName, sauda.UserName).Replace(Constants.OrderNumber, sauda.SaudaNumber).Replace(Constants.Date, sauda.CreatedDate)
                                            .Replace(Constants.Expirydate, sauda.ExpiredDate).ToString();
                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                        var subject = Constants.SaudaExpiryNoificationSubject;
                                        var toUsers = new List<string>();
                                        //if (isEmail)
                                        //{
                                        toUsers.Add(sauda.Email);

                                        amazonNotificationService.SftpSendEmail(toUsers, subject, plainTemplate, "", true, htmlTemplate);
                                        //}

                                        //if (isSms)
                                        //{
                                        amazonNotificationService.SendMessage(smsTemplate, sauda.MobileNumber, emailTemplate.SMSTemplateID);
                                        //await SmsSendTemplateCreation(emailTemplates, toMobileNumbers, biddingWindow, saudaAllocationTime, notificationDto.NotificationActionId);
                                        //}

                                        if (isPushnotification)
                                        {
                                            // await SendPushnotificationTemplateCreation(dealerList, emailTemplates, biddingWindow, saudaAllocationTime, pushNotificationData, notificationDto.NotificationActionId);
                                        }
                                    }


                                }
                            }

                        }
                    }


                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
        }

        public void OverDueNotification()
        {
            _methodName = "OverDueNotification";
            try
            {

                using (var _emamiContext = new AdaniContext())
                {
                    var SaudaExpireList = _emamiContext.Configurations.FirstOrDefault(x => x.Id == (int)DTO.Enums.Configuration.SaudaExpireNotification);
                    if (SaudaExpireList != null)
                    {
                        var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.OverDueNotificationEmail);
                        var dateList = SaudaExpireList.Value.Split(',');
                        using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                        {
                            AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                            foreach (var item in dateList)
                            {
                                //string notificationAction = Utility.GetEnumFromString<DTO.Enums.NotificationActions>(1);
                                var resultSet = conn.QueryMultiple("SP_OverDueNotification", new
                                {
                                    DateRemainder = item
                                }, null, commandType: CommandType.StoredProcedure);

                                if (resultSet != null)
                                {
                                    var notificationList = resultSet.Read<OverDueNotificationAwlDto>().ToList();


                                    foreach (var sauda in notificationList)
                                    {
                                        var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.UserName, sauda.UserName).Replace(Constants.Date, sauda.DueDate)
                                            .Replace(Constants.Amount, sauda.DueAmount.ToString()).ToString();
                                        var smsTemplate = emailTemplate.SMSTemplate.Replace(Constants.UserName, sauda.UserName).Replace(Constants.Date, sauda.DueDate)
                                            .Replace(Constants.Amount, sauda.DueAmount.ToString()).ToString();
                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                        var subject = Constants.OverDueNotificationSubject;
                                        var toUsers = new List<string>();
                                        //if (isEmail)
                                        //{
                                        toUsers.Add(sauda.Email);

                                        amazonNotificationService.SftpSendEmail(toUsers, subject, plainTemplate, "", true, htmlTemplate);
                                        //}

                                        //if (isSms)
                                        //{
                                        amazonNotificationService.SendMessage(smsTemplate, sauda.MobileNumber, emailTemplate.SMSTemplateID);
                                        //await SmsSendTemplateCreation(emailTemplates, toMobileNumbers, biddingWindow, saudaAllocationTime, notificationDto.NotificationActionId);
                                        //}

                                        //if (isPushnotification)
                                        //{
                                        // await SendPushnotificationTemplateCreation(dealerList, emailTemplates, biddingWindow, saudaAllocationTime, pushNotificationData, notificationDto.NotificationActionId);
                                        //}
                                    }


                                }
                            }

                        }
                    }


                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
        }

        #endregion

        #endregion

        #region Call Recording

        public ResultDto DialerMobileNumberByBDODetails(CallRecordingGetInputDto inputDto)
        {
            var resultDto = new ResultDto();
            _methodName = "SaudaConversionAPPToSAP";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputDto)}");
            try
            {
                var saudaConversionViewDto = new UserMobileNumberDetails();
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.MobileNumber == inputDto.DialerMobileNumber && _.IsActive);
                if (inputDto.DealerId != 0)
                {
                    if (userContext == null)
                    {
                        if (inputDto.DialerMobileNumber.Count() == 12)
                        {
                            var mobilenumber = inputDto.DialerMobileNumber.Remove(0, 2);
                            userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.MobileNumber == mobilenumber && _.IsActive);
                            if (userContext == null)
                            {
                                return _resultService.ErrorMessage(Constants.DialerMobileNumberNotInDeal);
                            }
                        }
                        else
                        {
                            return _resultService.ErrorMessage(Constants.DialerMobileNumberNotInDeal);
                        }

                        if (userContext != null)
                        {
                            var dealerdetails = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == inputDto.DealerId && _.IsActive).Select(user => new ReceiverMobileNumberDetails { ReceiverId = user.Id, ReceiverMobileNumber = user.MobileNumber, ReceiverCode = user.Code, ReceiverName = user.Name }).ToList();

                            saudaConversionViewDto = new UserMobileNumberDetails()
                            {
                                DailerCode = userContext.Code,
                                DailerId = userContext.Id,
                                DailerMobileNumber = userContext.MobileNumber,
                                DailerName = userContext.Name,
                                ReceiverMobileNumberDetailsList = dealerdetails == null ? new List<ReceiverMobileNumberDetails>() : dealerdetails
                            };

                            saudaConversionViewDto.DailerMobileNumber = saudaConversionViewDto.DailerMobileNumber.Count() == 12 ? saudaConversionViewDto.DailerMobileNumber : string.Concat("91", saudaConversionViewDto.DailerMobileNumber);
                            foreach (var data in saudaConversionViewDto.ReceiverMobileNumberDetailsList)
                            {
                                data.ReceiverMobileNumber = data.ReceiverMobileNumber.Count() == 12 ? data.ReceiverMobileNumber : string.Concat("91", data.ReceiverMobileNumber);
                            }
                        }
                    }
                    else
                    {
                        var dealerdetails = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == inputDto.DealerId && _.IsActive).Select(user => new ReceiverMobileNumberDetails { ReceiverId = user.Id, ReceiverMobileNumber = user.MobileNumber, ReceiverCode = user.Code, ReceiverName = user.Name }).ToList();

                        saudaConversionViewDto = new UserMobileNumberDetails()
                        {
                            DailerCode = userContext.Code,
                            DailerId = userContext.Id,
                            DailerMobileNumber = userContext.MobileNumber,
                            DailerName = userContext.Name,
                            ReceiverMobileNumberDetailsList = dealerdetails == null ? new List<ReceiverMobileNumberDetails>() : dealerdetails
                        };

                        saudaConversionViewDto.DailerMobileNumber = saudaConversionViewDto.DailerMobileNumber.Count() == 12 ? saudaConversionViewDto.DailerMobileNumber : string.Concat("91", saudaConversionViewDto.DailerMobileNumber);
                        foreach (var data in saudaConversionViewDto.ReceiverMobileNumberDetailsList)
                        {
                            data.ReceiverMobileNumber = data.ReceiverMobileNumber.Count() == 12 ? data.ReceiverMobileNumber : string.Concat("91", data.ReceiverMobileNumber);
                        }
                    }
                }
                else
                {

                    if (userContext == null)
                    {
                        if (inputDto.DialerMobileNumber.Count() == 12)
                        {
                            var mobilenumber = inputDto.DialerMobileNumber.Remove(0, 2);
                            userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.MobileNumber == mobilenumber && _.IsActive);
                            if (userContext == null)
                            {
                                return _resultService.ErrorMessage(Constants.DialerMobileNumberNotInDeal);
                            }
                        }
                        else
                        {
                            return _resultService.ErrorMessage(Constants.DialerMobileNumberNotInDeal);
                        }
                    }

                    var userRole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id).RoleId;
                    if (userRole == (int)DTO.Enums.Role.Dealer)
                    {
                        var BDOIdsContext = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.CustomerId == userContext.Id).Select(user => user.UserId).ToList();
                        var BDORoleUsers = _emamiContext.UserRoles.AsNoTracking().Where(_ => BDOIdsContext.Contains(_.UserId) && _.RoleId == (int)DTO.Enums.Role.StateTrader).Select(a => a.UserId).ToList();
                        var BDODetailsContext = _emamiContext.Users.AsNoTracking().Where(_ => BDORoleUsers.Contains(_.Id) && _.IsActive).Select(user => new ReceiverMobileNumberDetails { ReceiverId = user.Id, ReceiverMobileNumber = user.MobileNumber, ReceiverCode = user.Code, ReceiverName = user.Name }).ToList();
                        //if (inputDto.VerticalId != 0)
                        //{
                        // BDODetailsContext = _emamiContext.Users.AsNoTracking().Where(_ => BDOIdsContext.Contains(_.Id) /*&& _.VerticalId == inputDto.VerticalId*/).Select(user => new ReceiverMobileNumberDetails { ReceiverId = user.Id, ReceiverMobileNumber = user.MobileNumber, ReceiverCode = user.Code, ReceiverName = user.Name }).ToList();
                        //}

                        saudaConversionViewDto = new UserMobileNumberDetails()
                        {
                            DailerCode = userContext.Code,
                            DailerId = userContext.Id,
                            DailerMobileNumber = userContext.MobileNumber,
                            DailerName = userContext.Name,
                            ReceiverMobileNumberDetailsList = BDODetailsContext == null ? new List<ReceiverMobileNumberDetails>() : BDODetailsContext
                        };

                        saudaConversionViewDto.DailerMobileNumber = saudaConversionViewDto.DailerMobileNumber.Count() == 12 ? saudaConversionViewDto.DailerMobileNumber : string.Concat("91", saudaConversionViewDto.DailerMobileNumber);
                        foreach (var data in saudaConversionViewDto.ReceiverMobileNumberDetailsList)
                        {
                            data.ReceiverMobileNumber = data.ReceiverMobileNumber.Count() == 12 ? data.ReceiverMobileNumber : string.Concat("91", data.ReceiverMobileNumber);
                        }
                    }
                    else if (userRole == (int)DTO.Enums.Role.StateTrader)
                    {
                        var dealerIdsContext = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == userContext.Id).Select(user => user.CustomerId).ToList();
                        var dealerdetails = _emamiContext.Users.AsNoTracking().Where(_ => dealerIdsContext.Contains(_.Id) && _.IsActive).Select(user => new ReceiverMobileNumberDetails { ReceiverId = user.Id, ReceiverMobileNumber = user.MobileNumber, ReceiverCode = user.Code, ReceiverName = user.Name }).ToList();
                        //if (inputDto.VerticalId != 0)
                        //{
                        //    dealerdetails = _emamiContext.Users.AsNoTracking().Where(_ => dealerIdsContext.Contains(_.Id) /*&& _.VerticalId == inputDto.VerticalId*/).Select(user => new ReceiverMobileNumberDetails { ReceiverId = user.Id, ReceiverMobileNumber = user.MobileNumber, ReceiverCode = user.Code, ReceiverName = user.Name }).ToList();
                        //}
                        saudaConversionViewDto = new UserMobileNumberDetails()
                        {
                            DailerCode = userContext.Code,
                            DailerId = userContext.Id,
                            DailerMobileNumber = userContext.MobileNumber,
                            DailerName = userContext.Name,
                            ReceiverMobileNumberDetailsList = dealerdetails == null ? new List<ReceiverMobileNumberDetails>() : dealerdetails
                        };

                        saudaConversionViewDto.DailerMobileNumber = saudaConversionViewDto.DailerMobileNumber.Count() == 12 ? saudaConversionViewDto.DailerMobileNumber : string.Concat("91", saudaConversionViewDto.DailerMobileNumber);
                        foreach (var data in saudaConversionViewDto.ReceiverMobileNumberDetailsList)
                        {
                            data.ReceiverMobileNumber = data.ReceiverMobileNumber.Count() == 12 ? data.ReceiverMobileNumber : string.Concat("91", data.ReceiverMobileNumber);
                        }
                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                resultDto.SuccessDto.Response = saudaConversionViewDto;
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                _logger.Error(message);
            }
            return resultDto;
        }
        public ResultDto SaveCallRecordingOfCustomers(CallRecordingInputDto inputDto, HttpPostedFile file, string imageFileName, int pageId)
        {
            _methodName = "SaveCallRecordingOfCustomers";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (string.IsNullOrEmpty(inputDto.DialerMobileNumber))
                {
                    return _resultService.ErrorMessage(Constants.DialerMobileNumberMissing);
                }
                if (string.IsNullOrEmpty(inputDto.ReceiverMobileNumber))
                {
                    return _resultService.ErrorMessage(Constants.ReceiverMobileNumberMissing);
                }
                if (inputDto.DialerId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DialerIdMissing);
                }
                if (inputDto.ReceiverId == 0)
                {
                    return _resultService.ErrorMessage(Constants.ReceiverIdMissing);
                }
                //var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                //if (userContext == null)
                //{
                //    return _resultService.ErrorMessage(Constants.UserNotFound);
                //}

                //var userRoleId = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id).RoleId;

                ////For Dealer app  - Call has made to Bdo so saved BdoId in UserId column
                //var userId = (userRoleId == (int)DTO.Enums.Role.Dealer) ? inputDto.BdoId : inputDto.DealerId;
                var FileSaveResult = UploadMediaAndReturnFileName(file, imageFileName, pageId);
                if (FileSaveResult.IsSuccess == true)
                {
                    // FileSaveResult.SuccessDto.Response = imageFileName;
                    var dailerrole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(userrole => userrole.UserId == inputDto.DialerId).RoleId;
                    var audioFileContext = new AudioFileDetailsForActiveCustomers()
                    {
                        UserId = dailerrole == (int)DTO.Enums.Role.Dealer ? inputDto.DialerId : inputDto.ReceiverId,
                        AudioFileName = FileSaveResult.SuccessDto.Response.ToString(),
                        MediaTypeId = (int)DTO.Enums.MediaType.Audio,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        CreatedBy = inputDto.DialerId,
                        ReceiverId = inputDto.ReceiverId,
                        DialerId = inputDto.DialerId,
                        DialerMobileNumber = inputDto.DialerMobileNumber,
                        ReceiverMobileNumber = inputDto.ReceiverMobileNumber,
                        CallDuation = inputDto.CallDuation,
                        CallStartTime = inputDto.CallStartTime,
                    };
                    _emamiContext.AudioFileDetailsForActiveCustomers.Add(audioFileContext);
                    _emamiContext.SaveChanges();
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = Constants.CallRecordedSavedSuccess;

                }
                else
                {
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = Constants.CallRecordedSavedFailed;
                }
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetDealerDetailsByBDO(CallRecordingGetInputDto inputDto)
        {
            var resultDto = new ResultDto();
            _methodName = "GetDealerDetailsByBDO";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputDto)}");
            try
            {
                var data = new ReceiverMobileNumberDetails();
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var bdoContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(user => user.MobileNumber == inputDto.DialerMobileNumber && user.IsActive);
                if (bdoContext != null)
                {
                    //DialerMobileNumber -> BDO mobile number 
                    var userContext = _emamiContext.BdoChoosenDealerDetailsDuringCall.AsNoTracking().OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.BDOId == bdoContext.Id);

                    if (userContext != null)
                    {
                        var currentdate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        TimeSpan timedifference = currentdate.Subtract(userContext.CreatedDate);
                        if (timedifference.TotalMinutes > Convert.ToDouble(Config.CallRecordingDealerDetailsExpireMins))
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Message = Constants.DataExpired;
                        }
                        else
                        {
                            data.ReceiverId = userContext.DealerId;
                            data.ReceiverMobileNumber = userContext.DealerMobileNumber;
                            data.DialerId = userContext.BDOId;
                            data.DialerMobileNumber = userContext.BDOMobileNumber;
                            resultDto.IsSuccess = true;
                            resultDto.SuccessDto.Message = Constants.SapSyncSuccessMessage;
                            resultDto.SuccessDto.Response = data;
                        }
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    }
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetBDODetailsWithMasterData(CallRecordingGetInputDto inputDto)
        {
            var resultDto = new ResultDto();
            _methodName = "GetBDODetailsWithMasterData";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputDto)}");
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                //DialerMobileNumber -> Dealer mobile number 
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.MobileNumber == inputDto.DialerMobileNumber);
                if (userContext != null)
                {

                    var bdoIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(ucm => ucm.CustomerId == userContext.Id).Select(a => a.UserId).ToList();
                    var userrolechecking = _emamiContext.UserRoles.AsNoTracking().Where(userrole => bdoIds.Contains(userrole.UserId) && userrole.RoleId == (int)DTO.Enums.Role.StateTrader).Select(a => a.UserId).Distinct().ToList();
                    var BdoList = _emamiContext.Users.AsNoTracking().Where(bdo => userrolechecking.Contains(bdo.Id)).Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        MobileNumber = s.MobileNumber
                    }).ToList();

                    var Bdodetails = _emamiContext.Users.AsNoTracking().Where(bdo => userrolechecking.Contains(bdo.Id)).Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        MobileNumber = s.MobileNumber
                    }).ToList();

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = Bdodetails;
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                }

            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.ErrorSapMessage;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto UploadMediaAndReturnFileName(HttpPostedFile file, string imageFileName, int pageId)
        {
            _methodName = "UploadMedia";
            try
            {
                _logger.Info($"Save media service {DateTime.Now}");
                var folderName = string.Empty;
                //if (file.ContentLength > Config.MaxFileSize)
                //{
                //    return _resultService.ErrorMessage(Constants.MaxFileSize);
                //}

                if (pageId == (int)DTO.Enums.PageType.Competitor)
                {
                    folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.Competitor);
                }
                else if (pageId == (int)DTO.Enums.PageType.ProspectiveDealer)
                {
                    folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.ProspectiveDealer);
                }
                else if (pageId == (int)DTO.Enums.PageType.Dealer)
                {
                    folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.Dealer);
                }
                else if (pageId == (int)DTO.Enums.PageType.Support)
                {
                    folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.Support);
                }
                else if (pageId == (int)DTO.Enums.PageType.DynamicFormAttachments)
                {
                    folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.DynamicFormAttachments);
                }
                else if (pageId == (int)DTO.Enums.PageType.AudioFiles)
                {
                    folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.AudioFiles);
                }
                else if (pageId == (int)DTO.Enums.PageType.ImagesSaudaMappingwithCallRecording)
                {
                    folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.ImagesSaudaMappingwithCallRecording);
                }
                var directory = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(@"~/" + ConfigurationManager.AppSettings["UploadAttachments"]), folderName);

                if (pageId == (int)DTO.Enums.PageType.ImagesSaudaMappingwithCallRecording || pageId == (int)DTO.Enums.PageType.AudioFiles)
                {
                    directory = Config.WebsitePhysicalPath + Path.Combine(ConfigurationManager.AppSettings["UploadMediaPaths"], folderName);
                }

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                var guid = Guid.NewGuid();
                var ext = Path.GetExtension(imageFileName);
                imageFileName = guid + ext;
                var filename = Path.Combine(directory, imageFileName);
                //Deletion exists file  
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                _logger.Info($"File write started {DateTime.Now}");
                file.SaveAs(filename);
                _logger.Info($"File write completed {DateTime.Now}");
                var imageNameAddDto = new ImageNameAddDto
                {
                    Url = imageFileName,
                    PageId = pageId
                };
                var result = new ResultDto();
                result.IsSuccess = true;
                result.SuccessDto.Response = imageFileName;
                return result;

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region Open Contract Request Raise
        public void ContractOpenRequestRaise(ContractOpenRequestRaiseDto contractRequestDto)
        {
            _methodName = "ContractOpenRequestRaise";
            try
            {
                List<OpenContractRequestDTO> data = new List<OpenContractRequestDTO>();
                var inputDto = new OpenContractRequestDTO
                {
                    SoldToParty = contractRequestDto.UserCode
                };
                data.Add(inputDto);

                #region Open Contract Data Delete
                var pendingContractsDelete = "DELETE FROM PendingContracts WHERE UserId =" + contractRequestDto.UserId + " and SalesOrgId =" + contractRequestDto.SalesOrganizationId + " and DistChnlId =" + contractRequestDto.DistributionChannelId + " and DivisionId =" + contractRequestDto.DivisionId;
                _logger.Info($"Query Pending Contracts Delete : {pendingContractsDelete}");
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {
                    var result = conn.Execute(pendingContractsDelete, new
                    {

                    });
                }
                #endregion

                ContractOpenBalanceRequest(data, contractRequestDto.SalesOrganizationCode, contractRequestDto.DistributionChannelCode, contractRequestDto.DivisionCode);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
        }
        #endregion

        #region AccountStatement

        public void AccountStatement(List<SAPAccountStatementDto> inputDto)
        {
            _methodName = "AccountStatement";
            try
            {
                _logger.Info($"SAP Service Start AccountStatement : {ServiceName} Controller-Method {_methodName}");
                _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputDto)}");

                using (var _emamiContext = new AdaniContext())
                {
                    var user = new User();

                    var groupedData = inputDto.GroupBy(dto => new { dto.SoldToPary, dto.Message });

                    foreach (var group in groupedData)
                    {
                        // Directly use group.Key to access SoldToPary and Message
                        var soldToPary = group.Key.SoldToPary;
                        var message = group.Key.Message;

                        user = _emamiContext.Users.FirstOrDefault(u => u.Code == soldToPary);
                        if (user != null && user.RegistrationTypeId > 0 && !string.IsNullOrEmpty(user.PushTokenKey))
                        {
                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                            {
                                PushTokenKey = user.PushTokenKey,
                                RegistrationTypeId = Convert.ToInt32(group.First().Request_Id), // Assuming all items in group have the same Request_Id
                                Message = message, // Use grouped message
                                Title = "Account Statement",
                            };

                            _notificationService.SendPushNotificationThroughFirebaseNew(pushNotificationInputDto);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var logMessage = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(logMessage);
            }
        }

        #endregion
    }
}