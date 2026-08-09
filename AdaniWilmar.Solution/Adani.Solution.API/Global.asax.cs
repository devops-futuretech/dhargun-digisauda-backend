using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using GMCore.Logger;
using Adani.Solution.API.Infrastructure;
using System;
using System.Web.Http;
using System.Web.Routing;
using System.Net;
namespace Adani.Solution.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static IWindsorContainer _container;
        private readonly ILogger _logger = Logging.GetLogger("API Application Error");
        protected void Application_Start()
        {
            try
            {
                GlobalConfiguration.Configure(WebApiConfig.Register);
                GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
                ConfigureWindsor(GlobalConfiguration.Configuration);
                RouteConfig.RegisterRoutes(RouteTable.Routes);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }
            catch (Exception exception)
            {
                _logger.Fatal("API Start Error:", exception);
            }
        }
        private static void ConfigureWindsor(HttpConfiguration configuration)
        {
            _container = new WindsorContainer();
            _container.Install(FromAssembly.This());
            _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel, true));
            var dependencyResolver = new WindsorDependencyResolver(_container);
            configuration.DependencyResolver = dependencyResolver;
        }

        protected void Application_End()
        {
            _container.Dispose();
            Dispose();
        }
    }
}
