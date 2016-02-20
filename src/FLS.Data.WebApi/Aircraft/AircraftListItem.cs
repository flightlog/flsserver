using System;

namespace FLS.Data.WebApi.Aircraft
{
    public class AircraftListItem 
    {
        /// <summary>
        /// Gets the Id of the object. The Id is set by the server.
        /// </summary>
        public Guid AircraftId { get; set; }

        public string CompetitionSign { get; set; }

        public string Immatriculation { get; set; }

        public int AircraftType { get; set; }

        public bool HasEngine { get; set; }

        public bool IsTowingOrWinchRequired { get; set; }

        public bool IsTowingstartAllowed { get; set; }

        public bool IsWinchstartAllowed { get;  set; }

        public Nullable<int> NrOfSeats { get; set; }

        public int CurrentAircraftState { get; set; }
    }
}
