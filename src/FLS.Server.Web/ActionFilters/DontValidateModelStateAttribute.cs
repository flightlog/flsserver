using System.Web.Http.Filters;

namespace FLS.Server.WebApi.ActionFilters
{

    /// <summary>
    /// Attribute to decorate an action or a controller to not validate the model
    /// http://stackoverflow.com/questions/12891115/skip-filter-on-particular-action-when-action-filter-is-registered-globally
    /// </summary>
    public class DontValidateModelStateAttribute : ActionFilterAttribute
    {
        
    }
}