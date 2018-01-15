using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Web.Http.Filters;
using FLS.Data.WebApi.Exceptions;
using FLS.Server.Data.Exceptions;
using NLog;
using System.Collections.Generic;
using FLS.Server.Data.Resources;

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
            var statusCode = HttpStatusCode.InternalServerError;

            var exType = context.Exception.GetType();
            var returningException = new FLSServerException(context.Exception.Message, context.Exception);

            if (exType == typeof(UnauthorizedAccessException))
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            else if (exType == typeof(AuthenticationException))
                statusCode = HttpStatusCode.Unauthorized;
            else if (exType == typeof(BadRequestException) || exType.BaseType == typeof(BadRequestException))
                statusCode = HttpStatusCode.BadRequest;
            else if (exType == typeof(InvalidCastException))
            {
                returningException = new FLSServerException(ErrorMessage.InvalidCastException);
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (exType == typeof(ArgumentException))
                statusCode = HttpStatusCode.BadRequest;
            else if (exType == typeof(ArgumentOutOfRangeException))
            {
                var ex = context.Exception as ArgumentOutOfRangeException;
                var parameters = new Dictionary<string, string>();
                parameters.Add("ArgumentName", ex.ParamName);
                returningException = new FLSServerException(ErrorMessage.ArgumentOutOfRangeException, parameters, ex);
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (exType == typeof(InternalServerException))
            {
                returningException = new FLSServerException(ErrorMessage.InternalServerException);
            }

            // create a new response and attach our ApiError object
            // which now gets returned on ANY exception result
            context.Response = context.Request.CreateResponse(statusCode, returningException);

            var logger = LogManager.GetCurrentClassLogger();
            logger.Error($"HttpStatusCode: {statusCode} with error message: {returningException.Message}, Exception: {context.Exception}, InnerException: {context.Exception.InnerException}", returningException);
            
            base.OnException(context);
        }
    }
}