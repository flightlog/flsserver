using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.Person
{
    public class ClubRelatedPersonFullDetails : ClubRelatedPersonDetails
    {
        public ClubRelatedPersonFullDetails(ClubRelatedPersonDetails clubRelatedPersonDetails)
            : base(clubRelatedPersonDetails)
        {
            
        }

        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedByUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedByUserId { get; set; }
        public int? RecordState { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedByUserId { get; set; }
        public Guid OwnerId { get; set; }
        public int OwnershipType { get; set; }
    }
}
