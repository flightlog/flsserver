using System;

namespace FLS.Data.WebApi.Aircraft
{
    public class AircraftOverviewSearchFilter
    {
        public string ManufacturerName { get; set; }

        public string AircraftModel { get; set; }

        public string CompetitionSign { get; set; }

        public string Immatriculation { get; set; }

        public string AircraftTypeName { get; set; }

        public bool? HasEngine { get; set; }

        public bool? IsTowingAircraft { get; set; }

        public bool? IsTowingOrWinchRequired { get; set; }

        public bool? IsTowingstartAllowed { get; set; }

        public bool? IsWinchstartAllowed { get; set; }

        public string NrOfSeats { get; set; }

        public int? CurrentAircraftState { get; set; }

        public string AircraftOwnerName { get; set; }
    }
}
