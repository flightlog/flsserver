using FLS.Data.WebApi.Exceptions;

namespace FLS.Server.Data.Exceptions
{
    public class BadRequestException : FLSServerException
    {
        public BadRequestException(string message)
            : base(message)
        {
        }
    }
}
