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
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/sapmanual")]
    public class SapSaudaManualTriggerController : BaseApiController
    {
        private const string ServiceName = "SAPIntegration Controller";
        private readonly ISAPIntegrationService _sapIntegrationService;
        private string _methodName;

        public SapSaudaManualTriggerController(ISAPIntegrationService sapIntegrationService) : base(ServiceName)
        {
            _methodName = "SapSauda Manual Trigger Controller";
            try
            {
                _sapIntegrationService = sapIntegrationService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        #region Sauda manual move to sap

        /// <summary>
        /// Method to Get Sauda details List - manual Trigger
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("sauda/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetSaudaDetails(SAPDataSyncInputDto inputDto)
        {
            _methodName = "GetSaudaDetails";
            var result = new ResultDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                // _sapIntegrationService.GetSaudaDetails();
                // _sapIntegrationService.GetSaudaDetails(inputDto.VerticalId, inputDto.TradeTicketWithOrWithoutId);

            });
            return Ok(result);
        }


        #endregion

    }
}
