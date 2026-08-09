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
    [RoutePrefix("api/dealer")]
    public class DealerController : BaseApiController
    {
        private const string ServiceName = "Dealer Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IDealerServices _dealerService;
        private string _methodName;

        public DealerController(IDealerServices dealerService)
        {
            _methodName = "Dealer Controller";
            try
            {
                _dealerService = dealerService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }
    }
}
