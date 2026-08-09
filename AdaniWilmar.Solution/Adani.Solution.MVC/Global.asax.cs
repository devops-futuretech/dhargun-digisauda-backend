using Castle.Windsor;
using Castle.Windsor.Installer;
using GMCore.Logger;
using Adani.Solution.MVC.Controllers;
using Adani.Solution.MVC.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Adani.Solution.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {

        private static IWindsorContainer _container;
        private readonly ILogger _logger = Logging.GetLogger("HandleCustomError");

        private static void BootstrapContainer()
        {
            _container = new WindsorContainer().Install(FromAssembly.This());
            var controllerFactory = new WindsorControllerFactory(_container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        protected void Application_Start()
        {

            // Disable X-AspNetMvc-Version header
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //if (!Context.Request.IsSecureConnection)
            //    Response.Redirect(Context.Request.Url.ToString().Replace("http:", "https:"));

            var app = sender as HttpApplication;

            if (app != null && app.Context != null)
            {
                app.Context.Response.Headers.Remove("Server");
            }
        }
        /// <summary>
        /// Handling application level error
        /// </summary>
        protected void Application_Error()
        {
            //Get server exception 
            var exception = Server.GetLastError();

            //Log the exception
            _logger.Fatal("Application Error:", exception);

            //Clearing the existing responses
            Response.Clear();

            //Typecasting the exception to HttpException
            var httpException = exception as HttpException;

            //Creating route data with controller and view names to execute/redirect
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Shared");

            //If not http exception-redirect to exception
            if (httpException == null)
            {
                routeData.Values.Add("action", "Error");
            }
            else
            {
                //Handling Http Exception
                switch (httpException.GetHttpCode())
                {
                    case 400:
                        // Bad request.
                        routeData.Values.Add("action", "HttpError400");
                        break;
                    case 401:
                        // Unauthorized.
                        routeData.Values.Add("action", "HttpError401");
                        break;
                    case 403:
                        // Forbidden.
                        routeData.Values.Add("action", "HttpError403");
                        break;
                    case 404:
                        // Page not found.
                        routeData.Values.Add("action", "HttpError404");
                        break;
                    case 500:
                        // Internal Server error.
                        routeData.Values.Add("action", "HttpError500");
                        break;
                    case 502:
                        // Bad Gateway.
                        routeData.Values.Add("action", "HttpError502");
                        break;
                    case 503:
                        // Service Unavailable.
                        routeData.Values.Add("action", "HttpError503");
                        break;
                    case 504:
                        // Gateway Timeout.
                        routeData.Values.Add("action", "HttpError504");
                        break;
                    default:
                        // Handle Views to other error codes.
                        routeData.Values.Add("action", "Error");
                        break;
                }
            }

            // Pass exception details to the target error View.
            routeData.Values.Add("ErrorException", exception.Message);
            routeData.Values.Add("InnerException", exception.InnerException);
            routeData.Values.Add("SourceException", exception.Source);
            routeData.Values.Add("TargetSiteException", exception.TargetSite);
            routeData.Values.Add("StackTraceException", exception.StackTrace);

            // Clear the error on server.
            Server.ClearError();

            // Avoid IIS7 getting in the middle
            Response.TrySkipIisCustomErrors = true;

            // Call target Controller and pass the routeData.
            IController errorController = new SharedController();
            errorController.Execute(new RequestContext(
                 new HttpContextWrapper(Context), routeData));
        }
    }
}
