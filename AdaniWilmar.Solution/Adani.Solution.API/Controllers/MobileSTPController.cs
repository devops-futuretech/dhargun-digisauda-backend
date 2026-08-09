using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Adani.Solution.API.App_Start;
using Adani.Solution.DTO;
using Adani.Solution.Service;
using GMCore.Authenticate;
using GMCore.Logger;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/mobileSTP")]
    public class MobileSTPController : BaseApiController
    {
        private const string ServiceName = "Mobile STP Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IMobileSTPService _stpService;
        private string _methodName;

        public MobileSTPController(IMobileSTPService stpService) : base(ServiceName)
        {
            _methodName = "Mobile STP Controller";
            try
            {
                _stpService = stpService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        #region MTP Mobile

        [HttpPost]
        [Route("MTP/currentOrUpcomingmonth")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetMTPDetailsForCurrentOrUpcomingMonth", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetMTPDetailsForCurrentOrUpcomingMonth([FromBody]string inputKey)
        {
            _methodName = "GetMTPDetailsForCurrentOrUpcomingMonth";
            return Result(inputKey, _methodName, (MTPDateWiseDetailsInputDto x) => { return _stpService.GetMTPDetailsForCurrentOrUpcomingMonth(x); });
        }

        [HttpPost]
        [Route("MTP/saveUpcoming")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveUpcomingMonthlyTourPlan", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveUpcomingMonthlyTourPlan([FromBody]string inputKey)
        {
            _methodName = "SaveUpcomingMonthlyTourPlan";
            return Result(inputKey, _methodName, (MTPInputDto x) => { return _stpService.SaveUpcomingMonthlyTourPlan(x); });
        }

        [HttpPost]
        [Route("MTP/novisit/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "NoVisitByUserPermanentJourneyPlan", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult NoVisitByUserPermanentJourneyPlan([FromBody]string inputKey)
        {
            _methodName = "NoVisitByUserPermanentJourneyPlan";
            return Result(inputKey, _methodName, (PJPIdDto x) => { return _stpService.NoVisitByUserPermanentJourneyPlan(x); });
        }

        [HttpPost]
        [Route("MTP/novisit/remarks")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveMTPNoVisitRemarks", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveMTPNoVisitRemarks([FromBody]string inputKey)
        {
            _methodName = "SaveMTPNoVisitRemarks";
            return Result(inputKey, _methodName, (MonthlyTourPlanUpdateDto x) => { return _stpService.SaveMTPNoVisitRemarks(x); });
        }

        #endregion

        [HttpPost]
        [Route("PJP/TotalPCPByUsers")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetTotalPCPByUsers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetTotalPCPByUsers([FromBody]string inputKey)
        {
            _methodName = "GetTotalPCPByUsers";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _stpService.GetTotalPCPByUsers(x); });
        }

        [HttpPost]
        [Route("TodayActivities/Add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddDealerVisit", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddDealerVisit([FromBody]string inputKey)
        {
            _methodName = "AddDealerVisit";
            return Result(inputKey, _methodName, (AddDealerVisitDto x) => { return _stpService.AddDealerVisit(x); });
        }
        [HttpPost]
        [Route("MTPDeviation/ApprovedMonthlyPlanDeviation")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ApprovedMonthlyPlanDeviationForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ApprovedMonthlyPlanDeviationForMobile([FromBody]string inputKey)
        {
            _methodName = "ApprovedMonthlyPlanDeviationForMobile";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _stpService.ApprovedMonthlyPlanDeviationForMobile(x); });
        }

        [HttpPost]
        [Route("MTPDeviation/PendingMonthlyPlanDeviation")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "PendingMonthlyPlanDeviationForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult PendingMonthlyPlanDeviationForMobile([FromBody]string inputKey)
        {
            _methodName = "ApprovedMonthlyPlanDeviationForMobile";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _stpService.PendingMonthlyPlanDeviationForMobile(x); });
        }
        [HttpPost]
        [Route("MTPDeviation/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddMonthlyPlanDeviation", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddMonthlyPlanDeviation([FromBody]string inputKey)
        {
            _methodName = "ApprovedMonthlyPlanDeviationForMobile";
            return Result(inputKey, _methodName, (MonthlyPlanDeviationListDto x) => { return _stpService.AddMonthlyPlanDeviation(x); });
        }
        [HttpPost]
        [Route("SalesTourPlanChart")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SalesTourPlanChart", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SalesTourPlanChart([FromBody]string inputKey)
        {
            _methodName = "SalesTourPlanChart";
            return Result(inputKey, _methodName, (SalesTourPlanInputDto x) => { return _stpService.SalesTourPlanChart(x); });
        }

        [HttpPost]
        [Route("WholeSellerVisit/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddWholeSellerVisit", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddWholeSellerVisit([FromBody]string inputKey)
        {
            _methodName = "AddWholeSellerVisit";
            return Result(inputKey, _methodName, (AddWholeSellerVisitDto x) => { return _stpService.AddWholeSellerVisit(x); });
        }
        [HttpPost]
        [Route("WholeSellerVisit/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetWholeSellerList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetWholeSellerList([FromBody]string inputKey)
        {
            _methodName = "AddWholeSellerVisit";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _stpService.GetWholeSellerList(x); });
        }

        [HttpPost]
        [Route("SecondarySalesFortheDay/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSecondarySalesFortheDay", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSecondarySalesFortheDay([FromBody]string inputKey)
        {
            _methodName = "GetSecondarySalesFortheDay";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _stpService.GetSecondarySalesFortheDay(x); });
        }
        [HttpPost]
        [Route("SecondarySalesFortheDay/detail")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSecondarySalesDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSecondarySalesDetails([FromBody]string inputKey)
        {
            _methodName = "GetSecondarySalesDetails";
            return Result(inputKey, _methodName, (WholesellerSecondarySalesInputDto x) => { return _stpService.GetSecondarySalesDetails(x); });
        }
        #region Holiday

        [HttpPost]
        [Route("holiday/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetHolidayList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetHolidayList([FromBody]string inputKey)
        {
            _methodName = "GetHolidayList";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _stpService.GetHolidayList(x); });
        }

        #endregion

    }
}
