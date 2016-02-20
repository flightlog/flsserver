
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;

namespace FLS.Server.WebApi
{
    /// <summary>
    /// http://michael-mckenna.com/blog/dependency-injection-for-asp-net-web-api-action-filters-in-3-easy-steps
    /// </summary>
    public class UnityFilterProvider : ActionDescriptorFilterProvider, IFilterProvider
    {
        private IUnityContainer _container;
        private readonly ActionDescriptorFilterProvider _defaultProvider = new ActionDescriptorFilterProvider();

        public UnityFilterProvider(IUnityContainer container)
        {
            _container = container;
        }

        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            var attributes = _defaultProvider.GetFilters(configuration, actionDescriptor);

            foreach (var attr in attributes)
            {
                _container.BuildUp(attr.Instance.GetType(), attr.Instance);
            }
            return attributes;
        }
    }

}