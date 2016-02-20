using System.Web.Http;
using Microsoft.Practices.Unity.WebApi;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(FLS.Server.WebApi.UnityWebApiActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(FLS.Server.WebApi.UnityWebApiActivator), "Shutdown")]

namespace FLS.Server.WebApi
{
    /// <summary>Provides the bootstrapping for integrating Unity with WebApi when it is hosted in ASP.NET</summary>
    public static class UnityWebApiActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start()
        {
            // Use UnityHierarchicalDependencyResolver if you want to use a new child container for each IHttpController resolution.
            // var resolver = new UnityHierarchicalDependencyResolver(UnityConfig.GetConfiguredContainer());
            var resolver = new UnityDependencyResolver(UnityConfig.GetConfiguredContainer());

            //var oldProvider = FilterProviders.Providers.Single(f => f is FilterAttributeFilterProvider);
            //FilterProviders.Providers.Remove(oldProvider);

            //var container = new UnityContainer();
            //var provider = new UnityFilterProvider(container);
            //FilterProviders.Providers.Add(provider);
            
            GlobalConfiguration.Configuration.DependencyResolver = resolver;

        }

        /// <summary>Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer();
            container.Dispose();
        }
    }
}
