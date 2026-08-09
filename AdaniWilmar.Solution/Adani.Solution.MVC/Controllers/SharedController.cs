using Adani.Solution.MVC.Helpers;
using GMCore.Logger;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Adani.Solution.MVC.Controllers
{
    [NoCache]
    public class SharedController : Controller
    {
        private readonly ILogger _logger = Logging.GetLogger("Shared error handling controller");

        /// <summary>
        /// Action result invoked during Application Error
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            _logger.Error("Common Error");
            LoadViewBagWithErrorDetails();
            //ClearData();
            return View();
        }

        /// <summary>
        /// Action result invoked during 400 Error
        /// Bad request 400
        /// The request had bad syntax or was inherently impossible to be satisfied.
        /// </summary>
        /// <returns></returns>
        public ActionResult HttpError400()
        {
            _logger.Error("HttpError400");
            LoadViewBagWithErrorDetails();
            ViewBag.BadRequest = "True";
            //ClearData();
            return View("Error");
        }

        /// <summary>
        /// Action result invoked during 401 Error
        /// Unauthorized 401
        /// The parameter to this message gives a specification of authorization schemes which are acceptable. 
        /// The client should retry the request with a suitable Authorization header.
        /// </summary>
        /// <returns></returns>
        public ActionResult HttpError401()
        {
            _logger.Error("HttpError401");
            LoadViewBagWithErrorDetails();
            ViewBag.Unauthorized = "True";
            //ClearData();
            return View("Error");
        }

        /// <summary>
        /// Action result invoked during 403 Error
        /// Forbidden 403
        /// The request is for something forbidden. Authorization will not help.
        /// </summary>
        /// <returns></returns>
        public ActionResult HttpError403()
        {
            _logger.Error("HttpError403");
            LoadViewBagWithErrorDetails();
            ViewBag.Forbidden = "True";
            //ClearData();
            return View("Error");
        }

        /// <summary>
        /// Action result invoked during 404 Error
        /// Not found 404
        /// The server has not found anything matching the URI given
        /// </summary>
        /// <returns></returns>
        public ActionResult HttpError404()
        {
            _logger.Error("HttpError404");
            LoadViewBagWithErrorDetails();
            ViewBag.NotFound = "True";
            //ClearData();
            return View("Error");
        }

        /// <summary>
        /// Action result invoked during 500 Error
        /// Internal Error 500
        /// The server encountered an unexpected condition which prevented it from fulfilling the request.
        /// </summary>
        /// <returns></returns>
        public ActionResult HttpError500()
        {
            _logger.Error("HttpError500");
            LoadViewBagWithErrorDetails();
            ViewBag.InternalServerError = "True";
            //ClearData();
            return View("Error");
        }

        /// <summary>
        /// Action result invoked during 502 Error
        /// 502 Bad Gateway
        /// The 502 status code, or Bad Gateway error, means that the server is a gateway or proxy server, 
        /// and it is not receiving a valid response from the backend servers that should actually fulfill the request.
        /// </summary>
        /// <returns></returns>
        public ActionResult HttpError502()
        {
            _logger.Error("HttpError502");
            LoadViewBagWithErrorDetails();
            ViewBag.BadGateway = "True";
            //ClearData();
            return View("Error");
        }

        /// <summary>
        /// Action result invoked during 503 Error
        /// 503 Service Unavailable
        /// The 503 status code, or Service Unavailable error, means that the server is overloaded or under maintenance. 
        /// This error implies that the service should become available at some point.
        /// </summary>
        /// <returns></returns>
        public ActionResult HttpError503()
        {
            _logger.Error("HttpError503");
            LoadViewBagWithErrorDetails();
            ViewBag.ServiceUnavailable = "True";
            //ClearData();
            return View("Error");
        }

        /// <summary>
        /// Action result invoked during 504 Error
        /// 504 Gateway Timeout
        /// The 504 status code, or Gateway Timeout error, means that the server is a gateway or proxy server, 
        /// and it is not receiving a response from the backend servers within the allowed time period.
        /// </summary>
        /// <returns></returns>
        public ActionResult HttpError504()
        {
            _logger.Error("HttpError504");
            LoadViewBagWithErrorDetails();
            ViewBag.GatewayTimeout = "True";
            //ClearData();
            return View("Error");
        }

        /// <summary>
        /// Action result invoked during 539 
        /// </summary>
        /// <returns></returns>
        public ActionResult Lockout()
        {
            _logger.Error("Lockout");
            //ClearData();
            return View();
        }

        /// <summary>
        /// Method to read the RouteData values and saving to ViewBag
        /// </summary>
        private void LoadViewBagWithErrorDetails()
        {
            ViewBag.ErrorDetails = GetErrorMessageFrom("ErrorDetails");
            _logger.Error(ViewBag.ErrorDetails);

            ViewBag.ErrorException = GetErrorMessageFrom("ErrorException");
            _logger.Error(ViewBag.ErrorException);

            ViewBag.InnerException = GetErrorMessageFrom("InnerException");
            _logger.Error(ViewBag.InnerException);

            ViewBag.SourceException = GetErrorMessageFrom("SourceException");
            _logger.Error(ViewBag.SourceException);

            ViewBag.TargetSiteException = GetErrorMessageFrom("TargetSiteException");
            _logger.Error(ViewBag.TargetSiteException);

            ViewBag.StackTraceException = GetErrorMessageFrom("StackTraceException");
            _logger.Error(ViewBag.StackTraceException);

            //ClearData();

        }

        /// <summary>
        /// Method to do null check route data object and get error message
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetErrorMessageFrom(string key)
        {
            //Declare variables to hold route data
            var routeData = string.Empty;

            //Route data-Check for null. If not null assign to the declared variable
            if (RouteData.Values[key] != null)
            {
                routeData = Convert.ToString(RouteData.Values[key]);
            }

            //Return route data else return empty
            if (!string.IsNullOrEmpty(routeData)) return routeData;
            return string.Empty;
        }

        private void ClearData()
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            FormsAuthentication.SignOut();
        }
    }
}