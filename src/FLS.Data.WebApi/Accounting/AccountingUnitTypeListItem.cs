using System;

namespace FLS.Data.WebApi.Accounting
{
    public class AccountingUnitTypeListItem 
    {
        /// <summary>
        /// Gets the Id of the object. The Id is set by the server.
        /// </summary>
        public int AccountingUnitTypeId { get; set; }

        public string AccountingUnitTypeName { get; set; }
    }
}
