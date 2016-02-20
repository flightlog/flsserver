using FLS.Data.WebApi.Exceptions;

namespace FLS.Server.Data.Exceptions
{
    public class LockedFlightException : BadRequestException
    {
        public LockedFlightException(string message) : base(message)
        {
        }
    }
}
