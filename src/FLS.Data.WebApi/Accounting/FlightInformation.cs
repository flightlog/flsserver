using System;

namespace FLS.Data.WebApi.Accounting
{
    public class FlightInformation
    {
        public Guid FlightId { get; set; }

        public DateTime FlightDate { get; set; }

        public string AircraftImmatriculation { get; set; }

        public string FlightTypeName { get; set; }
    }
}
