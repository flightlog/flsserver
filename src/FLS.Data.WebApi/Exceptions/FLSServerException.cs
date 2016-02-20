using System;
using System.Collections.Generic;

namespace FLS.Data.WebApi.Exceptions
{
    public class FLSServerException : Exception
    {
        public Dictionary<string, string> Parameters { get; set; }
        public string MessageKey { get; set; }

        public FLSServerException(string message, string messageKey, Dictionary<string, string> parameters)
            : base(message)
        {
            Parameters = parameters;
            MessageKey = messageKey;
        }

        public FLSServerException(string message, string messageKey, Dictionary<string, string> parameters, Exception innerException)
            : base(message, innerException)
        {
            Parameters = parameters;
            MessageKey = messageKey;
        }

        public FLSServerException(string message, Exception innerException)
            : base(message, innerException)
        {
            Parameters = new Dictionary<string, string>();
            MessageKey = string.Empty;
        }

        public FLSServerException(string message)
            : base(message)
        {
            Parameters = new Dictionary<string, string>();
            MessageKey = string.Empty;
        }
    }
}
