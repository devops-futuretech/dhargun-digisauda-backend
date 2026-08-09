using GMCore.Logger;
using System;
using Adani.Solution.Service;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Adani.Solution.DTO;
using GMCore.Authenticate;
using System.Web.Http.Description;
using Adani.Solution.API.App_Start;

namespace Adani.Solution.API.Controllers
{
    [AuditLogFilter]
    [RoutePrefix("api/report")]
    public class ReportController : BaseApiController
    {
        private const string ServiceName = "Report Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IReportService _reportService;
        private string _methodName;

        public ReportController(IReportService reportService) : base(ServiceName)
        {
            _methodName = "API Report Controller";
            try
            {
                _reportService = reportService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        [HttpPost]
        [Route("oilprice")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "OlilPriceReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult OlilPriceReport([FromBody]string inputKey)
        {
            _methodName = "OlilPriceReport";
            return Result(inputKey, _methodName, (OilPriceReportInputDto x) => { return _reportService.OilPriceReport(x); });
        }

        [HttpPost]
        [Route("sauda_booking")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaBookingReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaBookingReport([FromBody]string inputKey)
        {
            _methodName = "GetSaudaBookingReport";
            return Result(inputKey, _methodName, (SaudaReportFilterDto x) => { return _reportService.GetSaudaBookingReport(x); });
        }

        //[HttpPost]
        //[Route("counterbid")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetCounterBidOfferReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetCounterBidOfferReport([FromBody]string inputKey)
        //{
        //    _methodName = "GetCounterBidOfferReport";
        //    return Result(inputKey, _methodName, (SaudaReportFilterDto x) => { return _reportService.GetCounterBidOfferReport(x); });
        //}
        [HttpPost]
        [Route("costchangereport")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "CostChangeReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult CostChangeReport([FromBody]string inputKey)
        {
            _methodName = "CostChangeReport";
            return Result(inputKey, _methodName, (ReportInputDto x) => { return _reportService.CostChangeReport(x); });
        }


        [HttpPost]
        [Route("sales")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSalesReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSalesReport([FromBody]string inputKey)
        {
            _methodName = "GetSalesReport";
            return Result(inputKey, _methodName, (SalesReportInputDto x) => { return _reportService.GetSalesReport(x); });
        }

        [HttpPost]
        [Route("saudaorders")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaOrderDetailsReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaOrderDetailsReport([FromBody]string inputKey)
        {
            _methodName = "GetSaudaOrderDetailsReport";
            return Result(inputKey, _methodName, (SaudaOrderReportInputputDto x) => { return _reportService.GetSaudaOrderDetailsReport(x); });
        }

        [HttpPost]
        [Route("distributorstock")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDistributorStockReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDistributorStockReport([FromBody]string inputKey)
        {
            _methodName = "GetDistributorStockReport";
            return Result(inputKey, _methodName, (DistributorStockReportInputDto x) => { return _reportService.GetDistributorStockReport(x); });
        }

        [HttpPost]
        [Route("saudalimit")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCustomerSaudaLimitReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCustomerSaudaLimitReport([FromBody]string inputKey)
        {
            _methodName = "GetCustomerSaudaLimitReport";
            return Result(inputKey, _methodName, (ReportFilterDto x) => { return _reportService.GetCustomerSaudaLimitReport(x); });
        }

        [HttpPost]
        [Route("creditlimit")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCreditLimitReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCreditLimitReport([FromBody] string inputKey)
        {
            _methodName = "GetCreditLimitReport";
            return KendoGridResult(inputKey, _methodName, (ReportFilterDto x) => { return _reportService.GetCreditLimitReport(x); });
        }

        [HttpPost]
        [Route("sales/bdowise")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBDOWiseSalesReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBDOWiseSalesReport([FromBody]string inputKey)
        {
            _methodName = "GetBDOWiseSalesReport";
            return Result(inputKey, _methodName, (SalesReportInputDto x) => { return _reportService.GetBDOWiseSalesReport(x); });
        }

        [HttpPost]
        [Route("sauda/bdowise")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBDOWiseSaudaReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBDOWiseSaudaReport([FromBody]string inputKey)
        {
            _methodName = "GetBDOWiseSaudaReport";
            return Result(inputKey, _methodName, (SaudaOrderReportInputputDto x) => { return _reportService.GetBDOWiseSaudaReport(x); });
        }

        [HttpPost]
        [Route("indentlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "IndentListReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult IndentListReport([FromBody]string inputKey)
        {
            _methodName = "IndentListReport";
            return Result(inputKey, _methodName, (IndentReportInputDto x) => { return _reportService.IndentListReport(x); });
        }

        [HttpPost]
        [Route("MTP")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetMTPDetailsReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetMTPDetailsReport([FromBody]string inputKey)
        {
            _methodName = "GetMTPDetailsReport";
            return Result(inputKey, _methodName, (MonthlyTourPlanReportInputDto x) => { return _reportService.GetMTPDetailsReport(x); });
        }

        [HttpPost]
        [Route("PCP")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPCPDetailsReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPCPDetailsReport([FromBody]string inputKey)
        {
            _methodName = "GetPCPDetailsReport";
            return Result(inputKey, _methodName, (PermanentCoveragePlanReportInputDto x) => { return _reportService.GetPCPDetailsReport(x); });
        }

        [HttpPost]
        [Route("PendingSaudaReport")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingSaudaReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingSaudaReport([FromBody]string inputKey)
        {
            _methodName = "GetPendingSaudaReport";
            return Result(inputKey, _methodName, (PendingSaudaReportInput x) => { return _reportService.GetPendingSaudaReport(x); });
        }

        [HttpPost]
        [Route("pendingcontractreport")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingContractExport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingContractExport([FromBody]string inputKey)
        {
            _methodName = "GetPendingContractExport";
            return Result(inputKey, _methodName, (PendingContractReportDto x) => { return _reportService.GetPendingContractReport(x); });
        }

        [HttpPost]
        [Route("getverticalid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetVerticalId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetVerticalId([FromBody]string inputKey)
        {
            _methodName = "GetVerticalId";
            return Result(inputKey, _methodName, (long x) => { return _reportService.GetVerticalId(x); });
        }

        [HttpPost]
        [Route("OilTypesPendingContractReport")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "OilTypesPendingContractReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetOilTypesPendingContractReport([FromBody]string inputKey)
        {
            _methodName = "OilTypesPendingContractReport";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _reportService.GetOilTypesPendingContractReport(x); });
        }
        [HttpPost]
        [Route("PendingContractReportForMobile")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "PendingContractReportForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingContractReportForMobile([FromBody]string inputKey)
        {
            _methodName = "GetPendingContractReportForMobile";
            return Result(inputKey, _methodName, (PendingContractReportInputDto x) => { return _reportService.GetPendingContractReportForMobile(x); });
        }

        [HttpPost]
        [Route("PendingContractReportForManager")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingContractReportForManager", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingContractReportForManager([FromBody]string inputKey)
        {
            _methodName = "GetPendingContractReportForManager";
            return Result(inputKey, _methodName, (PendingContractReportInputDto x) => { return _reportService.GetPendingContractReportForManager(x); });
        }

        [HttpPost]
        [Route("saudacallrecordmapping/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaCallRecordMappingAttachments", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaCallRecordMappingAttachments([FromBody] string inputKey)
        {
            _methodName = "GetSaudaCallRecordMappingAttachments";
            return Result(inputKey, _methodName, (long x) => { return _reportService.GetSaudaCallRecordMappingAttachments(x); });
        }

        //[HttpPost]
        //[Route("dailybooking")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetDailyBookingReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetDailyBookingReport([FromBody] string inputKey)
        //{
        //    _methodName = "GetDailyBookingReport";
        //    return Result(inputKey, _methodName, (SaudaOrderReportInputputDto x) => { return _reportService.GetDailyBookingReport(x); });
        //}       
       
        [HttpPost]
        [Route("PendingContractReportForManagerAPP")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingContractReportForManagerAPP", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingContractReportForManagerAPP([FromBody]string inputKey)
        {
            _methodName = "GetPendingContractReportForManagerAPP";
            return Result(inputKey, _methodName, (PendingContractReportInputDto x) => { return _reportService.GetPendingContractReportForManagerAPP(x); });
        }

        #region Sauda report Mobile

        //[HttpPost]
        //[Route("saudaorders/mobile")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSaudaOrderDetailsReportForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSaudaOrderDetailsReportForMobile([FromBody] string inputKey)
        //{
        //    _methodName = "GetSaudaOrderDetailsReportForMobile";
        //    return Result(inputKey, _methodName, (SaudaOrderReportInputputDto x) => { return _reportService.GetSaudaOrderDetailsReportForMobile(x); });
        //}
        #endregion


        #region SchemeGeographyReport

        [HttpPost]
        [Route("schemegeographyreport")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSchemeGeographyDetailsReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSchemeGeographyDetailsReport([FromBody]string inputKey)
        {
            _methodName = "GetSchemeGeographyDetailsReport";
            return Result(inputKey, _methodName, (SchemeGeographyReportInputputDto x) => { return _reportService.GetSchemeGeographyDetailsReport(x); });
        }

        #endregion

        #region SchemeGeographyReport

        [HttpPost]
        [Route("demandplanbillingreport")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDemandPlanBillingDetailsReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDemandPlanBillingDetailsReport([FromBody]string inputKey)
        {
            _methodName = "GetSchemeGeographyDetailsReport";
            return Result(inputKey, _methodName, (DemandPlanBillingReportInputputDto x) => { return _reportService.GetDemandPlanBillingDetailsReport(x); });
        }

        #endregion

    }
}
