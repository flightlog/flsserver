using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;
using FLS.Data.WebApi.Accounting;
using FLS.Data.WebApi.Location;

namespace FLS.Data.WebApi.Flight
{
    public class FlightExchangeData
    {
        public Guid FlightId { get; set; }

        public string StartType { get; set; }

        public DateTime? FlightDate { get; set; }

        public string AircraftImmatriculation { get; set; }

        public FlightCrewData Pilot { get; set; }

        public FlightCrewData CoPilot { get; set; }

        public FlightCrewData Instructor { get; set; }

        public FlightCrewData Observer { get; set; }

        public RecipientDetails InvoiceRecipient { get; set; }

        public List<FlightCrewData> Passengers { get; set; }

        /// <summary>
        /// Gets or sets the engine operating counter before start engine in seconds
        /// </summary>
        public Nullable<long> EngineStartOperatingCounterInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the engine operating counter after engine shutdown in seconds
        /// </summary>
        public Nullable<long> EngineEndOperatingCounterInSeconds { get; set; }
        

        public string FlightComment { get; set; }

        public string AirState { get; set; }

        public string ValidationState { get; set; }

        public string ProcessState { get; set; }

        public string FlightTypeName { get; set; }

        public string FlightTypeCode { get; set; }

        public Nullable<DateTime> LdgDateTime { get; set; }
        
        public Nullable<DateTime> StartDateTime { get; set; }

        public Nullable<DateTime> BlockStartDateTime { get; set; }

        public Nullable<DateTime> BlockEndDateTime { get; set; }

        public TimeSpan FlightDuration { get; set; }

        public LocationData StartLocation { get; set; }

        public LocationData LdgLocation { get; set; }

        public int? FlightCostBalanceTypeId { get; set; }

        public Nullable<int> NrOfLdgs { get; set; }

        public int? NrOfLdgsOnStartLocation { get; set; }

        public bool IsSoloFlight { get; set; }

        public string OutboundRoute { get; set; }

        public string InboundRoute { get; set; }

        public bool NoStartTimeInformation { get; set; }

        public bool NoLdgTimeInformation { get; set; }

        public string CouponNumber { get; set; }

        public Guid TowFlightFlightId { get; set; }

        public string TowFlightAircraftImmatriculation { get; set; }

        public FlightCrewData TowFlightPilot { get; set; }

        public FlightCrewData TowFlightInstructor { get; set; }

        /// <summary>
        /// Gets or sets the engine operating counter before start engine in seconds
        /// </summary>
        public Nullable<long> TowFlightEngineStartOperatingCounterInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the engine operating counter after engine shutdown in seconds
        /// </summary>
        public Nullable<long> TowFlightEngineEndOperatingCounterInSeconds { get; set; }


        public string TowFlightFlightComment { get; set; }

        public string TowFlightAirState { get; set; }

        public string TowFlightValidationState { get; set; }

        public string TowFlightProcessState { get; set; }

        public string TowFlightFlightTypeName { get; set; }

        public string TowFlightFlightTypeCode { get; set; }

        public Nullable<DateTime> TowFlightLdgDateTime { get; set; }

        public Nullable<DateTime> TowFlightStartDateTime { get; set; }

        public Nullable<DateTime> TowFlightBlockStartDateTime { get; set; }

        public Nullable<DateTime> TowFlightBlockEndDateTime { get; set; }

        public TimeSpan TowFlightFlightDuration { get; set; }

        public LocationData TowFlightStartLocation { get; set; }

        public LocationData TowFlightLdgLocation { get; set; }

        public Nullable<int> TowFlightNrOfLdgs { get; set; }

        public string TowFlightOutboundRoute { get; set; }

        public string TowFlightInboundRoute { get; set; }

        public bool TowFlightNoStartTimeInformation { get; set; }

        public bool TowFlightNoLdgTimeInformation { get; set; }
    }
}
