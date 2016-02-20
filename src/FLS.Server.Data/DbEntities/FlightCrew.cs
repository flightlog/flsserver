using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using FLS.Data.WebApi;

namespace FLS.Server.Data.DbEntities
{
    [Table("FlightCrew")]
    public partial class FlightCrew : IFLSMetaData
    {
        public Guid FlightCrewId { get; set; }

        public Guid FlightId { get; set; }

        public Guid PersonId { get; set; }

        [Column("FlightCrewType")]
        public int FlightCrewTypeId { get; set; }

        public DateTime? BeginFlightDateTime { get; set; }

        public DateTime? EndFlightDateTime { get; set; }

        public DateTime? BeginInstructionDateTime { get; set; }

        public DateTime? EndInstructionDateTime { get; set; }

        public int? NrOfLdgs { get; set; }


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

        public virtual FlightCrewType FlightCrewType { get; set; }

        public virtual Flight Flight { get; set; }

        public virtual Person Person { get; set; }

        public Guid Id
        {
            get { return FlightCrewId; }
            set { FlightCrewId = value; }
        }

        public bool HasPerson
        {
            get { return PersonId.Equals(Guid.Empty) == false; }
        }

        internal EntityState EntityState { get; set; }

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
