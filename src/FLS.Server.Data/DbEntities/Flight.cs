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
        }

        public Guid FlightId { get; set; }

        public Guid AircraftId { get; set; }

        public int? StartPosition { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? StartDateTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LdgDateTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EngineTime { get; set; }

        /// <summary>
        /// Gets or sets the engine operating counter before start engine in minutes and decimal in seconds as divide of 60
        /// </summary>
        public Nullable<Decimal> EngineStartOperatingCounterInMinutes { get; set; }

        /// <summary>
        /// Gets or sets the engine operating counter after engine shutdown in minutes and decimal in seconds as divide of 60
        /// </summary>
        public Nullable<Decimal> EngineEndOperatingCounterInMinutes { get; set; }


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

        [Column("FlightState")]
        public int FlightStateId { get; set; }

        public int FlightAircraftType { get; set; }

        public string Comment { get; set; }

        public string IncidentComment { get; set; }

        [StringLength(20)]
        public string CouponNumber { get; set; }

        [Column("FlightCostBalanceType")]
        public int? FlightCostBalanceTypeId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? InvoicedOn { get; set; }

        [StringLength(100)]
        public string InvoiceNumber { get; set; }

        [StringLength(100)]
        public string DeliveryNumber { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ValidatedOn { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? InvoicePaidOn { get; set; }

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

        public virtual Flight TowFlight { get; set; }

        public virtual FlightState FlightState { get; set; }

        public virtual FlightType FlightType { get; set; }

        public virtual Location LdgLocation { get; set; }

        public virtual Location StartLocation { get; set; }

        public virtual StartType StartType { get; set; }

        #region additional methods
        internal int GetCalculatedFlightStateId()
        {
            if (FlightStateId >= (int)FLS.Data.WebApi.Flight.FlightState.Landed)
            {
                //only workflow processed flight states (invalid, valid, locked, invoiced, partial paid, paid)
                //or at minimum landed flights (after landed flights, flight state can't be calculated, except with workflows)
                return FlightStateId;
            }

            if (LdgDateTime.HasValue)
            {
                if (FlightStateId <= (int)FLS.Data.WebApi.Flight.FlightState.Landed)
                {
                    return (int)FLS.Data.WebApi.Flight.FlightState.Landed;
                }
            }
            else if (StartDateTime.HasValue)
            {
                if (FlightStateId <= (int)FLS.Data.WebApi.Flight.FlightState.Started)
                {
                    return (int)FLS.Data.WebApi.Flight.FlightState.Started;
                }
            }
            else if (FlightStateId == (int)FLS.Data.WebApi.Flight.FlightState.FlightPlanOpen)
            {
                return (int)FLS.Data.WebApi.Flight.FlightState.FlightPlanOpen;
            }

            return (int)FLS.Data.WebApi.Flight.FlightState.New;
        }

        /// <summary>
        /// Gets the calculated nr of landings based on the current FlightState (IMPORTANT: FlightState must be updated or set first).
        /// </summary>
        /// <returns></returns>
        internal int? GetCalculatedNrOfLandings(bool isTowingOrWinchRequired)
        {
            if (FlightStateId <= (int)FLS.Data.WebApi.Flight.FlightState.Started)
            {
                // not started or just started flights do not have a landing
                return 0;
            }
            
            if (FlightStateId == (int)FLS.Data.WebApi.Flight.FlightState.Landed
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
        /// returns if current flight is self start flight. If StartType is not set, returns null.
        /// </summary>
        public bool? IsSelfStartFlight
        {
            get
            {
                if (StartType == null) return null;
                return StartTypeId == (int)AircraftStartType.SelfStart;
            }
        }

        /// <summary>
        /// returns diffrence between landing and starting of flight, 
        /// or since how long flight is started, or zero when flight is not started
        /// </summary>
        public TimeSpan Duration
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
                    ret = DateTime.Now - StartDateTime.Value;
                }

                return TimeSpan.FromSeconds(Math.Round(ret.TotalSeconds));
            }
        }

        /// <summary>
        /// returns date when flight is started (<see cref="StartDateTime"/> without time)
        /// when flight is not started returns Today.
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                if (StartDateTime.HasValue)
                {
                    return StartDateTime.Value.Date;
                }

                return DateTime.Today;
            }
        }

        /// <summary>
        /// return if flight is landed.
        /// </summary>
        public bool IsLanded
        {
            get { return LdgDateTime.HasValue; }
        }

        /// <summary>
        /// returns all FlightCrews in a comma-seperated string.
        /// </summary>
        public string FlightCrewCommaSeperated
        {
            get
            {
                string ret = FlightCrews.Where(crew => crew.Person != null).OrderBy(crewType => crewType.FlightCrewTypeId).Aggregate("",
                                                                                      (current, crew) =>
                                                                                      current +
                                                                                      (crew.Person.Lastname + " " +
                                                                                       crew.Person.Firstname + ", "));
                if (ret.Length > 1)
                {
                    ret = ret.Remove(ret.Length - 2);
                }

                return ret;
            }
        }

        /// <summary>
        /// returns if a flight is able to start (when flightstatus is new)
        /// </summary>
        public bool CanStart
        {
            get
            {
                return Pilot != null && FlightStateId.Equals((int)FLS.Data.WebApi.Flight.FlightState.New);
            }
        }

        /// <summary>
        /// returns if a flight is able to land (when flightstatus is started)
        /// </summary>
        public bool CanLand
        {
            get { return FlightStateId.Equals((int)FLS.Data.WebApi.Flight.FlightState.Started); }
        }

        /// <summary>
        /// returns if flight can have an instructor (when flighttype is InstructorRequired
        /// </summary>
        public bool? IsInstructorRequired
        {
            get
            {
                if (FlightType == null) return null;
                return FlightType.InstructorRequired;
            }
        }

        public bool? IsObserverPilotOrInstructorRequired
        {
            get
            {
                if (FlightType == null) return null;
                return FlightType.ObserverPilotOrInstructorRequired;
            }
        }

        /// <summary>
        /// returns if flight can have a passenger (when flightType is passengerflight and the aircraft have enough space)
        /// </summary>
        public bool? CanHavePassenger
        {
            get
            {
                if (Aircraft == null || FlightType == null) return null;

                return FlightType.IsPassengerFlight && Aircraft.NrOfSeats >= 2;
            }
        }

        /// <summary>
        /// returns if flight can have two pilots (when flightType is not instructorRequired and not passengerflight and the aircraft have enough space)
        /// </summary>
        public bool? CanHaveTwoPilots
        {
            get
            {
                if (Aircraft == null) return null;

                if (Aircraft.NrOfSeats >= 2)
                {
                    if (FlightType == null)
                    {
                        return true;
                    }

                    return (!FlightType.InstructorRequired && !FlightType.IsPassengerFlight);
                }

                return false;
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
        /// returns if startlocation has a shortname.
        /// </summary>
        public bool HasStartLocationShortName
        {
            get
            {
                if (StartLocation == null) return false;
                return !string.IsNullOrEmpty(StartLocation.LocationShortName);
            }
        }

        /// <summary>
        /// returns if landinglocation has a shortname.
        /// </summary>
        public bool HasLdgLocationShortName
        {
            get
            {
                if (LdgLocation == null) return false;
                return !string.IsNullOrEmpty(LdgLocation.LocationShortName);
            }
        }

        /// <summary>
        /// returns if startlocation has a icao code
        /// </summary>
        public bool HasStartLocationIcaoCode
        {
            get
            {
                if (StartLocation == null) return false;
                return !string.IsNullOrEmpty(StartLocation.IcaoCode);
            }
        }

        /// <summary>
        /// returns if flight flights longer than one day
        /// </summary>
        public bool FlightsLongerThanOneDay
        {
            get { return Duration.Days >= 1; }
        }

        /// <summary>
        /// returns if landing location has a icao code
        /// </summary>
        public bool HasLdgLocationIcaoCode
        {
            get
            {
                if (LdgLocation == null) return false;
                return !string.IsNullOrEmpty(LdgLocation.IcaoCode);
            }
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

        public DateTime? FlightDate
        {
            get
            {
                if (StartDateTime.HasValue)
                {
                    return StartDateTime.Value.Date;
                }

                return null;
            }
        }

        public bool? IsPassengerFlight
        {
            get
            {
                if (FlightType == null) return null;

                if (FlightType.IsPassengerFlight)
                {
                    return true;
                }

                return false;
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

            sb.Append(", Flight-State: ");
            sb.Append(FlightStateId);

            if (TowFlight != null)
            {
                sb.Append(", TowFlight: ");
                sb.Append(TowFlight.AircraftImmatriculation);
                sb.Append(", TowPilot: ");
                sb.Append(TowFlight.PilotDisplayName);

                sb.Append(", Tow-Flight-State: ");
                sb.Append(TowFlight.FlightState);
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
                || StartDateTime.HasValue == false
                || LdgDateTime.HasValue == false
                || StartLocationId.HasValue == false
                || LdgLocationId.HasValue == false
                || StartTypeId.HasValue == false
                || FlightTypeId.HasValue == false
                || NrOfLdgs.HasValue == false
                || NrOfLdgs.Value < 1)
            {
                FlightStateId = (int) FLS.Data.WebApi.Flight.FlightState.Invalid;
                return;
            }

            if (FlightAircraftType == (int) FlightAircraftTypeValue.TowFlight)
            {
                //validation finished
                FlightStateId = (int)FLS.Data.WebApi.Flight.FlightState.Valid;
                return;
            }

            if (StartTypeId.Value == (int)AircraftStartType.TowingByAircraft)
            {
                if (TowFlightId == Guid.Empty
                    || TowFlight == null)
                {
                    FlightStateId = (int)FLS.Data.WebApi.Flight.FlightState.Invalid;
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
                    FlightStateId = (int)FLS.Data.WebApi.Flight.FlightState.Invalid;
                }
            }
            else if (StartTypeId.Value == (int)AircraftStartType.WinchLaunch)
            {
                if (WinchOperator == null
                    || WinchOperator.HasPerson == false)
                {
                    FlightStateId = (int)FLS.Data.WebApi.Flight.FlightState.Invalid;
                }
            }
            else if (StartTypeId.Value == (int)AircraftStartType.SelfStart)
            {
                //if (EngineTime.HasValue == false)
                //{
                //    return false;
                //}
            }
            else if (StartTypeId.Value == (int)AircraftStartType.MotorFlightStart)
            {
                //if (EngineTime.HasValue == false)
                //{
                //    FlightStateId = (int)FLS.Data.WebApi.Flight.FlightState.Invalid;
                //}
            }

            FlightStateId = (int)FLS.Data.WebApi.Flight.FlightState.Valid;
        }
        #endregion additional methods
    }
}
