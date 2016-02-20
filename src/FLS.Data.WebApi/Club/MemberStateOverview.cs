using System;

namespace FLS.Data.WebApi.Club
{
    public class MemberStateOverview : FLSBaseData
    {
        
        public Guid MemberStateId { get; set; }

        public string MemberStateName { get; set; }

        public override Guid Id
        {
            get { return MemberStateId; }
            set { MemberStateId = value; }
        }
    }
}
