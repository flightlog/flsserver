using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using FLS.Data.WebApi;

namespace FLS.Server.Data.DbEntities
{
    public partial class Location : IFLSMetaData
    {
        public Location()
        {
            Clubs = new HashSet<Club>();
            LandedFlights = new HashSet<Flight>();
            StartedFlights = new HashSet<Flight>();
            InOutboundPoints = new HashSet<InOutboundPoint>();
            PlanningDays = new HashSet<PlanningDay>();
            AircraftReservations = new HashSet<AircraftReservation>();
        }

        public Guid LocationId { get; set; }

        [Required]
        [StringLength(100)]
        public string LocationName { get; set; }

        [StringLength(50)]
        public string LocationShortName { get; set; }

        public Guid CountryId { get; set; }

        public Guid LocationTypeId { get; set; }

        [StringLength(10)]
        public string IcaoCode { get; set; }

        [StringLength(10)]
        public string Latitude { get; set; }

        [StringLength(10)]
        public string Longitude { get; set; }

        public int? Elevation { get; set; }

        [Column("ElevationUnitType")]
        public int? ElevationUnitTypeId { get; set; }

        [StringLength(50)]
        public string RunwayDirection { get; set; }

        public int? RunwayLength { get; set; }

        public int? RunwayLengthUnitType { get; set; }

        [StringLength(50)]
        public string AirportFrequency { get; set; }

        public string Description { get; set; }

        public int? SortIndicator { get; set; }

        public bool IsInboundRouteRequired { get; set; }

        public bool IsOutboundRouteRequired { get; set; }

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

        public virtual ICollection<Club> Clubs { get; set; }

        public virtual Country Country { get; set; }

        public virtual ElevationUnitType ElevationUnitType { get; set; }

        public virtual ICollection<Flight> LandedFlights { get; set; }

        public virtual ICollection<Flight> StartedFlights { get; set; }

        public virtual ICollection<InOutboundPoint> InOutboundPoints { get; set; }

        public virtual LengthUnitType LengthUnitType { get; set; }

        public virtual LocationType LocationType { get; set; }

        public virtual ICollection<PlanningDay> PlanningDays { get; set; }

        public virtual ICollection<AircraftReservation> AircraftReservations { get; set; }

        public Guid Id
        {
            get { return LocationId; }
            set { LocationId = value; }
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
