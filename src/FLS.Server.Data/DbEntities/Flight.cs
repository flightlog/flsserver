using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using FLS.Data.WebApi;
using FLS.Server.Data.Enums;

namespace FLS.Server.Data.DbEntities
{
    public partial class Flight : IFLSMetaData
    {
        public Flight()
        {
            FlightCrews = new HashSet<FlightCrew>();
            TowedFlights = new HashSet<Flight>();
            Deliveries = new HashSet<Delivery>();
            DeliveryCreationTests = new HashSet<DeliveryCreationTest>();
            ProcessStateId = (int) FLS.Data.WebApi.Flight.FlightProcessState.NotProcessed;
            Movements = new HashSet<Movement>();
        }

        public Guid FlightId { get; set; }

        public Guid AircraftId { get; set; }

        public int? StartPosition { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? FlightDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? StartDateTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LdgDateTime { get; set; }

        /// <summary>
        /// Gets or sets the engine operating counter before start engine (units see EngineOperatingCounterUnitTypeId)
        /// </summary>
        public Nullable<long> EngineStartOperatingCounterInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the engine operating counter after engine shutdown (units see EngineOperatingCounterUnitTypeId)
        /// </summary>
        public Nullable<long> EngineEndOperatingCounterInSeconds { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? BlockStartDateTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? BlockEndDateTime { get; set; }

        public Guid? StartLocationId { get; set; }

        public Guid? LdgLocationId { get; set; }

        [StringLength(5)]
        public string StartRunway { get; set; }

        [StringLength(5)]
        public string LdgRunway { get; set; }

        [StringLength(50)]
        public string OutboundRoute { get; set; }

        [StringLength(50)]
        public string InboundRoute { get; set; }

        public Guid? FlightTypeId { get; set; }

        public bool IsSoloFlight { get; set; }

        [Column("StartType")]
        public int? StartTypeId { get; set; }

        public Guid? TowFlightId { get; set; }

        public int? NrOfLdgs { get; set; }

        public int? NrOfLdgsOnStartLocation { get; set; }

        public bool NoStartTimeInformation { get; set; }

        public bool NoLdgTimeInformation { get; set; }
        
        public int AirStateId { get; set; }
        
        public int ProcessStateId { get; set; }

        public int FlightAircraftType { get; set; }

        public string Comment { get; set; }

        public string IncidentComment { get; set; }

        [StringLength(20)]
        public string CouponNumber { get; set; }

        [Column("FlightCostBalanceType")]
        public int? FlightCostBalanceTypeId { get; set; }
        
        [Column(TypeName = "datetime2")]
        public DateTime? DeliveryCreatedOn { get; set; }
        
        [Column(TypeName = "datetime2")]
        public DateTime? ValidatedOn { get; set; }

        public string ValidationErrors { get; set; }

        public int? NrOfPassengers { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedOn { get; set; }

        public Guid CreatedByUserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedOn { get; set; }

        public Guid? ModifiedByUserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DeletedOn { get; set; }

        public Guid? DeletedByUserId { get; set; }

        public int? RecordState { get; set; }

        public Guid OwnerId { get; set; }

        public int OwnershipType { get; set; }

        public bool IsDeleted { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? FlightReportSentOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [do not update meta data].
        /// Used for workflow processes to not create a modified user error when trying to save records.
        /// </summary>
        /// <value>
        /// <c>true</c> if [do not update meta data]; otherwise, <c>false</c>.
        /// </value>
        public bool DoNotUpdateMetaData { get; set; }

        public virtual Aircraft Aircraft { get; set; }

        public virtual FlightCostBalanceType FlightCostBalanceType { get; set; }

        public virtual ICollection<FlightCrew> FlightCrews { get; set; }

        public virtual ICollection<Flight> TowedFlights { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }

        public virtual ICollection<DeliveryCreationTest> DeliveryCreationTests { get; set; }

        public virtual ICollection<Movement> Movements { get; set; }

        public virtual Flight TowFlight { get; set; }

        public virtual FlightAirState FlightAirState { get; set; }
        
        public virtual FlightProcessState FlightProcessState { get; set; }

        public virtual FlightType FlightType { get; set; }

        public virtual Location LdgLocation { get; set; }

        public virtual Location StartLocation { get; set; }

        public virtual StartType StartType { get; set; }

        #region additional methods
        internal int GetCalculatedFlightAirStateId()
        {
            if (LdgDateTime.HasValue)
            {
                return (int)FLS.Data.WebApi.Flight.FlightAirState.Landed;
            }

            if (NoLdgTimeInformation)
            {
                if (StartDateTime.HasValue)
                {
                    return (int)FLS.Data.WebApi.Flight.FlightAirState.MightBeLandedOrInAir;
                }
            }

            if (StartDateTime.HasValue)
            {
                return (int)FLS.Data.WebApi.Flight.FlightAirState.Started;
            }

            if (NoStartTimeInformation)
            {
                return (int)FLS.Data.WebApi.Flight.FlightAirState.MightBeStarted;
            }

            if (AirStateId == (int)FLS.Data.WebApi.Flight.FlightAirState.FlightPlanOpen)
            {
                return (int)FLS.Data.WebApi.Flight.FlightAirState.FlightPlanOpen;
            }

            return (int)FLS.Data.WebApi.Flight.FlightAirState.New;
        }

        /// <summary>
        /// Gets the calculated nr of landings based on the current FlightState.
        /// </summary>
        /// <returns></returns>
        internal int? GetCalculatedNrOfLandings(bool isTowingOrWinchRequired)
        {
            var currentFlightAirStateId = GetCalculatedFlightAirStateId();

            if (currentFlightAirStateId <= (int)FLS.Data.WebApi.Flight.FlightAirState.Started)
            {
                // not or might be started or just started flights do not have a landing
                return 0;
            }
            
            if (currentFlightAirStateId == (int)FLS.Data.WebApi.Flight.FlightAirState.Landed
                && (NrOfLdgs.HasValue == false || NrOfLdgs.Value <= 0))
            {
                // only set a value if it is landed and has no value
                return 1;
            }

            if (isTowingOrWinchRequired)
            {
                // it is a glider flight without engine or selfstart ability, so maximum landings is fix 1!
                return 1;
            }

            return NrOfLdgs;
        }

        /// <summary>
        /// returns diffrence between landing and starting of flight (flight duration), 
        /// or since how long flight is started, or zero when flight is not started
        /// </summary>
        [NotMapped]
        public TimeSpan FlightDurationZeroBased
        {
            get
            {
                if (StartDateTime.HasValue == false)
                {
                    return TimeSpan.Zero;
                }

                TimeSpan ret;
                if (LdgDateTime.HasValue)
                {
                    ret = LdgDateTime.Value - StartDateTime.Value;
                }
                else
                {
                    if (StartDateTime.Value.Kind == DateTimeKind.Local)
                    {
                        ret = DateTime.Now - StartDateTime.Value;
                    }
                    else
                    {
                        ret = DateTime.UtcNow - StartDateTime.Value;
                    }
                }

                return TimeSpan.FromSeconds(Math.Round(ret.TotalSeconds));
            }
        }

        [NotMapped]
        public TimeSpan? FlightDuration
        {
            get
            {
                if (StartDateTime.HasValue == false
                    || LdgDateTime.HasValue == false
                    || NoStartTimeInformation
                    || NoLdgTimeInformation)
                {
                    return null;
                }

                return LdgDateTime.Value - StartDateTime.Value;
            }
        }

        [NotMapped]
        public TimeSpan EngineDurationZeroBased
        {
            get
            {
                if (EngineStartOperatingCounterInSeconds.HasValue
                    && EngineEndOperatingCounterInSeconds.HasValue)
                {
                    return
                        TimeSpan.FromSeconds(EngineEndOperatingCounterInSeconds.Value -
                                             EngineStartOperatingCounterInSeconds.Value);
                }

                return TimeSpan.Zero;
            }
        }

        /// <summary>
        /// returns the pilot of the flightcrew
        /// </summary>
        public FlightCrew Pilot
        {
            get
            {
                return
                    FlightCrews.FirstOrDefault(crew => crew.FlightCrewTypeId.Equals((int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent));
            }
        }

        /// <summary>
        /// returns the copilot of the flightcrew
        /// </summary>
        public FlightCrew CoPilot
        {
            get { return FlightCrews.FirstOrDefault(crew => crew.FlightCrewTypeId.Equals((int)FLS.Data.WebApi.Flight.FlightCrewType.CoPilot)); }
        }

        /// <summary>
        /// returns the instructor of the flightcrew
        /// </summary>
        public FlightCrew Instructor
        {
            get
            {
                return
                    FlightCrews.FirstOrDefault(crew => crew.FlightCrewTypeId.Equals((int)FLS.Data.WebApi.Flight.FlightCrewType.FlightInstructor));
            }
        }

        public FlightCrew ObserverPerson
        {
            get { return FlightCrews.FirstOrDefault(crew => crew.FlightCrewTypeId.Equals((int)FLS.Data.WebApi.Flight.FlightCrewType.Observer)); }
        }

        /// <summary>
        /// returns the first passenger of the flightcrew.
        /// </summary>
        public FlightCrew Passenger
        {
            get { return FlightCrews.FirstOrDefault(crew => crew.FlightCrewTypeId.Equals((int)FLS.Data.WebApi.Flight.FlightCrewType.Passenger)); }
        }

        public List<FlightCrew> Passengers
        {
            get { return FlightCrews.Where(crew => crew.FlightCrewTypeId.Equals((int)FLS.Data.WebApi.Flight.FlightCrewType.Passenger)).ToList(); }
        }

        public FlightCrew WinchOperator
        {
            get
            {
                return
                    FlightCrews.FirstOrDefault(crew => crew.FlightCrewTypeId.Equals((int)FLS.Data.WebApi.Flight.FlightCrewType.WinchOperator));
            }
        }

        public FlightCrew InvoiceRecipient
        {
            get { return FlightCrews.FirstOrDefault(crew => crew.FlightCrewTypeId.Equals((int)FLS.Data.WebApi.Flight.FlightCrewType.FlightCostInvoiceRecipient)); }
        }
        
        /// <summary>
        /// returns if flight is towed by aircraft. If the StartType is not set, it returns null.
        /// </summary>
        public bool? IsTowed
        {
            get
            {
                if (StartTypeId.HasValue == false) return null;
                return StartTypeId == (int)AircraftStartType.TowingByAircraft;
            }
        }

        public Guid Id
        {
            get { return FlightId; }
            set { FlightId = value; }
        }

        public bool IsGliderFlight
        {
            get { return FlightAircraftType == (int)FlightAircraftTypeValue.GliderFlight; }
        }

        public bool IsTowFlight
        {
            get { return FlightAircraftType == (int)FlightAircraftTypeValue.TowFlight; }
        }

        public bool IsMotorFlight
        {
            get { return FlightAircraftType == (int)FlightAircraftTypeValue.MotorFlight; }
        }
        
        public string PilotDisplayName
        {
            get
            {
                if (Pilot != null && Pilot.Person != null)
                {
                    return Pilot.Person.DisplayName;
                }

                return string.Empty;
            }
        }

        public string InstructorDisplayName
        {
            get
            {
                if (Instructor != null && Instructor.Person != null)
                {
                    return Instructor.Person.DisplayName;
                }

                return string.Empty;
            }
        }

        public string CoPilotDisplayName
        {
            get
            {
                if (CoPilot != null && CoPilot.Person != null)
                {
                    return CoPilot.Person.DisplayName;
                }

                return string.Empty;
            }
        }

        public string PassengerDisplayName
        {
            get
            {
                if (Passenger != null && Passenger.Person != null)
                {
                    return Passenger.Person.DisplayName;
                }

                return string.Empty;
            }
        }

        public string AircraftImmatriculation
        {
            get
            {
                if (Aircraft != null)
                {
                    return Aircraft.Immatriculation;
                }

                return string.Empty;
            }
        }

        public bool IsStarted
        {
            get
            {
                return StartDateTime.HasValue && LdgDateTime.HasValue == false;
            }
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();

            if (IsGliderFlight)
            {
                sb.Append("Glider-Flight: ");
            }
            else if (IsTowFlight)
            {
                sb.Append("Tow-Flight: ");
            }
            else if (IsMotorFlight)
            {
                sb.Append("Motor-Flight: ");
            }
            else
            {
                sb.Append("Unknown Flight: ");
            }

            sb.Append(AircraftImmatriculation);
            sb.Append(", Pilot: ");
            sb.Append(PilotDisplayName);

            if (StartDateTime.HasValue)
            {
                sb.Append(", Flight-Date: ");
                sb.Append(StartDateTime.Value.ToShortDateString());
                sb.Append(", Starttime: ");
                sb.Append(StartDateTime.Value.ToShortTimeString());
            }

            if (LdgDateTime.HasValue)
            {
                sb.Append(", LdgTime: ");
                sb.Append(LdgDateTime.Value.ToShortTimeString());
            }

            if (FlightType != null)
            {
                sb.Append(", Flightcode: ");
                sb.Append(FlightType.FlightCode);
            }

            sb.Append($", Nr of Ldgs: {NrOfLdgs.GetValueOrDefault(0)}/{NrOfLdgsOnStartLocation.GetValueOrDefault(0)}");

            sb.Append(", Flight-Air-State: ");
            sb.Append(AirStateId);
            sb.Append(", Flight-Process-State: ");
            sb.Append(ProcessStateId);

            if (TowFlight != null)
            {
                sb.Append(", TowFlight: ");
                sb.Append(TowFlight.AircraftImmatriculation);
                sb.Append(", TowPilot: ");
                sb.Append(TowFlight.PilotDisplayName);

                sb.Append(", Tow-Flight-Air-State: ");
                sb.Append(TowFlight.AirStateId);
                sb.Append(", Tow-Flight-Process-State: ");
                sb.Append(TowFlight.ProcessStateId);
            }

            return sb.ToString();
        }

        public bool? GetCalculatedIsSoloFlight(Aircraft aircraft = null, FlightType flightType = null)
        {
            if (aircraft != null)
            {
                if (aircraft.NrOfSeats.HasValue && aircraft.NrOfSeats.Value == 1)
                {
                    return true;
                }
            }

            if (flightType != null)
            {
                if (flightType.IsSoloFlight)
                {
                    IsSoloFlight = true;
                }
                else if (flightType.IsPassengerFlight)
                {
                    return false;
                }
            }

            return null;
        }

        internal void ValidateFlight()
        {
            ValidatedOn = DateTime.UtcNow;

            if (AircraftId == Guid.Empty
                || Pilot == null
                || Pilot.PersonId == Guid.Empty
                || (StartDateTime.HasValue == false && NoStartTimeInformation == false)
                || (LdgDateTime.HasValue == false && NoLdgTimeInformation == false)
                || StartLocationId.HasValue == false
                || LdgLocationId.HasValue == false
                || StartTypeId.HasValue == false
                || FlightTypeId.HasValue == false
                || NrOfLdgs.HasValue == false
                || NrOfLdgs.Value < 1)
            {
                ProcessStateId = (int) FLS.Data.WebApi.Flight.FlightProcessState.Invalid;
                return;
            }

            if (FlightAircraftType == (int) FlightAircraftTypeValue.TowFlight)
            {
                //validation finished
                ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.Valid;
                return;
            }

            if (StartTypeId.Value == (int)AircraftStartType.TowingByAircraft)
            {
                if (TowFlightId == Guid.Empty
                    || TowFlight == null)
                {
                    ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.Invalid;
                }

                if (TowFlight != null)
                {
                    TowFlight.ValidateFlight();
                }
            }
            else if (StartTypeId.Value == (int)AircraftStartType.ExternalStart)
            {
                if (TowFlightId.HasValue)
                {
                    ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.Invalid;
                }
            }
            else if (StartTypeId.Value == (int)AircraftStartType.WinchLaunch)
            {
                if (WinchOperator == null
                    || WinchOperator.HasPerson == false)
                {
                    ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.Invalid;
                }
            }
            else if (StartTypeId.Value == (int)AircraftStartType.SelfStart)
            {
                
            }
            else if (StartTypeId.Value == (int)AircraftStartType.MotorFlightStart)
            {
                
            }

            ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.Valid;
        }

        /// <summary>
        /// This method sets the flight process state back to the locked state, to create a new delivery during next workflow process.
        /// </summary>
        public void DeletedDeliveryForFlight()
        {
            ProcessStateId = (int) FLS.Data.WebApi.Flight.FlightProcessState.Locked;
        }
        #endregion additional methods
    }
}
