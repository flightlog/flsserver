using System;

namespace FLS.Data.WebApi.Flight.MasterData
{
    public class PersonData
    {
        public Guid PersonId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string City { get; set; }
    }
}
