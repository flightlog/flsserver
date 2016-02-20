using System;
using System.Collections.Generic;

namespace FLS.Data.WebApi.Flight.MasterData
{
    [Serializable]
    public class FlightTableMasterData
    {
        
        public virtual Guid ClubId { get; internal set; }

        public string ClubName { get; internal set; }

        public Nullable<Guid> HomebaseId { get; internal set; }

        public Nullable<int> DefaultStartType { get; internal set; }

        public Nullable<Guid> DefaultGliderFlightTypeId { get; internal set; }

        public Nullable<Guid> DefaultTowFlightTypeId { get; internal set; }

        public Nullable<Guid> DefaultMotorFlightTypeId { get; internal set; }

        public Nullable<Guid> DefaultCountryId { get; internal set; }

        public List<LocationData> Airfields { get; set; }

        public List<FlightTypeData> GliderFlightTypes { get; set; }

        public List<FlightTypeData> TowingFlightTypes { get; set; }

        public List<FlightTypeData> MotorFlightTypes { get; set; }

        public List<StartTypeData> StartTypes { get; set; }

        public List<AircraftData> Gliders { get; set; }

        public List<AircraftData> TowingAircrafts { get; set; }

        public List<AircraftData> MotorAircrafts { get; set; }

        public List<PersonData> GliderPilots { get; set; }

        public List<PersonData> GliderInstructors { get; set; }

        public List<PersonData> TowingPilots { get; set; }

        public List<PersonData> MotorPilots { get; set; }

        public List<PersonData> WinchOperators { get; set; }

        public List<PersonData> Passengers { get; set; }

        public List<LocationTypeData> LocationTypes { get; set; }

        public List<AircraftTypeData> AircraftTypes { get; set; }

        public List<CountryData> Countries { get; set; }

        public List<FlightStateData> FlightStates { get; set; }

        public bool IsInboundRouteRequired { get; set; }

        public bool IsOutboundRouteRequired { get; set; }
    }
}
