using System;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.Club
{
    public class MemberStateDetails : FLSBaseData
    {

        public Guid MemberStateId { get; set; }

        [Required]
        [StringLength(50)]
        public string MemberStateName { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        public override Guid Id
        {
            get { return MemberStateId; }
            set { MemberStateId = value; }
        }
    }
}
