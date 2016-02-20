using FLS.Data.WebApi.Exceptions;

namespace FLS.Server.Data.Exceptions
{
    public class DeleteEntityException : BadRequestException
    {
        public DeleteEntityException(string message)
            : base(message)
        {
        }
    }
}
