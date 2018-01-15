using FLS.Data.WebApi.Exceptions;

namespace FLS.Server.Data.Exceptions
{
    public class DataRecordNotFoundException : FLSServerException
    {
        public DataRecordNotFoundException(string message)
            : base(message)
        {
        }
    }
}
