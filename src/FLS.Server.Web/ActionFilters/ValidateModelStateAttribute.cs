using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using NLog;

namespace FLS.Server.WebApi.ActionFilters
{
    /// <summary>
    /// Validate model state attribut
    /// <see>
    ///     <cref>http://stackoverflow.com/questions/10732644/best-practice-to-return-errors-in-asp-net-web-api/22163675#22163675</cref>
    /// </see>
    /// </summary>
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //http://stackoverflow.com/questions/12891115/skip-filter-on-particular-action-when-action-filter-is-registered-globally
            if (actionContext.ActionDescriptor.GetCustomAttributes<DontValidateModelStateAttribute>().Any())
            {
                // The controller action is decorated with the [DontValidate]
                // custom attribute => don't do anything.
                
                return;
            }

            var modelState = actionContext.ModelState;
            
            if (modelState.IsValid == false)
            {
                //code part below can be useful for debugging the model state in details,
                //but the client receives all these error messages in the response message anyway
                //logging bad requests could result in bad performance while writing log data to database
                //try
                //{
                //    var logger = LogManager.GetCurrentClassLogger();
                //    var errors = string.Empty;

                //    foreach (var state in modelState.Values)
                //    {
                //        foreach (var error in state.Errors)
                //        {
                //            errors += $"{error.ErrorMessage}, ";
                //        }
                //    }
                    
                //    logger.Warn($"ModelState is invalid. Error(s): {errors}");
                //}
                //catch (Exception)
                //{
                //}
                
                actionContext.Response = actionContext.Request
                     .CreateErrorResponse(HttpStatusCode.BadRequest, modelState);
            }

            base.OnActionExecuting(actionContext);
        }
    }
}