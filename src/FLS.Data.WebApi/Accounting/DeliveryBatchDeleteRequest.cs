using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.Accounting
{
    public class DeliveryBatchDeleteRequest
    {
        public int BatchId { get; set; }
    }
}
