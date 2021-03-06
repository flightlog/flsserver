﻿using System;

namespace FLS.Data.WebApi.Person
{
    public class PersonFullDetails : PersonDetails, IFLSMetaData
    {
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedByUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedByUserId { get; set; }
        public int? RecordState { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedByUserId { get; set; }
        public Guid OwnerId { get; private set; }
        public int OwnershipType { get; private set; }
    }
}
