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
    public partial class FlightType : IFLSMetaData
    {
        public FlightType()
        {
            ClubsDefaultGliderFlightType = new HashSet<Club>();
            ClubsDefaultMotorFlightType = new HashSet<Club>();
            ClubsDefaultTowFlightType = new HashSet<Club>();
            Flights = new HashSet<Flight>();
        }

        public Guid FlightTypeId { get; set; }

        public Guid ClubId { get; set; }

        [Required]
        [StringLength(100)]
        public string FlightTypeName { get; set; }

        [StringLength(30)]
        public string FlightCode { get; set; }

        public bool InstructorRequired { get; set; }

        public bool ObserverPilotOrInstructorRequired { get; set; }

        public bool IsCheckFlight { get; set; }

        public bool IsPassengerFlight { get; set; }

        public bool IsSoloFlight { get; set; }

        public bool IsForGliderFlights { get; set; }

        public bool IsForTowFlights { get; set; }

        public bool IsForMotorFlights { get; set; }

        public bool IsFlightCostBalanceSelectable { get; set; }

        public bool IsCouponNumberRequired { get; set; }

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

        public virtual ICollection<Club> ClubsDefaultGliderFlightType { get; set; }

        public virtual ICollection<Club> ClubsDefaultMotorFlightType { get; set; }

        public virtual ICollection<Club> ClubsDefaultTowFlightType { get; set; }

        public virtual Club Club { get; set; }

        public virtual ICollection<Flight> Flights { get; set; }

        public Guid Id
        {
            get { return FlightTypeId; }
            set { FlightTypeId = value; }
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
