using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Aircraft;

namespace FLS.Server.Data.DbEntities
{
    [Table("Aircrafts")]
    public partial class Aircraft : IFLSMetaData
    {
        private AircraftAircraftState _currentAircraftAircraftState;

        public Aircraft()
        {
            AircraftAircraftStates = new HashSet<AircraftAircraftState>();
            Flights = new HashSet<Flight>();
        }

        public Guid AircraftId { get; set; }

        [StringLength(100)]
        public string ManufacturerName { get; set; }

        [StringLength(50)]
        public string AircraftModel { get; set; }

        [Required]
        [StringLength(15)]
        public string Immatriculation { get; set; }

        [StringLength(5)]
        public string CompetitionSign { get; set; }

        public int? NrOfSeats { get; set; }

        public int? DaecIndex { get; set; }

        [StringLength(250)]
        public string Comment { get; set; }

        [Column("AircraftType")]
        public int AircraftTypeId { get; set; }

        public bool IsTowingOrWinchRequired { get; set; }

        public bool IsTowingstartAllowed { get; set; }

        public bool IsWinchstartAllowed { get; set; }

        public bool IsTowingAircraft { get; set; }

        public Guid? AircraftOwnerClubId { get; set; }

        public Guid? AircraftOwnerPersonId { get; set; }

        [StringLength(50)]
        public string FLARMId { get; set; }

        [StringLength(20)]
        public string AircraftSerialNumber { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? YearOfManufacture { get; set; }

        [StringLength(1)]
        public string NoiseClass { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NoiseLevel { get; set; }

        public int? MTOM { get; set; }

        public int FlightDurationPrecision { get; set; }

        public int EngineOperatorCounterPrecision { get; set; }

        [StringLength(250)]
        public string SpotLink { get; set; }

        public bool IsFastEntryRecord { get; set; }

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

        public virtual ICollection<AircraftAircraftState> AircraftAircraftStates { get; set; }

        public virtual AircraftType AircraftType { get; set; }

        public virtual Club AircraftOwnerClub { get; set; }

        public virtual Person AircraftOwnerPerson { get; set; }

        public virtual ICollection<Flight> Flights { get; set; }

        public virtual ICollection<AircraftReservation> AircraftReservations { get; set; }

        public virtual ICollection<AircraftOperatingCounter> AircraftOperatingCounters { get; set; }

        /// <summary>
        /// actual status is newest status.
        /// </summary>
        public AircraftAircraftState CurrentAircraftAircraftState
        {
            get
            {
                if (_currentAircraftAircraftState == null && AircraftAircraftStates.Count > 0)
                {
                    _currentAircraftAircraftState =
                        AircraftAircraftStates.OrderByDescending(o => o.ValidFrom)
                                              .FirstOrDefault(o => o.ValidTo == null);
                }

                return _currentAircraftAircraftState;
            }
            set
            {
                _currentAircraftAircraftState = value;
            }
        }

        public AircraftOperatingCounter CurrentAircraftOperatingCounter
        {
            get
            {
                return AircraftOperatingCounters.OrderByDescending(o => o.AtDateTime).FirstOrDefault();
            }
        }

        public Guid Id
        {
            get { return AircraftId; }
            set { AircraftId = value; }
        }

        public bool HasEngine
        {
            get { return AircraftTypeId >= (int)FLS.Data.WebApi.Aircraft.AircraftType.GliderWithMotor; }
        }

        public bool HasAircraftStateChanges(AircraftDetails currentAircraftDetails)
        {
            if (CurrentAircraftAircraftState != null)
            {
                if (currentAircraftDetails.AircraftStateData != null)
                {
                    //state might be changed, so check details for changes
                    var currentState = CurrentAircraftAircraftState;
                    var detailState = currentAircraftDetails.AircraftStateData;
                    return currentState.NoticedByPersonId != detailState.NoticedByPersonId
                           || currentState.Remarks != detailState.Remarks
                           || currentState.ValidFrom != detailState.ValidFrom
                           || currentState.ValidTo != detailState.ValidTo;
                }
            }
            else
            {
                if (currentAircraftDetails.AircraftStateData != null)
                {
                    //new state and unknown state before
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Type type = GetType();
            sb.Append("[");
            sb.Append(type.Name);
            sb.Append(" -> ");
            foreach (FieldInfo info in type.GetFields())
            {
                sb.Append(string.Format("{0}: {1}, ", info.Name, info.GetValue(this)));
            }

            Type tColl = typeof(ICollection<>);
            foreach (PropertyInfo info in type.GetProperties())
            {
                Type t = info.PropertyType;
                if (t.IsGenericType && tColl.IsAssignableFrom(t.GetGenericTypeDefinition()) ||
                    t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == tColl)
                    || (t.Namespace != null && t.Namespace.Contains("FLS.Server.Data.DbEntities")))
                {
                    continue;
                }

                sb.Append(string.Format("{0}: {1}, ", info.Name, info.GetValue(this, null)));
            }

            sb.Append(" <- ");
            sb.Append(type.Name);
            sb.AppendLine("]");

            return sb.ToString();
        }
    }
}
