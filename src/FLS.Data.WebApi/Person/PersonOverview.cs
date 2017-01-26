﻿using System;

namespace FLS.Data.WebApi.Person
{
    public class PersonOverview : FLSBaseData
    {
        public Guid PersonId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string AddressLine { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string CountryName { get; set; }

        public string PrivateEmail { get; set; }

        public string MobilePhoneNumber { get; set; }

        public string MemberStateName { get; set; }

        public bool IsActive { get; set; }

        public override Guid Id
        {
            get { return PersonId; }
            set { PersonId = value; }
        }
    }
}
