using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Adani.Solution.MVC.Controllers;
using GMCore.Logger;

namespace Adani.Solution.MVC
{
    public class HandleCustomError: HandleErrorAttribute
    {
        private readonly ILogger _logger = Logging.GetLogger("HandleCustomError");

        public override void OnException(ExceptionContext filterContext)
        {
            Exception exception = null;
            var errorDetails = string.Empty;

            if (filterContext.Exception != null)
            {
                _logger.Error("HandleCustomError", filterContext.Exception);
                errorDetails = LogExceptionWhileHandlingError.BuildErrorDetails(filterContext);
                _logger.Error(errorDetails);

                exception = filterContext.Exception;

                _logger.Error(exception.Message);
                _logger.Error(exception.InnerException);
                _logger.Error(exception.Source);
                _logger.Error(exception.StackTrace);
            }

            //If the exeption is already handled we do nothing
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            //Make sure that we mark the exception as handled and
            //Advise subsequent exception filters not to interfere and stop asp.net from showing yellow screen of death
            filterContext.ExceptionHandled = true;

            //Create route data in order to redirect to custom exception view
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Shared");
            routeData.Values.Add("action", "Error");

            // Pass exception details to the target error View.
            if (exception != null)
            {
                routeData.Values.Add("ErrorDetails", errorDetails);
                routeData.Values.Add("ErrorException", exception.Message);
                routeData.Values.Add("InnerException", exception.InnerException);
                routeData.Values.Add("SourceException", exception.Source);
                routeData.Values.Add("TargetSiteException", exception.TargetSite);
                routeData.Values.Add("StackTraceException", exception.StackTrace);
            }

            // Call target Controller and pass the routeData.
            IController errorController = new SharedController();
            errorController.Execute(new RequestContext(
                new HttpContextWrapper(HttpContext.Current), routeData));
        }
    }

    public static class LogExceptionWhileHandlingError
    {
        /// <summary>
        /// Method to get the Exception Area, Controller and Action
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        public static string BuildErrorDetails(ExceptionContext filterContext)
        {
            var controller = Convert.ToString(filterContext.RouteData.Values["controller"]);
            var action = Convert.ToString(filterContext.RouteData.Values["action"]);
            var originArea = String.Empty;
            if (filterContext.RouteData.DataTokens.ContainsKey("area"))
                originArea = Convert.ToString(filterContext.RouteData.DataTokens["area"]);
            var loggerName = $"Controller- {controller}, Action- {action}, OriginArea-{originArea}";
            return loggerName;
        }

    }
}