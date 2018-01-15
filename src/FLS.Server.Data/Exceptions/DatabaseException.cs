using FLS.Data.WebApi.Exceptions;
using System;

namespace FLS.Server.Data.Exceptions
{
    public class DatabaseException : FLSServerException
    {
        public DatabaseException(string message)
            : base(message)
        {
        }

        public DatabaseException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}
