using System;

namespace FLS.Data.WebApi.Person
{
    public class PersonListItem 
    {
        public Guid PersonId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string City { get; set; }
    }
}
