using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FLS.Server.Data.DbEntities
{
    public partial class CounterUnitType
    {
        public CounterUnitType()
        {
            AircraftFlightOperatingCounters = new HashSet<Aircraft>();
            AircraftEngineOperatingCounters = new HashSet<Aircraft>();
            AircraftOperatingCounterFlightOperatingCounters = new HashSet<AircraftOperatingCounter>();
            AircraftOperatingCounterEngineOperatingCounters = new HashSet<AircraftOperatingCounter>();
        }

        public int CounterUnitTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string CounterUnitTypeName { get; set; }

        [Required]
        [StringLength(50)]
        public string CounterUnitTypeKeyName { get; set; }
        
        [StringLength(200)]
        public string Comment { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedOn { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<Aircraft> AircraftFlightOperatingCounters { get; set; }

        public virtual ICollection<Aircraft> AircraftEngineOperatingCounters { get; set; }

        public virtual ICollection<AircraftOperatingCounter> AircraftOperatingCounterFlightOperatingCounters { get; set; }

        public virtual ICollection<AircraftOperatingCounter> AircraftOperatingCounterEngineOperatingCounters { get; set; }

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
