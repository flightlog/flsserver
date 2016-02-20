using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Web.Http.Filters;
using FLS.Data.WebApi.Exceptions;
using FLS.Server.Data.Exceptions;
using NLog;

namespace FLS.Server.WebApi.ActionFilters
{
    /// <summary>
    /// <see>
    ///     <cref>http://weblog.west-wind.com/posts/2012/Aug/21/An-Introduction-to-ASPNET-Web-API#ErrorHandling</cref>
    /// </see>
    /// </summary>
    public class UnhandledExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var logger = LogManager.GetCurrentClassLogger();

            var status = HttpStatusCode.InternalServerError;

            var exType = context.Exception.GetType();

            if (exType == typeof(UnauthorizedAccessException))
                status = HttpStatusCode.Unauthorized;
            else if (exType == typeof(AuthenticationException))
                status = HttpStatusCode.Unauthorized;
            else if (exType == typeof(BadRequestException) || exType.BaseType == typeof(BadRequestException))
                status = HttpStatusCode.BadRequest;
            else if (exType == typeof(InternalServerException))
                status = HttpStatusCode.InternalServerError;
            else if (exType == typeof(ArgumentException))
                status = HttpStatusCode.NotFound;

            var exception = context.Exception;

            if (context.Exception.InnerException != null)
            {
                exception = context.Exception.InnerException;
            }

            var apiError = new FLSServerException(context.Exception.Message, exception);

            // create a new response and attach our ApiError object
            // which now gets returned on ANY exception result
            var errorResponse = context.Request.CreateResponse<FLSServerException>(status, apiError);
            context.Response = errorResponse;

            logger.Error(string.Format("HttpStatusCode: {0} with error message: {1}, Exception: {2}, InnerException: {3}", status, apiError.Message, context.Exception, context.Exception.InnerException), apiError);

            base.OnException(context);
        }
    }
}