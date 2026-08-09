using Adani.Solution.API.App_Start;
using Adani.Solution.DTO;
using Adani.Solution.Service;
using GMCore.Authenticate;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/tradeticket")]
    public class TradeTicketController : BaseApiController
    {

        private const string ServiceName = "Trade Ticket Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly ITradeTicketService _tradeTicketService;
        private string _methodName;

        public TradeTicketController(ITradeTicketService tradeTicketService) : base(ServiceName)
        {
            _methodName = "TradeTicket Controller";
            try
            {
                _tradeTicketService = tradeTicketService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        /// <summary>
        /// Method to get the trade ticket list
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("request/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "TradeTicketRequestList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult TradeTicketRequestList([FromBody]string inputKey)
        {
            _methodName = "TradeTicketRequestList";
            return Result(inputKey, _methodName, (TradeTicketParamDto x) => { return _tradeTicketService.TradeTicketRequestList(x); });
        }

        /// <summary>
        /// Method to get the trade ticket request creation
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("request/creation")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "TradeTicketRequestCreation", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult TradeTicketRequestCreation([FromBody]string inputKey)
        {
            _methodName = "TradeTicketRequestCreation";
            return Result(inputKey, _methodName, (TradeTicketInputDto x) => { return _tradeTicketService.TradeTicketRequestCreation(x); });
        }

        /// <summary>
        /// Method to get the trade ticket request modification
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("request/modification")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "TradeTicketRequestModification", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult TradeTicketRequestModification([FromBody]string inputKey)
        {
            _methodName = "TradeTicketRequestModification";
            return Result(inputKey, _methodName, (TradeTicketInputDto x) => { return _tradeTicketService.TradeTicketRequestModification(x); });
        }

        /// <summary>
        /// Method to get the trade ticket request modification
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("request/details")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "TradeTicketRequestDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult TradeTicketRequestDetails([FromBody]string inputKey)
        //{
        //    _methodName = "TradeTicketRequestDetails";
        //    return Result(inputKey, _methodName, (TradeTicketInputDto x) => { return _tradeTicketService.TradeTicketRequestDetails(x); });
        //}

        /// <summary>
        /// Method to get the trade ticket request modification
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("request/delete")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "TradeTicketRequestDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult TradeTicketDelete([FromBody]string inputKey)
        {
            _methodName = "TradeTicketDelete";
            return Result(inputKey, _methodName, (TradeTicketDeleteDto x) => { return _tradeTicketService.TradeTicketDelete(x); });
        }

        /// <summary>
        /// Method to get the trade ticket status list
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("status/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "TradeTickeStatusList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult TradeTickeStatusList([FromBody]string inputKey)
        {
            _methodName = "TradeTickeStatusList";
            return Result(inputKey, _methodName, (TradeTicketStatusSearchDto x) => { return _tradeTicketService.GetTradeTicketStatusList(x); });
        }

        /// <summary>
        /// Method to get the trade ticket status list
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("status/detail")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "MappedTradeTicketSaudaOrders", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult MappedTradeTicketSaudaOrders([FromBody]string inputKey)
        {
            _methodName = "TradeTickeStatusList";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _tradeTicketService.MappedTradeTicketSaudaOrders(x); });
        }

        /// <summary>
        /// Method to get the trade ticket status list
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("status/details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "TradeTickeStatusdetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult TradeTickeStatusdetails([FromBody]string inputKey)
        {
            _methodName = "TradeTickeStatusdetails";
            return Result(inputKey, _methodName, (TradeTicketInputDto x) => { return _tradeTicketService.TradeTickeStatusDetails(x); });
        }

        /// <summary>
        /// Method to get the trade ticket status list
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("dropdown")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "TradeTickeStatusdetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult TradeTicketDropDown([FromBody]string inputKey)  
        {
            _methodName = "TradeTickeStatusdetails";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _tradeTicketService.TradeTicketDropDown(x); });
        }

        /// <summary>
        /// Method to get the trade ticket oiltype list
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("oiltypes")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetTradeTicketOilTypes", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetTradeTicketOilTypes([FromBody]string inputKey)
        {
            _methodName = "GetTradeTicketOilTypes";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _tradeTicketService.GetTradeTicketOilTypesForDropdown(x); });
        }

        /// <summary>
        /// Method to get the trade ticket oiltype list
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("saudaunmapping")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "TradeTicketSaudaUnMapping", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult TradeTicketSaudaUnMapping([FromBody]string inputKey)
        {
            _methodName = "TradeTicketSaudaUnMapping";
            return Result(inputKey, _methodName, (TradeTicketSaudaUnMappingDto x) => { return _tradeTicketService.TradeTicketSaudaUnMapping(x); });
        }

        [HttpPost]
        [Route("dealers/stateid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealersListByStateId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealersListByStateId([FromBody]string inputKey)
        {
            _methodName = "GetDealersListByStateId";
            return Result(inputKey, _methodName, (List<int> x) => { return _tradeTicketService.GetDealersListByStateId(x); });
        }

        
        #region Export Trade Ticket

        [HttpPost]
        [Route("tradeTicketStatusExcelExport")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExcelExportTradeTicketStatus", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExcelExportTradeTicketStatus([FromBody]string inputKey)
        {
            _methodName = "ExcelExportTradeTicketStatus";
            return Result(inputKey, _methodName, (TradeTicketSearchDto x) => { return _tradeTicketService.ExcelExportTradeTicketStatus(x); });
        }

        [HttpPost]
        [Route("allTradeTicketsExcelExport")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportAllTradeTickets", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportAllTradeTickets([FromBody]string inputKey)
        {
            _methodName = "ExportAllTradeTickets";
            return Result(inputKey, _methodName, (TradeTicketSearchDto x) => { return _tradeTicketService.ExportAllTradeTickets(x); });
        }

        #endregion
    }
}
