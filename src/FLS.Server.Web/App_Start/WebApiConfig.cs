using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dependencies;
using System.Web.Http.Filters;
using System.Web.Mvc;
using FLS.Common.Converters;
using FLS.Server.WebApi.ActionFilters;
using FLS.Server.WebApi.Handlers;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity.WebApi;
using Newtonsoft.Json;
using IFilterProvider = System.Web.Http.Filters.IFilterProvider;

namespace FLS.Server.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //http://www.asp.net/web-api/overview/security/enabling-cross-origin-requests-in-web-api
            config.EnableCors(new EnableCorsAttribute(origins: "*", headers: "*", methods: "*")); //allow CORS globally, disable CORS on controllers and controller methods if required

            var handler = config.DependencyResolver.GetService(typeof(MethodOverrideHandler));
            config.MessageHandlers.Add((DelegatingHandler)handler);

            //Register the filter injector
            var providers = config.Services.GetFilterProviders().ToList();

            var defaultprovider = providers.Single(i => i is ActionDescriptorFilterProvider);
            config.Services.Remove(typeof(IFilterProvider), defaultprovider);
            var unityFilterProvider = config.DependencyResolver.GetService(typeof(UnityFilterProvider));
            //var unityFilterProvider = new UnityFilterProvider(UnityConfig.GetConfiguredContainer());
            config.Services.Add(typeof(IFilterProvider), unityFilterProvider);


            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            // see also: http://blogs.msdn.com/b/webdev/archive/2013/09/20/understanding-security-features-in-spa-template.aspx
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType)); //OAuthDefaults.AuthenticationType = "Bearer"


            //additional filters for FLS
            config.Filters.Add((IFilter)config.DependencyResolver.GetService(typeof(UnhandledExceptionFilterAttribute))); //new UnhandledExceptionFilterAttribute());
            config.Filters.Add((IFilter)config.DependencyResolver.GetService(typeof(UserInitActionFilter)));
            config.Filters.Add((IFilter)config.DependencyResolver.GetService(typeof(ValidateModelStateAttribute))); //new ValidateModelStateAttribute());
            config.Filters.Add((IFilter)config.DependencyResolver.GetService(typeof(NoCacheActionFilter)));

            //CheckModelForNullAttribute does not work with Unity
            //var filter = config.DependencyResolver.GetService(typeof (CheckModelForNullAttribute));
            config.Filters.Add(new CheckModelForNullAttribute());


            //set Json format as default, but if i want to use the XML format. I'll just append the xml=true.
            //http://stackoverflow.com/questions/9847564/how-do-i-get-asp-net-web-api-to-return-json-instead-of-xml-using-chrome
            config.Formatters.XmlFormatter.MediaTypeMappings.Add(new QueryStringMapping("xml", "true", "application/xml"));
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            config.Formatters.JsonFormatter.SerializerSettings =
            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

            //http://stackoverflow.com/questions/23282514/web-api-not-converting-json-empty-strings-values-to-null
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new EmptyStringToNullJsonConverter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            //default routes are not required as we implemented attribute routing on controllers
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}
