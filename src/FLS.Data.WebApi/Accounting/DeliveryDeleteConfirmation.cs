using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.Accounting
{
    public class DeliveryDeleteConfirmation
    {
        public List<Guid> DeliveryIds { get; set; }

        public bool ConfirmDeletion { get; set; }
    }
}
