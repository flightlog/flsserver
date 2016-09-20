using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using FLS.Data.WebApi;

namespace FLS.Server.Data.DbEntities
{
    public class AircraftOperatingCounter : IFLSMetaData
    {
        public Guid AircraftOperatingCounterId { get; set; }

        public Guid AircraftId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AtDateTime { get; set; }

        public int? TotalTowedGliderStarts { get; set; }

        public int? TotalWinchLaunchStarts { get; set; }

        public int? TotalSelfStarts { get; set; }

        public long? FlightOperatingCounterInSeconds { get; set; }

        public long? EngineOperatingCounterInSeconds { get; set; }

        public long? NextMaintenanceAtFlightOperatingCounterInSeconds { get; set; }

        public long? NextMaintenanceAtEngineOperatingCounterInSeconds { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedOn { get; set; }

        public Guid CreatedByUserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedOn { get; set; }

        public Guid? ModifiedByUserId { get; set; }

        public Guid Id 
        {
            get { return AircraftOperatingCounterId; }
            private set { AircraftOperatingCounterId = value; }
        }

        [Column(TypeName = "datetime2")]
        public DateTime? DeletedOn { get; set; }

        public Guid? DeletedByUserId { get; set; }

        public int? RecordState { get; set; }

        public Guid OwnerId { get; set; }

        public int OwnershipType { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Aircraft Aircraft { get; set; }

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
