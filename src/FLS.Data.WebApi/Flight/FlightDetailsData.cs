using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.Flight
{
    public class FlightDetailsData : FLSBaseData
    {
        public Guid FlightId { get; set; }

        [Required]
        [GuidNotEmptyValidator]
        public Guid AircraftId { get; set; }

        public Nullable<Guid> PilotPersonId { get; set; }

        public Nullable<Guid> CoPilotPersonId { get; set; }

        public Nullable<Guid> InstructorPersonId { get; set; }

        public Nullable<Guid> ObserverPersonId { get; set; }

        public Nullable<Guid> InvoiceRecipientPersonId { get; set; }
        
        /// <summary>
        /// Gets or sets the engine operating counter before start engine (units see EngineOperatingCounterUnitTypeId)
        /// </summary>
        public Nullable<long> EngineStartOperatingCounterInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the engine operating counter after engine shutdown (units see EngineOperatingCounterUnitTypeId)
        /// </summary>
        public Nullable<long> EngineEndOperatingCounterInSeconds { get; set; }
        

        public string FlightComment { get; set; }

        public int AirStateId { get; set; }
        
        public int ProcessStateId { get; set; }

        public Nullable<Guid> FlightTypeId { get; set; }

        public Nullable<DateTime> LdgDateTime { get; set; }
        
        public Nullable<DateTime> StartDateTime { get; set; }

        public TimeSpan FlightDuration { get; set; }

        public Nullable<Guid> StartLocationId { get; set; }

        public Nullable<Guid> LdgLocationId { get; set; }

        public int? FlightCostBalanceType { get; set; }

        public Nullable<int> NrOfLdgs { get; set; }

        public bool IsSoloFlight { get; set; }

        public string OutboundRoute { get; set; }

        public string InboundRoute { get; set; }

        public bool NoStartTimeInformation { get; set; }

        public bool NoLdgTimeInformation { get; set; }

        public string ValidationErrors { get; set; }

        public override Guid Id
        {
            get { return FlightId; }
            set { FlightId = value; }
        }
    }
}
