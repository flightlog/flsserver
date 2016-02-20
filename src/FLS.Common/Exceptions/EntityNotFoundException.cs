using System;

namespace FLS.Common.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(String entityName)
            : base(string.Format("{0} not found!", entityName))
        {
        }

        public EntityNotFoundException(String entityName, Guid id)
            : base(string.Format("{0} with Id: {1} not found!", entityName, id))
        {
        }
    }
}
