using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using GMCore.Helper;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Service
{
    public interface ITradeTicketService
    {
        ResultDto TradeTicketRequestCreation(TradeTicketInputDto inputDto);
        ResultDto TradeTicketRequestModification(TradeTicketInputDto inputDto);
        //ResultDto TradeTicketRequestDetails(TradeTicketInputDto inputDto);
        ResultDto TradeTicketRequestList(TradeTicketParamDto inputDto);
        ResultDto GetTradeTicketStatusList(TradeTicketStatusSearchDto inputDto);
        ResultDto TradeTickeStatusDetails(TradeTicketInputDto inputDto);
        ResultDto TradeTicketDropDown(LoginUserIdDto loginUserIdDto);
        ResultDto MappedTradeTicketSaudaOrders(IdInputDto inputDto);
        ResultDto GetTradeTicketOilTypesForDropdown(IdInputDto inputDto);
        ResultDto TradeTicketDelete(TradeTicketDeleteDto inputDto);

        ResultDto ExcelExportTradeTicketStatus(TradeTicketSearchDto inputDto);
        ResultDto ExportAllTradeTickets(TradeTicketSearchDto inputDto);

        ResultDto TradeTicketSaudaUnMapping(TradeTicketSaudaUnMappingDto inputDto);
        ResultDto GetDealersListByStateId(List<int> stateId);
    }

    public class TradeTicketService : ITradeTicketService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Trade Ticket Service");
        private const string ServiceName = "Trade Ticket Service";
        private string _methodName;

        public TradeTicketService(IAdaniContext salesContext)
        {
            try
            {
                _emamiContext = salesContext;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for sauda Service", exception);
            }
        }

        /// Method to create trade ticket
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto TradeTicketRequestCreation(TradeTicketInputDto inputDto)
        {
            _methodName = "TradeTicketRequestCreation";
            var resultDto = new ResultDto();
            try
            {

                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                //if (inputDto.TradeTicketDetails != null && inputDto.TradeTicketDetails.Any())
                //{
                //    var pricingIds = inputDto.TradeTicketDetails.Select(_ => _.PricingId).ToList();
                //    var existingPriceContext = _emamiContext.Pricing.AsNoTracking().Where(_ => pricingIds.Contains(_.Id) &&
                //    EntityFunctions.TruncateTime(_.BiddingDate) == EntityFunctions.TruncateTime(inputDto.BiddingDate)).ToList();

                //    if (existingPriceContext.Any())
                //    {
                //        resultDto.IsSuccess = false;
                //        resultDto.ErrorDto.ErrorCode = Constants.PriceAlreadyPublished;
                //        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.PriceAlreadyPublished, Constants.EnglishLanguage);
                //        return resultDto;
                //    }
                //}


                var tradeTicketContext = new TradeTicket
                {
                    ContractTypeId = inputDto.ContractTypeId,
                    MaterialTypeId = inputDto.MaterialTypeId,
                    BookingTypeId = inputDto.BookingTypeId,
                    ContractQuantity = inputDto.ContractQuantity,
                    OtherElement = inputDto.OtherElement,
                    UomId = inputDto.UomId,
                    DepotId = UtilityHelper.LongTryToParse(inputDto.PlantOrVendor),
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    IsSAPDataSync = false,
                    ValidFrom = inputDto.ValidFrom,
                    ValidTo = inputDto.ValidTo,
                    ContractDate = inputDto.ContractDate,
                    TotalOilCost = inputDto.TotalOilCost,
                    TotalProcessCost = inputDto.TotalProcessCost,
                    TotalCost = inputDto.TotalCost,
                    DivisionId = inputDto.VerticalId
                };

                _emamiContext.TradeTicket.Add(tradeTicketContext);
                _emamiContext.SaveChanges();

                if (inputDto.TradeTicketDetails != null && inputDto.TradeTicketDetails.Any())
                {
                    foreach (var item in inputDto.TradeTicketDetails)
                    {
                        var tradeTicketDetails = new TradeTicketDetails
                        {
                            TradeTicketOilTypeId = item.OilTypeId,
                            ProcessCost = item.ProcessCost,
                            Proportion = item.Proportion,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            TradeTicketId = tradeTicketContext.Id,
                            OilCost = item.OilCost,

                        };
                        _emamiContext.TradeTicketDetails.Add(tradeTicketDetails);
                        _emamiContext.SaveChanges();

                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = tradeTicketContext.Id;
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

        /// Method to modify trade ticket
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto TradeTicketRequestModification(TradeTicketInputDto inputDto)
        {
            _methodName = "TradeTicketRequestModification";
            var resultDto = new ResultDto();
            try
            {

                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (inputDto.TradeTicketId <= 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.TradeTicketIdMissing;
                    return resultDto;
                }

                var tradeTicketContext = _emamiContext.TradeTicket.FirstOrDefault(_ => _.Id == inputDto.TradeTicketId);
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }
                tradeTicketContext.IsSAPDataSync = false;
                tradeTicketContext.ContractQuantity = inputDto.ContractQuantity;
                tradeTicketContext.TotalOilCost = inputDto.TotalOilCost;
                tradeTicketContext.TotalProcessCost = inputDto.TotalProcessCost;
                tradeTicketContext.TotalCost = inputDto.TotalCost;
                tradeTicketContext.ModifiedBy = inputDto.LoginUserId;
                tradeTicketContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();

                if (inputDto.TradeTicketDetails != null && inputDto.TradeTicketDetails.Any())
                {
                    foreach (var item in inputDto.TradeTicketDetails)
                    {
                        var tradeTicketDetailContext = _emamiContext.TradeTicketDetails.FirstOrDefault(_ => _.Id == item.TradeTicketDetailsId);
                        if (tradeTicketDetailContext != null)
                        {
                            tradeTicketDetailContext.OilCost = item.OilCost;
                            tradeTicketDetailContext.ProcessCost = item.ProcessCost;
                            tradeTicketDetailContext.Proportion = item.Proportion;
                        }
                        _emamiContext.SaveChanges();

                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = tradeTicketContext.Id;
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

        /// Method to get trade ticket request details
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        //public ResultDto TradeTicketRequestDetails(TradeTicketInputDto inputDto)
        //{
        //    _methodName = "TradeTicketRequestDetails";
        //    var resultDto = new ResultDto();
        //    var outputDto = new TradeTicketViewDto();
        //    try
        //    {

        //        if (inputDto == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.Message = Constants.InvalidRequest;
        //            return resultDto;
        //        }

        //        if (inputDto.TradeTicketId <= 0)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.Message = Constants.TradeTicketIdMissing;
        //            return resultDto;
        //        }

        //        var tradeTicketContext = _emamiContext.TradeTicket.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.TradeTicketId);
        //        if (inputDto == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.Message = Constants.RecordNotFound;
        //            return resultDto;
        //        }

        //        int[] SaudaStatus = new int[] { (int)DTO.Enums.Status.Pending, (int)DTO.Enums.Status.Completed, (int)DTO.Enums.Status.Approved, (int)DTO.Enums.Status.Completed };
        //        var saudaOrderContext = new List<SaudaOrder>();
        //        if (!string.IsNullOrEmpty(tradeTicketContext.TradeTicketNumber))
        //        {
        //            saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.TradeTicketNumber == tradeTicketContext.TradeTicketNumber && SaudaStatus.Contains(_.StatusId)).ToList();
        //        }
        //        var tradeTicket = new TradeTicketViewDto
        //        {
        //            TradeTicketId = tradeTicketContext.Id,
        //            TradeTicketNumber = tradeTicketContext.TradeTicketNumber,
        //            ContractTypeId = tradeTicketContext.ContractTypeId,
        //            MaterialTypeId = tradeTicketContext.MaterialTypeId,
        //            BookingTypeId = tradeTicketContext.BookingTypeId,
        //            ContractQuantity = tradeTicketContext.ContractQuantity,
        //            OtherElement = tradeTicketContext.OtherElement,
        //            TotalOtherElement = tradeTicketContext.OtherElement,
        //            UomId = tradeTicketContext.UomId,
        //            DepotId = tradeTicketContext.DepotId,
        //            ContractDate = tradeTicketContext.ContractDate,
        //            ValidFrom = tradeTicketContext.ValidFrom,
        //            ValidTo = tradeTicketContext.ValidTo,
        //            Id = tradeTicketContext.Id,
        //            PlantOrVendor = tradeTicketContext.DepotId.ToString(),
        //            SaudaBookedQuantity = saudaOrderContext.Sum(_ => _.BidQuantity)
        //        };

        //        var tradeTicketDetailList = new List<TradeTicketDetailsDto>();

        //        var tradeTicketDetails = _emamiContext.TradeTicketDetails.AsNoTracking().Join(_emamiContext.TradeTicketOilTypes, tr => tr.TradeTicketOilTypeId, ot => ot.Id, (TradeTicketDetails, OilType) => new { TradeTicketDetails, OilName = OilType.OilTypeName }).Where(_ => _.TradeTicketDetails.TradeTicketId == inputDto.TradeTicketId).ToList();
        //        if (tradeTicketDetails != null && tradeTicketDetails.Any())
        //        {
        //            foreach (var item in tradeTicketDetails)
        //            {
        //                var tradeTicketDetailItem = new TradeTicketDetailsDto
        //                {
        //                    TradeTicketDetailsId = item.TradeTicketDetails.Id,
        //                    OilTypeId = item.TradeTicketDetails.TradeTicketOilTypeId,
        //                    OilCost = item.TradeTicketDetails.OilCost,
        //                    OilCostCalculated = (item.TradeTicketDetails.OilCost * item.TradeTicketDetails.Proportion) / 100,
        //                    ProcessCost = item.TradeTicketDetails.ProcessCost,
        //                    Proportion = item.TradeTicketDetails.Proportion,
        //                    ProcessCostProportion = (item.TradeTicketDetails.ProcessCost * item.TradeTicketDetails.Proportion) / 100,
        //                    OilName = item.OilName,
        //                    TradeTicketNumber = tradeTicketContext.TradeTicketNumber
        //                };

        //                tradeTicketDetailList.Add(tradeTicketDetailItem);
        //            }
        //        }
        //        if (tradeTicketDetailList != null && tradeTicketDetailList.Any())
        //        {
        //            //tradeTicket.TotalOilCost = Math.Round(tradeTicketDetailList.Sum(_ => _.OilCostCalculated));
        //            //tradeTicket.TotalProcessCost = tradeTicketDetailList.Sum(_ => _.ProcessCostProportion);
        //            //tradeTicket.TotalCost = tradeTicket.TotalOilCost + tradeTicket.TotalProcessCost + tradeTicketContext.OtherElement;
        //            tradeTicket.TotalOilCost = tradeTicketContext.TotalOilCost;
        //            tradeTicket.TotalProcessCost = tradeTicketContext.TotalProcessCost;
        //            tradeTicket.TotalCost = tradeTicketContext.TotalCost;
        //        }

        //        tradeTicket.TradeTicketDetail = tradeTicketDetailList;
        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = tradeTicket;
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



        /// Method to trade ticket request list
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto TradeTicketRequestList(TradeTicketParamDto inputDto)
        {
            _methodName = "TradeTicketRequestList";
            var resultDto = new ResultDto();
            var outputDto = new List<TradeTicketViewDto>();
            decimal saudaQuantity = 0;
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var tradeTicketListContext = _emamiContext.TradeTicket.AsNoTracking()
                    .Join(_emamiContext.BookingTypes, tt => tt.BookingTypeId, bt => bt.Id, (TradeTicket, BookingType) => new { TradeTicket, BookingType = BookingType.Name })
                    .Join(_emamiContext.ContractTypes, tb => tb.TradeTicket.ContractTypeId, ct => ct.Id, (tb, ContractType) => new { tb.TradeTicket, tb.BookingType, ContractType = ContractType.Name })
                    //.Join(_emamiContext.MaterialTypes, tbc => tbc.TradeTicket.MaterialTypeId, mt => mt.Id, (tbc, MeterialType) => new { tbc.TradeTicket, tbc.BookingType, tbc.ContractType, MeterialType = MeterialType.Name })
                    .Where(w => DbFunctions.TruncateTime(w.TradeTicket.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    && DbFunctions.TruncateTime(w.TradeTicket.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && w.TradeTicket.DivisionId == inputDto.VerticalId)
                    //.Join(_emamiContext.Uom,tbcm=>tbcm.TradeTicket.UomId,uom=>uom.Id,(tbcm,uom) => new { tbcm.TradeTicket, tbcm.BookingType, tbcm.ContractType, tbcm.MeterialType, UnitOfMeasure=uom.Name })
                    //.Join(_emamiContext.Plants, tbcmu => tbcmu.TradeTicket.UomId, pl => pl.Id, (tbcmu, pl) => new { tbcmu.TradeTicket, tbcmu.BookingType, tbcmu.ContractType, tbcmu.MeterialType, tbcmu.UnitOfMeasure, PlantOrVendor= pl.Name })
                    .OrderByDescending(_ => _.TradeTicket.CreatedDate).ToList();

                if (tradeTicketListContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }

                foreach (var rec in tradeTicketListContext)
                {
                    if (!string.IsNullOrEmpty(rec.TradeTicket.TradeTicketNumber))
                        saudaQuantity = GetTradeTicketSaudaQuantity(rec.TradeTicket.TradeTicketNumber);

                    var tradeTicketDetails = _emamiContext.TradeTicketDetails.AsNoTracking().Where(_ => _.TradeTicketId == rec.TradeTicket.Id)
                        .Select(s => new { OilTypeName = s.TradeTicketOilType.OilTypeName, ProcessCost = s.ProcessCost, OilCost = s.OilCost, Proportion = s.Proportion }).ToList();
                    var plantName = _emamiContext.Depots.Where(a => a.Id == rec.TradeTicket.DepotId).FirstOrDefault()?.Name;

                    decimal ratePerMT = 0;
                    if (tradeTicketDetails != null && tradeTicketDetails.Any())
                    {
                        foreach (var item in tradeTicketDetails)
                        {
                            var oilCostCalculated = (item.OilCost * item.Proportion) / 100;
                            ratePerMT += oilCostCalculated;
                            ratePerMT += item.ProcessCost;
                            ratePerMT = Math.Round(ratePerMT);
                        }
                    }

                    var tradeTicketContext = new TradeTicketViewDto
                    {
                        TradeTicketId = rec.TradeTicket.Id,
                        ContractTypeId = rec.TradeTicket.ContractTypeId,
                        MaterialTypeId = rec.TradeTicket.MaterialTypeId,
                        BookingTypeId = rec.TradeTicket.BookingTypeId,
                        OtherElement = rec.TradeTicket.OtherElement,
                        UomId = rec.TradeTicket.UomId,
                        DepotId = rec.TradeTicket.DepotId,
                        ValidFrom = rec.TradeTicket.ValidFrom,
                        ValidTo = rec.TradeTicket.ValidTo,
                        ContractDate = rec.TradeTicket.ContractDate,
                        ContractType = rec.ContractType,
                        //MaterialType = rec.MeterialType,
                        BookingType = rec.BookingType,
                        UnitOfMeasure = rec.TradeTicket.UnitOfMeasure ?? "",
                        PlantOrVendor = rec.TradeTicket.DepotId.ToString() ?? "",
                        TradeTicketNumber = rec.TradeTicket.TradeTicketNumber ?? "",
                        ContractQuantity = rec.TradeTicket.ContractQuantity,
                        SaudaBookedQuantity = saudaQuantity,
                        OpenQty = rec.TradeTicket.ContractQuantity - saudaQuantity,
                        PlantName = plantName,
                        TradeTicketOilTypes = string.Join(",", tradeTicketDetails.Select(_ => _.OilTypeName)),
                        SAPCreationDate = rec.TradeTicket.ContractDate,
                        //RatePerMT = tradeTicketDetails.Sum(_ => _.ProcessCost) + tradeTicketDetails.Sum(_ => _.OilCost),
                        RatePerMT = ratePerMT,
                    };
                    outputDto.Add(tradeTicketContext);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.Id).ToList() : outputDto;
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

        public decimal GetTradeTicketSaudaQuantity(string TradeTicketNumbert)
        {
            decimal saudaQuantity = 0;
            var saudaOrderList = _emamiContext.SaudaOrders.AsNoTracking()
                .Where(_ => /*_.TradeTicketNumber == TradeTicketNumbert &&*/ (_.StatusId == (int)DTO.Enums.Status.Pending || _.StatusId == (int)DTO.Enums.Status.Approved)).ToList();
            if (saudaOrderList != null && saudaOrderList.Any())
            {
                saudaQuantity = saudaOrderList.Sum(_ => _.BidQuantity);
            }
            return saudaQuantity;
        }

        /// Method to trade ticket status list
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto GetTradeTicketStatusList(TradeTicketStatusSearchDto inputDto)
        {
            _methodName = "GetTradeTicketStatusList";
            var resultDto = new ResultDto();
            var outputDto = new List<TradeTicketStatusListDto>();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var tradeTicketListContext = _emamiContext.TradeTicket.AsNoTracking()
                    .Where(_ => !string.IsNullOrEmpty(_.TradeTicketNumber) 
                    && DbFunctions.TruncateTime(_.ContractDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    && DbFunctions.TruncateTime(_.ContractDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                    //&& DbFunctions.TruncateTime(_.CreatedDate) == inputDto.SearchDate)
                    .OrderByDescending(_ => _.CreatedDate).ToList();

                if(inputDto.VerticalId != 0)
                {
                    tradeTicketListContext = tradeTicketListContext.Where(s => s.DivisionId == inputDto.VerticalId).ToList();
                }

                if (tradeTicketListContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }

                var DepotContext = _emamiContext.Depots.AsNoTracking();
                var tradeticketNumberlist = tradeTicketListContext.Select(_ => _.TradeTicketNumber).ToList();
                var saudaOrdercontext = _emamiContext.SaudaOrders.AsNoTracking()
                        ./*Where(_ => tradeticketNumberlist.Contains(_.TradeTicketNumber))*/ToList();
                var tradeTicketIdslist = tradeTicketListContext.Select(_ => _.Id).ToList();
                var tradeTicketDetailsContext = _emamiContext.TradeTicketDetails.AsNoTracking().Where(_ => tradeTicketIdslist.Contains(_.TradeTicketId)).ToList();
                foreach (var tradeTicket in tradeTicketListContext)
                {
                    var plantOrDepot = DepotContext.Where(a => a.Id == tradeTicket.DepotId).FirstOrDefault();
                    var plantName = plantOrDepot != null ? plantOrDepot.Name : string.Empty;

                    var saudaQuantity = (decimal)0;
                    var saudaOrderList = saudaOrdercontext
                        .Where(_ => 
                        (_.StatusId == (int)DTO.Enums.Status.Pending || _.StatusId == (int)Adani.Solution.DTO.Enums.Status.Approved)).ToList();
                    if (saudaOrderList != null && saudaOrderList.Any())
                    {
                        saudaQuantity = saudaOrderList.Sum(_ => _.BidQuantity);
                    }

                    //var materialType = (tradeTicket.MaterialTypeId != 0 && tradeTicket.DivisionId != 0 && _emamiContext.MaterialTypes.AsNoTracking().FirstOrDefault(_ => _.Id == tradeTicket.MaterialTypeId && _.DivisionId == tradeTicket.DivisionId) != null) ? _emamiContext.MaterialTypes.AsNoTracking().FirstOrDefault(_ => _.Id == tradeTicket.MaterialTypeId && _.DivisionId == tradeTicket.DivisionId).Name : string.Empty;
                    var tradeTicketDetails = tradeTicketDetailsContext.Where(_ => _.TradeTicketId == tradeTicket.Id).ToList();
                    var tradeTicketContext = new TradeTicketStatusListDto
                    {
                        TradeTicketId = tradeTicket.Id,
                        TradeTicketNumber = tradeTicket.TradeTicketNumber,
                        TotalQuantity = tradeTicket.ContractQuantity,
                        SaudaQuantity = saudaQuantity,
                        OpenQty = tradeTicket.OpenQuantityFromSap > 0 ? tradeTicket.OpenQuantityFromSap : (tradeTicket.ContractQuantity > 0) ?tradeTicket.ContractQuantity - saudaQuantity : 0,
                        PlantName = plantName,
                        TradeTicketOilTypes = string.Join(",", tradeTicketDetails.Select(_ => _.TradeTicketOilType.OilTypeName)),
                        SAPCreationDate = tradeTicket.ContractDate,
                        RatePerMT = (tradeTicket.TotalCost > 0 && tradeTicket.ContractQuantity > 0) ? tradeTicket.TotalCost / tradeTicket.ContractQuantity : 0,
                        //MaterialType = materialType,
                        TTStatus = tradeTicket.TTStatus != null ? tradeTicket.TTStatus : string.Empty
                    };
                    outputDto.Add(tradeTicketContext);
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

        public ResultDto MappedTradeTicketSaudaOrders(IdInputDto inputDto)
        {
            _methodName = "MapTradeTicketToSaudaOrders";
            var resultDto = new ResultDto();
            var outputdto = new List<SaudaOrderViewDto>();
            if (inputDto == null)
            {
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.InvalidRequest;
                return resultDto;
            }
            try
            {
                var TradeTicketContext = _emamiContext.TradeTicket.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.Id);
                if (TradeTicketContext != null)
                {
                    var saudaOrders = _emamiContext.SaudaOrders
                        .Where(_ => (_.StatusId == (int)Adani.Solution.DTO.Enums.Status.Pending || _.StatusId == (int)Adani.Solution.DTO.Enums.Status.Approved))
                        .ToList();
                    var skuUomMapping = saudaOrders.Join(_emamiContext.SkuUomMapping.AsNoTracking().Where(_ => _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos), x => x.SkuId, s => s.SkuId, (x, s) => new { NoOfSkusPerCase = s.ConversionFactor }).FirstOrDefault();

                    foreach (var orders in saudaOrders)
                    {
                        var sauda = new SaudaOrderViewDto
                        {
                            Id = orders.Id,
                            SaudhaId = orders.SaudaId,
                            SaudaOrderId = orders.Id,
                            SaudhaNumber = orders.SaudaNumber,
                            OilTypeId = orders.OilTypeId,
                            Oiltype = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == orders.OilTypeId).Name,
                            SkuId = orders.SkuId,
                            Sku = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == orders.SkuId).SkuName,
                            BidPrice = orders.BidPrice,
                            BidQuantity = orders.BidQuantity,
                            PlantName = _emamiContext.Depots.Where(_ => _.Id == orders.PlantId).FirstOrDefault().Name,
                            BookingDate = orders.CreatedDate,
                            BidPricePerSku = (orders.BidPrice / orders.BidQuantityCase) / skuUomMapping.NoOfSkusPerCase,
                            BidQuantityCase = orders.BidQuantityCase
                        };
                        outputdto.Add(sauda);
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputdto;
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

        /// Method to trade ticket status details
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto TradeTickeStatusDetails(TradeTicketInputDto inputDto)
        {
            _methodName = "TradeTickeStatusDetails";
            var resultDto = new ResultDto();
            var outputDto = new TradeTicketViewDto();
            try
            {

                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (inputDto.TradeTicketId <= 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.TradeTicketIdMissing;
                    return resultDto;
                }

                var tradeTicketContext = _emamiContext.TradeTicket.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.TradeTicketId);
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }

                var tradeTicket = new TradeTicketViewDto
                {
                    ContractTypeId = tradeTicketContext.ContractTypeId,
                    MaterialTypeId = tradeTicketContext.MaterialTypeId,
                    BookingTypeId = tradeTicketContext.BookingTypeId,
                    ContractQuantity = tradeTicketContext.ContractQuantity,
                    OtherElement = tradeTicketContext.OtherElement,
                    UomId = tradeTicketContext.UomId,
                    DepotId = tradeTicketContext.DepotId,



                };

                var tradeTicketDetailList = new List<TradeTicketDetailsDto>();

                var tradeTicketDetails = _emamiContext.TradeTicketDetails.AsNoTracking().Where(_ => _.Id == inputDto.TradeTicketId);
                if (tradeTicketDetails != null && tradeTicketDetails.Any())
                {
                    foreach (var item in tradeTicketDetails)
                    {
                        var tradeTicketDetailItem = new TradeTicketDetailsDto
                        {
                            OilTypeId = item.TradeTicketOilTypeId,
                            OilCost = item.OilCost,
                            ProcessCost = item.ProcessCost,
                            Proportion = item.Proportion,
                            OilName = item.TradeTicketOilType.OilTypeName
                        };

                        tradeTicketDetailList.Add(tradeTicketDetailItem);
                    }
                }
                tradeTicket.TradeTicketDetail = tradeTicketDetailList;

                //To DO : Need to write the code for get the sauda list map with trade ticket number

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = tradeTicket;
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

        public ResultDto TradeTicketDropDown(LoginUserIdDto inputDto)
        {
            _methodName = "TradeTicketDropDown";
            var resultDto = new ResultDto();
            var outputDto = default(List<DropDownDto>);
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                outputDto = _emamiContext.TradeTicket.AsNoTracking().Where(s => s.TradeTicketNumber != null)
                    .OrderByDescending(_ => _.CreatedDate).Select(s => new DropDownDto() { Id = s.Id, Name = s.TradeTicketNumber }).ToList();

                if (outputDto == default(List<DropDownDto>))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
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

        public ResultDto GetTradeTicketOilTypesForDropdown(IdInputDto inputDto)
        {
            _methodName = "GetTradeTicketOilTypesForDropdown";
            var resultDto = new ResultDto();
            try
            {
                var oiltypeList = new List<DropDownDto>();
                if (inputDto.Id == (int)DTO.Enums.Division.Hbc)
                {
                    oiltypeList = _emamiContext.TradeTicketOilTypes.AsNoTracking().Where(_ => _.IsActive && _.DivisionId == (int)DTO.Enums.Division.Hbc)
                    .Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.OilTypeName
                    }).ToList();
                }
                else if (inputDto.Id == (int)DTO.Enums.Division.SpecialityFat)
                {
                    oiltypeList = _emamiContext.TradeTicketOilTypes.AsNoTracking().Where(_ => _.IsActive && _.DivisionId == (int)DTO.Enums.Division.SpecialityFat)
                    .Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.OilTypeName
                    }).ToList();
                }
                else
                {
                    oiltypeList = _emamiContext.TradeTicketOilTypes.AsNoTracking().Where(_ => _.IsActive)
                    .Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.OilTypeName
                    }).ToList();
                }

                resultDto.SuccessDto.Response = oiltypeList;
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

        public ResultDto TradeTicketDelete(TradeTicketDeleteDto inputDto)
        {
            _methodName = "TradeTicketDelete";
            var resultDto = new ResultDto();
            try
            {
                var tradeTicket = _emamiContext.TradeTicket.FirstOrDefault(s => s.Id == inputDto.TradeTicketId);
                _emamiContext.TradeTicket.Remove(tradeTicket);
                _emamiContext.SaveChanges();
                inputDto.PostStatus = true;
                inputDto.PostMessage = "Trade ticket deleted successfully";
                resultDto.SuccessDto.Response = inputDto;

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

        public ResultDto TradeTicketSaudaUnMapping(TradeTicketSaudaUnMappingDto inputDto)
        {
            _methodName = "TradeTicketSaudaUnMapping";
            var resultDto = new ResultDto();
            try
            {
                var saudaDetails = _emamiContext.SaudaOrders.FirstOrDefault(s => s.Id == inputDto.SaudaId);
                if (saudaDetails == null)
                {
                    resultDto.IsSuccess = false;
                    inputDto.PostStatus = false;
                    inputDto.PostMessage = Constants.RecordNotFound;
                    resultDto.SuccessDto.Response = inputDto;
                }
                else
                {
                   // saudaDetails.TradeTicketNumber = string.Empty;
                    saudaDetails.ModifiedBy = inputDto.LoginUserId;
                    saudaDetails.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.SaveChanges();

                    resultDto.SuccessDto.Response = inputDto;
                    inputDto.PostStatus = true;
                    resultDto.IsSuccess = true;
                }
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

        public ResultDto GetDealersListByStateId(List<int> stateId)
        {
            _methodName = "GetDealersListByStateId";
            var resultDto = new ResultDto();
            
            try
            {
               var DelearDto = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), a => a.Id, b => b.UserId, (a, b) => new { a, b }).Where(_ => _.b.RoleId == (int)DTO.Enums.Role.Dealer && stateId.Contains(_.a.StateId)).Select(s => new DropDownDto() { Id = s.a.Id, Name = s.a.Name }).ToList();
                    
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = DelearDto;
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

        #region Export Trade Ticket

        public ResultDto ExcelExportTradeTicketStatus(TradeTicketSearchDto inputDto)
        {
            _methodName = "ExcelExportTradeTicketStatus";
            var resultDto = new ResultDto();
            var outputDto = new List<TradeTicketExportDto>();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var tradeTicketListContext = _emamiContext.TradeTicket.AsNoTracking()
                    .Where(_ => !string.IsNullOrEmpty(_.TradeTicketNumber)
                    && DbFunctions.TruncateTime(inputDto.FromDate) <= DbFunctions.TruncateTime(_.ContractDate)
                    && DbFunctions.TruncateTime(_.ContractDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && inputDto.VerticalId > 0 ? _.DivisionId == inputDto.VerticalId : _.DivisionId > 0)
                    .OrderByDescending(_ => _.CreatedDate).ToList();

                if (tradeTicketListContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }

                foreach (var tradeTicket in tradeTicketListContext)
                {
                    var plantOrDepot = _emamiContext.Depots.Where(a => a.Id == tradeTicket.DepotId).FirstOrDefault();
                    var plantName = plantOrDepot != null ? plantOrDepot.Name : string.Empty;

                    var saudaQuantity = (decimal)0;
                    var saudaOrderList = _emamiContext.SaudaOrders.AsNoTracking()
                        .Where(_ => (_.StatusId == (int)DTO.Enums.Status.Pending || _.StatusId == (int)Adani.Solution.DTO.Enums.Status.Approved)).ToList();
                    if (saudaOrderList != null && saudaOrderList.Any())
                    {
                        saudaQuantity = saudaOrderList.Sum(_ => _.BidQuantity);
                    }

                    var tradeTicketDetails = _emamiContext.TradeTicketDetails.AsNoTracking().Where(_ => _.TradeTicketId == tradeTicket.Id).ToList();
                    var tradeTicketExportDto = new TradeTicketExportDto
                    {
                        TradeTicketId = tradeTicket.Id,
                        TradeTicketNumber = tradeTicket.TradeTicketNumber,
                        TotalQuantity = tradeTicket.ContractQuantity,
                        SaudaQuantity = saudaQuantity,
                        OpenQty = tradeTicket.ContractQuantity - saudaQuantity,
                        PlantName = plantName,
                        TradeTicketOilTypes = string.Join(",", tradeTicketDetails.Select(_ => _.TradeTicketOilType.OilTypeName)),
                        SAPCreationDate = tradeTicket.ContractDate,
                        RatePerMT = tradeTicketDetails.Sum(_ => _.ProcessCost) + tradeTicketDetails.Sum(_ => _.OilCost),
                    };

                    var skuUomMapping = saudaOrderList.Join(_emamiContext.SkuUomMapping.AsNoTracking().Where(_ => _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos), x => x.SkuId, s => s.SkuId, (x, s) => new { NoOfSkusPerCase = s.ConversionFactor }).FirstOrDefault();

                    foreach (var orders in saudaOrderList)
                    {
                        var bidPricePerSku = 0.0m;
                        if (orders.BidQuantityCase > 0)
                        {
                            var bp = orders.BidPrice / orders.BidQuantityCase;
                            if (skuUomMapping.NoOfSkusPerCase > 0)
                            {
                                bidPricePerSku = bp / skuUomMapping.NoOfSkusPerCase;
                            }
                        }
                        var sauda = new SaudaOrderViewDto
                        {
                            SaudhaId = orders.SaudaId,
                            SaudaOrderId = orders.Id,
                            SaudhaNumber = orders.SaudaNumber,
                            OilTypeId = orders.OilTypeId,
                            Oiltype = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == orders.OilTypeId)?.Name,
                            SkuId = orders.SkuId,
                            Sku = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == orders.SkuId)?.SkuName,
                            BidPrice = orders.BidPrice,
                            BidQuantity = orders.BidQuantity,
                            PlantName = _emamiContext.Depots.Where(_ => _.Id == orders.PlantId).FirstOrDefault()?.Name,
                            BookingDate = orders.CreatedDate,
                            BidPricePerSku = bidPricePerSku,
                            BidQuantityCase = orders.BidQuantityCase
                        };
                        tradeTicketExportDto.TradeTicketDetailList.Add(sauda);
                    }

                    outputDto.Add(tradeTicketExportDto);
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

        public ResultDto ExportAllTradeTickets(TradeTicketSearchDto inputDto)
        {
            _methodName = "ExportAllTradeTickets";
            var resultDto = new ResultDto();
            var outputDto = new List<TradeTicketExportAllDto>();
            decimal saudaQuantity = 0;
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var tradeTicketListContext = _emamiContext.TradeTicket.AsNoTracking()
                    .Join(_emamiContext.BookingTypes, tt => tt.BookingTypeId, bt => bt.Id, (TradeTicket, BookingType) => new { TradeTicket, BookingType = BookingType.Name })
                    .Join(_emamiContext.ContractTypes, tb => tb.TradeTicket.ContractTypeId, ct => ct.Id, (tb, ContractType) => new { tb.TradeTicket, tb.BookingType, ContractType = ContractType.Name })
                    //.Join(_emamiContext.MaterialTypes, tbc => tbc.TradeTicket.MaterialTypeId, mt => mt.Id, (tbc, MeterialType) => new { tbc.TradeTicket, tbc.BookingType, tbc.ContractType, MeterialType = MeterialType.Name })
                    .Where(w => DbFunctions.TruncateTime(w.TradeTicket.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    && DbFunctions.TruncateTime(w.TradeTicket.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                    .OrderByDescending(_ => _.TradeTicket.CreatedDate).ToList();

                if (tradeTicketListContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }

                foreach (var rec in tradeTicketListContext)
                {
                    if (!string.IsNullOrEmpty(rec.TradeTicket.TradeTicketNumber))
                        saudaQuantity = GetTradeTicketSaudaQuantity(rec.TradeTicket.TradeTicketNumber);

                    var tradeTicketDetails = _emamiContext.TradeTicketDetails.AsNoTracking().Where(_ => _.TradeTicketId == rec.TradeTicket.Id)
                        .Select(s => new { s.TradeTicketOilType.OilTypeName, s.ProcessCost, s.OilCost }).ToList();
                    var plantName = _emamiContext.Depots.Where(a => a.Id == rec.TradeTicket.DepotId).FirstOrDefault()?.Name;

                    var tradeTicketExportAllDto = new TradeTicketExportAllDto
                    {
                        TradeTicketId = rec.TradeTicket.Id,
                        OtherElement = rec.TradeTicket.OtherElement,
                        ValidFrom = rec.TradeTicket.ValidFrom,
                        ValidTo = rec.TradeTicket.ValidTo,
                        ContractDate = rec.TradeTicket.ContractDate,
                        ContractType = rec.ContractType,
                        //MaterialType = rec.MeterialType,
                        BookingType = rec.BookingType,
                        UnitOfMeasure = rec.TradeTicket.UnitOfMeasure ?? "",
                        PlantOrVendor = rec.TradeTicket.DepotId.ToString() ?? "",
                        TradeTicketNumber = rec.TradeTicket.TradeTicketNumber ?? "",
                        ContractQuantity = rec.TradeTicket.ContractQuantity,
                        SaudaBookedQuantity = saudaQuantity,
                        OpenQty = rec.TradeTicket.ContractQuantity - saudaQuantity,
                        PlantName = plantName,
                        TradeTicketOilTypes = string.Join(",", tradeTicketDetails.Select(_ => _.OilTypeName)),
                        SAPCreationDate = rec.TradeTicket.ContractDate,
                        RatePerMT = tradeTicketDetails.Sum(_ => _.ProcessCost) + tradeTicketDetails.Sum(_ => _.OilCost),
                    };
                    outputDto.Add(tradeTicketExportAllDto);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.TradeTicketId).ToList() : outputDto;
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
    }
}
