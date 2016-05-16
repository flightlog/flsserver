using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLS.Server.Data
{
    public class DeletedFLSDataEntities : FLSDataEntities
    {
        public DeletedFLSDataEntities(IIdentityService identityService) : base(identityService)
        {
        }

        protected override void SetIsDeletedMapping(DbModelBuilder modelBuilder, bool isDeleted)
        {
            base.SetIsDeletedMapping(modelBuilder, true);
        }
    }
}
