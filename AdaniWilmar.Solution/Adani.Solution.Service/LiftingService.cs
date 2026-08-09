using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using Dapper;
using GMCore.Helper;
using GMCore.Logger;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Adani.Solution.Service
{
    public interface ILiftingService
    {
        ResultDto GetLiftingRequestList(DealersLiftingRequestInputDto dealersLiftingRequestInputDto);
        ResultDto GetLiftingRequestDetail(IdInputDto idInputDto);
        ResultDto LiftingRequestStatusChange(LiftingRequestStatusChangeDto liftingRequestStatusChangeDto);
        ResultDto LiftingRequestCreation(LiftingRequestInputDto inputDto);
        //ResultDto GetConfirmedLiftingRequestLists(LoginUserIdDto loginUserIdDto);
        //ResultDto GetInProgressLiftingRequestLists(LoginUserIdDto loginUserIdDto);
        ResultDto GetLiftingRequestCountList(LiftingRequestListInputDto loginUserIdDto);
        ResultDto GetDealersLiftingRequestList(DealersLiftingRequestInputDto dealersLiftingRequestInputDto);

        #region Lifting Request - Web

        ResultDto GetLiftingRequestListForWeb(DealersLiftingRequestInputDto dealersLiftingRequestInputDto);
        ResultDto GetLiftingRequestDetailsForWeb(IdInputDto idInputDto);
        ResultDto LiftingRequestStatusChanges(LiftingRequestStatusChangeDto liftingRequestStatusChangeDto);
        ResultDto GetLiftingRequestListForExport(DealersLiftingRequestInputDto inputDto);
        ResultDto GetLiftingRequestWithoutEnquiryNumberListForWeb(DealersLiftingRequestInputDto dealersLiftingRequestInputDto);
        ResultDto LiftingRequestApproveForAdmin(LiftingRequestStatusChangeDto liftingRequestStatusChangeDto);
        #endregion

        ResultDto GetSaudaOrderLiftingRequestDetails(IdInputDto idInputDto);
        ResultDto GetSaudaOrderLiftingRequestExcelExport(DealersLiftingRequestInputDto inputDto);
        ResultDto GetVehicleLodabilityList(IdInputDto InputDto);

        ResultDto GetLiftingRequestListForMobile(LiftingRequestListsInputDto inputDto);
        ResultDto GetLiftingRequestSODetailsForMobile(IdInputDto inputDto);
        ResultDto GetLiftingRequestDetailForPopup(SalesOrderInputDto idInputDto);
    }

    public class LiftingService : ILiftingService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Lifting Service");
        private const string ServiceName = "Lifting Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;
        private readonly ISAPIntegrationService _sapIntegrationService;

        public LiftingService(IAdaniContext salesContext, IResultService resultService, INotificationService notificationService, ISAPIntegrationService sapIntegrationService)
        {
            try
            {
                _emamiContext = salesContext;
                _resultService = resultService;
                _notificationService = notificationService;
                _sapIntegrationService = sapIntegrationService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for lifting Service", exception);
            }
        }


        public ResultDto LiftingRequestCreation(LiftingRequestInputDto inputDto)
        {
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputDto)}");
            //string mStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",CultureInfo.InvariantCulture);

            _methodName = "LiftingRequestCreation";
            var resultDto = new ResultDto();
            try
            {
                var errorMessage = string.Empty;
                var errorFlag = false;
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                if (inputDto.CustomerRemarks == null)
                {
                    return _resultService.ErrorMessage(Constants.CustomerRemarksNotFound);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
                if (dealerContext == null)
                {
                    return _resultService.ErrorMessage(Constants.DealerNotFound);
                }
                var dealerRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == dealerContext.Id);
                if (dealerRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.DealerNotFound);
                }

                //User shipToParty = new User();
                //if (inputDto.ShipToPartyId != 0)
                //{
                //    shipToParty = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.ShipToPartyId && _.IsActive);
                //    if (shipToParty == null)
                //    {
                //        return _resultService.ErrorMessage(Constants.ShipToPartyNotFound);
                //    }
                //}
                //if (inputDto.VehicleSizeId == 0)
                //{
                //    return _resultService.ErrorMessage(Constants.VehicleSizeNotFound);
                //}

                #region Commented Code

                //if (inputDto.LiftingRequestDetails != null && inputDto.LiftingRequestDetails.Any())
                //{
                //    foreach (var item in inputDto.LiftingRequestDetails)
                //    {
                //        decimal TotalSaudaQuantitybySku = 0;
                //        decimal TotalLiftingQuantitybySku = 0;
                //        decimal TotalSaudaQuantityCasebySku = 0;
                //        decimal TotalLiftingQuantityCasebySku = 0;
                //        //decimal TotalPendingQuantitybySku = 0;
                //        //decimal TotalPendingQuantityCasebySku = 0;

                //        var TotalSaudaSkuContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda.UserId == inputDto.DealerId
                //&& _.SkuId == item.SKUId &&
                //(_.StatusId == (int)DTO.Enums.Status.Approved || _.StatusId == (int)DTO.Enums.Status.Completed)).ToList();
                //        if (TotalSaudaSkuContext != null && TotalSaudaSkuContext.Any())
                //        {
                //            TotalSaudaQuantitybySku = TotalSaudaSkuContext.Sum(_ => _.BidQuantity);
                //            TotalSaudaQuantityCasebySku = TotalSaudaSkuContext.Sum(_ => _.BidQuantityCase);
                //        }

                //        var TotalSkuLiftedContext = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.SkuId == item.SKUId
                //    && _.LiftingRequest.UserId == inputDto.DealerId
                //          && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected);

                //        if (TotalSkuLiftedContext != null && TotalSkuLiftedContext.Any())
                //        {
                //            TotalLiftingQuantitybySku = TotalSkuLiftedContext.Sum(_ => _.LiftingQuantity);
                //            TotalLiftingQuantityCasebySku = TotalSkuLiftedContext.Sum(_ => _.LiftingQuantityCase);
                //        }

                //        var SkuAllowedQuantity = TotalSaudaQuantitybySku - TotalLiftingQuantitybySku;
                //        var SkuAllowedQuantityCase = TotalSaudaQuantityCasebySku - TotalLiftingQuantityCasebySku;

                //        //input lifting quantity in case
                //        if (Convert.ToDecimal(String.Format(Constants.DefaultDecimalPlacesForMT, _resultService.ConvertCasetoMetricTon(item.LiftingQuantity, item.SKUId))) > SkuAllowedQuantity)
                //        {
                //            errorFlag = true;
                //            var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == item.SKUId);
                //            if (skuContext != null)
                //            {
                //                if (string.IsNullOrEmpty(errorMessage))
                //                {
                //                    errorMessage = Constants.BindErrorMessage(skuContext.SkuName + " - " + (Constants.IndentRequestIsExceeds + Math.Round(SkuAllowedQuantityCase, 0) + " Case(s)"), errorMessage);
                //                }
                //                else
                //                {
                //                    errorMessage = Constants.BindErrorMessage((System.Environment.NewLine + skuContext.SkuName + " - " + Constants.IndentRequestIsExceeds + Math.Round(SkuAllowedQuantityCase, 0) + " Case(s)"), errorMessage);
                //                }
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    resultDto.IsSuccess = false;
                //    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                //    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                //    return resultDto;
                //}

                //string lrStartTime = "";
                //string lrEndTime = "";
                //string lrdStartTime = "";
                //string lrdEndTime = "";
                //string emailStartTime = "";
                //string emailEndTime = "";

                #endregion

                int StatusId = 0;
                var LoginRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.RoleId == (int)DTO.Enums.Role.Dealer);
                if (LoginRoleContext != null)
                {
                    StatusId = (int)DTO.Enums.Status.Pending;
                }
                else
                {
                    StatusId = (int)DTO.Enums.Status.Approved;
                }

                decimal LiftingQuantityInMT = 0;
                //decimal minimumVehicleLodability = 0;
                //decimal minimumVolumeLodability = 0;
                //decimal VehicleLodability = 0;
                //decimal VolumeLodability = 0;
                //var VehicleLodabilityContext = _emamiContext.VehicleLodability.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.VehicleSizeId);
                //if (VehicleLodabilityContext != null)
                //{
                //    VehicleLodability = VehicleLodabilityContext.VehicleSize;
                //}


                //if (inputDto.LiftingRequestDetails != null && inputDto.LiftingRequestDetails.Any())
                //{
                //    //1.If the user enters less than XX% of the vehicle size than app will give a warning(Kindly increase the quantity to utilise full truck load capacity) to the user and will not allow to save the indent request. 
                //    //2.If the total requested quantity more than vehicle capacity than it will allow to save the order with a message – “The requested quantity is more than vehicle capacity”
                //    var key = UtilityHelper.GetEnumDescription((DTO.Enums.Configuration.MinimumVehicleCapacityinPercent));
                //    var key1 = DTO.Enums.Configuration.MinimumVehicleCapacityinPercent.ToString();
                //    var configurationsContext = _emamiContext.Configurations.FirstOrDefault(_ => _.Key == key1);
                //    if (configurationsContext != null && !string.IsNullOrEmpty(configurationsContext.Value) && Convert.ToDecimal(configurationsContext.Value) > 0)
                //    {
                //        minimumVehicleLodability = Convert.ToDecimal(configurationsContext.Value);
                //    }
                //    else
                //    {
                //        return _resultService.ErrorMessage(Constants.WeightLoadabilityPercentageIsZero);
                //    }

                //    var volumeLoadabilityContext = _emamiContext.VolumeLoadability.AsNoTracking();
                //    foreach (var item in inputDto.LiftingRequestDetails)
                //    {
                //        var grossweight = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == item.SKUId) != null ? _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == item.SKUId).GrossWeight : 0;
                //        LiftingQuantityInMT = LiftingQuantityInMT + ((grossweight * item.LiftingQuantity) / VehicleLodabilityContext.VehicleSize);
                //        //_resultService.ConvertCasetoMetricTon(item.LiftingQuantity, item.SKUId);
                //        if (inputDto.LiftingRequestDetails.Count > 1)
                //        {
                //            item.MaxAllowable = volumeLoadabilityContext.FirstOrDefault(_ => _.SkuId == item.SKUId && _.IsActive && _.VehicleSize == VehicleLodabilityContext.VehicleSize && _.PlantId == inputDto.PlantId) != null ? volumeLoadabilityContext.FirstOrDefault(_ => _.SkuId == item.SKUId && _.IsActive && _.VehicleSize == VehicleLodabilityContext.VehicleSize && _.PlantId == inputDto.PlantId).MaxAllowableMultiplesku : 0;
                //        }
                //        else
                //        {
                //            item.MaxAllowable = volumeLoadabilityContext.FirstOrDefault(_ => _.SkuId == item.SKUId && _.IsActive && _.VehicleSize == VehicleLodabilityContext.VehicleSize && _.PlantId == inputDto.PlantId) != null ? volumeLoadabilityContext.FirstOrDefault(_ => _.SkuId == item.SKUId && _.IsActive && _.VehicleSize == VehicleLodabilityContext.VehicleSize && _.PlantId == inputDto.PlantId).MaxAllowableSinglesku : 0;
                //        }
                //    }
                //    LiftingQuantityInMT = LiftingQuantityInMT * 100;

                //    //if (LiftingQuantityInMT < VehicleLodability)
                //    //{
                //    //    return _resultService.ErrorMessage(Constants.KindlyincreaseQuantity);
                //    //}  

                //    foreach (var item in inputDto.LiftingRequestDetails)
                //    {
                //        if (item.MaxAllowable > 0)
                //        {
                //            VolumeLodability = VolumeLodability + ((item.LiftingQuantity / item.MaxAllowable));
                //        }

                //    }
                //    VolumeLodability = VolumeLodability * 100;
                //    var MinimumVolumeCapacityinPercentkey = UtilityHelper.GetEnumDescription((DTO.Enums.Configuration.MinimumVolumeCapacityinPercent));
                //    var configurationContextForVolumeMininum = _emamiContext.Configurations.FirstOrDefault(_ => _.Key == MinimumVolumeCapacityinPercentkey);
                //    if (configurationsContext != null && !string.IsNullOrEmpty(configurationsContext.Value) && Convert.ToDecimal(configurationsContext.Value) > 0)
                //    {
                //        minimumVolumeLodability = Convert.ToDecimal(configurationsContext.Value);
                //    }
                //    else
                //    {
                //        return _resultService.ErrorMessage(Constants.VolumeLoadabilityPercentageIsZero);
                //    }


                //    if (LiftingQuantityInMT < minimumVehicleLodability)
                //    {
                //        if (VolumeLodability < minimumVolumeLodability)
                //        {
                //            return _resultService.ErrorMessage(Constants.KindlyincreaseQuantity);
                //        }
                //    }
                //}

                LiftingRequestNotificationDto liftingRequestNotificationDto = new LiftingRequestNotificationDto();
                List<LiftingRequestSkuDto> liftingRequestSkuList = new List<LiftingRequestSkuDto>();
                if (!errorFlag)
                {
                    //lrStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
                    //                        CultureInfo.InvariantCulture);
                    var liftingRequestContext = new LiftingRequest
                    {
                        LiftingDate = (DateTime)DateHelper.UtcToIndia(DateTime.UtcNow),
                        ApproveDate = (DateTime)DateHelper.UtcToIndia(DateTime.UtcNow),
                        UserId = inputDto.DealerId,
                        LiftingStatusId = (int)DTO.Enums.LiftingRequestStatus.Inprogress,
                        StatusId = StatusId,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = (DateTime)DateHelper.UtcToIndia(DateTime.UtcNow),
                        CustomerRemarks = inputDto.CustomerRemarks,
                        //VehicleSizeId = inputDto.VehicleSizeId,
                        ShipToPartyId = inputDto.ShipToPartyId != 0 ? (long?)inputDto.ShipToPartyId : null,
                        PlantId = inputDto.PlantId,
                        //DepotId = inputDto.DepotId,
                        //SaudaId = inputDto.SaudaOrderId,
                        //SaudaNumber = inputDto.SaudaNumber,
                       // QantityInCase = inputDto.QantityInCase,
                    };
                    _emamiContext.LiftingRequest.Add(liftingRequestContext);
                    _emamiContext.SaveChanges();
                    liftingRequestContext.LiftingRequestNumber = liftingRequestContext.Id.ToString();
                    //lrEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
                    //                        CultureInfo.InvariantCulture);                   

                    if (inputDto.LiftingRequestDetails != null && inputDto.LiftingRequestDetails.Any())
                    {
                        //lrdStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
                        //                    CultureInfo.InvariantCulture);
                        long count = 1;
                        int i = 0;
                        foreach (var item in inputDto.LiftingRequestDetails)
                        {
                            item.SaudaNumber = item.SaudaNumber.Split('-').FirstOrDefault();
                            var sauda = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == item.SaudaNumber);
                            var skuData = _emamiContext.Skus.AsNoTracking().FirstOrDefault(f => f.Id == item.SKUId);
                            var skuUomMappings = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(f => f.SkuId == item.SKUId);
                            var uomName = "";
                            if(skuUomMappings != null)
                            {
                                uomName = _emamiContext.Uom.AsNoTracking().FirstOrDefault(f => f.Id == skuUomMappings.UomId).Name;
                            }

                            i = i + 10;
                            var liftingReq = new LiftingRequestDetails
                            {
                                LiftingRequestId = liftingRequestContext.Id,
                                SkuId = item.SKUId,
                                ItemNo = i.ToString(),
                                OilTypeId = (long)skuData.OilTypeId,
                                LiftingQuantity = _resultService.ConvertCasetoMetricTon(item.LiftingQuantity, item.SKUId),
                                LiftingQuantityCase = item.LiftingQuantity,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                SaudaNumber = item.SaudaNumber,
                                SaudaOrderId = item.SaudaOrderId,
                                SalesOrganizationId = sauda != null ? sauda.SalesOrganizationId :0,
                                DistributionhannelId = sauda != null ? sauda.DistributionChannelId : 0,
                                DivisionId = sauda!= null ? sauda.DivisionId : 0
                            };
                            _emamiContext.LiftingRequestDetails.Add(liftingReq);
                            _emamiContext.SaveChanges();

                            liftingRequestSkuList.Add(new LiftingRequestSkuDto()
                            {
                                ItemLine = count,
                                Sku = skuData.SkuName,
                                QtyInCase = item.LiftingQuantity,
                                UOM = uomName != null ? uomName : string.Empty
                            });
                            count++;
                        }
                        //lrdEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
                        //                    CultureInfo.InvariantCulture);

                        liftingRequestNotificationDto.LiftingRequestNumber = liftingRequestContext.Id.ToString();
                        liftingRequestNotificationDto.RemarksFromApp = inputDto.CustomerRemarks;
                        liftingRequestNotificationDto.UserId = inputDto.DealerId;
                        liftingRequestNotificationDto.CreatedBy = inputDto.LoginUserId;
                        string cityName = _emamiContext.City.AsNoTracking().FirstOrDefault(f => f.Id == dealerContext.CityId)?.CityName;
                        string districtName = _emamiContext.District.AsNoTracking().FirstOrDefault(f => f.Id == dealerContext.DistrictId)?.DistrictName;
                        string stateName = _emamiContext.State.AsNoTracking().FirstOrDefault(f => f.Id == dealerContext.StateId)?.StateName;
                        liftingRequestNotificationDto.BillToPartyName = dealerContext.Name;
                        liftingRequestNotificationDto.BillToPartyPlace = $"{cityName},{districtName},{stateName}";
                        //if (shipToParty != null)
                        //{
                        //    cityName = _emamiContext.City.AsNoTracking().FirstOrDefault(f => f.Id == shipToParty.CityId)?.CityName;
                        //    districtName = _emamiContext.District.AsNoTracking().FirstOrDefault(f => f.Id == shipToParty.DistrictId)?.DistrictName;
                        //    stateName = _emamiContext.State.AsNoTracking().FirstOrDefault(f => f.Id == shipToParty.StateId)?.StateName;
                        //}
                        //else { cityName = ""; districtName = ""; stateName = ""; }

                        //liftingRequestNotificationDto.ShipToPartyName = shipToParty.Name;
                        //liftingRequestNotificationDto.ShipToPartyPlace = string.IsNullOrEmpty(cityName) ? "" : $"{cityName},{districtName},{stateName}";
                        liftingRequestContext.LiftingRequestNumber = liftingRequestContext.Id.ToString();
                        liftingRequestNotificationDto.LiftingRequestSkuDetails = liftingRequestSkuList;
                        liftingRequestNotificationDto.APPIndentNoCreatedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                    }

                    #region Commented Code

                    //try
                    //{
                    //    emailStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
                    //                        CultureInfo.InvariantCulture);

                    //    List<User> usersContext = new List<User>();
                    //    List<string> toUsers = new List<string>();
                    //    User createdBy = new User();
                    //    User dealer = new User();
                    //    if (liftingRequestContext.CreatedBy == liftingRequestContext.UserId)
                    //    {
                    //        createdBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == liftingRequestContext.CreatedBy);
                    //        if (createdBy != null)
                    //        {
                    //            toUsers.Add(createdBy.Email);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == liftingRequestContext.CreatedBy || _.Id == liftingRequestContext.UserId).ToList();
                    //        if (usersContext != null && usersContext.Any())
                    //        {
                    //            createdBy = usersContext.FirstOrDefault(_ => _.Id == liftingRequestContext.CreatedBy);
                    //            dealer = usersContext.FirstOrDefault(_ => _.Id == liftingRequestContext.UserId);
                    //            if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                    //            {
                    //                toUsers.Add(createdBy.Email);
                    //            }
                    //            if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                    //            {
                    //                toUsers.Add(dealer.Email);
                    //            }
                    //        }
                    //    }
                    //    if ((usersContext != null && usersContext.Any()) || createdBy != null)
                    //    {
                    //        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                    //        if (_resultService.IsEmail() && toUsers != null && toUsers.Any())
                    //        {
                    //            var fromEmail = Constants.FromEmail;
                    //            var emailSubject = Constants.LiftingRequestCreationSubject;
                    //            var plainText = string.Empty;
                    //            var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.LiftingRequestCreationEmail);
                    //            if (emailTemplate != null)
                    //            {
                    //                var plainTemplate = emailTemplate.PlainTemplate;
                    //                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                    //                amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                    //            }

                    //        }
                    //        var smsPlainTemplate = string.Empty;
                    //        if (_resultService.IsSMS())
                    //        {
                    //            var smsMessage = string.Empty;
                    //            EmailTemplate smsTemplate = new EmailTemplate();
                    //            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.LiftingRequestCreationSMS);
                    //            if (smsTemplate != null)
                    //            {
                    //                smsPlainTemplate = smsTemplate.PlainTemplate;
                    //                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                    //                if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                    //                {
                    //                    amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber);
                    //                }
                    //                if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                    //                {
                    //                    amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber);
                    //                }
                    //            }
                    //        }
                    //    }

                    //    emailEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
                    //                        CultureInfo.InvariantCulture);
                    //}
                    //catch (Exception ex)
                    //{
                    //}

                    #endregion

                    //HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => LiftingRequestNotificationAsync(liftingRequestContext, cancellationToken));
                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => LiftingRequestNotificationAsyncNew(liftingRequestNotificationDto, cancellationToken));
                    bool IsReprocess = false;
                    List<long> LiftingRequestIds = new List<long>();
                    LiftingRequestIds.Add(liftingRequestContext.Id);
                    if (StatusId == (int)DTO.Enums.Status.Approved /*&& ConsoleSettings.IsInboundDirectSyncToSapAllowed*/)
                    {
                        HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                        {
                            _sapIntegrationService.GetLiftingRequestEnquiryNumberOutboundDetails(LiftingRequestIds, IsReprocess);
                        });
                    }

                }

                #region Commented Code

                //string mEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                //StringBuilder sb = new StringBuilder();
                //TimeSpan timeSpan = Convert.ToDateTime(mEndTime) - Convert.ToDateTime(mStartTime);
                //int mTotalMilliSeconds = (int)timeSpan.TotalMilliseconds;
                //TimeSpan timeSpan2 = Convert.ToDateTime(lrEndTime) - Convert.ToDateTime(lrStartTime);
                //int mTotalMilliSeconds2 = (int)timeSpan2.TotalMilliseconds;
                //TimeSpan timeSpan3 = Convert.ToDateTime(lrdEndTime) - Convert.ToDateTime(lrdStartTime);
                //int mTotalMilliSeconds3 = (int)timeSpan3.TotalMilliseconds;
                ////TimeSpan timeSpan4 = Convert.ToDateTime(emailEndTime) - Convert.ToDateTime(emailStartTime);
                ////int mTotalMilliSeconds4 = (int)timeSpan4.TotalMilliseconds;
                //sb.Append($"LoginUserId, {inputDto.LoginUserId}, LiftingRequest, StartTime, {mStartTime} ,EndTime, {mEndTime}, TotalLiftingRequestTime, {mTotalMilliSeconds}, DBOperation, LiftingRequest ,StartTime, {lrStartTime} ,EndTime, {lrEndTime}, TotalSaudaTime, {mTotalMilliSeconds2}, LiftingRequestDetails, StartTime, {lrdStartTime}, EndTime, {lrdEndTime}, TotalLiftingRequestDetailsTime, {mTotalMilliSeconds3}, Email, StartTime, {0}, EndTime, {0}, TotalEmailTime, {0}");
                //string serverFoloderPath = HostingEnvironment.MapPath("~/LogFiles/");
                //string filePath = Path.Combine(serverFoloderPath + "LiftingRequest.txt");
                //File.AppendAllText(filePath, sb.ToString() + Environment.NewLine);

                #endregion

                if (!errorFlag)
                {
                    //if (LiftingQuantityInMT > VehicleLodability)
                    //{
                    //    return _resultService.SuccessMessage(Constants.ExceededRequestedQuantity);
                    //}
                    //else
                    //{
                    return _resultService.SuccessMessage(Constants.IndentRequestSuccess);
                    // }
                }
                else
                {
                    return _resultService.ErrorMessage(errorMessage);
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public async Task LiftingRequestNotificationAsyncNew(LiftingRequestNotificationDto liftingRequest, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            try
            {
                using (AdaniContext _context = new AdaniContext())
                {
                    List<User> usersContext = new List<User>();
                    List<string> toUsers = new List<string>();
                    User createdBy = new User();
                    User dealer = new User();
                    bool isEmail = false;

                    var DealerNotificationContext = _context.TPNotification.AsNoTracking().
                                                    Join(_context.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                    .Where(_ => _.TPND.DealerId == liftingRequest.UserId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.IndentRequestCreation && _.TPND.IsActive).ToList();

                    var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                    if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                        isEmail = true;
                    else
                        isEmail = false;
                    if (liftingRequest.CreatedBy == liftingRequest.UserId)
                    {
                        usersContext = _context.Users.AsNoTracking().ToList();
                        if (usersContext != null && usersContext.Any())
                        {
                            createdBy = usersContext.FirstOrDefault(_ => _.Id == liftingRequest.CreatedBy);
                            var BdoForCorrespondingDealer = _context.UserCustomerMapping.AsNoTracking().FirstOrDefault(_ => _.CustomerId == liftingRequest.CreatedBy).UserId;
                            var BdoContext = usersContext.FirstOrDefault(_ => _.Id == BdoForCorrespondingDealer);
                            if (createdBy != null)
                            {
                                toUsers.Add(createdBy.Email);
                            }
                            if (BdoContext != null)
                            {
                                toUsers.Add(BdoContext.Email);
                            }
                        }
                    }
                    else
                    {
                        usersContext = _context.Users.AsNoTracking().Where(_ => _.Id == liftingRequest.CreatedBy || _.Id == liftingRequest.UserId).ToList();
                        if (usersContext != null && usersContext.Any())
                        {
                            createdBy = usersContext.FirstOrDefault(_ => _.Id == liftingRequest.CreatedBy);
                            dealer = usersContext.FirstOrDefault(_ => _.Id == liftingRequest.UserId);
                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                            {
                                toUsers.Add(createdBy.Email);
                            }
                            if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                            {
                                toUsers.Add(dealer.Email);
                            }
                        }
                    }
                    if ((usersContext != null && usersContext.Any()) || createdBy != null)
                    {
                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();

                        if (isEmail && toUsers != null && toUsers.Any())
                        {
                            var fromEmail = Constants.FromEmail;
                            var emailSubject = Constants.LiftingRequestCreationSubject;
                            var plainText = string.Empty;
                            var emailTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.LiftingRequestCreationEmail);
                            if (emailTemplate != null)
                            {
                                var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.LiftingRequestNumber, liftingRequest.LiftingRequestNumber);
                                var result = _notificationService.GenerateLiftingRequestEmailTemplate(liftingRequest);
                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, result);
                                await amazonNotificationService.SendEmailAsync(toUsers, emailSubject, plainText, htmlTemplate, true);
                            }
                        }
                        var smsPlainTemplate = string.Empty;

                        bool isSms = false;
                        var DealerSMSNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.SMS);
                        if (DealerSMSNotificationEnabled != null && DealerSMSNotificationEnabled.Any())
                            isSms = true;
                        else
                            isSms = false;

                        if (isSms)
                        {
                            var smsMessage = string.Empty;
                            EmailTemplate smsTemplate = new EmailTemplate();
                            smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.LiftingRequestCreationSMS);
                            if (smsTemplate != null)
                            {
                                var result = _notificationService.GenerateLiftingRequestSmsTemplate(liftingRequest);
                                smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.LiftingRequestNumber, liftingRequest.LiftingRequestNumber);
                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, result);
                                if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                {
                                    await amazonNotificationService.SendMessageAsync(smsMessage, createdBy.MobileNumber, smsTemplate.SMSTemplateID);
                                }
                                if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                {
                                    await amazonNotificationService.SendMessageAsync(smsMessage, dealer.MobileNumber, smsTemplate.SMSTemplateID);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void LiftingRequestNotificationAsync(LiftingRequest liftingRequest, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            try
            {
                using (AdaniContext _context = new AdaniContext())
                {
                    List<User> usersContext = new List<User>();
                    List<string> toUsers = new List<string>();
                    User createdBy = new User();
                    User dealer = new User();
                    bool isEmail = false;

                    var DealerNotificationContext = _context.TPNotification.AsNoTracking().
                                                    Join(_context.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                    .Where(_ => _.TPND.DealerId == liftingRequest.UserId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.IndentRequestCreation && _.TPND.IsActive).ToList();

                    var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                    if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                        isEmail = true;
                    else
                        isEmail = false;
                    if (liftingRequest.CreatedBy == liftingRequest.UserId)
                    {
                        createdBy = _context.Users.AsNoTracking().FirstOrDefault(_ => _.Id == liftingRequest.CreatedBy);
                        if (createdBy != null)
                        {
                            toUsers.Add(createdBy.Email);
                        }
                    }
                    else
                    {
                        usersContext = _context.Users.AsNoTracking().Where(_ => _.Id == liftingRequest.CreatedBy || _.Id == liftingRequest.UserId).ToList();
                        if (usersContext != null && usersContext.Any())
                        {
                            createdBy = usersContext.FirstOrDefault(_ => _.Id == liftingRequest.CreatedBy);
                            dealer = usersContext.FirstOrDefault(_ => _.Id == liftingRequest.UserId);
                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                            {
                                toUsers.Add(createdBy.Email);
                            }
                            if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                            {
                                toUsers.Add(dealer.Email);
                            }
                        }
                    }
                    if ((usersContext != null && usersContext.Any()) || createdBy != null)
                    {
                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();

                        if (isEmail && toUsers != null && toUsers.Any())
                        {
                            var fromEmail = Constants.FromEmail;
                            var emailSubject = Constants.LiftingRequestCreationSubject;
                            var plainText = string.Empty;
                            var emailTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.LiftingRequestCreationEmail);
                            if (emailTemplate != null)
                            {
                                var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.LiftingRequestNumber, liftingRequest.LiftingRequestNumber);
                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                            }

                        }
                        var smsPlainTemplate = string.Empty;

                        bool isSms = false;
                        var DealerSMSNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.SMS);
                        if (DealerSMSNotificationEnabled != null && DealerSMSNotificationEnabled.Any())
                            isSms = true;
                        else
                            isSms = false;

                        if (isSms)
                        {
                            var smsMessage = string.Empty;
                            EmailTemplate smsTemplate = new EmailTemplate();
                            smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.LiftingRequestCreationSMS);
                            if (smsTemplate != null)
                            {
                                smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.LiftingRequestNumber, liftingRequest.LiftingRequestNumber);
                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                {
                                    amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber, smsTemplate.SMSTemplateID);
                                }
                                if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                {
                                    amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber, smsTemplate.SMSTemplateID);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// Method to create Lifting Request
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        //public ResultDto LiftingRequestCreation(LiftingRequestInputDto inputDto)
        //{
        //    _methodName = "LiftingRequestCreation";
        //    var resultDto = new ResultDto();
        //    try
        //    {
        //        var errorMessage = string.Empty;
        //        var errorFlag = false;
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        if (inputDto.LoginUserId == 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidUser);
        //        }

        //        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
        //        if (userContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserNotFound);
        //        }
        //        var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
        //        if (dealerContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.DealerNotFound);
        //        }
        //        var dealerRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == dealerContext.Id);
        //        if (dealerRoleContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.DealerNotFound);
        //        }
        //        if (inputDto.ShipToPartyId != 0)
        //        {
        //            var shipToParty = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.ShipToPartyId && _.IsActive)?.Id;
        //            if (shipToParty == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.ShipToPartyNotFound);
        //            }
        //        }

        //        if (inputDto.LiftingRequestDetails != null && inputDto.LiftingRequestDetails.Any())
        //        {
        //            foreach (var item in inputDto.LiftingRequestDetails)
        //            {
        //                decimal TotalSaudaQuantitybySku = 0;
        //                decimal TotalLiftingQuantitybySku = 0;
        //                decimal TotalPendingQuantitybySku = 0;
        //                decimal TotalSaudaQuantityCasebySku = 0;
        //                decimal TotalLiftingQuantityCasebySku = 0;
        //                decimal TotalPendingQuantityCasebySku = 0;
        //                //var TotalSaudaSkuContext = (from sauda in _emamiContext.Sauda.AsNoTracking()
        //                //                            join saudaorder in _emamiContext.SaudaOrders.AsNoTracking() on sauda.Id equals saudaorder.SaudaId
        //                //                            where sauda.UserId == inputDto.DealerId
        //                //                            && (saudaorder.StatusId == (int)DTO.Enums.Status.Pending || saudaorder.StatusId == (int)DTO.Enums.Status.Approved)
        //                //                            && saudaorder.SkuId == item.SKUId
        //                //                            select saudaorder
        //                //          ).ToList();

        //                //var TotalSkuLiftedContext = (from sauda in _emamiContext.Sauda.AsNoTracking()
        //                //                             join saudaorder in _emamiContext.SaudaOrders.AsNoTracking() on sauda.Id equals saudaorder.SaudaId
        //                //                             join lifting in _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking() on saudaorder.Id equals lifting.SaudaOrderId
        //                //                             where saudaorder.SkuId == item.SKUId && sauda.UserId == inputDto.DealerId
        //                //                             select lifting
        //                //                            ).ToList();

        //                //var TotalSkuPendingContext = (from lifting in _emamiContext.LiftingRequest.AsNoTracking()
        //                //                              join liftingDetails in _emamiContext.LiftingRequestDetails.AsNoTracking() on lifting.Id equals liftingDetails.LiftingRequestId
        //                //                              where liftingDetails.SkuId == item.SKUId && lifting.UserId == inputDto.DealerId
        //                //                              && liftingDetails.StatusId != (int)DTO.Enums.Status.Deleted
        //                //                              && (liftingDetails.DeliveryOrderNumber == null || liftingDetails.DeliveryOrderNumber == string.Empty)
        //                //                              select liftingDetails
        //                //                            ).ToList();

        //                //if (TotalSaudaSkuContext != null && TotalSaudaSkuContext.Any())
        //                //{
        //                //    TotalSaudaQuantitybySku = TotalSaudaSkuContext.Sum(_ => _.BidQuantity);
        //                //    TotalSaudaQuantityCasebySku = TotalSaudaSkuContext.Sum(_ => _.BidQuantityCase);
        //                //}
        //                //if (TotalSkuLiftedContext != null && TotalSkuLiftedContext.Any())
        //                //{
        //                //    TotalLiftingQuantitybySku = TotalSkuLiftedContext.Sum(_ => _.LiftingQuantity);
        //                //    TotalLiftingQuantityCasebySku = TotalSkuLiftedContext.Sum(_ => _.LiftingQuantityCase);
        //                //}
        //                //if (TotalSkuPendingContext != null && TotalSkuPendingContext.Any())
        //                //{
        //                //    TotalPendingQuantitybySku = TotalSkuPendingContext.Sum(_ => _.LiftingQuantity);
        //                //    TotalPendingQuantityCasebySku = TotalSkuPendingContext.Sum(_ => _.LiftingQuantityCase);
        //                //}

        //                //var SkuAllowedQuantity = TotalSaudaQuantitybySku - (TotalLiftingQuantitybySku + TotalPendingQuantitybySku);
        //                //var SkuAllowedQuantityCase = TotalSaudaQuantityCasebySku - (TotalLiftingQuantityCasebySku + TotalPendingQuantityCasebySku);


        //                var TotalSaudaSkuContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda.UserId == inputDto.DealerId
        //        && _.SkuId == item.SKUId &&
        //        (_.StatusId == (int)DTO.Enums.Status.Approved || _.StatusId == (int)DTO.Enums.Status.Completed)).ToList();
        //                if (TotalSaudaSkuContext != null && TotalSaudaSkuContext.Any())
        //                {
        //                    TotalSaudaQuantitybySku = TotalSaudaSkuContext.Sum(_ => _.BidQuantity);
        //                    TotalSaudaQuantityCasebySku = TotalSaudaSkuContext.Sum(_ => _.BidQuantityCase);
        //                }

        //                var TotalSkuLiftedContext = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.SkuId == item.SKUId
        //            && _.LiftingRequest.UserId == inputDto.DealerId
        //                  && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected);

        //                if (TotalSkuLiftedContext != null && TotalSkuLiftedContext.Any())
        //                {
        //                    TotalLiftingQuantitybySku = TotalSkuLiftedContext.Sum(_ => _.LiftingQuantity);
        //                    TotalLiftingQuantityCasebySku = TotalSkuLiftedContext.Sum(_ => _.LiftingQuantityCase);
        //                }

        //                var SkuAllowedQuantity = TotalSaudaQuantitybySku - TotalLiftingQuantitybySku;
        //                var SkuAllowedQuantityCase = TotalSaudaQuantityCasebySku - TotalLiftingQuantityCasebySku;





        //                //input lifting quantity in case
        //                if (Convert.ToDecimal(String.Format(Constants.DefaultDecimalPlacesForMT, _resultService.ConvertCasetoMetricTon(item.LiftingQuantity, item.SKUId))) > SkuAllowedQuantity)
        //                {
        //                    errorFlag = true;
        //                    var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == item.SKUId);
        //                    if (skuContext != null)
        //                    {
        //                        if (string.IsNullOrEmpty(errorMessage))
        //                        {
        //                            errorMessage = Constants.BindErrorMessage(skuContext.SkuName + " - " + (Constants.IndentRequestIsExceeds + Math.Round(SkuAllowedQuantityCase, 0) + " Case(s)"), errorMessage);
        //                        }
        //                        else
        //                        {
        //                            errorMessage = Constants.BindErrorMessage((System.Environment.NewLine + skuContext.SkuName + " - " + Constants.IndentRequestIsExceeds + Math.Round(SkuAllowedQuantityCase, 0) + " Case(s)"), errorMessage);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        if (!errorFlag)
        //        {
        //            var liftingRequestContext = new LiftingRequest
        //            {
        //                LiftingDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        //                UserId = inputDto.DealerId,
        //                LiftingStatusId = (int)DTO.Enums.LiftingRequestStatus.Inprogress,
        //                StatusId = (int)DTO.Enums.Status.Pending,
        //                CreatedBy = inputDto.LoginUserId,
        //                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        //                CustomerRemarks = inputDto.CustomerRemarks,
        //                ShipToPartyId = inputDto.ShipToPartyId != 0 ? (long?)inputDto.ShipToPartyId : null,
        //            };

        //            _emamiContext.LiftingRequest.Add(liftingRequestContext);
        //            _emamiContext.SaveChanges();
        //            liftingRequestContext.LiftingRequestNumber = liftingRequestContext.Id.ToString();

        //            if (inputDto.LiftingRequestDetails != null && inputDto.LiftingRequestDetails.Any())
        //            {
        //                foreach (var item in inputDto.LiftingRequestDetails)
        //                {
        //                    var liftingReq = new LiftingRequestDetails
        //                    {
        //                        LiftingRequestId = liftingRequestContext.Id,
        //                        SkuId = item.SKUId,
        //                        OilTypeId = item.OilTypeId,
        //                        LiftingQuantity = _resultService.ConvertCasetoMetricTon(item.LiftingQuantity, item.SKUId),
        //                        LiftingQuantityCase = item.LiftingQuantity,
        //                        CreatedBy = inputDto.LoginUserId,
        //                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
        //                    };
        //                    _emamiContext.LiftingRequestDetails.Add(liftingReq);
        //                    _emamiContext.SaveChanges();
        //                }
        //            }

        //            try
        //            {
        //                List<User> usersContext = new List<User>();
        //                List<string> toUsers = new List<string>();
        //                User createdBy = new User();
        //                User dealer = new User();
        //                if (liftingRequestContext.CreatedBy == liftingRequestContext.UserId)
        //                {
        //                    createdBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == liftingRequestContext.CreatedBy);
        //                    if (createdBy != null)
        //                    {
        //                        toUsers.Add(createdBy.Email);
        //                    }
        //                }
        //                else
        //                {
        //                    usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == liftingRequestContext.CreatedBy || _.Id == liftingRequestContext.UserId).ToList();
        //                    if (usersContext != null && usersContext.Any())
        //                    {
        //                        createdBy = usersContext.FirstOrDefault(_ => _.Id == liftingRequestContext.CreatedBy);
        //                        dealer = usersContext.FirstOrDefault(_ => _.Id == liftingRequestContext.UserId);
        //                        if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
        //                        {
        //                            toUsers.Add(createdBy.Email);
        //                        }
        //                        if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
        //                        {
        //                            toUsers.Add(dealer.Email);
        //                        }
        //                    }
        //                }
        //                if ((usersContext != null && usersContext.Any()) || createdBy != null)
        //                {
        //                    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
        //                    if (_resultService.IsEmail() && toUsers != null && toUsers.Any())
        //                    {
        //                        var fromEmail = Constants.FromEmail;
        //                        var emailSubject = Constants.LiftingRequestCreationSubject;
        //                        var plainText = string.Empty;
        //                        var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.LiftingRequestCreationEmail);
        //                        if (emailTemplate != null)
        //                        {
        //                            var plainTemplate = emailTemplate.PlainTemplate;
        //                            var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
        //                            amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
        //                        }

        //                    }
        //                    var smsPlainTemplate = string.Empty;
        //                    if (_resultService.IsSMS())
        //                    {
        //                        var smsMessage = string.Empty;
        //                        EmailTemplate smsTemplate = new EmailTemplate();
        //                        smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.LiftingRequestCreationSMS);
        //                        if (smsTemplate != null)
        //                        {
        //                            smsPlainTemplate = smsTemplate.PlainTemplate;
        //                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
        //                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
        //                            {
        //                                amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber);
        //                            }
        //                            if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
        //                            {
        //                                amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber);
        //                            }
        //                        }
        //                    }
        //                    //if (_resultService.IsPushNotification())
        //                    //{
        //                    //    if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
        //                    //    {
        //                    //        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                    //        {
        //                    //            PushTokenKey = createdBy.PushTokenKey,
        //                    //            RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
        //                    //            Title = Constants.LiftingRequestCreationSubject,
        //                    //            Message = smsPlainTemplate,
        //                    //            //Id = liftingRequestContext.Id,
        //                    //        };
        //                    //        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                    //    }
        //                    //    if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
        //                    //    {
        //                    //        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                    //        {
        //                    //            PushTokenKey = dealer.PushTokenKey,
        //                    //            RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
        //                    //            Title = Constants.LiftingRequestCreationSubject,
        //                    //            Message = smsPlainTemplate,
        //                    //            //Id = liftingRequestContext.Id,
        //                    //        };
        //                    //        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                    //    }
        //                    //}
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //            }
        //        }
        //        if (!errorFlag)
        //        {
        //            return _resultService.SuccessMessage(Constants.IndentRequestSuccess);
        //        }
        //        else
        //        {
        //            return _resultService.ErrorMessage(errorMessage);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        resultDto.IsSuccess = false;
        //        resultDto.ErrorDto.ErrorCode = Constants.Exception;
        //        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
        //        _logger.Error(message);
        //        return resultDto;
        //    }

        //}

        ///// <summary>
        ///// Method to Get Confirmed LiftingRequest List
        ///// </summary>
        ///// <param name="inputDto"></param>
        ///// <returns></returns>
        //public ResultDto GetConfirmedLiftingRequestLists(LoginUserIdDto loginUserIdDto)
        //{
        //    _methodName = "GetConfirmedLiftingRequestLists";
        //    var resultDto = new ResultDto();
        //    var outputDto = new List<LiftingRequestCountDto>();
        //    try
        //    {
        //        if (loginUserIdDto == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        var LiftingRequestList = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => _.CreatedBy == loginUserIdDto.LoginUserId && _.StatusId == (int)DTO.Enums.Status.Approved).AsQueryable();
        //        if (!LiftingRequestList.Any())
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        var LiftingList = LiftingRequestList.GroupBy(x => new { x.UserId }).Select(x => new
        //        {
        //            x.Key.UserId,
        //            DealerId = x.Key.UserId,
        //            Dealer = _emamiContext.Users.FirstOrDefault(_ => _.Id == x.Key.UserId).Name,
        //            TotalLiftingCount = x.Count(),
        //        }).OrderBy(_ => _.UserId).ToList();

        //        foreach (var liftingRequest in LiftingList.ToList())
        //        {
        //            var liftingRequestDto = new LiftingRequestCountDto
        //            {
        //                Dealer = liftingRequest.Dealer,
        //                DealerId = liftingRequest.DealerId,
        //                TotalLiftingCount = liftingRequest.TotalLiftingCount
        //            };

        //            outputDto.Add(liftingRequestDto);
        //        }

        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = outputDto;
        //        return resultDto;
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        resultDto.IsSuccess = false;
        //        resultDto.ErrorDto.ErrorCode = Constants.Exception;
        //        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
        //        _logger.Error(message);
        //        return resultDto;
        //    }
        //}

        ///// <summary>
        ///// Method to Get Inprogress LiftingRequest List
        ///// </summary>
        ///// <param name="inputDto"></param>
        ///// <returns></returns>
        //public ResultDto GetInProgressLiftingRequestLists(LoginUserIdDto loginUserIdDto)
        //{
        //    _methodName = "GetInProgressLiftingRequestLists";
        //    var resultDto = new ResultDto();
        //    var outputDto = new List<LiftingRequestCountDto>();
        //    try
        //    {
        //        if (loginUserIdDto == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }E:\EmamiProject\Adani.Solution\Adani.Solution.API\Controllers\UserController.cs

        //        var LiftingRequestList = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => _.CreatedBy == loginUserIdDto.LoginUserId && _.StatusId == (int)DTO.Enums.Status.Pending).AsQueryable();
        //        if (!LiftingRequestList.Any())
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        var LiftingList = LiftingRequestList.GroupBy(x => new { x.UserId }).Select(x => new
        //        {
        //            x.Key.UserId,
        //            DealerId = x.Key.UserId,
        //            Dealer = _emamiContext.Users.FirstOrDefault(_ => _.Id == x.Key.UserId).Name,
        //            TotalLiftingCount = x.Count(),
        //        }).OrderBy(_ => _.UserId).ToList();

        //        foreach (var liftingRequest in LiftingList.ToList())
        //        {
        //            var liftingRequestDto = new LiftingRequestCountDto
        //            {
        //                Dealer = liftingRequest.Dealer,
        //                DealerId = liftingRequest.DealerId,
        //                TotalLiftingCount = liftingRequest.TotalLiftingCount
        //            };

        //            outputDto.Add(liftingRequestDto);
        //        }

        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = outputDto;
        //        return resultDto;
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        resultDto.IsSuccess = false;
        //        resultDto.ErrorDto.ErrorCode = Constants.Exception;
        //        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
        //        _logger.Error(message);
        //        return resultDto;
        //    }
        //}

        /// <summary>
        /// Method to Get LiftingRequest List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetLiftingRequestCountList(LiftingRequestListInputDto liftingRequestListInputDto)
        {
            _methodName = "GetLiftingRequestCountList";
            var resultDto = new ResultDto();
            var outputDto = new List<LiftingRequestCountDto>();
            try
            {
                var StatusList = new List<long>();
                if (liftingRequestListInputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                var LiftingRequestList = new List<LiftingRequest>();
                var dealerlist = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                  join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                  where ucm.UserId == liftingRequestListInputDto.LoginUserId
                                  select ucm.CustomerId).ToList();
                
                if (dealerlist != null)
                {
                    IEnumerable<LiftingRequest> saudaListDto = new List<LiftingRequest>();
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {

                        var sqlQuery = @"CREATE TABLE #DealerTemp(DealerId BIGINT)

Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

insert into #DealerTemp select CustomerId from UserCustomerMappings where UserId=@UserId

select 
l.*
from LiftingRequests l with(NOLOCK)
left join LiftingRequestDetails ld with(NOLOCK) on l.Id=ld.LiftingRequestId
join #UserDivision ud on ud.SalesOrganizationId=ld.SalesOrganizationId
and ud.DistributionChannelId=ld.DistributionhannelId and ud.DivisionId=ld.DivisionId
where 
l.UserId in (select DealerId from #DealerTemp)
and ((@StatusId=2 and l.SAPDocumentNo is not null) or l.SAPDocumentNo is null )
drop table #DealerTemp
drop table #UserDivision ";
                        saudaListDto = conn.Query<LiftingRequest>(sqlQuery, new
                        {
                            UserId = liftingRequestListInputDto.LoginUserId,
                            StatusId= liftingRequestListInputDto.StatusId
                        }).ToList();

                    }
                    //if (liftingRequestListInputDto.StatusId == (int)DTO.Enums.Status.Approved)
                    //{
                    //    StatusList.Add(liftingRequestListInputDto.StatusId);
                    //    StatusList.Add((int)DTO.Enums.Status.Completed);
                    //    LiftingRequestList = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => dealerlist.Contains(_.UserId) /*&& StatusList.Contains(_.StatusId)*/ && !String.IsNullOrEmpty(_.SAPDocumentNo)).ToList();
                    //}
                    //else
                    //{
                    //    StatusList.Add(liftingRequestListInputDto.StatusId);
                    //    LiftingRequestList = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => dealerlist.Contains(_.UserId) /*&& StatusList.Contains(_.StatusId)*/ && String.IsNullOrEmpty(_.SAPDocumentNo)).ToList();
                    //}

                    if (!saudaListDto.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                        return resultDto;
                    }

                    var liftingRequestId = saudaListDto.Select(x => x.Id);
                    //var UomData = _emamiContext.LiftingRequestDetails.AsNoTracking()
                    //    .Where(_ => liftingRequestId.Contains(_.LiftingRequestId))
                    //    .Select(s => new { Id = s.Id, SaudaNumber = s.SaudaNumber,s.LiftingRequestId }).Distinct();

                  //  var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == liftingRequestListInputDto.LoginUserId)
                  //.Select(_ => new DivisionDetailsDto
                  //{
                  //    SalesOrganizationId = _.SalesOrganizationId,
                  //    DistributionChannelId = _.DistributionChannelId,
                  //    DivisionId = _.DivisionId
                  //});

                    //var saudaListDto = (from l in LiftingRequestList
                    //                join ld in _emamiContext.LiftingRequestDetails.AsNoTracking() on l.Id equals ld.LiftingRequestId
                    //                join s in _emamiContext.Sauda.AsNoTracking() on ld.SaudaNumber equals s.SaudaNumber
                    //                join dm in divisionslogieduser on new { SalesOrganizationId = ld.SalesOrganizationId, DistributionChannelId = ld.DistributionhannelId, DivisionId = ld.DivisionId }
                    //                equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }                                   
                    //                where  dealerlist.Contains(l.UserId) 
                    //                select l).ToList();


                    var LiftingList = saudaListDto.GroupBy(x => x.UserId).Select(s => new { key = s.Key, group = s.ToList()}).ToList();
                    var usercontext = _emamiContext.Users.AsNoTracking();
                    outputDto = LiftingList.Select(x => new LiftingRequestCountDto
                    {
                        DealerId = x.key,
                        Dealer = usercontext.FirstOrDefault(_ => _.Id == x.key).Name,
                        TotalLiftingCount = x.group.GroupBy(s => s.Id).Count(),
                        IsCreatedBy = x.group.FirstOrDefault().CreatedBy != liftingRequestListInputDto.LoginUserId && x.group.FirstOrDefault().StatusId == (int)DTO.Enums.Status.Pending ? false : true
                    }).OrderBy(o => o.Dealer).ToList();

                    //foreach (var liftingRequest in LiftingListdata.ToList())
                    //{
                    //    var liftingRequestDto = new LiftingRequestCountDto
                    //    {
                    //        Dealer = liftingRequest.Dealer,
                    //        DealerId = liftingRequest.DealerId,
                    //        TotalLiftingCount = liftingRequest.TotalLiftingCount,
                    //    };

                    //    outputDto.Add(liftingRequestDto);
                    //}
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }
        /// <summary>
        /// Method to Get Dealers LiftingRequest List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetDealersLiftingRequestList(DealersLiftingRequestInputDto dealersLiftingRequestInputDto)
        {
            _methodName = "GetDealersLiftingRequestList";
            var resultDto = new ResultDto();
            var outputDto = new List<DealersLiftingRequestOutputDto>();
            try
            {
                var StatusList = new List<long>();
                if (dealersLiftingRequestInputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                var LiftingRequestList = new List<LiftingRequest>();
                if (dealersLiftingRequestInputDto.StatusId == (int)DTO.Enums.Status.Approved)
                {
                    StatusList.Add(dealersLiftingRequestInputDto.StatusId);
                    StatusList.Add((int)DTO.Enums.Status.Completed);
                    LiftingRequestList = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => _.UserId == dealersLiftingRequestInputDto.DealerId /*&& StatusList.Contains(_.StatusId)*/ && (DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(dealersLiftingRequestInputDto.FromDate) && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(dealersLiftingRequestInputDto.ToDate)) && !string.IsNullOrEmpty(_.SAPDocumentNo)).ToList();
                }
                else
                {
                    StatusList.Add(dealersLiftingRequestInputDto.StatusId);
                    LiftingRequestList = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => _.UserId == dealersLiftingRequestInputDto.DealerId /*&& StatusList.Contains(_.StatusId)*/ && (DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(dealersLiftingRequestInputDto.FromDate) && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(dealersLiftingRequestInputDto.ToDate)) && string.IsNullOrEmpty(_.SAPDocumentNo)).ToList();
                }
               
                if (!LiftingRequestList.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }

                foreach (var liftingRequest in LiftingRequestList)
                {
                    var LiftingRequestdetail = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.LiftingRequestId == liftingRequest.Id &&
                    _.StatusId != (int)DTO.Enums.Status.Rejected && _.StatusId != (int)DTO.Enums.Status.Deleted).ToList();
                    var liftingRequestDto = new DealersLiftingRequestOutputDto
                    {
                        DealerId = liftingRequest.UserId,
                        Dealer = _emamiContext.Users.FirstOrDefault(_ => _.Id == liftingRequest.UserId).Name,
                        ShipToPartyId = liftingRequest.ShipToPartyId,
                        ShipToParty = liftingRequest.ShipToParty?.Name,
                        LiftingRequestId = liftingRequest.Id,
                        LiftingRequestNumber = liftingRequest.LiftingRequestNumber,
                        LiftingRequestdate = liftingRequest.LiftingDate != null ? liftingRequest.LiftingDate.Date : liftingRequest.LiftingDate,
                        RequestedQuantity = LiftingRequestdetail.Sum(_ => _.LiftingQuantityCase)
                    };
                    if (LiftingRequestdetail != null && LiftingRequestdetail.Any())
                        outputDto.Add(liftingRequestDto);
                }
                outputDto = outputDto.OrderByDescending(_ => _.LiftingRequestId).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to Get Lifting Request Detail
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetLiftingRequestDetail(IdInputDto idInputDto)
        {
            _methodName = "GetLiftingRequestDetail";
            var resultDto = new ResultDto();
            var outputDto = new LiftingRequestDto();
            try
            {
                if (idInputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var LiftingRequestList = _emamiContext.LiftingRequest.FirstOrDefault(_ => _.Id == idInputDto.Id);

                if (LiftingRequestList == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                var LiftingRequestdetail = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.LiftingRequestId == LiftingRequestList.Id
                && _.StatusId != (int)DTO.Enums.Status.Rejected && _.StatusId != (int)DTO.Enums.Status.Deleted).ToList();

                var liftingRequestDto = new LiftingRequestDto
                {
                    LiftingId = LiftingRequestList.Id,
                    LiftingNumber = LiftingRequestList.LiftingRequestNumber,
                    DealerId = LiftingRequestList.UserId,
                    Dealer = _emamiContext.Users.FirstOrDefault(_ => _.Id == LiftingRequestList.UserId).Name,
                    LiftingDate = LiftingRequestList.LiftingDate,
                    TotalQuantity = LiftingRequestdetail.Sum(_ => _.LiftingQuantityCase),
                    CreatedUser = _emamiContext.Users.FirstOrDefault(_ => _.Id == LiftingRequestList.CreatedBy).Name,
                    ShipToPartyId = LiftingRequestList.ShipToPartyId,
                    ShipToParty = LiftingRequestList.ShipToParty?.Name,
                    StatusId = LiftingRequestList.StatusId,
                    Status = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == LiftingRequestList.StatusId)?.Name,
                    Remarks = string.IsNullOrEmpty(LiftingRequestList.ApproverRemarks) ? string.Empty : LiftingRequestList.ApproverRemarks,
                    CustomerRemarks = string.IsNullOrEmpty(LiftingRequestList.CustomerRemarks) ? string.Empty : LiftingRequestList.CustomerRemarks,
                  //  VehicleSize = LiftingRequestList.VehicleSizeId > 0 ? (VehicleLoadabilityContext.FirstOrDefault(_ => _.Id == LiftingRequestList.VehicleSizeId)) != null
                    //? VehicleLoadabilityContext.FirstOrDefault(_ => _.Id == LiftingRequestList.VehicleSizeId).VehicleSize : 0 : 0,
                    PlantName = LiftingRequestList.PlantId > 0 ? _emamiContext.Depots.FirstOrDefault(_ => _.Id == LiftingRequestList.PlantId).Name : string.Empty,
                  //  DepotName = LiftingRequestList.DepotId > 0 ? _emamiContext.Depots.FirstOrDefault(_ => _.Id == LiftingRequestList.DepotId).Name : string.Empty,
                    EnquiryNumber = LiftingRequestList.SAPDocumentNo != null ? LiftingRequestList.SAPDocumentNo : string.Empty,
                    EnquiryRemarks = LiftingRequestdetail.IsAny() ? LiftingRequestdetail.FirstOrDefault().EnquiryRemarks : string.Empty,
                };

                var pendingcontract = _emamiContext.PendingContracts.AsNoTracking();
                foreach (var detail in LiftingRequestdetail.ToList())
                {
                    decimal bidPrice = 0;
                    //if (saudaContext.IsAny())
                    //{
                    //    var saudaId = saudaContext.FirstOrDefault(_ => _.SaudaNumber == detail.SaudaNumber).Id;
                    //    bidPricePerCase = (saudaOrderContext.FirstOrDefault(so => so.SaudaId == saudaId && so.SkuId == detail.SkuId).BidPrice / saudaOrderContext.FirstOrDefault(so => so.SaudaId == saudaId && so.SkuId == detail.SkuId).BidQuantityCase);

                    //}
                    if (pendingcontract.IsAny())
                    {
                        bidPrice = pendingcontract.FirstOrDefault(_ => _.SaudaNumber == detail.SaudaNumber && _.MaterialCode == detail.Sku.SkuCode) !=null ? pendingcontract.FirstOrDefault(_ => _.SaudaNumber == detail.SaudaNumber && _.MaterialCode == detail.Sku.SkuCode).BasicRate : 0;
                    }

                    var liftingDetailDto = new LiftingRequestDetailDto
                    {
                        Id = detail.Id,
                        SkuName = _emamiContext.Skus.FirstOrDefault(_ => _.Id == detail.SkuId).SkuName,
                        OilType = _emamiContext.OilTypes.FirstOrDefault(_ => _.Id == detail.OilTypeId).Name,
                        LiftingQuantity = detail.LiftingQuantityCase,
                        LiftingQuantityInMT = detail.LiftingQuantity,
                        FinalRate = detail.LiftingQuantityCase * bidPrice,
                    };
                    liftingRequestDto.LiftingRequestDetailList.Add(liftingDetailDto);
                }

                outputDto = liftingRequestDto;
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to Get Lifting Request List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetLiftingRequestList(DealersLiftingRequestInputDto dealersLiftingRequestInputDto)
        {
            _methodName = "GetLiftingRequestList";
            var resultDto = new ResultDto();
            var outputDto = new List<DealersLiftingRequestOutputDto>();
            try
            {
                if (dealersLiftingRequestInputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var liftingRequestListQueryContext = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(dealersLiftingRequestInputDto.FromDate) && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(dealersLiftingRequestInputDto.ToDate)).AsNoTracking().AsQueryable();
                if (dealersLiftingRequestInputDto.StatusId > 0)
                {
                    liftingRequestListQueryContext = liftingRequestListQueryContext.Where(_ => _.StatusId == dealersLiftingRequestInputDto.StatusId);
                }

                foreach (var liftingRequest in liftingRequestListQueryContext.ToList())
                {
                    var LiftingRequestdetail = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.LiftingRequestId == liftingRequest.Id &&
                     _.StatusId != (int)DTO.Enums.Status.Rejected && _.StatusId != (int)DTO.Enums.Status.Deleted).ToList();
                    var liftingRequestDto = new DealersLiftingRequestOutputDto
                    {
                        DealerId = liftingRequest.UserId,
                        Dealer = _emamiContext.Users.FirstOrDefault(_ => _.Id == liftingRequest.UserId).Name,
                        LiftingRequestId = liftingRequest.Id,
                        LiftingRequestNumber = liftingRequest.Id.ToString(),
                        LiftingRequestdate = liftingRequest.LiftingDate,
                        RequestedQuantity = LiftingRequestdetail.Sum(_ => _.LiftingQuantityCase),
                        CreatedUser = _emamiContext.Users.FirstOrDefault(_ => _.Id == liftingRequest.CreatedBy).Name,
                        StatusID = liftingRequest.StatusId,
                        Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == liftingRequest.StatusId).Name,
                        Remarks = liftingRequest.ApproverRemarks,
                        ShipToPartyId = liftingRequest.ShipToPartyId,
                        ShipToParty = liftingRequest.ShipToParty?.Name,
                    };
                    if (LiftingRequestdetail.Count > 0)
                    {
                        liftingRequestDto.HasChildren = true;
                    }

                    if (LiftingRequestdetail != null && LiftingRequestdetail.Any())
                        outputDto.Add(liftingRequestDto);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto LiftingRequestStatusChange(LiftingRequestStatusChangeDto liftingRequestStatusChangeDto)
        {
            _methodName = "LiftingRequestStatusChange";
            var resultDto = new ResultDto();
            try
            {
                if (liftingRequestStatusChangeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (liftingRequestStatusChangeDto.LoginUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == liftingRequestStatusChangeDto.LoginUserId);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var liftingContext = _emamiContext.LiftingRequest.FirstOrDefault(_ => _.Id == liftingRequestStatusChangeDto.Id);
                if (liftingContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                liftingContext.StatusId = liftingRequestStatusChangeDto.StatusId;
                liftingContext.ApproverRemarks = liftingRequestStatusChangeDto.Remarks;
                liftingContext.ModifiedBy = liftingRequestStatusChangeDto.LoginUserId;
                liftingContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();

                try
                {
                    List<User> usersContext = new List<User>();
                    List<string> toUsers = new List<string>();
                    User createdBy = new User();
                    User dealer = new User();
                    if (liftingContext.CreatedBy == liftingContext.UserId)
                    {
                        createdBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == liftingContext.CreatedBy);
                        if (createdBy != null)
                        {
                            toUsers.Add(createdBy.Email);
                        }
                    }
                    else
                    {
                        usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == liftingContext.CreatedBy || _.Id == liftingContext.UserId).ToList();
                        if (usersContext != null && usersContext.Any())
                        {
                            createdBy = usersContext.FirstOrDefault(_ => _.Id == liftingContext.CreatedBy);
                            dealer = usersContext.FirstOrDefault(_ => _.Id == liftingContext.UserId);
                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                            {
                                toUsers.Add(createdBy.Email);
                            }
                            if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                            {
                                toUsers.Add(dealer.Email);
                            }
                        }
                    }
                    if ((usersContext != null && usersContext.Any()) || createdBy != null)
                    {
                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                        var emailSubject = string.Empty;
                        if (_resultService.IsEmail() && toUsers != null && toUsers.Any())
                        {
                            var fromEmail = Constants.FromEmail;
                            var plainText = string.Empty;
                            EmailTemplate emailTemplate = new EmailTemplate();
                            if (liftingRequestStatusChangeDto.StatusId == (int)DTO.Enums.Status.Approved)
                            {
                                emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.LiftingRequestApprovalEmail);
                                emailSubject = Constants.LiftingRequestApprovalSubject;
                            }
                            if (emailTemplate != null)
                            {
                                var plainTemplate = emailTemplate.PlainTemplate;
                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                            }

                        }
                        var smsPlainTemplate = string.Empty;
                        if (_resultService.IsSMS())
                        {
                            var smsMessage = string.Empty;
                            EmailTemplate smsTemplate = new EmailTemplate();
                            if (liftingRequestStatusChangeDto.StatusId == (int)DTO.Enums.Status.Approved)
                            {
                                smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.LiftingRequestApprovalSMS);
                            }
                            if (smsTemplate != null)
                            {
                                smsPlainTemplate = smsTemplate.PlainTemplate;
                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                {
                                    amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber, smsTemplate.SMSTemplateID);
                                }
                                if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                {

                                    amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber, smsTemplate.SMSTemplateID);
                                }
                            }
                        }
                        if (_resultService.IsPushNotification())
                        {
                            if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                            {
                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                {
                                    PushTokenKey = createdBy.PushTokenKey,
                                    RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                                    Title = emailSubject,
                                    Message = smsPlainTemplate,
                                    //Id = liftingContext.Id,
                                };
                                _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                            }
                            if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
                            {
                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                {
                                    PushTokenKey = dealer.PushTokenKey,
                                    RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                                    Title = emailSubject,
                                    Message = smsPlainTemplate,
                                    //Id = liftingContext.Id,
                                };
                                _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = liftingContext.Id;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto LiftingRequestStatusChanges(LiftingRequestStatusChangeDto liftingRequestStatusChangeDto)
        {
            _methodName = "LiftingRequestStatusChanges";
            var resultDto = new ResultDto();
            try
            {
                if (liftingRequestStatusChangeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (liftingRequestStatusChangeDto.LoginUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.UserIdMissing;
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == liftingRequestStatusChangeDto.LoginUserId);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }
                try
                {
                    List<User> usersContext = new List<User>();
                    List<string> toUsers = new List<string>();
                    User createdBy = new User();
                    User dealer = new User();

                    if (liftingRequestStatusChangeDto.LiftingIds != null && liftingRequestStatusChangeDto.LiftingIds.Any())
                    {
                        foreach (var liftingId in liftingRequestStatusChangeDto.LiftingIds)
                        {
                            var liftingContext = _emamiContext.LiftingRequest.FirstOrDefault(_ => _.Id == liftingId);
                            if (liftingContext == null)
                            {
                                return _resultService.ErrorMessage(Constants.RecordNotFound);
                            }
                            liftingContext.StatusId = liftingRequestStatusChangeDto.StatusId;
                            liftingContext.ApproverRemarks = liftingRequestStatusChangeDto.Remarks;
                            liftingContext.ModifiedBy = liftingRequestStatusChangeDto.LoginUserId;
                            liftingContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                            //If status is Rejected then update the LiftingRequestDetails table
                            if (liftingRequestStatusChangeDto.StatusId == (int)DTO.Enums.Status.Rejected)
                            {
                                var liftingRequestDetails = _emamiContext.LiftingRequestDetails.Where(_ => _.LiftingRequestId == liftingId);
                                if (liftingRequestDetails != null && liftingRequestDetails.Any())
                                {
                                    foreach (var liftingRequestDetail in liftingRequestDetails)
                                    {
                                        liftingRequestDetail.StatusId = liftingRequestStatusChangeDto.StatusId;
                                        liftingRequestDetail.ModifiedBy = liftingRequestStatusChangeDto.LoginUserId;
                                        liftingRequestDetail.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    }
                                }
                            }
                            _emamiContext.SaveChanges();

                            try
                            {
                                if (liftingContext.CreatedBy == liftingContext.UserId)
                                {
                                    createdBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == liftingContext.CreatedBy);
                                    if (createdBy != null)
                                    {
                                        toUsers.Add(createdBy.Email);
                                    }
                                }
                                else
                                {
                                    usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == liftingContext.CreatedBy || _.Id == liftingContext.UserId).ToList();
                                    if (usersContext != null && usersContext.Any())
                                    {
                                        createdBy = usersContext.FirstOrDefault(_ => _.Id == liftingContext.CreatedBy);
                                        dealer = usersContext.FirstOrDefault(_ => _.Id == liftingContext.UserId);
                                        if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                                        {
                                            toUsers.Add(createdBy.Email);
                                        }
                                        if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                                        {
                                            toUsers.Add(dealer.Email);
                                        }
                                    }
                                }
                                if ((usersContext != null && usersContext.Any()) || createdBy != null)
                                {
                                    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                    var emailSubject = string.Empty;
                                    if (_resultService.IsEmail() && toUsers != null && toUsers.Any())
                                    {
                                        var fromEmail = Constants.FromEmail;
                                        var plainText = string.Empty;
                                        EmailTemplate emailTemplate = new EmailTemplate();
                                        if (liftingRequestStatusChangeDto.StatusId == (int)DTO.Enums.Status.Approved)
                                        {
                                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.LiftingRequestApprovalEmail);
                                            emailSubject = Constants.LiftingRequestApprovalSubject;
                                        }
                                        if (emailTemplate != null)
                                        {
                                            var plainTemplate = emailTemplate.PlainTemplate;
                                            var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                            amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                        }
                                    }
                                    var smsPlainTemplate = string.Empty;
                                    if (_resultService.IsSMS())
                                    {
                                        var smsMessage = string.Empty;
                                        EmailTemplate smsTemplate = new EmailTemplate();
                                        if (liftingRequestStatusChangeDto.StatusId == (int)DTO.Enums.Status.Approved)
                                        {
                                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.LiftingRequestApprovalSMS);
                                        }
                                        if (smsTemplate != null)
                                        {
                                            smsPlainTemplate = smsTemplate.PlainTemplate;
                                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                            {
                                                amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber, smsTemplate.SMSTemplateID);
                                            }
                                            if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                            {
                                                amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber, smsTemplate.SMSTemplateID);
                                            }
                                        }
                                    }
                                    if (_resultService.IsPushNotification())
                                    {
                                        if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                                        {
                                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                            {
                                                PushTokenKey = createdBy.PushTokenKey,
                                                RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                                                Title = emailSubject,
                                                Message = smsPlainTemplate
                                            };
                                            _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                        }
                                        if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
                                        {
                                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                            {
                                                PushTokenKey = dealer.PushTokenKey,
                                                RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                                                Title = emailSubject,
                                                Message = smsPlainTemplate
                                            };
                                            _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.RecordNotFound);
                    }
                }
                catch (Exception ex)
                {
                }
                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto LiftingRequestApproveForAdmin(LiftingRequestStatusChangeDto liftingRequestStatusChangeDto)
        {
            _methodName = "LiftingRequestApproveForAdmin";
            var resultDto = new ResultDto();
            try
            {
                if (liftingRequestStatusChangeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (liftingRequestStatusChangeDto.LoginUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.UserIdMissing;
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == liftingRequestStatusChangeDto.LoginUserId);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }
                try
                {
                    List<User> usersContext = new List<User>();
                    List<string> toUsers = new List<string>();
                    User createdBy = new User();
                    User dealer = new User();

                    if (liftingRequestStatusChangeDto.LiftingIds != null && liftingRequestStatusChangeDto.LiftingIds.Any())
                    {
                        foreach (var liftingId in liftingRequestStatusChangeDto.LiftingIds)
                        {
                            var liftingContext = _emamiContext.LiftingRequest.FirstOrDefault(_ => _.Id == liftingId);
                            if (liftingContext == null)
                            {
                                return _resultService.ErrorMessage(Constants.RecordNotFound);
                            }
                            liftingContext.StatusId = (int)Adani.Solution.DTO.Enums.Status.Approved;
                            liftingContext.ApproverRemarks = liftingRequestStatusChangeDto.Remarks;
                            liftingContext.ApprovedBy = liftingRequestStatusChangeDto.LoginUserId;
                            liftingContext.ModifiedBy = liftingRequestStatusChangeDto.LoginUserId;
                            liftingContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            liftingContext.IsSAPDataSync = false;
                            ////If status is Rejected then update the LiftingRequestDetails table
                            //if (liftingRequestStatusChangeDto.StatusId == (int)DTO.Enums.Status.Rejected)
                            //{
                            //    var liftingRequestDetails = _emamiContext.LiftingRequestDetails.Where(_ => _.LiftingRequestId == liftingId);
                            //    if (liftingRequestDetails != null && liftingRequestDetails.Any())
                            //    {
                            //        foreach (var liftingRequestDetail in liftingRequestDetails)
                            //        {
                            //            liftingRequestDetail.StatusId = liftingRequestStatusChangeDto.StatusId;
                            //            liftingRequestDetail.ModifiedBy = liftingRequestStatusChangeDto.LoginUserId;
                            //            liftingRequestDetail.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            //        }
                            //    }
                            //}
                            _emamiContext.SaveChanges();
                        }
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.RecordNotFound);
                    }
                    //method to sync Liftng from APP to SAP 
                    bool IsReprocess = false;
                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                    {
                        _sapIntegrationService.GetLiftingRequestEnquiryNumberOutboundDetails(liftingRequestStatusChangeDto.LiftingIds, IsReprocess);
                    });

                }
                catch (Exception ex)
                {
                }
                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        #region Lifting Request - Web

        public ResultDto GetLiftingRequestListForWeb(DealersLiftingRequestInputDto dealersLiftingRequestInputDto)
        {
            _methodName = "GetLiftingRequestListForWeb";
            var resultDto = new ResultDto();
            var outputDto = new List<LiftingRequestOutputDto>();
            try
            {
                if (dealersLiftingRequestInputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (dealersLiftingRequestInputDto.StateIds == null || !dealersLiftingRequestInputDto.StateIds.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.StateNameIsEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.StateNameIsEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }

                var liftingRequestListQueryContext = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(dealersLiftingRequestInputDto.FromDate) && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(dealersLiftingRequestInputDto.ToDate)).AsNoTracking().AsQueryable();
                if (dealersLiftingRequestInputDto.StateIds != null && dealersLiftingRequestInputDto.StateIds.Any())
                {
                    liftingRequestListQueryContext = liftingRequestListQueryContext
                                                        .Join(_emamiContext.Users.AsNoTracking().Where(_ => dealersLiftingRequestInputDto.StateIds.Contains(_.StateId)), lfq => lfq.UserId, u => u.Id, (lfq, u) => new { lfq }).Select(_ => _.lfq);
                }

                List<long> statusIds = new List<long>();
                if (dealersLiftingRequestInputDto.StatusId > 0)
                {
                    statusIds.Add(dealersLiftingRequestInputDto.StatusId);
                }
                else
                {
                    statusIds = new List<long>() { (long)DTO.Enums.Status.Approved, (long)DTO.Enums.Status.Pending, (long)DTO.Enums.Status.Rejected };
                }

                liftingRequestListQueryContext = liftingRequestListQueryContext.Where(_ => statusIds.Contains(_.StatusId));

                foreach (var liftingRequest in liftingRequestListQueryContext.ToList())
                {
                    var LiftingRequestdetail = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.LiftingRequestId == liftingRequest.Id).ToList();
                    var liftingRequestDto = new LiftingRequestOutputDto
                    {
                        DealerId = liftingRequest.UserId,
                        Dealer = _emamiContext.Users.FirstOrDefault(_ => _.Id == liftingRequest.UserId).Name,
                        ShipToPartyId = liftingRequest.ShipToPartyId,
                        ShipToParty = liftingRequest.ShipToParty?.Name,
                        LiftingRequestId = liftingRequest.Id,
                        LiftingRequestNumber = liftingRequest.Id.ToString(),
                        LiftingRequestdate = liftingRequest.LiftingDate,
                        RequestedQuantity = LiftingRequestdetail.Sum(_ => _.LiftingQuantity),
                        RequestedQuantityInCase = LiftingRequestdetail.Sum(_ => _.LiftingQuantityCase),
                        CreatedUser = _emamiContext.Users.FirstOrDefault(_ => _.Id == liftingRequest.CreatedBy).Name,
                        StatusID = liftingRequest.StatusId,
                        Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == liftingRequest.StatusId).Name,
                        Remarks = liftingRequest.ApproverRemarks,
                        CustomerRemarks = liftingRequest.CustomerRemarks
                    };
                    if (LiftingRequestdetail.Count > 0)
                    {
                        liftingRequestDto.HasChildren = true;
                    }
                    outputDto.Add(liftingRequestDto);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.LiftingRequestId).ToList() : outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetLiftingRequestWithoutEnquiryNumberListForWeb(DealersLiftingRequestInputDto inputDto)
        {
            _methodName = "GetLiftingRequestWithoutEnquiryNumberListForWeb";
            var resultDto = new ResultDto();
            var outputDto = new List<LiftingRequestOutputDto>();
            try
            {

                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }


                //if (inputDto.StateIds == null || !inputDto.StateIds.Any())
                //{
                //    resultDto.IsSuccess = false;
                //    resultDto.ErrorDto.ErrorCode = Constants.StateNameIsEmpty;
                //    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.StateNameIsEmpty, Constants.EnglishLanguage);
                //    return resultDto;
                //}
                var userList = _emamiContext.Users.AsNoTracking();
                var roleId = _emamiContext.UserRoles.Where(_ => _.UserId == inputDto.LoginUserId).FirstOrDefault().RoleId;
                
                var bdoIds = new List<long>();
                var ZHIds = new List<long>();

                if (roleId == (int)DTO.Enums.Role.NationalTrader)
                {
                    ZHIds = userList.Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(s => s.Id).ToList();
                    bdoIds = userList.Where(_ => ZHIds.Contains((long)_.ReportingToId)).Select(s => s.Id).ToList();
                }
                if (roleId == (int)DTO.Enums.Role.ZonalTrader)
                {
                    ZHIds.Add(inputDto.LoginUserId);
                    bdoIds = userList.Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(s => s.Id).ToList();
                }
                if (roleId == (int)DTO.Enums.Role.StateTrader)
                {
                    bdoIds.Add(inputDto.LoginUserId);
                }
                IQueryable<LiftingRequest> liftingRequestListQueryContext = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)).AsNoTracking().AsQueryable();
                var userContext = _emamiContext.Users.AsNoTracking();
                var userDivContext = _emamiContext.UserDivisionMappings.AsNoTracking();

                if (inputDto.SalesOrganizationId > 0 && inputDto.VerticalId > 0 && inputDto.DistributionChannelId > 0)
                {
                    userContext = userDivContext.Where(_ => _.SalesOrganizationId == inputDto.SalesOrganizationId
                                                         && _.DistributionChannelId == inputDto.DistributionChannelId
                                                         && _.DivisionId == inputDto.VerticalId).Select(_ => _.User);
                }
                

                if (inputDto.StateIds != null && inputDto.StateIds.Any())
                {
                    liftingRequestListQueryContext = liftingRequestListQueryContext
                                                        .Join(userContext.Where(_ => inputDto.StateIds.Contains(_.StateId)), lfq => lfq.UserId, u => u.Id, (lfq, u) => new { lfq }).Select(_ => _.lfq);
                }


                List<long> statusIds = new List<long>();
                if (inputDto.StatusId > 0)
                {
                    statusIds.Add(inputDto.StatusId);
                }
                else
                {
                    statusIds = new List<long>() { (long)DTO.Enums.Status.Approved, (long)DTO.Enums.Status.Pending, (long)DTO.Enums.Status.Rejected };
                }
                var dealerIds = new List<long>();
                if (bdoIds.IsAny())
                {
                    dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(s => s.CustomerId).ToList();
                }

                var liftingRequestList = new List<LiftingRequest>();
                if (dealerIds.IsAny())
                {
                    liftingRequestList.AddRange(liftingRequestListQueryContext.Where(_ => statusIds.Contains(_.StatusId) && (dealerIds.Contains(_.UserId))).ToList());
                }
                else
                {
                    liftingRequestList.AddRange(liftingRequestListQueryContext.Where(_ => statusIds.Contains(_.StatusId)).ToList());
                }
                //if (ZHIds.IsAny())
                //{
                //    liftingRequestList.AddRange(liftingRequestListQueryContext.Where(_ => statusIds.Contains(_.StatusId) && (ZHIds.Contains(_.CreatedBy) )).ToList());
                //}
                //if (bdoIds.IsAny())
                //{
                //    liftingRequestList.AddRange(liftingRequestListQueryContext.Where(_ => statusIds.Contains(_.StatusId) && (bdoIds.Contains(_.CreatedBy))).ToList());
                //}
                //if(ZHIds.IsNotAny() && bdoIds.IsNotAny())
                //{
                //    liftingRequestList = liftingRequestListQueryContext.Where(_ => statusIds.Contains(_.StatusId)).ToList();

                //}
                //var liftingData=liftingRequestList.Join
                //var liftdata = new List<LiftingRequestOutputDto>();
                //using (IDbConnection connection = new SqlConnection(Config.DBConnectionString))
                //{
                //    try
                //    {
                //        connection.Open();
                //        string query = @"select  
                //                        lrd.LiftingRequestId,
                //                        Max(d.Name) as PlantName,
                //                        Max(d.Code) as PlantCode,
                //                        Max(lr.UserId) as DealerId,
                //                        Max(dealer.Name) as DealerName,
                //                        Sum(lrd.LiftingQuantity) as RequestedQuantity,
                //                        Sum(lrd.LiftingQuantityCase) as RequestedQuantityCase,
                //                        Max(lrd.EnquiryRemarks) as EnquiryRemarks,
                //                        Min(Cast(lrd.EnquiryNumberSyncFromSap as int)) as EnquiryNumberSyncFromSap,
                //                        lrd.LiftingRequestId as LiftingRequestNumber,
                //                        Max(lr.LiftingDate) as LiftingRequestdate,
                //                        Max(createuser.Name) as CreatedUser,
                //                        Max(status.Name) as Status,
                //                        Max(lr.ApproverRemarks) as Remarks,
                //                        Max(lr.CustomerRemarks) as CustomerRemarks,
                //                        Max(lr.SAPDocumentNo) as EnquiryNumber,
                //                        Max(lr.SAPDeliveryNo) as DeliveryOrderNumber
                //                        from LiftingRequests lr
                //                        join Users dealer on dealer.Id=lr.UserId
                //                        left join Users createuser on createuser.Id=lr.Id
                //                        left join Status status on status.Id=lr.StatusId
                //                        join Depots d on lr.PlantId=d.Id
                //                        right join LiftingRequestDetails lrd on lr.Id=lrd.LiftingRequestId 
                //                        where d.IsPlant=1 
                //                        and Convert(date,lr.CreatedDate) >= Convert(date,@StartDate) and Convert(date,lr.CreatedDate) <= Convert(date,@EndDate)
                //                        group by lrd.LiftingRequestId 
                //                        order by lrd.LiftingRequestId desc";
                //        liftdata = connection.Query<LiftingRequestOutputDto>(query,
                //                    new
                //                    {
                //                        @StartDate=inputDto.FromDate,
                //                        @EndDate=inputDto.ToDate
                //                    }).ToList();

                       
                //    }
                //    catch (Exception exception)
                //    {
                //        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                //        _logger.Error(message);
                //    }
                //    finally
                //    {
                //        connection.Close();
                //    }
                //}

                


                foreach (var liftingRequest in liftingRequestList)
                {
                    var LiftingRequestdetail = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.LiftingRequestId == liftingRequest.Id /*&& string.IsNullOrEmpty(_.EnquiryNumber) && string.IsNullOrEmpty(_.DeliveryOrderNumber)*/).ToList();

                    string remarks = string.Empty;


                    var plantDetails = _emamiContext.Depots.Where(_ => _.Id == liftingRequest.PlantId && _.IsPlant).FirstOrDefault();
                    if (LiftingRequestdetail != null && LiftingRequestdetail.Any())
                    {
                        var liftingRequestDto = new LiftingRequestOutputDto
                        {
                            EncryptedId = UtilityHelper.ConvertToMd5(liftingRequest.Id.ToString(), SecurityConstants.EncryptionKey),
                            PlantName = plantDetails==null ? string.Empty : plantDetails.Name,
                            PlantCode = plantDetails==null ? string.Empty : plantDetails.Code,
                            DealerId = liftingRequest.UserId,
                            Dealer = userList.FirstOrDefault(_ => _.Id == liftingRequest.UserId) != null ? userList.FirstOrDefault(_ => _.Id == liftingRequest.UserId).Name : string.Empty,
                            ShipToPartyId = liftingRequest.ShipToPartyId,
                            ShipToParty = liftingRequest.ShipToParty?.Name,
                            LiftingRequestId = liftingRequest.Id,
                            LiftingRequestNumber = liftingRequest.Id.ToString(),
                            LiftingRequestdate = liftingRequest.LiftingDate,
                            RequestedQuantity = LiftingRequestdetail.Sum(_ => _.LiftingQuantity),
                            RequestedQuantityInCase = LiftingRequestdetail.Sum(_ => _.LiftingQuantityCase),
                            CreatedUser = userList.FirstOrDefault(_ => _.Id == liftingRequest.CreatedBy) != null ? userList.FirstOrDefault(_ => _.Id == liftingRequest.CreatedBy).Name : string.Empty,
                            StatusID = liftingRequest.StatusId,
                            Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == liftingRequest.StatusId) != null ? _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == liftingRequest.StatusId).Name : string.Empty,
                            Remarks = liftingRequest.ApproverRemarks== null ? string.Empty : liftingRequest.ApproverRemarks,
                            CustomerRemarks = liftingRequest.CustomerRemarks == null ? string.Empty : liftingRequest.CustomerRemarks,
                            EnquiryNumber = liftingRequest.SAPDocumentNo == null ? string.Empty : liftingRequest.SAPDocumentNo,
                            DeliveryOrderNumber = liftingRequest.SAPDeliveryNo == null ? string.Empty : liftingRequest.SAPDeliveryNo,
                            EnquiryRemarks = LiftingRequestdetail.FirstOrDefault().EnquiryRemarks,
                            EnquiryNumberSyncFromSap = LiftingRequestdetail.FirstOrDefault().EnquiryNumberSyncFromSap
                        };
                        if (LiftingRequestdetail.Count > 0)
                        {
                            liftingRequestDto.HasChildren = true;
                        }
                        outputDto.Add(liftingRequestDto);
                    }

                }
                var datasourceResult = outputDto != null ? outputDto.OrderByDescending(_ => _.LiftingRequestId).ToDataSourceResult(inputDto.DataSourceRequest): outputDto.ToDataSourceResult(inputDto.DataSourceRequest);
                resultDto.IsSuccess = true;
                //resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.LiftingRequestId).ToList() : outputDto;
                resultDto.SuccessDto.Response = datasourceResult;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetLiftingRequestDetailsForWeb(IdInputDto idInputDto)
        {
            _methodName = "GetLiftingRequestDetailsForWeb";
            var resultDto = new ResultDto();
            var outputDto = new LiftingRequestWebDto();
            try
            {
                if (idInputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var LiftingRequestList = _emamiContext.LiftingRequest.FirstOrDefault(_ => _.Id == idInputDto.Id);

                if (LiftingRequestList == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userDetails = _emamiContext.Users.AsNoTracking().ToList();

                

                var LiftingRequestdetail = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.LiftingRequestId == LiftingRequestList.Id).ToList();
                var plantDetails = _emamiContext.Depots.Where(_ => _.Id == LiftingRequestList.PlantId && _.IsPlant).FirstOrDefault();
                var liftingRequestDto = new LiftingRequestWebDto
                {
                    PlantCode=plantDetails==null ? string.Empty : plantDetails.Code,
                    PlantName=plantDetails==null ? string.Empty : plantDetails.Name,
                    DeliveryOrderNumber= LiftingRequestList.SAPDeliveryNo,
                    EnquiryNumber= LiftingRequestList.SAPDocumentNo,
                    Dealer =userDetails.Where(_ => _.Id== LiftingRequestList.UserId ).FirstOrDefault() == null ? string.Empty : userDetails.Where(_ => _.Id == LiftingRequestList.UserId).FirstOrDefault().Name,
                    CreatedUser=userDetails.Where(_ => _.Id== LiftingRequestList.CreatedBy).FirstOrDefault() == null ? string.Empty : userDetails.Where(_ => _.Id == LiftingRequestList.CreatedBy).FirstOrDefault().Name,
                    LiftingId = LiftingRequestList.Id,
                    LiftingNumber=LiftingRequestList.Id.ToString(),
                    LiftingDate = LiftingRequestList.LiftingDate,
                    TotalQuantity = LiftingRequestdetail.Sum(_ => _.LiftingQuantity),
                    TotalQuantityInCase = LiftingRequestdetail.Sum(_ => _.LiftingQuantityCase),
                };

                liftingRequestDto.InvoiceList = _emamiContext.Invoices.Where(_ => _.LiftingRequestId == idInputDto.Id).Select(s => new InvoiceLiftingRequestDto()
                {
                    Id = s.Id,
                    InvoiceDate = s.CreatedDate,
                    InvoiceNumber = s.BillingDocument,
                    TotalAmount = s.TotalInvoice,
                    UserCode = s.UserCode
                }).ToList();

                liftingRequestDto.IsInvoiceExist = liftingRequestDto.InvoiceList != null ? true : false;
                foreach (var detail in LiftingRequestdetail.ToList())
                {
                    var oiltype = _emamiContext.OilTypes.FirstOrDefault(_ => _.Id == detail.OilTypeId);
                    var liftingDetailDto = new LiftingRequestDetailsOutputDto
                    {
                        Id = detail.Id,
                        SkuName = detail.Sku?.SkuName,
                        SkuCode = detail.Sku?.SkuCode,
                        OilType = oiltype!=null ? oiltype.Name+"-"+oiltype.SalesOrganization.Code+"/"+oiltype.DistributionChannel.Code+"/"+oiltype.Division.Code : String.Empty,
                        LiftingQuantity = detail.LiftingQuantity,
                        LiftingQuantityCase = detail.LiftingQuantityCase,
                        SaudaNumber = detail.SaudaNumber
                        //InvoiceDate = InvoiceDetails != null && InvoiceDetails.InvoiceDate != null ? InvoiceDetails.InvoiceDate : DateTime.MinValue,
                        //InvoiceNumber = InvoiceDetails != null && InvoiceDetails.InvoiceNumber != null ? InvoiceDetails.InvoiceNumber : string.Empty,
                        //TotalAmount = InvoiceDetails != null && InvoiceDetails.TotalAmount > 0 ? InvoiceDetails.TotalAmount : 0,                      
                    };
                    liftingRequestDto.LiftingRequestDetailList.Add(liftingDetailDto);
                }

                outputDto = liftingRequestDto;
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetLiftingRequestListForExport(DealersLiftingRequestInputDto inputDto)
        {
            _methodName = "GetLiftingRequestListForExport";
            var resultDto = new ResultDto();
            var outputDto = new List<LiftingRequestExportDto>();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var liftingRequestListQueryContext = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)).AsNoTracking().AsQueryable();

                var statusId = new List<long>();
                if (inputDto.StatusId == 0)
                {
                    statusId.Add((long)DTO.Enums.Status.Pending);
                    statusId.Add((long)DTO.Enums.Status.Approved);
                }
                else
                {
                    statusId.Add(inputDto.StatusId);
                }

                if (inputDto.StateId > 0)
                {
                    liftingRequestListQueryContext = liftingRequestListQueryContext//.Where(_ => statusId.Contains(_.StatusId))
                        .Join(_emamiContext.Users.AsNoTracking().Where(_ => _.StateId == inputDto.StateId), lfq => lfq.UserId, u => u.Id, (lfq, u) => new { lfq }).Select(_ => _.lfq);
                }
                else
                {
                    liftingRequestListQueryContext = liftingRequestListQueryContext//.Where(_ => statusId.Contains(_.StatusId))
                        .Join(_emamiContext.Users.AsNoTracking(), lfq => lfq.UserId, u => u.Id, (lfq, u) => new { lfq }).Select(_ => _.lfq);
                }

                if (inputDto.StatusId > 0)
                {
                    liftingRequestListQueryContext = liftingRequestListQueryContext.Where(_ => statusId.Contains(_.StatusId))
                        .Join(_emamiContext.Users.AsNoTracking(), lfq => lfq.UserId, u => u.Id, (lfq, u) => new { lfq }).Select(_ => _.lfq);
                }
                else
                {
                    liftingRequestListQueryContext = liftingRequestListQueryContext
                        .Join(_emamiContext.Users.AsNoTracking(), lfq => lfq.UserId, u => u.Id, (lfq, u) => new { lfq }).Select(_ => _.lfq);
                }

                if (liftingRequestListQueryContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                foreach (var liftingRequest in liftingRequestListQueryContext.ToList())
                {
                    var liftingRequestDetail = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.LiftingRequestId == liftingRequest.Id).ToList();

                    foreach (var detail in liftingRequestDetail)
                    {
                        var liftingDetailDto = new LiftingRequestExportDto
                        {
                            LiftingRequestId = detail.Id,
                            LiftingRequestNumber = liftingRequest.LiftingRequestNumber,
                            DealerName = liftingRequest.User?.Name,
                            ShipToPartyId = liftingRequest.ShipToPartyId,
                            ShipToPartyName = liftingRequest.ShipToParty?.Name,
                            ShipToPartyCode = liftingRequest.ShipToParty?.Code,
                            LiftingRequestdate = liftingRequest.LiftingDate,
                            TotalQuantity = liftingRequestDetail.Sum(_ => _.LiftingQuantity),
                            TotalQuantityInCase = liftingRequestDetail.Sum(_ => _.LiftingQuantityCase),
                            SkuName = detail.Sku?.SkuName,
                            SkuCode = detail.Sku?.SkuCode,
                            OilType = detail.OilType != null ? detail.OilType.Name + "-"+detail.OilType.SalesOrganization.Code +"/"+detail.OilType.DistributionChannel.Code+"/"+detail.OilType.Division.Code:string.Empty,
                            LiftingQuantity = detail.LiftingQuantity,
                            LiftingQuantityCase = detail.LiftingQuantityCase,
                            DeliveryOrderNumber = detail.DeliveryOrderNumber,
                            StatusName = detail.StatusId > 0 ? _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == detail.StatusId).Name : _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == liftingRequest.StatusId).Name,
                            ApproverRemarks = liftingRequest.ApproverRemarks,
                            CustomerRemarks = liftingRequest.CustomerRemarks
                        };
                        outputDto.Add(liftingDetailDto);
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion

        #region Sauda Order Lifting Request

        public ResultDto GetSaudaOrderLiftingRequestDetails(IdInputDto idInputDto)
        {
            _methodName = "GetSaudaOrderLiftingRequestDetails";
            var resultDto = new ResultDto();
            try
            {
                if (idInputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                //var LiftingRequestList = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking()
                //    .Join(_emamiContext.SaudaOrders.AsNoTracking(), lr => lr.SaudaOrderId, so => so.Id, (lr, so) => new { SaudaOrderNumber = so.SaudaNumber, SaudaOrderLiftingRequest = lr })
                //    .Where(w => w.SaudaOrderLiftingRequest.LiftingRequestDetailId == idInputDto.Id).ToList()
                //    .Select(s => new SaudaOrderLiftingRequestDto()
                //    {
                //        Id = s.SaudaOrderLiftingRequest.Id,
                //        SaudaNumber = s.SaudaOrderNumber,
                //        DeliveryOrderNumber = s.SaudaOrderLiftingRequest.DeliveryOrderNumber,
                //        LiftingQuantity = s.SaudaOrderLiftingRequest.LiftingQuantity,
                //        LiftingQuantityCase = s.SaudaOrderLiftingRequest.LiftingQuantityCase,
                //        StatusId = s.SaudaOrderLiftingRequest.StatusId,
                //        StatusName = Utility.GetEnumFromString<DTO.Enums.Status>(s.SaudaOrderLiftingRequest.StatusId)
                //    }).ToList();
                //var InvoiceDetails = _emamiContext.Invoices.Where(_ => _.LiftingRequestId == idInputDto.Id).Select(s => new InvoiceLiftingRequestDto()
                //{
                //    Id=s.Id,
                //    InvoiceDate=s.CreatedDate,
                //    InvoiceNumber=s.DeliveryOrderNumber,
                //    TotalAmount=s.TotalInvoice,
                //    UserCode=s.UserCode
                //});
                var skucontext = _emamiContext.Skus.AsNoTracking();
                var InvoiceSkuDetails = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.InvoiceId == idInputDto.Id).Select(s => new LiftingRequestDetailsOutputDto()
                {
                    Id=s.Id,
                    SkuId=s.SkuId,
                    SkuName = skucontext.Where(sku => sku.Id==s.SkuId).FirstOrDefault()!=null ? skucontext.Where(sku => sku.Id == s.SkuId).FirstOrDefault().SkuName : string.Empty ,
                    SkuCode = skucontext.Where(sku => sku.Id == s.SkuId).FirstOrDefault() != null ? skucontext.Where(sku => sku.Id == s.SkuId).FirstOrDefault().SkuCode : string.Empty,
                    LiftingQuantity = s.QuantityInCase,
                    LiftingQuantityCase = s.ActualBilledQuantity
                }).ToList();
                //var SkuDetails = _emamiContext.LiftingRequestDetails.Where(_ => _.LiftingRequestId == idInputDto.Id).Select(s => new LiftingRequestDetailsOutputDto()
                //{
                //    SkuName=s.Sku.SkuName,
                //    SkuCode=s.Sku.SkuCode,
                //    LiftingQuantity=s.LiftingQuantity,
                //    LiftingQuantityCase=s.LiftingQuantityCase
                //}).ToList();
                resultDto = _resultService.SuccessObject(InvoiceSkuDetails);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }


        public ResultDto GetSaudaOrderLiftingRequestExcelExport(DealersLiftingRequestInputDto inputDto)
        {
            _methodName = "GetSaudaOrderLiftingRequestExcelExport";
            var resultDto = new ResultDto();
            var outputDto = new List<LiftingRequestOutputDto>();
            try
            {

                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }


                //if (inputDto.StateIds == null || !inputDto.StateIds.Any())
                //{
                //    resultDto.IsSuccess = false;
                //    resultDto.ErrorDto.ErrorCode = Constants.StateNameIsEmpty;
                //    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.StateNameIsEmpty, Constants.EnglishLanguage);
                //    return resultDto;
                //}

                IQueryable<LiftingRequest> liftingRequestListQueryContext = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)).AsNoTracking().AsQueryable();
                var userContext = _emamiContext.Users.AsNoTracking();
                var userDivContext = _emamiContext.UserDivisionMappings.AsNoTracking();

                if (inputDto.SalesOrganizationId > 0 && inputDto.VerticalId > 0 && inputDto.DistributionChannelId > 0)
                {
                    userContext = userDivContext.Where(_ => _.SalesOrganizationId == inputDto.SalesOrganizationId
                                                         && _.DistributionChannelId == inputDto.DistributionChannelId
                                                         && _.DivisionId == inputDto.VerticalId).Select(_ => _.User);
                }


                if (inputDto.StateIds != null && inputDto.StateIds.Any() && !inputDto.StateIds.Contains(0))
                {
                    liftingRequestListQueryContext = liftingRequestListQueryContext
                                                        .Join(userContext.Where(_ => inputDto.StateIds.Contains(_.StateId) ), lfq => lfq.UserId, u => u.Id, (lfq, u) => new { lfq }).Select(_ => _.lfq);
                }


                List<long> statusIds = new List<long>();
                if (inputDto.StatusId > 0)
                {
                    statusIds.Add(inputDto.StatusId);
                }
                else
                {
                    statusIds = new List<long>() { (long)DTO.Enums.Status.Approved, (long)DTO.Enums.Status.Pending, (long)DTO.Enums.Status.Rejected };
                }

                var liftingRequestList = liftingRequestListQueryContext.Where(_ => statusIds.Contains(_.StatusId)).ToList();

                foreach (var liftingRequest in liftingRequestList)
                {
                    var LiftingRequestdetail = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.LiftingRequestId == liftingRequest.Id /*&& string.IsNullOrEmpty(_.EnquiryNumber) && string.IsNullOrEmpty(_.DeliveryOrderNumber)*/).ToList();

                    string remarks = string.Empty;


                    var plantDetails = _emamiContext.Depots.Where(_ => _.Id == liftingRequest.PlantId && _.IsPlant).FirstOrDefault();
                    if (LiftingRequestdetail != null && LiftingRequestdetail.Any())
                    {
                        var liftingRequestDto = new LiftingRequestOutputDto
                        {
                            
                            PlantName = plantDetails == null ? string.Empty : plantDetails.Name,
                            PlantCode = plantDetails == null ? string.Empty : plantDetails.Code,
                            DealerId = liftingRequest.UserId,
                            Dealer = _emamiContext.Users.FirstOrDefault(_ => _.Id == liftingRequest.UserId) != null ? _emamiContext.Users.FirstOrDefault(_ => _.Id == liftingRequest.UserId).Name : string.Empty,
                            ShipToPartyId = liftingRequest.ShipToPartyId,
                            ShipToParty = liftingRequest.ShipToParty?.Name,
                            LiftingRequestId = liftingRequest.Id,
                            LiftingRequestNumber = liftingRequest.Id.ToString(),
                            LiftingRequestdate = liftingRequest.LiftingDate,
                            RequestedQuantity = LiftingRequestdetail.Sum(_ => _.LiftingQuantity),
                            RequestedQuantityInCase = LiftingRequestdetail.Sum(_ => _.LiftingQuantityCase),
                            CreatedUser = _emamiContext.Users.FirstOrDefault(_ => _.Id == liftingRequest.CreatedBy) != null ? _emamiContext.Users.FirstOrDefault(_ => _.Id == liftingRequest.CreatedBy).Name : string.Empty,
                            StatusID = liftingRequest.StatusId,
                            Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == liftingRequest.StatusId) != null ? _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == liftingRequest.StatusId).Name : string.Empty,
                            Remarks = liftingRequest.ApproverRemarks == null ? string.Empty : liftingRequest.ApproverRemarks,
                            CustomerRemarks = liftingRequest.CustomerRemarks == null ? string.Empty : liftingRequest.CustomerRemarks,
                            EnquiryNumber = liftingRequest.SAPDocumentNo == null ? string.Empty : liftingRequest.SAPDocumentNo,
                            DeliveryOrderNumber = liftingRequest.SAPDeliveryNo == null ? string.Empty : liftingRequest.SAPDeliveryNo
                        };

                        foreach (var detail in LiftingRequestdetail.ToList())
                        {
                            var oiltype = _emamiContext.OilTypes.FirstOrDefault(_ => _.Id == detail.OilTypeId);
                            var liftingDetailDto = new LiftingRequestDetailsOutputDto
                            {
                                Id = detail.Id,
                                SkuName = detail.Sku?.SkuName,
                                SkuCode = detail.Sku?.SkuCode,
                                OilType = oiltype.Name+"-"+oiltype.SalesOrganization.Code+"/"+oiltype.DistributionChannel.Code+"/"+oiltype.Division.Code,
                                LiftingQuantity = detail.LiftingQuantity,
                                LiftingQuantityCase = detail.LiftingQuantityCase,
                                SaudaNumber = detail.SaudaNumber
                                //InvoiceDate = InvoiceDetails != null && InvoiceDetails.InvoiceDate != null ? InvoiceDetails.InvoiceDate : DateTime.MinValue,
                                //InvoiceNumber = InvoiceDetails != null && InvoiceDetails.InvoiceNumber != null ? InvoiceDetails.InvoiceNumber : string.Empty,
                                //TotalAmount = InvoiceDetails != null && InvoiceDetails.TotalAmount > 0 ? InvoiceDetails.TotalAmount : 0,                      
                            };
                            liftingRequestDto.LiftingRequestDetails.Add(liftingDetailDto);
                        }
                        if (LiftingRequestdetail.Count > 0)
                        {
                            liftingRequestDto.HasChildren = true;
                        }
                        outputDto.Add(liftingRequestDto);
                    }

                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.LiftingRequestId).ToList() : outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public List<SaudaOrderLiftingRequestDto> SaudaOrderLiftingRequest(List<SaudaOrderLiftingRequestMapping> saudaOrderLiftingRequest, List<SaudaOrderDetails> saudaOrderDetails)
        {
            var saudaLiftingResult = new List<SaudaOrderLiftingRequestDto>();
            if (saudaOrderLiftingRequest != null && saudaOrderLiftingRequest.Any())
            {
                //var saudaOrderLifting = saudaOrderLiftingRequest.Join(saudaOrderDetails, l => l.SaudaOrderId, so => so.SaudaOrderId, (l, so) => new { LiftingRequest = l, SaudaNumber = so.SaudaNumber }).ToList();
                foreach (var saudaLifting in saudaOrderLiftingRequest)
                {
                    var saudaOrderLifting = saudaOrderDetails.FirstOrDefault(_ => _.SaudaOrderId == saudaLifting.SaudaOrderId);
                    saudaLiftingResult.Add(new SaudaOrderLiftingRequestDto()
                    {
                        SaudaNumber = saudaOrderLifting != null ? saudaOrderLifting.SaudaNumber : string.Empty,
                        DeliveryOrderNumber = saudaLifting.DeliveryOrderNumber,
                        LiftingQuantity = saudaLifting.LiftingQuantity,
                        LiftingQuantityCase = saudaLifting.LiftingQuantityCase,
                        StatusName = Utility.GetEnumFromString<DTO.Enums.Status>(saudaLifting.StatusId)
                    });
                }
                return saudaLiftingResult;
            }
            return new List<SaudaOrderLiftingRequestDto>();
        }

        #endregion

        public ResultDto GetVehicleLodabilityList(IdInputDto InputDto)
        {
            _methodName = "GetVehicleLodabilityList";
            var resultDto = new ResultDto();
            List<DropDownDto> DropDownDto = new List<DropDownDto>();
            try
            {
                if (InputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                var DealerContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == InputDto.Id);
                if (DealerContext != null)
                {
                    var VehicleLoadabilityContext = _emamiContext.VehicleLodability.Where(_ => _.ZoneId == DealerContext.ZoneId && _.StateId == DealerContext.StateId
                    //&& _.FreightZoneId == DealerContext.FreightZoneId 
                    && _.IsActive).ToList();
                    if (VehicleLoadabilityContext != null && VehicleLoadabilityContext.Any())
                    {
                        foreach (var item in VehicleLoadabilityContext)
                        {
                            var Dto = new DropDownDto
                            {
                                Id = item.Id,
                                Name = item.VehicleSize.ToString()
                            };
                            DropDownDto.Add(Dto);
                        }
                    }
                }
                resultDto.SuccessDto.Response = DropDownDto;
                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        #region Lifting Request - Mobile

        public ResultDto GetLiftingRequestListForMobile(LiftingRequestListsInputDto inputDto)
        {
            _methodName = "GetLiftingRequestListForMobile";
            var resultDto = new ResultDto();
            var outputDto = new LiftingRequestMobileOutputDto();
            outputDto.LiftingRequestList = new List<LiftingRequestMobileListDto>();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var liftingRequestListQueryContext = _emamiContext.LiftingRequest.AsNoTracking();


                #region Filter

                if (inputDto.FromDate != null && inputDto.ToDate != null)
                {
                    liftingRequestListQueryContext = liftingRequestListQueryContext
                        .Where(_ => DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate));
                }

                if (inputDto.StateIds != null && inputDto.StateIds.Any())
                {
                    liftingRequestListQueryContext = liftingRequestListQueryContext
                        .Join(_emamiContext.Users.AsNoTracking()
                        .Where(_ => inputDto.StateIds.Contains(_.StateId)
                        //&& inputDto.VerticalId > 0 ? _.DivisionId == inputDto.VerticalId : _.DivisionId > 0
                        ), lfq => lfq.UserId, u => u.Id, (lfq, u) => new { lfq })
                        .Select(_ => _.lfq);
                }

                List<long> statusIds = new List<long>();
                if (inputDto.StatusId > 0)
                {
                    statusIds.Add(inputDto.StatusId);
                }
                else
                {
                    statusIds = new List<long>() { (long)DTO.Enums.Status.Approved, (long)DTO.Enums.Status.Pending, (long)DTO.Enums.Status.Rejected };
                }

                liftingRequestListQueryContext = liftingRequestListQueryContext.Where(_ => statusIds.Contains(_.StatusId));

                //var data = liftingRequestListQueryContext.ToList();
                var userContext = _emamiContext.Users.Where(_ => _.IsActive);
                if (inputDto.DealerIds != null && inputDto.DealerIds.Any())
                {
                    liftingRequestListQueryContext = liftingRequestListQueryContext.Where(_ => inputDto.DealerIds.Contains(_.UserId));
                }
                else if (inputDto.BdoIds.IsAny())
                {
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    liftingRequestListQueryContext = liftingRequestListQueryContext.Where(_ => dealerIds.Contains(_.UserId));
                }
                else if (inputDto.ZonalHeadIds.IsAny())
                {
                    var bdoIds = userContext.Where(_ => _.ReportingToId != null && inputDto.ZonalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    liftingRequestListQueryContext = liftingRequestListQueryContext.Where(_ => dealerIds.Contains(_.UserId));
                }
                else if (inputDto.NationalHeadIds.IsAny())
                {
                    var zonalHeadIds = userContext.Where(_ => _.ReportingToId != null && inputDto.NationalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var bdoIds = userContext.Where(_ => _.ReportingToId != null && zonalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    liftingRequestListQueryContext = liftingRequestListQueryContext.Where(_ => dealerIds.Contains(_.UserId));
                }

                #endregion

                var description = UtilityHelper.GetEnumDescription(DTO.Enums.Configuration.InboundInterfacenotSyncedToSAPMinutes);
                var configurationContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == description).Value;
                var statusContext = _emamiContext.ApprovalStatus.Where(_ => _.IsActive);
                var oilTypesContext = _emamiContext.OilTypes.Where(_ => _.IsActive);
                var pageSize = Constants.PageSize;
                var skip = pageSize * inputDto.PageNo;

                liftingRequestListQueryContext = liftingRequestListQueryContext.OrderByDescending(_ => _.Id).Skip(skip).Take(pageSize);
                outputDto.LiftingRequestList = liftingRequestListQueryContext
                   .ToList()
                   .Select(liftingRequest => new LiftingRequestMobileListDto()
                   {
                       LiftingRequestId = liftingRequest.Id,
                       DealerId = liftingRequest.UserId,
                       Dealer = userContext.FirstOrDefault(_ => _.Id == liftingRequest.UserId) != null ? userContext.FirstOrDefault(_ => _.Id == liftingRequest.UserId).Name : string.Empty,
                       ShipToPartyId = liftingRequest.ShipToPartyId,
                       ShipToParty = liftingRequest.ShipToParty?.Name,
                       LiftingRequestNumber = liftingRequest.Id.ToString(),
                       LiftingRequestdate = liftingRequest.LiftingDate,
                       CreatedUser = userContext.FirstOrDefault(_ => _.Id == liftingRequest.CreatedBy) != null ? userContext.FirstOrDefault(_ => _.Id == liftingRequest.CreatedBy).Name : string.Empty,
                       StatusID = liftingRequest.StatusId,
                       Status = statusContext.FirstOrDefault(_ => _.Id == liftingRequest.StatusId) != null ? statusContext.FirstOrDefault(_ => _.Id == liftingRequest.StatusId).Name : string.Empty,
                       Remarks = liftingRequest.ApproverRemarks,
                       CustomerRemarks = liftingRequest.CustomerRemarks,
                   }).ToList();



                outputDto.ListCount = outputDto.LiftingRequestList.Count();
                //outputDto.LiftingRequestList = outputDto.LiftingRequestList.OrderByDescending(_ => _.LiftingRequestId).Skip(skip).Take(pageSize).ToList();

                var liftingIds = outputDto.LiftingRequestList.Select(_ => _.LiftingRequestId).ToList();
                var liftingRequestdetails = _emamiContext.LiftingRequestDetails.Where(_ => liftingIds.Contains(_.LiftingRequestId)
                     && string.IsNullOrEmpty(_.EnquiryNumber) && string.IsNullOrEmpty(_.DeliveryOrderNumber));
                foreach (var liftingRequest in outputDto.LiftingRequestList)
                {
                    LiftingRequestDetDTO liftingReq = new LiftingRequestDetDTO();
                    var liftingRequestdetail = liftingRequestdetails.Where(_ => _.LiftingRequestId == liftingRequest.LiftingRequestId
                    && string.IsNullOrEmpty(_.EnquiryNumber) && string.IsNullOrEmpty(_.DeliveryOrderNumber)).ToList();
                    bool IsSapSyncReceivedForLiftingEnquiryUpdate = false;
                    string remarks = string.Empty;

                    if (liftingRequestdetail != null && liftingRequestdetail.Any())
                    {
                        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        TimeSpan difference = currentDate.Subtract(Convert.ToDateTime(liftingRequestdetail.FirstOrDefault().ModifiedDate));

                        if (difference.TotalMinutes > Convert.ToDouble(configurationContext))
                        {
                            if (liftingRequestdetail.FirstOrDefault().EnquiryNumberSyncFromSap)
                            {
                                IsSapSyncReceivedForLiftingEnquiryUpdate = true;
                                remarks = liftingRequestdetail.FirstOrDefault().EnquiryRemarks;
                            }
                            else
                            {
                                IsSapSyncReceivedForLiftingEnquiryUpdate = false;
                                remarks = "Lifting Enquiry Update Sync not received from sap";
                            }
                        }
                        else
                        {
                            IsSapSyncReceivedForLiftingEnquiryUpdate = liftingRequestdetail.FirstOrDefault().EnquiryNumberSyncFromSap;
                            remarks = liftingRequestdetail.FirstOrDefault().EnquiryRemarks;
                        }


                        liftingRequest.ReprocessStatusId = liftingRequestdetail.FirstOrDefault().ReprocessStatusId;
                        liftingRequest.RequestedQuantity = liftingRequestdetail.Sum(_ => _.LiftingQuantity);
                        liftingRequest.RequestedQuantityInCase = liftingRequestdetail.Sum(_ => _.LiftingQuantityCase);
                        liftingRequest.EnquiryNumberSyncFromSap = IsSapSyncReceivedForLiftingEnquiryUpdate;
                        liftingRequest.EnquiryRemarks = remarks;
                    }

                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetLiftingRequestSODetailsForMobile(IdInputDto inputDto)
        {
            _methodName = "GetLiftingRequestSODetailsForMobile";
            var resultDto = new ResultDto();
            var outputDto = new List<LiftingRequestDetailMobileListDto>();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var LiftingRequestList = _emamiContext.LiftingRequest.FirstOrDefault(_ => _.Id == inputDto.Id);
                if (LiftingRequestList == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var statusContext = _emamiContext.ApprovalStatus.AsNoTracking().Where(_ => _.IsActive);
                var oilTypesContext = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.IsActive);

                //var ss = _emamiContext.LiftingRequestDetails.AsNoTracking()
                //    .Where(_ => _.LiftingRequestId == inputDto.Id)
                //    .GroupJoin(_emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking(), ld => ld.Id, so => so.LiftingRequestDetailId, (ld, solrm) => new { ld, solrm })
                //    .SelectMany(x => x.solrm.DefaultIfEmpty(), (x, y) => new { ld = x.ld, solrm = y })
                //    .GroupJoin(_emamiContext.SaudaOrders.AsNoTracking(), lr => lr.solrm.SaudaOrderId, sos => sos.Id, (lr, sos) => new { sos, lr.ld, lr.solrm })
                //    .SelectMany(x => x.sos.DefaultIfEmpty(), (x, y) => new { ld = x.ld, solrm = x.solrm, sos = y })
                //    .ToList();
                outputDto = _emamiContext.LiftingRequestDetails.AsNoTracking()
                    .Where(_ => _.LiftingRequestId == inputDto.Id)
                    .GroupJoin(_emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking(), ld => ld.Id, so => so.LiftingRequestDetailId, (ld, solrm) => new { ld, solrm })
                    .SelectMany(x => x.solrm.DefaultIfEmpty(), (x, y) => new { ld = x.ld, solrm = y })
                    .GroupJoin(_emamiContext.SaudaOrders.AsNoTracking(), lr => lr.solrm.SaudaOrderId, sos => sos.Id, (lr, sos) => new { sos, lr.ld, lr.solrm })
                    .SelectMany(x => x.sos.DefaultIfEmpty(), (x, y) => new { ld = x.ld, solrm = x.solrm, sos = y })
                    .ToList()
                    .Select(_ => new LiftingRequestDetailMobileListDto
                    {
                        LiftingRequestDetailId = _.ld.Id,
                        SkuName = _.ld.Sku?.SkuName,
                        SkuCode = _.ld.Sku?.SkuCode,
                        OilType = oilTypesContext.FirstOrDefault(s => s.Id == _.ld.OilTypeId) != null ? oilTypesContext.FirstOrDefault(s => s.Id == _.ld.OilTypeId).Name : string.Empty,
                        LiftingQuantity = _.ld.LiftingQuantity,
                        LiftingQuantityCase = _.ld.LiftingQuantityCase,
                        LDDeliveryOrderNumber = _.ld.DeliveryOrderNumber,
                        LDStatusName = _.ld.StatusId > 0 ? statusContext.FirstOrDefault(s => s.Id == _.ld.StatusId).Name : string.Empty,
                        LDEnquiryNumber = _.ld.EnquiryNumber,
                        SaudaOrderLRId = _.solrm != null ? _.solrm.Id : 0,
                        SaudaNumber = _.sos != null ? _.sos.SaudaNumber : string.Empty,
                        SaudaOrderLRDeliveryOrderNumber = _.solrm != null ? _.solrm.DeliveryOrderNumber : string.Empty,
                        SaudaOrderLRLiftingQuantity = _.solrm != null ? _.solrm.LiftingQuantity : 0,
                        SaudaOrderLRLiftingQuantityCase = _.solrm != null ? _.solrm.LiftingQuantityCase : 0,
                        SaudaOrderLRStatusId = _.solrm != null ? _.solrm.StatusId : 0,
                        SaudaOrderLRStatus = _.solrm != null ? Utility.GetEnumFromString<DTO.Enums.Status>(_.solrm.StatusId) : string.Empty
                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion

        public ResultDto GetLiftingRequestDetailForPopup(SalesOrderInputDto inputDto)
        {
            _methodName = "GetLiftingRequestDetailForPopup";
            var resultDto = new ResultDto();
            var outputDto = new List<SalesOrderInputDto>();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }


                var saudaOrders = _emamiContext.SaudaOrders.AsNoTracking()
                    .Where(_ => _.Sauda.UserId == inputDto.DealerId
                    && _.SkuId == inputDto.SkuId).ToList();

                var saudaOrderIds = saudaOrders.Select(_ => _.Id).ToList();

                var LiftingRequestSaudaList = _emamiContext.LiftingRequest.Join(_emamiContext.LiftingRequestDetails.AsNoTracking(), l => l.Id , ld => ld.LiftingRequestId , (l,ld) => new { l , ld}).Where(_ => saudaOrderIds.Contains(_.ld.SaudaOrderId)).Select(_ => _.ld.SaudaOrderId).ToList();
                var finalSaudaOrderIds = saudaOrderIds.Except(LiftingRequestSaudaList).ToList();
                outputDto = saudaOrders.Where(_ => finalSaudaOrderIds.Contains(_.Id))
                   .Select(_ => new SalesOrderInputDto()
                   {
                       SaudaOrderId = _.Id,
                       SaudaNumber = _.SaudaNumber,
                       SkuId = _.SkuId,
                       SkuName = _.Sku.SkuName,
                       SkuCode = _.Sku.SkuCode,
                       QantityInCase = _.BidQuantityCase
                   }).ToList();


                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }
    }
}
