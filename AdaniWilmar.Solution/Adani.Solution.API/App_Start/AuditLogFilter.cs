using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;

namespace Adani.Solution.API.App_Start
{
    public class AuditLogFilter: System.Web.Http.Filters.ActionFilterAttribute
    {
        private const string ServiceName = "My Attribute";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        public static bool IsAPIURL => Convert.ToBoolean(ConfigurationManager.AppSettings["IsAPIURL"]);
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (IsAPIURL)
            {
                var message = $"{ServiceName} API URL : {actionContext.Request.RequestUri}";
                _logger.Info(message);
            }           
        }
    }
}