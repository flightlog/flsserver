using System;
using System.Collections.Generic;

namespace FLS.Data.WebApi.Exceptions
{
    public class FLSServerException : Exception
    {
        public Dictionary<string, string> Parameters { get; set; }

        public FLSServerException(string message, Dictionary<string, string> parameters)
            : base(message)
        {
            Parameters = parameters;
        }

        public FLSServerException(string message,  Dictionary<string, string> parameters, Exception innerException)
            : base(message, innerException)
        {
            Parameters = parameters;
        }

        public FLSServerException(string message, Exception innerException)
            : base(message, innerException)
        {
            Parameters = new Dictionary<string, string>();
        }

        public FLSServerException(string message)
            : base(message)
        {
            Parameters = new Dictionary<string, string>();
        }
    }
}
