using FLS.Data.WebApi.Exceptions;
using System;

namespace FLS.Server.Data.Exceptions
{
    public class InternalServerException : FLSServerException
    {
        public InternalServerException(string message)
            : base(message)
        {
        }

        public InternalServerException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
