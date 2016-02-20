using System;

namespace FLS.Data.WebApi.User
{
    public class RoleOverview : FLSBaseData
    {

        public Guid RoleId { get; set; }
        
        //[StringLength(100, ErrorMessage = ConstantUIStrings.MaxLength100)]
        //[Display(Name = ConstantUIStrings.RoleName)]
        public string RoleName { get; set; }

        //[StringLength(100, ErrorMessage = ConstantUIStrings.MaxLength100)]
        public string RoleApplicationKeyString { get; set; }
        
        public override Guid Id
        {
            get { return RoleId; }
            set { RoleId = value; }
        }
    }
}
