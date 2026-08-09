using Adani.Solution.API.Infrastructure;
using GMCore.Authenticate;
using Newtonsoft.Json;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Adani.Solution.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            //JSON and XML Serialization in Web API
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API routes    
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            if(SAPConstants.IsTokenValidationAllowed)
            {
               config.MessageHandlers.Add(new TokenValidationHandler());
            }

            config.Filters.Add(new AuthorizeUserAttribute());
            config.Filters.Add(new CustomExceptionAttribute());
            config.Filters.Add(new ThrottleAttribute());

            // Swagger configuration
            config.EnableSwagger(swagger =>
            {
                swagger.SingleApiVersion("2018-06-07", "Adani Solution");
                swagger.DocumentFilter<TrailingSlashDocumentFilter>();
            }).EnableSwaggerUi();
        }

        // TrailingSlashDocumentFilter
        private class TrailingSlashDocumentFilter : IDocumentFilter
        {
            public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
            {
                swaggerDoc.paths = swaggerDoc.paths.ToDictionary(
                    kvp => kvp.Key + "/",
                    kvp => kvp.Value
                );
            }
        }
    }
}
