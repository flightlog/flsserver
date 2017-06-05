using System;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.Location
{
    public class InOutboundPointDetails : FLSBaseData
    {

        public Guid InOutboundPointId { get; set; }

        [Required]
        [GuidNotEmptyValidator]
        public Guid LocationId { get; set; }

        [StringLength(50)]
        public string InOutboundPointName { get; set; }

        public bool IsInboundPoint { get; set; }

        public bool IsOutboundPoint { get; set; }

        public override Guid Id
        {
            get { return LocationId; }
            set { LocationId = value; }
        }
    }
}
