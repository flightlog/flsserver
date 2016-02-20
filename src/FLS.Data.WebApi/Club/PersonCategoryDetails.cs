using System;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.Club
{
    public class PersonCategoryDetails : FLSBaseData
    {

        public Guid PersonCategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }
        
        public Nullable<Guid> ParentPersonCategoryId { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        public override Guid Id
        {
            get { return PersonCategoryId; }
            set { PersonCategoryId = value; }
        }
    }
}
