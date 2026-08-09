using Adani.Solution.API.App_Start;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service;
using Adani.Solution.Service.Common;
using GMCore.Authenticate;
using GMCore.Helper;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;

namespace Adani.Solution.API.Controllers
{ 
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/mobilereverseauction")]
    public class MobileReverseAuctionController : BaseApiController
    {
        private const string ServiceName = "Reverse Auction Controller";
        private readonly IMobileReverseAuctionService _reverseAuctionService;
        private string _methodName;

        public MobileReverseAuctionController(IMobileReverseAuctionService reverseAuctionService)
            : base(ServiceName)
        {
            _methodName = "Reverse Auction Controller";
            try
            {
                _reverseAuctionService = reverseAuctionService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        //#region Bidding Window

        ///// <summary>
        ///// Method to get the biddding window list
        ///// </summary>
        ///// <param name="inputKey"></param>
        ///// <returns></returns>
        ////[HttpPost]
        ////[Route("biddingwindow/list")]
        ////[ResponseType(typeof(ContentDto))]
        ////[Throttle(Name = "GetBiddingWindowListForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        ////public IHttpActionResult GetBiddingWindowListForMobile([FromBody]string inputKey)
        ////{
        ////    _methodName = "GetBiddingWindowListForMobile";
        ////    return Result(inputKey, _methodName, (BiddingWindowInputDto x) => { return _reverseAuctionService.GetBiddingWindowListForMobile(x); });
        ////}
        ////[HttpPost]
        ////[Route("biddingwindow/discounts/benefits")]
        ////[ResponseType(typeof(ContentDto))]
        ////[Throttle(Name = "GetDiscountsAndBenefits", Message = "The request has been declined for security reasons.", Seconds = 1)]
        ////public IHttpActionResult GetDiscountsAndBenefits([FromBody]string inputKey)
        ////{
        ////    _methodName = "GetDiscountsAndBenefits";
        ////    return KendoGridResult(inputKey, _methodName, (IdInputDto Id) => { return _reverseAuctionService.GetDiscountsAndBenefits(Id); });
        ////}

        ////[HttpPost]
        ////[Route("biddingwindow/availablebidquantity")]
        ////[ResponseType(typeof(ContentDto))]
        ////[Throttle(Name = "GetAvailableBidQuantity", Message = "The request has been declined for security reasons.", Seconds = 1)]
        ////public IHttpActionResult GetAvailableBidQuantity([FromBody]string inputKey)
        ////{
        ////    _methodName = "GetAvailableBidQuantity";
        ////    return KendoGridResult(inputKey, _methodName, (AvailableBidQuantityInputDto Id) => { return _reverseAuctionService.GetAvailableBidQuantity(Id); });
        ////}

        //#endregion

        //#region StateTrader Bidding Window List

        ////[HttpPost]
        ////[Route("bdobiddingwindow/list")]
        ////[ResponseType(typeof(ContentDto))]
        ////[Throttle(Name = "GetBDOBiddingWindowDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        ////public IHttpActionResult GetBDOBiddingWindowDetails([FromBody]string inputKey)
        ////{
        ////    _methodName = "GetBDOBiddingWindowDetails";
        ////    return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _reverseAuctionService.GetBDOBiddingWindowDetails(x); });
        ////}
        
        //#endregion

        //#region Dealer Bidding Window List

        ////[HttpPost]
        ////[Route("dealerbiddingwindow/list")]
        ////[ResponseType(typeof(ContentDto))]
        ////[Throttle(Name = "GetDealerBiddingWindowDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        ////public IHttpActionResult GetDealerBiddingWindowDetails([FromBody]string inputKey)
        ////{
        ////    _methodName = "GetDealerBiddingWindowDetails";
        ////    return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _reverseAuctionService.GetDealerBiddingWindowDetails(x); });
        ////}
        
        //#endregion

        //#region Bidding

        ///// <summary>
        ///// Method to get the biddding window list
        ///// </summary>
        ///// <param name="inputKey"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("DealerAndBrokersByBiddingWindow/list")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetBiddingWindowListForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetDealerAndBrokersByBiddingWindow([FromBody]string inputKey)
        //{
        //    _methodName = "GetBiddingWindowListForMobile";
        //    return Result(inputKey, _methodName, (DealerAndBrokersInputDto x) => { return _reverseAuctionService.GetDealerAndBrokersByBiddingWindow(x); });
        //}

        //[HttpPost]
        //[Route("BiddingCart/OilTypes")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "BiddingCartOilTypes", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult BiddingCartOilTypes([FromBody]string inputKey)
        //{
        //    _methodName = "BiddingCartOilTypes";
        //    return Result(inputKey, _methodName, (DealerAndBrokersInputDto x) => { return _reverseAuctionService.BiddingCartOilTypes(x); });
        //}

        //[HttpPost]
        //[Route("BiddingCart/SkuDetails")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "BiddingCartSkuDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult BiddingCartSkuDetails([FromBody]string inputKey)
        //{
        //    _methodName = "BiddingCartSkuDetails";
        //    return Result(inputKey, _methodName, (BiddingCartSkuInputDto x) => { return _reverseAuctionService.BiddingCartSkuDetails(x); });
        //}

        //[HttpPost]
        //[Route("Sauda/BiddingCreation")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "SaudaBiddingCreation", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult SaudaBiddingCreation([FromBody]string inputKey)
        //{
        //    _methodName = "SaudaBiddingCreation";
        //    return Result(inputKey, _methodName, (SaudaBiddingCreationInputDto x) => { return _reverseAuctionService.SaudaBiddingCreation(x); });
        //}

        //[HttpPost]
        //[Route("Bidding/list")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "SaudaBiddingLists", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult SaudaBiddingLists([FromBody]string inputKey)
        //{
        //    _methodName = "SaudaBiddingLists";
        //    return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _reverseAuctionService.SaudaBiddingLists(x); });
        //}

        //[HttpPost]
        //[Route("Bidding/Details")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "SaudaBiddingDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult SaudaBiddingDetails([FromBody]string inputKey)
        //{
        //    _methodName = "SaudaBiddingDetails";
        //    return Result(inputKey, _methodName, (IdInputDto x) => { return _reverseAuctionService.SaudaBiddingDetails(x); });
        //}

        //[HttpPost]
        //[Route("Bidding/EditQuantity")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "EditSaudaBiddingQuantity", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult EditSaudaBiddingQuantity([FromBody]string inputKey)
        //{
        //    _methodName = "EditSaudaBiddingQuantity";
        //    return Result(inputKey, _methodName, (SaudaBiddingQuantityEditInputDto x) => { return _reverseAuctionService.EditSaudaBiddingQuantity(x); });
        //}

        //[HttpPost]
        //[Route("saudacounterbit/statusupdate")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "statusupdate", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult SaudaCounterbitStatusUpdate([FromBody]string inputKey)
        //{
        //    _methodName = "statusupdate";
        //    return Result(inputKey, _methodName, (SaudaCounterBidOfferStatusUpdate x) => { return _reverseAuctionService.SaudaCounterbitStatusUpdate(x); });
        //}

        //[HttpPost]
        //[Route("saudacounterbit/skulist/notification")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetCounterBidNotificationDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetCounterBidNotificationDetails([FromBody]string inputKey)
        //{
        //    _methodName = "GetCounterBidNotificationDetails";
        //    return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _reverseAuctionService.GetCounterBidNotificationDetails(x); });
        //}

        //[HttpPost]
        //[Route("SaudaConversion/Formula")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "SaudaConversionFormulaForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult SaudaConversionFormulaForMobile([FromBody]string inputKey)
        //{
        //    _methodName = "SaudaConversionFormulaForMobile";
        //    return Result(inputKey, _methodName, (IdInputDto x) => { return _reverseAuctionService.SaudaConversionFormulaForMobile(x); });
        //}

        //#endregion

        //#region Sauda Allocation

        //[HttpPost]
        //[Route("SaudaAllocation/saudalist/dealerId")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSaudaListByUserId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSaudaListByUserId([FromBody]string inputKey)
        //{
        //    _methodName = "GetSaudaListByUserId";
        //    return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _reverseAuctionService.GetSaudaListForSaudaAllocationByUserId(x); });
        //}

        //[HttpPost]
        //[Route("SaudaAllocation/creation")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "SaudaAllocationCreation", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult SaudaAllocationCreation([FromBody]string inputKey)
        //{

        //    _methodName = "SaudaAllocationCreation";
        //    _logger.Error("SaudaAllocationCreation Input " + inputKey);
        //    return Result(inputKey, _methodName, (SaudaBiddingCreationInputDto x) => { return _reverseAuctionService.SaudaAllocationCreation(x); });
        //}
        //[HttpPost]
        //[Route("saudaAllocation/skuDetails")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "SaudaAllocationSkuDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult SaudaAllocationSkuDetails([FromBody]string inputKey)
        //{
        //    _methodName = "SaudaAllocationSkuDetails";
        //    return Result(inputKey, _methodName, (BiddingCartSkuInputDto x) => { return _reverseAuctionService.SaudaAllocationSkuDetails(x); });
        //}

        //[HttpPost]
        //[Route("SaudaAllocation/list")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSaudaAllocationListForDealer", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSaudaAllocationListForDealer([FromBody]string inputKey)
        //{
        //    _methodName = "GetSaudaAllocationListForDealer";
        //    return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _reverseAuctionService.GetSaudaAllocationListForDealer(x); });
        //}

        //[HttpPost]
        //[Route("SaudaAllocation/listForBDO")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSaudaAllocationListForBDO", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSaudaAllocationListForBDO([FromBody]string inputKey)
        //{
        //    _methodName = "GetSaudaAllocationListForBDO";
        //    return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _reverseAuctionService.GetSaudaAllocationListForBDO(x); });
        //}


        //[HttpPost]
        //[Route("SaudaAllocation/details")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSaudaAllocationDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSaudaAllocationDetails([FromBody]string inputKey)
        //{
        //    _methodName = "GetSaudaAllocationDetails";
        //    return Result(inputKey, _methodName, (SaudaAllocationInputDto x) => { return _reverseAuctionService.GetSaudaAllocationDetails(x); });
        //}

        //#endregion

        //#region Sauda Details

        //[HttpPost]
        //[Route("Dealer/SaudaDetails")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSaudaDetailsForDealer", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSaudaDetailsForDealer([FromBody]string inputKey)
        //{
        //    _methodName = "GetSaudaDetailsForDealer";
        //    return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _reverseAuctionService.GetSaudaDetailsForDealer(x); });
        //}

        //[HttpPost]
        //[Route("SaudaDetails")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSaudaDetailsForBDO", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSaudaDetailsForBDO([FromBody]string inputKey)
        //{
        //    _methodName = "GetSaudaDetailsForBDO";
        //    return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _reverseAuctionService.GetSaudaDetailsForBDO(x); });
        //}


        //[HttpPost]
        //[Route("SaudaDetailsRANew")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSaudaDetailsRANew", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSaudaDetailsRANew([FromBody] string inputKey)
        //{
        //    _methodName = "GetSaudaDetailsRANew";
        //    return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _reverseAuctionService.GetSaudaDetailsRANew(x); });
        //}

        //[HttpPost]
        //[Route("SkusWithDealerAsList")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSkusWithDealerAsList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSkusWithDealerAsList([FromBody] string inputKey)
        //{
        //    _methodName = "GetSkusWithDealerAsList";
        //    return Result(inputKey, _methodName, (SkuwithDealerFilterInputDto x) => { return _reverseAuctionService.GetSkusWithDealerAsList(x); });
        //}
        //#endregion

    }
}
