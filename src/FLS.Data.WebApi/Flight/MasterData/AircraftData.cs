using System;

namespace FLS.Data.WebApi.Flight.MasterData
{
    public class AircraftData
    {
        public Guid AircraftId { get; set; }

        public string CompetitionSign { get; set; }

        public string Immatriculation { get; set; }

        public int AircraftType { get; set; }

        public bool HasEngine { get; internal set; }

        public bool IsTowingOrWinchRequired { get; set; }

        public bool IsTowingstartAllowed { get; set; }

        public bool IsWinchstartAllowed { get; set; }

        public Nullable<int> NrOfSeats { get; set; }

        public int CurrentAircraftState { get; internal set; }
    }
}
