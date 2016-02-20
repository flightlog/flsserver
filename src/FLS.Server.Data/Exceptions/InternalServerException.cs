using FLS.Data.WebApi.Exceptions;

namespace FLS.Server.Data.Exceptions
{
    public class InternalServerException : FLSServerException
    {
        public InternalServerException(string message)
            : base(message)
        {
        }
    }
}
