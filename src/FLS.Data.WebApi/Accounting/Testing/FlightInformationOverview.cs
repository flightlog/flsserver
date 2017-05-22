using System;

namespace FLS.Data.WebApi.Accounting.Testing
{
    public class FlightInformationOverview
    {
        public Guid FlightId { get; set; }

        public DateTime FlightDate { get; set; }

        public string AircraftImmatriculation { get; set; }

        public string FlightTypeName { get; set; }

        public string FlightCrewNames { get; set; }

        public string StartAndLdgLocationNames { get; set; }

        public int FlightDurationInSeconds { get; set; }

        public string TowFlightInformation { get; set; }

    }
}
