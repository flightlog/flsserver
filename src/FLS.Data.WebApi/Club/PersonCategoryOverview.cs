using System;

namespace FLS.Data.WebApi.Club
{
    public class PersonCategoryOverview : FLSBaseData
    {

        public Guid PersonCategoryId { get; set; }

        public string CategoryName { get; set; }
        
        public Nullable<Guid> ParentPersonCategoryId { get; set; }

        public override Guid Id
        {
            get { return PersonCategoryId; }
            set { PersonCategoryId = value; }
        }
    }
}
